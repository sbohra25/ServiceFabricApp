using Microsoft.AspNetCore.Mvc;
using ServiceFabricApp.API.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceFabricApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        string connectionString =
           "Data Source=antunes12;User ID=api;Password=StoreLynk;Initial Catalog=StoreLynkDB_Prod01;MultipleActiveResultSets=True";

        [HttpGet]
        public async Task<IEnumerable<EquipmentFirmware>> GetAsync()
        {
            // Populate data from database and mapped with EquipmentFirmware class
            IEnumerable<EquipmentFirmware> allEquipments = GetEquipmentFirmware();
            return allEquipments.Select(e => new EquipmentFirmware
            {
                Id = e.Id,
                EquipmentId = e.EquipmentId,
                FirmwareDescription = e.FirmwareDescription,
                CurrentVersion = e.CurrentVersion
            });

            // Fake data to validate web api           
            //return new[] { new ApiEquipment { Id = Guid.NewGuid(), Description = "Test" } };
        }

        private List<EquipmentFirmware> GetEquipmentFirmware()
        {
            List<EquipmentFirmware> result = new List<EquipmentFirmware>();
            // Assumes connectionString is a valid connection string.  
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEquipmentFirmware", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@EquipmentId", SqlDbType.VarChar).Value = "4";
                cmd.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = "1254";
                connection.Open();

                // Create the DataReader to retrieve data
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EquipmentFirmware item = new EquipmentFirmware()
                        {
                            Id = reader["Id"].ToString(),
                            EquipmentId = reader["EquipmentId"].ToString(),
                            FirmwareDescription = reader["FirmwareDescription"].ToString(),
                            CurrentVersion = reader["CurrentVersion"].ToString()
                        };
                        result.Add(item);
                    }
                }
            }
            return result;
        }    
    }
}
