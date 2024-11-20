using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;
using System;
using MySql.Data.MySqlClient;



namespace Cumulative_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {

        // This is dependancy injection
        private readonly SchoolDbContext _schoolcontext;
        public TeacherAPIController(SchoolDbContext schoolcontext)
        {
            _schoolcontext = schoolcontext;
        }


        /// <summary>
        /// When we click on Teachers in Navigation bar on Home page, We are directed to a webpage that lists all teachers in the database school
        /// </summary>
        /// <example>
        /// GET api/Teacher/ListTeachers -> [{"TeacherFname":"Manik", "TeacherLName":"Bansal"},{"TeacherFname":"Asha", "TeacherLName":"Bansal"},.............]
        /// GET api/Teacher/ListTeachers -> [{"TeacherFname":"Apurva", "TeacherLName":"Gupta"},{"TeacherFname":"Himani", "TeacherLName":"Garg"},.............]
        /// </example>
        /// <returns>
        /// A list all the teachers in the database school
        /// </returns>


        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers()
        {
            // Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {

                // Opening the connection
                Connection.Open();


                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // Writing the SQL Query we want to give to database to access information
                Command.CommandText = "select * from teachers";


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Teacher using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        // Assigning short names for properties of the Teacher
                        Teacher EachTeacher = new Teacher()
                        {
                            TeacherId = Id,
                            TeacherFName = FirstName,
                            TeacherLName = LastName,
                            TeacherHireDate = TeacherHireDate,
                            EmployeeNumber = EmployeeNumber,
                            TeacherSalary = Salary
                        };


                        // Adding all the values of properties of EachTeacher in Teachers List
                        Teachers.Add(EachTeacher);

                    }
                }
            }


            //Return the final list of Teachers 
            return Teachers;
        }


        /// <summary>
        /// When we select one teacher , it returns information of the selected Teacher in the database by their ID 
        /// </summary>
        /// <example>
        /// GET api/Teacher/FindTeacher/3 -> {"TeacherId":3,"TeacherFname":"Sam","TeacherLName":"Cooper"}
        /// </example>
        /// <returns>
        /// Information about the Teacher selected
        /// </returns>



        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {

            // Created an object SelectedTeacher using Teacher definition defined as Class in Models
            Teacher SelectedTeacher = new Teacher();


            // 'using' keyword is used that will close the connection by itself after executing the code given inside
            using (MySqlConnection Connection = _schoolcontext.AccessDatabase())
            {

                // Opening the Connection
                Connection.Open();

                // Establishing a new query for our database 
                MySqlCommand Command = Connection.CreateCommand();


                // @id is replaced with a 'sanitized'(masked) id so that id can be referenced
                // without revealing the actual @id
                Command.CommandText = "select * from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);


                // Storing the Result Set query in a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    // While loop is used to loop through each row in the ResultSet 
                    while (ResultSet.Read())
                    {

                        // Accessing the information of Teacher using the Column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        // Accessing the information of the properties of Teacher and then assigning it to the short names 
                        // created above for all properties of the Teacher
                        SelectedTeacher.TeacherId = Id;
                        SelectedTeacher.TeacherFName = FirstName;
                        SelectedTeacher.TeacherLName = LastName;
                        SelectedTeacher.TeacherHireDate = TeacherHireDate;
                        SelectedTeacher.EmployeeNumber = EmployeeNumber;
                        SelectedTeacher.TeacherSalary = Salary;

                    }
                }
            }


            //Return the Information of the SelectedTeacher
            return SelectedTeacher;
        }
    }


}