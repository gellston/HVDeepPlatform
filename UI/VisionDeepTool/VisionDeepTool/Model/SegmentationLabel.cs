using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VisionDeepTool.Model
{
    public class SegmentationLabel : ObservableObject
    {

        public SegmentationLabel()
        {

        }

        private string _Name = "Temp";
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }


        private PointCollection _Points = null;
        public PointCollection Points
        {
            get => _Points;
            set => SetProperty(ref _Points, value); 
        }

        private Color _Color;
        public Color Color
        {
            get => _Color;
            set => SetProperty(ref _Color, value);
        }


        public double _X = 0;
        public double X
        {
            get => _X;
            set => SetProperty(ref _X, value);
        }

        public double _Y = 0;
        public double Y
        {
            get => _Y;
            set => SetProperty(ref _Y, value);
        }

        public double _Z = 0;
        public double Z
        {
            get => _Z;
            set => SetProperty(ref _Z, value);
        }

        public double _Width = 0;
        public double Width
        {
            get => _Width;
            set => SetProperty(ref _Width, value);
        }

        public double _Height = 0;
        public double Height
        {
            get => _Height;
            set => SetProperty(ref _Height, value);
        }

    }
}
