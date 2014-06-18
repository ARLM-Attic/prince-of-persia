using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PrinceOfPersia
{
    public class TexturePoP
    {
        private string name = string.Empty;
        private Texture2D texture = null;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }


    }
}
