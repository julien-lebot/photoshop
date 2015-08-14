using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PhotoShop
{
    public static class DependencyObjectExtensions
    {
        public static bool IsValid(this DependencyObject node)
        {
            // Check if dependency object was passed
            if (node != null)
            {
                // Check if dependency object is valid.
                // NOTE: Validation.GetHasError works for controls that have validation rules attached 
                var isValid = !Validation.GetHasError(node);
                if (!isValid)
                {
                    // If the dependency object is invalid, and it can receive the focus,
                    // set the focus
                    var element = node as IInputElement;
                    if (element != null)
                    {
                        Keyboard.Focus(element);
                    }
                    return false;
                }
            }

            // If this dependency object is valid, check all child dependency objects
            return node != null && LogicalTreeHelper.GetChildren(node).OfType<DependencyObject>().All(IsValid);
        }
    }
}