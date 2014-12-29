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
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using PrinceOfPersia.PoN;

namespace PrinceOfPersia.PoN
{
    public class room
    {
        private int _size = 30;
        private int _index = 0;
        private PoN.links _links = new links();


        [XmlElement]
        public block[] block;

        [XmlElement("links")]
        public PoN.links links
        {
            get { return _links; }
            set { _links = value; }
        }

        
        [XmlAttribute("index")]
        public int index
        {
            get { return _index; }
            set { _index = value; }
        }

        public room()
        {
            block = new block[_size];
            for (int x = 0; x < block.Length; x++)
            {
                block[x] = new block();
            }
        }

        public void Serialize()
        {
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(PoN.room));
            //TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_"+ levelName.ToString() +".xml");
            TextWriter writer = new StreamWriter(@"C:\temp\room_pon.xml");
            ax.Serialize(writer, this);
        }

    }
}
