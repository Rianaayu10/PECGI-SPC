Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.Data
Imports DevExpress.Web
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports DevExpress.XtraGrid
Imports DevExpress.Data
Imports System.Web.Services

Public Class TroubleHistoryByLine
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim pUser As String = ""
    Private dt As DataTable
    Dim MenuID As String = ""

    ' AUTHORIZATION
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public AuthAccess As Boolean = False

    ' FILL COMBO BOX FILTER
    Dim Factory_Sel As String = "1"
    Dim ItemType_Sel As String = "2"

    ' PARAMETER SESSION
    Dim sFactoryCode = ""
    Dim sItemType = ""


#End Region

#Region "Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'PENGECEKAN VALIDASI TOKEN
        If Session("Action") IsNot Nothing Then
            If Session("Action").ToString = "SSO" Then
                Dim token = Session("token")
                Dim SSOHost As String = ConfigurationManager.AppSettings("SSOUrl").ToString()
                If sGlobal.VerifyToken(token, SSOHost) = False Then
                    Response.Redirect(SSOHost + "/account/login?logout=1")
                End If
            End If
        End If

        'PENGECEKAN SESSION USER
        If Session("user") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If

        pUser = Session("user")
        MenuID = "B070"

        sGlobal.getMenu(MenuID)
        Master.SiteTitle = MenuID & " - " & sGlobal.menuName
        show_error(MsgTypeEnum.Info, "", 0)

        AuthAccess = sGlobal.Auth_UserAccess(pUser, MenuID)
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        If Not Page.IsPostBack Then
            If Request.QueryString("menu") IsNot Nothing Then
                'LoadForm_ByAnotherform()
            Else
                LoadForm()
            End If
        End If
    End Sub
#End Region
#Region "Procedure"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        cb.JSProperties("cp_message") = ErrMsg
        cb.JSProperties("cp_type") = msgType
        cb.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_Fillcombo()
        Try
            Dim data As New clsTroubleHistoryByLineComboBox()
            data.UserID = pUser
            Dim ErrMsg As String = ""
            Dim a As String

            '============ FILL COMBO FACTORY CODE ================'
            dt = clsTroubleHistoryByLineDB.FillCombo(Factory_Sel, data)
            With cboFactory
                .DataSource = dt
                .DataBind()
            End With
            If sFactoryCode <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sFactoryCode Then
                        cboFactory.SelectedIndex = i
                        Exit For
                    End If
                Next
            Else
                cboFactory.SelectedIndex = 0
            End If
            If cboFactory.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboFactory.SelectedItem.GetFieldValue("CODE")
            End If
            data.FactoryCode = a
            '======================================================'


            '============== FILL COMBO ITEM TYPE =================='
            dt = clsTroubleHistoryByLineDB.FillCombo(ItemType_Sel, data)
            With cboItemType
                .DataSource = dt
                .DataBind()
            End With
            If sItemType <> "" Then
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows(i)("CODE") = sItemType Then
                        cboItemType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
            If cboItemType.SelectedIndex < 0 Then
                a = ""
            Else
                a = cboItemType.SelectedItem.GetFieldValue("CODE")
            End If
            data.ItemType_Code = a
            '======================================================'
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub LoadForm()
        up_Fillcombo()

        Dim ProdDate = DateTime.Now
        dtFromDate.Value = ProdDate
        dtToDate.Value = ProdDate
    End Sub

    Private Sub cb_Callback(ByVal source As Object, ByVal e As DevExpress.Web.CallbackEventArgs) Handles cb.Callback
        Try
            up_Fillcombo()
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

#End Region

