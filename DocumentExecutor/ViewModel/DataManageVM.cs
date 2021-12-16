using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DocumentExecutor.Annotations;
using DocumentExecutor.Model;
using DocumentExecutor.Model.Data;
using DocumentExecutor.View;
using DocumentVisor.Infrastructure;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

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
		public static string SelectedPathDataTypes { get; set; } = Settings["PathDataTypes"].ToString();
		
		// private static readonly ResourceDictionary Dictionary = new ResourceDictionary
		// {
		//     Source = new Uri(@"pack://application:,,,/Resources/StringResource.xaml")
		// };

		public static StaticData StaticTextData;
		#endregion

		#region ExecutorRecord
		public static string ExecutorRecordGuid { get; set; }
		public static string ExecutorRecordInfo { get; set; }
		public static DateTime ExecutorRecordOutputNumberDateTime { get; set; }
		public static string ExecutorRecordOutputNumber { get; set; }
		public static Division ExecutorRecordOutputDivision { get; set; } 
		public static bool ExecutorRecordEmpty { get; set; }
		public static string ExecutorRecordBlobDataPathFolder { get; set; }
		public static bool ExecutorRecordHasCd { get; set; }

		private readonly AsyncRelayCommand<object> _addNewExecutorRecord = null;

		public AsyncRelayCommand<object> AddNewExecutorRecord
		{
			get
			{
				return _addNewExecutorRecord ?? new AsyncRelayCommand<object>(async obj =>
					{
						var wnd = obj as AddExecutorRecordView;

						if (ExecutorRecordGuid == null || ExecutorRecordGuid.Replace(" ", "").Length == 0)
						{
							SetRedBlockControl(wnd, "ExecutorRecordGuidTextBox");
							// ShowMessageToUser(Dictionary["ArticleNameNeedToSelect"].ToString());
						}
						else
						{
							byte[] data = null;

							wnd.ProgressBar.IsIndeterminate = true;
							if (ExecutorRecordBlobDataPathFolder != null)
							{
								 data = await GetZipBytesFromFolder(ExecutorRecordBlobDataPathFolder);
							}

							wnd.ProgressBar.IsIndeterminate = false;

							var result = DataWorker.CreateExecutorRecord(ExecutorRecordGuid, ExecutorRecordInfo, ExecutorRecordOutputDivision, ExecutorRecordOutputNumberDateTime, ExecutorRecordOutputNumber, ExecutorRecordEmpty, ExecutorRecordHasCd, SelectedHost, SelectedDatabase, SelectedUserName, SelectedPassword, SelectedPort );
							if (data != null && data.Length > 0)
							{
								var recordData = DataWorker.CreateExecutorRecordData(result, (uint) data.Length, data, SelectedHost,
									SelectedDatabase, SelectedUserName, SelectedPassword, SelectedPort);
								if (recordData > 0)
								{
									DataWorker.LinkRecordData(result, recordData , SelectedHost, SelectedDatabase, SelectedUserName, SelectedPassword, SelectedPort);
								}
							}
							UpdateAllDataView();
							SetNullValuesToProperties();
							// ClearStackPanelArticlesView(wnd);
							ShowMessageToUser(result);
							wnd.Close();
						}
					}
				);
			}
		}
		
		private readonly AsyncRelayCommand<object> _selectBlobZipFolder = null;
		public AsyncRelayCommand<object> SelectBlobZipFolder
		{
			get
			{
				return _selectBlobZipFolder ?? new AsyncRelayCommand<object>(async obj =>
					{
						var wnd = obj as AddExecutorRecordView;
						var dialog = new CommonOpenFileDialog
						{
							IsFolderPicker = true
						};
						var result = dialog.ShowDialog();
						if (result != CommonFileDialogResult.Ok) return;
						ExecutorRecordBlobDataPathFolder = Path.GetFullPath(dialog.FileName);
						if (wnd != null)
							((wnd.FindName("ExecutorRecordBlobDataPathFolder") as TextBox)!).Text =
								Path.GetFullPath(dialog.FileName);
						return;
					}
				)
				{

				};
			}
		}
		private void SetNullValuesToProperties()
		{
			ExecutorRecordGuid = null;
			ExecutorRecordInfo = null;
			ExecutorRecordOutputNumberDateTime = DateTime.Now;
			ExecutorRecordOutputNumber = null;
			ExecutorRecordOutputDivision = null;
			ExecutorRecordEmpty = false;
			ExecutorRecordHasCd = false;
			ExecutorRecordBlobDataPathFolder = null;

		}

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
						App.StaticDataContext = new StaticData(SelectedPathDataTypes);
						ApplicationContext.DatabaseName = SelectedDatabase;
						ApplicationContext.Host = SelectedHost;
						ApplicationContext.Password = SelectedPassword;
						ApplicationContext.Port = SelectedPort;
						ApplicationContext.User = SelectedUserName;
						var anotherWindow = new DataBaseView();
						UpdateAllDataView();
						SetCenterPositionAndOpenDataBaseWindow(anotherWindow);
						window.Close();
						obj = anotherWindow;
					}
				);
			}
		}

		private readonly RelayCommand<object> _openPathWindow = null;

		public RelayCommand<object> OpenPathWindow
		{
			get
			{
				return _openPathWindow ?? new RelayCommand<object>(obj =>
					{
						var window = obj as Window;
						var folderBrowser = new OpenFileDialog
						{
							ValidateNames = false,
							CheckFileExists = true,
							CheckPathExists = true,
							FileName = "Select file Selection."
						};
						if ((bool) folderBrowser.ShowDialog())
						{
							SelectedPathDataTypes = Path.GetFullPath(folderBrowser.FileName);
							if (window?.FindName("PathTextBox") is TextBox textBox) textBox.Text = SelectedPathDataTypes;
						}
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

		private ObservableCollection<ExecutorRecord> _allExecutorRecords = DataWorker.GetAllExecutorRecords();

		public ObservableCollection<ExecutorRecord> AllExecutorRecords
		{
			get => _allExecutorRecords;
			private set
			{
				_allExecutorRecords = value;
				OnPropertyChanged(nameof(AllExecutorRecords));
			}
		}

		public static ExecutorRecord SelectedExecutorRecord { get; set; }
		#endregion

		#region Divisions

		private ObservableCollection<Division> _allDivisions = DataWorker.GetAllDivisions();

		public ObservableCollection<Division> AllDivisions
		{
			get => _allDivisions;
			private set
			{
				_allDivisions = value;
				OnPropertyChanged(nameof(AllDivisions));
			}
		}


		#endregion Divisions

		#region IdentifierTypes

		private ObservableCollection<IdentifierType> _allIdentifierTypes = DataWorker.GetAllIdentifierTypes();

		public ObservableCollection<IdentifierType> AllIdentifierTypes
		{
			get => _allIdentifierTypes;
			private set
			{
				_allIdentifierTypes = value;
				OnPropertyChanged(nameof(AllIdentifierTypes));
			}
		}

		#endregion
		private void SetCenterPositionAndOpenDataBaseWindow(Window window)
		{
			Application.Current.MainWindow = window;
			window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			window.Show();
		}


		private void SetCenterPositionAndOpen(Window window)
		{
			window.Owner = Application.Current.MainWindow;
			window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			window.ShowDialog();
		}

		private void OpenAddExecutorRecordViewMethod()
		{
			var wnd = new AddExecutorRecordView();
			SetCenterPositionAndOpen(wnd);
		}

		private readonly RelayCommand<object> _openAddQueryWnd = null;

		public RelayCommand<object> OpenAddExecutorRecordWnd
		{
			get
			{
				return _openAddQueryWnd ?? new RelayCommand<object>(obj =>
					{
						OpenAddExecutorRecordViewMethod();
					}
				);
			}
		}

		private void SetRedBlockControl(Window window, string blockName)
		{
			var block = window.FindName(blockName) as Control;
			block.BorderBrush = Brushes.Crimson;
		}

		private void ShowMessageToUser(string message)
		{
			var wnd = new MessageView(message);
			SetCenterPositionAndOpen(wnd);
		}

		private void UpdateAllDataView()
		{
			Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
			UpdateAllExecutorRecordsView();
			Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
		}

		private void UpdateAllExecutorRecordsView()
		{
			AllExecutorRecords = DataWorker.GetAllExecutorRecords();
			DataBaseView.AllExecutorRecordsView.ItemsSource = AllExecutorRecords;
		}

		private static async Task<byte[]> GetZipBytesFromFolder(string path)
		{
			byte[] archiveFile;
			await using (var archiveStream = new MemoryStream())
			{
				using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
				{
					foreach (var file in Directory.GetFiles(path,
								 "*.*",
								 SearchOption.AllDirectories))
					{
						var zipArchiveEntry = archive.CreateEntry(file, CompressionLevel.Fastest);

						await using var zipStream = zipArchiveEntry.Open();
						var fileContent = await File.ReadAllBytesAsync(file);
						await zipStream.WriteAsync(fileContent, 0, fileContent.Length);
					}
				}
				archiveFile = archiveStream.ToArray();
			}
			return await Task.FromResult<byte[]>(archiveFile); 
		}
	}
}