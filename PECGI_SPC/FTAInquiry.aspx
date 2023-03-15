<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FTAInquiry.aspx.vb" Inherits="PECGI_SPC.FTAInquiry" %>

<%@ Register Assembly="DevExpress.XtraCharts.v20.2.Web, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .header {
            border: 1px solid silver; 
            background-color: #F0F0F0;
            text-align: center;
        }
        .body {
            border: 1px solid silver; 
        }
        .auto-style1 {
            height: 12px;
        }
        .auto-style2 {
            margin-right: 0;
        }
        .auto-style3 {
            width: 60px;
        }
        .auto-style5 {
            height: 28px;
        }
        .auto-style9 {
            width: 80px;
            height: 28px;
        }
        .auto-style10 {
            width: 60px;
            height: 30px;
        }
        .auto-style11 {
            width: 180px;
            height: 30px;
        }
        .auto-style12 {
            width: 100px;
            height: 30px;
        }
        .auto-style13 {
            width: 130px;
            height: 30px;
        }
        .auto-style18 {
            height: 28px;
            width: 130px;
        }
        .auto-style19 {
            width: 130px;
        }
        .auto-style20 {
            width: 125px;
        }
        .auto-style21 {
            width: 30px;
        }
        </style>
    <script type="text/javascript" >
        var rowIndex, columnIndex;
        var prevShift;
        var prevSeq;

        function ClearGrid(s, e) {
            grid.CancelEdit();
	        grid.PerformCallback('clear');
        }

        function cboFactoryChanged(s, e) { 
            cboProcessGroup.SetEnabled(false);
            cboProcessGroup.PerformCallback(cboFactory.GetValue());
        }

        function cboProcessGroupChanged(s, e) {
            cboLineGroup.SetEnabled(false);
            cboLineGroup.PerformCallback(cboFactory.GetValue() + '|' + cboProcessGroup.GetValue());
        }    

        function cboLineGroupChanged(s, e) {
            cboProcess.SetEnabled(false);
            cboProcess.PerformCallback(cboFactory.GetValue() + '|' + cboProcessGroup.GetValue() + '|' + cboLineGroup.GetValue());
        } 

        function cboProcessChanged(s, e) {
            cboLine.SetEnabled(false);
            cboLine.PerformCallback(cboFactory.GetValue()  + '|' + cboProcess.GetValue());
        } 

        function cboLineChanged(s, e) {    
            cboType.SetEnabled(false);
            cboType.PerformCallback(cboFactory.GetValue() + '|' + cboLine.GetValue());
        }

        function cboTypeChanged(s, e) {
            cboItemCheck.SetEnabled(false);
            cboItemCheck.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue());
        }

        function GridLoad(s, e) {
            grid.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboMK.GetValue() + '|' + cboQC.GetValue() + '|0');
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

        var SPCResultID = '';
        function OnSelectionChanged(s, e) {
            var i = s.GetSelectedRowCount();
            btnSample.SetEnabled(i > 0);
            btnFTA.SetEnabled(i > 0);
            s.GetSelectedFieldValues("SPCResultID", GetSelectedFieldValuesCallback);
        }

        function GetSelectedFieldValuesCallback(values) {
            for (var i = 0; i < values.length; i++) {
                SPCResultID = values[i];
            }
        }

        function SPCSample() {
            window.open('ProdSampleInput.aspx?menu=prodSampleVerification.aspx' + '&SPCResultID=' + SPCResultID, '_blank');            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 0px 5px 5px 5px">
        <table class="auto-style3" style="width: 100%">
        <tr >
            <td style="padding:5px 0px 0px 0px" class="auto-style10">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Factory" Font-Names="Segoe UI" 
                    Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" padding:5px 0px 0px 0px" class="auto-style11">
                <dx:ASPxComboBox ID="cboFactory" runat="server" Theme="Office2010Black" TextField="FactoryName"
                    ClientInstanceName="cboFactory" ValueField="FactoryCode" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" 
                    Width="150px" TabIndex="6" EnableCallbackMode="True">
                    <ClientSideEvents SelectedIndexChanged="cboFactoryChanged" />

                    <ItemStyle Height="10px" Paddings-Padding="4px" >
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
            </td>
            <td style=" padding:5px 0px 0px 10px" class="auto-style12">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Machine" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding:5px 0px 0px 0px" class="auto-style13">

                <dx:ASPxComboBox ID="cboProcess" runat="server" Theme="Office2010Black" TextField="ProcessName"
                    ClientInstanceName="cboProcess" ValueField="ProcessCode" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="190px" TabIndex="4" NullValueItemDisplayText="{1}">
                    <ClientSideEvents SelectedIndexChanged="cboProcessChanged" EndCallback="function(s, e) {cboProcess.SetEnabled(true);}"/>
                    <ItemStyle Height="10px" Paddings-Padding="4px">
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>

                </td>

            <td style=" padding: 3px 0px 0px 10px; ">
                <dx:ASPxLabel ID="lblqeleader0" runat="server" Text="Item Check" 
                    Font-Names="Segoe UI" Font-Size="9pt" Width="100px">
                </dx:ASPxLabel>
            </td>
            <td style="padding: 5px 0px 0px 0px" colspan="5">
                
                    
                <dx:ASPxComboBox ID="cboItemCheck" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboItemCheck" ValueField="ItemCheckCode" TextField="ItemCheck" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="240px" TabIndex="5" >
                    <ItemStyle Height="10px" Paddings-Padding="4px"><Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px"><Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
                
                    
                </td>                        
        </tr>
            <tr>
                <td class="auto-style5">

                <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Process Group" Font-Names="Segoe UI" 
                    Font-Size="9pt" Width="90px" Height="16px">
                </dx:ASPxLabel>

                </td>
                <td style="padding:5px 0px 0px 0px" class="auto-style5">

                <dx:ASPxComboBox ID="cboProcessGroup" runat="server" Theme="Office2010Black" TextField="ProcessGroupName"
                    ClientInstanceName="cboProcessGroup" ValueField="ProcessGroup" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="150px" TabIndex="6" EnableCallbackMode="True">
                    <ClientSideEvents SelectedIndexChanged="cboProcessGroupChanged" EndCallback="function(s, e) {cboProcessGroup.SetEnabled(true);}"/>

                    <ItemStyle Height="10px" Paddings-Padding="4px" >
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>

                </td>
                <td style="padding:3px 0px 0px 10px" class="auto-style5">

                <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Machine Process" Font-Names="Segoe UI" 
                    Font-Size="9pt" Width="100px">
                </dx:ASPxLabel>

                </td>
                <td style="padding:5px 0px 0px 0px" class="auto-style18">

                <dx:ASPxComboBox ID="cboLine" runat="server" Theme="Office2010Black" TextField="LineName"
                    ClientInstanceName="cboLine" ValueField="LineCode" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="190px" TabIndex="4" NullValueItemDisplayText="{1}">
                    <ClientSideEvents SelectedIndexChanged="cboLineChanged" EndCallback="function(s, e) {cboLine.SetEnabled(true);}"/>
                    <ItemStyle Height="10px" Paddings-Padding="4px">
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>

                </td>
                <td style=" padding: 5px 0px 0px 10px; " class="auto-style9">

                <dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Prod Date" 
                    Font-Names="Segoe UI" Font-Size="9pt" Width="70px">
                </dx:ASPxLabel>

                </td>
                <td style="padding: 5px 0px 0px 0px; width: 110px;">

                                <dx:ASPxDateEdit ID="dtDate" runat="server" Theme="Office2010Black" 
                    Width="100px"
                        ClientInstanceName="dtDate" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="2" 
                    EditFormat="Custom">
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
                <td style="padding:5px 0px 0px 10px; width: 20px;">

                <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="to" 
                    Font-Names="Segoe UI" Font-Size="9pt" Width="10px">
                </dx:ASPxLabel>

                </td>
                <td style="padding:5px 0px 0px 0px; " colspan="2">

                                <dx:ASPxDateEdit ID="dtTo" runat="server" Theme="Office2010Black" 
                    Width="100px"
                        ClientInstanceName="dtTo" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="2" 
                    EditFormat="Custom">
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
                        </ButtonStyle>
                    </dx:ASPxDateEdit>

                
                </td>
                <td style="padding:5px 0px 0px 0px;">
                    &nbsp;</td>  
                <td style="width: 100%">

                </td>
            </tr>

        <tr>
            <td style=" padding:5px 0px 0px 0px" class="auto-style3">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Line Group" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" width:130px; padding:5px 0px 0px 0px">
                
                <dx:ASPxComboBox ID="cboLineGroup" runat="server" Theme="Office2010Black" TextField="LineGroupName"
                    ClientInstanceName="cboLineGroup" ValueField="LineGroup" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" 
                    Width="150px" TabIndex="6" EnableCallbackMode="True">
                    <ClientSideEvents SelectedIndexChanged="cboLineGroupChanged" EndCallback="function(s, e) {cboLineGroup.SetEnabled(true);}" />

                    <ItemStyle Height="10px" Paddings-Padding="4px" >
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>

            </td>
             <td style=" padding: 5px 0px 0px 10px; width:80px">
                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Type" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding: 5px 0px 0px 0px;" class="auto-style19">
                <dx:ASPxComboBox ID="cboType" runat="server" Theme="Office2010Black" TextField="Description"
                    ClientInstanceName="cboType" ValueField="ItemTypeCode" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="100px" TabIndex="6" EnableCallbackMode="True">                    
                    <ClientSideEvents SelectedIndexChanged="cboTypeChanged" EndCallback="function(s, e) {cboType.SetEnabled(true);}"/>
                    <ItemStyle Height="10px" Paddings-Padding="4px" >
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>

                
                </td>

<td style=" padding: 5px 0px 0px 10px; ">
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="MK Verification" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding:5px 0px 0px 0px">
                
                <dx:ASPxComboBox ID="cboMK" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboMK" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="100px" TabIndex="3">
                    <Items>
                        <dx:ListEditItem Text="ALL" Value="0" Selected="true" />
                        <dx:ListEditItem Text="Yes" Value="1" />
                        <dx:ListEditItem Text="No" Value="2" />
                    </Items>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>                
            </td>
            <td style="padding:5px 3px 0px 10px" colspan="2">
                <dx:ASPxLabel ID="lblqeleader2" runat="server" Text="QC Verification" 
                    Font-Names="Segoe UI" Font-Size="9pt" Width="90px">
                </dx:ASPxLabel>
            </td>            
            <td style="padding:5px 0px 0px 0px" colspan="2">
                
                <dx:ASPxComboBox ID="cboQC" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboQC" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="100px" TabIndex="3">
                    <Items>
                        <dx:ListEditItem Text="ALL" Value="0"  Selected="true"/>
                        <dx:ListEditItem Text="Yes" Value="1" />
                        <dx:ListEditItem Text="No" Value="2" />
                    </Items>

                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
                
            </td>
            <td>

                

            </td>
            <td style="padding:5px 0px 0px 15px">

                
                <dx:ASPxButton ID="btnSearch" runat="server" AutoPostBack="False" 
                    ClientInstanceName="btnSearch" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="Browse" Theme="Office2010Silver" UseSubmitBehavior="False" 
                    Width="90px" TabIndex="10">
                    <ClientSideEvents Click="function(s, e) {
                        var errmsg = '';
                        if(cboFactory.GetText() == '') {
                            cboFactory.Focus();
                            errmsg = 'Please select Factory!';                                                                
	                    } else if(cboType.GetText() == '') {
                            cboType.Focus();
                            errmsg = 'Please select Type!';
	                    } else if(cboLine.GetText() == '') {
                            cboLine.Focus();
                            errmsg = 'Please select Machine Process!';
	                    } else if(cboItemCheck.GetText() == '') {
                            cboItemCheck.Focus();
                            errmsg = 'Please select Item Check!';
	                    } else if(cboMK.GetText() == '') {
                            cboMK.Focus();
                            errmsg = 'Please select Shift!';
	                    } else if(cboQC.GetText() == '') {
                            cboQC.Focus();
                            errmsg = 'Please select Sequence!';
	                    }

                        if(errmsg != '') {
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
 	                    grid.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + dtTo.GetText() + '|' + cboMK.GetValue() + '|' + cboQC.GetValue());
                    }" />
                    <Paddings Padding="2px" />
                </dx:ASPxButton>

                
            </td>            
        </tr>
    </table>
    </div>
