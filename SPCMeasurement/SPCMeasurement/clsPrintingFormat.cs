using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SPCMeasurement
{
    class clsPrintingFormat
    {
        public Font FntTitle {
            get {
                Font ft = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
                return ft; 
            }
        }
        public Font FntTableHeader {
            get
            {
                Font ft = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
                return ft;
            }
        }

        public Font FntTableCell
        {
            get
            {
                Font ft = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
                return ft;
            }
        }

        private StringFormat mTopLeft = new StringFormat();
        private StringFormat mTopCenter = new StringFormat();
        private StringFormat mTopRight = new StringFormat();
        private StringFormat mMidLeft = new StringFormat();
        private StringFormat mMidCenter = new StringFormat();
        private StringFormat mMidRight = new StringFormat();
        private StringFormat mBotLeft = new StringFormat();
        private StringFormat mBotCenter = new StringFormat();
        private StringFormat mBotRight = new StringFormat();

        public StringFormat TopLeft
        {
            get
            {
                mTopLeft.LineAlignment = StringAlignment.Near;
                mTopLeft.Alignment = StringAlignment.Near;
                return mTopLeft;
            }            
        }

        public StringFormat TopCenter
        {
            get
            {
                mTopCenter.LineAlignment = StringAlignment.Near;
                mTopCenter.Alignment = StringAlignment.Center;
                return mTopCenter;
            }
        }

        public StringFormat TopRight
        {
            get
            {
                mTopRight.LineAlignment = StringAlignment.Near;
                mTopRight.Alignment = StringAlignment.Far;
                return mTopRight;
            }
        }

        public StringFormat MidLeft
        {
            get
            {
                mMidLeft.LineAlignment = StringAlignment.Near;
                mMidLeft.Alignment = StringAlignment.Near;
                return mMidLeft;
            }
        }

        public StringFormat MidCenter
        {
            get
            {
                mMidCenter.LineAlignment = StringAlignment.Near;
                mMidCenter.Alignment = StringAlignment.Center;
                return mMidCenter;
            }
        }

        public StringFormat MidRight
        {
            get
            {
                mMidRight.LineAlignment = StringAlignment.Near;
                mMidRight.Alignment = StringAlignment.Far;
                return mMidRight;
            }
        }

        public StringFormat BotLeft
        {
            get
            {
                mBotLeft.LineAlignment = StringAlignment.Near;
                mBotLeft.Alignment = StringAlignment.Near;
                return mBotLeft;
            }
        }

        public StringFormat BotCenter
        {
            get
            {
                mBotCenter.LineAlignment = StringAlignment.Near;
                mBotCenter.Alignment = StringAlignment.Center;
                return mBotCenter;
            }
        }

        public StringFormat BotRight
        {
            get
            {
                mBotRight.LineAlignment = StringAlignment.Near;
                mBotRight.Alignment = StringAlignment.Far;
                return mBotRight;
            }
        }

        public int PrintCellText(string strValue, int x, int y, int w, System.Drawing.Printing.PrintPageEventArgs e, Font font, StringFormat format, bool border = false, Brush fill = null, int h = 0)
        {
            RectangleF cellRect = new RectangleF();
            cellRect.Location = new Point(x, y);
            if(h > 0)
            {
                cellRect.Size = new Size(w, h);
            } else
            {
                int height = Convert.ToInt32(10 + e.Graphics.MeasureString(strValue, font, w - 10, StringFormat.GenericTypographic).Height);
                cellRect.Size = new Size(w, height);
            }
            return 0;
        }
    }
}
