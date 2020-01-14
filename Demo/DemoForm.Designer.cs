namespace Demo
{
	partial class DemoForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.FormSplitContainer = new System.Windows.Forms.SplitContainer();
			this.ControlPanelSplitContainer = new System.Windows.Forms.SplitContainer();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.MainPanelLabel = new System.Windows.Forms.Label();
			this.SubPanel = new System.Windows.Forms.Panel();
			this.SubPanelLabel = new System.Windows.Forms.Label();
			this.OpPanel = new System.Windows.Forms.Panel();
			this.InitializeMainPanelButton = new System.Windows.Forms.Button();
			this.RemoveEventsButton = new System.Windows.Forms.Button();
			this.ClearSubPanelButton = new System.Windows.Forms.Button();
			this.DuplicateButton = new System.Windows.Forms.Button();
			this.ReplaceGroupBoxButton = new System.Windows.Forms.Button();
			this.MessageTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.FormSplitContainer)).BeginInit();
			this.FormSplitContainer.Panel1.SuspendLayout();
			this.FormSplitContainer.Panel2.SuspendLayout();
			this.FormSplitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ControlPanelSplitContainer)).BeginInit();
			this.ControlPanelSplitContainer.Panel1.SuspendLayout();
			this.ControlPanelSplitContainer.Panel2.SuspendLayout();
			this.ControlPanelSplitContainer.SuspendLayout();
			this.OpPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// FormSplitContainer
			// 
			this.FormSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FormSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.FormSplitContainer.Name = "FormSplitContainer";
			this.FormSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// FormSplitContainer.Panel1
			// 
			this.FormSplitContainer.Panel1.Controls.Add(this.ControlPanelSplitContainer);
			this.FormSplitContainer.Panel1.Controls.Add(this.OpPanel);
			// 
			// FormSplitContainer.Panel2
			// 
			this.FormSplitContainer.Panel2.Controls.Add(this.MessageTextBox);
			this.FormSplitContainer.Size = new System.Drawing.Size(884, 611);
			this.FormSplitContainer.SplitterDistance = 400;
			this.FormSplitContainer.TabIndex = 0;
			// 
			// ControlPanelSplitContainer
			// 
			this.ControlPanelSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ControlPanelSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.ControlPanelSplitContainer.Name = "ControlPanelSplitContainer";
			// 
			// ControlPanelSplitContainer.Panel1
			// 
			this.ControlPanelSplitContainer.Panel1.Controls.Add(this.MainPanel);
			this.ControlPanelSplitContainer.Panel1.Controls.Add(this.MainPanelLabel);
			this.ControlPanelSplitContainer.Panel1.SizeChanged += new System.EventHandler(this.ControlPanelSplitContainer_Panel1_SizeChanged);
			// 
			// ControlPanelSplitContainer.Panel2
			// 
			this.ControlPanelSplitContainer.Panel2.Controls.Add(this.SubPanel);
			this.ControlPanelSplitContainer.Panel2.Controls.Add(this.SubPanelLabel);
			this.ControlPanelSplitContainer.Panel2.SizeChanged += new System.EventHandler(this.ControlPanelSplitContainer_Panel2_SizeChanged);
			this.ControlPanelSplitContainer.Size = new System.Drawing.Size(884, 372);
			this.ControlPanelSplitContainer.SplitterDistance = 300;
			this.ControlPanelSplitContainer.TabIndex = 3;
			// 
			// MainPanel
			// 
			this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.MainPanel.Location = new System.Drawing.Point(0, 21);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(300, 351);
			this.MainPanel.TabIndex = 1;
			// 
			// MainPanelLabel
			// 
			this.MainPanelLabel.AutoSize = true;
			this.MainPanelLabel.Location = new System.Drawing.Point(12, 9);
			this.MainPanelLabel.Name = "MainPanelLabel";
			this.MainPanelLabel.Size = new System.Drawing.Size(61, 12);
			this.MainPanelLabel.TabIndex = 0;
			this.MainPanelLabel.Text = "Main Panel";
			// 
			// SubPanel
			// 
			this.SubPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SubPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.SubPanel.Location = new System.Drawing.Point(0, 21);
			this.SubPanel.Name = "SubPanel";
			this.SubPanel.Size = new System.Drawing.Size(580, 351);
			this.SubPanel.TabIndex = 1;
			// 
			// SubPanelLabel
			// 
			this.SubPanelLabel.AutoSize = true;
			this.SubPanelLabel.Location = new System.Drawing.Point(12, 9);
			this.SubPanelLabel.Name = "SubPanelLabel";
			this.SubPanelLabel.Size = new System.Drawing.Size(56, 12);
			this.SubPanelLabel.TabIndex = 0;
			this.SubPanelLabel.Text = "Sub Panel";
			// 
			// OpPanel
			// 
			this.OpPanel.Controls.Add(this.InitializeMainPanelButton);
			this.OpPanel.Controls.Add(this.RemoveEventsButton);
			this.OpPanel.Controls.Add(this.ClearSubPanelButton);
			this.OpPanel.Controls.Add(this.DuplicateButton);
			this.OpPanel.Controls.Add(this.ReplaceGroupBoxButton);
			this.OpPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.OpPanel.Location = new System.Drawing.Point(0, 372);
			this.OpPanel.Name = "OpPanel";
			this.OpPanel.Size = new System.Drawing.Size(884, 28);
			this.OpPanel.TabIndex = 2;
			// 
			// InitializeMainPanelButton
			// 
			this.InitializeMainPanelButton.Location = new System.Drawing.Point(324, 3);
			this.InitializeMainPanelButton.Name = "InitializeMainPanelButton";
			this.InitializeMainPanelButton.Size = new System.Drawing.Size(150, 23);
			this.InitializeMainPanelButton.TabIndex = 2;
			this.InitializeMainPanelButton.Text = "Initialize Main Panel";
			this.InitializeMainPanelButton.UseVisualStyleBackColor = true;
			this.InitializeMainPanelButton.Click += new System.EventHandler(this.InitializeMainPanelButton_Click);
			// 
			// RemoveEventsButton
			// 
			this.RemoveEventsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RemoveEventsButton.Location = new System.Drawing.Point(566, 3);
			this.RemoveEventsButton.Name = "RemoveEventsButton";
			this.RemoveEventsButton.Size = new System.Drawing.Size(150, 23);
			this.RemoveEventsButton.TabIndex = 3;
			this.RemoveEventsButton.Text = "Remove Events";
			this.RemoveEventsButton.UseVisualStyleBackColor = true;
			this.RemoveEventsButton.Click += new System.EventHandler(this.RemoveEventsButton_Click);
			// 
			// ClearSubPanelButton
			// 
			this.ClearSubPanelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ClearSubPanelButton.Location = new System.Drawing.Point(722, 3);
			this.ClearSubPanelButton.Name = "ClearSubPanelButton";
			this.ClearSubPanelButton.Size = new System.Drawing.Size(150, 23);
			this.ClearSubPanelButton.TabIndex = 4;
			this.ClearSubPanelButton.Text = "Clear Sub Panel";
			this.ClearSubPanelButton.UseVisualStyleBackColor = true;
			this.ClearSubPanelButton.Click += new System.EventHandler(this.ClearSubPanelButton_Click);
			// 
			// DuplicateButton
			// 
			this.DuplicateButton.Location = new System.Drawing.Point(168, 3);
			this.DuplicateButton.Name = "DuplicateButton";
			this.DuplicateButton.Size = new System.Drawing.Size(150, 23);
			this.DuplicateButton.TabIndex = 1;
			this.DuplicateButton.Text = "Duplicate";
			this.DuplicateButton.UseVisualStyleBackColor = true;
			this.DuplicateButton.Click += new System.EventHandler(this.DuplicateButton_Click);
			// 
			// ReplaceGroupBoxButton
			// 
			this.ReplaceGroupBoxButton.Location = new System.Drawing.Point(12, 3);
			this.ReplaceGroupBoxButton.Name = "ReplaceGroupBoxButton";
			this.ReplaceGroupBoxButton.Size = new System.Drawing.Size(150, 23);
			this.ReplaceGroupBoxButton.TabIndex = 0;
			this.ReplaceGroupBoxButton.Text = "Replace GroupBox";
			this.ReplaceGroupBoxButton.UseVisualStyleBackColor = true;
			this.ReplaceGroupBoxButton.Click += new System.EventHandler(this.ReplaceGroupBoxButton_Click);
			// 
			// MessageTextBox
			// 
			this.MessageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MessageTextBox.Location = new System.Drawing.Point(0, 0);
			this.MessageTextBox.Multiline = true;
			this.MessageTextBox.Name = "MessageTextBox";
			this.MessageTextBox.ReadOnly = true;
			this.MessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.MessageTextBox.Size = new System.Drawing.Size(884, 207);
			this.MessageTextBox.TabIndex = 0;
			// 
			// DemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 611);
			this.Controls.Add(this.FormSplitContainer);
			this.Name = "DemoForm";
			this.Text = "Demo Form";
			this.Load += new System.EventHandler(this.DemoForm_Load);
			this.FormSplitContainer.Panel1.ResumeLayout(false);
			this.FormSplitContainer.Panel2.ResumeLayout(false);
			this.FormSplitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.FormSplitContainer)).EndInit();
			this.FormSplitContainer.ResumeLayout(false);
			this.ControlPanelSplitContainer.Panel1.ResumeLayout(false);
			this.ControlPanelSplitContainer.Panel1.PerformLayout();
			this.ControlPanelSplitContainer.Panel2.ResumeLayout(false);
			this.ControlPanelSplitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ControlPanelSplitContainer)).EndInit();
			this.ControlPanelSplitContainer.ResumeLayout(false);
			this.OpPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer FormSplitContainer;
		private System.Windows.Forms.Panel OpPanel;
		private System.Windows.Forms.Button RemoveEventsButton;
		private System.Windows.Forms.Button ClearSubPanelButton;
		private System.Windows.Forms.Button DuplicateButton;
		private System.Windows.Forms.Button ReplaceGroupBoxButton;
		private System.Windows.Forms.TextBox MessageTextBox;
		private System.Windows.Forms.Label SubPanelLabel;
		private System.Windows.Forms.Label MainPanelLabel;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.Panel SubPanel;
		private System.Windows.Forms.Button InitializeMainPanelButton;
		private System.Windows.Forms.SplitContainer ControlPanelSplitContainer;
	}
}

