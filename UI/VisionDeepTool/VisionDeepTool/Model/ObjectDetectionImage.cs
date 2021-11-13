using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionDeepTool.Model
{
    public class ObjectDetectionImage : ObservableObject
    {

        public ObjectDetectionImage()
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

        private string _LabelPath = "";
        public string LabelPath
        {
            get => _LabelPath;
            set => SetProperty(ref _LabelPath, value);
        }



        private ObservableCollection<ObjectDetectionLabel> _LabelCollection = new ObservableCollection<ObjectDetectionLabel>();
        public ObservableCollection<ObjectDetectionLabel> LabelCollection
        {
            get => _LabelCollection;
            set => SetProperty(ref _LabelCollection, value);
        }


    }
}
