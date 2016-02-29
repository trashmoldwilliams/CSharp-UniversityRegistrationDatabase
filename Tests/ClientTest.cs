using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Stylist_Clients
{
  public class ClientTest : IDisposable
  {
    public void ClientTestDB()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_ClientsEmptyAtFirst()
    {
      int result = Client.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrie_TrueForSameName()
    {
      var clientOne = new Client("Wolverine");
      var clientTwo = new Client("Wolverine");

      Assert.Equal(clientOne, clientTwo);
    }

    [Fact]
    public void Test_Save_SavesClientDataBase()
    {
      var testClient = new Client("Batman");
      testClient.Save();

      var testList = new List<Client>{testClient};
      var result = Client.GetAll();

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToOBjects()
    {
      var testClient = new Client("Blade");
      testClient.Save();

      var savedClient = Client.GetAll()[0];

      int result = savedClient.GetId();
      int testId = testClient.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindClientInDataBase()
    {
      var testClient = new Client("Timone");
      testClient.Save();

      var foundClient = Client.Find(testClient.GetId());

      Assert.Equal(testClient, foundClient);
    }

    [Fact]
    public void Test_Update_UpdatesClientInDataBase()
    {
      string name = "Poomba";
      var testClient = new Client(name);
      testClient.Save();

      string newName = "Simba";
      testClient.Update(newName);

      Client result = new Client(testClient.GetName());
      Client newClient = new Client(newName);
      // string result = testClient.GetName();

      Assert.Equal(newClient, result);
    }

    [Fact]
    public void Test_Delete_DeletesClientFromDatabase()
    {

      string nameOne = "Winnie";
      Stylist testStylist1 = new Stylist(nameOne);
      testStylist1.Save();

      Client testClient1 = new Client("Piglet");
      testClient1.Save();
      Client testClient2 = new Client("Roo");
      testClient2.Save();


      testClient1.Delete();
      List<Client> resultClients = testStylist1.GetClients();
      List<Client> testClientList = new List<Client> {testClient2};
      // Console.WriteLine("RESULT CLIENTS: " + resultClients);
      // Console.WriteLine(testClientList[0].GetName());
      Console.WriteLine("TEST CLIENT LIST: " + testClientList[0].GetName());
      Assert.Equal(testClientList, resultClients);
    }

    public void Dispose()
    {
      Stylist.DeleteAll();
      Client.DeleteAll();
    }

  }
}
