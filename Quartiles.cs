using System;

namespace tlsh.buckets
{
    internal class Quartiles
    {
        private const int ArraySampleSize = 128;

        private const int QRatioModule = 16;

        private readonly int[] _sampleArray;

        public Quartiles(int[] data)
        {
            if (data.Length < ArraySampleSize)
                throw new ApplicationException("cannot calculate quartiles for arrays with less than " + ArraySampleSize + " numbers");

            _sampleArray = GetSortedSampleArray(data);
        }

        public int GetFirst()
        {
            return _sampleArray[ArraySampleSize / 4 - 1];
        }

        public int GetSecond()
        {
            return _sampleArray[ArraySampleSize / 2 - 1];
        }

        public int GetThird()
        {
            return _sampleArray[ArraySampleSize - (ArraySampleSize / 4) - 1];
        }

        public int GetQ1Ratio()
        {
            return (GetFirst() * 100 / GetThird()) % QRatioModule;
        }

        public int GetQ2Ratio()
        {
            return (GetSecond() * 100 / GetThird()) % QRatioModule;
        }

        private int[] GetSortedSampleArray(int[] data)
        {
            int[] sampleArray = new int[ArraySampleSize];

            Array.Copy(data, 0, sampleArray, 0, ArraySampleSize);
            Array.Sort(sampleArray);

            return sampleArray;
        }
    }
}