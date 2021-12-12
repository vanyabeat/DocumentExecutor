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
using Syncfusion.UI.Xaml.Grid;

namespace DocumentExecutor.View
{
    /// <summary>
    /// Interaction logic for DataBaseView.xaml
    /// </summary>
    public partial class DataBaseView : Window
    {
        public static SfDataGrid AllExecutorRecordsView;
        public DataBaseView()
        {
            InitializeComponent();
            DataContext = new DataManageVm();

        }
    }
}
