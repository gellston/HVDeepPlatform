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
    public class ClassificationAugmentationViewModel : ObservableObject
    {
        private readonly ClassificationService classificationService;

        public ClassificationAugmentationViewModel(ClassificationService _classificationService)
        {

            this.classificationService = _classificationService;

            this.SourceClassificationImageCollection = this.classificationService.ClassificationMergeSourceImageCollection;
            this.TargetClassificationImageCollection = this.classificationService.ClassificationMergeTargetImageCollection;


            this.SourceMergeLabelCollection = this.classificationService.SourceMergeLabelCollection;
            this.TargetMergeLabelCollection = this.classificationService.TargetMergeLabelCollection;


        }

        private ObservableCollection<ClassificationImage> _SourceClassifcaitonImageCollection = null;
        public ObservableCollection<ClassificationImage> SourceClassificationImageCollection
        {
            get => _SourceClassifcaitonImageCollection;
            set => SetProperty(ref _SourceClassifcaitonImageCollection, value);
        }

        private ObservableCollection<ClassificationImage> _TargetClassificationImageCollection = null;
        public ObservableCollection<ClassificationImage> TargetClassificationImageCollection
        {
            get => _TargetClassificationImageCollection;
            set => SetProperty(ref _TargetClassificationImageCollection, value);
        }

        private ObservableCollection<ClassificationLabel> _SourceMergeLabelCollection = null;
        public ObservableCollection<ClassificationLabel> SourceMergeLabelCollection
        {
            get => _SourceMergeLabelCollection;
            set => SetProperty(ref _SourceMergeLabelCollection, value);
        }


        private ObservableCollection<ClassificationLabel> _TargetMergeLabelCollection = null;
        public ObservableCollection<ClassificationLabel> TargetMergeLabelCollection
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

                    await this.classificationService.LoadMergeSourceClassificationLabelAsync(path).ConfigureAwait(false);
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

                    await this.classificationService.LoadMergeSourceClassificationLabelAsync(path).ConfigureAwait(false);
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
