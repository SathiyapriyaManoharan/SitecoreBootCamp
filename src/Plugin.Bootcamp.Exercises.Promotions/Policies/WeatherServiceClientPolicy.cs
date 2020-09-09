using Sitecore.Commerce.Core;


namespace Plugin.Bootcamp.Exercises.Promotions
{
    class WeatherServiceClientPolicy : Policy
    {
        /* Property to store the API key.
         * Constructor to initialize it to an empty string. */

        public WeatherServiceClientPolicy()
        {
            this.ApplicationId = string.Empty;
        }

        public string ApplicationId { get; set; }
    }
}
