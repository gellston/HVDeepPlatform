using Newtonsoft.Json;
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

        private string _LabelLocation = "";
        public string LabelLocation
        {
            get => _LabelLocation;
            set => _LabelLocation = value;
        }

        public void AddTargetLabel(string name, Color labelColor)
        {

            this.TargetLabelCollection.Add(new ClassificationLabel()
            {
                Color = labelColor,
                Name = name
            });

        }

        public Task SaveClassificaitonImageAsync()
        {
            try
            {

                var task = Task.Run(() =>
                {
                    var images = ClassificationImageCollection.ToList();

                    foreach(var image in images)
                    {
                        try
                        {
                            var jsonContent = JsonConvert.SerializeObject(image.LabelCollection);
                            File.WriteAllText(image.LabelPath, jsonContent, Encoding.UTF8);
                        }
                        catch(Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                    }
                    try
                    {
                        var targetLabelCollection = this.TargetLabelCollection.ToList();
                        var labelInfoPath = this.LabelLocation + Path.DirectorySeparatorChar + "__LabelInfo.json";
                        var labelInfoContext = JsonConvert.SerializeObject(targetLabelCollection);
                        File.WriteAllText(labelInfoPath, labelInfoContext, Encoding.UTF8);
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    
                });

                return task;

            }catch(Exception e)
            {
                throw new Exception("unexpected error occured");
            }
        }


        public Task LoadClassificationImageAsync(string path)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(path, "*.jpeg|*.jpg");
                this.ClassificationImageCollection.Clear();
                var task = Task.Run(() =>
                {
                    string _directoryPath = path;
                    foreach (var image in images)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {

                            var directory = Path.GetDirectoryName(image);
                            var fileName = Path.GetFileNameWithoutExtension(image);
                            var labelPath = directory + Path.DirectorySeparatorChar + fileName + ".json";

                            var classificationImage = new ClassificationImage()
                            {
                                FileName = Path.GetFileName(image),
                                FilePath = image,
                                LabelPath = labelPath
                            };

                            if(File.Exists(labelPath) == true)
                            {
                                var labelContent = File.ReadAllText(labelPath, Encoding.UTF8);
                                try
                                {
                                    var labelCollection = JsonConvert.DeserializeObject<ObservableCollection<ClassificationLabel>>(labelContent);
                                    classificationImage.LabelCollection = labelCollection;
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine(e.Message);
                                }
                            }

                            
                            this.ClassificationImageCollection.Add(classificationImage);
                        }));
                    }


                    try
                    {
                        var labelInfoPath = _directoryPath + Path.DirectorySeparatorChar + "__LabelInfo.json";
                        var labelInfoContext = File.ReadAllText(labelInfoPath, Encoding.UTF8);

                        var targetCollection = JsonConvert.DeserializeObject<ObservableCollection<ClassificationLabel>>(labelInfoContext);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.TargetLabelCollection.Clear();
                            foreach (var target in targetCollection)
                                this.TargetLabelCollection.Add(target);
                        });

                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    


                    this.LabelLocation = _directoryPath;
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
