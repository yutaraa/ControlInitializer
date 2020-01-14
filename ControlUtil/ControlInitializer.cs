using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;

namespace ControlUtil
{
  /// <summary>
  /// This class contains several useful methods for Windows form controls.
  /// </summary>
  public static class ControlInitializer
  {

    /// <summary>
    /// Replace existing control.
    /// Type of 'replaceToControl' must equels to or dreived from 'existControl'.
    /// After 'existControl' is replaced to 'replaceToControl', child controls of 'existControl' are moved to 'replaceToControl'.
    /// Parent of 'replaceToControl' is set to that of 'existControl'. Parent of 'existControl' is set to null.
    /// Other property values in 'existControl' are copied to 'replaceToControl', only when the property is browsable.
    /// This is because this method is expected to be used with Visual Studio designer.
    /// For the same reason, field values are not copied.
    /// Event handlers added to 'existControl' are moved to 'replaceToControl'.
    /// </summary>
    /// <param name="existControl">Control to be replaced.</param>
    /// <param name="replaceToControl">Control replacing to.</param>
    public static void ReplaceControl( Control existControl, Control replaceToControl )
    {
      if( existControl == null )
      {
        throw new ArgumentNullException( nameof( existControl ) );
      }

      if( replaceToControl == null )
      {
        throw new ArgumentNullException( nameof( replaceToControl ) );
      }

      if( !replaceToControl.GetType().Equals( existControl.GetType() )
        && !replaceToControl.GetType().IsSubclassOf( existControl.GetType() ) )
      {
        throw new ArgumentException( $"{nameof( replaceToControl )} must equals to or derived from ${nameof( existControl )}" );
      }

      InitializeControl( replaceToControl, existControl, false );

      Control parent = existControl.Parent;
      existControl.Parent = null;
      replaceToControl.Parent = parent;

    }

    /// <summary>
    /// Duplicate exist control.
    /// Child controls are also duplicated from exist control.
    /// Parent of result control is set to null.
    /// Other properites of result control are copied from 'existControl' only when the property is browsable.
    /// This is because this method is expected to be used with Visual Studio designer.
    /// For the same reason, field values are not copied.
    /// Event handlers added to exist control and its child controls are also duplicated.
    /// </summary>
    /// <param name="existControl">Control to be duplicated</param>
    /// <returns>Duplicated control</returns>
    public static Control DuplicateControl( Control existControl )
    {
      if( existControl == null )
      {
        throw new ArgumentNullException( nameof( existControl ) );
      }

      Control newControl = ( Control )existControl.GetType().Assembly.CreateInstance( existControl.GetType().FullName );
      InitializeControl( newControl, existControl, true );

      return newControl;
    }

    /// <summary>
    /// Get public setter property with the specified name.
    /// This method is to avoid AmbiguousMatchException raised when the property is overridden with 'new' keyword.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="type">Type to get PropertyInfo from.</param>
    /// <returns>Found PropertyInfo, or null if not exist.</returns>
    private static PropertyInfo GetPublicSetProperty( string propertyName, Type type )
    {
      Type t = type;

      while( t != null )
      {
        PropertyInfo pi = t.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance| BindingFlags.SetProperty | BindingFlags.DeclaredOnly );
        if( pi != null )
        {
          return pi;
        }

        t = t.BaseType;
      }

      return null;
    }

    private static void InitializeControl( Control targetControl, Control originalControl, bool duplicating )
    {
      #region Handle Control.Controls property.

      // List child controls to move or to duplicate.
      List<Control> childControlList = new List<Control>();
      foreach( Control childControl in originalControl.Controls )
      {
        childControlList.Add( childControl );
      }

      foreach( Control childControl in childControlList )
      {
        if( duplicating )
        {
          Control duplicatedChildControl = DuplicateControl( childControl );
          targetControl.Controls.Add( duplicatedChildControl );
        }
        else
        {
          originalControl.Controls.Remove( childControl );
          targetControl.Controls.Add( childControl );
        }
      }

      #endregion

      #region Handle other properties.

      // Iterate public and readable properties from originalControl.
      foreach( PropertyInfo originalPi in originalControl.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty ) )
      {
        if( !originalPi.CanRead )
        {
          continue;
        }

        // Controls property is already handled.
        if( originalPi.Name == nameof( Control.Controls ) )
        {
          continue;
        }

        // Parent property must not be changed here.
        if( originalPi.Name == nameof( Control.Parent ) )
        {
          continue;
        }

        // Except Name property, not browsable properties are ignored.
        BrowsableAttribute originalBrowsable = originalPi.GetCustomAttribute<BrowsableAttribute>( true );
        if( ( originalBrowsable != null ) && !originalBrowsable.Browsable )
        {
          if( originalPi.Name != nameof( Control.Name ) )
          {
            continue;
          }
        }

        // Get corresponding public and writable property from targetControl.
        // Notice that the type of targetControl may be derived from the type of originalControl.
        // This is allowed when calling ReplaceControl method.
        PropertyInfo targetPi = GetPublicSetProperty( originalPi.Name, targetControl.GetType() );
        if( ( targetPi == null ) || !targetPi.CanWrite || !targetPi.PropertyType.IsAssignableFrom( originalPi.PropertyType ) )
        {
          continue;
        }

        // BrowsableAttribute also may be overwritten.
        BrowsableAttribute targetBrowsable = originalPi.GetCustomAttribute<BrowsableAttribute>( true );
        if( ( targetBrowsable != null ) && !targetBrowsable.Browsable )
        {
          continue;
        }

        object value = originalPi.GetValue( originalControl, null );
        targetPi.SetValue( targetControl, value, null );
      }

