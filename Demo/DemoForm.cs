using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ControlUtil;

namespace Demo
{
	public partial class DemoForm : Form
	{
		public DemoForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Example to replace a Windows form control to another one.
		/// </summary>
		private void ReplaceMainGroupBox()
		{
			GroupBox mainGroupBox = ( GroupBox )this.MainPanel.Controls[ 0 ];
			ControlInitializer.ReplaceControl( mainGroupBox, new HecticGroupBox() );
		}

		/// <summary>
		/// Example to duplicate a Windows form control.
		/// </summary>
		private void DuplicateGroupBox()
		{
			GroupBox mainGroupBox = ( GroupBox )this.MainPanel.Controls[ 0 ];
			GroupBox duplicatedGroupBox = ( GroupBox )ControlInitializer.DuplicateControl( mainGroupBox );
			this.SubPanel.Controls.Add( duplicatedGroupBox );
		}

		/// <summary>
		/// Example to remove all event handler delegates from a Windows form control.
		/// </summary>
		private void RemoveSubGroupBoxEvents()
		{
			GroupBox subGroupBox = ( GroupBox )this.SubPanel.Controls[ 0 ];
			ControlInitializer.RemoveAllEvents( subGroupBox );
		}


		private ToolTip toolTip;

		private DemoFormState formState;

		private void InitializeToolTip()
		{
			this.toolTip = new ToolTip();

			this.toolTip.SetToolTip( this.ReplaceGroupBoxButton, "Replace GroupBox control on Main Panel to a derived one." );
			this.toolTip.SetToolTip( this.DuplicateButton, "Duplicate all controls on Main Panel to Sub Panel." );
			this.toolTip.SetToolTip( this.InitializeMainPanelButton, "Initialize all controls on Main Panel." );
			this.toolTip.SetToolTip( this.RemoveEventsButton, "Remove all event handler delegates from controls on Sub Panel." );
			this.toolTip.SetToolTip( this.ClearSubPanelButton, "Remove all controls on Sub Panel." );
		}

		private void AddTabPage( TabControl tabControl, string tabText, params Control[] controls )
		{
			TabPage tabPage = new TabPage( tabText );
			tabControl.TabPages.Add( tabPage );

			List<Control> list = new List<Control>( controls );
			float span = ( float )( tabPage.Height - list.Sum( x => x.Height ) ) / ( list.Count );

			float y = 10f;
			foreach( Control c in list )
			{
				tabPage.Controls.Add( c );
				c.Left = 10;
				c.Top = ( int )y;
				y += ( c.Height + span );
			}
		}

		private void InitializeMainPanel()
		{
			if( this.MainPanel.HasChildren )
			{
				GroupBox existGroupBox = ( GroupBox )this.MainPanel.Controls[ 0 ];
				ControlInitializer.RemoveAllEvents( existGroupBox );
				existGroupBox.Parent = null;
				existGroupBox.Dispose();
			}

			GroupBox mainGroupBox = new GroupBox();
			mainGroupBox.Text = "GroupBox";
			mainGroupBox.Dock = DockStyle.Fill;
			mainGroupBox.SizeChanged += ( s, e ) => { this.AppendMessage( s, $"GroupBox size changed." ); };
			this.MainPanel.Controls.Add( mainGroupBox );

			TabControl tabControl = new TabControl();
			tabControl.Dock = DockStyle.Fill;
			tabControl.SelectedIndexChanged += ( s, e ) => { this.AppendMessage( s, "Selected tab page changed." ); };
			mainGroupBox.Controls.Add( tabControl );

			Button button = new Button();
			button.Text = "Button";
			button.Click += ( s, e ) => { this.AppendMessage( s, "Button clicked." ); };

			TextBox textBox = new TextBox();
			textBox.TextChanged += ( s, e ) => { this.AppendMessage( s, "Text changed." ); };

			CheckBox checkBox = new CheckBox();
			checkBox.Text = "CheckBox";
			checkBox.CheckStateChanged += ( s, e ) => { this.AppendMessage( s, $"CheckBox state changed. Current : { ( ( CheckBox )s ).CheckState}" ); };

			RadioButton radioButton1 = new RadioButton();
			radioButton1.Text = "RadioButton 1";
			radioButton1.Checked = true;
			radioButton1.CheckedChanged += ( s, e ) => { this.AppendMessage( s, $"RadioButton 1 state changed. Current : { ( ( RadioButton )s ).Checked}" ); };

			RadioButton radioButton2 = new RadioButton();
			radioButton2.Text = "RadioButton 2";
			radioButton2.Checked = false;
			radioButton2.CheckedChanged += ( s, e ) => { this.AppendMessage( s, $"RadioButton 2 state changed. Current : {( ( RadioButton )s ).Checked}" ); };

			this.AddTabPage( tabControl, "Controls 1", button, textBox );
			this.AddTabPage( tabControl, "Controls 2", checkBox, radioButton1, radioButton2 );
		}

