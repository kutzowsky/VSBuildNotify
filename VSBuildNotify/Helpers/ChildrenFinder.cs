using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VSBuildNotify.Helpers
{
    public static class ChildrenFinder
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int index = 0; index < VisualTreeHelper.GetChildrenCount(dependencyObject); index++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
