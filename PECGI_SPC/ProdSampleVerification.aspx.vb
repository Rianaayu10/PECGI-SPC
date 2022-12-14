Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.Data
Imports OfficeOpenXml
Imports DevExpress.Web
Imports OfficeOpenXml.Style
Imports DevExpress.XtraCharts
Imports DevExpress.XtraCharts.Web
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports DevExpress.XtraCharts.Native

Public Class ProdSampleVerification
    Inherits System.Web.UI.Page

#Region "DECLARATION"

    Dim pUser As String = ""
    Dim pEmplooyeeID As String = ""
    Dim MenuID As String = ""
    Dim dt As DataTable
    Dim ds As DataSet

    'AUTHORIZATION
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False

    ' FILL COMBO BOX
    Dim Factory_Sel As String = "1"
    Dim ItemType_Sel As String = "2"
    Dim Line_Sel As String = "3"
    Dim ItemCheck_Sel As String = "4"
    Dim Shift_Sel As String = "5"
    Dim Seq_Sel As String = "6"
    Dim User_Sel As String = "7"
    Dim PIC_Sel As String = "8"

    ' FILL GRID
    Dim GetHeader_ProdDate As String = "1"
    Dim GetHeader_ShifCode As String = "2"
    Dim GetHeader_Time As String = "3"
    Dim GetGridData As String = "4"
    Dim GetGridData_Activity As String = "5"
    Dim GetCharSetup As String = "6"

    'GET VALIDATION
    Dim GetVerifyPrivilege As String = "1"
    Dim GetVerifyChartSetup As String = "2"

    'SPECIFICATION CHART
    Dim VerifyStatus As String = "0"
    Dim VerifyDesc As String = ""
    Dim DescIndex As String = ""

    'EXCEL PARAMETER
    Dim row_GridTitle = 0
    Dim row_ChartSetup = 0
    Dim row_HeaderResult = 0
    Dim row_HeaderActivity = 0
    Dim row_CellResult = 0
    Dim row_CellChart = 0
    Dim row_CellActivity = 0
    Dim col_HeaderResult = 0
    Dim col_HeaderActivity = 0
    Dim col_CellResult = 0
    Dim col_CellActivity = 0
    Dim RowIndexName As String = ""
    Dim CharacteristicSts As String = ""
    Dim ChartType As String
    Dim LotNo As String = ""

    'FORM LOAD PARAMETER
    Dim menu = ""
    Dim prmFactoryCode = ""
    Dim prmItemType = ""
    Dim prmLineCode = ""
    Dim prmItemCheck = ""
    Dim prmProdDate = ""
    Dim prmShifCode = ""
    Dim prmSeqNo = ""
    Dim prmShowVerify = ""

#End Region

#Region "LOAD FORM"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pUser = Session("user")
        MenuID = "B040"
        Session("LogUserID") = pUser

        ' Authorization User Access
        AuthAccess = sGlobal.Auth_UserAccess(pUser, MenuID)
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If
        sGlobal.getMenu(MenuID)
        Master.SiteTitle = MenuID & " - " & sGlobal.menuName

        ' Authorization Update Privilege
        Dim commandColumn = TryCast(GridActivity.Columns(0), GridViewCommandColumn)
        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, MenuID)
        If AuthUpdate = False Then
            commandColumn.ShowEditButton = False
            commandColumn.ShowNewButtonInHeader = False
            btnVerification.Enabled = False
        End If

        ' Authorization Delete Privilege
        AuthDelete = sGlobal.Auth_UserDelete(pUser, MenuID)
        If AuthDelete = False Then
            commandColumn.ShowDeleteButton = False
            btnVerification.Enabled = False
        End If

        ' Authorization Delete and Update Privilege
        If AuthDelete = False And AuthUpdate = False Then
            commandColumn.Width = 0 'supaya button new/edit/delete muncul
        Else
            commandColumn.Width = 80 'supaya button new/edit/delete tidak muncul
        End If

        If Not Page.IsPostBack Then
            If Request.QueryString("menu") IsNot Nothing Then
                LoadForm_ByAnotherform() 'jika dipanggil dari menu lain
            Else
                commandColumn.Width = 0 'supaya button new/edit/delete tidak muncul
                LoadForm() 'jika dipanggil dari form itu sendiri=
            End If
        End If
    End Sub
#End Region

#Region "EVENT CALLBACK"
    Private Sub cboLineID_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineID.Callback
        Try
            Dim data As New clsProdSampleVerification()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ItemType_Code = e.Parameter.Split("|")(1)
            data.User = pUser

            dt = clsProdSampleVerificationDB.FillCombo(Line_Sel, data)
            With cboLineID
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.Info, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboItemCheck_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemCheck.Callback
        Try
            Dim data As New clsProdSampleVerification()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ItemType_Code = Split(e.Parameter, "|")(1)
            data.LineCode = Split(e.Parameter, "|")(2)
            data.User = pUser

            dt = clsProdSampleVerificationDB.FillCombo(ItemCheck_Sel, data)
            With cboItemCheck
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.Info, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboShift_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboShift.Callback
        Try
            Dim data As New clsProdSampleVerification()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ItemType_Code = Split(e.Parameter, "|")(1)
            data.LineCode = Split(e.Parameter, "|")(2)
            data.ItemCheck_Code = Split(e.Parameter, "|")(3)

            dt = clsProdSampleVerificationDB.FillCombo(Shift_Sel, data)
            With cboShift
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.Info, ex.Message, 0)
        End Try
    End Sub
    Private Sub cboSeq_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboSeq.Callback
        Try
            Dim data As New clsProdSampleVerification()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ItemType_Code = Split(e.Parameter, "|")(1)
            data.LineCode = Split(e.Parameter, "|")(2)
            data.ItemCheck_Code = Split(e.Parameter, "|")(3)
            data.ShiftCode = Split(e.Parameter, "|")(4)

            dt = clsProdSampleVerificationDB.FillCombo(Seq_Sel, data)
            With cboSeq
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.Info, ex.Message, 0)
        End Try
    End Sub
    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Dim cls As New clsProdSampleVerification
        cls.FactoryCode = cboFactory.Value
        cls.FactoryName = cboFactory.Text
        cls.ItemType_Code = cboItemType.Value
        cls.ItemType_Name = cboItemType.Text
        cls.LineCode = cboLineID.Value
        cls.LineName = cboLineID.Text
        cls.ItemCheck_Code = cboItemCheck.Value
        cls.ItemCheck_Name = cboItemCheck.Value
        cls.ProdDate = Convert.ToDateTime(dtProdDate.Value).ToString("yyyy-MM-dd")
        cls.Period = Convert.ToDateTime(dtProdDate.Value).ToString("yyyy MMM dd")
        cls.ShiftCode = cboShift.Value
        cls.ShiftName = cboShift.Text
        cls.Seq = cboSeq.Value
        cls.ShowVerify = cboShow.Value
        cls.ShowVerify_Desc = cboShow.Text

        up_Excel(cls)
    End Sub

#End Region

