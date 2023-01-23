<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SampleControlQuality.aspx.vb" Inherits="PECGI_SPC.SampleControlQuality" %>
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

        .vheader {
            border: 1px solid silver; 
            background-color: #F0F0F0;            
            text-align: left;
        }

        .body {
            border: 1px solid silver; 
        }
        
            .auto-style1 {
                width: 60%;
            }
        
            .auto-style2 {
                border: 1px solid silver;
                background-color: #F0F0F0;
                text-align: left;
                width: 70px;
                height: 17px;
            }
            .auto-style3 {
                border: 1px solid silver;
                width: 90px;
                height: 17px;
            }
            .auto-style4 {
                border: 1px solid silver;
                background-color: #F0F0F0;
                text-align: left;
                height: 16px;
            }
            .auto-style6 {
                border: 1px solid silver;
                background-color: #F0F0F0;
                text-align: left;
                width: 124px;
            }
            .auto-style7 {
                border: 1px solid silver;
                background-color: #F0F0F0;
                text-align: left;
                height: 16px;
                width: 124px;
            }
            .auto-style8 {
                border: 1px solid silver;
                background-color: #F0F0F0;
                text-align: left;                
                width: 124px;
                height: 17px;
            }
            .auto-style9 {
                border: 1px solid silver;
                width: 70px;
            }
            .auto-style10 {
                border: 1px solid silver;
                height: 16px;
                width: 70px;
            }
            .auto-style11 {
                border: 1px solid silver;
                width: 70px;
                height: 17px;
            }
            .auto-style12 {
                border: 1px solid silver;
                width: 67px;
                height: 17px;
            }
            .auto-style13 {
                border: 1px solid silver;
                width: 67px;
            }
            .auto-style14 {
                border: 1px solid silver;
                height: 16px;
                width: 67px;
            }
        
            .auto-style15 {
                width: 60px;
                height: 30px;
            }
            .auto-style16 {
                width: 130px;
                height: 30px;
            }
            .auto-style18 {
                width: 200px;
                height: 30px;
            }
                    
            .auto-style20 {
                height: 30px;
            }
        
            .auto-style21 {
                height: 2px;
            }
        
            .auto-style23 {
                width: 96px;
            }
        
            </style>
    <script type="text/javascript" >
        var rowIndex, columnIndex;
        function OnInit(s, e) {
            var d = new Date(2022, 9, 4);            
            var d2 = new Date(2022, 9, 11);
            var x = document.getElementById("chartRdiv");
            x.style.display = "none";
        }

        function isNumeric(n) {
            if (n == null || n == '') {
                return true;
            } else {
                return !isNaN(parseFloat(n)) && isFinite(n);
            }
        }

        function GridLoad(s, e) {
            gridX.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShow.GetValue());
        }

        function ValidateSave(s, e) {
            var rows = grid.GetVisibleRowsOnPage();
            var startIndex = 0;
            var i;
            var errmsg = '';      
            
            for (i = startIndex; i < rows - 1; i++) {
                if (isNumeric(grid.batchEditApi.GetCellValue(i, 'Value')) == false) {
                    errmsg = 'Please input valid numeric!';
                    grid.batchEditApi.StartEdit(i, 1);
                    break;
                }
            }                  
            if (errmsg != '') {
                e.cancel = true; 
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
            grid.UpdateEdit();
        }

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

        function OnBatchEditStartEditing(s, e) {
            if(cboSeq.GetValue() == '') {
                e.cancel = true;                
                toastr.warning('Please select Sequence!', 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;
            }
            currentColumnName = e.focusedColumn.fieldName;            
            currentEditableVisibleIndex = e.visibleIndex;          
        }      

        function OnBatchEditEndEditing(s, e) {            
            rowIndex = null;
            columnIndex = null;            
        }

        function ChartREndCallBack(s, e) {
            var i = s.cpShow;
            var x = document.getElementById("chartRdiv");
            if (i=='1') {
                x.style.display = "block";
            } else {
                x.style.display = "none";
            }
            
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

            lblUSL.SetText(s.cpUSL);
            lblLSL.SetText(s.cpLSL);
            lblUCL.SetText(s.cpUCL);
            lblCL.SetText(s.cpCL);
            lblLCL.SetText(s.cpLCL);
            lblXLCL.SetText(s.cpXLCL);
            lblXUCL.SetText(s.cpXUCL);
            lblRUCL.SetText(s.cpRUCL);
            lblD2.SetText(s.cpD2);

            lblMin.SetText(s.cpMin);
            lblMax.SetText(s.cpMax);
            lblCP.SetText(s.cpCP);
            lblCPK1.SetText(s.cpCPK1);
            lblCPK2.SetText(s.cpCPK2);
            lblCPKMin.SetText(s.cpCPKMin);
            lblXBarBar.SetText(s.cpXBarBar);
            lblRBar.SetText(s.cpRBar);

            lblFactory.SetText(s.cpFactory);
            lblType.SetText(s.cpType);
            lblLine.SetText(s.cpLine);
            lblItemCheck.SetText(s.cpItemCheck);
            lblProdDate.SetText(s.cpProdDate);
            lblUnit.SetText(s.cpUnit);


            if (s.cpXBarColor == '1') {                
                document.getElementById('tdXBarBar').style.backgroundColor = 'Pink';
            } else {
                document.getElementById('tdXBarBar').style.backgroundColor = 'White';
            }
            if (s.cpRBarColor == '1') {                
                document.getElementById('tdRBar').style.backgroundColor = 'Pink';
            } else {
                document.getElementById('tdRBar').style.backgroundColor = 'White';
            }

            chartX.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + dtTo.GetText() + '|' + cboShow.GetValue());
            chartR.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + dtTo.GetText() + '|' + cboShow.GetValue());                
            Histogram.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + dtTo.GetText() + '|' + cboShow.GetValue());
        }

        function ClosePopupRule1(s, e) {
            pcRule1.Hide();
            e.processOnServer = false;
        }

        function ClosePopupRule2(s, e) {
            pcRule2.Hide();
            e.processOnServer = false;
        }

        function ShowPopUpRule1(s, e) {
            pcRule1.Show();
        }

        function ShowPopUpRule2(s, e) {
            pcRule2.Show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 0px 5px 5px 5px">
        <table class="nav-justified">
        <tr >
            <td style="padding:5px 0px 0px 0px" class="auto-style20">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Factory" Font-Names="Segoe UI" 
                    Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" padding:5px 0px 0px 0px" class="auto-style20">
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
            <td style=" padding:5px 0px 0px 10px;" class="auto-style20">
                <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Machine" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding:5px 0px 0px 0px" class="auto-style20">

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
            <td style=" padding: 5px 0px 0px 10px;" class="auto-style20">
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Item Check" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>            
            <td style="padding:5px 0px 0px 10px; width:250px" colspan="3" class="auto-style20">                

                
                <dx:ASPxComboBox ID="cboItemCheck" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboItemCheck" ValueField="ItemCheckCode" TextField="ItemCheck" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="240px" TabIndex="5" >
                    <ClientSideEvents EndCallback="function(s, e) {
                            cboItemCheck.SetEnabled(true);                            
                       }"/>

                    <ItemStyle Height="10px" Paddings-Padding="4px">
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
                
                    
                </td>            
           
           
           
            <td style="padding: 5px 0px 0px 0px; width:100%" colspan="3">

                </td>
        </tr>

        <tr>
            <td style=" padding:5px 0px 0px 0px" class="auto-style15">

                <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Process Group" Font-Names="Segoe UI" 
                    Font-Size="9pt" Width="90px">
                </dx:ASPxLabel>

            </td>
            <td style=" padding:5px 0px 0px 0px" class="auto-style16">
                
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
            <td style=" padding:5px 0px 0px 10px">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Machine Process" 
                    Font-Names="Segoe UI" Font-Size="9pt" Width="100px">
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
            <td style=" padding:5px 0px 0px 10px;">
                <dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Prod Date" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" padding:5px 0px 0px 10px;">
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
            <td style=" padding:5px 0px 0px 8px; width:30px">
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="To" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel></td>
            <td style="padding:5px 0px 0px 0px;" align="right">
                                <dx:ASPxDateEdit ID="dtTo" runat="server" ClientInstanceName="dtTo" DisplayFormatString="dd MMM yyyy" EditFormat="Custom" EditFormatString="dd MMM yyyy" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="2" Theme="Office2010Black" Width="100px">
                                    <CalendarProperties ShowWeekNumbers="False">
                                        <HeaderStyle Font-Size="12pt" Paddings-Padding="5px">
                                        <Paddings Padding="5px" />
                                        </HeaderStyle>
                                        <DayStyle Font-Size="9pt" Paddings-Padding="5px">
                                        <Paddings Padding="5px" />
                                        </DayStyle>
                                        <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px">
                                            <Paddings Padding="5px" />
                                        </WeekNumberStyle>
                                        <FooterStyle Font-Size="9pt" Paddings-Padding="10px">
                                        <Paddings Padding="10px" />
                                        </FooterStyle>
                                        <ButtonStyle Font-Size="9pt" Paddings-Padding="10px">
                                            <Paddings Padding="10px" />
                                        </ButtonStyle>
                                    </CalendarProperties>
                                    <ButtonStyle Paddings-Padding="4px" Width="5px">
                                        <Paddings Padding="4px" />
                                    </ButtonStyle>
                                </dx:ASPxDateEdit>
                                
                
                
            

            </td>
            <td style="padding:5px 0px 0px 20px" colspan="3">

                </td>
        </tr>
            <tr>
                <td style="padding:5px 0px 0px 0px">
                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Line Group" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
                </td>
                <td style="padding:5px 0px 0px 0px">

                    <dx:ASPxComboBox ID="cboLineGroup" runat="server" ClientInstanceName="cboLineGroup" EnableCallbackMode="True" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" IncrementalFilteringMode="Contains" TabIndex="6" TextField="LineGroupName" Theme="Office2010Black" ValueField="LineGroup" Width="150px">
                        <ClientSideEvents EndCallback="function(s, e) {cboLineGroup.SetEnabled(true);}" SelectedIndexChanged="cboLineGroupChanged" />
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                        <Paddings Padding="4px" />
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px" />
                        </ButtonStyle>
                    </dx:ASPxComboBox>

                </td>
                <td style="padding:5px 0px 0px 10px">

                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Type" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>

                </td>
                <td style="padding:5px 0px 0px 0px">

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
                <td style=" padding:3px 0px 0px 10px;">

                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Show Verified Only" 
                        Font-Names="Segoe UI" Font-Size="9pt" Width="109px">
            </dx:ASPxLabel>                                

                </td>
                <td style="padding:5px 0px 0px 10px">

                <dx:ASPxComboBox ID="cboShow" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboShow" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="58px" TabIndex="9" SelectedIndex="0">
                    <Items>                        
                        <dx:ListEditItem Text="No" Value="0" Selected="true"/>
                        <dx:ListEditItem Text="Yes" Value="1" />
                    </Items>
                    <ItemStyle Height="10px" Paddings-Padding="4px"><Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px"><Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>                                    
                </td>
                <td style="width:30px">

                </td>
                <td>

                </td>
                <td style="padding:5px 0px 0px 20px">

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
	                    }

                        if(errmsg == '') {
                            var d1 = dtDate.GetValue();
                            var d2 = dtTo.GetValue();
                            var m1 = d1.getMonth() + 1;
                            var m2 = d2.getMonth() + 1;
                            var y1 = d1.getFullYear();
                            var y2 = d2.getFullYear();
                            var m = (m2 + y2 * 12) - (m1 + y1 * 12);
	    	                if (m &gt; 2) {
			                    errmsg = 'Maximum search 3 months period!';
		                    }
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
                         gridX.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + dtTo.GetText() + '|' + cboShow.GetValue());                        
                    }" />
                    <Paddings Padding="2px" />
                </dx:ASPxButton>
                
                </td>
                <td style="padding:5px 0px 0px 6px" class="auto-style23">

                                <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnExcel" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Excel" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="90px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                </dx:ASPxButton>                            

                </td>
                <td style="width:100%">

                </td>
            </tr>
    </table>
    </div>
