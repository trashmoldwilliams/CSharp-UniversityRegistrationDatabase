using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Students_Courses
{
  public class CourseTest : IDisposable
  {
    public void CourseTestDB()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=UniversityReg_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CoursesEmptyAtFirst()
    {
      int result = Course.GetAll().Count;

      Assert.Equal(0, result);
    }
//
    [Fact]
    public void Test_EqualOverride_TrueForSameName()
    {
      var courseOne = new Course("History", "101");
      var courseTwo = new Course("History", "101");

      Assert.Equal(courseOne, courseTwo);
    }
//
    [Fact]
    public void Test_Save_SavesCourseDataBase()
    {
      var testCourse = new Course("Women Studies", "301");
      testCourse.Save();

      var testList = new List<Course>{testCourse};
      var result = Course.GetAll();

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToOBjects()
    {
      var testCourse = new Course("Math", "404");
      testCourse.Save();

      var savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindCourseInDataBase()
    {
      var testCourse = new Course("Fun Studies", "999");
      testCourse.Save();

      var foundCourse = Course.Find(testCourse.GetId());

      Assert.Equal(testCourse, foundCourse);
    }

    [Fact]
    public void Test_Update_UpdatesCourseInDataBase()
    {
      var courseNameOne = "Underwater basket weaving";
      var courseNumberOne = "101";
      var testCourse = new Course(courseNameOne,courseNumberOne );
      testCourse.Save();

      var courseNameTwo = "History";
      var courseNumberTwo = "101";
      testCourse.Update(courseNameTwo, courseNumberTwo);

      Course result = new Course(testCourse.GetName(), testCourse.GetNumber());
      Course newCourse = new Course(courseNameTwo, courseNumberTwo);

      Console.WriteLine("result name: " + result.GetName());
      Console.WriteLine("result name: " + result.GetNumber());
      Console.WriteLine("newCourse name: " + newCourse.GetName());
      Console.WriteLine("newCourse name: " + newCourse.GetNumber());

      Assert.Equal(newCourse, result);
    }
//
    [Fact]
    public void Test_Delete_DeletesCourseFromDatabase()
    {

      string nameOne = "Winnie";
      Student testStudent1 = new Student(nameOne, new DateTime(1900,1,1));
      testStudent1.Save();

      Course testCourse1 = new Course("Piglet", "101");
      testCourse1.Save();
      Course testCourse2 = new Course("Roo", "102");
      testCourse2.Save();

      testStudent1.AddCourses(testCourse1);
      testStudent1.AddCourses(testCourse2);

      testCourse1.Delete();
      List<Course> resultCourses = testStudent1.GetCourses();
      List<Course> testCourseList = new List<Course> {testCourse2};

      Console.WriteLine(resultCourses);
      Assert.Equal(testCourseList, resultCourses);
    }
    [Fact]
    public void Test_AddStudent_AddsStudentToCourse()
    {
      //Arrange
      Course testCourse = new Course("Theology", "326");
      testCourse.Save();

      Student testStudent = new Student("Superman", new DateTime(2011,1,1));
      testStudent.Save();

      //Act
      testCourse.AddStudent(testStudent);

      List<Student> result = testCourse.GetStudents();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_GetStudent_ReturnsAllCourseStudents()
    {
      //Arrange
      Course testCourse = new Course("Cooking", "515");
      testCourse.Save();

      Student testStudent1 = new Student("Bob the builder", new DateTime(1795,1,1));
      testStudent1.Save();

      Student testStudent2 = new Student("Peter", new DateTime(1964, 2, 2));
      testStudent2.Save();

      //Act
      testCourse.AddStudent(testStudent1);
      List<Student> result = testCourse.GetStudents();
      List<Student> testList = new List<Student> {testStudent1};

      //Assert
      Assert.Equal(testList, result);
    }
//
    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
