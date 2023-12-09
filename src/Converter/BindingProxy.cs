using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Awe.UI.Converter
{
    public sealed class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(default(object)));

        public object Data
        {
            get => (object)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new BindingProxy();

        public override string ToString() => Data is FrameworkElement fe
            ? $"BindingProxy: {fe.Name}"
            : $"Binding Proxy: {Data?.GetType().FullName}";
    }
}
