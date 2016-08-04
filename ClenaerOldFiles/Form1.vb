Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Collections
Public Class Form1

    Public Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String,
            ByVal lpKeyName As String,
            ByVal lpDefault As String,
            ByVal lpReturnedString As StringBuilder,
            ByVal nSize As Integer,
            ByVal lpFileName As String) As Integer
    Sub PrepareGui(ByVal pathFrom As String, ByVal time As String, ByVal pathTo As String)
        Label1.Text = "Program CleanerOldFiles napisany dla ZD Kozieglowy. Sierpień 2016" & Environment.NewLine
        Label1.Text += "Autorzy: Jakub Krawczyk <jakubkrawczyk2014@gmail.com> Wojciech Pacyna <wojciech@note.pl>" & Environment.NewLine
        Label1.Text += "Program kasuje lub przesuwa do innego folderu pliki starsze niż X minut" & Environment.NewLine
        Label1.Text += "Dotyczy to wskazanego folderu oraz jego podfolderów" & Environment.NewLine
        Label1.Text += "Jeśli nie wskazano folderu docelowego - pliki zostaną skasowane" & Environment.NewLine
        Label1.Text += "Program zamyka swoje okno po 15 sekundach od zakończenia pracy" & Environment.NewLine & Environment.NewLine

        Label1.Text += Environment.NewLine & "Przykładowa konfiguracja pliku config.ini poniżej:" & Environment.NewLine
        Label1.Text += "katalog_zrodlowy=""C:\Skany""" & Environment.NewLine
        Label1.Text += "wiek_pliku_w_minutach=""120""" & Environment.NewLine
        Label1.Text += "opcjonalny_katalog_docelowy=""C:\stare_skany""" & Environment.NewLine & Environment.NewLine

        Label1.Text += Environment.NewLine & "Ten program został wywołany z poniższymi parametrami" & Environment.NewLine
        Label1.Text += "katalog_zrodlowy=" & pathFrom & Environment.NewLine
        Label1.Text += "wiek_pliku_w_minutach=" & time & Environment.NewLine
        Label1.Text += "opcjonalny_katalog_docelowy=" & pathTo & Environment.NewLine

    End Sub
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

        GetPrivateProfileString("SETTINGS", "wiek_pliku_w_minutach", "", timeToDelete, timeToDelete.Capacity, startupPath & "\config.ini")
        GetPrivateProfileString("SETTINGS", "katalog_zrodlowy", "", pathFrom, pathFrom.Capacity, startupPath & "\config.ini")
        Try
            GetPrivateProfileString("SETTINGS", "opcjonalny_katalog_docelowy", "", pathTo, pathTo.Capacity, startupPath & "\config.ini")
            haveToMove = True
        Catch
            haveToMove = False
        End Try

        If (pathTo.Length = 0) Then
            haveToMove = False
        End If

        PrepareGui(pathFrom.ToString, timeToDelete.ToString, pathTo.ToString)

        Dim currentMinutesTicks As Integer = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMinutes
        Dim fileWriter As New StreamWriter("logs.csv", True)

        If (pathFrom.Length > 0 And timeToDelete.Length > 0) Then
            If (System.IO.Directory.Exists(pathFrom.ToString)) Then
                Dim directoryInfoMain As New DirectoryInfo(pathFrom.ToString)
                Dim fileListMain As FileInfo() = directoryInfoMain.GetFiles()

                For Each item0 In fileListMain
                    Dim minutes As Integer = TimeSpan.FromTicks(item0.CreationTime.Ticks).TotalMinutes

                    If (item0.Extension.Equals(".pdf") Or item0.Extension.Equals(".jpeg") Or item0.Extension.Equals(".jpg") Or item0.Extension.Equals(".PDF") _
                        Or item0.Extension.Equals(".JPEG") Or item0.Extension.Equals(".JPG")) Then

                        Dim time As Double
                        Double.TryParse(timeToDelete.ToString, time)
                        If (currentMinutesTicks - minutes >= time) Then
                            If (haveToMove = False) Then
                                System.IO.File.Delete(pathFrom.ToString & "\" & item0.Name)
                                fileWriter.Write("Deleted," & pathFrom.ToString & "\" & item0.Name & "," & item0.CreationTime & "," & DateTime.Now & Environment.NewLine)
                            Else
                                Dim directoryPath As String = pathTo.ToString & item0.FullName.Substring(2, item0.FullName.Length - 6)
                                If (Not System.IO.Directory.Exists(pathTo.ToString & item0.FullName.Substring(2, item0.FullName.Length - item0.Name.Length - 2))) Then
                                    System.IO.Directory.CreateDirectory(pathTo.ToString & item0.FullName.Substring(2, item0.FullName.Length - item0.Name.Length - 2))
                                End If
                                If (My.Computer.FileSystem.FileExists(pathTo.ToString & item0.FullName.Substring(2, item0.FullName.Length - 2))) Then
                                    My.Computer.FileSystem.DeleteFile(pathTo.ToString & item0.FullName.Substring(2, item0.FullName.Length - 2))
                                End If
                                Dim filePath As String = pathTo.ToString & item0.FullName.Substring(2, item0.FullName.Length - item0.Name.Length - 3)
                                item0.MoveTo(pathTo.ToString & item0.FullName.Substring(2, item0.FullName.Length - 2))
                                fileWriter.Write("Moved," & pathFrom.ToString & "\" & item0.Name & "," & item0.FullName.Substring(2, item0.FullName.Length - 2) _
                                & "," & item0.CreationTime.ToString & "," & DateTime.Now.ToString & Environment.NewLine)
                            End If
                        End If
                    End If
                Next


                Dim subfoldersList As String() = GetSubDirectories(pathFrom.ToString)
                For Each item In subfoldersList
                    Dim directoryInfo As New DirectoryInfo(item)
                    Dim fileList As FileInfo() = directoryInfo.GetFiles()
                    For Each item2 In fileList
                        Dim minutes As Integer = TimeSpan.FromTicks(item2.CreationTime.Ticks).TotalMinutes

                        If (item2.Extension.Equals(".pdf") Or item2.Extension.Equals(".jpeg") Or item2.Extension.Equals(".jpg") Or item2.Extension.Equals(".JPG") Or item2.Extension.Equals(".JPEG") Or item2.Extension.Equals(".PDF")) Then

                            Dim time As Double
                            Double.TryParse(timeToDelete.ToString, time)
                            If (currentMinutesTicks - minutes >= time) Then
                                If (haveToMove = False) Then
                                    System.IO.File.Delete(item & "\" & item2.Name)

                                    fileWriter.Write("Deleted," & item & "\" & item2.Name & "," & item2.CreationTime.ToString & "," & DateTime.Now.ToString & Environment.NewLine)
                                Else
                                    Dim directoryPath As String = pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 6)
                                    If (Not System.IO.Directory.Exists(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 2))) Then
                                        System.IO.Directory.CreateDirectory(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 2))
                                    End If
                                    If (My.Computer.FileSystem.FileExists(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 2))) Then
                                        My.Computer.FileSystem.DeleteFile(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 2))
                                    End If
                                    Dim filePath As String = pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - item2.Name.Length - 3)
                                    item2.MoveTo(pathTo.ToString & item2.FullName.Substring(2, item2.FullName.Length - 2))
                                    fileWriter.Write("Moved," & item & "\" & item2.Name & "," & item2.FullName.Substring(2, item2.FullName.Length - 2) & "," & item2.CreationTime.ToString & "," & DateTime.Now.ToString & Environment.NewLine)
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        End If
        fileWriter.Close()
        For i As Integer = 1 To 1500
            System.Threading.Thread.Sleep(10)
            System.Windows.Forms.Application.DoEvents()
        Next
        Application.Exit()
    End Sub

    Declare Sub Sleep Lib "kernel32" (ByVal milliseconds As Long)


    Public Function GetSubDirectories(ByVal directory As String) As String()
        Dim file As New IO.DirectoryInfo(directory)
        Dim path() As String = {}
        For Each subfolder As IO.DirectoryInfo In file.GetDirectories()
            Array.Resize(path, path.Length + 1)
            path(path.Length - 1) = subfolder.FullName
            For Each s As String In GetSubDirectories(subfolder.FullName)
                Array.Resize(path, path.Length + 1)
                path(path.Length - 1) = s
            Next
        Next
        Return path
    End Function

End Class
