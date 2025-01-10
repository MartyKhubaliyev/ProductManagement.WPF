using MongoDB.Bson;
using MongoDB.Driver;
using ProductManagement.WPF.Helpers;

namespace ProductManagement.WPF.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<BsonDocument> _categoryEventsCollection;
        private readonly IMongoCollection<BsonDocument> _productEventsCollection;

        public MongoService(string mongoConnectionString, string databaseName)
        {
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(databaseName);
            _categoryEventsCollection = database.GetCollection<BsonDocument>("CategoryEvents");
            _productEventsCollection = database.GetCollection<BsonDocument>("ProductEvents");
        }

        public async Task AddCategoryEventAsync(string topic, string message)
        {
            try
            {
                var eventDocument = new BsonDocument
                {
                    { "Topic", topic },
                    { "Message", message },
                    { "Timestamp", DateTime.UtcNow }
                };

                await _categoryEventsCollection.InsertOneAsync(eventDocument);
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error saving event to MongoDB: {ex.Message}");
            }
        }

        public async Task AddProductEventAsync(string topic, string message)
        {
            try
            {
                var eventDocument = new BsonDocument
                {
                    { "Topic", topic },
                    { "Message", message },
                    { "Timestamp", DateTime.UtcNow }
                };

                await _productEventsCollection.InsertOneAsync(eventDocument);
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error saving event to MongoDB: {ex.Message}");
            }
        }

        public async Task<List<BsonDocument>> GetAllCategoryEventsAsync()
        {
            try
            {
                return await _categoryEventsCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error retrieving events from MongoDB: {ex.Message}");
                return new List<BsonDocument>();
            }
        }

        public async Task<List<BsonDocument>> GetAllProductEventsAsync()
        {
            try
            {
                return await _productEventsCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error retrieving events from MongoDB: {ex.Message}");
                return new List<BsonDocument>();
            }
        }

        public async Task ClearEventsAsync()
        {
            try
            {
                await _categoryEventsCollection.DeleteManyAsync(new BsonDocument());
                await _productEventsCollection.DeleteManyAsync(new BsonDocument());
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error clearing events from MongoDB: {ex.Message}");
            }
        }
    }
}
