using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Program.Objects.Stylist_Clients
{
  public class Stylist
  {
    private int _id;
    private string _stylist_name;

    public Stylist(string StylistName, int Id = 0)
    {
      _id = Id;
      _stylist_name = StylistName;
    }

    public override bool Equals(System.Object otherStylist)
    {
      if(!(otherStylist is Stylist))
      {
        return false;
      }
      else
      {
        var newStylist = (Stylist) otherStylist;
        bool idEquality = this.GetId() == newStylist.GetId();
        bool nameEquality = this.GetName() == newStylist.GetName();

        return (idEquality && nameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
    return _stylist_name;
    }

    public static List<Stylist> GetAll()
    {
      var allStylists = new List<Stylist>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM stylists;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        var StylistId = rdr.GetInt32(0);
        var StylistName = rdr.GetString(1);

        var newStylist = new Stylist(StylistName, StylistId);

        allStylists.Add(newStylist);
      }

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return allStylists;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("INSERT INTO stylists (stylist_name) OUTPUT INSERTED.id VALUES (@StylistName);", conn);
      var nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@StylistName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
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

    public static Stylist Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      var cmd = new SqlCommand("SELECT * FROM stylists WHERE id = @StylistId;", conn);
      var stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = id;
      cmd.Parameters.Add(stylistIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStylistId = 0;
      string foundStylistName = null;

      while(rdr.Read())
      {
        foundStylistId = rdr.GetInt32(0);
        foundStylistName = rdr.GetString(1);
      }

      var foundStylist = new Stylist(foundStylistName, foundStylistId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }

      return foundStylist;
    }

    public void AddClients(Client newClient)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO stylists_clients (stylist_id, client_id) VALUES (@StylistId, @ClientId)", conn);
      SqlParameter stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = this.GetId();
      cmd.Parameters.Add(stylistIdParameter);

      SqlParameter clientIdParameter = new SqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = newClient.GetId();
      cmd.Parameters.Add(clientIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Client> GetClients()
     {
       SqlConnection conn = DB.Connection();
       SqlDataReader rdr = null;
       conn.Open();

       SqlCommand cmd = new SqlCommand("SELECT client_id FROM stylists_clients WHERE stylist_id = @StylistId;", conn);
       SqlParameter stylistIdParameter = new SqlParameter();
       stylistIdParameter.ParameterName = "@StylistId";
       stylistIdParameter.Value = this.GetId();
       cmd.Parameters.Add(stylistIdParameter);

       rdr = cmd.ExecuteReader();

       List<int>clientsIds = new List<int> {};
       while(rdr.Read())
       {
         int ClientId = rdr.GetInt32(0);
         clientsIds.Add(ClientId);
       }
       if (rdr != null)
       {
         rdr.Close();
       }

       List<Client> clients = new List<Client> {};
       foreach (int ClientId in clientsIds)
       {
         SqlDataReader queryReader = null;
         SqlCommand clientQuery = new SqlCommand("SELECT * FROM clients WHERE id = @ClientId;", conn);

         SqlParameter clientIdParameter = new SqlParameter();
         clientIdParameter.ParameterName = "@ClientId";
         clientIdParameter.Value = ClientId;
         clientQuery.Parameters.Add(clientIdParameter);

         queryReader = clientQuery.ExecuteReader();
         while(queryReader.Read())
         {
               int thisClientId = queryReader.GetInt32(0);
               string clientDescription = queryReader.GetString(1);
               Client foundClient = new Client(clientDescription, thisClientId);
               clients.Add(foundClient);
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
       return clients;
     }
    // public List<Client> GetClients()
    // {
    //   SqlConnection conn = DB.Connection();
    //   SqlDataReader rdr = null;
    //   conn.Open();
    //
    //   var cmd = new SqlCommand("SELECT * FROM clients WHERE stylist_id = @StylistId;", conn);
    //   var stylistIdParameter = new SqlParameter();
    //   stylistIdParameter.ParameterName = "@StylistId";
    //   stylistIdParameter.Value = this.GetId();
    //   cmd.Parameters.Add(stylistIdParameter);
    //   rdr = cmd.ExecuteReader();
    //
    //   var clients = new List<Client> {};
    //   while(rdr.Read())
    //   {
    //     int ClientId = rdr.GetInt32(0);
    //     string ClientName = rdr.GetString(1);
    //     int ClientStylistId = rdr.GetInt32(2);
    //
    //     var newClient = new Client(ClientName, ClientStylistId, ClientId);
    //     clients.Add(newClient);
    //   }
    //
    //   if(rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //
    //   if(conn != null)
    //   {
    //     conn.Close();
    //   }
    //
    //   return clients;
    // }

    public void Update(string newStylistName)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      var cmd = new SqlCommand("UPDATE stylists SET stylist_name = @NewStylistName OUTPUT INSERTED.stylist_name WHERE id = @StylistId;", conn);

      var NewStylistNameParameter = new SqlParameter();
      NewStylistNameParameter.ParameterName = "@NewStylistName";
      NewStylistNameParameter.Value = newStylistName;
      cmd.Parameters.Add(NewStylistNameParameter);

      var stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = this.GetId();
      cmd.Parameters.Add(stylistIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._stylist_name = rdr.GetString(0);
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

      var cmd = new SqlCommand("DELETE FROM stylists WHERE id = @StylistId;", conn);

      var stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@StylistId";
      stylistIdParameter.Value = this.GetId();

      cmd.Parameters.Add(stylistIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM stylists;", conn);
      cmd.ExecuteNonQuery();
    }

  }
}
