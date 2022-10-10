<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ProdSampleInput.aspx.vb" Inherits="PECGI_SPC.ProdSampleInput" %>

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

        function ValidateSave(s, e) {
            grid.PerformCallback('save' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShow.GetValue() + '|' + txtSubLotNo.GetText() + '|' + txtRemarks.GetText() );           
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
            cboShift.SetEnabled(false);
            cboShift.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue());            
        }

        function cboItemCheckChanged(s, e) {
            cboShift.SetEnabled(false);
            cboShift.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue());                        
        }

        function cboShiftChanged(s, e) {    
            cboSeq.SetEnabled(false);        
            cboSeq.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + cboShift.GetValue());
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

        function GridLoad(s, e) {
            grid.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShow.GetValue());
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

            lblMKUser.SetText(s.cpMKUser);
            lblMKDate.SetText(s.cpMKDate);
            lblQCUser.SetText(s.cpQCUser);
            lblQCDate.SetText(s.cpQCDate);

            lblUSL.SetText(s.cpUSL);
            lblLSL.SetText(s.cpLSL);
            lblUCL.SetText(s.cpUCL);
            lblLCL.SetText(s.cpLCL);
            lblMin.SetText(s.cpMin);
            lblMax.SetText(s.cpMax);
            lblAve.SetText(s.cpAve);
            lblR.SetText(s.cpR);
            lblC.SetText(s.cpC);            
            txtRemarks.SetText(s.cpRemarks);
            txtSubLotNo.SetText(s.cpSubLotNo);
            
            if (s.cpNG == '2') {
                lblNG.SetText('NG');
                document.getElementById('NG').style.backgroundColor = 'Red';
            } else if (s.cpNG == '1') {
                lblNG.SetText('OK');
                document.getElementById('NG').style.backgroundColor = 'Green';
            } else if (s.cpNG == '0') {
                lblNG.SetText('OK');
                document.getElementById('NG').style.backgroundColor = 'Green';
            } else {
                lblNG.SetText('');
                document.getElementById('NG').style.backgroundColor = 'White';
            }
            if (s.cpC == 'C') {
                document.getElementById('C').style.backgroundColor = 'Orange';
            } else {
                document.getElementById('C').style.backgroundColor = 'White';
            }      

            if(s.cpMin != '') {
                if (s.cpMin < s.cpLSL) {
                    document.getElementById('Min').style.backgroundColor = 'Red';
                } else if (s.cpMin < s.cpLCL) {
                    document.getElementById('Min').style.backgroundColor = 'Pink';
                } else {
                    document.getElementById('Min').style.backgroundColor = 'White';
                }
            } else {
                document.getElementById('Min').style.backgroundColor = 'White';
            }
            if (s.cpMax > s.cpUSL) {
                document.getElementById('Max').style.backgroundColor = 'Red';
            } else if (s.cpMax > s.cpUCL) {
                document.getElementById('Max').style.backgroundColor = 'Pink';
            } else {
                document.getElementById('Max').style.backgroundColor = 'White';
            }
            if (s.cpRefresh == '1') {
                gridX.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShow.GetValue());
                chartX.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShow.GetValue());
                chartR.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText());                
            }            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 0px 5px 5px 5px">
        <table class="auto-style3" style="width: 100%">
        <tr >
            <td style="width:60px; padding:5px 0px 0px 0px">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Factory" Font-Names="Segoe UI" 
                    Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" width:130px; padding:5px 0px 0px 0px">
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
            <td style=" width:100px; padding:5px 0px 0px 0px">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Machine Process" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="width:130px; padding:5px 0px 0px 0px">
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
            <td style=" padding: 5px 0px 0px 10px; width:50px">
                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Date" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" width:10px">
                &nbsp;
                </td>
            <td style="padding: 5px 0px 0px 0px; width: 180px;" colspan="3">                
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
           
                <td style=" width:110px">                
                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Show Verified Only" 
                        Font-Names="Segoe UI" Font-Size="9pt" Width="109px">
                    </dx:ASPxLabel>                
                </td>            
            <td colspan="2" style="width: 120px">                
                
                <dx:ASPxComboBox ID="cboShow" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboShow" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="58px" TabIndex="9" SelectedIndex="0">
                    <Items>                        
                        <dx:ListEditItem Text="No" Value="0" Selected="true"/>
                        <dx:ListEditItem Text="Yes" Value="1" />
                    </Items>
                    <ItemStyle Height="10px" Paddings-Padding="4px">
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
                
            </td>                        
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
                       }"
                        SelectedIndexChanged="cboItemCheckChanged"/>

                    <ItemStyle Height="10px" Paddings-Padding="4px">
