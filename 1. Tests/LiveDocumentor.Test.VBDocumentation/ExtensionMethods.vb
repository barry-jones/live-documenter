Imports System.Runtime.CompilerServices

Public Class ExtensionMethods
    Public Shared Function WhereEntityIs(
        self As IEnumerable(Of String),
        metadata As String()
        ) As IEnumerable(Of String)
        WhereEntityIs = New List(Of String)()
    End Function
End Class

