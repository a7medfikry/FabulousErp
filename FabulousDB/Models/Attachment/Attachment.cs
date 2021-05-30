using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models.Attachment
{
    public class Attachment_head
    {
        [Key]
        public int Id { get; set; }
        public string Page { get; set; }
        [ForeignKey("General_journal_entry")]
        public int? C_PostingNumber { get; set; }
        public int? Relation_id { get; set; }

        public virtual ICollection<Attachment_files> Attachment_files { get; set; }
        public C_GeneralJournalEntry_Table General_journal_entry { get; set; }
    }
}