<Paddings Padding="4px"></Paddings>
                    </ItemStyle>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
                
                    
            </td>
            <td style=" padding: 3px 0px 0px 10px; ">
                <dx:ASPxLabel ID="lblqeleader0" runat="server" Text="Shift" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style=" width:10px">
                &nbsp;</td>
            <td style="width:100px; padding:3px 0px 0px 0px">
                
                <dx:ASPxComboBox ID="cboShift" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboShift" ValueField="ShiftCode" TextField="ShiftName" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="100px" TabIndex="3">
                    <ClientSideEvents SelectedIndexChanged="cboShiftChanged" 
                        EndCallback="function(s, e) {cboShift.SetEnabled(true);}"
                        />
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>                
            </td>
            <td style="width:30px; padding:3px 0px 0px 5px">
                <dx:ASPxLabel ID="lblqeleader2" runat="server" Text="Seq" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>            
            <td style="width:60px; padding:3px 0px 0px 0px">
                
                <dx:ASPxComboBox ID="cboSeq" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboSeq" ValueField="SequenceNo" TextField="SequenceNo" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="60px" TabIndex="3">
                    <ClientSideEvents EndCallback="function(s, e) {cboSeq.SetEnabled(true);}"/>

                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>
                
            </td>            
            <td>
                &nbsp;</td>            
            <td align="left" style="width: 60px">
                
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
	                    } else if(cboShift.GetText() == '') {
                            cboShift.Focus();
                            errmsg = 'Please select Shift!';
	                    } else if(cboSeq.GetText() == '') {
                            cboSeq.Focus();
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
                        grid.CancelEdit();
 	                    grid.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShow.GetValue());
                    }" />
                    <Paddings Padding="2px" />
                </dx:ASPxButton>
                
            </td>   
            <td align="left">
                &nbsp;</td>   
                     
        </tr>
    </table>
    </div>
<div style="height:10px">
    <dx:ASPxHiddenField ID="hfRevNo" runat="server" ClientInstanceName="hfRevNo">
    </dx:ASPxHiddenField>
</div>
<hr style="border-color:darkgray; " class="auto-style1"/>
<div>
    <table style="width: 100%;">
        <tr>
            <td>
                <table>
                    <tr>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnSave" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Save" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="90px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                    <ClientSideEvents Click="ValidateSave"
                                    />
                                </dx:ASPxButton>
                        </td>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnRead" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnRead" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Read from Device" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="90px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                </dx:ASPxButton>                             
                        </td>
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
        
            </td>
        </tr>
    </table>
    
</div>
<div style="padding: 10px 5px 5px 5px">
<dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
            EnableTheming="True" KeyFieldName="SeqNo" Theme="Office2010Black"            
            OnBatchUpdate="grid_BatchUpdate"          
            Width="100%" 
            Font-Names="Segoe UI" Font-Size="9pt">
            <ClientSideEvents 
                EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing" 
             />
            <SettingsResizing ColumnResizeMode="Control" />
            <SettingsDataSecurity AllowDelete="False" />

