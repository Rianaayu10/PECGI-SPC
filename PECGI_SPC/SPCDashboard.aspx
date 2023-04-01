<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SPCDashboard.aspx.vb" Inherits="PECGI_SPC.SPCDashboard"  %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ MasterType VirtualPath="~/Site.Master" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>SPC Dashboard</title>
    <link href="Styles/images/favicon_pecgi.ico" rel="SHORTCUT ICON" type="image/icon" />
    
    <meta charset="utf-8">
    <meta name="description" content="">
    <meta name="author" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">

    <!-- Custom Styles -->
    <link rel="stylesheet" type="text/css" media="screen" href="css/global-circuit-store.css">

    <!-- Basic Styles -->
    <link rel="stylesheet" type="text/css" media="screen" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" media="screen" href="css/font-awesome.min.css">

    <!-- SmartAdmin Styles : Caution! DO NOT change the order -->
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-production-plugins.min.css">
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-production.min.css">
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-skins.min.css">

    <!-- SmartAdmin RTL Support  -->
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-rtl.min.css">

    <!-- Toastr -->
    <script src="Scripts/toastr.js"></script>
    <link rel="Stylesheet" type="text/css" href="content/toastr.css">
    <link rel="Stylesheet" href="content/toastr.min.css">

    <style>
        /*tbody {
            display:block;
            height:20vh;
            overflow:auto;
        }
        thead, tbody tr {
            display:table;
            width:100%;
            table-layout:fixed;
        }*/
        /*thead {
            width: calc( 100% - 1em )
        }*/
        /*table {
            width:400px;
        }
        .tableNav {
            height: 10px
        }*/
        th {background-color:rgb(69, 132, 224);color: white;}
        
        @media screen and (min-width: 800px) and (max-width: 1366px) {
        .tableFixHead          { overflow: auto; height: 20%; }
        .tableFixHead thead th { position: sticky; top: 0; z-index: 1; }
            /*.margin-div1 {height: 100px;}
            .margin-div2 {height: 100px; margin-top: 15%}
            .margin-div3 {height: 100px; margin-top: 15%}*/
        }

        /* if the browser window is at least 1000px-s wide: */
        @media screen and (min-width: 1900px) {
            /*.margin-div2 {height: 25vh; margin-top: 15vh}
            .margin-div3 {height:20vh; margin-top: 5vh}*/
        .tableFixHead          { overflow: auto; height: 25%; }
        .tableFixHead thead th { position: sticky; top: 0; z-index: 1; }
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">

            <header>
                <table style="width:100%; background-color:#4584e0; " >
                    <tbody style="height: 40px; overflow-x: hidden; overflow-y: hidden;">
                        <tr style="height:10px">
                            <td align="center" width="10%"><a href="Main.aspx"><img src="img/logo-panasonic.svg" alt="" width="100" height="50" class="d-inline-block align-text-top" style="margin-top: -10px;"></a></td>
                            <td width="80%" align="center">
                                <font style="color:black; font-size:19pt; font-weight:bold; font-family:Segoe UI" ><u><label style="margin-top: -10px;"><b>SPC MONITORING</b></label></u></font>
                            </td>
                        
                            <td style="padding-right:10px">
                                
                            </td>
                            <td align="right" style="width:10%; background-color:black">
                                <%--<input type="text" id="fdate" name="fdate" disabled="disabled" style="text-align:left; padding-left:50px; width:150px; background-color:transparent; color:white; font-family:'Segoe UI'; font-size:7pt; border:0px; font-weight:900">
                                <input type="text" id="ftime" name="ftime" disabled="disabled" style="text-align:left; padding-bottom:5px; padding-left:50px; width:150px; background-color:transparent; color:white; font-family:'Segoe UI'; font-size:7pt; border:0px; font-weight:900">--%>
                                <small><p style="text-align : left;color: white;margin-top: 5px;margin-left: 5px">Date : <dx:ASPxLabel ID="lblDateNow" ClientInstanceName="lblOK" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                                <br /> Time : <label id="lblTimeNow"></label> </p></small>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </header>

        <br />

    <asp:ScriptManager ID="ScriptManagerNGResult" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel runat="server" ID="PanelNGResult">
        <ContentTemplate>

            <asp:Timer runat="server" ID="TimerNGResult" Interval="60000" OnTick="TimerNGResult_Tick" />
                
                
                <!-- NG Result -->
                <div style="border: 2px solid black; border-collapse: collapse;margin-left: 30px; margin-right: 30px">
                    
                    <div>
                    
                        <table width="100%">
                            <tr>
                                <th>
                                    <center>
                                        <b><small><p class="paragraphNavTitle" style="margin-top: 5px">NG RESULT ( <asp:Label runat="server" ID="lblCountNGresult" Text="0" /> )</p></small></b>
                                    </center>
                                </th>
                            </tr>
                            <%--<tr height="1px">
                                <td style="background-color: transparent"></td>
                            </tr>
                            <tr>
                                <th colspan="10">SPC Verification</th>
                            </tr>--%>
                        </table>

                        

                    </div>

                    <div class="tableFixHead">

                        <table width="100%">
                            <tr height="1px">
                                <td style="background-color: transparent"></td>
                            </tr>
                            <tr>
                                <asp:Repeater runat="server" ID="rptNGInput" OnItemDataBound="rptNGInput_OnItemDataBound">
                                    <HeaderTemplate>
                                        <table width="100%" class="table table-bordered table-responsive" style=" font-size: 13px;">
                                            <thead>
                                                <tr>
                                                    <th colspan="17"><center>SPC Verification</center></th>
                                                    <th colspan="5"><center>FTA Verification</center></th>
                                                </tr>
                                                <tr>
                                                    <th width="5%">Type</th>
                                                    <th width="10%">Machine Process</th>
                                                    <th width="15%">Item Check</th>
                                                    <th width="5%">Date</th>
                                                    <th width="5%">Shift</th>
                                                    <th width="5%">Seq</th>
                                                    <th>USL</th>
                                                    <th>LSL</th>
                                                    <th>UCL</th>
                                                    <th>LCL</th>
                                                    <th>Min</th>
                                                    <th>Max</th>
                                                    <th>Average</th>
                                                    <th>R</th>
                                                    <th>Operator</th>
                                                    <th>MK</th>
                                                    <th>QC</th>
                                                    <th>Current Condition</th>
                                                    <th>Status</th>
                                                    <th>Action</th>
                                                    <th>MK</th>
                                                    <th>QC</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <tr>
                                                    <td width="5%">
                                                        <asp:Label ID="lblTypeNGInput" runat="server" Text='<%# Eval("ItemTypeName") %>' />
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblItemCheck" runat="server" Text='<%# Eval("ItemCheck") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblShift" runat="server" Text='<%# Eval("ShiftCode") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblSeq" runat="server" Text='<%# Eval("SequenceNo") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUSL" runat="server" Text='<%# Eval("USL") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLSL" runat="server" Text='<%# Eval("LSL") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUCL" runat="server" Text='<%# Eval("UCL") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLCL" runat="server" Text='<%# Eval("LCL") %>' />
                                                    </td>
                                                    <td runat="server" id="MinValueNG">
                                                        <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                                    </td>
                                                    <td runat="server" id="MaxValueNG">
                                                        <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                                    </td>
                                                    <td runat="server" id="AveValueNG">
                                                        <asp:Label ID="lblAve" runat="server" Text='<%# Eval("Average") %>' />
                                                    </td>
                                                    <td runat="server" id="RValue">
                                                        <asp:Label ID="lblRValue" runat="server" Text='<%# Eval("RValueSPCDashboard") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOperator" runat="server" Text='<%# Eval("Operator") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMK" runat="server" Text='<%# Eval("MK") %>' />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblQC" runat="server" Text='<%# Eval("QC") %>' />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblCurrentConditionFTA" runat="server" Text='<%# Eval("LineName") %>' />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblStatusFTA" runat="server" Text='<%# Eval("StatusFTA") %>' />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblActionFTA" runat="server" Text='<%# Eval("ActionFTA") %>' />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblMKFTA" runat="server" Text='<%# Eval("MKFTA") %>' />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblQCFTA" runat="server" Text='<%# Eval("QCFTA") %>' />
                                                    </td>
                                                </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>

                            </tr>
                        </table>

                    </div>

                </div>
                
                <br />

                <!-- Delay Input -->
                <div style="border: 2px solid black; border-collapse: collapse;margin-left: 30px; margin-right: 30px">

                    <div>
                    
                        <table width="100%">
                            <tr>
                                <th>
                                    <center>
                                        <b><small><p class="paragraphNavTitle" style="margin-top: 5px">DELAY INPUT ( <asp:Label runat="server" ID="lblCountDelayInput" Text="0" /> )</p></small></b>
                                    </center>
                                </th>
                            </tr>
                        </table>

                    </div>
                    
                    <div class="tableFixHead">

                        <table width="100%">
                            <tr height="1px">
                                <td style="background-color: transparent"></td>
                            </tr>
                            <tr>
                            
                                <asp:Repeater runat="server" ID="rptDdelayInput" OnItemDataBound="rptDelayInput_OnItemDataBound">
                                    <HeaderTemplate>
                                        <table width="100%" class="table table-bordered table-responsive" style=" font-size: 13px;">
                                            <thead>
                                                <tr>
                                                    <th width="5%">Type</th>
                                                    <th width="10%">Machine Process</th>
                                                    <th width="15%">Item Check</th>
                                                    <th width="5%">Date</th>
                                                    <th width="5%">Shift</th>
                                                    <th width="5%">Seq</th>
                                                    <th width="10%">Schedule Start</th>
                                                    <th width="10%">Schedule End</th>
                                                    <th>Delay</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <tr>
                                                    <td width="5%">
                                                        <asp:Label ID="lblTypeDelayInput" runat="server" Text='<%# Eval("TypeHeader") %>' />
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblItemCheck" runat="server" Text='<%# Eval("ItemCheck") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblShift" runat="server" Text='<%# Eval("ShiftCode") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblSeq" runat="server" Text='<%# Eval("SequenceNo") %>' />
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblScheduleStart" runat="server" Text='<%# Eval("StartTime") %>' />
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblScheduleEnd" runat="server" Text='<%# Eval("EndTime") %>' />
                                                    </td>
                                                    <td runat="server" id="DelayInput">
                                                        <asp:Label ID="lblDelay" runat="server" Text='<%# Eval("Delay") %>' />
                                                    </td>
                                                </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>

                            </tr>
                        </table>

                    </div>
                </div>
                
                <br />

                <!-- Delay Verification -->
                <div style="border: 2px solid black; border-collapse: collapse;margin-left: 30px; margin-right: 30px">

                    <div>
                    
                        <table width="100%">
                            <tr>
                                <th>
                                    <center>
                                        <b><small><p class="paragraphNavTitle" style="margin-top: 5px">DELAY VERIFICATION ( <asp:Label runat="server" ID="lblCountDelayVerif" Text="0" /> ) </p></small></b>
                                    </center>
                                </th>
                            </tr>
                        </table>

                    </div>
                    
                    <div class="tableFixHead">

                        <table width="100%">
                            <tr height="1px">
                                <td style="background-color: transparent"></td>
                            </tr>
                            <tr>
                            
                                <asp:Repeater runat="server" ID="rptDelayVerification" OnItemDataBound="rptDelayVerification_OnItemDataBound">
                                    <HeaderTemplate>
                                        <table width="100%" class="table table-bordered table-responsive" style=" font-size: 13px;">
                                            <thead>
                                                <tr>
                                                    <th width="5%">Type</th>
                                                    <th width="10%">Machine Process</th>
                                                    <th width="15%">Item Check</th>
                                                    <th width="5%">Date</th>
                                                    <th width="5%">Shift</th>
                                                    <th width="5%">Seq</th>
                                                    <th>USL</th>
                                                    <th>LSL</th>
                                                    <th>UCL</th>
                                                    <th>LCL</th>
                                                    <th>Min</th>
                                                    <th>Max</th>
                                                    <th>Average</th>
                                                    <th>R</th>
                                                    <th>Operator</th>
                                                    <th>MK</th>
                                                    <th>QC</th>
                                                    <th>Delay Verification</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <tr>
                                                    <td width="5%">
                                                        <asp:Label ID="lblTypeDelayVerification" runat="server" Text='<%# Eval("ItemTypeName") %>' />
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblItemCheck" runat="server" Text='<%# Eval("ItemCheck") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblShift" runat="server" Text='<%# Eval("ShiftCode") %>' />
                                                    </td>
                                                    <td width="5%">
                                                        <asp:Label ID="lblSeq" runat="server" Text='<%# Eval("SequenceNo") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUSL" runat="server" Text='<%# Eval("USL") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLSL" runat="server" Text='<%# Eval("LSL") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUCL" runat="server" Text='<%# Eval("UCL") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLCL" runat="server" Text='<%# Eval("LCL") %>' />
                                                    </td>
                                                    <td runat="server" id="MinValueDV">
                                                        <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                                    </td>
                                                    <td runat="server" id="MaxValueDV">
                                                        <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                                    </td>
                                                    <td runat="server" id="AveValueDV">
                                                        <asp:Label ID="lblAve" runat="server" Text='<%# Eval("Average") %>' />
                                                    </td>
                                                    <td runat="server" id="RValue">
                                                        <asp:Label ID="lblRValue" runat="server" Text='<%# Eval("RValueSPCDashboard") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOperator" runat="server" Text='<%# Eval("Operator") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMK" runat="server" Text='<%# Eval("MK") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblQC" runat="server" Text='<%# Eval("QC") %>' />
                                                    </td>
                                                    <td runat="server" id="DelayVerification">
                                                        <asp:Label ID="lblDelayVerif" runat="server" Text='<%# Eval("DelayVerif") %>' />
                                                    </td>
                                                </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>

                            </tr>
                        </table>

                    </div>

                </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TimerNGResult" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
        <script>
            window.onload = function () {

                setTimer1 = setInterval(function () {
                    var today = new Date();
                    //document.getElementById('lblTimeNow').innerHTML = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
                    document.getElementById('lblTimeNow').innerHTML = ("0" + today.getHours()).slice(-2) + ":" + ("0" + today.getMinutes()).slice(-2) + ":" + ("0" + today.getSeconds()).slice(-2);
                }, 1000, (1));

            }
        </script>
    </form>

</body>
</html>
