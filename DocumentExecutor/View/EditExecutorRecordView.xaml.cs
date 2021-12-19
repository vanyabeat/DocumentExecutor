using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DocumentExecutor.Model;
using DocumentExecutor.ViewModel;
using Newtonsoft.Json;

namespace DocumentExecutor.View
{
    /// <summary>
    /// Interaction logic for EditExecutorRecordView.xaml
    /// </summary>
    public partial class EditExecutorRecordView : Window
    {
        public static DatePicker AllOuterNumberDatePicker;
        public static DataGrid AllIdentifiersDataGrid;
        public EditExecutorRecordView(ExecutorRecord selecetedExecutorRecord)
        {
            InitializeComponent();
            DataContext = new DataManageVm();
            AllOuterNumberDatePicker = ExecutorRecordOutputDateTimePicker;
            AllIdentifiersDataGrid = ExecutorRecordIdentifiersDataGrid;
            DataManageVm.ExecutorRecordOutputNumberDateTime = selecetedExecutorRecord.OutputNumberDateTime;
            DataManageVm.ExecutorRecordGuid = selecetedExecutorRecord.Guid;
            DataManageVm.ExecutorRecordInfo = selecetedExecutorRecord.Info;
            DataManageVm.ExecutorRecordEmpty = selecetedExecutorRecord.Empty;
            DataManageVm.ExecutorRecordHasCd = selecetedExecutorRecord.IsCd;
            DataManageVm.ExecutorRecordOutputDivision = selecetedExecutorRecord.OutputDivision;
            DataManageVm.ExecutorRecordOutputNumber = selecetedExecutorRecord.OutputNumber;
            ((DataManageVm) DataContext).ExecutorRecordsIdentifiers = selecetedExecutorRecord.IdentifiersJson == null ? new SortedSet<Identifier>() : JsonConvert.DeserializeObject<SortedSet<Identifier>>(selecetedExecutorRecord.IdentifiersJson);
           
        }
    }
}