#Region "Web Service"
    <WebMethod()>
    Public Shared Function GetChartMachineProcess(ByVal Factory As String, ByVal Type As String, ByVal DateFrom As String, ByVal DateTo As String) As clsTroubleHistoryByLineChartSetting
        Dim UserID = HttpContext.Current.Session("user")
        Dim db As clsTroubleHistoryByLineDB = New clsTroubleHistoryByLineDB()
        Dim dt As List(Of clsTroubleHistoryByLineChartData) = db.GetChartMachineProcess(Factory, Type, DateFrom, DateTo, UserID)
        Dim labels As Object() = dt.Select(Function(t) t.LABELNAME).Distinct.ToArray()
        Dim message As String = "["
        Dim axis As String = "["
        Dim max As Integer

        For i As Integer = 0 To dt.Count - 1
            max = max + dt(i).DATAVAL
        Next

        For i As Integer = 0 To labels.Length - 1
            message += "{'color':'#525454','data':["
            Dim d = dt.Where(Function(x) x.LABELNAME = labels(i).ToString()).Select(Function(x) New With {Key .axisValue = x.AXISVAL, Key .dataValue = x.DATAVAL, Key .axisName = x.AXISNAME})

            For Each item In d
                message += "[" & item.axisValue & "," + item.dataValue & "],"

                If i = 0 Then
                    axis += "[" & item.axisValue & ",'" + item.axisValue & "'],"
                End If
            Next

            message = message.Substring(0, message.Length - 1) & "], 'bars': {'show': true,  'clickable': true, 'align': 'center', 'barWidth' : 1, 'fillColor': '#fdbff9' , 'lineWidth':1 }}, { 'color':'#525454','data': [[0.5,0],"
            Dim e = dt.Where(Function(x) x.LABELNAME = labels(i).ToString()).Select(Function(x) New With {Key .axisValue = x.AXISVAL2, Key .dataValue = x.DATAVAL2, Key .axisName = x.AXISNAME})

            For Each item In e
                message += "[" & item.axisValue & "," + item.dataValue & "],"
            Next

            message = message.Substring(0, message.Length - 1) & "],  'yaxis': 2,  'points': { 'symbol': 'circle', 'fillColor': '#5fa9f5','radius' : 1, 'show': true }, 'lines': {'show':true,'lineWidth':0.5}},"

        Next

        message = message.Substring(0, message.Length - 1)
        message += "]"
        axis = axis.Substring(0, axis.Length - 1)
        axis += "]"


        Dim chart As clsTroubleHistoryByLineChartSetting = New clsTroubleHistoryByLineChartSetting()
        chart.data = message
        chart.xaxisTicks = axis
        chart.Max = max.ToString

        Return chart
    End Function

    <WebMethod()>
    Public Shared Function GetTableMachineProcess(ByVal Factory As String, ByVal Type As String, ByVal DateFrom As String, ByVal DateTo As String) As List(Of clsTroubleHistoryByLineTableData)
        Dim UserID = HttpContext.Current.Session("user")
        Dim db As clsTroubleHistoryByLineDB = New clsTroubleHistoryByLineDB()
        Dim dt As List(Of clsTroubleHistoryByLineTableData) = db.GetTableMachineProcess(Factory, Type, DateFrom, DateTo, UserID)

        Return dt
    End Function

    <WebMethod()>
    Public Shared Function GetChartItemCheck(ByVal Factory As String, ByVal Type As String, ByVal Line As String, ByVal DateFrom As String, ByVal DateTo As String) As clsTroubleHistoryByLineChartSetting
        Dim UserID = HttpContext.Current.Session("user")
        Dim db As clsTroubleHistoryByLineDB = New clsTroubleHistoryByLineDB()
        Dim dt As List(Of clsTroubleHistoryByLineChartData) = db.GetChartItemCheck(Factory, Type, Line, DateFrom, DateTo, UserID)
        Dim labels As Object() = dt.Select(Function(t) t.LABELNAME).Distinct.ToArray()
        Dim message As String = "["
        Dim axis As String = "["
        Dim max As Integer

        For i As Integer = 0 To dt.Count - 1
            max = max + dt(i).DATAVAL
        Next

        For i As Integer = 0 To labels.Length - 1
            message += "{'color':'#525454','data':["
            Dim d = dt.Where(Function(x) x.LABELNAME = labels(i).ToString()).Select(Function(x) New With {Key .axisValue = x.AXISVAL, Key .dataValue = x.DATAVAL, Key .axisName = x.AXISNAME})

            For Each item In d
                message += "[" & item.axisValue & "," + item.dataValue & "],"

                If i = 0 Then
                    axis += "[" & item.axisValue & ",'" + item.axisValue & "'],"
                End If
            Next

            message = message.Substring(0, message.Length - 1) & "], 'bars': {'show': true,  'clickable': true, 'align': 'center', 'barWidth' : 1, 'fillColor': '#fdbff9' , 'lineWidth':1 }}, { 'color':'#525454','data': [[0.5,0],"
            Dim e = dt.Where(Function(x) x.LABELNAME = labels(i).ToString()).Select(Function(x) New With {Key .axisValue = x.AXISVAL2, Key .dataValue = x.DATAVAL2, Key .axisName = x.AXISNAME})

            For Each item In e
                message += "[" & item.axisValue & "," + item.dataValue & "],"
            Next

            message = message.Substring(0, message.Length - 1) & "],  'yaxis': 2,  'points': { 'symbol': 'circle', 'fillColor': '#5fa9f5','radius' : 1, 'show': true }, 'lines': {'show':true,'lineWidth':0.5}},"

        Next

        message = message.Substring(0, message.Length - 1)
        message += "]"
        axis = axis.Substring(0, axis.Length - 1)
        axis += "]"


        Dim chart As clsTroubleHistoryByLineChartSetting = New clsTroubleHistoryByLineChartSetting()
        chart.data = message
        chart.xaxisTicks = axis
        chart.Max = max.ToString

        Return chart
    End Function

    <WebMethod()>
    Public Shared Function GetTableItemCheck(ByVal Factory As String, ByVal Type As String, ByVal Line As String, ByVal DateFrom As String, ByVal DateTo As String) As List(Of clsTroubleHistoryByLineTableData)
        Dim UserID = HttpContext.Current.Session("user")
        Dim db As clsTroubleHistoryByLineDB = New clsTroubleHistoryByLineDB()
        Dim dt As List(Of clsTroubleHistoryByLineTableData) = db.GetTableItemCheck(Factory, Type, Line, DateFrom, DateTo, UserID)

        Return dt
    End Function
#End Region

End Class