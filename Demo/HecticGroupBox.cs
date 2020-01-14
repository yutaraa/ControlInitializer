using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
	class HecticGroupBox : GroupBox
	{
		private Timer timer;

		private readonly List<KnownColor> colorList;

		public HecticGroupBox()
			: base()
		{
			this.colorList = new List<KnownColor>();
			foreach( object c in Enum.GetValues( typeof( KnownColor ) ) )
			{
				colorList.Add( ( KnownColor )c );
			}
			colorList.Remove( KnownColor.Transparent );

			this.timer = new Timer();
			timer.Interval = 1000;
			timer.Tick += ( s, e ) => { this.BackColor = Color.FromKnownColor( this.colorList[ Environment.TickCount % this.colorList.Count ] ); };
			timer.Start();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				this.timer?.Dispose();
				this.timer = null;
			}

			base.Dispose( disposing );
		}

	}
}
