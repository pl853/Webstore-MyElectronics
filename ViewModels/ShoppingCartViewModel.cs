using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Webstore_MyElectronics.Models;


namespace Webstore_MyElectronics.ViewModels
{
     public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart{get; set;}

        public decimal ShoppingCartTotal{get; set;}
        
    }

}