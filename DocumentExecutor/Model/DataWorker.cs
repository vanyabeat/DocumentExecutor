using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using DocumentExecutor.Model.Data;

namespace DocumentExecutor.Model
{
    public class DataWorker
    {
        public static ObservableCollection<ExecutorRecord> GetAllExecutorRecords(string host, string database, string user, string password, string port)
        {
            try
            {
                using var db = new ApplicationContext(host, database, user, password, port);
                return new ObservableCollection<ExecutorRecord>(db.ExecutorRecords);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return null;
            }

        }
    }
}
