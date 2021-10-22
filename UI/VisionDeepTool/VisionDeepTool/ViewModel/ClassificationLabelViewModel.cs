using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionDeepTool.Helper;
using VisionDeepTool.Model;
using VisionDeepTool.Service;

namespace VisionDeepTool.ViewModel
{
    public class ClassificationLabelViewModel : ObservableObject
    {



        private readonly ClassificationService classificationService;

        public ClassificationLabelViewModel(ClassificationService _classificationService)
        {

            this.classificationService = _classificationService;


            this.ClassificationImageCollection = this.classificationService.ClassificationImageCollection;
            this.TargetLabelCollection = this.classificationService.TargetLabelCollection;


        }

        private ObservableCollection<ClassificationImage> _ClassificationImageCollection = null;
        public ObservableCollection<ClassificationImage> ClassificationImageCollection
        {
            get => _ClassificationImageCollection;
            set => SetProperty(ref _ClassificationImageCollection, value);
        }

        private ClassificationLabel _SelectedTargetLabel = null;
        public ClassificationLabel SelectedTargetLabel
        {
            get => _SelectedTargetLabel;
            set => SetProperty(ref _SelectedTargetLabel, value);
        }

        private ObservableCollection<ClassificationLabel> _TargetLabelCollection = null;
        public ObservableCollection<ClassificationLabel> TargetLabelCollection
        {
            get => _TargetLabelCollection;
            set => SetProperty(ref _TargetLabelCollection, value);
        }


        private ClassificationImage _SelectedClassificationImage = null;
        public ClassificationImage SelectedClassificationImage
        {
            get => _SelectedClassificationImage;
            set{



                try
                {

                    ClassificationImage image = value;
                    if(image != null)
                    {
                        var cvMat = new OpenCvSharp.Mat(image.FilePath);
                        this.SelectedImage = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(cvMat);

                    }

                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }
                
                
                SetProperty(ref _SelectedClassificationImage, value);
            }
        }


        private Color _SelectedLabelColor = Colors.White;
        public Color SelectedLabelColor
        {
            get => _SelectedLabelColor;
            set => SetProperty(ref _SelectedLabelColor, value);
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
                        await this.classificationService.LoadClassificationImageAsync(path);

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
                        this.classificationService.AddTargetLabel("Temp", this.SelectedLabelColor);

                    }
                    catch (Exception e)
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

                    }
                    catch (Exception e)
                    {
                        ToastMessageHelper.ShowToastErrorMessage("Error", e.Message);
                    }

                });
                return _DeleteLabelCommand;
            }
        }


        private WriteableBitmap _SelectedImage = null;
        public WriteableBitmap SelectedImage
        {
            get => _SelectedImage;
            set => SetProperty(ref _SelectedImage, value);
        }




    }
}
