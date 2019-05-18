namespace SorterControls.ViewModel.Genome
{
    public static class ValidationEx
    {
        public static bool IsAValidLowKey(this int? keyVal, int? hiKeyVal, int keyCount)
        {
            if (! keyVal.HasValue)
            {
                return false;
            }

            if (keyVal.Value < 0)
            {
                return false;
            }

            if (keyVal.Value >= keyCount)
            {
                return false;
            }

            if (hiKeyVal.HasValue)
            {
                if (keyVal.Value >= hiKeyVal.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsAValidHiKey(this int? keyVal, int? lowKeyVal, int keyCount)
        {
            if (!keyVal.HasValue)
            {
                return false;
            }

            if (keyVal.Value < 0)
            {
                return false;
            }

            if (keyVal.Value >= keyCount)
            {
                return false;
            }

            if (lowKeyVal.HasValue)
            {
                if (keyVal.Value <= lowKeyVal.Value)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
