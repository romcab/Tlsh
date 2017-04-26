using System;
using tlsh.buckets;

namespace tlsh.digests
{
    internal class DigestBuilder
    {
        private Checksum _checksum;

        private LValue _lValue;

        private Q _q;

        private Body _body;

        public DigestBuilder WithHash(string hash)
        {
            int[] digestData = FromHex(hash);
            int i = 0;

            WithChecksumData(digestData[i++]);
            WithLValueData(digestData[i++]);
            WithQData(digestData[i++]);
            WithBodyData(GetBodyData(digestData, i));

            return this;
        }

        public Digest Build()
        {
            return new Digest(_checksum, _lValue, _q, _body);
        }

        private static int[] FromHex(string s)
        {
            int[] result = new int[s.Length / 2];

            for (int i = 0; i < s.Length; i += 2)
            {
                result[i / 2] = Convert.ToInt32(s.Substring(i, i + 2), 16);
                //result[i / 2] = Integer.parseInt(s.Substring(i, i + 2), 16);
            }

            return result;
        }

        private void WithChecksumData(int data)
        {
            this._checksum = new Checksum(new int[] { swap(data) });
        }

        private void WithLValueData(int data)
        {
            this._lValue = new LValue(swap(data));
        }

        private void WithQData(int data)
        {
            this._q = new Q(swap(data));
        }

        private void WithBodyData(int[] data)
        {
            int[] bodyData = new int[data.Length];

            for (int j = 0; j < data.Length; j++)
            {
                bodyData[j] = (data[data.Length - 1 - j]);
            }

            _body = new Body(bodyData);
        }

        private static int[] GetBodyData(int[] digestData, int from)
        {
            int[] bodyData = new int[ProcessedBuckets.CodeSize];
            Array.Copy(digestData, from, bodyData, 0, bodyData.Length);
            return bodyData;
        }

        private int swap(int data)
        {
            return ByteSwapper.Swap(data);
        }
    }
}