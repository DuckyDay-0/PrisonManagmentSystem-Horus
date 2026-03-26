using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;
using System;

namespace ReqnrollTestProject.StepDefinitions.PrisonerStepDefinition
{
    [Binding]
    public class UpdatePrisonerStepDefinitions
    {
        private PrisonDBContext context;
        private PrisonerService service;
        private string currentUserRole = "Medic";
        private ResultService<Prisoner>? resultService;
        //private Prisoner updatedPrisoner;

        [Given("The system is ready for prisoner to be updated")]
        public void GivenTheSystemIsReadyForPrisonerToBeUpdated()
        {
            string dbName = "HorusDB" + Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            context = new PrisonDBContext(options);
            service = new PrisonerService(context);
        }

        [Given("There are prisoners in the database to be updated")]
        public void GivenThereArePrisonersInTheDatabaseToBeUpdated()
        {
            var prisoners = new List<Prisoner>()
            {
                new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 121241212},
                new Prisoner { FirstName = "Michel", LastName = "Thist", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 123421455},
                new Prisoner { FirstName = "Doni", LastName = "Donni", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 11221}
            };

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
        }

        [Given("User is logged in with {string} role")]
        public void GivenUserIsLoggedInWithRole(string role)
        {
            currentUserRole = role;
        }

        [When("User chooses option number {int} on the menu to updates the FirstName of prisoner with PersonalIDNumber {int} to {string}")]
        public async Task WhenUserChoosesOptionNumberOnTheMenuToUpdatesTheFirstNameOfPrisonerWithPersonalIDNumberTo(int choice, int pidn, string newValue)
        {
           resultService = await service.UpdatePrisonerAsync(pidn, choice, currentUserRole, newValue);
        }

        [Then("The prisoner's FirstName should be successfully updated to {string}")]
        public void ThenThePrisonersFirstNameShouldBeSuccessfullyUpdatedTo(string newValue)
        {
            Assert.True(resultService.Success);
            Assert.Equal(newValue, resultService.Data.FirstName);
        }

        [When("User choose option number {int} on the menu to updates the Sentence Length of prisoner with PersonalIDNumber {int} to {int} years")]
        public async Task WhenUserChoosesOptionNumberOnTheMenuToUpdatesTheSentenceLengthOfPrisonerWithPersonalIDNumberToYears(int choice, int pidn, string newValue)
        {
            resultService = await service.UpdatePrisonerAsync(pidn, choice, currentUserRole, newValue);
        }

        [Then("The Sentence Length should be updated to {int}")]
        public void ThenTheSentenceLengthShouldBeUpdatedTo(int newValue)
        {
            Assert.True(resultService.Success);
            Assert.Equal(newValue, resultService.Data.SentenceLenght);
        }

        [Then("The Release Date should be automatically recalculated")]
        public void ThenTheReleaseDateShouldBeAutomaticallyRecalculated()
        {
            var expectedReleaseDate = resultService.Data.EntryDate.AddYears(resultService.Data.SentenceLenght);
            Assert.Equal(expectedReleaseDate, resultService.Data.ReleaseDate);
        }

        [When("User choose option number {int} on the menu to update the Age of prisoner with PersonalIDNumber {int} to {int}")]
        public async Task WhenUserChoosesOptionNumberOnTheMenuToUpdateTheAgeOfPrisonerWithPersonalIDNumberTo(int choice, int pidn, string newValue)
        {
            resultService = await service.UpdatePrisonerAsync(pidn, choice, currentUserRole, newValue);
        }

        [Then("The system will show an error message saying {string}")]
        public void ThenTheSystemWillShowAnErrorMessageSaying(string message)
        {
            Assert.Equal(message, resultService.Message);
            Assert.False(resultService.Success);
        }

        [When("User chooses option number {int} on the menu to update the Crime of prisoner with PersonalIDNumber {int} to {string}")]
        public async Task WhenUserChoosesOptionNumberOnTheMenuToUpdateTheCrimeOfPrisonerWithPersonalIDNumber(int pidn, int choice, string newValue)
        {
            resultService = await service.UpdatePrisonerAsync(pidn, choice, currentUserRole, newValue);
        }

    }
}