		private void ClearSubPanel()
		{
			GroupBox subGroupBox = ( GroupBox )this.SubPanel.Controls[ 0 ];
			ControlInitializer.RemoveAllEvents( subGroupBox );
			subGroupBox.Parent = null;
			subGroupBox.Dispose();
		}

		private void AppendMessage( object sender,  string message )
		{
			Control c = ( Control )sender;
			while( !c.GetType().Equals( typeof( Panel ) ) )
			{
				c = c.Parent;
			}

			this.AppendMessage( $"[Event from {c.Name}] {message}" );
		}

		private void AppendMessage( string message )
		{
			this.MessageTextBox.AppendText( $"{message}{Environment.NewLine}" );
		}

		private void UpdateFormState( DemoFormState state )
		{
			this.formState = state;
			this.UpdateFormState();
		}

		private void UpdateFormState()
		{
			this.ReplaceGroupBoxButton.Enabled = !( this.MainPanel.Controls[ 0 ] is HecticGroupBox );
			this.InitializeMainPanelButton.Enabled = !this.ReplaceGroupBoxButton.Enabled;

			switch( this.formState )
			{
				case DemoFormState.Initial:
					{
						this.DuplicateButton.Enabled = true;
						this.RemoveEventsButton.Enabled = false;
						this.ClearSubPanelButton.Enabled = false;
						break;
					}
				case DemoFormState.Duplicated:
					{
						this.DuplicateButton.Enabled = false;
						this.RemoveEventsButton.Enabled = true;
						this.ClearSubPanelButton.Enabled = true;
						break;
					}
				case DemoFormState.EventsRemoved:
					{
						this.DuplicateButton.Enabled = false;
						this.RemoveEventsButton.Enabled = false;
						this.ClearSubPanelButton.Enabled = true;
						break;
					}
				default:
					{
						throw new ApplicationException( $"Invalid DemoFormState Enum Value. Value = {(int)this.formState}." );
					}
			}
		}


		private void DemoForm_Load( object sender, EventArgs e )
		{
			this.InitializeToolTip();
			this.InitializeMainPanel();
			this.UpdateFormState( DemoFormState.Initial );
		}

		private void ControlPanelSplitContainer_Panel1_SizeChanged( object sender, EventArgs e )
		{
			this.MainPanel.Height = this.ControlPanelSplitContainer.Panel1.Height - this.MainPanelLabel.Bottom - 3;
		}

		private void ControlPanelSplitContainer_Panel2_SizeChanged( object sender, EventArgs e )
		{
			this.SubPanel.Height = this.ControlPanelSplitContainer.Panel2.Height - this.SubPanelLabel.Bottom - 3;
		}

		private void ReplaceGroupBoxButton_Click( object sender, EventArgs e )
		{
			this.ReplaceMainGroupBox();
			this.AppendMessage( "[Operation] GroupBox control on Main Panel is replaced to derived one." );
			this.UpdateFormState();
		}

		private void DuplicateButton_Click( object sender, EventArgs e )
		{
			this.DuplicateGroupBox();
			this.AppendMessage( "[Operation] Controls on Main Panel are Duplicated to Sub Panel." );
			this.UpdateFormState( DemoFormState.Duplicated );
		}

		private void InitializeMainPanelButton_Click( object sender, EventArgs e )
		{
			this.InitializeMainPanel();
			this.AppendMessage( "[Operation] Main Panel is initialized." );
			this.UpdateFormState();
		}

		private void RemoveEventsButton_Click( object sender, EventArgs e )
		{
			this.RemoveSubGroupBoxEvents();
			this.AppendMessage( "[Operation] Event handler delegates added to all Sub Panel controls are removed." );
			this.UpdateFormState( DemoFormState.EventsRemoved );
		}

		private void ClearSubPanelButton_Click( object sender, EventArgs e )
		{
			this.ClearSubPanel();
			this.AppendMessage( "[Operation] Controls on Sub Panel are removed." );
			this.UpdateFormState( DemoFormState.Initial );
		}

		private enum DemoFormState
		{
			Initial,
			Duplicated,
			EventsRemoved,
		}
	}
}
