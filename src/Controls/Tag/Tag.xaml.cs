using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Awe.UI.Controls
{
    public class Tag : ContentControl
    {


        public TagTypeEnum TagType
        {
            get { return (TagTypeEnum)GetValue(TagTypeProperty); }
            set { SetValue(TagTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TagType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagTypeProperty =
            DependencyProperty.Register("TagType", typeof(TagTypeEnum), typeof(Tag), new PropertyMetadata());


    }
}
