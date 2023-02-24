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
            border: 1px solid silver;
            width: 45px;
        }
        .auto-style21 {
            width: 75px;
        }
        .auto-style22 {
            width: 165px;
        }
        </style>
    <script type="text/javascript" >
        var prevOnLoad = window.onload;
        window.onload = myOnLoad;
        setInterval(AutoRefresh, 5000);
        function myOnLoad(){
            if(prevOnLoad != null)
                prevOnLoad();
            document.onkeydown = myOnKeyDown;            
        }

        function DisableButton() {
            btnSearch.SetEnabled(false);
        }

        function EnableButton() {
            btnSearch.SetEnabled(true);
        }

        function AutoRefresh() {
            return;
            if(lblAuto.GetText() == 'OFF' | cboFactory.GetText() == '' | cboProcessGroup.GetText() == '' | cboLineGroup.GetText() == '' | cboProcess.GetText() == '' | cboType.GetText() == '' | cboLine.GetText() == '' | cboItemCheck.GetText() == '' | cboShift.GetText() == '' | cboSeq.GetText() == '') {                
                return;
	        }
            DisableButton();
 	        grid.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShow.GetValue());
        }

        function myOnKeyDown(){            
            if(event.keyCode == 112)
                grid.StartEdit();
            if(event.keyCode == 27)
                grid.CancelEdit();
            if(event.keyCode == 13)
                grid.UpdateEdit();
            if ((event.altKey && event.keyCode == 78) || (event.altKey && event.keyCode == 46))  
              AddNewRow(); 
        }

        function ExcelClicked() {
            lblAuto.SetText('OFF');
            DisableButton();
        }

        function OpenMeasurement() {
            var url = "SPCMeasurement:";            
            var userID = hfUserID.Get("UserID");
            var factoryCode = cboFactory.GetValue();
            var processGroup = cboProcessGroup.GetValue();
            var lineGroup = cboLineGroup.GetValue();
            var processCode = cboProcess.GetValue();
            var lineCode = cboLine.GetValue();
            var itemType = cboType.GetValue();
            var itemCheck = cboItemCheck.GetValue();
            var shiftCode = cboShift.GetValue();
            var seqNo = cboSeq.GetValue();
            var prodDate = dtDate.GetText();

            var cmdLine = url + "," + userID  + "," + factoryCode  + "," + processGroup  + "," + lineGroup  + "," + processCode  + "," + lineCode + "," + itemType + "," + itemCheck + "," + shiftCode + "," + seqNo + "," + prodDate;
            window.open(cmdLine);self.focus();
        }

        function AddNewRow(s, e) {
            lblAuto.SetText('OFF');
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
	        } else {
                var date1 = dtDate.GetValue();
                var d = new Date();
                var date2 = new Date(d.getFullYear(), d.getMonth(), d.getDate());   
                
                if(date1 > date2) {
                    errmsg = 'Selected date is newer than current date!'; 
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
                lblAuto.SetText('ON');
		        return;
            }
            grid.AddNewRow(); 
        }


        var rowIndex, columnIndex;
        var prevShift;
        var prevSeq;

        function OnInit(s, e) {
            var x = document.getElementById("chartRdiv");
            var y = document.getElementById("colXBarUCL");
            var z = document.getElementById("colXBarLCL");
            var y2 = document.getElementById("colXBarUCL2");
            var z2 = document.getElementById("colXBarLCL2");
            var w = document.getElementById("headerXBar");
            var v = document.getElementById("colSpecial");
            
            x.style.display = "";
            y.style.display = "";            
            y2.style.display = "";
            z.style.display = "";
            z2.style.display = "";
            w.style.display = "";
            v.style.display = "none";
            btnNew.SetEnabled(false);
            btnRead.SetEnabled(false);
            btnSave.SetEnabled(false);
        }

        function isNumeric(n) {
            if (n == null || n == '') {
                return true;
            } else {
                return !isNaN(parseFloat(n)) && isFinite(n);
            }
        }

        
        function cboShiftEndCallback(s, e) {
            cboShift.SetEnabled(true);            
            cboShift.SetValue(prevShift);    
            cboSeq.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + cboShift.GetValue());            
        }

        function cboSeqEndCallback(s, e) {
            cboSeq.SetEnabled(true);
        }

        function ValidateSave(s, e) {
            lblAuto.SetText('OFF');
            grid.PerformCallback('save' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShow.GetValue() + '|' + txtSubLotNo.GetText() + '|' + txtRemarks.GetText() );           
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

        function cboItemCheckChanged(s, e) {
            cboShift.SetEnabled(false);
            prevShift = cboShift.GetValue();
            cboShift.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue());                        
        }

        function cboShiftChanged(s, e) {    
            cboSeq.SetEnabled(false);        
            cboSeq.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + cboShift.GetValue());
        }

        function OnBatchEditStartEditing(s, e) {
            if(cboSeq.GetValue() == null | cboSeq.GetValue() == '') {
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

        function GridXEndCallBack(s, e) {
            var i = s.cpShow;
            var y = document.getElementById("colXBarUCL");            
            var z = document.getElementById("colXBarLCL");
            var y2 = document.getElementById("colXBarUCL2");
            var z2 = document.getElementById("colXBarLCL2");
            var w = document.getElementById("headerXBar");
            var v = document.getElementById("colSpecial");
            if (i=='1') {
                y.style.display = "";
                y2.style.display = "";
                z.style.display = "";
                z2.style.display = "";
                w.style.display = "";
                v.style.display = "";
            } else {
                y.style.display = "none";
                y2.style.display = "none";
                z.style.display = "none";
                z2.style.display = "none";
                w.style.display = "none";                
                v.style.display = "none";
            }            
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
            if (s.IsEditing()) {  
                var form = s.GetPopupEditForm();  
                form.PopUp.AddHandler(function(s,e) {  
                    var editor = grid.GetEditor('Value');  
                    if (editor.GetValue() == null)
                    {
                        editor.Focus();
                    } else {
                        var editor2 = grid.GetEditor('Remark');  
                        editor2.Focus();
                    }
                    lblAuto.SetText('OFF');
                });  
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
                    lblAuto.SetText('OFF');                    
                    grid.AddNewRow();
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
                    lblAuto.SetText('ON');
                    EnableButton();
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
                    lblAuto.SetText('ON');
                    EnableButton();
                }
            }
            else if (s.cp_message == "" && s.cp_val == 0) {
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;
                lblAuto.SetText('ON');
                EnableButton();
            }            
            lblMKUser.SetText(s.cpMKUser);
            lblMKDate.SetText(s.cpMKDate);
            lblQCUser.SetText(s.cpQCUser);
            lblQCDate.SetText(s.cpQCDate);

            lblUSL.SetText(s.cpUSL);
            lblLSL.SetText(s.cpLSL);
            
            lblUCL.SetText(s.cpUCL);
            lblCL.SetText(s.cpCL);
            lblLCL.SetText(s.cpLCL);
            lblRUCL.SetText(s.cpRUCL);

            lblXBarUCL.SetText(s.cpXBarUCL);
            lblXBarLCL.SetText(s.cpXBarLCL);

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

            var vMin = Number(s.cpMin);
            var vMax = Number(s.cpMax);
            var vLSL = Number(s.cpLSL);
            var vUSL = Number(s.cpUSL);
            var vLCL = Number(s.cpLCL);
            var vUCL = Number(s.cpUCL);
            var vAve = Number(s.cpAve); 

            if(s.cpMin != '') {
                if (vMin < vLSL | vMin > vUSL) {
                    document.getElementById('Min').style.backgroundColor = 'Red';
                } else if (vMin < vLCL | vMin > vUCL) {
                    if (s.cpChartType == '1') {
                        document.getElementById('Min').style.backgroundColor = '#FFFF99';
                    } else {
                        document.getElementById('Min').style.backgroundColor = 'Pink';
                    }                    
                } else {
                    document.getElementById('Min').style.backgroundColor = 'White';
                }
            } else {
                document.getElementById('Min').style.backgroundColor = 'White';
            }
            if(s.cpMax != '') {
                if (vMax > vUSL | vMax < vLSL) {
                    document.getElementById('Max').style.backgroundColor = 'Red';
                } else if (vMax > vUCL | vMax < vLCL) {                    
                    if (s.cpChartType == '1') {
                        document.getElementById('Max').style.backgroundColor = '#FFFF99';
                    } else {
                        document.getElementById('Max').style.backgroundColor = 'Pink';
                    }                      
                } else {
                    document.getElementById('Max').style.backgroundColor = 'White';
                }
            } else {
                document.getElementById('Max').style.backgroundColor = 'White';
            }

            if(vAve != '') {
                if (vAve > vUSL | vAve < vLSL) {
                    document.getElementById('Ave').style.backgroundColor = 'Red';
                } else if (vAve > vUCL | vAve < vLCL) {
                    if (s.cpChartType == '1') {
                        document.getElementById('Ave').style.backgroundColor = '#FFFF99';
                    } else {
                        document.getElementById('Ave').style.backgroundColor = 'Pink';
                    }                         
                } else {
                    document.getElementById('Ave').style.backgroundColor = 'White';
                }
            } else {
                document.getElementById('Ave').style.backgroundColor = 'White';
            }

            if(s.cpRNG == '1') {
                document.getElementById('R').style.backgroundColor = 'Yellow';
            } else if(s.cpRNG == '2') {
                document.getElementById('R').style.backgroundColor = 'Pink';
            } else {
                document.getElementById('R').style.backgroundColor = 'White';
            }
            if (s.cpAllowInsert == '1') {
                btnNew.SetEnabled(true);
                btnRead.SetEnabled(true);
            } else {
                btnNew.SetEnabled(false);
                btnRead.SetEnabled(false);
            }
            if (s.cpAllowUpdate == '1') {
                btnSave.SetEnabled(true);
            } else {
                btnSave.SetEnabled(false);
            }
            if (s.cpRefresh == '1') {
                gridX.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShow.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShift.GetValue());
                chartX.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShow.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShift.GetValue());
                chartR.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShow.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShift.GetValue());                                
            }                        
        }

        function ClosePopupRule1(s, e) {
            pcRule1.Hide();
            lblAuto.SetText('ON'); 
            e.processOnServer = false;
        }

        function ClosePopupRule2(s, e) {
            pcRule2.Hide();
            lblAuto.SetText('ON'); 
            e.processOnServer = false;
        }

        function ShowPopUpRule1(s, e) {
            lblAuto.SetText('OFF');
            pcRule1.Show();
        }

        function ShowPopUpRule2(s, e) {
            lblAuto.SetText('OFF');
            pcRule2.Show();
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
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding: 5px 0px 0px 0px" colspan="4">
                
                    
                <dx:ASPxComboBox ID="cboItemCheck" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboItemCheck" ValueField="ItemCheckCode" TextField="ItemCheck" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="240px" TabIndex="5" >
                    <ClientSideEvents EndCallback="function(s, e) {
                            cboItemCheck.SetEnabled(true);}"
                        SelectedIndexChanged="cboItemCheckChanged"/>

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
                <td colspan="2" style="padding:5px 0px 0px 10px; width:100px">
                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Show Verified Only" 
                        Font-Names="Segoe UI" Font-Size="9pt" Width="110px">
                    </dx:ASPxLabel>                

                </td>
                <td style="padding:5px 0px 0px 0px;">
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
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Shift" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding:5px 0px 0px 0px">
                
                <dx:ASPxComboBox ID="cboShift" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboShift" ValueField="ShiftCode" TextField="ShiftCode" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="100px" TabIndex="3">
                    <ClientSideEvents SelectedIndexChanged="cboShiftChanged" 
                        EndCallback="cboShiftEndCallback"
                        />
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
<Paddings Padding="4px"></Paddings>
                    </ButtonStyle>
                </dx:ASPxComboBox>                
            </td>
            <td style="padding:5px 3px 0px 10px">
                <dx:ASPxLabel ID="lblqeleader2" runat="server" Text="Seq" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>            
            <td style="padding:5px 0px 0px 0px">
                
                <dx:ASPxComboBox ID="cboSeq" runat="server" Theme="Office2010Black" 
                    ClientInstanceName="cboSeq" ValueField="SequenceNo" TextField="SequenceNo" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" 
                    Width="60px" TabIndex="3">
                    <ClientSideEvents EndCallback="cboSeqEndCallback"/>

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
 	                    grid.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + cboShow.GetValue());
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
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnNew" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="New (Alt+N)" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="110px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                    <ClientSideEvents Click="AddNewRow"
                                    />
                                </dx:ASPxButton>
                        </td>
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
                                    <ClientSideEvents Click="OpenMeasurement" />
                                </dx:ASPxButton>                             
                        </td>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnExcel" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Excel" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="90px" TabIndex="10">
                                    <ClientSideEvents Click="ExcelClicked" />
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
            OnBatchUpdate="grid_BatchUpdate"   
            OnRowValidating="grid_RowValidating"
            Width="100%" 
            Font-Names="Segoe UI" Font-Size="9pt" CssClass="auto-style2">
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

            <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0" Width="50px">
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn Caption="Data#" VisibleIndex="1" FieldName="SeqNo" Width="50px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Value" VisibleIndex="2" FieldName="Value" Width="80px">
                <PropertiesTextEdit SelectInputTextOnClick="True" DisplayFormatString="0.000" Width="70px">
                    <MaskSettings UseInvariantCultureDecimalSymbolOnClient="True" />
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
            <dx:GridViewDataTextColumn Caption="Operator" VisibleIndex="4" FieldName="RegisterUser" Width="160px">
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
            <dx:GridViewDataTextColumn Caption="Last User" VisibleIndex="9" FieldName="RegisterUser" Width="160px">
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

