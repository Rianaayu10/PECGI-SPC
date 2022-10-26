<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProdSampleQCSummary.aspx.vb" Inherits="PECGI_SPC.ProdSampleQCSummary" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .align {
            text-align: left;
        }
        .text {
            font-weight: bold;
            font-size: 9pt;
            font-family: Segoe UI;
        }
    </style>

    <script type="text/javascript">
        function OnEndCallback(s, e) {
            debugger
            if (s.cp_header == "Yes") {
                lblSampleTime.SetText(s.cp_sampletime);
                HF.Set('sampletime', s.cp_sampletime);
                lblOK.SetText(s.cp_ok);
                HF.Set('ok', s.cp_ok);
                lblNG.SetText(s.cp_ng);
                HF.Set('ng', s.cp_ng);
                lblInComplete.SetText(s.cp_no);
                HF.Set('NOK', s.cp_no);
                lblTotal.SetText(s.cp_total);
                HF.Set('total', s.cp_total);
            }
            else if (s.cp_header == "No") {
                lblSampleTime.SetText('-');
                HF.Set('sampletime', '-');
                lblOK.SetText(0);
                HF.Set('ok', 0);
                lblNG.SetText(0);
                HF.Set('ng', 0);
                lblInComplete.SetText(0);
                HF.Set('NOK', 0);
                lblTotal.SetText(0);
                HF.Set('total', 0);
            }

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
            debugger
            HF.Set('Excel', '2');
            var FactoryCode = cboFactory.GetValue();
            var ItemType_Code = cboType.GetValue();
            var LineCode = cboMachine.GetValue();
            var Frequency = cboFrequency.GetValue();
            var Period = dtPeriod.GetText();
            var Sequence = cboSequence.GetValue();

            if (FactoryCode == null || ItemType_Code == null || LineCode == null || Frequency == null || Period == "" || Sequence == null) {
                toastr.warning('Please Fill All Combo Box Filter!', 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
            }
            else {
                Gridx.PerformCallback('Load');
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            gridHeight(220);
            
            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(120);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(330);
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
            Gridx.SetHeight(height - hAll);
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="divhead" style="padding: 5px 5px 5px 5px; padding-bottom: 20px; border-bottom: groove">
        <dx:ASPxHiddenField ID="HF" runat="server" ClientInstanceName="HF"></dx:ASPxHiddenField>
        <table style="width:100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="padding-right: 1em">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Factory" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>

                            <td style="padding-right: 1em">
                                <dx:ASPxComboBox ID="cboFactory" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboFactory" TextField="Description" ValueField="Code">
                                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                                    <ButtonStyle Width="5px" Paddings-Padding="4px" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                        debugger
                                        HF.Set('Excel', '0');
                                        var FactoryCode = cboFactory.GetValue();
                                        HF.Set('FactoryCode', FactoryCode);
                                        var TypeCode = cboType.GetValue();
                                        HF.Set('TypeCode', TypeCode);

                                        btnBrowse.SetEnabled(false);
                                        btnClear.SetEnabled(false);
                                        btnExcel.SetEnabled(false);

                                        cboMachine.SetEnabled(false);
                                        cboMachine.PerformCallback();
                                
                                        Gridx.PerformCallback('Kosong');
                                    }" />
                                </dx:ASPxComboBox>
                            </td>

                            <td style="padding-right: 1em">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Machine Process" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>

                            <td style="padding-right: 1em">
                                <dx:ASPxComboBox ID="cboMachine" runat="server" Theme="Office2010Black" Width="200px" Height="25px" ClientInstanceName="cboMachine" TextField="Description" ValueField="Code">
                                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                                    <ButtonStyle Width="5px" Paddings-Padding="4px" />
                                    <ClientSideEvents EndCallback="function(s, e) {cboMachine.SetEnabled(true);}" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                        debugger
                                        HF.Set('Excel', '0');
                                        var MachineCode = cboMachine.GetValue();
                                        HF.Set('MachineCode', MachineCode);
                                
                                        btnBrowse.SetEnabled(false);
                                        btnClear.SetEnabled(false);
                                        btnExcel.SetEnabled(false);

                                        cboFrequency.SetEnabled(false);
                                        cboFrequency.PerformCallback();

                                        Gridx.PerformCallback('Kosong');
                                    }" />
                                </dx:ASPxComboBox>
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Frequency" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxComboBox ID="cboFrequency" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboFrequency" TextField="Description" ValueField="Code">
                                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                                    <ButtonStyle Width="5px" Paddings-Padding="4px" />
                                    <ClientSideEvents EndCallback="function(s, e) {cboFrequency.SetEnabled(true);}" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                        HF.Set('Excel', '0');
                                        var Frequency = cboFrequency.GetValue();
                                        HF.Set('FrequencyCode', Frequency);
                                
                                        btnBrowse.SetEnabled(false);
                                        btnClear.SetEnabled(false);
                                        btnExcel.SetEnabled(false);

                                        cboSequence.SetEnabled(false);
                                        cboSequence.PerformCallback();

                                        Gridx.PerformCallback('Kosong');
                                    }" />
                                </dx:ASPxComboBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Type" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxComboBox ID="cboType" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboType" TextField="Description" ValueField="Code">
                                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                                    <ButtonStyle Width="5px" Paddings-Padding="4px" />
                                    <ClientSideEvents EndCallback="function(s, e) {cboType.SetEnabled(true);}" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                        HF.Set('Excel', '0');
                                        var TypeCode = cboType.GetValue();
                                        HF.Set('TypeCode', TypeCode);

                                        cboMachine.SetEnabled(false);
                                        cboMachine.PerformCallback();

                                        Gridx.PerformCallback('Kosong');
                                    }" />
                                </dx:ASPxComboBox>
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Prod. Date" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxDateEdit ID="dtPeriod" runat="server" Theme="Office2010Black" EditFormat="Date" ClientInstanceName="dtPeriod" Height="25px" Width="200px"
                                    DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" AutoPostBack="false">
                                    <ClientSideEvents Init="function(s, e){var today = new Date(); dtPeriod.SetDate(today);}" />
                                    <ClientSideEvents ValueChanged="function(s, e){
                                        HF.Set('Excel', '0');
                                        Gridx.PerformCallback('Kosong');
                                    }" />
                                    <CalendarProperties>
                                        <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                                        <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                                        <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                                        <FooterStyle Font-Size="9pt" Paddings-Padding="5px" />
                                        <ButtonStyle Font-Size="9pt" Paddings-Padding="5px"></ButtonStyle>
                                    </CalendarProperties>
                                    <ButtonStyle Width="5px" Paddings-Padding="4px"></ButtonStyle>
                                </dx:ASPxDateEdit>

                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Sequence" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxComboBox ID="cboSequence" runat="server" Theme="Office2010Black" Width="100px" Height="15px" ClientInstanceName="cboSequence" TextField="Description" ValueField="Code">
                                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                                    <ButtonStyle Width="5px" Paddings-Padding="4px" />
                                    <ClientSideEvents EndCallback="function(s, e) {
                                        HF.Set('Excel', '0');
                                        cboSequence.SetEnabled(true);
                                        btnBrowse.SetEnabled(true);
                                        btnClear.SetEnabled(true);
                                        btnExcel.SetEnabled(true); 
                                    }" />
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                        Gridx.PerformCallback('Kosong');
                                    }" />
                                </dx:ASPxComboBox>
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Theme="Office2010Silver" Height="28px"
                                    Text="Browse" Font-Names="Segoe UI" Font-Size="10pt">
                                    <ClientSideEvents Click="up_Browse" />
                                </dx:ASPxButton>
                            </td>

                            <td style="padding-right: 1em; padding-top: 0.5em">
                                <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" ClientInstanceName="btnClear" Theme="Office2010Silver" Height="28px"
                                    Text="Clear" Font-Names="Segoe UI" Font-Size="10pt">
                                    <ClientSideEvents Click="function(s, e) {
                                        HF.Set('Excel', '0');
                                        cboFactory.SetSelectedIndex(-1);
                                        cboMachine.SetSelectedIndex(-1);
                                        cboFrequency.SetSelectedIndex(-1);
                                        cboType.SetSelectedIndex(-1);
                                        cboSequence.SetSelectedIndex(-1);
                                        var today = new Date(); dtPeriod.SetDate(today);
                                        Gridx.PerformCallback('Kosong');
                                    }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="width: 100%; height: 50px">
                        <tr>
                            <td style="width: 80px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Sample Time" CssClass="text" />
                            </td>
                            <td style="width: 100px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text=":" CssClass="text" />
                                &nbsp
                                <dx:ASPxLabel ID="lblSampleTime" ClientInstanceName="lblSampleTime" runat="server" Text="-" CssClass="text" />
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 80px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Total Sample" CssClass="text" />
                            </td>
                            <td style="width: 100px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text=":" CssClass="text" />
                                &nbsp
                                <dx:ASPxLabel ID="lblTotal" ClientInstanceName="lblTotal" runat="server" Text="0" CssClass="text" />
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 80px;" align="center" class="align">
                            </td>
                            <td style="width: 100px;" align="center" class="align">
                                &nbsp
                            </td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="width: 100%; height: 50px">
                        <tr>
                            <td style="width: 80px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="OK Result" CssClass="text" />
                            </td>
                            <td style="width: 33px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text=":" CssClass="text" />
                                &nbsp
                                <dx:ASPxLabel ID="lblOK" ClientInstanceName="lblOK" runat="server" Text="0" CssClass="text" />
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 80px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="NG Result" CssClass="text" />
                            </td>
                            <td style="width: 33px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text=":" CssClass="text" />
                                &nbsp
                                <dx:ASPxLabel ID="lblNG" ClientInstanceName="lblNG" runat="server" Text="0" CssClass="text" />
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 80px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Delay" CssClass="text" />
                            </td>
                            <td style="width: 33px;" align="center" class="align">
                                <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text=":" CssClass="text" />
                                &nbsp
                                <dx:ASPxLabel ID="lblInComplete" ClientInstanceName="lblInComplete" runat="server" Text="0" CssClass="text" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <div style="padding: 20px 5px 5px 5px;">
        <table style="width:100%">
            <tr>
                <td style="vertical-align:top">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" ClientInstanceName="btnExcel"
                                    Font-Names="Segoe UI" Font-Size="10pt" Text="Excel" Theme="Office2010Silver" Width="100px">
                                    <ClientSideEvents Click="function(s, e){
                                        debugger
                                        HF.Set('Excel', '1');
                                            if (Gridx.GetVisibleRowsOnPage() == 0){
                                                toastr.warning('Please Click Browse First!', 'Warning');
                                                toastr.options.closeButton = false;
                                                toastr.options.debug = false;
                                                toastr.options.newestOnTop = false;
                                                toastr.options.progressBar = false;
                                                toastr.options.preventDuplicates = true;
                                                toastr.options.onclick = null;
        		                                e.processOnServer = false;
        		                                return;
                                            }
                                        }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <div style="padding: 20px 0px 0px 0px">
        <dx:ASPxGridView ID="Gridx" runat="server" AutoGenerateColumns="False" ClientInstanceName="Gridx"
            EnableTheming="True" KeyFieldName="Type" Theme="Office2010Black" Width="100%"
            Font-Names="Segoe UI" Font-Size="9pt">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <Columns>
                <dx:GridViewDataTextColumn Caption="Type" VisibleIndex="0" Width="75px" Settings-AutoFilterCondition="Contains">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>
            </Columns>

            <SettingsBehavior ColumnResizeMode="Control" />
            <SettingsPager Mode="ShowPager" PageSize="20" AlwaysShowPager="true">
                <PageSizeItemSettings Visible="True" />
            </SettingsPager>
            <Settings HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="225" />
        </dx:ASPxGridView>
    </div>

    <div style="padding: 5px 5px 5px 5px; padding-top: 20px;">
        <table style="width: 100%; height: 15px">
            <tr>
                <td style="background-color:yellow; width:0.2%; text-align:center">
                    <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text=" "/>
                </td>
                <td align="center" class="align" style="width:0.5%;padding-left:0.5%">
                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Delay" CssClass="text" />
                </td>

                <td style="background-color:red; width:0.2%; text-align:center; color:#fff">
                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="NG" />
                </td>
                <td align="center" class="align" style="width:0.5%;padding-left:0.5%">
                    <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Result NG" CssClass="text" />
                </td>

                <td style="background-color:gray; width:0.2%; text-align:center">
                    <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text=" " />
                </td>
                <td align="center" class="align" style="width:5%;padding-left:0.5%">
                    <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="No Production Plan" CssClass="text" />
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
