using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class userdata
    {
        private string _fields = "";

        [XmlAttribute("fields")]
        public string fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        [XmlElement]
        public field[] field;

    }
}
