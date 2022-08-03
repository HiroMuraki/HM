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
    [TemplatePart(Name = "PART_TrackBorder", Type = typeof(Border))]
    public class GSlider : Slider
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(GSlider), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.Register(nameof(AccentBrush), typeof(Brush), typeof(GSlider), new PropertyMetadata(null));

        public Brush AccentBrush
        {
            get
            {
                return (Brush)GetValue(AccentBrushProperty);
            }
            set
            {
                SetValue(AccentBrushProperty, value);
            }
        }
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

        static GSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GSlider), new FrameworkPropertyMetadata(typeof(GSlider)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ((Border)Template.FindName("PART_TrackBorder", this)).CornerRadius = new CornerRadius()
            {
                TopLeft = 0,
                TopRight = CornerRadius.TopRight,
                BottomLeft = 0,
                BottomRight = CornerRadius.BottomRight
            };
        }
    }
}
