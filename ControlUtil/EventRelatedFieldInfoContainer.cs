using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Reflection.Emit;

namespace ControlUtil
{
	/// <summary>
	/// This class contains a list of EventRelatedFieldInfo.
	/// Each record is related to an event of the specified type of Windows form control.
	/// </summary>
	class EventRelatedFieldInfoContainer
	{
		public readonly Type ControlType;

		private List<EventRelatedFieldInfo> fieldInfoList;

		/// <summary>
		/// Iterate EventRelatedFieldInfo.
		/// </summary>
		/// <returns>Iteration of EventRelatedFieldInfo</returns>
		public IEnumerable<EventRelatedFieldInfo> GetEventRelatedFieldInfos()
		{
			foreach( EventRelatedFieldInfo info in this.fieldInfoList )
			{
				yield return info;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="controlType">Type of control to initialize this instance</param>
		public EventRelatedFieldInfoContainer( Type controlType )
		{
			if( !controlType.Equals( typeof( System.Windows.Forms.Control ))
				&& !controlType.IsSubclassOf( typeof( System.Windows.Forms.Control )))
			{
				throw new ArgumentException( $"{nameof( controlType )} must be derived from {typeof( System.Windows.Forms.Control ).FullName}", nameof( controlType ) );
			}

			// Special check for WebBrowser control.
			if( controlType.Equals( typeof( System.Windows.Forms.WebBrowser ) )
				|| controlType.IsSubclassOf( typeof( System.Windows.Forms.WebBrowser )) )
			{
				if( System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA )
				{
					throw new ArgumentException( $"{controlType.FullName} can only be used in thread set to single thread apartment(STA) mode." );
				}
			}

			this.ControlType = controlType;
			this.FillFieldInfoList();
		}

		/// <summary>
		/// Iterate FieldInfo of specified type.
		/// This method also iteretes FieldInfo of base classes' fields, and it is guaranteed that fields declared in derived class are returned prior to those declared in base classes.
		/// In other words, derived fields precede to base class fields.
		/// </summary>
		/// <param name="targetType">Type of the class to iterate</param>
		/// <returns>Iteration of FieldInfo</returns>
		private IEnumerable<FieldInfo> GetFieldInfos( Type targetType )
		{
			foreach( FieldInfo fi in targetType.GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly ) )
			{
				yield return fi;
			}

			if( targetType.BaseType != null )
			{
				foreach( FieldInfo fi in this.GetFieldInfos( targetType.BaseType ))
				{
					yield return fi;
				}
			}
		}

		/// <summary>
		/// Fills fieldInfoList.
		/// </summary>
		private void FillFieldInfoList()
		{
			this.fieldInfoList = new List<EventRelatedFieldInfo>();

			// Declare a dictinary to store delegate and related EventInfo
			Dictionary<Delegate, EventInfo> delegateToEventInfoDict = new Dictionary<Delegate, EventInfo>();

			// Create control instance dynamically. This instance is used only for analyzation.
			object dynamicInstance = this.ControlType.Assembly.CreateInstance( this.ControlType.FullName );

			// Prepare objects for creating methods to be invoked when events are raised.
			AssemblyName asmName = new AssemblyName();
			asmName.Name = $"DynamicAssemblyFor{this.ControlType.Name}";
			AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly( asmName, AssemblyBuilderAccess.Run );
			ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule( asmName.Name );

			// Add event handlers to all available events.
			// Each loop of this block dynamically creates a method to be invoked when the event is raised.
			// The created method doesn't do anyhing, but it's O.K. We just want to use the created delegate as a key to the event.
			// After creating the method, an event handler delegate to the method is created and added to the event.
			// [Example] In case of MouseDown event, a class like below is created dynamically
			//
			// public class MouseDownEventHandlerContainer
			// {
			//   public static void DynamicEventHandler( object o, MouseEventArgs e )
			//   {
			//     return;
			//   }
			// }
			//
			foreach( EventInfo ei in this.ControlType.GetEvents( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static ) )
			{
				// Define a class that contains event handler delegate method.
				TypeBuilder containerClassTypeBuilder = modBuilder.DefineType( $"{ei.Name}EventHandlerContainer", TypeAttributes.Class | TypeAttributes.Public );

				// Get the signature of event handler delegate method.
				MethodInfo invokeMethodInfo = ei.EventHandlerType.GetMethod( "Invoke" );
				List<ParameterInfo> invokeParameterList = invokeMethodInfo.GetParameters().ToList();
				Type[] invokeParameterTypes = invokeParameterList.ConvertAll<Type>( x => x.ParameterType ).ToArray();

				// Define a method to be invoked when the event is raised.
				// Note : We define this method as a static method, so that we don't need to instantiate the defined class.
				MethodBuilder handlerMethodBuilder = containerClassTypeBuilder.DefineMethod(
					"DynamicEventHandler", MethodAttributes.Public | MethodAttributes.Static, invokeMethodInfo.ReturnType, invokeParameterTypes );

				// Generated method does only 'return'.
				ILGenerator generator = handlerMethodBuilder.GetILGenerator();
				generator.Emit( OpCodes.Ret );

				// Create the defined type.
				Type createdContainerClassType = containerClassTypeBuilder.CreateType();

				// Now we can get created method's information.
				MethodInfo createdHandlerMethodInfo = createdContainerClassType.GetMethod( handlerMethodBuilder.Name );

				// Create an event handler delegate.
				Delegate dlg = Delegate.CreateDelegate( ei.EventHandlerType, createdHandlerMethodInfo );

				// Add created event handler delegate to the event.
				ei.AddEventHandler( dynamicInstance, dlg );

				// Store created event handler delegate with relevant EventInfo to dictionary.
				delegateToEventInfoDict.Add( dlg, ei );
			}

			// Get EventHandlerList from dynamically created control instance.
			PropertyInfo eventsPropertyInfo = this.ControlType.GetProperty( "Events", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty );
			System.ComponentModel.EventHandlerList eventHandlerList = ( System.ComponentModel.EventHandlerList )eventsPropertyInfo.GetValue( dynamicInstance, null );

			// Iterate each field and check if it is relevant to any event.
			foreach( FieldInfo fi in this.GetFieldInfos( this.ControlType ) )
			{
				// Reject inherited fields.
				if( this.fieldInfoList.Exists( x => x.FieldInfo.Name == fi.Name ) )
				{
					continue;
				}

				// Get field value from dynamically created control instance.
				object fieldValue = fi.GetValue( dynamicInstance );

				if( fieldValue is Delegate )
				{
					// This field may be a event hander delegate which we added.
					Delegate eventHandler = fieldValue as Delegate;
					EventInfo ei;
					bool exist = delegateToEventInfoDict.TryGetValue( eventHandler, out ei );
					if( exist )
					{
						// This field is an event handler delegate which we added to an event.
						this.fieldInfoList.Add( new EventRelatedDelegateFieldInfo( this.ControlType, ei, fi ) );
					}

					continue;
				}

				if( eventHandlerList[ fieldValue ] != null )
				{
					// This field is a key to EventHandlerList, but the event handler delegate may not be the one which we added.
					Delegate eventHandler = eventHandlerList[ fieldValue ];
					EventInfo ei;
					bool exist = delegateToEventInfoDict.TryGetValue( eventHandler, out ei );
					if( exist )
					{
						// This field is an object used as a key to 'Events' property, and the related delegate is the one which we added.
						this.fieldInfoList.Add( new EventRelatedEventsKeyFieldInfo( this.ControlType, ei, fi ) );
					}

					continue;
				}
			}

			// Sort filled fieldInfoList by event name.
			this.fieldInfoList.Sort( ( x, y ) => x.EventInfo.Name.CompareTo( y.EventInfo.Name ) );
		}
	}
}
