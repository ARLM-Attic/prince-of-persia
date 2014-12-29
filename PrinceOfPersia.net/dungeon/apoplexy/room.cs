	//-----------------------------------------------------------------------//
	// <copyright file="room.cs" company="A.D.F.Software">
	// Copyright "A.D.F.Software" (c) 2014 All Rights Reserved
	// <author>Andrea M. Falappi</author>
	// <date>Wednesday, September 24, 2014 11:36:49 AM</date>
	// </copyright>
	//
	// * NOTICE:  All information contained herein is, and remains
	// * the property of Andrea M. Falappi and its suppliers,
	// * if any.  The intellectual and technical concepts contained
	// * herein are proprietary to A.D.F.Software
	// * and its suppliers and may be covered by World Wide and Foreign Patents,
	// * patents in process, and are protected by trade secret or copyright law.
	// * Dissemination of this information or reproduction of this material
	// * is strictly forbidden unless prior written permission is obtained
	// * from Andrea M. Falappi.
	//-----------------------------------------------------------------------//

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        public void Serialize()
        {
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(Apoplexy.room));
            //TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_"+ levelName.ToString() +".xml");
            TextWriter writer = new StreamWriter(@"C:\temp\room_apo.xml");
            ax.Serialize(writer, this);
        }

    }
}
