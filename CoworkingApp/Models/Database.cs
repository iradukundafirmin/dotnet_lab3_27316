using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace CoworkingApp.Models
{
    // Small helper that opens the SQLite database and creates the tables.
    public static class Database
    {
        private static readonly string DbPath =
            Path.Combine(AppContext.BaseDirectory, "coworking.db");

        public static SqliteConnection GetConnection()
        {
            var conn = new SqliteConnection("Data Source=" + DbPath);
            conn.Open();

            // SQLite needs this on every connection so ON DELETE CASCADE works
            var pragma = conn.CreateCommand();
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();

            return conn;
        }

        public static void Initialize()
        {
            using (var conn = GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Members (
                        Id    INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name  TEXT NOT NULL,
                        Phone TEXT NOT NULL DEFAULT '',
                        Email TEXT NOT NULL DEFAULT ''
                    );

                    CREATE TABLE IF NOT EXISTS Desks (
                        Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name        TEXT NOT NULL,
                        Type        TEXT NOT NULL,
                        PricePerDay REAL NOT NULL DEFAULT 0
                    );

                    CREATE TABLE IF NOT EXISTS Bookings (
                        Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                        MemberId    INTEGER NOT NULL,
                        DeskId      INTEGER NOT NULL,
                        BookingDate TEXT NOT NULL,
                        FOREIGN KEY (MemberId) REFERENCES Members(Id) ON DELETE CASCADE,
                        FOREIGN KEY (DeskId)   REFERENCES Desks(Id)   ON DELETE CASCADE
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
