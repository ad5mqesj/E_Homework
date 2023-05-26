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
        void HappyPathFoo1Succeeds()
        {
            Foo2 f2 = GetValidFoo2();
            var validator = new Foo2Validator();
            var result = validator.Validate(f2);
            result.IsValid.Should().BeTrue();
        }
        #endregion Tests

    }
}
