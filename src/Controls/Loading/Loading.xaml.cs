using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Awe.UI.Controls
{
    public class Loading : UserControl
    {
        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("arc") is Path pt)
            {
                var segment = new ArcSegment();

                var diameter = 120 - pt.StrokeThickness * 2;
                var arc = 90 * Math.PI / 270;

                var sin = Math.Sin(arc);
                // 获取余弦值：b/c 
                var cos = Math.Cos(arc);
                // 获取半径：c
                var radius = diameter / 2;
                // 获取 a
                var a = radius * sin;
                // 获取 b
                var b = radius * cos;
                // 获取终点位置：X
                var x = radius + a;
                // 获取终点位置：Y
                var y = radius - b;

                segment.IsLargeArc = true;
                segment.SweepDirection = SweepDirection.Counterclockwise;

                segment.Size = new Size(radius, radius);

                segment.Point = new Point(x ,y);

                var figure = new PathFigure();

                figure.StartPoint = new Point(radius, 0);

                figure.IsClosed = false;

                figure.Segments.Add(segment);



                pt.Data = new PathGeometry()
                {
                    Figures =
                    {
                        figure
                    }
                };
            }
            //base.OnApplyTemplate();
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new LoadingAutomationPeer(this);
        }
    }
}
