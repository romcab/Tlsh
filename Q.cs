namespace tlsh.digests
{
    internal class Q
    {
        private const int RangeQratio = 16;

        private readonly int _value;

        public Q(int value)
        {
            _value = value;
        }

        public Q(int qLo, int qHi) : this(CalculateValue(qLo, qHi))
        {
        }

        public int CalculateDifference(Q other)
        {
            int diff = 0;

            int q1Diff = ModularDifferenceCalculator.Calculate(GetQLo(), other.GetQLo(), RangeQratio);

            if (q1Diff <= 1)
            {
                diff += q1Diff;
            }
            else
            {
                diff += (q1Diff - 1) * 12;
            }

            int q2Diff = ModularDifferenceCalculator.Calculate(GetQHi(), other.GetQHi(), RangeQratio);

            if (q2Diff <= 1)
            {
                diff += q2Diff;
            }
            else
            {
                diff += (q2Diff - 1) * 12;
            }

            return diff;
        }

        public int GetValue()
        {
            return _value;
        }

        private int GetQLo()
        {
            return _value & 0x0F;
        }

        private int GetQHi()
        {
            return (_value & 0xF0) >> 4;
        }

        private static int CalculateValue(int qLo, int qHi)
        {
            return (((0 & 0xF0) | (qLo & 0x0F)) & 0x0F) | ((qHi & 0x0F) << 4);
        }
    }
}