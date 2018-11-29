using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using FocusToElementTest.Helper;
using FocusToElementTest.Model;
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
            var editor = (Button)sender;
            var rowData = (editor.DataContext as GridCellData)?.RowData;
            var propertyName = (rowData.Row as ValidationSummary)?.PropertyName;
            FocusHelper1.FocusToBindedProperty(this, $"Entity.{propertyName}");                     
            FlyoutControl.IsOpen = false;
        }

        private void HyperlinkEditSettings_RequestNavigation(object sender, DevExpress.Xpf.Editors.HyperlinkEditRequestNavigationEventArgs e)
        {
            var editor = (HyperlinkEdit)sender;
            var rowData = (editor.DataContext as GridCellData)?.RowData;
            var propertyName = (rowData.Row as ValidationSummary)?.PropertyName;
            FocusHelper1.FocusToBindedProperty(this, $"Entity.{propertyName}");
            FlyoutControl.IsOpen = false;
        }
    }


}
