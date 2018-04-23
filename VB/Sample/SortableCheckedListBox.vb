Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraEditors
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Drawing

Namespace Sample
	Friend Class CustomCheckedlistBox
		Inherits CheckedListBoxControl

		Private dragSourceIndex, dragTargetIndex As Integer
'INSTANT VB NOTE: The variable isDraging was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private isDraging_Renamed As Boolean
		Private allowMove() As Boolean

		Private dragBeginRect As Rectangle
		Public Sub New()
			MyBase.New()
			SelectionMode = SelectionMode.None
			TabStop = False
			CheckOnClick = True
			isDraging_Renamed = False
		End Sub
		Protected Friend Overridable Sub SetAllowMoving(ByVal index As Integer, ByVal value As Boolean)
			allowMove(index) = value
		End Sub
		Protected Friend Overridable Sub CreateAllowMovingArray()
			allowMove = New Boolean(Items.Count - 1){}
		End Sub
		Protected Overridable Property IsDraging() As Boolean
			Get
				Return isDraging_Renamed
			End Get
			Set(ByVal value As Boolean)
				If isDraging_Renamed <> value Then
					isDraging_Renamed = value
				End If
			End Set
		End Property
		Protected Overrides Function CreateViewInfo() As BaseStyleControlViewInfo
			Return New CustomCheckedListBoxViewInfo(Me)
		End Function
		Protected Overrides Function CreatePainter() As BaseControlPainter
			Return New CustomPainterCheckedListBox()
		End Function
		Protected Shadows Overridable ReadOnly Property ViewInfo() As CustomCheckedListBoxViewInfo
			Get
				Return TryCast(MyBase.ViewInfo, CustomCheckedListBoxViewInfo)
			End Get
		End Property
		Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
			MyBase.OnMouseDown(e)
			If e.Button = MouseButtons.Left Then
				Dim index As Integer = Me.IndexFromPoint(e.Location)
				If index >= 0 AndAlso index < Items.Count AndAlso (Not allowMove(index)) Then
					Return
				End If
				IsDraging = False
				dragSourceIndex = index
				dragTargetIndex = dragSourceIndex
				If dragSourceIndex <> -1 Then
					Dim dragSize As Size = SystemInformation.DragSize
					dragBeginRect = New Rectangle(New Point(e.X - (dragSize.Width \ 2), e.Y - (dragSize.Height \ 2)), dragSize)
				Else
					dragBeginRect = Rectangle.Empty
				End If
			End If
		End Sub
		Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
			If (Not IsDraging) OrElse dragBeginRect = Rectangle.Empty Then
				MyBase.OnMouseUp(e)
			End If
			If dragBeginRect = Rectangle.Empty Then
				Return
			End If
			If dragSourceIndex <> -1 AndAlso dragTargetIndex <> dragSourceIndex Then
				ChangeItemsPositionCore(dragSourceIndex, dragTargetIndex)
			End If

			dragBeginRect = Rectangle.Empty
		End Sub
		Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
			MyBase.OnMouseMove(e)
			If (e.Button And MouseButtons.Left) = MouseButtons.Left Then
				If dragBeginRect <> Rectangle.Empty AndAlso (Not dragBeginRect.Contains(e.X, e.Y)) Then
					IsDraging = True
					dragTargetIndex = Me.IndexFromPoint(e.Location)
					If dragTargetIndex = -1 Then
						If e.Y < Me.ViewInfo.GetItemRectangle(0).Bottom Then
							dragTargetIndex = 0
						End If
					End If
					Dim info As CheckedListBoxViewInfo.CheckedItemInfo = ViewInfo.GetItemByIndex(dragSourceIndex)
					If info IsNot Nothing Then
						info.PaintAppearance.ForeColor = Color.Red
					End If
					ViewInfo.MarkItem(dragTargetIndex, dragSourceIndex)
				End If
			End If
		End Sub
		Protected Overridable Sub ChangeItemsPositionCore(ByVal source As Integer, ByVal target As Integer)
			CorrectAllowMove(source, target)
			If target = -1 Then
				Items.Add(Items(source))
			Else
				Items.Insert(target, Items(source))
				If source > target Then
					source += 1
				End If
			End If
			Items.RemoveAt(source)
		End Sub

		Protected Overridable Sub CorrectAllowMove(ByVal source As Integer, ByVal target As Integer)
			Dim b As Boolean = allowMove(source)
			If target = -1 Then
				target = Items.Count-1
			End If
			If target > source Then
				For i As Integer = source To target - 1
					allowMove(i) = allowMove(i+1)
				Next i
			Else
				For i As Integer = source To target + 1 Step -1
					allowMove(i) = allowMove(i-1)
				Next i
			End If
			allowMove(target) = b
		End Sub
	End Class
	Public Class CustomCheckedListBoxViewInfo
		Inherits CheckedListBoxViewInfo

