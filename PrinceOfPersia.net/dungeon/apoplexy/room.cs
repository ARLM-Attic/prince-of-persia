using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class room
    {
        private int size = 30;
        
        
       

        private string _number = "0";

        [XmlElement]
        public tile[] tile;

        [XmlElement]
        public guard[] guard;

        [XmlElement]
        public links links;

        
        [XmlAttribute("number")]
        public string number
        {
            get { return _number; }
            set { _number = value; }
        }

        public room()
        {
            tile = new tile[size];
            for (int x = 0; x < tile.Length; x++)
            {
                tile[x] = new tile();
            }

        }

        //public RoomApoplexy(int sizeX)
        //{
        //    columns = new Column[sizeX];
        //    for(int x = 0; x < columns.Length; x++)
        //    {
        //        columns[x] = new Column();
        //    }
        //}

    }
}
