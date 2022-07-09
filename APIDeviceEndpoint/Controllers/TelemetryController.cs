using APIDeviceEndpoint.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;

namespace APIDeviceEndpoint.Controllers
{
    [ApiController]
    [Route("device")]
    public class TelemetryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TelemetryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("{deviceId}/telemetry")]
        public JsonResult Post([FromBody] Telemetry value, string deviceId)
        {
            Debug.WriteLine("Adresse mac : " + deviceId);
            Debug.WriteLine("Type : " + value.deviceType.ToString());
            Debug.WriteLine("Value : " + value.metricValue.ToString());
            Debug.WriteLine("Value : " + value.metricDate.ToString());

            string query = "insert into metriques(id_device,type_device,valeur_metrique,date_metrique) values('" + deviceId + "', '" + value.deviceType + "', '" + value.metricValue + "', '" + value.metricDate + "')";
            
            string uid = "root";
            string password = "";
            string sqlDataSource = "SERVER=localhost;PORT=3306;" +
                 "DATABASE=algeco_metrics_calculation;" +
                 "UID=" + uid + ";PASSWORD=" + password;
            MySqlDataReader myReader;

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(value);
        }

    }

}
