using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eto.Forms;
using Eto.Drawing;

using Aii.Assurance.BindingBaseViewModel;

namespace Aii.Assurance.DropShadowLabelViewModel
{
    public enum StringAlignment
    {
        Center,
        Far,
        Near
    }

    public enum ContentAlignment
    {
        MiddleLeft,
        BottomCenter,
        MiddleCenter,
        TopCenter,
        BottomLeft,
        TopLeft,
        BottomRight,
        MiddleRight,
        TopRight
    }
    
    public class StringFormat
    {
        public StringAlignment Alignment { get; set; }
        public StringAlignment LineAlignment { get; set; }

        public StringFormat()
        {
            Alignment = StringAlignment.Near;
            LineAlignment = StringAlignment.Center;
        }
    }


    public class DropShadownLabelVM : BindableBase
    {
        private StringFormat mformat;

        public DropShadownLabelVM()
        {
            mformat = new StringFormat();

            yOffset = 1;
            xOffset = 1;
            RemoteLink = false;

            //BackgroundColor = "Colors.Transparent";
            //mForeColor = "Colors.White";
            //mShadowColor = "Colors.Black";

            TextAlign = ContentAlignment.MiddleRight;

            mText = "DropShadowLabel";
            mEnabled = true;
            mVisible = true;
        }

		/// <summary>
		/// Raises the property change. staying on the current thread..
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void RaisePropertyChange(string propertyName)
		{            
			OnPropertyChanged(propertyName);
		}


        public StringFormat Format
        {
            get { return mformat; }            
        }

		public string Tagname { get; set; }

	
        private bool mRemoteLink;
        public bool RemoteLink
        {
			get { return mRemoteLink; }
			set { SetProperty(ref mRemoteLink, value); }
        }


        private float mxOffset;
        public float xOffset
        {
            get { return mxOffset; }
            set { SetProperty(ref mxOffset, value); }
        }

        private float myOffset;
        public float yOffset
        {
            get { return myOffset; }
            set { SetProperty(ref myOffset, value); }
        }

        private string mText;
        public string Text
        {
            get { return mText; }
            set { SetProperty(ref mText, value); }
        }

        private string mToolTip;
        public string ToolTip
        {
            get { return mToolTip; }
            set { SetProperty(ref mToolTip, value); }
        }

        private bool mEnabled;
        public bool Enabled
        {
            get { return mEnabled; }
            set { SetProperty(ref mEnabled, value); }
        }

        private bool mVisible;
        public bool Visible
        {
            get { return mVisible; }
            set { SetProperty(ref mVisible, value); }
        }

        private ContentAlignment mTextAlign;
        public ContentAlignment TextAlign
        {
            get { return mTextAlign; }
            set
            {
                mTextAlign = value;
                SetAlignment();                
            }
        }
        private void SetAlignment()
        {
            switch (TextAlign)
            {
                case ContentAlignment.BottomCenter:
                    Format.LineAlignment = StringAlignment.Far;
                    Format.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    Format.LineAlignment = StringAlignment.Center;
                    Format.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopCenter:
                    Format.LineAlignment = StringAlignment.Near;
                    Format.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    Format.LineAlignment = StringAlignment.Far;
                    Format.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    Format.LineAlignment = StringAlignment.Center;
                    Format.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopLeft:
                    Format.LineAlignment = StringAlignment.Near;
                    Format.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    Format.LineAlignment = StringAlignment.Far;
                    Format.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.MiddleRight:
                    Format.LineAlignment = StringAlignment.Center;
                    Format.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopRight:
                    Format.LineAlignment = StringAlignment.Near;
                    Format.Alignment = StringAlignment.Far;
                    break;
                default:
                    Format.LineAlignment = StringAlignment.Near;
                    Format.Alignment = StringAlignment.Near;
                    break;
            }

        }

    }

}
