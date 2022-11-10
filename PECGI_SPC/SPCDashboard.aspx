<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SPCDashboard.aspx.vb" Inherits="PECGI_SPC.SPCDashboard"  %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ MasterType VirtualPath="~/Site.Master" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>SPC Dashboard</title>
    
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
        tbody {
            display:block;
            height:20vh;
            overflow:auto;
        }
        thead, tbody tr {
            display:table;
            width:100%;
            table-layout:fixed;
        }
        thead {
            width: calc( 100% - 1em )
        }
        table {
            width:400px;
        }
        .tableNav {
            height: 10px
        }
        th {background-color:rgb(69, 132, 224);color: white;}
        
        @media screen and (min-width: 800px) and (max-width: 1366px) {
            .navTable {width: 142%}
            .height-table {height : 85px}
            .paragraphNavTitle {margin-left: -100%}
            .paragraphNavTitle2 {margin-left: -85%}
            /*.margin-div1 {height: 100px;}
            .margin-div2 {height: 100px; margin-top: 15%}
            .margin-div3 {height: 100px; margin-top: 15%}*/
        }

        /* if the browser window is at least 1000px-s wide: */
        @media screen and (min-width: 1900px) {
            /*.margin-div2 {height: 25vh; margin-top: 15vh}
            .margin-div3 {height:20vh; margin-top: 5vh}*/
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">
        
    <%--<nav class="navbar" style="background-color:#4584e0">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-4">
                <img src="img/logo-panasonic.svg" alt="" width="100" height="50" class="d-inline-block align-text-top" style="">
                </div>
                <div class="col-lg-4">
                    <center>
                        <h1><b>SPC MONITORING</b></h1>
                    </center>
                </div>
                <div class="col-lg-4" style="color: white">
                    <div class="col-lg-8"></div>
                    <div class="col-lg-4">
                        <p style="text-align : left;color: black">Date : <dx:ASPxLabel ID="lblDateNow" ClientInstanceName="lblOK" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                            <br /> Time : <label id="lblTimeNow"></label> </p>
                    </div>

                </div>
            </div>
        </div>
    </nav>--%>

            <header>
                <table style="width:100%; background-color:#4584e0; " >
                    <tbody style="height: 40px; overflow-x: hidden; overflow-y: hidden;">
                        <tr style="height:10px">
                            <td align="center" width="10%"><img src="img/logo-panasonic.svg" alt="" width="100" height="50" class="d-inline-block align-text-top" style="margin-top: -10px;"></td>
                            <td width="80%" align="center">
                                <font style="color:black; font-size:19pt; font-weight:bold; font-family:Segoe UI" ><b><u><label style="margin-top: -10px;">SPC MONITORING</label></u></b></font>
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

                <div style="margin-left: 30px; margin-right: 30px">

                    <div class="row">

                        <!-- NG Result -->
                        <div class="col-lg-12">

                            <div class="table-responsive">
                            
                            <nav class="navTable" style="background-color:rgb(69, 132, 224);color: white;height: 25px;">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-lg-4">
                                        &nbsp;
                                        </div>
                                        <div class="col-lg-4">
                                            <center>
                                                <b><p class="paragraphNavTitle">NG RESULT ( <asp:Label runat="server" ID="lblCountNGresult" Text="0" /> )</p></b>
                                            </center>
                                        </div>
                                        <div class="col-lg-4" style="color: white">
                                            &nbsp;
                                        </div>
                                    </div>
                                </div>
                            </nav>

                                <asp:Repeater runat="server" ID="rptNGInput" OnItemDataBound="rptNGInput_OnItemDataBound">
                                    <HeaderTemplate>
                                        <table id="tblNG" class="table table-bordered table-responsive" style=" font-size: 13px">
                                            <thead>
                                                <tr>
                                                    <th style="width: 100px">Type</th>
                                                    <th style="width: 200px">Machine Process</th>
                                                    <th style="width: 250px">Item Check</th>
                                                    <th style="width: 100px">Date</th>
                                                    <th style="width: 50px">Shift</th>
                                                    <th style="width: 50px">Seq</th>
                                                    <th style="width: 100px">USL</th>
                                                    <th style="width: 100px">LSL</th>
                                                    <th style="width: 100px">UCL</th>
                                                    <th style="width: 100px">LCL</th>
                                                    <th style="width: 100px">Min</th>
                                                    <th style="width: 100px">Max</th>
                                                    <th style="width: 100px">Average</th>
                                                    <th style="width: 150px">Operator</th>
                                                    <th style="width: 120px">MK</th>
                                                    <th style="width: 135px">QC</th>
                                                </tr>
                                            </thead>
                                            <tbody class="height-table">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                                <tr>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("ItemTypeName") %>' />
                                                    </td>
                                                    <td style="width: 200px">
                                                        <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                                    </td>
                                                    <td style="width: 250px">
                                                        <asp:Label ID="lblItemCheck" runat="server" Text='<%# Eval("ItemCheck") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblShift" runat="server" Text='<%# Eval("ShiftCode") %>' />
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblSeq" runat="server" Text='<%# Eval("SequenceNo") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblUSL" runat="server" Text='<%# Eval("USL") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblLSL" runat="server" Text='<%# Eval("LSL") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblUCL" runat="server" Text='<%# Eval("UCL") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblLCL" runat="server" Text='<%# Eval("LCL") %>' />
                                                    </td>
                                                    <td style="width: 100px" runat="server" id="MinValueNG">
                                                        <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                                    </td>
                                                    <td style="width: 100px" runat="server" id="MaxValueNG">
                                                        <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                                    </td>
                                                    <td style="width: 100px" runat="server" id="AveValueNG">
                                                        <asp:Label ID="lblAve" runat="server" Text='<%# Eval("Average") %>' />
                                                    </td>
                                                    <td style="width: 150px">
                                                        <asp:Label ID="lblOperator" runat="server" Text='<%# Eval("Operator") %>' />
                                                    </td>
                                                    <td style="width: 120px">
                                                        <asp:Label ID="lblMK" runat="server" Text='<%# Eval("MK") %>' />
                                                    </td>
                                                    <td style="width: 135px">
                                                        <asp:Label ID="lblQC" runat="server" Text='<%# Eval("QC") %>' />
                                                    </td>
                                                </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>

                            </div>

                        </div>   

                        <!-- Delay Input -->
                        <div class="col-lg-12">
                        
                            <div class="table-responsive">

                                <nav style="background-color:rgb(69, 132, 224);color: white;height: 25px;width: 100%">
                                    <div class="container-fluid">
                                        <div class="row">
                                            <div class="col-lg-4">
                                            &nbsp;
                                            </div>
                                            <div class="col-lg-4">
                                                <center>
                                                    <b><p>DELAY INPUT ( <asp:Label runat="server" ID="lblCountDelayInput" Text="0" /> )</p></b>
                                                </center>
                                            </div>
                                            <div class="col-lg-4" style="color: white">
                                                &nbsp;
                                            </div>
                                        </div>
                                    </div>
                                </nav>

                                <asp:Repeater runat="server" ID="rptDdelayInput" OnItemDataBound="rptDelayInput_OnItemDataBound">
                                    <HeaderTemplate>
                                        <table id="tblDelayInput" class="table table-bordered table-responsive" style="font-size: 13px;">
                                            <thead >
                                                <tr>
                                                    <%--<th scope="col">Action</th>--%>
                                                    <th style="width: 15%">Machine Process</th>
                                                    <th style="width: 20%">Item Check</th>
                                                    <th>Date</th>
                                                    <th>Shift</th>
                                                    <th>Seq</th>
                                                    <th>Schedule Start</th>
                                                    <th>Schedule End</th>
                                                    <th style="width: 25%">Delay</th>
                                                    <%--<th scope="col">#</th>--%>
                                                </tr>
                                            </thead>
                                            <tbody class="height-table">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                            <tr>
                                                <td style="width: 15%">
                                                    <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                                </td>
                                                <td style="width: 20%">
                                                    <asp:Label ID="lblItemCheck" runat="server" Text='<%# Eval("ItemCheck") %>' />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblShift" runat="server" Text='<%# Eval("ShiftCode") %>' />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSeq" runat="server" Text='<%# Eval("SequenceNo") %>' />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblScheduleStart" runat="server" Text='<%# Eval("StartTime") %>' />
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblScheduleEnd" runat="server" Text='<%# Eval("EndTime") %>' />
                                                </td>
                                                <td style="width: 25%" runat="server" id="DelayInput">
                                                    <asp:Label ID="lblDelay" runat="server" Text='<%# Eval("Delay") %>' />
                                                </td>
                                            </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>

                                </asp:Repeater>
                            </div>

                        </div>

                        <!-- Delay Verification -->
                        <div class="col-lg-12">
                                                
                            <div class="table-responsive">

                                <nav class="navTable" style="background-color:rgb(69, 132, 224);color: white;height: 25px;">
                                    <div class="container-fluid">
                                        <div class="row">
                                            <div class="col-lg-4">
                                            &nbsp;
                                            </div>
                                            <div class="col-lg-4">
                                                <center>
                                                    <b><p class="paragraphNavTitle2">DELAY VERIFICATION ( <asp:Label runat="server" ID="lblCountDelayVerif" Text="0" /> ) </p></b>
                                                </center>
                                            </div>
                                            <div class="col-lg-4" style="color: white">
                                                &nbsp;
                                            </div>
                                        </div>
                                    </div>
                                </nav>

                                <asp:Repeater runat="server" ID="rptDelayVerification" OnItemDataBound="rptDelayVerification_OnItemDataBound">
                                    <HeaderTemplate>
                                        <table id="tblDelayVerification" class="table table-bordered table-responsive" style="width:100%; font-size: 13px">
                                            <thead>
                                                <tr>
                                                    <th scope="col" style="width: 100px">Type</th>
                                                    <th scope="col" style="width: 200px">Machine Process</th>
                                                    <th scope="col" style="width: 250px">Item Check</th>
                                                    <th scope="col" style="width: 150px">Date</th>
                                                    <th scope="col" style="width: 50px">Shift</th>
                                                    <th scope="col" style="width: 50px">Seq</th>
                                                    <th scope="col" style="width: 100px">USL</th>
                                                    <th scope="col" style="width: 100px">LSL</th>
                                                    <th scope="col" style="width: 100px">UCL</th>
                                                    <th scope="col" style="width: 100px">LCL</th>
                                                    <th scope="col" style="width: 100px">Min</th>
                                                    <th scope="col" style="width: 100px">Max</th>
                                                    <th scope="col" style="width: 100px">Average</th>
                                                    <th scope="col" style="width: 80px">Operator</th>
                                                    <th scope="col" style="width: 80px">MK</th>
                                                    <th scope="col" style="width: 80px">QC</th>
                                                    <th scope="col" style="width: 115px">Delay Verif</th>
                                                </tr>
                                            </thead>
                                            <tbody class="height-table">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                                <tr>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("ItemTypeName") %>' />
                                                    </td>
                                                    <td style="width: 200px">
                                                        <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                                    </td>
                                                    <td style="width: 250px">
                                                        <asp:Label ID="lblItemCheck" runat="server" Text='<%# Eval("ItemCheck") %>' />
                                                    </td>
                                                    <td style="width: 150px">
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblShift" runat="server" Text='<%# Eval("ShiftCode") %>' />
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="lblSeq" runat="server" Text='<%# Eval("SequenceNo") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblUSL" runat="server" Text='<%# Eval("USL") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblLSL" runat="server" Text='<%# Eval("LSL") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblUCL" runat="server" Text='<%# Eval("UCL") %>' />
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblLCL" runat="server" Text='<%# Eval("LCL") %>' />
                                                    </td>
                                                    <td style="width: 100px" runat="server" id="MinValueDV">
                                                        <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                                    </td>
                                                    <td style="width: 100px" runat="server" id="MaxValueDV">
                                                        <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                                    </td>
                                                    <td style="width: 100px" runat="server" id="AveValueDV">
                                                        <asp:Label ID="lblAve" runat="server" Text='<%# Eval("Average") %>' />
                                                    </td>
                                                    <td style="width: 80px">
                                                        <asp:Label ID="lblOperator" runat="server" Text='<%# Eval("Operator") %>' />
                                                    </td>
                                                    <td style="width: 80px">
                                                        <asp:Label ID="lblMK" runat="server" Text='<%# Eval("MK") %>' />
                                                    </td>
                                                    <td style="width: 80px">
                                                        <asp:Label ID="lblQC" runat="server" Text='<%# Eval("QC") %>' />
                                                    </td>
                                                    <td style="width: 115px" runat="server" id="DelayVerification">
                                                        <asp:Label ID="lblDelayVerif" runat="server" Text='<%# Eval("DelayVerif") %>' />
                                                    </td>
                                                </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                    
                            </div>

                        </div>

                    </div>

                </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TimerNGResult" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
        <script>
            $(document).ready(function () {

            });
            
        </script>
        <script>
            window.onload = function () {

                setTimer1 = setInterval(function () {
                    var today = new Date();
                    document.getElementById('lblTimeNow').innerHTML = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
                }, 1000, (1));

            }
        </script>
    </form>

</body>
</html>
