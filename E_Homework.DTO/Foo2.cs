using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Homework.DTO.Models
{
    public class Foo2
    {
        #region PublicProperties
        public int CompanyId { get; set; } = 0;
        public string Company { get; set; } = string.Empty;

        public List<Device> Devices { get; set; } = new List<Device>();
        #endregion PublicProperties
    }

    public class Device
    {
        #region PublicProperties
        public int DeviceID { get; set; } = 0;
        public string Name { get; set; }= string.Empty;
        public DateTimeOffset StartDateTime { get; set; } = DateTimeOffset.MinValue;

        public List<F2Sensor> SensorData { get; set; } = new List<F2Sensor>();
        #endregion PublicProperties
    }

    public class F2Sensor
    {
        #region PrivateProperties
        F2SensorType _sensorType = F2SensorType.Unknown;
        #endregion PrivateProperties

        #region PublicProperties
        //this rather akward construcion provides a fairly simple way to force input
        //sensor type to conform to a known type 
        public string SensorType 
        {
            get
            {
                return Enum.GetName(typeof(F2SensorType), _sensorType)??"Unknown";
            }
            set
            {
                Enum.TryParse(value, out _sensorType);
            }
        }

        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.MinValue;

        public float Value { get; set; } = 0.0f;

        #endregion PublicProperties

    }

    public enum F2SensorType
    {
        Unknown,
        TEMP,
        HUM
    }
}
