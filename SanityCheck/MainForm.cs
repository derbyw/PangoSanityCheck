using System;
using System.Diagnostics;


using System.Runtime.CompilerServices;

using Eto.Forms;
using Eto.Drawing;
using Aii.Assurance.DropShadowsEto;

namespace SanityCheck
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

	/// <summary>
	/// Your application's main form
	/// </summary>
	public  class MainForm : Form
	{

		private Drawable d;
		private Label L1;
		private DropShadowLabel L2;

		private Label L3;
		private Label L4;

		private DropShadowLabel DS1;
		private DropShadowLabel DS2;
		private DropShadowLabel DS3;
		private DropShadowLabel DS4;

		private Process ThisProcess;

		// these should be part of a control
		private int data;
		private Font myFont;
		private string myText;
		private StringFormat myFormat;
		private SolidBrush shadowBrush;
		private SolidBrush textBrush;
		private int ShadowOffset;

		private long initial_total;
		private long pprior;
		private long prior_total;
		private long gprior_total;

		private UITimer timer;


		private readonly object stateGuard = new object ();

		public MainForm ()
		{
			Title = "Pango Leak Sanity Check";
			ClientSize = new Size (800, 500);
			BackgroundColor = Colors.RoyalBlue;

			data = 0;
			myFont = new Font (FontFamilies.SansFamilyName, 12);
			myText = data.ToString();
			myFormat = new StringFormat ();
			myFormat.Alignment = StringAlignment.Center;
			myFormat.LineAlignment = StringAlignment.Center;
			shadowBrush = new SolidBrush (Colors.Black);
			textBrush = new SolidBrush (Colors.White);
			ShadowOffset = 1;


			L1 = new Label { Text = "L1", TextColor = Colors.Wheat };

			L2 = new DropShadowLabel ();

			DS1 = new DropShadowLabel ();
			DS2 = new DropShadowLabel ();
			DS3 = new DropShadowLabel ();
			DS4 = new DropShadowLabel ();



			d = new Drawable();
			d.Size = new Size (200, 50);
			d.Paint += L2_Paint;

			initial_total = GC.GetTotalMemory(true);
			L4 = new Label { Text = string.Format("Initial Memory {0}",initial_total), TextColor = Colors.Wheat };
			L3 = new Label { Text = "L3", TextColor = Colors.Wheat };

				

			timer = new UITimer ();
			timer.Interval = 0.100;
			timer.Elapsed += Timer_Elapsed;
				// table with three rows
			TableLayout L = new TableLayout (
				
					// row with 5 columns
				new TableRow {
					Cells = {
						new TableCell (L1, true),
						new TableCell ("pad", true),
						new TableCell (DS1, true),
						new TableCell ("pad", true)
					}
				},
				                new TableRow {
					Cells = {
						new TableCell (L2, true),
						new TableCell ("pad", true),
						new TableCell (DS2, true),
						new TableCell ("pad", true)
					}
				},
				                new TableRow {
					Cells = {
						new TableCell (L3, true),
						new TableCell ("pad", true),
						new TableCell (DS3, true),
						new TableCell ("pad", true)
					}
				},
				                new TableRow {
					Cells = {
						new TableCell (L4, true),
						new TableCell ("pad", true),
						new TableCell (DS4, true),
						new TableCell ("pad", true)
					}
				});
			
			L.SetColumnScale (1, false);




			Content = L;
			

			// create a few commands that can be used for the menu and toolbar
			var StartStop = new Command {
				MenuText = "Start/Stop",
				ToolBarText = "Start/Stop"
			};
			StartStop.Executed += (sender, e) => {
				if (timer.Started) {
					timer.Stop ();
				} else {
					timer.Start ();
				}
			};

			var MemStatus = new Command {
				MenuText = "Memory Status",
				ToolBarText = "Memory Status"
			};
			MemStatus.Executed += MemStatus_Executed;

			var quitCommand = new Command {
				MenuText = "Quit",
				Shortcut = Application.Instance.CommonModifier | Keys.Q
			};
			quitCommand.Executed += (sender, e) => Application.Instance.Quit ();

			var aboutCommand = new Command { MenuText = "About..." };
			aboutCommand.Executed += (sender, e) => MessageBox.Show (this, "About my app...");

			// create menu
			Menu = new MenuBar {
				Items = {
					// File submenu
					new ButtonMenuItem { Text = "&File", Items = { StartStop } },
				},
				ApplicationItems = {
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
				},
				QuitItem = quitCommand,
				AboutItem = aboutCommand
			};

			// create toolbar			
			ToolBar = new ToolBar { Items = { StartStop, MemStatus} };

			// snapshot of original memory state
			using (Process ThisProcess = Process.GetCurrentProcess()) {
				gprior_total = ThisProcess.PrivateMemorySize64;
			}
			initial_total = GC.GetTotalMemory (true);
		}

		

		void MemStatus_Executed (object sender, EventArgs e)
		{
			using (Process ThisProcess = Process.GetCurrentProcess()) {
				long end_total = GC.GetTotalMemory (true);
				long diff = end_total - initial_total;
				long pend = ThisProcess.PrivateMemorySize64;			    
				long pdiff = pend - gprior_total;

				L1.Text = string.Format ("GC Memory : 0x{0:X8} Total 0x{1:X8}", diff, end_total);
				L3.Text = string.Format ("Process 0x{0:X8} - Total 0x{1:X8}", pdiff, pend);

			}

		}


		/// <summary>
		/// Augmented Drawstring function with ability to format/anchor text in a bounding box
		/// </summary>
		/// <param name="g">The green component.</param>
		/// <param name="str">String.</param>
		/// <param name="f">F.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="bound">Bound.</param>
		/// <param name="format">Format.</param>
		private void DrawString(Graphics g, string str, Font f, SolidBrush b, RectangleF bound, StringFormat format)
		{
			PointF pos = new PointF(bound.Left, bound.Top);
			SizeF offset = g.MeasureString(f, str);

			/*
             * implement the string format functionality here..
             * 
             */
			switch (format.LineAlignment)
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

			switch (format.Alignment)
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




		void L2_Paint (object sender, PaintEventArgs e)
		{
			//using (stateGuard.Lock (TimeSpan.FromMilliseconds (100))) {
				RectangleF draw_bounds = new RectangleF (0, 0, d.Width, d.Height);				
				//if (mBackgroundColor != Colors.Transparent)
					//e.Graphics.Clear (mBackgroundColor);

				RectangleF draw_shadow_pos = new RectangleF (ShadowOffset, ShadowOffset,
					d.Width - ShadowOffset, d.Height - ShadowOffset);

				DrawString (e.Graphics, myText, myFont, shadowBrush, draw_shadow_pos, myFormat);
				DrawString (e.Graphics, myText, myFont, textBrush, draw_bounds, myFormat);
				
			//}
		}

		void Timer_Elapsed (object sender, EventArgs e)
		{
			
			if (++data > 10000) data = 0;
			myText = data.ToString();
			d.Invalidate ();

			L2.Text = data.ToString ();

			int t = data % 23 * 12;
			DS1.Text = t.ToString ();

			t = data % 19 * 27;
			DS2.Text = t.ToString ();

			t = data % 42 * 100;
			DS3.Text = t.ToString ();

			t = data % 71 * 2;
			DS4.Text = t.ToString ();

			if ((data % 10) == 0) {
				MemStatus_Executed (null, null);
			}

		}
	}
}


