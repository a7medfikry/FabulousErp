﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_adjustment_relation
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Receive_po")]
        public int Receive_po_id { get; set; }
        [ForeignKey("Main_po")]
        public int Main_po_id { get; set; }
        public float Quantity { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [ForeignKey("Adjustment")]
        public int Adjustment_id { get; set; }
        public Inv_receive_po Receive_po { get; set; }
        public Inv_receive_po Main_po { get; set; }
        public Inv_item_adjustment Adjustment { get; set; }
        public Inv_item Item { get; set; }
    }
}
