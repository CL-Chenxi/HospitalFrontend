using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongJumpers
{
    public class LongJumper
    {
        // Auto-implemented properties for Name and Event
        public string Name { get; }
        public string Event { get; }

        // Property for Personal Best
        public double PersonalBest { get; private set; }


        // Collection to store jumps
        private List<Jump> jumps;

        // Constructor to initialize the LongJumper object
        public LongJumper(string name, string @event)
        {
            Name = name;
            Event = @event;
            PersonalBest = 0;
            jumps = new List<Jump>();
        }

        // Default constructor
        public LongJumper() : this("Unknown", "Unknown")
        {
        }

        // Method to record a new jump
        public void RecordJump(Jump newJump)
        {
            if (jumps.Count > 0 && newJump.Date < jumps[jumps.Count - 1].Date)
            {
                throw new InvalidOperationException("Cannot add a jump with a date earlier than the last recorded jump.");
            }

            jumps.Add(newJump);

            if (newJump.Length > PersonalBest)
            {
                PersonalBest = newJump.Length;
            }
        }


        // Override ToString method
        public override string ToString()
        {
            return $"Name: {Name}, Event: {Event}, Personal Best: {PersonalBest:F2} metres";
        }
    }

}
