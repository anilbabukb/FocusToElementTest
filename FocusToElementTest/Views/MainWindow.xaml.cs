using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using FocusToElementTest.Helper;
using System.Windows;
using System.Windows.Controls;

namespace FocusToElementTest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void BarItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            FlyoutControl.PlacementTarget = e.Link.LinkControl as FrameworkElement;
            FlyoutControl.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            var error = ((Button)sender).Tag;
            if (error != null)
            {
                var propertyName = error as string;
                FocusHelper.FocusToBindedProperty(this, $"Entity.{propertyName}");
                FlyoutControl.IsOpen = false;
            }
        }

    }


}