'INSTANT VB NOTE: The variable dragDropLineColor was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private dragDropLineColor_Renamed As Color
		Public Sub New(ByVal listBox As CheckedListBoxControl)
			MyBase.New(listBox)
		End Sub
		Protected Overrides Function CreateItemInfo(ByVal bounds As Rectangle, ByVal item As Object, ByVal text As String, ByVal index As Integer) As ItemInfo
			Dim info As CheckedItemInfo = TryCast(MyBase.CreateItemInfo(bounds, item, text, index), CheckedItemInfo)
			Dim patchedInfo As New CustomCheckedItemInfo(info)
			Return patchedInfo
		End Function
		Public Overridable Property DragDropLineColor() As Color
			Get
				Return dragDropLineColor_Renamed
			End Get
			Set(ByVal value As Color)
				If dragDropLineColor_Renamed <> value Then
					dragDropLineColor_Renamed = value
				End If
			End Set
		End Property
		Protected Friend Overridable Sub UnderlineItem(ByVal index As Integer)
			Dim info As CustomCheckedItemInfo = TryCast(MyBase.GetItemByIndex(index), CustomCheckedItemInfo)
			If info IsNot Nothing Then
				info.IsUnderLine = True
			End If
		End Sub
		Protected Friend Overridable Sub OverlineItem(ByVal index As Integer)
			Dim info As CustomCheckedItemInfo = TryCast(MyBase.GetItemByIndex(index), CustomCheckedItemInfo)
			If info IsNot Nothing Then
				info.IsOverLine = True
			End If
		End Sub
		Protected Friend Overridable Function ItemCountAccessMethod() As Integer
			Return ItemCount
		End Function
		Protected Friend Overridable Sub DropLine()
			For Each info As CustomCheckedItemInfo In VisibleItems
				info.DropLine()
			Next info
		End Sub
		Protected Friend Overridable Sub MarkItem(ByVal targetIndex As Integer, ByVal sourceIndex As Integer)
			DropLine()
			dragDropLineColor_Renamed = Color.Red
			If (targetIndex = sourceIndex) OrElse (targetIndex = sourceIndex + 1) OrElse (sourceIndex = ItemCount - 1 AndAlso targetIndex = -1) Then
				dragDropLineColor_Renamed = Color.LightGray
			End If
			If targetIndex = -1 Then
				UnderlineItem(ItemCount - 1)
			Else
				OverlineItem(targetIndex)
			End If
			If targetIndex > 0 Then
				UnderlineItem(targetIndex - 1)
			End If
			OwnerControl.Invalidate()
		End Sub

		Public Class CustomCheckedItemInfo
			Inherits CheckedItemInfo

'INSTANT VB NOTE: The variable isUnderLine was renamed since Visual Basic does not allow variables and other class members to have the same name:
'INSTANT VB NOTE: The variable isOverLine was renamed since Visual Basic does not allow variables and other class members to have the same name:
			Private isUnderLine_Renamed, isOverLine_Renamed As Boolean

			Public Sub New(ByVal ownerControl As BaseListBoxControl, ByVal rect As Rectangle, ByVal item As Object, ByVal text As String, ByVal index As Integer, ByVal checkState As CheckState, ByVal enabled As Boolean)
				MyBase.New(ownerControl, rect, item, text, index, checkState, enabled)
				DropLine()
			End Sub
			Public Sub New(ByVal info As CheckedItemInfo)
				Me.New(info.ListBoxControl, info.Bounds, info.Item, info.Text, info.Index, info.CheckArgs.CheckState, info.Enabled)
				Me.CheckArgs.Assign(info.CheckArgs)
				Me.TextRect = info.TextRect
			End Sub
			Protected Friend Overridable Property IsUnderLine() As Boolean
				Get
					Return isUnderLine_Renamed
				End Get
				Set(ByVal value As Boolean)
					isUnderLine_Renamed = value
				End Set
			End Property
			Protected Friend Overridable Property IsOverLine() As Boolean
				Get
					Return isOverLine_Renamed
				End Get
				Set(ByVal value As Boolean)
					isOverLine_Renamed = value
				End Set
			End Property
			Protected Friend Overridable Sub DropLine()
				isUnderLine_Renamed = False
				isOverLine_Renamed = False
			End Sub
		End Class
	End Class
	Public Class CustomPainterCheckedListBox
		Inherits PainterCheckedListBox

		Private Const lineWidth As Integer = 1
		Protected Overrides Sub DrawItemCore(ByVal info As ControlGraphicsInfoArgs, ByVal itemInfo As BaseListBoxViewInfo.ItemInfo, ByVal e As ListBoxDrawItemEventArgs)
			MyBase.DrawItemCore(info, itemInfo, e)
			Dim customInfo As Sample.CustomCheckedListBoxViewInfo.CustomCheckedItemInfo = TryCast(itemInfo, Sample.CustomCheckedListBoxViewInfo.CustomCheckedItemInfo)
			If customInfo Is Nothing Then
				Return
			End If
			Dim rec As New Rectangle(itemInfo.Bounds.Location, New Size(itemInfo.Bounds.Width, lineWidth))
			Dim lineColor As Color = CType(info.ViewInfo, CustomCheckedListBoxViewInfo).DragDropLineColor
			If customInfo.IsOverLine Then
				If customInfo.Index = 0 Then
					rec.Height += 1
				End If
				e.Cache.FillRectangle(lineColor, rec)
			End If
			If customInfo.IsUnderLine Then
				rec.Offset(0, itemInfo.Bounds.Height - lineWidth)
				If customInfo.Index = CType(info.ViewInfo, CustomCheckedListBoxViewInfo).ItemCountAccessMethod() - 1 Then
					rec.Height += 1
				End If
				e.Cache.FillRectangle(lineColor, rec)
			End If
		End Sub
	End Class
End Namespace
