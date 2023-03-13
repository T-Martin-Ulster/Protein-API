using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProteinApi.Models;

public class Batch
{

    public Batch()
    {
        BatchDate = DateTime.Now;

        InStock = true;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BatchId { get; set; } = null!;

    public string BatchName { get; set; } = null!;

    public string? BatchDesc { get; set; }

    public DateTime BatchDate { get; set; }

    public string BatchType { get; set; } = null!; //Crop, Mixed, Other

    //Crop details
    public string? CropId { get; set; }

    public string? CropMsgId { get; set; }

    //Mixed
    public string? MixedBatchId { get; set; }

    public string? MixeBatchMsgId { get; set; }

    //Inventory
    public string HolderId { get; set; } = null!;

    public string HolderMsgId { get; set; } = null!;

    public bool InStock { get; set; }

    public double Weight { get; set; } //Kg

    public string? Location { get; set; } //for buisness use only

    //Traceability
    public string? TransactionId { get; set; }

    public string? TransactionMsgId { get; set; }

    public string? MessageId { get; set; }

}

