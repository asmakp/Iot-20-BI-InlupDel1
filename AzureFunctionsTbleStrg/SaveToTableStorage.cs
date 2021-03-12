using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using AzureFunctionsTbleStrg.Model;
using Newtonsoft.Json;
using System;

namespace AzureFunctionsTbleStrg
{
    public static class SaveToTableStorage
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToTableStorage")]
        [return: Table("sensor")]
        public static MessageTable Run([IoTHubTrigger("messages/events", Connection = "IotHubConnection")]EventData message, ILogger log)
        {
            try
            {
                var payload = JsonConvert.DeserializeObject<MessageTable>(Encoding.UTF8.GetString(message.Body.Array));
               // payload.PartitionKey = message.Properties["type"].ToString(); 
               // payload.RowKey = Guid.NewGuid().ToString();
                var _deviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
               // var _deviceType = message.Properties["type"].ToString();
                var _schoolName = message.Properties["SchoolName"].ToString();
                var _studentName = message.Properties["StudentName"].ToString();

                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(message.Properties["epochTime"].ToString()));
                DateTime _measurementTime = dateTimeOffset.DateTime;

                var _table = new MessageTable()
                {
                    PartitionKey = message.Properties["type"].ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    DeviceId = _deviceId,
                    epochTime = _measurementTime,
                    SchoolName = _schoolName,
                    StudentName = _studentName,
                    Distance = payload.Distance,
                    Latitude = payload.Latitude,
                    Longitude = payload.Longitude,
                };
                var _tablejson = JsonConvert.SerializeObject(_table);
                log.LogInformation($"Measurement was saved to Table Storage, Message:: {_tablejson}");

                return _table;

               }
            catch
            {
                log.LogInformation("Failed to Deserialize message. No data was save to Table Storage");
            }
            return null;
        }
    }
}