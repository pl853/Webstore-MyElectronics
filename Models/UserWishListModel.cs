using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Webstore_MyElectronics.Models
{
    public class UserWishList
    {
        [Key]
        public int ListId{get; set;}
        public ApplicationUser User{get; set;}

        public Product product{get; set;}


    }

}