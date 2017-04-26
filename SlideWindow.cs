using System.Collections.Generic;

namespace tlsh.buckets
{
    internal class SlideWindow
    {
        private const int ChecksumLength = 1;

        private const int SlidingWindowSize = 5;

        private readonly int[] _storage;

        private int _counter;

        public SlideWindow()
        {
            _storage = new int[SlidingWindowSize];
            _counter = 0;
        }

        public void Put(int value)
        {
            _storage[GetPivot()] = value & 0xff;
            _counter++;
        }

        public List<int> GetTripletHashes(int fromStartWindow)
        {
            if (!IsComplete()) return new List<int>();

            int startWindow = fromStartWindow;
            int j2 = (startWindow + 1) % SlidingWindowSize;
            int j3 = (startWindow + 2) % SlidingWindowSize;
            int j4 = (startWindow + 3) % SlidingWindowSize;
            int endWindow = (startWindow + 4) % SlidingWindowSize;

            return new List<int>()
                   {
                       GetHash(_storage[startWindow], _storage[endWindow], _storage[j4], 2),
                       GetHash(_storage[startWindow], _storage[endWindow], _storage[j3], 3),
                       GetHash(_storage[startWindow], _storage[j4], _storage[j3], 5),
                       GetHash(_storage[startWindow], _storage[j4], _storage[j2], 7),
                       GetHash(_storage[startWindow], _storage[endWindow], _storage[j2], 11),
                       GetHash(_storage[startWindow], _storage[j3], _storage[j2], 13)
                   };
        }

        public int[] GetChecksum(int fromStartWindow, int[] lastChecksum)
        {
            if (!IsComplete()) return null;

            int endWindow = (fromStartWindow + 4) % SlidingWindowSize;
            int[] checksum = new int[ChecksumLength];

            for (int i = 0; i < ChecksumLength; i++)
            {
                int c1 = GetValue(fromStartWindow);
                int c2 = GetValue(endWindow);
                int c3 = 0;
                int salt = 0;

                if (lastChecksum != null)
                {
                    c3 = lastChecksum[i];
                }

                if (i != 0)
                {
                    salt = checksum[i - 1];
                }

                checksum[i] = GetHash(c1, c2, c3, salt);
            }

            return checksum;
        }

        public int GetPivot()
        {
            return _counter % SlidingWindowSize;
        }

        private bool IsComplete()
        {
            return _counter >= SlidingWindowSize;
        }

        private int GetHash(int c1, int c2, int c3, int salt)
        {
            return new Triplet(c1, c2, c3, salt).GetHash();
        }

        private int GetValue(int index)
        {
            return _storage[index];
        }
    }
}