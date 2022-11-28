Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services

Public Class AlertDelayVerification
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Public AuthInsert As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False
    Public dt As DataTable
    Dim UCL As String = ""
    Dim LCL As String = ""
    Dim USL As String = ""
    Dim LSL As String = ""
    Dim MinValue As String = ""
    Dim MaxValue As String = ""
    Dim Average As String = ""
    Dim CharacteristicStatus As Integer
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            'up_GridLoad()
            GetComboBoxData()
            hdInterval.Value = 60000
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sGlobal.getMenu("X030")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName
        pUser = Session("user")
        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "X030")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "X030")
        If AuthUpdate = False Then
        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "X030")
        If AuthDelete = False Then

        End If

        lblDateNow.Text = DateTime.Now.ToString("dd-MMM-yyyy") 'HH:mm:ss

        If Not IsPostBack And Not IsCallback Then
            dtDate.Value = DateTime.Now
            rbAuto.Checked = True
        End If
    End Sub
    Private Sub GetComboBoxData()
        GetFactoryCode()
    End Sub
    Private Sub GetFactoryCode()
        cboFactory.DataSource = ClsSPCItemCheckByTypeDB.FillComboFactoryGrid("1", Session("user"))
        cboFactory.DataBind()
        cboFactory.SelectedIndex = 1
    End Sub

