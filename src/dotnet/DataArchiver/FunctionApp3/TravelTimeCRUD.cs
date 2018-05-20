using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace WSDOT
{
    public static class TravelTimeCRUD
    {
        [FunctionName("TravelTimeCRUD")]
        public static void Run([TimerTrigger("0 */20 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            GetTravelTimes(log);
        }

        public static void GetTravelTimes(TraceWriter log)
        {
            log.Info("Making WSDOT Request");
            var wsdotUrl = ConfigurationManager.AppSettings["WSDOTUrl"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(wsdotUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            var resp = request.GetResponse();
            var dataStream = resp.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            var reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            log.Info("Completed WSDOT Request");
            var ttRoutes = JsonConvert.DeserializeObject<List<TravelTimeRoute>>(responseFromServer);
            log.Info("Starting DB Insert");
            InsertRow(ttRoutes);
            log.Info("Finished DB Insert");
            // Clean up the streams and the response.  
            reader.Close();
            resp.Close();
        }

        public static void InsertRow(List<TravelTimeRoute> ttRoutes)
        {
            //INSERT INTO TravelTimes VALUES(value1, value2, value3, ...);

            var connectionString = ConfigurationManager.AppSettings["DBConnString"];
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var state = connection.State;
                foreach(var route in ttRoutes)
                {
                    var commandStr = $"IF NOT EXISTS (SELECT * FROM TravelTimes WHERE TravelTimeID = {route.TravelTimeID} AND TimeUpdated = '{route.TimeUpdated}') INSERT INTO TravelTimes VALUES({route.TravelTimeID}, '{route.Name}', '{route.Description}', '{route.TimeUpdated}', {route.Distance}, {route.AverageTime}, {route.CurrentTime});";
                    using (SqlCommand command = new SqlCommand(commandStr, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void CreateTable()
        {
            var connectionString = ConfigurationManager.AppSettings["DBConnString"];

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                var commandStr = "If not exists (select name from sysobjects where name = 'TravelTimes') CREATE TABLE TravelTimes(TravelTimeID INT,Name CHAR(255),Description TEXT,TimeUpdated DATETIME, Distance FLOAT, AverageTime INT, CurrentTime INT)";

                using (SqlCommand command = new SqlCommand(commandStr, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