<div style="height:10px">
    <dx:ASPxHiddenField ID="hfRevNo" runat="server" ClientInstanceName="hfRevNo">
    </dx:ASPxHiddenField>
</div>
<div style="padding: 10px 5px 5px 5px">
<dx:ASPxGridView ID="gridX" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridX"
                                EnableTheming="True" KeyFieldName="Des" Theme="Office2010Black"            
                                Width="100%" 
                                Font-Names="Segoe UI" Font-Size="9pt">


                    <ClientSideEvents EndCallback="OnEndCallback" />


                    <SettingsPager Mode="ShowAllRecords">
                    </SettingsPager>


                    <Settings HorizontalScrollBarMode="Auto" VerticalScrollableHeight="420" VerticalScrollBarMode="Auto" />
                    <SettingsBehavior AllowSort="False" />
                    <SettingsResizing ColumnResizeMode="Control" />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />


<SettingsPopup>
<FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>
                            <Columns>
                                <dx:GridViewBandColumn Caption="DATE" VisibleIndex="0">
                                    <Columns>
                                        <dx:GridViewBandColumn Caption="SHIFT" VisibleIndex="0">
                                            <Columns>
                                                <dx:GridViewBandColumn Caption="TIME" VisibleIndex="0">
                                                </dx:GridViewBandColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>
                                    </Columns>
                                </dx:GridViewBandColumn>
                    </Columns>
                            <Styles>
                                <Header HorizontalAlign="Center">
                                </Header>
                            </Styles>


                            </dx:ASPxGridView>    
