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
    public class Foo1ValidatorTests
    {
        #region PrivateProperties
        private readonly string foo1File = "DeviceDataFoo1.json";
        private JsonSerializerOptions options;
        #endregion PrivateProperties

        #region Construcor(s)
        public Foo1ValidatorTests()
        {
            options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new CustomDateTimeConverter("MM-dd-yyyy H:mm:ss"));
        }
        #endregion Construcor(s)

        #region HelperMethods
        private Foo1 GetValidFoo1()
        {
            var json = File.ReadAllText(foo1File);
            Foo1 retf = JsonSerializer.Deserialize<Foo1>(json, options);
            return retf;
        }
        #endregion HelperMethods

        #region Tests
        [Fact]
        void HappyPathFoo1Succeeds()
        {
            Foo1 f1 = GetValidFoo1();
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void SerDesSuccess()
        {
            Foo1 retf = GetValidFoo1();
            var js = JsonSerializer.Serialize<Foo1>(retf, options);
            var fd = JsonSerializer.Deserialize<Foo1>(js, options);
            retf.ShouldDeepEqual(fd);

        }

        [Fact]
        void FailsWithEmptyPartnerName()
        {
            Foo1 f1 = GetValidFoo1();
            f1.PartnerName = string.Empty;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadPartnerId()
        {
            Foo1 f1 = GetValidFoo1();
            f1.PartnerId = 0;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        void FailsWithNoTrackers()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers = new List<Tracker>();
            //break valid data
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadTrackerId()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].Id = 0;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadTrackerModel()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].Model = string.Empty;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        void FailsWithBadTrackerDate()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].ShipmentStartDtm = DateTimeOffset.MinValue;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithNoSensors()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].Sensors = new List<Sensor>();
            //break valid data
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadSensorId()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].Sensors[0].Id = 0;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadSensorName()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].Sensors[0].Name = string.Empty;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithNoCrumbs()
        {
            Foo1 f1 = GetValidFoo1();
            //break valid data
            foreach (Tracker t in f1.Trackers)
            {
                foreach (Sensor s in t.Sensors)
                {
                    s.Crumbs = new List<SensorCrumb>();
                }
            }
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadCrumbDate()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].Sensors[0].Crumbs[0].CreatedDtm = DateTimeOffset.MinValue;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        void FailsWithBadCrumbValue()
        {
            Foo1 f1 = GetValidFoo1();
            f1.Trackers[0].Sensors[0].Crumbs[0].Value = float.NaN;
            var validator = new Foo1Validator();
            var result = validator.Validate(f1);
            result.IsValid.Should().BeFalse();
        }
        #endregion Tests

    }
}
