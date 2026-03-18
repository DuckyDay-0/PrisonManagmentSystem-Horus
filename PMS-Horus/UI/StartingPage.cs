using PMS_Horus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.UI
{
    internal class StartingPage
    {
        IPrisonerServices service;
        PrisonerActions actions;
        public StartingPage(IPrisonerServices service, PrisonerActions actions) 
        {
            this.service = service;
            this.actions = actions;
        }
        public async Task RunAsync()
        {
            bool running = true;
            while (running)
            {

                Console.WriteLine("----PMS-Horus---Control-Panel----");
                Console.WriteLine();
                Console.WriteLine("1.Add Prisoner.");
                Console.WriteLine("2.Show All Prisoners.");
                Console.WriteLine("3.Remove Prisoner.");
                Console.WriteLine("4.Update Prisoner.");
                Console.WriteLine("5.Search for Prisoner.");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        await actions.AddPrisonerMenuAsync();
                        break;

                    case 2:
                        await actions.GetAllPrisoners();
                        break;

                    case 3:
                        await actions.RemovePrisonerAsync();
                        break;

                    case 4:
                        await actions.UpdatePrisonerAsync();
                        break;

                    case 5:
                        await actions.SearchForPrisoner();
                        break;
                    case 0:
                        running = false;
                        break;

                    default:
                        throw new Exception("Invalid Data.");
                }
            }
            

        }
    }
}