#Region "GRID X CALLBACK"
    Private Sub Grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles GridX.CustomCallback
        Try
            Dim SpcResultID As String = ""
            Dim msgErr As String = ""
            Dim pAction As String = Split(e.Parameters, "|")(0)

            Dim cls As New clsProdSampleVerification
            cls.FactoryCode = HideValue.Get("FactoryCode")
            cls.ItemType_Code = HideValue.Get("ItemType_Code")
            cls.LineCode = HideValue.Get("LineCode")
            cls.ItemCheck_Code = HideValue.Get("ItemCheck_Code")
            cls.ProdDate = Convert.ToDateTime(HideValue.Get("ProdDate")).ToString("yyyy-MM-dd")
            cls.ShiftCode = HideValue.Get("ShiftCode")
            cls.Seq = HideValue.Get("Seq")
            cls.ShowVerify = HideValue.Get("ShowVerify")
            cls.User = pUser

            If pAction = "Load" Then

                Up_GridLoad(cls)
                Up_GridChartSetup(cls)
                Validation_Verify(cls)
                GetURL(cls)

            ElseIf pAction = "Verify" Then

                Validation_Verify(cls)
                Verify(cls)
                Up_GridLoad(cls)

            ElseIf pAction = "Clear" Then
                Dim data As New clsProdSampleVerification
                Up_GridLoad(data)
                Up_GridChartSetup(data)
            End If

        Catch ex As Exception
            show_errorGrid(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub Grid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles GridX.HtmlDataCellPrepared
        Try
            Dim cs As New clsSPCColor

            If e.DataColumn.FieldName = "nDescIndex" Then
                DescIndex = e.CellValue
            ElseIf e.DataColumn.FieldName = "nDesc" Then
                If e.CellValue = "1" Then
                    e.Cell.BackColor = cs.Color1
                ElseIf e.CellValue = "2" Then
                    e.Cell.BackColor = cs.Color2
                ElseIf e.CellValue = "3" Then
                    e.Cell.BackColor = cs.Color3
                ElseIf e.CellValue = "4" Then
                    e.Cell.BackColor = cs.Color4
                ElseIf e.CellValue = "5" Then
                    e.Cell.BackColor = cs.Color5
                End If
            ElseIf e.DataColumn.FieldName <> "nDesc" Then
                If DescIndex = "EachData" Or DescIndex = "XBar" Or DescIndex = "Judgement" Or DescIndex = "Correction" Or DescIndex = "Verification" Then
                    Dim a = e.CellValue
                    If IsDBNull(a) Then
                        e.Cell.BackColor = Color.White
                    Else
                        Dim val = Split(a, "|")(0)
                        Dim sColor = Split(a, "|")(1)

                        If DescIndex = "Judgement" Then
                            Dim slink = Split(a, "|")(2)
                            e.Cell.Text = ""
                            e.Cell.BackColor = ColorTranslator.FromHtml(sColor)

                            Dim Link As New HyperLink()
                            Link.Text = val
                            Link.ForeColor = Color.Black
                            Link.NavigateUrl = slink
                            Link.Target = "_blank"
                            e.Cell.Controls.Add(Link)
                        Else
                            e.Cell.Text = val
                            e.Cell.BackColor = ColorTranslator.FromHtml(sColor)
                        End If

                    End If

                    'ElseIf DescIndex = "View" Then
                    '   
                    '    e.Cell.ForeColor = Color.Blue
                    '    Dim Link As New HyperLink()
                    '    Link.Text = "View"
                    '    Link.NavigateUrl = e.CellValue
                    '    Link.Target = "_blank"
                    '    e.Cell.Controls.Add(Link)
                End If
            End If

            If DescIndex = "GridNothing" Then
                e.Cell.Text = ""
                e.Cell.BackColor = ColorTranslator.FromHtml("#878787")
                e.Cell.BorderStyle = BorderStyle.None
            End If

        Catch ex As Exception
            Throw New Exception("Error_EditingGrid !" & ex.Message)
        End Try
    End Sub
#End Region

#Region "GRID ACITIVITY MONITORING CALLBACK"
    Private Sub GridActivity_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles GridActivity.CustomCallback
        Try
            Dim msgErr As String = ""
            Dim pAction As String = Split(e.Parameters, "|")(0)

            Dim cls As New clsProdSampleVerification With {
            .FactoryCode = HideValue.Get("FactoryCode"),
            .ItemType_Code = HideValue.Get("ItemType_Code"),
            .LineCode = HideValue.Get("LineCode"),
            .ItemCheck_Code = HideValue.Get("ItemCheck_Code"),
            .ProdDate = Convert.ToDateTime(dtProdDate.Value).ToString("yyyy-MM-dd"),
            .ShiftCode = HideValue.Get("ShiftCode"),
            .Seq = HideValue.Get("Seq"),
            .User = pUser
            }

            If pAction = "Load" Then

                Up_GridLoadActivities(cls)

            ElseIf pAction = "Clear" Then
                Dim data As New clsProdSampleVerification
                Up_GridLoadActivities(data)
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub GridActivity_BeforeGetCallbackResult(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridActivity.BeforeGetCallbackResult
        If GridActivity.IsNewRowEditing Then
            GridActivity.SettingsCommandButton.UpdateButton.Text = "Save"
        End If
    End Sub
    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles GridActivity.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then

            Dim cls As New clsProdSampleVerification With {
            .FactoryCode = cboFactory.Value,
            .ItemType_Code = cboItemType.Value,
            .LineCode = cboLineID.Value,
            .ItemCheck_Code = cboItemCheck.Value,
            .ProdDate = Convert.ToDateTime(dtProdDate.Value).ToString("yyyy-MM-dd"),
            .ShiftCode = cboShift.Value,
            .Seq = cboSeq.Value,
            .User = pUser
            }

            Up_GridLoadActivities(cls)

        End If
    End Sub
#End Region

#Region "CHART CALLBACK"
    Private Sub chartX_CustomCallback(sender As Object, e As CustomCallbackEventArgs) Handles chartX.CustomCallback

        Dim cls As New clsProdSampleVerification
        cls.FactoryCode = HideValue.Get("FactoryCode")
        cls.ItemType_Code = HideValue.Get("ItemType_Code")
        cls.LineCode = HideValue.Get("LineCode")
        cls.ItemCheck_Code = HideValue.Get("ItemCheck_Code")
        cls.ProdDate = Convert.ToDateTime(HideValue.Get("ProdDate")).ToString("yyyy-MM-dd")
        cls.ShiftCode = HideValue.Get("ShiftCode")
        cls.Seq = HideValue.Get("Seq")
        cls.ShowVerify = HideValue.Get("ShowVerify")
        cls.User = pUser

        Dim dt = clsProdSampleVerificationDB.Validation(GetVerifyChartSetup, cls)
        Dim Resp = dt.Rows(0)("response")
        Dim RespDesc = dt.Rows(0)("respDesc")

        If Resp = "1" Then
            LoadChartX(cls)
        Else
            show_errorGrid(MsgTypeEnum.Warning, RespDesc, 1)
        End If
    End Sub
    Private Sub chartR_CustomCallback(sender As Object, e As CustomCallbackEventArgs) Handles chartR.CustomCallback
        Dim cls As New clsProdSampleVerification
        cls.FactoryCode = HideValue.Get("FactoryCode")
        cls.ItemType_Code = HideValue.Get("ItemType_Code")
        cls.LineCode = HideValue.Get("LineCode")
        cls.ItemCheck_Code = HideValue.Get("ItemCheck_Code")
        cls.ProdDate = Convert.ToDateTime(HideValue.Get("ProdDate")).ToString("yyyy-MM-dd")
        cls.ShiftCode = HideValue.Get("ShiftCode")
        cls.Seq = HideValue.Get("Seq")
        cls.ShowVerify = HideValue.Get("ShowVerify")
        cls.User = pUser

        Dim dt = clsProdSampleVerificationDB.Validation(GetVerifyChartSetup, cls)
        Dim Resp = dt.Rows(0)("response")
        Dim RespDesc = dt.Rows(0)("respDesc")

        If Resp = "1" Then
            LoadChartR(cls)
        Else
            show_errorGrid(MsgTypeEnum.Warning, RespDesc, 1)
        End If

    End Sub
    Private Sub chartX_CustomDrawSeries(sender As Object, e As CustomDrawSeriesEventArgs) Handles chartX.CustomDrawSeries
        Dim cs As New clsSPCColor
        Dim s As String = e.Series.Name
        If s = "#1" Then
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Circle
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor1
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color1
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color1
        ElseIf s = "#2" Then
            'CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Diamond
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor2
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color2
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color2
        ElseIf s = "#3" Then
            'CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Triangle
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor3
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color3
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color3
        ElseIf s = "#4" Then
            'CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Square
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor4
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color4
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color4
        ElseIf s = "#5" Then
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.Kind = MarkerKind.Circle
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.BorderColor = cs.BorderColor5
            CType(e.SeriesDrawOptions, PointDrawOptions).Color = cs.Color5
            CType(e.SeriesDrawOptions, PointDrawOptions).Marker.FillStyle.FillMode = FillMode.Solid
            e.LegendDrawOptions.Color = cs.Color5
        End If
    End Sub
#End Region

#Region "INSERT - UPDATE - DELETE ACTIVITY MONITORING CELL EDITOR"
    Private Sub Grid_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles GridActivity.CellEditorInitialize
        If e.Column.FieldName = "FactoryName" Or e.Column.FieldName = "ItemTypeName" Or e.Column.FieldName = "LineName" Or e.Column.FieldName = "ItemCheckName" Or e.Column.FieldName = "ShiftName" Or e.Column.FieldName = "[IC" Then
            e.Editor.ReadOnly = True
            e.Editor.ForeColor = Color.Silver
        End If

        If GridActivity.IsNewRowEditing Then
            If e.Column.FieldName = "FactoryCode" Then
                e.Editor.Value = cboFactory.Value
            ElseIf e.Column.FieldName = "FactoryName" Then
                e.Editor.Value = cboFactory.Text
            ElseIf e.Column.FieldName = "ItemTypeCode" Then
                e.Editor.Value = cboItemType.Value
            ElseIf e.Column.FieldName = "ItemTypeName" Then
                e.Editor.Value = cboItemType.Text
            ElseIf e.Column.FieldName = "LineCode" Then
                e.Editor.Value = cboLineID.Value
            ElseIf e.Column.FieldName = "LineName" Then
                e.Editor.Value = cboLineID.Text
            ElseIf e.Column.FieldName = "ItemCheckCode" Then
                e.Editor.Value = cboItemCheck.Value
            ElseIf e.Column.FieldName = "ItemCheckName" Then
                e.Editor.Value = cboItemCheck.Text
            ElseIf e.Column.FieldName = "ShiftCode" Then
                e.Editor.Value = cboShift.Value
            ElseIf e.Column.FieldName = "ShiftName" Then
                e.Editor.Value = cboShift.Text
            ElseIf e.Column.FieldName = "ProdDate" Then
                e.Editor.Value = dtProdDate.Value
            End If
        ElseIf Not GridActivity.IsNewRowEditing Then
            If e.Column.FieldName = "ProdDate" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
            End If
        End If
        If e.Column.FieldName = "PIC" Then
            Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            If GridActivity.IsEditing Then Call up_FillcomboGrid(combo, HideValue.Get("LineCode")) : combo.Value = e.Value

        End If
    End Sub

    Private Sub up_FillcomboGrid(ByVal cmb As ASPxComboBox, Line As String)
        Dim data As New clsProdSampleVerification()
        data.LineCode = Line
        dt = clsProdSampleVerificationDB.FillCombo(PIC_Sel, data)

        With cmb
            .Items.Clear() : .Columns.Clear()
            .DataSource = dt
            .DataBind()
            .SelectedIndex = -1
        End With
    End Sub

    Protected Sub GridActivity_Validating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles GridActivity.RowValidating
        Dim dataCol As New GridViewDataColumn
        For Each column As GridViewColumn In GridActivity.Columns
            Dim dataColumn As GridViewDataColumn = TryCast(column, GridViewDataColumn)
            If dataColumn Is Nothing Then
                Continue For
            End If

            If dataColumn.FieldName = "ProdDate" Then
                If IsNothing(e.NewValues("ProdDate")) OrElse e.NewValues("ProdDate").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Fill Production Date!"
                End If
            End If

            If dataColumn.FieldName = "Time" Then
                If IsNothing(e.NewValues("Time")) OrElse e.NewValues("Time").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Fill Time!"
                End If
            End If

            If dataColumn.FieldName = "PIC" Then
                If IsNothing(e.NewValues("PIC")) OrElse e.NewValues("PIC").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Fill PIC!"
                End If
            End If

            If dataColumn.FieldName = "Action" Then
                If IsNothing(e.NewValues("Action")) OrElse e.NewValues("Action").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Fill Action!"
                End If
            End If

            If dataColumn.FieldName = "Result" Then
                If IsNothing(e.NewValues("Result")) OrElse e.NewValues("Result").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Fill Result!"
                End If
            End If
        Next

    End Sub
    Protected Sub GridActivity_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles GridActivity.RowInserting
        e.Cancel = True

        Dim data As New clsProdSampleVerification With {
            .FactoryCode = e.NewValues("FactoryCode") & "",
            .ItemType_Code = e.NewValues("ItemTypeCode") & "",
            .LineCode = e.NewValues("LineCode") & "",
            .ItemCheck_Code = e.NewValues("ItemCheckCode") & "",
            .ProdDate = Convert.ToDateTime(e.NewValues("ProdDate")).ToString("yyyy-MM-dd"),
            .Time = Convert.ToDateTime(e.NewValues("Time")).ToString("HH:mm"),
            .ShiftCode = e.NewValues("ShiftCode") & "",
            .Action = e.NewValues("Action") & "",
            .PIC = e.NewValues("PIC") & "",
            .Result = e.NewValues("Result") & "",
            .Remark = e.NewValues("Remark") & "",
            .User = pUser}
        Try
            Dim Msg = clsProdSampleVerificationDB.Activity_Insert("CREATE", data)
            If Msg = "" Then
                show_error(MsgTypeEnum.Success, "Save data successfully!", 1)
                GridActivity.CancelEdit()
                Up_GridLoadActivities(data)
                Return
            Else
                show_error(MsgTypeEnum.Warning, Msg, 1)
                GridActivity.CancelEdit()
                Up_GridLoadActivities(data)
            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub GridActivity_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles GridActivity.RowUpdating
        e.Cancel = True
        Dim data As New clsProdSampleVerification With {
            .FactoryCode = e.NewValues("FactoryCode") & "",
            .ItemType_Code = e.NewValues("ItemTypeCode") & "",
            .LineCode = e.NewValues("LineCode") & "",
            .ItemCheck_Code = e.NewValues("ItemCheckCode") & "",
            .ProdDate = Convert.ToDateTime(e.NewValues("ProdDate")).ToString("yyyy-MM-dd"),
            .Time = Convert.ToDateTime(e.NewValues("Time")).ToString("HH:mm"),
            .ShiftCode = e.NewValues("ShiftCode") & "",
            .Action = e.NewValues("Action") & "",
            .PIC = e.NewValues("PIC") & "",
            .ActivityID = e.NewValues("ActivityID") & "",
            .Result = e.NewValues("Result") & "",
            .Remark = e.NewValues("Remark") & "",
            .User = pUser}
        Try
            Dim Msg = clsProdSampleVerificationDB.Activity_Insert("UPDATE", data)
            If Msg = "" Then
                show_error(MsgTypeEnum.Success, "Update data successfully!", 1)
                GridActivity.CancelEdit()
                Up_GridLoadActivities(data)
                Return
            Else
                show_error(MsgTypeEnum.Warning, Msg, 1)
                GridActivity.CancelEdit()
                Up_GridLoadActivities(data)
            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub GridActivity_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles GridActivity.RowDeleting
        e.Cancel = True

        Dim data As New clsProdSampleVerification With {
            .ActivityID = e.Values("ActivityID"),
            .FactoryCode = HideValue.Get("FactoryCode"),
            .ItemType_Code = HideValue.Get("ItemType_Code"),
            .LineCode = HideValue.Get("LineCode"),
            .ItemCheck_Code = HideValue.Get("ItemCheck_Code"),
            .ProdDate = Convert.ToDateTime(dtProdDate.Value).ToString("yyyy-MM-dd"),
            .ShiftCode = HideValue.Get("ShiftCode"),
            .Seq = HideValue.Get("Seq")
        }
        Try
            Dim Msg = clsProdSampleVerificationDB.Activity_Insert("DELETE", data)
            If Msg = "" Then
                show_error(MsgTypeEnum.Success, "Delete data successfully!", 1)
                GridActivity.CancelEdit()
                Up_GridLoadActivities(data)
            Else
                show_error(MsgTypeEnum.Warning, Msg, 1)
                GridActivity.CancelEdit()
                Up_GridLoadActivities(data)
            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
#End Region

#Region "FUNCTION"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        GridActivity.JSProperties("cp_message") = ErrMsg
        GridActivity.JSProperties("cp_type") = msgType
        GridActivity.JSProperties("cp_val") = pVal
    End Sub
    Private Sub show_errorGrid(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        GridX.JSProperties("cp_message") = ErrMsg
        GridX.JSProperties("cp_type") = msgType
        GridX.JSProperties("cp_val") = pVal
    End Sub
    Private Function AFormat(v As Object) As String
        If v Is Nothing OrElse IsDBNull(v) Then
            Return ""
        Else
            Return Format(v, "0.000")
        End If
    End Function
    Private Sub UpFillCombo()
        Try
            Dim data As New clsProdSampleVerification()
            data.User = pUser
            Dim a As String

            '============ FILL COMBO FACTORY CODE ================'
            dt = clsProdSampleVerificationDB.FillCombo(Factory_Sel, data)
            With cboFactory
                .DataSource = dt
                .DataBind()
            End With
            If prmFactoryCode <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = prmFactoryCode Then
                        cboFactory.SelectedIndex = i
                        Exit For
                    End If
                Next
                cboFactory.Enabled = False
            Else
                cboFactory.SelectedIndex = 0
            End If
            If cboFactory.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboFactory.SelectedItem.GetFieldValue("CODE")
            End If
            HideValue.Set("FactoryCode", a)
            data.FactoryCode = HideValue.Get("FactoryCode")
            '======================================================'

            '============== FILL COMBO ITEM TYPE =================='
            dt = clsProdSampleVerificationDB.FillCombo(ItemType_Sel, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With
            If prmItemType <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = prmItemType Then
                        cboItemType.SelectedIndex = i
                        Exit For
                    End If
                Next
                cboItemType.Enabled = False
            End If

            Dim ItemTypeDesc = ""
            If cboItemType.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboItemType.SelectedItem.GetFieldValue("CODE")
                ItemTypeDesc = cboItemType.SelectedItem.GetDataItem("CODENAME")
            End If
            HideValue.Set("ItemType_Code", a)
            HideValue.Set("ItemTypeDesc", ItemTypeDesc)
            data.ItemType_Code = HideValue.Get("ItemType_Code")
            '======================================================'

            '============== FILL COMBO LINE CODE =================='
            dt = clsProdSampleVerificationDB.FillCombo(Line_Sel, data)
            With cboLineID
                .DataSource = dt
                .DataBind()
            End With
            If prmLineCode <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = prmLineCode Then
                        cboLineID.SelectedIndex = i
                        Exit For
                    End If
                Next
                cboLineID.Enabled = False
            End If
            If cboLineID.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboLineID.SelectedItem.GetFieldValue("CODE")
            End If
            HideValue.Set("LineCode", a)
            data.LineCode = HideValue.Get("LineCode")
            '======================================================'


            '============== FILL COMBO ITEM CHECK =================='
            dt = clsProdSampleVerificationDB.FillCombo(ItemCheck_Sel, data)
            With cboItemCheck
                .DataSource = dt
                .DataBind()
            End With
            If prmItemCheck <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = prmItemCheck Then
                        cboItemCheck.SelectedIndex = i
                        Exit For
                    End If
                Next
                cboItemCheck.Enabled = False
            End If
            If cboItemCheck.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboItemCheck.SelectedItem.GetFieldValue("CODE")
            End If
            HideValue.Set("ItemCheck_Code", a)
            data.ItemCheck_Code = HideValue.Get("ItemCheck_Code")
            '======================================================'

            '============== FILL COMBO SHIFY =================='
            dt = clsProdSampleVerificationDB.FillCombo(Shift_Sel, data)
            With cboShift
                .DataSource = dt
                .DataBind()
            End With
            If prmShifCode <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = prmShifCode Then
                        cboShift.SelectedIndex = i
                        Exit For
                    End If
                Next
                cboShift.Enabled = False
            End If
            If cboShift.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboShift.SelectedItem.GetFieldValue("CODE")
            End If
            HideValue.Set("ShiftCode", a)
            data.ShiftCode = HideValue.Get("ShiftCode")
            '======================================================'

            '============== FILL COMBO SEQ =================='
            dt = clsProdSampleVerificationDB.FillCombo(Seq_Sel, data)
            With cboSeq
                .DataSource = dt
                .DataBind()
            End With
            If prmSeqNo <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = prmSeqNo Then
                        cboSeq.SelectedIndex = i
                        Exit For
                    End If
                Next
                cboSeq.Enabled = False
            End If
            If cboSeq.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboSeq.SelectedItem.GetFieldValue("CODE")
            End If
            HideValue.Set("Seq", a)
            data.Seq = HideValue.Get("Seq")
            '======================================================'

            '============== FILL COMBO SEQ =================='

            If prmShowVerify <> "" Then
                If prmShowVerify = "0" Then
                    cboShow.SelectedIndex = 0
                Else
                    cboShow.SelectedIndex = 1
                End If
                'cboShow.Enabled = False
                HideValue.Set("ShowVerify", prmShowVerify)
                data.Seq = HideValue.Get("ShowVerify")
            End If
            '======================================================'

        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "", 0)
        End Try
    End Sub
    Private Sub Up_GridLoad(cls As clsProdSampleVerification)
        Dim msgErr As String = ""

        With GridX
            .Columns.Clear()
            Dim ColDescIndex As New GridViewDataTextColumn
            ColDescIndex.FieldName = "nDescIndex"
            ColDescIndex.Width = 0
            ColDescIndex.CellStyle.HorizontalAlign = HorizontalAlign.Center
            .Columns.Add(ColDescIndex)

            Dim Band1 As New GridViewBandColumn
            Band1.Caption = "Date"
            .Columns.Add(Band1)

            Dim Band2 As New GridViewBandColumn
            Band2.Caption = "Shift"
            Band1.Columns.Add(Band2)

            Dim ColDesc As New GridViewDataTextColumn
            ColDesc.FieldName = "nDesc"
            ColDesc.Caption = "Time"
            ColDesc.Width = 80
            ColDesc.CellStyle.HorizontalAlign = HorizontalAlign.Center
            Band2.Columns.Add(ColDesc)

            ds = clsProdSampleVerificationDB.GridLoad(GetHeader_ProdDate, cls)
            Dim dtDate As DataTable = ds.Tables(0)
            If dtDate.Rows.Count > 0 Then
                For i = 0 To dtDate.Rows.Count - 1
                    Dim Col_ProdDate As New GridViewBandColumn
                    Dim nProdDate = dtDate.Rows(i)("ProdDate")
                    Col_ProdDate.Caption = nProdDate
                    .Columns.Add(Col_ProdDate)

                    cls.ProdDate_Grid = Convert.ToDateTime(nProdDate).ToString("yyyy-MM-dd")
                    ds = clsProdSampleVerificationDB.GridLoad(GetHeader_ShifCode, cls)
                    Dim dtShift As DataTable = ds.Tables(0)
                    If dtShift.Rows.Count > 0 Then
                        For n = 0 To dtShift.Rows.Count - 1

                            Dim Col_Shift As New GridViewBandColumn
                            Dim nShiftCode = dtShift.Rows(n)("ShiftCode")
                            'If nShiftCode = "SH001" Then
                            '    nShiftCode = "Shift 1"
                            'ElseIf nShiftCode = "SH002" Then
                            '    nShiftCode = "Shift 2"
                            'End If

                            Col_Shift.Caption = nShiftCode
                            Col_ProdDate.Columns.Add(Col_Shift)

                            cls.Shiftcode_Grid = dtShift.Rows(n)("ShiftCode")
                            ds = clsProdSampleVerificationDB.GridLoad(GetHeader_Time, cls)
                            Dim dtSeq As DataTable = ds.Tables(0)
                            If dtSeq.Rows.Count > 0 Then
                                For r = 0 To dtSeq.Rows.Count - 1
                                    Dim Col_Seq As New GridViewDataTextColumn
                                    Col_Seq.Width = 100
                                    Col_Seq.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                                    Col_Seq.CellStyle.HorizontalAlign = HorizontalAlign.Center
                                    Col_Seq.FieldName = dtSeq.Rows(r)("nTime")
                                    Col_Seq.Caption = dtSeq.Rows(r)("nTimeDesc")
                                    Col_Shift.Columns.Add(Col_Seq)
                                Next
                            End If
                        Next
                    End If
                Next
            Else
                show_errorGrid(MsgTypeEnum.Warning, "Data Not Found", 1)
            End If

            ds = clsProdSampleVerificationDB.GridLoad(GetGridData, cls)
            Dim dtGrid As DataTable = ds.Tables(0)
            If dtGrid.Rows.Count > 0 Then
                .KeyFieldName = "nDesc"
                .DataSource = dtGrid
                .DataBind()
                .Styles.CommandColumn.BackColor = Color.White
                .Styles.CommandColumn.ForeColor = Color.Black
            End If
            GridX.JSProperties("cp_GridTot") = dtGrid.Rows.Count
        End With
    End Sub
    Private Sub Up_GridChartSetup(cls As clsProdSampleVerification)
        ds = clsProdSampleVerificationDB.GridLoad(GetCharSetup, cls)
        Dim dtChartSetup As DataTable = ds.Tables(0)
        If dtChartSetup.Rows.Count > 0 Then
            GridX.JSProperties("cpUSL") = AFormat(dtChartSetup.Rows(0)("USL"))
            GridX.JSProperties("cpLSL") = AFormat(dtChartSetup.Rows(0)("LSL"))
            GridX.JSProperties("cpUCL") = AFormat(dtChartSetup.Rows(0)("UCL"))
            GridX.JSProperties("cpLCL") = AFormat(dtChartSetup.Rows(0)("LCL"))
            GridX.JSProperties("cpXBarUCL") = AFormat(dtChartSetup.Rows(0)("XBarUCL"))
            GridX.JSProperties("cpXBarLCL") = AFormat(dtChartSetup.Rows(0)("XBarUCL"))
            GridX.JSProperties("cpRUCL") = AFormat(dtChartSetup.Rows(0)("RUCL"))
            GridX.JSProperties("cpRLCL") = AFormat(dtChartSetup.Rows(0)("RLCL"))
            GridX.JSProperties("cpMIN") = AFormat(dtChartSetup.Rows(0)("nMIN"))
            GridX.JSProperties("cpMAX") = AFormat(dtChartSetup.Rows(0)("nMAX"))
            GridX.JSProperties("cpAVG") = AFormat(dtChartSetup.Rows(0)("nAVG"))
            GridX.JSProperties("cpR") = AFormat(dtChartSetup.Rows(0)("nR"))
            GridX.JSProperties("cpC") = dtChartSetup.Rows(0)("C").ToString
            GridX.JSProperties("cpNG") = dtChartSetup.Rows(0)("NG").ToString
            GridX.JSProperties("cpCS") = dtChartSetup.Rows(0)("CS").ToString

            GridX.JSProperties("cpMINClr") = dtChartSetup.Rows(0)("nMINClr")
            GridX.JSProperties("cpMAXClr") = dtChartSetup.Rows(0)("nMAXClr")
            GridX.JSProperties("cpAVGClr") = dtChartSetup.Rows(0)("nAVGClr")
            GridX.JSProperties("cpRClr") = dtChartSetup.Rows(0)("nRClr")
            GridX.JSProperties("cpC_Clr") = dtChartSetup.Rows(0)("C_Clr")
            GridX.JSProperties("cpNG_Clr") = dtChartSetup.Rows(0)("NG_Clr")
        End If
    End Sub
    Private Sub Up_GridLoadActivities(cls As clsProdSampleVerification)
        With GridActivity

            ds = clsProdSampleVerificationDB.GridLoad(GetGridData_Activity, cls)
            Dim dtGridActivity As DataTable = ds.Tables(0)
            .DataSource = dtGridActivity
            .DataBind()
        End With
    End Sub
    Private Sub LoadChartR(cls As clsProdSampleVerification)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartR(cls.FactoryCode, cls.ItemType_Code, cls.LineCode, cls.ItemCheck_Code, cls.ProdDate, "", cls.ShowVerify, cls.Seq)
        If xr.Count = 0 Then
            chartR.JSProperties("cpShow") = "0"
            CharacteristicSts = "0"
        Else
            chartR.JSProperties("cpShow") = "1"
            CharacteristicSts = "1"
        End If
        With chartR
            .DataSource = xr
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            diagram.AxisX.WholeRange.MinValue = 0
            diagram.AxisX.WholeRange.MaxValue = 12

            diagram.AxisX.GridLines.LineStyle.DashStyle = DashStyle.Solid
            diagram.AxisX.GridLines.MinorVisible = True
            diagram.AxisX.MinorCount = 1
            diagram.AxisX.GridLines.Visible = False

            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(cls.FactoryCode, cls.ItemType_Code, cls.LineCode, cls.ItemCheck_Code, cls.ProdDate)
            diagram.AxisY.ConstantLines.Clear()
            If Setup IsNot Nothing Then
                Dim RCL As New ConstantLine("CL R")
                RCL.Color = System.Drawing.Color.Red
                RCL.LineStyle.Thickness = 1
                RCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(RCL)
                RCL.AxisValue = Setup.RCL

                Dim RUCL As New ConstantLine("UCL R")
                RUCL.Color = System.Drawing.Color.Red
                RUCL.LineStyle.Thickness = 1
                RUCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(RUCL)
                RUCL.AxisValue = Setup.RUCL

                CType(.Diagram, XYDiagram).SecondaryAxesY.Clear()
                Dim myAxisY As New SecondaryAxisY("my Y-Axis")
                myAxisY.Visibility = DevExpress.Utils.DefaultBoolean.False
                myAxisY.WholeRange.EndSideMargin = 0
                myAxisY.WholeRange.StartSideMargin = 0
                CType(.Diagram, XYDiagram).SecondaryAxesY.Add(myAxisY)
                CType(.Series("RuleYellow").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                CType(.Series("RuleRed").View, XYDiagramSeriesViewBase).AxisY = myAxisY

                Dim MaxValue As Double
                If xr.Count > 0 Then
                    If xr(0).MaxValue > Setup.RUCL Then
                        MaxValue = xr(0).MaxValue
                    Else
                        MaxValue = Setup.RUCL
                    End If
                End If
                diagram.AxisY.WholeRange.MaxValue = MaxValue
                diagram.AxisY.VisualRange.MaxValue = MaxValue
                If MaxValue > 0 Then
                    Dim GridAlignment As Double = Math.Round(MaxValue / 20, 4)
                    diagram.AxisY.NumericScaleOptions.CustomGridAlignment = GridAlignment
                End If
            End If
            .DataBind()
        End With
    End Sub
    Private Sub LoadChartX(cls As clsProdSampleVerification)
        ChartType = clsXRChartDB.GetChartType(cls.FactoryCode, cls.ItemType_Code, cls.LineCode, cls.ItemCheck_Code)
        Dim xr As List(Of clsXRChart) = clsXRChartDB.GetChartXR(cls.FactoryCode, cls.ItemType_Code, cls.LineCode, cls.ItemCheck_Code, cls.ProdDate, cls.ShowVerify, cls.Seq)
        With chartX
            .DataSource = xr
            Dim diagram As XYDiagram = CType(.Diagram, XYDiagram)
            diagram.AxisX.WholeRange.MinValue = 0
            diagram.AxisX.WholeRange.MaxValue = 12

            diagram.AxisX.GridLines.LineStyle.DashStyle = DashStyle.Solid
            diagram.AxisX.GridLines.MinorVisible = True
            diagram.AxisX.MinorCount = 1
            diagram.AxisX.GridLines.Visible = False

            diagram.AxisY.NumericScaleOptions.CustomGridAlignment = 0.005
            diagram.AxisY.GridLines.MinorVisible = False
            If ChartType = "1" Or ChartType = "2" Then
                .Titles(0).Text = "X Bar Control Chart"
            Else
                .Titles(0).Text = "Graph Monitoring"
            End If


            Dim Setup As clsChartSetup = clsChartSetupDB.GetData(cls.FactoryCode, cls.ItemType_Code, cls.LineCode, cls.ItemCheck_Code, cls.ProdDate)
            diagram.AxisY.ConstantLines.Clear()
            If Setup IsNot Nothing Then
                Dim LCL As New ConstantLine("LCL")
                LCL.Color = System.Drawing.Color.Red
                LCL.LineStyle.Thickness = 1
                LCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(LCL)
                LCL.AxisValue = Setup.CPLCL

                Dim UCL As New ConstantLine("UCL")
                UCL.Color = System.Drawing.Color.Red
                UCL.LineStyle.Thickness = 1
                UCL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(UCL)
                UCL.AxisValue = Setup.CPUCL

                Dim CL As New ConstantLine("CL")
                CL.Color = System.Drawing.Color.Red
                CL.LineStyle.Thickness = 1
                CL.LineStyle.DashStyle = DashStyle.Dash
                diagram.AxisY.ConstantLines.Add(CL)
                CL.AxisValue = Setup.CPCL

                If ChartType = "1" Or ChartType = "2" Then
                    Dim XBarLCL As New ConstantLine("XBarLCL")
                    XBarLCL.Color = System.Drawing.Color.Yellow
                    XBarLCL.LineStyle.Thickness = 1
                    XBarLCL.LineStyle.DashStyle = DashStyle.Dash
                    diagram.AxisY.ConstantLines.Add(XBarLCL)
                    XBarLCL.AxisValue = Setup.XBarLCL

                    Dim XBarUCL As New ConstantLine("XBarUCL")
                    XBarUCL.Color = System.Drawing.Color.Yellow
                    XBarUCL.LineStyle.Thickness = 1
                    XBarUCL.LineStyle.DashStyle = DashStyle.Dash
                    diagram.AxisY.ConstantLines.Add(XBarUCL)
                    XBarUCL.AxisValue = Setup.XBarUCL
                End If

                Dim LSL As New ConstantLine("LSL")
                LSL.Color = System.Drawing.Color.Red
                LSL.LineStyle.Thickness = 1
                LSL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(LSL)
                LSL.AxisValue = Setup.SpecLSL

                Dim USL As New ConstantLine("USL")
                USL.Color = System.Drawing.Color.Red
                USL.LineStyle.Thickness = 1
                USL.LineStyle.DashStyle = DashStyle.Solid
                diagram.AxisY.ConstantLines.Add(USL)
                USL.AxisValue = Setup.SpecUSL

                Dim MinValue As Double, MaxValue As Double
                If xr.Count > 0 Then
                    MinValue = xr(0).MinValue
                    MaxValue = xr(0).MaxValue
                End If
                If Setup.SpecLSL < MinValue Then
                    MinValue = Setup.SpecLSL
                End If
                If Setup.SpecUSL > MaxValue Then
                    MaxValue = Setup.SpecUSL
                End If
                Dim EndSideMargin As Single = Math.Round((MaxValue - MinValue) / 20, 3)

                diagram.AxisY.WholeRange.MinValue = MinValue
                diagram.AxisY.WholeRange.MaxValue = MaxValue
                diagram.AxisY.WholeRange.EndSideMargin = EndSideMargin

                diagram.AxisY.VisualRange.MinValue = MinValue
                diagram.AxisY.VisualRange.MaxValue = MaxValue
                diagram.AxisY.VisualRange.EndSideMargin = EndSideMargin

                Dim diff As Double = MaxValue - MinValue
                If diff > 0 Then
                    Dim gridAlignment As Double = Math.Round(diff / 15, 3)
                    diagram.AxisY.NumericScaleOptions.CustomGridAlignment = gridAlignment
                End If

                CType(.Diagram, XYDiagram).SecondaryAxesY.Clear()
                Dim myAxisY As New SecondaryAxisY("my Y-Axis")
                myAxisY.Visibility = DevExpress.Utils.DefaultBoolean.False
                myAxisY.WholeRange.EndSideMargin = 0
                myAxisY.WholeRange.StartSideMargin = 0
                CType(.Diagram, XYDiagram).SecondaryAxesY.Add(myAxisY)
                CType(.Series("Rule").View, XYDiagramSeriesViewBase).AxisY = myAxisY
                CType(.Series("RuleYellow").View, XYDiagramSeriesViewBase).AxisY = myAxisY
            End If
            .DataBind()
            'If xr.Count > 5 Then
            '    .Width = xr.Count * 20
            'End If
        End With
    End Sub
    Private Sub LoadForm_ByAnotherform()

        prmFactoryCode = Request.QueryString("FactoryCode")
        prmItemType = Request.QueryString("ItemTypeCode")
        prmLineCode = Request.QueryString("Line")
        prmItemCheck = Request.QueryString("ItemCheckCode")
        prmProdDate = Request.QueryString("ProdDate")
        prmShifCode = Request.QueryString("Shift")
        prmSeqNo = Request.QueryString("Sequence")
        prmShowVerify = Request.QueryString("ShowVerify")

        Dim cls As New clsProdSampleVerification
        cls.FactoryCode = prmFactoryCode
        cls.ItemType_Code = prmItemType
        cls.LineCode = prmLineCode
        cls.ItemCheck_Code = prmItemCheck
        cls.ProdDate = Convert.ToDateTime(prmProdDate).ToString("yyyy-MM-dd")
        cls.ShiftCode = prmShifCode
        cls.Seq = prmSeqNo
        cls.ShowVerify = prmShowVerify
        cls.User = pUser

        UpFillCombo()
        Validation_Verify(cls)
        GetURL(cls)
        Up_GridLoad(cls)
        Up_GridLoadActivities(cls)
        Up_GridChartSetup(cls)

        Dim dt = clsProdSampleVerificationDB.Validation(GetVerifyChartSetup, cls)
        Dim Resp = dt.Rows(0)("response")
        Dim RespDesc = dt.Rows(0)("respDesc")

        If Resp = "1" Then
            LoadChartX(cls)
            LoadChartR(cls)
        Else
            show_errorGrid(MsgTypeEnum.Warning, RespDesc, 1)
        End If

        dtProdDate.Enabled = False
        btnClear.Enabled = False
        btnBack.Visible = False
        dtProdDate.Value = Convert.ToDateTime(prmProdDate)
        HideValue.Set("ProdDate", prmProdDate)

        If Request.QueryString("menu") = "ProductionSampleVerificationList.aspx" Then
            HideValue.Set("prm_factory", prmFactoryCode)
            HideValue.Set("prm_ItemType", prmItemType)
            HideValue.Set("prm_Line", Request.QueryString("cboLine"))
            HideValue.Set("prm_ItemCheck", Request.QueryString("cboItemCheck"))
            HideValue.Set("prm_FromDate", Request.QueryString("FromDate"))
            HideValue.Set("prm_ToDate", Request.QueryString("ToDate"))
            HideValue.Set("prm_MK", Request.QueryString("MK"))
            HideValue.Set("prm_QC", Request.QueryString("QC"))
            btnBack.Visible = True
        End If

    End Sub
    Private Sub LoadForm()
        UpFillCombo()

        Dim ToDay = DateTime.Now
        dtProdDate.Value = ToDay
        HideValue.Set("ProdDate", ToDay.ToString("dd MMM yyyy"))
        HideValue.Set("ShowVerify", cboShow.Value)
        btnBack.Visible = False

        GridX.JSProperties("cp_GridTot") = 0  'for disabled button Verify and Download Excel
        GridX.JSProperties("cp_Verify") = VerifyStatus 'for authorization verify
        GridX.JSProperties("cpChartSetup") = 0
    End Sub
    Private Sub Verify(cls As clsProdSampleVerification)
        Try
            Dim Verify = clsProdSampleVerificationDB.Verify(cls)
            If Verify = "" Then
                show_errorGrid(MsgTypeEnum.Success, "Verify data successfully!", 1)
                GridX.JSProperties("cp_Verify") = "0" 'Status Verify Success
                Return
            Else
                show_errorGrid(MsgTypeEnum.Warning, Verify, 1)
                Return
            End If
        Catch ex As Exception
            show_errorGrid(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub Validation_Verify(cls As clsProdSampleVerification)
        pEmplooyeeID = clsIOT.GetEmployeeID(pUser)
        Dim AllowSkill As Boolean = clsIOT.AllowSkill(pEmplooyeeID, cls.FactoryCode, cls.LineCode, cls.ItemType_Code)
        GridX.JSProperties("cp_AllowSkill") = AllowSkill 'parameter to authorization verify
        If AllowSkill = False Then
            show_errorGrid(MsgTypeEnum.Warning, "Can not verify the data, Please check allow skill map!", 1)
        End If

        Dim dt = clsProdSampleVerificationDB.Validation(GetVerifyPrivilege, cls)
        VerifyStatus = dt.Rows(0)("Response").ToString
        VerifyDesc = dt.Rows(0)("RespDesc").ToString

        GridX.JSProperties("cp_Verify") = VerifyStatus  'parameter to authorization verify
        If VerifyStatus = "0" Then
            show_errorGrid(MsgTypeEnum.Warning, VerifyDesc, 1)
        End If
    End Sub
    Private Sub GetURL(cls As clsProdSampleVerification)
        Dim ProcessGroup = ""
        Dim LineGroup = ""
        Dim ProcessCode = ""
        Dim ItemCode = ""
        Dim ItemCode_Traceability = ""
        Dim LotNo = ""
        Dim ProcessTableLineCode = ""

        Dim URL = clsIOT.GetURL(pUser)
        Dim ds = clsProdSampleVerificationDB.GridLoad(GetCharSetup, cls)
        Dim dt As DataTable = ds.Tables(0)
        If dt.Rows.Count > 0 Then
            LotNo = dt.Rows(0)("SubLotNo").ToString
            ProcessTableLineCode = dt.Rows(0)("ProcessTableLineCode").ToString
        End If

        Dim dtIOT_ProcessTable = clsIOT.GetIOT_ProcessTable(cls.FactoryCode, ProcessTableLineCode, cls.ItemType_Code, cls.ProdDate, cls.ShiftCode)
        If dtIOT_ProcessTable.Rows.Count > 0 Then
            ProcessGroup = dtIOT_ProcessTable.Rows(0)("ProcessGroup").ToString.Trim
            LineGroup = dtIOT_ProcessTable.Rows(0)("LineGroup").ToString.Trim
            ProcessCode = dtIOT_ProcessTable.Rows(0)("ProcessCode").ToString.Trim
            ItemCode = dtIOT_ProcessTable.Rows(0)("ItemCode").ToString.Trim
        End If

        Dim dtIOT_Traceability = clsIOT.GetIOT_Traceability(cls.FactoryCode, cls.LineCode, cls.ItemType_Code, LotNo, cls.ProdDate, cls.ShiftCode)
        If dtIOT_Traceability.Rows.Count > 0 Then
            ItemCode_Traceability = dtIOT_Traceability.Rows(0)("ItemCode").ToString.Trim
        End If

        GridX.JSProperties("cp_URL") = If(URL = "", "-", URL)
        GridX.JSProperties("cp_LotNo") = If(LotNo = "", "-", LotNo)
        GridX.JSProperties("cp_ProcessTableLineCode") = If(ProcessTableLineCode = "", "-", ProcessTableLineCode)
        GridX.JSProperties("cp_ProcessGroup") = If(ProcessGroup = "", "-", ProcessGroup)
        GridX.JSProperties("cp_LineGroup") = If(LineGroup = "", "-", LineGroup)
        GridX.JSProperties("cp_ProcessCode") = If(ProcessCode = "", "-", ProcessCode)
        GridX.JSProperties("cp_ItemCode") = If(ItemCode = "", "-", ItemCode)
        GridX.JSProperties("cp_ItemCode_Traceability") = If(ItemCode_Traceability = "", "-", ItemCode_Traceability)

    End Sub
#End Region

#Region "DOWNLOAD EXCEl"
    Private Sub up_Excel(cls As clsProdSampleVerification)
        Try
            Dim ps As New PrintingSystem()

            LoadChartR(cls)
            Dim linkR As New PrintableComponentLink(ps)
            linkR.Component = (CType(chartR, IChartContainer)).Chart

            LoadChartX(cls)
            Dim linkX As New PrintableComponentLink(ps)
            linkX.Component = (CType(chartX, IChartContainer)).Chart

            Dim compositeLink As New CompositeLink(ps)
            If CharacteristicSts = "1" Then
                compositeLink.Links.AddRange(New Object() {linkX, linkR})
            Else
                compositeLink.Links.AddRange(New Object() {linkX})
            End If

            compositeLink.CreateDocument()
            Dim Path As String = Server.MapPath("Download")
            Dim streamImg As New MemoryStream
            compositeLink.ExportToImage(streamImg)

            Using excel As New ExcelPackage
                Dim ws As ExcelWorksheet
                ws = excel.Workbook.Worksheets.Add("BO4 - Prod Sample Verifiaction")

                With ws
                    GridTitle(ws, cls)
                    'ADD Chart Setup
                    SpecGrid(ws, cls)

                    'ADD GRID RESULT
                    HeaderResult(ws, cls)
                    CellResult(ws, cls)

                    ' ADD CHART
                    If CharacteristicSts = "1" Then
                        row_CellChart = row_CellResult + 40
                    Else
                        row_CellChart = row_CellResult + 25
                    End If
                    .InsertRow(row_CellResult + 2, row_CellChart)
                    Dim fi As New FileInfo(Path & "\chart.png")
                    Dim Picture As OfficeOpenXml.Drawing.ExcelPicture
                    Picture = .Drawings.AddPicture("chart", Image.FromStream(streamImg))
                    Picture.SetPosition(row_CellResult + 2, 0, 0, 0)

                    ' ADD GRID ACTIVITY
                    HeaderActivity(ws, cls)
                    CellActivity(ws, cls)

                End With

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment; filename=Production Sample Verification_" & Format(Date.Now, "yyyy-MM-dd_HHmmss") & ".xlsx")
                Using MyMemoryStream As New MemoryStream()
                    excel.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.End()
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Private Sub GridTitle(ByVal pExl As ExcelWorksheet, cls As clsProdSampleVerification)
        With pExl
            Try
                Dim irow = 1

                .Cells(irow, 1).Value = "Production Sample Verification"
                .Cells(irow, 1, irow, 13).Merge = True
                .Cells(irow, 1, irow, 13).Style.HorizontalAlignment = HorzAlignment.Near
                .Cells(irow, 1, irow, 13).Style.VerticalAlignment = VertAlignment.Center
                .Cells(irow, 1, irow, 13).Style.Font.Bold = True
                .Cells(irow, 1, irow, 13).Style.Font.Size = 16
                .Cells(irow, 1, irow, 13).Style.Font.Name = "Segoe UI"
                irow = irow + 2

                Dim irowStar = irow
                .Cells(irow, 1).Value = "Factory"
                .Cells(irow, 3).Value = ": " & cls.FactoryName
                .Cells(irow, 5).Value = "Prod. Date"
                .Cells(irow, 7).Value = ": " & cls.Period
                irow = irow + 1

                .Cells(irow, 1).Value = "Type"
                .Cells(irow, 3).Value = ": " & cls.ItemType_Name
                .Cells(irow, 5).Value = "Shift"
                .Cells(irow, 7).Value = ": " & cls.ShiftName
                irow = irow + 1

                .Cells(irow, 1).Value = "Machine Process"
                .Cells(irow, 3).Value = ": " & cls.LineName
                .Cells(irow, 5).Value = "Seq"
                .Cells(irow, 7).Value = ": " & cls.Seq
                irow = irow + 1

                .Cells(irow, 1).Value = "Item Check"
                .Cells(irow, 3).Value = ": " & cls.ItemCheck_Name
                .Cells(irow, 5).Value = "Show Verified Only"
                .Cells(irow, 7).Value = ": " & cls.ShowVerify_Desc
                irow = irow + 1

                Dim rgHeader As ExcelRange = .Cells(irowStar, 1, irow, 7)
                rgHeader.Style.HorizontalAlignment = HorzAlignment.Near
                rgHeader.Style.VerticalAlignment = VertAlignment.Center
                rgHeader.Style.Font.Size = 10
                rgHeader.Style.Font.Name = "Segoe UI"
                rgHeader.Style.Font.Bold = True
                row_GridTitle = irow

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End With
    End Sub
    Private Sub SpecGrid(ByVal pExl As ExcelWorksheet, cls As clsProdSampleVerification)
        With pExl
            Try
                ds = clsProdSampleVerificationDB.GridLoad(GetCharSetup, cls)
                Dim dtChartSetup As DataTable = ds.Tables(0)
                Dim USL = dtChartSetup.Rows(0)("USL")
                Dim LSL = dtChartSetup.Rows(0)("LSL")
                Dim UCL = dtChartSetup.Rows(0)("UCL")
                Dim LCL = dtChartSetup.Rows(0)("LCL")
                Dim XBarUCL = dtChartSetup.Rows(0)("XBarUCL")
                Dim XBarLCL = dtChartSetup.Rows(0)("XBarLCL")
                Dim RUCL = dtChartSetup.Rows(0)("RUCL")
                Dim RLCL = dtChartSetup.Rows(0)("RLCL")
                Dim CS = dtChartSetup.Rows(0)("CS")
                Dim MAX = dtChartSetup.Rows(0)("nMAX")
                Dim MIN = dtChartSetup.Rows(0)("nMIN")
                Dim AVG = dtChartSetup.Rows(0)("nAVG")
                Dim R = dtChartSetup.Rows(0)("nR")
                Dim C = dtChartSetup.Rows(0)("C")
                Dim NG = dtChartSetup.Rows(0)("NG")

                Dim MAXClr = dtChartSetup.Rows(0)("nMAXClr")
                Dim MINClr = dtChartSetup.Rows(0)("nMINClr")
                Dim AVGClr = dtChartSetup.Rows(0)("nAVGClr")
                Dim RClr = dtChartSetup.Rows(0)("nRClr")
                Dim C_Clr = dtChartSetup.Rows(0)("C_Clr")
                Dim NG_Clr = dtChartSetup.Rows(0)("NG_Clr")

                Dim irow = row_GridTitle + 2
                Dim icolhdr1 = 1
                Dim icolhdr2 = 1
                Dim icolbd = 1

                '-----ADD HEADER 1-------
                .Cells(irow, icolhdr1).Value = "Specification"
                .Cells(irow, icolhdr1, irow, icolhdr1 + 1).Merge = True
                icolhdr1 = icolhdr1 + 2

                .Cells(irow, icolhdr1).Value = "Control Plan"
                .Cells(irow, icolhdr1, irow, icolhdr1 + 1).Merge = True
                icolhdr1 = icolhdr1 + 2

                If CS = "1" Then
                    .Cells(irow, icolhdr1).Value = "X Bar Control"
                    .Cells(irow, icolhdr1, irow, icolhdr1 + 1).Merge = True
                    icolhdr1 = icolhdr1 + 2
                End If

                .Cells(irow, icolhdr1).Value = "Result"
                .Cells(irow, icolhdr1, irow, icolhdr1 + 5).Merge = True

                Dim rgCellhdr1 As ExcelRange = .Cells(irow, 1, irow, icolhdr1 + 5)
                rgCellhdr1.Style.Font.Size = 10
                rgCellhdr1.Style.Font.Name = "Segoe UI"
                rgCellhdr1.Style.HorizontalAlignment = HorzAlignment.Center
                rgCellhdr1.Style.Font.Color.SetColor(Color.White)
                rgCellhdr1.Style.Fill.PatternType = ExcelFillStyle.Solid
                rgCellhdr1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)
                '-------------------------

                '-----ADD HEADER 2-------
                irow = irow + 1
                .Cells(irow, icolhdr2).Value = "USL"
                icolhdr2 = icolhdr2 + 1

                .Cells(irow, icolhdr2).Value = "LSL"
                icolhdr2 = icolhdr2 + 1

                .Cells(irow, icolhdr2).Value = "UCL"
                icolhdr2 = icolhdr2 + 1

                .Cells(irow, icolhdr2).Value = "LCL"
                icolhdr2 = icolhdr2 + 1

                If CS = "1" Then
                    .Cells(irow, icolhdr2).Value = "XBarUCL"
                    icolhdr2 = icolhdr2 + 1

                    .Cells(irow, icolhdr2).Value = "XBarLCL"
                    icolhdr2 = icolhdr2 + 1
                End If

                .Cells(irow, icolhdr2).Value = "Min"
                icolhdr2 = icolhdr2 + 1

                .Cells(irow, icolhdr2).Value = "Max"
                icolhdr2 = icolhdr2 + 1

                .Cells(irow, icolhdr2).Value = "Ave"
                icolhdr2 = icolhdr2 + 1

                .Cells(irow, icolhdr2).Value = "R"

                Dim rgCellhdr2 As ExcelRange = .Cells(irow, 1, irow, icolhdr2)
                rgCellhdr2.Style.Font.Size = 10
                rgCellhdr2.Style.Font.Name = "Segoe UI"
                rgCellhdr2.Style.HorizontalAlignment = HorzAlignment.Center
                rgCellhdr2.Style.Font.Color.SetColor(Color.White)
                rgCellhdr2.Style.Fill.PatternType = ExcelFillStyle.Solid
                rgCellhdr2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)
                '-------------------------

                '-----ADD HEADER 2-------
                irow = irow + 1
                .Cells(irow, icolbd).Value = USL
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                icolbd = icolbd + 1

                .Cells(irow, icolbd).Value = LSL
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                icolbd = icolbd + 1

                .Cells(irow, icolbd).Value = UCL
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                icolbd = icolbd + 1

                .Cells(irow, icolbd).Value = LCL
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                icolbd = icolbd + 1

                If CS = "1" Then
                    .Cells(irow, icolbd).Value = XBarUCL
                    .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                    icolbd = icolbd + 1

                    .Cells(irow, icolbd).Value = XBarLCL
                    .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                    icolbd = icolbd + 1
                End If

                .Cells(irow, icolbd).Value = MIN
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                .Cells(irow, icolbd).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(irow, icolbd).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(MINClr))
                icolbd = icolbd + 1

                .Cells(irow, icolbd).Value = MAX
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                .Cells(irow, icolbd).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(irow, icolbd).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(MAXClr))
                icolbd = icolbd + 1

                .Cells(irow, icolbd).Value = AVG
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                .Cells(irow, icolbd).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(irow, icolbd).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(AVGClr))
                icolbd = icolbd + 1

                .Cells(irow, icolbd).Value = R
                .Cells(irow, icolbd).Style.Numberformat.Format = "####0.000"
                .Cells(irow, icolbd).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(irow, icolbd).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(RClr))
                icolbd = icolbd + 1

                .Cells(irow - 1, icolbd).Value = C
                .Cells(irow - 1, icolbd, irow, icolbd).Merge = True
                .Cells(irow - 1, icolbd, irow, icolbd).Style.HorizontalAlignment = HorizontalAlign.Center
                .Cells(irow - 1, icolbd, irow, icolbd).Style.VerticalAlignment = VertAlignment.Center
                .Cells(irow - 1, icolbd, irow, icolbd).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(irow - 1, icolbd, irow, icolbd).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(C_Clr))
                icolbd = icolbd + 1

                .Cells(irow - 1, icolbd).Value = NG
                .Cells(irow - 1, icolbd, irow, icolbd).Merge = True
                .Cells(irow - 1, icolbd, irow, icolbd).Style.HorizontalAlignment = HorizontalAlign.Center
                .Cells(irow - 1, icolbd, irow, icolbd).Style.VerticalAlignment = VertAlignment.Center
                .Cells(irow - 1, icolbd, irow, icolbd).Style.Font.Bold = True
                .Cells(irow - 1, icolbd, irow, icolbd).Style.Fill.PatternType = ExcelFillStyle.Solid
                .Cells(irow - 1, icolbd, irow, icolbd).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(NG_Clr))

                Dim Border As ExcelRange = .Cells(irow - 2, 1, irow, icolbd)
                Border.Style.Border.Top.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Right.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Left.Style = ExcelBorderStyle.Thin
                Border.Style.Font.Size = 10
                Border.Style.Font.Name = "Segoe UI"

                row_ChartSetup = irow

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End With
    End Sub
    Private Sub HeaderResult(ByVal pExl As ExcelWorksheet, cls As clsProdSampleVerification)
        With pExl
            Try
                Dim irow = row_ChartSetup + 2
                Dim irowTitle = irow
                Dim irowhdr1 = irowTitle + 1
                Dim irowhdr2 = irowhdr1 + 1
                Dim irowhdr3 = irowhdr2 + 1
                Dim iColhdr1 = 2
                Dim iColhdr2 = 2
                Dim iColhdr3 = 2
                row_HeaderResult = irowhdr3

                .Cells(irowhdr1, 1).Value = "Date"
                .Cells(irowhdr2, 1).Value = "shift"
                .Cells(irowhdr3, 1).Value = "time"
                .Column(1).Width = 15

                ds = clsProdSampleVerificationDB.GridLoad(GetHeader_ProdDate, cls)
                Dim dtDate As DataTable = ds.Tables(0)
                If dtDate.Rows.Count > 0 Then
                    For i = 0 To dtDate.Rows.Count - 1
                        cls.ProdDate_Grid = Convert.ToDateTime(dtDate.Rows(i)("ProdDate")).ToString("yyyy-MM-dd")
                        ds = clsProdSampleVerificationDB.GridLoad(GetHeader_ShifCode, cls)
                        Dim dtShift As DataTable = ds.Tables(0)
                        If dtShift.Rows.Count > 0 Then
                            For n = 0 To dtShift.Rows.Count - 1
                                cls.Shiftcode_Grid = dtShift.Rows(n)("ShiftCode")
                                ds = clsProdSampleVerificationDB.GridLoad(GetHeader_Time, cls)
                                Dim dtSeq As DataTable = ds.Tables(0)
                                If dtSeq.Rows.Count > 0 Then
                                    For r = 0 To dtSeq.Rows.Count - 1
                                        .Cells(irowhdr3, iColhdr3).Value = dtSeq.Rows(r)("nTimeDesc")
                                        .Cells(irowhdr3, iColhdr3).Style.HorizontalAlignment = HorzAlignment.Center
                                        .Column(iColhdr3).Width = 15
                                        iColhdr3 = iColhdr3 + 1
                                    Next
                                End If

                                Dim iColhdr2_End = iColhdr3 - 1

                                Dim nShiftCode = dtShift.Rows(n)("ShiftCode")
                                'If nShiftCode = "SH001" Then
                                '    nShiftCode = "Shift 1"
                                'ElseIf nShiftCode = "SH002" Then
                                '    nShiftCode = "Shift 2"
                                'End If

                                .Cells(irowhdr2, iColhdr2, irowhdr2, iColhdr2_End).Value = nShiftCode
                                .Cells(irowhdr2, iColhdr2, irowhdr2, iColhdr2_End).Merge = True
                                .Cells(irowhdr2, iColhdr2, irowhdr2, iColhdr2_End).Style.HorizontalAlignment = HorzAlignment.Center

                                iColhdr2 = iColhdr3
                            Next
                        End If

                        Dim icolhdr1_End = iColhdr2 - 1

                        .Cells(irowhdr1, iColhdr1, irowhdr1, icolhdr1_End).Value = dtDate.Rows(i)("ProdDate")
                        .Cells(irowhdr1, iColhdr1, irowhdr1, icolhdr1_End).Merge = True
                        .Cells(irowhdr1, iColhdr1, irowhdr1, icolhdr1_End).Style.HorizontalAlignment = HorzAlignment.Center

                        iColhdr1 = iColhdr2
                        col_HeaderResult = iColhdr1 - 1
                    Next
                End If

                .Cells(irowTitle, 1, irowTitle, col_HeaderResult).Value = "STATISTIC PRODUCT MONITORING"
                .Cells(irowTitle, 1, irowTitle, col_HeaderResult).Merge = True
                .Cells(irowTitle, 1, irowTitle, col_HeaderResult).Style.HorizontalAlignment = MenuItemAlignment.Center
                .Cells(irowTitle, 1, irowTitle, col_HeaderResult).Style.Font.Bold = True

                Dim rgCell As ExcelRange = .Cells(irowhdr1, 1, irowhdr3, col_HeaderResult)
                rgCell.Style.Font.Size = 10
                rgCell.Style.Font.Name = "Segoe UI"
                rgCell.Style.HorizontalAlignment = HorzAlignment.Center
                rgCell.Style.Font.Color.SetColor(Color.White)
                rgCell.Style.Fill.PatternType = ExcelFillStyle.Solid
                rgCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End With
    End Sub
    Private Sub CellResult(ByVal pExl As ExcelWorksheet, cls As clsProdSampleVerification)
        With pExl
            Try
                Dim irow = row_HeaderResult + 1

                ds = clsProdSampleVerificationDB.GridLoad(GetGridData, cls)
                Dim dtGrid As DataTable = ds.Tables(0)
                If dtGrid.Rows.Count > 0 Then
                    For i = 0 To dtGrid.Rows.Count - 1
                        For n = 1 To dtGrid.Columns.Count - 1
                            Try
                                Dim data = dtGrid.Rows(i)(n)
                                Dim RowIndex = Trim(dtGrid.Rows(i)(0))
                                If n > 1 Then
                                    If RowIndex = "EachData" Or RowIndex = "XBar" Or RowIndex = "Judgement" Or RowIndex = "Correction" Or RowIndex = "Verification" Then
                                        If IsDBNull(data) Then
                                            .Cells(irow + i, n).Value = data
                                        Else
                                            Dim value = Split(data, "|")(0)
                                            Dim color = Split(data, "|")(1)
                                            .Cells(irow + i, n).Style.Fill.PatternType = ExcelFillStyle.Solid
                                            .Cells(irow + i, n).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color))
                                            If RowIndex = "EachData" Or RowIndex = "XBar" Then
                                                If value <> "" Then
                                                    .Cells(irow + i, n).Value = CDec(value)
                                                    .Cells(irow + i, n).Style.Numberformat.Format = "####0.000"
                                                Else
                                                    .Cells(irow + i, n).Value = value
                                                End If
                                            Else
                                                .Cells(irow + i, n).Value = value
                                            End If
                                        End If
                                    Else
                                        .Cells(irow + i, n).Value = data
                                    End If
                                Else
                                    .Cells(irow + i, n).Value = data
                                End If
                                If RowIndex = "GridNothing" Then
                                    .Cells(irow + i, n).Style.Fill.PatternType = ExcelFillStyle.Solid
                                    .Cells(irow + i, n).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#878787"))
                                End If

                            Catch ex As Exception
                                Throw New Exception(ex.Message)
                            End Try
                        Next
                    Next
                End If

                col_CellResult = dtGrid.Columns.Count
                row_CellResult = irow + dtGrid.Rows.Count

                Dim Border As ExcelRange = .Cells(row_ChartSetup + 4, 1, row_CellResult - 1, col_CellResult - 1)
                Border.Style.Border.Top.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Right.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Left.Style = ExcelBorderStyle.Thin
                Border.Style.Font.Size = 10
                Border.Style.Font.Name = "Segoe UI"
                Border.Style.HorizontalAlignment = HorzAlignment.Center
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End With
    End Sub
    Private Sub HeaderActivity(ByVal pExl As ExcelWorksheet, cls As clsProdSampleVerification)
        Try
            Dim irow = row_CellChart + 5
            With pExl
                .Cells(irow, 1, irow, 7).Value = "ACTIVITY MONITORING"
                .Cells(irow, 1, irow, 7).Merge = True
                .Cells(irow, 1, irow, 7).Style.HorizontalAlignment = MenuItemAlignment.Center
                .Cells(irow, 1, irow, 7).Style.Font.Bold = True
                irow = irow + 1

                .Cells(irow, 1).Value = "Date"
                .Cells(irow, 2).Value = "Shift"
                .Cells(irow, 3).Value = "Time"
                .Cells(irow, 4).Value = "PIC"

                .Cells(irow, 5, irow, 6).Value = "Action"
                .Cells(irow, 5, irow, 6).Style.HorizontalAlignment = HorzAlignment.Center
                .Cells(irow, 5, irow, 6).Merge = True
                .Cells(irow, 5, irow, 6).Style.WrapText = True

                .Cells(irow, 7, irow, 8).Value = "Remark"
                .Cells(irow, 7, irow, 8).Style.HorizontalAlignment = HorzAlignment.Center
                .Cells(irow, 7, irow, 8).Merge = True
                .Cells(irow, 7, irow, 8).Style.WrapText = True

                .Cells(irow, 9).Value = "Result"

                .Cells(irow, 10).Value = "Last User"
                .Cells(irow, 11).Value = "Last Update"

                col_HeaderActivity = 11
                row_HeaderActivity = irow

                Dim rgCell As ExcelRange = .Cells(irow, 1, irow, col_HeaderActivity)
                rgCell.Style.Font.Size = 10
                rgCell.Style.Font.Name = "Segoe UI"
                rgCell.Style.HorizontalAlignment = HorzAlignment.Center
                rgCell.Style.Font.Color.SetColor(Color.White)
                rgCell.Style.Fill.PatternType = ExcelFillStyle.Solid
                rgCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub CellActivity(ByVal pExl As ExcelWorksheet, cls As clsProdSampleVerification)
        Try
            Dim irow = row_HeaderActivity
            Dim nRow = 0
            With pExl
                ds = clsProdSampleVerificationDB.GridLoad(GetGridData_Activity, cls)
                Dim dtGridActivity As DataTable = ds.Tables(0)
                If dtGridActivity.Rows.Count > 0 Then
                    nRow = dtGridActivity.Rows.Count - 1
                    irow = irow + 1

                    For i = 0 To dtGridActivity.Rows.Count - 1
                        .Cells(irow + i, 1).Value = dtGridActivity.Rows(i)("ProdDate")
                        .Cells(irow + i, 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                        .Cells(irow + i, 2).Value = dtGridActivity.Rows(i)("ShiftName")
                        .Cells(irow + i, 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                        .Cells(irow + i, 3).Value = dtGridActivity.Rows(i)("Time_Desc")
                        .Cells(irow + i, 3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                        .Cells(irow + i, 4).Value = dtGridActivity.Rows(i)("PIC")
                        .Cells(irow + i, 4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left

                        .Cells(irow + i, 5, irow + i, 6).Value = dtGridActivity.Rows(i)("Action")
                        .Cells(irow + i, 5, irow + i, 6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left
                        .Cells(irow + i, 5, irow + i, 6).Merge = True
                        .Cells(irow + i, 5, irow + i, 6).Style.WrapText = True

                        .Cells(irow + i, 7, irow + i, 8).Value = dtGridActivity.Rows(i)("Remark")
                        .Cells(irow + i, 7, irow + i, 8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left
                        .Cells(irow + i, 7, irow + i, 8).Merge = True
                        .Cells(irow + i, 7, irow + i, 8).Style.WrapText = True

                        .Cells(irow + i, 9).Value = If(dtGridActivity.Rows(i)("Result") = 0, "OK", "NG")
                        .Cells(irow + i, 9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                        .Cells(irow + i, 10).Value = dtGridActivity.Rows(i)("LastUser")
                        .Cells(irow + i, 10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left

                        .Cells(irow + i, 11).Value = dtGridActivity.Rows(i)("LastDate")
                        .Cells(irow + i, 11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center

                        .Column(1).Width = 15
                        .Column(2).Width = 15
                        .Column(3).Width = 15
                        .Column(4).Width = 15
                        .Column(5).Width = 15
                        .Column(6).Width = 15
                        .Column(7).Width = 15
                        .Column(8).Width = 15
                        .Column(9).Width = 15
                        .Column(10).Width = 15
                        .Column(11).Width = 15

                    Next
                End If

                col_CellActivity = 11
                row_CellActivity = irow + nRow

                Dim Border As ExcelRange = .Cells(irow, 1, row_CellActivity, col_CellActivity)
                Border.Style.Border.Top.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Right.Style = ExcelBorderStyle.Thin
                Border.Style.Border.Left.Style = ExcelBorderStyle.Thin
                Border.Style.Font.Size = 10
                Border.Style.Font.Name = "Segoe UI"

            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
#End Region
End Class