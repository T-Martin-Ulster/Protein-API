using ProteinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProteinApi.Services;

public class DataService
{
    private readonly IMongoCollection<Data> _dataCollection;

    public DataService(
        IOptions<ProteinIDatabaseSettings> proteinIDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            proteinIDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            proteinIDatabaseSettings.Value.DatabaseName);

        _dataCollection = mongoDatabase.GetCollection<Data>(
            proteinIDatabaseSettings.Value.DataCollectionName);
    }

    public async Task<List<Data>> GetAsync() =>
        await _dataCollection.Find(_ => true).ToListAsync();

    public async Task<Data?> GetAsync(string id) =>
        await _dataCollection.Find(x => x.DataId == id).FirstOrDefaultAsync();

    public async Task<List<Data>> GetByTargetAsync(string targetId) =>
        await _dataCollection.Find(x => x.TargetId == targetId).ToListAsync();

    public async Task CreateAsync(Data newData) =>
        await _dataCollection.InsertOneAsync(newData);

    public async Task UpdateAsync(string id, Data updatedData) =>
        await _dataCollection.ReplaceOneAsync(x => x.DataId == id, updatedData);

    public async Task RemoveAsync(string id) =>
        await _dataCollection.DeleteOneAsync(x => x.DataId == id);
}



