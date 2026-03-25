using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using PMS_Horus.UI;
using Reqnroll;
using System;
using System.Threading.Tasks;

namespace ReqnrollTestProject.StepDefinitions.MedicalRecordStepDefinition
{
    [Binding]
    public class AddMedicalRecordStepDefinitions
    {
        private PrisonDBContext context;
        private PrisonerExtensionServices extensionServices;
        private string currentUserRole = "Medic";
        private ResultService<MedicalRecord>? resultService;

        [Given("The system is ready for medical record to be added")]
        public void GivenTheSystemIsReadyForMedicalRecordToBeAdded()
        {
            string dbName = "TestDB" + Guid.NewGuid();
            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            context = new PrisonDBContext(options);
            extensionServices = new PrisonerExtensionServices(context);
        }

        [Given("There are prisoners in the database")]
        public void GivenThereArePrisonersInTheDatabase()
        {
            var baseDate = new DateOnly(2023, 05, 10);
            var prisoners = new List<Prisoner>()
            {
                new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = 31, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 11221},
                new Prisoner { FirstName = "Michel", LastName = "Thist", Age = 31, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 123421455},
                new Prisoner { FirstName = "Doni", LastName = "Donni", Age = 31, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 343123}
            };

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
        }

        [When("User tries to add Medical Record for prisoner with PIDN {int}")]
        public async Task WhenUserTriesToAddMedicalRecordForPrisonerWithPIDN(int pidn)
        {
            var medicalRecord = new MedicalRecord
            {
                PrisonerId = pidn,
                BloodType = "A+",
                Allergies = "none",
                ChronicConditions = "none"
            };
          resultService = await extensionServices.AddMedicalRecordAsync(medicalRecord, currentUserRole);
        }

        [Then("Medical Record will be added for the prisoner")]
        public async Task ThenMedicalRecordWillBeAddedForThePrisoner()
        {
            var result = await context.MedicalRecords.ToListAsync();
            Assert.NotNull(result);
        }

        [Given("There are no prisoners")]
        public void GivenThereAreNoPrisoners()
        {
        }

        [Then("The system will show an error message and medical record won't be added")]
        public void ThenTheSystemWillShowAnErrorMessageAndMedicalRecordWontBeAdded()
        {
            Assert.False(resultService.Success);
            Assert.Equal("There are no prisoners registered in the system", resultService.Message);
        }
    }
}
