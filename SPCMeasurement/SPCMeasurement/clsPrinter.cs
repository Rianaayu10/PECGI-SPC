using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;

namespace SPCMeasurement
{
    class PrintData
    {
        private int mID;
        private string mStringValue = null;
        private Font mFnt = null;
        private int[] mColumnWidth = null;
        private StringFormat[] mCAlign = null;
        private int miLeft;
        private Image mImg = null;
        private int mWidth;
        private int mHeight;

        public PrintData(int ID, string StringValue = "", Font Fnt = null, int[] ColumnWidth = null, StringFormat[] CAlign = null, int iLeft = 0)
        {
            mID = ID;
            mStringValue = StringValue;
            mFnt = Fnt;
            mColumnWidth = ColumnWidth;
            mCAlign = CAlign;
            miLeft = iLeft;
        }

        public PrintData(int ID, Image img)
        {
            mID = ID;
            mImg = img;
        }

        public PrintData(int ID, Image img, int Width, int Height)
        {
            mID = ID;
            mImg = img;
            mWidth = Width;
            mHeight = Height;
        }

        public int ID { 
            get { return mID; } 
            set { mID = value; } 
        }

        public string StringValue
        {
            get { return mStringValue; }
            set { mStringValue = value; }
        }

        public Font Fnt
        {
            get { return mFnt; }
            set { mFnt = value; }
        }

        public int[] ColumnWidth
        {
            get { return mColumnWidth; }
            set { mColumnWidth = value; }
        }

        public StringFormat[] CAlign
        {
            get { return mCAlign; }
            set { mCAlign = value; }
        }

        public int iLeft
        {
            get { return miLeft; }
            set { miLeft = value; }
        }

        public Image img
        {
            get { return mImg; }
            set { mImg = value; }
        }

        public int Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        public int Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }

    }

    public class PrintDataList
    {

    }
}
