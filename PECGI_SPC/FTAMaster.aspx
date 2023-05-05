<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FTAMaster.aspx.vb" Inherits="PECGI_SPC.FTAMaster" MasterPageFile="~/Site.Master" %>

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
        function cboLineChanged(s, e) {
            cboItemCheck.SetEnabled(false);
            cboItemCheck.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue());
        }
        function cboTypeChanged(s, e) {
            cboItemCheck.SetEnabled(false);
            cboItemCheck.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue());
        }
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

        function ShowPopUpIK(s) {
            cbkPanel.PerformCallback(s);
            pcIK.Show();
        }
        function ClosePopupIK(s, e) {
            pcIK.Hide();
            e.processOnServer = false;
        }
        function ShowPopUpUploadIK(s) {
            cbkPanelUploadIK.PerformCallback(s);
            pcUploadIK.Show();
        }
        function ClosePopupUploadIK(s, e) {
            pcUploadIK.Hide();
            //e.processOnServer = false;
        }
        function ShowPopUpAction(s) {
            cbkPanelAction.PerformCallback(s);
            pcAction.Show();
        }
        function ClosePopupAction(s, e) {
            pcAction.Hide();
            e.processOnServer = false;
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
            cboProcessGroup.SetValue('');
            cboLineGroup.SetValue('');
            cboMachine.SetValue('');
            cboItemCheck.SetValue('');
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
                <td style="width: 50px"></td>
                <td style="width:100px; padding:5px 0px 0px 0px">
                    &nbsp;<dx:ASPxLabel ID="ASPxLabel9" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Item Check">
                    </dx:ASPxLabel>
                </td>
                <td style="width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxComboBox ID="cboItemCheck" runat="server" Theme="Office2010Black" TextField="ItemCheck" ClientInstanceName="cboItemCheck" ValueField="ItemCheck" Font-Names="Segoe UI" 
                        Font-Size="9pt" Height="25px" Width="190px" TabIndex="4" NullValueItemDisplayText="{1}">
                        <ClientSideEvents EndCallback="function(s, e) {cboItemCheck.SetEnabled(true);}"/>
                        <%--<ClientSideEvents SelectedIndexChanged="cboMachineChanged" EndCallback="function(s, e) {cboItemCheck.SetEnabled(true);}"/>--%>
                        <ItemStyle Height="10px" Paddings-Padding="4px">
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
                        <ClientSideEvents SelectedIndexChanged="cboLineChanged" EndCallback="function(s, e) {cboLine.SetEnabled(true);}"/>
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td style="width: 50px"></td>
                <td style="width:100px; padding:5px 0px 0px 0px">
                    &nbsp;
                    <dx:ASPxButton ID="btnSearch" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Theme="Office2010Silver" Height="28px"
                        Text="Browse" >
                        <ClientSideEvents Click="up_Browse" />
                    </dx:ASPxButton>
                </td>
                <td style="width:130px; padding:5px 0px 0px 0px">
                    <dx:ASPxButton ID="btnReset" runat="server" AutoPostBack="False" ClientInstanceName="btnReset" Theme="Office2010Silver" Height="28px"
                        Text="Clear">
                        <ClientSideEvents Click="Clear" />
                    </dx:ASPxButton>
                </td>
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
                        <%--<ClientSideEvents  EndCallback="function(s, e) {cboType.SetEnabled(true);}"/>--%>
                        <ClientSideEvents SelectedIndexChanged="cboTypeChanged" EndCallback="function(s, e) {cboType.SetEnabled(true);}"/>
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
                                        Height="25px" Text="Excel" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                        Width="90px" TabIndex="10">
                                        <Paddings Padding="2px" />
                                    </dx:ASPxButton>                            
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:300px">            
        
                    <asp:Label ID="Label2" runat="server" BackColor="#FFFF99" Visible="False" ForeColor="#333333"></asp:Label>
        
                </td>
            </tr>
        </table>
    
    </div>
    
        <asp:SqlDataSource ID="dsFactor1" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_FTAMaster_FillCombo '1' "></asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsFactor2" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_FTAMaster_FillCombo '2' "></asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsFactor3" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_FTAMaster_FillCombo '3' "></asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsFactor4" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_FTAMaster_FillCombo '4' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsCounterMeasure" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_FTAMaster_FillCombo '5' "></asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsCheckItem" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_FTAMaster_FillCombo '6' "></asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsCheckOrder" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_FTAMaster_FillCombo '7' "></asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsType" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '2' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsMachine" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '3' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsItemCheck" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_ItemCheckByBattery_FillCombo '5' "></asp:SqlDataSource>



    <div style="padding: 20px 5px 5px 5px">

        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            EnableTheming="True" KeyFieldName="FTAID" Theme="Office2010Black" Width="100%"
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
                
                <dx:GridViewDataTextColumn Caption="FTA ID" FieldName="FTAID"
                    VisibleIndex="1" Width="100px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit Width="70px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Top">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn Caption="Factor 1" FieldName="Factor1" VisibleIndex="2"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsFactor1" DropDownStyle="DropDown" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Factor1" ValueField="Factor1" ClientInstanceName="Factor1">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains" AllowCellMerge="True" ></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Factor 2" FieldName="Factor2" VisibleIndex="3"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsFactor2" DropDownStyle="DropDown" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Factor2" ValueField="Factor2" ClientInstanceName="Factor2">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains" AllowCellMerge="True" ></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Factor 3" FieldName="Factor3" VisibleIndex="4"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsFactor3" DropDownStyle="DropDown" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Factor3" ValueField="Factor3" ClientInstanceName="Factor3">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains" AllowCellMerge="True" ></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Factor 4" FieldName="Factor4" VisibleIndex="5"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsFactor4" DropDownStyle="DropDown" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                        TextField="Factor4" ValueField="Factor4" ClientInstanceName="Factor4">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains" AllowCellMerge="True" ></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataTextColumn Caption="Counter Measure" FieldName="CounterMeasure"
                    VisibleIndex="6" Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit  Width="195px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" AllowCellMerge="True" ></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left"></CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Check Item" FieldName="CheckItem"
                    VisibleIndex="7" Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit Width="195px">
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

                <dx:GridViewDataTextColumn Caption="Action" ShowInCustomizationForm="True" VisibleIndex="8" Width="50px">
                        <DataItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxHyperLink ID="linkAction" Font-Names="Segoe UI" Font-Size="9pt"
                                            runat="server" Text="View" OnInit="ActionLink_Init">
                                        </dx:ASPxHyperLink>
                                    </td>
                                </tr>
                            </table>
                        </DataItemTemplate>
                <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                </HeaderStyle>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
                
                <dx:GridViewDataTextColumn Caption="Instruksi Kerja (IK)" FieldName="IK" ShowInCustomizationForm="True" VisibleIndex="9" Width="100px">
                        <DataItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxHyperLink ID="linkUploadIK" Font-Names="Segoe UI" Font-Size="9pt"
                                            runat="server" Text="Upload" OnInit="linkUploadIK_Init">
                                        </dx:ASPxHyperLink>
                                    </td>
                                    <td> &nbsp;&nbsp; | &nbsp;&nbsp;</td>
                                    <td>
                                        <dx:ASPxHyperLink ID="linkIK" Font-Names="Segoe UI" Font-Size="9pt"
                                            runat="server" Text="View" OnInit="IKLink_Init">
                                        </dx:ASPxHyperLink>
                                    </td>
                                </tr>
                            </table>
                        </DataItemTemplate>
                <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                </HeaderStyle>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
                <%--<Settings AutoFilterCondition="Contains" AllowHeaderFilter="False" ></Settings>--%>
            </dx:GridViewDataTextColumn>
                
                <dx:GridViewDataTextColumn Caption="Check Order" FieldName="CheckOrder"
                    VisibleIndex="10" Width="50px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit Width="35px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataTextColumn>
                
                <dx:GridViewDataTextColumn Caption="Remark" FieldName="Remark"
                    VisibleIndex="11" Width="150px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit Width="195px">
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
                
                <dx:GridViewDataCheckColumn Caption="Active Status" FieldName="ActiveStatus" 
                    VisibleIndex="12" Width="60px">
                    <PropertiesCheckEdit ValueChecked="1" ValueType="System.Char" 
                        ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <Settings AllowSort="False" />
                </dx:GridViewDataCheckColumn>

                <dx:GridViewDataTextColumn Caption="Last User" FieldName="UpdateUser"
                    VisibleIndex="13" Width="70px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit Width="70px">
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
                    VisibleIndex="14" Width="150px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit Width="200px">
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

                <dx:GridViewDataComboBoxColumn Caption="" FieldName="LineName" VisibleIndex="15"
                    Width="1px" Settings-AutoFilterCondition="Contains" >
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
                
                <dx:GridViewDataComboBoxColumn Caption="" FieldName="ItemCheck" VisibleIndex="16"
                    Width="1px" Settings-AutoFilterCondition="Contains">
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

<FilterControl AutoUpdatePosition="False"></FilterControl>
            </SettingsPopup>

            <Styles EditFormColumnCaption-Paddings-PaddingLeft="10px" EditFormColumnCaption-Paddings-PaddingRight="10px">
                <Header Wrap="True">
                    <Paddings Padding="2px"></Paddings>
                </Header>

                <Cell VerticalAlign="Top">
                </Cell>

                <EditFormColumnCaption Font-Size="9pt" Font-Names="Segoe UI">
                    <Paddings PaddingLeft="5px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                </EditFormColumnCaption>
            </Styles>

            <Templates>
                <EditForm>
                    <div style="padding: 15px 15px 15px 15px; width: 300px">
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <table align="center">
                                <%--<tr style="height:30px">
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Factory Name" Width="90px"></dx:ASPxLabel>
                                    </td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFactoryCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="FactoryCode" >
                                        </dx:ASPxGridViewTemplateReplacement> 
                                    </td>
                                </tr>
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
                                    <td>Line Code</td>                                
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
                                </tr>--%>
                                <tr style="height:30px">
                                    <td>FTA ID</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFTAID" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="FTAID">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Factor 1</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFactor1" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Factor1">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Factor 2</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFactor2" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Factor2">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Factor 3</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFactor3" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Factor3">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Factor 4</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFactor4" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Factor4">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Counter Measure</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editCounterMeasure" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="CounterMeasure">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Check Item</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editCheckItem" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="CheckItem">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Check Order</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editCheckOrder" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="CheckOrder">
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
    
    <div>
        <dx:ASPxPopupControl ID="pcIK" runat="server" ClientInstanceName="pcIK" Height="600px" Width="450px" HeaderText="Instruksi Kerja (IK)" Modal="True" 
            CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
            
            <ContentCollection>

                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxCallbackPanel ID="cbkPanel" runat="server" Width="20%" ClientInstanceName="cbkPanel">
                        <SettingsLoadingPanel Enabled="False" />
                        <PanelCollection>
                            <dx:PanelContent runat="server">
                                <table class="auto-style20">
                                    <tr style="height:100%">
                                        <td style="text-align:center">
                                            <asp:Image ID="imgIK" runat="server" BorderStyle="Solid" Height="500" Width="430"/>
                                        </td>
                                    </tr>
                                </table>                            
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                    <table style="width:20%">
                        <tr style="width:100px">
                            <td style="text-align:center; padding-top: 10px;" class="dxcpCurrentColor">
                
                                <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" ClientInstanceName="btnCloseIK" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" Width="90px">
                                    <ClientSideEvents Click="ClosePopupIK" />
                                    <Paddings Padding="2px" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
    
                </dx:PopupControlContentControl>

            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
    
    <div>
        <dx:ASPxPopupControl ID="pcUploadIK" runat="server" ClientInstanceName="pcUploadIK" Height="100px" Width="250px" HeaderText="Upload Instruksi Kerja (IK)" Modal="True" 
            CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
            
            <ContentCollection>

                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxCallbackPanel ID="cbkPanelUploadIK" runat="server" Width="20%" ClientInstanceName="cbkPanelUploadIK">
                        <SettingsLoadingPanel Enabled="False" />
                        <PanelCollection>
                            <dx:PanelContent runat="server">
                                <table class="auto-style20">
                                    <tr style="height:100%">
                                        <td style="text-align:center">
                                            <asp:FileUpload ID="updIK" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Width="250px" Height="20px" />
                                            <asp:Label ID="lblMsgUpload" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Width="250px" Height="20px" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>                            
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                    <table style="width:20%">
                        <tr style="width:100px">
                            <td style="text-align:center; padding-top: 10px;" class="dxcpCurrentColor">

                                <table>
                                    <tr>
                                        <td>
                                            
                                            <dx:ASPxButton ID="btnUploadIK" runat="server" AutoPostBack="False" ClientInstanceName="btnUploadIK"
                                            Font-Names="Segoe UI" Font-Size="9pt" Text="Upload" Theme="Office2010Silver" Width="90px" Height="25px">
                                            </dx:ASPxButton>

                                        </td>
                                        <td> &nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td>

                                            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" ClientInstanceName="btnCloseIK" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" Width="90px">
                                                <ClientSideEvents Click="ClosePopupUploadIK" />
                                                <Paddings Padding="2px" />
                                            </dx:ASPxButton>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
    
                </dx:PopupControlContentControl>

            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>

    
    <div>
        <dx:ASPxPopupControl ID="pcAction" runat="server" ClientInstanceName="pcAction" Height="300px" Width="770px" HeaderText="A050 - FTA MASTER" Modal="True" 
            CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="True">
            
            <ContentCollection>

                <dx:PopupControlContentControl runat="server">

                    <dx:ASPxCallbackPanel ID="cbkPanelAction" runat="server" Width="100%" ClientInstanceName="cbkPanelAction">
                        <SettingsLoadingPanel Enabled="False" />
                        <PanelCollection>
                            <dx:PanelContent runat="server">
                                
                                <%--Grid Action--%>
                                
                                    <dx:ASPxGridView ID="gvFTAAction" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvFTAAction"
                                        EnableTheming="True" KeyFieldName="FTAIDAction;ActionID" Theme="Office2010Black" Width="100%"
                                        Font-Names="Segoe UI" Font-Size="9pt" OnRowInserting="gvFTAAction_RowInserting" OnRowDeleting="gvFTAAction_RowDeleting">
                                        <%--OnRowValidating="Grid_RowValidating" OnStartRowEditing="Grid_StartRowEditing"
                                        
                                        OnAfterPerformCallback="Grid_AfterPerformCallback">--%>
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

                                            <dx:GridViewDataTextColumn Caption="Action" FieldName="ActionName"
                                                VisibleIndex="1" Width="300px" Settings-AutoFilterCondition="Contains">
                                                <PropertiesTextEdit Width="300px">
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

                                            <dx:GridViewDataTextColumn Caption="Remark" FieldName="RemarkAction"
                                                VisibleIndex="2" Width="300px" Settings-AutoFilterCondition="Contains">
                                                <PropertiesTextEdit Width="300px" ClientInstanceName="RemarkAction">
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

                                            <dx:GridViewDataTextColumn Caption="" FieldName="FTAIDAction"
                                                VisibleIndex="3" Width="0px" Settings-AutoFilterCondition="Contains">
                                                <PropertiesTextEdit Width="100px" ClientInstanceName="FTAIDAction">
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

                                             <dx:GridViewDataTextColumn Caption="" FieldName="ActionID"
                                                VisibleIndex="4" Width="0px" Settings-AutoFilterCondition="Contains">
                                                <PropertiesTextEdit Width="100px" ClientInstanceName="ActionID">
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
                                        <Settings ShowFilterRow="False" VerticalScrollBarMode="Auto"
                                            VerticalScrollableHeight="300" HorizontalScrollBarMode="Auto" />
                                        <SettingsText ConfirmDelete="Are you sure want to delete ?"></SettingsText>
                                        <SettingsPopup>
                                            <EditForm Modal="false" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200" />

<FilterControl AutoUpdatePosition="False"></FilterControl>
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
                                                <div style="padding: 15px 15px 15px 15px; width: 400px">
                                                    <dx:ContentControl ID="ContentControl1" runat="server">
                                                        <table align="center">
                                                            <tr style="height:30px">
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="FTA ID" Width="90px"></dx:ASPxLabel>
                                                                </td>                                
                                                                <td>
                                                                    <dx:ASPxGridViewTemplateReplacement ID="editFTAIDAction" ReplacementType="EditFormCellEditor"
                                                                        runat="server" ColumnID="FTAIDAction">
                                                                    </dx:ASPxGridViewTemplateReplacement>   
                                                                </td>
                                                            </tr>
                                                            <tr style="height:30px">
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Action" Width="90px"></dx:ASPxLabel>
                                                                </td>                                
                                                                <td>
                                                                    <dx:ASPxGridViewTemplateReplacement ID="editActionName" ReplacementType="EditFormCellEditor"
                                                                        runat="server" ColumnID="ActionName" >
                                                                    </dx:ASPxGridViewTemplateReplacement> 
                                                                </td>
                                                            </tr>
                                                            <tr style="height:30px">
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Remark" Width="90px"></dx:ASPxLabel>
                                                                </td>                                
                                                                <td>
                                                                    <dx:ASPxGridViewTemplateReplacement ID="editRemarkAction" ReplacementType="EditFormCellEditor"
                                                                        runat="server" ColumnID="RemarkAction">
                                                                    </dx:ASPxGridViewTemplateReplacement>   
                                                                </td>
                                                            </tr>
                                                            <tr style="height:30px">
                                                                <td>
                                                                </td>                                
                                                                <td>
                                                                    <dx:ASPxGridViewTemplateReplacement ID="editActionID" ReplacementType="EditFormCellEditor"
                                                                        runat="server" ColumnID="ActionID">
                                                                    </dx:ASPxGridViewTemplateReplacement>   
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
                                

                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
    
                </dx:PopupControlContentControl>

            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
</asp:Content>
