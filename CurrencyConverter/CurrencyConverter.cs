namespace CurrencyConverter
{
    using System;
    using System.Text;

    /// <summary>
    /// Converts currency value into its' text representation in english.
    /// </summary>
    public sealed class CurrencyConverter
    {
        private const string DollarSingularText = "dollar";
        private const string DollarPluralText = "dollars";
        private const string CentSingularText = "cent";
        private const string CentPluralText = "cents";
        private const string ConjugationText = " and ";

        private static readonly string[] First20Texts = new string[]
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
        };

        private static readonly string[] DecimalTexts = new string[]
        {
            string.Empty, string.Empty, "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety",
        };

        private static readonly string[] HigherTexts = new string[]
        {
            string.Empty, " thousand", " million",
        };

        /// <summary>
        /// Converts currency value into its' text representation in english.
        /// </summary>
        /// <param name="value">Currency value.</param>
        /// <returns>Text representation of the input in english.</returns>
        public string ToText(decimal value)
        {
            uint dollars = (uint)value;
            decimal cents = (value % 1.0m) * 100;
            return this.ToText(dollars, (byte)cents);
        }

        /// <summary>
        /// Converts currency value into its' text representation in english.
        /// </summary>
        /// <param name="dollars">Dollars value.</param>
        /// <param name="cents">Cents value.</param>
        /// <returns>Text representation of the input in english.</returns>
        public string ToText(uint dollars, byte cents = 0)
        {
            var stringBuilder = new StringBuilder();
            ConvertToText(dollars, stringBuilder);

            if (dollars == 1)
            {
                stringBuilder.Append(DollarSingularText);
            }
            else
            {
                stringBuilder.Append(DollarPluralText);
            }

            if (cents > 0)
            {
                stringBuilder.Append(ConjugationText);
                ConvertToTextUpToTwoDigits(cents, stringBuilder);
                stringBuilder.Append(" ");

                if (cents == 1)
                {
                    stringBuilder.Append(CentSingularText);
                }
                else
                {
                    stringBuilder.Append(CentPluralText);
                }
            }

            return stringBuilder.ToString();
        }

        private static void ConvertToText(uint number, StringBuilder stringBuilder, int iteration = 0)
        {
            // Recursively convert higher groups first
            var numberNext = number / 1000;
            if (numberNext > 0)
            {
                ConvertToText(numberNext, stringBuilder, iteration + 1);
            }

            // Write current group
            var residual = number % 1000;
            ConvertToTextUpToThreeDigits(residual, stringBuilder);
            stringBuilder.Append(HigherTexts[iteration]);
            stringBuilder.Append(" ");
        }

        private static void ConvertToTextUpToThreeDigits(uint number, StringBuilder stringBuilder)
        {
            if (number >= 1000)
            {
                // Guard - this should never happen
                throw new ArgumentOutOfRangeException(
                    nameof(number),
                    number,
                    $"{nameof(ConvertToTextUpToThreeDigits)} can handle only arguments in [0; 1000) range.");
            }

            var hundreds = number / 100;
            var residual = (byte)(number % 100);
            if (hundreds > 0)
            {
                stringBuilder.Append(First20Texts[hundreds])
                             .Append(" hundred");

                if (residual > 0)
                {
                    stringBuilder.Append(" ");
                    ConvertToTextUpToTwoDigits(residual, stringBuilder);
                }
            }
            else
            {
                ConvertToTextUpToTwoDigits(residual, stringBuilder);
            }
        }

        private static void ConvertToTextUpToTwoDigits(byte number, StringBuilder stringBuilder)
        {
            if (number < First20Texts.Length)
            {
                stringBuilder.Append(First20Texts[number]);
            }
            else if (number < 100)
            {
                stringBuilder.Append(DecimalTexts[number / 10]);

                var residual = number % 10;
                if (residual > 0)
                {
                    stringBuilder.Append("-")
                                 .Append(First20Texts[residual]);
                }
            }
            else
            {
                // Guard - this should never happen
                throw new ArgumentOutOfRangeException(
                    nameof(number),
                    number,
                    $"{nameof(ConvertToTextUpToTwoDigits)} can handle only arguments in [0; 100) range.");
            }
        }
    }
}
