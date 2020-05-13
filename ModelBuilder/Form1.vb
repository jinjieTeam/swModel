Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim db As HPMDEntities

        db = New HPMDEntities()
        Dim resultList As List（Of sysInfo）

        resultList = db.sysInfo.Where(Function(s) s.sysName <> "").ToList()

        For i = 0 To resultList.Count - 1
            MessageBox.Show(resultList(i).ID)
        Next

        Dim result As sysInfo
        result = db.sysInfo.Find(2)
        MessageBox.Show(result.sysName)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim t As Double = Function(x As Double) As Double  return x*x))


    End Sub
End Class
