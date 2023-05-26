namespace E_Homework.DTO.Models
{
    public class Foo1
    {
        #region PublicProperties
        public int PartnerId { get; set; } = 0;
        public string PartnerName { get; set; } = string.Empty;
        public List<Tracker> Trackers { get; set; } = new List<Tracker>();

        #endregion PublicProperties

    }

    public class Tracker
    {
        #region PublicProperties
        public int Id { get; set; } = 0;
        public string Model { get; set; } = string.Empty;
        public DateTimeOffset ShipmentStartDtm { get; set; } = DateTimeOffset.MinValue;
        public List<Sensor> Sensors { get; set; } = new List<Sensor>();

        #endregion PublicProperties
    }

    public class Sensor
    {
        #region PublicProperties
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        
        public List<SensorCrumb> Crumbs { get; set; } = new List<SensorCrumb>();

        #endregion PublicProperties
    }

    public class SensorCrumb
    {
        #region PublicProperties
        public DateTimeOffset CreatedDtm { get; set; } = DateTimeOffset.MinValue;
        public float Value { get; set; } = 0.0f;

        #endregion PublicProperties
    }
}
