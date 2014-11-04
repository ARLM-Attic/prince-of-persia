	//-----------------------------------------------------------------------//
	// <copyright file="FontSprite.cs" company="A.D.F.Software">
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

// ---- AngelCode BmFont XML serializer ----------------------
// ---- By DeadlyDan @ deadlydan@gmail.com -------------------
// ---- There's no license restrictions, use as you will. ----
// ---- Credits to http://www.angelcode.com/ -----------------

using System;
//using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace PrinceOfPersia
{
    [Serializable]
    [XmlRoot("font")]
    public class FontFile
    {
        [XmlElement("info")]
        public FontInfo Info
        {
            get;
            set;
        }

        [XmlElement("common")]
        public FontCommon Common
        {
            get;
            set;
        }

        [XmlArray("pages")]
        [XmlArrayItem("page")]
        public List<FontPage> Pages
        {
            get;
            set;
        }

        [XmlArray("chars")]
        [XmlArrayItem("char")]
        public List<FontChar> Chars
        {
            get;
            set;
        }

        [XmlArray("kernings")]
        [XmlArrayItem("kerning")]
        public List<FontKerning> Kernings
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontInfo
    {
        [XmlAttribute("face")]
        public String Face
        {
            get;
            set;
        }

        [XmlAttribute("size")]
        public Int32 Size
        {
            get;
            set;
        }

        [XmlAttribute("bold")]
        public Int32 Bold
        {
            get;
            set;
        }

        [XmlAttribute("italic")]
        public Int32 Italic
        {
            get;
            set;
        }

        [XmlAttribute("charset")]
        public String CharSet
        {
            get;
            set;
        }

        [XmlAttribute("unicode")]
        public Int32 Unicode
        {
            get;
            set;
        }

        [XmlAttribute("stretchH")]
        public Int32 StretchHeight
        {
            get;
            set;
        }

        [XmlAttribute("smooth")]
        public Int32 Smooth
        {
            get;
            set;
        }

        [XmlAttribute("aa")]
        public Int32 SuperSampling
        {
            get;
            set;
        }

        private Rectangle _Padding;
        [XmlAttribute("padding")]
        public String Padding
        {
            get
            {
                return _Padding.X + "," + _Padding.Y + "," + _Padding.Width + "," + _Padding.Height;
            }
            set
            {
                String[] padding = value.Split(',');
                _Padding = new Rectangle(Convert.ToInt32(padding[0]), Convert.ToInt32(padding[1]), Convert.ToInt32(padding[2]), Convert.ToInt32(padding[3]));
            }
        }

        private Point _Spacing;
        [XmlAttribute("spacing")]
        public String Spacing
        {
            get
            {
                return _Spacing.X + "," + _Spacing.Y;
            }
            set
            {
                String[] spacing = value.Split(',');
                _Spacing = new Point(Convert.ToInt32(spacing[0]), Convert.ToInt32(spacing[1]));
            }
        }

        [XmlAttribute("outline")]
        public Int32 OutLine
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontCommon
    {
        [XmlAttribute("lineHeight")]
        public Int32 LineHeight
        {
            get;
            set;
        }

        [XmlAttribute("base")]
        public Int32 Base
        {
            get;
            set;
        }

        [XmlAttribute("scaleW")]
        public Int32 ScaleW
        {
            get;
            set;
        }

        [XmlAttribute("scaleH")]
        public Int32 ScaleH
        {
            get;
            set;
        }

        [XmlAttribute("pages")]
        public Int32 Pages
        {
            get;
            set;
        }

        [XmlAttribute("packed")]
        public Int32 Packed
        {
            get;
            set;
        }

        [XmlAttribute("alphaChnl")]
        public Int32 AlphaChannel
        {
            get;
            set;
        }

        [XmlAttribute("redChnl")]
        public Int32 RedChannel
        {
            get;
            set;
        }

        [XmlAttribute("greenChnl")]
        public Int32 GreenChannel
        {
            get;
            set;
        }

        [XmlAttribute("blueChnl")]
        public Int32 BlueChannel
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontPage
    {
        [XmlAttribute("id")]
        public Int32 ID
        {
            get;
            set;
        }

        [XmlAttribute("file")]
        public String File
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontChar
    {
        [XmlAttribute("id")]
        public Int32 ID
        {
            get;
            set;
        }

        [XmlAttribute("x")]
        public Int32 X
        {
            get;
            set;
        }

        [XmlAttribute("y")]
        public Int32 Y
        {
            get;
            set;
        }

        [XmlAttribute("width")]
        public Int32 Width
        {
            get;
            set;
        }

        [XmlAttribute("height")]
        public Int32 Height
        {
            get;
            set;
        }

        [XmlAttribute("xoffset")]
        public Int32 XOffset
        {
            get;
            set;
        }

        [XmlAttribute("yoffset")]
        public Int32 YOffset
        {
            get;
            set;
        }

        [XmlAttribute("xadvance")]
        public Int32 XAdvance
        {
            get;
            set;
        }

        [XmlAttribute("page")]
        public Int32 Page
        {
            get;
            set;
        }

        [XmlAttribute("chnl")]
        public Int32 Channel
        {
            get;
            set;
        }
    }

    [Serializable]
    public class FontKerning
    {
        [XmlAttribute("first")]
        public Int32 First
        {
            get;
            set;
        }

        [XmlAttribute("second")]
        public Int32 Second
        {
            get;
            set;
        }

        [XmlAttribute("amount")]
        public Int32 Amount
        {
            get;
            set;
        }
    }

    public class FontLoader
    {
        public static FontFile Load(String filename)
        {
#if ANDROID
            FontFile file;
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            using (Stream stream = Game.Activity.Assets.Open(filename))
            {
                //TextReader tr = new StreamReader(stream);
                file = (FontFile)deserializer.Deserialize(stream);
            }
            return file;

            using (var stream = TitleContainer.OpenStream(filename))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                file = (FontFile)formatter.Deserialize(stream);
            }
            return file;
#else
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            TextReader textReader = new StreamReader(filename);
            FontFile file = (FontFile)deserializer.Deserialize(textReader);
            textReader.Close();
            return file;
#endif
        }
    }


    public class FontRenderer
    {
        public FontRenderer(FontFile fontFile, Texture2D fontTexture)
        {
            _fontFile = fontFile;
            _texture = fontTexture;
            _characterMap = new Dictionary<char, FontChar>();

            foreach (var fontCharacter in _fontFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                _characterMap.Add(c, fontCharacter);
            }
        }

        private Dictionary<char, FontChar> _characterMap;
        private FontFile _fontFile;
        private Texture2D _texture;
        public void DrawString(SpriteBatch spriteBatch, int x, int y, string text)
        {
            DrawString(spriteBatch, new Vector2(x, y), text);
        }

        public void DrawString(SpriteBatch spriteBatch, Vector2 position, string text)
        {
            int dx = (int)position.X;
            int dy = (int)position.Y;
            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    spriteBatch.Draw(_texture, position, sourceRectangle, Color.White);
                    dx += fc.XAdvance;
                }
            }
        }



        public Vector2 MeasureString(string text)
        {
            int dx = 0;
            int dy = 0;
            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    dx += fc.XAdvance;
                    dy = fc.YOffset;
                }
            }
            return new Vector2(dx,dy);
        }


    }
}