<div style="height:10px">
    <dx:ASPxHiddenField ID="hfUserID" runat="server" ClientInstanceName="hfUserID">
    </dx:ASPxHiddenField>
</div>
<hr style="border-color:darkgray; " class="auto-style1"/>
<div>
    <table style="width: 100%;">
        <tr>
            <td>
                <table>
                    <tr>
                        <td style="padding-right:5px; padding-left:5px">
                                <dx:ASPxButton ID="btnFTA" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnFTA" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Corrective Action SPC" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="160px" TabIndex="10">
                                    <Paddings Padding="2px" />                                    
                                </dx:ASPxButton>
                        </td>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnSample" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnSample" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="SPC Sample" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="90px" TabIndex="10">
                                    <Paddings Padding="2px" />  
                                    <ClientSideEvents Click="SPCSample"/>
                                </dx:ASPxButton>                             
                        </td>
                        <td style="padding-right:5px" class="auto-style20">
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
<div style="padding: 10px 5px 5px 5px">
<dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
            EnableTheming="True" KeyFieldName="SeqNo" Theme="Office2010Black"            
            Width="100%" 
            Font-Names="Segoe UI" Font-Size="9pt" CssClass="auto-style2">
            <ClientSideEvents 
                EndCallback="OnEndCallback" SelectionChanged="OnSelectionChanged"
             />
            <SettingsResizing ColumnResizeMode="Control" />
            <SettingsDataSecurity AllowDelete="False" />

