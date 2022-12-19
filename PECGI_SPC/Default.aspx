<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="PECGI_SPC._Default" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Statistic Process Control System</title>
    <link href="~/Styles/SiteLogin.css" type="text/css" rel="Stylesheet" runat="server" />
    <link href="~/Styles/images/favicon_pecgi.ico" rel="SHORTCUT ICON" type="image/icon" />
    <style type="text/css">
        .style1 {
            width: 4px;
        }

        .auto-style1 {
            width: 324px;
            height: 80px;
        }

        .auto-style2 {
            width: 349px;
        }

        .auto-style3 {
            width: 188px;
        }

        .auto-style4 {
            height: 35px;
        }
    </style>
</head>

<body onload="document.getElementById('txtusername').focus()">

    <div class="form-area" style="background-color: white; width: 420px; height: 467px; vertical-align: central; margin-top: 200px">
        <form id="form1" runat="server">
            <table align="center" width="100%">
                <tr>
                    <td>
                        <img class="auto-style1" src="img/panasonic-login.png" />
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 900px">
                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="SPC SYSTEM-LOGIN"
                            ForeColor="Black" Font-Bold="True"
                            Font-Size="25px">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" bgcolor="White" class="auto-style2" valign="bottom">
                        <table align="center" id="tblLogin" border="0">
                            <tr>
                                <td align="left" class="auto-style3">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="USER ID"
                                        Font-Names="Segoe UI" Font-Bold="true"
                                        ForeColor="Black" Font-Size="12px">
                                    </dx:ASPxLabel>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3" class="auto-style4">
                                    <dx:ASPxTextBox ID="txtusername" runat="server" Border-BorderStyle="None"
                                        Width="300px" Height="30px" Font-Names="Segoe UI" Font-Size="14px"
                                        ClientInstanceName="txtusername" HorizontalAlign="Left">
                                        <Border BorderStyle="Solid" BorderColor="Silver"></Border>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="auto-style3">
                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="PASSWORD"
                                        Font-Names="Segoe UI" Font-Bold="true"
                                        ForeColor="Black" Font-Size="12px">
                                    </dx:ASPxLabel>
                                </td>
                                <td align="center">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr style="height: 35px">
                                <td align="left" colspan="3">
                                    <dx:ASPxTextBox ID="txtpassword" runat="server" Password="True" Border-BorderStyle="None"
                                        Width="300px" Height="30px" Font-Names="Segoe UI" Font-Size="14px">
                                        <Border BorderStyle="Solid" BorderColor="#CCCCCC"></Border>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="errMsg" colspan="3">&nbsp;                                                                                                                              
                                                    <dx:ASPxLabel ID="lblInfo" runat="server" Text="" Font-Names="Segoe UI"
                                                        ForeColor="Red" CssClass="errMsg" Font-Bold="True">
                                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="errMsg" colspan="3">
                                    <dx:ASPxHyperLink ID="linkForgot" runat="server"
                                        ClientInstanceName="linkForgot" ClientVisible="False"
                                        Text="Forgot password?">
                                        <ClientSideEvents Click="function (s,e) {{location.href = 'ChangePassword.aspx';}}" />
                                    </dx:ASPxHyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <dx:ASPxButton ID="btnLogin" runat="server" Text="LOGIN"
                                        Width="300px" Height="30px" Font-Names="Segoe UI" Font-Size="10pt"
                                        CssFilePath="~/css/bootstrap-theme.css"
                                        CssClass="btn-primary" Font-Bold="True" ForeColor="White">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <dx:ASPxButton ID="btnVersion" runat="server" Text="Version"
                                        ClientInstanceName="btnVersion" RenderMode="Link" ForeColor="Black"
                                        UseSubmitBehavior="False" AutoPostBack="False">
                                        <ClientSideEvents Click="function(s, e) {           			                  	
			                                                var w = 500;
                                                            var h = 500;
                                                            var left = (screen.width / 2) - (w / 2);
                                                            var top = (screen.height / 2) - (h / 2);
                                                            var winseting = 'height=' + h + ',width=' + w + ',left=' + left + ',top=' + top;
                                                            var arg = window.open('PopUpVersion.aspx', 'ModalPopUp', winseting,scrollbars=true);
		    	                                            }" />
                                    </dx:ASPxButton>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" class="version" CssClass="version"
                            ForeColor="Black" Font-Size="9pt" Font-Bold="true"
                            Text="© 2022 - PT. PANASONIC GOBEL ENERGY INDONESIA. ALL RIGHTS RESERVED.">
                        </dx:ASPxLabel>
                    </td>
                </tr>
            </table>
        </form>
    </div>
    <%-- <div class="footer-area">
            <table style="width:100%">
                <tr>
                    <td class="style1">
                        &nbsp;</td>
                    <td align="center" style="height: 40px">
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </div>--%>
</body>
</html>
