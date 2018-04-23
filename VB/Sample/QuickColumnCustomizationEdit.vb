Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Registrator
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Collections
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.Controls

Namespace Sample
	Public Class RepositoryItemQuickHideEdit
		Inherits RepositoryItemPopupContainerEdit
		Private columns_Renamed As ColumnPropertiesCollection
		Private Const minFormWidth As Integer = 105, minFormHeight As Integer = 70
		Shared Sub New()
			RegisterQuickHideEdit()
		End Sub
		Public Sub New()
			PopupFormMinSize = New System.Drawing.Size(minFormWidth, minFormHeight)
			columns_Renamed = New ColumnPropertiesCollection()
		End Sub
		Public Property Columns() As ColumnPropertiesCollection
			Get
				Return columns_Renamed
			End Get
			Set(ByVal value As ColumnPropertiesCollection)
				columns_Renamed = value
			End Set
		End Property

		Public Const QuickHideEditName As String = "QuickHideEdit"
		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return QuickHideEditName
			End Get
		End Property
		Public Shared Sub RegisterQuickHideEdit()
			EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(QuickHideEditName, GetType(QuickHideEdit), GetType(RepositoryItemQuickHideEdit), GetType(PopupContainerEditViewInfo), New QuickHideEditPainter(), True))
		End Sub
		Public Overrides Function CreateViewInfo() As BaseEditViewInfo
			Return New QuickHideEditViewInfo(Me)
		End Function
	End Class
	Public Class QuickHideEdit
		Inherits PopupContainerEdit
		Private Const editorWidth As Integer = 10, editorHeight_Renamed As Integer = 11
		Private gridView_Renamed As CustomGridView
		Private integratedPopupControl As PopupContainerControl
		Shared Sub New()
			RepositoryItemQuickHideEdit.RegisterQuickHideEdit()
		End Sub
		Public Sub New()
			MyBase.New()
		End Sub
		Public Sub New(ByVal view As CustomGridView)
			MyBase.New()
			gridView_Renamed = view
			Properties.LookAndFeel.Assign(gridView_Renamed.GetLookAndFeel())
			MakeUnEnable()
			Size = New Size(editorWidth, editorHeight_Renamed)
			BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
			CreatePopupControl()
		End Sub

		Private Sub CreatePopupControl()
			integratedPopupControl = New PopupContainerControl()
			Controls.Add(integratedPopupControl)
			Properties.PopupControl = integratedPopupControl
		End Sub


		Protected Friend Overridable Sub MakeEnable()
			Visible = True
			Enabled = True
		End Sub
		Protected Friend Overridable Sub MakeUnEnable()
			Visible = False
			Enabled = False
		End Sub
		Public ReadOnly Property EditorHeight() As Integer
			Get
				Return editorHeight_Renamed
			End Get
		End Property

		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return RepositoryItemQuickHideEdit.QuickHideEditName
			End Get
		End Property
		Public Shadows ReadOnly Property Properties() As RepositoryItemQuickHideEdit
			Get
				Return TryCast(MyBase.Properties, RepositoryItemQuickHideEdit)
			End Get
		End Property
		Protected Overrides Function CreatePopupForm() As PopupBaseForm
			Dim form As New QuickHidePopupForm(Me)
			Return form
		End Function
		Protected Overrides Sub DoShowPopup()
			MyBase.DoShowPopup()
			CType(PopupForm, QuickHidePopupForm).PopulateListBox()
		End Sub
		Protected Overrides Sub DoClosePopup(ByVal closeMode As PopupCloseMode)
			MyBase.DoClosePopup(closeMode)
			MakeUnEnable()
		End Sub
		Public Overrides Overloads Sub ClosePopup()
			MyBase.ClosePopup()
			Properties.Columns = (CType(PopupForm, QuickHidePopupForm)).GetCollumns(Properties.Columns)
			If gridView_Renamed IsNot Nothing Then
				gridView_Renamed.AcceptQuickHide()
			End If
		End Sub
		Public ReadOnly Property GridView() As CustomGridView
			Get
				Return gridView_Renamed
			End Get
		End Property
	End Class
	Public Class QuickHideEditViewInfo
		Inherits PopupContainerEditViewInfo
		Public Sub New(ByVal item As RepositoryItem)
			MyBase.New(item)
		End Sub
		Protected Overrides Function CalcMinHeightCore(ByVal g As Graphics) As Integer
			Return (CType(OwnerEdit, QuickHideEdit)).EditorHeight
		End Function
		Protected Friend Overridable ReadOnly Property GetIcon() As QuickCustomizationIcon
			Get
				If (CType(OwnerEdit, QuickHideEdit)).GridView IsNot Nothing Then
					Return (CType(OwnerEdit, QuickHideEdit)).GridView.OptionsCustomization.QuickCustomizationIcons
				End If
				Return New QuickCustomizationIcon()
			End Get
		End Property
	End Class
	Public Class QuickHideEditPainter
		Inherits ButtonEditPainter
		Protected Overrides Sub DrawContent(ByVal info As ControlGraphicsInfoArgs)
			info.Graphics.FillRectangle(info.Cache.GetSolidBrush(Color.White), info.ViewInfo.ClientRect)
			Dim icon As QuickCustomizationIcon = (CType(info.ViewInfo, QuickHideEditViewInfo)).GetIcon
			If icon.Image IsNot Nothing Then
				Dim attr As New ImageAttributes()
				attr.SetColorKey(icon.TransperentColor, icon.TransperentColor)
				info.Graphics.DrawImage(icon.Image, info.ViewInfo.ClientRect, 0, 0, icon.Image.Width, icon.Image.Height, GraphicsUnit.Pixel, attr)
			End If
		End Sub
	End Class

	Public Class QuickHidePopupForm
		Inherits PopupContainerForm
		Private listBox As CustomCheckedlistBox
		Public Sub New(ByVal ownerEdit As PopupContainerEdit)
			MyBase.New(ownerEdit)
			CreateChekedListBox()
		End Sub

		Private Sub CreateChekedListBox()
			listBox = New CustomCheckedlistBox()
			OwnerEdit.Properties.PopupControl.Controls.Add(listBox)
		End Sub
		Protected Overrides Sub SetupButtons()
			MyBase.SetupButtons()
			fShowOkButton = True
		End Sub

		Public Overridable Shadows ReadOnly Property Properties() As RepositoryItemQuickHideEdit
			Get
				Return CType(MyBase.Properties, RepositoryItemQuickHideEdit)
			End Get
		End Property
		Public Overridable Sub PopulateListBox()
			listBox.Items.Clear()
			For Each column As ColumnProperties In Properties.Columns
				listBox.Items.Add(column.Caption, column.CheckState, column.AllowQuickHide)
			Next column
			listBox.CreateAllowMovingArray()
			For i As Integer = 0 To listBox.ItemCount - 1
				listBox.SetAllowMoving(i, Properties.Columns(i).AllowMove)
			Next i
		End Sub
		Public Overridable Function GetCollumns(ByVal oldCollection As ColumnPropertiesCollection) As ColumnPropertiesCollection
			Dim newCollection As New ColumnPropertiesCollection()
			For Each item As CheckedListBoxItem In listBox.Items
				newCollection.Add(item.Value.ToString(), item.CheckState = CheckState.Checked, oldCollection(listBox.Items.IndexOf(item)).VisibleIndex, item.Enabled)
			Next item
			Return newCollection

		End Function
		Protected Overrides Sub UpdateControlPositionsCore()
			MyBase.UpdateControlPositionsCore()
			listBox.Bounds = ViewInfo.ContentRect
			listBox.Width = listBox.Width - listBox.Left * 2
		End Sub
	End Class


End Namespace
