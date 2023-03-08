using ProteinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProteinApi.Services;

public class MixedBatchService
{
    private readonly IMongoCollection<MixedBatch> _mixedbatchCollection;

    public MixedBatchService(
        IOptions<ProteinIDatabaseSettings> proteinIDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            proteinIDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            proteinIDatabaseSettings.Value.DatabaseName);

        _mixedbatchCollection = mongoDatabase.GetCollection<MixedBatch>(
            proteinIDatabaseSettings.Value.BatchCollectionName);
    }

    public async Task<List<MixedBatch>> GetAsync() =>
        await _mixedbatchCollection.Find(_ => true).ToListAsync();

    public async Task<MixedBatch?> GetAsync(string id) =>
        await _mixedbatchCollection.Find(x => x.MixedBatchId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(MixedBatch newBatch) =>
        await _mixedbatchCollection.InsertOneAsync(newBatch);

    public async Task UpdateAsync(string id, MixedBatch updatedBatch) =>
        await _mixedbatchCollection.ReplaceOneAsync(x => x.MixedBatchId == id, updatedBatch);

    public async Task RemoveAsync(string id) =>
        await _mixedbatchCollection.DeleteOneAsync(x => x.MixedBatchId == id);
}



