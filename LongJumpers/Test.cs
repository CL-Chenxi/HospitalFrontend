namespace LongJumpers
{
    public class Test
    {
        public static void Main()
        {
            try
            {
                // Creating a new jump instance
                Jump jump1 = new Jump(9.45, new DateTime(2023, 6, 10));
                Console.WriteLine(jump1);

                // Attempt to create an invalid jump (will throw an exception)
                Jump invalidJump = new Jump(12.34, DateTime.Now);



                // Creating a new long jumper instance
                LongJumper jumper1 = new LongJumper("John Doe", "Men's");
                Console.WriteLine(jumper1);


                // Recording jumps
                jumper1.RecordJump(new Jump(8.50, new DateTime(2023, 6, 10)));
                jumper1.RecordJump(new Jump(9.00, new DateTime(2024, 5, 15)));

                // Displaying jumper details
                Console.WriteLine(jumper1);

                // Creating another long jumper instance
                LongJumper jumper2 = new LongJumper("Jane Doe", "Ladies");
                Console.WriteLine(jumper2);

                // Recording jumps
                jumper2.RecordJump(new Jump(7.50, new DateTime(2023, 7, 20)));
                jumper2.RecordJump(new Jump(7.80, new DateTime(2024, 3, 10)));

                // Displaying jumper details
                Console.WriteLine(jumper2);

                // Attempt to record an invalid jump (earlier date)
                jumper2.RecordJump(new Jump(7.90, new DateTime(2022, 12, 25)));
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
