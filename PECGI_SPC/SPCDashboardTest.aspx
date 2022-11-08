<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SPCDashboardTest.aspx.vb" Inherits="PECGI_SPC.SPCDashboardTest"  %>

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
    <%--<link href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet" />--%>

    <style>
        /*.list-group{
            max-height: 27%;
            margin-bottom: 10px;
            overflow:scroll;
            -webkit-overflow-scrolling: touch;
        }
        .header-fixed {
            max-width: inherit;
            width: 100%;
            position:fixed;
        }*/
        /*table ,tr td{
            border:1px solid red
        }*/
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
        /*thead {
            width: calc( 100% - 1em )
        }*/
        /*table {
            width:400px;
        }*/
    </style>

</head>
<body>

    <form id="form1" runat="server">

    <nav class="navbar" style="background-color:#4584e0">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">
                <img src="img/logo-panasonic.svg" alt="" width="100" height="40" class="d-inline-block align-text-top">
            </a>
            <div class="text-center">
                <%--<a class="navbar-brand" href="Main.aspx">SPC MONITORING</a>--%>
            </div>
            <div class="text-center" style="background-color: black; color: white">

                <%--<dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Date :" CssClass="text" />
                &nbsp
                <dx:ASPxLabel ID="lblDateNow" ClientInstanceName="lblOK" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />

                <br />

                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Time :" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                &nbsp
                <label id="lblTimeNow"></label>--%>

            </div>
        </div>
    </nav>

    <br />

    <asp:ScriptManager ID="ScriptManagerNGResult" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel runat="server" ID="PanelNGResult">
        <ContentTemplate>

            <asp:Timer runat="server" ID="TimerNGResult" Interval="5000" OnTick="TimerNGResult_Tick" />

                <div style="margin-left: 30px; margin-right: 30px">

                    <!-- NG Result -->
                    <div style="height: 10vh">

                        <%--<nav class="navbar" style="background-color:rgb(69, 132, 224);color: white;">
                            <div class="container-fluid">
                                <div class="container-fluid text-center">
                                    <asp:Label runat="server" ID="lblNGInput" Text="NG RESULT" Font-Bold="true"/>
                                </div>
                            </div>
                        </nav>--%>
                        <span class="badge" style="background-color:rgb(69, 132, 224);color: white; width: 100%">
                            <center>NG RESULT</center>
                        </span>

                        <div class="table-responsive">

                            <asp:Repeater runat="server" ID="rptNGInput">
                                <HeaderTemplate>
                                    <table id="tblNG" class="table table-bordered table-responsive">
                                        <thead>
                                            <tr>
                                                <th scope="col" style="width: 100px">Type</th>
                                                <th scope="col" style="width: 200px">Machine Process</th>
                                                <th scope="col" style="width: 250px">Item Check</th>
                                                <th scope="col" style="width: 100px">Date</th>
                                                <th scope="col" style="width: 50px">Shift</th>
                                                <th scope="col" style="width: 50px">Seq</th>
                                                <th scope="col" style="width: 100px">USL</th>
                                                <th scope="col" style="width: 100px">LSL</th>
                                                <th scope="col" style="width: 100px">UCL</th>
                                                <th scope="col" style="width: 100px">LCL</th>
                                                <th scope="col" style="width: 100px">Min</th>
                                                <th scope="col" style="width: 100px">Max</th>
                                                <th scope="col" style="width: 100px">Average</th>
                                                <th scope="col" style="width: 150px">Operator</th>
                                                <th scope="col" style="width: 120px">MK</th>
                                                <th scope="col" style="width: 135px">QC</th>
                                            </tr>
                                        </thead>
                                        <tbody>
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
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblAverage" runat="server" Text='<%# Eval("Average") %>' />
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
                    <div class="col-12" style="height: 20vh; margin-top: 15vh">
                        
                        
                        <span class="badge" style="background-color:rgb(69, 132, 224);color: white; width: 100%">
                            <center>DELAY INPUT</center>
                        </span>

                        <div class="table-responsive">

                            <asp:Repeater runat="server" ID="rptDdelayInput" >
                                <HeaderTemplate>
                                    <table id="tblDelayInput" class="table table-bordered table-responsive" style="width: 100%;">
                                        <thead>
                                            <tr>
                                                <%--<th scope="col">Action</th>--%>
                                                <th scope="col" style="width: 200px">Machine Process</th>
                                                <th scope="col" style="width: 250px">Item Check</th>
                                                <th scope="col" style="width: 100px">Date</th>
                                                <th scope="col" style="width: 50px">Shift</th>
                                                <th scope="col" style="width: 50px">Seq</th>
                                                <th scope="col" style="width: 200px">Schedule Start</th>
                                                <th scope="col" style="width: 200px">Schedule End</th>
                                                <th scope="col" style="width: 200px">Delay</th>
                                                <%--<th scope="col">#</th>--%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr>
                                            <%--<td>
                                                <asp:Label runat="server" Text="edit" />
                                            </td>--%>
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
                                            <td style="width: 200px">
                                                <asp:Label ID="lblScheduleStart" runat="server" Text='<%# Eval("StartTime") %>' />
                                            </td>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblScheduleEnd" runat="server" Text='<%# Eval("EndTime") %>' />
                                            </td>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblDelay" runat="server" Text='<%# Eval("DelayHeader") %>' />
                                            </td>
                                            <%--<td>
                                                <asp:Label runat="server" Text="send email" />
                                            </td>--%>
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
                    <div class="col-12" style="height:20vh; margin-top: 15vh">
                                                
                        <span class="badge" style="background-color:rgb(69, 132, 224);color: white; width: 100%">
                            <center>DELAY VERIFICATION</center>
                        </span>

                        <div class="table-responsive">

                            <asp:Repeater runat="server" ID="rptDelayVerification">
                                <HeaderTemplate>
                                    <table id="tblDelayVerification" class="table table-bordered table-responsive" style="width:100%">
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
                                                <th scope="col" style="width: 100px">Verif Time</th>
                                                <th scope="col" style="width: 300px">Delay Verif</th>
                                            </tr>
                                        </thead>
                                        <tbody>
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
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblAverage" runat="server" Text='<%# Eval("Average") %>' />
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
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblVerifTime" runat="server" Text='<%# Eval("VerifTime") %>' />
                                                </td>
                                                <td style="width: 300px">
                                                    <asp:Label ID="lblDelayVerif" runat="server" Text='<%# Eval("DelayHeader") %>' />
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

                    <%--<div class="panel panel-primary" id="result_panel">
                        <div class="panel-body">
                            
                                        <nav class="navbar" style="background-color:rgb(69, 132, 224);color: white">
                                            <div class="container-fluid">
                                                <table>
                                                    <tr>
                                                        <td style="color: white;">
                                                            <asp:Label runat="server" ID="lblNGResult" Text="NG RESULT" Font-Bold="true"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </nav>

                            <ul class="list-group">
                                <li class="list-group-item">

                                    

                                </li>
                            </ul>
                            
                                        <nav class="navbar" style="background-color:rgb(69, 132, 224);color: white;" id="NavbarDelayInput">
                                            <div class="container-fluid">
                                                <table>
                                                    <tr>
                                                        <td style="color: white;">
                                                            <asp:Label runat="server" ID="lblDelayInput" Text="DELAY INPUT" Font-Bold="true"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </nav>
                                        
                                        <table class="display table table-bordered table-responsive " style="width:100%; margin-bottom: -2px">
                                                        <thead style="background-color: rgb(69, 132, 224); color: white;">
                                                            <tr>
                                                                <th scope="col" style="max-width: 200px">Machine Process</th>
                                                                <th scope="col" style="max-width: 200px">Item Check</th>
                                                                <th scope="col" style="max-width: 50px">Date</th>
                                                                <th scope="col" style="max-width: 50px">Shift</th>
                                                                <th scope="col" style="max-width: 50px">Seq</th>
                                                                <th scope="col" style="max-width: 50px">Schedule Start</th>
                                                                <th scope="col" style="max-width: 50px">Schedule End</th>
                                                                <th scope="col" style="max-width: 50px">Delay</th>
                                                            </tr>
                                                        </thead>
                                            </table>


                                        <nav class="navbar" style="background-color:rgb(69, 132, 224);color: white">
                                            <div class="container-fluid">
                                                <table>
                                                    <tr>
                                                        <td style="color: white;">
                                                            <asp:Label runat="server" ID="lblDelayVerification" Text="DELAY VERIFICATION" Font-Bold="true"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </nav>

                            <ul class="list-group">
                                <li class="list-group-item">

                                    

                                </li>
                            </ul>
                        </div>
                    </div>--%>

                </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TimerNGResult" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>

        <script>
            $(document).ready(function () {
                $('#tblDelayInput').DataTable();
            });
            window.onload = function () {

                setTimer1 = setInterval(function () {
                    var today = new Date();
                    document.getElementById('lblTimeNow').innerHTML = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
                }, 1000, (1));


                //var oTable = $('#tblDelayInput').DataTable({
                //    pagingType: 'full_numbers',
                //    bFilter: false,
                //    bInfo: false,
                //    lengthChange: false,
                //    'rowCallback': function (row, data, index) {
                //        if (data[7] > 1) {
                //            $(row).find('td:eq(7)').css('color', 'red');
                //        }
                //        if (data[8] > 1) {
                //            $(row).find('td:eq(8)').css('color', 'red');
                //        }
                //    }
                //});
            }
        </script>
    </form>

</body>
</html>
