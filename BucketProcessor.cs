namespace tlsh.buckets
{
    internal class BucketProcessor
    {
        private const int ArrayBucketSize = 256;

        public ProcessedBuckets Process(byte[] data)
        {
            int[] checksum = null;
            int[] bucketArray = new int[ArrayBucketSize];

            SlideWindow slideWindow = new SlideWindow();
            int length = data.Length;

            for (int i = 0; i < length; i++)
            {
                int startWindow = slideWindow.GetPivot();
                slideWindow.Put(data[i]);

                checksum = slideWindow.GetChecksum(startWindow, checksum);

                foreach (var tripletHash in slideWindow.GetTripletHashes(startWindow))
                {
                    bucketArray[tripletHash]++;
                }
            }

            return buildProcessedBuckets(length, bucketArray, checksum);
        }

        //public ProcessedBuckets Process(string data)
        //{
        //    int[] checksum = null;
        //    int[] bucketArray = new int[ArrayBucketSize];

        //    SlideWindow slideWindow = new SlideWindow();
        //    int length = data.Length;

        //    for (int i = 0; i < length; i++)
        //    {
        //        int startWindow = slideWindow.GetPivot();
        //        slideWindow.Put(data[i]);

        //        checksum = slideWindow.GetChecksum(startWindow, checksum);

        //        foreach (var tripletHash in slideWindow.GetTripletHashes(startWindow))
        //        {
        //            bucketArray[tripletHash]++;
        //        }
        //    }

        //    return buildProcessedBuckets(length, bucketArray, checksum);
        //}

        private ProcessedBuckets buildProcessedBuckets(int dataLength, int[] bucketArray, int[] checksum)
        {
            Quartiles quartiles = new Quartiles(bucketArray);
            return new ProcessedBuckets(checksum, bucketArray, dataLength, quartiles);
        }
    }
}