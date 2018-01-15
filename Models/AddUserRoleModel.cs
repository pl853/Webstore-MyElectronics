using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Webstore_MyElectronics.Models
{
    public class AddUserRole
    {
        public string UserId{get; set;}
        public string NewRole{get;set;}
        public string RemoveRole{get;set;}
        public string Email{get;set;}

        public SelectList Roles{get; set;}
    }

}