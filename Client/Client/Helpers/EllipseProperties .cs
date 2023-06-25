using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Helpers
{
    public class EllipseProperties : DependencyObject
    {
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.RegisterAttached("Row", typeof(int), typeof(EllipseProperties), new PropertyMetadata(0));

        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached("Column", typeof(int), typeof(EllipseProperties), new PropertyMetadata(0));

        public static int GetRow(DependencyObject obj)
        {
            return (int)obj.GetValue(RowProperty);
        }

        public static void SetRow(DependencyObject obj, int value)
        {
            obj.SetValue(RowProperty, value);
        }

        public static int GetColumn(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnProperty);
        }

        public static void SetColumn(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnProperty, value);
        }
    }

}
