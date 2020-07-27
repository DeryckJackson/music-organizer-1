using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MusicOrganizer.Models
{
  public class Artist
  {
    public string Name { get; }
    public int Id { get; set; }
    public List<Album> Albums { get; set; }

    public Artist(string name)
    {
      Name = name;
      Albums = new List<Album> {};
    }

    public Artist(string name, int id)
    {
      Name = name;
      Id = id;
      Albums = new List<Album> {};
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM artists;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
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
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `artists` WHERE artistId = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      string name = "";
      int id = 0;
      while (rdr.Read())
      {
        name = rdr.GetString(0);
        id = rdr.GetInt32(1);
      }

      Artist foundArtist = new Artist(name, id);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundArtist;
    }

    public void AddAlbum(Album album)
    {
      
    }

    public void GetArtistAlbums(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `albums` WHERE artistId = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Album> foundAlbums = new List<Album>();

      while (rdr.Read())
      {
        string title = rdr.GetString(0);
        int albumId = rdr.GetInt32(1);
        int artistId = rdr.GetInt32(2);
        Album newAlbum = new Album(title, albumId, artistId);
        foundAlbums.Add(newAlbum);
      }

      Albums = foundAlbums;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO artists (name) VALUES (@ArtistName);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@ArtistName";
      name.Value = this.Name;
      cmd.Parameters.Add(name);
      
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}