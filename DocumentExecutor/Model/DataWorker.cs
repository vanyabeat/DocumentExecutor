using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using DocumentExecutor.Model.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace DocumentExecutor.Model
{
    public class DataWorker
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary()
        {
            Source = new Uri(@"pack://application:,,,/Resources/StringResource.xaml")
        };

        #region ExecutorRecords

        public static string CreateExecutorRecord(string guid, string info, Division division, DateTime outputDateTime, string outputNumber, bool empty, bool cd, string jsonIds)
        {
            var result = "";
            using var db = new ApplicationContext();
            var checkIsExist = db.ExecutorRecords.Any(el => el.Guid == guid);
            if (!checkIsExist)
            {
                db.ExecutorRecords.Add(new ExecutorRecord { Guid = guid, Info = info, RecordDataId = null, OutputDivision = division, OutputNumberDateTime = outputDateTime, OutputNumber = outputNumber, Empty = empty, IsCd = cd, IdentifiersJson = jsonIds});
                db.SaveChanges();
                result = guid;
            }

            return result;
        }


        public static string EditExecutorRecord(string guid, string info, Division division, DateTime outputDateTime, string outputNumber, bool empty, bool cd, string jsonIds)
        {
            using (var db = new ApplicationContext())
            {
                var item = db.ExecutorRecords.FirstOrDefault(d => d.Guid == guid);
                item.Info = info;
                item.OutputDivision = division;
                item.OutputNumberDateTime = outputDateTime;
                item.OutputNumber = outputNumber;
                item.Empty = empty;
                item.IsCd = cd;
                item.IdentifiersJson = jsonIds;
                db.SaveChanges();
            }

            return guid;
        }

        public static int CreateExecutorRecordData(string guid, uint size, byte[] data)
        {
            var result = -1;
            using var db = new ApplicationContext();
            var checkIsExist = db.ExecutorRecordDatas.Any(el => el.RecordGuid == guid);
            if (!checkIsExist)
            {
                var entity = db.ExecutorRecordDatas.Add(new ExecutorRecordData { RecordGuid = guid, BytesSize = size, Data = data});
                db.SaveChanges();
                result = entity.Entity.Id;
            }

            return result;
        }

        public static void DeleteExecutorRecordData(string guidRecord)
        {
            using var db = new ApplicationContext();
            try
            {
                var r = db.ExecutorRecordDatas.Any(r => r.RecordGuid == guidRecord);
                if (!r) return;
                {
                    db.ExecutorRecordDatas.Remove(db.ExecutorRecordDatas.FirstOrDefault(p => p.RecordGuid == guidRecord)!);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public static void DeleteExecutorRecord(ExecutorRecord rec)
        {
            using var db = new ApplicationContext();
            try
            {
                var checkIsExist = db.ExecutorRecords.Any(el => el.Guid == rec.Guid);
                if (checkIsExist)
                {
                    db.ExecutorRecords.Remove(rec);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public static byte[] GetExecutorRecordData(string guid)
        {
            using var db = new ApplicationContext();
            try
            {
                var checkIsExist = db.ExecutorRecordDatas.Any(el => el.RecordGuid == guid);
                return checkIsExist ? db.ExecutorRecordDatas.FirstOrDefault(x => x.RecordGuid == guid)?.Data : new byte[1];
            }
            catch (Exception e)
            {
                return new byte[1];
            }
        }

        public static void LinkRecordData(string guid, int dataId, string host, string database, string user, string password, string port)
        {
            using var db = new ApplicationContext();
            if (!db.ExecutorRecords.Any(el => el.Guid == guid) ||
                !db.ExecutorRecordDatas.Any(el => el.Id == dataId)) return;
            var entity = db.ExecutorRecords.FirstOrDefault(a => a.Guid == guid);
            entity.RecordDataId = dataId;
            db.SaveChanges();
        }

        public static uint GetExecutorRecordSizeById(int id)
        {
            using (var db = new ApplicationContext())
            {
                var pos = db.ExecutorRecordDatas.FirstOrDefault(p => p.Id == id);

                return pos.BytesSize;
            }
        }
        #endregion
        public static ObservableCollection<ExecutorRecord> GetAllExecutorRecords()
        {
            try
            {
                using var db = new ApplicationContext();
                return new ObservableCollection<ExecutorRecord>(db.ExecutorRecords);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static StaticData StaticDataContext = App.StaticDataContext;

        public static ObservableCollection<Division> GetAllDivisions()
        {
            return StaticData.Divisions == null ? new ObservableCollection<Division>() : new ObservableCollection<Division>(StaticData.Divisions);
        }


        public static ObservableCollection<IdentifierType> GetAllIdentifierTypes()
        {
            return StaticData.IdentifierTypes == null ? new ObservableCollection<IdentifierType>() : new ObservableCollection<IdentifierType>(StaticData.IdentifierTypes);
        }

        public static Division GetDivisionById(int id)
        {
            return GetAllDivisions().FirstOrDefault(x => x.Id == id);
        }
    }
}
