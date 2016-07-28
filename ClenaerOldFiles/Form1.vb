Imports System
Imports System.IO
Imports System.Collections
Public Class Form1
    Sub Work()

        Dim arguments As String() = Environment.GetCommandLineArgs
        Dim haveToMove = False
        Dim timeToDelete As Integer = arguments(2)
        Dim path As String = arguments(1)
        Dim pathToMove As String
        Try
            pathToMove = arguments(3)
            haveToMove = True
        Catch
            haveToMove = False
        End Try

        Dim currentMinutesTicks As Integer = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMinutes
        Dim directoryInfo As New DirectoryInfo(path)
        Dim fileList As FileInfo() = directoryInfo.GetFiles()
        Dim fileWriter As New StreamWriter("logs.csv")

        TextBox1.AppendText("Current time is: " & currentMinutesTicks & Environment.NewLine)
        TextBox1.AppendText("Interval is set on: " & timeToDelete & Environment.NewLine)

        For Each item In fileList
            Dim minutes As Integer = TimeSpan.FromTicks(item.CreationTime.Ticks).TotalMinutes

            If (item.Extension.Equals(".pdf") Or item.Extension.Equals(".jpg") Or item.Extension.Equals(".JPG") Or item.Extension.Equals(".JPEG") Or item.Extension.Equals(".PDF")) Then

                TextBox1.AppendText("Name: " & item.Name & " Data utworzenia: " & minutes & Environment.NewLine)
                TextBox1.AppendText("Checking: " & currentMinutesTicks & " " & minutes & " " & timeToDelete & Environment.NewLine)

                If (currentMinutesTicks - minutes >= timeToDelete) Then
                    If (haveToMove = False) Then
                        TextBox1.AppendText("Deleted," & path & "\" & item.Name & "," & item.CreationTime & "," & DateTime.Now & Environment.NewLine)
                        System.IO.File.Delete(path & "\" & item.Name)

                        fileWriter.Write("Deleted," & path & "\" & item.Name & "," & item.CreationTime & "," & DateTime.Now & Environment.NewLine)
                    Else

                        If (My.Computer.FileSystem.FileExists(pathToMove & "\" & item.Name)) Then
                            My.Computer.FileSystem.DeleteFile(pathToMove & "\" & item.Name)
                        End If
                        item.MoveTo(pathToMove & "\" & item.Name)
                            TextBox1.AppendText("Moved," & path & "\" & item.Name & "," & pathToMove & "\" & item.Name & "," & Environment.NewLine)
                            fileWriter.Write("Moved," & path & "\" & item.Name & "," & pathToMove & "\" & item.Name & "," & item.CreationTime & "," & DateTime.Now & Environment.NewLine)
                        End If
                    End If
            End If
        Next
        fileWriter.Close()
        Application.Exit()
    End Sub
End Class
