using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Webstore_MyElectronics.Models
{
    public class OrderedProducts
    {
        [Key]
        public int OrderProductsId {get; set;}
        public Product product {get; set;}
        public ApplicationUser User{get; set;}


    }
}