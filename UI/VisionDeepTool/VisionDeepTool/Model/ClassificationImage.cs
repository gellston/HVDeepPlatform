using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionDeepTool.Model
{
    public class ClassificationImage : ObservableObject
    {

        public ClassificationImage()
        {

        }


        private string _FileName = "";
        public string FileName
        {
            get => _FileName;
            set => SetProperty(ref _FileName, value);
        }


        private string _FilePath = "";
        public string FilePath
        {
            get => _FilePath;
            set => SetProperty(ref _FilePath, value);
        }



        private ObservableCollection<ClassificationLabel> _LabelCollection = new ObservableCollection<ClassificationLabel>();
        public ObservableCollection<ClassificationLabel> LabelCollection
        {
            get => _LabelCollection;
            set => SetProperty(ref _LabelCollection, value);
        }


    }
}
