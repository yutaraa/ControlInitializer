using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dump
{
	class Program
	{
		static void Main( string[] args )
		{
			try
			{
				Program p = new Program();
				p.Run( args );
			}
			catch( Exception ex )
			{
				Console.Out.WriteLine( "Unexpected exception." );
				Console.Out.WriteLine( ex.ToString() );
			}
			finally
			{
				if( System.Diagnostics.Debugger.IsAttached )
				{
					Console.Out.WriteLine( "Press any key to continue..." );
					Console.In.ReadLine();
				}
			}
		}

		private void Run( string[] args )
		{
			foreach( string line in ControlUtil.ControlInitializer.DumpControlEvents() )
			{
				Console.Out.Write( line );
			}
		}
	}
}
