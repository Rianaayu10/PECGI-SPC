﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TCCSMasterApproval.aspx.vb" Inherits="PECGI_SPC.TCCSMasterApproval" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" >
        <%--Function for Message--%>
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

 if (s.cp_viewtccs == 1){
        window.open('TCCSMaster.aspx?PartID=' + s.cp_partid + '&PartName=' + s.cp_partname + '&LineID=' + s.cp_lineid + '&SubLineID=' + s.cp_sublineid + '&MachineNo=' + s.cp_machineno + '&RevNo=' + s.cp_revno, "NewWindow");
        s.cp_viewtccs = null;
}

if(s.cp_messageapprove == 1){
    
    toastr.warning('Please check the data that you will approve!', 'Warning');
    toastr.options.closeButton = false;
    toastr.options.debug = false;
    toastr.options.newestOnTop = false;
    toastr.options.progressBar = false;
    toastr.options.preventDuplicates = true;
    toastr.options.onclick = null;
    e.processOnServer = false;
    s.cp_messageapprove = null;
    return;   
}

}//End Callback

function SelectLineID(s, e) {
    var sublineid
    sublineid = cbosublineid.GetValue();
    cbosublineid.PerformCallback(cbolineid.GetValue());
    if (sublineid == 'ALL'){
        cbosublineid.SetValue('ALL');
    }
}

function SelectSubLineID(s, e) {
    var machineno
    machineno = cbomachineno.GetValue();
    cbomachineno.PerformCallback(cbolineid.GetValue() + '|' + cbosublineid.GetValue());
    if (machineno == 'ALL'){
        cbomachineno.SetValue('ALL');
    }
}

function SelectMachineNo(s, e) {
    var partid
    partid = cbopartid.GetValue();
    cbopartid.PerformCallback();
    if (partid == 'ALL'){
        cbopartid.SetValue('ALL');
    }
    else{
        txtpartname.SetText('');
    }
}

var startTime;
function OnBeginCallbackPartID() {
	startTime = new Date();
}
function OnEndCallbackPartID() {
	var result = new Date() - startTime;
	result /= 1000;
	result = result.toString();
	if(result.length > 4){
		result = result.substr(0, 4);
    }
}

function SelectPartID(s, e) {
    var revno
    revno = cborevno.GetValue();
    txtpartname.SetText(cbopartid.GetSelectedItem().GetColumnText(1));
    cborevno.PerformCallback(cbolineid.GetValue() + '|' + cbosublineid.GetValue() + '|' + cbomachineno.GetValue() + '|' + cbopartid.GetValue());
    if (revno == 'ALL' || revno == 'Latest Rev'){
        cborevno.SetValue(revno);
    }
}

function SelectRevNo(s, e) {
    
}

function BtnClearClick(s, e) {
    dtstart.SetText('');
    dtend.SetText('');
    cbolineid.SetText('');
    cbosublineid.SetText('');
    cbomachineno.SetText('');
    cbopartid.SetText('');
    txtpartname.SetText('');
    cborevno.SetText('');
    cboapprovalstatus.SetText('');
    GridMenu.PerformCallback('Clear');
}

