﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DesignLibrary
{
    //XTabControlTest
    public class TabControl : System.Windows.Forms.TabControl
    {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
        private System.ComponentModel.Container components = null;

        public TabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer 
                | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    
        #region Interop
        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr HWND;
            public uint idFrom;
            public int code;
            public override String ToString()
            {
                return String.Format("Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code);
            }
        }
    
        private const int TCN_FIRST = 0 - 550;       
        private const int TCN_SELCHANGING = (TCN_FIRST - 2);
    
        private const int WM_USER = 0x400;
        private const int WM_NOTIFY = 0x4E;
        private const int WM_REFLECT = WM_USER + 0x1C00;
    
        #endregion
        #region BackColor Manipulation
        //As well as exposing the property to the Designer we want it to behave just like any other 
        //controls BackColor property so we need some clever manipulation.
        private Color m_Backcolor = Color.Empty;

        [Browsable(true),Description("The background color used to display text and graphics in a control.")]
        public override Color BackColor
        {
            get
            {
                if (m_Backcolor.Equals(Color.Empty))
                {
                    if (Parent == null)
                    {
                        //return Color.Green;
                        return Control.DefaultBackColor;
                    }
                    else
                    {
                        return Parent.BackColor;
                        //return Color.Pink;
                    }
                }
                return m_Backcolor;
            }
            set
            {
                if (m_Backcolor.Equals(value)) return;
                m_Backcolor = value;
                Invalidate();
                //Let the Tabpages know that the backcolor has changed.
                base.OnBackColorChanged(EventArgs.Empty);
            }
        }

        public bool ShouldSerializeBackColor()
        {
            return !m_Backcolor.Equals(Color.Empty);
        }
    
        public override void ResetBackColor()
        {
            m_Backcolor = Color.Empty;
            Invalidate();
        }
        #endregion   
       
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged (e);
            Invalidate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged (e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint (e);
            e.Graphics.Clear(BackColor);
            Rectangle r = ClientRectangle;
            if (TabCount <= 0) return;

            //Draw a custom background for Transparent TabPages
            r = SelectedTab.Bounds;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            this.SizeMode = TabSizeMode.FillToRight;

            Font DrawFont = new Font(Font.FontFamily, 24, FontStyle.Regular, GraphicsUnit.Pixel);
            ControlPaint.DrawStringDisabled(e.Graphics, "Micks Ownerdraw TabControl", DrawFont, Color.Green, (RectangleF)r, sf);
            DrawFont.Dispose();

            //Draw a border around TabPage
            r.Inflate(1, 1);
            TabPage tp = TabPages[SelectedIndex];
            SolidBrush PaintBrush = new SolidBrush(Color.Red);
            //e.Graphics.FillRectangle(PaintBrush, r);
            //ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, ButtonBorderStyle.Outset);

            //Draw the Tabs
            for (int index = 0; index <= TabCount - 1; index++)
            {
                tp = TabPages[index];
                r = GetTabRect(index);
                ButtonBorderStyle bs = ButtonBorderStyle.Outset;
                tp.BackColor = Color.FromArgb(54, 54, 54);

                if (index == SelectedIndex) 
                    bs = ButtonBorderStyle.Inset;

                tp.Font = new System.Drawing.Font("Segoe UI", 14);

                if (this.SelectedTab == tp)
                {
                    SizeF size;
                    PaintBrush.Color = Color.WhiteSmoke;

                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                    {
                        size = graphics.MeasureString(tp.Text, new System.Drawing.Font("Segoe UI", 14));
                    }
                    //r.Width = (int)size.Width + 20;
                    e.Graphics.FillRectangle(PaintBrush, r);
                    ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs);
                }
                else
                {
                    SizeF size;
                    PaintBrush.Color = Color.FromArgb(220, 220, 220);

                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                    {
                        size = graphics.MeasureString(tp.Text, new System.Drawing.Font("Segoe UI", 14));
                    }
                    //r.Width = (int)size.Width + 20;
                    e.Graphics.FillRectangle(PaintBrush, r);
                    ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs);
                }
                PaintBrush.Color = tp.ForeColor;

                //Set up rotation for left and right aligned tabs
                if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right)
                {
                    float RotateAngle = 90;
                    if (Alignment == TabAlignment.Left) 
                        RotateAngle = 270;
                    
                    PointF cp = new PointF(r.Left + (r.Width >> 1), r.Top + (r.Height >> 1));
                    cp.X += 10;
                    cp.Y += 10;
                    e.Graphics.TranslateTransform(cp.X, cp.Y);
                    e.Graphics.RotateTransform(RotateAngle);
                    r = new Rectangle(-(r.Height >> 1), -(r.Width-10 >> 1), r.Height, r.Width+10);
                }

                
                SolidBrush testBrush = new SolidBrush(Color.FromArgb(54,54,54));
                
                //Draw the Tab Text
                if (tp.Enabled)
                {
                    e.Graphics.DrawString(tp.Text, Font, testBrush, (RectangleF)r, sf); 
                }
                else
                {
                    ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, testBrush.Color, (RectangleF)r, sf);
                }
      
                e.Graphics.ResetTransform();
            }
            PaintBrush.Dispose();
        }

        [Description("Occurs as a tab is being changed.")]
        public event SelectedTabPageChangeEventHandler SelectedIndexChanging;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (WM_REFLECT + WM_NOTIFY))
            {
                NMHDR hdr = (NMHDR)(Marshal.PtrToStructure(m.LParam, typeof(NMHDR)));
                if (hdr.code == TCN_SELCHANGING)
                {
                    TabPage tp = TestTab(PointToClient(Cursor.Position));
                    if (tp != null)
                    {
                        TabPageChangeEventArgs e = new TabPageChangeEventArgs(SelectedTab, tp);
                        if (SelectedIndexChanging != null)
                            SelectedIndexChanging(this, e);
                        if (e.Cancel || tp.Enabled == false)
                        {
                            m.Result = new IntPtr(1);
                            return;
                        }
                    }
                }
            }
            base.WndProc (ref m);
        }

        private TabPage TestTab(Point pt)
        {
            for (int index = 0; index <= TabCount - 1; index++)
            {
                if (GetTabRect(index).Contains(pt.X, pt.Y))
                    return TabPages[index];
            }
            return null;
        }
    }

    public class TabPageChangeEventArgs : EventArgs
    {
        private TabPage _Selected = null;
        private TabPage _PreSelected = null;
        public bool Cancel = false;
        public TabPage CurrentTab
        {
            get
            {
                return _Selected;
            }
        }

        public TabPage NextTab
        {
            get
            {
                return _PreSelected;
            }
        }

        public TabPageChangeEventArgs(TabPage CurrentTab, TabPage NextTab)
        {
            _Selected = CurrentTab;
            _PreSelected = NextTab;
        }
    }

    public delegate void SelectedTabPageChangeEventHandler(Object sender, TabPageChangeEventArgs e);
}
