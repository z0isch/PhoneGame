using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PhoneGameService.Models.Plivo
{
    public class Speak
    {
        [XmlText]
        public string body {get;set;}

        [XmlAttribute]
        public string loop {get;set;}
        
        [XmlAttribute]
        public string language {get;set;}

        [XmlAttribute]
        public string voice {get;set;}
    }
}
