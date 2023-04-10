Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.Linq



Public Class UploadFTAMaster
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Public AuthAccess As Boolean = False
    Public AuthInsert As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public FilePath As String = ""
    Public Ext As String = ""
    Public FileName As String = ""
    Public aMsg As String = ""
    Dim Memo As String = ""
    Private Shared intProgress As Integer = 0
    Public dt As DataTable
    Public pTypeValue As String
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            GetFactoryCode()
            '    dsRegNo.SelectParameters("Type").DefaultValue = "6"
            '    dsRegNo.SelectParameters("FactoryCode1").DefaultValue = "F001"
            '    'FillCBRegNoGrid()
            'Else
            '    dsRegNo.SelectParameters("Type").DefaultValue = "6"
            '    dsRegNo.SelectParameters("FactoryCode1").DefaultValue = cboFactory.Value
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu("A060")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName

        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "A060")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "A060")
        If AuthUpdate = False Then
            'Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            'commandColumn.ShowEditButton = False
            'commandColumn.ShowNewButtonInHeader = False
        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "A060")
        If AuthDelete = False Then
            'Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            'commandColumn.ShowDeleteButton = False
        End If

    End Sub
    Private Sub cbTypeValue_Callback(source As Object, e As CallbackEventArgs) Handles cbTypeValue.Callback

        Session("TypeValue") = cboType.Value

    End Sub
    Private Sub GetFactoryCode()
        'cboFactory.DataSource = clsFactoryDB.GetList
        cboFactory.DataSource = ClsSPCItemCheckByTypeDB.FillComboFactoryGrid("1", Session("user"))
        cboFactory.DataBind()
    End Sub
    Private Sub FillCBRegNoGrid()
        Dim DT As New DataTable
        DT = ClsSPCItemCheckByTypeDB.GetRegNo(cboFactory.Value)
        'Dim myCombo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
        'CType(TryCast(sender, ASPxGridView).Columns("RegistrationNo"), GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = DT
        'myCombo.DataSource = DT
        'myCombo.DataBindItems()

        'Dim comboColumn = CType(Grid.Columns("RegistrationNo"), GridViewDataComboBoxColumn)
        'comboColumn.PropertiesComboBox.DataSource = DT
        'comboColumn.PropertiesComboBox.TextField = "Description"
        'comboColumn.PropertiesComboBox.ValueField = "RegistrationNo"
        'comboColumn.PropertiesComboBox.ValueType = GetType(String)

    End Sub
