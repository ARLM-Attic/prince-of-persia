using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PrinceOfPersia
{
    class Utils
    {

        private static XDocument LoadDocument(string documentPath)
        {
            XDocument document = XDocument.Load(documentPath);
            return document;
        }


        public static List<Frame> ParseXMLFrame(string documentPath)
        {

            List<Frame> lFrame = new List<Frame>();

            //const string documentPath = @"C:\Users\a.falappi\documents\visual studio 2010\Projects\WindowsGame2\WindowsGame2\WindowsGame2\KID_sequencexml.xml";
            XDocument document = LoadDocument(documentPath);

            var sequence = from x in document.Descendants("sequence") select x;


            foreach (var localmap in sequence)
            {
                var animation = from x in localmap.Descendants("animation") select x;
                foreach (var frame in animation)
                {
                    Frame cFrame = new Frame();
                    List<XAttribute> framelist = frame.Attributes().ToList();
                    foreach (XAttribute attribute in framelist)
                    {
                        if (attribute.Name.LocalName == "name")
                            cFrame.name = attribute.Value;
                        if (attribute.Name.LocalName == "value")
                            cFrame.value= attribute.Value;
                        
                    }
                    lFrame.Add(cFrame);
                }
            }
            return lFrame;
        }
    }
}
