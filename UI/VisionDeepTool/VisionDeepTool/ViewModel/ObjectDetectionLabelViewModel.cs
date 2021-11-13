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
    public class ObjectDetectionLabelViewModel : ObservableObject
    {



        private readonly ObjectDetectionService objectDetectionService;

        public ObjectDetectionLabelViewModel(ObjectDetectionService _objectDetectionService)
        {

            this.objectDetectionService = _objectDetectionService;


            this.ObjectDetectionImageCollection = this.objectDetectionService.ObjectDetectionImageCollection;
            this.TargetLabelCollection = this.objectDetectionService.TargetLabelCollection;


        }

        private ObservableCollection<ObjectDetectionImage> _ObjectDetectionImageCollection = null;
        public ObservableCollection<ObjectDetectionImage> ObjectDetectionImageCollection
        {
            get => _ObjectDetectionImageCollection;
            set => SetProperty(ref _ObjectDetectionImageCollection, value);
        }

        private ObjectDetectionLabel _SelectedTargetLabel = null;
        public ObjectDetectionLabel SelectedTargetLabel
        {
            get => _SelectedTargetLabel;
            set => SetProperty(ref _SelectedTargetLabel, value);
        }

        private ObservableCollection<ObjectDetectionLabel> _TargetLabelCollection = null;
        public ObservableCollection<ObjectDetectionLabel> TargetLabelCollection
        {
            get => _TargetLabelCollection;
            set => SetProperty(ref _TargetLabelCollection, value);
        }


        private ObjectDetectionImage _SelectedObjectDetectionImage = null;
        public ObjectDetectionImage SelectedObjectDetectionImage
        {
            get => _SelectedObjectDetectionImage;
            set{



                try
                {

                    ObjectDetectionImage image = value;
                    if(image != null)
                    {
                        var cvMat = new OpenCvSharp.Mat(image.FilePath);
                        this.SelectedImage = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(cvMat);

                    }

                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }
                
                
                SetProperty(ref _SelectedObjectDetectionImage, value);
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
                        await this.objectDetectionService.LoadObjectDetectionImageAsync(path).ConfigureAwait(false);

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
                    await this.objectDetectionService.SaveObjectDetectionImageAsync().ConfigureAwait(false); ;
                    Helper.ToastMessageHelper.ShowToastSuccessMessage("라벨 저장 성공", "정상적으로 저장되었습니다.");
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
                        this.objectDetectionService.AddTargetLabel("Temp", this.SelectedLabelColor);

                    }
                    catch (Exception e)
                    {
                        ToastMessageHelper.ShowToastErrorMessage("Error", e.Message);
                    }

                });

                return _AddLabelCommand;
            }
        }

        private ICommand _DeleteTargetLabelCommand = null;
        public ICommand DeleteTargetLabelCommand
        {
            get
            {
                _DeleteTargetLabelCommand = new RelayCommand(() =>
                {

                    try
                    {
                        if (this.SelectedTargetLabel == null)
                            return;


                        this.TargetLabelCollection.Remove(this.SelectedTargetLabel);


                    }catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                });

                return _DeleteTargetLabelCommand;
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
                        if(this.SelectedObjectDetectionImage != null)
                        {
                            this.SelectedObjectDetectionImage.LabelCollection.ToList().ForEach((data) =>
                            {
                                if(data.IsSelected == true)
                                    this.SelectedObjectDetectionImage.LabelCollection.Remove(data);
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        ToastMessageHelper.ShowToastErrorMessage("Error", e.Message);
                    }

                });
                return _DeleteLabelCommand;
            }
        }

        private ICommand _SelectionNextShiftCommand = null;
        public ICommand SelectionNextShiftCommand
        {
            get
            {
                _SelectionNextShiftCommand ??= new AsyncRelayCommand(async () =>
                {

                    if (_SelectedObjectDetectionImage == null) return;
                    int currentIndex = this.ObjectDetectionImageCollection.IndexOf(_SelectedObjectDetectionImage);

                    currentIndex += 1;

                    if (this.ObjectDetectionImageCollection.Count > currentIndex)
                        this.SelectedObjectDetectionImage = this.ObjectDetectionImageCollection[currentIndex];


                });

                return _SelectionNextShiftCommand;
            }
        }

        private ICommand _SelectionPrevShiftCommand = null;
        public ICommand SelectionPrevShiftCommand
        {
            get
            {
                _SelectionPrevShiftCommand ??= new AsyncRelayCommand(async () =>
                {

                    if (_SelectedObjectDetectionImage == null) return;
                    int currentIndex = this.ObjectDetectionImageCollection.IndexOf(_SelectedObjectDetectionImage);

                    currentIndex -= 1;

                    if (this.ObjectDetectionImageCollection.Count > currentIndex && currentIndex >= 0)
                        this.SelectedObjectDetectionImage = this.ObjectDetectionImageCollection[currentIndex];


                });

                return _SelectionPrevShiftCommand;
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
