<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FTACorrectiveAction.aspx.vb" Inherits="PECGI_SPC.FTACorrectiveAction" %>

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
            width: 100%;
            height: 477px;
        }
        </style>
    <script type="text/javascript" >
        var rowIndex, columnIndex;
        var prevShift;
        var prevSeq;

        function cboShiftEndCallback(s, e) {
            cboShift.SetEnabled(true);            
            cboShift.SetValue(prevShift);    
            cboSeq.PerformCallback(cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + cboShift.GetValue());            
        }

        function cboSeqEndCallback(s, e) {
            cboSeq.SetEnabled(true);
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

        function GridLoad(s, e) {
            grid.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|0');
        }

        function OnBatchEditEndEditing(s, e) {
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
                GridLoad();
        }

        function ValidateSave() {
            cbkValid.PerformCallback('validate');
        }

        function cbkValidEndCallback(s, e) {
            if(s.cpErrMsg != '') {
                toastr.warning(s.cpErrMsg, 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                toastr.options.onclick = null;
            } else {
                SaveData();
            }
        }
        
        function gridEditOnEndCallback(s, e) {
            gridEdit.CancelEdit();    
            if(s.cpUpdate != '') {
                SaveData();
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
            if(s.cpCount == '') {
                txtRemark.SetEnabled(false);
                btnSubmit.SetEnabled(false);
                btnMK.SetEnabled(false);
                btnQC.SetEnabled(false);
            } else {                
                if(s.cpMKVerificationStatus == '1' | s.cpJobPosition != 'MK') {
                    btnMK.SetEnabled(false);
                } else {    
                    btnMK.SetEnabled(true);
                }
                if(s.cpMKVerificationStatus == '1' & s.cpQCVerificationStatus != '1' & s.cpJobPosition != 'QC') {
                    btnQC.SetEnabled(false);
                } else {
                    btnQC.SetEnabled(true);
                }
                if(s.cpMKVerificationStatus == '1' | s.cpQCVerificationStatus == '1') {
                    txtRemark.SetEnabled(false);
                    btnSubmit.SetEnabled(false);
                } else {
                    txtRemark.SetEnabled(true);
                    btnSubmit.SetEnabled(true);
                }
            }
            txtRemark.SetText(s.cpRemark);
        }

        function ClosePopUpFTA(s, e) {
            pcFTA.Hide();
            e.processOnServer = false;
        }

        function ClosePopupAction(s, e) {
            pcAction.Hide();
            e.processOnServer = false;
        }

        function SaveAction() {   
            var isChecked = chkOther.GetChecked();
            var FTAID = hfDetail.Get('FTAID');
            var DetSeqNo = hfDetail.Get('DetSeqNo');         
            var pAction = txtOther.GetText();
            var idx = hfDetail.Get('Index');

            hfAct.Set(idx, pAction);
            if(isChecked) {
                
                gridEdit.PerformCallback('save|' + FTAID + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + txtRemark.GetText() + '|' + pAction + '|2|' + DetSeqNo );
            } else {
                gridEdit.UpdateEdit();
            }
            pcEdit.Hide();
            chkOther.SetChecked(false);
            txtOther.SetText('');           
            txtOther.SetEnabled(false);
            e.processOnServer = false;
        }

        function ClosePopupIK(s, e) {
            pcIK.Hide();
            e.processOnServer = false;
        }

        function ShowPopUpFTA(s, e) {
            gridFTA.PerformCallback('load' + '|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue());
            pcFTA.Show();
        }

        function ShowPopUpAction(s) {
            gridAction.PerformCallback(s);
            pcAction.Show();
        }

        function ShowPopUpEdit(FTAID, DetSeqNo, Index) {
            pcEdit.Show();
            gridEdit.PerformCallback('load|' + FTAID);
            hfDetail.Set('FTAID', FTAID);
            hfDetail.Set('DetSeqNo', DetSeqNo);      
            hfDetail.Set('Index', Index);
        }

        function ShowPopUpIK(s) {
            cbkPanel.PerformCallback(s);
            pcIK.Show();
        }

        function SPCSample() {
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

            var Factory = cboFactory.GetValue();
            var ItemType = cboType.GetValue();
            var Line = cboLine.GetValue();
            var ItemCheck = cboItemCheck.GetValue();
            var ProdDate = dtDate.GetText();
            var Shift = cboShift.GetValue();
            var Seq = cboSeq.GetValue();

            window.open('ProdSampleInput.aspx?menu=FTACorrectiveAction.aspx' + '&FactoryCode=' + Factory + '&ItemTypeCode=' + ItemType
                + '&Line=' + Line + '&ItemCheckCode=' + ItemCheck + '&ProdDate=' + ProdDate + '&Shift=' + Shift + '&Sequence=' + Seq
                + '', '_blank');
        }

        function MKVerify() {
            grid.PerformCallback('mkverify|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue());
        }

        function QCVerify() {
            grid.PerformCallback('qcverify|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue());
        }

        function SaveData() {
            grid.PerformCallback('save|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|' + txtRemark.GetText());
        }

        var checkBoxColumns = ['Select'];
        var lastEditedColumn;
        var rowIndex;
        function OnBatchEditStartEditing(s, e) {
            lastEditedColumn = e.focusedColumn.fieldName;
            rowIndex = e.visibleIndex;
        }

        function OnCheckedChanged(s, e) {
            gridEdit.SetFocusedRowIndex(-1);
            var isChecked = s.GetChecked();
            if(isChecked) {
                chkOther.SetChecked(false);
                txtOther.SetEnabled(false);
            }
            for(var i = 0; i < gridEdit.GetVisibleRowsOnPage(); i++) {
                if(i != rowIndex) {
                    gridEdit.batchEditApi.SetCellValue(i, "Select", 0);
                }
            }

        }

        function SelectNoCheck(startRow, checked) {
            for(var i = startRow + 1; i < grid.GetVisibleRowsOnPage(); i++) {
                grid.batchEditApi.SetCellValue(i, "NoCheck", checked);
            }
        }

        function chkOtherCheckedChanged(s, e) {
            var isChecked = s.GetChecked();
            if(isChecked) 
            {
                for (var i = 0; i < gridEdit.GetVisibleRowsOnPage(); i++) {
                    gridEdit.batchEditApi.SetCellValue(i, "Select", 0);
                }
                txtOther.SetEnabled(true);
                txtOther.Focus();
            } else {
                txtOther.SetEnabled(false);
            }
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
                    &nbsp;</td>
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
 	                    grid.PerformCallback('load|' + cboFactory.GetValue() + '|' + cboType.GetValue() + '|' + cboLine.GetValue() + '|' + cboItemCheck.GetValue() + '|' + dtDate.GetText() + '|' + cboShift.GetValue() + '|' + cboSeq.GetValue() + '|0');
                    }" />
                    <Paddings Padding="2px" />
                </dx:ASPxButton>

                
            </td>            
        </tr>
    </table>
    </div>
<div style="height:10px">
    <dx:ASPxHiddenField ID="hfDetail" runat="server" ClientInstanceName="hfDetail">
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
                                <dx:ASPxButton ID="btnView" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnView" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="View FTA Diagram" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="120px" TabIndex="10">
                                    <ClientSideEvents Click="ShowPopUpFTA"/>
                                    <Paddings Padding="2px" />                                    
                                </dx:ASPxButton>
                        </td>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnIOT" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnIOT" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="IOT Traceability" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="120px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                </dx:ASPxButton>
                        </td>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnSample" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnSample" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="SPC Sample" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="120px" TabIndex="10">
                                    <Paddings Padding="2px" />    
                                    <ClientSideEvents Click="SPCSample" />
                                </dx:ASPxButton>                             
                        </td>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnMK" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnMK" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Verification by MK" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="120px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                    <ClientSideEvents Click="MKVerify" />
                                </dx:ASPxButton>                            
                        </td>
                        <td style="padding-right:5px">
                                <dx:ASPxButton ID="btnQC" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnQC" Font-Names="Segoe UI" Font-Size="9pt" 
                                    Height="25px" Text="Verification by QC" Theme="Office2010Silver" UseSubmitBehavior="False" 
                                    Width="120px" TabIndex="10">
                                    <Paddings Padding="2px" />
                                    <ClientSideEvents Click="QCVerify" />
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
            EnableTheming="True" KeyFieldName="No" Theme="Office2010Black"            
            Width="100%" 
            Font-Names="Segoe UI" Font-Size="9pt" CssClass="auto-style2">
            <ClientSideEvents 
                EndCallback="OnEndCallback"
             />
            <SettingsResizing ColumnResizeMode="Control" />
            <SettingsDataSecurity AllowDelete="False" />

<SettingsPopup>
    <EditForm Modal="false" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200" />
    <FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>
        <Columns>

            <dx:GridViewDataTextColumn FieldName="No" ShowInCustomizationForm="True" VisibleIndex="0" Width="40px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Description" ShowInCustomizationForm="True" VisibleIndex="1" Width="300px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="NoCheck" ShowInCustomizationForm="True" VisibleIndex="3" Width="50px">
                        <DataItemTemplate>
                            <dx:ASPxCheckBox ID="chkNo" runat="server" OnInit="chkNo_Init" Value='<%# Eval("NoCheck")%>' />
                        </DataItemTemplate>
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn FieldName="Action" ShowInCustomizationForm="True" VisibleIndex="6" Width="260px">
                        <DataItemTemplate>
                            <table style="width:100%">
                                <tr>
                                    <td style="width:36px; text-align:left">
                                        <dx:ASPxHyperLink ID="linkEdit" Font-Names="Segoe UI" Font-Size="9pt"
                                            runat="server" Text="Edit" OnInit="EditLink_Init">
                                        </dx:ASPxHyperLink>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel runat="server" Text='<%# Eval("Action")%>'></dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                            
                            
                        </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LastUser" ShowInCustomizationForm="True" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LastUpdate" ShowInCustomizationForm="True" VisibleIndex="8" Width="120px">
                <PropertiesTextEdit DisplayFormatString="dd MMM yyyy HH:mm">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ViewIK" ShowInCustomizationForm="True" VisibleIndex="4" Width="50px">
                        <DataItemTemplate>
                            <dx:ASPxHyperLink ID="linkIK" Font-Names="Segoe UI" Font-Size="9pt"
                                runat="server" Text="View" OnInit="IKLink_Init">
                            </dx:ASPxHyperLink>
                        </DataItemTemplate>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewBandColumn Caption="Check Result" ShowInCustomizationForm="True" VisibleIndex="2">
                <Columns>
                    <dx:GridViewDataCheckColumn FieldName="OK" ShowInCustomizationForm="True" VisibleIndex="0" Width="40px">
                        <DataItemTemplate>
                            <dx:ASPxCheckBox ID="chkOK" runat="server" OnInit="chkOK_Init" Value='<%# Eval("OK")%>' />
                        </DataItemTemplate>
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataCheckColumn FieldName="NG" ShowInCustomizationForm="True" VisibleIndex="1" Width="40px">
                        <DataItemTemplate>
                            <dx:ASPxCheckBox ID="chkNG" runat="server" OnInit="chkNG_Init" Value='<%# Eval("NG")%>'/>
                        </DataItemTemplate>
                    </dx:GridViewDataCheckColumn>
                </Columns>
            </dx:GridViewBandColumn>

            <dx:GridViewDataTextColumn FieldName="FTAID" VisibleIndex="9" Width="0px">
                <PropertiesTextEdit>
                    <Style Wrap="False">
                    </Style>
                </PropertiesTextEdit>
                <CellStyle Wrap="False">
                </CellStyle>
            </dx:GridViewDataTextColumn>
        </Columns>        
        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" AllowDragDrop="False" AllowSort="False" />
        <SettingsEditing Mode="Batch" EditFormColumnCount="1" >
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
            <Cell Wrap="False">
            </Cell>
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
<div>
<div style="height:10px"></div>
<div style="width:100%">
    <table class="nav-justified">
        <tr>
            <td style="width:70px; padding-left:5px; vertical-align:top">

                <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Remarks" 
                    Font-Names="Segoe UI" Font-Size="9pt" Width="70px">
                </dx:ASPxLabel>

            </td>
            <td style="width:510px">

                        <dx:ASPxMemo ID="txtRemark" runat="server" Height="45px" Width="500px" ClientInstanceName="txtRemark">
                        </dx:ASPxMemo>

            </td>
            <td style="padding-left:10px; vertical-align:top">

                
                <dx:ASPxButton ID="btnSubmit" runat="server" AutoPostBack="False" 
                    ClientInstanceName="btnSubmit" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="Submit" Theme="Office2010Silver" UseSubmitBehavior="False" 
                    Width="90px" TabIndex="10" ClientEnabled="false">                    
                    <Paddings Padding="2px" PaddingLeft="5px" />

                    <ClientSideEvents Click="ValidateSave"/>
                </dx:ASPxButton>

                
            </td>
            <td style="padding-left:10px; vertical-align:top">
                <dx:ASPxHiddenField ID="hfID" runat="server" ClientInstanceName="hfID">
                </dx:ASPxHiddenField>                
                <dx:ASPxHiddenField ID="hfOK" runat="server" ClientInstanceName="hfOK">
                </dx:ASPxHiddenField>
                <dx:ASPxHiddenField ID="hfNG" runat="server" ClientInstanceName="hfNG">
                </dx:ASPxHiddenField>
                <dx:ASPxHiddenField ID="hfNo" runat="server" ClientInstanceName="hfNo">
                </dx:ASPxHiddenField>
                <dx:ASPxHiddenField ID="hfAct" runat="server" ClientInstanceName="hfAct">
                </dx:ASPxHiddenField>

                <dx:ASPxCallback ID="cbkValid" runat="server" ClientInstanceName="cbkValid">
                    <ClientSideEvents EndCallback="cbkValidEndCallback" />
                </dx:ASPxCallback>

            </td>
        </tr>
    </table>
</div>

<div>
<dx:ASPxPopupControl ID="pcFTA" runat="server" ClientInstanceName="pcFTA" Height="520px" Width="1024px" HeaderText="FTA Diagram" Modal="True"
                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                        <ContentCollection>
<dx:PopupControlContentControl runat="server">

    <dx:ASPxGridView ID="gridFTA" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridFTA" CssClass="auto-style2" EnableTheming="True" Font-Names="Segoe UI" Font-Size="9pt" KeyFieldName="FTAID" Theme="Office2010Black" Width="100%">
        <SettingsPager AlwaysShowPager="True" Mode="ShowAllRecords" PageSize="30">
        </SettingsPager>
        <SettingsEditing EditFormColumnCount="1" Mode="Batch">
            <BatchEditSettings ShowConfirmOnLosingChanges="False" />
        </SettingsEditing>
        <Settings HorizontalScrollBarMode="Auto" ShowStatusBar="Hidden" VerticalScrollableHeight="400" VerticalScrollBarMode="Auto" />
        <SettingsBehavior AllowDragDrop="False" AllowSort="False" ColumnResizeMode="Control" ConfirmDelete="True" />
        <SettingsResizing ColumnResizeMode="Control" />
        <SettingsDataSecurity AllowDelete="False" />
        <SettingsPopup>
            <EditForm HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200px">
            </EditForm>
            <FilterControl AutoUpdatePosition="False">
            </FilterControl>
        </SettingsPopup>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Factor1" ShowInCustomizationForm="True" VisibleIndex="0" Width="160px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Factor2" ShowInCustomizationForm="True" VisibleIndex="1" Width="160px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Factor3" ShowInCustomizationForm="True" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IK" ShowInCustomizationForm="True" VisibleIndex="7" Width="50px">
                <DataItemTemplate>
                    <dx:ASPxHyperLink ID="linkIK0" runat="server" Font-Names="Segoe UI" Font-Size="9pt" OnInit="IKLink2_Init" Text="View">
                    </dx:ASPxHyperLink>
                </DataItemTemplate>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Factor3" ShowInCustomizationForm="True" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CounterMeasure" ShowInCustomizationForm="True" VisibleIndex="4" Width="180px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CheckItem" ShowInCustomizationForm="True" VisibleIndex="5" Width="180px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Action" ShowInCustomizationForm="True" VisibleIndex="6" Width="50px">
                <DataItemTemplate>
                    <dx:ASPxHyperLink ID="linkAction" runat="server" Font-Names="Segoe UI" Font-Size="9pt" OnInit="ActionLink_Init" Text="View">
                    </dx:ASPxHyperLink>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FTAID" ShowInCustomizationForm="True" VisibleIndex="8" Width="0px">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Styles>
            <Header HorizontalAlign="Center" Wrap="True">
                <Paddings Padding="2px" />
            </Header>
            <DetailCell Wrap="False">
            </DetailCell>
            <SelectedRow BackColor="White" ForeColor="Black">
            </SelectedRow>
            <CommandColumnItem ForeColor="SteelBlue">
            </CommandColumnItem>
            <EditFormColumnCaption Font-Names="Segoe UI" Font-Size="9pt">
                <Paddings PaddingBottom="5px" PaddingLeft="15px" PaddingRight="15px" PaddingTop="5px" />
            </EditFormColumnCaption>
            <BatchEditModifiedCell BackColor="#FFFF99" ForeColor="Black">
            </BatchEditModifiedCell>
        </Styles>
    </dx:ASPxGridView>

    <table style="width:100%">
        <tr style="width:100px">
            <td style="text-align:center; padding-top: 10px;">
                <dx:ASPxButton ID="btnHide" runat="server" AutoPostBack="False" ClientInstanceName="btnHide" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" Width="90px">
                    <ClientSideEvents Click="ClosePopUpFTA" />
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
<dx:ASPxPopupControl ID="pcIK" runat="server" ClientInstanceName="pcIK" Height="520px" Width="1024px" HeaderText="Instruksi Kerja (IK)" Modal="True"
                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                        <ContentCollection>
<dx:PopupControlContentControl runat="server">
<dx:ASPxCallbackPanel ID="cbkPanel" runat="server" Width="100%" ClientInstanceName="cbkPanel">
    <SettingsLoadingPanel Enabled="False" />
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table class="auto-style20">
                                <tr style="height:100%">
                                    <td style="text-align:center">
                                        <asp:Image ID="imgIK" runat="server" BorderStyle="Solid" Height="160" Width="230"/>
                                    </td>
                                </tr>
                            </table>                            
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
    <table style="width:100%">
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
<dx:ASPxPopupControl ID="pcAction" runat="server" ClientInstanceName="pcAction" Height="350px" Width="586px" HeaderText="FTA Action" Modal="True"
                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                        <ContentCollection>
<dx:PopupControlContentControl runat="server">

    <dx:ASPxGridView ID="gridAction" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridAction" CssClass="auto-style2" EnableTheming="True" Font-Names="Segoe UI" Font-Size="9pt" KeyFieldName="FTAID" Theme="Office2010Black" Width="100%">
        <SettingsPager AlwaysShowPager="True" Mode="ShowAllRecords" PageSize="30">
        </SettingsPager>
        <SettingsEditing EditFormColumnCount="1" Mode="Batch">
            <BatchEditSettings ShowConfirmOnLosingChanges="False" />
        </SettingsEditing>
        <Settings HorizontalScrollBarMode="Auto" ShowStatusBar="Hidden" VerticalScrollableHeight="260" VerticalScrollBarMode="Auto" />
        <SettingsBehavior AllowDragDrop="False" AllowSort="False" ColumnResizeMode="Control" ConfirmDelete="True" />
        <SettingsResizing ColumnResizeMode="Control" />
        <SettingsDataSecurity AllowDelete="False" />
        <SettingsPopup>
            <EditForm HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200px">
            </EditForm>
            <FilterControl AutoUpdatePosition="False">
            </FilterControl>
        </SettingsPopup>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ActionName" ShowInCustomizationForm="True" VisibleIndex="1" Width="200px" Caption="Action">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ActionID" ShowInCustomizationForm="True" VisibleIndex="0" Width="60px" Caption="No">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Styles>
            <Header HorizontalAlign="Center" Wrap="True">
                <Paddings Padding="2px" />
            </Header>
            <DetailCell Wrap="False">
            </DetailCell>
            <SelectedRow BackColor="White" ForeColor="Black">
            </SelectedRow>
            <CommandColumnItem ForeColor="SteelBlue">
            </CommandColumnItem>
            <EditFormColumnCaption Font-Names="Segoe UI" Font-Size="9pt">
                <Paddings PaddingBottom="5px" PaddingLeft="15px" PaddingRight="15px" PaddingTop="5px" />
            </EditFormColumnCaption>
            <BatchEditModifiedCell BackColor="#FFFF99" ForeColor="Black">
            </BatchEditModifiedCell>
        </Styles>
    </dx:ASPxGridView>

    <table style="width:100%">
        <tr style="width:100px">
            <td style="text-align:center; padding-top: 10px;">
                <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" ClientInstanceName="btnHideAction" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False" Width="90px">
                    <ClientSideEvents Click="ClosePopupAction" />
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
<dx:ASPxPopupControl ID="pcEdit" runat="server" ClientInstanceName="pcEdit" Height="350px" Width="600px" HeaderText="FTA Action" Modal="True"
                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                        <ContentCollection>
<dx:PopupControlContentControl runat="server">

    <dx:ASPxGridView ID="gridEdit" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridEdit" CssClass="auto-style2" EnableTheming="True" Font-Names="Segoe UI" Font-Size="9pt" KeyFieldName="ActionID" Theme="Office2010Black" Width="100%" OnBatchUpdate="gridEdit_BatchUpdate">
        <ClientSideEvents BatchEditStartEditing="OnBatchEditStartEditing" EndCallback="gridEditOnEndCallback" />
        <SettingsPager AlwaysShowPager="True" Mode="ShowAllRecords" PageSize="30">
        </SettingsPager>
        <SettingsEditing EditFormColumnCount="1" Mode="Batch">
            <BatchEditSettings ShowConfirmOnLosingChanges="False" />
        </SettingsEditing>
        <Settings HorizontalScrollBarMode="Auto" ShowStatusBar="Hidden" VerticalScrollableHeight="260" VerticalScrollBarMode="Auto" />
        <SettingsBehavior AllowDragDrop="False" AllowSort="False" ColumnResizeMode="Control" ConfirmDelete="True" />
        <SettingsDataSecurity AllowDelete="False" />
        <SettingsPopup>
            <EditForm HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200px">
            </EditForm>
            <FilterControl AutoUpdatePosition="False">
            </FilterControl>
        </SettingsPopup>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ActionName" ShowInCustomizationForm="True" VisibleIndex="2" Width="420px" Caption="Action">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ActionID" ShowInCustomizationForm="True" VisibleIndex="1" Width="60px" Caption="ID">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption=" " FieldName="Select" ShowInCustomizationForm="True" VisibleIndex="0" Width="40px">
                <PropertiesCheckEdit ClientInstanceName="chkSelect" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                            <ClientSideEvents CheckedChanged="OnCheckedChanged" />
                        </PropertiesCheckEdit>
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn FieldName="FTAID" ShowInCustomizationForm="True" VisibleIndex="3" Width="90px">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Styles>
            <Header HorizontalAlign="Center" Wrap="True">
                <Paddings Padding="2px" />
            </Header>
            <DetailCell Wrap="False">
            </DetailCell>
            <SelectedRow BackColor="White" ForeColor="Black">
            </SelectedRow>
            <CommandColumnItem ForeColor="SteelBlue">
            </CommandColumnItem>
            <EditFormColumnCaption Font-Names="Segoe UI" Font-Size="9pt">
                <Paddings PaddingBottom="5px" PaddingLeft="15px" PaddingRight="15px" PaddingTop="5px" />
            </EditFormColumnCaption>
            <BatchEditModifiedCell ForeColor="Black">
            </BatchEditModifiedCell>
        </Styles>
    </dx:ASPxGridView>
    <div style="height:10px"></div>
    <table style="width:100%">
        <tr>
            <td style="width:80px; padding-left:5px">

                <dx:ASPxCheckBox ID="chkOther" runat="server" CheckState="Unchecked" Text="Other" Width="70px" ClientInstanceName="chkOther">
                    <ClientSideEvents CheckedChanged="chkOtherCheckedChanged" />
                </dx:ASPxCheckBox>

            </td>
            <td>

                <dx:ASPxTextBox ID="txtOther" runat="server" Width="450px" ClientInstanceName="txtOther" ClientEnabled="False" MaxLength="50">
                    <ReadOnlyStyle BackColor="Silver">
                    </ReadOnlyStyle>
                    <DisabledStyle BackColor="Silver">
                    </DisabledStyle>
                </dx:ASPxTextBox>

            </td>
        </tr>
        <tr style="width:100px">

            <td style="text-align:center; padding-top: 10px;" colspan="2" align="center">
                <dx:ASPxButton ID="btnSaveAction" runat="server" AutoPostBack="False" ClientInstanceName="btnSaveAction" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="10" Text="Save" Theme="Office2010Silver" UseSubmitBehavior="False" Width="90px">
                    <ClientSideEvents Click="SaveAction" />
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
        <dx:ASPxCallback ID="cbkRefresh" runat="server" ClientInstanceName="cbkRefresh"></dx:ASPxCallback>

    </div>
</asp:Content>

