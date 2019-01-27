Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO


Public Class Form1
    Dim dt As DataTable
    Public Sub New()
        InitializeComponent()
        dt = New DataTable
        dt.Columns.Add("Dosage", GetType(Integer))
        dt.Columns.Add("Drug", GetType(String))
        dt.Columns.Add("Patient", GetType(String))
        dt.Columns.Add("Date", GetType(DateTime))

        dt.Rows.Add(25, "Indocin", "David", DateTime.Now)
        dt.Rows.Add(50, "Enebrel", "Sam", DateTime.Now)
        dt.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now)
        dt.Rows.Add(21, "Combivent", "Janet", DateTime.Now)
        dt.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now)

        DataGridView1.DataSource = dt

        SaveFileDialog1.FileName = ""
        SaveFileDialog1.Filter = "PDF (*.pdf)|*.pdf"

        TextBox1.Text = "Listado de Pacientes"
        TextBox2.Text = "C:\prueba.pdf"


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = SaveFileDialog1.FileName
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim paragraph As Paragraph
        Dim PdfFile As New Document(PageSize.LETTER, 40, 40, 2, 20)
        PdfFile.AddTitle(TextBox1.Text)
        Dim writer As PdfWriter = PdfWriter.GetInstance(PdfFile, New FileStream(TextBox2.Text, FileMode.Create))
        PdfFile.Open()

        'Declarasion de Estilos
        Dim pTitle As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK)
        Dim pTable As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)

        'Imagen Cabecera        
        Dim converter As New ImageConverter
        Dim headerImg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(converter.ConvertTo(Global.Create_PDF_VB.My.Resources.Resources.logo, GetType(Byte())))
        Dim head As New PdfPTable(1)
        Dim cimg As New PdfPCell(headerImg, True)
        cimg.HorizontalAlignment = Element.ALIGN_CENTER
        cimg.FixedHeight = 60.0F
        cimg.Border = PdfPCell.NO_BORDER
        head.AddCell(cimg)
        PdfFile.Add(head)


        'Agregar Titulo al PDf
        paragraph = New Paragraph(New Chunk(TextBox1.Text, pTitle))
        paragraph.Alignment = Element.ALIGN_CENTER
        paragraph.SpacingAfter = 5.0F
        paragraph.SpacingBefore = 5.0F
        PdfFile.Add(paragraph)


        'Insertar datos en la tabla
        Dim PdfTable As New PdfPTable(DataGridView1.Columns.Count)

        'COnfigurara Tabla
        PdfTable.TotalWidth = 500.0F
        PdfTable.LockedWidth = True

        Dim widths(0 To DataGridView1.Columns.Count - 1) As Single

        For i As Integer = 0 To DataGridView1.Columns.Count - 1
            widths(i) = 1.0F
        Next

        PdfTable.SetWidths(widths)
        PdfTable.HorizontalAlignment = 0
        PdfTable.SpacingBefore = 5.0F

        'Declaracion de las celdas 
        Dim pdfcell As New PdfPCell
        For i As Integer = 0 To DataGridView1.Columns.Count - 1

            'cabecera de la celda
            pdfcell = New PdfPCell(New Phrase(New Chunk(DataGridView1.Columns(i).HeaderText, pTable)))
            pdfcell.HorizontalAlignment = PdfPCell.ALIGN_LEFT

            PdfTable.AddCell(pdfcell)

        Next

        For i As Integer = 0 To DataGridView1.Rows.Count - 2
            For j As Integer = 0 To DataGridView1.Columns.Count - 1
                pdfcell = New PdfPCell(New Phrase(DataGridView1(j, i).Value.ToString(), pTable))
                PdfTable.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                PdfTable.AddCell(pdfcell)
            Next
        Next

        PdfFile.Add(PdfTable)
        PdfFile.Close()
        MsgBox("Pdf Creado", MsgBoxStyle.OkOnly)

    End Sub


End Class
