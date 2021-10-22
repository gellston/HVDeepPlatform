using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using VisionDeepTool.Model;

namespace VisionDeepTool.UC
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }





        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ClassificationLabel property = this.DataContext as ClassificationLabel;

            if (property != null)
            {

                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:

                        double origin = property.X;
                        double width = property.Width;

                        property.X += e.HorizontalChange;
                        property.Width -= e.HorizontalChange;

                        if (property.Width <= 10 || property.X <= 0)
                        {
                            property.X = origin;
                            property.Width = width;

                        }
                        break;
                    case HorizontalAlignment.Right:

                        property.Width += e.HorizontalChange;
                        break;

                }

                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        double origin = property.Y;
                        double height = property.Height;

                        property.Y += e.VerticalChange;
                        property.Height -= e.VerticalChange;

                        if (property.Height <= 10 || property.Y <= 0)
                        {
                            property.Y = origin;
                            property.Height = height;

                        }
                        break;
                    case VerticalAlignment.Bottom:

                        property.Height += e.VerticalChange;
                        break;

                }

            }
        }
    }
}
