using System;
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;

namespace PrinceOfPersia
{
    class Animation
    {
        private GraphicsDevice _graphicsDevice;

        private const int FRAME_WIDTH = 114;
        private const int FRAME_HEIGHT = 114;


        public enum TYPE_ANIMATION
        {
            KID_STAND,
            KID_TURN,
            KID_STARTRUN,
            KID_RUNSTOP,
            KID_RUNTURN,
            KID_RUNNING
        }


        /// <summary>
        /// All frames in the animation arranged horizontally.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth
        {
            // Assume square frames.
            get { 
                return Texture.Height; 
            }
        }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        /// <summary>
        /// Constructors a new animation.
        /// </summary>        
        public Animation(GraphicsDevice graphicsDevice,  Texture2D[] SetOfTexture, TYPE_ANIMATION TypeAnimation)
        {
            _graphicsDevice = graphicsDevice;
            //setOfTexture = SetOfTexture;

            switch (TypeAnimation)
            {
                case TYPE_ANIMATION.KID_RUNNING:
                    texture = LoadAnimation(KID_RUNNING, SetOfTexture); break;
                case TYPE_ANIMATION.KID_RUNSTOP:
                    texture = LoadAnimation(KID_RUNSTOP, SetOfTexture); break;
                case TYPE_ANIMATION.KID_RUNTURN:
                    texture = LoadAnimation(KID_RUNTURN, SetOfTexture); break;
                case TYPE_ANIMATION.KID_STAND:
                    texture = LoadAnimation(KID_STAND, SetOfTexture); break;
                case TYPE_ANIMATION.KID_STARTRUN:
                    texture = LoadAnimation(KID_STARTRUN, SetOfTexture); break;
                case TYPE_ANIMATION.KID_TURN:
                    texture = LoadAnimation(KID_TURN, SetOfTexture); break;
            }

        }

        private Texture2D LoadAnimation(int[] textureIDs, Texture2D[] setOfTexture)
        {
            bool first = false;
            Texture2D newTexture = null;
           // newTexture = setOfTexture[1];
            for (int x = 0; x < textureIDs.Length; x++)
            {
                if (setOfTexture[textureIDs[x]] == null)
                        continue;
                    if (first == false)
                        newTexture = setOfTexture[textureIDs[x]];
                    else
                        newTexture = TextureEngine.Union(newTexture, setOfTexture[textureIDs[x]], TextureUnionLocation.Right);
                    first = true;
            }
            return newTexture;
        }

        public Texture2D Union(Texture2D original, Texture2D destination)
        {
            int dataPerPart = original.Width * original.Height;
            Color[] partData = new Color[dataPerPart];
            Color[] originalData = new Color[original.Width * original.Height];

            original.GetData<Color>(originalData);
            destination.SetData<Color>(partData);

            return destination;
        }

        /// <summary>
        /// Splits a texture into an array of smaller textures of the specified size.
        /// </summary>
        /// <param name="original">The texture to be split into smaller textures</param>
        /// <param name="partWidth">The width of each of the smaller textures that will be contained in the returned array.</param>
        /// <param name="partHeight">The height of each of the smaller textures that will be contained in the returned array.</param>
        public Texture2D[] Split(Texture2D original, int partWidth, int partHeight, out int xCount, out int yCount)
        {
            yCount = original.Height / partHeight + (partHeight % original.Height == 0 ? 0 : 1);//The number of textures in each horizontal row
            xCount = original.Height / partHeight + (partHeight % original.Height == 0 ? 0 : 1);//The number of textures in each vertical column
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
    }
}
