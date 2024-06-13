using System;
using System.Collections.Generic;
namespace Students
{


    public class Student
    {
        public int ID { get; set; }  //Id is properties that represent the attributes of a student. Here's a detailed explanation of each:


        public string Name { get; set; }
        public string Gender { get; set; }

        public Student() { }

        public Student(int id, string name, string gender) //he parameters id, name, and gender are arguments
                                                           //that you pass when creating a new instance of
                                                           //the Student class. These parameters are used to
                                                           //initialize the properties ID, Name, and Gender of the
                                                           //Student class. 
        {
            ID = id;
            Name = name;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Gender: {Gender}";
        }
    }

    public class StudentClass
    {
        public int CRN { get; set; }
        public string LecturerName { get; set; }
        private List<Student> students;

        public StudentClass(int crn, string lecturerName)
        {
            CRN = crn;
            LecturerName = lecturerName;
            students = new List<Student>();
        }

        public void AddStudent(Student student)  //method definition within a class.
                                                 //Return Type: void (meaning this method does not return a value)
                                                 //Parameter:
                                                 //Student student: This parameter accepts an object of type Student.
                                                 //Student is the name of the class.indicates that the method AddStudent
                                                 //expects an argument of type Student.
                                                 //student is the name of the parameter
                                                 //This parameter represents an instance of the Student class
                                                 //that is passed to the AddStudent method when it is called.
        {
            if (students.Exists(s => s.ID == student.ID))  //Exists is a method
            {
                throw new InvalidOperationException("Student with the same ID already exists in the class.");
            }
            students.Add(student);
        }

        public Student this[int index] //When used in the context of an indexer, this allows you to define
                                       //how an instance of the class can be indexed.
                                       //It essentially overloads the indexing operator [] to allow custom indexing logic.
        {
            get
            {
                if (index < 0 || index >= students.Count)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                return students[index];
            }
        }

        public Student this[string id]
        {
            get
            {
                if (!int.TryParse(id, out int studentId))
                //The TryParse method in C# is a useful and safe way to convert a string representation 
                //of a value to its corresponding type. Unlike the Parse method, TryParse does not throw
                //an exception if the conversion fails. Instead, it returns a boolean value indicating
                //whether the conversion was successful or not.
                {
                    throw new ArgumentException("Invalid student ID format.");
                }
                var student = students.Find(s => s.ID == studentId);
                if (student == null)
                {
                    throw new KeyNotFoundException("No student found with the specified ID.");
                }
                return student;
            }
        }
    }
}


public class Student
{
    // Class members like fields, properties, methods, etc., go here.
}

//public void AddStudent(Student student)
//{
//    // Method logic goes here.
//}
