Public Class frmReadme

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.Dispose()
    End Sub

    Private Sub frmReadme_Load(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load
        Dim filename As String = Nothing
        filename = "https://dl.dropboxusercontent.com/u/100084516/KoolKit%20V1RC2.htm"
        'MsgBox(filename)
        readmeWebBrowser.Navigate(filename)

    End Sub

    Shared Function App_Path() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory()
    End Function
End Class