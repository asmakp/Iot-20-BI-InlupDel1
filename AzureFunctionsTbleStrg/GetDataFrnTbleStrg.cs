using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionsTbleStrg.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionsTbleStrg
{
    public static class GetDataFrnTbleStrg
    {
        [FunctionName("GetDataFrnTbleStrg")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
             [Table("sensor")] CloudTable cloudTable,
            ILogger log)
        {
           // string limit = req.Query["limit"];
           // string orderby = req.Query["orderby"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
           // dynamic data = JsonConvert.DeserializeObject(requestBody);

            IEnumerable<MessageTable> results = await cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<MessageTable>(), null);
            //results = results.OrderBy(ts => ts.Timestamp);
            results = results.OrderByDescending(ts => ts.Timestamp).Take<MessageTable>(10);

            //if (orderby == "desc")
            //    results = results.OrderByDescending(ts => ts.Timestamp);

            //if (limit != null)
            //    results = results.Take(int.Parse(limit));

            return new OkObjectResult(results);
        }
    }
}