<SettingsPopup>
    <EditForm Modal="false" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200" />
    <FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>
        <Columns>

            <dx:GridViewCommandColumn Caption=" " ShowRecoverButton="False" ShowSelectCheckbox="True" VisibleIndex="0" Width="30px">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="ProdDate" VisibleIndex="1" Width="90px">
                <PropertiesTextEdit DisplayFormatString="dd MMM yyyy">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="ShiftCode" VisibleIndex="2" Width="50px" Caption="Shift">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SequenceNo" VisibleIndex="3" Width="40px" Caption="Seq">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ItemCheck" VisibleIndex="4" Width="200px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="5" Width="40px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CheckItem" VisibleIndex="6" Width="160px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ActionName" VisibleIndex="7" Caption="Action" Width="120px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Remark" VisibleIndex="12">
            </dx:GridViewDataTextColumn>
            <dx:GridViewBandColumn Caption="MK Verification" VisibleIndex="8">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="PIC" FieldName="MKVerificationUser" VisibleIndex="0" Width="90px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date" FieldName="MKVerificationDate" VisibleIndex="1" Width="90px">
                        <PropertiesTextEdit DisplayFormatString="dd MMM yyyy">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:GridViewBandColumn>
            <dx:GridViewBandColumn Caption="QC Verification" VisibleIndex="9">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="PIC" FieldName="QCVerificationUser" VisibleIndex="0" Width="90px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date" FieldName="QCVerificationDate" VisibleIndex="1" Width="90px">
                        <PropertiesTextEdit DisplayFormatString="dd MMM yyyy">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:GridViewBandColumn>

            <dx:GridViewDataTextColumn FieldName="SPCResultID" Visible="False" VisibleIndex="13">
            </dx:GridViewDataTextColumn>

        </Columns>        
        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" AllowDragDrop="False" AllowSort="False" />
        <SettingsEditing Mode="Batch" EditFormColumnCount="1" >
            <BatchEditSettings ShowConfirmOnLosingChanges="False" />
            </SettingsEditing>
        <SettingsPager AlwaysShowPager="true" Mode="ShowAllRecords" PageSize="30">
        </SettingsPager>
        <Settings HorizontalScrollBarMode="Auto" VerticalScrollableHeight="180" 
            VerticalScrollBarMode="Auto" ShowStatusBar="Hidden" />
        <Styles EditFormColumnCaption-Paddings-PaddingLeft="10px" 
            EditFormColumnCaption-Paddings-PaddingRight="10px" 
            Header-Paddings-Padding="5px">
            <Header HorizontalAlign="Center" Wrap="True">
                <Paddings Padding="2px" />
            </Header>
            <DetailCell Wrap="False">
                            </DetailCell>
                            <SelectedRow BackColor="White" ForeColor="Black">
                            </SelectedRow>
            <EditFormColumnCaption Font-Names="Segoe UI" Font-Size="9pt">
                <Paddings PaddingBottom="5px" PaddingLeft="15px" PaddingRight="15px" PaddingTop="5px" />
            </EditFormColumnCaption>
            <CommandColumnItem ForeColor="SteelBlue">
            </CommandColumnItem>
            <BatchEditModifiedCell BackColor="#FFFF99" ForeColor="Black">
            </BatchEditModifiedCell>
        </Styles>
        <Templates>
                <EditForm>
                    <div style="padding: 5px 15px 5px 15px; width: 300px">
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <table align="center">
                                <tr style="height: 26px">
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Value" Width="90px"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editValue" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Value"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 26px">
                                    <td>Delete Status</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editDeleteStatus" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="DeleteStatus"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 26px">
                                    <td>Remarks</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editRemark" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Remark"></dx:ASPxGridViewTemplateReplacement>
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
</div>
</asp:Content>

