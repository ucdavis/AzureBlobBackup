using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobBackup
{
    class Program
    {
        /// <summary>
        /// Grab all files from the azure blob and save them to a the executing directory on disk
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Getting Azure Storage Configuration");

            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(CloudConfigurationManager.GetSetting("FileContainer"));
            
            
            
        }
    }
}
