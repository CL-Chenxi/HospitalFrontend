using System;
using System.Collections.Generic;
using System.Linq;

// 2. MusicFile class inheriting from MediaFile

namespace music;
public class MusicFile : MediaFile
{
    // Additional properties for a music file
    public string Title { get; }
    public string Artist { get; }
    public string Genre { get; }

    // Constructor to initialize all properties
    public MusicFile(string filename, string title, string artist, string genre) : base(filename)
    {
        ValidateProperties(title, artist, genre);
        Title = title;
        Artist = artist;
        Genre = genre;
    }

    // Constructor to initialize title and artist with default genre "other"
    public MusicFile(string filename, string title, string artist) : base(filename)
    {
        ValidateProperties(title, artist, "other");
        Title = title;
        Artist = artist;
        Genre = "other";
    }

    // Method to validate title, artist, and genre
    private void ValidateProperties(string title, string artist, string genre)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be blank or null.", nameof(title));
        }
        if (string.IsNullOrWhiteSpace(artist))
        {
            throw new ArgumentException("Artist cannot be blank or null.", nameof(artist));
        }
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new ArgumentException("Genre cannot be blank or null.", nameof(genre));
        }
    }

    // Override ToString method
    public override string ToString()
    {
        return $"Music File: {Title} - {Artist} ({Genre})";
    }
    //test



        static void Test_MusicFile_InvalidTitleOrArtist_ThrowsArgumentException()
        {
            try
            {
                var musicFile1 = new MusicFile("song.mp3", null, "Artist", "Genre");
                Console.WriteLine("Test failed: ArgumentNullException not thrown for null title.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Test passed: ArgumentNullException thrown for null title.");
            }

            try
            {
                var musicFile2 = new MusicFile("song.mp3", "Title", "", "Genre");
                Console.WriteLine("Test failed: ArgumentException not thrown for empty artist.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Test passed: ArgumentException thrown for empty artist.");
            }
        }
    }

    // MusicFile class definition
    //public class MusicFile : MediaFile
    //{
    //    // Additional properties for a music file
    //    public string Title { get; }
    //    public string Artist { get; }
    //    public string Genre { get; }

    //    // Constructor to initialize all properties
    //    public MusicFile(string filename, string title, string artist, string genre) : base(filename)
    //    {
    //        ValidateProperties(title, artist, genre);
    //        Title = title;
    //        Artist = artist;
    //        Genre = genre;
    //    }

    //    // Constructor to initialize title and artist with default genre "other"
    //    public MusicFile(string filename, string title, string artist) : base(filename)
    //    {
    //        ValidateProperties(title, artist, "other");
    //        Title = title;
    //        Artist = artist;
    //        Genre = "other";
    //    }

    //    // Method to validate title, artist, and genre
    //    private void ValidateProperties(string title, string artist, string genre)
    //    {
    //        if (string.IsNullOrWhiteSpace(title))
    //        {
    //            throw new ArgumentException("Title cannot be blank or null.", nameof(title));
    //        }
    //        if (string.IsNullOrWhiteSpace(artist))
    //        {
    //            throw new ArgumentException("Artist cannot be blank or null.", nameof(artist));
    //        }
    //        if (string.IsNullOrWhiteSpace(genre))
    //        {
    //            throw new ArgumentException("Genre cannot be blank or null.", nameof(genre));
    //        }
    //    }

    //    // Override ToString method
    //    public override string ToString() => $"Music File: {Title} - {Artist} ({Genre})";
    //}

    //// MediaFile class definition (for the sake of completeness)
    //public abstract class MediaFile
    //{
    //    // Read/write property for filename
    //    public string Filename { get; set; }

    //    // Constructor with filename parameter
    //    public MediaFile(string filename)
    //    {
    //        if (string.IsNullOrWhiteSpace(filename))
    //        {
    //            throw new ArgumentException("Filename cannot be blank or null.", nameof(filename));
    //        }
    //        Filename = filename;
    //    }

    //    // Override ToString method
    //    public override string ToString() => $"Filename: {Filename}";
    //}
    







