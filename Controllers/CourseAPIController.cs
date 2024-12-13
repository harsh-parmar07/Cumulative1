using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;



namespace Cumulative_1.Controllers
{
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a collection of all courses available in the system.
        /// </summary>
        /// <example>
        /// Example: 
        /// GET api/Course/ListCourses -> [{"courseId":1,"courseCode":"http5101","teacherId":1,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Web Application Development"},{"courseId":2,"courseCode":"http5102","teacherId":2,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Project Management"},{"courseId":3,"courseCode":"http5103","teacherId":5,"startDate":"2018-09-04","finishDate":"2018-12-14","courseName":"Web Programming"},...]
        /// </example>
        /// <returns>
        /// Returns a collection of course objects.
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
        /// Fetches a specific course from the database based on its ID.
        /// </summary>
        /// <param name="id">An integer representing the course ID.</param>
        /// <example>
        /// Example: 
        /// GET api/Course/FindCourse/7 -> {"courseId":7,"courseCode":"http5202","teacherId":3,"startDate":"2019-01-08","finishDate":"2019-04-27","courseName":"Web Application Development 2"}
        /// </example>
        /// <returns>
        /// Returns the course object matching the provided ID. If no course is found, returns an empty object.
        /// </returns>

        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {
            // Empty Course
            Course SelectedCourse = new Course();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "SELECT * FROM courses WHERE courseid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop Through Each Row of the Result Set
                    while (ResultSet.Read())
                    {
                        // Access Column information by the DB column name as an index
                        SelectedCourse.CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        SelectedCourse.CourseCode = (ResultSet["coursecode"]).ToString();
                        SelectedCourse.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        SelectedCourse.StartDate = Convert.ToDateTime(ResultSet["startdate"]).ToString("yyyy-MM-dd");
                        SelectedCourse.FinishDate = Convert.ToDateTime(ResultSet["finishdate"]).ToString("yyyy-MM-dd");
                        SelectedCourse.CourseName = (ResultSet["coursename"]).ToString();

                    }
                }
            }

            // Return the final list of course names
            return SelectedCourse;
        }

        /// <summary>
        /// Inserts a new course into the database.
        /// </summary>
        /// <param name="CourseData">An object representing the course details.</param>
        /// <example>
        /// Example: 
        /// POST: api/Course/AddCourse  
        /// Headers: Content-Type: application/json  
        /// Request Body:  
        /// {  
        /// "CourseCode": "http 5110",  
        /// "TeacherId": 0,  
        /// "StartDate": "01-15-2019",  
        /// "FinishDate": "04-30-2019",  
        /// "CourseName": "Web Development"  
        /// } -> 25
        /// </example>
        /// <returns>
        /// Returns the ID of the newly added course if successful, or 0 if the operation fails.
        /// </returns>


        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Set the SQL Command
                Command.CommandText = "INSERT INTO courses(coursecode,teacherid,startdate,finishdate,coursename) VALUES(@coursecode,@teacherid,@startdate,@finishdate,@coursename)";

                Command.Parameters.AddWithValue("@coursecode", CourseData.CourseCode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.TeacherId);
                Command.Parameters.AddWithValue("@startdate", CourseData.StartDate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.FinishDate);
                Command.Parameters.AddWithValue("@coursename", CourseData.CourseName);

                Command.ExecuteNonQuery();

                // Send the last inserted id of the data created
                return Convert.ToInt32(Command.LastInsertedId);
            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Removes a course from the database.
        /// </summary>
        /// <param name="CourseId">The ID of the course to be deleted.</param>
        /// <example>
        /// Example:  
        /// DELETE: api/Course/DeleteCourse/{CourseId} -> "The course with the given ID {CourseId} has been removed from the database."
        /// </example>
        /// <returns>
        /// Returns a message:  
        /// - "The course with the given ID {CourseId} has been removed from the database" if the course exists.  
        /// - "The course with the given ID {CourseId} is not found" if the course does not exist.  
        /// </returns>


        [HttpDelete(template: "DeleteCourse/{CourseId}")]
        public string DeleteCourse(int CourseId)
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
                Command.CommandText = "DELETE FROM courses WHERE courseid=@id";
                Command.Parameters.AddWithValue("@id", CourseId);

                RowsAffected = Command.ExecuteNonQuery();
            }
            // Check for the deletion
            if (RowsAffected > 0)
            {
                return $"The course with given id {CourseId} has been removed from the DB";
            }
            else
            {
                return $"The course with given id {CourseId} is not found";
            }

        }

        /// <summary>
        /// Updates an existing course in the database. The data is provided as a Course object, and the request query includes the course ID.
        /// </summary>
        /// <param name="CourseData">An object containing the updated course details.</param>
        /// <param name="CourseId">The primary key of the course to be updated.</param>
        /// <example>
        /// Example:  
        /// PUT: api/Course/UpdateCourse/4  
        /// Headers: Content-Type: application/json  
        /// Request Body:  
        /// {  
        ///	    "CourseCode": "Math 104",  
        ///	    "TeacherId": "1",  
        ///	    "StartDate": "2019-01-15 00:00:00",  
        ///	    "FinishDate": "2019-04-30 00:00:00",  
        ///	    "CourseName": "Statistics"  
        /// } ->  
        /// {  
        ///     "CourseId": 4,  
        ///	    "CourseCode": "Math 104",  
        ///	    "TeacherId": "1",  
        ///	    "StartDate": "2019-01-15 00:00:00",  
        ///	    "FinishDate": "2019-04-30 00:00:00",  
        ///	    "CourseName": "Statistics"  
        /// }
        /// </example>
        /// <returns>
        /// Returns the updated course object.
        /// </returns>


        [HttpPut(template: "UpdateCourse/{CourseId}")]
        public Course UpdateCourse(int CourseId, [FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "UPDATE courses SET coursecode=@coursecode, teacherid=@teacherid, startdate=@startdate, finishdate=@finishdate, coursename=@coursename where courseid=@id";
                Command.Parameters.AddWithValue("@coursecode", CourseData.CourseCode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.TeacherId);
                Command.Parameters.AddWithValue("@startdate", CourseData.StartDate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.FinishDate);
                Command.Parameters.AddWithValue("@coursename", CourseData.CourseName);

                Command.Parameters.AddWithValue("@id", CourseId);

                Command.ExecuteNonQuery();
            }


            return FindCourse(CourseId);
        }

    }
}
