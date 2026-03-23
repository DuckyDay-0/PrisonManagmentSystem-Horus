using System;
using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll;
using Reqnroll.Formatters.PayloadProcessing.Cucumber;

namespace ReqnrollTestProject.StepDefinitions
{
    [Binding]
    public class GetPrisonerStepDefinitions
    {
        private PrisonDBContext context;
        private PrisonerService service;
        private ResultService<Prisoner> resultService;
        private ResultService<List<Prisoner>> listResultService;

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
           listResultService = await service.GetAllPrisonersAsync();
        }

        [Then("The system will return all prisoners")]
        public void ThenTheSystemWillReturnAllPrisoners()
        {
            Assert.NotEmpty(listResultService.Data);   
        }


        [Given("There are no prisoners in the database")]
        public void GivenThereAreNoPrisonersInTheDatabase()
        {
        }

        [Then("The system will return an exception")]
        public void ThenTheSystemWillReturnAnException()
        {
            Assert.False(listResultService.Success);
            Assert.Equal("No prisoners are registered!", listResultService.Message);
            Assert.Empty(listResultService.Data);
        }

        [When("User tries to get prisoner with ID {int}")]
        public async Task WhenUserTriesToGetPrisonerWithID(int id)
        {
            resultService = await service.GetPrisonerByIDAsync(id);
        }

        [Then("The system will return prisoner with ID {int}")]
        public void ThenTheSystemWillReturnPrisonerWithID(int id)
        {
            Assert.NotNull(resultService);
            Assert.Equal(id, resultService.Data.PrisonerId);
            Assert.Equal("Doni", resultService.Data.FirstName);
        }
        
        [When("User tries to get prisoner with FirstName {string} and LastName {string}")]
        public async Task WhenUserTriesToGetPrisonerWithFirstNameAndLastName(string firstName, string lastName)
        {      
            resultService = await service.GetPrisonerByNameAsync(firstName, lastName);           
        }

        [Then("The system will return prisoner with Name {string} and ID {int}")]
        public void ThenTheSystemWillReturnPrisonerWithNameAndID(string name, int id)
        {
            Assert.NotNull(resultService.Data);
            Assert.Equal(id, resultService.Data.PrisonerId);
            Assert.Equal($"{name}", resultService.Data.FirstName);  
        }

        [When("Prisoner with this Name does not exists")]
        public void WhenPrisonerWithThisNameDoesNotExists()
        {
            Assert.Null(resultService.Data);
        }

        [Then("The system will throw an error message")]
        public void ThenTheSystemWillThrowAnErrorMessage()
        {
            Assert.Equal("No Prisoner with those details!", resultService.Message);
        }

    }
}
