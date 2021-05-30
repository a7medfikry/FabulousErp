﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings
{
    public class Client_DB
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }
    }
}
