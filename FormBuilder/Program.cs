using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Form myForm = new Form();

            //myForm.SearchForDuplicateGuids();
            Console.WriteLine("Form Builder");
            Console.WriteLine("Enter version:\t");
            myForm.Version = Console.ReadLine();
            Console.WriteLine("Enter Display name:\t");
            myForm.DisplayName = Console.ReadLine();

            Console.WriteLine("---------- Submission Details ------------------");
            myForm.SubmissionDetails = new SubmissionDetails();
            Console.WriteLine("Enter Type id:\t");
            myForm.SubmissionDetails.SubmissionTypeId = Console.ReadLine();
            Console.WriteLine("Enter Type description:\t");
            myForm.SubmissionDetails.SubmissionTypeDesc = Console.ReadLine();
            Console.WriteLine("Enter related register id:\t");
            myForm.SubmissionDetails.RelatedRegisterId = Console.ReadLine();
            Console.WriteLine("Enter num of RegEdits");
            string num = Console.ReadLine();
            int val = Convert.ToInt16(num);
            val = Convert.ToInt16(num);
            myForm.CreateRegEdits(val);

            Console.WriteLine("---------- Steps ------------------");
            Console.WriteLine("Enter num of tabs");
            num = Console.ReadLine();
            val = Convert.ToInt16(num);
            myForm.CreateSteps(val);
            


            string jsonIgnoreNullValues = JsonConvert.SerializeObject(myForm, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine(jsonIgnoreNullValues);



            // TO DO: Export JSON to a file or open in text editor
            string path = @"C:\Users\pec\source\repos\FormBuilder\FormBuilder\sample.json";
            if (File.Exists(path))
            {
                File.WriteAllText(path, jsonIgnoreNullValues);
            }

            //Console.ReadKey();
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class Form
        {

            [JsonProperty(Order = 0)]
            public string Version { get; set; }
            [JsonProperty(Order = 1)]
            public string DisplayName { get; set; }
            [JsonProperty(Order = 2)]
            public SubmissionDetails SubmissionDetails { get; set; }
            // progressLinks
            [JsonProperty(Order = 3)]
            public List<Step> steps;
            [JsonIgnore]
            public int NumSubmissionRegEdits { get; set; }
            public Form()
            {
                steps = new List<Step>();
            }

            public void CreateRegEdits(int NumSubmissionRegEdits)
            {
                string number, name;
                int num;
                for (int i = 0; i < NumSubmissionRegEdits; i++)
                {
                    // Generate regSysKey, name, registyereEditId
                    Console.WriteLine("RegEdit {0}.\tEnter regedit name?", i + 1);
                    name = Console.ReadLine();
                    Console.WriteLine("Enter registerEditId:");
                    number = Console.ReadLine();
                    num = Convert.ToInt16(number);
                    SubmissionRegisterEdit SubRegEdit = new SubmissionRegisterEdit(num, name);
                    SubmissionDetails.SubmissionRegisterEdits.Add(SubRegEdit);
                }
            }

            public void SearchForDuplicateGuids()
            {
                string path = @"C:\Users\pec\source\repos\FormBuilder\FormBuilder\sample.json";
                string jsonText;
                using (StreamReader r = new StreamReader(path))
                {
                    jsonText = r.ReadToEnd();
                }


                var guids = Regex.Matches(jsonText, @"(\{){0,1}(?<![\@]{1})([0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1})")
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .ToList();  //Match all substrings in guids

                Console.WriteLine("count of guids: {0}", guids.Count());

                var duplicates = guids.GroupBy(x => x).Where(g => g.Count() > 1).SelectMany(r => r).Distinct();

                Console.WriteLine("count of duplicates: {0}", duplicates.Count());

                if (duplicates.Count() > 0)
                {
                    foreach (var item in duplicates)
                    {
                        Console.WriteLine(item);
                    }
                }
                else
                    Console.WriteLine("No Duplicates.");

            }

            public void CreateSteps(int num)
            {
                string s;
                int numSections;
                short numQuestions;
                for (int i = 0; i < num; i++)
                {
                    Step tab = new Step(i + 1);
                    Console.WriteLine("Enter tab label:");
                    s = Console.ReadLine();

                    tab.Label = s;
                    if (s.Equals("Details"))
                    {
                        tab.AddDetailsSection();
                        tab.AddAddressSection();
                    }
                    else
                    {
                        Console.WriteLine("How many sections in tab {0}?", i + 1);
                        s = Console.ReadLine();
                        numSections = Convert.ToInt16(s);
                        for (int k = 0; k < numSections; k++)
                        {
                            Section section = new Section();
                            tab.Questions.Add(section);
                            Console.WriteLine("How many question in section {0}?", k + 1);
                            s = Console.ReadLine();
                            numQuestions = Convert.ToInt16(s);
                            section.AddQuestionsToSection(numQuestions);
                        }
                    }
                    steps.Add(tab);
                }
            }


            public override string ToString()
            {
                return string.Format("{0}, Type id = {1}", DisplayName, SubmissionDetails.SubmissionTypeId);
            }
        }

        public class SubmissionDetails
        {
            public string SubmissionTypeId { get; set; }
            public string SubmissionTypeDesc { get; set; }
            public string RelatedRegisterId { get; set; }
            public List<SubmissionRegisterEdit> SubmissionRegisterEdits;

            public SubmissionDetails()
            {
                SubmissionRegisterEdits = new List<SubmissionRegisterEdit>();
            }
        }



        public class SubmissionRegisterEdit
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public int RegisterEditId { get; set; }

            public SubmissionRegisterEdit(int id, string name)
            {
                Key = Guid.NewGuid().ToString();
                Name = name;
                RegisterEditId = id;
            }
        }

    }
}
