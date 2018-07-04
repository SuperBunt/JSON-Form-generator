using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormBuilder.Program;

namespace FormBuilder
{
    public class Question
    {
        public string Key { get; set; }
        public string ControlType { get; set; }
        public string Label { get; set; }
        public string Placeholder { get; set; }
        public string Tip { get; set; }
        public string Value { get; set; }
        public string Visible { get; set; }
        public string Required { get; set; }
        public string Localizationkey { get; set; }
        public string placeholderLocalizationkey { get; set; }
        public string tipLocalizationkey { get; set; }
        public string RegSysKey { get; set; }
        public string RegSysType { get; set; }
        public List<Question> Questions { get => questions; set => questions = value; }

        List<Option> options;
        private List<Question> questions;

        public Question(string type)
        {
            questions = new List<Question>();
            Key = Guid.NewGuid().ToString();
            ControlType = type;
        }
    }
    public abstract class ControlType
    {
        public abstract string controlType { get; } 
    }

    public class Step : ControlType
    {
        public override string controlType => "Step";
        public string Key { get; set; }
        public string Localizationkey { get; set; }
        public string Label { get; set; }
        public bool Visible { get; set; }
        //public string controlType { get; set; }
        public List<Question> Questions;

        public Step()
            : base()
        {
            Questions = new List<Question>();
            Key = Guid.NewGuid().ToString();
        }
    }

    public class Textbox : ControlType
    {
        public override string controlType => "Textbox";
    
    }
    public class Textarea : ControlType
    {
        public override string controlType => "Textarea";
        
    }
    public class Radio : ControlType
    {
        public override string controlType => "Radio";
        //public Radio()
        //    : base()
        //{
        //    Type = "Radio";
        //}
    }
    public class Checkbox : ControlType
    {
        public override string controlType => "Checkbox";
    }

    public class SummaryForm : ControlType
    {
        public override string controlType => "summary-form";
    }

    public class Option
    {
        public int Key { get; set; }
        public string LocalizationKey { get; set; }
        public string Value { get; set; }
    }
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
}
