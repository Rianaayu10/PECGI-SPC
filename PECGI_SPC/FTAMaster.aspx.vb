Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports DevExpress.XtraCharts.Native
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class FTAMaster
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Public AuthAccess As Boolean = False
    Public AuthInsert As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Dim FTAIDforUploadIK As String = ""
    Dim FTAIDAction As String = ""
    Public dt As DataTable
#End Region

#Region "Events"
    Private Sub Page_Init(ByVal sender As Object, ByVale As System.EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            GetFactoryCode()
            'GetItemCheck()
        End If
    End Sub

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

        sGlobal.getMenu("A050")
        Master.SiteTitle = sGlobal.idMenu & " - " & sGlobal.menuName

        pUser = Session("user")
        AuthAccess = sGlobal.Auth_UserAccess(pUser, "A050")
        If AuthAccess = False Then
            Response.Redirect("~/Main.aspx")
        End If

        AuthUpdate = sGlobal.Auth_UserUpdate(pUser, "A050")
        If AuthUpdate = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowEditButton = False
            commandColumn.ShowNewButtonInHeader = False
        End If

        AuthDelete = sGlobal.Auth_UserDelete(pUser, "A050")
        If AuthDelete = False Then
            Dim commandColumn = TryCast(Grid.Columns(0), GridViewCommandColumn)
            commandColumn.ShowDeleteButton = False
        End If

    End Sub

    Protected Sub Grid_AfterPerformCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles Grid.AfterPerformCallback
        If e.CallbackName <> "CANCELEDIT" Then
            up_GridLoad(cboFactory.Value, cboType.Value, cboLine.Text)
        End If
    End Sub
    Private Sub GetFactoryCode()
        'cboFactory.DataSource = clsFactoryDB.GetList
        cboFactory.DataSource = ClsFTAMasterDB.FillComboFactoryGrid("1", Session("user"))
        cboFactory.DataBind()
    End Sub
    Private Sub GetItemCheck()
        cboItemCheck.DataSource = ClsSPCItemCheckMasterDB.FillComboItemCheck("7")
        cboItemCheck.DataBind()
    End Sub
    Private Sub FillCBRegNoGrid()
        Dim DT As New DataTable
        DT = ClsFTAMasterDB.GetRegNo(cboFactory.Value)
        'Dim myCombo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
        'CType(TryCast(sender, ASPxGridView).Columns("RegistrationNo"), GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = DT
        'myCombo.DataSource = DT
        'myCombo.DataBindItems()

        Dim comboColumn = CType(Grid.Columns("RegistrationNo"), GridViewDataComboBoxColumn)
        comboColumn.PropertiesComboBox.DataSource = DT
        comboColumn.PropertiesComboBox.TextField = "Description"
        comboColumn.PropertiesComboBox.ValueField = "RegistrationNo"
        comboColumn.PropertiesComboBox.ValueType = GetType(String)

    End Sub
    Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles Grid.RowInserting
        e.Cancel = True
        Dim pErr As String = ""
        Dim FactoryCode As String = cboFactory.Value
        Dim ItemTypeCode As String = cboType.Value
        Dim LineCode As String = cboLine.Value
        Dim ItemCheck As String = cboItemCheck.Value
        Dim FTAMaster As New ClsFTAMaster With {
            .FactoryCode = FactoryCode,
            .ItemTypeCode = ItemTypeCode,
            .LineCode = LineCode,
            .ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -")),
            .FTAID = e.NewValues("FTAID"),
            .Factor1 = e.NewValues("Factor1"),
            .Factor2 = e.NewValues("Factor2"),
            .Factor3 = e.NewValues("Factor3"),
            .Factor4 = e.NewValues("Factor4"),
            .CounterMeasure = e.NewValues("CounterMeasure"),
            .CheckItem = e.NewValues("CheckItem"),
            .CheckOrder = e.NewValues("CheckOrder"),
            .Remark = e.NewValues("Remark"),
            .ActiveStatus = e.NewValues("ActiveStatus"),
            .UpdateUser = pUser,
            .CreateUser = pUser
        }
        Try

            Dim CheckFTAID As ClsFTAMaster = ClsFTAMasterDB.ValidateData(FTAMaster)
            If CheckFTAID IsNot Nothing Then
                show_error(MsgTypeEnum.ErrorMsg, "Can't insert data, FTA ID '" + CheckFTAID.FTAID + "' is already exist", 1)
                Return
            End If
            ClsFTAMasterDB.Insert(FTAMaster)
            Grid.CancelEdit()
            up_GridLoad(cboFactory.Value, cboType.Value, cboLine.Text)
            show_error(MsgTypeEnum.Success, "Save data successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles Grid.RowUpdating
        e.Cancel = True
        Dim pErr As String = ""

        Dim FactoryCode As String = cboFactory.Value
        Dim ItemTypeCode As String = cboType.Value
        Dim LineCode As String = cboLine.Value
        Dim ItemCheck As String = cboItemCheck.Value
        Dim FTAID As String = e.NewValues("FTAID")
        Dim Factor1 As String = e.NewValues("Factor1")
        Dim Factor2 As String = e.NewValues("Factor2")
        Dim Factor3 As String = e.NewValues("Factor3")
        Dim Factor4 As String = e.NewValues("Factor4")
        Dim CounterMeasure As String = e.NewValues("CounterMeasure")
        Dim CheckItem As String = e.NewValues("CheckItem")
        Dim CheckOrder As String = e.NewValues("CheckOrder")
        Dim Remark As String = e.NewValues("Remark")
        Dim ActiveStatus As String = e.NewValues("ActiveStatus")

        ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

        Dim FTAUpdate As New ClsFTAMaster With {
        .FactoryCode = FactoryCode,
        .ItemTypeCode = ItemTypeCode,
        .LineCode = LineCode,
        .ItemCheck = ItemCheck,
        .FTAID = FTAID,
        .Factor1 = Factor1,
        .Factor2 = Factor2,
        .Factor3 = Factor3,
        .Factor4 = Factor4,
        .CounterMeasure = CounterMeasure,
        .CheckItem = CheckItem,
        .CheckOrder = CheckOrder,
        .Remark = Remark,
        .ActiveStatus = ActiveStatus,
        .UpdateUser = pUser
        }
        Try

            ClsFTAMasterDB.Update(FTAUpdate)
            Grid.CancelEdit()
            up_GridLoad(cboFactory.Value, cboType.Value, cboLine.Text)
            show_error(MsgTypeEnum.Success, "Update data successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles Grid.RowDeleting
        e.Cancel = True
        Try
            Dim FactoryCode As String = cboFactory.Value
            Dim ItemTypeCode As String = cboType.Value
            Dim LineCode As String = cboLine.Value
            Dim ItemCheck As String = cboItemCheck.Value
            Dim FTAID As String = e.Values("FTAID")

            ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))

            Dim CheckFTAID As ClsFTAMaster = ClsFTAMasterDB.ValidateDataFTAIDinC010(FTAID)
            If CheckFTAID IsNot Nothing Then
                show_error(MsgTypeEnum.ErrorMsg, "Can't Delete, FTA ID '" + FTAID + "' has been used in Corrective Action SPC Process", 1)
                Return
            End If

            Dim CountDel As Integer = ClsFTAMasterDB.Delete(FactoryCode, ItemTypeCode, ItemCheck, FTAID)
            Grid.CancelEdit()
            up_GridLoad(cboFactory.Value, cboType.Value, cboLine.Text)

            If CountDel > 0 Then
                show_error(MsgTypeEnum.Success, "Delete data successfully!", 1)
            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub

    Private Sub Grid_BeforeGetCallbackResult(ByVal sender As Object, ByVal e As System.EventArgs) Handles Grid.BeforeGetCallbackResult
        If Grid.IsNewRowEditing Then
            Grid.SettingsCommandButton.UpdateButton.Text = "Save"
        End If
    End Sub
    Protected Sub Grid_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs) Handles Grid.RowValidating
        Dim dataCol As New GridViewDataColumn
        Dim AdaError As Boolean = False

        For Each column As GridViewColumn In Grid.Columns
            Dim dataColumn As GridViewDataColumn = TryCast(column, GridViewDataColumn)
            If dataColumn Is Nothing Then
                Continue For
            End If

            If dataColumn.FieldName = "FTAID" Then
                If IsNothing(e.NewValues("FTAID")) OrElse e.NewValues("FTAID").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input FTAID!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "CounterMeasure" Then
                If IsNothing(e.NewValues("CounterMeasure")) OrElse e.NewValues("CounterMeasure").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input CounterMeasure!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "CheckItem" Then
                If IsNothing(e.NewValues("CheckItem")) OrElse e.NewValues("CheckItem").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Check Item!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            If dataColumn.FieldName = "CheckOrder" Then
                If IsNothing(e.NewValues("CheckOrder")) OrElse e.NewValues("CheckOrder").ToString.Trim = "" Then
                    e.Errors(dataColumn) = "Please Input Check Order!"
                    show_error(MsgTypeEnum.Warning, "Please fill in all required fields!", 1)
                    AdaError = True
                End If
            End If

            'If dataColumn.FieldName = "LastUser" Then
            '    dataCol = dataColumn
            'End If
        Next column


    End Sub

    Protected Sub Grid_StartRowEditing(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs) Handles Grid.StartRowEditing
        If (Not Grid.IsNewRowEditing) Then
            Grid.DoRowValidation()
        End If
        show_error(MsgTypeEnum.Info, "", 0)
    End Sub
