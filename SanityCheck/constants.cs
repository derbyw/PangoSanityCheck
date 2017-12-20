using System;
using System.Globalization;

namespace Aii.Assurance.Common
{

	public struct SizeVM
	{
		public int height { get; set; }
		public int width { get; set; }

		public SizeVM(int width, int height)
		{
			this.width = width;
			this.height = height;
		}
	}
}
