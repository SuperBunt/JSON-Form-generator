using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormBuilder.Program;

namespace FormBuilder
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Question
    {
        public string Key { get; set; }
        public string ControlType { get; set; }
        public string Label { get; set; }
        public string Placeholder { get; set; }
        public string Tip { get; set; }
        public string Value { get; set; }
        public bool Visible { get; set; }
        public bool Required { get; set; }
        public string Localizationkey { get; set; }
        public string placeholderLocalizationkey { get; set; }
        public string tipLocalizationkey { get; set; }
        public string RegSysKey { get; set; }
        public string RegSysType { get; set; }
        public List<Question> Questions { get => questions; set => questions = value; }

        private List<Question> questions;

        public Question()
        {
            Key = Guid.NewGuid().ToString();
            ControlType = "textarea";
            Visible = true;
            Label = "Edit Label";
            Required = false;
        }
        public Question(string type)
        {
            questions = new List<Question>();
            Key = Guid.NewGuid().ToString();
            ControlType = type;
            Visible = true;
        }
    }


    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Step : Question
    {
        public int Order { get; set; }
        public Step(int order)
        {
            Questions = new List<Question>();
            Key = Guid.NewGuid().ToString();
            Order = order;
            Visible = true;
            Order = order;
            ControlType = "step";
        }

        public void AddDetailsSection()
        {
            Question section = new Question("section");
            section.Label = "Society Details";
            Question societyAffiliated = new Radio();
            section.Questions.Add(societyAffiliated);
            Question category = new Dropdown(3);
            section.Questions.Add(category);
            Questions.Add(section);
        }

        public void AddAddressSection()
        {
            AddressSection section = new AddressSection();
            Questions.Add(section);
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Section : Question
    {
        public Section()
            : base()
        {
            Questions = new List<Question>();
            Key = Guid.NewGuid().ToString();
            // Order = order;
            Visible = true;
            Label = "Edit Section label";
            ControlType = "section";
        }

        public void AddQuestionsToSection(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Question toAdd = new Question();
                Questions.Add(toAdd);
            }
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class AddressSection : Section
    {
        public AddressSection()
        {
            Label = "Society Address";
            Questions = new List<Question>();
            Question search = new Question()
            {
                Placeholder = "Click here to enter your address",
                ControlType = "quick-autocomplete-address",
                Required = false,
                Label = "Search Address"
            };
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Fetchsource
    {
        public string Source { get; set; }
        public Option Fetchkeys { get; set; }
        public DisplayInformation Display { get; set; }
        public AdditionalCallInformation AdditionalCallInfo { get; set; }

        public Fetchsource()
        {
            Source = @"autoaddress/autoaddresssearch";
            Fetchkeys = new StringOption()
            {
                Key = "countryId",
                Value = "@ ------ Add reference ------"
            };
            Display = new DisplayInformation() { Seperator = " - ", Keys = new List<string>() { "displayname" } };
            AdditionalCallInfo = new AdditionalCallInformation() {
                CallRequirementsResult = "",
                CallRequirements = new List<Option>(),
                RequestKey = "displayName",
                UrlForCall = @"autoaddress/autoaddressfind"
            };

        }

        public struct DisplayInformation
        {
            public string Seperator { get; set; }
            public List<string> Keys { get; set; }
        }

        public struct AdditionalCallInformation
        {
            public string CallRequirementsResult { get; set; }
            public List<Option> CallRequirements { get; set; }
            public string RequestKey { get; set; }
            public string UrlForCall { get; set; }
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Radio : Question
    {
        public string Orientation { get; set; }
        public List<Option> Options { get => options; set => options = value; }

        private List<Option> options;

        public Radio()
        {
            ControlType = "radio";
            Orientation = "horizontal";
            Required = true;
            RegSysKey = "";
            options = new List<Option>();
            AddBooleanOption();
        }

        private void AddBooleanOption()
        {
            BoolOption option1 = new BoolOption();
            option1.Key = true;
            option1.Value = "Option 1";
            BoolOption option2 = new BoolOption();
            option2.Key = false;
            option2.Value = "Option 2";
            options.Add(option1);
            options.Add(option2);
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class Dropdown : Question
    {
        public List<Option> Options { get => options; set => options = value; }

        private List<Option> options;
        public Dropdown(int? num)
        {
            ControlType = "dropdown";
            RegSysType = "number";
            Value = "Please Select";
            options = new List<Option>();
            AddOptions(num);
        }

        private void AddOptions(int? num = 2)
        {
            for (int i = 0; i < num; i++)
            {
                IntOption option = new IntOption();
                option.Key = num.Value - 1;
                option.Value = string.Format("Option {0}....", num.Value - 1);
            }
        }
    }

    #region Options
    public class Option
    {
        public string Operator { get; set; }
        public string LocalizationKey { get; set; }
        public string Value { get; set; }
    }

    public class BoolOption : Option
    {
        public bool Key { get; set; }
    }

    public class IntOption : Option
    {
        public int Key { get; set; }
    }

    public class StringOption : Option
    {
        public string Key { get; set; }
    }
    #endregion
}
//public class Textbox : ControlType
//{
//    public override string ControlType => "textbox";
//}


//public class Textarea : ControlType
//{
//    public override string ControlType => "textarea";

//    public override string Key { get ; set; }

//    public Textarea()
//    {
//        Key = Guid.NewGuid().ToString();
//    }

//}
//public class Radio : ControlType
//{
//    public override string ControlType => "radio";
//    //public Radio()
//    //    : base()
//    //{
//    //    Type = "Radio";
//    //}
//}
//public class Checkbox : ControlType
//{
//    public override string ControlType => "Checkbox";
//}

//public class SummaryForm : ControlType
//{
//    public override string ControlType => "summary-form";
//}


//public class CheckboxList : ControlType
//{
//    public CheckboxList()
//        : base()
//    {
//        Type = "Textarea";
//    }
//}
//public class Dropdown : ControlType
//{
//    public Dropdown()
//        : base()
//    {
//        Type = "Dropdown";
//    }
//}
//public class Date : ControlType
//{
//    public Date()
//        : base()
//    {
//        Type = "Date";
//    }
//}
//public class MultiSelect : ControlType
//{
//    public MultiSelect()
//        : base()
//    {
//        Type = "MultiSelect";
//    }
//}
//public class FileUpload : ControlType
//{
//    public FileUpload()
//        : base()
//    {
//        Type = "FileUpload";
//    }
//}

//public class Numeric : ControlType
//{
//    public Numeric()
//        : base()
//    {
//        Type = "Numeric";
//    }
//}
//public class QuickAutocomplete : ControlType
//{
//    public QuickAutocomplete()
//        : base()
//    {
//        Type = "QuickAutocomplete";
//    }
//}

