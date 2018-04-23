Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid.Drawing
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Registrator
Imports DevExpress.XtraGrid.Views.Grid.Handler
Imports DevExpress.XtraGrid.Views.Base.Handler
Imports DevExpress.XtraGrid.Dragging
Imports System.ComponentModel
Imports DevExpress.Utils.Serializing
Imports DevExpress.Utils.Controls
Imports System.Windows.Forms
Imports System.Drawing
Imports DevExpress.LookAndFeel.Helpers
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Base.ViewInfo
Imports System.IO
Imports DevExpress.Utils
Imports System.Drawing.Imaging
Imports DevExpress.Skins
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Views.Layout.Modes
Imports DevExpress.XtraEditors.Controls

Namespace Sample
	Public Class CustomGridView
		Inherits GridView

		Private hideEdit As QuickHideEdit
		Public Sub New()
			MyBase.New()
		End Sub
		Protected Friend Overridable Sub SetGridControlAccessMetod(ByVal newControl As GridControl)
			SetGridControl(newControl)
		End Sub
		Protected Overrides ReadOnly Property ViewName() As String
			Get
				Return "CustomGridView"
			End Get
		End Property
		Protected Overrides Function CreateColumnCollection() As GridColumnCollection
			Return New CustomGridColumnCollection(Me)
		End Function
		Protected Overrides Function CreateOptionsCustomization() As GridOptionsCustomization
			Return New CustomGridOptionsCustomization()
		End Function
		<Description("Provides access to the View's customization options."), Category("Options"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.DefaultValue)> _
		Public Shadows ReadOnly Property OptionsCustomization() As CustomGridOptionsCustomization
			Get
				Return TryCast(MyBase.OptionsCustomization, CustomGridOptionsCustomization)
			End Get
		End Property
		Protected Friend Overridable Function GetLookAndFeel() As EmbeddedLookAndFeel
			Return ElementsLookAndFeel
		End Function
		Public Overrides ReadOnly Property IsFocusedView() As Boolean
			Get
				If hideEdit IsNot Nothing Then
					If hideEdit.Enabled = True Then
						Return True
					End If
				End If
				Return MyBase.IsFocusedView

			End Get
		End Property
		Protected Friend Overridable Sub ShowColumnCustomizationMenu(ByVal p As Point)
			If hideEdit Is Nothing Then
				CreateHideEdit()
			End If
			LocateHideEdit()
			PopulateHideEdit()
			hideEdit.ShowPopup()
		End Sub
		Private Sub LocateHideEdit()
			hideEdit.MakeEnable()
			hideEdit.Location = (CType(ViewInfo, CustomGridViewInfo)).QuickCustomisationBounds.Location
		End Sub
		Private Sub CreateHideEdit()
			hideEdit = New QuickHideEdit(Me)
			LocateHideEdit()
			GridControl.Controls.Add(hideEdit)
			Me.Focus()
		End Sub
		Protected Friend Overridable Sub AcceptQuickHide()
			For Each col As GridColumn In Columns
				Dim cp As ColumnProperties = hideEdit.Properties.Columns(col.ToString())
				If cp Is Nothing Then
					Continue For
				End If
				col.VisibleIndex = cp.VisibleIndex
				col.Visible = cp.Visible
			Next col
		End Sub
		Protected Overridable Sub PopulateHideEdit()
			hideEdit.Properties.Columns.Clear()
			If Columns.Count = 0 Then
				Return
			End If
			For Each col As CustomGridColumn In Columns
				If col.Visible OrElse col.OptionsColumn.ShowInCustomizationForm Then
					hideEdit.Properties.Columns.Add(col.ToString(), col.Visible, col.VisibleIndex, GetColumnHideState(col), GetColumnMoveState(col))
				End If
			Next col
			hideEdit.Properties.Columns.Sort()
		End Sub
		Protected Overridable Function GetColumnHideState(ByVal column As CustomGridColumn) As Boolean
			Return column.OptionsColumn.AllowQuickHide AndAlso OptionsCustomization.AllowQuickHideColumns
		End Function
		Protected Overridable Function GetColumnMoveState(ByVal column As CustomGridColumn) As Boolean
			Return column.OptionsColumn.AllowMove AndAlso OptionsCustomization.AllowColumnMoving
		End Function
	End Class
	Public Class CustomGridControl
		Inherits GridControl
		Public Sub New()
			MyBase.New()
		End Sub
		Protected Overrides Sub RegisterAvailableViewsCore(ByVal collection As InfoCollection)
			MyBase.RegisterAvailableViewsCore(collection)
			collection.Add(New CustomGridInfoRegistrator())
		End Sub
		Protected Overrides Function CreateDefaultView() As BaseView
			Return CreateView("CustomGridView")
		End Function
	End Class
	Public Class CustomGridInfoRegistrator
		Inherits GridInfoRegistrator
		Public Sub New()
			MyBase.New()
		End Sub
		Public Overrides ReadOnly Property ViewName() As String
			Get
				Return "CustomGridView"
			End Get
		End Property
		Public Overrides Function CreateHandler(ByVal view As BaseView) As BaseViewHandler
			Return New CustomGridHandler(TryCast(view, GridView))
		End Function
		Public Overrides Function CreatePainter(ByVal view As BaseView) As BaseViewPainter
			Return New CustomGridPainter(TryCast(view, GridView))
		End Function
		Public Overrides Function CreateViewInfo(ByVal view As BaseView) As BaseViewInfo
			Return New CustomGridViewInfo(TryCast(view, GridView))
		End Function
		Public Overrides Function CreateView(ByVal grid As GridControl) As BaseView
			Dim view As New CustomGridView()
			view.SetGridControlAccessMetod(grid)
			Return view
		End Function
	End Class
	Public Class CustomGridViewInfo
		Inherits GridViewInfo
		Public QuickCustomisationIconStatus As QuickCustomisationIconStatus
		Private Shared QuickCustomisationWidth As Integer = 10, QuickCustomisationHeight As Integer = 11, QuickCustomisationSpacing As Integer = 2
		Private quickCustumisationBounds As Rectangle
		Public Sub New(ByVal gridView As GridView)
			MyBase.New(gridView)
			quickCustumisationBounds = Rectangle.Empty
			QuickCustomisationIconStatus = QuickCustomisationIconStatus.Hidden
		End Sub
		Public Overridable ReadOnly Property QuickCustomisationBounds() As Rectangle
			Get
				Dim rec As New Rectangle()
				rec.Location = New Point(ColumnsInfo(0).Bounds.Right - QuickCustomisationWidth - QuickCustomisationSpacing, ColumnsInfo(0).Bounds.Top + QuickCustomisationSpacing)
				rec.Size = New Size(QuickCustomisationWidth, QuickCustomisationHeight)
				Return rec
			End Get
		End Property
		Public Overridable Function IsQuickCustomisationButton(ByVal p As Point) As Boolean
			Return QuickCustomisationBounds.Contains(p)
		End Function
		Public ReadOnly Property AllowQuickCustomisation() As Boolean
			Get
				Return (CType(View, CustomGridView)).OptionsCustomization.AllowQuickCustomisation
			End Get
		End Property

		Public Overridable ReadOnly Property QuickCustomisationIcon() As QuickCustomizationIcon
			Get
				Return (CType(View, CustomGridView)).OptionsCustomization.QuickCustomizationIcons
			End Get
		End Property
	End Class
	Public Class CustomGridPainter
		Inherits GridPainter
		Public Sub New(ByVal view As GridView)
			MyBase.New(view)
		End Sub
		Protected Overrides Sub DrawIndicatorCore(ByVal e As GridViewDrawArgs, ByVal info As IndicatorObjectInfoArgs, ByVal rowHandle As Integer, ByVal kind As IndicatorKind)
			MyBase.DrawIndicatorCore(e, info, rowHandle, kind)
			DrawQuickCustomisationIcon(e, info, kind)
		End Sub
		Protected Overridable Sub DrawQuickCustomisationIcon(ByVal e As GridViewDrawArgs, ByVal info As IndicatorObjectInfoArgs, ByVal kind As IndicatorKind)
			If kind = DevExpress.Utils.Drawing.IndicatorKind.Header AndAlso (CType(e.ViewInfo, CustomGridViewInfo)).QuickCustomisationIconStatus <> QuickCustomisationIconStatus.Hidden Then
				DrawQuickCustomisationIconCore(e, info, (CType(e.ViewInfo, CustomGridViewInfo)).QuickCustomisationIcon, (CType(e.ViewInfo, CustomGridViewInfo)).QuickCustomisationBounds, (CType(e.ViewInfo, CustomGridViewInfo)).QuickCustomisationIconStatus)
			End If
		End Sub
		Protected Overridable Sub DrawQuickCustomisationIconCore(ByVal e As GridViewDrawArgs, ByVal info As IndicatorObjectInfoArgs, ByVal icon As QuickCustomizationIcon, ByVal bounds As Rectangle, ByVal status As QuickCustomisationIconStatus)
			Dim patchedRec As New Rectangle(bounds.X + 1, bounds.Y, bounds.Width - 1, bounds.Height)
			Dim args As New GridColumnInfoArgs(e.Cache, e.ViewInfo.ColumnsInfo(0).Column)
			args.Cache = e.Cache
			args.Bounds = patchedRec
			CType(args, HeaderObjectInfoArgs).HeaderPosition = HeaderPositionKind.Center
			If status = QuickCustomisationIconStatus.Hot Then
				CType(args, HeaderObjectInfoArgs).State = ObjectState.Hot
			End If
			ElementsPainter.Column.DrawObject(args)

			If icon.Image IsNot Nothing Then
				Dim rec As New Rectangle()
				rec.Location = New Point(bounds.Left + 1, bounds.Top + 1)
				rec.Size = New Size(bounds.Width - 2, bounds.Height - 2)
				Dim attr As New ImageAttributes()
				attr.SetColorKey(icon.TransperentColor, icon.TransperentColor)
				e.Graphics.DrawImage(icon.Image, rec, 0, 0, icon.Image.Width, icon.Image.Height, GraphicsUnit.Pixel, attr)
			End If
		End Sub
	End Class
	Public Class CustomGridHandler
		Inherits GridHandler
		Public Sub New(ByVal gridView As GridView)
			MyBase.New(gridView)
		End Sub
		Protected Overrides Function CreateDragManager() As GridDragManager
			Return New CustomGridDragManager(View)
		End Function
		Public Overrides Sub DoClickAction(ByVal hitInfo As DevExpress.XtraGrid.Views.Base.ViewInfo.BaseHitInfo)
			MyBase.DoClickAction(hitInfo)
			Dim hit As GridHitInfo = TryCast(hitInfo, GridHitInfo)
			If hit.HitTest = GridHitTest.ColumnButton AndAlso (CType(View, CustomGridView)).OptionsCustomization.AllowQuickCustomisation Then
				If (CType(ViewInfo, CustomGridViewInfo)).IsQuickCustomisationButton(hitInfo.HitPoint) Then
					CType(View, CustomGridView).ShowColumnCustomizationMenu(hit.HitPoint)
				End If
			End If

		End Sub
		Protected Overrides Function OnMouseMove(ByVal ev As MouseEventArgs) As Boolean
			Dim e As DXMouseEventArgs = DXMouseEventArgs.GetMouseArgs(ev)
			Dim p As New Point(e.X, e.Y)
			UpdateQuickCustumisationIconState(p)
			Return MyBase.OnMouseMove(ev)
		End Function
		Protected Overridable Sub UpdateQuickCustumisationIconState(ByVal p As Point)
			Dim vi As CustomGridViewInfo = TryCast(ViewInfo, CustomGridViewInfo)
			If (Not vi.AllowQuickCustomisation) Then
				Return
			End If
			Dim hi As GridHitInfo = ViewInfo.CalcHitInfo(p)
			If hi.HitTest = GridHitTest.ColumnButton Then
				If vi.IsQuickCustomisationButton(p) Then
					If vi.QuickCustomisationIconStatus <> QuickCustomisationIconStatus.Hot Then
						vi.QuickCustomisationIconStatus = QuickCustomisationIconStatus.Hot
						ViewInfo.View.Invalidate()
					End If
					Return
				End If
				If vi.QuickCustomisationIconStatus <> QuickCustomisationIconStatus.Enabled Then
					vi.QuickCustomisationIconStatus = QuickCustomisationIconStatus.Enabled
					ViewInfo.View.Invalidate()
				End If
			Else
				If vi.QuickCustomisationIconStatus <> QuickCustomisationIconStatus.Hidden Then
					vi.QuickCustomisationIconStatus = QuickCustomisationIconStatus.Hidden
					ViewInfo.View.Invalidate()
				End If
			End If
		End Sub
	End Class
	Public Class CustomGridDragManager
		Inherits GridDragManager
		Public Sub New(ByVal view As GridView)
			MyBase.New(view)
		End Sub
		Protected Overrides Function CalcColumnDrag(ByVal hit As GridHitInfo, ByVal column As GridColumn) As PositionInfo
			Dim patchedPI As New PositionInfo()
			patchedPI = MyBase.CalcColumnDrag(hit, column)
			If patchedPI.Index = HideElementPosition AndAlso patchedPI.Valid Then
				Dim col As CustomGridColumn = TryCast(column, CustomGridColumn)
				If col IsNot Nothing Then
					If (Not col.OptionsColumn.AllowQuickHide) Then
						patchedPI = New PositionInfo()
						patchedPI.Valid = False
					End If
				End If
			End If
			Return patchedPI
		End Function
	End Class
	Public Class CustomGridColumn
		Inherits GridColumn
		Public Sub New()
			MyBase.New()
		End Sub
		Protected Overrides Function CreateOptionsColumn() As OptionsColumn
			Return New CustomOptionsColum()
		End Function
		<Description("Provides access to the column's options."), Category("Options"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.DefaultValue)> _
		Public Shadows ReadOnly Property OptionsColumn() As CustomOptionsColum
			Get
				Return CType(MyBase.OptionsColumn, CustomOptionsColum)
			End Get
		End Property
	End Class
	Public Class CustomGridColumnCollection
		Inherits GridColumnCollection
		Public Sub New(ByVal view As ColumnView)
			MyBase.New(view)
		End Sub
		Protected Overrides Function CreateColumn() As GridColumn
			Return New CustomGridColumn()
		End Function
	End Class
	Public Class CustomOptionsColum
		Inherits OptionsColumn
		Private allowQuickHide_Renamed As Boolean
		Public Sub New()
			MyBase.New()
			allowQuickHide_Renamed = True
		End Sub
		<Description("Gets or sets whether the column allow quick hide."), DefaultValue(True), XtraSerializableProperty()> _
		Public Property AllowQuickHide() As Boolean
			Set(ByVal value As Boolean)
				If allowQuickHide_Renamed = value Then
					Return
				End If
				allowQuickHide_Renamed = value
			End Set
			Get
				Return allowQuickHide_Renamed
			End Get
		End Property
	End Class
	Public Class CustomGridOptionsCustomization
		Inherits GridOptionsCustomization
		Private allowQuickCustomisation_Renamed As Boolean
		Private quickCustomizationIcon As QuickCustomizationIcon
		Public Sub New()
			MyBase.New()
			Me.allowQuickCustomisation_Renamed = True
			quickCustomizationIcon = New QuickCustomizationIcon()
		End Sub
		<Description("Gets or sets a value which specifies whether end-users can use quick customisation drop dawn."), DefaultValue(True), XtraSerializableProperty()> _
		Public Overridable Property AllowQuickCustomisation() As Boolean
			Get
				Return allowQuickCustomisation_Renamed
			End Get
			Set(ByVal value As Boolean)
				If allowQuickCustomisation_Renamed = value Then
					Return
				End If
				Dim prevValue As Boolean = allowQuickCustomisation_Renamed
				allowQuickCustomisation_Renamed = value
				OnChanged(New BaseOptionChangedEventArgs("AllowQuickCustomisation", prevValue, allowQuickCustomisation_Renamed))
			End Set
		End Property
		<Description("Allow chose different icon for QuickCustomizationButton."), Category("Options"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), XtraSerializableProperty()> _
		Public Overridable ReadOnly Property QuickCustomizationIcons() As QuickCustomizationIcon
			Get
				Return quickCustomizationIcon
			End Get
		End Property
	End Class
	Public Class QuickCustomizationIcon
		Inherits ViewBaseOptions
		Private image_Renamed As Image
		Private transperentColor_Renamed As Color
		Public Sub New()
			transperentColor_Renamed = Color.Empty
		End Sub
		<Description("Allow to chose image to show on QuickCustomisationButton"), XtraSerializableProperty()> _
		Public Property Image() As Image
			Set(ByVal value As Image)
				If image_Renamed IsNot value Then
					image_Renamed = value
				End If
			End Set
			Get
				Return image_Renamed
			End Get
		End Property
		<Description("Allow to chose transperent color for QuickCustumisationImage"), XtraSerializableProperty()> _
		Public Property TransperentColor() As Color
			Get
				Return transperentColor_Renamed
			End Get
			Set(ByVal value As Color)
				If transperentColor_Renamed <> value Then
					transperentColor_Renamed = value
				End If
			End Set
		End Property
	End Class
	Public Enum QuickCustomisationIconStatus
		Hidden
		Enabled
		Hot
	End Enum
End Namespace

