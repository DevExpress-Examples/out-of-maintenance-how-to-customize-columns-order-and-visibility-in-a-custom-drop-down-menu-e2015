Namespace Sample
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Me.customGridControl1 = New Sample.CustomGridControl()
			Me.customGridView1 = New Sample.CustomGridView()
			Me.customGridColumn1 = New Sample.CustomGridColumn()
			Me.customGridColumn2 = New Sample.CustomGridColumn()
			Me.customGridColumn3 = New Sample.CustomGridColumn()
			Me.customGridColumn4 = New Sample.CustomGridColumn()
			Me.customGridColumn5 = New Sample.CustomGridColumn()
			Me.customGridColumn6 = New Sample.CustomGridColumn()
			Me.customGridColumn7 = New Sample.CustomGridColumn()
			CType(Me.customGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.customGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' customGridControl1
			' 
			Me.customGridControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.customGridControl1.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(2)
			Me.customGridControl1.Location = New System.Drawing.Point(0, 0)
			Me.customGridControl1.MainView = Me.customGridView1
			Me.customGridControl1.Margin = New System.Windows.Forms.Padding(2)
			Me.customGridControl1.Name = "customGridControl1"
			Me.customGridControl1.Size = New System.Drawing.Size(808, 306)
			Me.customGridControl1.TabIndex = 0
			Me.customGridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.customGridView1})
			' 
			' customGridView1
			' 
			Me.customGridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() { Me.customGridColumn1, Me.customGridColumn2, Me.customGridColumn3, Me.customGridColumn4, Me.customGridColumn5, Me.customGridColumn6, Me.customGridColumn7})
			Me.customGridView1.GridControl = Me.customGridControl1
			Me.customGridView1.Name = "customGridView1"
			Me.customGridView1.OptionsCustomization.QuickCustomizationIcons.Image = (CType(resources.GetObject("customGridView1.OptionsCustomization.QuickCustomizationIcons.Image"), System.Drawing.Image))
			Me.customGridView1.OptionsCustomization.QuickCustomizationIcons.TransperentColor = System.Drawing.Color.White
			' 
			' customGridColumn1
			' 
			Me.customGridColumn1.Caption = "customGridColumn1"
			Me.customGridColumn1.Name = "customGridColumn1"
			Me.customGridColumn1.Visible = True
			Me.customGridColumn1.VisibleIndex = 0
			' 
			' customGridColumn2
			' 
			Me.customGridColumn2.Caption = "customGridColumn2"
			Me.customGridColumn2.Name = "customGridColumn2"
			Me.customGridColumn2.Visible = True
			Me.customGridColumn2.VisibleIndex = 1
			' 
			' customGridColumn3
			' 
			Me.customGridColumn3.Caption = "customGridColumn3"
			Me.customGridColumn3.Name = "customGridColumn3"
			Me.customGridColumn3.Visible = True
			Me.customGridColumn3.VisibleIndex = 2
			' 
			' customGridColumn4
			' 
			Me.customGridColumn4.Caption = "No move"
			Me.customGridColumn4.Name = "customGridColumn4"
			Me.customGridColumn4.OptionsColumn.AllowMove = False
			Me.customGridColumn4.Visible = True
			Me.customGridColumn4.VisibleIndex = 3
			' 
			' customGridColumn5
			' 
			Me.customGridColumn5.Caption = "No hide"
			Me.customGridColumn5.Name = "customGridColumn5"
			Me.customGridColumn5.OptionsColumn.AllowQuickHide = False
			Me.customGridColumn5.Visible = True
			Me.customGridColumn5.VisibleIndex = 4
			' 
			' customGridColumn6
			' 
			Me.customGridColumn6.Caption = "No move No hide"
			Me.customGridColumn6.Name = "customGridColumn6"
			Me.customGridColumn6.OptionsColumn.AllowMove = False
			Me.customGridColumn6.OptionsColumn.AllowQuickHide = False
			Me.customGridColumn6.Visible = True
			Me.customGridColumn6.VisibleIndex = 5
			' 
			' customGridColumn7
			' 
			Me.customGridColumn7.Caption = "Dont show in form"
			Me.customGridColumn7.Name = "customGridColumn7"
			Me.customGridColumn7.OptionsColumn.ShowInCustomizationForm = False
			Me.customGridColumn7.Visible = True
			Me.customGridColumn7.VisibleIndex = 6
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(808, 306)
			Me.Controls.Add(Me.customGridControl1)
			Me.Name = "Form1"
			Me.Text = "Form1"
'			Me.Load += New System.EventHandler(Me.Form1_Load)
			CType(Me.customGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.customGridView1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private customGridControl1 As CustomGridControl
		Private customGridView1 As CustomGridView
		Private customGridColumn1 As CustomGridColumn
		Private customGridColumn2 As CustomGridColumn
		Private customGridColumn3 As CustomGridColumn
		Private customGridColumn4 As CustomGridColumn
		Private customGridColumn5 As CustomGridColumn
		Private customGridColumn6 As CustomGridColumn
		Private customGridColumn7 As CustomGridColumn


	End Class
End Namespace

