using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection.Metadata;
using System.Xml.Linq;

// Abstract class ThreeDShape
abstract class ThreeDShape
{
    // Field for the type of shape
    private string _shapeType;

    // Read-only property for the shape type
    //    This line is an example of a C# expression-bodied property. It's a concise way of writing a read-only property, and it's equivalent to writing a full getter method.

    //Explanation:
    //public string ShapeType: This declares a public property named ShapeType that returns a string.
    //=> (Lambda Operator): It indicates that what follows is the body of the property.
    //_shapeType: This is the backing field for the property, which stores the type of the shape.
    //Functionality:
    //This property provides read-only access to the _shapeType field.
    //When you access ShapeType, it returns the value of _shapeType.
    //Benefits:
    //Conciseness: It's a more concise way of declaring read-only properties compared to traditional getter methods.
    //Readability: Especially useful for properties with a simple getter, making the code more readable.
    //**using Lambda operator:
    /* public string ShapeType => _shapeType;*/  //=> (Lambda Operator): It indicates that what follows is the body of the property.
                                                 //same as below:

    public string ShapeType
    {
        get { return _shapeType; }
    }

    // Constructor
    protected ThreeDShape(string shapeType)
    {
        _shapeType = shapeType;
    }

    // Abstract method to calculate volume
    public abstract double CalculateVolume();

    // Override ToString method
    public override string ToString()
    {
        return $"Shape: {ShapeType}";
    }
}

// Sphere class inheriting from ThreeDShape
class Sphere : ThreeDShape
{
    // Field for radius
    private double _radius;

    // Property for radius with validation
    public double Radius
    {
        get { return _radius; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Radius must be positive.");
            _radius = value;
        }
    }

    // Constructor
    public Sphere(double radius) : base("Sphere")
    {
        Radius = radius;
    }

    // Override CalculateVolume method
    public override double CalculateVolume()
    {
        return (4.0 / 3.0) * Math.PI * Math.Pow(Radius, 3);
    //public override double CalculateVolume(): This declares a method named CalculateVolume that returns a double value.
    //It's marked as override because it overrides the abstract method declared in the ThreeDShape class.
    //return (4.0 / 3.0) * Math.PI* Math.Pow(Radius, 3);:
    //This line calculates the volume of the sphere using the formula for the volume of a sphere.
    //(4.0 / 3.0) is the fraction 4/3  
    // , which is a constant factor in the formula for the volume of a sphere.
    //Math.PI represents the mathematical constant π.
    //Math.Pow(Radius, 3) calculates the radius raised to the power of 3, which is the radius cubed.
    }

    // Override ToString method
    public override string ToString()
    {
        return base.ToString() + $", Radius: {Radius}, Volume: {CalculateVolume():F2}";
    }
}


