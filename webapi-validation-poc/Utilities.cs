namespace WebApiValidationPoC
{
    public static class Utilities
    {
        // todo: custom validation logic
        public static bool IsValidSIN(int sin)
        {
            if (sin < 0 || sin > 999999998)
            {
                return false;
            }

            var checksum = 0;

            for (var i = 4; i != 0; i--)
            {
                checksum += sin % 10;
                sin /= 10;

                var addend = 2 * (sin % 10);

                if (addend >= 10)
                {
                    addend -= 9;
                }

                checksum += addend;
                sin /= 10;
            }

            return (checksum + sin) % 10 == 0;
        }
        
    }
}
