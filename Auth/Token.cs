namespace MrX.Web.Auth;

public class Token
{
    public static Token32 Generate()
    {
        return new Token32();
    }
    public static bool ValidToken(Guid Token)
    {
        byte[] b = Token.ToByteArray();
        Token32 t = new(b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7]);
        return (t.ToString() == Token.ToString());
    }
    public struct Token32
    {

        public Token32(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7, byte c8)
        {
            C1 = c1;
            C2 = c2;
            C3 = c3;
            C4 = c4;
            C5 = c5;
            C6 = c6;
            C7 = c7;
            C8 = c8;
        }
        public Token32()
        {
            C1 = (byte)System.Random.Shared.Next(1, 256);
            C2 = (byte)System.Random.Shared.Next(1, 256 - C1);
            C3 = (byte)System.Random.Shared.Next(1, 256);
            C4 = (byte)System.Random.Shared.Next(1, 256 - C3);
            C5 = (byte)System.Random.Shared.Next(1, 256);
            C6 = (byte)System.Random.Shared.Next(1, 256 - C5);
            C7 = (byte)System.Random.Shared.Next(1, 256);
            C8 = (byte)System.Random.Shared.Next(1, 256 - C7);
        }

        public byte C1, C2, C3, C4, C5, C6, C7, C8;
        public byte C9 => (byte)((C1 + C2) * C4 / C3);
        public byte C10 => (byte)((C5 + C6) * C8 / C7);
        public byte C11 => (byte)((C9 * C10) % 256);
        public byte C12 => (byte)(C1 + C2);
        public byte C13 => (byte)(C5 + C6);
        public byte C14 => (byte)(C3 * 256 / C4);
        public byte C15 => (byte)(C7 * 256 / C8);
        public byte C16 => (byte)(DateOnly.FromDateTime(DateTime.UtcNow).DayOfYear % 256);

        public override readonly string ToString()
        {
            return ((Guid)this).ToString();
        }
        public override readonly bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Token32 or Guid) return false;
            if ((Guid)obj == this) return true;
            return false;
        }
        public override readonly int GetHashCode() => ((Guid)this).GetHashCode();
        public static implicit operator Guid(Token32 t) => new([t.C1, t.C2, t.C3, t.C4, t.C5, t.C6, t.C7, t.C8, t.C9, t.C10, t.C11, t.C12, t.C13, t.C14, t.C15, t.C16]);
        public static bool operator ==(Guid s1, Token32 s2) => s1 == (Guid)s2;
        public static bool operator !=(Guid s1, Token32 s2) => s1 != (Guid)s2;

    }
}