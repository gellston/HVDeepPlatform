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
    public partial class ClassificationLabelControl : UserControl, INotifyPropertyChanged
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


        public ClassificationLabelControl()
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


        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(ClassificationLabelControl));
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

        public static readonly DependencyProperty ZoomMaxProperty = DependencyProperty.Register("ZoomMax", typeof(double), typeof(ClassificationLabelControl));
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

        public static readonly DependencyProperty ZoomMinProperty = DependencyProperty.Register("ZoomMin", typeof(double), typeof(ClassificationLabelControl));
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

        public static readonly DependencyProperty ZoomStepProperty = DependencyProperty.Register("ZoomStep", typeof(double), typeof(ClassificationLabelControl));
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


        public static readonly DependencyProperty TranslationXProperty = DependencyProperty.Register("TranslationX", typeof(double), typeof(ClassificationLabelControl));
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


        public static readonly DependencyProperty TranslationYProperty = DependencyProperty.Register("TranslationY", typeof(double), typeof(ClassificationLabelControl));
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


        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(WriteableBitmap), typeof(ClassificationLabelControl), new PropertyMetadata(OnCustomerChangedCallBack));
        public WriteableBitmap Image
        {
            get
            {
                return (WriteableBitmap)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        private static void OnCustomerChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ClassificationLabelControl control = sender as ClassificationLabelControl;
            if (control != null)
            {
                WriteableBitmap image = e.NewValue as WriteableBitmap;
                if (image == null) return;
                control.CanvasWidth = image.PixelWidth;
                control.CanvasHeight = image.PixelHeight;
                control.TranslationX = 0;
                control.TranslationY = 0;
                //control.Zoom = 1;
            }
        }






        public static readonly DependencyProperty ClassificationLabelCollectionProperty = DependencyProperty.Register("ClassificationLabelCollection", typeof(ObservableCollection<ClassificationLabel>), typeof(ClassificationLabelControl));
        public ObservableCollection<ClassificationLabel> ClassificationLabelCollection
        {
            get
            {
                return (ObservableCollection<ClassificationLabel>)GetValue(ClassificationLabelCollectionProperty);
            }

            set
            {
                SetValue(ClassificationLabelCollectionProperty, value);
            }
        }


        public static readonly DependencyProperty SelectedLabelProperty = DependencyProperty.Register("SelectedLabel", typeof(ClassificationLabel), typeof(ClassificationLabelControl), new PropertyMetadata(OnSelectedItemChangedCallBack));
        public ClassificationLabel SelectedLabel
        {
            get
            {
                return (ClassificationLabel)GetValue(SelectedLabelProperty);
            }

            set
            {
                SetValue(SelectedLabelProperty, value);
            }
        }


        private static void OnSelectedItemChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ClassificationLabelControl control = sender as ClassificationLabelControl;
            if (control != null)
            {
                control.SelectedLabel = e.NewValue as ClassificationLabel;
            }
        }


        public static readonly DependencyProperty TargetLabelProperty = DependencyProperty.Register("TargetLabel", typeof(ClassificationLabel), typeof(ClassificationLabelControl), new PropertyMetadata(OnTargetItemChangedCallBack));
        public ClassificationLabel TargetLabel
        {
            get
            {
                return (ClassificationLabel)GetValue(TargetLabelProperty);
            }

            set
            {
                SetValue(TargetLabelProperty, value);
            }
        }

        private static void OnTargetItemChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ClassificationLabelControl control = sender as ClassificationLabelControl;
            if (control != null)
            {
                control.SelectedLabel = null;
            }
        }





        private void ChildCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.ReleaseMouseCapture();
            if (this.SelectedLabel != null)
                this.SelectedLabel.IsSelected = false;

            this.SelectedLabel = null;
            this.IsRectSelected = false;
            this.IsCanvasCaptured = false;
        }



        private Point CanvasStart;
        private Point CanvasOrigin;
        private bool IsRectSelected = false;
        private bool IsCanvasCaptured = false;

        private void ChildCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Point pressedPoint = e.GetPosition(sender as IInputElement);

            HitTestResult hitTestResult = VisualTreeHelper.HitTest(sender as Visual, e.GetPosition(sender as IInputElement));
            if (hitTestResult == null) return;
            var element = hitTestResult.VisualHit;




            if (Keyboard.IsKeyDown(Key.LeftCtrl) == true && element.GetType() == typeof(Rectangle))
            {
                ClassificationLabel datacontext = (element as Rectangle).DataContext as ClassificationLabel;
                datacontext.IsSelected = !datacontext.IsSelected;
                if (datacontext.IsSelected == true)
                    this.SelectedLabel = datacontext;
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift) == true && element.GetType() == typeof(Canvas))
            {
                this.CanvasStart = e.GetPosition(this);
                this.CanvasOrigin = new Point(this.TranslationX, this.TranslationY);

                var draggableControl = sender as Canvas;
                draggableControl.CaptureMouse();
                IsCanvasCaptured = true;
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift) == true && element.GetType() == typeof(Rectangle))
            {
                ClassificationLabel datacontext = (element as Rectangle).DataContext as ClassificationLabel;
                datacontext.IsSelected = true;

                this.IsRectSelected = true;
                this.SelectedLabel = datacontext;

                var draggableControl = sender as Canvas;
                draggableControl.CaptureMouse();
                return;
            }

            if (this.SelectedLabel == null && this.TargetLabel != null && this.Image != null && Keyboard.IsKeyDown(Key.LeftAlt) == true)
            {

                ClassificationLabel boxItem = new ClassificationLabel();
                boxItem.X = pressedPoint.X;
                boxItem.Y = pressedPoint.Y;
                boxItem.Width = 10;
                boxItem.Height = 10;
                boxItem.Color = this.TargetLabel.Color;
                boxItem.Name = this.TargetLabel.Name;


                this.SelectedLabel = boxItem;
                this.ClassificationLabelCollection.Add(boxItem);
                var draggableControl = sender as Canvas;
                draggableControl.CaptureMouse();
            }

        }

        private void ChildCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point cursorPosition = e.GetPosition(this);
            Canvas canvas = sender as Canvas;
            if (canvas != null)
            {
                if (canvas.IsMouseCaptured == true && canvas != null && Keyboard.IsKeyDown(Key.LeftShift) && this.IsRectSelected == false)
                {
                    Vector v = this.CanvasStart - cursorPosition;
                    this.TranslationX = CanvasOrigin.X - v.X;
                    this.TranslationY = CanvasOrigin.Y - v.Y;
                    return;
                }

                if (canvas.IsMouseCaptured == true && canvas != null && Keyboard.IsKeyDown(Key.LeftAlt) && this.SelectedLabel != null)
                {

                    Point canvasCursorPosition = e.GetPosition(canvas);

                    this.SelectedLabel.Width = canvasCursorPosition.X - this.SelectedLabel.X;
                    this.SelectedLabel.Height = canvasCursorPosition.Y - this.SelectedLabel.Y;
                    System.Console.WriteLine("Box Test x: " + this.SelectedLabel.X);
                    System.Console.WriteLine("Box Test y: " + this.SelectedLabel.Y);
                    System.Console.WriteLine("Box Test Width: " + this.SelectedLabel.Width);
                    System.Console.WriteLine("Box Test Height: " + this.SelectedLabel.Height);
                    return;
                }

                if (this.IsRectSelected == true && Keyboard.IsKeyDown(Key.LeftShift))
                {

                    Point canvasCursorPosition = e.GetPosition(canvas);

                    this.SelectedLabel.X = canvasCursorPosition.X - this.SelectedLabel.Width / 2;
                    this.SelectedLabel.Y = canvasCursorPosition.Y - this.SelectedLabel.Height / 2;

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
            this.IsRectSelected = false;
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

        private void view_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.SelectedLabel != null && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C))
            {
                ClassificationLabel label = new ClassificationLabel()
                {
                    X = this.SelectedLabel.X + 10,
                    Y = this.SelectedLabel.Y + 10,
                    Width = this.SelectedLabel.Width,
                    Height = this.SelectedLabel.Height,
                    Name = this.SelectedLabel.Name,
                    Color = this.SelectedLabel.Color,
                    IsSelected = true
                };
                this.SelectedLabel.IsSelected = false;
                this.SelectedLabel = label;
                this.ClassificationLabelCollection.Add(this.SelectedLabel);
            }
        }
    }
}
