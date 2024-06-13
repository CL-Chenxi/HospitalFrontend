//excercise: Delegate

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherDemo
{
    // Define a delegate for encryption methods
    public delegate string CipherDelegate(string plaintext);

    public static class CaesarCipher
    {
        public static string Encrypt(string plaintext)
        {
            char[] buffer = plaintext.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                char letter = buffer[i];
                letter = (char)(letter + 1);
                buffer[i] = letter;
            }
            return new string(buffer);
        }
    }

    public static class ReverseCipher
    {
        public static string Encrypt(string plaintext)
        {
            char[] buffer = plaintext.ToCharArray();
            Array.Reverse(buffer);
            return new string(buffer);
        }
    }


}

