<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UserLine.aspx.vb" Inherits="PECGI_SPC.UserLine" %>

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
            if (currentColumnName == "LineID" || currentColumnName == "LineName") {
                e.cancel = true;
            }
            currentEditableVisibleIndex = e.visibleIndex;
        }

        function OnShowCheckedChanged(s, e) {
            gridMenu.SetFocusedRowIndex(-1);
            if (s.GetValue() == -1) s.SetValue(1);
            for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
                if (gridMenu.batchEditApi.GetCellValue(i, "AllowShow", false) != s.GetValue()) {
                    gridMenu.batchEditApi.SetCellValue(i, "AllowShow", s.GetValue());
                }
            }
        }

        function OnUpdateCheckedChanged(s, e) {
            gridMenu.SetFocusedRowIndex(-1);
            if (s.GetValue() == -1) s.SetValue(1);
            for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
                if (gridMenu.batchEditApi.GetCellValue(i, "AllowUpdate", false) != s.GetValue()) {
                    gridMenu.batchEditApi.SetCellValue(i, "AllowUpdate", s.GetValue());
                }
            }
        }

        function OnVerifyCheckedChanged(s, e) {
            gridMenu.SetFocusedRowIndex(-1);
            if (s.GetValue() == -1) s.SetValue(1);
            for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
                if (gridMenu.batchEditApi.GetCellValue(i, "AllowVerify", false) != s.GetValue()) {
                    gridMenu.batchEditApi.SetCellValue(i, "AllowVerify", s.GetValue());
                }
            }
        }

        function SaveData(s, e) {
            gridMenu.UpdateEdit();
            millisecondsToWait = 1000;
            setTimeout(function () {
                gridMenu.PerformCallback('save|');
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
                            <td style="width: 120px">&nbsp;<dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="User">
                            </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cboUserID" runat="server" Font-Names="Segoe UI"
                                    Font-Size="9pt" Theme="Office2010Black" EnableTheming="True" TextField="UserID" TextFormatString="{0}"
                                    ValueField="UserID" ClientInstanceName="cboUserID">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                                  var UserID = cboUserID.GetValue();
                                                  HideValue.Set('UserID', UserID);
	                                              gridMenu.PerformCallback('load|');
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
                <dx:ASPxGridView ID="gridMenu" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridMenu"
                    Font-Names="Segoe UI" Font-Size="9pt" KeyFieldName="LineID" Theme="Office2010Black"
                    Width="100%">
                    <ClientSideEvents BatchEditStartEditing="OnBatchEditStartEditing"
                        EndCallback="function(s, e) { 
	                                            gridMenu.CancelEdit();
                                            }"
                        CallbackError="function(s, e) {
	                                        e.Cancel=True;
                                        }" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Machine Process" FieldName="LineID"
                            VisibleIndex="1" Width="100px">
                            <CellStyle Font-Names="Segoe UI" Font-Size="9pt" HorizontalAlign="Left"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Description" FieldName="LineName"
                            VisibleIndex="2" Width="320px">
                            <CellStyle Font-Names="Segoe UI" Font-Size="9pt" HorizontalAlign="Left">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataCheckColumn Caption="Show" FieldName="AllowShow"
                            VisibleIndex="3" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String"
                                ValueUnchecked="0" AllowGrayedByClick="false">
                            </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                                <dx:ASPxCheckBox ID="chkShow" runat="server" ClientInstanceName="chkShow"
                                    ClientSideEvents-CheckedChanged="OnShowCheckedChanged" ValueType="System.String"
                                    ValueChecked="1" ValueUnchecked="0" Text="Show" Font-Names="Segoe UI"
                                    Font-Size="9pt" ForeColor="White">
                                </dx:ASPxCheckBox>
                            </HeaderCaptionTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>

                        <dx:GridViewDataCheckColumn Caption="Update" FieldName="AllowUpdate"
                            VisibleIndex="3" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String"
                                ValueUnchecked="0" AllowGrayedByClick="false">
                            </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                                <dx:ASPxCheckBox ID="chkUpdate" runat="server" ClientInstanceName="chkUpdate"
                                    ClientSideEvents-CheckedChanged="OnUpdateCheckedChanged" ValueType="System.String"
                                    ValueChecked="1" ValueUnchecked="0" Text="Update" Font-Names="Segoe UI"
                                    Font-Size="9pt" ForeColor="White">
                                </dx:ASPxCheckBox>
                            </HeaderCaptionTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>

                        <dx:GridViewDataCheckColumn Caption="Verify" FieldName="AllowVerify"
                            VisibleIndex="3" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String"
                                ValueUnchecked="0" AllowGrayedByClick="false">
                            </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                                <dx:ASPxCheckBox ID="chkVerify" runat="server" ClientInstanceName="chkVerify"
                                    ClientSideEvents-CheckedChanged="OnVerifyCheckedChanged" ValueType="System.String"
                                    ValueChecked="1" ValueUnchecked="0" Text="Verify" Font-Names="Segoe UI"
                                    Font-Size="9pt" ForeColor="White">
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
                        VerticalScrollableHeight="160" VerticalScrollBarMode="Visible" ShowFilterRow="true" />
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
                                <dx:ASPxButton ID="btnCancel" TabIndex="2" runat="server" AutoPostBack="False"
                                    ClientInstanceName="btnCancel"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="Back" Theme="Office2010Silver"
                                    Width="80px">
                                    <ClientSideEvents Click="function close() {window.open('UserSetup.aspx', '_self' );}" />
                                </dx:ASPxButton>
                                &nbsp;&nbsp;
                                <dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="False" ClientInstanceName="btnSave"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="Save" Theme="Office2010Silver"
                                    Width="80px">
                                    <ClientSideEvents Click="SaveData" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxHiddenField ID="HideValue" runat="server" ClientInstanceName="HideValue"></dx:ASPxHiddenField>
</asp:Content>

