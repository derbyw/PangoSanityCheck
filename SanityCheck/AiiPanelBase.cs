
using System;
using System.Collections.Generic;
using Aii.Assurance.Common;

using Eto.Forms;
using Eto.Drawing;

namespace Aii.Assurance.AiiCommonEto
{
    /// <summary>
    /// A custom panel
    /// </summary>
    public class AiiPanelBase : Panel
    {

		protected Dictionary<string,Color> ColorLookup;

        public AiiPanelBase()
        {
			ColorLookup = new Dictionary<string, Color> ();
			BuildColorTables ();
            
        }

		/// <summary>
		/// Builds the color tables.
		/// uses both Colors and SystemColors
		/// </summary>
		protected void BuildColorTables()
		{
			ColorLookup.Clear ();
			Type ctype = typeof(Colors); // ctype is static class with static properties
			foreach (var p in ctype.GetProperties())
			{
				var v = p.GetValue(null, null); // static classes cannot be instanced, so use null...
				ColorLookup.Add(p.Name,(Eto.Drawing.Color)v);
			}
			ctype = typeof(SystemColors); // ctype is static class with static properties
			foreach (var p in ctype.GetProperties())
			{
				var v = p.GetValue(null, null); // static classes cannot be instanced, so use null...
				ColorLookup.Add(p.Name,(Eto.Drawing.Color)v);
			}
		}

		/// <summary>
		/// Decodes the color.
		/// currently there is a duplcate in AiiFormBase as well
		/// </summary>
		/// <returns>The color.</returns>
		/// <param name="colorname">Colorname.</param>
		protected Color DecodeColor(string colorname)
		{
			Color c = Colors.BlanchedAlmond;
			if (colorname != null) {
				if (colorname.StartsWith ("#")) {				
					int data = int.Parse (colorname.Substring(1),System.Globalization.NumberStyles.AllowHexSpecifier);
					//Console.WriteLine ("{0:X} = {1}", data, colorname);
					return Color.FromArgb (data);				
				} else {				
					string[] parts = colorname.Split ('.');
					if (parts.Length == 2) {
						string source = parts [0];
						string key = parts [1];
						if (ColorLookup.ContainsKey (key)) {					
							return ColorLookup [key];
						} else {
							/*						 
	            ActiveColor = "Color.FromArgb(255, 17, 25)";
	            CompletedColor = "Color.FromArgb(117, 151, 255)";
	            PendingColor = "Color.FromArgb(0, 140, 143)";
	            			*/
							if (key.Contains ("Color.FromArgb(")) {							
								Console.WriteLine ("From Unknown {0}",colorname);
							} else {
								Console.WriteLine ("Unknown {0}",colorname);
							}
						}							
					}
				}
			} else {
				// crazy default to so undefined colors pop
				c = Colors.HotPink; 
			}
			return c;
		}


		protected TextAlignment DecodeTextAlignment(string option)
		{
			return (TextAlignment)Enum.Parse (typeof(TextAlignment), option);
		}

		protected VerticalAlignment DecodeVertAlignment(string option)
		{			
			return (VerticalAlignment)Enum.Parse (typeof(VerticalAlignment), option);
		}

		protected Size DecodeVMSize(SizeVM	option)
		{
			return new Size (option.width, option.height);
		}

		protected Font DecodeFont(string 	fontinfo)
		{
			float fsize = 10;

			Console.WriteLine ("Font Alloc : {0} ", fontinfo);

			string[] parts = fontinfo.Split (',');
			if (parts.Length > 1) {
				fsize = float.Parse (parts [1]);
			}
			if (parts.Length > 0) {
				return new Font (parts[0], fsize);
			}

			return new Font (FontFamilies.SansFamilyName, fsize);
		}        
    }
}
