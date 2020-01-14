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
	/// This class contains information of an event and a 'Events' property's key field relevant to the event.
	/// </summary>
	class EventRelatedEventsKeyFieldInfo : EventRelatedFieldInfo
	{
		public EventRelatedEventsKeyFieldInfo( Type controlType, EventInfo eventInfo, FieldInfo fieldInfo )
			: base( controlType, eventInfo, fieldInfo )
		{
		}

		public override string FieldUsage => "EventsKey";

		protected override Delegate GetDelegateImpl( Control control )
		{
			PropertyInfo ehlPropertyInfo = control.GetType().GetProperty( "Events", BindingFlags.NonPublic | BindingFlags.Instance );
			System.ComponentModel.EventHandlerList ehl = ( System.ComponentModel.EventHandlerList )ehlPropertyInfo.GetValue( control, null );
			return ehl[ this.FieldInfo.GetValue( control ) ];
		}
	}
}
