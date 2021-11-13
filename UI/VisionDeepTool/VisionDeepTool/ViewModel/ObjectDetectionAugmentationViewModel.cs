using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VisionDeepTool.Model;
using VisionDeepTool.Service;

namespace VisionDeepTool.ViewModel
{
    public class ObjectDetectionAugmentationViewModel : ObservableObject
    {
        private readonly ObjectDetectionService objectDetectionService;

        public ObjectDetectionAugmentationViewModel(ObjectDetectionService _objectDetectionService)
        {

            this.objectDetectionService = _objectDetectionService;

            this.SourceClassificationImageCollection = this.objectDetectionService.ObjectDetectionMergeSourceImageCollection;
            this.TargetClassificationImageCollection = this.objectDetectionService.ObjectDetectionMergeTargetImageCollection;


            this.SourceMergeLabelCollection = this.objectDetectionService.SourceMergeLabelCollection;
            this.TargetMergeLabelCollection = this.objectDetectionService.TargetMergeLabelCollection;


        }

        private ObservableCollection<ObjectDetectionImage> _SourceClassifcaitonImageCollection = null;
        public ObservableCollection<ObjectDetectionImage> SourceClassificationImageCollection
        {
            get => _SourceClassifcaitonImageCollection;
            set => SetProperty(ref _SourceClassifcaitonImageCollection, value);
        }

        private ObservableCollection<ObjectDetectionImage> _TargetClassificationImageCollection = null;
        public ObservableCollection<ObjectDetectionImage> TargetClassificationImageCollection
        {
            get => _TargetClassificationImageCollection;
            set => SetProperty(ref _TargetClassificationImageCollection, value);
        }

        private ObservableCollection<ObjectDetectionLabel> _SourceMergeLabelCollection = null;
        public ObservableCollection<ObjectDetectionLabel> SourceMergeLabelCollection
        {
            get => _SourceMergeLabelCollection;
            set => SetProperty(ref _SourceMergeLabelCollection, value);
        }


        private ObservableCollection<ObjectDetectionLabel> _TargetMergeLabelCollection = null;
        public ObservableCollection<ObjectDetectionLabel> TargetMergeLabelCollection
        {
            get => _TargetMergeLabelCollection;
            set => SetProperty(ref _TargetMergeLabelCollection, value);
        }


        private ICommand _LoadTargetMergeLabelCommand = null;
        public ICommand LoadTargetMergeLabelCommand
        {
            get
            {
                _LoadTargetMergeLabelCommand ??= new AsyncRelayCommand(async () =>
                {
                    var path = Helper.DialogHelper.OpenFolder();

                    await this.objectDetectionService.LoadMergeSourceObjectDetectionLabelAsync(path).ConfigureAwait(false);
                });
                return _LoadTargetMergeLabelCommand;
            }
        }

        private ICommand _LoadSourceMergeLabelCommand = null;
        public ICommand LoadSourceMergeLabelCommand
        {
            get
            {
                

                _LoadSourceMergeLabelCommand ??= new AsyncRelayCommand(async ()=>
                {
                    var path = Helper.DialogHelper.OpenFolder();

                    await this.objectDetectionService.LoadMergeSourceObjectDetectionLabelAsync(path).ConfigureAwait(false);
                });

                return _LoadSourceMergeLabelCommand;
            }
        }

        private ICommand _MergeLabelCommand = null;
        public ICommand MergeLabelCommand
        {
            get
            {
                _MergeLabelCommand ??= new AsyncRelayCommand(async() =>
                {
                    
                    

                });

                return _MergeLabelCommand;

            }
        }
    }
}
