using System.Security.Cryptography;
using System.Text; // only used in the third example

// Step 1: Choose two small prime numbers (p and q)
int p = 11;  // Small prime number
int q = 13;  // Small prime number

// int p = 61;  // Small prime number
// int q = 53;  // Small prime number

// Step 2: Compute n = p * q
int n = p * q;
Console.WriteLine("n (part of public key): " + n);

// Step 3: Compute Euler's Totient Function: φ(n) = (p-1) * (q-1)
int phiN = (p - 1) * (q - 1);
Console.WriteLine("φ(n): " + phiN);

// Step 4: Choose an integer e (public exponent) such that 1 < e < φ(n) and gcd(e, φ(n)) = 1
int e = 7;  // A commonly used small public exponent (should be coprime with φ(n))
// int e = 17;  // A commonly used small public exponent (should be coprime with φ(n))
Console.WriteLine("e (public exponent): " + e);

// Step 5: Compute the private key d, where d * e ≡ 1 (mod φ(n))
int d = ModInverse(e, phiN);
Console.WriteLine("d (private key): " + d);

// Public key is (e, n), private key is (d, n)
Tuple<int, int> publicKey = new Tuple<int, int>(e, n);
Tuple<int, int> privateKey = new Tuple<int, int>(d, n);
Console.WriteLine("Public Key: (" + e + ", " + n + ")");
Console.WriteLine("Private Key: (" + d + ", " + n + ")");


// ----------------------------------------------
// PART 1: encryption and decryption
// ----------------------------------------------

int plaintext = 6;

// Step 2: Encrypt the plaintext with the public key (e, n)
int ciphertext = ModPow(plaintext, publicKey);
Console.WriteLine("Encrypted message: " + ciphertext);

// Step 3: Decrypt the ciphertext with the private key (d, n)
int decryptedMessage = ModPow(ciphertext, privateKey);
Console.WriteLine("Decrypted message: " + decryptedMessage);


// ----------------------------------------------
// PART 2: signing and verifying, using RSA
// ----------------------------------------------
// Step 1: Sign a message using the private key
int m = plaintext;  // Message must be smaller than n

// Sign the message with the private key: signature = (m^d) % n
var signature = ModPow(plaintext, privateKey);
Console.WriteLine("Signature: " + signature);

// Step 2: Verify the signature using the public key
// Verification: verified_message = (signature^e) % n
var verifiedMessage = ModPow(signature, publicKey);
Console.WriteLine("Verified Message: " + verifiedMessage);

// Check if the verified message matches the original message
bool isVerified = verifiedMessage == plaintext;
Console.WriteLine("Is the signature valid? " + isVerified);




// Function to compute modular inverse of e mod φ(n) using the extended Euclidean algorithm
static int ModInverse(int e, int phi)
{
    int t = 0, newT = 1;
    int r = phi, newR = e;

    while (newR != 0)
    {
        int quotient = r / newR;

        // Update t and r
        int tempT = t;
        t = newT;
        newT = tempT - quotient * newT;

        int tempR = r;
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
static int ModPow(int baseValue, Tuple<int, int> values)
{
    int exp = values.Item1;
    int mod = values.Item2;

    int result = 1;
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
