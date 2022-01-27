using DocumentExecutor.Model;
using DocumentExecutor.ViewModel;
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

namespace DocumentExecutor.View
{
    /// <summary>
    /// Interaction logic for AddBatchIdentifiersView.xaml
    /// </summary>
    public partial class AddBatchIdentifiersView : Window
    {
        public IdentifierType CurrentIdentifierType { get; set; }
        public AddBatchIdentifiersView(IdentifierType identifierType, SortedSet<Identifier> identifiers)
        {
            InitializeComponent();
            DataContext = new DataManageVm();
            CurrentIdentifierType = identifierType;
            ((DataManageVm) DataContext).ExecutorRecordsIdentifiers = identifiers;
            ((DataManageVm) DataContext).ExecutorRecordsIdentifiersBatch = new SortedSet<Identifier>();
            Label.Content = identifierType.Name;
        }
    }
}