<SettingsPopup>
    <EditForm Modal="false" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200" />
    <FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>
        <Columns>

            <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0" Width="50px" ShowNewButtonInHeader="True">
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn Caption="Data#" VisibleIndex="1" FieldName="SeqNo" Width="50px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Value" VisibleIndex="2" FieldName="Value" Width="80px">
                <PropertiesTextEdit SelectInputTextOnClick="True" DisplayFormatString="0.000" Width="70px">
                    <MaskSettings UseInvariantCultureDecimalSymbolOnClient="True" Mask="&lt;0..9999g&gt;.&lt;000..999&gt;" />
                    <ValidationSettings>
                        <RegularExpression ErrorText="Please input valid value" />
                    </ValidationSettings>
                    <Style HorizontalAlign="Right">
                    </Style>
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Judgement" VisibleIndex="3" Width="80px" FieldName="Judgement">
                <EditFormSettings Visible="False" />
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Operator" VisibleIndex="4" FieldName="RegisterUser">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Sample Time" VisibleIndex="5" Width="70px" FieldName="RegisterDate">
                <PropertiesTextEdit DisplayFormatString="HH:mm">
                </PropertiesTextEdit>
                <EditFormSettings Visible="False" />
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Remarks" VisibleIndex="8" FieldName="Remark">
                <PropertiesTextEdit Width="120px">
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last User" VisibleIndex="9" FieldName="RegisterUser">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last Update" VisibleIndex="10" FieldName="RegisterDate" Width="160px">
                <PropertiesTextEdit DisplayFormatString="d MMM yyyy HH:mm:ss">
                </PropertiesTextEdit>
                <EditFormSettings Visible="False" />
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataCheckColumn Caption="Delete Status" FieldName="DeleteStatus" VisibleIndex="6" Width="70px" Visible="False">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn Caption="Delete Status" FieldName="DelStatus" VisibleIndex="7" Width="70px">
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>

        </Columns>        
        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" AllowDragDrop="False" AllowSort="False" />
        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" >
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

<div style="height:10px">