<div style="height:16px">
    <table style="width:115px">
        <tr>
            <td class="auto-style21">
                <dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Auto Refresh: " ClientInstanceName="label31"
                    Font-Names="Segoe UI" Font-Size="9pt" ClientVisible="False">
                </dx:ASPxLabel>
            </td>
            <td style="text-align:left">
                <dx:ASPxLabel ID="lblAuto" runat="server" Text="OFF" ClientInstanceName="lblAuto"
                    Font-Names="Segoe UI" Font-Size="9pt" ClientVisible="False">
                </dx:ASPxLabel>
            </td>
        </tr>
    </table>

                


</div>    
<div>
<table style="width:100%">
    <tr>
        <td>
            <table style="width:100%">
                <tr style="height:26px">
                    <td style="width:100px">
                        <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Lot No" Font-Names="Segoe UI" Font-Size="9pt" Width="80px"></dx:ASPxLabel>
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
        <td style="width:600px">
            <table style="width:100%">
                <tr>
                    <td rowspan="3" class="auto-style20" align="center" id="colSpecial"><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="@" Font-Names="Segoe UI" Font-Size="16pt" Font-Bold="false"></dx:ASPxLabel></td>
                    <td colspan="2" class="header"><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Specification" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td colspan="3" class="header"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Control Plan" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td colspan="2" class="header" id="headerXBar"><dx:ASPxLabel runat="server" Text="X Bar Control" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header"><dx:ASPxLabel runat="server" Text="R" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td colspan="6" class="header"><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Result" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                </tr>
                <tr>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="USL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="LSL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="CL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="LCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px" id="colXBarUCL"><dx:ASPxLabel ID="label16" runat="server" Text="UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>                    
                    <td class="header" style="width:50px" id="colXBarLCL"><dx:ASPxLabel ID="label17" runat="server" Text="LCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
                    <td class="header" style="width:50px"><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel></td>
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
                    <td class="body" align="center"><dx:ASPxLabel ID="lblCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCL" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center"><dx:ASPxLabel ID="lblLCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLCL" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center" id="colXBarUCL2"><dx:ASPxLabel runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblXBarUCL" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center" id="colXBarLCL2"><dx:ASPxLabel runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblXBarLCL" ForeColor="Black"></dx:ASPxLabel></td>
                    <td class="body" align="center"><dx:ASPxLabel ID="lblRUCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblRUCL" ForeColor="Black"></dx:ASPxLabel></td>
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
                    <ClientSideEvents EndCallback="GridXEndCallBack" />
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
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <table>
                        <tr>
                            <td style="padding:10px 5px 10px 0px; width:130px">

                
                <dx:ASPxButton ID="btnRule" runat="server" AutoPostBack="False" 
                    ClientInstanceName="btnRule" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="View SPC Rule (ENG)" Theme="Office2010Silver" UseSubmitBehavior="False" 
                    Width="120px" TabIndex="10">                    
                    <Paddings Padding="2px" />
                    <ClientSideEvents Click="ShowPopUpRule1"/>
                </dx:ASPxButton>

                
                            </td>
                            <td style="padding:10px 0px 10px 5px; width:130px">
