using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;
using System;

namespace ReqnrollTestProject.StepDefinitions.MedicalRecordStepDefinition
{
    [Binding]
    public class DeleteMedicalRecordStepDefinitions
    {
        private PrisonDBContext context;
        private PrisonerExtensionServices extensionServices;
        private string currentUserRole = "Medic";
        private ResultService<MedicalRecord>? resultService;

        [Given("The system is ready for medical record to be deleted")]
        public void GivenTheSystemIsReadyForMedicalRecordToBeDeleted()
        {
            var dbName = "HorusDB" + Guid.NewGuid();

            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                        .UseInMemoryDatabase(dbName)
                        .Options;
            context = new PrisonDBContext(options);
            extensionServices = new PrisonerExtensionServices(context);
        }

        [Given("The user is assigned with a {string} role")]
        public void GivenTheUserIsAssignedWithARole(string role)
        {
            currentUserRole = role;
        }


        [Given("There are {int} prisoners registered in the system")]
        public void GivenThereArePrisonersRegisteredInTheSystem(int p0)
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

        [Given("Prisoner with PIDN {int} has a medical record")]
        public async Task GivenPrisonerWithPIDNHasAMedicalRecord(int pidn)
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

        [When("The user tries to delete the medical record for prisoner with PIDN {int}")]
        public async Task WhenTheUserTriesToDeleteTheMedicalRecordForPrisonerWithPIDN(int pidn)
        {
            resultService = await extensionServices.RemoveMedicalRecordAsync(pidn);
        }

        [Then("The medical record will be removed from the database and message will be shown")]
        public void ThenTheMedicalRecordWillBeRemovedFromTheDatabaseAndMessageWillBeShown()
        {
            Assert.True(resultService.Success);
            Assert.Equal($"Medical Record for Prisoner removed", resultService.Message);
        }


        [Given("There is no prisoner with PIDN {int}")]
        public void GivenThereIsNoPrisonerWithPIDN(int pidn)
        {
        }

        [Then("The system will show an error message and the result service will return false")]
        public void ThenTheSystemWillShowAnErrorMessageAndTheResultServiceWillReturnFalse()
        {
            Assert.False(resultService.Success);
            Assert.Equal("No prisoner found with the given PIDN", resultService.Message);
        }


        [Given("Prisoner with PIDN {int} does not have a medical record")]
        public void GivenPrisonerWithPIDNDoesNotHaveAMedicalRecord(int pidn)
        {
        }

        [Then("Then the system will show an error message and result service will return false")]
        public void ThenThenTheSystemWillShowAnErrorMessageAndResultServiceWillReturnFalse()
        {
            Assert.False(resultService.Success);
            Assert.Equal("No Med Records Found for this prisoner.", resultService.Message);
        }


        [Given("The User is assigned a {string} role")]
        public void GivenTheUserIsAssignedARole(string role)
        {
            currentUserRole = role;
        }

        [Then("Then the system will show a role error message and result service will return false")]
        public void ThenThenTheSystemWillShowARoleErrorMessageAndResultServiceWillReturnFalse()
        {
            Assert.False(resultService.Success);
            Assert.Equal("You are not authorized to perform this kind of action!", resultService.Message);
        }

    }
}
