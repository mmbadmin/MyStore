namespace MyStore.Infrastructure.Tests.Helper
{
    using FluentValidation;
    using System.Globalization;

    public class BaseTest
    {
        public BaseTest()
        {
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("en-CA");
        }
    }
}
