Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Utils
Imports System.IO
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System.Drawing

Public Class UploadFTAMaster
    Inherits System.Web.UI.Page

#Region "Declare"
    Dim pUser As String = ""
    Public AuthAccess As Boolean = False
    Public AuthInsert As Boolean = False
    Public AuthUpdate As Boolean = False
    Public AuthDelete As Boolean = False
    Public dt As DataTable
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
#End Region

End Class