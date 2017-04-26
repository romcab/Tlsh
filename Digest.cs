namespace tlsh.digests
{
    internal class Digest
    {
        private readonly Checksum _checksum;

        private readonly LValue _lValue;

        private readonly Q _q;

        private readonly Body _body;

        public Digest(Checksum checksum, LValue lValue, Q q, Body body)
        {
            _lValue = lValue;
            _checksum = checksum;
            _q = q;
            _body = body;
        }

        public int CalculateDifference(Digest other, bool lengthDiff)
        {
            int difference = 0;

            if (lengthDiff)
            {
                difference += _lValue.CalculateDifference(other._lValue);
            }

            difference += _q.CalculateDifference(other._q);
            difference += _checksum.CalculateDifference(other._checksum);
            difference += _body.CalculateDiffence(other._body);

            return difference;
        }

        public string toString()
        {
            return new DigestStringBuilder().Append(_checksum).Append(_lValue).Append(_q).Append(_body).Build();
        }
    }
}