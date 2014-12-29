	//-----------------------------------------------------------------------//
	// <copyright file="links.cs" company="A.D.F.Software">
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
    public class links
    {
        private int _left = 0;
        private int _right = 0;
        private int _up = 0;
        private int _down = 0;

        [XmlAttribute("left")]
        public int left
        {
            get { return _left; }
            set { _left = value; }
        }


        [XmlAttribute("right")]
        public int right
        {
            get { return _right; }
            set { _right = value; }
        }

        [XmlAttribute("up")]
        public int up
        {
            get { return _up; }
            set { _up = value; }
        }

        [XmlAttribute("down")]
        public int down
        {
            get { return _down; }
            set { _down = value; }
        }

        public links()
        {
        }

   

    }
}
