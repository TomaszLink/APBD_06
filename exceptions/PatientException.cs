namespace APBD_06.exceptions;

public class PatientException : Exception
{
    public PatientException() : base("Patient not found.") { }
}