      #endregion

      #region Handle events.

      EventRelatedFieldInfoContainer infoContainer = new EventRelatedFieldInfoContainer( originalControl.GetType() );

      foreach( EventRelatedFieldInfo info in infoContainer.GetEventRelatedFieldInfos() )
      {
        if( duplicating )
        {
          info.CopyDelegate( targetControl, originalControl );
        }
        else
        {
          info.MoveDelegate( targetControl, originalControl );
        }
      }

      #endregion

      // Duplicated control's Visible property tends to become false. Force to set true by default.
      targetControl.Visible = true;
    }

    /// <summary>
    /// Remove all added event handlers from a control.
    /// Event handlers added to child controls are also removed.
    /// </summary>
    /// <param name="control">Control to remove events</param>
    public static void RemoveAllEvents( Control control )
      => RemoveAllEvents( control, false );

    /// <summary>
    /// Remove all event handlers from a control.
    /// </summary>
    /// <param name="control">Control to remove event handlers</param>
    /// <param name="argumentOnly">True if you want to remove event handlers only from the specified control and remain child contols' events.
    /// Otherwise, child controls' event handlers are also removed.</param>
    public static void RemoveAllEvents( Control control, bool argumentOnly )
    {
      List<Control> list = new List<Control>();
      list.Add( control );
      if( !argumentOnly )
      {
        list.AddRange( GetAllChildControls( control ) );
      }

      // To improve performance, once created EventRelatedFieldInfoContainer instance is stored for reuse.
      List<EventRelatedFieldInfoContainer> containerList = new List<EventRelatedFieldInfoContainer>();
      foreach( Control c in list )
      {
        EventRelatedFieldInfoContainer container = containerList.Find( x => x.ControlType.Equals( c.GetType() ) );
        if( container == null )
        {
          container = new EventRelatedFieldInfoContainer( c.GetType() );
          containerList.Add( container );
        }

        foreach( EventRelatedFieldInfo info in container.GetEventRelatedFieldInfos() )
        {
          info.RemoveDelegate( c );
        }
      }
    }

    private static IEnumerable<Control> GetAllChildControls( Control control )
    {
      foreach( Control child in control.Controls )
      {
        yield return child;

        foreach( Control grandChild in GetAllChildControls( child ) )
        {
          yield return grandChild;
        }
      }
    }

    /// <summary>
    /// Collect all Windows form control's event information and dump it.
    /// </summary>
    /// <returns>Iteration of dumped string</returns>
    public static IEnumerable<string> DumpControlEvents()
    {
      foreach( Type controlType in typeof( System.Windows.Forms.Control ).Assembly.GetTypes() )
      {
        if( controlType.IsAbstract )
        {
          continue;
        }

        if( !controlType.Equals( typeof( System.Windows.Forms.Control ) )
          && !controlType.IsSubclassOf( typeof( System.Windows.Forms.Control ) ) )
        {
          continue;
        }

        if( controlType.Equals( typeof( System.Windows.Forms.WebBrowser ) )
          && System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA )
        {
          continue;
        }

        ConstructorInfo ci = controlType.GetConstructor( Type.EmptyTypes );
        if( ( ci == null )
          || ( !ci.IsPublic ) )
        {
          continue;
        }

        EventRelatedFieldInfoContainer container = new EventRelatedFieldInfoContainer( controlType );

        yield return $"Control Type : {container.ControlType.Name}{Environment.NewLine}";

        foreach( EventRelatedFieldInfo info in container.GetEventRelatedFieldInfos() )
        {
          yield return info.GetInformationString();
        }

        yield return Environment.NewLine;
      }
    }
  }
}
