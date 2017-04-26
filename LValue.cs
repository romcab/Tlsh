using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tlsh.digests
{
    class LValue
    {
        private const int RangeLvalue = 256;

        private readonly int _value;

        public LValue(int value)
        {
            _value = value;
        }

        public int GetValue()
        {
            return _value;
        }

        public int CalculateDifference(LValue other)
        {
            int ldiff = ModularDifferenceCalculator.Calculate(_value, other.GetValue(), RangeLvalue);

            if (ldiff == 0)
            {
                return 0;
            }

            if (ldiff == 1)
            {
                return 1;
            }

            return ldiff * 12;
        }
    }
}