function ClickRefresh(s, e) {
    if (dtstart.GetText() == '' ) {
        toastr.warning('Please select Date From No!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        } 

     if (dtend.GetText() == '' ) {
        toastr.warning('Please select Date To No!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        } 

    if (cbolineid.GetText() == '' ) {
        toastr.warning('Please select Line No!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        } 

        if (cbosublineid.GetText() == '' ) {
        toastr.warning('Please select Sub Line No!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        } 

        if (cbomachineno.GetText() == '' ) {
        toastr.warning('Please select Machine No!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        } 

        if (cbopartid.GetText() == '' ) {
        toastr.warning('Please select Part ID!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        }

        if (cborevno.GetText() == '' ) {
        toastr.warning('Please select Rev No!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        }

        if (cboapprovalstatus.GetText() == '' ) {
        toastr.warning('Please select Approval Status!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
        }
        
        GridMenu.PerformCallback('Refresh');
        btnExcel.SetEnabled(true);
}

function BtnApproveClick(s, e) {
    var r = confirm("Are you sure want to approve?");
    if (r == true) {
        GridMenu.PerformCallback('Approve');
    }     
}

function ViewTCCS(s, e) {
    var count = GridMenu.GetSelectedRowCount();
    if (count == 0){
        toastr.warning('Please select data!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
    }else if(count > 1){
        toastr.warning('Please select only one data!', 'Warning');
        toastr.options.closeButton = false;
        toastr.options.debug = false;
        toastr.options.newestOnTop = false;
        toastr.options.progressBar = false;
        toastr.options.preventDuplicates = true;
        toastr.options.onclick = null;

        e.processOnServer = false;
        return;
    }else{
        GridMenu.PerformCallback('ViewQCS|');  
    } 
    
}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 0px 5px 5px 5px">
    <table style="width: 100%;">
        <tr>
            <td style="padding: 5px 0px 5px 0px" width="65px">
                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Date From" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding: 5px 0px 5px 0px" width="10px">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px" width="65px">
                <dx:ASPxDateEdit ID="dtstart" runat="server" Theme="Office2010Black" 
                    Width="100px" AutoPostBack="false"
                        ClientInstanceName="dtstart" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="7">
                        <%--<ClientSideEvents Init="function(s, e){var today = new Date(); dtstart.SetDate(today);}" />--%>
                        <CalendarProperties>
                            <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="10px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="10px"></ButtonStyle>
                        </CalendarProperties>
                        <ButtonStyle Width="5px" Paddings-Padding="4px" ></ButtonStyle>
                    </dx:ASPxDateEdit>
                    
            </td>
            <td style="padding: 5px 0px 5px 0px" width="10px">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px" width="70px">
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Line No." 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 5px 0px 3px 0px" width="75px">
               <dx:ASPxComboBox ID="cbolineid" runat="server" Theme="Office2010Black" TextField="LineName"
                    ClientInstanceName="cbolineid" DropDownStyle="DropDownList" ValueField="LineID"
                    EnableIncrementalFiltering="True" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" TextFormatString="{0}" DisplayFormatString="{0}"
                    Width="75px" TabIndex="1">
                    <ClientSideEvents SelectedIndexChanged="SelectLineID" Init="function(s,e){cbolineid.SetValue('ALL');}"/>
                    <Columns>
                        <dx:ListBoxColumn Caption="Line ID" FieldName="LineID" Width="70px" />
                        <dx:ListBoxColumn Caption="Line Name" FieldName="LineName" 
                            Width="250px" />
                    </Columns>
                    <ItemStyle Height="10px" Paddings-Padding="4px"/>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
                    </ButtonStyle>
                </dx:ASPxComboBox>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td width="50px" style="padding: 5px 0px 5px 0px">
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Machine" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 5px 0px 3px 0px" width="95px">
               <dx:ASPxComboBox ID="cbomachineno" runat="server" Theme="Office2010Black" TextField="MachineNo"
                    ClientInstanceName="cbomachineno" DropDownStyle="DropDownList" ValueField="MachineNo"
                    EnableIncrementalFiltering="True" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" TextFormatString="{0}" DisplayFormatString="{0}"
                    Width="95px" TabIndex="3">
                    <ClientSideEvents SelectedIndexChanged="SelectMachineNo" Init="function(s,e){cbomachineno.PerformCallback('ALL' + '|' + 'ALL'); cbomachineno.SetValue('ALL');}"/>
                    <Columns>
                        <dx:ListBoxColumn Caption="Machine No" FieldName="MachineNo" Width="70px" />
                        <dx:ListBoxColumn Caption="Line ID" FieldName="LineID" Width="70px" />
                        <dx:ListBoxColumn Caption="Sub Line ID" FieldName="SubLineID" Width="80px" />
                    </Columns>
                    <ItemStyle Height="10px" Paddings-Padding="4px"/>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
                    </ButtonStyle>
                </dx:ASPxComboBox>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px" width="75px">
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Part No." 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td width="10px">
                &nbsp;
            </td>
            <td style="padding: 5px 0px 3px 0px" width="150px">
                <dx:ASPxComboBox ID="cbopartid" runat="server" Theme="Office2010Black" TextField="PartName"
                    ClientInstanceName="cbopartid" DropDownStyle="DropDownList" ValueField="PartID"
                    EnableIncrementalFiltering="True" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" TextFormatString="{0}-{1}" DisplayFormatString="{0}"
                    Width="150px" TabIndex="2" EnableCallbackMode="true" CallbackPageSize="10">
                    <ClientSideEvents SelectedIndexChanged="SelectPartID" BeginCallback="function(s, e) { OnBeginCallbackPartID(); }" EndCallback="function(s, e) { OnEndCallbackPartID(); } " Init="function(s,e){cbopartid.PerformCallback(); cbopartid.SetValue('ALL');}"/>
                    <Columns>
                        <dx:ListBoxColumn Caption="Part ID" FieldName="PartID" Width="110px" />
                        <dx:ListBoxColumn Caption="Part Name" FieldName="PartName" Width="250px" />
                    </Columns>
                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
                    </ButtonStyle>
                </dx:ASPxComboBox>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px" width="85px">
                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Rev. No." 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 5px 0px 3px 0px" width="100px">
                <dx:ASPxComboBox ID="cborevno" runat="server" Theme="Office2010Black" TextField="RevNo"
                    ClientInstanceName="cborevno" DropDownStyle="DropDownList" ValueField="RevNo"
                    EnableIncrementalFiltering="True" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="StartsWith" TextFormatString="{0}-{1}-{2}" DisplayFormatString="{0}"
                    Width="100px" TabIndex="4">
                    <ClientSideEvents SelectedIndexChanged="SelectRevNo" Init="function(s,e){cborevno.PerformCallback('ALL' + '|' + 'ALL' + '|' + 'ALL' + '|' + 'ALL'); cborevno.SetValue('Latest Rev');}"/> <%--KeyDown="Key" KeyPress="Key" KeyUp="Key"--%>
                    <Columns>
                        <dx:ListBoxColumn Caption="Rev No" FieldName="RevNo" Width="70px"/>
                        <dx:ListBoxColumn Caption="Rev Date" FieldName="RevDate" Width="70px"/>
                        <dx:ListBoxColumn Caption="Rev History" FieldName="RevHistory" Width="200px"/>
                    </Columns>
                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                    <ButtonStyle Width="5px" Paddings-Padding="4px" ></ButtonStyle>
                </dx:ASPxComboBox>
            </td>
            <td style="padding: 5px 0px 3px 0px" width="10px">
                &nbsp;</td>
            <td width="90px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="padding: 3px 0px 5px 0px" width="65px">
                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="To" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td style="padding: 3px 0px 5px 0px" width="10px">
                &nbsp;</td>
            <td style="padding: 3px 0px 5px 0px" width="65px">
                <dx:ASPxDateEdit ID="dtend" runat="server" Theme="Office2010Black" 
                    Width="100px" AutoPostBack="false"
                        ClientInstanceName="dtend" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="7">
                        <ClientSideEvents Init="function(s, e){var today = new Date(); dtend.SetDate(today);}" />
                        <CalendarProperties>
                            <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="10px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="10px"></ButtonStyle>
                        </CalendarProperties>
                        <ButtonStyle Width="5px" Paddings-Padding="4px" ></ButtonStyle>
                    </dx:ASPxDateEdit>
                    
            </td>
            <td style="padding: 3px 0px 5px 0px" width="10px">
                &nbsp;</td>
            <td style="padding: 3px 0px 5px 0px" width="70px">
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Sub Line No." 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 0px 0px 3px 0px" width="75px">
               <dx:ASPxComboBox ID="cbosublineid" runat="server" Theme="Office2010Black" TextField="SubLineID"
                    ClientInstanceName="cbosublineid" DropDownStyle="DropDownList" ValueField="SubLineID"
                    EnableIncrementalFiltering="True" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" TextFormatString="{0}" DisplayFormatString="{0}"
                    Width="75px" TabIndex="1">
                    <ClientSideEvents SelectedIndexChanged="SelectSubLineID" Init="function(s,e){cbosublineid.PerformCallback('ALL'); cbosublineid.SetValue('ALL');}"/>
                    <Columns>
                        <dx:ListBoxColumn Caption="Sub Line ID" FieldName="SubLineID" Width="70px" />
                    </Columns>
                    <ItemStyle Height="10px" Paddings-Padding="4px"/>
                    <ButtonStyle Paddings-Padding="4px" Width="5px">
                    </ButtonStyle>
                </dx:ASPxComboBox>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td width="50px" style="padding: 3px 0px 5px 0px">
                &nbsp;</td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 0px 0px 3px 0px" width="95px">
                &nbsp;</td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 3px 0px 5px 0px" width="75px">
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Part Name" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td width="10px">
                &nbsp;
            </td>
            <td style="padding: 0px 0px 3px 0px" width="150px">
                <dx:ASPxTextBox ID="txtpartname" runat="server" BackColor="WhiteSmoke" 
                    ClientInstanceName="txtpartname" EnableTheming="True" Font-Names="Segoe UI" 
                    Font-Size="9pt" Height="25px" HorizontalAlign="Left" 
                    Theme="Office2010Black" Width="150px" ReadOnly="True" TabIndex="3">
                    <ClientSideEvents Init="function(s,e){txtpartname.SetValue('ALL');}" />
                </dx:ASPxTextBox>
                    
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 3px 0px 5px 0px" width="85px">
                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Approval Status" 
                    Font-Names="Segoe UI" Font-Size="9pt">
                </dx:ASPxLabel>
            </td>
            <td width="10px">
                &nbsp;</td>
            <td style="padding: 0px 0px 3px 0px" width="100px">
                <dx:ASPxComboBox ID="cboapprovalstatus" runat="server" Theme="Office2010Black" TextField="Status"
                    ClientInstanceName="cboapprovalstatus" DropDownStyle="DropDown" ValueField="Status"
                    EnableIncrementalFiltering="True" Font-Names="Segoe UI" Font-Size="9pt" Height="25px" 
                    IncrementalFilteringMode="Contains" TextFormatString="{0}" DisplayFormatString="{0}"
                    Width="100px" TabIndex="4">
                    <ClientSideEvents Init="function(s,e){cboapprovalstatus.PerformCallback(); cboapprovalstatus.SetValue('Unapproved');}" />
                    <Columns>
                        <dx:ListBoxColumn Caption="Status" FieldName="Status" Width="70px"/>
                    </Columns>
                    <ItemStyle Height="10px" Paddings-Padding="4px" />
                    <ButtonStyle Width="5px" Paddings-Padding="4px" ></ButtonStyle>
                </dx:ASPxComboBox>
                    
            </td>
            <td style="padding: 0px 0px 3px 0px" width="10px">
                &nbsp;</td>
            <td width="90px">
                <dx:ASPxButton ID="btnRefresh" runat="server" Text="Refresh" UseSubmitBehavior="false"
                    Font-Names="Segoe UI" Font-Size="9pt" Width="90px" Height="25px" AutoPostBack="false"
                    ClientInstanceName="btnRefresh" Theme="Default" TabIndex="12">
                    <ClientSideEvents Click="ClickRefresh" Init="ClickRefresh"/>
                    <Paddings Padding="2px" />
                </dx:ASPxButton>
            </td>
        </tr>
        </table>
</div>

<div style="padding: 2px 5px 5px 5px">
        <dx:ASPxGridView ID="GridMenu" runat="server" AutoGenerateColumns="False" ClientInstanceName="GridMenu"
            EnableTheming="True" KeyFieldName="MachineNo;PartID;RevNo;LineID;SubLineID" Theme="Office2010Black" 
            OnStartRowEditing="GridMenu_StartRowEditing" OnRowValidating="GridMenu_RowValidating"
            OnRowInserting="GridMenu_RowInserting" OnRowDeleting="GridMenu_RowDeleting" 
            OnAfterPerformCallback="GridMenu_AfterPerformCallback" Width="100%" 
            Font-Names="Segoe UI" Font-Size="9pt">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <Columns>
                <dx:GridViewCommandColumn ShowSelectCheckbox="True" 
                    ShowClearFilterButton="true" VisibleIndex="1" SelectAllCheckboxMode="Page" 
                    Width="30px" FixedStyle="Left" />

                <dx:GridViewDataTextColumn Caption="Line No" FieldName="LineID" 
                    VisibleIndex="2" Width="60px" FixedStyle="Left">
                    <PropertiesTextEdit ClientInstanceName="LineID">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Sub Line No" FieldName="SubLineID" 
                    VisibleIndex="3" Width="60px" FixedStyle="Left">
                    <PropertiesTextEdit ClientInstanceName="SubLineID">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                 <dx:GridViewDataTextColumn Caption="Machine No" FieldName="MachineNo" 
                    VisibleIndex="4" Width="60px" FixedStyle="Left">
                    <PropertiesTextEdit ClientInstanceName="ProcessID">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Part No" FieldName="PartID" 
                    VisibleIndex="5" Width="110px" FixedStyle="Left">
                    <PropertiesTextEdit ClientInstanceName="PartID">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Part Name" FieldName="PartName" 
                    VisibleIndex="6" Width="145px" FixedStyle="Left">
                    <PropertiesTextEdit ClientInstanceName="PartName">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewDataTextColumn>
                
                <dx:GridViewDataTextColumn Caption="Rev No" FieldName="RevNo" VisibleIndex="7" 
                    Width="50px" FixedStyle="Left">
                    <PropertiesTextEdit ClientInstanceName="RevNo">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Rev Date" FieldName="RevDate" 
                    VisibleIndex="8" Width="90px">
                    <PropertiesTextEdit DisplayFormatString="dd-MMM-yyyy" ClientInstanceName="RevDate">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Revision History" FieldName="RevHistory" 
                    VisibleIndex="9" Width="200px">
                    <PropertiesTextEdit ClientInstanceName="RevHistory"></PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Prepared By" FieldName="PreparedBy" 
                    VisibleIndex="10" Width="75px">
                    <PropertiesTextEdit ClientInstanceName="PreparedBy"></PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewBandColumn Caption="Approval" VisibleIndex="13">
                    <Columns>
                        <dx:GridViewBandColumn Caption="Prod. Sec. Head" VisibleIndex="0">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Date" FieldName="ApprovalDate" 
                                    VisibleIndex="0" Width="85px">
                                    <PropertiesTextEdit DisplayFormatString="dd-MMM-yyyy" ClientInstanceName="ApprovalDate">
                                    </PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="PIC" FieldName="ApprovalPIC" 
                                    VisibleIndex="1" Width="85px">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </dx:GridViewBandColumn>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewBandColumn>
                
               
               
                <dx:GridViewDataTextColumn Caption="Remark" FieldName="Remark" VisibleIndex="14">
                </dx:GridViewDataTextColumn>
                
            </Columns>
        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
        <SettingsPager Mode="ShowPager" PageSize="20" AlwaysShowPager="true" PageSizeItemSettings-Visible="true">
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
        </SettingsPager>
        <Settings HorizontalScrollBarMode="Auto" VerticalScrollableHeight="250" 
            VerticalScrollBarMode="Auto" />
        <SettingsText ConfirmDelete="Are you sure want to delete ?" />
        <SettingsPopup>
            <EditForm HorizontalAlign="WindowCenter" Modal="false" 
                VerticalAlign="WindowCenter" Width="320" />
        </SettingsPopup>
        <Styles EditFormColumnCaption-Paddings-PaddingLeft="10px" 
            EditFormColumnCaption-Paddings-PaddingRight="10px" 
            Header-Paddings-Padding="5px">
            <Header>
                <Paddings Padding="2px" />
            </Header>
            <EditFormColumnCaption Font-Names="Segoe UI" Font-Size="9pt">
                <Paddings PaddingBottom="5px" PaddingLeft="15px" PaddingRight="15px" 
                    PaddingTop="5px" />
            </EditFormColumnCaption>
            <CommandColumnItem ForeColor="SteelBlue">
            </CommandColumnItem>
        </Styles>
        <Templates>
            <EditForm>
                <div style="padding: 15px 15px 15px 15px">
                    <dx:ContentControl ID="ContentControl1" runat="server">
                        <dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server" 
                            ReplacementType="EditFormEditors" />
                    </dx:ContentControl>
                </div>
                <div style="text-align: left; padding: 5px 5px 5px 15px">
                    <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" runat="server" 
                        ReplacementType="EditFormUpdateButton" />
                    <dx:ASPxGridViewTemplateReplacement ID="CancelButton" runat="server" 
                        ReplacementType="EditFormCancelButton" />
                </div>
            </EditForm>
        </Templates>
    </dx:ASPxGridView>
</div>

<div style="padding: 0px 5px 5px 5px">
    <table class="auto-style3" style="width: 100%">
        <tr >
            <td style="width:250px; padding:5px 0px 5px 0px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style=" padding: 5px 0px 5px 0px; width:250px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style=" padding: 5px 0px 5px 0px; width:250px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style=" padding: 5px 0px 5px 0px; width:250px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style=" padding: 5px 0px 5px 0px; width:250px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="width:10px; padding:5px 0px 5px 0px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style=" padding: 5px 0px 5px 0px; width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px; width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td class="style1" 
                style="padding: 5px 0px 5px 0px; border-top: 1px solid silver; width: 10px;">
                &nbsp;</td>
            <td class="style1" 
                style="padding: 5px 0px 5px 0px; border-top: 1px solid silver; width: 10px;">
                &nbsp;</td>
            <td class="style1" 
                style="padding: 5px 0px 5px 0px; border-top: 1px solid silver; width: 10px;">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px; width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px; width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px; width:10px; border-top: 1px solid silver">
                <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" UseSubmitBehavior="false"
                    Font-Names="Segoe UI" Font-Size="9pt" Width="90px" Height="25px" AutoPostBack="false"
                    ClientInstanceName="btnClear" Theme="Default" TabIndex="13">
                    <ClientSideEvents Click="BtnClearClick"/>
                    <Paddings Padding="2px" />
                </dx:ASPxButton>
            </td>
            <td style="width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px; width:10px; border-top: 1px solid silver">
                <dx:ASPxButton ID="btnExcel" runat="server" Text="Excel" UseSubmitBehavior="false"
                    Font-Names="Segoe UI" Font-Size="9pt" Width="90px" Height="25px" AutoPostBack="false"
                    ClientInstanceName="btnExcel" Theme="Default" TabIndex="14" 
                    ClientEnabled="False">
                    <ClientSideEvents Click="function(s, e) {GridMenu.PerformCallback('Excel');}" />
                    <Paddings Padding="2px" />
                </dx:ASPxButton>
            </td>
            <td style="width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="padding: 5px 0px 5px 0px; width:10px; border-top: 1px solid silver">
                <dx:ASPxButton ID="btnViewQCS" runat="server" AutoPostBack="false" 
                    ClientInstanceName="btnViewQCS" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="View CICS Master" Theme="Default" UseSubmitBehavior="false" 
                    Width="90px" TabIndex="15" ClientEnabled="true">
                    <ClientSideEvents Click="ViewTCCS" />
                    <Paddings Padding="2px" />
                </dx:ASPxButton>
            </td>
            <td style="width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="width:10px; border-top: 1px solid silver">
                &nbsp;</td>
            <td style="width: 10px; border-top: 1px solid silver">
            </td>
            <td style="width:10px; padding:5px 0px 5px 0px; border-top: 1px solid silver">
                <dx:ASPxButton ID="btnApprove" runat="server" AutoPostBack="false" 
                    ClientInstanceName="btnApprove" Font-Names="Segoe UI" Font-Size="9pt" 
                    Height="25px" Text="Approve" Theme="Default" UseSubmitBehavior="false" 
                    Width="90px" TabIndex="16" ClientEnabled="False">
                    <ClientSideEvents Click="BtnApproveClick"/>
                    <Paddings Padding="2px" />
                </dx:ASPxButton>
            </td>
        </tr>
        </table>
</div>
</asp:Content>
