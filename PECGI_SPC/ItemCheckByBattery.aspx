<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ItemCheckByBattery.aspx.vb" Inherits="PECGI_SPC.ItemCheckByBattery" MasterPageFile="~/Site.Master" %>

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
        //function cboLineChanged(s, e) {
        //    cboType.SetEnabled(false);
        //    cboType.PerformCallback(cboFactory.GetValue());
        //}
        //function cboTypeChanged(s, e) {

        //}
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
                <td style="width: 50px"></td>
                <td style="width:130px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Machine">
                    </dx:ASPxLabel>
                </td>
                <td style="width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxComboBox ID="cboMachine" runat="server" Theme="Office2010Black" TextField="ProcessName" ClientInstanceName="cboMachine" ValueField="ProcessCode" Font-Names="Segoe UI" 
                        Font-Size="9pt" Height="25px" Width="190px" TabIndex="4" NullValueItemDisplayText="{1}">
                        <ClientSideEvents SelectedIndexChanged="cboMachineChanged" EndCallback="function(s, e) {cboMachine.SetEnabled(true);}"/>
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td style="width:20px; padding:5px 0px 0px 0px"></td>
                <td style="padding:5px 0px 0px 0px">
                    <dx:ASPxButton ID="btnSearch" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Theme="Office2010Silver" Height="28px"
                        Text="Browse" >
                        <ClientSideEvents Click="up_Browse" />
                    </dx:ASPxButton>
                    &nbsp;&nbsp;
                    <dx:ASPxButton ID="btnReset" runat="server" AutoPostBack="False" ClientInstanceName="btnReset" Theme="Office2010Silver" Height="28px"
                        Text="Clear">
                        <ClientSideEvents Click="Clear" />
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr style="height: 30px">
                <td style="width:130px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Process Group">
                    </dx:ASPxLabel>
                </td>
                <td style=" width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxComboBox ID="cboProcessGroup" runat="server" Theme="Office2010Black" TextField="ProcessGroupName" ClientInstanceName="cboProcessGroup" ValueField="ProcessGroup" Font-Names="Segoe UI" 
                        Font-Size="9pt" Height="25px" IncrementalFilteringMode="Contains" Width="190px" TabIndex="6">
                        <ClientSideEvents SelectedIndexChanged="cboProcessGroupChanged" EndCallback="function(s, e) {cboProcessGroup.SetEnabled(true);}"/>
                        <ItemStyle Height="10px" Paddings-Padding="4px" >
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td style="width: 50px"></td>
                <td style="width:130px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Machine Proccess">
                    </dx:ASPxLabel>
                </td>
                <td style="width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxComboBox ID="cboLine" runat="server" Theme="Office2010Black" TextField="LineName" ClientInstanceName="cboLine" ValueField="LineCode" Font-Names="Segoe UI" 
                        Font-Size="9pt" Height="25px" Width="190px" TabIndex="4" NullValueItemDisplayText="{1}">
                        <ClientSideEvents EndCallback="function(s, e) {cboLine.SetEnabled(true);}"/>
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr style="height: 30px">
                <td style="width:130px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Line Group">
                    </dx:ASPxLabel>
                </td>
                <td style=" width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxComboBox ID="cboLineGroup" runat="server" Theme="Office2010Black" TextField="LineGroupName" ClientInstanceName="cboLineGroup" ValueField="LineGroup" Font-Names="Segoe UI" 
                        Font-Size="9pt" Height="25px" IncrementalFilteringMode="Contains" Width="190px" TabIndex="6">
                        <ClientSideEvents SelectedIndexChanged="cboLineGroupChanged" EndCallback="function(s, e) {cboLineGroup.SetEnabled(true);}"/>
                        <ItemStyle Height="10px" Paddings-Padding="4px" >
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td style="width: 50px"></td>
                <td style="width:130px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Segoe UI" 
                        Font-Size="9pt" Text="Type">
                    </dx:ASPxLabel>
                </td>
                <td style=" width:130px; padding:5px 0px 0px 0px">
                    <%--<dx:ASPxComboBox ID="cboTypeCode" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboTypeCode">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                    </dx:ASPxComboBox>--%>
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
                <td></td>
                <td></td>
            </tr>
        </table>
    </div>
    
        <asp:SqlDataSource ID="dsFactory" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '1' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsType" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '2' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsMachine" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '3' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsFrequency" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '4' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsItemCheck" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '5' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsRegNo" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '6' ">
            <%--<SelectParameters>
                <asp:Parameter Name="Type"  />
                <asp:Parameter Name="FactoryCode1"  />
            </SelectParameters>--%>
           <%-- <SelectParameters>
                <asp:ControlParameter ControlID="cboFactory"
                    PropertyName="Value"
                    Name="FactoryCode1"/>
            </SelectParameters>--%>
        </asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsCharStatus" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '7' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsEvaluation" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '8' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsPrevValue" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '9' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsPrevItemCheck" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '10' "></asp:SqlDataSource>


    <div style="padding: 20px 5px 5px 5px">

        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            EnableTheming="True" KeyFieldName="ItemTypeName;LineCode;ItemCheck" Theme="Office2010Black" Width="100%"
            Font-Names="Segoe UI" Font-Size="9pt"
            OnRowValidating="Grid_RowValidating" OnStartRowEditing="Grid_StartRowEditing"
            OnRowInserting="Grid_RowInserting" OnRowDeleting="Grid_RowDeleting"
            OnAfterPerformCallback="Grid_AfterPerformCallback">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <Columns>
                <dx:GridViewCommandColumn FixedStyle="Left"
                    VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true" 
                    ShowNewButtonInHeader="true" ShowClearFilterButton="true" Width="80px">
                    <HeaderStyle Paddings-PaddingLeft="3px" HorizontalAlign="Center" 
                        VerticalAlign="Middle" >
                        <Paddings PaddingLeft="3px"></Paddings>
                    </HeaderStyle>
                </dx:GridViewCommandColumn>

                <dx:GridViewDataComboBoxColumn Caption="Factory" FieldName="FactoryCode" VisibleIndex="0"
                    Width="70px" Settings-AutoFilterCondition="Contains" Visible="false">
                    <PropertiesComboBox DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="70px"
                        TextField="FactoryName" ValueField="FactoryCode" ClientInstanceName="FactoryCode">
                        <%--<ClientSideEvents ValueChanged = "gridFactorySelected" />--%>
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

                <dx:GridViewDataComboBoxColumn Caption="Type" FieldName="ItemTypeCode" VisibleIndex="0"
                    Width="80px" Settings-AutoFilterCondition="Contains" Visible="false" >
                    <PropertiesComboBox DataSourceID="dsType" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="70px"
                        TextField="ItemTypeName" ValueField="ItemTypeCode" ClientInstanceName="ItemTypeCode">
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

                <dx:GridViewDataComboBoxColumn FixedStyle="Left" Caption="Type" FieldName="ItemTypeName" VisibleIndex="1"
                    Width="80px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsType" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="70px"
                        TextField="ItemTypeName" ValueField="ItemTypeName" ClientInstanceName="ItemTypeName">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AllowAutoFilter="False" AllowFilterBySearchPanel="False" ShowInFilterControl="False" AllowHeaderFilter="False" />
                    <%--<Settings AutoFilterCondition="Contains"></Settings>--%>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn FixedStyle="Left" Caption="Machine Process" FieldName="LineName" VisibleIndex="2"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsMachine" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="LineName" ValueField="LineName" ClientInstanceName="LineName">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AllowAutoFilter="False" AllowFilterBySearchPanel="False" ShowInFilterControl="False" AllowHeaderFilter="False" />
                    <%--<Settings AutoFilterCondition="Contains"></Settings>--%>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>
                
                <dx:GridViewDataComboBoxColumn FixedStyle="Left" Caption="Item Check" FieldName="ItemCheck" VisibleIndex="3"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsItemCheck" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="ItemCheck" ValueField="ItemCheck" ClientInstanceName="ItemCheck">
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

                <dx:GridViewDataComboBoxColumn Caption="Frequency" FieldName="FrequencyCode" VisibleIndex="4"
                    Width="70px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsFrequency" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="70px"
                        TextField="FrequencyName" ValueField="FrequencyCode" ClientInstanceName="FrequencyCode">
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

                <dx:GridViewDataComboBoxColumn Caption="Registration Alat Ukur" FieldName="RegistrationNo" VisibleIndex="5"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsRegNo" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="RegistrationName" ValueField="RegistrationNo" ClientInstanceName="RegistrationNo">
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

                <dx:GridViewDataSpinEditColumn Caption="Sample Size" FieldName="SampleSize" VisibleIndex="6" Width="80px" Settings-AutoFilterCondition="Contains">
                    <PropertiesSpinEdit MaxValue="999" MinValue="0" Width="70px" Style-HorizontalAlign="Right">
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px" />
                        </ButtonStyle>
                    </PropertiesSpinEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                    <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </dx:GridViewDataSpinEditColumn>

                <dx:GridViewDataComboBoxColumn Caption="Evaluation" FieldName="Evaluation" VisibleIndex="7"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsEvaluation" DropDownStyle="DropDown" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Evaluation" ValueField="Evaluation" ClientInstanceName="Evaluation">
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

                <dx:GridViewDataComboBoxColumn Caption="Special Characteristics" FieldName="CharacteristicStatus" VisibleIndex="8"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsCharStatus" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="CharacteristicStatus" ValueField="CharacteristicStatus" ClientInstanceName="CharacteristicStatus">
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
                                
                <dx:GridViewDataComboBoxColumn Caption="Process Table Line Code" FieldName="ProcessTableLineCode" VisibleIndex="9"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsMachine" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="LineName" ValueField="LineName" ClientInstanceName="LineName">
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

                <dx:GridViewDataSpinEditColumn Caption="FTA Ratio %" FieldName="FTARatio"
                    width="70px" Settings-AutoFilterCondition="Contains" VisibleIndex="10">
                    <PropertiesSpinEdit MaxValue="10000" MinValue="0" DecimalPlaces="2" Increment="1" Style-VerticalAlign="Middle" Style-HorizontalAlign="Right" Width="195px">
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px" />
                        </ButtonStyle>
                    </PropertiesSpinEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                    <CellStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </dx:GridViewDataSpinEditColumn>

                <dx:GridViewDataTextColumn Caption="Station ID" FieldName="StationID"
                    VisibleIndex="11" Width="70px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="50" Width="70px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataTextColumn>
                
                <dx:GridViewDataComboBoxColumn Caption="Prev Item Check" FieldName="PrevItemCheck" VisibleIndex="12"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsPrevItemCheck" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="ItemCheck" ValueField="ItemCheck" ClientInstanceName="PrevItemCheck">
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

               <dx:GridViewDataComboBoxColumn Caption="Prev Value" FieldName="PrevValue" VisibleIndex="13"
                    Width="50px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsPrevValue" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="45px"
                        TextField="PrevValue" ValueField="PrevValue" ClientInstanceName="PrevValue">
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

                <dx:GridViewDataCheckColumn Caption="Active Status" FieldName="ActiveStatus" 
                    VisibleIndex="14" Width="60px">
                    <PropertiesCheckEdit ValueChecked="1" ValueType="System.Char" 
                        ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <Settings AllowSort="False" />
                </dx:GridViewDataCheckColumn>

                <dx:GridViewDataTextColumn Caption="Last User" FieldName="UpdateUser"
                    VisibleIndex="15" Width="70px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="50" Width="70px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Last Update" FieldName="UpdateDate"
                    VisibleIndex="16" Width="150px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="50" Width="200px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Remark" FieldName="Remark"
                    VisibleIndex="17" Width="150px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="35" Width="195px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataTextColumn>
                                
            </Columns>

            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
            <SettingsPager Mode="ShowPager" PageSize="50" AlwaysShowPager="true">
                <PageSizeItemSettings Visible="True" />
            </SettingsPager>
            <Settings ShowFilterRow="True" VerticalScrollBarMode="Auto"
                VerticalScrollableHeight="300" HorizontalScrollBarMode="Auto" />
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
                    <div style="padding: 15px 15px 15px 15px; width: 300px">
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <table align="center">
                                <tr style="height:30px">
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Factory Name" Width="90px"></dx:ASPxLabel>
                                    </td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFactoryCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="FactoryCode" >
                                        </dx:ASPxGridViewTemplateReplacement> 
                                    </td>
                                </tr>
                                <%--<tr style="height:30px">
                                    <td>
                                    </td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editItemTypeCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemTypeCode" >
                                        </dx:ASPxGridViewTemplateReplacement> 
                                    </td>
                                </tr>--%>
                                <tr style="height:30px">
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Type Name" Width="90px"></dx:ASPxLabel>
                                    </td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editItemTypeName" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemTypeCode">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Machine Process</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editLineCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="LineName">
                                        </dx:ASPxGridViewTemplateReplacement>   
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Item Check</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editItemCheck" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemCheck">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Frequency</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFrequencyCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="FrequencyCode">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Registration Alat Ukur</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editRegistrationNo" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="RegistrationNo">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Sample Size</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editSampleSize" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="SampleSize">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Remark</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editRemark" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Remark">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Evaluation</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editEvaluation" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Evaluation">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Special Characteristic</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editCharacteristicStatus" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="CharacteristicStatus">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Process Table Line Code</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editProcessTableLineCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ProcessTableLineCode">
                                        </dx:ASPxGridViewTemplateReplacement>   
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>FTA Ratio</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFTARatio" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="FTARatio">
                                        </dx:ASPxGridViewTemplateReplacement>   
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Station ID</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editStationID" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="StationID">
                                        </dx:ASPxGridViewTemplateReplacement>   
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Prev Item Check</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editPrevItemCheck" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="PrevItemCheck">
                                        </dx:ASPxGridViewTemplateReplacement>   
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Prev Value</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editPrevValue" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="PrevValue">
                                        </dx:ASPxGridViewTemplateReplacement>   
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Active Status
                                    </td>
                                    <td>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxGridViewTemplateReplacement ID="editActiveStatus" ReplacementType="EditFormCellEditor"
                                                runat="server" ColumnID="ActiveStatus">
                                            </dx:ASPxGridViewTemplateReplacement>       
                                        </dx:LayoutItemNestedControlContainer>                                    
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </div>
                    <div style="text-align: left; padding: 5px 5px 5px 15px">
                        <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                            runat="server">
                        </dx:ASPxGridViewTemplateReplacement>
                        <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </dx:ASPxGridViewTemplateReplacement>
                    </div>
                </EditForm>
            </Templates>

        </dx:ASPxGridView>
        <input type="hidden" runat="server" id="hdUserLogin" value="<%=hdUserLogin %>" />  
        <input type="hidden" runat="server" id="hdFactoryCode" value="<%=hdFactoryCode %>" />  
        <input type="hidden" runat="server" id="hdItemTypeCode" value="<%=hdItemTypeCode %>" />  
        <dx:ASPxHiddenField ID="HF" runat="server" ClientInstanceName="HF"></dx:ASPxHiddenField>
    </div>
</asp:Content>
