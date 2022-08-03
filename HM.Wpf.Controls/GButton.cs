using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HM.Wpf.Controls
{
    public class GButton : Button
    {
        public static readonly DependencyProperty CornerRadiusProperty =
             DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(GButton), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register(nameof(FocusedBackground), typeof(Brush), typeof(GButton), new PropertyMetadata(null));

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
        public Brush FocusedBackground
        {
            get
            {
                return (Brush)GetValue(FocusedBackgroundProperty);
            }
            set
            {
                SetValue(FocusedBackgroundProperty, value);
            }
        }

        static GButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GButton), new FrameworkPropertyMetadata(typeof(GButton)));
        }
    }
}
