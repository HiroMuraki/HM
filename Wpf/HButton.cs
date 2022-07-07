using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HM.Wpf
{
    public class HTextBox : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HTextBox), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// comment
        /// </summary>
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

        static HTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HTextBox), new FrameworkPropertyMetadata(typeof(HTextBox)));
        }
    }

    public class HButton : ButtonBase
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HButton), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty BackgroundMouseHoverProperty =
            DependencyProperty.Register(nameof(BackgroundMouseHover), typeof(Brush), typeof(HButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static readonly DependencyProperty BackgroundMouseDownProperty =
            DependencyProperty.Register(nameof(BackgroundMouseDown), typeof(Brush), typeof(HButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static readonly DependencyProperty ForegroundMouseHoverProperty =
            DependencyProperty.Register(nameof(ForegroundMouseHover), typeof(Brush), typeof(HButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static readonly DependencyProperty ForegroundMouseDownProperty =
            DependencyProperty.Register(nameof(ForegroundMouseDown), typeof(Brush), typeof(HButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// comment
        /// </summary>
        public Brush ForegroundMouseDown
        {
            get
            {
                return (Brush)GetValue(ForegroundMouseDownProperty);
            }
            set
            {
                SetValue(ForegroundMouseDownProperty, value);
            }
        }
        /// <summary>
        /// comment
        /// </summary>
        public Brush ForegroundMouseHover
        {
            get
            {
                return (Brush)GetValue(ForegroundMouseHoverProperty);
            }
            set
            {
                SetValue(ForegroundMouseHoverProperty, value);
            }
        }
        /// <summary>
        /// comment
        /// </summary>
        public Brush BackgroundMouseDown
        {
            get
            {
                return (Brush)GetValue(BackgroundMouseDownProperty);
            }
            set
            {
                SetValue(BackgroundMouseDownProperty, value);
            }
        }
        /// <summary>
        /// comment
        /// </summary>
        public Brush BackgroundMouseHover
        {
            get
            {
                return (Brush)GetValue(BackgroundMouseHoverProperty);
            }
            set
            {
                SetValue(BackgroundMouseHoverProperty, value);
            }
        }
        /// <summary>
        /// comment
        /// </summary>
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

        static HButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HButton), new FrameworkPropertyMetadata(typeof(HButton)));
        }
    }
}
