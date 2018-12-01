using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

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
                obj.BringIntoView();
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
                        parent.UpdateLayout();
                    }
                }

                if (element.Parent is LayoutGroup parentLayoutGroup && parentLayoutGroup.View == LayoutGroupView.Tabs)
                {
                    parentLayoutGroup.SelectTab(element);                    
                }

                if (element.Parent is FrameworkElement parentElement)
                    FocusTabbedUIElement(parentElement);
            }
        }

        private static void FindDependencyObjectWithBindingToPropertyRecursive(string propertyName, DependencyObject dependencyObject)
        {
            if (dependencyObject is GridControl gridControl)
            {
                var name = gridControl.Name;
            }
            //var dependencyProperties = new List<DependencyProperty>();
            //var properites = MarkupWriter.GetMarkupObjectFor(dependencyObject).Properties.ToList();            
            //dependencyProperties.AddRange(properites.Where(x => x.DependencyProperty != null).Select(x => x.DependencyProperty).ToList());
            //dependencyProperties.AddRange(properites.Where(x => x.IsAttached).Select(x => x.DependencyProperty).ToList());          

            var dependencyProperties = GetAttachedProperties(dependencyObject);

            var bindings = dependencyProperties.Select(x => BindingOperations.GetBindingBase(dependencyObject, x)).Where(x => x != null).ToList();

            Predicate<Binding> condition = binding => binding != null && binding.Path.Path == propertyName;

            foreach (var bindingBase in bindings)
            {
                if (bindingBase is Binding)
                {
                    if (condition(bindingBase as Binding))
                    {
                        _obj = dependencyObject;                        
                        return;
                    }
                }
            }

            var children = LogicalTreeHelper.GetChildren(dependencyObject).OfType<DependencyObject>().ToList();
            //var children = LayoutTreeHelper.GetLogicalChildren(dependencyObject).OfType<DependencyObject>().ToList();
            if (children.Count == 0)
                return;

            foreach (var child in children)
            {
                FindDependencyObjectWithBindingToPropertyRecursive(propertyName, child);
                if (_obj != null)
                    return;
            }
        }

        private static IList<DependencyProperty> GetAttachedProperties(DependencyObject obj)
        {
            List<DependencyProperty> result = new List<DependencyProperty>();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(obj,
                new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                DependencyPropertyDescriptor dpd =
                    DependencyPropertyDescriptor.FromProperty(pd);

                if (dpd != null)
                {
                    result.Add(dpd.DependencyProperty);
                }
            }

            return result;
        }
    }
}
