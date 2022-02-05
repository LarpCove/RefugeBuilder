using System;

#pragma warning disable CA1303 // Do not pass literals as localized parameters

namespace RefugeBuilder.Terminal
{
    /// <summary>Handles running chess on the console.</summary>
    public static class Program
    {
        /// <summary>Application entry point.</summary>
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Hello World!");

                string input = Console.ReadLine() ?? "";
                switch (input)
                {
                    default:
                        Console.WriteLine(input);
                        break;
                    case "quit":
                    case "q":
                        return;
                }
            }
        }
    }
}

#pragma warning restore CA1303 // Do not pass literals as localized parameters
