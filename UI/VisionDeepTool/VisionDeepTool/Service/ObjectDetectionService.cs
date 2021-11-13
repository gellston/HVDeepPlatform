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
    public class ObjectDetectionService
    {

        private readonly ApplicationConfigService applicationConfigService;


        public ObjectDetectionService(ApplicationConfigService _applicationConfigService)
        {
            this.applicationConfigService = _applicationConfigService;
        }

        private ObservableCollection<ObjectDetectionImage> _ObjectDetectionImageCollection = null;
        public ObservableCollection<ObjectDetectionImage> ObjectDetectionImageCollection
        {
            get
            {
                _ObjectDetectionImageCollection ??= new ObservableCollection<ObjectDetectionImage>();
                return _ObjectDetectionImageCollection;
            }
        }




        private ObservableCollection<ObjectDetectionLabel> _TargetLabelCollection = null;
        public ObservableCollection<ObjectDetectionLabel> TargetLabelCollection
        {
            get
            {
                _TargetLabelCollection ??= new ObservableCollection<ObjectDetectionLabel>();
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

            this.TargetLabelCollection.Add(new ObjectDetectionLabel()
            {
                Color = labelColor,
                Name = name
            });

        }

        public Task SaveObjectDetectionImageAsync()
        {
            try
            {

                var task = Task.Run(() =>
                {
                    var images = ObjectDetectionImageCollection.ToList();

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


        public Task LoadObjectDetectionImageAsync(string path)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(path, "*.jpeg|*.jpg");
                this.ObjectDetectionImageCollection.Clear();
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

                            var classificationImage = new ObjectDetectionImage()
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
                                    var labelCollection = JsonConvert.DeserializeObject<ObservableCollection<ObjectDetectionLabel>>(labelContent);
                                    classificationImage.LabelCollection = labelCollection;
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine(e.Message);
                                }
                            }

                            
                            this.ObjectDetectionImageCollection.Add(classificationImage);
                        }));
                    }


                    try
                    {
                        var labelInfoPath = _directoryPath + Path.DirectorySeparatorChar + "__LabelInfo.json";
                        var labelInfoContext = File.ReadAllText(labelInfoPath, Encoding.UTF8);

                        var targetCollection = JsonConvert.DeserializeObject<ObservableCollection<ObjectDetectionLabel>>(labelInfoContext);

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


        // Merging Service

        private ObservableCollection<ObjectDetectionImage> _ObjectDetectionMergeSourceImageCollection = null;
        public ObservableCollection<ObjectDetectionImage> ObjectDetectionMergeSourceImageCollection
        {
            get
            {
                _ObjectDetectionMergeSourceImageCollection ??= new ObservableCollection<ObjectDetectionImage>();
                return _ObjectDetectionMergeSourceImageCollection;
            }
        }


        private ObservableCollection<ObjectDetectionImage> _ObjectDetectionMergeTargetImageCollection = null;
        public ObservableCollection<ObjectDetectionImage> ObjectDetectionMergeTargetImageCollection
        {
            get
            {
                _ObjectDetectionMergeTargetImageCollection ??= new ObservableCollection<ObjectDetectionImage>();
                return _ObjectDetectionMergeTargetImageCollection;
            }
        }

        private ObservableCollection<ObjectDetectionImage> _ObjectDetectionMergeResultImageCollection = null;
        public ObservableCollection<ObjectDetectionImage> ObjectDetectionMergeResultImageCollection
        {
            get
            {
                _ObjectDetectionMergeResultImageCollection ??= new ObservableCollection<ObjectDetectionImage>();
                return _ObjectDetectionMergeResultImageCollection;
            }
        }

        private ObservableCollection<ObjectDetectionLabel> _TargetMergeLabelCollection = null;
        public ObservableCollection<ObjectDetectionLabel> TargetMergeLabelCollection
        {
            get
            {
                _TargetMergeLabelCollection ??= new ObservableCollection<ObjectDetectionLabel>();
                return _TargetMergeLabelCollection;
            }
        }

        private ObservableCollection<ObjectDetectionLabel> _SourceMergeLabelCollection = null;
        public ObservableCollection<ObjectDetectionLabel> SourceMergeLabelCollection
        {
            get
            {
                _SourceMergeLabelCollection ??= new ObservableCollection<ObjectDetectionLabel>();
                return _SourceMergeLabelCollection;
            }
        }

        private ObservableCollection<ObjectDetectionLabel> _ResultMergeLabelCollection = null;
        public ObservableCollection<ObjectDetectionLabel> ResultMergeLabelCollection
        {
            get
            {
                _ResultMergeLabelCollection ??= new ObservableCollection<ObjectDetectionLabel>();
                return _ResultMergeLabelCollection;
            }
        }

        public Task MergeObjectDetectionLabelAsync()
        {
            try
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        var sourceLabelCollection = this.SourceMergeLabelCollection.ToList();
                        var targetLabelCollection = this.TargetMergeLabelCollection.ToList();

                        var unionLabelCollection = sourceLabelCollection.Union(targetLabelCollection).OrderBy(data => data.Name).ToList();



                        

                    }catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }

                });

                return task;

            }catch(Exception e)
            {
                throw new Exception("Invalid path error or unexpected error occured");
            }
        }

        public Task LoadMergeSourceObjectDetectionLabelAsync(string source)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(source, "*.jpeg|*.jpg");
                this.ObjectDetectionMergeSourceImageCollection.Clear();
                var task = Task.Run(() =>
                {
                    string _directoryPath = source;
                    foreach (var image in images)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {

                            var directory = Path.GetDirectoryName(image);
                            var fileName = Path.GetFileNameWithoutExtension(image);
                            var labelPath = directory + Path.DirectorySeparatorChar + fileName + ".json";

                            var objectDetectionImage = new ObjectDetectionImage()
                            {
                                FileName = Path.GetFileName(image),
                                FilePath = image,
                                LabelPath = labelPath
                            };

                            if (File.Exists(labelPath) == true)
                            {
                                var labelContent = File.ReadAllText(labelPath, Encoding.UTF8);
                                try
                                {
                                    var labelCollection = JsonConvert.DeserializeObject<ObservableCollection<ObjectDetectionLabel>>(labelContent);
                                    objectDetectionImage.LabelCollection = labelCollection;
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine(e.Message);
                                }
                            }


                            this.ObjectDetectionMergeSourceImageCollection.Add(objectDetectionImage);
                        }));
                    }


                    try
                    {
                        var labelInfoPath = _directoryPath + Path.DirectorySeparatorChar + "__LabelInfo.json";
                        var labelInfoContext = File.ReadAllText(labelInfoPath, Encoding.UTF8);

                        var targetCollection = JsonConvert.DeserializeObject<ObservableCollection<ObjectDetectionLabel>>(labelInfoContext);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.TargetMergeLabelCollection.Clear();
                            foreach (var target in targetCollection)
                                this.TargetMergeLabelCollection.Add(target);
                        });

                    }
                    catch (Exception e)
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

        public Task LoadMergeTargetObjectDetectionLabelAsync(string target)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(target, "*.jpeg|*.jpg");
                this.ObjectDetectionMergeTargetImageCollection.Clear();
                var task = Task.Run(() =>
                {
                    string _directoryPath = target;
                    foreach (var image in images)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {

                            var directory = Path.GetDirectoryName(image);
                            var fileName = Path.GetFileNameWithoutExtension(image);
                            var labelPath = directory + Path.DirectorySeparatorChar + fileName + ".json";

                            var objectDetectionImage = new ObjectDetectionImage()
                            {
                                FileName = Path.GetFileName(image),
                                FilePath = image,
                                LabelPath = labelPath
                            };

                            if (File.Exists(labelPath) == true)
                            {
                                var labelContent = File.ReadAllText(labelPath, Encoding.UTF8);
                                try
                                {
                                    var labelCollection = JsonConvert.DeserializeObject<ObservableCollection<ObjectDetectionLabel>>(labelContent);
                                    objectDetectionImage.LabelCollection = labelCollection;
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine(e.Message);
                                }
                            }


                            this.ObjectDetectionMergeTargetImageCollection.Add(objectDetectionImage);
                        }));
                    }


                    try
                    {
                        var labelInfoPath = _directoryPath + Path.DirectorySeparatorChar + "__LabelInfo.json";
                        var labelInfoContext = File.ReadAllText(labelInfoPath, Encoding.UTF8);

                        var targetCollection = JsonConvert.DeserializeObject<ObservableCollection<ObjectDetectionLabel>>(labelInfoContext);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.SourceMergeLabelCollection.Clear();
                            foreach (var target in targetCollection)
                                this.TargetMergeLabelCollection.Add(target);
                        });

                    }
                    catch (Exception e)
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


        // Merging Service
    }
}
