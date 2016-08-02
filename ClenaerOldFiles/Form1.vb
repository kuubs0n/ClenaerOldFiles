Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Collections
Public Class Form1

    Public Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String,
            ByVal lpKeyName As String,
            ByVal lpDefault As String,
            ByVal lpReturnedString As StringBuilder,
            ByVal nSize As Integer,
            ByVal lpFileName As String) As Integer

    Sub Work()

        'Dim arguments As String() = Environment.GetCommandLineArgs
        'Dim haveToMove = False
        'Dim timeToDelete As Integer = arguments(2)
        'Dim path As String = arguments(1)
        'Dim pathToMove As String
        'Try
        '    pathToMove = arguments(3)
        '    haveToMove = True
        'Catch
        '    haveToMove = False
        'End Try

        Dim haveToMove = False
        Dim timeToDelete As StringBuilder = New StringBuilder(100)
        Dim pathFrom As StringBuilder = New StringBuilder(100)
        Dim pathTo As StringBuilder = New StringBuilder(100)
        Dim startupPath As String = Application.StartupPath()
        GetPrivateProfileString("SETTINGS", "TIME", "", timeToDelete, timeToDelete.Capacity, startupPath & "\config.ini")
        GetPrivateProfileString("SETTINGS", "FROM", "", pathFrom, pathFrom.Capacity, startupPath & "\config.ini")
        Try
            GetPrivateProfileString("SETTINGS", "TO", "", pathTo, pathTo.Capacity, startupPath & "\config.ini")
            haveToMove = True
        Catch
            haveToMove = False
        End Try
        If (pathTo.Length = 0) Then
            haveToMove = False
        End If


        Dim currentMinutesTicks As Integer = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMinutes
        Dim fileWriter As New StreamWriter("logs.csv", True)

        TextBox1.AppendText("Current time is: " & currentMinutesTicks & Environment.NewLine)
        TextBox1.AppendText("Interval is set on: " & timeToDelete.ToString & Environment.NewLine)

        Dim subfoldersList As String() = GetSubDirectories(pathFrom.ToString)

        For Each item In subfoldersList
            Dim directoryInfo As New DirectoryInfo(item)
            Dim fileList As FileInfo() = directoryInfo.GetFiles()
            For Each item2 In fileList
                Dim minutes As Integer = TimeSpan.FromTicks(item2.CreationTime.Ticks).TotalMinutes

                If (item2.Extension.Equals(".pdf") Or item2.Extension.Equals(".jpeg") Or item2.Extension.Equals(".jpg") Or item2.Extension.Equals(".JPG") Or item2.Extension.Equals(".JPEG") Or item2.Extension.Equals(".PDF")) Then

                    TextBox1.AppendText("Name: " & item2.Name & " Creation Time: " & minutes & Environment.NewLine)
                    TextBox1.AppendText("Checking: " & currentMinutesTicks & " " & minutes & " " & timeToDelete.ToString & Environment.NewLine)

                    If (currentMinutesTicks - minutes >= timeToDelete.ToString) Then
                        If (haveToMove = False) Then
                            TextBox1.AppendText("Deleted," & item & "\" & item2.Name & "," & item2.CreationTime & "," & DateTime.Now & Environment.NewLine)
                            System.IO.File.Delete(item & "\" & item2.Name)

                            fileWriter.Write("Deleted," & item & "\" & item2.Name & "," & item2.CreationTime & "," & DateTime.Now & Environment.NewLine)
                        Else
                            'item2.MoveTo(pathToMove & "\" & item2.FullName)
                            Dim directoryPath As String = pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 6)
                            If (Not System.IO.Directory.Exists(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 2))) Then
                                System.IO.Directory.CreateDirectory(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 2))
                            End If
                            If (My.Computer.FileSystem.FileExists(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 2))) Then
                                My.Computer.FileSystem.DeleteFile(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 2))
                            End If
                            Dim filePath As String = pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 3)
                            item2.MoveTo(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 2))
                            TextBox1.AppendText("Moved," & item & "\" & item2.Name & "," & pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 2) & "," & Environment.NewLine)
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
