using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;

namespace ControlUtil
{
	/// <summary>
	/// This class contains information of an event and a delegate field relevant to the event.
	/// </summary>
	class EventRelatedDelegateFieldInfo : EventRelatedFieldInfo
	{
		public EventRelatedDelegateFieldInfo( Type controlType, EventInfo eventInfo, FieldInfo fieldInfo )
			: base( controlType, eventInfo, fieldInfo )
		{
		}

		public override string FieldUsage => "Delegate";

		protected override Delegate GetDelegateImpl( Control control )
		{
			return ( Delegate )this.FieldInfo.GetValue( control );
		}
	}
}
