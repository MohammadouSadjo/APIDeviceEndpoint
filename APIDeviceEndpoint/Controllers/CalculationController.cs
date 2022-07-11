using APIDeviceEndpoint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace APIDeviceEndpoint.Controllers
{
    [ApiController]
    [Route("calculation")]
    public class CalculationController : Controller
    {

        [HttpGet("{deviceId}/moyenne")]
        public JsonResult GetMoyenne(string deviceId)
        {
            string query = "select * from metriques_calculees where id_device='"+deviceId+"' order by id_metriques_calculees desc limit 1";

            string uid = "root";
            string password = "";
            string sqlDataSource = "SERVER=localhost;PORT=3306;" +
                 "DATABASE=algeco_metrics_calculation;" +
                 "UID=" + uid + ";PASSWORD=" + password;
            MySqlDataReader myReader;

            ResultMoyenne resultMoyenne = new ResultMoyenne();

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    myReader.Read();

                    resultMoyenne.id_metriques_calculees = (int)myReader["id_metriques_calculees"];
                    resultMoyenne.id_device = (string)myReader["id_device"];
                    resultMoyenne.moyenne = (string)myReader["moyenne"];

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(resultMoyenne);
        }

        [HttpGet("{deviceId}/pourcentage")]
        public JsonResult GetPourcentage(string deviceId)
        {
            string query = "select * from metriques_calculees where id_device='" + deviceId + "' order by id_metriques_calculees desc limit 1";

            string uid = "root";
            string password = "";
            string sqlDataSource = "SERVER=localhost;PORT=3306;" +
                 "DATABASE=algeco_metrics_calculation;" +
                 "UID=" + uid + ";PASSWORD=" + password;
            MySqlDataReader myReader;

            ResultPourcentage resultPourcentage = new ResultPourcentage();

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    myReader.Read();

                    resultPourcentage.id_metriques_calculees = (int)myReader["id_metriques_calculees"];
                    resultPourcentage.id_device = (string)myReader["id_device"];
                    resultPourcentage.pourcentage = (string)myReader["pourcentage"];

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(resultPourcentage);
        }
    }
}
