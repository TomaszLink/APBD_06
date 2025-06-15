using APBD_06.exceptions;
using APBD_06.models;
using APBD_06.repositorycontext;
using APBD_06.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var optionsBuilder = new DbContextOptionsBuilder<Repository>();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
var repository = new Repository(optionsBuilder.Options);
var prescriptionService = new PrescriptionService(repository);

app.MapPost("/api/prescriptions", (CreatePrescriptionRequest request) =>
{
    try
    {
        prescriptionService.CreateNewPrescription(request);
        return Results.Created();

    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/patients/{id}", (int id) =>
{
    try
    {
        var patientResponse = prescriptionService.GetPatientById(id);
        return Results.Ok(patientResponse);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (PatientException ex)
    {
        return Results.NotFound(ex.Message);
    }
});


app.Run();