using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Webstore_MyElectronics.Models
{
    public class Product
    {
        [Key]
        public int ProductId{get; set;}

        [DisplayName("Category")]
        [Required(ErrorMessage = "Product category is required" )]
        public int CategoryId{get; set;}

        public string ProductBrand{get; set;}

        [DisplayName("Name")]
        [Required(ErrorMessage="Product title is required")]
        public string ProductName{get; set;}
    
        [DisplayName("Price")]
        [Required(ErrorMessage="Product price is required")]
        public decimal ProductPrice{get; set;}
        [Required(ErrorMessage = "old price is required you can set it to null if product is not on sale" )]
        public int OldPrice {get; set;}

        [DisplayName("Description")]
        public string ProductDescription{get; set;}

        public string spec1 {get; set;}
        public string spec2 {get; set;}
        public string spec3 {get; set;}
        public string spec4 {get; set;}
        public string spec5 {get; set;}

        public bool OnSale {get; set;}

        [Required(ErrorMessage = "Stock value is required" )]
        public int Stock {get; set;}
        

        public int TimesBought {get; set;}


        [DisplayName("Image")]
        public string ImgUrl{get; set;}
        public virtual Category Category{get; set;}
    }

}