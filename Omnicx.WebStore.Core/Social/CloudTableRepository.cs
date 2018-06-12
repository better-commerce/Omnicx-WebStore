using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Omnicx.WebStore.Core.Data
{
    public class CloudTableRepository
    {
        public CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }

            return storageAccount;
        }

        public CloudTable CreateTable(string tableName)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            try
            {
                table.CreateIfNotExistsAsync();
                //if (await table.CreateIfNotExistsAsync())
                //{
                //    Console.WriteLine("Created Table named: {0}", tableName);
                //}
                //else
                //{
                //    Console.WriteLine("Table {0} already exists", tableName);
                //}
            }
            catch (StorageException)
            {
                throw;
            }

            return table;
        }

        public void InsertUser(CloudTable table, dynamic user)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            TableOperation to = TableOperation.Insert(user);

            try { table.Execute(to); }
            catch (StorageException) { }
        }

    }
}