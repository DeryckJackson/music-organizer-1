using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MusicOrganizer.Models
{
  public class Artist
  {
    public string Name { get; }
    public int Id { get; }

    public Artist(string name)
    {
      Name = name;
    }

    public Artist(string name, int id)
    {
      Name = name;
      Id = id;
    }

    public static void ClearAll()
    {
      
    }

    public static List<Artist> GetAll()
    {
      List<Artist> allArtists = new List<Artist>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM artists;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        string name = rdr.GetString(0);
        int id = rdr.GetInt32(1);
        Artist newArtist = new Artist(name, id);
        allArtists.Add(newArtist);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allArtists;
    }
    public static Artist Find(int searchId)
    {
      return new Artist("name");
    }

    public void AddAlbum(Album album)
    {
      
    }

  }
}