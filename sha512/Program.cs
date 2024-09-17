using System;
using System.Security.Cryptography;
using System.Text;


string password = "mySecurePassword123";
Console.WriteLine("Password: " + password);
string hashedPassword = PasswordHasher.HashPassword(password);

Console.WriteLine("Hashed Password: " + hashedPassword);

bool isValid = PasswordHasher.ValidatePassword(password, hashedPassword);
Console.WriteLine("Password is valid: " + isValid);

public class PasswordHasher
{
    private const int SaltSize = 16;  // 128-bit salt
    private const int HashSize = 64;  // 512-bit hash
    private const int Iterations = 10000;  // Number of iterations for PBKDF2

    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        // Hash the password with PBKDF2 using SHA-512
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Combine the salt and hash and return it as a base64 string, with XX in the middle
        byte[] hashBytes = new byte[SaltSize + HashSize + 2];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize + 2, HashSize);
        hashBytes[SaltSize] = 88;  // ASCII value for 'X'
        hashBytes[SaltSize+1] = 88;  // ASCII value for 'X'
        return Convert.ToBase64String(hashBytes);
    }

    public static bool ValidatePassword(string password, string storedHash)
    {
        // Extract the salt and hash from the stored password hash
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Hash the input password with the same salt and compare to stored hash
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Compare the stored hash with the hash generated from input password
        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize + 2] != hash[i])
            {
                return false;  // Passwords do not match
            }
        }

        return true;  // Passwords match
    }

}