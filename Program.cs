using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using Example1.Models;
using Newtonsoft.Json;

namespace Example1
{
    class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private static readonly string AUTHORING_KEY = ConfigurationManager.AppSettings["MicrosoftLuisAuthoringKey"];
        private static readonly string APPLICATION_ID = ConfigurationManager.AppSettings["MicrosoftLuisAppId"];
        private static readonly string VERSION = "0.1";
        private const string HOST = "https://westus.api.cognitive.microsoft.com/";

        private const double INTENT_THRESHOLD = 0.2;

        static void Main()
        {
            //Programmatic API Example
            //Get a list of all Intents in the app
            var getIntentsPath = $"luis/api/v2.0/apps/{APPLICATION_ID}/versions/{VERSION}/intents";
            var intents = MakeRequest<List<Intent>>(getIntentsPath, string.Empty);

            //Prediction API Example
            //Send an Utterance to determine the Intent and Entities
            var sendQueryPath = $"luis/v2.0/apps/{APPLICATION_ID}?subscription-key={AUTHORING_KEY}&q=";

            var query = Console.ReadLine();
            var queryResponse = MakeRequest<QueryResponse>(sendQueryPath, query);
            ProcessResponse(queryResponse);

            Console.ReadLine();
        }

        public static T MakeRequest<T>(string path, string query)
        {
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri($"{HOST}{path}{query}");
                request.Headers.Add("Ocp-Apim-Subscription-Key", AUTHORING_KEY);
                var response = _httpClient.SendAsync(request).Result;

                var responseText = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseText);

                return JsonConvert.DeserializeObject<T>(responseText);
            }
        }

        public static void ProcessResponse(QueryResponse res)
        {
            var intentName = res.topScoringIntent.Intent;

            //Not enough confidence in the Intent. Do nothing
            if (res.topScoringIntent.score < INTENT_THRESHOLD)
            {
                Console.WriteLine("Unable to determine the intent. Please try again");
                return;
            }

            switch (intentName)
            {
                case "Calendar.Add":
                    Console.WriteLine("Adding an entry into the calendar");
                    //...
                    break;
                case "Calendar.Edit":
                    Console.WriteLine("Editing the calendar entry");
                    //...
                    break;
                case "Calendar.Delete":
                    Console.WriteLine("Removing the calendar entry");
                    //...
                    break;
                default:
                    Console.WriteLine("Intent not recognized. Please try again.");
                    break;
            }
        }
    }
}
