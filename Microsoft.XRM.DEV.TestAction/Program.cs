using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.XRM.DEV.TestAction
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private HttpResponseMessage APICall(string requestURI)
        {
            //Get configuration data from App.config connectionStrings
            string connectionString = ConfigurationManager.ConnectionStrings["Connect"].ConnectionString;

            using (HttpClient client = HTTPHelper.GetHttpClient(connectionString, HTTPHelper.clientId, HTTPHelper.redirectUrl))
            {
                var response = client.GetAsync(requestURI).Result;
                return response;
            }
        }

        public void SampleAPIACtion()
        {
            // Use the WhoAmI function
            var response = APICall("WhoAmI");
            Guid userId = Guid.Empty;

            if (response.IsSuccessStatusCode)
            {
                //Get the response content and parse it.  
                JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                userId = (Guid)body["UserId"];
                Console.WriteLine("Your UserId is {0}", userId);
            }


        }

        public void SampleAPIFunction()
        {
            // Use the WhoAmI function
            var response = APICall("WhoAmI");
            Guid userId = Guid.Empty;

            if (response.IsSuccessStatusCode)
            {
                //Get the response content and parse it.  
                JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                userId = (Guid)body["UserId"];
                Console.WriteLine("Your UserId is {0}", userId);
            }


        }

 
        public void GetContactWebAPI()
        {
            string queryOptions;
            string[] contactProperties = { "fullname", "jobtitle", "annualincome" };
            string filter = @"&$filter=contains(fullname,'(sample)')";
            queryOptions = "?$select=" + String.Join(",", contactProperties) + filter;
            string contactSearch = "contacts?$select=fullname,jobtitle&$filter=contains(fullname,'Thorell')";
            var response = APICall(contactSearch);
            if (response.IsSuccessStatusCode)
            {
                JObject collection = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            }
        }


        private static void DisplayFormattedEntities(string label, JArray entities, string[] properties)
        {
            Console.Write(label);
            int lineNum = 0;
            foreach (JObject entity in entities)
            {
                lineNum++;
                List<string> propsOutput = new List<string>();
                //Iterate through each requested property and output either formatted value if one 
                //exists, otherwise output plain value.
                foreach (string prop in properties)
                {
                    string propValue;
                    string formattedProp = prop + "@OData.Community.Display.V1.FormattedValue";
                    if (null != entity[formattedProp])
                    { propValue = entity[formattedProp].ToString(); }
                    else
                    { propValue = entity[prop].ToString(); }
                    propsOutput.Add(propValue);
                }
                Console.Write("\n\t{0}) {1}", lineNum, String.Join(", ", propsOutput));
            }
            Console.Write("\n");
        }
        ///<summary>Overloaded helper version of method that unpacks 'collection' parameter.</summary>
        private static void DisplayFormattedEntities(string label, JObject collection, string[] properties)
        {
            JToken valArray;
            //Parameter collection contains an array of entities in 'value' member.
            if (collection.TryGetValue("value", out valArray))
            {
                DisplayFormattedEntities(label, (JArray)valArray, properties);
            }
            //Otherwise it just represents a single entity.
            else
            {
                JArray singleton = new JArray(collection);
                DisplayFormattedEntities(label, singleton, properties);
            }
        }

    }
}
