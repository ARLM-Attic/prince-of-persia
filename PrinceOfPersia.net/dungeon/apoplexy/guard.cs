	//-----------------------------------------------------------------------//
	// <copyright file="guard.cs" company="A.D.F.Software">
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
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class guard
    {

        private string _location = "";
        private string _direction = "";
        private string _skill = "";
        private string _colors = "";

        [XmlAttribute("location")]
        public string location
        {
            get { return _location; }
            set { _location = value; }
        }


        [XmlAttribute("direction")]
        public string direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        [XmlAttribute("skill")]
        public string skill
        {
            get { return _skill; }
            set { _skill = value; }
        }

        [XmlAttribute("colors")]
        public string colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        public guard()
        {
        }

   

    }
}
