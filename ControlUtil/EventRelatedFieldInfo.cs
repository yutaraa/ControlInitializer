using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace ControlUtil
{
	/// <summary>
	/// This class contains information of an event and a field relevant to the event.
	/// </summary>
	abstract class EventRelatedFieldInfo
	{
		/// <summary>
		/// Type of the control of this information.
		/// </summary>
		public readonly Type ControlType;

		/// <summary>
		/// EventInfo of this information.
		/// </summary>
		public readonly EventInfo EventInfo;

		/// <summary>
		/// FieldInfo related to the event.
		/// </summary>
		public readonly FieldInfo FieldInfo;

		/// <summary>
		/// How this FieldInfo is used.
		/// </summary>
		public abstract string FieldUsage { get; }

		private bool IsControlTypeMatched( System.Windows.Forms.Control control, bool allowDerived )
		{
			Type argControlType = control.GetType();

			if( argControlType.Equals( this.ControlType ) )
			{
				return true;
			}

			return allowDerived && argControlType.IsSubclassOf( this.ControlType );			
		}

		public EventRelatedFieldInfo( Type controlType, EventInfo eventInfo, FieldInfo fieldInfo )
		{
			if( !controlType.Equals( typeof( System.Windows.Forms.Control ) )
				&& !controlType.IsSubclassOf( typeof( System.Windows.Forms.Control ) ) )
			{
				throw new ArgumentException( $"{nameof( controlType )} must be derived from {typeof( System.Windows.Forms.Control ).FullName}", nameof( controlType ) );
			}

			if( !controlType.Equals( eventInfo.DeclaringType )
				&& !controlType.IsSubclassOf( eventInfo.DeclaringType ) )
			{
				throw new ArgumentException( $"{nameof( eventInfo )} is not an EventInfo of {nameof( controlType )}" );
			}

			if( !controlType.Equals( fieldInfo.DeclaringType )
				&& !controlType.IsSubclassOf( fieldInfo.DeclaringType ) )
			{
				throw new ArgumentException( $"{nameof( fieldInfo )} is not a FieldInfo of {nameof( controlType )}" );
			}

			this.ControlType = controlType;
			this.EventInfo = eventInfo;
			this.FieldInfo = fieldInfo;
		}

		/// <summary>
		/// Get Delegate from specified instance.
		/// </summary>
		/// <param name="control">Control to get delegate. Type of this parameter must equals to or derived from the ControlType property of this instance.</param>
		/// <returns>Delegate added to the event, or null if not added.</returns>
		public Delegate GetDelegate( System.Windows.Forms.Control control )
		{
			if( control == null )
			{
				throw new ArgumentNullException( nameof( control ) );
			}

			if( !this.IsControlTypeMatched( control, true ) )
			{
				throw new ArgumentException( $"{nameof( control )} is irreverent to ${nameof( this.ControlType )}." );
			}

			try
			{
				return this.GetDelegateImpl( control );
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Get added event handler delegate from a Windows form control.
		/// </summary>
		/// <param name="control">Control to get delegate</param>
		/// <returns>delegate added to the event, or null if not added</returns>
		protected abstract Delegate GetDelegateImpl( System.Windows.Forms.Control control );

		/// <summary>
		/// Move added event handler delegat from one control to another.
		/// </summary>
		/// <param name="moveTo">Control moving delegate to. Type of this parameter must equals to or derived from the ControlType property of this instance.</param>
		/// <param name="moveFrom">Control moving delegate from. Type of this parameter must equals to the ControlType property of this instance.</param>
		public void MoveDelegate( System.Windows.Forms.Control moveTo, System.Windows.Forms.Control moveFrom )
		{
			if( moveTo == null )
			{
				throw new ArgumentNullException( nameof( moveTo ) );
			}

			if( moveFrom == null )
			{
				throw new ArgumentNullException( nameof( moveFrom ) );
			}

			if( !this.IsControlTypeMatched( moveTo, true ) )
			{
				throw new ArgumentException( $"{nameof( moveTo )} is irreverent to this ${nameof( this.ControlType )}." );
			}

			if( !this.IsControlTypeMatched( moveFrom, false ) )
			{
				throw new ArgumentException( $"{nameof( moveFrom )} is not type of ${this.ControlType.FullName}." );
			}

			this.HandleDelegate( moveFrom, moveTo, true );
		}

		/// <summary>
		/// Copy added event handler elegate from one control to another.
		/// </summary>
		/// <param name="copyTo">Control copying delegate to. Type of this parameter must equals to or derived from the ControlType property of this instance.</param>
		/// <param name="copyFrom">Control copying delegate from. Type of this parameter must equals to the ControlType property of this instance.</param>
		public void CopyDelegate( System.Windows.Forms.Control copyTo, System.Windows.Forms.Control copyFrom )
		{
			if( copyTo == null )
			{
				throw new ArgumentNullException( nameof( copyTo ) );
			}

			if( copyFrom == null )
			{
				throw new ArgumentNullException( nameof( copyFrom ) );
			}
			if( !this.IsControlTypeMatched( copyTo, true ) )
			{
				throw new ArgumentException( $"{nameof( copyTo )} is irreverent to this ${nameof( this.ControlType )}." );
			}

			if( !this.IsControlTypeMatched( copyFrom, false ) )
			{
				throw new ArgumentException( $"{nameof( copyFrom )} is not type of ${this.ControlType.FullName}." );
			}

			this.HandleDelegate( copyFrom, copyTo, false );
		}

		private void HandleDelegate( System.Windows.Forms.Control fromControl, System.Windows.Forms.Control toControl, bool removeExistDelegate )
		{
			Delegate existDelegate = this.GetDelegate( fromControl );
			if( existDelegate == null )
			{
				return;
			}

			this.EventInfo.AddEventHandler( toControl, ( Delegate )existDelegate.Clone() );

			if( removeExistDelegate )
			{
				this.EventInfo.RemoveEventHandler( fromControl, existDelegate );
			}
		}

		/// <summary>
		/// Remove added event handler delegate from a control.
		/// </summary>
		/// <param name="control">Control to remove event handler Delegate</param>
		/// <returns>True when added evend handler delegate exists and is removed</returns>
		public bool RemoveDelegate( System.Windows.Forms.Control control )
		{
			Delegate existDelegate = this.GetDelegate( control );
			if( existDelegate == null )
			{
				return false;
			}

			this.EventInfo.RemoveEventHandler( control, existDelegate );
			return true;
		}


		/// <summary>
		/// Get information string of this instance.
		/// </summary>
		/// <returns>String of this instance information</returns>
		public string GetInformationString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine( new string( '-', 80 ) );
			sb.AppendLine( $"  Event Name         : {this.EventInfo.Name}" );
			sb.AppendLine( $"  Event Handler Type : {this.EventInfo.EventHandlerType.FullName}" );
			sb.AppendLine( $"  Field Name         : {this.FieldInfo.Name}" );
			sb.AppendLine( $"  Field Usage        : {this.FieldUsage}" );

			return sb.ToString();
		}
	}
}
