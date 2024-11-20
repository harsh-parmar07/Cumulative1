using Cumulative_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesAPIController : ControllerBase
    {

        // This is dependancy injection
        private readonly SchoolDbContext _schoolcontext;
        public CoursesAPIController(SchoolDbContext schoolcontext)
        {
            _schoolcontext = schoolcontext;
        }


        /// <summary>
        /// When we click on Courses in Navigation bar on Home page, We are directed to a webpage that lists all courses in the database school
        /// </summary>
        /// <example>
        /// GET api/Course/ListCourses -> [{"Coursename":"Digital Design","Coursename":"Database Development",.............]
        /// <returns>
        /// A list all the courses in the database school
        /// </returns>


        [HttpGet]
        [Route(template: "ListCourses")]
        public List<Course> ListCourses()
        {
            // Create an empty list of Courses
            List<Course> Courses = new List<Course>();

            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {

                // Opening the connection
                Connection.Open();


                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // Writing the SQL Query we want to give to database to access information
                Command.CommandText = "select * from courses";


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Course using the Column name as an index
                        int C_Id = Convert.ToInt32(ResultSet["courseid"]);
                        string Code = ResultSet["coursecode"].ToString();
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]).Date;
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]).Date;
                        string Name = ResultSet["coursename"].ToString();


                        // Assigning short names for properties of the Course
                        Course EachCourse = new Course()
                        {
                            CourseId = C_Id,
                            CourseCode = Code,
                            TeacherId = Id,
                            CourseStartDate = StartDate,
                            CourseFinishDate = FinishDate,
                            CourseName = Name
                        };


                        // Adding all the values of properties of EachCourse in Courses List
                        Courses.Add(EachCourse);

                    }
                }
            }


            //Return the final list of Courses 
            return Courses;
        }


        /// <summary>
        /// When we select one course , it returns information of the selected Course in the database by their ID 
        /// </summary>
        /// <example>
        /// GET api/Course/FindCourse/3 -> {"CourseId":3,"Coursename":Web Development, .......}
        /// </example>
        /// <returns>
        /// Information about the Course selected
        /// </returns>



        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {

            // Created an object SelectedCourse using Course definition defined as Class in Models
            Course SelectedCourse = new Course();


            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {

                // Opening the Connection
                Connection.Open();

                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // @id is replaced with a 'sanitized'(masked) id so that id can be referenced
                // without revealing the actual @id
                Command.CommandText = "select * from courses where courseid = @id;";
                Command.Parameters.AddWithValue("@id", id);


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Course using the Column name as an index
                        int C_Id = Convert.ToInt32(ResultSet["courseid"]);
                        string Code = ResultSet["coursecode"].ToString();
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]).Date;
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]).Date;
                        string Name = ResultSet["coursename"].ToString();


                        // Accessing the information of the properties of Course and then assigning it to the short names 
                        // created above for all properties of the Course
                        SelectedCourse.CourseId = C_Id;
                        SelectedCourse.CourseCode = Code;
                        SelectedCourse.TeacherId = Id;
                        SelectedCourse.CourseStartDate = StartDate;
                        SelectedCourse.CourseFinishDate = FinishDate;
                        SelectedCourse.CourseName = Name;

                    };
                }
            }


            //Return the Information of the SelectedCourse
            return SelectedCourse;
        }
    }


}

