using System;

namespace tlsh.digests
{
    internal class ModularDifferenceCalculator
    {
        public static int Calculate(int initialPosition, int finalPosition, int circularQueueSize)
        {
            int internalDistance = Math.Abs(finalPosition - initialPosition);
            int externalDistance = circularQueueSize - internalDistance;

            return Math.Min(internalDistance, externalDistance);
        }
    }
}