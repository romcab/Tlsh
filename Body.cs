namespace tlsh.digests
{
    internal class Body
    {
        private readonly int[] _body;

        public Body(int[] bodyData)
        {
            _body = bodyData;
        }

        public int[] GetBody()
        {
            return _body;
        }

        public int CalculateDiffence(Body other)
        {
            return HDistance(other);
        }

        private int HDistance(Body other)
        {
            int diff = 0;

            for (int i = 0; i < _body.Length; i++)
            {
                diff += BitPairsTable.GetValue(_body[i], other.GetBody()[i]);
            }

            return diff;
        }
    }
}