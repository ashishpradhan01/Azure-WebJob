using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace ThumbnailWebjob
{
    class Program
    {
        static async Task Main()
        {
            string AZURE_STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=storashish;AccountKey=yXhTEab/9aCu7N+wsYrrqXrVV2Iy1KWP/pkD/slFBevWGbqsSxusN2IfX9RkCrP4vBxMITSTMF5r+AStVpGb1w==;EndpointSuffix=core.windows.net";
            string desContainerName = "copywebjob";
            string srcContainerName = "imagebox";

            BlobServiceClient blobServiceClient = new BlobServiceClient(AZURE_STORAGE_CONNECTION_STRING);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(srcContainerName);
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine("Copying \t" + blobItem.Name);
               await copyBlobToAnotherContainer(AZURE_STORAGE_CONNECTION_STRING, srcContainerName, blobItem.Name, desContainerName, blobItem.Name);

            }
        }

        static async Task copyBlobToAnotherContainer(string connectionString, string sourceContainerName, string sourceFilePath
            , string destContainerName, string destFilePath)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var sourceContainer = blobClient.GetContainerReference(sourceContainerName);
            var destContainer = blobClient.GetContainerReference(destContainerName);

            CloudBlockBlob sourceBlob = sourceContainer.GetBlockBlobReference(sourceFilePath);
            CloudBlockBlob destBlob = destContainer.GetBlockBlobReference(destFilePath);
            await destBlob.StartCopyAsync(sourceBlob);
        }
    }
}
