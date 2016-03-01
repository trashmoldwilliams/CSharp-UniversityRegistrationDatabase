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
    //
    // [Fact]
    // public void Test_Update_UpdatesStylistInDataBase()
    // {
    //   string name = "Roger";
    //   var testStylist = new Stylist(name);
    //   testStylist.Save();
    //
    //   string newName = "Snoop Dog";
    //   testStylist.Update(newName);
    //   string result = testStylist.GetName();
    //
    //   Assert.Equal(newName, result);
    // }
    //
    // [Fact]
    // public void Test_Delete_DeletesStylistsFromDataBase()
    // {
    //   string nameOne = "Ice Cube";
    //   var testStylistOne = new Stylist(nameOne);
    //   testStylistOne.Save();
    //
    //   string nameTwo = "Mashal Mathers";
    //   var testStylistTwo = new Stylist(nameTwo);
    //   testStylistTwo.Save();
    //
    //   testStylistOne.Delete();
    //   var resultStylists = Stylist.GetAll();
    //   var testStylistList = new List<Stylist> {testStylistTwo};
    //
    //   Assert.Equal(testStylistList, resultStylists);
    //
    // }
    // [Fact]
    // public void Test_AddClient_AddClientToStylist()
    // {
    //   Stylist testStylist = new Stylist("Harry Styles");
    //   testStylist.Save();
    //
    //   Stylist testStylist2 = new Stylist("Billy Ray Cyrus");
    //   testStylist2.Save();
    //
    //   Client testClient = new Client("Zayne");
    //   testClient.Save();
    //
    //   Client testClient2 = new Client("Zack and Cody");
    //   testClient2.Save();
    //
    //   testStylist.AddClients(testClient);
    //   testStylist.AddClients(testClient2);
    //
    //   List<Client> result = testStylist.GetClients();
    //   List<Client> testList = new List<Client> {testClient, testClient2};
    //
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Test_GetClients_ReturnsAllStylistsClients()
    // {
    //   Stylist testStylist = new Stylist("Tim");
    //   testStylist.Save();
    //
    //   Client testClient1 = new Client("Andrew");
    //   testClient1.Save();
    //
    //   Client testClient2 = new Client("Thea");
    //   testClient2.Save();
    //
    //   testStylist.AddClients(testClient1);
    //   List<Client> savedClients = testStylist.GetClients();
    //   List<Client> testList = new List<Client> {testClient1};
    //
    //   Assert.Equal(testList, savedClients);
    //
    // }

    //Leave at bottom of test
    public void Dispose()
    {
      Student.DeleteAll();
      // Course.DeleteAll();
    }
  }
}
