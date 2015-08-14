using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace PhotoShop
{
    public class RelayCommand : ICommand
    {
        private ISet<string> _properties;
        private readonly Predicate<object> _predicate;
        private readonly Action<object> _action;

        public RelayCommand(Predicate<object> predicate, Action<object> action)
        {
            _predicate = predicate;
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return _predicate(parameter);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public class NotificationBuilder
        {
            private readonly RelayCommand _relayCommand;

            public NotificationBuilder(RelayCommand relayCommand)
            {
                _relayCommand = relayCommand;
            }

            public NotificationBuilder Monitor<T>(Expression<Func<T>> propertyExpression)
            {
                if (_relayCommand._properties == null)
                {
                    _relayCommand._properties = new HashSet<string>();
                }
                _relayCommand._properties.Add(GetPropertyName(propertyExpression));
                return this;
            }

            private string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
            {
                if (propertyExpression == null)
                    throw new ArgumentNullException(nameof(propertyExpression));

                var body = propertyExpression.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("Invalid argument", nameof(propertyExpression));

                var property = body.Member as PropertyInfo;
                if (property == null)
                    throw new ArgumentException("Argument is not a property", nameof(propertyExpression));

                return property.Name;
            }
        }

        public NotificationBuilder For<T>(T type) where T : INotifyPropertyChanged
        {
            type.PropertyChanged += Type_PropertyChanged;
            return new NotificationBuilder(this);
        }

        private void Type_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_properties != null && _properties.Contains(e.PropertyName))
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void StopMonitoring<T>(T type) where T : INotifyPropertyChanged
        {
            type.PropertyChanged -= Type_PropertyChanged;
        }
    }
}