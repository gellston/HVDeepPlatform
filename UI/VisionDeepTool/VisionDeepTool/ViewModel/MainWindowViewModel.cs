using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisionDeepTool.ViewModel
{
    public class MainWindowViewModel : ObservableRecipient
    {

        public MainWindowViewModel()
        {
            
        }



        private ObservableObject _CurrentContentViewModel = null;
        public ObservableObject CurrentContentViewModel
        {
            get => _CurrentContentViewModel;
            set => SetProperty(ref _CurrentContentViewModel, value);
        }



        private RelayCommand<ObservableObject> _SelectCurrentMenuCommand = null;
        public RelayCommand<ObservableObject> SelectCurrentMenuCommand
        {

            get
            {
                _SelectCurrentMenuCommand ??= new RelayCommand<ObservableObject>((viewModel) =>
                {

                    try
                    {

                        this.CurrentContentViewModel = viewModel;

                    }catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    }
                });

                return _SelectCurrentMenuCommand;
            }

        }
    }
}
