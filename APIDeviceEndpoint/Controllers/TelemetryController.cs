using APIDeviceEndpoint.Models;
using APIDeviceEndpoint.Models.Send;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIDeviceEndpoint.Controllers
{
    [ApiController]
    [Route("device")]
    public class TelemetryController : ControllerBase
    {

        [HttpPost("{deviceId}/telemetry")]
        public async Task<JsonResult> PostAsync([FromBody] Telemetry value, string deviceId)
        {
            Debug.WriteLine("Adresse mac : " + deviceId);
            Debug.WriteLine("Type : " + value.deviceType.ToString());
            Debug.WriteLine("Value : " + value.metricValue.ToString());
            Debug.WriteLine("Value : " + value.metricDate.ToString());

            value.device_id = deviceId;

            TelemetrySend telemetrySend = new TelemetrySend();

            telemetrySend.device_id = value.device_id;
            telemetrySend.valeur_metrique = value.metricValue;
            telemetrySend.date_metrique = value.metricDate;
            telemetrySend.type_device = value.deviceType;

            string query = "insert into metriques(id_device,type_device,valeur_metrique,date_metrique) values('" + deviceId + "', '" + value.deviceType + "', '" + value.metricValue + "', '" + value.metricDate + "')";
            
            string uid = "root";
            string password = "";
            string sqlDataSource = "SERVER=localhost;PORT=3306;" +
                 "DATABASE=algeco_metrics_calculation;" +
                 "UID=" + uid + ";PASSWORD=" + password;
            MySqlDataReader myReader;

            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(telemetrySend);

            using (var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/api/v1/metriques"))
            {
                using var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = stringContent;

                using var send = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);

                if (!send.IsSuccessStatusCode)
                    throw new Exception();

                var response = send.EnsureSuccessStatusCode();
            }



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
