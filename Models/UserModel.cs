using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Webstore_MyElectronics.Models
{
    public class User
    {
        public List<ApplicationUser> Users{get; set;}


    }

}