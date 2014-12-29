	//-----------------------------------------------------------------------//
	// <copyright file="prince.cs" company="A.D.F.Software">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.PoN;

namespace PrinceOfPersia.PoN
{
    public class item
    {
        private string _color = string.Empty;
        private Enumeration.TileType _type = Enumeration.TileType.space;
        private Enumeration.StateTile _state = Enumeration.StateTile.normal;

        [XmlAttribute("type")]
        public Enumeration.TileType type
        {
            get { return _type; }
            set { _type = value; }
        }

        [XmlAttribute("color")]
        public string color
        {
            get { return _color; }
            set { _color = value; }
        }


        [XmlAttribute("state")]
        public Enumeration.StateTile state
        {
            get { return _state; }
            set { _state= value; }
        }


    }
}
