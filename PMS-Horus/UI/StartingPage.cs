using PMS_Horus.Interfaces;
using PMS_Horus.UI.GetUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.UI
{
    internal class StartingPage
    {
        private IPrisonerServices service;
        private IPrisonerExtensionServices extensionServices;
        private PrisonerActions actions;
        private PrisonerExtensionActions extensionsActions;
        private GetPrisonerByUI getPrisonerByUI;
        public StartingPage(IPrisonerServices service, PrisonerActions actions, IPrisonerExtensionServices extensionServices, PrisonerExtensionActions extensionActions, GetPrisonerByUI getPrisonerByUI) 
        {
            this.service = service;
            this.actions = actions;
            this.extensionServices = extensionServices;
            this.extensionsActions = extensionActions;
            this.getPrisonerByUI = getPrisonerByUI;
        }
        public async Task RunAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();

                Console.WriteLine("====Prison System 'Horus'====");
                Console.WriteLine();
                Console.WriteLine("1. Show All Prisoners.");
                Console.WriteLine("2. Search for Prisoner.");
                Console.WriteLine("3. Add Prisoner.");
                Console.WriteLine("4. Update Prisoner.");
                Console.WriteLine("5. Remove Prisoner.");
                Console.WriteLine();
                Console.WriteLine("=============================");
                Console.WriteLine();
                Console.WriteLine("6. Medical Record.");
                Console.WriteLine("7. Behavior Record");
                Console.WriteLine();
                Console.WriteLine("=============================");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        await actions.GetAllPrisoners();
                        break;

                    case 2:
                        await getPrisonerByUI.GetPrisonerByUIMenu();
                        break;

                    case 3:
                        await actions.AddPrisonerMenuAsync();
                        break;

                    case 4:
                        await actions.UpdatePrisonerAsync();
                        break;

                    case 5:
                        await actions.RemovePrisonerAsync();
                        break;

                    case 6:
                        await extensionsActions.MedicalRecordActions();
                        break;

                    case 7:

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
