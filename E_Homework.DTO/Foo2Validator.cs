using FluentValidation;

using E_Homework.DTO.Models;

namespace E_Homework.DTO.Validators
{
    public class Foo2Validator : AbstractValidator<Foo2>
    {
        public Foo2Validator()
        {
            RuleFor(f => f.CompanyId).GreaterThan(0);
            RuleFor(f => f.Company).NotEmpty();
            RuleFor(f => f.Devices).NotEmpty();
            RuleForEach(f => f.Devices).SetValidator(new DeviceValidator());
        }

        public class DeviceValidator : AbstractValidator<Device>
        {
            public DeviceValidator()
            {
                RuleFor(d => d.DeviceID).GreaterThan(0);
                RuleFor(d => d.Name).NotEmpty();
                RuleFor(d => d.StartDateTime).Must(CustomTests.BeWithin3yearsOfNow);
                RuleFor(d => d.SensorData).NotEmpty();
                RuleForEach(d => d.SensorData).SetValidator(new F2SensorValidator());
            }
        }

        public class F2SensorValidator : AbstractValidator<F2Sensor>
        {
            public F2SensorValidator() 
            { 
                RuleFor(s => s.SensorType).NotEmpty();
                RuleFor(s => s.SensorType).Must(x => !x.Equals("Unknown"));
                RuleFor(s => s.DateTime).Must(CustomTests.BeWithin3yearsOfNow);
                RuleFor(s => s.Value).GreaterThan(float.MinValue).LessThan(float.MaxValue);
            }
        }
    }
}
