using System;

namespace Bk
{
    public class Projection
    {
        public Projection(float _min, float _max)
        {
            Min = _min;
            Max = _max;
        }

        public bool overlaps(Projection _other)
        {
            return !(Min > _other.Max || _other.Min > Max);
        }

        public float getOverlap(Projection _other)
        {
            if (overlaps(_other))
            {
                return Math.Min(Max, _other.Max) - Math.Max(Min, _other.Min);
            }

            return -1;
        }

        readonly float Min;
        readonly float Max;
    }
}