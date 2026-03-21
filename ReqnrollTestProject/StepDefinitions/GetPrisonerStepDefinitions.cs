using System;
using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;

namespace ReqnrollTestProject.StepDefinitions
{
    [Binding]
    public class GetPrisonerStepDefinitions
    {
        private PrisonDBContext context;
        private PrisonerService service;
        private List<Prisoner> resultList;
        private Prisoner result;
        private Exception lastException;
        private string exceptionMsg;


        [Given("The system is ready to get prisoners")]
        public void GivenTheSystemIsReadyToGetPrisoners()
        {
            string dbName = "TestDB" + Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            context = new PrisonDBContext(options);
            service = new PrisonerService(context);
        }

        [Given("There are three prisoners in the database")]
        public void GivenThereAreThreePrisonersInTheDatabase()
        {
            var prisoners = new List<Prisoner>()
            {
                new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 121241212},
                new Prisoner { FirstName = "Michel", LastName = "Thist", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 123421455},
                new Prisoner { FirstName = "Doni", LastName = "Donni", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 343123}
            };

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
        }

        [When("User tries to get all prisoners")]
        public async Task WhenUserTriesToGetAllPrisoners()
        {
            try
            {
                resultList = await service.GetAllPrisonersAsync();
            }
            catch (Exception ex)
            {
                lastException = ex;
                exceptionMsg = lastException.Message;
            }
        }

        [Then("The system will return all prisoners")]
        public void ThenTheSystemWillReturnAllPrisoners()
        {
            Assert.NotEmpty(resultList);   
        }

        //No prisoners in the db
        [Then("The system will return an exception")]
        public void ThenTheSystemWillReturnAnException()
        {
            Assert.NotNull(lastException);
            Assert.Contains("No prisoners are registered!", exceptionMsg);
        }

        [When("User tries to get prisoner with ID {int}")]
        public async Task WhenUserTriesToGetPrisonerWithID(int id)
        {
            try
            {
               result = await service.GetPrisonerByIDAsync(id);
            }
            catch(Exception e)
            {
                lastException = e;
                exceptionMsg = lastException.Message;
            }
        }

        [Then("The system will return prisoner with ID {int}")]
        public void ThenTheSystemWillReturnPrisonerWithID(int id)
        {
            Assert.NotNull(result);
            Assert.Equal(id, result.PrisonerId);
            Assert.Equal("Doni", result.FirstName);
        }
        
        [When("User tries to get prisoner with FirstName {string} and LastName {string}")]
        public async Task WhenUserTriesToGetPrisonerWithFirstNameAndLastName(string firstName, string lastName)
        {           
            try
            {
                result = await service.GetPrisonerByNameAsync(firstName, lastName);
            }
            catch(Exception e)
            { 
                lastException = e;
                exceptionMsg = lastException.Message;
            }
        }

        [Then("The system will return prisoner with Name {string} and ID {int}")]
        public void ThenTheSystemWillReturnPrisonerWithNameAndID(string name, int id)
        {
            Assert.NotNull(result);
            Assert.Equal(id, result.PrisonerId);
            Assert.Equal($"{name}", result.FirstName);  
        }

        [When("Prisoner with this Name does not exists")]
        public void WhenPrisonerWithThisNameDoesNotExists()
        {
            Assert.Null(result);
        }

        [Then("The system will throw an error message")]
        public void ThenTheSystemWillThrowAnErrorMessage()
        {
            Assert.NotNull(lastException);
            Assert.Equal("No Prisoner with those details!", exceptionMsg);
        }

    }
}
