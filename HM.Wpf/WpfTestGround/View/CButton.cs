using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HM.Wpf;

namespace WpfTestGround {
    public class CButton : Button, ISelectable {
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(CButton), new PropertyMetadata(false));

        public bool IsSelected {
            get {
                return (bool)GetValue(IsSelectedProperty);
            }
            set {
                SetValue(IsSelectedProperty, value);
            }
        }

        public CButton() {
            Loaded += (s, e) => {
                var t = Template.FindName("PART_CheckBox", this) as ToggleButton;
                var b = BindingOperations.GetBinding(this, IsSelectedProperty);
                if (t is not null && b is not null) {
                    t.SetBinding(ToggleButton.IsCheckedProperty, b);
                }
            };
        }
    }
}
