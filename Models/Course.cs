namespace Cumulative_1.Models
{

    // Created Course Class which is a definition to define the properties of the Course
    // This definition is used to create objects
    // The properties of that object is then accessed which are sent to view to display on the webpage
    // Here, Course Class is created and the definition mentions that it has 6 properties (CourseId, TeacherId,
    // CourseName, CourseStartDate, CourseEndDate, CourseCode which are accessed in Controller and then return
    // to View to display that properties information on the web page


    public class Course
    {
        // Unique identifier for each course. It is used as the primary key in Courses table.
        public int CourseId { get; set; }

        // It is the code of each course. It is stored as a string.
        public string CourseCode { get; set; }

        // It is the date on which course started.
        public DateTime CourseStartDate { get; set; }

        // It is the date on which the course ended.
        public DateTime CourseFinishDate { get; set; }

        // Unique identifier for each teacher. It is used as the Foreign key in Courses table.
        public int TeacherId { get; set; }

        // It is the name of the course.
        public string CourseName { get; set; }
    }
}
