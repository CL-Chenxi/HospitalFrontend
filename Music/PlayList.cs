
using System;
using System.Collections.Generic;
using System.Linq;

namespace music;
// 3. Playlist class
public class Playlist
{
    // Auto-implemented property for playlist name
    public string Name { get; }

    // Field to store music files
    private List<MusicFile> musicFiles;

    // Read-only property for the collection of music files
    public IReadOnlyList<MusicFile> MusicFiles => musicFiles;

    // Constructor to initialize playlist name and create an empty collection of music files
    public Playlist(string name)
    {
        Name = name;
        musicFiles = new List<MusicFile>();
    }

    // Method to add a music track to the playlist
    public void AddTrack(MusicFile track)
    {
        // Check if the playlist already contains a track with the same title and artist
        if (musicFiles.Any(m => m.Title == track.Title && m.Artist == track.Artist))
        {
            throw new ArgumentException("Track with the same title and artist already exists in the playlist.");
        }
        musicFiles.Add(track);
    }

    // Enumerator implementation to iterate over music files in the playlist
    public IEnumerator<MusicFile> GetEnumerator()
    {
        return musicFiles.GetEnumerator();
    }

    // Read-only indexer to retrieve music files of a specified genre
    public IReadOnlyList<MusicFile> this[string genre]
    {
        get
        {
            return musicFiles.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
    //test
    class Test
    {
      
        static void Test_Playlist_AddTrack_DoesNotAddDuplicateTracks()
        {
            var playlist = new Playlist("My Playlist");
            var track1 = new MusicFile("song1.mp3", "Title1", "Artist1", "Genre1");
            var track2 = new MusicFile("song2.mp3", "Title2", "Artist2", "Genre2");

            playlist.AddTrack(track1);
            Console.WriteLine("Track1 added to playlist.");

            try
            {
                playlist.AddTrack(track2);
                Console.WriteLine("Track2 added to playlist.");

                playlist.AddTrack(track1);
                Console.WriteLine("Test failed: ArgumentException not thrown for adding duplicate track.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Test passed: ArgumentException thrown for adding duplicate track.");
            }
        }

        static void Test_Playlist_Indexer_ReturnsMusicFilesByGenre()
        {
            var playlist = new Playlist("My Playlist");
            var track1 = new MusicFile("song1.mp3", "Title1", "Artist1", "Genre1");
            var track2 = new MusicFile("song2.mp3", "Title2", "Artist2", "Genre2");
            var track3 = new MusicFile("song3.mp3", "Title3", "Artist3", "Genre1");

            playlist.AddTrack(track1);
            playlist.AddTrack(track2);
            playlist.AddTrack(track3);

            var genre1Tracks = playlist["Genre1"];
            var genre2Tracks = playlist["Genre2"];

            if (genre1Tracks.Count == 2 && genre2Tracks.Count == 1 &&
                genre1Tracks.Contains(track1) && genre1Tracks.Contains(track3) &&
                genre2Tracks.Contains(track2))
            {
                Console.WriteLine("Test passed: Indexer returns correct music files by genre.");
            }
            else
            {
                Console.WriteLine("Test failed: Indexer does not return correct music files by genre.");
            }
        }
    }
}

    // Playlist class definition
    //public class Playlist
    //{
    //    // Auto-implemented property for playlist name
    //    public string Name { get; }

    //    // Field to store music files
    //    private List<MusicFile> musicFiles;

    //    // Read-only property for the collection of music files
    //    public IReadOnlyList<MusicFile> MusicFiles => musicFiles;

    //    // Constructor to initialize playlist name and create an empty collection of music files
    //    public Playlist(string name)
    //    {
    //        Name = name;
    //        musicFiles = new List<MusicFile>();
    //    }

        // Method to add a music track to the playlist
    //    public void AddTrack(MusicFile track)
    //    {
    //        // Check if the playlist already contains a track with the same title and artist
    //        if (musicFiles.Any(m => m.Title == track.Title && m.Artist == track.Artist))
    //        {
    //            throw new ArgumentException("Track with the same title and artist already exists in the playlist.");
    //        }
    //        musicFiles.Add(track);
    //    }

    //    // Indexer to retrieve music files by genre
    //    public IReadOnlyList<MusicFile> this[string genre] =>
    //        musicFiles.Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
    //}

    // MusicFile class definition (for the sake of completeness)
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

        // Method to validate title, artist, and genre
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

    // MediaFile class definition (for the sake of completeness)
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
