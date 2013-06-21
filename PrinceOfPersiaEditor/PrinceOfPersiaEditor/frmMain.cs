using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrinceOfPersiaEditor 
{
  // A form that implement;
  // - Accept drop of MyUserControl objects (recreates and adds object of the type MyUserControl)
  // - A grid for exact positioning of MyUserControl objects (display of the grid and provide a method used by MyUserControl)
  public partial class frmMain: Form 
  {

      public const int SIZE_X = 10;
      public const int SIZE_Y = 3;


    // Constructor
    public frmMain() 
    {
      InitializeComponent();
    }




    //**************************** Handle the Grid ********************************

    private Graphics gridGraphics = null;
    private Pen gridPen = new Pen(Color.LightGray);
    

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
    private void checkBoxGrid_CheckedChanged(object sender, EventArgs e) {
      DisplayOrHideGrid();
   
     
     
        this.Invalidate();
          }




    //********************************* Misc **************************************

    // On closing this form; Exit the application
    private void MyForm_FormClosing(object sender, FormClosingEventArgs e) {
      Application.Exit();
    }

    private void frmMain_Paint(object sender, PaintEventArgs e)
    {

    }



 
  }
}