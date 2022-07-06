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
using System.Windows.Interop;
using Microsoft.Xaml.Behaviors;
using HM.Wpf;

namespace WpfTestGround2 {
    public class DragSortInListBoxBehaviour : Behavior<ItemsControl> {
        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.DragOver += AssociatedObject_DragOver;
            AssociatedObject.Drop += AssociatedObject_Drop;
            AssociatedObject.DragLeave += AssociatedObject_DragLeave;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;

        }
        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.DragOver -= AssociatedObject_DragOver;
            AssociatedObject.Drop -= AssociatedObject_Drop;
            AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
        }

        private bool _isDragging;
        public Type ItemControlType { get; set; }
        private void AssociatedObject_Drop(object sender, DragEventArgs e) {
        }
        private void AssociatedObject_DragOver(object sender, DragEventArgs e) {

        }
        private void AssociatedObject_DragLeave(object sender, DragEventArgs e) {

        }
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var r = VisualTreeHelper.HitTest(AssociatedObject, e.GetPosition(AssociatedObject));
            if (r is null) {
                return;
            }
            var item = VisualTreeUtil.FindVisualParent(r.VisualHit, ItemControlType);

            System.Diagnostics.Debug.WriteLine($"{item}"); // debug output
            System.Diagnostics.Debug.WriteLine($"find = {item}"); // debug output
        }

    }
    public class DragInCanvasBehaviour : Behavior<UIElement> {
        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }
        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }

        private Canvas? _canvas;
        private bool _isDragging;
        private Point _mouseOffset;
        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (_isDragging) {
                AssociatedObject.ReleaseMouseCapture();
                _isDragging = false;
            }
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e) {
            if (_isDragging) {
                var point = e.GetPosition(_canvas);

                AssociatedObject.SetValue(Canvas.TopProperty, point.Y - _mouseOffset.Y);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X - _mouseOffset.X);
            }
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (_canvas is null) {
                _canvas = (Canvas)VisualTreeHelper.GetParent(AssociatedObject);
            }
            _isDragging = true;
            _mouseOffset = e.GetPosition(AssociatedObject);
            AssociatedObject.CaptureMouse();
        }

    }

    public class TranslateTransformToRectViewboxVisualBrushConverter : IValueConverter {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var translate = value as TranslateTransform;
            if (translate != null) {
                return new Rect(translate.X, translate.Y, 0d, 0d);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public string[] StringData { get; } = {
            "AAA",
            "BBB",
            "CCC",
            "DDD",
            "EEE"
        };

        public MainWindow() {
            InitializeComponent();
        }

        private void HButton_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("hello");
        }
    }
}
