using Microsoft.Extensions.Options;
using PMS_Horus.Interfaces;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.UI
{
    internal class PrisonerExtensionActions
    {
        private ValidationServices validationServices;
        private IPrisonerExtensionServices extensionServices;
        private string currentUserRole = "Medic";

        public PrisonerExtensionActions(IPrisonerExtensionServices extensionServices)
        {
            this.extensionServices = extensionServices;
        }

        public async Task MedicalRecordActions()
        {
            bool running = true;
            while (running)
            {

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
                        await AddPrisonerMenuAsync();
                        break;
                    case 0:
                        running = false;
                        break;

                    default:
                        throw new Exception("Invalid Data.");
                }
            }


        }
        public async Task ShowMedicalRecordAsync()
        {
            Console.Clear();
            Console.Write("Please enter the Prisoner's Personal ID:");
            int prisonerId = validationServices.ReadInt();
            MedicalRecord medicalRecord = await extensionServices.GetMedicalRecordAsync(prisonerId);

            Console.WriteLine($"{medicalRecord.Id} | {medicalRecord.Allergies}");
            Console.WriteLine($"{medicalRecord.BloodType} | {medicalRecord.ChronicConditions}");
        }

        public async Task AddPrisonerMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("Add Prisoner ->");
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

                await extensionServices.AddMedicalRecordAsync(medicalRecord,currentUserRole);
                Console.WriteLine("Prisoner Added!");

            }

            catch (InvalidDataException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
