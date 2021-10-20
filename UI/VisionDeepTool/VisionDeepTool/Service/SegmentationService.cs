using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisionDeepTool.Model;

namespace VisionDeepTool.Service
{
    public class SegmentationService
    {

        public SegmentationService()
        {

        }

        private ObservableCollection<SegmentationImage> _SegmentationImageCollection = null;
        public ObservableCollection<SegmentationImage> SegmentaitonImageCollection
        {
            get
            {
                _SegmentationImageCollection ??= new ObservableCollection<SegmentationImage>();
                return _SegmentationImageCollection;
            }
        }





        public Task LoadSegmentationImageAsync(string path)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(path, "*.jpeg|*.jpg");
                this.SegmentaitonImageCollection.Clear();
                var task = Task.Run(() =>
                {
                    foreach (var image in images)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.SegmentaitonImageCollection.Add(new SegmentationImage()
                            {
                                FileName = Path.GetFileName(image),
                                FilePath = image
                            });
                        }));
                    }
                });
                return task;
                
            }catch(Exception e)
            {
                throw new Exception("Invalid path error or unexpected error occured");
            }
        }
        
    }
}
