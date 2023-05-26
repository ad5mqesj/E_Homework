using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentValidation;
using E_Homework.DTO.Models;

namespace E_Homework.DTO.Validators
{
    public class Foo1Validator : AbstractValidator<Foo1>
    {
        public Foo1Validator()
        {
            RuleFor(f => f.PartnerId).GreaterThan(0);
            RuleFor(f => f.PartnerName).NotEmpty();
            RuleFor(f => f.Trackers).NotEmpty();
            RuleForEach(f => f.Trackers).SetValidator(new TrackerValidator());
        }

    }//Foo1Validator

    public class TrackerValidator : AbstractValidator<Tracker>
    {
        public TrackerValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0);
            RuleFor(t => t.Model).NotEmpty();
            RuleFor(t => t.ShipmentStartDtm).Must(SpecialTests.BeWithin3yearsOfNow);
            RuleFor(t => t.Sensors).NotEmpty();
            RuleForEach(t=>t.Sensors).SetValidator(new SensorValidator());
        }
    }

    public class SensorValidator : AbstractValidator<Sensor>
    {
        public SensorValidator()
        {
            RuleFor(s => s.Id).GreaterThan(0);
            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s=>s.Crumbs).NotEmpty();
            RuleForEach(s => s.Crumbs).SetValidator(new SensorCrumbValidator());
        }
    }

    public class SensorCrumbValidator : AbstractValidator<SensorCrumb>
    {
        public SensorCrumbValidator()
        {
            RuleFor(c=>c.CreatedDtm).Must(SpecialTests.BeWithin3yearsOfNow);
            RuleFor(c=>c.Value).GreaterThan(float.MinValue).LessThan(float.MaxValue);
        }
    }

    public static class SpecialTests
    {
        public static bool BeWithin3yearsOfNow(DateTimeOffset target)
        {
            var rightNow = DateTimeOffset.UtcNow;
            var diff = rightNow.Subtract(target);
            return diff.TotalSeconds >= 0 && diff.TotalDays < 365 * 3;
        }

    }

}
