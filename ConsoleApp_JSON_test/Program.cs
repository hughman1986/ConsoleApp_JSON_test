using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_JSON_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Serialize_and_Deserialize_with_an_Object();



            Serialize_an_Object();

            Serialize_a_Collection();

            Serialize_a_Dictionary();

            Read_JSON_with_JsonTextReader();

            Read_JSON_with_JsonTextWriter();

            Read_JSON_with_JsonTextWriter();

            Querying_JSON_with_JSON_Path();

            Serialize_JSON_to_a_file();
            Deserialize_JSON_from_a_file();

            Serialize_JSON_to_a_file_dictionary_test();
            Serialize_JSON_to_a_file_dictionary_test();

            Deserialize_JSON_from_a_file_dictionary_test();

            // wait - not to end
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public class Account
        {
            public string Email { get; set; }
            public bool Active { get; set; }
            public DateTime CreatedDate { get; set; }
            public IList<string> Roles { get; set; }
        }

        private static void Serialize_an_Object()
        {
            Console.WriteLine("==Serialize_an_Object");

            Account account = new Account
            {
                Email = "james@example.com",
                //Active = true,
                CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                Roles = new List<string>
                {
                    "User",
                    "Admin"
                }
            };

            string json = JsonConvert.SerializeObject(account, Formatting.Indented);
            Console.Write(json);
            Console.Write("\n\r");

        }


        public class Account2
        {
            public string Email { get; set; }
            //public bool Active { get; set; }
            public DateTime CreatedDate { get; set; }
            public IList<string> Roles { get; set; }
            public string Active22 { get; set; }

        }

        public class Account3
        {
            public DateTime CreatedDate { get; set; }
            public IList<string> Roles { get; set; }
            public string Email { get; set; }
            public bool Active { get; set; }

        }
        private static void Serialize_and_Deserialize_with_an_Object()
        {
            Console.WriteLine("==Serialize_an_Object");

            Account account = new Account
            {
                Email = "james@example.com",
                Active = true,
                CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                Roles = new List<string>
                {
                    "User",
                    "Admin"
                }
            };

            string json = JsonConvert.SerializeObject(account, Formatting.Indented);
            Console.Write(json);
            Console.Write("\n\r");

            // 測試object格式不同的時候會發生甚麼狀況 
            Account2 account2 = JsonConvert.DeserializeObject<Account2>(json);
            string json2 = JsonConvert.SerializeObject(account2, Formatting.Indented);

            Console.WriteLine(json2);
            Console.Write("\n\r");

            // 測試object格式變數的順序不同時會發生甚麼事情  
            Account3 account3 = JsonConvert.DeserializeObject<Account3>(json);
            string json3 = JsonConvert.SerializeObject(account3, Formatting.Indented);

            Console.WriteLine(json3);
            Console.Write("\n\r");

        }


        private static void Serialize_a_Collection()
        {
            Console.WriteLine("==Serialize_a_Collection");

            List<string> videogames = new List<string>
            {
                "Starcraft",
                "Halo",
                "Legend of Zelda"
            };

            string json = JsonConvert.SerializeObject(videogames);
            Console.Write(json);
            Console.Write("\n\r");

        }

        private static void Serialize_a_Dictionary()
        {
            Console.WriteLine("==Serialize_a_Dictionary");

            Dictionary<string, int> points = new Dictionary<string, int>
            {
                { "James", 9001 },
                { "Jo", 3474 },
                { "Jess", 11926 }
            };

            string json = JsonConvert.SerializeObject(points, Formatting.Indented);

            Console.Write(json);
            Console.Write("\n\r");

        }



        private static void Read_JSON_with_JsonTextReader()
        {
            Console.WriteLine("==Read_JSON_with_JsonTextReader");

            string json = @"{
                           'CPU': 'Intel',
                           'PSU': '500W',
                           'Drives': [
                             'DVD read/writer'
                             /*(broken)*/,
                             '500 gigabyte hard drive',
                             '200 gigabyte hard drive'
                           ]
                        }";

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                }
                else
                {
                    Console.WriteLine("Token: {0}", reader.TokenType);
                }
            }
            Console.Write("\n\r");

        }


        private static void Read_JSON_with_JsonTextWriter()
        {
            Console.WriteLine("==Read_JSON_with_JsonTextWriter");

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("CPU");
                writer.WriteValue("Intel");

                writer.WritePropertyName("PSU");
                writer.WriteValue("500W");

                writer.WritePropertyName("Drives");
                writer.WriteStartArray();
                writer.WriteValue("DVD read/writer");
                writer.WriteComment("(broken)");
                writer.WriteValue("500 gigabyte hard drive");
                writer.WriteValue("200 gigabyte hard drive");
                writer.WriteEnd();

                writer.WriteEndObject();
            }

            Console.WriteLine(sb.ToString());
            Console.Write("\n\r");

        }


        private static void Querying_JSON_with_JSON_Path()
        {
            Console.WriteLine("==Querying_JSON_with_JSON_Path");

            JObject o = JObject.Parse(@"{
              'Stores': [
                'Lambton Quay',
                'Willis Street'
              ],
              'Manufacturers': [
                {
                  'Name': 'Acme Co',
                  'Products': [
                    {
                      'Name': 'Anvil',
                      'Price': 50
                    }
                  ]
                },
                {
                  'Name': 'Contoso',
                  'Products': [
                    {
                      'Name': 'Elbow Grease',
                      'Price': 99.95
                    },
                    {
                      'Name': 'Headlight Fluid',
                      'Price': 4
                    }
                  ]
                }
              ]
            }");

            string name = (string)o.SelectToken("Manufacturers[0].Name");

            Console.WriteLine(name);
            // Acme Co

            decimal productPrice = (decimal)o.SelectToken("Manufacturers[0].Products[0].Price");

            Console.WriteLine(productPrice);
            // 50

            string productName = (string)o.SelectToken("Manufacturers[1].Products[0].Name");

            Console.WriteLine(productName);
            Console.Write("\n\r");

        }

        private static void Serialize_JSON_to_a_file()
        {
            Console.WriteLine("==Serialize_JSON_to_a_file");

            Account account = new Account
            {
                Email = "james@example.com",
                //Active = true,
                CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                Roles = new List<string>
                {
                    "User",
                    "Admin"
                }
            };

            File.WriteAllText(@"./account.json", JsonConvert.SerializeObject(account, Formatting.Indented));

            List<Account> list_account = new List<Account>();
            list_account.Add(account);
            list_account.Add(account);

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@"./account_list.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, list_account);
            }
        }


        private static void Deserialize_JSON_from_a_file()
        {
            // read file into a string and deserialize JSON to a type
            Account Account1 = JsonConvert.DeserializeObject<Account>(File.ReadAllText(@"./account.json"));

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(@"./account.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Account Account2 = (Account)serializer.Deserialize(file, typeof(Account));
            }

            // read file into a string and deserialize JSON to a type
            List<Account> AccountList1 = JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(@"./account_list.json"));

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(@"./account_list.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Account> AccountList2 = (List<Account>)serializer.Deserialize(file, typeof(List<Account>));
            }


        }



        private static void Serialize_JSON_to_a_file_dictionary_test()
        {
            Console.WriteLine("==Serialize_JSON_to_a_file");

            Dictionary<string, int[]> ScoreTracking_date = new Dictionary<string, int[]>();

            ScoreTracking_date.Add("a", new int[] { 1, 2, 3 });
            ScoreTracking_date.Add("b", new int[] { 4, 5, 6 });
            ScoreTracking_date.Add("c", new int[] { 7, 8, 9 });


            File.WriteAllText(@"./ScoreTracking_date.json", JsonConvert.SerializeObject(ScoreTracking_date, Formatting.Indented));

        }

        private static void Deserialize_JSON_from_a_file_dictionary_test()
        {
            Dictionary<string, int[]> ScoreTracking_date
                = JsonConvert.DeserializeObject<Dictionary<string, int[]>>(File.ReadAllText(@"./ScoreTracking_date.json"));
        }

    }
}
