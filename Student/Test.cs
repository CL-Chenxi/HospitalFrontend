namespace Students
{
    public class Test
    {
        static void Main()
        {
            try
            {
                Student student1 = new Student(1, "Alice", "Female");
                Student student2 = new Student(2, "Bob", "Male");
                Student student3 = new Student(3, "Charlie", "Male");

                StudentClass studentClass = new StudentClass(101, "Dr. Smith");

                studentClass.AddStudent(student1);
                studentClass.AddStudent(student2);
                studentClass.AddStudent(student3);

                Console.WriteLine("Student at index 0: " + studentClass[0]);
                Console.WriteLine("Student with ID 2: " + studentClass["2"]);

                try
                {
                    studentClass.AddStudent(new Student(1, "David", "Male"));
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Exception caught: " + ex.Message);
                }

                try
                {
                    Console.WriteLine("Student at index 5: " + studentClass[5]);
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Exception caught: " + ex.Message);
                }

                try
                {
                    Console.WriteLine("Student with ID 5: " + studentClass["5"]);
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine("Exception caught: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception caught: " + ex.Message);
            }
        }
    }
}

