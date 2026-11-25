using System.Collections.Generic;
using CoworkingApp.Models;

namespace CoworkingApp.Controllers
{
    public class DeskController
    {
        public List<Desk> GetAll()
        {
            var list = new List<Desk>();
            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id, Name, Type, PricePerDay FROM Desks ORDER BY Name";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Desk
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Type = reader.GetString(2),
                            PricePerDay = (decimal)reader.GetDouble(3)
                        });
                    }
                }
            }
            return list;
        }

        public string Add(Desk d)
        {
            if (string.IsNullOrWhiteSpace(d.Name))
                return "Name is required.";
            if (string.IsNullOrWhiteSpace(d.Type))
                return "Please choose a type.";

            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Desks (Name, Type, PricePerDay) VALUES (@name, @type, @price)";
                cmd.Parameters.AddWithValue("@name", d.Name.Trim());
                cmd.Parameters.AddWithValue("@type", d.Type);
                cmd.Parameters.AddWithValue("@price", (double)d.PricePerDay);
                cmd.ExecuteNonQuery();
            }
            return null;
        }

        public string Update(Desk d)
        {
            if (string.IsNullOrWhiteSpace(d.Name))
                return "Name is required.";
            if (string.IsNullOrWhiteSpace(d.Type))
                return "Please choose a type.";

            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Desks SET Name=@name, Type=@type, PricePerDay=@price WHERE Id=@id";
                cmd.Parameters.AddWithValue("@name", d.Name.Trim());
                cmd.Parameters.AddWithValue("@type", d.Type);
                cmd.Parameters.AddWithValue("@price", (double)d.PricePerDay);
                cmd.Parameters.AddWithValue("@id", d.Id);
                cmd.ExecuteNonQuery();
            }
            return null;
        }

        public void Delete(int id)
        {
            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Desks WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
