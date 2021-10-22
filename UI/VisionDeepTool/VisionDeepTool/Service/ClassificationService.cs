using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VisionDeepTool.Model;

namespace VisionDeepTool.Service
{
    public class ClassificationService
    {

        public ClassificationService()
        {

        }

        private ObservableCollection<ClassificationImage> _ClassificationImageCollection = null;
        public ObservableCollection<ClassificationImage> ClassificationImageCollection
        {
            get
            {
                _ClassificationImageCollection ??= new ObservableCollection<ClassificationImage>();
                return _ClassificationImageCollection;
            }
        }




        private ObservableCollection<ClassificationLabel> _TargetLabelCollection = null;
        public ObservableCollection<ClassificationLabel> TargetLabelCollection
        {
            get
            {
                _TargetLabelCollection ??= new ObservableCollection<ClassificationLabel>();
                return _TargetLabelCollection;
            }
        }



        public void AddTargetLabel(string name, Color labelColor)
        {

            this.TargetLabelCollection.Add(new ClassificationLabel()
            {
                Color = labelColor,
                Name = name
            });

        }




        public Task LoadClassificationImageAsync(string path)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(path, "*.jpeg|*.jpg");
                this.ClassificationImageCollection.Clear();
                var task = Task.Run(() =>
                {
                    foreach (var image in images)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.ClassificationImageCollection.Add(new ClassificationImage()
                            {
                                FileName = Path.GetFileName(image),
                                FilePath = image
                            });
                        }));
                    }
                });
                return task;

            }
            catch (Exception e)
            {
                throw new Exception("Invalid path error or unexpected error occured");
            }
        }

    }
}
