using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProteinApi.Models;

public class MixedBatch
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string MixedBatchId { get; set; } = null!;

    public string[] BatchIds { get; set; } = null!;

    public string[] BatchMsgIds { get; set; } = null!;

    //Traceability
    public string? MessageId { get; set; }

}

