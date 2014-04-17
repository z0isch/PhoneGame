using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PhoneGameService.Models.Plivo
{
    public class Record
    {
        [XmlAttribute]
        public string action {get;set;}
    }
}