</div>    
<div>
<table style="width:100%">
    <tr>
        <td>
            <table style="width:100%">
                <tr style="height:26px">
                    <td style="width:100px">
                        <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Sub Lot No" Font-Names="Segoe UI" Font-Size="9pt" Width="80px"></dx:ASPxLabel>
                    </td>
                    <td>

                        <dx:ASPxTextBox ID="txtSubLotNo" runat="server" Width="160px" ClientInstanceName="txtSubLotNo">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr style="height:26px">
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Remarks" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxTextBox ID="txtRemarks" runat="server" Width="160px" ClientInstanceName="txtRemarks">
                        </dx:ASPxTextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <table style="width: 240px; height: 50px">
                    <tr>
                        <td style="width: 80px;" align="center" class="header">
                            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Verification" 
                                Font-Names="Segoe UI" Font-Size="9pt">
                            </dx:ASPxLabel>
                        </td>
                        <td style="width: 80px;" align="center" class="header">
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="PIC" 
                                Font-Names="Segoe UI" Font-Size="9pt">
                            </dx:ASPxLabel>
                        </td>
                       <td style="width: 80px;" align="center" class="header">
                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Date" 
                                Font-Names="Segoe UI" Font-Size="9pt">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="border: 1px solid silver;" align="center">
                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="MK" 
                                Font-Names="Segoe UI" Font-Size="9pt">
                            </dx:ASPxLabel>
                        </td>
                        <td style="border: 1px solid silver; width: 100px" align="center">                
                            <dx:ASPxLabel ID="lblMKUser" runat="server" Text="" 
                                Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMKUser">
                            </dx:ASPxLabel>
                        </td>
                        <td style="border: 1px solid silver; width: 100px" align="center">                
                            <dx:ASPxLabel ID="lblMKDate" runat="server" Text="" 
                                Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMKDate">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="border: 1px solid silver;" align="center">                
                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="QC" 
                                Font-Names="Segoe UI" Font-Size="9pt">
                            </dx:ASPxLabel>
                        </td>
                        <td style="border: 1px solid silver; width: 100px" align="center">
                            <dx:ASPxLabel ID="lblQCUser" runat="server" Text="" 
                                Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblQCUser">
                            </dx:ASPxLabel>
                        </td>
                        <td style="border: 1px solid silver; width: 100px" align="center">
                            <dx:ASPxLabel ID="lblQCDate" runat="server" Text="" 
                                Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblQCDate">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                </table>
        </td>
        <td style="width:500px">
            <table style="width:100%">
                <tr>
                    <td colspan="2" class="header"><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Specification" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td colspan="2" class="header"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="X Bar Control" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td colspan="6" class="header"><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Result" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                </tr>
                <tr>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="USL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="LSL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="LCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Min" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Max" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Ave" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="R" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="body" align="center" rowspan="2" style="width:50px" id="C"><dx:ASPxLabel ID="lblC" runat="server" Text="" Font-Names="Segoe UI" Font-Size="Medium" Font-Bold="True" ForeColor="Black" ClientInstanceName="lblC"></dx:ASPxLabel></td>
                    <td class="body" align="center" rowspan="2" style="width:50px" id="NG"><dx:ASPxLabel ID="lblNG" runat="server" Text="" Font-Names="Segoe UI" Font-Size="Medium" ClientInstanceName="lblNG" Font-Bold="True" ForeColor="Black"></dx:ASPxLabel></td>
                </tr>
                <tr>
                    <td class="body" align="center"><dx:ASPxLabel ID="lblUSL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblUSL" ForeColor="Black"></dx:ASPxLabel>&nbsp;</td>
                    <td class="body" align="center"><dx:ASPxLabel ID="lblLSL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLSL" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center"><dx:ASPxLabel ID="lblUCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblUCL" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center"><dx:ASPxLabel ID="lblLCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLCL" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center" id="Min"><dx:ASPxLabel ID="lblMin" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMin" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center" id="Max"><dx:ASPxLabel ID="lblMax" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMax" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center" id="Ave"><dx:ASPxLabel ID="lblAve" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblAve" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center" id="R"><dx:ASPxLabel ID="lblR" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblR" ForeColor="Black"></dx:ASPxLabel></td>                    
                </tr>
            </table>
        </td>
    </tr>
</table>    
    <div style="height:10px; vertical-align:middle">

    </div>

<div>
    
    <table style="width:100%;">
        <tr>
            <td>
                <dx:ASPxGridView ID="gridX" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridX"
                                EnableTheming="True" KeyFieldName="Des" Theme="Office2010Black"            
                                Width="100%" 
                                Font-Names="Segoe UI" Font-Size="9pt">


                    <SettingsPager Mode="ShowAllRecords">
                    </SettingsPager>


                    <Settings HorizontalScrollBarMode="Auto" VerticalScrollableHeight="380" VerticalScrollBarMode="Auto" />
                    <SettingsBehavior AllowSort="False" />
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
            </td>
        </tr>
        <tr>
            <td>
                <div style="height:10px"></div>
                
