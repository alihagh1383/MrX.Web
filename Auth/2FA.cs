
namespace MrX.Web.Auth
{
    public class _2FA
    {
        private const int StepSeconds = 30; // هر 30 ثانیه کد تغییر می‌کند
        private const int Digits = 6; // طول کد (معمولاً 6 رقم است)
        public static string GenerateCode(string secretKey, int StepSeconds = StepSeconds, int Digits = Digits, long ToBack = 0)
        {
            byte[] bytes = Base32Encoding.ToBytes(secretKey);

            Totp totp = new(bytes, StepSeconds, Digits);

            string result = totp.ComputeTotp(ToBack);

            return result;
        }
        public static bool ValidateCode(string secretKey, string code, int StepSeconds = StepSeconds, int Digits = Digits)
        {
            return GenerateCode(secretKey, StepSeconds, Digits) == code;
        }
    }
    public class Totp(byte[] secretKey, int step = 30, int totpSize = 6)
    {
        const long UnixEpochTicks = 621355968000000000L;

        const long ticksToSeconds = 10000000L;

        public string ComputeTotp(long ToBack = 0)
        {
            long window = CalculateTimeStepFromTimestamp(DateTime.UtcNow) - ToBack;

            byte[] data = GetBigEndianBytes(window);

            System.Security.Cryptography.HMACSHA1 hmac = new()
            {
                Key = secretKey
            };
            byte[] hmacComputedHash = hmac.ComputeHash(data);

            int offset = hmacComputedHash[^1] & 0x0F;
            int otp = (hmacComputedHash[offset] & 0x7f) << 24
                   | (hmacComputedHash[offset + 1] & 0xff) << 16
                   | (hmacComputedHash[offset + 2] & 0xff) << 8
                   | (hmacComputedHash[offset + 3] & 0xff) % 1000000;

            string result = Digits(otp, totpSize);

            return result;
        }

        private static byte[] GetBigEndianBytes(long input)
        {
            // Since .net uses little endian numbers, we need to reverse the byte order to get big endian.
            byte[] data = BitConverter.GetBytes(input);
            Array.Reverse(data);
            return data;
        }

        private long CalculateTimeStepFromTimestamp(DateTime timestamp)
        {
            long UnixTimestamp = (timestamp.Ticks - UnixEpochTicks) / ticksToSeconds;
            long window = UnixTimestamp / (long)step;
            return window;
        }

        private static string Digits(long input, int digitCount)
        {
            int truncatedValue = ((int)input % (int)Math.Pow(10, digitCount));
            return truncatedValue.ToString().PadLeft(digitCount, '0');
        }

    }
    public static class Base32Encoding
    {
        public static byte[] ToBytes(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.TrimEnd('='); //remove padding characters
            int byteCount = input.Length * 5 / 8; //this must be TRUNCATED
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            int arrayIndex = 0;

            foreach (char c in input)
            {
                int cValue = CharToValue(c);
                int mask;
                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }

        public static string ToString(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException(nameof(input));
            }

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        private static int CharToValue(char c)
        {
            int value = (int)c;

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }
            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }
            //97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }

            throw new ArgumentException("Character is not a Base32 character.", nameof(c));
        }

        private static char ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Byte is not a value Base32 value.", nameof(b));
        }

    }
}
