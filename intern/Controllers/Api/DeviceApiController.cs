using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;

namespace intern.Controllers.Api
{
    public class DeviceApiController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Device> devices = new List<Device>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name FROM Device";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Device device = new Device
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()
                    };
                    devices.Add(device);
                }
            }

            return Ok(devices);
        }

        [HttpGet]
        public IHttpActionResult GetFilteredDevices(string searchTerm)
        {
            List<Device> filteredDevices = new List<Device>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name FROM Device WHERE LOWER(Name) LIKE @SearchTerm";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Device device = new Device
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()
                    };
                    filteredDevices.Add(device);
                }
            }

            return Ok(filteredDevices);
        }

        [HttpPost]
        public IHttpActionResult AddDevice(Device newDevice)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Device (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", newDevice.Name);

                connection.Open();
                int newDeviceId = Convert.ToInt32(command.ExecuteScalar());
                newDevice.Id = newDeviceId;
            }

            return Ok(newDevice);
        }

        [HttpPut]
        public IHttpActionResult UpdateDevice(int id, Device updatedDevice)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Device SET Name = @Name WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", updatedDevice.Name);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(updatedDevice);
        }

        public class Device
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
