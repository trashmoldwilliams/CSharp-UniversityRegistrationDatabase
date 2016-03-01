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
    //
    // [Fact]
    // public void Test_Equal_ReturnsTrueForSameName()
    // {
    //   var StylistOne = new Stylist("Roberto");
    //   var StylistTwo = new Stylist("Roberto");
    //
    //   Assert.Equal(StylistOne, StylistTwo);
    // }
    //
    // [Fact]
    // public void Test_Save_SavesStylistToDataBase()
    // {
    //   var testStylist = new Stylist("Alice");
    //   testStylist.Save();
    //
    //   var result = Stylist.GetAll();
    //   var testList = new List<Stylist>{testStylist};
    //
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Test_Save_AssignsIdToStylistObject()
    // {
    //   var testStylist = new Stylist("Mr. Chop");
    //   testStylist.Save();
    //
    //   var savedStylist = Stylist.GetAll()[0];
    //
    //   int result = savedStylist.GetId();
    //   int testId = testStylist.GetId();
    //
    //   Assert.Equal(testId, result);
    // }
    //
    // [Fact]
    // public void Test_Find_FindsStylistInDataBase()
    // {
    //   var testStylist = new Stylist("Edward Scissorhands");
    //   testStylist.Save();
    //
    //   var foundStylist = Stylist.Find(testStylist.GetId());
    //
    //   Assert.Equal(testStylist, foundStylist);
    // }
    //
    // // [Fact]
    // // public void Test_GetClients_RetrieveAllClientsWithStylist()
    // // {
    // //   var testStylist = new Stylist("Marco");
    // //   testStylist.Save();
    // //
    // //   var clientOne = new Client("Johnathon", testStylist.GetId(), 1);
    // //   clientOne.Save();
    // //
    // //   var clientTwo = new Client("Bobbie", testStylist.GetId(), 2);
    // //   clientTwo.Save();
    // //
    // //   var testClientList = new List<Client> {clientOne, clientTwo};
    // //   var resultClientList = testStylist.GetClients();
    // //
    // //   Assert.Equal(testClientList, resultClientList);
    // // }
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
