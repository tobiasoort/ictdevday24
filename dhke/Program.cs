// Pre-agreed public values (in practice, these would be large and well-chosen primes)
int p = 23;  // Prime modulus
int g = 5;   // Generator

Console.WriteLine("Publicly shared prime (p): " + p);
Console.WriteLine("Publicly shared generator (g): " + g);

// Simulate Alice's key pair
int alicePrivate = 6;  // Alice's private key (should be kept secret)
int alicePublic = ModExp(g, alicePrivate, p);  // Alice's public key

// Simulate Bob's key pair
int bobPrivate = 15;  // Bob's private key (should be kept secret)
int bobPublic = ModExp(g, bobPrivate, p);  // Bob's public key

Console.WriteLine("Alice's Public Key: " + alicePublic);
Console.WriteLine("Bob's Public Key: " + bobPublic);

// Alice and Bob exchange public keys and compute the shared secret
int aliceSharedSecret = ModExp(bobPublic, alicePrivate, p);  // Alice computes shared secret
int bobSharedSecret = ModExp(alicePublic, bobPrivate, p);    // Bob computes shared secret

Console.WriteLine("Alice's computed shared secret: " + aliceSharedSecret);
Console.WriteLine("Bob's computed shared secret: " + bobSharedSecret);

// If the protocol is followed correctly, both secrets should match
if (aliceSharedSecret == bobSharedSecret)
{
    Console.WriteLine("Shared secret established successfully!");
}
else
{
    Console.WriteLine("Error: Shared secrets do not match.");
}

static int ModExp(int baseValue, int exponent, int modulus)
{
    int result = 1;
    baseValue = baseValue % modulus;

    while (exponent > 0)
    {
        if ((exponent & 1) == 1)  // If exponent is odd
            result = (result * baseValue) % modulus;

        exponent = exponent >> 1;  // Divide exponent by 2
        baseValue = (baseValue * baseValue) % modulus;
    }
    return result;
}