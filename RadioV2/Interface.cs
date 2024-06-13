using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioV2
{
    public interface IStreamable
    {
        void Play();
        void Pause();
    }

    public class RadioStation : IStreamable
    {
        // Auto-implemented properties
        public string Name { get; set; }
        public string Genre { get; set; }

        // Private field for FM frequency
        private double _fmFrequency;

        // Property with validation for FM frequency
        public double FmFrequency
        {
            get { return _fmFrequency; }
            set
            {
                if (value < 87.5 || value > 108.0) 
                //value < 87.5 || value > 108.0 is a logical expression that combines
                //two individual conditions using the logical OR operator (||).
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "FM frequency must be between 87.5 and 108.0 inclusive.");
                }
                _fmFrequency = value;
            }
        }

        // Constructor
        public RadioStation(string name, string genre, double fmFrequency)
        {
            Name = name;
            Genre = genre;
            FmFrequency = fmFrequency;
        }

        // Implementing IStreamable interface methods
        public void Play()
        {
            Console.WriteLine($"{Name} is now playing.");
        }

        public void Pause()
        {
            Console.WriteLine($"{Name} is paused.");
        }

        // Override ToString method to return formatted string
        public override string ToString()  //The method constructs a formatted string that includes
                                           //the name, genre, and FM frequency of the radio station.
        {
            return $"Radio Station: {Name}\nGenre: {Genre}\nFM Frequency: {FmFrequency} MHz";
        }
    }
}



