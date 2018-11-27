using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup.Primitives;

namespace FocusToElementTest.Helper
{
    public static class FocusHelper1
    {

        static DependencyObject _obj;

        public static void FocusToBindedProperty(DependencyObject dependencyObject, string propertyName)
        {
            _obj = null;
            FindDependencyObjectWithBindingToPropertyRecursive(propertyName, dependencyObject);    
            FocusTabbedUIElement(_obj);
            if (_obj is FrameworkElement obj)
            {
                obj.Focus();
                Debug.WriteLine($"Focusing to {_obj.GetType()} in Layout Item : {((LayoutItem)((FrameworkElement)_obj).Parent).Label}");
            }
        }

        static void FocusTabbedUIElement(DependencyObject obj)
        {
            if (obj is FrameworkElement element)
            {
                if (element.Parent is DXTabItem tabItem)
                {
                    if (tabItem.Parent is DXTabControl parent)
                    {
                        parent.SelectedItem = tabItem;
                        Debug.WriteLine($"Selecting tab Item : {tabItem.Header}");
                        parent.UpdateLayout();
                    }
                }

                if (element.Parent is LayoutGroup parentLayoutGroup && parentLayoutGroup.View == LayoutGroupView.Tabs)
                {
                    parentLayoutGroup.SelectTab(element);
                    Debug.WriteLine($"Selecting group tab Item : {((LayoutGroup)element).Header}");
                }

                if (element.Parent is FrameworkElement parentElement)
                    FocusTabbedUIElement(parentElement);
            }
        }

        private static void FindDependencyObjectWithBindingToPropertyRecursive(string propertyName, DependencyObject dependencyObject)
        {           

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
                        _obj = dependencyObject;
                        Debug.WriteLine($"Found {_obj.GetType()} in Layout Item : {((LayoutItem)((FrameworkElement)_obj).Parent).Label}");
                        return;
                    }
                }
            }

            var children = LogicalTreeHelper.GetChildren(dependencyObject).OfType<DependencyObject>().ToList();
            if (children.Count == 0)
                return;

            foreach (var child in children)
            {
                FindDependencyObjectWithBindingToPropertyRecursive(propertyName, child);
                if (_obj != null)
                    return;
            }
        }
    }
}
