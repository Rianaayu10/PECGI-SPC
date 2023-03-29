<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadFTAMaster.aspx.vb" Inherits="PECGI_SPC.UploadFTAMaster" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .ButtonStyleNo * {
            background-color: #003fbe;
            color: #fff;
        }
    </style>

    <script type="text/javascript">

        function cboFactoryChanged(s, e) {
            cboType.SetEnabled(true);
            cboType.PerformCallback(cboFactory.GetValue());
        }
        function OnEndCallback(s, e) {
            lpProgress.Hide();
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
                lpProgress.Hide();
            }
            if (s.cp_Memo == 1) {
                var dt = new Date();
                var date = dt.getDate();
                var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][dt.getMonth()];
                var year = dt.getFullYear();
                var hours = dt.getHours();
                var min = dt.getMinutes();
                var sec = dt.getSeconds();
                var str = 'Process End ' + date + ' ' + month + ' ' + year + ' ' + hours + ':' + min + ':' + sec;
                var p = str;
                ASPxMemo1.SetText(t + s.cp_MemoValue + p);
                //pbProgressing.SetPosition(100);
                lpProgress.Hide();
                s.cp_Memo = 0;
            }

            if (s.cp_AlertFileNotValid == 1) {
                toastr.warning('File format is not valid!', 'Warning');
                //                ASPxMemo1.SetText(s.cp_MemoValue);
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;
                e.processOnServer = false;
                s.cp_AlertFileNotValid = 0;
                var dt = new Date();
                var date = dt.getDate();
                var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][dt.getMonth()];
                var year = dt.getFullYear();
                var hours = dt.getHours();
                var min = dt.getMinutes();
                var sec = dt.getSeconds();
                var str = 'Process End ' + date + ' ' + month + ' ' + year + ' ' + hours + ':' + min + ':' + sec;
                var p = str;
                ASPxMemo1.SetText(t + s.cp_MemoValue + p);
                lpProgress.Hide();
                return;
            }
            if (s.cp_AlertFileNotValid == 2) {
                toastr.error('Error upload file, cannot find Upload folder in server path!', 'Error');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;
                e.processOnServer = false;
                s.cp_AlertFileNotValid = 0;
                var dt = new Date();
                var date = dt.getDate();
                var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][dt.getMonth()];
                var year = dt.getFullYear();
                var hours = dt.getHours();
                var min = dt.getMinutes();
                var sec = dt.getSeconds();
                var str = 'Process End ' + date + ' ' + month + ' ' + year + ' ' + hours + ':' + min + ':' + sec;
                var p = str;
                ASPxMemo1.SetText(t + s.cp_MemoValue + p);
                lpProgress.Hide();
                return;
            }
        }

        function Clear() {
            cboFactory.SetValue('');
            cboType.SetValue('');
        }
        function ClickClear(s, e) {
            ASPxMemo1.SetText('');
        } //ClickClear
        function ClickUpload(s, e) {
            var dt = new Date();
            var date = dt.getDate();
            var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][dt.getMonth()]; s
            var year = dt.getFullYear();
            var hours = dt.getHours();
            var min = dt.getMinutes();
            var sec = dt.getSeconds();
            var str = 'Process start ' + date + ' ' + month + ' ' + year + ' ' + hours + ':' + min + ':' + sec + '\n\n';
            t = str;
            if (cboFactory.GetText() == '') {
                cboFactory.Focus();
                errmsg = 'Please select Factory!';
            } else if (cboType.GetText() == '') {
                cboType.Focus();
                errmsg = 'Please select Type!';
            } else {
                errmsg = '';
            }

            if (errmsg != '') {
                toastr.warning(errmsg, 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;
                e.processOnServer = false;
                return;
            }

            ASPxMemo1.SetText(str);
            //lpProgress.Show();
            var test = rdlThemeID1.GetValue();

            if (test == "FTAMaster") {
                uplFTAMaster.Upload();

            } else if (test == "FTAAction") {
                Uploader.Upload();
            }  
        }
        function EnableDisableUpload(s, e) {
            var test = rdlThemeID1.GetValue();

            if (test == "FTAMaster") {
                Uploader.SetEnabled(false);
                uplFTAMaster.SetEnabled(true);

            } else if (test == "FTAAction") {
                uplFTAMaster.SetEnabled(false);
                Uploader.SetEnabled(true);
            }

        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="divhead" style="padding: 0px 5px 5px 5px">
        <table>
            <tr style="height: 30px">
                <td style="width:60px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Factory">
                    </dx:ASPxLabel>
                </td>
                <td style=" width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxComboBox ID="cboFactory" runat="server" Theme="Office2010Black" TextField="FactoryName" ClientInstanceName="cboFactory" ValueField="FactoryCode" Font-Names="Segoe UI" 
                        Font-Size="9pt" Height="25px" IncrementalFilteringMode="Contains" Width="100px" TabIndex="6">
                        <ClientSideEvents SelectedIndexChanged="cboFactoryChanged" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" >
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr style="height: 30px">
                <td style="width:130px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Segoe UI" 
                        Font-Size="9pt" Text="Type">
                    </dx:ASPxLabel>
                </td>
                <td style=" width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxComboBox ID="cboType" runat="server" Theme="Office2010Black" TextField="Description" ClientInstanceName="cboType" ValueField="ItemTypeCode" Font-Names="Segoe UI" 
                        Font-Size="9pt" Height="25px" Width="100px" TabIndex="6" EnableCallbackMode="True">                    
                        <%--<ClientSideEvents  EndCallback="function(s, e) {cboType.SetEnabled(true);cbTypeValue.PerformCallback(cboType.GetValue());}"/>--%>
                        <ClientSideEvents ValueChanged="function(s, e){cbTypeValue.PerformCallback(cboType.GetValue());}"/>
                        <ItemStyle Height="10px" Paddings-Padding="4px" >
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr style="height: 30px">
                <td style="width:260px; padding:5px 0px 0px 0px" colspan="2">
                    <dx:ASPxRadioButtonList ID="rdlThemeID1" Width="260px" ItemSpacing="10px" Font-Names="Segoe UI" Font-Size="8pt" Theme="Office2010Black" runat="server" RepeatDirection="Horizontal" ClientInstanceName="rdlThemeID1" >                    
                        <Items>
                            <dx:ListEditItem Text=" Upload FTA Master" Value="FTAMaster"/>
                            <dx:ListEditItem Text=" Upload FTA Action" Value="FTAAction" />
                        </Items>
                        <ClientSideEvents SelectedIndexChanged="EnableDisableUpload" />
                        <Border BorderStyle="None" />
                    </dx:ASPxRadioButtonList>
                </td>
            </tr>
            <tr style="height: 30px">
                <td style=" width:130px; padding:5px 0px 0px 0px">
                    <%--<asp:FileUpload ID="ufFTAMaster" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Width="250px" Height="20px" />--%>
                    <dx:ASPxUploadControl ID="uplFTAMaster" runat="server"  
                        Width="80%" Font-Names="Verdana" Font-Size="8pt"
                        ClientInstanceName="uplFTAMaster"
                        ShowClearFileSelectionButton="False"
                        NullText="FTA Master browse"
                        OnFileUploadComplete="uplImage_FileUploadComplete">
                        <ClientSideEvents FileUploadComplete="OnEndCallback"></ClientSideEvents>
                        <ValidationSettings AllowedFileExtensions=".xls,.xlsx" MaxFileSize="4000000" />
                        <BrowseButton Text="...">
                        </BrowseButton> 
                        <BrowseButtonStyle Paddings-Padding="3px" >
                        </BrowseButtonStyle>
                    </dx:ASPxUploadControl>
                </td>
                <td style=" width:130px; padding:5px 0px 0px 0px">
                    <%--<asp:FileUpload ID="ufFTAAction" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Width="250px" Height="20px" />--%>
                    <dx:ASPxUploadControl ID="Uploader" runat="server"  
                        Width="80%" Font-Names="Verdana" Font-Size="8pt"
                        ClientInstanceName="Uploader"
                        ShowClearFileSelectionButton="False"
                        NullText="FTA Action browse"
                        OnFileUploadComplete="uplImage_FileUploadComplete">
                        <ClientSideEvents FileUploadComplete="OnEndCallback"></ClientSideEvents>
                        <ValidationSettings AllowedFileExtensions=".xls,.xlsx" MaxFileSize="4000000" />
                        <BrowseButton Text="...">
                        </BrowseButton> 
                        <BrowseButtonStyle Paddings-Padding="3px" >
                        </BrowseButtonStyle>
                    </dx:ASPxUploadControl>
                </td>
            </tr>
        </table>
    </div>


    <div style="padding: 20px 5px 5px 5px">

        <hr style="border-color:darkgray; " class="auto-style1"/>
        <div>
            <br />
            <table style="width: 100%;">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="padding-right:5px">&nbsp;</td>
                                <td style="padding-right:5px">
                                        <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" 
                                            ClientInstanceName="btnExcel" Font-Names="Segoe UI" Font-Size="9pt" 
                                            Height="25px" Text="Excel Template" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                            Width="90px" TabIndex="10">
                                            <Paddings Padding="2px" />
                                        </dx:ASPxButton>                            
                                </td>
                                <td style="padding-right:5px">&nbsp;</td>
                                <td style="padding-right:5px">
                                        <dx:ASPxButton ID="btnClear" runat="server" AutoPostBack="False" 
                                            ClientInstanceName="btnClear" Font-Names="Segoe UI" Font-Size="9pt" 
                                            Height="25px" Text="Clear" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                            Width="90px" TabIndex="10">
                                            <ClientSideEvents Click="ClickClear"/>
                                            <Paddings Padding="2px" />
                                        </dx:ASPxButton>                            
                                </td>
                                <td style="padding-right:5px">&nbsp;</td>
                                <td style="padding-right:5px">
                                        <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" 
                                            ClientInstanceName="btnUpload" Font-Names="Segoe UI" Font-Size="9pt" 
                                            Height="25px" Text="Upload" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                            Width="90px" TabIndex="10">
                                            <ClientSideEvents Click="ClickUpload" />
                                            <Paddings Padding="2px" />
                                        </dx:ASPxButton>                            
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
    
        </div>

        <div style="padding: 0px 5px 5px 5px">
            <table style="width: 100%;">
                <tr>
                    <td style="padding: 5px 0px 5px 0px">
                        <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="250px" Width="100%" ClientInstanceName="ASPxMemo1" ReadOnly="true">
                            <Border BorderStyle="Solid" BorderWidth="2px" BorderColor="Gray" />
                        </dx:ASPxMemo>
                    </td>
                </tr>
            </table>
        </div>
        
        <dx:ASPxLoadingPanel ID="lpProgress" runat="server" ClientInstanceName="lpProgress" Modal="false">
        </dx:ASPxLoadingPanel>
        
        <dx:ASPxCallback ID="cbTypeValue" runat="server" ClientInstanceName="cbTypeValue">
            <ClientSideEvents EndCallback="OnEndCallback"/>
        </dx:ASPxCallback>

        <input type="hidden" runat="server" id="hdUserLogin" value="<%=hdUserLogin %>" />  
        <input type="hidden" runat="server" id="hdFactoryCode" value="<%=hdFactoryCode %>" />  
        <input type="hidden" runat="server" id="hdItemTypeCode" value="<%=hdItemTypeCode %>" />  
        <dx:ASPxHiddenField ID="HF" runat="server" ClientInstanceName="HF"></dx:ASPxHiddenField>
    </div>
</asp:Content>
