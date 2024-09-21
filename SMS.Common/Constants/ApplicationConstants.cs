namespace SMS.Common.Constants
{
    public static class ApplicationConstants
    {
        public const string NoConfigurationsFoundToUpdate = "No configurations found to update.";
        public const string GetVehicleTypeEndpoint = "VehicleType/GetVehicleTypes";
        public const string GetAllProductsEndpoint = "Product/GetProducts";
        public const string GetAllProductsActivitiesEndpoint = "ProductActivity/GetAllProductsActivities";
        public const string GetVehicleDetailEndpoint = "Vehicle/GetVehicleDetails";
        public const long RoundOffTimeTicks = 3000000000;
        public const string DefaultTimeSpanValue = "00:00";
        public const string DefaultTimeValue = "00:00:00";
        public const string DefaultSpeedFactor = "1";
        public const string ZeroValue = "0";
        public const int DefaultZeroFactor = 0;
        public const int DefaultPositiveFactor = 1;
        public const int DefaultOneFactor = 1;
        public const int DefaultTwoFactor = 2;
        public const int DefaultThreeFactor = 3;
        public const int DefaultCapacity = 25;
        public const int DefaultNegativeFactor = -1;
        public const string DefaultMinTimeSpanValue = "00:00:00";
        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DefaultKMSuffix = " km";
        public const string DefaultMinServiceTimeValue = "00:03:00";
        public const string DefaultStopTimeForInfrastructureLocation = "00:15:00";
        public const string DefaultStopTimeForCommercialLocation = "00:05:00";
        public const string DefaultOpenhours = "00:00:00-23:59:00";
        public const string DefaultMaxTimeSpanValue = "23:59:00";
        public const string TicksToMilliSeconds = "10000";
        public const string MinimumBufferTime = "0.5";
        public const int DefaultMaxParallelism = 10;
        public const int DenmarkTimeZoneOffset = 60;
        public const string DenmarkTimeZone = "Romance Standard Time";
        public const int PakistanTimeZoneOffset = 300;
        public const string LocationSequenceKey = "Sequence";
        public const int DefaultTransportationMode = 2;
        public const int DefaultVehicleTypeId = 11;
        public const string DefaultJobColor = "#FF7700";
        public const string DefaultJobName = "UnAssigned Locations";
        public const string DefaultJobNumber = "0";
        public const string DefaultVehicleTypeName = "Small Van";
        public const string AllowedHost = "AllowedHost";
        public const string DefaultDistributionAreaName = "None";
        public const string DefaultDistributionAreaId = "3eb045a3-a95e-4779-bf7d-aaa85ca4eb27";
        public const string PickupActivityType = "Pickup";
        public const string DeliveryActivityType = "Delivery";
        public const string RoutePrefix = "Route-";
        public const string RouteNumberPrefix = "R-";
        public const string AllActivities = "All Activities";
        public const string DefaultTenantId = "981CEFC1-DECC-403E-8D0B-466E0967593C";
        public const string Schedule = "Schedule";
        public const string EndTerminal = "End Terminal";
        public const string StartTerminal = "Start Terminal";
        public const int KiloMetersToMeters = 1000;
        public const string Infrastructure = "DB4EEB9D-DEB5-4D7D-9FDB-0DF0ECF790A3";
        public const int RefreshTokenTimeDifference = 5;
        public const string NoJobTerminalInRoute = "Job Terminal cannot be duplicated along the route.";
    }
}
