<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ReportYearlyByType.aspx.vb" Inherits="PECGI_SPC.ReportYearlyByType" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="js/jquery-3.5.1.min.js"></script>
    <link rel="stylesheet" href="css/dx.light.css" />
    <script type="text/javascript" src="js/dx.all.js"></script>
    <script type="text/javascript">

        var MMM = {
            Jan: "01", Feb: "02", Mar: "03", Apr: "04", May: "05", Jun: "06",
            Jul: "07", Aug: "08", Sep: "09", Oct: "10", Nov: "11", Dec: "12"
        };

        var nRow = 1

        $(document).ready(function () {
            var today = new Date();
            dtFromDate.SetDate(today);
            dtToDate.SetDate(today);
            /*     TmPeriod_From.SetDate(today);*/
            btnExcel.SetEnabled(false);
        })

        function ChangeFactory() {
            var FactoryCode = cboFactory.GetValue();
            HideValue.Set('FactoryCode', FactoryCode);
            cboProcessGroup.PerformCallback(FactoryCode);
        };

        function ChangeProcessGroup() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessGroup = cboProcessGroup.GetValue();
            HideValue.Set('ProcessGroup', ProcessGroup);
            cboLineGroup.PerformCallback(FactoryCode + '|' + ProcessGroup);
        };

        function ChangeLineGroup() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessGroup = cboProcessGroup.GetValue();
            var LineGroup = cboLineGroup.GetValue();
            HideValue.Set('LineGroup', LineGroup);
            cboProcessCode.PerformCallback(FactoryCode + '|' + ProcessGroup + '|' + LineGroup);
        };

        function ChangeProcessCode() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessCode = cboProcessCode.GetValue();
            HideValue.Set('ProcessCode', ProcessCode);
            cboLineCode.PerformCallback(FactoryCode + '|' + ProcessCode);
        };

        function ChangeLineCode() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessCode = cboProcessCode.GetValue();
            var LineCode = cboLineCode.GetValue();
            HideValue.Set('LineCode', LineCode);
            cboItemType.PerformCallback(FactoryCode + '|' + ProcessCode + '|' + LineCode);
        };

        function ChangeItemType() {
            var ItemType = cboItemType.GetValue();
            HideValue.Set('ItemType', ItemType);
        }

        function Browse() {

            var User = HideValue.Get("UserID");
            var FactoryCode = HideValue.Get("FactoryCode");
            var ProcessGroup = HideValue.Get("ProcessGroup");
            var LineGroup = HideValue.Get("LineGroup");
            var ProcessCode = HideValue.Get("ProcessCode");
            var LineCode = HideValue.Get("LineCode");
            var ItemType = HideValue.Get("ItemType");

            var pfromDate = dtFromDate.GetText().split(' ');
            var ProdDate_From = pfromDate[1] + "-" + MMM[pfromDate[0]] + "-" + '01'

            var ptoDate = dtToDate.GetText().split(' ');
            var ProdDate_To = ptoDate[1] + "-" + MMM[ptoDate[0]] + "-" + '01'

            //var pfromDate = dtFromDate.GetText().split(' ');
            //var ProdDate_From = pfromDate[2] + "-" + MMM[pfromDate[1]] + "-" + pfromDate[0]

            //var ptoDate = dtToDate.GetText().split(' ');
            //var ProdDate_To = ptoDate[2] + "-" + MMM[ptoDate[1]] + "-" + ptoDate[0]

            var nMonth = monthDiff(parseDate(dtFromDate.GetText()), parseDate(dtToDate.GetText()));

            if (FactoryCode == null) {
                toastr.warning("Please, Choose Factory !", 'Warning', { timeOut: 3000, closeButton: true });
            } else if (ProcessGroup == null) {
                toastr.warning("Please, Choose Process Group !", 'Warning', { timeOut: 3000, closeButton: true });
            } else if (LineGroup == null) {
                toastr.warning("Please, Choose Line Group !", 'Warning', { timeOut: 3000, closeButton: true });
            } else if (ProcessCode == null) {
                toastr.warning("Please, Choose Machine !", 'Warning', { timeOut: 3000, closeButton: true });
            } else if (LineCode == null) {
                toastr.warning("Please, Choose Machine Process !", 'Warning', { timeOut: 3000, closeButton: true });
            } else if (ItemType == null) {
                toastr.warning("Please, Choose Type !", 'Warning', { timeOut: 3000, closeButton: true });
            } else if (parseDate(dtFromDate.GetText()) > parseDate(dtToDate.GetText())) {
                toastr.warning("To Date can not less then From Date !", 'Warning', { timeOut: 3000, closeButton: true });
            } else if (nMonth > 11) {
                toastr.warning("Periode can not more than 12 period !", 'Warning', { timeOut: 3000, closeButton: true });
            } else {
                tableLineGroup(User, FactoryCode, ProcessGroup, LineGroup, ProcessCode, LineCode, ItemType, ProdDate_From, ProdDate_To);
            }
        }

        function LoadHeader() {
            var prodDate_From = dtFromDate.GetText();
            var prodDate_To = dtToDate.GetText();

            var nMonth = monthDiff(parseDate(prodDate_From), parseDate(prodDate_To)); //Hitung period month
            var tr = document.getElementById('tableLineGroup').tHead.children[0];

            // Insert column 1 dan 2 
            tr.insertCell(0).outerHTML = '<th style="text-align: center; background-color: gray; color: white; font-weight: 100;"> No </th>';
            tr.insertCell(1).outerHTML = '<th style="text-align: center; background-color: gray; color: white; font-weight: 100;"> MachineProcess </th>';

            var now = parseDate(prodDate_From);
            var year = now.getFullYear();
            var month = now.getMonth();

            for (let i = 0; i <= nMonth; i++) {
                var current = now;
                var current = new Date(year, month, 1);

                if (month == 11) {
                    year = year + 1;
                    month = 0;
                } else {
                    month = month + 1;
                }
                now = new Date(current);
                tr.insertCell(i + 2).outerHTML = '<th style="text-align: center; background-color: gray; color: white; font-weight: 100;">' + formatDate(current) + '</th>'
            }
        }

        function tableLineGroup(User, FactoryCode, ProcessGroup, LineGroup, ProcessCode, LineCode, ItemType, ProdDate_From, ProdDate_To) {
            nRow = 1;
            $.ajax({
                url: 'ReportYearlyByType.aspx/LoadData',
                type: 'POST',
                data: '{ User : "' + User + '", FactoryCode : "' + FactoryCode + '", ProcessGroup : "' + ProcessGroup + '", LineGroup :"' + LineGroup + '", ProcessCode :"' + ProcessCode + '", LineCode : "' + LineCode + '", ItemType : "' + ItemType + '", ProdDate_From : "' + ProdDate_From + '", ProdDate_To : "' + ProdDate_To + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.Message == "Success") {
                        $('#tableLineGroup tr th').remove();
                        $('#tableLineGroup tr td').remove();
                        LoadHeader();
                        chartLineGroup(User, FactoryCode, ProcessGroup, LineGroup, ProcessCode, LineCode, ItemType, ProdDate_From, ProdDate_To);
                        Object.values(result.d.Contents).forEach(LineGroupContent);
                    } else {
                        toastr.warning(result.d.Message, 'Warning', { timeOut: 3000, closeButton: true });
                    }
                },
                error: function (ex) {
                    toastr.error(ex.Message, 'Failed', { timeOut: 3000, "closeButton": true });
                }
            });
        }

        function tableLineDetail(User, LineGroup, LineGroupName, Periode, Qty) {
            var ProcessCode = HideValue.Get("ProcessCode");
            var LineCode = HideValue.Get("LineCode");

            /*Save data to hide value for using generate excel data*/
            HideValue.Set("sUser", User);
            HideValue.Set("sLineGroup", LineGroup);
            HideValue.Set("sProcessCode", ProcessCode);
            HideValue.Set("sLineCode", LineCode);
            HideValue.Set("sPeriode", Periode);
            HideValue.Set("sQty", Qty);

            nRow = 1;
            $('#tableLineDetail tr td').remove();
            $.ajax({
                url: 'ReportYearlyByType.aspx/LoadDetail',
                type: 'POST',
                data: '{ User : "' + User + '", ProcessCode :"' + ProcessCode + '", LineCode :"' + LineCode + '", LineGroup : "' + LineGroup + '",  Periode : "' + Periode + '", Qty : "' + Qty + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.Message == "Success") {
                        if (result.d.Contents != "") {
                            /*  btnExcel.SetEnabled(true);*/
                            Object.values(result.d.Contents).forEach(LineDetailContent);
                        }
                    } else {
                        toastr.warning(result.d.Message, 'Warning', { timeOut: 3000, closeButton: true });
                    }
                },
                error: function (ex) {
                    toastr.error(ex.Message, 'Failed', { timeOut: 3000, "closeButton": true });
                }
            });

            chartLineDetail(User, LineGroup, LineGroupName, Periode, Qty)
        }


        function LineGroupContent(dt) {
            var table = document.getElementById("tableLineGroup");
            var row = table.insertRow(nRow);

            let length = dt.length;
            for (let i = 0; i <= length - 1; i++) {
                if (i == 0) {
                    row.insertCell(i).outerHTML = '<td style="text-align: center; background-color: white; font-weight: 100;">' + dt[i] + '</td>'
                }
                else if (i == 1) {
                    row.insertCell(i).outerHTML = '<td style="text-align: left; background-color: white; font-weight: 100;">' + dt[i] + '</td>'
                } else {
                    if (dt[i].split('|')[4] == "0") {
                        row.insertCell(i).outerHTML = '<td style="text-align: center; background-color: white; font-weight: 100;">' + dt[i].split('|')[4] + '</td>'
                    } else {
                        row.insertCell(i).outerHTML = '<td style="text-align: center; background-color: white; font-weight: 100;">' + '<a href="javascript:void(0)" onclick="return tableLineDetail(\'' + dt[i].split("|")[0] + '\', \'' + dt[i].split("|")[1] + '\', \'' + dt[i].split("|")[2] + '\',\'' + dt[i].split("|")[3] + '\',\'' + dt[i].split("|")[4] + '\')" >' + '<div>' + dt[i].split('|')[4] + '</div>' + '</a>' + '</td>'
                    }
                }
            }
            nRow = nRow + 1
        }

        function LineDetailContent(dt) {
            var table = document.getElementById("tableLineDetail");
            var row = table.insertRow(nRow);

            let length = dt.length;
            for (let i = 0; i <= length - 1; i++) {

                if (i == 1) {
                    row.insertCell(i).outerHTML = '<td style="text-align: left; background-color: white; font-weight: 100;">' + dt[i] + '</td>'
                } else if (i == 3) {
                    row.insertCell(i).outerHTML = '<td style="text-align: center; background-color: white; font-weight: 100;">' + dt[i] + '%' + '</td>'
                } else {
                    row.insertCell(i).outerHTML = '<td style="text-align: center; background-color: white; font-weight: 100;">' + dt[i] + '</td>'
                }
            }
            nRow = nRow + 1
        }


        function chartLineGroup(User, FactoryCode, ProcessGroup, LineGroup, ProcessCode, LineCode, ItemType, ProdDate_From, ProdDate_To) {
            $.ajax({
                url: 'ReportYearlyByType.aspx/ChartLineGroup',
                type: 'POST',
                data: '{ User : "' + User + '", FactoryCode : "' + FactoryCode + '", ProcessGroup : "' + ProcessGroup + '", LineGroup :"' + LineGroup + '", ProcessCode :"' + ProcessCode + '", LineCode : "' + LineCode + '", ItemType : "' + ItemType + '", ProdDate_From : "' + ProdDate_From + '", ProdDate_To : "' + ProdDate_To + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.Message == "Success") {
                        if (result.d.Contents != "") {

                            const DS = result.d.Contents
                            var periode = ''
                            var startPeriod = ProdDate_To.split("-")[0];
                            var endPeriod = ProdDate_From.split("-")[0];
                            var ItemTypeName = cboItemType.GetText();

                            if ((parseInt(startPeriod, 10) - parseInt(endPeriod, 10)) > 0) {
                                periode = endPeriod + "/" + startPeriod
                            } else {
                                periode = startPeriod
                            }

                            var display1 = [];
                            var n = 0;
                            $.each(DS, function (dt) {
                                if (n > 0) {
                                    var display2 = {};
                                    let length = DS[dt].length;
                                    var t = 0;
                                    for (let i = 0; i <= length - 1; i++) {
                                        var nName = "data" + t;
                                        if (i == 0) {
                                            display2["Periode"] = DS[dt][i];
                                        } else {
                                            display2[nName] = parseInt(DS[dt][i].split("|")[3], 10);
                                        }
                                        t = t + 1;
                                    }
                                    display1.push(display2);
                                }
                                n = n + 1;
                            });

                            var d1 = [];
                            let length = DS[0].length;
                            var t = 0;
                            for (let i = 0; i <= length - 1; i++) {
                                if (i > 0) {
                                    var d2 = {};
                                    var nName = "data" + t;
                                    d2["valueField"] = nName;
                                    d2["name"] = DS[0][i];
                                    d1.push(d2)
                                }
                                t = t + 1;
                            }

                            document.getElementById('lblTitleChart').innerHTML = 'Resume FTA SPC ' + periode;
                            document.getElementById('lblSubTitleChart').innerHTML = '( ' + ItemTypeName + ' )';

                            $('#chart').dxChart({
                                palette: 'Pastel',
                                dataSource: display1,
                                commonSeriesSettings: {
                                    argumentField: 'Periode',
                                    type: 'stackedBar',
                                },
                                series: d1,
                                legend: {
                                    verticalAlignment: 'bottom',
                                    horizontalAlignment: 'center',
                                    itemTextPosition: 'top',
                                },
                                valueAxis: {
                                    position: 'left',
                                },
                                export: {
                                    enabled: false,
                                },
                                tooltip: {
                                    enabled: true,
                                    location: 'edge',
                                    customizeTooltip(arg) {
                                        return {
                                            text: `${arg.seriesName} = ${arg.valueText}`,
                                        };
                                    },
                                },
                            });

                        }
                    } else {
                        toastr.warning(result.d.Message, 'Warning', { timeOut: 3000, closeButton: true });
                    }
                },
                error: function (ex) {
                    toastr.error(ex.Message, 'Failed', { timeOut: 3000, "closeButton": true });
                }
            });

        }


        function chartLineDetail(User, LineGroup, LineGroupName, Periode, Qty) {
            var ProcessCode = HideValue.Get("ProcessCode");
            var LineCode = HideValue.Get("LineCode");
            $.ajax({
                url: 'ReportYearlyByType.aspx/ChartLineDetail',
                type: 'POST',
                data: '{ User : "' + User + '", ProcessCode :"' + ProcessCode + '", LineCode :"' + LineCode + '", LineGroup : "' + LineGroup + '",  Periode : "' + Periode + '", Qty : "' + Qty + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data, textStatus, jQxhr) {
                    document.getElementById('lblTitleChartDetail').innerHTML = 'Summary FTA SPC ' + Periode;
                    document.getElementById('lblSubTitleChartDetail').innerHTML = '(' + LineGroupName + ')' + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;n = ' + data.d.Max;
                    //}
                    //if (Factory == "" && Type == "" && Line == "") {
                    //    document.getElementById('lblDescsChartLineDetail').innerHTML = 'Period : ' + ' - ' + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;n = ' + data.d.Max;
                    //}
                    //else {
                    //    document.getElementById('lblDescsChartLineDetail').innerHTML = 'Period : ' + dtFromDate.GetText() + ' - ' + dtToDate.GetText() + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;n = ' + data.d.Max;
                    //}


                    var options = {
                        xaxis: {
                            axisLabelUseCanvas: true,
                            show: true,
                            tickLength: 0,
                            axisLabelFontSizePixels: 12,
                            axisLabelFontFamily: 'arial,sans-serif',
                            axisLabelPadding: 500,
                            ticks: eval(data.d.xaxisTicks)
                        },

                        yaxes: [{
                            position: 'left',
                            max: data.d.Max,
                            min: 0,
                            axisLabel: 'FTA Result',
                        }, {
                            position: 'right',
                            max: 100,
                            min: 0,
                            axisLabel: 'Percentage',
                        }
                        ],

                        grid: {
                            hoverable: true,
                            clickable: true,
                            borderWidth: 1,
                            backgroundColor: '#FFF',
                        }
                    };
                    $.plot($('#chartdetail'), eval(data.d.data), options);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }

        function Clear() {
            $('#tableLineGroup tr th').remove();
            $('#tableLineGroup tr td').remove();

        }

        function formatDate(dateString) {
            let date = new Date(dateString),
                month = date.getMonth(),
                year = date.toLocaleString('default', { year: '2-digit' });
            months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            return months[month] + '-' + year;
        }

        function parseDate(str) {
            var mdy = str.split(' ');
            return new Date(mdy[1], MMM[mdy[0]] - 1, '01');
        }

        function monthDiff(dateFrom, dateTo) {
            var months;
            months = (dateTo.getFullYear() - dateFrom.getFullYear()) * 12;
            months -= dateFrom.getMonth();
            months += dateTo.getMonth();
            return months <= 0 ? 0 : months;
        }

        function ExcelClick(s, e) {

            //var dt = new Date();
            //var year = dt.getFullYear();
            //var month = (dt.getMonth() + 1).toString().padStart(2, "0");
            //var day = dt.getDate().toString().padStart(2, "0");
            //var hours = dt.getTime();
            //var sDate = day + month + year + hours;
            //console.log(sDate);
            //var filename = 'chart_' + sDate;
            //var chart = $("#chart").dxChart("instance");
            //chart.exportTo(filename, 'PNG');



            //var detfilename = 'chartdetail_' + sDate;
            //var chart = $("#chartdetail").dxChart("instance");


            var chart = $("#chart").dxChart("instance");
            chart.exportTo(detfilename, 'PNG').thenthen(function (imgData) {
                Image.SetImageUrl(imgData);
                /* Grid.PerformCallback('Excel' + '|' + ASPxImage1.GetImageUrl());*/
                Image.SetVisible(false);
                HideValue.Set("FileName", Image.GetImageUrl());
            });
            //chart.exporting.getImage("jpg").then(function (imgData) {
            //   Image.SetImageUrl(imgData);
            //   /* Grid.PerformCallback('Excel' + '|' + ASPxImage1.GetImageUrl());*/
            //    Image.SetVisible(false);
            //    HideValue.Set("FileName", Image.GetImageUrl());
            //});

            //HideValue.Set("FileName", filename);
            //HideValue.Set("DetFileName", detfilename);
        }

        function MessageError(s, e) {
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
                else if (s.cp_message == "" && s.cp_val == 0) {
                    toastr.options.closeButton = false;
                    toastr.options.debug = false;
                    toastr.options.newestOnTop = false;
                    toastr.options.progressBar = false;
                    toastr.options.preventDuplicates = true;
                    toastr.options.onclick = null;
                }
            }
        }

    </script>

    <style type="text/css">
        #chart {
            width: 40vw;
            height: 50vh;
        }

        .txtAlignLeft {
            text-align: left;
        }

        .txtAlignCenter {
            text-align: center;
        }

        .txtAlignRight {
            text-align: right;
        }

        .tooltip-header {
            margin-bottom: 5px;
            font-size: 16px;
            font-weight: 500;
            padding-bottom: 5px;
            border-bottom: 1px solid #c5c5c5;
        }

        .tooltip-body {
            width: 170px;
        }

            .tooltip-body .series-name {
                font-weight: normal;
                opacity: 0.6;
                display: inline-block;
                line-height: 1.5;
                padding-right: 10px;
                width: 126px;
            }

            .tooltip-body .value-text {
                display: inline-block;
                line-height: 1.5;
                width: 30px;
            }
    </style>

    <style type="text/css">
        .flot {
            left: 0px;
            top: 0px;
            right: 0px;
            bottom: 0px;
            height: 50vh;
            width: 30vw;
        }

        #flotTip {
            padding: 3px 5px;
            background-color: #000;
            z-index: 100;
            color: #fff;
            opacity: .80;
            filter: alpha(opacity=85);
        }

        #marking {
            z-index: 100;
        }

        .hidden {
            display: none;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 0px 5px 5px 5px; padding-bottom: 15px; border-bottom: groove 2px">
        <table>
            <tr style="height: 40px">
                <td>
                    <dx:ASPxLabel ID="lblFactory" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Factory"></dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboFactory" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboFactory" Width="150px">
                        <ClientSideEvents SelectedIndexChanged="ChangeFactory" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 30px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblProcessCode" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Machine"></dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboProcessCode" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboProcessCode" Width="150px">
                        <ClientSideEvents SelectedIndexChanged="ChangeProcessCode" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 30px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblPeriod" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Prod. Date"></dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxTimeEdit ID="dtFromDate" EditFormat="Custom" EditFormatString="MMM yyyy" ClientInstanceName="dtFromDate"
                        DisplayFormatString="MMM yyyy" Theme="Office2010Black" runat="server" Width="100px">
                        <ButtonStyle Width="5px" Paddings-Padding="4px"></ButtonStyle>
                    </dx:ASPxTimeEdit>

                </td>

                <td style="width: 10px">&nbsp;</td>

                <td style="width: 10px">
                    <dx:ASPxLabel ID="lblToDate" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="To"></dx:ASPxLabel>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td>&nbsp;</td>
                <td style="width: 20px" colspan="3">
                    <dx:ASPxTimeEdit ID="dtToDate" EditFormat="Custom" EditFormatString="MMM yyyy" ClientInstanceName="dtToDate"
                        DisplayFormatString="MMM yyyy" Theme="Office2010Black" runat="server" Width="100px">
                        <ButtonStyle Width="5px" Paddings-Padding="4px"></ButtonStyle>
                    </dx:ASPxTimeEdit>

                </td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxLabel ID="lblProcessGroup" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Process Group"></dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboProcessGroup" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboProcessGroup" Width="150px">
                        <ClientSideEvents SelectedIndexChanged="ChangeProcessGroup" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 30px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblLineCode" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Machine Process"></dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboLineCode" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboLineCode" Width="150px">
                        <ClientSideEvents SelectedIndexChanged="ChangeLineCode" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr style="height: 40px">
                <td>
                    <dx:ASPxLabel ID="lblLineGroup" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Line Group"></dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboLineGroup" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboLineGroup" Width="150px">
                        <ClientSideEvents SelectedIndexChanged="ChangeLineGroup" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 30px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblItemType" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Type"></dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboItemType" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboItemType" Width="150px">
                        <ClientSideEvents SelectedIndexChanged="ChangeItemType" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    <dx:ASPxButton ID="btnBrowse" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Height="25px"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Browse" Theme="Office2010Silver" Width="80px">
                        <ClientSideEvents Click="Browse" />
                    </dx:ASPxButton>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxButton ID="btnClear" runat="server" AutoPostBack="False" ClientInstanceName="btnClear" Height="25px"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Clear" Theme="Office2010Silver" Width="80px">
                        <%-- <ClientSideEvents Click="Clear" />--%>
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 10px">
        <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" ClientInstanceName="btnExcel" Height="30px"
            Font-Names="Segoe UI" Font-Size="9pt" Text="Excel" Theme="Office2010Silver" Width="100px">
            <ClientSideEvents Click="ExcelClick" />
        </dx:ASPxButton>
    </div>
    <dx:ASPxCallback ID="cb" runat="server" ClientInstanceName="cb">
        <ClientSideEvents CallbackComplete="MessageError" />
    </dx:ASPxCallback>
    <dx:ASPxHiddenField ID="HideValue" runat="server" ClientInstanceName="HideValue"></dx:ASPxHiddenField>
    <dx:ASPxImage ID="Image" ClientInstanceName="Image" runat="server" ShowLoadingImage="true">
    </dx:ASPxImage>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding: 0px 5px 5px 5px; padding-bottom: 10px;" class="widget-body">
        <section id="widget-grid" class="">
            <div class="row">
                <article class="col-sm-6 col-md-6 col-lg-6">
                    <div class="jarviswidget" id="wid1" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <div style="text-align: left; background-color: gray; padding-left: 10px;">
                                <h2 style="color: white">FTA SPC Result by Item Group (Yearly)</h2>
                            </div>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox">
                            </div>
                            <div class="widget-body no-padding">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <table style="width: 100%; min-height: 250px" border="1">
                                            <tr>
                                                <td align="center">
                                                    <div>
                                                        <h3 id="lblTitleChart"></h3>
                                                        <p p style="margin-top: -10px" id="lblSubTitleChart"></p>
                                                    </div>
                                                    <div id="chart" class="float"></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="row">
                                    &nbsp
                                </div>
                                <div class="row">
                                    <div class="col-xs-12" style="min-height: 200px;">
                                        <table id="tableLineGroup" class="table table-striped table-bordered table-hover row-border" style="font-family: 'Trebuchet MS'">
                                            <thead>
                                                <tr role="row">
                                                    <th style="text-align: center; background-color: gray; color: white; font-weight: 100;" width="5%">No</th>
                                                    <th style="text-align: center; background-color: gray; color: white; font-weight: 100;" width="70%">Machine Process</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </article>

                <article class="col-sm-6 col-md-6 col-lg-6">
                    <div class="jarviswidget" id="wid2" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <div style="text-align: left; background-color: gray; padding-left: 10px;">
                                <h2 style="color: white">FTA SPC RESULT by Line (Monthly)</h2>
                            </div>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox">
                            </div>
                            <div class="widget-body no-padding">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <table style="width: 100%; min-height: 250px;" border="1">
                                            <tr>
                                                <td align="center">
                                                    <div>
                                                        <h3 id="lblTitleChartDetail"></h3>
                                                        <p style="margin-top: -10px" id="lblSubTitleChartDetail"></p>
                                                    </div>
                                                    <div id="chartdetail" class="flot"></div>
                                                    <%-- <div id="chartdetail" ></div>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="row">
                                    &nbsp
                                </div>
                                <div class="row">
                                    <div class="col-xs-12" style="min-height: 200px;">
                                        <table id="tableLineDetail" class="table table-striped table-bordered table-hover row-border" style="font-family: 'Trebuchet MS'">
                                            <thead>
                                                <tr role="row">
                                                    <th style="text-align: center; background-color: gray; color: white; font-weight: 100;" width="5%">No
                                                    </th>
                                                    <th style="text-align: center; background-color: gray; color: white; font-weight: 100;" width="30%">Item FTA by Line
                                                    </th>
                                                    <th style="text-align: center; background-color: gray; color: white; font-weight: 100;" width="25%">Qty Total
                                                    </th>
                                                    <th style="text-align: center; background-color: gray; color: white; font-weight: 100;" width="30%">Percentage (%)
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
        </section>
    </div>
</asp:Content>
