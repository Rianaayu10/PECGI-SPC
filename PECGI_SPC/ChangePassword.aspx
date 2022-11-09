<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="PECGI_SPC.ChangePassword" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function MessageError(s, e) {
            if (s.cpMessage == "Update data successful.") {
                toastr.success(s.cpMessage, 'Success');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;

                millisecondsToWait = 2000;
                setTimeout(function () {
                    window.open('ChangePassword.aspx', '_self');
                }, millisecondsToWait);

            } else {
                toastr.warning(s.cpMessage, 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;
            }
        }

        function txtCurrentPassword_Init(s, e) {
            //debugger
            txtCurrentPassword.GetInputElement().setAttribute("type", "password");
        }

        function txtNewPassword_Init(s, e) {
            //debugger
            txtNewPassword.GetInputElement().setAttribute("type", "password");
        }

        function OnCurrentPasswordCheckedChanged(s, e) {
            //debugger
            if (s.GetValue() == "1") {
                txtCurrentPassword.GetInputElement().setAttribute("type", "password");
            } else {
                txtCurrentPassword.GetInputElement().setAttribute("type", "text");
            }
        }

        function OnNewPasswordCheckedChanged(s, e) {
            //debugger
            if (s.GetValue() == "1") {
                txtNewPassword.GetInputElement().setAttribute("type", "password");
            } else {
                txtNewPassword.GetInputElement().setAttribute("type", "text");
            }
        }


    </script>
    <style type="text/css">
        .style1 {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <table style="width: 500px; border: 1px solid silver" cellpadding="5" cellspacing="6">
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr style="height: 30px">
            <td style="width: 160px" align="right">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Names="Segoe UI" Font-Size="10pt" Text="UserID">
                </dx:ASPxLabel>
            </td>
            <td style="width: 20px">&nbsp;</td>
            <td style="width: 50px">
                <dx:ASPxTextBox ID="txtUserID" runat="server" Font-Names="Verdana" Width="200px" ClientInstanceName="txtUserID" ClientEnabled="false">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr style="height: 30px">
            <td style="width: 160px" align="right">
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Names="Segoe UI" Font-Size="10pt" Text="Full Name">
                </dx:ASPxLabel>
            </td>
            <td style="width: 20px">&nbsp;</td>
            <td style="width: 50px">
                <dx:ASPxTextBox ID="txtFullName" runat="server" Font-Names="Verdana" Width="200px" ClientInstanceName="txtFullName" ClientEnabled="false">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr style="height: 30px" align="right">
            <td style="width: 160px">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="10pt" Text="Current Password">
                </dx:ASPxLabel>
            </td>
            <td style="width: 20px">&nbsp;</td>
            <td style="width: 50px">
                <dx:ASPxTextBox ID="txtCurrentPassword" runat="server" Font-Names="Verdana" Width="200px" ClientInstanceName="txtCurrentPassword" ClientEnabled="false">
                    <ClientSideEvents
                        Init="txtCurrentPassword_Init" />
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr style="height: 30px" align="left">
            <td style="width: 160px">&nbsp;
            </td>
            <td style="width: 20px">&nbsp;</td>
            <td>
                <dx:ASPxCheckBox ID="cbCurrentPassword" runat="server" ClientInstanceName="cbCurrentPassword" ValueType="System.String"
                    ValueChecked="1" ValueUnchecked="0" ForeColor="White" ClientSideEvents-CheckedChanged="OnCurrentPasswordCheckedChanged">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr style="height: 30px" align="right">
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Segoe UI" Font-Size="10pt" Text="New Password">
                </dx:ASPxLabel>
            </td>
            <td style="width: 20px">&nbsp;</td>
            <td>
                <dx:ASPxTextBox ID="txtNewPassword" runat="server" Font-Names="Verdana" Width="200px" ClientInstanceName="txtNewPassword">
                    <ClientSideEvents
                        Init="txtNewPassword_Init" />
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxCallback ID="cbProgress" runat="server" ClientInstanceName="cbProgress">
                    <ClientSideEvents CallbackComplete="MessageError" />
                </dx:ASPxCallback>
            </td>
        </tr>
        <tr style="height: 30px" align="left">
            <td style="width: 160px">&nbsp;
            </td>
            <td style="width: 20px">&nbsp;</td>
            <td>
                <dx:ASPxCheckBox ID="cbNewPassword" runat="server" ForeColor="White" ClientSideEvents-CheckedChanged="OnNewPasswordCheckedChanged">
                </dx:ASPxCheckBox>
            </td>
        </tr>
       <%-- <tr style="height: 30px" align="right">
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Segoe UI" Font-Size="10pt" Text="Confirm New Password">
                </dx:ASPxLabel>
            </td>
            <td style="width: 20px">&nbsp;</td>
            <td>
                <dx:ASPxTextBox ID="txtConfirmPassword" runat="server" Font-Names="Verdana" Width="200px" ClientInstanceName="txtConfirmPassword" Password="True">
                </dx:ASPxTextBox>
            </td>
        </tr>--%>

        <%--        <tr style="height:25px">
            <td colspan="6"></td>
        </tr>--%>

        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="2">
                <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" AutoPostBack="False" Height="16px" Width="80px" Font-Names="Segoe UI" Font-Size="10pt" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {
	                    txtNewPassword.SetText('');
                        txtNewPassword.Focus();
                    }" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnSubmit" runat="server" Text="Submit" Height="16px" Width="80px" Font-Names="Segoe UI" Font-Size="10pt" AutoPostBack="False"
                    UseSubmitBehavior="False" ClientInstanceName="btnSubmit">
                    <ClientSideEvents Click="function(s, e) {
                    
		                if (txtCurrentPassword.GetText() == ''){
                            toastr.warning('Please Insert Current Password!', 'Warning');
                            toastr.options.closeButton = false;
                            toastr.options.debug = false;
                            toastr.options.newestOnTop = false;
                            toastr.options.progressBar = false;
                            toastr.options.preventDuplicates = true;
                            toastr.options.onclick = null;
		                    e.processOnServer = false;
                            return;
	                    }          
                        if (txtNewPassword.GetText() == ''){
                            toastr.warning('Please Insert New Password!', 'Warning');
                            toastr.options.closeButton = false;
                            toastr.options.debug = false;
                            toastr.options.newestOnTop = false;
                            toastr.options.progressBar = false;
                            toastr.options.preventDuplicates = true;
                            toastr.options.onclick = null;
		                    e.processOnServer = false;
                            return;
	                    }   
                         if (txtNewPassword.GetText() == txtCurrentPassword.GetText()){
                            toastr.warning('New Password and Current Password can not be the same!', 'Warning');
                            toastr.options.closeButton = false;
                            toastr.options.debug = false;
                            toastr.options.newestOnTop = false;
                            toastr.options.progressBar = false;
                            toastr.options.preventDuplicates = true;
                            toastr.options.onclick = null;
		                    e.processOnServer = false;
                            return;
	                    }
                         cbProgress.PerformCallback();                             
                        }" />
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
