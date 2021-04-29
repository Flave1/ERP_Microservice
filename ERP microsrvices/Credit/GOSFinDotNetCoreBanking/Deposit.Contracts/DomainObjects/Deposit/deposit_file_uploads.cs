using Deposit.Contracts.GeneralExtension;
using System.ComponentModel.DataAnnotations;

namespace Deposit.DomainObjects.Deposit
{
    public class deposit_file_uploads : GeneralEntity
    {
        [Key]
        public long ID { get; set; }
        public string FileGuid { get; set; }
        public byte[] FileByte { get; set; }
        public string  FileName { get; set; }
        public string FullPath { get; set; }
        public string DbPath { get; set; }
        public string Type { get; set; }
        public string Extention { get; set; }
        public long TargetId { get; set; }
    }
}