#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)

    End Sub

    Private Sub up_GridLoad(FactoryCode As String)
        Dim pProdDateType As Integer

        'If ProdDateSelection.SelectedValue = "rbAuto" Then
        '    rbProdDate = 1
        'ElseIf ProdDateSelection.SelectedValue = "rbManual" Then
        '    rbProdDate = 2
        'End If
        If rbAuto.Checked = True Then
            pProdDateType = 1
        ElseIf rbManual.Checked = True Then
            pProdDateType = 2
        End If

        Dim pProdDate As DateTime = Convert.ToDateTime(dtDate.Date)
        Dim pProdDate2 = dtDate.Date.ToString()

        LoadGridDelayVerif(FactoryCode, pProdDateType, pProdDate)
    End Sub
    Private Sub LoadGridDelayVerif(FactoryCode As String, pProdDateType As Integer, pProdDate As DateTime)
        Try
            GridDelayVerif.DataSource = clsSPCAlertDashboardDB.GetDelayVerificationGrid(pUser, FactoryCode, pProdDateType, pProdDate)
            GridDelayVerif.DataBind()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function DelayInput(pUser As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSiteMasterDB.GetDelayInput(pUser)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If msg <> "" Then msg = msg & ";"
                msg = msg & dt.Rows(i)("Label").ToString

                If msg <> "" Then msg = msg & "||"
                msg = msg & dt.Rows(i)("Link").ToString & "||"
            Next
        Else
            msg = ""
        End If
        Return msg
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function NGInput(pUser As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSiteMasterDB.GetNGInput(pUser)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If msg <> "" Then msg = msg & ";"
                msg = msg & dt.Rows(i)("Label").ToString

                If msg <> "" Then msg = msg & "||"
                msg = msg & dt.Rows(i)("Link").ToString & "||"
            Next
        Else
            msg = ""
        End If
        Return msg
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function DelayVerify(pUser As String) As String

        Dim dt As DataTable
        Dim msg As String = ""

        dt = clsSiteMasterDB.GetDelayVerify(pUser)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If msg <> "" Then msg = msg & ";"
                msg = msg & dt.Rows(i)("Label").ToString

                If msg <> "" Then msg = msg & "||"
                msg = msg & dt.Rows(i)("Link").ToString & "||"
            Next
        Else
            msg = ""
        End If
        Return msg
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function AlertNotif(pUser As String) As String
        Dim msg As String = ""

        Dim dtDI = clsSiteMasterDB.GetDelayInput(pUser)
        Dim dtDV = clsSiteMasterDB.GetDelayVerify(pUser)
        Dim dtNG = clsSiteMasterDB.GetNGInput(pUser)

        Dim TotDI = dtDI.Rows.Count
        Dim TotDV = dtDV.Rows.Count
        Dim TotNG = dtNG.Rows.Count
        Dim TotNotif = TotDI + TotDV + TotNG
        msg = TotNotif & "|" & TotDI & "|" & TotDV & "|" & TotNG & "|" & DateTime.Now.ToString("yyyy MMM dd HH:mm:ss")

        Return msg
    End Function


    Protected Sub GridDelayVerif_RowInserting(sender As Object, e As ASPxDataInsertingEventArgs)

    End Sub

    Protected Sub GridDelayVerif_RowDeleting(sender As Object, e As ASPxDataDeletingEventArgs)

    End Sub

    Protected Sub GridDelayVerif_AfterPerformCallback(sender As Object, e As ASPxGridViewAfterPerformCallbackEventArgs)
        If e.CallbackName <> "CANCELEDIT" Then
            up_GridLoad(cboFactory.Value)
        End If
    End Sub

    Protected Sub GridDelayVerif_StartRowEditing(sender As Object, e As ASPxStartRowEditingEventArgs)

    End Sub

    Protected Sub GridDelayVerif_RowValidating(sender As Object, e As ASPxDataValidationEventArgs)

    End Sub
    Private Sub GridDelayVerif_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles GridDelayVerif.HtmlDataCellPrepared
        Dim Delay As String = ""
        Dim Link As New HyperLink()
        If e.CellValue Is Nothing Then
            Dim Ab As String = "Test"
        Else
            If e.CellValue.ToString.Contains("Edit") AndAlso e.CellValue.ToString IsNot Nothing Then
                e.Cell.Text = ""

                Link.ForeColor = Color.Blue
                Link.Text = "<label class='fa fa-edit'></label>"
                Link.NavigateUrl = Split(e.CellValue, "||")(1)
                Link.Target = "_blank"

                e.Cell.Controls.Add(Link)
            End If
        End If

        If e.DataColumn.FieldName = "ItemTypeName" Then
            e.Cell.Text = Split(e.CellValue, "||")(0)
            CharacteristicStatus = Split(e.CellValue, "||")(1)
        End If

        If e.DataColumn.FieldName = "DelayVerif" Then
            Delay = (e.CellValue)
            Dim Test = TimeSpan.FromMinutes(e.CellValue)

            If Delay <= 60 Then
                If CharacteristicStatus = 0 Then
                    e.Cell.BackColor = System.Drawing.Color.Pink
                ElseIf CharacteristicStatus = 1 Then
                    e.Cell.BackColor = Color.FromArgb(255, 255, 153)
                End If
            ElseIf Delay > 60 Then
                e.Cell.BackColor = System.Drawing.Color.Red
            End If

            Dim Days = Test.Days * 24
            Dim Hours = Days + Test.Hours

            If Days > 0 Then

                e.Cell.Text = Convert.ToString(Test.Days & " Day " & Test.Hours & " Hours " & Test.Minutes & " Minutes")

            Else

                If Hours > 0 Then
                    e.Cell.Text = Convert.ToString(Test.Hours & " Hours " & Test.Minutes & " Minutes")
                Else
                    e.Cell.Text = Convert.ToString(Test.Minutes & " Minutes")
                End If

            End If
            'e.Cell.Text = Convert.ToString(Hours & ":" & Test.Minutes & ":00")
        End If
        If e.DataColumn.FieldName = "LSL" Then
            LSL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "USL" Then
            USL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "UCL" Then
            UCL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "LCL" Then
            LCL = (e.CellValue)
        ElseIf e.DataColumn.FieldName = "MinValue" Then
            MinValue = (e.CellValue)
            If MinValue < LSL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf MinValue < LCL Then

                If CharacteristicStatus = 0 Then
                    e.Cell.BackColor = System.Drawing.Color.Pink
                ElseIf CharacteristicStatus = 1 Then
                    e.Cell.BackColor = Color.FromArgb(255, 255, 153)
                End If

            End If
        ElseIf e.DataColumn.FieldName = "MaxValue" Then
            MaxValue = (e.CellValue)
            If MaxValue > USL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf MaxValue > UCL Then

                If CharacteristicStatus = 0 Then
                    e.Cell.BackColor = System.Drawing.Color.Pink
                ElseIf CharacteristicStatus = 1 Then
                    e.Cell.BackColor = Color.FromArgb(255, 255, 153)
                End If

            End If
        ElseIf e.DataColumn.FieldName = "Average" Then
            Average = (e.CellValue)
            If Average > USL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf Average > UCL Then

                If CharacteristicStatus = 0 Then
                    e.Cell.BackColor = System.Drawing.Color.Pink
                ElseIf CharacteristicStatus = 1 Then
                    e.Cell.BackColor = Color.FromArgb(255, 255, 153)
                End If

            ElseIf Average < LSL Then
                e.Cell.BackColor = System.Drawing.Color.Red
            ElseIf Average < LCL Then

                If CharacteristicStatus = 0 Then
                    e.Cell.BackColor = System.Drawing.Color.Pink
                ElseIf CharacteristicStatus = 1 Then
                    e.Cell.BackColor = Color.FromArgb(255, 255, 153)
                End If

            End If
        End If
    End Sub
    Private Sub GridDelayVerif_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles GridDelayVerif.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "Load" Then
                up_GridLoad(cboFactory.Value)
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub GridDelayVerif_CustomButtonCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomButtonCallbackEventArgs) Handles GridDelayVerif.CustomButtonCallback

        Try

            Dim CountSendEmail As Integer
            Dim CheckAvailableData As DataTable
            Dim UserTo As String

            Dim FactoryCode = cboFactory.Value
            Dim ItemTypeName = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeName")
            'Dim ItemTypeCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemTypeCode")
            Dim LineCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "LineCode")
            Dim ItemCheck = GridDelayVerif.GetRowValues(e.VisibleIndex, "ItemCheck")
            Dim LinkDate = GridDelayVerif.GetRowValues(e.VisibleIndex, "LinkDate")
            Dim ShiftCode = GridDelayVerif.GetRowValues(e.VisibleIndex, "ShiftCode")
            Dim SequenceNo = GridDelayVerif.GetRowValues(e.VisibleIndex, "SequenceNo")

            Dim LSL = GridDelayVerif.GetRowValues(e.VisibleIndex, "LSL")
            Dim USL = GridDelayVerif.GetRowValues(e.VisibleIndex, "USL")
            Dim LCL = GridDelayVerif.GetRowValues(e.VisibleIndex, "LCL")
            Dim UCL = GridDelayVerif.GetRowValues(e.VisibleIndex, "UCL")
            Dim MinValue = GridDelayVerif.GetRowValues(e.VisibleIndex, "MinValue")
            Dim MaxValue = GridDelayVerif.GetRowValues(e.VisibleIndex, "MaxValue")
            Dim Average = GridDelayVerif.GetRowValues(e.VisibleIndex, "Average")
            Dim VerifTime = GridDelayVerif.GetRowValues(e.VisibleIndex, "VerifTime")
            Dim DelayTime = GridDelayVerif.GetRowValues(e.VisibleIndex, "DelayVerif")
            Dim Status = GridDelayVerif.GetRowValues(e.VisibleIndex, "Status")
            Dim MK = GridDelayVerif.GetRowValues(e.VisibleIndex, "MK")
            Dim QC = GridDelayVerif.GetRowValues(e.VisibleIndex, "QC")
            ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

            Dim Test = DirectCast(GridDelayVerif.GetRowValues(e.VisibleIndex, GridDelayVerif.KeyFieldName, "LinkDate"), Object())(1)

            If MK.ToString() = "" AndAlso QC.ToString() = "" Then
                UserTo = clsSPCAlertDashboardDB.GetUserLine(FactoryCode, LineCode, "3")
            ElseIf MK.ToString() = "" Then
                UserTo = clsSPCAlertDashboardDB.GetUserLine(FactoryCode, LineCode, "2")
            End If

            CountSendEmail = clsSPCAlertDashboardDB.SendEmail(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo, "3", LSL, USL, LCL, UCL, MinValue, MaxValue, Average, Status, "", "", VerifTime, DelayTime, UserTo)

            If CountSendEmail > 0 Then
                show_error(MsgTypeEnum.Success, "Send Email Success", 1)
            Else
                show_error(MsgTypeEnum.ErrorMsg, "Send Email Failed", 1)
            End If

            'CheckAvailableData = clsSPCAlertDashboardDB.CheckDataSendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            'If CheckAvailableData.Rows.Count <= 0 Then

            '    CountSendEmail = clsSPCAlertDashboardDB.SendNotification(FactoryCode, ItemTypeName, LineCode, ItemCheck, LinkDate, ShiftCode, SequenceNo)

            'Else
            '    Return
            'End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Shared Function GetUserInLine(pUserLine As String) As String
        Dim ListData As String
        Dim DataUser As DataTable

        Return ListData
    End Function
#End Region

End Class