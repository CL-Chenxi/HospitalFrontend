using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongJumpers
{
    public class Jump
    {
        // Private field for the length of the jump
        private double length;

        // Property for the length of the jump with validation
        public double Length
        {
            get => length;
            private set
            {
                if (value <= 0 || value > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), 
                        "Jump length must be greater than 0 and less than or equal to 10 metres.");
                }
                length = value;
            }
        }

        // Auto-implemented property for the date of the jump
        public DateTime Date { get; }

        // Constructor to initialize the Jump object
        public Jump(double length, DateTime date)
        {
            Length = length;
            Date = date;
        }

        // Override ToString method
        public override string ToString()
        {
            return $"Jump Length: {Length:F2} metres, Date: {Date:dd/MM/yyyy}";
        }
    }


}

