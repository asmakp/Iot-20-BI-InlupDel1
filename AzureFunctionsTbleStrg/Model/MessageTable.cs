using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsTbleStrg.Model
{
    public class MessageTable : TableEntity
    {
       // public string PartitionKey {get;set;}
       // public string  RowKey { get; set; }
        public double Distance { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DeviceId { get; set; }
        public DateTime epochTime { get; set; }
        public string SchoolName { get; set; }
        public string StudentName { get; set; }


    }
}
