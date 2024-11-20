namespace Cumulative_1.Models
{

    // Created Student Class which is a definition to define the properties of the Student
    // This definition is used to create objects
    // The properties of that object is then accessed which are sent to view to display on the webpage
    // Here, Teacher Class is created and the definition mentions that it has 6 properties (StudentId, StudentFName,
    // StudentLName, StudentEnrolmentDate, StudentNumber which are accessed in Controller and then return
    // to View to display that properties information on the web page
    public class Student
    {
        // Unique identifier for each student. It is used as the primary key in Student table.
        public int StudentId { get; set; }

        // First name of the student. It stores the student's first name as a string.
        public string StudentFName { get; set; }

        // Last name of the student. It stores the student's last name as a string.
        public string StudentLName { get; set; }

        // It tells the enrolment date of the student. 
        public DateTime StudentEnrolmentDate { get; set; }

        // It is the student number of each student. It is stored as a string.
        public string StudentNumber { get; set; }
    }
}

