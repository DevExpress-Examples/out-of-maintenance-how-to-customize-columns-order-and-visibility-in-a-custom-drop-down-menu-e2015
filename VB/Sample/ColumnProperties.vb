Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Collections
Imports System.Windows.Forms

Namespace Sample
	Public Class ColumnProperties
		Implements IComparable

'INSTANT VB NOTE: The variable visible was renamed since Visual Basic does not allow variables and other class members to have the same name:
'INSTANT VB NOTE: The variable allowQuickHide was renamed since Visual Basic does not allow variables and other class members to have the same name:
'INSTANT VB NOTE: The variable allowMove was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private visible_Renamed, allowQuickHide_Renamed, allowMove_Renamed As Boolean
'INSTANT VB NOTE: The variable visibleIndex was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private visibleIndex_Renamed As Integer
'INSTANT VB NOTE: The variable caption was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private caption_Renamed As String
		Public Sub New(ByVal caption As String, ByVal visible As Boolean, ByVal visibleIndex As Integer, ByVal allowQuickHide As Boolean, ByVal allowMove As Boolean)
			Me.visible_Renamed = visible
			Me.visibleIndex_Renamed = visibleIndex
			Me.allowQuickHide_Renamed = allowQuickHide
			Me.allowMove_Renamed = allowMove
			Me.caption_Renamed = caption
		End Sub
		Public Property Visible() As Boolean
			Get
				Return visible_Renamed
			End Get
			Set(ByVal value As Boolean)
				visible_Renamed = value
			End Set
		End Property
		Public Property VisibleIndex() As Integer
			Get
				Return visibleIndex_Renamed
			End Get
			Set(ByVal value As Integer)
				visibleIndex_Renamed = value
			End Set
		End Property
		Public Property AllowQuickHide() As Boolean
			Get
				Return allowQuickHide_Renamed
			End Get
			Set(ByVal value As Boolean)
				allowQuickHide_Renamed = value
			End Set
		End Property
		Public Property AllowMove() As Boolean
			Get
				Return allowMove_Renamed
			End Get
			Set(ByVal value As Boolean)
				allowMove_Renamed = value
			End Set
		End Property
		Public Property Caption() As String
			Get
				Return caption_Renamed
			End Get
			Set(ByVal value As String)
				caption_Renamed = value
			End Set
		End Property
		Public Property CheckState() As CheckState
			Get
				If Visible Then
					Return CheckState.Checked
				Else
					Return CheckState.Unchecked
				End If
			End Get
			Set(ByVal value As CheckState)
				If value = CheckState.Checked Then
					Visible = True
				Else
					Visible = False
				End If
			End Set
		End Property


		#Region "IComparable Members"

		Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
			Dim column As ColumnProperties = TryCast(obj, ColumnProperties)
			If column Is Nothing Then
				Return 0
			End If
			If visibleIndex_Renamed < 0 AndAlso column.VisibleIndex >= 0 Then
				Return 1
			End If
			If visibleIndex_Renamed >= 0 AndAlso column.VisibleIndex < 0 Then
				Return -1
			End If
			If visibleIndex_Renamed < 0 AndAlso column.VisibleIndex < 0 Then
				Return Caption.CompareTo(column.Caption)
			End If
			If VisibleIndex > column.VisibleIndex Then
				Return 1
			ElseIf VisibleIndex < column.VisibleIndex Then
				Return -1
			Else
				Return 0
			End If
		End Function

		#End Region
	End Class
	Public Class ColumnPropertiesCollection
		Inherits ArrayList

		Default Public Shadows Overridable Property Item(ByVal index As Integer) As ColumnProperties
			Get
				If index < 0 OrElse index > Count - 1 Then
					Return Nothing
				End If
				Return TryCast(MyBase.Item(index), ColumnProperties)
			End Get
			Set(ByVal value As ColumnProperties)
				If index < 0 OrElse index > Count - 1 Then
					Return
				End If
				MyBase.Item(index) = value
			End Set
		End Property
		Default Public Shadows Overridable ReadOnly Property Item(ByVal caption As String) As ColumnProperties
			Get
				Return Me(IndexFromeCaption(caption))
			End Get
		End Property

		Private Function IndexFromeCaption(ByVal caption As String) As Integer
			For i As Integer = 0 To Count - 1
				If Me(i).Caption = caption Then
					Return i
				End If
			Next i
			Return -1
		End Function
		Public Overloads Sub Add(ByVal caption As String, ByVal visible As Boolean, ByVal visibleIndex As Integer, ByVal allowQuickHide As Boolean, ByVal allowMove As Boolean)
			MyBase.Add(New ColumnProperties(caption, visible, visibleIndex, allowQuickHide, allowMove))
		End Sub
		Public Overloads Sub Add(ByVal caption As String, ByVal visible As Boolean, ByVal visibleIndex As Integer, ByVal allowQuickHide As Boolean)
			MyBase.Add(New ColumnProperties(caption, visible, visibleIndex, allowQuickHide, True))
		End Sub

		Public Function GetCopy() As ColumnPropertiesCollection
			Dim collection As New ColumnPropertiesCollection()
			For Each column As ColumnProperties In Me
				collection.Add(column)
			Next column
			Return collection
		End Function
	End Class
End Namespace
