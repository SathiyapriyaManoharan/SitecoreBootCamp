using System;

using Sitecore.Commerce.Core;
using Sitecore.Framework.Rules;
using Sitecore.Commerce.Plugin.Carts;

namespace Plugin.Bootcamp.Exercises.Promotions
{
    [EntityIdentifier("MinTemperatureCondition")]
    public class MinTemperatureCondition : ICartsCondition
    {
        /* IRuleValue properties for the city, country, and
         * minimum temperature.
         */
        public IRuleValue<Decimal> MinimumTemperature { get; set; }
        public IRuleValue<String> City { get; set; }
        public IRuleValue<String> Country { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {

            var minimumTemperature = MinimumTemperature.Yield(context);
            var city = City.Yield(context);
            var country = Country.Yield(context);

            /* Complete the Evaluate method to:
            * Retrieve the current temperature
            * Compare it to the temperature stored in the Policy
            * Return the result of that comparison
            */
            CommerceContext commerceContext = context.Fact<CommerceContext>((string)null);
            var weatherServicePolicy = commerceContext.GetPolicy<WeatherServiceClientPolicy>();

            var currentTemperature = GetCurrentTemperature(city, country, weatherServicePolicy.ApplicationId);

            if (currentTemperature > minimumTemperature)
                return true;
            else
                return false;
        }

        /*Retrieve the current temperature from Weather Service*/

        public decimal GetCurrentTemperature(string city, string country, string applicationId)
        {
            WeatherService weatherService = new WeatherService(applicationId);
            var temperature = weatherService.GetCurrentTemperature(city, country).Result;

            return (decimal)temperature.Max;
        }
    }
}
