using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WpfTestGround {
    public class TestVM : ObservableObject {
        private bool _isOn;
        /// <summary>
        /// comment
        /// </summary>
        public bool IsOn {
            get {
                return _isOn;
            }
            set {
                _isOn = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public TestVM TestVM { get; } = new();
        private bool _isOn;

        /// <summary>
        /// IsOn
        /// </summary>
        public bool IsOn {
            get {
                return _isOn;
            }
            set {
                _isOn = value;
                OnPropertyChanged(nameof(IsOn));
            }
        }

        public MainWindow() {
            InitializeComponent();
            var t = Attribute.GetCustomAttributes(typeof(CheckBox));
            foreach (var item in t) {
                if (item is TemplatePartAttribute) {
                    System.Diagnostics.Debug.WriteLine($"{item}"); // debug output
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

        }

        private void Border_MouseMove(object sender, MouseEventArgs e) {

        }
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.S) {

                TestVM.IsOn = !TestVM.IsOn;
            }
        }
    }
}
