using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tlsh.buckets;
using tlsh.exceptions;

namespace tlsh
{
    public class TlshWrapper
    {
        //private readonly string _data;

        public TlshWrapper()
        {
        }

        public string Hash(byte[] data)
        {
            var buckets = new BucketProcessor().Process(data);

            if (buckets.IsProcessedDataTooSimple())
            {
                throw new InsufficientComplexityException("Input data hasn't enough complexity");
            }

            return buckets.BuildDigest().toString();
        }

        //public string Hash(string data)
        //{
        //    if (string.IsNullOrEmpty(data))
        //    {
        //        throw new ArgumentException("Input data should not be empty or null");
        //    }

        //    var buckets = new BucketProcessor().Process(data);

        //    if (buckets.IsProcessedDataTooSimple())
        //    {
        //        throw new InsufficientComplexityException("Input data hasn't enough complexity");
        //    }

        //    return buckets.BuildDigest().toString();
        //}
    }
}
