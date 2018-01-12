using System;
using System.IO;
using System.Net.Mail;
using System.Linq;
/*
 * This program doesn't not follow RCF email validation rule sets.
 * This uses arbitrary email validation rules to clean CSV flies of email addresses.
 * 
 * Ray Winkelman, Jan 2018
 */
namespace NoBouncey.Core
{
    internal class Program
    {
        private static string _defaultTarget = AppDomain.CurrentDomain.BaseDirectory + "NoBounceyOutput.txt";

        private static void Main(string[] args)
        {
            //args = new string[1];
            //args[0] = @"C:\Users\rw\Desktop\emails2.csv";

            switch (args.Length)
            {
                case 1:
                    WriteToFile(args[0], _defaultTarget);
                    break;

                case 2:
                    WriteToFile(args[0], args[1]);
                    break;

                default:
                    Console.WriteLine("Hint:");
                    Console.WriteLine("Supply the source file containing an email address per line, and an optional output destination.");
                    Console.WriteLine(@"Ex: dotnet NoBouncy.dll C:\emails.csv D:\output.txt");
                    break;
            }
        }

        private static bool Validate(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email && !email.ContainsAny(@"!#$%^&*()[]{}:;'<>/?\|=+");
            }
            catch
            {
                return false;
            }
        }

        private static void WriteToFile(string from, string to)
        {
            if (!File.Exists(from))
            {
                throw new Exception("The source file doesn't exist.");
            }

            var dir = Directory.GetParent(to);
            if (!dir.Exists)
            {
                throw new Exception("The target directory doesn't exist.");
            }

            foreach (string address in File.ReadLines(from))
            {
                var addr = address.TrimEnd(',', ' ');
                if (Validate(addr))
                {
                    File.AppendAllText(to, address + Environment.NewLine);
                    Console.WriteLine(addr);
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool ContainsAny(this string str, string chars)
        {
            foreach (char c in chars)
            {
                if (str.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}