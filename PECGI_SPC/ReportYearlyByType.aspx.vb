Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports System.Web.Services
Public Class ReportYearlyByType
    Inherits System.Web.UI.Page
    Dim Factory_ComboType = "1"
    Dim ProcessGroup_ComboType = "2"
    Dim LineGroup_ComboType = "3"
    Dim ProcessCode_ComboType = "4"
    Dim LineCode_ComboType = "5"
    Dim ItemType_ComboType = "6"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("user") Is Nothing Then
                Response.Redirect("~/Default.aspx")
            End If
            UpFillCombo()
        End If
    End Sub

    Private Sub cboProcessGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcessGroup.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = e.Parameter
            data.UserID = Session("user").ToString

            dt = clsReportYearlyByType.FillCombo(ProcessGroup_ComboType, data)
            With cboProcessGroup
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub cboLineGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineGroup.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ProcessGroup = Split(e.Parameter, "|")(1)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(LineGroup_ComboType, data)
            With cboLineGroup
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub cboProcessCode_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcessCode.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = Split(e.Parameter, "|")(0)
            data.ProcessGroup = Split(e.Parameter, "|")(1)
            data.LineGroup = Split(e.Parameter, "|")(2)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(ProcessCode_ComboType, data)
            With cboProcessCode
                .DataSource = dt
                .DataBind()
            End With
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub cboLineCode_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineCode.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ProcessCode = e.Parameter.Split("|")(1)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(LineCode_ComboType, data)
            With cboLineCode
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub cboItemType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemType.Callback
        Try
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.FactoryCode = e.Parameter.Split("|")(0)
            data.ProcessCode = e.Parameter.Split("|")(1)
            data.LineCode = e.Parameter.Split("|")(2)
            data.UserID = Session("user").ToString

            Dim ErrMsg As String = ""
            dt = clsReportYearlyByType.FillCombo(ItemType_ComboType, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 0)
        End Try
    End Sub

    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        cb.JSProperties("cp_message") = ErrMsg
        cb.JSProperties("cp_type") = msgType
        cb.JSProperties("cp_val") = pVal
    End Sub

    Private Sub UpFillCombo()
        Try
            Dim HD As String
            Dim dt As New DataTable
            Dim data As New clsFillCombo()
            data.UserID = Session("user").ToString

            'FILL COMBO FACTORY CODE'
            dt = clsReportYearlyByType.FillCombo(Factory_ComboType, data)
            With cboFactory
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboFactory.SelectedItem.GetFieldValue("Code")
            HideValue.Set("UserID", data.UserID)
            HideValue.Set("FactoryCode", HD)
            data.FactoryCode = HideValue.Get("FactoryCode")

            'FILL COMBO PROCESS GROUP'
            dt = clsReportYearlyByType.FillCombo(ProcessGroup_ComboType, data)
            With cboProcessGroup
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboProcessGroup.SelectedItem.GetFieldValue("Code")
            HideValue.Set("ProcessGroup", HD)
            data.ProcessGroup = HideValue.Get("ProcessGroup")

            'FILL COMBO LINE GROUP'
            dt = clsReportYearlyByType.FillCombo(LineGroup_ComboType, data)
            With cboLineGroup
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboLineGroup.SelectedItem.GetFieldValue("Code")
            HideValue.Set("LineGroup", HD)
            data.LineGroup = HideValue.Get("LineGroup")

            'FILL COMBO PROCESS CODE'
            dt = clsReportYearlyByType.FillCombo(ProcessCode_ComboType, data)
            With cboProcessCode
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboProcessCode.SelectedItem.GetFieldValue("Code")
            HideValue.Set("ProcessCode", HD)
            data.ProcessCode = HideValue.Get("ProcessCode")

            'FILL COMBO LINECODE'
            dt = clsReportYearlyByType.FillCombo(LineCode_ComboType, data)
            With cboLineCode
                .DataSource = dt
                .DataBind()
                .SelectedIndex = 0
            End With
            HD = cboLineCode.SelectedItem.GetFieldValue("Code")
            HideValue.Set("LineCode", HD)
            data.LineCode = HideValue.Get("LineCode")

            'FILL ITEM TYPE'
            dt = clsReportYearlyByType.FillCombo(ItemType_ComboType, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With


        Catch ex As Exception
            show_error(MsgTypeEnum.Info, "", 0)
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function LoadData(User As String, FactoryCode As String, ProcessGroup As String, LineGroup As String, ProcessCode As String, LineCode As String, ItemType As String, ProdDate_From As String, ProdDate_To As String) As clsContent
        Dim content As New clsContent
        Try
            Dim dt As New DataTable
            Dim resp As New List(Of clsReportYearlyByType)
            Dim myTable As List(Of String()) = New List(Of String())()
            Dim data As New clsReportYearlyByType
            data.UserID = User
            data.FactoryCode = FactoryCode
            data.ProcessGroup = ProcessGroup
            data.LineGroup = LineGroup
            data.ProcessCode = ProcessCode
            data.LineCode = LineCode
            data.ItemType = ItemType
            data.ProdDateFrom = ProdDate_From
            data.ProdDateTo = ProdDate_To

            dt = clsReportYearlyByType.LoadData(data)

            If dt.Rows(0)(0).ToString <> "" Then
                For n = 0 To dt.Rows.Count - 1
                    Dim columnCount As Integer = 0
                    Dim myTableRow As String() = New String(dt.Columns.Count - 1) {}

                    For Each dc As DataColumn In dt.Columns
                        myTableRow(columnCount) = dt.Rows(n)(dc.ToString())
                        columnCount += 1
                    Next
                    myTable.Add(myTableRow)
                Next

                Dim table2DArray = myTable.ToArray()
                content.Message = "Success"
                content.Contents = table2DArray
            Else
                content.Message = "Data Not Found!"
                content.Contents = ""
            End If
        Catch ex As Exception
            content.Message = "Error"
            content.Contents = ex.Message
        End Try
        Return content
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function LoadDetail(User As String, ProcessCode As String, LineCode As String, LineGroup As String, Periode As String, Qty As String) As clsContent
        Dim content As New clsContent
        Try
            Dim dt As New DataTable
            Dim myTable As List(Of String()) = New List(Of String())()
            Dim data As New clsReportYearlyByType
            data.UserID = User
            data.ProcessCode = ProcessCode
            data.LineCode = LineCode
            data.LineGroup = LineGroup
            data.Periode = Periode
            data.QtyFTA = Qty

            dt = clsReportYearlyByType.LoadDetail(data)

            For n = 0 To dt.Rows.Count - 1
                Dim columnCount As Integer = 0
                Dim myTableRow As String() = New String(dt.Columns.Count - 1) {}

                For Each dc As DataColumn In dt.Columns
                    myTableRow(columnCount) = dt.Rows(n)(dc.ToString())
                    columnCount += 1
                Next
                myTable.Add(myTableRow)
            Next

            Dim table2DArray = myTable.ToArray()

            content.Message = "Success"
            content.Contents = table2DArray
        Catch ex As Exception
            content.Message = "Error"
            content.Contents = ex.Message
        End Try
        Return content
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <WebMethod()>
    Public Shared Function ChartLineGroup(User As String, FactoryCode As String, ProcessGroup As String, LineGroup As String, ProcessCode As String, LineCode As String, ItemType As String, ProdDate_From As String, ProdDate_To As String) As clsContent
        Dim content As New clsContent
        Try
            Dim dt As New DataTable
            Dim resp As New List(Of clsReportYearlyByType)
            Dim myTable As List(Of String()) = New List(Of String())()
            Dim data As New clsReportYearlyByType
            data.UserID = User
            data.FactoryCode = FactoryCode
            data.ProcessGroup = ProcessGroup
            data.LineGroup = LineGroup
            data.ProcessCode = ProcessCode
            data.LineCode = LineCode
            data.ItemType = ItemType
            data.ProdDateFrom = ProdDate_From
            data.ProdDateTo = ProdDate_To

            dt = clsReportYearlyByType.ChartLineGroup(data)
            For n = 0 To dt.Rows.Count
                Dim columnCount As Integer = 0
                Dim myTableRow As String() = New String(dt.Columns.Count - 1) {}

                If n = 0 Then
                    For Each dc As DataColumn In dt.Columns
                        myTableRow(columnCount) = dc.ToString()
                        columnCount += 1
                    Next
                Else
                    For Each dc As DataColumn In dt.Columns
                        myTableRow(columnCount) = dt.Rows(n - 1)(dc.ToString())
                        columnCount += 1
                    Next
                End If

                myTable.Add(myTableRow)
            Next
            Dim table2DArray = myTable.ToArray()

            content.Message = "Success"
            content.Contents = table2DArray

        Catch ex As Exception
            content.Message = "Error"
            content.Contents = ex.Message
        End Try
        Return content
    End Function

End Class