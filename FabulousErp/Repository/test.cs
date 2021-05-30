using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FabulousErp.Repository
{
    public class test
    {
        public test1 test1 { get; set; }

        public test2 test2 { get; set; }
    }

    public class test1
    {
        [Required(ErrorMessage ="Fill Name")]
        public string Name { get; set; }
    }

    public class test2
    {
        [Required(ErrorMessage = "Fill Address")]
        public string Address { get; set; }
    }
}