using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;

namespace EpicodusGames.Models
{
    public class Account
    {
      public int Id {get; set;}
      public string Accountname {get; set;}
      public string Email {get; set;}
      public string Password {get; set;}
      public bool ActiveAccount {get; set;}
      public int Level {get; set;}
      public int TotalXp {get; set;}
      public int CurrentLevelXp {get; set;}
      public float LevelBar {get; set;}
      public int NextLevel {get; set;}

      public Account(string accountname, string email, string password, int id = 0)
      {
        Accountname = accountname;
        Email = email.ToLower();
        Password = password;
        Id = id;
        Level = 1;
        TotalXp = 0;
        CurrentLevelXp = 0;
        LevelBar = 0;
        ActiveAccount = false;
        NextLevel = Level + 1;
      }



      public Account(string accountname, string email, string password, int level, int totalXp, int currentLevelXp, float levelBar, int id = 0)
      {
        Accountname = accountname;
        Email = email.ToLower();
        Password = password;
        Id = id;
        Level = level;
        TotalXp = totalXp;
        CurrentLevelXp = currentLevelXp;
        LevelBar = levelBar;
        ActiveAccount = false;
        NextLevel = Level + 1;
      }

      public void Save()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO account (id, account_name, email, password, level, total_xp, current_level_xp, level_bar) VALUES ('"+Id+"', '"+Accountname+"', '"+Email+"', '"+Password+"', '"+Level+"', '"+TotalXp+"', '"+CurrentLevelXp+"', '"+LevelBar+"');";
        cmd.ExecuteNonQuery();
        Id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public void UpdateXp(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE account SET total_xp = '"+TotalXp+"' WHERE id = '"+id+"';";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public void UpdateCurrentLevelXp(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE account SET current_level_xp = '"+CurrentLevelXp+"' WHERE id = '"+id+"';";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public void UpdateLevelBar(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE account SET level_bar = '"+LevelBar+"' WHERE id = '"+id+"';";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static Account Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM account WHERE id = '"+id+"';";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int accountId = 0;
        string accountAccountname = "";
        string accountEmail = "";
        string accountPassword = "";
        int accountLevel = 0;
        int accountTotalXp = 0;
        int accountCurrentXp = 0;
        float accountLevelBar = 0;
        bool accountActivity = false;
        while(rdr.Read())
        {
          accountId = rdr.GetInt32(0);
          accountAccountname = rdr.GetString(1);
          accountEmail = rdr.GetString(2);
          accountPassword = rdr.GetString(3);
          accountLevel = rdr.GetInt32(4);
          accountTotalXp = rdr.GetInt32(5);
          accountCurrentXp = rdr.GetInt32(6);
          accountLevelBar = rdr.GetFloat(7);
          accountActivity = rdr.GetBoolean(8);
        }
        Account newAccount = new Account(accountAccountname, accountEmail, accountPassword, accountLevel, accountTotalXp, accountCurrentXp, accountLevelBar, accountId);
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return newAccount;
      }

      //Overloaded find method using email and password
      public static Account FindAccount(string email, string password)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM account WHERE email = '"+email+"' AND password = '"+password+"';";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int accountId = 0;
        string accountAccountname = "";
        string accountEmail = "";
        string accountPassword = "";
        int accountLevel = 0;
        int accountTotalXp = 0;
        int accountCurrentXp = 0;
        float accountLevelBar = 0;
        bool accountActivity = false;
        while(rdr.Read())
        {
          accountId = rdr.GetInt32(0);
          accountAccountname = rdr.GetString(1);
          accountEmail = rdr.GetString(2);
          accountPassword = rdr.GetString(3);
          accountLevel = rdr.GetInt32(4);
          accountTotalXp = rdr.GetInt32(5);
          accountCurrentXp = rdr.GetInt32(6);
          accountLevelBar = rdr.GetFloat(7);
          accountActivity = rdr.GetBoolean(8);
        }
        Account newAccount = new Account(accountAccountname, accountEmail, accountPassword, accountLevel, accountTotalXp, accountCurrentXp, accountLevelBar, accountId);
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return newAccount;
      }

      public bool ValidateEmail()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM account WHERE email = '"+Email+"';";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            string accountEmail = rdr.GetString(1);
            if(accountEmail == Email)
            {
              return true;
            }
        }
        return false;
      }

      public bool ValidatePassword()
      {
        if(Password.Length < 5)
        {
          return false;
        }
        else if(!(Password.Any(char.IsUpper)))
        {
          return false;
        }
        return true;
      }

      public static Account FindActiveAccount()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM account WHERE active_account = 'true';";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int accountId = 0;
        string accountAccountname = "";
        string accountEmail = "";
        string accountPassword = "";
        int accountLevel = 0;
        int accountTotalXp = 0;
        int accountCurrentXp = 0;
        float accountLevelBar = 0;
        bool accountActivity = false;
        while(rdr.Read())
        {
          accountId = rdr.GetInt32(0);
          accountAccountname = rdr.GetString(1);
          accountEmail = rdr.GetString(2);
          accountPassword = rdr.GetString(3);
          accountLevel = rdr.GetInt32(4);
          accountTotalXp = rdr.GetInt32(5);
          accountCurrentXp = rdr.GetInt32(6);
          accountLevelBar = rdr.GetFloat(7);
          accountActivity = rdr.GetBoolean(8);
        }
        Account newAccount = new Account(accountAccountname, accountEmail, accountPassword, accountLevel, accountTotalXp, accountCurrentXp, accountLevelBar, accountId);
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return newAccount;
      }



      // If a duplicate is found, return false. FALSE = INVALID
      public bool CheckDuplicateAccountname()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM account WHERE account_name = '"+Accountname+"' OR email ='"+Email+"';";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          string email = rdr.GetString(1);
          string accountname = rdr.GetString(3);
          if(accountname == Accountname || email == Email)
          {
            return false;
          }
        }
        return true;
      }

      public void CheckLevelUp()
      {
        int xpUntilNextLevel = 0;
        if(Level == 20)
        {
          xpUntilNextLevel = 5000;
          if(CurrentLevelXp >= xpUntilNextLevel)
          {
            Level++;
            CurrentLevelXp = 0;
          }
          // Max acc level
        }
        else if(Level >= 15 )
        {
          xpUntilNextLevel = 4000;
          if(CurrentLevelXp >= xpUntilNextLevel)
          {
            Level++;
            CurrentLevelXp = 0;
          }
        }
        else if(Level >= 10)
        {
          xpUntilNextLevel = 3000;
          if(CurrentLevelXp >= xpUntilNextLevel)
          {
            Level++;
            CurrentLevelXp = 0;
          }
        }
        else if(Level >= 5)
        {
          xpUntilNextLevel = 2000;
          if(CurrentLevelXp >= xpUntilNextLevel)
          {
            Level++;
            CurrentLevelXp = 0;
          }
        }
        else if(Level >= 1)
        {
          xpUntilNextLevel = 1000;
          if(CurrentLevelXp >= xpUntilNextLevel)
          {
            Level++;
            CurrentLevelXp = 0;
          }
        }
        if(CurrentLevelXp > 0)
        {
          LevelBar = CurrentLevelXp / xpUntilNextLevel;
        }
        // else {
        //   LevelBar = 0;
        // }
      }

      public void AddXp()
      {
        TotalXp += 500;
        CurrentLevelXp += 500;
        CheckLevelUp();
      }

      // forgot password

    }
}
