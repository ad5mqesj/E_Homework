using E_Homework.DTO.Models;
using E_Homework.Providers.Implementation;

namespace E_Homework.Providers.Interface
{
    public class DataConverter : IDataConverter
    {
        public IEnumerable<CommonDeviceData> ConvertData(Object data)
        {
            //make sure input is one of the expected formats
            if (data is not Foo1 && data is not Foo2)
                return Enumerable.Empty<CommonDeviceData>();
            if (data is Foo1)
                return fromFoo1(data as Foo1);

            return fromFoo2(data as Foo2);
        }

        private IEnumerable<CommonDeviceData> fromFoo1 (Foo1 ? data)
        {
            if (data == null)
                return Enumerable.Empty<CommonDeviceData>();

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
                return Enumerable.Empty<CommonDeviceData>();

            List<CommonDeviceData> returnData = new List<CommonDeviceData>();

            return returnData;
        }

    }
}
