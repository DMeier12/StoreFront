﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace StoreFront.Models
{
    public partial class OrderDetails
    {
        public int OrderDetailsID { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public short Quantity { get; set; }
        public string PricePerUnit { get; set; }
    }
}