using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup.Primitives;

namespace FocusToElementTest.Helper
{
    public static class FocusHelper
    {        
        static bool _focused;    

        public static void FocusToBindedProperty(DependencyObject dependencyObject, string propertyName)
        {           
            _focused = false;
            
            GetDependencyObjectsWithBindingToPropertyRecursive(propertyName, dependencyObject);
        }

        private static void GetDependencyObjectsWithBindingToPropertyRecursive(string propertyName, DependencyObject dependencyObject)
        {         
            if (dependencyObject is DXTabItem tabItem)
            {
                if (tabItem.Parent is DXTabControl parent)
                {
                    parent.SelectedItem = tabItem;
                    parent.UpdateLayout();
                }
            }

            if (dependencyObject is FrameworkElement layoutGroup)
            {
                if (layoutGroup.Parent is LayoutGroup parentLayoutGroup && parentLayoutGroup.View == LayoutGroupView.Tabs)
                    parentLayoutGroup.SelectTab(layoutGroup);
            }

            var dependencyProperties = new List<DependencyProperty>();
            dependencyProperties.AddRange(MarkupWriter.GetMarkupObjectFor(dependencyObject).Properties.Where(x => x.DependencyProperty != null).Select(x => x.DependencyProperty).ToList());
            dependencyProperties.AddRange(
                MarkupWriter.GetMarkupObjectFor(dependencyObject).Properties.Where(x => x.IsAttached).Select(x => x.DependencyProperty).ToList());

            var bindings = dependencyProperties.Select(x => BindingOperations.GetBindingBase(dependencyObject, x)).Where(x => x != null).ToList();

            Predicate<Binding> condition = binding => binding != null && binding.Path.Path == propertyName;

            foreach (var bindingBase in bindings)
            {
                if (bindingBase is Binding)
                {
                    if (condition(bindingBase as Binding))
                    {
                        ((UIElement)dependencyObject).Focus();
                        _focused = true;
                        return;
                    }                   
                }               
            }          

            var children = LogicalTreeHelper.GetChildren(dependencyObject).OfType<DependencyObject>().ToList();
            if (children.Count == 0)
                return;

            foreach (var child in children)
            {               
                GetDependencyObjectsWithBindingToPropertyRecursive(propertyName, child);
                if (_focused)
                    return;
            }
        }
    }
}
