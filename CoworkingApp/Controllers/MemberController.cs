using System.Collections.Generic;
using CoworkingApp.Models;

namespace CoworkingApp.Controllers
{
    public class MemberController
    {
        public List<Member> GetAll()
        {
            var list = new List<Member>();
            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id, Name, Phone, Email FROM Members ORDER BY Name";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Phone = reader.GetString(2),
                            Email = reader.GetString(3)
                        });
                    }
                }
            }
            return list;
        }

        // Each method returns an error message, or null when everything went fine.
        public string Add(Member m)
        {
            if (string.IsNullOrWhiteSpace(m.Name))
                return "Name is required.";

            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Members (Name, Phone, Email) VALUES (@name, @phone, @email)";
                cmd.Parameters.AddWithValue("@name", m.Name.Trim());
                cmd.Parameters.AddWithValue("@phone", m.Phone.Trim());
                cmd.Parameters.AddWithValue("@email", m.Email.Trim());
                cmd.ExecuteNonQuery();
            }
            return null;
        }

        public string Update(Member m)
        {
            if (string.IsNullOrWhiteSpace(m.Name))
                return "Name is required.";

            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Members SET Name=@name, Phone=@phone, Email=@email WHERE Id=@id";
                cmd.Parameters.AddWithValue("@name", m.Name.Trim());
                cmd.Parameters.AddWithValue("@phone", m.Phone.Trim());
                cmd.Parameters.AddWithValue("@email", m.Email.Trim());
                cmd.Parameters.AddWithValue("@id", m.Id);
                cmd.ExecuteNonQuery();
            }
            return null;
        }

        public void Delete(int id)
        {
            using (var conn = Database.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Members WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
