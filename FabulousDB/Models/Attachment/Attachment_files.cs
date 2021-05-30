using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models.Attachment
{
    public class Attachment_files
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string File { get; set; }
        [ForeignKey("Attachment_head")]
        public int Attachment_id { get; set; }
        public string File_key { get; set; }
        public Attachment_head Attachment_head { get; set; }
    }
    public enum File_type
    {
        image_jpeg = 1,
    }
}
