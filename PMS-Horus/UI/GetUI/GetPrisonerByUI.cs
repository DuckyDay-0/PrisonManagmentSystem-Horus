using PMS_Horus.Models;
using PMS_Horus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.UI.GetUI
{
    internal class GetPrisonerByUI
    {
        private ValidationServices validationServices = new ValidationServices();
        private PrisonerActions actions;
        public GetPrisonerByUI(PrisonerActions actions)
        { 
            this.actions = actions;
        }
        public async Task GetPrisonerByUIMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose citeria.");
            Console.WriteLine("1. By Name.");
            Console.WriteLine("2. By Personal ID Number.");

            int choice = validationServices.ReadInt();
            try
            {
                switch (choice)
                {
                    case 1:
                        await actions.GetPrisonerByNameAsync();
                        break;

                    case 2:
                        await actions.GetPrisonerByPIDN();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}
