using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MusicOrganizer.Models
{
  public class Album
  {
    public string Title { get; }
    public int Id { get; set; }
    public int ArtistId { get; set; }

    public Album(string title)
    {
      Title = title;
    }

    public Album(string title, int artistId)
    {
      Title = title;
      ArtistId = artistId;
    }
    public Album(string title, int id, int artistId)
    {
      Title = title;
      Id = id;
      ArtistId = artistId;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM albums;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Album> GetAll()
    {
      List<Album> allAlbums = new List<Album>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM albums;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        string title = rdr.GetString(0);
        int id = rdr.GetInt32(1);
        int artistId = rdr.GetInt32(2);
        Album newAlbum = new Album(title, id, artistId);
        allAlbums.Add(newAlbum);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allAlbums;
    }

    public static Album Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `albums` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      string title = "";
      int id = 0;
      int artistId = 0;
      while (rdr.Read())
      {
        title = rdr.GetString(0);
        id = rdr.GetInt32(1);
        artistId = rdr.GetInt32(2);
      }

      Album foundAlbum = new Album(title, id, artistId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundAlbum;
    }

    public void Save(int artistId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO artists (title, artistId) VALUES (@AlbumTitle, @ArtistId);";

      MySqlParameter title = new MySqlParameter();
      title.ParameterName = "@AlbumTitle";
      title.Value = this.Title;
      cmd.Parameters.Add(title);
      MySqlParameter aId = new MySqlParameter();
      aId.ParameterName = "@ArtistId";
      aId.Value = artistId;
      cmd.Parameters.Add(artistId);
      
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
