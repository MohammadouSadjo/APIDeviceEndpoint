namespace APIDeviceEndpoint.Models.Send
{
    public class TelemetrySend
    {
        public string device_id { get; set; }
        public string date_metrique { get; set; }
        public string type_device { get; set; }
        public string valeur_metrique { get; set; }
    }
}
