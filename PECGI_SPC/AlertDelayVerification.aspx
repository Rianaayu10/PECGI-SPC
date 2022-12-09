<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AlertDelayVerification.aspx.vb" Inherits="PECGI_SPC.AlertDelayVerification" MasterPageFile="~/Site.Master" %>

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
            GridDelayVerif.PerformCallback('Load');
        }

        <%--window.onload=function(){

            var interval = document.getElementById("<%=hdInterval.ClientID %>").value;

            //var interval = document.getElementById("hdInterval").value;
            setInterval(() => {
                Grid.PerformCallback('Load');
                GridNG.PerformCallback('Load');
            }, interval);

        }--%>
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            gridHeight(200);

            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(80);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(300);
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
            GridDelayVerif.SetHeight(height - hAll);
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="divhead" style="padding: 0px 5px 5px 5px">
        <table style="width:100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width:60px; padding:1px 0px 0px 0px">
                                &nbsp;<dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Factory">
                                </dx:ASPxLabel>
                            </td>
                            <td style=" width:130px; padding:1px 0px 0px 0px">
                                <dx:ASPxComboBox ID="cboFactory" runat="server" Theme="Office2010Black" TextField="FactoryName" ClientInstanceName="cboFactory" ValueField="FactoryCode" Font-Names="Segoe UI" 
                                    Font-Size="9pt" Height="25px" IncrementalFilteringMode="Contains" Width="100px" TabIndex="6">
                                    <ItemStyle Height="10px" Paddings-Padding="4px" >
                                        <Paddings Padding="4px"></Paddings>
                                    </ItemStyle>
                                    <ButtonStyle Paddings-Padding="4px" Width="5px">
                                        <Paddings Padding="4px"></Paddings>
                                    </ButtonStyle>
                                </dx:ASPxComboBox>
                            </td>
                            <td style=" width:80px; padding:1px 0px 0px 0px">
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Prod Date" 
                                    Font-Names="Segoe UI" Font-Size="9pt">
                                </dx:ASPxLabel>
                            </td>
                            <td style=" width:80px; padding:1px 0px 0px 0px">
                                <table>
                                    <tr>
                                        <td style=" width:60px; padding:1px 0px 0px 0px">
                                            <asp:RadioButton ID="rbAuto" runat="server" GroupName="ProdDateSelection"/>  
                                        </td>
                                        <td style=" width:150px; padding:1px 0px 0px 0px">
                                            <p style="font-family: Segoe UI; Font-Size: 9pt">Today</p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style=" width:130px; padding:1px 0px 0px 0px">
                                <table>
                                    <tr>
                                        <td style=" width:60px; padding:1px 0px 0px 0px">
                                            <asp:RadioButton ID="rbManual" runat="server" Text="" GroupName="ProdDateSelection" />  
                                        </td>
                                        <td style=" width:100px; padding:1px 0px 0px 0px">
                                            <dx:ASPxDateEdit ID="dtDate" runat="server" Theme="Office2010Black" Width="100px" ClientInstanceName="dtDate" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                                            Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="2" EditFormat="Custom">
                                                <CalendarProperties ShowWeekNumbers="False">
                                                    <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" ><Paddings Padding="5px"></Paddings>
                                                    </HeaderStyle>
                                                    <DayStyle Font-Size="9pt" Paddings-Padding="5px" ><Paddings Padding="5px"></Paddings>
                                                    </DayStyle>
                                                    <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"><Paddings Padding="5px"></Paddings>
                                                    </WeekNumberStyle>
                                                    <FooterStyle Font-Size="9pt" Paddings-Padding="10px" ><Paddings Padding="10px"></Paddings>
                                                    </FooterStyle>
                                                    <ButtonStyle Font-Size="9pt" Paddings-Padding="10px"><Paddings Padding="10px"></Paddings>
                                                    </ButtonStyle>
                                                </CalendarProperties>                        
                                                <ButtonStyle Width="5px" Paddings-Padding="4px" >
                                                    <Paddings Padding="4px"></Paddings>
                                                </ButtonStyle>
                                            </dx:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style=" width:100px;">
                                <dx:ASPxButton ID="btnSearch" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Theme="Office2010Silver" Height="28px"
                                    Text="Browse" style="margin-left: 30px">
                                    <ClientSideEvents Click="up_Browse" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>

                <td style="float: left; width: 130px ">
                    <table style="width: 200px; height: 50px">
                        <tr>
                            <td style="width: 70px;" align="left">
                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>
                            <td style="width: 130px;" align="left">
                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Date :" CssClass="text" />
                                &nbsp
                                <dx:ASPxLabel ID="lblDateNow" ClientInstanceName="lblOK" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px;" align="left">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                            </td>
                            <td style="width: 130px;" align="left">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Time :" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                                &nbsp
                                <%--<dx:ASPxLabel ID="lblTimeNow" runat="server" Text="" CssClass="text" />--%>
                                <label id="lblTimeNow" style="font-family: 'Segoe UI'; font-size: 9pt"></label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    
        <asp:SqlDataSource ID="dsFactory" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPCAlertDashboard_FillCombo '1' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsType" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPCAlertDashboard_FillCombo '2' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsMachine" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPCAlertDashboard_FillCombo '3' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsItemCheck" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPCAlertDashboard_FillCombo '5' "></asp:SqlDataSource>
    
        <asp:SqlDataSource ID="dsShiftCode" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPCAlertDashboard_FillCombo '6' "></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsSequence" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPCAlertDashboard_FillCombo '7' "></asp:SqlDataSource>
                    
        <!-- Grid Delay Verification -->

        <div style="padding: 10px 5px 5px 5px">

            <div class="bg-color-grayDark" style="width: 100%;height: 25px">
                <center>
                    <label style="color: white; margin-top: 5px" >Production Sample - Delay Verification</label>
                </center>
            </div>

            <dx:ASPxGridView ID="GridDelayVerif" runat="server" AutoGenerateColumns="False" 
                ClientInstanceName="GridDelayVerif" EnableTheming="True" KeyFieldName="ItemTypeName;LineCode;ItemCheck" Theme="Office2010Black" Width="100%"
                Font-Names="Segoe UI" Font-Size="9pt" OnRowValidating="GridDelayVerif_RowValidating" OnStartRowEditing="GridDelayVerif_StartRowEditing"
                OnRowInserting="GridDelayVerif_RowInserting" OnRowDeleting="GridDelayVerif_RowDeleting" OnAfterPerformCallback="GridDelayVerif_AfterPerformCallback">
                <ClientSideEvents EndCallback="OnEndCallback" />
                <Columns>               
                    
                        <dx:GridViewDataTextColumn Caption="Action" FieldName="Edit"
                            VisibleIndex="0" Width="50px" Settings-AutoFilterCondition="Contains" 
                            FixedStyle="Left" >
                            <PropertiesTextEdit MaxLength="15" Width="50px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AllowAutoFilter="False" AllowFilterBySearchPanel="False" ShowInFilterControl="False" AllowHeaderFilter="False" />
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataComboBoxColumn Caption="Factory Code" FieldName="FactoryCode" VisibleIndex="0"
                            Width="200px" Settings-AutoFilterCondition="Contains" Visible="false">
                            <PropertiesComboBox DataSourceID="dsFactory" DropDownStyle="DropDownList" TextFormatString="{0}"
                                IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                                TextField="FactoryCode" ValueField="FactoryCode" ClientInstanceName="FactoryCode">
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

                        <dx:GridViewDataTextColumn Caption="Item Type Code" FieldName="ItemTypeCode"
                            VisibleIndex="0" Width="100px" Settings-AutoFilterCondition="Contains" 
                            FixedStyle="Left" Visible="false">
                            <PropertiesTextEdit MaxLength="15" Width="120px">
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

                        <dx:GridViewDataComboBoxColumn Caption="Type" FieldName="ItemTypeName" VisibleIndex="1"
                            Width="70px" Settings-AutoFilterCondition="Contains">
                            <PropertiesComboBox DataSourceID="dsType" DropDownStyle="DropDownList" TextFormatString="{0}"
                                IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="60px"
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

                        <dx:GridViewDataComboBoxColumn Caption="Machine Process" FieldName="LineCode" VisibleIndex="2"
                            Width="200px" Settings-AutoFilterCondition="Contains">
                            <PropertiesComboBox DataSourceID="dsMachine" DropDownStyle="DropDownList" TextFormatString="{0}"
                                IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="195px"
                                TextField="LineName" ValueField="LineCode" ClientInstanceName="LineCode">
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
                
                        <dx:GridViewDataComboBoxColumn Caption="Item Check" FieldName="ItemCheck" VisibleIndex="3"
                            Width="250px" Settings-AutoFilterCondition="Contains">
                            <PropertiesComboBox DataSourceID="dsItemCheck" DropDownStyle="DropDownList" TextFormatString="{0}"
                                IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="245px"
                                TextField="ItemCheck" ValueField="ItemCheckCode" ClientInstanceName="ItemCheckCode">
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

                        <dx:GridViewDataTextColumn Caption="Date" FieldName="Date"
                            VisibleIndex="4" Width="100px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="100px">
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

                        <dx:GridViewDataComboBoxColumn Caption="Shift" FieldName="ShiftCode" VisibleIndex="5"
                            Width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesComboBox DataSourceID="dsShiftCode" DropDownStyle="DropDownList" TextFormatString="{0}"
                                IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="50px"
                                TextField="ShiftCode" ValueField="ShiftCode" ClientInstanceName="ShiftCode">
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
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"/>
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataComboBoxColumn Caption="Seq" FieldName="SequenceNo" VisibleIndex="6"
                            Width="40px" Settings-AutoFilterCondition="Contains">
                            <PropertiesComboBox DataSourceID="dsSequence" DropDownStyle="DropDownList" TextFormatString="{0}"
                                IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="40px"
                                TextField="SequenceNo" ValueField="SequenceNo" ClientInstanceName="SequenceNo">
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
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"/>
                        </dx:GridViewDataComboBoxColumn>

                        <dx:GridViewDataTextColumn Caption="USL" FieldName="USL"
                            VisibleIndex="7" Width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="50px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="LSL" FieldName="LSL"
                            VisibleIndex="8" Width="50px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="50px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="UCL" FieldName="UCL"
                            VisibleIndex="8" Width="60px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="100px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="LCL" FieldName="LCL"
                            VisibleIndex="9" Width="60px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="100px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Min" FieldName="MinValue"
                            VisibleIndex="10" Width="60px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="100px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Max" FieldName="MaxValue"
                            VisibleIndex="11" Width="60px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="100px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Ave" FieldName="Average"
                            VisibleIndex="12" Width="60px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="100px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Operator" FieldName="Operator"
                            VisibleIndex="13" Width="70px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="70px">
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

                        <dx:GridViewDataTextColumn Caption="MK" FieldName="MK"
                            VisibleIndex="14" Width="70px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="70px">
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

                        <dx:GridViewDataTextColumn Caption="QC" FieldName="QC"
                            VisibleIndex="15" Width="70px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="70px">
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

                        <dx:GridViewDataTextColumn Caption="Verif Time" FieldName="VerifTime"
                            VisibleIndex="16" Width="100px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="100px">
                                <Style HorizontalAlign="Center"></Style>
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

                        <dx:GridViewDataTextColumn Caption="Delay Verif" FieldName="DelayVerif"
                            VisibleIndex="17" Width="170px" Settings-AutoFilterCondition="Contains">
                            <PropertiesTextEdit MaxLength="25" Width="170px">
                                <Style HorizontalAlign="Right"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Right" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewCommandColumn 
                            VisibleIndex="18" ShowClearFilterButton="true" Width="80px">
                            <HeaderStyle Paddings-PaddingLeft="3px" HorizontalAlign="Center" 
                                VerticalAlign="Middle" >
                                <Paddings PaddingLeft="3px"></Paddings>
                            </HeaderStyle>
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="SendEmail" Text="Send Email">
                                    <%--<Image ToolTip="Clone Record" Url="Images/clone.png" />--%>
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>

                        <dx:GridViewDataTextColumn Caption="LinkDate" FieldName="LinkDate"
                            VisibleIndex="19" Width="100px" Settings-AutoFilterCondition="Contains" Visible="false">
                            <PropertiesTextEdit MaxLength="25" Width="0px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="0px">
                            <Paddings PaddingRight="0px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="0px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="0px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status"
                            VisibleIndex="20" Width="100px" Settings-AutoFilterCondition="Contains" Visible="false">
                            <PropertiesTextEdit MaxLength="25" Width="0px">
                                <Style HorizontalAlign="Left"></Style>
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains"></Settings>
                            <FilterCellStyle Paddings-PaddingRight="0px">
                            <Paddings PaddingRight="0px"></Paddings>
                            </FilterCellStyle>
                            <HeaderStyle Paddings-PaddingLeft="0px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="0px"></Paddings>
                            </HeaderStyle>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                        </dx:GridViewDataTextColumn>
                                
                    </Columns>

                <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
                <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
                <SettingsPager Mode="ShowPager" PageSize="50" AlwaysShowPager="true">
                    <PageSizeItemSettings Visible="True" />
                </SettingsPager>
                <Settings ShowFilterRow="False" VerticalScrollBarMode="Auto" VerticalScrollableHeight="300" HorizontalScrollBarMode="Auto" />
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
            </dx:ASPxGridView>

            <br />
            <button disabled="disabled" style="background-color:yellow;width: 20px;height:10px"></button> <label> Delay < 60 Minutes</label>
            <button disabled="disabled" style="background-color:red;width: 20px;height:10px"></button> <label> Delay > 60 Minutes</label>

        </div>

        <!-- End Grid Delay Verification -->

    <div style="height:10px">
        <input type="hidden" runat="server" id="hdInterval" value="<%=hdInterval %>" />  
    </div>
    
    <script type="text/javascript" >
        window.onload = function () {

            var interval = document.getElementById("<%=hdInterval.ClientID %>").value;

            setTimer0 = setInterval(function () {

                Grid.PerformCallback('Load');
                GridNG.PerformCallback('Load');
                GridDelayVerif.PerformCallback('Load');
            }, interval, (0));

            setTimer1 = setInterval(function () {
                var today = new Date();
                document.getElementById('lblTimeNow').innerHTML = ("0" + today.getHours()).slice(-2) + ":" + ("0" + today.getMinutes()).slice(-2) + ":" + ("0" + today.getSeconds()).slice(-2);
            }, 1000, (1));
        }
    </script>
</asp:Content>
