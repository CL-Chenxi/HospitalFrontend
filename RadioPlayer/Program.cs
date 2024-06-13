using System;
using System.Collections.Generic;
using System.Linq;

// 1. IStreamable interface
public interface IStreamable
{
    void Play();
    void Pause();
}

// 2. RadioStation class implementing IStreamable
public class RadioStation : IStreamable
{
    // Auto-implemented properties
    public string Name { get; }
    public string Genre { get; }
    public double Frequency { get; set; }

    // Constructor
    public RadioStation(string name, string genre, double frequency)
    {
        Name = name;
        Genre = genre;
        Frequency = frequency;
    }

    // Override ToString to provide formatted information
    public override string ToString()
    {
        return $"{Name} ({Genre}) - {Frequency} MHz";
    }

    // Implementing IStreamable interface
    public void Play()
    {
        Console.WriteLine($"Playing {Name}...");
    }

    public void Pause()
    {
        Console.WriteLine($"Pausing {Name}...");
    }
}

// 3. RadioPlayerApp class
public class RadioPlayerApp
{
    // Collection of radio stations
    private List<RadioStation> stations = new List<RadioStation>
    {
        new RadioStation("RTE Radio 1", "General", 89),
        new RadioStation("RTE 2FM", "Music", 90),
        new RadioStation("Newstalk", "News", 106),
        new RadioStation("FM 104", "Music", 104.4),
        new RadioStation("98 FM", "Music", 98)
    };

    // Collection of favorite radio stations
    private List<RadioStation> favoriteStations = new List<RadioStation>();

    // Read-only property to access favorite stations
    public IReadOnlyList<RadioStation> FavoriteStations => favoriteStations;

    // Method to like a station and add it to favorites
    public void LikeStation(string stationName)
    {
        var station = stations.FirstOrDefault(s => s.Name == stationName);
        if (station != null && !favoriteStations.Contains(station))
        {
            favoriteStations.Add(station);
            Console.WriteLine($"Added {stationName} to favorites.");
        }
        else
        {
            Console.WriteLine($"Station '{stationName}' not found or already in favorites.");
        }
    }

    // Indexer to retrieve stations by genre
    public List<string> this[string genre]
    {
        get
        {
            return stations.Where(s => s.Genre == genre)
                           .OrderBy(s => s.Frequency)
                           .Select(s => s.Name)
                           .ToList();
        }
    }
}

// 4. Unit tests
class Program
{
    static void Main(string[] args)
    {
        // Create RadioPlayerApp instance
        var radioApp = new RadioPlayerApp();

        // Test liking a station
        radioApp.LikeStation("RTE Radio 1");
        radioApp.LikeStation("BBC World"); // Non-existent station

        // Test indexing stations by genre
        var musicStations = radioApp["Music"];
        var newsStations = radioApp["News"];

        Console.WriteLine("\nFavorite Stations:");
        foreach (var station in radioApp.FavoriteStations)
        {
            Console.WriteLine(station);
        }

        Console.WriteLine("\nMusic Stations:");
        foreach (var station in musicStations)
        {
            Console.WriteLine(station);
        }

        Console.WriteLine("\nNews Stations:");
        foreach (var station in newsStations)
        {
            Console.WriteLine(station);
        }
    }
}
