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
        private Exception lastException;
        private string exceptionMsg;
        private string adminRole = "Admin";
        private string officerRole = "Correctional Role";


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
                new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 12121212},
                new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 12121212},
                new Prisoner { FirstName = "Hary", LastName = "Jackson", Age = 31, Crime = "Murder", EntryDate = new DateOnly(12,12,12), SentenceLenght = 2, ReleaseDate = new DateOnly(12,12,12).AddYears(2), PrisonBlock = "O Block", PrisonCell = 12, PersonalIDNumber = 12121212}
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

    }
}