</div>

<table>
            <tr>
                <td style="padding:10px 5px 10px 0px">
                    <dx:ASPxButton ID="btnRule" runat="server" AutoPostBack="False" ClientInstanceName="btnRule" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="View Table Rule" Theme="Office2010Silver" UseSubmitBehavior="False" Width="120px">
                        <Paddings Padding="2px" />
                        <ClientSideEvents Click="ShowPopUpRule1" />
                    </dx:ASPxButton>
                </td>
                <td style="padding:10px 0px 10px 5px">
                    &nbsp;</td>
                <td>
                    <dx:ASPxPopupControl ID="pcRule1" runat="server" ClientInstanceName="pcRule1" CloseAction="CloseButton" CloseOnEscape="true" HeaderText="Table Rule" Height="250px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="600px">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <div style="height:100%; text-align: center; padding-top: 30px;">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/rule1.png" />
                                </div>
                                <table style="width:100%">
                                    <tr>
                                        <td style="text-align:center; padding-top: 10px;">
                                            <dx:ASPxButton ID="btnHide" runat="server" AutoPostBack="False" ClientInstanceName="btnHide" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" Width="90px">
                                                <Paddings Padding="2px" />
                                                <ClientSideEvents Click="ClosePopupRule1" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                </td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxPopupControl ID="pcRule2" runat="server" ClientInstanceName="pcRule2" CloseAction="CloseButton" CloseOnEscape="true" HeaderText="Break SPC Rule" Height="250px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="600px">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <div style="height:100%; text-align: center; padding-top: 30px;">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/rule2.png" />
                                </div>
                                <table style="width:100%">
                                    <tr>
                                        <td style="text-align:center; padding-top: 10px;">
                                            <dx:ASPxButton ID="btnHide2" runat="server" AutoPostBack="False" ClientInstanceName="btnHide2" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" Width="90px">
                                                <Paddings Padding="2px" />
                                                <ClientSideEvents Click="ClosePopupRule2" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                </td>
            </tr>
        </table>
