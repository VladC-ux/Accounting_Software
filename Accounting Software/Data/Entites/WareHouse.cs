﻿using Accounting_Software.Data.Entites;
using Accounting_Software.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Date.Entites
{
    public class WareHouse
    {
        public int Id { get; set; }
        public DateTime DateBuy { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public double Balance { get; set; }
        public Unit_of_mass Unitofmass { get; set; }

       


        

    }

}
