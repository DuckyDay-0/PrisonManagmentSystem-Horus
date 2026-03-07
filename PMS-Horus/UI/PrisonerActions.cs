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
        //public async Task AddPrisonerMenu()
        //{
        //    PrisonerActions prisonerActions = new PrisonerActions(services);
        //   await prisonerActions.AddPrisonerMenuAsync();
        //   //await AddPrisonerMenuAsync();
        //}
        public async Task AddPrisonerMenuAsync()
        {
            Console.WriteLine("Add Prisoner ->");
            Console.WriteLine();
            try
            {
                Console.WriteLine("Name: ");
                string name = Console.ReadLine();

                Console.WriteLine("Age: ");
                int age = int.Parse(Console.ReadLine());

                Console.WriteLine("Entry Date: ");
                DateOnly entryDate = DateOnly.Parse(Console.ReadLine());

                Console.WriteLine("Sentence Lenght: ");
                int sentenceLenght = int.Parse(Console.ReadLine());

                Console.WriteLine("Crime Commited");
                string crime = Console.ReadLine();

                Console.WriteLine("Cell Block");
                string prisonBlock = Console.ReadLine();

                Console.WriteLine("Cell");
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

        public static void GetAllPrisoners()
        { 
        
        }

        public static void SearchForPrisoner()
        { 
        
        }
    }
}