<div style="width:100%; border:1px solid black">
<dx:WebChartControl ID="chartX" runat="server" ClientInstanceName="chartX"
        Height="434px" Width="1080px" CrosshairEnabled="True" SeriesDataMember="Description" ToolTipEnabled="False">
        <seriestemplate SeriesDataMember="Description" ArgumentDataMember="Seq" ValueDataMembersSerializable="Value" CrosshairLabelPattern="{S}: {V:0.000}">
            <viewserializable>
                <cc1:PointSeriesView>                    
                    <PointMarkerOptions kind="Circle" BorderColor="255, 255, 255" Size="4"></PointMarkerOptions>
                </cc1:PointSeriesView>
            </viewserializable>
        </seriestemplate>    
        <SeriesSerializable>
            <cc1:Series ArgumentDataMember="Seq" Name="Rule" ValueDataMembersSerializable="RuleValue" LabelsVisibility="False" ShowInLegend="False" CrosshairEnabled="False">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Red" Transparency="200" AxisYName="Secondary AxisY 1">
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="RuleYellow" ValueDataMembersSerializable="RuleYellow" LabelsVisibility="False" ShowInLegend="False" CrosshairEnabled="False">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Yellow" Transparency="200" AxisYName="Secondary AxisY 1">
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="Average" ValueDataMembersSerializable="AvgValue">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="Blue">
                        <LineStyle Thickness="1" />
                        <LineMarkerOptions Color="Blue" Size="3" Kind="Diamond">
                        </LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
        </SeriesSerializable>     
        <DiagramSerializable>
            <cc1:XYDiagram>
                <AxisX VisibleInPanesSerializable="-1" MinorCount="1" Visibility="False">
                    <Label Alignment="Center">
                        <ResolveOverlappingOptions AllowHide="False" />
                    </Label>
                    <WholeRange AutoSideMargins="False" EndSideMargin="0.5" StartSideMargin="0.5" />
                    <GridLines MinorVisible="True">
                    </GridLines>
                    <NumericScaleOptions AutoGrid="False" />
                </AxisX>
                <AxisY VisibleInPanesSerializable="-1" MinorCount="1">
                    <Tickmarks MinorVisible="False" />
                    <Label TextPattern="{V:0.000}" Font="Tahoma, 7pt">
                        <ResolveOverlappingOptions AllowHide="True" />
                    </Label>
                    <VisualRange Auto="False" AutoSideMargins="False" EndSideMargin="0.015" MaxValueSerializable="2.715" MinValueSerializable="2.7" StartSideMargin="0.025" />
                    <WholeRange AlwaysShowZeroLevel="False" Auto="False" AutoSideMargins="False" EndSideMargin="0.005" MaxValueSerializable="2.73" MinValueSerializable="2.7" StartSideMargin="0.005" />
                    <GridLines>
                        <LineStyle DashStyle="Dot" />
                    </GridLines>
                    <NumericScaleOptions AutoGrid="False" CustomGridAlignment="0.005" GridAlignment="Custom" />
                </AxisY>
                <SecondaryAxesY>
                    <cc1:SecondaryAxisY AxisID="0" Name="Secondary AxisY 1" Visibility="True" VisibleInPanesSerializable="-1">
                        <WholeRange AutoSideMargins="False" EndSideMargin="0" StartSideMargin="0" />
                    </cc1:SecondaryAxisY>
                </SecondaryAxesY>
            </cc1:XYDiagram>
        </DiagramSerializable>
        <titles>
            <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="Graph Monitoring" Alignment="Center" />
        </titles>
        <legend alignmenthorizontal="Left" alignmentvertical="BottomOutside" 
            direction="LeftToRight"></legend> 
    </dx:WebChartControl>
