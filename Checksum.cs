using System.Linq;

namespace tlsh.digests
{
    internal class Checksum
    {
        private const int DiffChecksumValue = 1;

        private const int EqualsChecksumValue = 0;

        private readonly int[] _value;

        public Checksum(int[] checksum)
        {
            _value = checksum;
        }

        public int CalculateDifference(Checksum other)
        {
            if (!_value.SequenceEqual(other.GetValue())) return DiffChecksumValue;
            return EqualsChecksumValue;
        }

        public int[] GetValue()
        {
            return _value;
        }
    }
}