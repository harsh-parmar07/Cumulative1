using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;
using System;
using MySql.Data.MySqlClient;



namespace Cumulative_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {

        // This is a dependancy injection
        private readonly SchoolDbContext _studentcontext;
        public StudentAPIController(SchoolDbContext studentcontext)
        {
            _studentcontext = studentcontext;
        }


        /// <summary>
        /// When we click on Students in Navigation bar on Home page, We are directed to a webpage that lists all students in the database student
        /// </summary>
        /// <example>
        /// GET api/Student/ListStudents -> [{"StudentFname":"Mohit", "StudentLName":"Bansal"},{"StudentFname":"Darshan", "StudentLName":"Malhotra"},.............]
        /// GET api/Student/ListStudents -> [{"StudentFname":"Aakriti", "StudentLName":"Garg"},{"StudentFname":"Himanshi", "StudentLName":"Goyal"},.............]
        /// </example>
        /// <returns>
        /// A list all the students in the database student
        /// </returns>


        [HttpGet]
        [Route(template: "ListStudents")]
        public List<Student> ListStudents()
        {
            // Create an empty list of Students
            List<Student> Students = new List<Student>();

            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _studentcontext.AccessDatabase())
            {

                // Opening the connection
                Connection.Open();


                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // Writing the SQL Query we want to give to database to access information
                Command.CommandText = "select * from students";


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Student using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string S_Number = ResultSet["studentnumber"].ToString();
                        DateTime EnrolmentDate = Convert.ToDateTime(ResultSet["enroldate"]);


                        // Assigning short names for properties of the Student
                        Student EachStudent = new Student()
                        {
                            StudentId = Id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            StudentEnrolmentDate = EnrolmentDate,
                            StudentNumber = S_Number,
                        };


                        // Adding all the values of properties of EachStudent in Students List
                        Students.Add(EachStudent);

                    }
                }
            }


            //Return the final list of Students 
            return Students;
        }


        /// <summary>
        /// When we select one student , it returns information of the selected Student in the database by their ID 
        /// </summary>
        /// <example>
        /// GET api/Student/FindStudent/3 -> {"StudentId":3,"StudentFname":"Sam","StudentLName":"Cooper", .......}
        /// </example>
        /// <returns>
        /// Information about the Student selected
        /// </returns>



        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {

            // Created an object SelectedStudent using Student definition defined as Class in Models
            Student SelectedStudent = new Student();


            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _studentcontext.AccessDatabase())
            {

                // Opening the Connection
                Connection.Open();

                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // @id is replaced with a 'sanitized'(masked) id so that id can be referenced
                // without revealing the actual @id
                Command.CommandText = "select * from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", id);


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Student using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string S_Number = ResultSet["studentnumber"].ToString();
                        DateTime EnrolmentDate = Convert.ToDateTime(ResultSet["enroldate"]);


                        // Accessing the information of the properties of Student and then assigning it to the short names 
                        // created above for all properties of the Student
                        SelectedStudent.StudentId = Id;
                        SelectedStudent.StudentFName = FirstName;
                        SelectedStudent.StudentLName = LastName;
                        SelectedStudent.StudentEnrolmentDate = EnrolmentDate;
                        SelectedStudent.StudentNumber = S_Number;

                    }
                }
            }


            //Return the Information of the SelectedStudent
            return SelectedStudent;
        }
    }


}