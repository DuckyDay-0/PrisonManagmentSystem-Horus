using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS_Horus.Data;
using PMS_Horus.Models;
using Reqnroll;

namespace ReqnrollTestProject.StepDefinitions
{
    [Binding]
    public class AddPrisonerStepDefinitions
    {
        private Prisoner prisoner;
        private PrisonDBContext dbContext;
        private int prisonCapacity = 100;
        private bool isAdmin = true;
        private Exception exception;

        [Given("The prison capacity is {int}")]
        public void GivenThePrisonCapacityIs(int prisonCapacity)
        {
            this.prisonCapacity = prisonCapacity;

            var options = new DbContextOptionsBuilder<PrisonDBContext>()
                .UseInMemoryDatabase(databaseName: "HorusDB")
                .Options;

            dbContext = new PrisonDBContext(options);
        }

        [When("I add a prisoner with Name {string}, Age {int}, crime {string}, Entry Date {string}, Sentence Lenght {int}, Release Date {string}")]
        public void WhenIAddAPrisonerWithNameAgeCrimeEntryDateSentenceLenghtReleaseDate(string name, int age, string crime, string entryDateStr, int sentenceLenght, string releaseDateStr)
        {
            DateOnly entryDate = DateOnly.Parse(entryDateStr);
            DateOnly releaseDate = DateOnly.Parse(releaseDateStr);
            var currentPrisonerCount = dbContext.Prisoners.Count();

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action!");
            }

            try
            {
                if (currentPrisonerCount > prisonCapacity)
                {
                    throw new Exception("Prison Capacity Full! Please transfer to another prison!");
                }
                if (age < 0 && name.IsNullOrEmpty() && crime.IsNullOrEmpty() && sentenceLenght < 0)
                {
                    throw new Exception("Invalid input. Try again.");
                }

                prisoner = new Prisoner
                {
                    Name = name,
                    Age = age,
                    Crime = crime,
                    EntryDate = entryDate,
                    SentenceLenght = sentenceLenght,
                    ReleaseDate = entryDate.AddYears(sentenceLenght)
                };

                dbContext.Add(prisoner);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            { 
                exception = ex;
            }
        }

        [Then("Prisoner will be added, an ID will be generated and the Current Prisoner Count will increase")]
        public void ThenPrisonerWillBeAddedAnIDWillBeGeneratedAndTheCurrentPrisonerCountWillIncrease()
        {
            Assert.NotNull(prisoner);
            Assert.True(prisoner.Id > 0);
            Assert.Null(exception);
            Assert.Equal(1, dbContext.Prisoners.Count());
        }

            [Given("The prison capacity is {int} and im not authorized to add prisoners")]
    public void GivenThePrisonCapacityIsAndImNotAuthorizedToAddPrisoners(int p0)
    {
        throw new PendingStepException();
    }

    [When("I try to add a prisoner with Name {string}, Age {int}, crime {string}, Entry Date {string}, Sentence Lenght {int}, Release Date {string}")]
    public void WhenITryToAddAPrisonerWithNameAgeCrimeEntryDateSentenceLenghtReleaseDate(string p0, int p1, string p2, string p3, int p4, string p5)
    {
        throw new PendingStepException();
    }

    [Then("I receive an exception message")]
    public void ThenIReceiveAnExceptionMessage()
    {
        throw new PendingStepException();
    }

    }
}
