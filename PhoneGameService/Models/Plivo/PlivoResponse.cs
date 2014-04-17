using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PhoneGameService.Models.Plivo
{
    [XmlRoot("Response")]
    public class PlivoResponse 
    {
        [XmlElement("Speak")]
        public Speak speak { get; set; }

        [XmlElement("Record")]
        public Record record { get; set; }
    }
}
