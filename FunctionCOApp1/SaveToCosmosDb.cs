using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionCOApp1.Model;
using System;

namespace FunctionCOApp1
{
    public static class SaveToCosmosDb
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToCosmosDb")]
        public static void Run(
            [IoTHubTrigger("messages/events", Connection = "IotHubConnection",ConsumerGroup ="cosmosdb")]EventData message,
            [CosmosDB(
            databaseName:"IOT20", 
            collectionName: "Messages1", 
            ConnectionStringSetting = "CosmosDbConnection",
            CreateIfNotExists =true
            )]out dynamic cosmos,
            ILogger log)

        { 
                try
                {
                    var _data = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(message.Body.Array));
                    var _deviceId = message.SystemProperties["iothub-connection-device-id"].ToString(); //docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-construct
                    var _schoolName = message.Properties["SchoolName"].ToString();          
                    var _studentName = message.Properties["StudentName"].ToString();
                

                var _cosmos = new CosmosData()
                {
                    deviceId = _deviceId,
                    schoolName = _schoolName,
                    studentName = _studentName,
                    data = _data
                    };
                   

                   
                    var _cosmosjson = JsonConvert.SerializeObject(_cosmos);
                    log.LogInformation($"Measurement was saved to Cosmos DB, Message:: {_cosmosjson}");
                    cosmos = _cosmosjson;

                   
                }
                catch (Exception e)
                {
                    log.LogInformation($"Unable to process Request, Error:: {e.Message}");
                    cosmos = null;
                }

        }
           
        
    }
}