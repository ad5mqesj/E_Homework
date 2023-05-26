using DeepEqual.Syntax;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;

using E_Homework.DTO.Models;
using E_Homework.DTO.Validators;

namespace E_Homework.DTO.Test
{
    public class Foo2ValidatorTests
    {
        #region PrivateProperties
        private readonly string foo2File = "DeviceDataFoo2.json";
        private JsonSerializerOptions options;
        #endregion PrivateProperties

        #region Construcor(s)
        public Foo2ValidatorTests()
        {
            options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new CustomDateTimeConverter("MM-dd-yyyy H:mm:ss"));
        }
        #endregion Construcor(s)

        #region HelperMethods
        private Foo2 GetValidFoo2()
        {
            var json = File.ReadAllText(foo2File);
            Foo2 retf = JsonSerializer.Deserialize<Foo2>(json, options);
            return retf;
        }
        #endregion HelperMethods

        #region Tests
        [Fact]
        void HappyPathFoo2Succeeds()
        {
            Foo2 f2 = GetValidFoo2();
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void SerDesSuccess()
        {
            Foo2 retf = GetValidFoo2();
            var js = JsonSerializer.Serialize<Foo2>(retf, options);
            var fd = JsonSerializer.Deserialize<Foo2>(js, options);
            retf.ShouldDeepEqual(fd);

        }

        [Fact]
        void FailsWithEmptyCompany()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Company = string.Empty;
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadCompanyId()
        {
            Foo2 f2 = GetValidFoo2();
            f2.CompanyId = 0;
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        void FailsWithNoDevices()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices = new List<Device>();
            //break valid data
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadDeviceId()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices[0].DeviceID = 0;
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadDeviceModel()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices[0].Name = string.Empty;
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        void FailsWithBadDeviceDate()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices[0].StartDateTime = DateTimeOffset.MinValue;
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithNoSensorData()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices[0].SensorData = new List<F2Sensor>();
            //break valid data
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }


        [Fact]
        void FailsWithBadSensorDataDate()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices[0].SensorData[0].DateTime = DateTimeOffset.MinValue;
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadSensorDataValue()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices[0].SensorData[0].Value = float.NaN;
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadSensorDataType()
        {
            Foo2 f2 = GetValidFoo2();
            f2.Devices[0].SensorData[0].SensorType = "Barometer";
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeFalse();
        }
        #endregion Tests

    }
}
