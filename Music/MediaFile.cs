using System;
using System.Collections.Generic;
using System.Linq;

namespace music;
// 1. MediaFile class
public class MediaFile
{
    // Read/write property for filename
    public string Filename { get; set; }

    // Constructor with filename parameter
    public MediaFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be blank or null.", nameof(filename));
        }
        Filename = filename;
    }

    // Override ToString method
    public override string ToString()
    {
        return $"Filename: {Filename}";
    }


    //test
    class Test
    {
      
        static void Test_MediaFile_InvalidFilename_ThrowsArgumentException()
        {
            try
            {
                var mediaFile1 = new MediaFile(null);
                Console.WriteLine("Test failed: ArgumentNullException not thrown for null filename.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Test passed: ArgumentNullException thrown for null filename.");
            }

            try
            {
                var mediaFile2 = new MediaFile("");
                Console.WriteLine("Test failed: ArgumentException not thrown for empty filename.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Test passed: ArgumentException thrown for empty filename.");
            }

            try
            {
                var mediaFile3 = new MediaFile(" ");
                Console.WriteLine("Test failed: ArgumentException not thrown for whitespace filename.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Test passed: ArgumentException thrown for whitespace filename.");
            }
        }
    }
}

//    // MediaFile class definition
//    public abstract class MediaFile
//    {
//        // Read/write property for filename
//        public string Filename { get; set; }

//        // Constructor with filename parameter
//        public MediaFile(string filename)
//        {
//            if (string.IsNullOrWhiteSpace(filename))
//            {
//                throw new ArgumentException("Filename cannot be blank or null.", nameof(filename));
//            }
//            Filename = filename;
//        }

//        // Override ToString method
//        public override string ToString() => $"Filename: {Filename}";
//    }
//}