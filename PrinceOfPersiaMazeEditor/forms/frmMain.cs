using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
//using XnaAlias = Microsoft.Xna.Framework.Graphics;
//using MonoAlias= Microsoft.Xna.Framework.Content;
//using MonogameGraphics = Microsoft.Xna.Framework.Graphics;
using PrinceOfPersia;

extern alias MonoAlias;
using MonoAlias;
extern alias XnaAlias;
using XnaAlias;

namespace PrinceOfPersiaMazeEditor 
{
  // A form that implement;
  // - Accept drop of MyUserControl objects (recreates and adds object of the type MyUserControl)
  // - A grid for exact positioning of MyUserControl objects (display of the grid and provide a method used by MyUserControl)
  public partial class frmMain: Form 
  {

      public const int SIZE_X = 10;
      public const int SIZE_Y = 3;
      public MonoAlias.ContentManager content;
      //public Monogame..SpriteBatch spriteBatch;
      //public SpriteFont hudFont;
      public SpriteFontControl scontrol;
      public GraphicsDeviceControl gcontrol;
      public PrinceOfPersia.Game persiaGame = new PrinceOfPersia.Game();
      public Xna.Texture2D tx;

    // Constructor
    public frmMain() 
    {
        InitializeComponent();

        //content = new ContentManager(Services, "Content");
        scontrol = new SpriteFontControl();

        content = new MonogameContent.ContentManager(null, "Content");
        //content = new ContentManager(persiaGame.Services, "Content");
        content.RootDirectory = "Content";
        //spriteBatch = new SpriteBatch(PrinceOfPersiaMazeEditor.GraphicsDeviceServic.);

        //hudFont = content.Load<SpriteFont>("Fonts/Hud");
        tx = content.Load<Xna.Texture2D>("Tiles/dos/BlockA");


        MemoryStream mem = new MemoryStream();
        //tx = Texture2D.FromStream(graphicsDevice, mem);
        //tx.SaveAsPng(mem, 0, 0);



        


        //imgLst.Images.Add(Texture2Image(tx));

        //PoPFont = content.Load<SpriteFont>("Fonts/PoP");
    }




    /// <summary>
    /// Convert an XNA Texture2D to a 2D array of Color.
    /// </summary>
    /// <param name="Texture">The texture.</param>
    /// <returns></returns>
    //public static Microsoft.Xna.Framework.Color[,] Texture2DToColorArray2D(Texture2D Texture)
    //{
    //    Microsoft.Xna.Framework.Color[] Colors1D = new Microsoft.Xna.Framework.Color[Texture.Width * Texture.Height];
    //    Texture.GetData(Colors1D);

    //    Microsoft.Xna.Framework.Color[,] Colors2D = new Microsoft.Xna.Framework.Color[Texture.Width, Texture.Height];
    //    for (int x = 0; x < Texture.Width; x++)
    //    {
    //        for (int y = 0; y < Texture.Height; y++)
    //        {
    //            Colors2D[x, y] = Colors1D[x + (y * Texture.Width)];
    //        }
    //    }
    //    return (Colors2D);
    //}


    /// <summary>
    /// Convert a 2D array of Color into a full Texture2D ready to be drawn.
    /// </summary>
    /// <param name="Colors2D">The 2D array of Color data.</param>
    /// <param name="Graphics">The XNA graphics device currently in use.</param>
    /// <returns></returns>
    //public static Texture2D ColorArray2DToTexture2D(Microsoft.Xna.Framework.Color[,] Colors2D, Monogame.Graphics.GraphicsDevice Graphics)
    //{
    //    Xna.Texture2D Texture;
    //    // Figure out the width and height of the new texture,
    //    // by looking at the dimensions of the array.
    //    int TextureWidth = Colors2D.GetUpperBound(0);
    //    int TextureHeight = Colors2D.GetUpperBound(1);
    //    Microsoft.Xna.Framework.Color[] Colors1D = new Microsoft.Xna.Framework.Color[TextureWidth * TextureHeight];

    //    for (int x = 0; x < TextureWidth; x++)
    //    {
    //        for (int y = 0; y < TextureHeight; y++)
    //        {

    //            Colors1D[x + (y * TextureWidth)] = Colors2D[x, y];

    //        }
    //    }

    //    Texture = new Xna.Texture2D(Graphics, TextureWidth, TextureHeight, false, Xna.SurfaceFormat.Color);
    //    Texture.SetData(Colors1D);

    //    return (Texture);
    //}

    //public static System.Drawing.Image Texture2Image(Xna.Texture2D texture)
    //{
    //    if (texture == null)
    //    {
    //        return null;
    //    }

    //    if (texture.IsDisposed)
    //    {
    //        return null;
    //    }

    //    //Memory stream to store the bitmap data.
    //    MemoryStream ms = new MemoryStream();

    //    //Save the texture to the stream.
    //    texture.SaveAsPng(ms, texture.Width, texture.Height);

    //    //Seek the beginning of the stream.
    //    ms.Seek(0, SeekOrigin.Begin);

    //    //Create an image from a stream.
    //    System.Drawing.Image bmp2 = System.Drawing.Bitmap.FromStream(ms);

    //    //Close the stream, we nolonger need it.
    //    ms.Close();
    //    ms = null;
    //    return bmp2;
    //}





    //public static void Image2Texture(System.Drawing.Image image, Monogame.Graphics.GraphicsDevice graphics, ref Xna.Texture2D texture)
    //{
    //    if (image == null)
    //    {
    //        return;
    //    }

