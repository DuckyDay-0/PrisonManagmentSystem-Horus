using Microsoft.Extensions.Options;
using PMS_Horus.Interfaces;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll.Assist;
using Reqnroll.Formatters.PayloadProcessing.Cucumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.UI
{
    internal class PrisonerExtensionActions
    {
        private ValidationServices validationServices = new ValidationServices();
        private IPrisonerExtensionServices extensionServices;
        private string currentUserRole = "Medic";

        public PrisonerExtensionActions(IPrisonerExtensionServices extensionServices)
        {
            this.extensionServices = extensionServices;
        }

        public async Task MedicalRecordActions()
        {
            Console.Clear();
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("====Prison System 'Horus'====");
                Console.WriteLine("====Medical Record====");
                Console.WriteLine();
                Console.WriteLine("1. Show Medical Record.");
                Console.WriteLine("2. Add Medical Record.");
                Console.WriteLine("3. Remove Medical Record.");
                Console.WriteLine("4. Update Medical Record.");
                Console.WriteLine();
                Console.WriteLine("=============================");
                Console.WriteLine();
                Console.WriteLine("5. Go Back");
                Console.WriteLine();
                Console.WriteLine("=============================");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        await ShowMedicalRecordAsync();
                        break;

                    case 2:
                        await AddMedicalRecordAsync();
                        break;

                    case 3:
                        await DeleteMedicalRecord();
                        break;

                    case 4:
                        await UpdateMedicalRecord();
                        break;
                    case 0:
                        running = false;
                        break;

                    default:
                        throw new Exception("Invalid Data.");
                }
            }
        }

        public async Task UpdateMedicalRecord()
        {
            Console.Clear();
            Console.WriteLine("Update Medical Record:");
            Console.WriteLine("What do you want to update?");
            Console.WriteLine("1.Blood Type.");
            Console.WriteLine("2.Allergies.");
            Console.WriteLine("3.Chronic Conditions.");
            
            int choice = int.Parse(Console.ReadLine());

            Console.Write("Please Provide ID of the prisoner's medical record you want to update:");
            int id = int.Parse(Console.ReadLine());
            string newValue = Console.ReadLine();
            await extensionServices.UpdateMedicalRecordAsync(id, choice, currentUserRole, newValue);
        }
        public async Task DeleteMedicalRecord()
        {
            Console.Clear();
            Console.WriteLine("Please enter the Prisoner's PIDN: ");
            int pidn = validationServices.ReadInt();
            var result = await extensionServices.RemoveMedicalRecordAsync(pidn, currentUserRole);

            if (!result.Success)
            {
                Console.Clear();
                Console.WriteLine(result.Message);
                Console.WriteLine("Press Any button to continue!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{result.Message}");
                Console.ReadKey();
            }
        }

        public async Task ShowMedicalRecordAsync()
        {
            Console.Clear();
            Console.Write("Please enter the Prisoner's Personal ID:");
            int prisonerId = validationServices.ReadInt();
            var result = await extensionServices.GetMedicalRecordAsync(prisonerId, currentUserRole);

            if (!result.Success)
            {
                Console.Clear();
                Console.WriteLine(result.Message);
                Console.WriteLine("Press Any button to continue!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Medical Record ID: {result.Data.Id}");
                Console.WriteLine($"Allergies: {result.Data.Allergies}");
                Console.WriteLine($"Blood Type: {result.Data.BloodType} | Chronic Conditions: {result.Data.ChronicConditions}");
                Console.ReadKey();
            }


        }

        public async Task AddMedicalRecordAsync()
        {
            Console.Clear();
            if (currentUserRole != "Admin" && currentUserRole != "Medic")
            {
                Console.WriteLine("You are not authorized to perform this action!");
            }
            Console.WriteLine("Add Medical Record ->");
            Console.WriteLine();
            try
            {
                Console.Write("Prisoner ID: ");
                int prisonerID = validationServices.ReadInt();

                Console.Write("Blood Type: ");
                string bloodType = validationServices.ReadString();

                Console.Write("Allergies: ");
                string allergies = validationServices.ReadString();

                Console.Write("Chronic Conditions: ");
                string chronicConditions = validationServices.ReadString();

                var medicalRecord = new MedicalRecord
                {
                    PrisonerId = prisonerID,
                    BloodType = bloodType,
                    Allergies = allergies,
                    ChronicConditions = chronicConditions,
                };

                await extensionServices.AddMedicalRecordAsync(medicalRecord, currentUserRole);
                Console.WriteLine("Prisoner Added!");

            }

            catch (InvalidDataException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
