using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Services
{
    public class ValidationServices
    {
        private bool isValid = false;
        public int ReadInt()
        {
            int result;
            if (!int.TryParse(Console.ReadLine(), out result))
            {
                throw new InvalidDataException("Invalid Data! Try Again!");
            }

            return result;
        }
        public DateOnly ReadDateOnly()
        {
            DateOnly result;
            if (!DateOnly.TryParse(Console.ReadLine(), out result))
            {
                throw new InvalidDataException("Invalid Data! Try Again!");
            }

            return result;
        }
        public DateTime ReadDateTime()
        {
            DateTime result;
            if (!DateTime.TryParse(Console.ReadLine(), out result))
            {
                throw new InvalidDataException("Invalid Data! Try Again!");
            }

            return result;
        }
        public string ReadString()
        {
            string result = Console.ReadLine();
            
                if (string.IsNullOrEmpty(result))
                {
                    throw new InvalidDataException("Invalid Data! Try Again!");
                }            
                
            return result;
        }
    }
}
