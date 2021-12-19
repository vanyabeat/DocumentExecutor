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
using DocumentExecutor.ViewModel;

namespace DocumentExecutor.View
{
    /// <summary>
    /// Interaction logic for AddExecutorRecordView.xaml
    /// </summary>
    public partial class AddExecutorRecordView : Window
    {
        public static DatePicker AllOuterNumberDatePicker;
        public static DataGrid AllIdentifiersDataGrid;
        public AddExecutorRecordView()
        {
            InitializeComponent();
            DataContext = new DataManageVm();
            AllOuterNumberDatePicker = ExecutorRecordOutputDateTimePicker;
            AllIdentifiersDataGrid = ExecutorRecordIdentifiersDataGrid;
            DataManageVm.ExecutorRecordOutputNumberDateTime = DateTime.Now;
        }
    }
}
