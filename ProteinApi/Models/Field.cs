using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProteinApi.Models;

public class Field
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FieldId { get; set; } = null!;

    public string? FieldName { get; set; }

    public double? FieldArea { get; set; }

    public string FarmId { get; set; } = null!;

    //Traceability
    public string MessageId { get; set; } = null!;

}

