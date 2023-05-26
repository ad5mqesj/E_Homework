using E_Homework.DTO.Models;
using E_Homework.Providers.Implementation;

namespace E_Homework.Providers.Interface
{
    public class DataConverter : IDataConverter
    {
        #region PrivateProperties
        private readonly ILogger<DataConverter> logger;
        #endregion PrivateProperties

        #region Construcor(s)
        public DataConverter (ILogger<DataConverter> _logger)
        {
            this.logger = _logger;
        }
        #endregion Construcor(s)

        public IEnumerable<CommonDeviceData> ConvertData(Object data)
        {
            //make sure input is one of the expected formats
            if (data is not Foo1 && data is not Foo2)
            {
                logger.LogError($"Input data type not recognized {data.GetType().ToString()}");
                return Enumerable.Empty<CommonDeviceData>();
            }
            if (data is Foo1)
                return fromFoo1(data as Foo1);

            return fromFoo2(data as Foo2);
        }

        private IEnumerable<CommonDeviceData> fromFoo1 (Foo1 ? data)
        {
            if (data == null)
            {
                logger.LogError("Data recognized as Foo1, but fails to cast");
                return Enumerable.Empty<CommonDeviceData>();
            }
            List<CommonDeviceData> returnData = new List<CommonDeviceData>();
            foreach (Tracker t in data.Trackers)
            {
                CommonDeviceData d = new CommonDeviceData()
                {
                    CompanyId = data.PartnerId,
                    CompanyName = data.PartnerName,
                    DeviceId = t.Id,
                    DeviceName = t.Model
                };
                var allCrumbs = t.Sensors.SelectMany(s => s.Crumbs);
                d.FirstReadingDtm = (from c in allCrumbs select c.CreatedDtm).Min().DateTime;
                d.LastReadingDtm = (from c in allCrumbs select c.CreatedDtm).Max().DateTime;
                
                var tempcrumbs = (from s in t.Sensors where s.Name.Equals("Temperature") select s).SelectMany(s => s.Crumbs);
                d.TemperatureCount = tempcrumbs.Count();
                d.AverageTemperature = 0.0;
                foreach (SensorCrumb c in tempcrumbs)
                {
                    d.AverageTemperature += c.Value;
                }
                d.AverageTemperature = d.AverageTemperature / (double)d.TemperatureCount;

                var humcrumbs = (from s in t.Sensors where s.Name.Equals("Humidty") select s).SelectMany(s => s.Crumbs);
                d.HumidityCount = humcrumbs.Count();
                d.AverageHumdity = 0.0;
                foreach (SensorCrumb c in humcrumbs)
                {
                    d.AverageHumdity += c.Value;
                }
                d.AverageHumdity = d.AverageHumdity / (double)d.HumidityCount;
                returnData.Add(d);
            }

            return returnData;
        }

        private IEnumerable<CommonDeviceData> fromFoo2(Foo2? data)
        {
            if (data == null)
            {
                logger.LogError("Data recognized as Foo2, but fails to cast");
                return Enumerable.Empty<CommonDeviceData>();
            }

            List<CommonDeviceData> returnData = new List<CommonDeviceData>();
            foreach (Device d in data.Devices) 
            {
                CommonDeviceData cd = new CommonDeviceData()
                {
                    CompanyId = data.CompanyId,
                    CompanyName = data.Company,
                    DeviceId = d.DeviceID,
                    DeviceName = d.Name
                };
                cd.FirstReadingDtm = (from s in d.SensorData select s.DateTime).Min().DateTime;
                cd.LastReadingDtm = (from s in d.SensorData select s.DateTime).Max().DateTime;

                var temps = (from s in d.SensorData where s.SensorType.Equals("TEMP") select s);
                cd.TemperatureCount = temps.Count();
                cd.AverageTemperature = 0.0;

                foreach (F2Sensor s in temps)
                {
                    cd.AverageTemperature += s.Value;
                }
                cd.AverageTemperature = cd.AverageTemperature / (double)cd.TemperatureCount;

                var hums = (from s in d.SensorData where s.SensorType.Equals("HUM") select s);
                cd.HumidityCount = hums.Count();

                cd.AverageHumdity = 0.0;
                foreach (F2Sensor s in hums)
                {
                    cd.AverageHumdity += s.Value;
                }
                cd.AverageHumdity = cd.AverageHumdity / (double)cd.HumidityCount;
                returnData.Add(cd);
            }

            return returnData;
        }

    }
}
