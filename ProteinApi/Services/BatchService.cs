using ProteinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProteinApi.Services;

public class BatchService
{
    private readonly IMongoCollection<Batch> _batchCollection;

    public BatchService(
        IOptions<ProteinIDatabaseSettings> proteinIDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            proteinIDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            proteinIDatabaseSettings.Value.DatabaseName);

        _batchCollection = mongoDatabase.GetCollection<Batch>(
            proteinIDatabaseSettings.Value.BatchCollectionName);
    }

    public async Task<List<Batch>> GetAsync() =>
        await _batchCollection.Find(_ => true).ToListAsync();

    public async Task<Batch?> GetAsync(string id) =>
        await _batchCollection.Find(x => x.BatchId == id).FirstOrDefaultAsync();

    public async Task<List<Batch>> GetByBusinessAsync(string businessId) =>
        await _batchCollection.Find(x => x.HolderId == businessId).ToListAsync();

    public async Task CreateAsync(Batch newBatch) =>
        await _batchCollection.InsertOneAsync(newBatch);

    public async Task UpdateAsync(string id, Batch updatedBatch) =>
        await _batchCollection.ReplaceOneAsync(x => x.BatchId == id, updatedBatch);

    public async Task RemoveAsync(string id) =>
        await _batchCollection.DeleteOneAsync(x => x.BatchId == id);
}



