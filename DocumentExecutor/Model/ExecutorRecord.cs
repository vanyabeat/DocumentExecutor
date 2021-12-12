using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocumentExecutor.Model
{
    public class ExecutorRecord
    {
        [Key]
        public string Guid { get; set; }
    }
}
