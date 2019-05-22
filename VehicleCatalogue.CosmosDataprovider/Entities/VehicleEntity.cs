using System;

namespace VehicleCatalogue.CosmosDataprovider.Entities
{
    public class VehicleEntity:ICosmosEntity
    {
        public string  Tenant { get; set; }

        public string RowId { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Version { get; set; }

        public int? ModelYear { get; set; }

        public string Vin { get; set; }

        public string RegistrationNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int? MileageValue { get; set; }

        public string MileageUnit { get; set; }

        public int? CashPrice { get; set; }

        public string Currency { get; set; }

        public int? PreviousOwners { get; set; }

        public int? DealerId { get; set; }

        public string StockLocation { get; set; }

        public string FullDescription { get; set; }

        public string Comments { get; set; }

        public string Options { get; set; }

        public string MainColour { get; set; }

        public string FullColourDescription { get; set; }

        public string BodyType { get; set; }

        public int? Doors { get; set; }

        public int? SeatCount { get; set; }

        public int? EngineSizeValue { get; set; }

        public string EngineSizeUnit { get; set; }

        public int? CylinderCount { get; set; }

        public double? EnginePowerValue { get; set; }
        public string EnginePowerUnit { get; set; }

        public double? TorqueValue { get; set; }

        public string TorqueUnit { get; set; }

        public double? MaxTorqueRpm { get; set; }

        public int? GearCount { get; set; }

        public string GearBoxType { get; set; }

        public string FuelType { get; set; }

        public int? CO2Value { get; set; }

        public string CO2Unit { get; set; }

        public string EuroEmissions { get; set; }

        public double? UrbanFuelEconomyValue { get; set; }

        public double? ExtraUrbanFuelEconomyValue { get; set; }

        public double? CombinedFuelEconomyValue { get; set; }

        public string FuelEconomyUnit { get; set; }

        public string Tyres { get; set; }

        public double? VehicleLength { get; set; }

        public double? VehicleWidth { get; set; }

        public double? VehicleHeight { get; set; }

        public string DimensionUnit { get; set; }

        public int? VehicleWeight { get; set; }

        public int? GrossCombinationMass { get; set; }

        public int? GrossVehicleMass { get; set; }

        public string WeightUnit { get; set; }

        public double? FuelTankSizeValue { get; set; }

        public string FuelTankSizeUnit { get; set; }

        public double? TopSpeedValue { get; set; }

        public string TopSpeedUnit { get; set; }

        public double? AccZeroTo100Kph { get; set; }

        public double? AccZeroTo60Mph { get; set; }

        public string DriveWheel { get; set; }

        public bool? Abs { get; set; }

        public bool? Esp { get; set; }

        public int? AirbagCount { get; set; }

        public string ServiceHistory { get; set; }

        public string ImportId { get; set; }

        public string UniqueId { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastUpdatedDate { get; set; }
    }
}
