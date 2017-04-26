using System;
using System.Text;

namespace tlsh.digests
{
    internal class DigestStringBuilder
    {
        private readonly StringBuilder _value;

        public DigestStringBuilder()
        {
            _value = new StringBuilder();
        }

        public DigestStringBuilder Append(Checksum checksum)
        {
            int[] swappedChecksum = new int[checksum.GetValue().Length];

            for (int k = 0; k < swappedChecksum.Length; k++)
            {
                swappedChecksum[k] = swap(checksum.GetValue()[k]);
            }

            _value.Append(ToHex(swappedChecksum));
            return this;
        }

        public DigestStringBuilder Append(LValue lValue)
        {
            _value.Append(ToHex(swap(lValue.GetValue())));
            return this;
        }

        public DigestStringBuilder Append(Q q)
        {
            _value.Append(ToHex(swap(q.GetValue())));
            return this;
        }

        public DigestStringBuilder Append(Body body)
        {
            int[] swappedBody = new int[body.GetBody().Length];

            for (int i = 0; i < swappedBody.Length; i++)
            {
                swappedBody[i] = body.GetBody()[swappedBody.Length - 1 - i];
            }

            _value.Append(ToHex(swappedBody));
            return this;
        }

        public string Build()
        {
            return _value.ToString();
        }

        private string ToHex(int val)
        {
            int num = (0xFF & val);
            return num.ToString("X2");
            //return string.Format("%02X", (0xFF & val));
        }

        private string ToHex(int[] values)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < values.Length; i++)
            {
                result.Append(ToHex(values[i]));
            }

            return result.ToString().ToUpper();
        }

        private int swap(int data)
        {
            return ByteSwapper.Swap(data);
        }
    }
}