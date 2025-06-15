using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APBD_06.models;

[Table("Prescription")]
public class Prescription
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    [ForeignKey("Patient")]
    public int IdPatient { get; set; }
    public Patient Patient { get; set; }
    [ForeignKey("Doctor")]
    public int IdDoctor { get; set; }
    public Doctor Doctor { get; set; }
    [JsonIgnore] 
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}