<div id="chartXdiv" style="overflow-x:auto; width:100%; border:1px solid black"">
<dx:WebChartControl ID="chartX" runat="server" ClientInstanceName="chartX"
        Height="490px" Width="1080px" CrosshairEnabled="True" SeriesDataMember="Description" ToolTipEnabled="False">
        <seriestemplate SeriesDataMember="Description" ArgumentDataMember="Seq" ValueDataMembersSerializable="Value" ToolTipPointPattern="{V:0.000}">
            <viewserializable>
                <cc1:PointSeriesView>                    
                    <PointMarkerOptions kind="Circle" BorderColor="255, 255, 255"></PointMarkerOptions>
                </cc1:PointSeriesView>
            </viewserializable>
        </seriestemplate>    
        <SeriesSerializable>
            <cc1:Series ArgumentDataMember="Seq" Name="Rule" ValueDataMembersSerializable="RuleValue" LabelsVisibility="False" ShowInLegend="False" ToolTipEnabled="False" ToolTipSeriesPattern="">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Red" Transparency="200" AxisYName="Secondary AxisY 1">
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="RuleYellow" ValueDataMembersSerializable="RuleYellow" LabelsVisibility="False" ShowInLegend="False" ToolTipEnabled="False" ToolTipSeriesPattern="">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Yellow" Transparency="200">
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="Average" ValueDataMembersSerializable="AvgValue">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="Blue">
                        <LineStyle Thickness="1" />
                        <LineMarkerOptions Color="Blue" Size="1"></LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="LCL" ValueDataMembersSerializable="LCL" LabelsVisibility="False">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="0, 32, 96" MarkerVisibility="False">
                        <LineStyle DashStyle="DashDot" Thickness="2" />
                        <LineMarkerOptions Size="1">
                        </LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
                <LabelSerializable>
                    <cc1:PointSeriesLabel Position="Center">
                    </cc1:PointSeriesLabel>
                </LabelSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="UCL" ValueDataMembersSerializable="UCL">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="0, 32, 96" MarkerVisibility="False">
                        <LineStyle DashStyle="DashDot" Thickness="2" />
                        <LineMarkerOptions Size="1">
                        </LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="USL" ValueDataMembersSerializable="USL">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="240, 0, 0" MarkerVisibility="False">
                        <LineStyle Thickness="2" />
                        <LineMarkerOptions Size="1">
                        </LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="LSL" ValueDataMembersSerializable="LSL">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="240, 0, 0" MarkerVisibility="False">
                        <LineStyle Thickness="2" />
                        <LineMarkerOptions Size="1">
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
                    <cc1:SecondaryAxisY Alignment="Near" AxisID="0" Name="Secondary AxisY 1" Visibility="False" VisibleInPanesSerializable="-1">
                        <WholeRange AutoSideMargins="False" EndSideMargin="0" StartSideMargin="0" />
                    </cc1:SecondaryAxisY>
                </SecondaryAxesY>
            </cc1:XYDiagram>
        </DiagramSerializable>
        <titles>
            <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="Graph Monitoring" />
        </titles>
        <legend alignmenthorizontal="Left" alignmentvertical="BottomOutside" 
            direction="LeftToRight"></legend> 
        <ToolTipOptions ShowForPoints="False">
        </ToolTipOptions>
    </dx:WebChartControl>
</div>
    


            </td>
        </tr>

        <tr>
            <td>
                <div style="height:10px"></div>
                
<div id="chartRdiv">
    <dx:WebChartControl ID="chartR" runat="server" ClientInstanceName="chartR"
        Height="450px" Width="1080px" CrosshairEnabled="True">
        <SeriesSerializable>
            <cc1:Series ArgumentDataMember="Seq" Name="R" ValueDataMembersSerializable="RValue">
                <ViewSerializable>
                    <cc1:LineSeriesView>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
        </SeriesSerializable>
        <seriestemplate ValueDataMembersSerializable="Value">            
            <viewserializable>
                <cc1:LineSeriesView>
                    <LineMarkerOptions BorderColor="White" Size="8">
                    </LineMarkerOptions>
                </cc1:LineSeriesView>
            </viewserializable>
        </seriestemplate>  
        <DiagramSerializable>
            <cc1:XYDiagram>
                <AxisX VisibleInPanesSerializable="-1" MinorCount="1">
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
        <ClientSideEvents EndCallback="ChartREndCallBack" />
    </dx:WebChartControl>
</div>
            </td>
        </tr>

    </table>
</div>
    <div>
        <dx:ASPxCallback ID="cbkRefresh" runat="server" ClientInstanceName="cbkRefresh">
            <ClientSideEvents EndCallback="function(s, e) {
	            lblUSL.SetText(s.cpUSL);
                lblLSL.SetText(s.cpLSL);
                lblUCL.SetText(s.cpUCL);
                lblLCL.SetText(s.cpLCL);
            }" 
            />

        </dx:ASPxCallback>

    </div>
</div>

</asp:Content>
