using System;
using System.Collections.Generic;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Elasticsearch.Net;
using Nest;

namespace ElasticSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Elastic Search Time..!");

                  System.Console.WriteLine();
            var _PatientService = new PatientService();
            var patientDtl = _PatientService.GenarateNewPatient();
            _PatientService.SavePatient(patientDtl);
        }



    }

    public class PatientService
    {

        public void SavePatient(Patient patient)
        {
            // Save this to Db 
            // After save process 
            InsertToElastic(patient);

        }
        private bool InsertToElastic(Patient patient)
        {
            bool IsValid = false;
            try
            {
                var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                      .DefaultIndex("patientindex"); // Replace with your actual index name

                var elasticClient = new ElasticClient(connectionSettings);
                var indexResponse = elasticClient.IndexDocument(patient);

                if (indexResponse.IsValid)
                {
                    IsValid = true;
                    Console.WriteLine("Document indexed successfully!");
                }
                else
                {
                    IsValid = false;
                    Console.WriteLine($"Failed to index document. Error: {indexResponse.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
            return IsValid;
        }
        public Patient GenarateNewPatient()
        {
            Random random = new Random();
            int randomNumber = random.Next(); // Generates a random integer

            // You can also specify a range for the random number:
            int min = 1;
            int max = 100;
            int randomInRange = random.Next(min, max); // Generates a random integer between min (inclusive) and max (exclusive)


            Patient patient = new Patient()
            {
                Id = randomInRange,
                MobileNo = "1234567890",
                PatientName = "Kahlid" + randomInRange.ToString()
            };

            return patient;
        }
    }

    public class Patient
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string MobileNo { get; set; }
    }
}
