using ProteinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProteinApi.Services;

public class FieldService
{
    private readonly IMongoCollection<Field> _fieldCollection;

    public FieldService(
        IOptions<ProteinIDatabaseSettings> proteinIDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            proteinIDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            proteinIDatabaseSettings.Value.DatabaseName);

        _fieldCollection = mongoDatabase.GetCollection<Field>(
            proteinIDatabaseSettings.Value.FieldCollectionName);
    }

    public async Task<List<Field>> GetAsync() =>
        await _fieldCollection.Find(_ => true).ToListAsync();

    public async Task<Field?> GetAsync(string id) =>
        await _fieldCollection.Find(x => x.FieldId == id).FirstOrDefaultAsync();

    public async Task<List<Field>> GetByBusinessAsync(string businessId) =>
        await _fieldCollection.Find(x => x.FarmId == businessId).ToListAsync();

    public async Task CreateAsync(Field newField) =>
        await _fieldCollection.InsertOneAsync(newField);

    public async Task UpdateAsync(string id, Field updatedField) =>
        await _fieldCollection.ReplaceOneAsync(x => x.FieldId == id, updatedField);

    public async Task RemoveAsync(string id) =>
        await _fieldCollection.DeleteOneAsync(x => x.FieldId == id);
}



