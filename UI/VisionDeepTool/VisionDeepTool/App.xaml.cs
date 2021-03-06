using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using VisionDeepTool.Service;
using VisionDeepTool.ViewModel;

namespace VisionDeepTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        public App()
        {
            Services = ConfigureServices();


            this.InitializeComponent();


        }

        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();


            //Service
            services.AddSingleton<SegmentationService>();
            services.AddSingleton<ObjectDetectionService>();
            services.AddSingleton<ApplicationConfigService>();
            services.AddSingleton<ClassificationService>();


            //ViewModel
            //ViewModel등록
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<ReleaseNoteViewModel>();

            // Segmentation ViewModel
            services.AddSingleton<SegmentationTrainViewModel>();
            services.AddSingleton<SegmentationLabelViewModel>();
            services.AddSingleton<SegmentationAugmentationViewModel>();
            services.AddSingleton<SegmentationModelExporterViewModel>();



            // ObjectDetection ViewModel
            services.AddSingleton<ObjectDetectionTrainViewModel>();
            services.AddSingleton<ObjectDetectionLabelViewModel>();
            services.AddSingleton<ObjectDetectionAugmentationViewModel>();
            services.AddSingleton<ObjectDetectionModelExporterViewModel>();



            // Classification ViewModel

            services.AddSingleton<ClassificationLabelViewModel>();


            return services.BuildServiceProvider();
        }

    }
}
