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
        Dim fileWriter As New StreamWriter("logs.csv", True)

        TextBox1.AppendText("Current time is: " & currentMinutesTicks & Environment.NewLine)
        TextBox1.AppendText("Interval is set on: " & timeToDelete & Environment.NewLine)

        Dim subfoldersList As String() = GetSubDirectories(path)

        For Each item In subfoldersList
            Dim directoryInfo As New DirectoryInfo(item)
            Dim fileList As FileInfo() = directoryInfo.GetFiles()
            For Each item2 In fileList
                Dim minutes As Integer = TimeSpan.FromTicks(item2.CreationTime.Ticks).TotalMinutes

                If (item2.Extension.Equals(".pdf") Or item2.Extension.Equals(".jpeg") Or item2.Extension.Equals(".jpg") Or item2.Extension.Equals(".JPG") Or item2.Extension.Equals(".JPEG") Or item2.Extension.Equals(".PDF")) Then

                    TextBox1.AppendText("Name: " & item2.Name & " Creation Time: " & minutes & Environment.NewLine)
                    TextBox1.AppendText("Checking: " & currentMinutesTicks & " " & minutes & " " & timeToDelete & Environment.NewLine)

                    If (currentMinutesTicks - minutes >= timeToDelete) Then
                        If (haveToMove = False) Then
                            TextBox1.AppendText("Deleted," & item & "\" & item2.Name & "," & item2.CreationTime & "," & DateTime.Now & Environment.NewLine)
                            System.IO.File.Delete(item & "\" & item2.Name)

                            fileWriter.Write("Deleted," & item & "\" & item2.Name & "," & item2.CreationTime & "," & DateTime.Now & Environment.NewLine)
                        Else
                            'item2.MoveTo(pathToMove & "\" & item2.FullName)
                            Dim directoryPath As String = pathToMove & item2.FullName.Substring(2, item2.FullName.Length - 6)
                            If (Not System.IO.Directory.Exists(pathToMove & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 2))) Then
                                System.IO.Directory.CreateDirectory(pathToMove & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 2))
                            End If
                            If (My.Computer.FileSystem.FileExists(pathToMove & item2.FullName.Substring(2, item2.FullName.Length - 2))) Then
                                My.Computer.FileSystem.DeleteFile(pathToMove & item2.FullName.Substring(2, item2.FullName.Length - 2))
                            End If
                            Dim filePath As String = pathToMove & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 3)
                            item2.MoveTo(pathToMove & item2.FullName.Substring(2, item2.FullName.Length - 2))
                            TextBox1.AppendText("Moved," & item & "\" & item2.Name & "," & pathToMove & item2.FullName.Substring(2, item2.FullName.Length - 2) & "," & Environment.NewLine)
                            fileWriter.Write("Moved," & item & "\" & item2.Name & "," & item2.FullName.Substring(2, item2.FullName.Length - 2) & "," & item2.CreationTime & "," & DateTime.Now & Environment.NewLine)
                        End If
                    End If
                End If
            Next
        Next
        fileWriter.Close()
        Application.Exit()
    End Sub
    Public Function GetSubDirectories(ByVal directory As String) As String()
        Dim file As New IO.DirectoryInfo(directory)
        Dim path() As String = {}
        For Each subfolder As IO.DirectoryInfo In file.GetDirectories()
            Array.Resize(path, path.Length + 1)
            path(path.Length - 1) = subfolder.FullName
            For Each s As String In GetSubDirectories(subfolder.FullName)
                Array.Resize(path, path.Length + 1)
                path(path.Length - 1) = s
                TextBox1.Text = TextBox1.Text + s + Environment.NewLine
            Next
        Next
        Return path
    End Function
End Class
