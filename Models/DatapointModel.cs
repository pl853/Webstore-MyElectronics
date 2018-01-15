using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
 
namespace Webstore_MyElectronics.Models
{
	//DataContract for Serializing Data - required to serve in JSON format
	[DataContract]
	   public class DataPoint
    {
        public DataPoint(string label, decimal y)
        {
            this.Label = label;
            this.Y = y;
        }

        [DataMember(Name = "y")]
        public Nullable<decimal> Y = null;

        [DataMember(Name = "label")]
        public string Label = null;
    }

}
 