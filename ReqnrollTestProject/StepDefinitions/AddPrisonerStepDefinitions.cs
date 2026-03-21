using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ReqnrollTestProject.StepDefinitions
{
    [Binding]
    public class AddPrisonerStepDefinitions
    {
        private PrisonDBContext context;
        private string currentUserRole = "Admin";
        private PrisonerService services;
        private Exception lastException;
        private string exceptionMsg;

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
        public void WhenUserAddsAPrisonerWithFirsNameLastNameAgeCrimeEntryDateSentenceLenghtReleaseDatePrisonBlockPrisonCell(string firstName, string lastName, int age, string crime, string entryDate, int sentenceLenght, string releaseDate, string prisonBlock, int prisonCell)
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
            services.AddPrisonerAsync(prisoner, currentUserRole);
        }

        [Then("Prisoner will be added, an ID will be generated")]
        public void ThenPrisonerWillBeAddedAnIDWillBeGenerated()
        {
            var prisoners = context.Prisoners.ToList();


            Assert.NotEmpty(prisoners);
            Assert.True(prisoners[0].PrisonerId > 0);
        }


        [Given("User is not with an {string} role")]
        public void GivenUserIsNotWithAnRole(string admin)
        {
            currentUserRole = "Correctional Officer";
        }


        [Then("No prisoner will be added")]
        public void NoPrisonerWillBeAdded()
        {
            var prisoners = context.Prisoners.ToList();

            Assert.Empty(prisoners);
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
            try
            {
                await services.AddPrisonerAsync(prisoner, currentUserRole);
            }
            catch (Exception e)
            { 
                lastException = e;
                exceptionMsg = lastException.Message;
            }
        }

        [Then("User receives an exception and no prisoner is added")]
        public void ThenUserReceiveAnExceptionAndNoPrisonerIsAdded()
        {
            Assert.NotNull(lastException);
            Assert.Contains("There was a problem with the data being added. Try Again!", exceptionMsg);
            var prisoners = context.Prisoners.ToList();
            Assert.Empty(prisoners);          
        }      
    }
}
