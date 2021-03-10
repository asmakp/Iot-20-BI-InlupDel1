using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using FunctionCOApp1.Model;

// Delete the old database from cosmosDB on azure every time you run the program.
namespace FunctionCOApp1
{
    public static class GetDataFromCosmosDb
    {
        [FunctionName("GetDataFromCosmosDb")]
        //IActionResult
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = null)] HttpRequest req,
            [CosmosDB(
            databaseName:"IOT20",
            collectionName: "Messages1",
            ConnectionStringSetting = "CosmosDbConnection",
            SqlQuery = "SELECT TOP 10 * FROM c ORDER BY c._ts DESC"
             
           )]IEnumerable<dynamic> cosmos,
            ILogger log)
            //IEnumerable<CosmosData> cosmos, ILogger log
        //IEnumerable -to get the data in the form of a list
        {
            log.LogInformation("HTTP trigger function executed.");

            

            return new OkObjectResult(cosmos);
        }
    }
}