#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        'Grid.JSProperties("cp_message") = ErrMsg
        'Grid.JSProperties("cp_type") = msgType
        'Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_GridLoad(FactoryCode As String, ItemTypeDescription As String, MachineProccess As String)
        Dim dtItemCheckByType As DataTable
        Try
            If FactoryCode Is Nothing Then
                FactoryCode = ""
            End If
            If ItemTypeDescription Is Nothing Then
                ItemTypeDescription = ""
            End If
            If MachineProccess Is Nothing Then
                MachineProccess = ""
            End If

            If MachineProccess <> "ALL" AndAlso MachineProccess <> "" Then
                MachineProccess = MachineProccess.Substring(0, MachineProccess.IndexOf(" -"))
            End If

            dtItemCheckByType = ClsSPCItemCheckByTypeDB.GetList(pUser, FactoryCode, ItemTypeDescription, MachineProccess, cboType.Value)
            'Grid.DataSource = dtItemCheckByType
            'Grid.DataBind()

            hdUserLogin.Value = pUser
            hdFactoryCode.Value = FactoryCode
            hdItemTypeCode.Value = cboType.Value

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub cboType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboType.Callback
        cboType.DataSource = ClsSPCItemCheckByTypeDB.GetListItemType()
        cboType.DataBind()

        If (IsNothing(Session("cboFactoryValue")) OrElse Session("cboFactoryValue") = "") Then
            Session("cboFactoryValue") = cboFactory.Value
        End If
    End Sub
    Private Sub up_FillcomboGrid(ByVal cmb As ASPxComboBox, Type As String, Optional Param As String = "")
        dt = ClsSPCItemCheckByTypeDB.FillComboFactoryGrid(Type, Param)
        With cmb
            .Items.Clear() : .Columns.Clear()
            .DataSource = dt
            '.Columns.Add("FactoryCode") : .Columns(0).Visible = False
            '.Columns.Add("FactoryName") : .Columns(1).Width = 100

            .TextField = "FactoryName"
            .ValueField = "FactoryCode"
            .DataBind()
            .SelectedIndex = IIf(Type = 0, 0, -1)
        End With
    End Sub
    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        ExportExcel()
    End Sub
    Private Sub ExportExcel()
        Try
            Dim FileName As String = ""

            If rdlThemeID1.Value = "FTAMaster" Then
                FileName = "Template FTA Master.xlsx"
            ElseIf rdlThemeID1.Value = "FTAAction" Then
                FileName = "Template FTA Action.xlsx"
            End If

            Dim fi As New FileInfo(Server.MapPath("~\Template\" + FileName))

            Using excel As New ExcelPackage(fi)


                Dim stream As MemoryStream = New MemoryStream(excel.GetAsByteArray())
                Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                Response.AppendHeader("Content-Disposition", "attachment; filename=" & Format(Date.Now, "yyyy-MM-dd") & "_" & FileName)
                Response.BinaryWrite(stream.ToArray())
                Response.End()
            End Using
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub uplImage_FileUploadComplete(ByVal sender As Object, ByVal e As FileUploadCompleteEventArgs)
        e.CallbackData = UploadFile(e.UploadedFile, DirectCast(sender, DevExpress.Web.ASPxUploadControl).ClientInstanceName.ToString())
    End Sub
    Private Function UploadFile(ByVal uploadedFile As UploadedFile, ByVal pSenderName As String) As String
        Try
            Dim DirFolderDownload = Server.MapPath("~/Upload")
            If Not Directory.Exists(DirFolderDownload) Then
                Directory.CreateDirectory(DirFolderDownload)
            End If
            Dim dateNow As String = Format(CDate(Date.Now), "ddMMyyyy-hhmmss")
            Dim spltFileName() As String = Split(uploadedFile.FileName, ".")
            FileName = spltFileName(0) & "_" & dateNow & "." & spltFileName(1)
            Ext = Path.Combine(MapPath(""))
            FilePath = Ext & "\Upload\" & FileName
            Dim fi As New FileInfo(Server.MapPath("~\Upload\" & FileName))
            If fi.Exists Then
                fi.Delete()
                fi = New FileInfo(Server.MapPath("~\Upload\" & FileName))
            End If
            uploadedFile.SaveAs(FilePath)

            Dim excel As New FileInfo(FilePath)
            Using package = New ExcelPackage(excel)
                Dim workbook = package.Workbook
                Dim worksheet = workbook.Worksheets.First

                ReadWriteContent(worksheet, FileName, pSenderName)
            End Using
            File.Delete(FilePath)
            Return Nothing

        Catch ex As Exception
            'Uploader.JSProperties("cp_AlertFileNotValid") = "2"
            'Uploader.JSProperties("cp_MemoValue") = "Error upload file, cannot find Upload folder in server path!" & vbCrLf
            Return Nothing
        End Try
    End Function
    Private Function countEndOfData(oSheet As ExcelWorksheet) As Integer
        Dim row As Integer = 4
        Dim i As Integer = 0
        Do Until IsNothing(oSheet.Cells(row, 1).Value) And IsNothing(oSheet.Cells(row, 2).Value) And IsNothing(oSheet.Cells(row, 3).Value) And IsNothing(oSheet.Cells(row, 4).Value) And IsNothing(oSheet.Cells(row, 5).Value) And IsNothing(oSheet.Cells(row, 6).Value) And
            IsNothing(oSheet.Cells(row, 7).Value) And IsNothing(oSheet.Cells(row, 8).Value) And IsNothing(oSheet.Cells(row, 9).Value) And IsNothing(oSheet.Cells(row, 10).Value) And IsNothing(oSheet.Cells(row, 11).Value) And IsNothing(oSheet.Cells(row, 12).Value)
            row += 1
            i += 1
        Loop
        Return row
    End Function
    Private Sub ReadWriteContent(oSheet As ExcelWorksheet, ByVal pFileName As String, ByVal pSenderName As String)
        Dim culture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CurrentCulture '("es-ES")
        Dim dateNow As DateTime = Format(CDate(CDate(Date.Now)), "dd MMM yyy hh:mm:ss")
        Dim strDate As String = dateNow.ToString("MM/dd/yyyy") '"G", culture)
        Dim int As Integer
        Try
            Dim fileName() As String = Split(pFileName, "_")
            If pSenderName = "uplFTAMaster" Then

                If oSheet.Name = "FTAMaster" Then
                    Dim errList As New List(Of String)
                    Dim errListRow As Integer = 1
                    Dim totalRows As Integer '= oSheet.Dimension.[End].Row
                    Dim totalCols As Integer '= oSheet.Dimension.[End].Column
                    Dim dt As New DataTable()
                    Dim dr As DataRow = Nothing
                    Dim lstContent As New List(Of String)

                    dt.Columns.Add("FactoryCode")
                    dt.Columns.Add("ItemTypeCode")
                    dt.Columns.Add("ItemCheckCode")
                    dt.Columns.Add("FTAID")
                    dt.Columns.Add("Factor1")
                    dt.Columns.Add("Factor2")
                    dt.Columns.Add("Factor3")
                    dt.Columns.Add("Factor4")
                    dt.Columns.Add("CounterMeasure")
                    dt.Columns.Add("CheckItem")
                    dt.Columns.Add("CheckOrder")
                    dt.Columns.Add("Remark")
                    dt.Columns.Add("ActiveStatus")
                    dt.Columns.Add("RegisterUser")

                    totalRows = countEndOfData(oSheet)
                    totalCols = oSheet.Dimension.[End].Column
                    Dim RowEnd As Boolean = False
                    Dim CellValue As String = ""
                    For i = 4 To totalRows - 1
                        For y = 1 To totalCols

                            If y = 1 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                Dim dtItemType = ClsUploadFTAMasterDB.GetItemTypeCode(CellValue)

                                If dtItemType.Rows.Count > 0 Then

                                    For ItemType = 0 To dtItemType.Rows.Count

                                        CellValue = dtItemType.Rows(ItemType)("ItemTypeCode").ToString()
                                        oSheet.Cells(i, y).Value = dtItemType.Rows(ItemType)("ItemTypeCode").ToString()
                                        Exit For

                                    Next

                                End If

                                If CellValue = "" Then
                                    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Type' can't be empty!")
                                    errListRow += 1
                                ElseIf CellValue <> Session("TypeValue") Then
                                    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Type' doesn't match !")
                                    errListRow += 1
                                ElseIf IsNothing(ClsUploadFTAMasterDB.GetItemType(CellValue)) OrElse ClsUploadFTAMasterDB.GetItemType(CellValue).Rows.Count <= 0 Then
                                    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Type', Type Code doesn't exist !")
                                    errListRow += 1
                                End If

                            End If
                            If y = 2 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Item Check' can't be empty!")
                                    errListRow += 1
                                ElseIf IsNothing(ClsUploadFTAMasterDB.GetItemCheck(CellValue)) OrElse ClsUploadFTAMasterDB.GetItemCheck(CellValue).Rows.Count <= 0 Then
                                    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Item Check', Item Check doesn't exist !")
                                    errListRow += 1
                                End If

                            End If
                            If y = 3 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'FTA ID' can't be empty!")
                                    errListRow += 1
                                    'ElseIf ClsUploadFTAMasterDB.GetFTAID(CellValue).Rows.Count > 0 Then
                                    '    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'FTA ID', FTA ID can't be duplicate !")
                                    '    errListRow += 1
                                End If

                            End If
                            If y = 4 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                'If CellValue = "" Then
                                '    errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Factor 1' can't be empty!")
                                '    errListRow += 1
                                'End If

                            End If
                            If y = 5 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                'If CellValue = "" Then
                                '    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Factor 2' can't be empty!")
                                '    errListRow += 1
                                'End If

                            End If
                            If y = 6 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                'If CellValue = "" Then
                                '    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Factor 3' can't be empty!")
                                '    errListRow += 1
                                'End If

                            End If
                            If y = 7 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                'If CellValue = "" Then
                                '    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Factor 4' can't be empty!")
                                '    errListRow += 1
                                'End If

                            End If
                            If y = 8 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Counter Measure' can't be empty!")
                                    errListRow += 1
                                End If

                            End If
                            If y = 9 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Check Item' can't be empty!")
                                    errListRow += 1
                                End If

                            End If
                            If y = 10 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Check Order' can't be empty!")
                                    errListRow += 1
                                ElseIf Not Integer.TryParse(CellValue, int) Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Check Order' incorrect format!")
                                    errListRow += 1
                                End If

                            End If
                            If y = 11 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                'If CellValue = "" Then
                                '    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Remark' can't be empty!")
                                '    errListRow += 1
                                'End If

                            End If
                            If y = 12 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Active Status' can't be empty!")
                                    errListRow += 1
                                ElseIf CellValue.ToLower() <> "yes" AndAlso CellValue.ToLower() <> "no" AndAlso Convert.ToInt32(CellValue) <> 1 AndAlso Convert.ToInt32(CellValue) <> 0 Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Active Status' incorrect format!")
                                    errListRow += 1
                                End If

                            End If


                        Next
                        If RowEnd Then
                            Exit For
                        End If
                    Next
                    If errList.Count = 0 Then
                        For row = 4 To totalRows - 1
                            Try
                                For col = 1 To 12
                                    lstContent.Add(IIf(Trim(oSheet.Cells(row, col).Value) = "", "-", Trim(oSheet.Cells(row, col).Value)))
                                Next
                                dr = dt.Rows.Add()
                                dr(0) = Session("cboFactoryValue")
                                dr(1) = lstContent(0)
                                dr(2) = lstContent(1)
                                dr(3) = lstContent(2)
                                dr(4) = IIf(lstContent(3) <> "", lstContent(3), "-")
                                dr(5) = IIf(lstContent(4) <> "", lstContent(4), "-")
                                dr(6) = IIf(lstContent(5) <> "", lstContent(5), "-")
                                dr(7) = IIf(lstContent(6) <> "", lstContent(6), "-")
                                dr(8) = lstContent(7)
                                dr(9) = lstContent(8)
                                dr(10) = lstContent(9)
                                dr(11) = lstContent(10)
                                dr(12) = lstContent(11)
                                dr(13) = Session("user")
                                lstContent.Clear()
                            Catch ex As Exception
                            End Try
                        Next
                    End If
                    If errList.Count > 0 Then
                        Dim strErrMsg As String
                        For Each strErr In errList
                            strErrMsg += strErr & vbCrLf
                        Next
                        Memo += strErrMsg & vbCrLf
                        Memo += "Upload process failed." & vbCrLf
                        show_error(MsgTypeEnum.ErrorMsg, "Upload Process failed!", "1")
                    End If
                    uplFTAMaster.JSProperties("cp_Memo") = "1"
                    uplFTAMaster.JSProperties("cp_MemoValue") = Memo

                    If Memo = "" Then
                        If InsertUpload(dt, 1) Then
                            errList.Add("[" & totalRows - 4 & "] records update")
                            errList.Add("Upload process successfull")
                            If errList.Count > 0 Then
                                Dim strErrMsg As String
                                For Each strErr In errList
                                    strErrMsg += strErr & vbCrLf
                                Next
                                Memo += strErrMsg & vbCrLf
                            End If
                            uplFTAMaster.JSProperties("cp_Memo") = "1"
                            uplFTAMaster.JSProperties("cp_MemoValue") = Memo
                            show_error(MsgTypeEnum.Success, "Save data successfully!", 1)
                        Else
                            uplFTAMaster.JSProperties("cp_MemoValue") = "Error upload file, Cannot insert data to system!" & vbCrLf
                            show_error(MsgTypeEnum.Warning, "Error upload file!", 1)
                        End If

                    End If
                Else
                    uplFTAMaster.JSProperties("cp_AlertFileNotValid") = "1"
                    uplFTAMaster.JSProperties("cp_MemoValue") = "Invalid Template !" & vbCrLf
                End If

            ElseIf pSenderName = "Uploader" Then

                If oSheet.Name = "FTAAction" Then

                    Dim errList As New List(Of String)
                    Dim errListRow As Integer = 1
                    Dim totalRows As Integer '= oSheet.Dimension.[End].Row
                    Dim totalCols As Integer '= oSheet.Dimension.[End].Column
                    Dim dt As New DataTable()
                    Dim dr As DataRow = Nothing
                    Dim lstContent As New List(Of String)

                    dt.Columns.Add("FTAID")
                    'dt.Columns.Add("ActionID")
                    dt.Columns.Add("ActionName")
                    dt.Columns.Add("Remark")

                    totalRows = countEndOfData(oSheet)
                    totalCols = oSheet.Dimension.[End].Column
                    Dim RowEnd As Boolean = False
                    Dim CellValue As String = ""
                    Dim TempFTAID As String = ""
                    For i = 4 To totalRows - 1
                        For y = 1 To totalCols

                            If y = 1 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'FTA ID' can't be empty!")
                                    errListRow += 1
                                End If

                                TempFTAID = CellValue

                            End If
                            'If y = 2 Then
                            '    CellValue = Trim(oSheet.Cells(i, y).Value)

                            '    If CellValue = "" Then
                            '        errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Action ID' can't be empty!")
                            '        errListRow += 1
                            '    ElseIf ClsUploadFTAMasterDB.GetActionID(TempFTAID, CellValue).Rows.Count > 0 Then
                            '        errList.Add("Row " & errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'FTA ID', FTA ID can't be duplicate !")
                            '        errListRow += 1
                            '    End If

                            'End If
                            If y = 2 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                If CellValue = "" Then
                                    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Action Name' can't be empty!")
                                    errListRow += 1
                                End If

                            End If
                            If y = 3 Then
                                CellValue = Trim(oSheet.Cells(i, y).Value)

                                'If CellValue = "" Then
                                '    errList.Add(errListRow & ". Cell " & oSheet.Cells(i, y).Address & " Field 'Remark' can't be empty!")
                                '    errListRow += 1
                                'End If

                            End If

                        Next
                        If RowEnd Then
                            Exit For
                        End If
                    Next
                    If errList.Count = 0 Then
                        For row = 4 To totalRows - 1
                            Try
                                For col = 1 To 4
                                    lstContent.Add(IIf(Trim(oSheet.Cells(row, col).Value) = "", "NULL", Trim(oSheet.Cells(row, col).Value)))
                                Next
                                dr = dt.Rows.Add()
                                dr(0) = lstContent(0)
                                dr(1) = lstContent(1)
                                dr(2) = lstContent(2)
                                dr(3) = lstContent(3)
                                lstContent.Clear()
                            Catch ex As Exception
                            End Try
                        Next
                    End If
                    If errList.Count > 0 Then
                        Dim strErrMsg As String
                        For Each strErr In errList
                            strErrMsg += strErr & vbCrLf
                        Next
                        Memo += strErrMsg & vbCrLf
                        Memo += "Upload process failed." & vbCrLf
                        show_error(MsgTypeEnum.ErrorMsg, "Upload Process failed!", "1")
                    End If
                    Uploader.JSProperties("cp_Memo") = "1"
                    Uploader.JSProperties("cp_MemoValue") = Memo

                    If Memo = "" Then
                        If InsertUpload(dt, 2) Then
                            errList.Add("[" & totalRows - 4 & "] records update")
                            errList.Add("Upload process successfull")
                            If errList.Count > 0 Then
                                Dim strErrMsg As String
                                For Each strErr In errList
                                    strErrMsg += strErr & vbCrLf
                                Next
                                Memo += strErrMsg & vbCrLf
                            End If
                            Uploader.JSProperties("cp_Memo") = "1"
                            Uploader.JSProperties("cp_MemoValue") = Memo
                            show_error(MsgTypeEnum.Success, "Save data successfully!", 1)
                        Else
                            Uploader.JSProperties("cp_MemoValue") = "Error upload file, Cannot insert data to system!" & vbCrLf
                            show_error(MsgTypeEnum.Warning, "Error upload file!", 1)
                        End If

                    End If
                Else
                    Uploader.JSProperties("cp_AlertFileNotValid") = "1"
                    Uploader.JSProperties("cp_MemoValue") = "Invalid Template !" & vbCrLf
                End If

            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, "1")
            Uploader.JSProperties("cp_MemoValue") = ex.Message & vbCrLf '& "Process end  " & strDate 'Format(CDate(Date.Now), "dd MMM yyy hh:mm:ss") & vbCrLf & vbCrLf
        End Try
    End Sub
    Private Function InsertUpload(dtbFileUpload As DataTable, pType As String) As Boolean
        If ClsUploadFTAMasterDB.InsertData(dtbFileUpload, pType, Session("user")) Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

End Class