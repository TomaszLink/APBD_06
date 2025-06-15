using APBD_06.apimodels.responses;
using APBD_06.exceptions;
using APBD_06.models;
using APBD_06.repositorycontext;
using Microsoft.EntityFrameworkCore;

namespace APBD_06.services;

public class PrescriptionService(Repository repository)
{
    private readonly Repository repository = repository;


    public void CreateNewPrescription(CreatePrescriptionRequest request)
    {
        if (request.Medicaments.Count > 10)
            throw new ArgumentException("Prescription cannot contain more than 10 medicaments.");
        
        if (request.DueDate < request.Date)
            throw new ArgumentException("DueDate must be greater than or equal to Date.");
        
        var doctor = repository.Doctors.Find(request.IdDoctor);
        if (doctor == null)
            throw new ArgumentException($"Doctor with id {request.IdDoctor} not found.");

        var patient = repository.Patients.Find(request.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                Birthdate = request.Patient.Birthdate
            };
            repository.Patients.Add(patient);
            repository.SaveChanges();
        }

        if (request.Medicaments.Any(medicament => repository.Medicaments.Find(medicament.IdMedicament) == null))
        {
            throw new ArgumentException("Medicament not found.");
        }
        
        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdDoctor = doctor.IdDoctor,
            IdPatient = patient.IdPatient
        };
        repository.Prescriptions.Add(prescription);
        repository.SaveChanges();
        
        foreach (var m in request.Medicaments)
        {
            var presMed = new PrescriptionMedicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description
            };
            repository.PrescriptionMedicaments.Add(presMed);
        }
        
        repository.SaveChanges();
    }

    public PatientResponse GetPatientById(int id)
    {
        var patient = repository.Patients
            .Include(p => p.Prescriptions).ThenInclude(prescription => prescription.Doctor)
            .Include(patient => patient.Prescriptions).ThenInclude(prescription => prescription.PrescriptionMedicaments)
            .ThenInclude(prescriptionMedicament => prescriptionMedicament.Medicament)
            .FirstOrDefault(p => p.IdPatient == id);

        if (patient == null)
        {
            throw new PatientException();
        }

        return new PatientResponse()
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .Select(prescription => new PrescriptionResponse
                {
                    IdPrescription = prescription.IdPrescription,
                    Date = prescription.Date,
                    DueDate = prescription.DueDate,
                    Doctor = new DoctorResponse
                    {
                        IdDoctor = prescription.Doctor.IdDoctor,
                        FirstName = prescription.Doctor.FirstName
                    },
                    Medicaments = prescription.PrescriptionMedicaments
                        .Select(pm => new MedicamentResponse()
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Medicament.Description
                        })
                        .ToList()
                })
                .OrderBy(p => p.DueDate)
                .ToList()
        };
    }
}