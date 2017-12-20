using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;

using Eto.Forms;
using Eto.Drawing;
using Aii.Assurance.Common;
using Aii.Assurance.BindingBaseViewModel;
using Aii.Assurance.DropShadowLabelViewModel;
using Aii.Assurance.AiiCommonEto;


namespace Aii.Assurance.DropShadowsEto
{
 

	partial class DropShadowLabel : AiiPanelBase
    {
        protected DropShadownLabelVM model;
        private SolidBrush shadowBrush;
        private SolidBrush textBrush;

		SynchronizationContext UIctx;

        private Drawable d;
		private const int baseline_height = 20;
		private const int baseline_width = 150;
		private const float baseline_fontsize = 10.0F;
		//private bool need_fontcalc = true;
		private string Fontname = FontFamilies.SansFamilyName;

		private readonly object stateGuard = new object ();


        void InitializeComponent()
        {
            if (model == null) model = new DropShadownLabelVM();

			Size = new Size(baseline_width, baseline_height);

			mBackgroundColor = Colors.Transparent;
			mForeColor = Colors.White;
			mShadowColor = Colors.Black;

			UIctx = SynchronizationContext.Current;

			shadowBrush = new SolidBrush(mShadowColor);
			textBrush = new SolidBrush(mForeColor);


			model.PropertyChanged += Model_PropertyChanged;

            
            /// create our canvas and attach the paint function..
            d = new Drawable();
            d.ToolTip = model.ToolTip;
            d.Paint += OnPaint;

            Content = d;
        }


