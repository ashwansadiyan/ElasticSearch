using Nest;
using System;

namespace Elastic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        } 
    }

    public class Patient
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string MobileNo { get; set; }
    }
    
    public class PatientService
    {
        public Patient GenerateNewPatient()
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
                PatientName = "Patient_" + randomInRange.ToString()
            };

            return patient;
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
    }

}
