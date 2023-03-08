using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProteinApi.Models;

public class Crop
{

    public Crop()
    {
        PlantDate = DateTime.Now;

        Harvested = true;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CropId { get; set; } = null!;

    public string CropName { get; set; } = null!;

    public string? CropVariety { get; set; }

    public DateTime PlantDate { get; set; }

    public string? FieldId { get; set; }

    public string? FieldMsgId { get; set; }

    //Inventory
    public string ProducerId { get; set; } = null!;

    public string ProducerMsgId { get; set; } = null!;

    public bool Harvested { get; set; }

    //Traceability
    public string MessageId { get; set; } = null!;

}

