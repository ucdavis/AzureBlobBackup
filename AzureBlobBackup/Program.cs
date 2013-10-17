using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

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

            //Get the storage account passed in, otherwise use the one from the settings file
            var storageAccount =
                CloudStorageAccount.Parse(args.Length > 0
                                              ? args[0]
                                              : CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();

            //Get the container to use if passed in as the second arg, else use the one from the settings file
            var container =
                blobClient.GetContainerReference(args.Length > 1
                                                     ? args[1]
                                                     : CloudConfigurationManager.GetSetting("FileContainer"));
            
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var fileSystemInfos = currentDirectory.GetFileSystemInfos().ToArray();

            //If we don't have any files, just set that we haven't had any modifications lately, else set to the latest creation time
            var lastFileModified = fileSystemInfos.Any()
                                       ? fileSystemInfos.Max(x => x.CreationTimeUtc)
                                       : DateTime.MinValue;
            
            Console.WriteLine("Last modified for the directory {0} is {1}", currentDirectory.Name, lastFileModified);

            Task.WaitAll(
                (from blob in container.ListBlobs().OfType<CloudBlockBlob>()
                 where blob.Properties.LastModified > lastFileModified
                 select blob.DownloadToFileAsync(blob.Name, FileMode.Create))
                    .ToArray());

            Console.WriteLine("Finished downloading all files");
        }
    }
}
