using FocusToElementTest.Model;
using Prism.Mvvm;

namespace FocusToElementTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {        
        public MainWindowViewModel()
        {
            Entity = new TestModel();            
        }

        public TestModel Entity { get; set; }
    }
}
