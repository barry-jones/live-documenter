''' <summary>
''' Concatenation operators can only be overloaded in Visual Basic .NET
''' </summary>
''' <remarks></remarks>
Public Class ConcatenationOperator
    Public Shared Operator &(ByVal left As String, ByVal right As ConcatenationOperator) As String
        Return left & " " & right.ToString()
    End Operator
End Class