    //    if (texture == null || texture.IsDisposed ||
    //        texture.Width != image.Width ||
    //        texture.Height != image.Height ||
    //        texture.Format != SurfaceFormat.Color)
    //    {
    //        if (texture != null && !texture.IsDisposed)
    //        {
    //            texture.Dispose();
    //        }

    //        texture = new Texture2D(graphics, image.Width, image.Height, false, SurfaceFormat.Color);
    //    }
    //    else
    //    {
    //        for (int i = 0; i < 16; i++)
    //        {
    //            if (graphics.Textures[i] == texture)
    //            {
    //                graphics.Textures[i] = null;
    //                break;
    //            }
    //        }
    //    }

    //    //Memory stream to store the bitmap data.
    //    MemoryStream ms = new MemoryStream();

    //    //Save to that memory stream.
    //    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

    //    //Go to the beginning of the memory stream.
    //    ms.Seek(0, SeekOrigin.Begin);

    //    //Fill the texture.
    //    //texture = Texture2D.FromStream(graphics, ms, image.Width, image.Height, false);


    //    //Close the stream.
    //    ms.Close();
    //    ms = null;
    //}


    //**************************** Handle the Grid ********************************

    private Graphics gridGraphics = null;
    private Pen gridPen = new Pen(System.Drawing.Color.LightGray);
    

    // Recalculate the coordinates to snap to grid
    // Note! Used by the MyUserControl object when positioning
    //public Point SnapToGrid(Point dropPoint) 
    //{
    //  // Continue?
    //  if(checkBoxGrid.Checked == false) return dropPoint;
    //  // Snap to grid
    //  int X_snap = (int)(Math.Round((decimal)(dropPoint.X) / grid) * grid);
    //  int Y_snap = (int)(Math.Round((decimal)(dropPoint.Y) / grid) * grid);
    //  // Check that we stay within the visible area
    //  if(X_snap < 0) X_snap = 0;
    //  if(Y_snap < 0) Y_snap = 0;
    //  return new Point(X_snap, Y_snap);
    //}


   

    // Draw the grid or not
    private void DisplayOrHideGrid() 
    {
        
        int grid_width = pnl.Width / SIZE_X;
        int grid_height = pnl.Height / SIZE_Y;
        pnl.Width = grid_width * SIZE_X;

        this.Refresh();

      // Draw a new grid?
        if (checkBoxGrid.Checked == true)
        {
            gridGraphics = pnl.CreateGraphics();
            // Horizontal lines
            for (int X = 1; X <= SIZE_X; X += 1)
            {
                gridGraphics.DrawLine(gridPen, grid_width * X, 0, grid_width * X, pnl.Height);
            }

            // Vertical lines
            for (int Y = 1; Y <= SIZE_Y; Y += 1)
            {
                gridGraphics.DrawLine(gridPen, 0, grid_height * Y, pnl.Width, grid_height * Y);
            }


           

        }
       

    }

    // Show the grid or not
    private void checkBoxGrid_CheckedChanged(object sender, EventArgs e) 
    {
      DisplayOrHideGrid();
      this.Invalidate();
    }




    //********************************* Misc **************************************

    // On closing this form; Exit the application
    private void MyForm_FormClosing(object sender, FormClosingEventArgs e) 
    {
      Application.Exit();
    }

    private void frmMain_Paint(object sender, PaintEventArgs e)
    {

    }


    private void frmMain_Shown(object sender, EventArgs e)
    {

    }

    private void tblLay_Paint(object sender, PaintEventArgs e)
    {

    }
      
        //static Microsoft.Xna.Framework.Color[ GetColorDataFromTexture(Texture2D texture)] 
        //{ 
        //    Microsoft.Xna.Framework.Color colors = new Microsoft.Xna.Framework.Color(texture.Width * texture.Height); 
        //    texture.GetData(colors); 
        //    return colors; 
        //} 

        //public static Bitmap FastTextureToBitmap(Texture2D texture) 
        //{ 
        //    // Setup pointer back to bitmap 
        //    Bitmap newBitmap = new Bitmap(texture.Width, texture.Height); 
 
        //    // Get color data from the texture 
        //    //Microsoft.Xna.Framework.Graphics.Color textureColors = GetColorDataFromTexture(texture); 
        //    Microsoft.Xna.Framework.Color textureColors = GetColorDataFromTexture(texture); 
 
        //    System.Drawing.Imaging.BitmapData bmpData = newBitmap.LockBits(new System.Drawing.Rectangle(0, 0, newBitmap.Width, newBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb); 
 
        //    // Loop through pixels and set values 
        //    unsafe 
        //    { 
        //        byte* bmpPointer = (byte*)bmpData.Scan0; 
        //        for (int y = 0; y < texture.Height; y++) 
        //        { 
        //            for (int x = 0; x < texture.Width; x++) 
        //            { 
 
        //                bmpPointer[0] = textureColors[x + y * texture.Width].B; 
        //                bmpPointer[1] = textureColors[x + y * texture.Width].G; 
        //                bmpPointer[2] = textureColors[x + y * texture.Width].R; 
        //                bmpPointer[3] = textureColors[x + y * texture.Width].A; 
 
        //                bmpPointer += 4; 
 
        //            } 
 
        //            bmpPointer += bmpData.Stride - (bmpData.Width * 4); 
        //        } 
        //    } 
 
        //    textureColors = null; 
        //    newBitmap.UnlockBits(bmpData); 
 
 
        //    return newBitmap; 
        //} 

 
  }
}