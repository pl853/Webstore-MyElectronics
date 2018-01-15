using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Webstore_MyElectronics.Models
{
    public class Category
    {
        [Key]
        public int CategoryId{get; set;}

        [DisplayName("Name")]
        [Required(ErrorMessage="A Name is required.")]
        public string CategoryName{get; set;}
        
        [DisplayName("Description")]
        [Required(ErrorMessage="A Description is required.")]
        public string CategoryDescription{get; set;}
        public List<Product> Products{get; set;}
    }
    
}