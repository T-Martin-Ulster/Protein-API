using ProteinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProteinApi.Services;

public class ShipmentService
{
    private readonly IMongoCollection<Shipment> _shipmentCollection;

    public ShipmentService(
        IOptions<ProteinIDatabaseSettings> proteinIDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            proteinIDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            proteinIDatabaseSettings.Value.DatabaseName);

        _shipmentCollection = mongoDatabase.GetCollection<Shipment>(
            proteinIDatabaseSettings.Value.ShipmentCollectionName);
    }

    public async Task<List<Shipment>> GetAsync() =>
        await _shipmentCollection.Find(_ => true).ToListAsync();

    public async Task<Shipment?> GetAsync(string id) =>
        await _shipmentCollection.Find(x => x.ShipmentId == id).FirstOrDefaultAsync();

    public async Task<List<Shipment>> GetByBusinessAsync(string businessId) =>
        await _shipmentCollection.Find(x => x.SendorId == businessId).ToListAsync();

    public async Task<List<Shipment>> GetByBatchAsync(string batchId) =>
        await _shipmentCollection.Find(x => x.BatchId == batchId).ToListAsync();

    public async Task CreateAsync(Shipment newShipment) =>
        await _shipmentCollection.InsertOneAsync(newShipment);

    public async Task UpdateAsync(string id, Shipment updatedShipment) =>
        await _shipmentCollection.ReplaceOneAsync(x => x.ShipmentId == id, updatedShipment);

    public async Task RemoveAsync(string id) =>
        await _shipmentCollection.DeleteOneAsync(x => x.ShipmentId == id);
}



