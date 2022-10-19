<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ControlChartSetup.aspx.vb" Inherits="PECGI_SPC.ControlChartSetup" MasterPageFile="~/Site.Master" %>

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

        function up_Browse() {
            var FactoryCode = cboFactory.GetValue();
            var ItemType_Code = cboType.GetValue();
            var LineCode = cboMachine.GetValue();
            var Period = dtPeriod.GetText();

            if (FactoryCode == null || ItemType_Code == null || LineCode == null ||Period == "") {
                toastr.warning('Please Fill All Combo Box Filter!', 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
            }
            else {
                Grid.PerformCallback('Load');
            }
        }

        function gridFactorySelected() {
            var a = Grid.GetEditor("Factory").GetValue().toString();
            Grid.GetEditor("MachineEditGrid").PerformCallback(a);
        }

        function gridItemTypeSelected() {
            var a = Grid.GetEditor("Factory").GetValue().toString();
            var b = Grid.GetEditor("TypeEditGrid").GetValue().toString();
            Grid.GetEditor("MachineEditGrid").PerformCallback(a + '|' + b);
        }

        function gridMachineSelected() {
            var a = Grid.GetEditor("Factory").GetValue().toString();
            var b = Grid.GetEditor("TypeEditGrid").GetValue().toString();
            var c = Grid.GetEditor("MachineEditGrid").GetValue().toString();
            Grid.GetEditor("ItemCheckEditGrid").PerformCallback(a + '|' + b + '|' + c);
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            gridHeight(110);

            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(10);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(210);
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
        <%--<div style="padding: 5px 5px 5px 5px;">
            <div class="row" style="margin-bottom:0.5%">
            <div class="col col-1" style="width: 5%">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Factory" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt"/>
            </div>
            <div class="col col-1">
                <dx:ASPxComboBox ID="cboFactory" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboFactory">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                var FactoryCode = cboFactory.GetValue();
                                cboMachine.PerformCallback(FactoryCode);
                                HF.Set('FactoryCode', FactoryCode);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxComboBox>
            </div>
            <div class="col col-1">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Machine Process" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt"/>
            </div>
            <div class="col col-1">
                <dx:ASPxComboBox ID="cboMachine" runat="server" Theme="Office2010Black" Width="200px" Height="25px" ClientInstanceName="cboMachine">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                        <ClientSideEvents EndCallback="OnEndCallback" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                var MachineCode = cboMachine.GetValue();
                                HF.Set('MachineCode', MachineCode);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxComboBox>
            </div>
        </div>

        <div class="row">
            <div class="col col-1" style="width: 5%">
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Type" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt" />
            </div>
            <div class="col col-1">
                <dx:ASPxComboBox ID="cboType" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboType">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                var TypeCode = cboType.GetValue();
                                HF.Set('Type', TypeCode);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxComboBox>
            </div>
            <div class="col col-1">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Period" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt"/>
            </div>
            <div class="col col-1" style="margin-bottom:1%;width: 7%">
                <dx:ASPxDateEdit ID="dtPeriod" runat="server" Theme="Office2010Black" EditFormat="Date" ClientInstanceName="dtPeriod" Height="25px" Width="100px"
                        DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" AutoPostBack="false">
                        <ClientSideEvents Init="function(s, e){var today = new Date(); dtPeriod.SetDate(today);}" />
                        <ClientSideEvents ValueChanged="function(s, e){
                                Grid.PerformCallback('Kosong');
                            }" />
                        <CalendarProperties>
                            <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="5px"></ButtonStyle>
                        </CalendarProperties>
                        <ButtonStyle Width="5px" Paddings-Padding="4px" ></ButtonStyle>
                    </dx:ASPxDateEdit>
            </div>
            <div class="col col-1" style="width:5%">
                <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Theme="Office2010Silver" Height="28px"
                        Text="Browse" Font-Names="Segoe UI" Font-Size="10pt">
                        <ClientSideEvents Click="up_Browse" />
                    </dx:ASPxButton>
            </div>
            <div class="col col-1" style="width:5%">
                <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" ClientInstanceName="btnClear" Theme="Office2010Silver" Height="28px"
                        Text="Clear " Font-Names="Segoe UI" Font-Size="10pt">
                        <ClientSideEvents Click="function(s, e) {
                                cboFactory.SetSelectedIndex(0);
                                cboMachine.SetSelectedIndex(0);
                                cboType.SetSelectedIndex(0);
                                var today = new Date(); dtPeriod.SetDate(today);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxButton>
            </div>
        </div>
    </div>--%>

    <div id="divhead" style="padding: 5px 5px 5px 5px;">
        <table>
            <tr>
                <td style="padding-right: 1em">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Factory" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt">
                    </dx:ASPxLabel>
                </td>

                <td style="padding-right: 1em">
                    <dx:ASPxComboBox ID="cboFactory" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboFactory" TextField="Description" ValueField="Code">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                var FactoryCode = cboFactory.GetValue();
                                cboMachine.PerformCallback(FactoryCode);
                                HF.Set('FactoryCode', FactoryCode);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxComboBox>
                </td>

                <td style="padding-right: 1em">
                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Machine Process" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt">
                    </dx:ASPxLabel>
                </td>

                <td style="padding-right: 1em">
                    <dx:ASPxComboBox ID="cboMachine" runat="server" Theme="Office2010Black" Width="200px" Height="25px" ClientInstanceName="cboMachine" TextField="Description" ValueField="Code">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                        <ClientSideEvents EndCallback="OnEndCallback" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                var MachineCode = cboMachine.GetValue();
                                HF.Set('MachineCode', MachineCode);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxComboBox>
                </td>
            </tr>

            <tr>
                <td style="padding-right: 1em; padding-top: 0.5em">
                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Type" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt">
                    </dx:ASPxLabel>
                </td>

                <td style="padding-right: 1em; padding-top: 0.5em">
                    <dx:ASPxComboBox ID="cboType" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboType" TextField="Description" ValueField="Code">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                                var FactoryCode = cboFactory.GetValue();
                                var TypeCode = cboType.GetValue();
                                HF.Set('Type', TypeCode);
                                cboMachine.PerformCallback(FactoryCode + '|' + TypeCode);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxComboBox>
                </td>

                <td style="padding-right: 1em; padding-top: 0.5em">
                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Period" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt">
                    </dx:ASPxLabel>
                </td>

                <td style="padding-right: 1em; padding-top: 0.5em">
                    <dx:ASPxDateEdit ID="dtPeriod" runat="server" Theme="Office2010Black" EditFormat="Date" ClientInstanceName="dtPeriod" Height="25px" Width="100px"
                        DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" AutoPostBack="false">
                        <ClientSideEvents Init="function(s, e){var today = new Date(); dtPeriod.SetDate(today);}" />
                        <ClientSideEvents ValueChanged="function(s, e){
                                Grid.PerformCallback('Kosong');
                            }" />
                        <CalendarProperties>
                            <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="5px"></ButtonStyle>
                        </CalendarProperties>
                        <ButtonStyle Width="5px" Paddings-Padding="4px" ></ButtonStyle>
                    </dx:ASPxDateEdit>

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
                                cboFactory.SetSelectedIndex(0);
                                cboMachine.SetSelectedIndex(0);
                                cboType.SetSelectedIndex(0);
                                var today = new Date(); dtPeriod.SetDate(today);
                                Grid.PerformCallback('Kosong');
                            }" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
    
    <div style="padding: 20px 0px 0px 0px">
        <asp:SqlDataSource ID="dsType" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ChartSetup_FillCombo '1' "></asp:SqlDataSource>

        <%--<!<asp:SqlDataSource ID="dsMachine" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ChartSetup_FillCombo '4' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsItemCheck" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ChartSetup_FillCombo '6' ">
            <SelectParameters>
                <asp:Parameter Name="Type" DefaultValue="6" Type="String"/>
                <asp:ControlParameter ControlID="cboFactory" PropertyName="Value" Name="Param" Type="String"/>
                <asp:ControlParameter ControlID="cboMachine" PropertyName="Value" Name="Param2" Type="String"/>
                <asp:ControlParameter ControlID="cboType" PropertyName="Value" Name="Param3" Type="String"/>
            </SelectParameters>
        </asp:SqlDataSource>--%>

        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            EnableTheming="True" Theme="Office2010Black" Width="100%" KeyFieldName="Factory;Type;Machine;ItemCheck;Start;LastUpdate"
            Font-Names="Segoe UI" Font-Size="9pt"
            OnRowValidating="Grid_RowValidating" OnStartRowEditing="Grid_StartRowEditing"
            OnRowInserting="Grid_RowInserting" OnRowDeleting="Grid_RowDeleting"
            OnAfterPerformCallback="Grid_AfterPerformCallback">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true" FixedStyle="Left"
                    ShowNewButtonInHeader="true" ShowClearFilterButton="true" Width="70px">
                    <HeaderStyle Paddings-PaddingLeft="3px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="3px"></Paddings>
                    </HeaderStyle>
                </dx:GridViewCommandColumn>

                <dx:GridViewDataComboBoxColumn Caption="Type" FieldName="Type" VisibleIndex="1" FixedStyle="Left"
                    Width="85px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsType" DropDownStyle="DropDownList" TextFormatString="{0}" DisplayFormatString="{1}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="100%"
                        TextField="Description" ValueField="Description" ClientInstanceName="Type">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Machine Process" FieldName="Machine" VisibleIndex="2" FixedStyle="Left"
                    Width="175px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="100%"
                        TextField="Description" ValueField="Description" ClientInstanceName="Machine">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Item Check" FieldName="ItemCheck" VisibleIndex="3" FixedStyle="Left"
                    Width="175px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="100%"
                        TextField="Description" ValueField="Description" ClientInstanceName="ItemCheck">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewBandColumn Caption="Period" VisibleIndex="4" FixedStyle="Left">
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <dx:GridViewDataDateColumn Caption="Start" FieldName="Start"
                            Width="85px" Settings-AutoFilterCondition="Contains">
                            <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormat="Custom" EditFormatString="dd MMM yyyy" MaxDate="9999-12-31" MinDate="2000-12-01">
                                <ButtonStyle Width="5px" Paddings-Padding="2px"/>
                                <CalendarProperties>
                                    <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                                    <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                                    <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                                    <FooterStyle Font-Size="9pt" Paddings-Padding="5px" />
                                    <ButtonStyle Font-Size="9pt" Paddings-Padding="5px"></ButtonStyle>
                                </CalendarProperties>
                            </PropertiesDateEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataDateColumn Caption="End" FieldName="End"
                            Width="85px" Settings-AutoFilterCondition="Contains">
                            <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormat="Custom" EditFormatString="dd MMM yyyy" MaxDate="9999-12-31" MinDate="2000-12-01">
                                <ButtonStyle Width="5px" Paddings-Padding="2px"/>
                                <CalendarProperties>
                                    <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                                    <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                                    <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                                    <FooterStyle Font-Size="9pt" Paddings-Padding="5px" />
                                    <ButtonStyle Font-Size="9pt" Paddings-Padding="5px"></ButtonStyle>
                                </CalendarProperties>
                            </PropertiesDateEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                        </dx:GridViewDataDateColumn>
                    </Columns>
                </dx:GridViewBandColumn>

                <dx:GridViewDataTextColumn Caption="Measuring Unit" FieldName="MeasuringUnit" VisibleIndex="5"
                    Width="75px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="15" Width="55px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewBandColumn Caption="Specification" VisibleIndex="6">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                    <Columns>
                        <dx:GridViewDataSpinEditColumn Caption="USL" FieldName="SpecUSL"
                            width="50px" Settings-AutoFilterCondition="Contains" >
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>

                        <dx:GridViewDataSpinEditColumn Caption="LSL" FieldName="SpecLSL"
                            width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0.001" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>
                    </Columns>
                </dx:GridViewBandColumn>

                <dx:GridViewBandColumn Caption="X Bar Control" VisibleIndex="7">
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <dx:GridViewDataSpinEditColumn Caption="CL" FieldName="XCL"
                            width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>

                        <dx:GridViewDataSpinEditColumn Caption="UCL" FieldName="XUCL"
                            width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>

                        <dx:GridViewDataSpinEditColumn Caption="LCL" FieldName="XLCL"
                            width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>
                    </Columns>
                </dx:GridViewBandColumn>

                <dx:GridViewBandColumn Caption="R Control" VisibleIndex="8">
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <dx:GridViewDataSpinEditColumn Caption="CL" FieldName="RCL"
                            width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>

                        <dx:GridViewDataSpinEditColumn Caption="UCL" FieldName="RUCL"
                            width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>

                        <dx:GridViewDataSpinEditColumn Caption="LCL" FieldName="RLCL"
                            width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="3" Increment="0.001" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right">
                                <ButtonStyle Width="5px" Paddings-Padding="2px">
                                    <Paddings Padding="2px" />
                                </ButtonStyle>
                            </PropertiesSpinEdit>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </dx:GridViewDataSpinEditColumn>
                    </Columns>
                </dx:GridViewBandColumn>

                <dx:GridViewDataTextColumn Caption="Last User" FieldName="LastUser" VisibleIndex="9"
                    Width="75px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="15" Width="100px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Last Update" FieldName="LastUpdate" VisibleIndex="10"
                    Width="135px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="15" Width="100px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn Caption="Factory" FieldName="Factory" VisibleIndex="17"
                    Width="100px" Settings-AutoFilterCondition="Contains" Visible="false">
                    <PropertiesComboBox DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="95px"
                        TextField="Description" ValueField="Code" ClientInstanceName="Factory">
                        <ClientSideEvents ValueChanged = "gridFactorySelected" />
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Machine EDIT" FieldName="MachineEditGrid" VisibleIndex="18"
                    Width="200px" Settings-AutoFilterCondition="Contains" Visible="false">
                    <PropertiesComboBox DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Description" ValueField="Code" ClientInstanceName="MachineEditGrid">
                        <ClientSideEvents ValueChanged = "gridMachineSelected" />
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Item EDIT" FieldName="ItemCheckEditGrid" VisibleIndex="19"
                    Width="200px" Settings-AutoFilterCondition="Contains" Visible="false">
                    <PropertiesComboBox DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Description" ValueField="Code" ClientInstanceName="ItemCheckEditGrid">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Type EDIT" FieldName="TypeEditGrid" VisibleIndex="20"
                    Width="200px" Settings-AutoFilterCondition="Contains" Visible="false">
                    <PropertiesComboBox DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Description" ValueField="Code" ClientInstanceName="TypeEditGrid">
                        <ClientSideEvents ValueChanged = "gridItemTypeSelected" />
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>
            </Columns>

            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
            <SettingsPager Mode="ShowPager" PageSize="20" AlwaysShowPager="true">
                <PageSizeItemSettings Visible="True" />
            </SettingsPager>
            <Settings ShowFilterRow="True" VerticalScrollBarMode="Auto"
                VerticalScrollableHeight="275" HorizontalScrollBarMode="Auto" />
            <SettingsText ConfirmDelete="Are you sure want to delete ?"></SettingsText>
            <SettingsPopup>
                <EditForm Modal="false" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200" />
            </SettingsPopup>

            <Styles EditFormColumnCaption-Paddings-PaddingLeft="10px" EditFormColumnCaption-Paddings-PaddingRight="10px">
                <Header Wrap="True">
                    <Paddings Padding="2px"></Paddings>
                </Header>

                <EditFormColumnCaption Font-Size="9pt" Font-Names="Segoe UI">
                    <Paddings PaddingLeft="5px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                </EditFormColumnCaption>
            </Styles>

             <Templates>
                <EditForm>
                    <div style="padding: 5px 15px 5px 15px; width: 325px">
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <table>
                                <tr style="height: 30px">
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Factory" Width="120px"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFactory" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Factory"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Type</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editType" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="TypeEditGrid"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Machine Process</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editMachine" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="MachineEditGrid"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Item Check</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editItemCheck" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemCheckEditGrid"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Start</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editStart" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Start"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>End</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="End"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Spesification USL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editSpecUSL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="SpecUSL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Spesification LSL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editSpecLSL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="SpecLSL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>X Bar CL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editXCL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="XCL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>X Bar UCL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editXUCL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="XUCL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>X Bar LCL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editXLCL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="XLCL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>R CL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editRCL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="RCL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>R UCL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editRUCL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="RUCL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>R LCL</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editRLCL" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="RLCL"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </div>
                    <div style="text-align: left; padding: 5px 5px 5px 15px">
                        <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                            runat="server"></dx:ASPxGridViewTemplateReplacement>
                        <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                            runat="server"></dx:ASPxGridViewTemplateReplacement>
                    </div>
                </EditForm>
            </Templates>

        </dx:ASPxGridView>
        <dx:ASPxHiddenField ID="HF" runat="server" ClientInstanceName="HF"></dx:ASPxHiddenField>
    </div>
    
</asp:Content>
