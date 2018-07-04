using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Form myForm = new Form();
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
            Console.WriteLine("Enter num of steps");
            num = Console.ReadLine();
            val = Convert.ToInt16(num);
            myForm.CreateSteps(val);
            
            Console.WriteLine("---------- Summary Steps ------------------");
            Console.WriteLine("Enter num of summary steps");
            num = Console.ReadLine();
            val = Convert.ToInt16(num);
            myForm.CreateSummarySteps(val);

            string jsonIgnoreNullValues = JsonConvert.SerializeObject(myForm, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine(jsonIgnoreNullValues);

            // TO DO: Export JSON to a file or open in text editor

            Console.ReadKey();
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
            [JsonProperty(Order = 4)]
            public List<Step> summarySteps;
            [JsonIgnore]
            public int NumSubmissionRegEdits { get; set; }
            public Form()
            {
                steps = new List<Step>();
                summarySteps = new List<Step>();
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

            public void CreateSteps(int num)
            {
                string s;
                int numQuestions;
                for (int i = 0; i < num; i++)
                {
                    Step toAdd = new Step();
                    //List<Question> questions = new List<Question>();
                    Console.WriteLine("How many questions in step {0}?", i+1);
                    s = Console.ReadLine();
                    numQuestions = Convert.ToInt16(s);
                    for (int j = 0; j < numQuestions; j++)
                    {
                        // Add a question
                        Console.WriteLine("question {0}.\tEnter control type?",j+1);
                        s = Console.ReadLine();
                        Question question = new Question(s);
                        question.Localizationkey = "";
                        question.RegSysKey = "";
                        question.RegSysType = "";
                        //questions.Add(question);
                        toAdd.Questions.Add(question);
                    }                    
                    toAdd.Key = Guid.NewGuid().ToString();
                    toAdd.Visible = true;
                    steps.Add(toAdd);
                }                
            }

            public void CreateSummarySteps(int num)
            {
                for (int i = 0; i < num; i++)
                {
                    Step toAdd = new Step();
                    summarySteps.Add(toAdd);
                }
            }

            public override string ToString()
            {
                return string.Format("{0}, Type id = {1}",DisplayName,SubmissionDetails.SubmissionTypeId);
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

            //public void CreateRegEdits(int NumSubmissionRegEdits)
            //{
            //    string number, name;
            //    int num;
            //    for (int i = 0; i < NumSubmissionRegEdits; i++)
            //    {
            //        // Generate regSysKey, name, registyereEditId
            //        Console.WriteLine("RegEdit {0}.\tEnter regedit name?", i + 1);
            //        name = Console.ReadLine();
            //        Console.WriteLine("Enter registerEditId:");
            //        number = Console.ReadLine();
            //        num = Convert.ToInt16(number);
            //        SubmissionRegisterEdit SubRegEdit = new SubmissionRegisterEdit(num, name);
            //        SubmissionRegisterEdits.Add(SubRegEdit);
            //    }
            //}

        }

        //public class CommonStep
        //{
        //    public string key { get; set; }
        //    public string Localizationkey { get; set; }
        //    public string Label { get; set; }
        //    public string ControlType { get; set; }
        //    public List<Question> Questions { get; set; }

        //    public CommonStep()
        //    {
        //        Questions = new List<Question>();
        //    }
        //}
        //public class Step : CommonStep
        //{
        //    public int Order { get; set; }
        //    public bool Visible { get; set; }
        //}

        

        //public class StepQuestion : Question
        //{
        //    public string Label { get; set; }
        //    public string LocalizationKey { get; set; }
        //    public int? Order { get; set; }
        //    public bool? Visible { get; set; }

        //    //public List<StepSubQuestion> Questions;

        //    public void AddQuestions(int num)
        //    {
        //        StepSubQuestion toAdd = new StepSubQuestion();
                
        //    }

        //}

        //public class SummaryStep : CommonStep
        //{
        //    public string Label { get; set; }
        //    public string LocalizationKey { get; set; }
        //    public bool? Visible { get; set; }
        //}

        

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

        //public class StepSubQuestion
        //{
        //    public string Key { get; set; }
        //    public ControlType ControlType { get; set; }
        //    public string RegSysKey { get; set; }
        //    public string RegSysType { get; set; }
        //}

        //public class SummaryQuestion : Question
        //{
        //    public string Label { get; set; }
        //    public bool Visible { get; set; }

        //}
    }
}