#End Region

#Region "Functions"
    Private Sub show_error(ByVal msgType As MsgTypeEnum, ByVal ErrMsg As String, ByVal pVal As Integer)
        Grid.JSProperties("cp_message") = ErrMsg
        Grid.JSProperties("cp_type") = msgType
        Grid.JSProperties("cp_val") = pVal
    End Sub

    Private Sub up_GridLoad(FactoryCode As String, ItemType As String, LineCode As String)
        Dim dtFTAMaster As DataTable
        Dim ItemCheck As String = ""
        Try
            If FactoryCode Is Nothing Then
                FactoryCode = ""
            End If
            If ItemType Is Nothing Then
                ItemType = ""
            End If
            If LineCode Is Nothing Then
                LineCode = ""
            End If

            If LineCode <> "ALL" AndAlso LineCode <> "" Then
                LineCode = LineCode.Substring(0, LineCode.IndexOf(" -"))
            End If

            If cboItemCheck.Value IsNot Nothing Then
                ItemCheck = cboItemCheck.Value.Substring(0, cboItemCheck.Value.IndexOf(" -"))
            End If

            dtFTAMaster = ClsFTAMasterDB.GetList(FactoryCode, ItemCheck, ItemType)
            Grid.DataSource = dtFTAMaster
            Grid.DataBind()

            hdUserLogin.Value = pUser
            hdFactoryCode.Value = FactoryCode
            hdItemTypeCode.Value = cboType.Value

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub Grid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles Grid.CustomCallback
        Try
            Dim pAction As String = Split(e.Parameters, "|")(0)

            If pAction = "Load" Then
                up_GridLoad(cboFactory.Value, cboType.Value, cboLine.Text)
            End If

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub cboType_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboType.Callback
        cboType.DataSource = ClsFTAMasterDB.GetListItemType()
        cboType.DataBind()
    End Sub
    Private Sub cboLine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLine.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim Machine As String = cboMachine.Value
        Dim UserID As String = Session("user") & ""
        cboLine.DataSource = ClsFTAMasterDB.GetMachineProccess(UserID, FactoryCode, Machine)
        cboLine.DataBind()
    End Sub
    Private Sub up_FillcomboGrid(ByVal cmb As ASPxComboBox, Type As String, Optional Param As String = "")
        dt = ClsFTAMasterDB.FillComboFactoryGrid(Type, Param)
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
    Private Sub cboProcessGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboProcessGroup.Callback
        cboProcessGroup.DataSource = clsProcessGroupDB.GetList(pUser, cboFactory.Value)
        cboProcessGroup.DataBind()
    End Sub
    Private Sub cboLineGroup_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboLineGroup.Callback
        cboLineGroup.DataSource = clsLineGroupDB.GetList(pUser, cboFactory.Value, cboProcessGroup.Value)
        cboLineGroup.DataBind()
    End Sub
    Private Sub cboMachine_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboMachine.Callback
        cboMachine.DataSource = clsProcessDB.GetList(pUser, cboFactory.Value, cboProcessGroup.Value, cboLineGroup.Value)
        cboMachine.DataBind()
    End Sub
    Protected Sub ActionLink_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim templatecontainer As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim FTAID As String = ""
        FTAID = templatecontainer.Grid.GetRowValues(templatecontainer.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpAction('" + FTAID + "');}"
            FTAIDAction = FTAID
        End If
    End Sub
    Protected Sub linkUploadIK_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim templatecontainer As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim FTAID As String = ""
        FTAID = templatecontainer.Grid.GetRowValues(templatecontainer.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpUploadIK('" + FTAID + "');}"
            'Session("FTA_ID") = FTAID
        End If
    End Sub
    Protected Sub IKLink_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As DevExpress.Web.ASPxHyperLink = CType(sender, DevExpress.Web.ASPxHyperLink)
        Dim templatecontainer As GridViewDataItemTemplateContainer = CType(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.NavigateUrl = "javascript:void(0)"
        link.ForeColor = Color.SteelBlue

        Dim FTAID As String = ""
        FTAID = templatecontainer.Grid.GetRowValues(templatecontainer.VisibleIndex, "FTAID") & ""
        If FTAID <> "" Then
            link.ClientSideEvents.Click = "function (s,e) {ShowPopUpIK('" + FTAID + "');}"
        End If
    End Sub
    Private Sub cbkPanel_Callback(sender As Object, e As CallbackEventArgsBase) Handles cbkPanel.Callback
        Dim s As String = e.Parameter
        ShowIK(s)
    End Sub
    Public Sub ShowIK(FTAID As String)
        Dim Img As Object = clsFTAResultDB.GetIK(FTAID)
        Dim ImageUrl As String = "~/img/noimage.png"
        If Img IsNot Nothing AndAlso Not IsDBNull(Img) Then
            Dim fcByte As Byte() = Nothing
            fcByte = Img
            ImageUrl = "data:image;base64," + Convert.ToBase64String(fcByte)
        End If
        imgIK.ImageUrl = ImageUrl
    End Sub
    Private Sub cbkPanelUploadIK_Callback(sender As Object, e As CallbackEventArgsBase) Handles cbkPanelUploadIK.Callback
        Dim s As String = e.Parameter
        lblMsgUpload.Text = ""
        lblMsgUpload.ForeColor = Color.Black
        Session("FTA_ID") = s
    End Sub
    Protected Sub btnUploadIK_Click(sender As Object, e As EventArgs) Handles btnUploadIK.Click
        Try
            Dim PostedFile As HttpPostedFile = updIK.PostedFile
            Dim FileName As String = Path.GetFileName(PostedFile.FileName)
            Dim FileExtension As String = Path.GetExtension(FileName)
            Dim Stream As Stream = PostedFile.InputStream
            Dim Br As BinaryReader = New BinaryReader(Stream)
            Dim Bytes As Byte() = Br.ReadBytes(Stream.Length)

            'LineCode = e.NewValues("LineName")
            '.LineCode = LineCode.Substring(0, LineCode.IndexOf(" -")),
            '.ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -")),

            Dim ItemCheck = cboItemCheck.Value
            Dim UploadIK As New ClsFTAMaster
            UploadIK.FactoryCode = cboFactory.Value
            UploadIK.ItemTypeCode = cboType.Value
            UploadIK.LineCode = cboLine.Value
            UploadIK.FTAID = Session("FTA_ID")
            UploadIK.ItemCheck = ItemCheck.Substring(0, ItemCheck.IndexOf(" -"))
            If Bytes.Length > 0 Then
                UploadIK.Image = Bytes
            End If
            UploadIK.UpdateUser = pUser

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ClosePopupUploadIK();", True)

            Dim i As Integer = ClsFTAMasterDB.UploadIK_UpdIns(UploadIK)
            If i >= 1 Then
                lblMsgUpload.Text = "Upload Success"
                lblMsgUpload.ForeColor = Color.Green
            Else
                lblMsgUpload.Text = "Upload Failed"
                lblMsgUpload.ForeColor = Color.Red
            End If

            'up_GridLoad(Menu.CatererID, MenuType, ActiveStatus, ShowPicture)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub cbkPanelAction_Callback(sender As Object, e As CallbackEventArgsBase) Handles cbkPanelAction.Callback
        Dim s As String = e.Parameter
        BindGridAction(s)
    End Sub
    Public Sub BindGridAction(FTAID As String)
        Dim dtFTAMaster As DataTable
        Try

            dtFTAMaster = ClsFTAMasterDB.GetListActionByFTAID(FTAID)
            gvFTAAction.DataSource = dtFTAMaster
            gvFTAAction.DataBind()
            Session("FTA_ID") = FTAID

        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub gvFTAAction_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles gvFTAAction.RowInserting
        e.Cancel = True
        Try
            Dim FTAIDAction = e.NewValues("FTAIDAction")
            Dim ActionName = e.NewValues("ActionName")
            Dim RemarkAction = e.NewValues("RemarkAction")

            ClsFTAMasterDB.InsertAction(FTAIDAction, ActionName, RemarkAction)
            gvFTAAction.CancelEdit()
            BindGridAction(FTAIDAction)
            show_error(MsgTypeEnum.Success, "Save data action successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub gvFTAAction_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs) Handles gvFTAAction.RowDeleting
        e.Cancel = True
        Try
            Dim FTAIDAction = e.Values("FTAIDAction")
            Dim ActionName = e.Values("ActionName")
            Dim RemarkAction = e.Values("RemarkAction")
            Dim ActionID = e.Values("ActionID")

            ClsFTAMasterDB.DeleteAction(FTAIDAction, actionID)
            gvFTAAction.CancelEdit()
            BindGridAction(FTAIDAction)
            show_error(MsgTypeEnum.Success, "Delete data action successfully!", 1)
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Protected Sub gvFTAAction_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gvFTAAction.RowUpdating
        e.Cancel = True
        Try
            Dim FTAIDAction = e.NewValues("FTAIDAction")
            Dim ActionName = e.NewValues("ActionName")
            Dim RemarkAction = e.NewValues("RemarkAction")
            Dim ActionID = Session("ActionID")


            Dim UpdateAction = ClsFTAMasterDB.UpdateFTAAction(FTAIDAction, ActionName, ActionID, RemarkAction)
            gvFTAAction.CancelEdit()
            BindGridAction(FTAIDAction)

            If UpdateAction >= 1 Then
                show_error(MsgTypeEnum.Success, "Update data action successfully!", 1)
                Session("ActionID") = ""
            End If
        Catch ex As Exception
            show_error(MsgTypeEnum.ErrorMsg, ex.Message, 1)
        End Try
    End Sub
    Private Sub gvFTAAction_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles gvFTAAction.CellEditorInitialize

        If gvFTAAction.IsNewRowEditing Then

            If e.Column.FieldName = "FTAIDAction" Then
                e.Editor.Value = Session("FTA_ID")
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = True
            ElseIf e.Column.FieldName = "ActionID" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = False
            Else
                e.Editor.ReadOnly = False
            End If

        End If
        If Not gvFTAAction.IsNewRowEditing Then

            If e.Column.FieldName = "FTAIDAction" Then
                e.Editor.Value = Session("FTA_ID")
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = True
            ElseIf e.Column.FieldName = "ActionID" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = False
                Session("ActionID") = gvFTAAction.GetRowValues(e.VisibleIndex, "ActionID")
            End If

        End If


    End Sub
    Private Sub Grid_CellEditorInitialize(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles Grid.CellEditorInitialize

        If Not Grid.IsNewRowEditing Then

            If e.Column.FieldName = "FTAID" Then
                e.Editor.ReadOnly = True
                e.Editor.ForeColor = Color.Silver
                e.Editor.Visible = True
            End If

        End If

        If Grid.IsNewRowEditing Then

            If e.Column.FieldName = "FTAID" Then

                If cboItemCheck.Value <> "" OrElse Not IsNothing(cboItemCheck.Value) Then
                    Dim ItemCheck = cboItemCheck.Value
                    e.Editor.Value = ItemCheck.Substring(0, ItemCheck.IndexOf(" -")) + "-"
                End If

            End If

        End If


    End Sub
    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        DownloadExcel()
    End Sub
    Private Sub DownloadExcel()
        Dim ps As New PrintingSystem()
        Dim compositeLink As New CompositeLink(ps)

        Using Pck As New ExcelPackage
            Dim ws As ExcelWorksheet = Pck.Workbook.Worksheets.Add("Sheet1")
            With ws
                Dim iDay As Integer = 2
                Dim iCol As Integer = 2
                Dim lastCol As Integer = 1
                Dim Seq As Integer = 0

                Dim Hdr As New ClsFTAMaster
                Hdr.FactoryCode = cboFactory.Value
                Hdr.FactoryName = cboFactory.Text
                Hdr.ItemTypeCode = cboType.Value
                Hdr.ItemTypeName = cboType.Text
                Hdr.LineCode = cboLine.Value
                Hdr.LineName = cboLine.Text
                Hdr.ItemCheck = cboItemCheck.Text
                Hdr.ProcessGroup = cboProcessGroup.Text
                Hdr.LineGroup = cboLineGroup.Text
                Hdr.Machine = cboMachine.Text

                GridTitle(ws, Hdr)
                GridExcel(ws, Hdr)

            End With

            Dim stream As MemoryStream = New MemoryStream(Pck.GetAsByteArray())
            Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            Response.AppendHeader("Content-Disposition", "attachment; filename=FTAMaster_" & Format(Date.Now, "yyyy-MM-dd") & ".xlsx")
            Response.BinaryWrite(stream.ToArray())
            Response.End()

        End Using
    End Sub
    Private Sub GridTitle(ByVal pExl As ExcelWorksheet, cls As ClsFTAMaster)
        With pExl
            Try
                .Cells(1, 1).Value = "FTA Master"
                .Cells(1, 1, 1, 13).Merge = True
                .Cells(1, 1, 1, 13).Style.HorizontalAlignment = HorzAlignment.Near
                .Cells(1, 1, 1, 13).Style.VerticalAlignment = VertAlignment.Center
                .Cells(1, 1, 1, 13).Style.Font.Bold = True
                .Cells(1, 1, 1, 13).Style.Font.Size = 16
                .Cells(1, 1, 1, 13).Style.Font.Name = "Segoe UI"

                .Cells(3, 1, 3, 1).Value = "Factory"
                .Cells(3, 2).Value = ": " & cls.FactoryName

                .Cells(4, 1, 4, 1).Value = "Process Group"
                .Cells(4, 2).Value = ": " & cls.ProcessGroup

                .Cells(5, 1, 5, 1).Value = "Line Group"
                .Cells(5, 2).Value = ": " & cls.LineGroup

                .Cells(6, 1, 6, 1).Value = "Machine"
                .Cells(6, 2).Value = ": " & cls.Machine

                .Cells(7, 1, 7, 1).Value = "Machine Process"
                .Cells(7, 2).Value = ": " & cls.LineName

                .Cells(8, 1, 8, 1).Value = "Type"
                .Cells(8, 2).Value = ": " & cls.ItemTypeName

                .Cells(9, 1, 9, 1).Value = "Item Check"
                .Cells(9, 2).Value = ": " & cls.ItemCheck

                Dim rgHdr As ExcelRange = .Cells(3, 3, 9, 4)
                rgHdr.Style.HorizontalAlignment = HorzAlignment.Near
                rgHdr.Style.VerticalAlignment = VertAlignment.Center
                rgHdr.Style.Font.Size = 10
                rgHdr.Style.Font.Name = "Segoe UI"
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End With
    End Sub
    Private Sub GridExcel(pExl As ExcelWorksheet, Hdr As ClsFTAMaster)

        Dim ItemCheck As String = ""
        Dim LineCode As String = ""
        Dim FTAID As String = ""
        Dim Factor1 As String = ""
        Dim Factor2 As String = ""
        Dim Factor3 As String = ""
        Dim Factor4 As String = ""
        Dim CounterMeasure As String = ""
        Dim CheckItem As String = ""
        Dim CheckOrder As String = ""
        Dim Remark As String = ""
        Dim ActiveStatus As String = ""
        Dim LastUser As String = ""
        Dim LastUpdate As String = ""
        Dim iRow As Integer
        Dim StartRow As Integer = 12

        With pExl

            If cboItemCheck.Value IsNot Nothing Then
                ItemCheck = cboItemCheck.Value.Substring(0, cboItemCheck.Value.IndexOf(" -"))
            End If
            Dim ds As DataSet = ClsFTAMasterDB.GetListForExcel(cboFactory.Value, ItemCheck, cboType.Value)

            Dim dtFTA As DataTable = ds.Tables(0)

            iRow = 12
            .Cells(iRow, 1).Value = "FTA ID"
            .Cells(iRow, 2).Value = "Factor 1"
            .Cells(iRow, 3).Value = "Factor 2"
            .Cells(iRow, 4).Value = "Factor 3"
            .Cells(iRow, 5).Value = "Factor 4"
            .Cells(iRow, 6).Value = "Counter Measure"
            .Cells(iRow, 7).Value = "Check Item"
            .Cells(iRow, 8).Value = "Check Order"
            .Cells(iRow, 9).Value = "Remark"
            .Cells(iRow, 10).Value = "Active Status"
            .Cells(iRow, 11).Value = "Last User"
            .Cells(iRow, 12).Value = "Last Update"


            .Column(1).Width = 20
            .Column(2).Width = 40
            .Column(3).Width = 40
            .Column(4).Width = 40
            .Column(5).Width = 40
            .Column(6).Width = 40
            .Column(7).Width = 40
            .Column(8).Width = 10
            .Column(9).Width = 40
            .Column(10).Width = 10
            .Column(11).Width = 20
            .Column(12).Width = 20

            For excelFTA = 0 To dtFTA.Rows.Count - 1

                iRow = iRow + 1

                .Cells(iRow, 1).Value = dtFTA.Rows(excelFTA)("FTAID")
                .Cells(iRow, 2).Value = dtFTA.Rows(excelFTA)("Factor1")
                .Cells(iRow, 3).Value = dtFTA.Rows(excelFTA)("Factor2")
                .Cells(iRow, 4).Value = dtFTA.Rows(excelFTA)("Factor3")
                .Cells(iRow, 5).Value = dtFTA.Rows(excelFTA)("Factor4")
                .Cells(iRow, 6).Value = dtFTA.Rows(excelFTA)("CounterMeasure")
                .Cells(iRow, 7).Value = dtFTA.Rows(excelFTA)("CheckItem")
                .Cells(iRow, 8).Value = dtFTA.Rows(excelFTA)("CheckOrder")
                .Cells(iRow, 9).Value = dtFTA.Rows(excelFTA)("Remark")
                .Cells(iRow, 10).Value = dtFTA.Rows(excelFTA)("ActiveStatus")
                .Cells(iRow, 11).Value = dtFTA.Rows(excelFTA)("UpdateUser")
                .Cells(iRow, 12).Value = dtFTA.Rows(excelFTA)("UpdateDate").ToString()


            Next

            ExcelHeader(pExl, StartRow, 1, StartRow, 12)
            ExcelBorder(pExl, StartRow, 1, iRow, 12)
            ExcelFont(pExl, StartRow, 1, iRow, 12, 9)
            'LastRow = iRow + 1
        End With
    End Sub
    Private Sub ExcelHeader(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer)
        With Exl
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Fill.PatternType = ExcelFillStyle.Solid
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#878787"))
            .Cells(StartRow, StartCol, EndRow, EndCol).Style.Font.Color.SetColor(Color.White)
        End With
    End Sub
    Private Sub ExcelBorder(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer)
        With Exl
            Dim Range As ExcelRange = .Cells(StartRow, StartCol, EndRow, EndCol)
            Range.Style.Border.Top.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Right.Style = ExcelBorderStyle.Thin
            Range.Style.Border.Left.Style = ExcelBorderStyle.Thin
            Range.Style.Font.Size = 10
            Range.Style.Font.Name = "Segoe UI"
            Range.Style.HorizontalAlignment = HorzAlignment.Center
        End With
    End Sub
    Private Sub ExcelFont(Exl As ExcelWorksheet, StartRow As Integer, StartCol As Integer, EndRow As Integer, EndCol As Integer, FontSize As Integer)
        With Exl
            Dim Range As ExcelRange = .Cells(StartRow, StartCol, EndRow, EndCol)
            Range.Style.Font.Size = FontSize
            Range.Style.Font.Name = "Segoe UI"
        End With
    End Sub
    Private Sub cboItemCheck_Callback(sender As Object, e As CallbackEventArgsBase) Handles cboItemCheck.Callback
        Dim FactoryCode As String = Split(e.Parameter, "|")(0)
        Dim ItemTypeCode As String = Split(e.Parameter, "|")(1)
        Dim LineCode As String = Split(e.Parameter, "|")(2)
        cboItemCheck.DataSource = clsItemCheckDB.GetList(FactoryCode, ItemTypeCode, LineCode)
        cboItemCheck.DataBind()
    End Sub
#End Region

End Class