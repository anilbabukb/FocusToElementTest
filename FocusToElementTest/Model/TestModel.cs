using System.ComponentModel.DataAnnotations;

namespace FocusToElementTest.Model
{
    public class TestModel : PropertyChangedNotification
    {
        [Required(ErrorMessage = "Enter Value for Property1")]
        public string Property1
        {
            get { return GetValue(() => Property1); }
            set { SetValue(() => Property1, value); }
        }

        [Required(ErrorMessage = "Enter Value for Property2")]        
        public string Property2 {
            get { return GetValue(() => Property2); }
            set { SetValue(() => Property2, value); }
        }

        [Required(ErrorMessage = "Enter Value for Property3")]        
        public string Property3 {
            get { return GetValue(() => Property3); }
            set { SetValue(() => Property3, value); }
        }

        [Required(ErrorMessage = "Enter Value for Property4")]        
        public string Property4 {
            get { return GetValue(() => Property4); }
            set { SetValue(() => Property4, value); }
        }

        [Required(ErrorMessage = "Enter Value for Property5")]
        public string Property5 {
            get { return GetValue(() => Property5); }
            set { SetValue(() => Property5, value); }
        }







        [Required(ErrorMessage = "Enter Value for Prop1")]
        public string Prop1
        {
            get { return GetValue(() => Prop1); }
            set { SetValue(() => Prop1, value); }
        }

        [Required(ErrorMessage = "Enter Value for Prop2")]
        public string Prop2
        {
            get { return GetValue(() => Prop2); }
            set { SetValue(() => Prop2, value); }
        }

        [Required(ErrorMessage = "Enter Value for Prop3")]
        public string Prop3
        {
            get { return GetValue(() => Prop3); }
            set { SetValue(() => Prop3, value); }
        }

        [Required(ErrorMessage = "Enter Value for Prop4")]
        public string Prop4
        {
            get { return GetValue(() => Prop4); }
            set { SetValue(() => Prop4, value); }
        }

        [Required(ErrorMessage = "Enter Value for Prop5")]
        public string Prop5
        {
            get { return GetValue(() => Prop5); }
            set { SetValue(() => Prop5, value); }
        }

    }
}

