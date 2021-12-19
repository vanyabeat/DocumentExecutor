using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DocumentExecutor.Model
{
    public class ExecutorRecord
    {
        [Key]
        public string Guid { get; set; }
        public int? RecordDataId { get; set; }

        [NotMapped]
        public virtual ExecutorRecordData RecordData { get; set; }

        [NotMapped]
        public virtual uint RecordDataSize {
            get
            {
                if (RecordDataId == null)
                {
                    return 0;
                }
                else
                {
                    return DataWorker.GetExecutorRecordSizeById((int) RecordDataId);
                }
            } }
        
        public string Info { get; set; }

        public string OutputNumber { get; set; }

        [NotMapped]
        public virtual DateTime OutputNumberDateTime
        {
            get => new DateTime(OutputNumberDate);
            set => OutputNumberDate = value.Ticks;
        }

        public int OutputDivisionId { get; set; }

        [NotMapped]
        public virtual Division OutputDivision
        {
            get => DataWorker.GetDivisionById(OutputDivisionId);
            set => OutputDivisionId = value.Id;
        }

        public string IdentifiersJson { get; set; }

        public long OutputNumberDate { get; set; }

        public int IsEmpty { get; set; }

        [NotMapped]
        public virtual bool Empty
        {
            get => IsEmpty == 1;
            set => IsEmpty = value ? 1 : 0;
        }

        [NotMapped]
        public virtual bool IsCd
        {
            get => HasCd == 1;
            set => HasCd = value ? 1 : 0;
        }

        public int HasCd { get; set; }

        public int IsExported { get; set; }
       
        [NotMapped]
        public virtual bool Exported
        {
            get => IsExported == 1;
            set => IsExported = value ? 1 : 0;
        }
    }
}