<dx:ASPxButton ID="btnRule2" runat="server" AutoPostBack="False" 
                    ClientInstanceName="btnRule2" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="View SPC Rule (IND)" Theme="Office2010Silver" UseSubmitBehavior="False" 
                    Width="120px" TabIndex="10">                    
                    <Paddings Padding="2px" />
                    <ClientSideEvents Click="ShowPopUpRule2"/>
                </dx:ASPxButton>                
                            </td>
                        <td>
<dx:ASPxPopupControl ID="pcRule1" runat="server" ClientInstanceName="pcRule1" Height="250px" Width="600px" HeaderText="SPC Rule" Modal="True"
                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                        <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <div style="height:100%; text-align: center; padding-top: 30px;">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/SPCRuleEN.png" />
    </div>
    <table style="width:100%">
        <tr>
            <td style="text-align:center; padding-top: 10px;">
                <dx:ASPxButton ID="btnHide" runat="server" AutoPostBack="False" 
                    ClientInstanceName="btnHide" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" 
                    Width="90px" TabIndex="10">                    
                    <Paddings Padding="2px" />
                    <ClientSideEvents Click="ClosePopupRule1"/>
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
                            <td class="auto-style22">
<dx:ASPxPopupControl ID="pcRule2" runat="server" ClientInstanceName="pcRule2" Height="250px" Width="600px" HeaderText="SPC Rule" Modal="True"
                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                        <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <div style="height:100%; text-align: center; padding-top: 30px;">
        <asp:Image ID="Image2" runat="server" ImageUrl="~/img/SPCRuleIN.png" />
    </div>
    <table style="width:100%">
        <tr>
            <td style="text-align:center; padding-top: 10px;">
                <dx:ASPxButton ID="btnHide2" runat="server" AutoPostBack="False" 
                    ClientInstanceName="btnHide2" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" 
                    Width="90px" TabIndex="10">                    
                    <Paddings Padding="2px" />
                    <ClientSideEvents Click="ClosePopupRule2"/>
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

                </div>
                
