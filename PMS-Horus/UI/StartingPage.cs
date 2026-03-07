using PMS_Horus.Services;
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
                Console.WriteLine("1.Add Prisoner");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        await actions.AddPrisonerMenuAsync();
                        break;

                    case 0:
                        running = false;
                        break;
                }
            }
            

        }
    }
}