		/// <summary>
		/// The model has updated (typically on a different thread) and we need to
		/// 
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
        void Model_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {


			//Console.WriteLine (e.PropertyName);
			//Application.Instance.Invoke(DrawWithCreateGraphics);
			//Application.Instance.Invoke(() => d.Invalidate());
			//Application.Instance.Invoke(DrawWithCreateGraphics);
			if (this.Loaded) {
				d.Invalidate ();
			}
        }
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (shadowBrush != null) shadowBrush.Dispose();
                if (textBrush != null) textBrush.Dispose();
				if (textBrush != null) textBrush.Dispose();
            }
            base.Dispose(disposing);
        }

		/*
		void DrawWithCreateGraphics()
		{
			using (var graphics = d.CreateGraphics())
			{
				if (graphics.NativeHandle != IntPtr.Zero) {
					PaintLabel (graphics);	
				}
			}
		}
		*/




		private Color mForeColor;
        public Color ForeColor
        {
            get { 
				return mForeColor; }
            set {
				bool changed = false;
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					changed = mForeColor != value;
					mForeColor = value;
					if (textBrush.Color != mForeColor) {
						textBrush.Dispose ();
						textBrush = null;
						textBrush = new SolidBrush (mForeColor);
					}
				}
				if (changed) Invalidate(); 
				//if (changed) Task.Run(() => DrawAsyncText());
			}
        }        

		private Color mShadowColor;
		public Color ShadowColor
		{
			get { 
				return mShadowColor; }
			set {
				bool changed = false;
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					changed = mShadowColor != value;
					mShadowColor = value;
					if (shadowBrush.Color != mShadowColor) {
						shadowBrush.Dispose ();
						shadowBrush = null;
						shadowBrush = new SolidBrush (mShadowColor);
					}
				}

				if (changed) Invalidate();
				//if (changed) Task.Run(() => DrawAsyncText());
			}
		}        

		private Color mBackgroundColor;
		public Color BackColor
		{
			get { 
				return mBackgroundColor; }
			set {
				bool changed = false;
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					changed = mBackgroundColor != value;
					mBackgroundColor = value;
				}
				if (changed) Invalidate(); 
				//if (changed) Task.Run(() => DrawAsyncText());
			}
		}        

        /// <summary>
        /// supply the font for drawing the text
        /// </summary>
        private Font mFont;
        public Font Font
        {
            get { if (mFont == null) {
					using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
						mFont = new Font (Fontname, 10);
					}
                  }
                  return mFont;
            }

            set { 	
				bool changed = false;
				if (value != null) {
					using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
						if (mFont != null) {							
							mFont.Dispose ();
						}						
						mFont = value;
						changed = true;
					}
				}
				if (changed) Invalidate ();
				//if (changed) DrawAsyncText();
            }
        }

        
        public string Text
        {
            get { return model.Text; }
            set { 
				bool changed = false;
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					if (value != null) {
						changed = model.Text != value;
						model.Text = value;
					}
				}
				//if (changed) Invalidate();
				//if (changed) Task.Run(() => DrawAsyncText());
            }
        }


        public bool RemoteLink
        {
            get { return model.RemoteLink; }
            set
            {
				bool changed = false;
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					changed = model.RemoteLink != value;
					model.RemoteLink = value;
				}
				//if (changed) Invalidate();
				//if (changed) Task.Run (() => DrawAsyncText ());
            }
        }

        public float xOffset
        {
            get { return model.xOffset; }
            set { 
				bool changed = false;
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					changed = model.xOffset != value;
					model.xOffset = value; 
				}
				//if (changed) Invalidate(); 
				//if (changed) Task.Run (() => DrawAsyncText ());
			}
        }

        public float yOffset
        {
            get { return model.yOffset; }
            set { 
				bool changed = false;
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					changed = model.yOffset != value; 
					model.yOffset = value; 
				}
				//if (changed) Invalidate(); 
				//if (changed) Task.Run(() => DrawAsyncText());
			}
        }

        public ContentAlignment TextAlign
        {
            get { return model.TextAlign; }
            set
            {
				using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
					model.TextAlign = value;  
				}
            }
        }

        private void DrawString(Graphics g, string str, Font f, SolidBrush b, RectangleF bound, StringFormat format)
        {
            PointF pos = new PointF(bound.Left, bound.Top);
			SizeF offset = g.MeasureString(f, str);
			//SizeF offset = new SizeF(10,10);

            /*
             * implement the string format functionality here..
             * 
             */
            switch (model.Format.LineAlignment)
            {
                case StringAlignment.Far:
                    pos.Y = bound.Bottom - offset.Height;
                    break;
                case StringAlignment.Center:
                    pos.Y = bound.Center.Y - (offset.Height / 2);
                    break;
                case StringAlignment.Near:
                    pos.Y = bound.Top;
                    break;
            }

            switch (model.Format.Alignment)
            {
                case StringAlignment.Far:
                    pos.X = bound.Right - offset.Width;
                    break;
                case StringAlignment.Center:
                    pos.X = bound.Center.X - (offset.Width / 2);
                    break;
                case StringAlignment.Near:
                    pos.X = bound.Left;
                    break;
            }
            
			g.DrawText(f, b, pos, str);


        }

	private void PaintLabel(Graphics Graphics)
	{
		using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
			RectangleF draw_bounds = new RectangleF (0, 0, d.Width, d.Height);

			if (model.RemoteLink) {				
				if (mBackgroundColor != Colors.Transparent)
					Graphics.Clear (mBackgroundColor);
				DrawString (Graphics, Text, Font,
					textBrush, draw_bounds, model.Format);
			} else {
				if (mBackgroundColor != Colors.Transparent)
					Graphics.Clear (mBackgroundColor);

				RectangleF draw_shadow_pos = new RectangleF (model.xOffset, model.yOffset,
					                             Bounds.Width - model.xOffset, Bounds.Height - model.yOffset);

				DrawString (Graphics, Text, Font, shadowBrush, draw_shadow_pos, model.Format);
				DrawString (Graphics, Text, Font, textBrush, draw_bounds, model.Format);
			}
		}
			
	}

		// obsolete
    private void OnPaint(object sender, PaintEventArgs e)
		{    
			PaintLabel (e.Graphics);
			//DrawWithCreateGraphics();
		}
	}
}
