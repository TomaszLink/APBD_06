namespace APBD_06.apimodels.responses;

public class PrescriptionResponse
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentResponse> Medicaments { get; set; }
    public DoctorResponse Doctor { get; set; }
}