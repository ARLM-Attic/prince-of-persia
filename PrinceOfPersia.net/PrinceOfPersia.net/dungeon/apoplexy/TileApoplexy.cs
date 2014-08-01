using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class tile
    {

        private string _element = "";
        private string _modifier = "";

        [XmlAttribute("element")]
        public string element
        {
            get { return _element; }
            set { _element = value; }
        }


        [XmlAttribute("modifier")]
        public string modifier
        {
            get { return _modifier; }
            set { _modifier = value; }
        }

        public tile()
        {
        }

   

    }
}
