
namespace E_Homework.DTO.Models
{
    public class CommonDeviceData
    {

        #region PublicProperties
        public int CompanyId { get; set; } = 0; // Foo1: PartnerId, Foo2: CompanyId
        public string CompanyName { get; set; } = string.Empty;// Foo1: PartnerName, Foo2: Company
        public int? DeviceId { get; set; } = null;// Foo1: Id, Foo2: DeviceID
        public string DeviceName { get; set; } = string.Empty;// Foo1: Model, Foo2: Name
        public DateTime? FirstReadingDtm { get; set; } = null;// Foo1: Trackers.Sensors.Crumbs, Foo2: Devices.SensorData
        public DateTime? LastReadingDtm { get; set; } = null;
        public int? TemperatureCount { get; set; }  = null;
        public double? AverageTemperature { get; set; } = null;
        public int? HumidityCount { get; set; } = null;
        public double? AverageHumdity { get; set; } = null;

        #endregion PublicProperties

    }

}
