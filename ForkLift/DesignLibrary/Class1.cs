using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;


namespace DesignLibrary
{
    public sealed class THEME
    {
        const int TRANSPARENT = 1;
        const int OPAQUE = 2;

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private extern static IntPtr CreatePatternBrush(IntPtr hImage);

        [DllImport("gdi32.dll")]
        private static extern int SetBkMode(IntPtr hdc, int iBkMode);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        private static extern IntPtr GetStockObject(StockObjects fnObject);

        [DllImport("gdi32.dll")]
        private static extern int GetROP2(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern int SetROP2(IntPtr hdc, BinaryRasterOperations ops);

        [DllImport("gdi32.dll")]
        private static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        private static extern int SetROP2(IntPtr hdc, int fnDrawMode);

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private static IntPtr GeneratePatternMask()
        {
            // the size of the tile pattern
            Rectangle patternRect = new Rectangle(0, 0, 4, 4);
            Bitmap patternMask = new Bitmap(patternRect.Width, patternRect.Height, PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(patternMask);

            // colours used in drawing
            Color black = Color.FromArgb(0, 0, 0);
            Color white = Color.FromArgb(255, 255, 255);
            Color transparent = Color.Transparent;

            // fill the bitmap with white
            SolidBrush backgroundBrush = new SolidBrush(white);
            gfx.FillRectangle(backgroundBrush, patternRect);

            // set the pixels to black where the stipple pattern is to appear
            patternMask.SetPixel(0, 0, black);
            patternMask.SetPixel(0, 1, black);
            patternMask.SetPixel(2, 2, black);
            patternMask.SetPixel(2, 3, black);

            return patternMask.GetHbitmap(transparent);
        }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private static IntPtr GenerateStipplePattern(Color lightcol, Color Darkcol)
        {
            // the size of the tile pattern
            Rectangle patternRect = new Rectangle(0, 0, 4, 4);
            Bitmap stipplePattern = new Bitmap(patternRect.Width, patternRect.Height, PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(stipplePattern);
            // colours used in drawing
            Color black = Color.FromArgb(0, 0, 0);
            Color darkerBlue = Darkcol;
            Color lighterBlue = lightcol;
            Color transparent = Color.Transparent;

            SolidBrush backgroundBrush = new SolidBrush(black);
            gfx.FillRectangle(backgroundBrush, patternRect);
            // draw the stipple pattern pixels
            stipplePattern.SetPixel(0, 0, darkerBlue);
            stipplePattern.SetPixel(0, 1, lighterBlue);
            stipplePattern.SetPixel(2, 2, darkerBlue);
            stipplePattern.SetPixel(2, 3, lighterBlue);
            // get a gdi hbitmap from gdi+

            return stipplePattern.GetHbitmap(transparent);
        }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private static IntPtr GeneratePatternMaskBrush()
        {
            IntPtr bgMaskPatternBitmap = GeneratePatternMask();
            // create the mask pattern brush from the monochrome mask bitmap
            IntPtr maskPatternBrush = CreatePatternBrush(bgMaskPatternBitmap);
            // delete the pattern mask bitmap which is no longer required
            DeleteObject(bgMaskPatternBitmap);

            return maskPatternBrush;
        }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private static IntPtr GenerateStipplePatternBrush(Color lightcol, Color Darkcol)
        {   // generate the pattern mask bitmap
            IntPtr bgStipplePatternBitmap = GenerateStipplePattern(lightcol, Darkcol);
            // create the mask pattern brush from the monochrome mask bitmap
            IntPtr stipplePatternBrush = CreatePatternBrush(bgStipplePatternBitmap);
            // delete the pattern mask bitmap which is no longer required
            DeleteObject(bgStipplePatternBitmap);

            return stipplePatternBrush;
        }

        private static void DrawBackgroundGradient(IntPtr dc, Rectangle clientRect, Color lightcol, Color Darkcol)
        {  //Color.FromArgb(41, 57, 85);
            //Color.FromArgb(53, 73, 106);
            // dark at the top and bottom, and lighter in the centre
            Color darkerBlue = Darkcol;
            Color lighterBlue = lightcol;
            // contains the colour for each corner of the gradient rect
            Win32Helper.TRIVERTEX[] vertex = new Win32Helper.TRIVERTEX[4];
            // top-left
            vertex[0].x = 0;
            vertex[0].y = 0;
            vertex[0].Red = (ushort)(darkerBlue.R << 8);
            vertex[0].Green = (ushort)(darkerBlue.G << 8);
            vertex[0].Blue = (ushort)(darkerBlue.B << 8);
            vertex[0].Alpha = (ushort)(darkerBlue.A << 8);
            // center-right
            vertex[1].x = clientRect.Right;
            vertex[1].y = clientRect.Bottom / 2;
            vertex[1].Red = (ushort)(lighterBlue.R << 8);
            vertex[1].Green = (ushort)(lighterBlue.G << 8);
            vertex[1].Blue = (ushort)(lighterBlue.B << 8);
            vertex[1].Alpha = (ushort)(lighterBlue.A << 8);
            // center-left
            vertex[2].x = 0;
            vertex[2].y = clientRect.Bottom / 2;
            vertex[2].Red = (ushort)(lighterBlue.R << 8);
            vertex[2].Green = (ushort)(lighterBlue.G << 8);
            vertex[2].Blue = (ushort)(lighterBlue.B << 8);
            vertex[2].Alpha = (ushort)(lighterBlue.A << 8);
            // bottom-right
            vertex[3].x = clientRect.Right;
            vertex[3].y = clientRect.Bottom;
            vertex[3].Red = (ushort)(darkerBlue.R << 8);
            vertex[3].Green = (ushort)(darkerBlue.G << 8);
            vertex[3].Blue = (ushort)(darkerBlue.B << 8);
            vertex[3].Alpha = (ushort)(darkerBlue.A << 8);
            // rectangle struct references vertices
            // Initialize the data to be used in the call to GradientFill.

            Win32Helper.GRADIENT_RECT[] gRect = new Win32Helper.GRADIENT_RECT[2];
            gRect[0].UpperLeft = 0;
            gRect[0].LowerRight = 1;
            gRect[1].UpperLeft = 2;
            gRect[1].LowerRight = 3;
            // paint the gradient vertically with a triangle-strip
            Win32Helper.GradientFill(dc, vertex, 4, gRect, 2, Win32Helper.GRADIENT_FILL_RECT_V);
        }

        // Draw the stipple pattern over the client rect
        private static void DrawBackgroundStipplePattern(IntPtr dc, Rectangle clientRect, Color lightcol, Color Darkcol)
        {    // remember the default GDI object
            IntPtr prevObject;
            // +1 on height and width to include the right and bottom edges of the rectangle
            Rectangle bgRect = new Rectangle(0, 0, clientRect.Right + 1, clientRect.Bottom + 1);
            // set the dc to not modify the background on drawing
            SetBkMode(dc, TRANSPARENT);
            // select in a null pen
            SelectObject(dc, GetStockObject(StockObjects.NULL_PEN));
            // set the raster operation mode to bit-wise AND the colour at the
            // destination and the colour being written. where the pen is white the
            // colour already in th buffer shows through - where the pen is black, the
            // pixel is turned black. this has the effect of drawing black onto the
            // screen where the pixels in the mask bitmap are black
            int prevROP2Mode = SetROP2(dc, BinaryRasterOperations.R2_MASKPEN);
            // select the mask pattern brush into the device context
            prevObject = SelectObject(dc, GeneratePatternMaskBrush());
            // tile the mask pattern brush over the client area
            Rectangle(dc, bgRect.X, bgRect.Y, bgRect.Width, bgRect.Height);
            // select the default object type back into the dc so the brush can be deleted
            SelectObject(dc, prevObject);
            // bit-wise OR with existing pixel colour - as the pixels where the stipple
            // pattern is to show through are black, OR-ing them with any colour will
            // result in that colour being written to the pixel
            SetROP2(dc, BinaryRasterOperations.R2_MERGEPEN);
            // select the stipple pattern brush into the device context
            prevObject = SelectObject(dc, GenerateStipplePatternBrush(lightcol, Darkcol));
            // tile the stipple pattern over the client area
            Rectangle(dc, bgRect.X, bgRect.Y, bgRect.Width, bgRect.Height);
            // select the default object type back into the dc so the brush can be deleted
            SelectObject(dc, prevObject);
            // restore the default ROP2 mode
            SetROP2(dc, prevROP2Mode);
        }

        public static void DrawBackground(IntPtr HDC, Rectangle Rectangle, Color DarkColor, Color LightColor)
        {
            DrawBackgroundGradient(HDC, Rectangle, LightColor, DarkColor);
            DrawBackgroundStipplePattern(HDC, Rectangle, LightColor, DarkColor);
        }

        #region HELPER CLASSES

        enum BinaryRasterOperations
        {
            R2_BLACK = 1,
            R2_NOTMERGEPEN = 2,
            R2_MASKNOTPEN = 3,
            R2_NOTCOPYPEN = 4,
            R2_MASKPENNOT = 5,
            R2_NOT = 6,
            R2_XORPEN = 7,
            R2_NOTMASKPEN = 8,
            R2_MASKPEN = 9,
            R2_NOTXORPEN = 10,
            R2_NOP = 11,
            R2_MERGENOTPEN = 12,
            R2_COPYPEN = 13,
            R2_MERGEPENNOT = 14,
            R2_MERGEPEN = 15,
            R2_WHITE = 16
        }

        enum StockObjects
        {
            WHITE_BRUSH = 0,
            LTGRAY_BRUSH = 1,
            GRAY_BRUSH = 2,
            DKGRAY_BRUSH = 3,
            BLACK_BRUSH = 4,
            NULL_BRUSH = 5,
            HOLLOW_BRUSH = NULL_BRUSH,
            WHITE_PEN = 6,
            BLACK_PEN = 7,
            NULL_PEN = 8,
            OEM_FIXED_FONT = 10,
            ANSI_FIXED_FONT = 11,
            ANSI_VAR_FONT = 12,
            SYSTEM_FONT = 13,
            DEVICE_DEFAULT_FONT = 14,
            DEFAULT_PALETTE = 15,
            SYSTEM_FIXED_FONT = 16,
            DEFAULT_GUI_FONT = 17,
            DC_BRUSH = 18,
            DC_PEN = 19,
        }

        public sealed class Win32Helper
        {
            public struct TRIVERTEX
            {
                public int x;
                public int y;
                public ushort Red;
                public ushort Green;
                public ushort Blue;
                public ushort Alpha;
                public TRIVERTEX(int x, int y, Color color)
                    : this(x, y, color.R, color.G, color.B, color.A)
                {
                }
                public TRIVERTEX(
                int x, int y,
                ushort red, ushort green, ushort blue,
                ushort alpha)
                {
                    this.x = x;
                    this.y = y;
                    this.Red = (ushort)(red << 8);
                    this.Green = (ushort)(green << 8);
                    this.Blue = (ushort)(blue << 8);
                    this.Alpha = (ushort)(alpha << 8);
                }
            }
            public struct GRADIENT_RECT
            {
                public uint UpperLeft;
                public uint LowerRight;
                public GRADIENT_RECT(uint ul, uint lr)
                {
                    this.UpperLeft = ul;
                    this.LowerRight = lr;
                }
            }

            [DllImport("gdi32.dll", EntryPoint = "GdiGradientFill", ExactSpelling = true)]
            public extern static bool GradientFill(
            IntPtr hdc,
            TRIVERTEX[] pVertex,
            uint dwNumVertex,
            GRADIENT_RECT[] pMesh,
            uint dwNumMesh,
            uint dwMode);

            public const int GRADIENT_FILL_RECT_H = 0x00000000;
            public const int GRADIENT_FILL_RECT_V = 0x00000001;

        }

        #endregion
    }
}