<div id="chartXdiv" style="overflow-x:auto; width:100%; border:1px solid black"">
<dx:WebChartControl ID="chartX" runat="server" ClientInstanceName="chartX"
        Height="490px" Width="1080px" CrosshairEnabled="True" SeriesDataMember="Description" ToolTipEnabled="False">
        <seriestemplate SeriesDataMember="Description" ArgumentDataMember="Seq" ValueDataMembersSerializable="Value" ToolTipPointPattern="{V:0.000}" CrosshairLabelPattern="{S}: {V:0.000}">
            <viewserializable>
                <cc1:PointSeriesView>                    
                    <PointMarkerOptions kind="Circle" BorderColor="255, 255, 255" Size="5"></PointMarkerOptions>
                </cc1:PointSeriesView>
            </viewserializable>
        </seriestemplate>    
        <SeriesSerializable>
            <cc1:Series ArgumentDataMember="Seq" Name="Rule" ValueDataMembersSerializable="RuleValue" LabelsVisibility="False" ShowInLegend="False" ToolTipEnabled="False" ToolTipSeriesPattern="" CrosshairEnabled="False">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Red" Transparency="200" AxisYName="Secondary AxisY 1">
                        <Border Visibility="False" />
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="RuleYellow" ValueDataMembersSerializable="RuleYellow" LabelsVisibility="False" ShowInLegend="False" ToolTipEnabled="False" ToolTipSeriesPattern="" CrosshairEnabled="False">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Yellow" Transparency="200">
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="XBar" ValueDataMembersSerializable="AvgValue" CrosshairLabelPattern="{S}: {V:0.000}">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="Blue">
                        <LineStyle Thickness="1" />
                        <LineMarkerOptions Color="Blue" Size="5" Kind="Diamond"></LineMarkerOptions>
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
                    <VisualRange Auto="False" MaxValueSerializable="9" MinValueSerializable="0" />
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
                    <VisualRange Auto="False" AutoSideMargins="False" EndSideMargin="0.015" MaxValueSerializable="2.715" MinValueSerializable="2.71" StartSideMargin="0.025" />
                    <WholeRange AlwaysShowZeroLevel="False" Auto="False" AutoSideMargins="False" EndSideMargin="0.005" MaxValueSerializable="2.73" MinValueSerializable="2.71" StartSideMargin="0.005" />
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
            <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="" />
        </titles>
        <legend alignmenthorizontal="Left" alignmentvertical="BottomOutside" 
            direction="LeftToRight"></legend> 
        <ToolTipOptions ShowForPoints="False" ShowForSeries="True">
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
        Height="230px" Width="1080px" CrosshairEnabled="True">
        <SeriesSerializable>
            <cc1:Series ArgumentDataMember="Seq" Name="R" ValueDataMembersSerializable="RValue">
                <ViewSerializable>
                    <cc1:LineSeriesView Color="Blue">
                        <LineStyle Thickness="1" />
                    <LineMarkerOptions Size="7" Color="Blue" Kind="Diamond"></LineMarkerOptions>
                    </cc1:LineSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series Name="RuleYellow" ArgumentDataMember="Seq" ShowInLegend="False" ToolTipEnabled="False" ValueDataMembersSerializable="RuleYellow" CrosshairEnabled="False">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="255, 255, 0" Transparency="100" AxisYName="Secondary AxisY 1">
                        <Border Visibility="False" />
                        <FillStyle FillMode="Solid">
                        </FillStyle>
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Seq" Name="RuleRed" ShowInLegend="False" ValueDataMembersSerializable="RuleRed" CrosshairEnabled="False" ToolTipEnabled="False">
                <ViewSerializable>
                    <cc1:FullStackedBarSeriesView BarWidth="1" Color="Pink" Transparency="100">
                        <FillStyle FillMode="Solid">
                        </FillStyle>
                    </cc1:FullStackedBarSeriesView>
                </ViewSerializable>
            </cc1:Series>
        </SeriesSerializable>
        <seriestemplate ValueDataMembersSerializable="Value">            
            <viewserializable>
                <cc1:LineSeriesView>
                    <LineMarkerOptions BorderColor="White" Size="3">
                    </LineMarkerOptions>
                </cc1:LineSeriesView>
            </viewserializable>
        </seriestemplate>  
        <DiagramSerializable>
            <cc1:XYDiagram>
                <AxisX VisibleInPanesSerializable="-1" MinorCount="1" Visibility="False">
                    <Tickmarks Visible="False" />
                    <WholeRange AutoSideMargins="False" EndSideMargin="0.5" StartSideMargin="0.5" />
                    <GridLines MinorVisible="True">
                    </GridLines>
                    <NumericScaleOptions AutoGrid="False" ScaleMode="Manual" />
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
                <SecondaryAxesY>
                    <cc1:SecondaryAxisY AxisID="0" Name="Secondary AxisY 1" Visibility="False" VisibleInPanesSerializable="-1">
                        <Tickmarks MinorVisible="False" Visible="False" />
                    </cc1:SecondaryAxisY>
                </SecondaryAxesY>
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
                lblCL.SetText(s.cpCL);
                lblLCL.SetText(s.cpLCL);
            }" 
            />

        </dx:ASPxCallback>

    </div>
</div>

</asp:Content>
