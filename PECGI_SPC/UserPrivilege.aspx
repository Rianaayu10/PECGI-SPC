﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UserPrivilege.aspx.vb" Inherits="PECGI_SPC.UserPrivilege" %>

<%--<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserPrivilege.aspx.vb" Inherits="PECGI_SPC.UserPrivilege" %>
<%@ Register Namespace="DevExpress.Web" TagPrefix="ASPxCallbackPanel" %>--%>
<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function OnEndCallback(s, e) {
            if (s.cp_message != "" && s.cp_val == 1) {

                if (s.cp_type == "Success" && s.cp_val == 1) {
                    toastr.success(s.cp_message, 'Success');
                    toastr.options.closeButton = false;
                    toastr.options.debug = false;
                    toastr.options.newestOnTop = false;
                    toastr.options.progressBar = false;
                    toastr.options.preventDuplicates = true;
                    toastr.options.onclick = null;
                    s.cp_val = 0;
                    s.cp_message = "";
                }
                else if (s.cp_type == "ErrorMsg" && s.cp_val == 1) {
                    toastr.error(s.cp_message, 'Error');
                    toastr.options.closeButton = false;
                    toastr.options.debug = false;
                    toastr.options.newestOnTop = false;
                    toastr.options.progressBar = false;
                    toastr.options.preventDuplicates = true;
                    toastr.options.onclick = null;
                    s.cp_val = 0;
                    s.cp_message = "";

                }
                else if (s.cp_type == "Warning" && s.cp_val == 1) {
                    toastr.warning(s.cp_message, 'Warning');
                    toastr.options.closeButton = false;
                    toastr.options.debug = false;
                    toastr.options.newestOnTop = false;
                    toastr.options.progressBar = false;
                    toastr.options.preventDuplicates = true;
                    toastr.options.onclick = null;
                    s.cp_val = 0;
                    s.cp_message = "";
                }
                else if (s.cp_message == "" && s.cp_val == 0) {
                    toastr.options.closeButton = false;
                    toastr.options.debug = false;
                    toastr.options.newestOnTop = false;
                    toastr.options.progressBar = false;
                    toastr.options.preventDuplicates = true;
                    toastr.options.onclick = null;
                }

            }
        }
        function OnBatchEditStartEditing(s, e) {
            currentColumnName = e.focusedColumn.fieldName;
            if (currentColumnName == "GroupID" || currentColumnName == "MenuID" || currentColumnName == "MenuDesc") {
                e.cancel = true;
            }
            currentEditableVisibleIndex = e.visibleIndex;
        }

        function OnAccessCheckedChanged(s, e) {
            gridMenu.SetFocusedRowIndex(-1);
            if (s.GetValue() == -1) s.SetValue(1);
            for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
                if (gridMenu.batchEditApi.GetCellValue(i, "AllowAccess", false) != s.GetValue()) {
                    gridMenu.batchEditApi.SetCellValue(i, "AllowAccess", s.GetValue());
                }
            }
        }

        function OnUpdateCheckedChanged(s, e) {
            if (s.GetValue() == -1) s.SetValue(1);
            gridMenu.SetFocusedRowIndex(-1);
            for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
                if (gridMenu.batchEditApi.GetCellValue(i, "AllowUpdate", false) != s.GetValue()) {
                    gridMenu.batchEditApi.SetCellValue(i, "AllowUpdate", s.GetValue());
                }
            }
        }

        function OnDeleteCheckedChanged(s, e) {
            if (s.GetValue() == -1) s.SetValue(1);
            gridMenu.SetFocusedRowIndex(-1);
            for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
                if (gridMenu.batchEditApi.GetCellValue(i, "AllowDelete", false) != s.GetValue()) {
                    gridMenu.batchEditApi.SetCellValue(i, "AllowDelete", s.GetValue());
                }
            }
        }

        function OnBatchEditStartEditing(s, e) {
            currentColumnName = e.focusedColumn.fieldName;
            if (currentColumnName == "GroupID" || currentColumnName == "MenuID" || currentColumnName == "MenuDesc") {  /*cbkValid.PerformCallback('save|' + cboUser.GetText());*/
                e.cancel = true;
            }
            currentEditableVisibleIndex = e.visibleIndex;
        }

        function SaveData(s, e) {        
            cbkValid.PerformCallback('save|' + cboUser.GetText() + '|' + txtUser.GetText());
        }

        function SavePrivilege(s, e) {
            gridMenu.UpdateEdit();
            millisecondsToWait = 1000;
            setTimeout(function () {
                gridMenu.PerformCallback('save|' + txtUser.GetText());
            }, millisecondsToWait);
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 5px 5px 5px 5px">
        <div style="padding: 5px 5px 5px 5px">
            <div class="widget-body">
                <div>
                    <table>
                        <tr style="height: 30px">
                            <td style="width: 120px">&nbsp;
                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="User">
                                    </dx:ASPxLabel>
                            </td>
                            <td>
                                <%--<dx:ASPxComboBox ID="txtUser" runat="server" Font-Names="Segoe UI"
                                    Font-Size="8pt" Theme="Office2010Black" DataSourceID="dsUser"
                                    EnableTheming="True" TextField="UserID" TextFormatString="{0}"
                                    ValueField="UserID" ClientInstanceName="txtUser">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                              gridMenu.PerformCallback('load|' + txtUser.GetText());
                                             }" />
                                    <Columns>
                                        <dx:ListBoxColumn Caption="User ID" FieldName="UserID" Width="60px" />
                                        <dx:ListBoxColumn Caption="Full Name" FieldName="FullName" Width="120px" />
                                    </Columns>
                                </dx:ASPxComboBox>--%>

                                <dx:ASPxTextBox ID="txtUser" runat="server" ClientInstanceName="txtUser"
                                    Width="170px" ReadOnly="True" Font-Names="Segoe UI" Font-Size="8pt">
                                    <ReadOnlyStyle BackColor="WhiteSmoke">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>

                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td>&nbsp;
                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Copy Privileges From">
                                    </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cboUser" runat="server" Font-Names="Segoe UI"
                                    Font-Size="8pt" Theme="Office2010Black" DataSourceID="dsUser"
                                    EnableTheming="True" TextField="UserID" TextFormatString="{0}"
                                    ValueField="UserID" ClientInstanceName="cboUser">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                gridMenu.PerformCallback('load|' + cboUser.GetText());
                                                                }" />
                                    <Columns>
                                        <dx:ListBoxColumn Caption="User ID" FieldName="UserID" Width="60px" />
                                        <dx:ListBoxColumn Caption="Full Name" FieldName="FullName" Width="120px" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 10px">
                    <asp:SqlDataSource ID="dsUser" runat="server" ConnectionString="<%$ConnectionStrings:ApplicationServices %>"
                        SelectCommand="select UserID, FullName from spc_UserSetup order by UserID"></asp:SqlDataSource>
                </div>

                <dx:ASPxGridView ID="gridMenu" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridMenu"
                    Font-Names="Segoe UI" Font-Size="8pt" KeyFieldName="MenuID" Theme="Office2010Black"
                    Width="100%">
                    <ClientSideEvents
                        BatchEditStartEditing="OnBatchEditStartEditing"
                        EndCallback="OnEndCallback" CallbackError="function(s, e) {
	                                        e.Cancel=True;
                                        }" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Menu Group" FieldName="GroupID" Name="GroupID"
                            ReadOnly="True" VisibleIndex="0" Width="200px">
                            <CellStyle Font-Names="Segoe UI" Font-Size="8pt">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Menu ID" FieldName="MenuID" Name="MenuID"
                            VisibleIndex="1" Width="100px">
                            <CellStyle Font-Names="Segoe UI" Font-Size="8pt" HorizontalAlign="Left">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Menu Name" FieldName="MenuDesc"
                            Name="MenuDesc" VisibleIndex="2" Width="320px">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataCheckColumn Caption="" FieldName="AllowAccess" Name="Access"
                            VisibleIndex="3" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String"
                                ValueUnchecked="0" AllowGrayedByClick="false">
                            </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                                <dx:ASPxCheckBox ID="chkAccess" runat="server" ClientInstanceName="chkAccess" ClientSideEvents-CheckedChanged="OnAccessCheckedChanged" ValueType="System.String" ValueChecked="1" ValueUnchecked="0" Text="Access" Font-Names="Segoe UI" Font-Size="8pt" ForeColor="White">
                                </dx:ASPxCheckBox>
                            </HeaderCaptionTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>

                        <dx:GridViewDataCheckColumn Caption="" FieldName="AllowUpdate" Name="Update"
                            VisibleIndex="4" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String"
                                ValueUnchecked="0" AllowGrayedByClick="false">
                            </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                                <dx:ASPxCheckBox ID="chkUpdate" runat="server" ClientInstanceName="chkUpdate" ClientSideEvents-CheckedChanged="OnUpdateCheckedChanged" ValueType="System.String" ValueChecked="1" ValueUnchecked="0" Text="Update" Font-Names="Segoe UI" Font-Size="8pt" ForeColor="White">
                                </dx:ASPxCheckBox>
                            </HeaderCaptionTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>

                        <dx:GridViewDataCheckColumn Caption="" FieldName="AllowDelete" Name="Delete"
                            VisibleIndex="4" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String"
                                ValueUnchecked="0" AllowGrayedByClick="false">
                            </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                                <dx:ASPxCheckBox ID="chkDelete" runat="server" ClientInstanceName="chkDelete" ClientSideEvents-CheckedChanged="OnDeleteCheckedChanged" ValueType="System.String" ValueChecked="1" ValueUnchecked="0" Text="Delete" Font-Names="Segoe UI" Font-Size="8pt" ForeColor="White">
                                </dx:ASPxCheckBox>
                            </HeaderCaptionTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>

                    </Columns>
                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" ColumnResizeMode="Control" EnableRowHotTrack="True" />
                    <SettingsPager Mode="ShowAllRecords" NumericButtonCount="10">
                    </SettingsPager>
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="False" />
                    </SettingsEditing>
                    <Settings HorizontalScrollBarMode="Visible" ShowStatusBar="Hidden" ShowVerticalScrollBar="True"
                        VerticalScrollableHeight="320" VerticalScrollBarMode="Visible" />
                    <Styles>
                        <Header HorizontalAlign="Center">
                            <Paddings PaddingBottom="5px" PaddingTop="5px" />
                        </Header>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                </dx:ASPxGridView>

                <div style="height: 10px">
                </div>
                <div align="right">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btnCancel" TabIndex="2" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel"
                                    Font-Names="Segoe UI" Font-Size="8pt" Text="Cancel" Theme="Office2010Silver"
                                    Width="80px">
                                    <ClientSideEvents Click="function close() {window.open('UserSetup.aspx', '_self' );}" />
                                </dx:ASPxButton>
                                &nbsp;&nbsp;
                                <dx:ASPxButton ID="btnSave" TabIndex="1" runat="server" AutoPostBack="False" ClientInstanceName="btnSave"
                                    Font-Names="Segoe UI" Font-Size="8pt" Text="Save" Theme="Office2010Silver"
                                    Width="80px">
                                    <ClientSideEvents Click="SaveData" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxCallback ID="cbkValid" runat="server" ClientInstanceName="cbkValid">
                                    <ClientSideEvents EndCallback="SavePrivilege" />
                                </dx:ASPxCallback>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

