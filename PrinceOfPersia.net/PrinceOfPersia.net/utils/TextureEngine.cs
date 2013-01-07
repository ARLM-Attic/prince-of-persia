using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PrinceOfPersia
{
    public enum TextureUnionLocation 
    {
        Right,
        Bottom,
        Top,
        Left
    }

    public static class TextureEngine
    {
        /// http://gamedev.stackexchange.com/questions/11584/xna-splitting-one-large-texture-into-an-array-of-smaller-textures
        /// <summary>
        /// Splits a texture into an array of smaller textures of the specified size.
        /// </summary>
        /// <param name="original">The texture to be split into smaller textures</param>
        /// <param name="partWidth">The width of each of the smaller textures that will be contained in the returned array.</param>
        /// <param name="partHeight">The height of each of the smaller textures that will be contained in the returned array.</param>
        /// <returns>A multidimensional array represting the rows/coulmns in the texture.</returns>
        public static Texture2D[,] Split(this Texture2D original, int partWidth, int partHeight, out int xCount, out int yCount)
        {
            yCount = original.Height / partHeight; //+ (partHeight % original.Height == 0 ? 0 : 1);//The number of textures in each horizontal row
            xCount = original.Width / partWidth; //+(partWidth % original.Width == 0 ? 0 : 1);//The number of textures in each vertical column
            Texture2D[,] r = new Texture2D[yCount,xCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            //int index = 0;
            int currxidx = 0;
            int curryidx = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
            {
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[curryidx, currxidx] = part;
                    curryidx++;
                }
                curryidx = 0;
                curryidx++;
            }
            //Return the array of parts.
            return r;
        }

        // http://gamedev.stackexchange.com/questions/11584/xna-splitting-one-large-texture-into-an-array-of-smaller-textures
        /// <summary>
        /// Splits a texture into an array of smaller textures of the specified size.
        /// </summary>
        /// <param name="original">The texture to be split into smaller textures</param>
        /// <param name="partWidth">The width of each of the smaller textures that will be contained in the returned array.</param>
        /// <param name="partHeight">The height of each of the smaller textures that will be contained in the returned array.</param>
        /// <param name="offsetWidth">The width offset whitespace to ignore</param>
        /// <param name="offsetHeight">The height offset whitespace to ignore</param>
        /// <param name="xCount">The number of textures per row</param>
        /// <param name="yCount">The number of texture per column</param>
        /// <returns>A multidimensional array represting the rows/coulmns in the texture.</returns>
        public static Texture2D[,] Split(this Texture2D original, int partWidth, int partHeight, int offsetWidth, int offsetHeight, out int xCount, out int yCount)
        {
            yCount = original.Height / partHeight; //+ (partHeight % original.Height == 0 ? 0 : 1);//The number of textures in each horizontal row
            xCount = original.Width / partWidth; //+(partWidth % original.Width == 0 ? 0 : 1);//The number of textures in each vertical column
            
            //xCount -= (xCount % offsetWidth);
            //yCount -= (yCount % offsetHeight);
            Texture2D[,] r = new Texture2D[yCount, xCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            //int index = 0;
            int currxidx = 0;
            int curryidx = 0;
            for (int y = 0; y < yCount * partHeight; y += (partHeight + offsetHeight))
            {
                for (int x = 0; x < xCount * partWidth; x += (partWidth + offsetWidth))
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[curryidx, currxidx] = part;
                    currxidx++;
                }
                currxidx = 0;
                curryidx++;
            }
            //Return the array of parts.
            return r;
        }

        /// http://gamedev.stackexchange.com/questions/11584/xna-splitting-one-large-texture-into-an-array-of-smaller-textures
        /// <summary>
        /// Splits a texture into an array of smaller textures of the specified size.
        /// </summary>
        /// <param name="original">The texture to be split into smaller textures</param>
        /// <param name="partWidth">The width of each of the smaller textures that will be contained in the returned array.</param>
        /// <param name="partHeight">The height of each of the smaller textures that will be contained in the returned array.</param>
        public static Texture2D[] SplitFlat(this Texture2D original, int partWidth, int partHeight, out int xCount, out int yCount)
        {
            yCount = original.Height / partHeight; //+ (partHeight % original.Height == 0 ? 0 : 1);//The number of textures in each horizontal row
            xCount = original.Width / partWidth; //+(partWidth % original.Width == 0 ? 0 : 1);//The number of textures in each vertical column
            Texture2D[] r = new Texture2D[xCount * yCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[index++] = part;
                }
            //Return the array of parts.
            return r;
        }

        /// http://forums.create.msdn.com/forums/t/79258.aspx
        /// <summary>
        /// Combines one texture with another
        /// </summary>
        /// <param name="original">The first texture</param>
        /// <param name="texturetoadd">The second texture</param>
        /// <param name="loc">The location where to put the texture in reference to the first</param>
        /// <returns></returns>
        public static Texture2D Union(this Texture2D original, Texture2D texturetoadd, TextureUnionLocation loc)
        {
            int newWidth = 0;
            int newHeight = 0;
            if (loc == TextureUnionLocation.Right || loc == TextureUnionLocation.Left)
            {
                newWidth = original.Width + texturetoadd.Width;
                newHeight = original.Height;
            }
            else if (loc == TextureUnionLocation.Bottom || loc == TextureUnionLocation.Top)
            {
                newWidth = original.Width;
                newHeight = original.Height + texturetoadd.Height;
            }
            Texture2D r = new Texture2D(original.GraphicsDevice, newWidth, newHeight);
            Color[] originaldata = new Color[original.Width * original.Height];
            Color[] texturetoadddata = new Color[texturetoadd.Width * texturetoadd.Height];
            Color[] newtexturedata = new Color[newHeight * newWidth];

            original.GetData(originaldata);
            texturetoadd.GetData(texturetoadddata);

            if (loc == TextureUnionLocation.Right)
            {
                r.SetData(0, new Rectangle(0, 0, original.Width, original.Height), originaldata, 0, original.Width * original.Height);

                r.SetData(0, new Rectangle(original.Width, 0, texturetoadd.Width, texturetoadd.Height), texturetoadddata, 0, texturetoadd.Width * texturetoadd.Height);
            }
            else if (loc == TextureUnionLocation.Bottom)
            {
                r.SetData(0, new Rectangle(0, 0, original.Width, original.Height), originaldata, 0, original.Width * original.Height);

                r.SetData(0, new Rectangle(0, original.Height, texturetoadd.Width, texturetoadd.Height), texturetoadddata, 0, texturetoadd.Width * texturetoadd.Height);
            }
            else if (loc == TextureUnionLocation.Left)
            {
                r.SetData(0, new Rectangle(0, 0, texturetoadd.Width, texturetoadd.Height), texturetoadddata, 0, texturetoadd.Width * texturetoadd.Height);

                r.SetData(0, new Rectangle(texturetoadd.Width, 0, original.Width, original.Height), originaldata, 0, original.Width * original.Height);
            }
            else if (loc == TextureUnionLocation.Top)
            {
                r.SetData(0, new Rectangle(0, 0, texturetoadd.Width, texturetoadd.Height), texturetoadddata, 0, texturetoadd.Width * texturetoadd.Height);

                r.SetData(0, new Rectangle(0, texturetoadd.Height, original.Width, original.Height), originaldata, 0, original.Width * original.Height);
            }
            

            return r;
        }

    }
}
