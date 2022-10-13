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
        .body {
            border: 1px solid silver; 
        }
        
            </style>
    <script type="text/javascript" >
        var rowIndex, columnIndex;
        function OnInit(s, e) {
            var d = new Date(2022, 8, 3);
            dtDate.SetDate(d);  
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
            cboType.SetEnabled(false);
            cboType.PerformCallback(cboFactory.GetValue());
        }

        function cboTypeChanged(s, e) {
            cboLine.SetEnabled(false);   
            cboLine.PerformCallback(cboFactory.GetValue()  + '|' + cboType.GetValue());
        }

        function cboLineChanged(s, e) {    
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

        function OpenWindow(s, e) {
            var selectedDate = dtDate.GetText();
            window.open('ProdSampleInput.aspx?FactoryCode=' + cboFactory.GetValue() + '&ItemTypeCode=' + cboType.GetValue() + '&Line=' + cboLine.GetValue() + '&ItemCheckCode=' + cboItemCheck.GetValue() + '&Shift=' + cboShift.GetValue() + '&ProdDate=' + dtDate.GetText() + '&Sequence=' + cboSeq.GetValue(), "NewWindow");
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
                    ss.cp_val = 0;
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
            lblMin.SetText(s.cpMin);
            lblMax.SetText(s.cpMax);
            lblAvg.SetText(s.cpAvg);
            lblSTD.SetText(s.cpSTD);
            lblCP.SetText(s.cpCP);
            lblCPK1.SetText(s.cpCPK1);
            lblCPK2.SetText(s.cpCPK2);

            chartX.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + dtTo.GetText() + '|' + cboShow.GetValue());
            Histogram.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + dtTo.GetText());
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 0px 5px 5px 5px">
        <table style="width: 100%">
        <tr >
            <td style="padding:5px 0px 0px 0px" >
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Factory" Font-Names="Segoe UI" 
                    Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" padding:5px 0px 0px 0px" >
                <dx:ASPxComboBox ID="cboFactory" runat="server" Theme="Office2010Black" TextField="FactoryName"
                    ClientInstanceName="cboFactory" ValueField="FactoryCode" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" 
                    Width="100px" TabIndex="6" EnableCallbackMode="True">
                    <ClientSideEvents SelectedIndexChanged="cboFactoryChanged" />

                    <ItemStyle Height="10px" Paddings-Padding="4px" >
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
            </td>
            <td style=" padding:5px 0px 0px 0px" >
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Machine Process" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding:5px 0px 0px 0px" >
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
            <td style=" padding: 5px 0px 0px 10px; " >
                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Date" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td>
                &nbsp;
                </td>
            <td style="padding: 5px 0px 0px 0px; ">                
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
           
           
           
            <td style="padding: 5px 0px 0px 0px; ">  
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="To" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel></td>            
           
            <td style="padding: 5px 0px 0px 0px; ">  
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
            <td style="padding: 5px 0px 0px 0px">

                &nbsp;</td>
            <td></td>
        </tr>

        <tr>
            <td style=" width:60px; padding:3px 0px 0px 0px">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Type" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" width:130px; padding:3px 0px 0px 0px">
                
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
            <td style=" padding:3px 0px 0px 0px">
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Item Check" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="width:200px; padding:3px 0px 0px 0px">
                
                    
                <dx:ASPxComboBox ID="cboItemCheck" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboItemCheck" ValueField="ItemCheckCode" TextField="ItemCheck" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="190px" TabIndex="5" >
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
            <td style=" padding: 3px 0px 0px 10px; ">
                
               
                </td>
            <td style=" width:10px">
                                    
            </td>
            <td style="padding:3px 0px 0px 0px">
                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Show Verified Only" 
                        Font-Names="Segoe UI" Font-Size="9pt" Width="109px">
            </dx:ASPxLabel> 
                                
                
                
            </td>
            <td></td>
            <td>
                                
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
            <td style="width:100px">

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
            <td style="width:100px; padding-left:2px">

                                <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnExcel" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Excel" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="90px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                </dx:ASPxButton>                            

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


                    <Settings HorizontalScrollBarMode="Auto" VerticalScrollableHeight="260" VerticalScrollBarMode="Auto" />
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
    <div style="height:10px"></div>

