namespace CurrencyConverter
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Manages console input.
    /// </summary>
    internal sealed class InputManager
    {
        private const decimal MinInput = 0.0m;
        private const decimal MaxInput = 999999999.99m;

        private readonly NumberFormatInfo inputFormat = new NumberFormatInfo()
        {
            CurrencyDecimalSeparator = ",",
            NumberDecimalSeparator = ",",
            CurrencyGroupSeparator = " ",
            NumberGroupSeparator = " ",
        };

        /// <summary>
        /// Tries to read a number from the console input.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Boolean flag indicating whether input has been read successfully.</returns>
        public async Task<decimal> TryReadNumberAsync(CancellationToken token)
        {
            StringBuilder stringBuilder = new StringBuilder();

            do
            {
                // Read keys asynchronously so that we can cancel it externaly
                while (Console.KeyAvailable == false)
                {
                    await Task.Delay(250, token)
                        .ConfigureAwait(false);
                }

                ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    // Check if input empty
                    if (stringBuilder.Length > 0)
                    {
                        Console.WriteLine();
                        break;
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    throw new OperationCanceledException();
                }
                else if (key.KeyChar != '\0')
                {
                    stringBuilder.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
            while (true);

            token.ThrowIfCancellationRequested();

            // Parse and validate input
            string input = stringBuilder.ToString();
            if (!decimal.TryParse(input, NumberStyles.Any, this.inputFormat, out decimal number)
                || number < MinInput
                || number > MaxInput)
            {
                throw new Exception("Input our of range.");
            }

            return number;
        }
    }
}
