// See https://aka.ms/new-console-template for more information
using CosmoDBConsole;
using Microsoft.Azure.Cosmos;
using System.Net;

Console.WriteLine("Hello, World!");

// ADD THIS PART TO YOUR CODE

// The Azure Cosmos DB endpoint for running this sample.
string EndpointUri = "https://cosmoforstudents.documents.azure.com:443/";
// The primary key for the Azure Cosmos account.
string PrimaryKey = "Keq3AbOx2x3Ak1PFPTkhb9rR4KDiIBxIRBmSN9mlfmJ8kEPhCxh58Va54k88uyranarMo0gZFkO4UC3RyCSPCg==";

// The Cosmos client instance
CosmosClient cosmosClient;





// The name of the database and container we will create
string databaseId = "FamilyDatabase";
string containerId = "FamilyContainer";

await DoStuff();

async Task DoStuff()
{
    try
    {
        Console.WriteLine("Beginning operations...\n");
        await GetStartedDemoAsync();

    }
    catch (CosmosException de)
    {
        Exception baseException = de.GetBaseException();
        Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
    }
    catch (Exception e)
    {
        Console.WriteLine("Error: {0}", e);
    }
    finally
    {
        Console.WriteLine("End of demo, press any key to exit.");
        Console.ReadKey();
    }
}


/*
    Entry point to call methods that operate on Azure Cosmos DB resources in this sample
*/

async Task GetStartedDemoAsync()
{

    // The database we will create
    Database database;
    // The container we will create.
    Container container;
    // Create a new instance of the Cosmos Client
    cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

    database = await CreateDatabaseAsync();

    container = await CreateContainerAsync(database);

    await AddItemsToContainerAsync(container);

    await QueryItemsAsync(container);

    await ReplaceFamilyItemAsync(container);

    await DeleteFamilyItemAsync(container);
}



/// <summary>
/// Create the database if it does not exist
/// </summary>
async Task<Database> CreateDatabaseAsync()
{
    // Create a new database
    Database database;
    database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
    Console.WriteLine("Created Database: {0}\n", database.Id);
    return database;
}

/// <summary>
/// Create the container if it does not exist. 
/// Specifiy "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
/// </summary>
/// <returns></returns>
async Task<Container> CreateContainerAsync(Database database)
{
    // Create a new container
    // The container we will create.
    Container container;
    container = await database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
    Console.WriteLine("Created Container: {0}\n", container.Id);
    return container;
}

/// <summary>
/// Add Family items to the container
/// </summary>
async Task AddItemsToContainerAsync(Container container)
{
    // Create a family object for the Andersen family
    Family andersenFamily = new Family
    {
        Id = "Andersen.1",
        LastName = "Andersen",
        Parents = new Parent[]
        {
            new Parent { FirstName = "Thomas" },
            new Parent { FirstName = "Mary Kay" }
        },
        Children = new Child[]
        {
            new Child
            {
                FirstName = "Henriette Thaulow",
                Gender = "female",
                Grade = 5,
                Pets = new Pet[]
                {
                    new Pet { GivenName = "Fluffy" }
                }
            }
        },
        Address = new Address { State = "WA", County = "King", City = "Seattle" },
        IsRegistered = false
    };

    try
    {
        // Read the item to see if it exists.  
        ItemResponse<Family> andersenFamilyResponse = await container.ReadItemAsync<Family>(andersenFamily.Id, new PartitionKey(andersenFamily.LastName));
        Console.WriteLine("Item in database with id: {0} already exists\n", andersenFamilyResponse.Resource.Id);
    }
    catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
        ItemResponse<Family> andersenFamilyResponse = await container.CreateItemAsync<Family>(andersenFamily, new PartitionKey(andersenFamily.LastName));

        // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
        Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", andersenFamilyResponse.Resource.Id, andersenFamilyResponse.RequestCharge);
    }

    // Create a family object for the Wakefield family
    Family wakefieldFamily = new Family
    {
        Id = "Wakefield.7",
        LastName = "Wakefield",
        Parents = new Parent[]
        {
            new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
            new Parent { FamilyName = "Miller", FirstName = "Ben" }
        },
        Children = new Child[]
        {
            new Child
            {
                FamilyName = "Merriam",
                FirstName = "Jesse",
                Gender = "female",
                Grade = 8,
                Pets = new Pet[]
                {
                    new Pet { GivenName = "Goofy" },
                    new Pet { GivenName = "Shadow" }
                }
            },
            new Child
            {
                FamilyName = "Miller",
                FirstName = "Lisa",
                Gender = "female",
                Grade = 1
            }
        },
        Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
        IsRegistered = true
    };

    try
    {
        // Read the item to see if it exists
        ItemResponse<Family> wakefieldFamilyResponse = await container.ReadItemAsync<Family>(wakefieldFamily.Id, new PartitionKey(wakefieldFamily.LastName));
        Console.WriteLine("Item in database with id: {0} already exists\n", wakefieldFamilyResponse.Resource.Id);
    }
    catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        // Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
        ItemResponse<Family> wakefieldFamilyResponse = await container.CreateItemAsync<Family>(wakefieldFamily, new PartitionKey(wakefieldFamily.LastName));

        // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
        Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", wakefieldFamilyResponse.Resource.Id, wakefieldFamilyResponse.RequestCharge);
    }
}

/// <summary>
/// Run a query (using Azure Cosmos DB SQL syntax) against the container
/// </summary>
async Task QueryItemsAsync(Container container)
{
    var sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Andersen'";

    Console.WriteLine("Running query: {0}\n", sqlQueryText);

    QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
    FeedIterator<Family> queryResultSetIterator = container.GetItemQueryIterator<Family>(queryDefinition);

    List<Family> families = new List<Family>();

    while (queryResultSetIterator.HasMoreResults)
    {
        FeedResponse<Family> currentResultSet = await queryResultSetIterator.ReadNextAsync();
        foreach (Family family in currentResultSet)
        {
            families.Add(family);
            Console.WriteLine("\tRead {0}\n", family);
        }
    }
}

/// <summary>
/// Replace an item in the container
/// </summary>
async Task ReplaceFamilyItemAsync(Container container)
{
    ItemResponse<Family> wakefieldFamilyResponse = await container.ReadItemAsync<Family>("Wakefield.7", new PartitionKey("Wakefield"));
    var itemBody = wakefieldFamilyResponse.Resource;

    // update registration status from false to true
    itemBody.IsRegistered = true;
    // update grade of child
    itemBody.Children[0].Grade = 6;

    // replace the item with the updated content
    wakefieldFamilyResponse = await container.ReplaceItemAsync<Family>(itemBody, itemBody.Id, new PartitionKey(itemBody.LastName));
    Console.WriteLine("Updated Family [{0},{1}].\n \tBody is now: {2}\n", itemBody.LastName, itemBody.Id, wakefieldFamilyResponse.Resource);
}

/// <summary>
/// Delete an item in the container
/// </summary>
async Task DeleteFamilyItemAsync(Container container)
{
    var partitionKeyValue = "Wakefield";
    var familyId = "Wakefield.7";

    // Delete an item. Note we must provide the partition key value and id of the item to delete
    ItemResponse<Family> wakefieldFamilyResponse = await container.DeleteItemAsync<Family>(familyId, new PartitionKey(partitionKeyValue));
    Console.WriteLine("Deleted Family [{0},{1}]\n", partitionKeyValue, familyId);
}
