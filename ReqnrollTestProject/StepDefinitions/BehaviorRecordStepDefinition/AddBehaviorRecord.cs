using Microsoft.EntityFrameworkCore;
using PMS_Horus.Data;
using PMS_Horus.Models;
using PMS_Horus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqnrollTestProject.StepDefinitions.BehaviorRecordStepDefinition
{
    internal class AddBehaviorRecord
    {
        private PrisonDBContext context;
        private BehaviorRecordService behaviorServices;
        private string currentUserRole = "Correctional Officer";
        private ResultService<BehaviorRecord>? resultService;
        [Given("The system is ready for behavior records")]
        public void GivenTheSystemIsReadyForBehaviorRecords()
        {
            string dbName = "TestDB" + Guid.NewGuid();
            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            context = new PrisonDBContext(options);
            behaviorServices = new BehaviorRecordService(context);
        }


        [Given("There are three prisoners registered in the system")]
        public void GivenThereAreThreePrisonersRegisteredInTheSystem()
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

        [When("User adds a Behavior Record with Severity {string}, Description {string} for prisoner with PIDN {int}")]
        public async Task WhenUserAddsABehaviorRecordWithSeverityDescriptionForPrisonerWithPIDN(string behavior, string description, int pidn)
        {
            var behaviorRecord = new BehaviorRecord { Severity = behavior, Description = description, PrisonerId = pidn };

            resultService = await behaviorServices.AddBehaviorRecordAsync(behaviorRecord, currentUserRole);
        }


        [Then("The behavior record is saved successfully")]
        public void ThenTheBehaviorRecordIsSavedSuccessfully()
        {
            throw new PendingStepException();
        }

        [Given("User is assigned with a {string} role")]
        public void GivenUserIsAssignedWithARole(string medic)
        {
            throw new PendingStepException();
        }

        [Then("The system returns error {string}")]
        public void ThenTheSystemReturnsError(string p0)
        {
            throw new PendingStepException();
        }

        [Given("There are no prisoners in the database for behavior record to be added")]
        public void GivenThereAreNoPrisonersInTheDatabaseForBehaviorRecordToBeAdded()
        {
            throw new PendingStepException();
        }

        [When("User adds a Behavior Record with Severity {string}, Description {string} for prisoner with PIDN {int}")]
        public void WhenUserAddsABehaviorRecordWithSeverityDescriptionForPrisonerWithPIDN(string bad, string escape, int p2)
        {
            throw new PendingStepException();
        }

        [When("User adds a Behavior Record with Severity {string}, Description {string} for prisoner with PIDN {int}")]
        public void WhenUserAddsABehaviorRecordWithSeverityDescriptionForPrisonerWithPIDN(string good, string p1, int p2)
        {
            throw new PendingStepException();
        }

        [When("User adds a Behavior Record with Severity {string}, Description {string} for prisoner with PIDN {int}")]
        public void WhenUserAddsABehaviorRecordWithSeverityDescriptionForPrisonerWithPIDN(string bad, string p1, int p2)
        {
            throw new PendingStepException();
        }

        [Then("The prisoner has {int} behavior records in total")]
        public void ThenThePrisonerHasBehaviorRecordsInTotal(int p0)
        {
            throw new PendingStepException();
        }

    }
}
