using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class field
    {

        private string _key = "";
        private string _value = "";

        [XmlAttribute("key")]
        public string key
        {
            get { return _key; }
            set { _key = value; }
        }


        [XmlAttribute("value")]
        public string value
        {
            get { return _value; }
            set { _value = value; }
        }
           

    }
}
