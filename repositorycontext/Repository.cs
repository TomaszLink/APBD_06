using APBD_06.models;
using Microsoft.EntityFrameworkCore;

namespace APBD_06.repositorycontext;

public class Repository(DbContextOptions<Repository> options) : DbContext(options)
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
}