using System;
using System.Collections.Generic;
using System.Globalization;
using CoworkingApp.Models;

namespace CoworkingApp.Controllers
{
    public class BookingController
    {
        private const string DateFormat = "yyyy-MM-dd";

        public List<Booking> GetAll()
        {
            var list = new List<Booking>();
            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT b.Id, b.MemberId, b.DeskId, m.Name, d.Name, b.BookingDate
                    FROM Bookings b
                    JOIN Members m ON m.Id = b.MemberId
                    JOIN Desks d   ON d.Id = b.DeskId
                    ORDER BY b.BookingDate DESC";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Booking
                        {
                            Id = reader.GetInt32(0),
                            MemberId = reader.GetInt32(1),
                            DeskId = reader.GetInt32(2),
                            MemberName = reader.GetString(3),
                            DeskName = reader.GetString(4),
                            BookingDate = DateTime.ParseExact(reader.GetString(5), DateFormat, CultureInfo.InvariantCulture)
                        });
                    }
                }
            }
            return list;
        }

        public string Add(Booking b)
        {
            if (IsDeskTaken(b.DeskId, b.BookingDate, 0))
                return "This desk is already booked on that date.";

            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Bookings (MemberId, DeskId, BookingDate) VALUES (@member, @desk, @date)";
                cmd.Parameters.AddWithValue("@member", b.MemberId);
                cmd.Parameters.AddWithValue("@desk", b.DeskId);
                cmd.Parameters.AddWithValue("@date", b.BookingDate.ToString(DateFormat, CultureInfo.InvariantCulture));
                cmd.ExecuteNonQuery();
            }
            return null;
        }

        public string Update(Booking b)
        {
            if (IsDeskTaken(b.DeskId, b.BookingDate, b.Id))
                return "This desk is already booked on that date.";

            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Bookings SET MemberId=@member, DeskId=@desk, BookingDate=@date WHERE Id=@id";
                cmd.Parameters.AddWithValue("@member", b.MemberId);
                cmd.Parameters.AddWithValue("@desk", b.DeskId);
                cmd.Parameters.AddWithValue("@date", b.BookingDate.ToString(DateFormat, CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@id", b.Id);
                cmd.ExecuteNonQuery();
            }
            return null;
        }

        public void Delete(int id)
        {
            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Bookings WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // True if another booking already uses this desk on this date.
        // ignoreId lets an update skip the booking being edited (use 0 when adding).
        private bool IsDeskTaken(int deskId, DateTime date, int ignoreId)
        {
            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Bookings WHERE DeskId=@desk AND BookingDate=@date AND Id<>@id";
                cmd.Parameters.AddWithValue("@desk", deskId);
                cmd.Parameters.AddWithValue("@date", date.ToString(DateFormat, CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@id", ignoreId);
                return System.Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}
