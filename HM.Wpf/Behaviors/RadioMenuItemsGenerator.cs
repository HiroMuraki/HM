using HM.Wpf.ValueConverters;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace HM.Wpf.Behaviors;

public interface ILabelValueGetter
{
    string GetLabelValue(object value);
}

[ContentProperty(nameof(RadioMenuItemDefines))]
public class RadioMenuItemsGenerator : Behavior<ContextMenu>
{
    public ILabelValueGetter? LabelValueGetter { get; }
    public List<RadioMenuItemInfo> RadioMenuItemDefines { get; } = new();

    protected override void OnAttached()
    {
        base.OnAttached();

        for (int i = 0; i < RadioMenuItemDefines.Count; i++)
        {
            ArgumentNullException.ThrowIfNull(RadioMenuItemDefines[i].EnumType);

            var enumValues = Enum.GetValues(RadioMenuItemDefines[i].EnumType!);

            foreach (object? item in enumValues)
            {
                var binding = new Binding()
                {
                    Path = new PropertyPath(RadioMenuItemDefines[i].BindingProperty),
                    Converter = BooleanToValueConverter.Default,
                    ConverterParameter = item,
                };

                var radioMenuItem = new RadioMenuItem()
                {
                    Header = LabelValueGetter?.GetLabelValue(item) ?? item.ToString()
                };
                radioMenuItem.SetBinding(MenuItem.IsCheckedProperty, binding);

                AssociatedObject.Items.Add(radioMenuItem);
            }

            if (i != RadioMenuItemDefines.Count - 1)
            {
                AssociatedObject.Items.Add(new Separator());
            }
        }
    }
}