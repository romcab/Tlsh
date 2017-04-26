using System;

namespace tlsh.digests
{
    internal class BitPairsTable
    {
        private const int BitPairsDiffTableSize = 256;

        private static readonly int[][] BitPairsDiffTable = GenerateDefaultBitPairsDiffTable();

        private static int[][] GenerateDefaultBitPairsDiffTable()
        {
            int[][] result = new int[BitPairsDiffTableSize][];

            for (int i = 0; i < BitPairsDiffTableSize; i++)
            {
                for (int j = 0; j < BitPairsDiffTableSize; j++)
                {
                    int x = i;
                    int y = j;
                    int diff = 0;

                    for (int z = 0; z < 4; z++)
                    {
                        int d = Math.Abs(x % 4 - y % 4);

                        if (d == 3)
                        {
                            diff += d * 2;
                        }
                        else
                        {
                            diff += d;
                        }

                        if (z < 3)
                        {
                            x /= 4;
                            y /= 4;
                        }
                    }

                    result[i][j] = diff;
                }
            }

            return result;
        }

        public static int GetValue(int row, int column)
        {
            return BitPairsDiffTable[row][column];
        }
    }
}