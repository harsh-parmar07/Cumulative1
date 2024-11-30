using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics;


namespace Cumulative_1.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Retrieves a list of all teachers in the system
        /// </summary>
        /// <example>
        /// GET api/Teacher/ListTeachers -> [{"teacherId":1,"teacherFName":"Alexander","teacherLName":"Bennett","employeeNumber":"T378","hireDate":"2016-08-05 00:00:00","salary":55.30,"coursesByTeacher":[{"courseId":1,"courseCode":"http5101","teacherId":1,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Web Application Development"}]},{"teacherId":2,"teacherFName":"Caitlin","teacherLName":"Cummings","employeeNumber":"T381","hireDate":"2014-06-10 00:00:00","salary":62.77,"coursesByTeacher":[{"courseId":2,"courseCode":"http5102","teacherId":2,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Project Management"},{"courseId":6,"courseCode":"http5201","teacherId":2,"startDate":"2019-01-08","finishDate":"2019-04-27","courseName":"Security & Quality Assurance"}]},..]
        /// </example>
        /// <returns>
        /// A collection of teacher objects, each including associated courses
        /// </returns>

        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers()
        {

            // Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set SQL Query
                Command.CommandText = "SELECT * FROM teachers";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop Through Each Row of the Result Set
                    while (ResultSet.Read())
                    {
                        Teacher CurrentTeacher = new Teacher();
                        List<Course> Courses = new List<Course>();
                        // Access Column information by the DB column name as an index
                        CurrentTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        CurrentTeacher.TeacherFName = ResultSet["teacherfname"].ToString();
                        CurrentTeacher.TeacherLName = ResultSet["teacherlname"].ToString();
                        CurrentTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                        CurrentTeacher.HireDate = ResultSet["hiredate"] != DBNull.Value ? Convert.ToDateTime(ResultSet["hiredate"]).ToString("yyyy/MM/dd HH:mm:ss") : "";
                        CurrentTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
                        foreach (Course CourseDetails in ListCourses())
                        {
                            if (CurrentTeacher.TeacherId == CourseDetails.TeacherId)
                            {

                                Courses.Add(CourseDetails);
                            }
                        }
                        CurrentTeacher.CoursesByTeacher = Courses;
                        // Add it to the Teachers list
                        Teachers.Add(CurrentTeacher);
                    }

                }

            }

            // Return the final list of teachers
            return Teachers;
        }


        /// <summary>
        /// Retrieves a list of all courses available in the system
        /// </summary>
        /// <example>
        /// GET api/Course/ListCourses -> [{"courseId":1,"courseCode":"http5101","teacherId":1,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Web Application Development"},{"courseId":2,"courseCode":"http5102","teacherId":2,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Project Management"},{"courseId":3,"courseCode":"http5103","teacherId":5,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Web Programming"},..]
        /// </example>
        /// <returns>
        /// A collection of course objects
        /// </returns>

        [HttpGet]
        [Route(template: "ListCourses")]
        public List<Course> ListCourses()
        {
            // Create an empty list of Courses
            List<Course> Courses = new List<Course>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "SELECT * FROM courses";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop Through Each Row of the Result Set
                    while (ResultSet.Read())
                    {
                        Course CurrentCourse = new Course();
                        // Access Column information by the DB column name as an index
                        CurrentCourse.CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        CurrentCourse.CourseCode = (ResultSet["coursecode"]).ToString();
                        CurrentCourse.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        CurrentCourse.StartDate = Convert.ToDateTime(ResultSet["startdate"]).ToString("yyyy-MM-dd");
                        CurrentCourse.FinishDate = Convert.ToDateTime(ResultSet["finishdate"]).ToString("yyyy-MM-dd");
                        CurrentCourse.CourseName = (ResultSet["coursename"]).ToString();
                        // Add it to the Courses list
                        Courses.Add(CurrentCourse);
                    }
                }
            }

            // Return the final list of courses
            return Courses;
        }



        /// <summary>
        /// Retrieves a teacher from the database by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the teacher (integer)</param>
        /// <example>
        /// GET api/Teacher/FindTeacher/7 -> {"teacherId":7,"teacherFName":"Shannon","teacherLName":"Barton","employeeNumber":"T397","hireDate":"2013-08-04 00:00:00","salary":64.70,"coursesByTeacher":[{"courseId":4,"courseCode":"http5104","teacherId":7,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Digital Design"}]}
        /// </example>
        /// <returns>
        /// A teacher object matching the provided ID. An empty object is returned if the teacher is not found.
        /// </returns>

        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Empty Teacher
            Teacher SelectedTeacher = new Teacher();
            List<Course> Courses = new List<Course>();
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Establish a new command (query) for our database
                Connection.Open();

                // Create command
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "SELECT * FROM teachers WHERE teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop Through Each Row of the Result Set
                    while (ResultSet.Read())
                    {
                        // Access Column information by the DB column name as an index
                        SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        SelectedTeacher.TeacherFName = ResultSet["teacherfname"].ToString();
                        SelectedTeacher.TeacherLName = ResultSet["teacherlname"].ToString();
                        SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                        SelectedTeacher.HireDate = ResultSet["hiredate"] != DBNull.Value ? Convert.ToDateTime(ResultSet["hiredate"]).ToString("yyyy/MM/dd HH:mm:ss") : "";
                        SelectedTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
                        foreach (Course CourseDetails in ListCourses())
                        {
                            if (SelectedTeacher.TeacherId == CourseDetails.TeacherId)
                            {

                                Courses.Add(CourseDetails);
                            }
                        }
                        SelectedTeacher.CoursesByTeacher = Courses;
                    }

                }

            }

            return SelectedTeacher;

        }

        /// <summary>
        /// Adds a new teacher to the database
        /// </summary>
        /// <param name="TeacherData">The teacher object containing details to be added</param>
        /// <example>
        /// POST: api/Teacher/AddTeacher
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        /// "TeacherFname": "Robert",
        /// "TeacherLname": "Smith",  
        /// "EmployeeNumber": "T102",
        /// "HireDate": "2019-09-04",
        /// "Salary": 55.25
        /// } -> 25
        /// </example>
        /// <returns>
        /// The ID of the newly inserted teacher if the operation is successful. Returns 0 if the operation fails.
        /// </returns>


        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] Teacher TeacherData)

        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "INSERT INTO teachers (teacherfname,teacherlname,employeenumber,hiredate,salary) VALUES (@teacherfname,@teacherlname,@employeenumber,@hiredate,@salary)";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeNumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.HireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);


                Command.ExecuteNonQuery();


                // Send the last inserted id of the data created
                return Convert.ToInt32(Command.LastInsertedId);
            }

            // if failure
            return 0;
        }

        /// <summary>
        /// Deletes a teacher from the database
        /// </summary>
        /// <param name="TeacherId">The primary key of the teacher to delete</param>
        /// <example>
        /// DELETE: api/Teacher/DeleteTeacher/{TeacherId}
        /// Response: "The teacher with given id {TeacherId} has been removed from the DB"
        /// </example>
        /// <returns>
        /// Returns a message indicating whether the teacher was successfully deleted:
        /// - "The teacher with given id {teacherId} has been removed from the DB" if found and deleted.
        /// - "The teacher with given id {teacherId} is not found" if the teacher ID is not found in the database.
        /// </returns>


        [HttpDelete(template: "DeleteTeacher/{TeacherId}")]
        public string DeleteTeacher(int TeacherId)
        {
            // initialize the variable to track the rows affected
            int RowsAffected = 0;

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "DELETE FROM teachers WHERE teacherid=@id";
                Command.Parameters.AddWithValue("@id", TeacherId);

                RowsAffected = Command.ExecuteNonQuery();

            }
            // Check for the deletion
            if (RowsAffected > 0)
            {
                return $"The teacher with given id {TeacherId} has been removed from the DB";
            }
            else
            {
                return $"The teacher with given id {TeacherId} is not found";
            }

        }


    }



}
