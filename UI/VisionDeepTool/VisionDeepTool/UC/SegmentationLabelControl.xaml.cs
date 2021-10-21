using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisionDeepTool.Model;

namespace VisionDeepTool.UC
{
    /// <summary>
    /// SegmentationLabelControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SegmentationLabelControl : UserControl, INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public void Set<T>(string _name, ref T _reference, T _value)
        {
            if (!Equals(_reference, _value))
            {
                _reference = _value;
                OnPropertyRaised(_name);
            }
        }


        public SegmentationLabelControl()
        {
            InitializeComponent();
        }


        private double _CanvasWidth = 0;
        public double CanvasWidth
        {
            get => _CanvasWidth;
            set => Set<double>(nameof(CanvasWidth), ref _CanvasWidth, value);
        }

        private double _CanvasHeight = 0;
        public double CanvasHeight
        {
            get => _CanvasHeight;
            set => Set<double>(nameof(CanvasHeight), ref _CanvasHeight, value);
        }


        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(SegmentationLabelControl));
        public double Zoom
        {
            get
            {
                return (double)GetValue(ZoomProperty);
            }

            set
            {
                SetValue(ZoomProperty, value);
                OnPropertyRaised(nameof(this.TransformGroup));
            }
        }

        public static readonly DependencyProperty ZoomMaxProperty = DependencyProperty.Register("ZoomMax", typeof(double), typeof(SegmentationLabelControl));
        public double ZoomMax
        {
            get
            {
                return (double)GetValue(ZoomMaxProperty);
            }

            set
            {
                SetValue(ZoomMaxProperty, value);
            }
        }

        public static readonly DependencyProperty ZoomMinProperty = DependencyProperty.Register("ZoomMin", typeof(double), typeof(SegmentationLabelControl));
        public double ZoomMin
        {
            get
            {
                return (double)GetValue(ZoomMinProperty);
            }

            set
            {
                SetValue(ZoomMinProperty, value);
            }
        }

        public static readonly DependencyProperty ZoomStepProperty = DependencyProperty.Register("ZoomStep", typeof(double), typeof(SegmentationLabelControl));
        public double ZoomStep
        {
            get
            {
                return (double)GetValue(ZoomStepProperty);
            }

            set
            {
                SetValue(ZoomStepProperty, value);
            }
        }


        public static readonly DependencyProperty TranslationXProperty = DependencyProperty.Register("TranslationX", typeof(double), typeof(SegmentationLabelControl));
        public double TranslationX
        {
            get
            {
                return (double)GetValue(TranslationXProperty);
            }

            set
            {
                SetValue(TranslationXProperty, value);
                OnPropertyRaised(nameof(this.TransformGroup));
            }
        }


        public static readonly DependencyProperty TranslationYProperty = DependencyProperty.Register("TranslationY", typeof(double), typeof(SegmentationLabelControl));
        public double TranslationY
        {
            get
            {
                return (double)GetValue(TranslationYProperty);
            }

            set
            {
                SetValue(TranslationYProperty, value);
                OnPropertyRaised(nameof(this.TransformGroup));
            }
        }

        public TransformGroup TransformGroup
        {
            get
            {

                ScaleTransform scaleTransform = new ScaleTransform();
                scaleTransform.ScaleX = this.Zoom;
                scaleTransform.ScaleY = this.Zoom;


                TranslateTransform translateTransform = new TranslateTransform();
                translateTransform.X = this.TranslationX;
                translateTransform.Y = this.TranslationY;


                var transformGroup = new TransformGroup();


                transformGroup.Children.Add(scaleTransform);
                transformGroup.Children.Add(translateTransform);

                return transformGroup;
            }
        }


        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(SegmentationLabelControl), new PropertyMetadata(OnCustomerChangedCallBack));
        public BitmapImage Image
        {
            get
            {
                return (BitmapImage)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        private static void OnCustomerChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SegmentationLabelControl control = sender as SegmentationLabelControl;
            if (control != null)
            {
                BitmapImage image = e.NewValue as BitmapImage;
                if (image == null) return;
                control.CanvasWidth = image.PixelWidth;
                control.CanvasHeight = image.PixelHeight;
                control.TranslationX = 0;
                control.TranslationY = 0;
                //control.Zoom = 1;
            }
        }






        public static readonly DependencyProperty SegmentationLabelCollectionProperty = DependencyProperty.Register("SegmentationLabelCollection", typeof(ObservableCollection<SegmentationLabel>), typeof(SegmentationLabelControl));
        public ObservableCollection<SegmentationLabel> SegmentationLabelCollection
        {
            get
            {
                return (ObservableCollection<SegmentationLabel>)GetValue(SegmentationLabelCollectionProperty);
            }

            set
            {
                SetValue(SegmentationLabelCollectionProperty, value);
            }
        }


        public static readonly DependencyProperty SelectedLabelProperty = DependencyProperty.Register("SelectedLabel", typeof(SegmentationLabel), typeof(SegmentationLabelControl), new PropertyMetadata(OnSelectedItemChangedCallBack));
        public SegmentationLabel SelectedLabel
        {
            get
            {
                return (SegmentationLabel)GetValue(SelectedLabelProperty);
            }

            set
            {
                SetValue(SelectedLabelProperty, value);
            }
        }


        private static void OnSelectedItemChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SegmentationLabelControl control = sender as SegmentationLabelControl;
            if (control != null)
            {
                control.SelectedLabel = e.NewValue as SegmentationLabel;
            }
        }


        public static readonly DependencyProperty TargetLabelProperty = DependencyProperty.Register("TargetLabel", typeof(SegmentationLabel), typeof(SegmentationLabelControl), new PropertyMetadata(OnTargetItemChangedCallBack));
        public SegmentationLabel TargetLabel
        {
            get
            {
                return (SegmentationLabel)GetValue(TargetLabelProperty);
            }

            set
            {
                SetValue(TargetLabelProperty, value);
            }
        }

        private static void OnTargetItemChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SegmentationLabelControl control = sender as SegmentationLabelControl;
            if (control != null)
            {
                control.SelectedLabel = null;
            }
        }





        private void ChildCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.ReleaseMouseCapture();
        }

        private Point CanvasStart;
        private Point CanvasOrigin;

        private void ChildCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Point pressedPoint = e.GetPosition(sender as IInputElement);

            HitTestResult hitTestResult = VisualTreeHelper.HitTest(sender as Visual, e.GetPosition(sender as IInputElement));
            if (hitTestResult == null) return;
            var element = hitTestResult.VisualHit;



            //if (Keyboard.IsKeyDown(Key.LeftCtrl) == true && element.GetType() == typeof(Polygon))
            //{
            //    SegmentLabelPolygon datacontext = (element as Polygon).DataContext as SegmentLabelPolygon;
            //    datacontext.IsSelected = !datacontext.IsSelected;
            //    if (datacontext.IsSelected == true)
            //        this.SelectedItem = datacontext;
            //    return;
            //}

            if (Keyboard.IsKeyDown(Key.LeftShift) == true && element.GetType() == typeof(Canvas))
            {
                this.CanvasStart = e.GetPosition(this);
                this.CanvasOrigin = new Point(this.TranslationX, this.TranslationY);

                var draggableControl = sender as Canvas;
                draggableControl.CaptureMouse();
                return;
            }



            if (this.SelectedLabel == null && this.TargetLabel != null && this.Image != null && Keyboard.IsKeyDown(Key.LeftAlt) == true)
            {
                //SegmentationPolygon polygonItem = this.TargetItem.Clone() as SegmentationPolygon;

                SegmentationLabel polygonItem = new SegmentationLabel();
                polygonItem.X = 0;
                polygonItem.Y = 0;
                polygonItem.Width = this.Image.Width;
                polygonItem.Height = this.Image.Height;
                polygonItem.Color = this.TargetLabel.Color;
                polygonItem.Name = this.TargetLabel.Name;

                //polygonItem.X = 0;
                //polygonItem.Y = 0;
                //polygonItem.Width = this.Image.Width;
                //polygonItem.Height = this.Image.Height;

                this.SelectedLabel = polygonItem;
                this.SegmentationLabelCollection.Add(polygonItem);
            }

            if (this.SelectedLabel != null && Keyboard.IsKeyDown(Key.LeftAlt) == true)
            {
                this.SelectedLabel.Points.Add(new Point()
                {
                    X = pressedPoint.X,
                    Y = pressedPoint.Y
                });

                PointCollection tempCollection = this.SelectedLabel.Points;
                PointCollection newCollection = new PointCollection(tempCollection);
                this.SelectedLabel.Points = newCollection;
            }

        }

        private void ChildCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point cursorPosition = e.GetPosition(this);
            Canvas canvas = sender as Canvas;
            if (canvas != null)
            {
                if (canvas.IsMouseCaptured == true && canvas != null)
                {
                    Vector v = this.CanvasStart - cursorPosition;
                    this.TranslationX = CanvasOrigin.X - v.X;
                    this.TranslationY = CanvasOrigin.Y - v.Y;
                    return;
                }
            }
        }

        //private void ChildCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    var draggableControl = sender as Canvas;
        //    if (draggableControl.IsMouseCaptured == true)
        //        return;

        //    if (e.Delta > 0)
        //    {
        //        this.Zoom -= this.ZoomStep;
        //        if (this.Zoom <= this.ZoomMin)
        //            this.Zoom = this.ZoomMin;
        //    }
        //    else
        //    {
        //        this.Zoom += this.ZoomStep;
        //        if (this.Zoom >= this.ZoomMax)
        //            this.Zoom = this.ZoomMax;

        //    }

        //}

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectedLabel = null;
        }

        private void OutScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (this.IsMouseCaptured == true)
                return;

            if (e.Delta > 0)
            {
                this.Zoom -= ((this.ZoomMax - this.ZoomMin) / 100);
                if (this.Zoom <= this.ZoomMin)
                    this.Zoom = this.ZoomMin;
            }
            else
            {
                this.Zoom += ((this.ZoomMax - this.ZoomMin) / 100);
                if (this.Zoom >= this.ZoomMax)
                    this.Zoom = this.ZoomMax;
            }

            OutScrollViewer.ScrollToVerticalOffset(OutScrollViewer.ScrollableHeight / 2);
            OutScrollViewer.ScrollToHorizontalOffset(OutScrollViewer.ScrollableWidth / 2);

        }
    }
}
