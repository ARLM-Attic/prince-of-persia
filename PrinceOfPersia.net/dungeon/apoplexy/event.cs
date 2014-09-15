	//-----------------------------------------------------------------------//
	// <copyright file="event.cs" company="A.D.F.Software">
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
    public class @event
    {

        private string _number = "";
        private string _room = "";
        private string _location = "";
        private string _next = "";

        [XmlAttribute("number")]
        public string number
        {
            get { return _number; }
            set { _number = value; }
        }


        [XmlAttribute("room")]
        public string room
        {
            get { return _room; }
            set { _room = value; }
        }

        [XmlAttribute("location")]
        public string location
        {
            get { return _location; }
            set { _location = value; }
        }

        [XmlAttribute("next")]
        public string next
        {
            get { return _next; }
            set { _next = value; }
        }


   

    }
}
