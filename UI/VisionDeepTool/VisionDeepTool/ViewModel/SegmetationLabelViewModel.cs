using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VisionDeepTool.Helper;
using VisionDeepTool.Model;
using VisionDeepTool.Service;

namespace VisionDeepTool.ViewModel
{
    public class SegmentationLabelViewModel : ObservableObject
    {

        private readonly SegmentationService segmentationService;


        public SegmentationLabelViewModel(SegmentationService _segmentationService)
        {

            this.segmentationService = _segmentationService;

            this.SegmentationImageCollection = this.segmentationService.SegmentaitonImageCollection;

        }

        private ObservableCollection<SegmentationImage> _SegmentationImageCollection = null;
        public ObservableCollection<SegmentationImage> SegmentationImageCollection
        {
            get => _SegmentationImageCollection;
            set => SetProperty(ref _SegmentationImageCollection, value);
        }


        private SegmentationImage _SelectedSegmentationImage = null;
        public SegmentationImage SelectedSegmentationImage
        {
            get => _SelectedSegmentationImage;
            set => SetProperty(ref _SelectedSegmentationImage, value);
        }


        private ICommand _OpenImageFolderCommand = null;

        public ICommand OpenImageFolderCommand
        {
            get
            {
                _OpenImageFolderCommand ??= new AsyncRelayCommand(async () =>
                {

                    try
                    {
                        var path = DialogHelper.OpenFolder();
                        await this.segmentationService.LoadSegmentationImageAsync(path);

                    }
                    catch (Exception e)
                    {
                        ToastMessageHelper.ShowToastErrorMessage("Error", e.Message);
                    }
                });

                return _OpenImageFolderCommand;
            }
        }

        private ICommand _SaveLabelCommand = null;
        public ICommand SaveLabelCommand
        {
            get
            {
                _SaveLabelCommand ??= new AsyncRelayCommand(async () =>
                {

                });

                return _SaveLabelCommand;

            }
        }



        private ICommand _AddLabelCommand = null;
        public ICommand AddLabelCommand
        {
            get
            {
                _AddLabelCommand ??= new AsyncRelayCommand(async () =>
                {
                    try
                    {

                    }catch(Exception e)
                    {
                        ToastMessageHelper.ShowToastErrorMessage("Error", e.Message);
                    }

                });

                return _AddLabelCommand;
            }
        }


        private ICommand _DeleteLabelCommand = null;
        public ICommand DeleteLabelCommand
        {
            get
            {
                _DeleteLabelCommand ??= new AsyncRelayCommand(async () =>
                {
                    try
                    {

                    }catch(Exception e)
                    {
                        ToastMessageHelper.ShowToastErrorMessage("Error", e.Message);
                    }

                });
                return _DeleteLabelCommand;
            }
        }





    }
}
