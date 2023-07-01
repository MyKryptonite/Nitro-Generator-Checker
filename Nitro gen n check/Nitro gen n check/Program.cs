using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Drawing;
using Console = Colorful.Console;


namespace NitroCodeChecker
{
    class Program
    {
        const string VALID_CODES_FILE = "valid_codes.txt";
        
        static string GenerateCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static async Task<bool> CheckCodeAsync(string code)
        {
            string url = $"https://discordapp.com/api/v9/entitlements/gift-codes/{code}?with_application=false&with_subscription_plan=true";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    return response.IsSuccessStatusCode;
                }
                catch (HttpRequestException)
                {
                    return false;
                }
            }
        }

        static async Task Main(string[] args)
        {
            Console.Title = "Meme Gen and Check";
            Console.WriteLine(@"                           ███╗   ██╗██╗████████╗██████╗  ██████╗  ██████╗ ███╗   ██╗ ██████╗
                           ████╗  ██║██║╚══██╔══╝██╔══██╗██╔═══██╗██╔════╝ ████╗  ██║██╔════╝
                           ██╔██╗ ██║██║   ██║   ██████╔╝██║   ██║██║  ███╗██╔██╗ ██║██║     
                           ██║╚██╗██║██║   ██║   ██╔══██╗██║   ██║██║   ██║██║╚██╗██║██║     
                           ██║ ╚████║██║   ██║   ██║  ██║╚██████╔╝╚██████╔╝██║ ╚████║╚██████╗
                           ╚═╝  ╚═══╝╚═╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝
                                                                  ", Color.Purple);
            Console.WriteLine("                          \t\tNitro checker made by Kryptonite.", Color.HotPink);
            Console.WriteLine();
            Console.Write("Amount to gen and check: ", Color.GreenYellow);
            int numCodes = int.Parse(Console.ReadLine());
            int numThreads;
            do
            {
                Console.Write("Enter the number of threads to use (50-250): ", Color.GreenYellow);
            } while (!int.TryParse(Console.ReadLine(), out numThreads) || numThreads < 50 || numThreads > 250);

            if (args.Length > 0 && int.TryParse(args[0], out int argNumCodes))
            {
                numCodes = argNumCodes;
            }

            List<string> validCodes = new List<string>();

            Console.WriteLine($"Generating and checking {numCodes} Nitro codes...", Color.DarkGreen);
            Console.WriteLine();

            for (int i = 0; i < numCodes; i++)
            {
                string code = GenerateCode(16);
                bool isValid = await CheckCodeAsync(code);

                Console.Write($"https://discord.gift/{code} - ", Color.DarkCyan);
                if (isValid)
                {

                    Console.WriteLine("Valid", Color.Green);
                    validCodes.Add(code);
                }
                else
                {

                    Console.WriteLine("Invalid", Color.Red);
                }
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine($"Found {validCodes.Count} valid Nitro codes.", Color.BlueViolet);

            if (validCodes.Count > 0)
            {
                Console.WriteLine("Saving valid codes to file...");
                File.WriteAllLines(VALID_CODES_FILE, validCodes);
                Console.WriteLine($"Valid codes saved to {VALID_CODES_FILE}.");
            }

            Console.WriteLine("Press any key to exit...", Color.CadetBlue);
            Console.ReadKey();
        }
    }
}