<div style="width:100%; overflow-x: auto; border:1px solid black">
<dx:WebChartControl ID="chartX" runat="server" ClientInstanceName="chartX"
        Height="434px" Width="400px" CrosshairEnabled="True" SeriesDataMember="Description" ToolTipEnabled="False">
        <seriestemplate SeriesDataMember="Description" ArgumentDataMember="Seq" ValueDataMembersSerializable="Value" CrosshairLabelPattern="{S}: {V:0.000}">
            <viewserializable>
                <cc1:PointSeriesView>                    
                    <PointMarkerOptions kind="Circle" BorderColor="255, 255, 255"></PointMarkerOptions>
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
                        <LineMarkerOptions Color="Blue" Size="3">
                        </LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
        </SeriesSerializable>     
        <DiagramSerializable>
            <cc1:XYDiagram>
                <AxisX VisibleInPanesSerializable="-1" MinorCount="1">
                    <Label Alignment="Center">
                        <ResolveOverlappingOptions AllowHide="False" />
                    </Label>
                    <WholeRange AutoSideMargins="False" EndSideMargin="-0.5" StartSideMargin="-0.5" />
                    <GridLines MinorVisible="True">
                    </GridLines>
                    <NumericScaleOptions AutoGrid="False" />
                </AxisX>
                <AxisY VisibleInPanesSerializable="-1" MinorCount="1">
                    <Tickmarks MinorVisible="False" />
                    <Label TextPattern="{V:0.000}" Font="Tahoma, 7pt">
                        <ResolveOverlappingOptions AllowHide="True" />
                    </Label>
                    <VisualRange Auto="False" AutoSideMargins="False" EndSideMargin="0.015" MaxValueSerializable="2.715" MinValueSerializable="2.645" StartSideMargin="0.025" />
                    <WholeRange AlwaysShowZeroLevel="False" Auto="False" AutoSideMargins="False" EndSideMargin="0.015" MaxValueSerializable="2.73" MinValueSerializable="2.62" StartSideMargin="0.025" />
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
            <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="Graph Monitoring" Alignment="Near" />
        </titles>
        <legend alignmenthorizontal="Left" alignmentvertical="BottomOutside" 
            direction="LeftToRight"></legend> 
    </dx:WebChartControl>
</div>

<div style="width:100%; overflow-x: auto; border:1px solid black">
    <table style="width:100%">
        <tr>
            <td style="width:70%">
<dx:WebChartControl ID="Histogram" runat="server" CrosshairEnabled="True" Height="350px" Width="800px" ClientInstanceName="Histogram">

                        <Titles>
                            <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="Histogram" Alignment="Near" />
                        </Titles>
                        <DiagramSerializable>
                <cc1:XYDiagram Rotated="True">
                <AxisX VisibleInPanesSerializable="-1" Visibility="True">
                    <Tickmarks MinorVisible="False" />
                    <Label TextPattern="{A:0.000}">
                    </Label>
                    <WholeRange AutoSideMargins="False" EndSideMargin="0.01" StartSideMargin="0.5" />
                    <GridLines Visible="True">
                    </GridLines>
                    <NumericScaleOptions AutoGrid="False" ScaleMode="Interval" AggregateFunction="Histogram" GridAlignment="Custom" GridSpacing="0.001" IntervalOptions-DivisionMode="Count" IntervalOptions-GridLayoutMode="GridAndLabelShifted" IntervalOptions-OverflowValue="2.715" IntervalOptions-Count="6" IntervalOptions-Pattern="{A1:0.000}-{A2:0.000}" IntervalOptions-UnderflowValue="2.645" />
                    </AxisX>

                <AxisY VisibleInPanesSerializable="-1" MinorCount="1" Visibility="True">
                    <Tickmarks MinorLength="1" MinorVisible="False" />
                    <WholeRange AutoSideMargins="False" EndSideMargin="1" StartSideMargin="0" />
                    <NumericScaleOptions AutoGrid="False" MinGridSpacingLength="1" />
                    </AxisY>
                </cc1:XYDiagram>
                </DiagramSerializable>

                        <Legend Visibility="False"></Legend>

                        <SeriesSerializable>
                            <cc1:Series ArgumentDataMember="Value" Name="Histogram" ShowInLegend="False">
                                <ViewSerializable>
                                    <cc1:SideBySideBarSeriesView BarWidth="1" ColorEach="True">
                                        <Border Color="0, 0, 0" Visibility="True" />
                                    </cc1:SideBySideBarSeriesView>
                                </ViewSerializable>
                            </cc1:Series>
                        </SeriesSerializable>
                    </dx:WebChartControl>
            </td>
            <td style="vertical-align:top; text-align:left; width:30%; align-items: baseline; padding-right:5px">
                <table style="width:100%">
                    <tr >
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Min" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-right:2px">
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text=":" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblMin" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMin"></dx:ASPxLabel>
                        </td>
                        <td style="width:20px">

                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="CP" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text=":" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblCP" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCP"></dx:ASPxLabel>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Max" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-right:2px">
                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text=":" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblMax" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMax"></dx:ASPxLabel>
                        </td>
                        <td>

                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Cpk1" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-right:2px">
                            <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text=":" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblCPK1" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCPK1"></dx:ASPxLabel>
                        </td>
                        <td>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Average" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-right:2px">
                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text=":" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblAvg" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblAvg"></dx:ASPxLabel>
                        </td>
                        <td>

                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Cpk2" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-right:2px">
                            <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text=":" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblCPK2" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCPK2"></dx:ASPxLabel>
                        </td>
                        <td>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Deviation" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td style="padding-right:2px">
                            <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text=":" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblSTD" runat="server" Text="" Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblSTD"></dx:ASPxLabel>
                        </td>
                        <td>

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
</div>
</asp:Content>
