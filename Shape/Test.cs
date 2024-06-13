using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shape 
{

    // Test the classes
    class Test
    {
        static void Main(string[] args)
        {
            // Test ThreeDShape class (abstract class cannot be instantiated directly)
            // ThreeDShape shape = new ThreeDShape("Shape"); // This line would cause a compilation error

            // Create and test Sphere objects
            Sphere sphere1 = new Sphere(76);
            Sphere sphere2 = new Sphere(22);

            // Display information and calculate volume polymorphically
            Console.WriteLine(sphere1.ToString());
            Console.WriteLine(sphere2.ToString());

            // Create a collection of spheres
            List<Sphere> spheres = new List<Sphere> { sphere1, sphere2 };

            // Call various methods on each sphere
            foreach (var sphere in spheres)
            {
                Console.WriteLine($"Sphere details: {sphere}");
                Console.WriteLine($"Volume of the sphere: {sphere.CalculateVolume():F2}");
            }
        }
    }
}