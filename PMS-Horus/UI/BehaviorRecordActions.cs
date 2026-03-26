using PMS_Horus.Interfaces;
using PMS_Horus.Models;
using PMS_Horus.Services;
using Reqnroll.Formatters.PayloadProcessing.Cucumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.UI
{
    internal class BehaviorRecordActions
    {
        private ValidationServices validationServices = new ValidationServices();
        private IBehaviorRecordService behaviorRecordService;
        private string currentUserRole = "Correctional Officer";

        public BehaviorRecordActions(IBehaviorRecordService behaviorRecordService)
        {
            this.behaviorRecordService = behaviorRecordService;
        }

        public async Task BehaviorRecordActionsUI()
        {
            Console.Clear();
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("====Prison System 'Horus'====");
                Console.WriteLine("====Behavior Record====");
                Console.WriteLine();
                Console.WriteLine("1. Show Behavior Record.");
                Console.WriteLine("2. Add Behavior Record.");
                Console.WriteLine("3. Remove Behavior Record.");
                Console.WriteLine("4. Update Behavior Record.");
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
                        await ShowBehaviorRecordAsync();
                        break;

                    case 2:
                        await AddBehaviorRecordAsync();
                        break;

                    case 3:
                        await DeleteBehaviorRecord();
                        break;

                    case 4:
                        await UpdateBehaviorRecord();
                        break;

                    case 0:
                        running = false;
                        break;

                    default:
                        throw new Exception("Invalid Data.");
                }
            }
        }

        public async Task UpdateBehaviorRecord()
        {
            Console.Clear();

            if (currentUserRole != "Admin" && currentUserRole != "Correctional Officer")
            {
                Console.WriteLine("You are not authorized to perform this action!");
            }

            Console.WriteLine("Update Behavior Record:");
            Console.WriteLine("What do you want to update?");
            Console.WriteLine("1.Severity.");
            Console.WriteLine("2.Description.");
            Console.WriteLine("3.Date.");

            int choice = int.Parse(Console.ReadLine());

            Console.Write("Please Provide ID of the prisoner's behavior record you want to update:");
            int id = int.Parse(Console.ReadLine());
            string newValue = Console.ReadLine();
            await behaviorRecordService.UpdateBehaviorRecordAsync(id, choice, currentUserRole, newValue);
        }

        public async Task DeleteBehaviorRecord()
        {
            Console.Clear();

            if (currentUserRole != "Admin" && currentUserRole != "Correctional Officer")
            {
                Console.WriteLine("You are not authorized to perform this action!");
            }

            Console.WriteLine("Please enter the Prisoner's PIDN: ");
            int pidn = validationServices.ReadInt();
            var result = await behaviorRecordService.RemoveBehaviorRecordAsync(pidn, currentUserRole);

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

        public async Task ShowBehaviorRecordAsync()
        {
            Console.Clear();
            Console.Write("Please enter the Prisoner's Personal ID:");
            int prisonerId = validationServices.ReadInt();
            var result = await behaviorRecordService.GetBehaviorRecordAsync(prisonerId, currentUserRole);

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
                foreach (var behaviorRecord in result.Data)
                {
                    Console.WriteLine($"Behavior Record ID: {behaviorRecord.Id}");
                    Console.WriteLine($"Severity: {behaviorRecord.Severity}");
                    Console.WriteLine($"Description: {behaviorRecord.Description}");
                    Console.WriteLine($"Date: {behaviorRecord.DateTime}");
                    Console.WriteLine();
                }
                Console.ReadKey();
            }


        }
        public async Task AddBehaviorRecordAsync()
        {
            Console.Clear();
            if (currentUserRole != "Admin" && currentUserRole != "Correctional Officer")
            {
                Console.WriteLine("You are not authorized to perform this action!");
            }
            Console.WriteLine("Add Behavior Record ->");
            Console.WriteLine();
            try
            {
                Console.Write("Prisoner ID: ");
                int prisonerID = validationServices.ReadInt();

                Console.Write("Severity: ");
                string severity = validationServices.ReadString();

                Console.Write("Description: ");
                string description = validationServices.ReadString();

                //sConsole.WriteLine("Date the event took place:");
                DateTime dateTime = DateTime.Now;

                var behaviorRecord = new BehaviorRecord
                {
                    PrisonerId = prisonerID,
                    Severity = severity,
                    Description = description,
                    DateTime = dateTime,
                };

                await behaviorRecordService.AddBehaviorRecordAsync(behaviorRecord, currentUserRole);
                Console.WriteLine("Behavior Record Added!");

            }

            catch (InvalidDataException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }


}
