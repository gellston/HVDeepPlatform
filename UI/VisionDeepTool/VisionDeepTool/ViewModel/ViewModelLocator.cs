using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionDeepTool.ViewModel
{
    public class ViewModelLocator
    {

        public ViewModelLocator()
        {

        }

        public ObservableObject MainWindowViewModel
        {
            get => App.Current.Services.GetService<MainWindowViewModel>();

        }

        public ObservableObject ReleaseNoteViewModel
        {
            get => App.Current.Services.GetService<ReleaseNoteViewModel>();
        }

        public ObservableObject SegmentationTrainViewModel
        {
            get => App.Current.Services.GetService<SegmentationTrainViewModel>();

        }

        public ObservableObject SegmentationLabelViewModel
        {
            get => App.Current.Services.GetService<SegmentationLabelViewModel>();
        }

        public ObservableObject SegmentaitonAugmentationViewModel
        {
            get => App.Current.Services.GetService<SegmentationAugmentationViewModel>();
        }

        public ObservableObject SegmentaitonModelExporterViewModel
        {
            get => App.Current.Services.GetService<SegmentationModelExporterViewModel>();
        }
    }
}
