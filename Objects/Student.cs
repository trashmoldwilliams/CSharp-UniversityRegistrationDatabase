using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Students_Courses
{
  public class Student
  {
    private int _id;
    private string _student_name;
    private DateTime _enrollement;

    public Student(string studentName, DateTime enrollement, int Id = 0)
    {
      _id = Id;
      _student_name = studentName;
      _enrollement = enrollement;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if(!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        var newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool nameEquality = this.GetName() == newStudent.GetName();
        bool enrollementEquality = this.GetDateTime() == newStudent.GetDateTime();

        return (idEquality && nameEquality && enrollementEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
    return _student_name;
    }

    public DateTime GetDateTime()
    {
      return _enrollement;
    }

    public static List<Student> GetAll()
    {
      var allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM students;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {

        var StudentId = rdr.GetInt32(0);
        var StudentName = rdr.GetString(1);
        var StudentDate = rdr.GetDateTime(2);
        Console.WriteLine("eauirgriuhartigh");

        var newStudent = new Student(StudentName, StudentDate, StudentId);
        allStudents.Add(newStudent);

      }

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return allStudents;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("INSERT INTO students (name, dateOfEnrollement) OUTPUT INSERTED.id VALUES (@StudentName, @StudentDate);", conn);
      var nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@StudentName";
      nameParameter.Value = this.GetName();

      var dateParameter = new SqlParameter();
      dateParameter.ParameterName = "@StudentDate";
      dateParameter.Value = this.GetDateTime();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(dateParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      var studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = id;
      cmd.Parameters.Add(studentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime foundStudentDate = new DateTime(1,1,1);

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundStudentDate = rdr.GetDateTime(2);
      }

      var foundStudent = new Student(foundStudentName, foundStudentDate, foundStudentId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }

      return foundStudent;
    }

    public void AddCourses(Course newCourse)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId)", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = newCourse.GetId();
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Course> GetCourses()
     {
       SqlConnection conn = DB.Connection();
       SqlDataReader rdr = null;
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT course_id FROM students_courses WHERE student_id = @StudentId;", conn);
       SqlParameter studentIdParameter = new SqlParameter();
       studentIdParameter.ParameterName = "@StudentId";
       studentIdParameter.Value = this.GetId();
       cmd.Parameters.Add(studentIdParameter);

       rdr = cmd.ExecuteReader();

       List<int>coursesIds = new List<int> {};
       while(rdr.Read())
       {
         int CourseId = rdr.GetInt32(0);
         coursesIds.Add(CourseId);
       }
       if (rdr != null)
       {
         rdr.Close();
       }

       List<Course> courses = new List<Course> {};
       foreach (int CourseId in coursesIds)
       {
         SqlDataReader queryReader = null;
         SqlCommand courseQuery = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);

         SqlParameter courseIdParameter = new SqlParameter();
         courseIdParameter.ParameterName = "@CourseId";
         courseIdParameter.Value = CourseId;
         courseQuery.Parameters.Add(courseIdParameter);

         queryReader = courseQuery.ExecuteReader();
         while(queryReader.Read())
         {
               int thisCourseId = queryReader.GetInt32(0);
               string courseDescription = queryReader.GetString(1);
               string courseNumber = queryReader.GetString(2);
               Course foundCourse = new Course(courseDescription, courseNumber, thisCourseId);
               courses.Add(foundCourse);
         }
         if (queryReader != null)
         {
           queryReader.Close();
         }
       }
       if (conn != null)
       {
         conn.Close();
       }
       return courses;
     }

    public void Update(string newStudentName)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("UPDATE students SET student_name = @NewStudentName OUTPUT INSERTED.student_name WHERE id = @StudentName;", conn);

      var newStudentIdParameter = new SqlParameter();
      newStudentIdParameter.ParameterName = "@NewStudentName";
      newStudentIdParameter.Value = newStudentName;
      cmd.Parameters.Add(newStudentIdParameter);

      var studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._student_name = rdr.GetString(0);
      }

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId;", conn);

      var studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(studentIdParameter);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    //IDisposable testing | Dispose/Delete method
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
