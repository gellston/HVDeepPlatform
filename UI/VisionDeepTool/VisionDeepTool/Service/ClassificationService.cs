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

        private readonly ApplicationConfigService applicationConfigService;


        public ClassificationService(ApplicationConfigService _applicationConfigService)
        {
            this.applicationConfigService = _applicationConfigService;
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


        // Merging Service

        private ObservableCollection<ClassificationImage> _ClassificationMergeSourceImageCollection = null;
        public ObservableCollection<ClassificationImage> ClassificationMergeSourceImageCollection
        {
            get
            {
                _ClassificationImageCollection ??= new ObservableCollection<ClassificationImage>();
                return _ClassificationMergeSourceImageCollection;
            }
        }


        private ObservableCollection<ClassificationImage> _ClassificationMergeTargetImageCollection = null;
        public ObservableCollection<ClassificationImage> ClassificationMergeTargetImageCollection
        {
            get
            {
                _ClassificationMergeTargetImageCollection ??= new ObservableCollection<ClassificationImage>();
                return _ClassificationMergeTargetImageCollection;
            }
        }

        private ObservableCollection<ClassificationImage> _ClassificationMergeResultImageCollection = null;
        public ObservableCollection<ClassificationImage> ClassificationMergeResultImageCollection
        {
            get
            {
                _ClassificationMergeResultImageCollection ??= new ObservableCollection<ClassificationImage>();
                return _ClassificationMergeResultImageCollection;
            }
        }

        private ObservableCollection<ClassificationLabel> _TargetMergeLabelCollection = null;
        public ObservableCollection<ClassificationLabel> TargetMergeLabelCollection
        {
            get
            {
                _TargetMergeLabelCollection ??= new ObservableCollection<ClassificationLabel>();
                return _TargetMergeLabelCollection;
            }
        }

        private ObservableCollection<ClassificationLabel> _SourceMergeLabelCollection = null;
        public ObservableCollection<ClassificationLabel> SourceMergeLabelCollection
        {
            get
            {
                _SourceMergeLabelCollection ??= new ObservableCollection<ClassificationLabel>();
                return _SourceMergeLabelCollection;
            }
        }

        private ObservableCollection<ClassificationLabel> _ResultMergeLabelCollection = null;
        public ObservableCollection<ClassificationLabel> ResultMergeLabelCollection
        {
            get
            {
                _ResultMergeLabelCollection ??= new ObservableCollection<ClassificationLabel>();
                return _ResultMergeLabelCollection;
            }
        }

        public Task MergeClassificationLabelAsync()
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

        public Task LoadMergeSourceClassificationLabelAsync(string source)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(source, "*.jpeg|*.jpg");
                this.ClassificationMergeSourceImageCollection.Clear();
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

                            var classificationImage = new ClassificationImage()
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
                                    var labelCollection = JsonConvert.DeserializeObject<ObservableCollection<ClassificationLabel>>(labelContent);
                                    classificationImage.LabelCollection = labelCollection;
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine(e.Message);
                                }
                            }


                            this.ClassificationMergeSourceImageCollection.Add(classificationImage);
                        }));
                    }


                    try
                    {
                        var labelInfoPath = _directoryPath + Path.DirectorySeparatorChar + "__LabelInfo.json";
                        var labelInfoContext = File.ReadAllText(labelInfoPath, Encoding.UTF8);

                        var targetCollection = JsonConvert.DeserializeObject<ObservableCollection<ClassificationLabel>>(labelInfoContext);

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

        public Task LoadMergeTargetClassificationLabelAsync(string target)
        {
            try
            {

                var images = Helper.FileSystemHelper.GetFiles(target, "*.jpeg|*.jpg");
                this.ClassificationMergeTargetImageCollection.Clear();
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

                            var classificationImage = new ClassificationImage()
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
                                    var labelCollection = JsonConvert.DeserializeObject<ObservableCollection<ClassificationLabel>>(labelContent);
                                    classificationImage.LabelCollection = labelCollection;
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine(e.Message);
                                }
                            }


                            this.ClassificationMergeTargetImageCollection.Add(classificationImage);
                        }));
                    }


                    try
                    {
                        var labelInfoPath = _directoryPath + Path.DirectorySeparatorChar + "__LabelInfo.json";
                        var labelInfoContext = File.ReadAllText(labelInfoPath, Encoding.UTF8);

                        var targetCollection = JsonConvert.DeserializeObject<ObservableCollection<ClassificationLabel>>(labelInfoContext);

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
