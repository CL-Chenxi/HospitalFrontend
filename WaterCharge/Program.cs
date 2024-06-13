using System;
using System.Buffers.Text;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WaterChargesCalculator
{
    class Program
    {
        static void Main()
        {
            const int freeAllowancePerHousehold = 30000;
            const int freeAllowancePerChild = 21000;
            const double chargePer1000Litres = 4.88;
            const double reducedChargePer1000Litres = 2.44;
            const int baseConsumptionPerPerson = 66000;
            const int additionalConsumptionPerPerson = 21000;

            Console.Write("Enter the number of adults in the household: ");
            int numberOfAdults = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of children in the household: ");
            int numberOfChildren = int.Parse(Console.ReadLine());

            Console.Write("Does the household require waste water treatment? (yes/no): ");
            bool requiresWasteWaterTreatment = Console.ReadLine().ToLower() == "yes";

            // Calculate total consumption
            int totalPeople = numberOfAdults + numberOfChildren;
            int totalConsumption = baseConsumptionPerPerson + (totalPeople - 1) * additionalConsumptionPerPerson;

            // Calculate total free allowance
            int totalFreeAllowance = freeAllowancePerHousehold + numberOfChildren * freeAllowancePerChild;

            // Calculate chargeable consumption
            int chargeableConsumption = totalConsumption - totalFreeAllowance;

            // error handling: Ensure chargeable consumption is not negative
            chargeableConsumption = Math.Max(chargeableConsumption, 0);
            //Math.Max(a, b): This function returns the larger of the two values a and b.
            //To prevent chargeableConsumption from being negative by ensuring that if it is less than 0, it is set to 0.

            // Calculate charges
            //Ternary Operator(? :):

            //This is a shorthand for an if-else statement.It evaluates a condition and returns one of two values based on whether the condition is true or false.
            //Syntax: condition? value_if_true : value_if_false
            //Condition(requiresWasteWaterTreatment):

            //requiresWasteWaterTreatment is a boolean variable indicating whether the household requires waste water treatment.
            //Values:

            //chargePer1000Litres is the charge rate per 1000 litres if waste water treatment is required(€4.88 per 1000 litres).
            //reducedChargePer1000Litres is the reduced charge rate per 1000 litres if waste water treatment is not required(€2.44 per 1000 litres).
            //Result:

            //            If requiresWasteWaterTreatment is true, chargeRate is set to chargePer1000Litres.
            //If requiresWasteWaterTreatment is false, chargeRate is set to reducedChargePer1000Litres.
            //short handed way below:
            //double chargeRate = requiresWasteWaterTreatment ? chargePer1000Litres : reducedChargePer1000Litres;
            //***writing the above line in in statement ***
            double chargeRate;

            if (requiresWasteWaterTreatment)
            {
                chargeRate = chargePer1000Litres;
            }
            else
            {
                chargeRate = reducedChargePer1000Litres;
            }
            double totalCharges = chargeableConsumption / 1000.0 * chargeRate;

            // Output the results
            Console.WriteLine($"Total water consumption: {totalConsumption} litres");
            Console.WriteLine($"Total free allowance: {totalFreeAllowance} litres");
            Console.WriteLine($"Chargeable consumption: {chargeableConsumption} litres");
            Console.WriteLine($"Total annual water charges: €{totalCharges:F2}");

            // Keep console window open
            //Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
