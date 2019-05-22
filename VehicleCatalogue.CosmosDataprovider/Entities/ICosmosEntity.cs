namespace VehicleCatalogue.CosmosDataprovider.Entities
{
    public interface ICosmosEntity
    {
        string Tenant { get; set; }

        string RowId { get; set; }
    }
}
