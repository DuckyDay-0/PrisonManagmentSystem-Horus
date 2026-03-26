using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;
using System;
using System.Threading.Tasks;

namespace ReqnrollTestProject.StepDefinitions
{
    [Binding]
    public class DeletePrisonerStepDefinitions
    {
        private PrisonDBContext context;
        private PrisonerService service;
        private string currentUserRole = "Medic";
        private ResultService<Prisoner>? resultService;
        private PrisonerExtensionServices extensionServices;
        private ResultService<MedicalRecord>? medRecordResultService;


        [Given("The system is ready for prisoner to be deleted")]
        public void GivenTheSystemIsReadyForPrisonerToBeDeleted()
        {
            string dbName = "HorusDB" + Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            context = new PrisonDBContext(options);
            service = new PrisonerService(context);
            extensionServices = new PrisonerExtensionServices(context);
        }

        [Given("There are three prisoners in the database ready to be deleted")]
        public void GivenThereAreThreePrisonersInTheDatabaseReadyToBeDeleted()
        {
            var baseDate = new DateOnly(2023, 05, 10);
            var prisoners = new List<Prisoner>()
            {
                new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = 31, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 11221},
                new Prisoner { FirstName = "Michel", LastName = "Thist", Age = 31, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 12345},
                new Prisoner { FirstName = "Doni", LastName = "Donni", Age = 31, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 343123}
            };

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
        }

        [Given("The User is assigned with an {string} role")]
        public void GivenTheUserIsAssignedWithAnRole(string role)
        {
            currentUserRole = role;
        }

        [When("User tries to delete prisoner with PIDN {int}")]
        public async Task WhenUserTriesToDeletePrisonerWithPIDN(int pidn)
        {
            resultService = await service.DeletePrisonerAsync(pidn, currentUserRole);
        }

        [Then("The system will return a message and the prisoner with PIDN {int} will be deleted")]
        public async Task ThenTheSystemWillReturnAMessageAndThePrisonerWithPIDNWillBeDeleted(int pidn)
        {
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == pidn);

            Assert.Equal("Prisoner Deleted", resultService.Message);
            Assert.Null(prisoner);
            
        }

        [Then("The system will return a message saying {string} and resultService will return false")]
        public void ThenTheSystemWillReturnAMessageSayingAndResultServiceWillReturnFalse(string message)
        {
            Assert.Equal(message, resultService.Message);
            Assert.False(resultService.Success);
        }

        [Given("The Prisoner with PIDN {int} has a Medical Record")]
        public async Task GivenThePrisonerWithPIDNHasAMedicalRecord(int pidn)
        {
            var medicalRecord = new MedicalRecord
            {
                PrisonerId = pidn,
                BloodType = "A+",
                Allergies = "none",
                ChronicConditions = "none"
            };
            await extensionServices.AddMedicalRecordAsync(medicalRecord, currentUserRole);
        }
        


        [Then("The system will return a message saying {string}, resultService will return true and the Medical Record for Prisoner with PIDN {int} will be removed")]
        public async Task ThenTheSystemWillReturnAMessageSayingResultServiceWillReturnTrueAndTheMedicalRecordForPrisonerWithPIDNWillBeRemoved(string message, int pidn)
        {
            var medRecord = await context.MedicalRecords.FirstOrDefaultAsync(m => m.Prisoner.PersonalIDNumber == pidn);

            Assert.Equal(message, resultService.Message);
            Assert.True(resultService.Success);
            Assert.Null(medRecord);
        }


    }
}