</div>

<div id="chartRdiv" style="width:100%; border:1px solid black">
<dx:WebChartControl ID="chartR" runat="server" ClientInstanceName="chartR"
        Height="230px" Width="1080px" CrosshairEnabled="True">
        <SeriesSerializable>
            <cc1:Series ArgumentDataMember="Seq" Name="R" ValueDataMembersSerializable="RValue">
                <ViewSerializable>
                    <cc1:LineSeriesView>
                        <LineStyle Thickness="1" />
                        <LineMarkerOptions Size="3" Color="Blue" Kind="Diamond">
                    </LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="RuleYellow" ShowInLegend="False" ValueDataMembersSerializable="RuleYellow">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Yellow" Transparency="130">
                        <FillStyle FillMode="Solid">
                        </FillStyle>
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="RuleRed" ShowInLegend="False" ValueDataMembersSerializable="RuleRed">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Pink" Transparency="100">
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
        </SeriesSerializable>
        <seriestemplate ValueDataMembersSerializable="Value">            
            <viewserializable>
                <cc1:LineSeriesView>
                    <LineMarkerOptions BorderColor="White" Size="4">
                    </LineMarkerOptions>
                </cc1:LineSeriesView>
            </viewserializable>
        </seriestemplate>  
        <DiagramSerializable>
            <cc1:XYDiagram>
                <AxisX VisibleInPanesSerializable="-1" MinorCount="1" Visibility="False">
                    <WholeRange AutoSideMargins="False" EndSideMargin="0.5" StartSideMargin="0.5" />
                    <GridLines MinorVisible="True">
                    </GridLines>
                </AxisX>
                <AxisY VisibleInPanesSerializable="-1" MinorCount="1">
                    <Tickmarks MinorLength="1" MinorVisible="False" />
                    <Label TextAlignment="Near" TextPattern="{V:0.000}">
                        <ResolveOverlappingOptions AllowHide="True" />
                    </Label>
                    <VisualRange Auto="False" AutoSideMargins="False" EndSideMargin="0.001" MaxValueSerializable="0.027" MinValueSerializable="0" StartSideMargin="0" />
                    <WholeRange Auto="False" MaxValueSerializable="0.027" MinValueSerializable="0" AutoSideMargins="False" EndSideMargin="1" StartSideMargin="1" />
                    <GridLines>
                        <LineStyle DashStyle="Dot" />
                    </GridLines>
                    <NumericScaleOptions AutoGrid="False" CustomGridAlignment="0.001" GridAlignment="Custom" GridOffset="1" />
                </AxisY>
            </cc1:XYDiagram>
        </DiagramSerializable>
        <titles>
            <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="R Control Chart" />
        </titles>
        <legend alignmenthorizontal="Left" alignmentvertical="BottomOutside" 
            direction="LeftToRight"></legend> 
        <ClientSideEvents EndCallback="ChartREndCallBack" Init="OnInit" />
    </dx:WebChartControl>
