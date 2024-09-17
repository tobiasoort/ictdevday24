// See https://aka.ms/new-console-template for more information

MD5Hasher md5 = new MD5Hasher();
string hash = md5.ComputeHash(File.ReadAllBytes("input.txt"));
Console.WriteLine($"MD5 Hash: {hash}");

class MD5Hasher
{
    private uint[] T = new uint[64];  // Lookup table for the MD5 transformation
    private uint A, B, C, D;          // MD5 state

    public MD5Hasher()
    {
        // Initialize the T table for MD5
        for (int i = 0; i < 64; i++)
        {
            T[i] = (uint)(4294967296 * Math.Abs(Math.Sin(i + 1)));
        }
        
        // Initialize MD5 state variables
        A = 0x67452301;
        B = 0xEFCDAB89;
        C = 0x98BADCFE;
        D = 0x10325476;
    }

    public string ComputeHash(byte[] message)
    {
        byte[] paddedMessage = PadMessage(message);
        Console.WriteLine($"Padded message: {BitConverter.ToString(paddedMessage).Replace("-", "")}");
        ProcessMessageBlocks(paddedMessage);

        return $"{ToHex(A, "A")}{ToHex(B, "B")}{ToHex(C, "C")}{ToHex(D, "D")}";
    }

    private byte[] PadMessage(byte[] message)
    {
        // Add padding (message + 1 + zeros + length)
        int originalLength = message.Length;
        int paddedLength = ((originalLength + 8) / 64 + 1) * 64;

        byte[] paddedMessage = new byte[paddedLength];
        Buffer.BlockCopy(message, 0, paddedMessage, 0, originalLength);

        paddedMessage[originalLength] = 0x80; // Append '1' bit followed by zeros
        ulong bitLength = (ulong)originalLength * 8;
        Buffer.BlockCopy(BitConverter.GetBytes(bitLength), 0, paddedMessage, paddedLength - 8, 8);
        return paddedMessage;
    }

    private void ProcessMessageBlocks(byte[] paddedMessage)
    {
        for (int i = 0; i < paddedMessage.Length / 64; i++)
        {
            Console.WriteLine($"Processing block {i}");
            // Break 512-bit block into sixteen 32-bit words
            uint[] X = new uint[16];
            for (int j = 0; j < 16; j++)
            {
                Console.WriteLine($"Processing word {j}");
                X[j] = BitConverter.ToUInt32(paddedMessage, i * 64 + j * 4);
            }

            // Save current state
            uint AA = A, BB = B, CC = C, DD = D;

            // Main MD5 transformation function (4 rounds of 16 operations each)
            for (int k = 0; k < 64; k++)
            {
                Console.WriteLine($"Processing round {k}");
                uint F, g;
                if (k < 16)
                {
                    F = (B & C) | (~B & D);
                    g = (uint)k;
                }
                else if (k < 32)
                {
                    F = (D & B) | (~D & C);
                    g = (uint)((5 * k + 1) % 16);
                }
                else if (k < 48)
                {
                    F = B ^ C ^ D;
                    g = (uint)(3 * k + 5) % 16;
                }
                else
                {
                    F = C ^ (B | ~D);
                    g = (uint)(7 * k) % 16;
                }

                uint temp = D;
                D = C;
                C = B;
                B = B + RotateLeft(A + F + X[g] + T[k], ShiftAmount(k));
                A = temp;
            }

            // Add this chunk's hash to result so far
            A += AA;
            B += BB;
            C += CC;
            D += DD;
        }
    }

    private uint RotateLeft(uint x, int n)
    {
        return (x << n) | (x >> (32 - n));
    }

    private int ShiftAmount(int i)
    {
        // MD5's specific shift amounts for each round
        if (i < 16) return new[] { 7, 12, 17, 22 }[i % 4];
        if (i < 32) return new[] { 5, 9, 14, 20 }[i % 4];
        if (i < 48) return new[] { 4, 11, 16, 23 }[i % 4];
        return new[] { 6, 10, 15, 21 }[i % 4];
    }

    private string ToHex(uint value, string blockname = "")
    {
        var result = BitConverter.ToString(BitConverter.GetBytes(value)).Replace("-", "").ToLowerInvariant();
        Console.WriteLine($"Block {blockname}: Value: {value}, Bytes: {BitConverter.ToString(BitConverter.GetBytes(value)).Replace("-", "")}, Hex: {result}");
        return result;
    }
}