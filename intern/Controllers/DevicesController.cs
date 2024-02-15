using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace intern.Controllers
{
    
    public class DevicesController : ApiController
    {
        SqlConnection con;
        public readonly string connectionString = @"server=DEVILSBREATH\\MSSQLSERVER02; database=Devices;Integrated Security=True;";
        public string GetAllDevicesFromDatabase()
        {
            using(con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Device", con))
                {
                    DataTable dt = new DataTable();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            string devices = JsonConvert.SerializeObject(dt);
                            return devices;
                        }
                        else
                        {
                            return "No Data Found";
                        }
                    }
                }
            }
        }

        [HttpPost]
        public IHttpActionResult AddDeviceToDatabase(Device device)
        {

            {
                string query = "INSERT INTO Device (Name) VALUES (@Name)";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Name", device.Name);

                con.Open();
                command.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult UpdateDeviceInDatabase(int id, Device updatedDevice)
        {
            {
                string query = "UPDATE Device SET Name = @Name WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Name", updatedDevice.Name);
                command.Parameters.AddWithValue("@Id", id);

                con.Open();
                command.ExecuteNonQuery();
            }

            return Ok();
        }


        [HttpGet]
        public IHttpActionResult GetAllDevices()
        {
            List<Device> devices = new List<Device>();

            {
                string query = "SELECT Id, Name FROM Device";
                SqlCommand command = new SqlCommand(query, con);

                con.Open();
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

                reader.Close();
            }

            return Ok(devices);
        }

        [HttpGet]
        public IHttpActionResult GetDeviceById(int id)
        {
            Device device = null;

            {
                string query = "SELECT Id, Name FROM Device WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    device = new Device
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()
                    };
                }

                reader.Close();
            }

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        [HttpPost]
        public IHttpActionResult AddDevice(Device device)
        {
            {
                string query = "INSERT INTO Device (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Name", device.Name);

                con.Open();
                int newDeviceId = Convert.ToInt32(command.ExecuteScalar());
                device.Id = newDeviceId;
            }

            return Created(Request.RequestUri + "/" + device.Id, device);
        }

        [HttpPut]
        public IHttpActionResult UpdateDevice(int id, Device device)
        {
            {
                string query = "UPDATE Device SET Name = @Name WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Name", device.Name);
                command.Parameters.AddWithValue("@Id", id);

                con.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(device);
        }

        public class Device
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
