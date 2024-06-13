using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Interface
{
    public interface IHasVolume
    {
        double CalculateVolume();
    }


public class Sphere : IHasVolume
    {
        public double Radius { get; set; }

        public Sphere(double radius)
        {
            Radius = radius;
        }

        public double CalculateVolume()
        {
            // Volume of a sphere formula: V = 4/3 * π * r^3
            return (4.0 / 3.0) * Math.PI * Math.Pow(Radius, 3);
        }

        public override string ToString()
        {
            return $"Sphere with Radius: {Radius}, Volume: { CalculateVolume()}";
        }
    }

    public class Test
    {
        static void Main()
        {
            // Create a Sphere object
            IHasVolume sphere = new Sphere(5);

            // Display details of the sphere
            Console.WriteLine(sphere.ToString());

            // Calculate and display the volume polymorphically
            double volume = sphere.CalculateVolume();
            Console.WriteLine($"Calculated Volume: {volume} cubic units");

            // Create a collection of spheres
            var spheres = new List<Sphere>
        {
            new Sphere(3),
            new Sphere(4),
            new Sphere(5)
        };

            // Call various methods on the collection
            foreach (var s in spheres)
            {
                Console.WriteLine(s.ToString());
            }
        }
    }
}


//Interface Definition
//public interface IHasVolume :
//public: This keyword specifies that the interface is accessible from other classes and assemblies.
//interface: This keyword declares a new interface. An interface is a reference type in C# that defines a contract for classes or structs that implement it. It can contain method signatures, properties, events, and indexers but no implementation.
//IHasVolume: This is the name of the interface. By convention, interface names in C# start with the letter 'I'.