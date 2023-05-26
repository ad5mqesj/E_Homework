using DeepEqual.Syntax;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

using E_Homework.DTO.Models;
using E_Homework.DTO.Validators;
using System.IO;
using System.Runtime.Intrinsics.Arm;

namespace E_Homework.DTO.Test
{
    public class Foo1ValidatorTests
    {
        private readonly string foo1File = "DeviceDataFoo1.json";
        private JsonSerializerOptions options;

        public Foo1ValidatorTests()
        {
            options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new CustomDateTimeConverter("MM-dd-yyyy H:mm:ss"));
        }
        private Foo1 GetValidFoo1()
        {
            var json = File.ReadAllText(foo1File);
            Foo1 retf = JsonSerializer.Deserialize<Foo1>(json, options);
            return retf;
        }

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
    }
}
