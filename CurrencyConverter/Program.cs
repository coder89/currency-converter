namespace CurrencyConverter
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Console app entrypoint.
    /// </summary>
    internal sealed class Program
    {
        private static async Task Main()
        {
            InputManager inputManager = new InputManager();
            CurrencyConverter converter = new CurrencyConverter();

            do
            {
                Console.Write("Provide number (press ESC to exit): ");

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (sender, args) =>
                {
                    cts.Cancel();
                };

                try
                {
                    decimal number = await inputManager.TryReadNumberAsync(cts.Token);
                    string output = converter.ToText(number);
                    Console.WriteLine(output);
                }
                catch (OperationCanceledException)
                {
                    Environment.Exit(0); // Ensures the app exits when Ctrl+C is pressed with debugger attached.
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (true);
        }
    }
}
