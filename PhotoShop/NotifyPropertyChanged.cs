using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PhotoShop
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {

        protected NotifyPropertyChanged()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}