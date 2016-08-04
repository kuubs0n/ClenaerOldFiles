<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label1.Location = New System.Drawing.Point(15, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "TEST"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ClenaerOldFiles.My.Resources.Resources.kozieglowy
        Me.PictureBox1.Location = New System.Drawing.Point(545, 15)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(142, 102)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(750, 275)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Me.PrepareGui()
        Me.Invalidate()
        Me.Update()
        Me.Refresh()
        Me.Work()
    End Sub
    Public Sub PrepareGui()
        Me.Label1.Text = "Program CleanerOldFiles napisany dla ZD Kozieglowy. Sierpień 2016"
        Me.Label1.Text += "Autorzy: Jakub Krawczyk <jakubkrawczyk2014@gmail.com> Wojciech Pacyna <wojciech@note.pl>"
        Me.Label1.Text += "Program kasuje lub przesuwa do innego folderu pliki starsze niż X minut"
        Me.Label1.Text += "Dotyczy to wskazanego folderu oraz jego podfolderów"
        Me.Label1.Text += "Jeśli nie wskazano folderu docelowego - pliki zostaną skasowane"
        Me.Label1.Text += "Program zamyka swoje okno po 15 sekundach od zakończenia pracy"
        Me.Label1.Text += "____________________________________________________________________________________"

        Me.Label1.Text += "Przykładowa konfiguracja pliku config.ini poniżej:"
        Me.Label1.Text += "katalog_zrodlowy=""C:\Skany"""
        Me.Label1.Text += "wiek_pliku_w_minutach=120"
        Me.Label1.Text += "opcjonalny_katalog_docelowy=""C:\stare_skany"""

        Me.Label1.Text += "____________________________________________________________________________________"

        Me.Label1.Text += "Ten program został wywołany z poniższymi parametrami"
        Me.Label1.Text += "katalog_zrodlowy="
        Me.Label1.Text += "wiek_pliku_w_minutach="
        Me.Label1.Text += "opcjonalny_katalog_docelowy="
    End Sub
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents PictureBox1 As PictureBox
End Class
