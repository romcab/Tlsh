namespace tlsh.buckets
{
    internal class Triplet
    {
        private readonly int _c1;

        private readonly int _c2;

        private readonly int _c3;

        private readonly int _salt;

        public Triplet(int c1, int c2, int c3, int salt)
        {
            _c1 = c1;
            _c2 = c2;
            _c3 = c3;
            _salt = salt;
        }

        public int GetHash()
        {
            return PearsonHash.Compute(new[] { _salt, _c1, _c2, _c3 });
        }
    }
}