using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Homework.DTO.Validators
{
        public static class CustomTests
        {
            public static bool BeWithin3yearsOfNow(DateTimeOffset target)
            {
                var rightNow = DateTimeOffset.UtcNow;
                var diff = rightNow.Subtract(target);
                return diff.TotalSeconds >= 0 && diff.TotalDays < 365 * 3;
            }

        }
}