</div>

<div style="width:100%; border:1px solid black;">
    <table style="width:100%; padding-top:10px">
        <tr>
            <td style="width:400px">
                <dx:WebChartControl ID="Histogram" runat="server" CrosshairEnabled="True" Height="320px" Width="390px" ClientInstanceName="Histogram" CssClass="editableform" >

                        <Titles>
                            <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="Histogram" Alignment="Center" />
                        </Titles>
                        <BorderOptions Visibility="True" />
                        <DiagramSerializable>
                <cc1:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1" Visibility="True">
                    <Tickmarks MinorVisible="False" />
                    <Label TextPattern="{A:0.000}">
                        <ResolveOverlappingOptions AllowHide="False" />
                    </Label>
                    <WholeRange AutoSideMargins="False" EndSideMargin="3" StartSideMargin="3" />
                    <NumericScaleOptions AutoGrid="False" ScaleMode="Interval" AggregateFunction="Histogram" GridAlignment="Custom" GridSpacing="0.001" IntervalOptions-DivisionMode="Count" IntervalOptions-GridLayoutMode="GridShiftedLabelCentered" IntervalOptions-Count="8" IntervalOptions-Pattern="{A1:0.000} - {A2:0.000}" IntervalOptions-OverflowValuePattern="{OS}" IntervalOptions-UnderflowValuePattern="{US}" />
                    </AxisX>

                <AxisY VisibleInPanesSerializable="-1" MinorCount="1">
                    <Tickmarks MinorLength="1" MinorVisible="False" />
                    <Label>
                        <ResolveOverlappingOptions AllowRotate="False" AllowStagger="False" />
                    </Label>
                    <WholeRange AutoSideMargins="False" EndSideMargin="2" StartSideMargin="2" />
                    <GridLines Visible="False">
                    </GridLines>
                    <NumericScaleOptions AutoGrid="False" MinGridSpacingLength="1" />
                    </AxisY>
                </cc1:XYDiagram>
                </DiagramSerializable>

                        <Legend Visibility="False"></Legend>

                        <SeriesSerializable>
                            <cc1:Series ArgumentDataMember="Value" Name="Histogram" ShowInLegend="False">
                                <ViewSerializable>
                                    <cc1:SideBySideBarSeriesView BarWidth="1.07" Color="84, 141, 212">
                                        <Border Color="63, 63, 63" Visibility="True" Thickness="1"/>
                                        <FillStyle FillMode="Solid">
                                        </FillStyle>
                                    </cc1:SideBySideBarSeriesView>
                                </ViewSerializable>
                                <LabelSerializable>
                                    <cc1:SideBySideBarSeriesLabel Position="Top">
                                    </cc1:SideBySideBarSeriesLabel>
                                </LabelSerializable>
                            </cc1:Series>
                        </SeriesSerializable>
                        <PaletteWrappers>
                            <dx:PaletteWrapper Name="SPC" ScaleMode="Extrapolate">
                                <Palette>
                                    <cc1:PaletteEntry Color="105, 228, 180" Color2="105, 228, 180" />
                                    <cc1:PaletteEntry Color="155, 248, 139" Color2="155, 248, 139" />
                                    <cc1:PaletteEntry Color="159, 222, 255" Color2="159, 222, 255" />
                                    <cc1:PaletteEntry Color="197, 166, 255" Color2="197, 166, 255" />
                                    <cc1:PaletteEntry Color="255, 196, 243" Color2="255, 196, 243" />
                                    <cc1:PaletteEntry Color="255, 157, 140" Color2="255, 157, 140" />
                                </Palette>
                            </dx:PaletteWrapper>
                        </PaletteWrappers>
                    </dx:WebChartControl>
            </td>
            <td style="vertical-align:top; text-align:left; padding-top:10px; align-items: baseline; " class="auto-style1">
                <table style="padding-left:5px">
                    <tr>
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Factory" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style3" style="padding-left:5px" colspan="3">
                            <dx:ASPxLabel ID="lblFactory" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblFactory"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Type" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style3" style="padding-left:5px" colspan="3">
                            <dx:ASPxLabel ID="lblType" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblType"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Machine Process" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style3" style="padding-left:5px" colspan="3">
                            <dx:ASPxLabel ID="lblLine" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLine"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Item Check" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style3" style="padding-left:5px" colspan="3">
                            <dx:ASPxLabel ID="lblItemCheck" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblItemCheck"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Prod Date" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style3" style="padding-left:5px" colspan="3">
                            <dx:ASPxLabel ID="lblProdDate" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblProdDate"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Unit Measurement" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style3" style="padding-left:5px" colspan="3">
                            <dx:ASPxLabel ID="lblUnit" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblUnit"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="auto-style21">
                        </td>
                    </tr>

                    <tr >
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Specification USL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblSpecUSL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblUSL"></dx:ASPxLabel>
                        </td>                        
                    </tr>
                    <tr >
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Specification LSL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblSpecLSL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLSL"></dx:ASPxLabel>
                        </td>      
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="d<sub>2</sub>" Font-Names="Segoe UI" Font-Size="9pt" EncodeHtml="False"></dx:ASPxLabel>                            
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblD2" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblD2"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr >
                        <td class="auto-style7" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Control Plan UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style10" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblUCL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblUCL"></dx:ASPxLabel>
                        </td>                   

                        <td class="auto-style7" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="X Bar UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style10" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblXUCL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblXUCL"></dx:ASPxLabel>
                        </td>                         
                    </tr>
                    <tr >
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Control Plan CL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblCL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCL"></dx:ASPxLabel>
                        </td>           
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="X Bar LCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblXLCL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblXLCL"></dx:ASPxLabel>
                        </td>                        
                    </tr>
                    <tr >
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Control Plan LCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblLCL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLCL"></dx:ASPxLabel>
                        </td>  
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel39" runat="server" Text="R UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblRUCL" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblRUCL"></dx:ASPxLabel>
                        </td>                                                 
                    </tr>

                    <tr>
                        <td colspan="4" style="height:2px">
                        </td>
                    </tr>


                    <tr >
                        <td class="auto-style8" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Min" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style11" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblMin" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMin"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style2" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Cp" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td  class="auto-style12" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblCP" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCP"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style6" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Max" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td class="auto-style9" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblMax" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMax"></dx:ASPxLabel>
                        </td>
                        <td class="vheader" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Cpu" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td class="auto-style13" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblCPK1" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCPK1"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style7" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="X Bar Bar" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-left:5px" id="tdXBarBar" class="auto-style10">
                            <dx:ASPxLabel ID="lblXBarBar" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblXBarBar"></dx:ASPxLabel>
                        </td>
                        <td class="auto-style4" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Cpl" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td class="auto-style14" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblCPK2" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCPK2"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style6" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="R Bar" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-left:5px" id="tdRBar" class="auto-style9">
                            <dx:ASPxLabel ID="lblRBar" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblRBar"></dx:ASPxLabel>
                        </td>
                        <td class="vheader" style="padding-left:5px">
                            <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Cpk" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td class="auto-style13" style="padding-left:5px">
                            <dx:ASPxLabel ID="lblCPKMin" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCPKMin"></dx:ASPxLabel>
                        </td>
                    </tr>
                </table>
            </td>
            <td>

            </td>
        </tr>
    </table>
    
</div>
</asp:Content>
