Imports System
Imports System.IO
Imports System.Collections
Public Class Form1
    Function GetTime() As String
        Return CStr(Now)
    End Function
    Sub DisplayTime()
        MsgBox(GetTime)
    End Sub
    Sub Application(value As Integer)
        Dim path As String = "C:\vb"
        Dim currentMinutesTicks As Integer = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMinutes
        Dim directoryInfo As New DirectoryInfo(path)
        Dim fileList As FileInfo() = directoryInfo.GetFiles()
        TextBox1.AppendText("Current time is: " & currentMinutesTicks & Environment.NewLine)
        TextBox1.AppendText("Interval is set on: " & value & Environment.NewLine)
        For Each item In fileList
            If (item.Extension.Equals("txt")) Then
                Dim minutes As Integer = TimeSpan.FromTicks(item.CreationTime.Ticks).TotalMinutes
                TextBox1.AppendText("Name: " & item.Name & " Data utworzenia: " & minutes & Environment.NewLine)
                TextBox1.AppendText("Checking: " & currentMinutesTicks & " " & minutes & " " & value & Environment.NewLine)
                If (currentMinutesTicks - minutes >= value) Then
                    System.IO.File.Delete(path & "\" & item.Name)
                    TextBox1.AppendText("Deleted: " & path & "\" & item.Name & Environment.NewLine)
                End If
            End If
        Next
    End Sub
End Class
