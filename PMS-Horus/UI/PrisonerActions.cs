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

        public PrisonerActions(IPrisonerServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task AddPrisonerMenuAsync()
        {
            Console.WriteLine("Add Prisoner ->");
            Console.WriteLine();
            try
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Age: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Entry Date: ");
                DateOnly entryDate = DateOnly.Parse(Console.ReadLine());

                Console.Write("Sentence Lenght: ");
                int sentenceLenght = int.Parse(Console.ReadLine());

                Console.Write("Crime Commited");
                string crime = Console.ReadLine();

                Console.Write("Cell Block");
                string prisonBlock = Console.ReadLine();

                Console.Write("Cell");
                int prisoneCell = int.Parse(Console.ReadLine());

                var prisoner = new Prisoner
                {
                    Name = name,
                    Age = age,
                    EntryDate = entryDate,
                    SentenceLenght = sentenceLenght,
                    ReleaseDate = entryDate.AddYears(sentenceLenght),
                    Crime = crime,
                    PrisonBlock = prisonBlock,
                    PrisonCell = prisoneCell
                };

                await services.AddPrisonerAsync(prisoner, currentUserRole);
                Console.WriteLine("Prisoner Added!");
                    
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void RemovePrisoner()
        {

        }

        public static void UpdatePrisoner()
        { 
        
        }

        public async Task GetAllPrisoners()
        {
            var prisoners = await services.GetAllPrisonersAsync();

            try
            {
                foreach (var prisoner in prisoners)
                {
                    Console.WriteLine($"ID: {prisoner.Id} | Name: {prisoner.Name} | Age: {prisoner.Crime} | Crime: {prisoner.Crime}");
                    Console.WriteLine($"Entry Date: {prisoner.EntryDate} | Release Date: {prisoner.ReleaseDate} | Sentence Lenght: {prisoner.SentenceLenght}");
                    Console.WriteLine($"Cell Block: {prisoner.PrisonBlock} | Cell: {prisoner.PrisonCell}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void SearchForPrisoner()
        { 
        
        }
    }
}
