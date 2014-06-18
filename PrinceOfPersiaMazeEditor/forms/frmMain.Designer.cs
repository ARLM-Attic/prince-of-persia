namespace PrinceOfPersiaMazeEditor
{
  partial class frmMain {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if(disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tblLay = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxGrid = new System.Windows.Forms.CheckBox();
            this.pnl = new System.Windows.Forms.Panel();
            this.lstTile = new System.Windows.Forms.ListView();
            this.imgLst = new System.Windows.Forms.ImageList(this.components);
            this.tblLay.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblLay
            // 
            this.tblLay.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tblLay.ColumnCount = 2;
            this.tblLay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.37425F));
            this.tblLay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.62576F));
            this.tblLay.Controls.Add(this.checkBoxGrid, 0, 1);
            this.tblLay.Controls.Add(this.pnl, 1, 0);
            this.tblLay.Controls.Add(this.lstTile, 0, 0);
            this.tblLay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLay.Location = new System.Drawing.Point(0, 0);
            this.tblLay.Name = "tblLay";
            this.tblLay.RowCount = 2;
            this.tblLay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.43427F));
            this.tblLay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.565737F));
            this.tblLay.Size = new System.Drawing.Size(703, 466);
            this.tblLay.TabIndex = 1;
            this.tblLay.Paint += new System.Windows.Forms.PaintEventHandler(this.tblLay_Paint);
            // 
            // checkBoxGrid
            // 
            this.checkBoxGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxGrid.AutoSize = true;
            this.checkBoxGrid.Location = new System.Drawing.Point(4, 445);
            this.checkBoxGrid.Name = "checkBoxGrid";
            this.checkBoxGrid.Size = new System.Drawing.Size(45, 17);
            this.checkBoxGrid.TabIndex = 2;
            this.checkBoxGrid.Text = "Grid";
            this.checkBoxGrid.UseVisualStyleBackColor = true;
            this.checkBoxGrid.CheckedChanged += new System.EventHandler(this.checkBoxGrid_CheckedChanged);
            // 
            // pnl
            // 
            this.pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(140, 4);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(559, 417);
            this.pnl.TabIndex = 3;
            // 
            // lstTile
            // 
            this.lstTile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTile.Location = new System.Drawing.Point(4, 4);
            this.lstTile.Name = "lstTile";
            this.lstTile.Size = new System.Drawing.Size(129, 417);
            this.lstTile.TabIndex = 4;
            this.lstTile.UseCompatibleStateImageBehavior = false;
            // 
            // imgLst
            // 
            this.imgLst.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgLst.ImageSize = new System.Drawing.Size(16, 16);
            this.imgLst.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 466);
            this.Controls.Add(this.tblLay);
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Smart Drag Drop 1.2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyForm_FormClosing);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.tblLay.ResumeLayout(false);
            this.tblLay.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tblLay;
    private System.Windows.Forms.CheckBox checkBoxGrid;
    private System.Windows.Forms.Panel pnl;
    private System.Windows.Forms.ListView lstTile;
    private System.Windows.Forms.ImageList imgLst;



  }
}

