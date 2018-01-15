using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Webstore_MyElectronics.Models
{
    public class DropdownModel
    {
        public pageItems pageItems { get; set; }
    }

    public enum pageItems
    {
        five = 5,
        ten = 10,
        fifteen = 15,
        twenty = 20
    };
}