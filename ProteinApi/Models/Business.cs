using System.Net.NetworkInformation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ProteinApi.Models;


public class Business
{

    [BsonId]
    public string BusinessId { get; set; } = null!;

    public string BusinessName { get; set; } = null!;

    public string? Decription { get; set; }

    public string? BusinessNumber { get; set; }

    public string? GpsCoordinates { get; set; }

    //Tangle
    public string MessageId { get; set; } = null!;
}


