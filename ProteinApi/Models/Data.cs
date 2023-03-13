using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProteinApi.Models;

public class Data
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string DataId { get; set; } = null!;

    public string DataName { get; set; } = null!;

    public double DataValue { get; set; }

    public string TargetId { get; set; } = null!; //Crop, batch or field Id

    //Traceability
    public string? MessageId { get; set; }

}

