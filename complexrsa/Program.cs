using System.Numerics;
using System.Security.Cryptography;
using System.Text; 

// // Step 1: Choose two small prime numbers (p and q)
BigInteger p = 50527;  // Small prime number
BigInteger q = 60013;  // Small prime number

// Step 2: Compute n = p * q
BigInteger n = p * q;
Console.WriteLine("n (part of public key): " + n);

// Step 3: Compute Euler's Totient Function: φ(n) = (p-1) * (q-1)
BigInteger phiN = (p - 1) * (q - 1);
Console.WriteLine("φ(n): " + phiN);

// Step 4: Choose an integer e (public exponent) such that 1 < e < φ(n) and gcd(e, φ(n)) = 1
BigInteger e = 65537;  // A commonly used small public exponent (should be coprime with φ(n))
Console.WriteLine("e (public exponent): " + e);

// Step 5: Compute the private key d, where d * e ≡ 1 (mod φ(n))
BigInteger d = ModInverse(e, phiN);
Console.WriteLine("d (private key): " + d);

// Public key is (e, n), private key is (d, n)
Tuple<BigInteger, BigInteger> publicKey = new Tuple<BigInteger, BigInteger>(e, n);
Tuple<BigInteger, BigInteger> privateKey = new Tuple<BigInteger, BigInteger>(d, n);
Console.WriteLine("Public Key: (" + e + ", " + n + ")");
Console.WriteLine("Private Key: (" + d + ", " + n + ")");


// ----------------------------------------------
// Sign with a hash intermediate
// ----------------------------------------------
// 
string plaintextString = "Hello, Developer Days! This is a test message. 🔥";

// generate sha256 hash of the plaintext
using SHA256 sha256 = SHA256.Create();
byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plaintextString));
int hash = BitConverter.ToInt32(hashBytes, 0);
Console.WriteLine("Hash of the message: " + hash);

// Sign the hash with the private key: signature = (m^d) % n
var signature = ModPow(hash, privateKey);
Console.WriteLine("Signature: " + signature);

// Step 2: Verify the signature using the public key
// Verification: verified_message = (signature^e) % n
var verifiedMessage = ModPow(signature, publicKey);
Console.WriteLine("Verified Message: " + verifiedMessage);

// Check if the verified message matches the original message
// generate sha256 hash of the plaintext
using SHA256 secondsha256 = SHA256.Create();
byte[] secondHashBytes = secondsha256.ComputeHash(Encoding.UTF8.GetBytes(plaintextString));
int secondhash = BitConverter.ToInt32(secondHashBytes, 0);
bool isVerified = verifiedMessage == secondhash;
if (isVerified)
{
    Console.WriteLine($"The signature is valid. The message was \"{ plaintextString }\".");
}
else
{
    Console.WriteLine("The signature is invalid.");
}


// Function to compute modular inverse of e mod φ(n) using the extended Euclidean algorithm
static BigInteger ModInverse(BigInteger e, BigInteger phi)
{
    BigInteger t = 0, newT = 1;
    BigInteger r = phi, newR = e;

    while (newR != 0)
    {
        BigInteger quotient = r / newR;

        // Update t and r
        BigInteger tempT = t;
        t = newT;
        newT = tempT - quotient * newT;

        BigInteger tempR = r;
        r = newR;
        newR = tempR - quotient * newR;
    }

    if (r > 1)
    {
        throw new Exception("e and φ(n) are not coprime");
    }

    if (t < 0)
    {
        t = t + phi;
    }

    return t;
}

// Function to perform modular exponentiation: (base^exp) % mod
static BigInteger ModPow(BigInteger baseValue, Tuple<BigInteger, BigInteger> values)
{
    BigInteger exp = values.Item1;
    BigInteger mod = values.Item2;

    BigInteger result = 1;
    baseValue = baseValue % mod;

    while (exp > 0)
    {
        if ((exp % 2) == 1) // If exp is odd, multiply base with result
        {
            result = (result * baseValue) % mod;
        }
        exp = exp >> 1;  // Divide exp by 2
        baseValue = (baseValue * baseValue) % mod; // Square the base
    }
    return result;
}
