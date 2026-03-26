using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ReqnrollTestProject.StepDefinitions.PrisonerStepDefinition
{
    [Binding]
    public class AddPrisonerStepDefinitions
    {
        private PrisonDBContext context;
        private string currentUserRole = "Admin";
        private PrisonerService services;
        private ResultService<Prisoner> resultService;

        [Given("The system is ready for prisoner to be added")]
        public void GivenTheSystemIsReadyForPrisonerToBeAdded()
        {
            string dbName = "TestDB" + Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            context = new PrisonDBContext(options);

            services = new PrisonerService(context);
        }

        [When("User adds a prisoner with FirsName {string}, LastName {string}, Age {int}, crime {string}, Entry Date {string}, Sentence Lenght {int}, Release Date {string}, Prison Block {string}, Prison Cell {int}")]
        public async Task WhenUserAddsAPrisonerWithFirsNameLastNameAgeCrimeEntryDateSentenceLenghtReleaseDatePrisonBlockPrisonCell(string firstName, string lastName, int age, string crime, string entryDate, int sentenceLenght, string releaseDate, string prisonBlock, int prisonCell)
        {
            DateOnly entryDateParsed = DateOnly.Parse(entryDate);
            services = new PrisonerService(context);
            var prisoner = new Prisoner()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Crime = crime,
                EntryDate = entryDateParsed,
                SentenceLenght = sentenceLenght,
                ReleaseDate = entryDateParsed.AddYears(sentenceLenght),
                PrisonBlock = prisonBlock,
                PrisonCell = prisonCell
            };

           resultService = await services.AddPrisonerAsync(prisoner, currentUserRole);
        }

        [Then("Prisoner will be added, an ID will be generated")]
        public void ThenPrisonerWillBeAddedAnIDWillBeGenerated()
        {
            Assert.True(resultService.Success);
            Assert.NotEqual(0, resultService.Data.PrisonerId);
        }


        [Given("User is not with an {string} role")]
        public void GivenUserIsNotWithAnRole(string admin)
        {
            currentUserRole = "Correctional Officer";
        }


        [Then("No prisoner will be added")]
        public void NoPrisonerWillBeAdded()
        {
            Assert.False(resultService.Success);
        }


        [Given("User is with an {string} role")]
        public void GivenUserIsWithAnRole(string admin)
        {
            currentUserRole = "Admin";
        }

        [When("User tries to add a prisoner with no Name")]
        public async Task WhenITryToAddAPrisonerWithNoName()
        {
            var prisoner = new Prisoner() { FirstName = "" };       
            resultService = await services.AddPrisonerAsync(prisoner, currentUserRole);
        }

        [Then("User receives an exception and no prisoner is added")]
        public void ThenUserReceiveAnExceptionAndNoPrisonerIsAdded()
        {
            Assert.Contains("There was a problem with the data being added. Try Again!", resultService.Message);
            Assert.False(resultService.Success);          
        }


        [When("User tries to add a prisoner with Age {int}")]
        public async Task WhenUserTriesToAddAPrisonerWithAge(int age)
        {
            var baseDate = new DateOnly(2023, 05, 10);

            var prisoner = new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = age, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 11221 };
           resultService = await services.AddPrisonerAsync(prisoner, currentUserRole);
        }
       
        [Then("User receives an error message saying {string} and no prisoner is added")]
        public async Task ThenUserReceivesAnErrorMessageSayingAndNoPrisonerIsAdded(string message)
        {
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == resultService.Data.PersonalIDNumber);
            Assert.Null(prisoner);
            Assert.Equal(message, resultService.Message);
        }

        [When("User tries to add a prisoner with negative Sentence Length")]
        public void WhenUserTriesToAddAPrisonerWithNegativeSentenceLength()
        {
            throw new PendingStepException();
        }

        [Given("There is already a prisoner with PersonalIDNumber {int}")]
        public void GivenThereIsAlreadyAPrisonerWithPersonalIDNumber(int pidn)
        {
            var baseDate = new DateOnly(2023, 05, 10);
            var prisoners = new List<Prisoner>()
            {
                new Prisoner { FirstName = "Doni", LastName = "Donni", Age = 31, Crime = "Murder", EntryDate = baseDate, SentenceLenght = 2, ReleaseDate = baseDate.AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 0000}
            };

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
        }

        [When("User tries to add a prisoner with PersonalIDNumber {int}")]
        public void WhenUserTriesToAddAPrisonerWithPersonalIDNumber(int pidn)
        {
            throw new PendingStepException();
        }

    }
}
