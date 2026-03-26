using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;
using System;

namespace ReqnrollTestProject.StepDefinitions.MedicalRecordStepDefinition
{
    [Binding]
    public class GetMedicalRecordStepDefinitions
    {
        private PrisonDBContext context;
        private PrisonerExtensionServices extensionServices;
        private string currentUserRole = "Medic";
        private ResultService<MedicalRecord>? resultService;

        [Given("The system is ready to get a medical record")]
        public void GivenTheSystemIsReadyToGetAMedicalRecord()
        {
            string dbName = "TestDB" + Guid.NewGuid();
            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            context = new PrisonDBContext(options);
            extensionServices = new PrisonerExtensionServices(context);
        }


        [Given("There are prisoners in the database.")]
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

        [Given("The prisoner with PIDN {int} has a medical record.")]
        public async Task GivenThePrisonerWithPIDNHasAMedicalRecord_(int pidn)
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

        [When("User tries to get Medical Record for prisoner with PIDN {int}.")]
        public async Task WhenUserTriesToGetMedicalRecordForPrisonerWithPIDN_(int pidn)
        {
            resultService = await extensionServices.GetMedicalRecordAsync(pidn, currentUserRole);
        }

        [Then("The system will return the medical record for prisoner with PIDN {int}.")]
        public void ThenTheSystemWillReturnTheMedicalRecordForPrisonerWithPIDN_(int pidn)
        {
            var prisoner = context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == pidn);

            Assert.Equal("Medical Record is Available", resultService.Message);
            Assert.True(resultService.Success);


            Assert.Equal(prisoner.Id, resultService.Data.PrisonerId);
        }



        [When("User tries to get Medical Record for prisoner with PIDN {int}")]
        public async Task WhenUserTriesToGetMedicalRecordForPrisonerWithPIDN(int pidn)
        {
            resultService = await extensionServices.GetMedicalRecordAsync(pidn, currentUserRole); 
        }

        [Then("The system should show an error saying {string}")]
        public void ThenTheSystemShouldShowAnErrorSaying(string message)
        {
            Assert.Equal(message, resultService.Message);
        }

        [Given("The prisoner with PIDN {int} does not have a medical record")]
        public void GivenThePrisonerWithPIDNDoesNotHaveAMedicalRecord(int p0)
        {
        }

        [Given("User is assigned with {string} role")]
        public void GivenUserIsAssignedWithRole(string role)
        {
            currentUserRole = role;
        }
    }
}
