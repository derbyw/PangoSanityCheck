using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Aii.Assurance.DropShadowLabelViewModel;

namespace Aii.Assurance.DropShadowsEto
{
    public partial class DropShadowLabel
    {
        public DropShadowLabel() 
        {
            InitializeComponent();
        }

        public DropShadowLabel(DropShadownLabelVM M)
        {
            model = M;
            InitializeComponent();
        }

    }
}
