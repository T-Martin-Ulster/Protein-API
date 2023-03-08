using ProteinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProteinApi.Services;

public class TransactionService
{
    private readonly IMongoCollection<Transaction> _transactionCollection;

    public TransactionService(
        IOptions<ProteinIDatabaseSettings> proteinIDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            proteinIDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            proteinIDatabaseSettings.Value.DatabaseName);

        _transactionCollection = mongoDatabase.GetCollection<Transaction>(
            proteinIDatabaseSettings.Value.TransactionCollectionName);
    }

    public async Task<List<Transaction>> GetAsync() =>
        await _transactionCollection.Find(_ => true).ToListAsync();

    public async Task<Transaction?> GetAsync(string id) =>
        await _transactionCollection.Find(x => x.TransactionId == id).FirstOrDefaultAsync();

    public async Task<List<Transaction>> GetBySendorAsync(string businessId) =>
        await _transactionCollection.Find(x => x.SendorId == businessId).ToListAsync();

    public async Task<List<Transaction>> GetByReciverAsync(string businessId) =>
        await _transactionCollection.Find(x => x.ReciverMsgId == businessId).ToListAsync();

    public async Task CreateAsync(Transaction newTransaction) =>
        await _transactionCollection.InsertOneAsync(newTransaction);

    public async Task UpdateAsync(string id, Transaction updatedTransaction) =>
        await _transactionCollection.ReplaceOneAsync(x => x.TransactionId == id, updatedTransaction);

    public async Task RemoveAsync(string id) =>
        await _transactionCollection.DeleteOneAsync(x => x.TransactionId == id);
}



