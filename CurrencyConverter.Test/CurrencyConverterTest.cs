using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurrencyConverter.Test
{
    [TestClass]
    public class CurrencyConverterTest
    {
        private readonly CurrencyConverter converter = new CurrencyConverter();

        public TestContext TestContext { get; set; }

        [TestMethod]
        [DeploymentItem("TestData.xml")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.XML",
            @"|DataDirectory|\\TestData.xml",
            "test",
            DataAccessMethod.Sequential)]
        public void ConversionTest()
        {
            decimal value = Convert.ToDecimal(TestContext.DataRow["value"]);
            string expectedOutput = Convert.ToString(TestContext.DataRow["expectedOutput"]);
            string actualOutput = converter.ToText(value);
            Assert.AreEqual(expectedOutput, actualOutput);
        }
    }
}
