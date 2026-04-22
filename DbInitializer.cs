using API_LM.Data;
using API_LM.Models;
using Bogus;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.Patients.Any())
            return;

        var documentTypes = new[] { "CC", "TI", "CE", "PAS" };

        var faker = new Faker<Patient>()
            .RuleFor(p => p.DocumentType, f => f.PickRandom(documentTypes))
            .RuleFor(p => p.DocumentNumber, f => f.Random.ReplaceNumbers("##########"))
            .RuleFor(p => p.FirstName, f => f.Name.FirstName(Bogus.DataSets.Name.Gender.Male))
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.BirthDate, f => DateOnly.FromDateTime(
                f.Date.Past(80, DateTime.Now.AddYears(-1))
            ))
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber("3#########"))
            .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.FirstName, p.LastName))
            .Ignore(p => p.CreatedAt);

        var patients = faker.Generate(100); // cantidad de registros

        context.Patients.AddRange(patients);
        context.SaveChanges();
    }
}