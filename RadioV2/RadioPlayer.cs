using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioV2
{
    // Class RadioPlayerApp to manage the collection of RadioStation
    public class RadioPlayerApp
    {
        // Collection to store the radio stations
        private List<RadioStation> radioStations;

        // Collection to store the favorite radio stations
        private List<RadioStation> favoriteStations;

        // Read-only property to access favorite radio stations
        public IReadOnlyList<RadioStation> FavoriteStations => favoriteStations.AsReadOnly();

        // Constructor to initialize the collection with given radio stations
        public RadioPlayerApp()
        {
            radioStations = new List<RadioStation>
        {
            new RadioStation("RTE Radio 1", "General", 89),
            new RadioStation("RTE 2FM", "Music", 90),
            new RadioStation("Newstalk", "News", 106),
            new RadioStation("FM 104", "Music", 104.4),
            new RadioStation("98 FM", "Music", 98)
        };
            favoriteStations = new List<RadioStation>();
        }

        // Method to play a radio station by name
        public void PlayStation(string name)
        {
            var station = radioStations.Find(rs => rs.Name == name);
            if (station != null)
            {
                station.Play();
            }
            else
            {
                Console.WriteLine($"Radio station {name} not found.");
            }
        }

        // Method to pause a radio station by name
        public void PauseStation(string name)
        {
            var station = radioStations.Find(rs => rs.Name == name);
            if (station != null)
            {
                station.Pause();
            }
            else
            {
                Console.WriteLine($"Radio station {name} not found.");
            }
        }

        // Method to like a radio station by name (example method to demonstrate additional functionality)
        public void LikeStation(string name)
        {
            var station = radioStations.Find(rs => rs.Name == name);
            if (station != null)
            {
                Console.WriteLine($"You liked {name}.");
            }
            else
            {
                Console.WriteLine($"Radio station {name} not found.");
            }
        }

        // Method to display all radio stations
        public void DisplayStations()
        {
            foreach (var station in radioStations)
            {
                Console.WriteLine(station.ToString());
                Console.WriteLine();
            }
        }

        // Indexer to retrieve station names by genre/LINQ Query 
        public List<string> this[string genre]
        {
            get
            {
                return radioStations
                        .Where(rs => rs.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                        //This filters the radioStations list to include only those stations whose Genre property
                        //matches the specified genre. The StringComparison.OrdinalIgnoreCase parameter ensures
                        //that the comparison is case-insensitive.
                        .OrderBy(rs => rs.FmFrequency)
                        //This sorts the filtered list by the FmFrequency property in ascending order.
                        .Select(rs => rs.Name)
                        //This projects each RadioStation object in the sorted list into its Name property,
                        //effectively creating a collection of station names.
                        .ToList();
                        //This converts the resulting collection of station names to a List<string>.
            }
        }
    }
}
