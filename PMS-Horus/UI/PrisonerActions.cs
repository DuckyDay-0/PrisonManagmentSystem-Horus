using Microsoft.IdentityModel.Tokens;
using PMS_Horus.Data;
using PMS_Horus.Interfaces;
using PMS_Horus.Models;
using PMS_Horus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.UI
{
    internal class PrisonerActions
    {
        private IPrisonerServices services;
        private string currentUserRole = "Admin";
        private ValidationServices validationServices = new ValidationServices();
        public PrisonerActions(IPrisonerServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task AddPrisonerMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("Add Prisoner ->");
            Console.WriteLine();
            try
            {
                Console.Write("First Name: ");
                string firstName = validationServices.ReadString();

                Console.Write("Last Name: ");
                string lastName = validationServices.ReadString();

                Console.Write("Personal ID Number Number: ");
                int personalIDNumber = validationServices.ReadInt();

                Console.Write("Age: ");
                int age = validationServices.ReadInt();

                Console.Write("Entry Date: ");
                DateOnly entryDate = validationServices.ReadDateOnly();

                Console.Write("Sentence Lenght: ");
                int sentenceLenght = validationServices.ReadInt();

                Console.Write("Crime: ");
                string crime = validationServices.ReadString();

                Console.Write("Prison Block: ");
                string prisonBlock = validationServices.ReadString();

                Console.Write("Prison Cell: ");
                int prisonCell = validationServices.ReadInt();

                var prisoner = new Prisoner
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PersonalIDNumber = personalIDNumber,
                    Age = age,
                    EntryDate = entryDate,
                    SentenceLenght = sentenceLenght,
                    ReleaseDate = entryDate.AddYears(sentenceLenght),
                    Crime = crime,
                    PrisonBlock = prisonBlock,
                    PrisonCell = prisonCell
                };

                await services.AddPrisonerAsync(prisoner, currentUserRole);
                Console.WriteLine("Prisoner Added!");
                    
            }

            catch (InvalidDataException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task RemovePrisonerAsync()
        {
            Console.Clear();
            Console.WriteLine("Please enter the ID Number of the prisoner you want to remove.");
            Console.WriteLine("Press any key or 0 to cancel.");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int id) || id <= 0)
            {
                Console.WriteLine("Operation Cancelled.");
                Console.WriteLine("Press Any button to continue!");
                Console.ReadKey();
                return;
            }

            var prisoner = await services.GetPrisonerByIDAsync(id);
            if (prisoner == null)
            {
                Console.WriteLine($"No Prisoner Found with ID: {id}");
                return;
            }

            //Confirmation
            Console.WriteLine("Confirm if this is the prisoner you want to delete.");
            Console.WriteLine();
            Console.WriteLine($"{id} | Full Name: {prisoner.FirstName} {prisoner.LastName}");
            Console.WriteLine($"Crime: {prisoner.Crime}");
            Console.WriteLine($"Sentence Lenght: {prisoner.SentenceLenght}");
            Console.WriteLine("If you want to continue please type ~Yes~");
            string confirmation = validationServices.ReadString();
            if (confirmation != "Yes")
            {
                Console.WriteLine("Operation Cancelled");
                Console.ReadKey();
                return;
            }

            await services.DeletePrisonerAsync(id, currentUserRole);
            Console.WriteLine("Prisoner Deleted!");
        }

        public async Task UpdatePrisonerAsync()
        {
            Console.WriteLine("Update Prisoner");
            Console.WriteLine("What do you want to update?");
            Console.WriteLine("1.Full Name.");
            Console.WriteLine("2.Age.");
            Console.WriteLine("3.Crime.");
            Console.WriteLine("4.Sentence Lenght.");
            Console.WriteLine("5.Prison Block and Prisone Cell.");
            Console.WriteLine("6.Entry Date.");
            Console.WriteLine("7.ID Number.");

            int choice = int.Parse(Console.ReadLine());

            Console.Write("Please Provide ID of the prisoner you want to update:");
            int id = int.Parse(Console.ReadLine());
            string newValue = Console.ReadLine();
            await services.UpdatePrisonerAsync(id, choice, currentUserRole, newValue);
        }

        public async Task GetAllPrisoners()
        {
            Console.Clear();
            var prisoners = await services.GetAllPrisonersAsync();
            if (prisoners.IsNullOrEmpty())
            {
                Console.WriteLine("No Prisoners registered at the prison.");
                Console.ReadLine();
                return;
            }
            try
            {
                foreach (var prisoner in prisoners)
                {
                    Console.WriteLine($"ID Number:  | Full Name: {prisoner.FirstName} {prisoner.LastName}| Age: {prisoner.Age} | Crime: {prisoner.Crime}");
                    Console.WriteLine($"Entry Date: {prisoner.EntryDate} | Release Date: {prisoner.ReleaseDate} | Sentence Lenght: {prisoner.SentenceLenght}");
                    Console.WriteLine($"Cell Block: {prisoner.PrisonBlock} | Cell: {prisoner.PrisonCell}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();
        }

        public async Task SearchForPrisoner()
        {
            Console.Clear();
            Console.WriteLine("Please enter one of the following -> id/name/crime/age/entry date/cell/cell block");
            var keywrod = Console.ReadLine();
            if (keywrod.IsNullOrEmpty())
            {
                throw new Exception("Invalid Data");
            }
            var prisonersResult = await services.SearchPrisonerAsync(keywrod);
            try
            {
                foreach (var prisoner in prisonersResult)
                {
                    Console.WriteLine($"ID: {prisoner.Id} | Full Name: {prisoner.FirstName} {prisoner.LastName} | Age: {prisoner.Age} | Crime: {prisoner.Crime}");
                    Console.WriteLine($"Entry Date: {prisoner.EntryDate} | Release Date: {prisoner.ReleaseDate} | Sentence Lenght: {prisoner.SentenceLenght}");
                    Console.WriteLine($"Cell Block: {prisoner.PrisonBlock} | Cell: {prisoner.PrisonCell}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine();
        }





    }
}
