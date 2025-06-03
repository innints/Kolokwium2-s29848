

using Kolokwium2.Data;
using Kolokwium2.DTOs;
using Kolokwium2.Exceptions;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Services;
public interface IDbService
{
    public Task<IEnumerable<PatientGetDto>> GetAllPatientsDetailsAsync();
    
    
    public Task<PrescriptionGetDto> CreatePrescriptionAsync(PrescriptionCreateDto prescriptionData);
    
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<IEnumerable<PatientGetDto>> GetAllPatientsDetailsAsync()
    {

        return await data.Patients
            //.Where(pa => pa.IdPatient == idPatient) gdyby vyło dla konkretnego id
            .Select(pa => new PatientGetDto
            {
                IdPatient = pa.IdPatient,
                FirstName = pa.FirstName,
                LastName = pa.LastName,
                Birthdate = pa.Birthdate,
                Prescriptions = pa.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PatientGetDtoPrescription
                        {
                            IdPrescription = pr.IdPrescription,
                            Date = pr.Date,
                            DueDate = pr.DueDate,
                            Medicaments = pr.PrescriptionMedicaments.Select(pm => new PatientGetDtoMedicament
                            {
                                IdMedicament = pm.IdMedicament,
                                Name = pm.Medicament.Name,
                                Dose = pm.Dose,
                                Description = pm.Medicament.Description,
                            }).ToList(),
                            Doctor = new PatientGetDtoDoctor
                            {
                                IdDoctor = pr.IdDoctor,
                                FirstName = pr.Doctor.FirstName,
                                LastName = pr.Doctor.LastName,
                                Email = pr.Doctor.Email
                            }

                        }

                    ).ToList()
            }).ToListAsync();
    }

    public async Task<PrescriptionGetDto> CreatePrescriptionAsync(PrescriptionCreateDto prescriptionData)
    {
        if (prescriptionData.DueDate < prescriptionData.Date)
        {
            throw new IncorrectDateException("Due date cannot be before Date");
        }

        if (prescriptionData.Medicaments.Count > 10)
        {
            throw new TooManyMedicamentsException("Number of medicaments cannot be more than 10");
        }



        // At first, we have to check if medicaments exist
        if (prescriptionData.Medicaments.Count != 0) //prescriptionData.Medicaments is not null  to nie bo jest not null
        {
            foreach (var medicamentDto in prescriptionData.Medicaments)
            {
                var medicament =
                    await data.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == medicamentDto.IdMedicament);
                if (medicament is null)
                {
                    throw new NotFoundException($"Medicament with id: {medicamentDto.IdMedicament} not found");
                }

            }
        }

        //czy doktor istnieje
        var doctor = await data.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == prescriptionData.IdDoctor);
        if (doctor is null)
        {

            throw new NotFoundException($"Doctor with id: {prescriptionData.IdDoctor} not found");
        }


        await using var transaction = await data.Database.BeginTransactionAsync();
        try
        {
            var patient =
                await data.Patients.FirstOrDefaultAsync(p => p.IdPatient == prescriptionData.Patient.IdPatient);
            if (patient is null)
            {
                patient = new Patient
                {
                    // IdPatient = prescriptionData.Patient.IdPatient, bez tego id jest automatycznie
                    FirstName = prescriptionData.Patient.FirstName,
                    LastName = prescriptionData.Patient.LastName,
                    Birthdate = prescriptionData.Patient.Birthdate
                };
                await data.Patients.AddAsync(patient);
                await data.SaveChangesAsync();
            }







            var prescription = new Prescription
            {
                Date = prescriptionData.Date,
                DueDate = prescriptionData.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = doctor.IdDoctor
            };

            await data.Prescriptions.AddAsync(prescription);
            await data.SaveChangesAsync();




            foreach (var m in prescriptionData.Medicaments)
            {
                await data.PrescriptionMedicaments.AddAsync(new PrescriptionMedicament
                {

                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = m.IdMedicament,
                    Dose = m.Dose ?? null,
                    Details = m.Details
                });
            }


            await data.SaveChangesAsync();
            await transaction.CommitAsync();
            return new PrescriptionGetDto
            {
                IdPrescription = prescription.IdPrescription,
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                IdDoctor = doctor.IdDoctor,
                IdPatient = prescription.IdPatient,
            };
        }
        catch (Exception)
        {

            await transaction.RollbackAsync();
            throw;
        }



    }
}