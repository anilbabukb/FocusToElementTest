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

    }
}

