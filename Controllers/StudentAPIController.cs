using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        public StudentAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all students in the system.
        /// </summary>
        /// <example>
        /// Example:  
        /// GET api/Student/ListStudents -> [{"studentId":1,"studentFName":"Sarah","studentLName":"Valdez","studentNumber":"N1678","enrolDate":"2018-06-18"},{"studentId":2,"studentFName":"Jennifer","studentLName":"Faulkner","studentNumber":"N1679","enrolDate":"2018-08-02"},{"studentId":3,"studentFName":"Austin","studentLName":"Simon","studentNumber":"N1682","enrolDate":"2018-06-14"},...]
        /// </example>
        /// <returns>
        /// Returns a collection of student objects.
        /// </returns>

        [HttpGet]
        [Route(template: "ListStudents")]
        public List<Student> ListStudents()
        {
            // Create an empty list of Students
            List<Student> Students = new List<Student>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Query
                Command.CommandText = "SELECT * FROM students";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop Through Each Row of the Result Set
                    while (ResultSet.Read())
                    {
                        Student CurrentStudent = new Student();
                        // Access Column information by the DB column name as an index
                        CurrentStudent.StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        CurrentStudent.StudentFName = (ResultSet["studentfname"]).ToString();
                        CurrentStudent.StudentLName = (ResultSet["studentlname"]).ToString();
                        CurrentStudent.StudentNumber = (ResultSet["studentnumber"]).ToString();
                        CurrentStudent.EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]).ToString("yyyy-MM-dd");
                        // Add it to the Students list
                        Students.Add(CurrentStudent);
                    }
                }
            }
            // Return the final list of students
            return Students;
        }


        /// <summary>
        /// Fetches a specific student from the database using their ID.
        /// </summary>
        /// <param name="id">An integer representing the student ID.</param>
        /// <example>
        /// Example:  
        /// GET api/Student/FindStudent/7 -> {"studentId":7,"studentFName":"Jason","studentLName":"Freeman","studentNumber":"N1694","enrolDate":"2018-08-16"}
        /// </example>
        /// <returns>
        /// Returns the student object matching the given ID. If no student is found, returns an empty object.
        /// </returns>

        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            //Empty Student
            Student SelectedStudent = new Student();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Query
                Command.CommandText = "SELECT * FROM students WHERE studentid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop Through Each Row of the Result Set
                    while (ResultSet.Read())
                    {
                        // Access Column information by the DB column name as an index
                        SelectedStudent.StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        SelectedStudent.StudentFName = (ResultSet["studentfname"]).ToString();
                        SelectedStudent.StudentLName = (ResultSet["studentlname"]).ToString();
                        SelectedStudent.StudentNumber = (ResultSet["studentnumber"]).ToString();
                        SelectedStudent.EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]).ToString("yyyy-MM-dd");

                    }
                }
            }
            // Return the final list of student names
            return SelectedStudent;
        }


        /// <summary>
        /// Inserts a new student into the database.
        /// </summary>
        /// <param name="StudentData">An object containing the student's details.</param>
        /// <example>
        /// Example:  
        /// POST: api/Student/AddStudent  
        /// Headers: Content-Type: application/json  
        /// Request Body:  
        /// {  
        /// "StudentFname": "James",  
        /// "StudentLname": "Oliver",  
        /// "StudentNumber": "N1243",  
        /// "EnrolDate": "01-15-2018"  
        /// } -> 25
        /// </example>
        /// <returns>
        /// Returns the ID of the newly added student if successful, or 0 if the operation fails.
        /// </returns>


        [HttpPost(template: "AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "INSERT INTO students (studentfname,studentlname,studentnumber,enroldate) VALUES (@studentfname,@studentlname,@studentnumber,@enroldate)";

                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrolDate);

                Command.ExecuteNonQuery();

                // Send the last inserted id of the data created
                return Convert.ToInt32(Command.LastInsertedId);

            }

            // if failure
            return 0;
        }

        /// <summary>
        /// Removes a student from the database.
        /// </summary>
        /// <param name="StudentId">The ID of the student to be deleted.</param>
        /// <example>
        /// Example:  
        /// DELETE: api/Student/DeleteStudent/{StudentId} -> "The student with the given ID {StudentId} has been removed from the database."
        /// </example>
        /// <returns>
        /// Returns a message:  
        /// - "The student with the given ID {StudentId} has been removed from the database" if the student exists.  
        /// - "The student with the given ID {StudentId} is not found" if the student does not exist.  
        /// </returns>


        [HttpDelete(template: "DeleteStudent/{StudentId}")]
        public string DeleteStudent(int StudentId)
        {
            // initialize the variable to track the rows affected
            int RowsAffected = 0;

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "DELETE FROM students WHERE studentid=@id";

                Command.Parameters.AddWithValue("@id", StudentId);

                RowsAffected = Command.ExecuteNonQuery();

            }
            // Check for the deletion
            if (RowsAffected > 0)
            {
                return $"The student with given id {StudentId} has been removed from the DB";
            }
            else
            {
                return $"The student with given id {StudentId} is not found";
            }
        }

        /// <summary>
        /// Updates an existing student in the database. The student data is provided as an object, and the request query includes the student ID.
        /// </summary>
        /// <param name="StudentData">An object containing the updated student details.</param>
        /// <param name="StudentId">The primary key of the student to be updated.</param>
        /// <example>
        /// Example:  
        /// PUT: api/Student/UpdateStudent/4  
        /// Headers: Content-Type: application/json  
        /// Request Body:  
        /// {  
        ///	    "StudentFname": "Alice",  
        ///	    "StudentLname": "Johnson",  
        ///	    "StudentNumber": "T222",  
        ///	    "EnrolDate": "2024-11-03 00:00:00"  
        /// } ->  
        /// {  
        ///     "StudentId": 4,  
        ///	    "StudentFname": "Alice",  
        ///	    "StudentLname": "Johnson",  
        ///	    "StudentNumber": "T222",  
        ///	    "EnrolDate": "2024-11-03 00:00:00"  
        /// }
        /// </example>
        /// <returns>
        /// Returns the updated student object.
        /// </returns>

        [HttpPut(template: "UpdateStudent/{StudentId}")]
        public Student UpdateStudent(int StudentId, [FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "UPDATE students SET studentfname=@studentfname, studentlname=@studentlname, studentnumber=@studentnumber, enroldate=@enroldate where studentid=@id";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrolDate);

                Command.Parameters.AddWithValue("@id", StudentId);

                Command.ExecuteNonQuery();
            }


            return FindStudent(StudentId);
        }

    }
}
