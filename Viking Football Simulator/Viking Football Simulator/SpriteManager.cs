using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VikingFootballSimulator
{
    public partial class SpriteManager : Component
    {
        public SpriteManager()
        {
            InitializeComponent();
        }

        public SpriteManager(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
