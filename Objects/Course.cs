using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Students_Courses
{
  public class Course
  {
    private int _id;
    private string _course_name;
    private string _course_number;

    public Course(string CourseName, string CourseNumber, int Id = 0)
    {
      _id = Id;
      _course_name = CourseName;
      _course_number = CourseNumber;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if(!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        var newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool nameEquality = this.GetName() == newCourse.GetName();
        bool numberEquality = this.GetNumber() == newCourse.GetNumber();
        return (idEquality && nameEquality && numberEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _course_name;
    }

    public string GetNumber()
    {
      return _course_number;
    }

    public static List<Course> GetAll()
    {
      var AllCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM courses", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int CourseId = rdr.GetInt32(0);
        string CourseName = rdr.GetString(1);
        string CourseNumber = rdr.GetString(2);

        var newCourse = new Course(CourseName, CourseNumber, CourseId);
        AllCourses.Add(newCourse);
      }

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return AllCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("INSERT INTO courses (name, number) OUTPUT INSERTED.id VALUES (@CourseName, @CourseNumber);", conn);

      var nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CourseName";
      nameParameter.Value = this.GetName();

      var numberParameter = new SqlParameter();
      numberParameter.ParameterName = "@CourseNumber";
      numberParameter.Value = this.GetNumber();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(numberParameter);

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

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);
      var courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = id.ToString();
      cmd.Parameters.Add(courseIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundCourseNumber = null;

      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
        foundCourseNumber = rdr.GetString(2);
      }

      var foundCourse = new Course(foundCourseName, foundCourseNumber, foundCourseId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return foundCourse;
    }

    public void Update(string newName, string newNumber)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("UPDATE courses SET name = @NewName OUTPUT INSERTED.name WHERE id = @CourseId; UPDATE courses SET number = @NewNumber OUTPUT INSERTED.number WHERE id = @CourseId", conn);

      var newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      var newNumberParameter = new SqlParameter();
      newNumberParameter.ParameterName = "@NewNumber";
      newNumberParameter.Value = newNumber;
      cmd.Parameters.Add(newNumberParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();
      cmd.Parameters.Add(courseIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._course_name = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = new SqlCommand("DELETE FROM courses WHERE id = @CourseId;", conn);

      var courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();

      cmd.Parameters.Add(courseIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses", conn);
      cmd.ExecuteNonQuery();
    }

public void AddStudent(Student newStudent)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = newStudent.GetId();
      cmd.Parameters.Add(studentIdParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT student_id FROM students_courses WHERE course_id = @CourseId;", conn);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = this.GetId();
      cmd.Parameters.Add(courseIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> studentIds = new List<int> {};

      while (rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        studentIds.Add(studentId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Student> students = new List<Student> {};

      foreach (int studentId in studentIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand studentQuery = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);

        SqlParameter studentIdParameter = new SqlParameter();
        studentIdParameter.ParameterName = "@StudentId";
        studentIdParameter.Value = studentId;
        studentQuery.Parameters.Add(studentIdParameter);

        queryReader = studentQuery.ExecuteReader();
        while (queryReader.Read())
        {
          int thisStudentId = queryReader.GetInt32(0);
          string studentName = queryReader.GetString(1);
          DateTime studentNumber = queryReader.GetDateTime(2);
          Student foundStudent = new Student(studentName, studentNumber, thisStudentId);
          students.Add(foundStudent);
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
      return students;
    }
  }
}
