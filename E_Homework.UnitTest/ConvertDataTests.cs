using E_Homework.Controllers;
using E_Homework.Providers.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.IO;
using E_Homework.DTO.Models;
using System.Collections.Generic;
using System.Collections;

namespace E_Homework.UnitTest
{
    public class ConvertDataTests
    {
        private readonly ILogger<ConvertData> logger;
        private readonly ConvertData dataConvertController;
        private Mock<IDataConverter> mockCnvProvider;

        public ConvertDataTests()
        {
            logger = new LoggerFactory().CreateLogger<ConvertData>();
            var moqProvider = new MoqProvider();
            this.mockCnvProvider = moqProvider.MockConverterProvider;
            this.dataConvertController = new ConvertData(logger, this.mockCnvProvider.Object);
        }

        [Theory]
        [ClassData(typeof(TestFoo1Data))]
        public void ConvertFoo1(string dt)
        {
            var res = dataConvertController.Convert(dt);
            res.Should().NotBeNull();
            var result = (OkObjectResult)res;
            result.Value.Should().NotBeNull();
            result.Value.Should().As<IEnumerable<CommonDeviceData>>();
        }

    }
    public class TestFoo1Data : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                File.ReadAllText("DeviceDataFoo1.json")
        
        };

        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
