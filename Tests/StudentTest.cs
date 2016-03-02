using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Students_Courses
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=UniversityReg_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_StudentsEmptyAtFirst()
    {
      int result = Student.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      var StudentOne = new Student("Roberto", new DateTime (2016,2,2));
      var StudentTwo = new Student("Roberto", new DateTime(2016,2,2));

      Assert.Equal(StudentOne, StudentTwo);
    }

    [Fact]
    public void Test_Save_SavesStudentToDataBase()
    {
      var testStudent = new Student("Alice", new DateTime(2000,5,9));
      testStudent.Save();

      var result = Student.GetAll();
      var testList = new List<Student>{testStudent};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToStudentObject()
    {
      var testStudent = new Student("Mr. Chop", new DateTime(1789, 1, 1));
      testStudent.Save();

      var savedStudent = Student.GetAll()[0];

      int result = savedStudent.GetId();
      int testId = testStudent.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsStudentInDataBase()
    {
      var testStudent = new Student("Edward Scissorhands", new DateTime(2020, 12, 10));
      testStudent.Save();

      var foundStudent = Student.Find(testStudent.GetId());

      Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void Test_GetCourses_RetrieveAllCoursesWithStudents()
    {
      var testStudent = new Student("Marco", new DateTime(1995, 9, 21));
      testStudent.Save();

      var courseOne = new Course("Johnathon", "123");
      courseOne.Save();

      var courseTwo = new Course("Bobbie", "43298");
      courseTwo.Save();

      testStudent.AddCourses(courseOne);
      testStudent.AddCourses(courseTwo);

      var testCourseList = new List<Course> {courseOne, courseTwo};
      var resultCourseList = testStudent.GetCourses();

      Assert.Equal(testCourseList, resultCourseList);
    }

    [Fact]
    public void Test_Update_UpdatesStudentInDataBase()
    {
      string name = "Roger";
      var testStudent = new Student(name, new DateTime(1900,1,1));
      testStudent.Save();

      string newName = "Snoop Dog";
      testStudent.Update(newName, new DateTime(1900,1,1));
      string result = testStudent.GetName();

      Assert.Equal(newName, result);
    }
    //
    [Fact]
    public void Test_Delete_DeletesStudentsFromDataBase()
    {
      string nameOne = "Ice Cube";
      var testStudentOne = new Student(nameOne, new DateTime(1900,1,1));
      testStudentOne.Save();

      string nameTwo = "Mashal Mathers";
      var testStudentTwo = new Student(nameTwo, new DateTime(1900,1,1));
      testStudentTwo.Save();

      testStudentOne.Delete();
      var resultStudents = Student.GetAll();
      var testStudentList = new List<Student> {testStudentTwo};

      Assert.Equal(testStudentList, resultStudents);

    }
    [Fact]
    public void Test_AddCourse_AddCourseToStudent()
    {
      Student testStudent = new Student("Harry Styles", new DateTime(1900,1,1));
      testStudent.Save();

      Student testStudent2 = new Student("Billy Ray Cyrus", new DateTime(1900,1,1));
      testStudent2.Save();

      Course testCourse = new Course("Zayne", "101");
      testCourse.Save();

      Course testCourse2 = new Course("Zack and Cody", "102");
      testCourse2.Save();

      testStudent.AddCourses(testCourse);
      testStudent.AddCourses(testCourse2);

      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse, testCourse2};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetCourses_ReturnsAllStudentsCourses()
    {
      Student testStudent = new Student("Tim", new DateTime(1900,1,1));
      testStudent.Save();

      Course testCourse1 = new Course("Andrew", "101");
      testCourse1.Save();

      Course testCourse2 = new Course("Thea", "102");
      testCourse2.Save();

      testStudent.AddCourses(testCourse1);
      List<Course> savedCourses = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse1};

      Assert.Equal(testList, savedCourses);

    }

    //Leave at bottom of test
    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
