using FluentValidation;
using FluentValidation.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace FocusToElementTest.Model
{
    public abstract class ModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        protected virtual IValidator FluentValidator => null;

        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();



        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);

            SetValue<T>(propertyName, value);
        }

        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value;
            NotifyPropertyChanged(propertyName);
        }


        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }


        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

        protected virtual string OnValidate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            string error = string.Empty;
            var value = GetValue(propertyName);
            var results = new List<ValidationResult>(1);
            var result = Validator.TryValidateProperty(
                value,
                new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results);

            var fluentValidationResult = FluentValidator != null ? 
                FluentValidator.Validate(new FluentValidation.ValidationContext(this, new PropertyChain(), new MemberNameValidatorSelector(new[] { propertyName}))) :
                null;

            if (!result)
            {
                var validationResult = results.First();
                error = validationResult.ErrorMessage;                
                UpdateValidationSummary(propertyName);
                ValidationSummary.Add(new ValidationSummary { ErrorMessage = error, PropertyName = propertyName });
            }
            else if(fluentValidationResult.Errors.Count > 0)
            {
                var validationResult = fluentValidationResult.Errors.First();
                error = validationResult.ErrorMessage;
                UpdateValidationSummary(propertyName);
                ValidationSummary.Add(new ValidationSummary { ErrorMessage = error, PropertyName = propertyName });
            }
            else
            {
                UpdateValidationSummary(propertyName);
            }

            return error;
        }

        private void UpdateValidationSummary(string propertyName)
        {
            var summary = ValidationSummary.SingleOrDefault(i => i.PropertyName == propertyName);
            if (summary != null)
                ValidationSummary.Remove(summary);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
                
            }
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                string propertyName = GetPropertyName(propertySelector);
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                //return null;
                throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return OnValidate(propertyName);
            }
        }

        private string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        private object GetValue(string propertyName)
        {
            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                var propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);
                if (propertyDescriptor == null)
                {
                    throw new ArgumentException("Invalid property name", propertyName);
                }

                value = propertyDescriptor.GetValue(this);
                _values.Add(propertyName, value);
            }

            return value;
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,   
            // public, instance property on this object. 
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
        public ObservableCollection<ValidationSummary> ValidationSummary { get; set; } = new ObservableCollection<ValidationSummary>();
        
    }
}

