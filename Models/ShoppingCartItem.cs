using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Webstore_MyElectronics.Models
{
     public class ShoppingCartItem
    {
        public int ShoppingCartItemId{get; set;}

        public Product Product{get; set;}
        
        public int Amount {get; set;}

        public string ShoppingCartId{get;set;}
    }

}