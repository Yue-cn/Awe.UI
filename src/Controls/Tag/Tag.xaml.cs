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



        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Tag), new PropertyMetadata(new CornerRadius(13)));



        public TagSizeEnum SizeEnum
        {
            get { return (TagSizeEnum)GetValue(SizeEnumProperty); }
            set { SetValue(SizeEnumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SizeEnum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeEnumProperty =
            DependencyProperty.Register("SizeEnum", typeof(TagSizeEnum), typeof(Tag), new PropertyMetadata(TagSizeEnum.Normal));


    }
}
