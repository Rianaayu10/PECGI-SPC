<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SPCDashboard.aspx.vb" Inherits="PECGI_SPC.SPCDashboard"  %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ MasterType VirtualPath="~/Site.Master" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>SPC Dashboard</title>
    
    <%--Css--%>
    <link href="CustomFile/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Zenh87qX5JnK2Jl0vWa8Ck2rdkQ2Bzep5IDxbcnCeuOxjzrPF/et3URy9Bv1WTRi" crossorigin="anonymous">
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css">

</head>
<body>

    <form id="form1" runat="server">

    <nav class="navbar" style="background-color:#4584e0">
        <div class="container-fluid">
            <a class="navbar-brand" href="Main.aspx">
                <img src="https://getbootstrap.com/docs/5.2/assets/brand/bootstrap-logo.svg" alt="Logo" width="30" height="24" class="d-inline-block align-text-top"> SPC Dashboard
            </a>
        </div>
    </nav>

    <div style="margin-top: 50px; margin-left: 30px; margin-right: 30px">

        <div class="row">

            <!-- NG Result -->
            <div class="col-12" style="height: 33%">

                <div class="table-responsive">

                    <asp:Repeater runat="server" ID="rptNGInput">
                        <HeaderTemplate>
                            <table id="tblNG" class="display table table-bordered table-responsive" style="width:100%">
                                <thead style="background-color: rgb(69, 132, 224); color: white;">
                                    <tr>
                                        <th scope="col">Action</th>
                                        <th scope="col">Type</th>
                                        <th scope="col">Machine Process</th>
                                        <th scope="col">Item Check</th>
                                        <th scope="col">Date</th>
                                        <th scope="col">Shift</th>
                                        <th scope="col">Seq</th>
                                        <th scope="col">USL</th>
                                        <th scope="col">LSL</th>
                                        <th scope="col">UCL</th>
                                        <th scope="col">LCL</th>
                                        <th scope="col">Min</th>
                                        <th scope="col">Max</th>
                                        <th scope="col">Average</th>
                                        <th scope="col">Operator</th>
                                        <th scope="col">MK</th>
                                        <th scope="col">QC</th>
                                        <th scope="col">#</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%--<asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />--%>
                                            <asp:Label runat="server" Text="Edit" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("ItemTypeName") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                        </td>
                                        <td>
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
                                        <td>
                                            <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAverage" runat="server" Text='<%# Eval("Average") %>' />
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
                                        <td>
                                            <%--<asp:Label ID="lbl" runat="server" Text='<%# Eval("LineName") %>' />--%>
                                            <asp:Label runat="server" Text="Send Email" />
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
            <div class="col-12" style="margin-top: 50px; height: 33%">

                <div class="table-responsive">

                    <asp:Repeater runat="server" ID="rptDdelayInput" >

                        <HeaderTemplate>
                            <table id="tblDelayInput" class="display table table-bordered table-responsive" style="width:100%">
                                <thead style="background-color: rgb(69, 132, 224); color: white;">
                                    <tr>
                                        <th scope="col">Action</th>
                                        <th scope="col">Machine Process</th>
                                        <th scope="col">Item Check</th>
                                        <th scope="col">Date</th>
                                        <th scope="col">Shift</th>
                                        <th scope="col">Seq</th>
                                        <th scope="col">Schedule Start</th>
                                        <th scope="col">Schedule End</th>
                                        <th scope="col">Delay</th>
                                        <th scope="col">#</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td>
                                        <%--<asp:Label ID="lblFirstname" runat="server" Text='<%# Eval("CustomerID") %>' />--%>
                                        <asp:Label runat="server" Text="edit" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                    </td>
                                    <td>
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
                                    <td>
                                        <asp:Label ID="lblScheduleEnd" runat="server" Text='<%# Eval("EndTime") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDelay" runat="server" Text='<%# Eval("Delay") %>' />
                                    </td>
                                    <td>
                                        <%--<asp:Label ID="Label9" runat="server" Text='<%# Eval("CustomerID") %>' />--%>
                                        <asp:Label runat="server" Text="send email" />
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
            <div class="col-12" style="margin-top: 30px; height: 33%">

                <div class="table-responsive">

                    <asp:Repeater runat="server" ID="rptDelayVerification">
                        <HeaderTemplate>
                            <table id="tblDelayVerification" class="display table table-bordered table-responsive" style="width:100%">
                                <thead style="background-color: rgb(69, 132, 224); color: white;">
                                    <tr>
                                        <th scope="col">Action</th>
                                        <th scope="col">Type</th>
                                        <th scope="col">Machine Process</th>
                                        <th scope="col">Item Check</th>
                                        <th scope="col">Date</th>
                                        <th scope="col">Shift</th>
                                        <th scope="col">Seq</th>
                                        <th scope="col">USL</th>
                                        <th scope="col">LSL</th>
                                        <th scope="col">UCL</th>
                                        <th scope="col">LCL</th>
                                        <th scope="col">Min</th>
                                        <th scope="col">Max</th>
                                        <th scope="col">Average</th>
                                        <th scope="col">Operator</th>
                                        <th scope="col">MK</th>
                                        <th scope="col">QC</th>
                                        <th scope="col">Verif Time</th>
                                        <th scope="col">Delay Verif</th>
                                        <th scope="col">#</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%--<asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />--%>
                                            <asp:Label runat="server" Text="Edit" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("ItemTypeName") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMachineProcess" runat="server" Text='<%# Eval("LineName") %>' />
                                        </td>
                                        <td>
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
                                        <td>
                                            <asp:Label ID="lblMin" runat="server" Text='<%# Eval("MinValue") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMax" runat="server" Text='<%# Eval("MaxValue") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAverage" runat="server" Text='<%# Eval("Average") %>' />
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
                                        <td>
                                            <asp:Label ID="lblVerifTime" runat="server" Text='<%# Eval("VerifTime") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDelayVerif" runat="server" Text='<%# Eval("DelayVerif") %>' />
                                        </td>
                                        <td>
                                            <%--<asp:Label ID="lbl" runat="server" Text='<%# Eval("LineName") %>' />--%>
                                            <asp:Label runat="server" Text="Send Email" />
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


        <%--Javascript--%>
    <%--<script src="CustomFile/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
    <script src="CustomFile/popper.min.js" integrity="sha384-oBqDVmMz9ATKxIep9tiCxS/Z9fNfEXiDAYTujMAeBAsjFuCZSmKbSSUnQlmh/jp3" crossorigin="anonymous"></script>
    <script src="CustomFile/js/bootstrap.min.js" integrity="sha384-IDwe1+LCz02ROU9k972gdyvl+AESN10+x7tBKgc9I5HFtuNz0wWnPclzo6p9vxnk" crossorigin="anonymous"></script>--%>
        <script type="text/javascript" charset="utf8" src="https://code.jquery.com/jquery-3.5.1.js"></script>
        <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>

        <script>
            $(document).ready(function () {
                $('#tblNG').DataTable({
                    pagingType: 'full_numbers',
                    bFilter: false,
                    bInfo: false,
                    lengthChange: false,
                }),
                $('#tblDelayVerification').DataTable({
                    pagingType: 'full_numbers',
                    bFilter: false,
                    bInfo: false,
                    lengthChange: false,
                }),
                $('#tblDelayInput').DataTable({
                    pagingType: 'full_numbers',
                    bFilter: false,
                    bInfo: false,
                    lengthChange : false,
                });
            });
        </script>
    </form>

</body>
</html>
