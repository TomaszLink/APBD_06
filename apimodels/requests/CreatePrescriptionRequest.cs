namespace APBD_06.models;

public class CreatePrescriptionRequest
{
    public int IdDoctor { get; set; }
    public PatientDto Patient { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    
}