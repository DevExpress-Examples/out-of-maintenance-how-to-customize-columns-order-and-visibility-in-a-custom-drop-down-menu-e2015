Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Collections

Namespace Sample
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub
		Private myUsers As New Users()
		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			myUsers.Add(New User("Antuan", "Acapulco", 23))
			myUsers.Add(New User("Bill", "Brussels", 17))
			myUsers.Add(New User("Charli", "Chicago", 45))
			myUsers.Add(New User("Denn", "Denver", 20))
			myUsers.Add(New User("Eva", "Everton", 23))
			customGridControl1.DataSource = myUsers
			customGridColumn1.FieldName = "Name"
			customGridColumn1.Caption = customGridColumn1.FieldName
			customGridColumn2.FieldName = "City"
			customGridColumn2.Caption = customGridColumn2.FieldName
			customGridColumn3.FieldName = "Age"
			customGridColumn3.Caption = customGridColumn3.FieldName
		End Sub
	End Class
	Public Class User
'INSTANT VB NOTE: The variable name was renamed since Visual Basic does not allow variables and other class members to have the same name:
'INSTANT VB NOTE: The variable city was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private name_Renamed, city_Renamed As String
'INSTANT VB NOTE: The variable age was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private age_Renamed As Integer
		Public Sub New(ByVal name As String, ByVal city As String, ByVal age As Integer)
			Me.name_Renamed = name
			Me.city_Renamed = city
			Me.age_Renamed = age
		End Sub
		Public Property Age() As Integer
			Set(ByVal value As Integer)
				age_Renamed = value
			End Set
			Get
				Return age_Renamed
			End Get
		End Property
		Public Property Name() As String
			Set(ByVal value As String)
				name_Renamed = value
			End Set
			Get
				Return name_Renamed
			End Get
		End Property
		Public Property City() As String
			Set(ByVal value As String)
				city_Renamed = value
			End Set
			Get
				Return city_Renamed
			End Get
		End Property
	End Class
	Public Class Users
		Inherits ArrayList

		Default Public Overrides Property Item(ByVal index As Integer) As Object
			Get
				Return CType(MyBase.Item(index), User)
			End Get
			Set(ByVal value As Object)
				MyBase.Item(index) = value
			End Set
		End Property
	End Class
End Namespace