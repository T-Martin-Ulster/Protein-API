using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ProteinApi.Models;

public class Transaction
{

    public Transaction()
    {
       TransactionDate = DateTime.Now;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TransactionId { get; set; } = null!;

    //Transaction
    public DateTime TransactionDate { get; set; }

    //Item
    public string BatchId { get; set; } = null!;

    public string BatchMsgId { get; set; } = null!;

    public double Weight { get; set; } //Kg

    //Actors
    public string SendorId { get; set; } = null!;

    public string SendorMsgId { get; set; } = null!;

    public string ReciverId { get; set; } = null!;

    public string ReciverMsgId { get; set; } = null!;

    //Tangle message
    public string? MessageId { get; set; }
}

