namespace ProteinApi.Models;

public class ProteinIDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set;} = null!;

    public string BatchCollectionName { get; set; } = null!;

    public string BusinessCollectionName { get; set; } = null!;

    public string CropCollectionName { get; set; } = null!;

    public string DataCollectionName { get; set; } = null!;

    public string FieldCollectionName { get; set; } = null!;

    public string MixedBatchCollectionName { get; set; } = null!;

    public string ShipmentCollectionName { get; set; } = null!;

    public string TransactionCollectionName { get; set; } = null!;
}

