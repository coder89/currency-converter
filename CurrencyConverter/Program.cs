namespace CurrencyConverter
{
    using System;

    /// <summary>
    /// Console app entrypoint.
    /// </summary>
    internal sealed class Program
    {
        private static void Main()
        {
            CurrencyConverter converter = new CurrencyConverter();

            do
            {
                Console.Write("Provide number (press ENTER to exit): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                if (!decimal.TryParse(input, out decimal number) || number < 0.0m || number > 999999999.99m)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }

                string output = converter.ToText(number);
                Console.WriteLine(output);
            }
            while (true);
        }
    }
}
