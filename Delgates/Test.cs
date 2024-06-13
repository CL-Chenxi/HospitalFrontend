
using global::CipherDemo;
using System;

namespace CipherDemo
{
    public class Test
    {
        static void Main(string[] args)
        {
            // Define a plaintext message
            string plaintext = "Hello, World!";

            // Create delegate instances for each cipher
            CipherDelegate caesarCipher = new CipherDelegate(CaesarCipher.Encrypt);
            CipherDelegate reverseCipher = new CipherDelegate(ReverseCipher.Encrypt);

            // Use the Caesar cipher
            Console.WriteLine("Using Caesar Cipher:");
            string caesarCiphertext = caesarCipher(plaintext);
            Console.WriteLine($"Plaintext: {plaintext}");
            Console.WriteLine($"Ciphertext: {caesarCiphertext}");
            Console.WriteLine();

            // Use the Reverse cipher
            Console.WriteLine("Using Reverse Cipher:");
            string reverseCiphertext = reverseCipher(plaintext);
            Console.WriteLine($"Plaintext: {plaintext}");
            Console.WriteLine($"Ciphertext: {reverseCiphertext}");
        }
    }
}


