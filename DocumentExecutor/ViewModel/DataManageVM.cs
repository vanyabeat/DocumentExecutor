using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DocumentExecutor.Annotations;
using DocumentExecutor.Model;
using DocumentExecutor.View;
using DocumentVisor.Infrastructure;

namespace DocumentExecutor.ViewModel
{
    public class DataManageVm : INotifyPropertyChanged
    {
        private static readonly ResourceDictionary Settings = new ResourceDictionary
        {
            Source = new Uri(@"pack://application:,,,/settings.xaml")
        };

        #region DataBase

        public static string SelectedUserName { get; set; } = Settings["User"].ToString();
        public static string SelectedPassword { get; set; } = Settings["Password"].ToString();
        public static string SelectedHost { get; set; } = Settings["Host"].ToString();
        public static string SelectedDatabase { get; set; } = Settings["DataBase"].ToString();
        public static string SelectedPort { get; set; } = Settings["Port"].ToString();

        #endregion

        #region Login

        private readonly RelayCommand<object> _openDataBaseWindow = null;

        public RelayCommand<object> OpenDataBaseWindow
        {
            get
            {
                return _openDataBaseWindow ?? new RelayCommand<object>(obj =>
                    {
                        var window = obj as Window;
                        var anotherWindow = new DataBaseView();
                        SetCenterPositionAndOpenDataBaseWindow(anotherWindow);
                        window.Close();
                        obj = anotherWindow;
                    }
                );
            }
        }

        #endregion

        #region MVVM

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region ExecutorRecords

        private ObservableCollection<ExecutorRecord> _alleExecutorRecords = DataWorker.GetAllExecutorRecords(SelectedHost, SelectedDatabase, SelectedUserName, SelectedPassword, SelectedPort);

        public ObservableCollection<ExecutorRecord> AllExecutorRecords
        {
            get => _alleExecutorRecords;
            private set
            {
                _alleExecutorRecords = value;
                OnPropertyChanged(nameof(AllExecutorRecords));
            }
        }

        public static ExecutorRecord SelectedExecutorRecord { get; set; }
        #endregion
        private void SetCenterPositionAndOpenDataBaseWindow(Window window)
        {
            Application.Current.MainWindow = window;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }
    }
}