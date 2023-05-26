using Moq;
using System.Threading.Tasks;

using E_Homework.DTO;
using E_Homework.DTO.Models;
using E_Homework.DTO.Validators;
using E_Homework.Providers.Implementation;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using System;

namespace E_Homework.UnitTest
{
    public class MoqProvider
    {
        public Mock<IDataConverter> MockConverterProvider { get; set; }

        public MoqProvider()
        {
            this.MockConverterProvider = new Mock<IDataConverter>();

            SetupCommonDataProvider();
        }

        private void SetupCommonDataProvider() 
        {
            MockConverterProvider.Setup(x => x.ConvertData(It.IsAny<Foo1>())).Returns(moqCommonDeviceDataList);
            MockConverterProvider.Setup(x => x.ConvertData(It.IsAny<Foo2>())).Returns(moqCommonDeviceDataList);
        }

        private List<CommonDeviceData> moqCommonDeviceDataList()
        {
            List<CommonDeviceData> cdd = new List<CommonDeviceData>();
            cdd.Add(new CommonDeviceData()
            {
                CompanyId = 1,
                CompanyName = "Test",
                DeviceId = 1,
                DeviceName = "TestDevice",
                TemperatureCount = 1,
                AverageTemperature = 20.0,
                HumidityCount = 1,
                AverageHumdity = 20.0,
                FirstReadingDtm = DateTime.UtcNow.AddDays(-1),
                LastReadingDtm = DateTime.UtcNow.AddMinutes(-1)
            });
            return cdd;
        }
    }
}
