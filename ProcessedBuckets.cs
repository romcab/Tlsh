using tlsh.digests;

namespace tlsh.buckets
{
    internal class ProcessedBuckets
    {
        public const int CodeSize = 32;

        private const int MinimumHashInputLength = 512;

        private readonly int[] _checksum;

        private readonly int[] _bucketArray;

        private readonly int _processedDataLength;

        private readonly Quartiles _quartiles;

        public ProcessedBuckets(int[] checksum, int[] bucketArray, int processedDataLength, Quartiles quartiles)
        {
            _checksum = checksum;
            _bucketArray = bucketArray;
            _processedDataLength = processedDataLength;
            _quartiles = quartiles;
        }

        public bool IsProcessedDataTooSimple()
        {
            return !HasMinimumAmountOfDataProcessed() || !HasMinimumNonZeroBuckets();
        }

        public Digest BuildDigest()
        {
            int[] bodyData = CalculateBody();

            return new DigestBuilder()
                    .WithChecksum(_checksum)
                    .WithLength(_processedDataLength)
                    .WithQuartiles(_quartiles)
                    .WithBody(bodyData).Build();
        }

        private int[] CalculateBody()
        {
            int[] body = new int[CodeSize];

            for (int i = 0; i < CodeSize; i++)
            {
                int h = 0;
                for (int j = 0; j < 4; j++)
                {
                    int k1 = _bucketArray[4 * i + j];

                    if (_quartiles.GetThird() < k1)
                    {
                        h += 3 << (j * 2);
                    }
                    else if (_quartiles.GetSecond() < k1)
                    {
                        h += 2 << (j * 2);
                    }
                    else if (_quartiles.GetFirst() < k1)
                    {
                        h += 1 << (j * 2);
                    }
                }

                body[i] = h;
            }

            return body;
        }

        private bool HasMinimumAmountOfDataProcessed()
        {
            return _processedDataLength >= MinimumHashInputLength;
        }

        private bool HasMinimumNonZeroBuckets()
        {
            int nonZeroBuckets = 0;

            for (int index = 0; index < (CodeSize * 4); index++)
            {
                if (IsPositiveBucket(index))
                {
                    nonZeroBuckets++;
                }
            }

            return nonZeroBuckets > (2 * CodeSize);
        }

        private bool IsPositiveBucket(int index)
        {
            return _bucketArray[index] > 0;
        }
    }
}