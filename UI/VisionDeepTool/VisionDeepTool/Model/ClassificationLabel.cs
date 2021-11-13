using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VisionDeepTool.Model
{
    public class ClassificationLabel : ObservableObject
    {


        public ClassificationLabel()
        {

        }

        private string _Name = "Temp";
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private bool _IsSelected = false;
        public bool IsSelected
        {
            get => _IsSelected;
            set => SetProperty(ref _IsSelected, value);
        }

        private Color _Color;
        public Color Color
        {
            get => _Color;
            set => SetProperty(ref _Color, value);
        }
    }

}
