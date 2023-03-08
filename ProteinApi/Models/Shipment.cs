using System.Net.NetworkInformation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProteinApi.Models;

public class Shipment
{

    public Shipment()
    {
        ShipmentDate = DateTime.Now;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ShipmentId { get; set; } = null!;

    //Transaction
    public DateTime ShipmentDate { get; set; }

    //Item
    public string BatchId { get; set; } = null!;

    public string BatchMsgId { get; set; } = null!;

    public double Weight { get; set; } //Kg

    //Sendor
    public string SendorId { get; set; } = null!;

    public string SendorMsgId { get; set; } = null!;

}
