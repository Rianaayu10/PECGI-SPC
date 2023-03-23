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
            cboProcessGroup.SetEnabled(false);
            cboProcessGroup.PerformCallback(cboFactory.GetValue());
            cboType.SetEnabled(false);
            cboType.PerformCallback(cboFactory.GetValue());
        }
        function cboProcessGroupChanged(s, e) {
            cboLineGroup.SetEnabled(false);
            cboLineGroup.PerformCallback(cboProcessGroup.GetValue());
        }
        function cboLineGroupChanged(s, e) {
            cboMachine.SetEnabled(false);
            cboMachine.PerformCallback(cboLineGroup.GetValue());
        }
        function cboMachineChanged(s, e) {
            cboLine.SetEnabled(false);
            cboLine.PerformCallback(cboFactory.GetValue());
        }
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

        function up_Browse() {
            var errmsg = '';
            if (cboFactory.GetText() == '') {
                cboFactory.Focus();
                errmsg = 'Please select Factory!';
            } else if (cboType.GetText() == '') {
                cboType.Focus();
                errmsg = 'Please select Type!';
            } else if (cboLine.GetText() == '') {
                cboLine.Focus();
                errmsg = 'Please select Machine Process!';
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
            Grid.CancelEdit();
            Grid.PerformCallback('Load');
        }
        function Clear() {
            cboFactory.SetValue('');
            cboLine.SetValue('');
            cboType.SetValue('');
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            gridHeight(150);

            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(50);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(260);
                    $("#flscr").val("0");
                }
            })
        });

        function gridHeight(pF) {
            var h1 = 49;
            var p1 = 10;
            var h2 = 34;
            var p2 = 13;
            var h3 = $("#divhead").height();

            var hAll = h1 + p1 + h2 + p2 + h3 + pF;
            /* alert(h1 + p1 + h2 + p2 + h3);*/
            var height = Math.max(0, document.documentElement.clientHeight);
            Grid.SetHeight(height - hAll);
        };
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
                        <ClientSideEvents  EndCallback="function(s, e) {cboType.SetEnabled(true);}"/>
                        <ItemStyle Height="10px" Paddings-Padding="4px" >
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
            </tr>
        </table>
    </div>


    <div style="padding: 20px 5px 5px 5px">

        <input type="hidden" runat="server" id="hdUserLogin" value="<%=hdUserLogin %>" />  
        <input type="hidden" runat="server" id="hdFactoryCode" value="<%=hdFactoryCode %>" />  
        <input type="hidden" runat="server" id="hdItemTypeCode" value="<%=hdItemTypeCode %>" />  
        <dx:ASPxHiddenField ID="HF" runat="server" ClientInstanceName="HF"></dx:ASPxHiddenField>
    </div>
</asp:Content>
