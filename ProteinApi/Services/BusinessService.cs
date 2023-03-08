using ProteinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProteinApi.Services;

public class BusinessService
{
    private readonly IMongoCollection<Business> _businessCollection;

    public BusinessService(
        IOptions<ProteinIDatabaseSettings> proteinIDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            proteinIDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            proteinIDatabaseSettings.Value.DatabaseName);

        _businessCollection = mongoDatabase.GetCollection<Business>(
            proteinIDatabaseSettings.Value.BusinessCollectionName);
    }

    public async Task<List<Business>> GetAsync() =>
        await _businessCollection.Find(_ => true).ToListAsync();

    public async Task<Business?> GetAsync(string id) =>
        await _businessCollection.Find(x => x.BusinessId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Business newBusiness) =>
        await _businessCollection.InsertOneAsync(newBusiness);

    public async Task UpdateAsync(string id, Business updatedBusiness) =>
        await _businessCollection.ReplaceOneAsync(x => x.BusinessId == id, updatedBusiness);

    public async Task RemoveAsync(string id) =>
        await _businessCollection.DeleteOneAsync(x => x.BusinessId == id);
}



