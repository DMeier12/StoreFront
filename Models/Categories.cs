﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace StoreFront.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CetegoryDescription { get; set; }
        public int? IsDeleted { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}