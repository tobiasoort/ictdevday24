// Method to generate the HMAC signature
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

// Sample JSON message
var jsonMessage = """
    {
        "userId": 123,
        "message": "Hello, HMAC!"
    }
    """;

    // Convert JSON object to string
    Console.WriteLine("Original JSON Message: " + jsonMessage);

    // Secret key (shared between sender and receiver)
    string secretKey = "my_super_secret_key";

    // Sign the JSON message using HMAC-SHA256
    string hmacSignature = GenerateHmacSignature(jsonMessage, secretKey);
    Console.WriteLine("Generated HMAC Signature: " + hmacSignature);
    
    // Verify the signature (optional)
    bool isSignatureValid = VerifyHmacSignature(jsonMessage, secretKey, hmacSignature);
    Console.WriteLine("Is Signature Valid: " + isSignatureValid);
    
static string GenerateHmacSignature(string message, string secretKey)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
    byte[] hashBytes = hmac.ComputeHash(messageBytes);
    return Convert.ToBase64String(hashBytes); // Return Base64-encoded HMAC
}

// Method to verify the HMAC signature (optional)
static bool VerifyHmacSignature(string message, string secretKey, string providedSignature)
{
    string generatedSignature = GenerateHmacSignature(message, secretKey);
    return providedSignature == generatedSignature;
}