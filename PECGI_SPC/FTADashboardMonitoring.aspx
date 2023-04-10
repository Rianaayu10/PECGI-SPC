﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FTADashboardMonitoring.aspx.vb" Inherits="PECGI_SPC.FTADashboardMonitoring" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>FTA Dashboard Dashboard</title>
    <link href="Styles/images/favicon_pecgi.ico" rel="SHORTCUT ICON" type="image/icon" />

    <meta charset="utf-8" content="" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/global-circuit-store.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-production-plugins.min.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-production.min.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-skins.min.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/smartadmin-rtl.min.css" />
    <script src="Scripts/toastr.js" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="content/toastr.css" />
    <link href="css/fixedColumns.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" href="content/toastr.min.css" />

    <style>
        .position {
            position: absolute;
            z-index: 2;
            min-height: 2vh;
            min-width: 2vw;
            text-align: center;
            border-color: black;
            border-style: solid;
        }

        .position2 {
            position: absolute;
            z-index: 2;
            min-height: 2vh;
            min-width: 3.5vw;
            text-align: center;
            border-color: black;
            border-style: solid;
        }

        .position3 {
            position: absolute;
            z-index: 2;
            min-height: 3.2vh;
            min-width: 3.5vw;
            text-align: center;
            border-color: black;
            border-style: solid;
        }

        .bk_color {
            background-color: grey;
        }

        .delay_font {
            font-size: 13vh;
            font-weight: 600;
        }
    </style>


    <style type="text/css">
        .card {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 305px;
            padding: 10px 10px 10px 10px;
        }

            .card:hover {
                box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
            }

        .loader {
            border: 10px solid #f3f3f3;
            border-radius: 50%;
            border-top: 10px solid #3498db;
            width: 60px;
            height: 60px;
            -webkit-animation: spin 2s linear infinite; /* Safari */
            animation: spin 2s linear infinite;
        }

        .overlay {
            display: none;
            width: 100%;
            height: 100%;
            left: 0;
            right: 0;
            position: absolute;
            z-index: 999;
            background: rgba(0, 0, 0, 0.65);
        }

        /* Safari */
        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div style="display: block;">
            <div>
                <table style="width: 100%; background-color: #4584e0;">
                    <tbody style="height: 40px; overflow-x: hidden; overflow-y: hidden;">
                        <tr style="height: 10px">
                            <td align="center" width="10%"><a href="Main.aspx">
                                <img src="img/logo-panasonic.svg" alt="" width="100" height="50" class="d-inline-block align-text-top"></a></td>
                            <td width="80%" align="center">
                                <font style="color: black; font-size: 4.5vh; font-weight: bold; font-family: Segoe UI"><u>
                                    <label style="color: white"><b>FTA Dashboard Monitoring</b></label>
                                </u></font>
                            </td>

                            <td style="padding-right: 10px"></td>
                            <td align="right" style="width: 10%; background-color: black">
                                <small>
                                    <p style="text-align: left; color: white; margin-top: 5px; margin-left: 5px">
                                        Date :
                                        <dx:ASPxLabel ID="lblDateNow" ClientInstanceName="lblOK" runat="server" Text="" CssClass="text" Font-Names="Segoe UI" Font-Size="9pt" />
                                        <br />
                                        Time :
                                        <label id="lblTimeNow"></label>
                                    </p>
                                </small>
                            </td>
                        </tr>

                        <%--  <tr style="height: 10px">
                            <td align="center" width="10%"><a href="Main.aspx">
                                <img src="img/logo-panasonic.svg" alt="" width="100" height="50" class="d-inline-block align-text-top"></a></td>
                            <td width="80%" align="center">
                                <font style="color: black; font-size: 4.5vh; font-weight: bold; font-family: Segoe UI"><u>
                                    <label style="color: white"><b>FTA Dashboard Monitoring</b></label>
                                </u></font>
                            </td>
                        </tr>--%>
                    </tbody>
                </table>
            </div>
            <div style="margin-left: 2Vh">
                <div style="height: 60vh">
                    <div style="position: absolute; display: flex;">
                        <img src="img/DashboardFTA.png" style="width: 80vw; height: 60vh" />
                    </div>

                    <div id="Station1" class="position bk_color" style="margin-top: 14vh; margin-left: 4.9vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">1</span>
                    </div>

                    <div id="Station2" class="position bk_color" style="margin-top: 28.7vh; margin-left: 1.3vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">2</span>
                    </div>

                    <div id="Station3" class="position bk_color" style="margin-top: 46vh; margin-left: 21vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">3</span>
                    </div>

                    <div id="Station4" class="position bk_color" style="margin-top: 5.3vh; margin-left: 20.3vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">4</span>
                    </div>

                    <div id="Station5" class="position bk_color" style="margin-top: 5.3vh; margin-left: 23.7vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">5</span>
                    </div>

                    <div id="Station6" class="position bk_color" style="margin-top: 14.3vh; margin-left: 34.8vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">6</span>
                    </div>

                    <div id="Station7" class="position bk_color" style="margin-top: 33.7vh; margin-left: 40.5vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">7</span>
                    </div>

                    <div id="Station8" class="position bk_color" style="margin-top: 45.2vh; margin-left: 44.3vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">8</span>
                    </div>

                    <div id="Station9" class="position bk_color" style="margin-top: 35.7vh; margin-left: 45.1vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">9</span>
                    </div>

                    <div id="Station10" class="position2 bk_color" style="margin-top: 34.1vh; margin-left: 49.4vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">10</span>
                    </div>

                    <div id="Station11" class="position2 bk_color" style="margin-top: 48vh; margin-left: 52.6vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">11</span>
                    </div>

                    <div id="Station12" class="position2 bk_color" style="margin-top: 34.5vh; margin-left: 57.4vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">12</span>
                    </div>

                    <div id="Station13" class="position2 bk_color" style="margin-top: 50.3vh; margin-left: 59.3vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">13</span>
                    </div>

                    <div id="Station14" class="position2 bk_color" style="margin-top: 11.3vh; margin-left: 43.1vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">14</span>
                    </div>

                    <div id="Station15" class="position3 bk_color" style="margin-top: 14.1vh; margin-left: 68.5vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">15</span>
                    </div>

                    <div id="Station16" class="position2 bk_color" style="margin-top: 14vh; margin-left: 75.6vw">
                        <span style="font-size: 1.5vh; font-weight: 600;">16</span>
                    </div>
                </div>

                <div style="display: flex; margin-top: 2vh;">
                    <div style="border-style: solid; min-height: 30vh; min-width: 20vw; margin-right: 2vh;">
                        <div style="justify-content: center; display: flex;">
                            <div style="border-bottom: solid; border-left-style: solid; border-right-style: solid; min-width: 10vw; min-height: 5vh; align-items: center; display: flex; justify-content: center;">
                                <span style="color: black; font-weight: 600; font-size: 2vh">Status Station</span>
                            </div>
                        </div>
                        <div style="margin-left: 2Vh">
                            <div style="display: flex; justify-content: left; margin-top: 2vh;">
                                <div style="border-style: solid; background-color: green; min-width: 2vw; min-height: 2vh; margin-right: 2vh"></div>
                                <div><span style="font-size: 2vh;">Safe</span></div>
                            </div>
                            <div style="display: flex; justify-content: left; margin-top: 2vh;">
                                <div style="border-style: solid; background-color: yellow; min-width: 2vw; min-height: 2vh; margin-right: 2vh"></div>
                                <div><span style="font-size: 2vh;">Delay Input</span></div>
                            </div>
                            <div style="display: flex; justify-content: left; margin-top: 2vh;">
                                <div style="border-style: solid; background-color: red; min-width: 2vw; min-height: 2vh; margin-right: 2vh"></div>
                                <div><span style="font-size: 2vh;">NG Result</span></div>
                            </div>
                            <div style="display: flex; justify-content: left; margin-top: 2vh;">
                                <div style="border-style: solid; background-color: blue; min-width: 2vw; min-height: 2vh; margin-right: 2vh"></div>
                                <div><span style="font-size: 2vh;">No Production</span></div>
                            </div>
                        </div>
                    </div>
                    <div style="border-style: solid; min-height: 30vh; min-width: 15vw; margin-right: 2vh;">
                        <div style="display: flex; justify-content: center; margin-bottom: 2vh">
                            <div style="border-bottom: solid; border-left-style: solid; border-right-style: solid; min-width: 12vw; min-height: 5vh; align-items: center; display: flex; justify-content: center;">
                                <span style="color: black; font-weight: 600; font-size: 2vh;">Delay Input</span>
                            </div>
                        </div>
                        <div style="display: flex; justify-content: center;">
                            <button id="btnDelay" style="border-style: none">
                                <div style="border-style: solid; min-width: 10vw; min-height: 20vh; text-align: center; align-items: center; display: flex; justify-content: center;">
                                    <span id="DelayInput">--- </span>
                                </div>
                            </button>

                        </div>
                    </div>
                    <div style="border-style: solid; min-height: 30vh; min-width: 15vw;">
                        <div style="display: flex; justify-content: center; margin-bottom: 2vh">
                            <div style="border-bottom: solid; border-left-style: solid; border-right-style: solid; min-width: 12vw; min-height: 5vh; align-items: center; display: flex; justify-content: center;">
                                <span style="color: black; font-weight: 600; font-size: 2vh;">NG Result</span>
                            </div>
                        </div>
                        <div style="display: flex; justify-content: center;">
                            <button id="btnNG" style="border-style: none">
                                <div style="border-style: solid; min-width: 10vw; min-height: 20vh; text-align: center; align-items: center; display: flex; justify-content: center;">
                                    <span id="DelayNG">--- </span>
                                </div>
                            </button>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <dx:ASPxHiddenField ID="HideValue" runat="server" ClientInstanceName="HideValue"></dx:ASPxHiddenField>
    </form>


    <script type="text/javascript" src="js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript">
        var UserID = ""
        var ChangeValue = ""

        $(document).ready(function () {
            UserID = HideValue.Get("UserID");
            loaddashboard(userid);
        })

        window.onload = function () {

            setTimer1 = setInterval(function () {
                var today = new Date();
                document.getElementById('lblTimeNow').innerHTML = ("0" + today.getHours()).slice(-2) + ":" + ("0" + today.getMinutes()).slice(-2) + ":" + ("0" + today.getSeconds()).slice(-2);
            }, 1000, (1));

            setTimer1 = setInterval(function () {
                loadDashboard(UserID);
            }, 5000, (1));

        }

        $("#btnNG").click(function () {
            /* window.open('AlertNGResult.aspx', '_blank');*/
            window.open('AlertNGResult.aspx?ProdDate=1', '_blank');
        })

        $("#btnDelay").click(function () {
            /*    window.open('AlertDelayInput.aspx', '_blank');*/
            window.open('AlertDelayInput.aspx?ProdDate=1', '_blank');
        })


        function loadDashboard(UserID) {
            $.ajax({
                url: 'FTADashboardMonitoring.aspx/LoadData',
                type: 'POST',
                data: '{ User : "' + UserID + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.Message == "Success") {

                        var station1 = result.d.Contents.Station1
                        var station2 = result.d.Contents.Station2
                        var station3 = result.d.Contents.Station3
                        var station4 = result.d.Contents.Station4
                        var station5 = result.d.Contents.Station5
                        var station6 = result.d.Contents.Station6
                        var station7 = result.d.Contents.Station7
                        var station8 = result.d.Contents.Station8
                        var station9 = result.d.Contents.Station9
                        var station10 = result.d.Contents.Station10
                        var station11 = result.d.Contents.Station11
                        var station12 = result.d.Contents.Station12
                        var station13 = result.d.Contents.Station13
                        var station14 = result.d.Contents.Station14
                        var station15 = result.d.Contents.Station15
                        var station16 = result.d.Contents.Station16
                        var DelayNG = result.d.Contents.DelayNG
                        var DelayInput = result.d.Contents.DelayInput

                        if (ChangeValue == "") {
                            DashboarContent(station1, station2, station3, station4, station5, station6, station7, station8, station9, station10, station11, station12, station13, station14, station15, station16, DelayNG, DelayInput);
                        } else {
                            console.log(station1);
                            var laststation1 = HideValue.Get("station1")
                            var laststation2 = HideValue.Get("station2")
                            var laststation3 = HideValue.Get("station3")
                            var laststation4 = HideValue.Get("station4")
                            var laststation5 = HideValue.Get("station5")
                            var laststation6 = HideValue.Get("station6")
                            var laststation7 = HideValue.Get("station7")
                            var laststation8 = HideValue.Get("station8")
                            var laststation9 = HideValue.Get("station9")
                            var laststation10 = HideValue.Get("station10")
                            var laststation11 = HideValue.Get("station11")
                            var laststation12 = HideValue.Get("station12")
                            var laststation13 = HideValue.Get("station13")
                            var laststation14 = HideValue.Get("station14")
                            var laststation15 = HideValue.Get("station15")
                            var laststation16 = HideValue.Get("station16")
                            var lastDelayInput = HideValue.Get("DelayInput")
                            var lastDelayNG = HideValue.Get("DelayNG")

                            if (station1 != laststation1 || station2 != laststation2 || station3 != laststation3 || station4 != laststation4 || station5 != laststation5 || station6 != laststation6 || station7 != laststation7 || station8 != laststation8 || station9 != laststation9 || station10 != laststation10 || station11 != laststation11 || station2 != laststation12 || station13 != laststation13 || station14 != laststation14 || station15 != laststation15 || station16 != laststation16 || DelayInput != lastDelayInput || DelayNG != lastDelayNG) {
                                DashboarContent(station1, station2, station3, station4, station5, station6, station7, station8, station9, station10, station11, station12, station13, station14, station15, station16, DelayNG, DelayInput);
                            }
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

        function DashboarContent(station1, station2, station3, station4, station5, station6, station7, station8, station9, station10, station11, station12, station13, station14, station15, station16, DelayNG, DelayInput) {
            HideValue.Set("station1", station1)
            HideValue.Set("station2", station2)
            HideValue.Set("station3", station3)
            HideValue.Set("station4", station4)
            HideValue.Set("station5", station5)
            HideValue.Set("station6", station6)
            HideValue.Set("station7", station7)
            HideValue.Set("station8", station8)
            HideValue.Set("station9", station9)
            HideValue.Set("station10", station10)
            HideValue.Set("station11", station11)
            HideValue.Set("station12", station12)
            HideValue.Set("station13", station13)
            HideValue.Set("station14", station14)
            HideValue.Set("station15", station15)
            HideValue.Set("station16", station16)
            HideValue.Set("DelayNG", DelayNG)
            HideValue.Set("DelayInput", DelayInput)

            //console.log(data.DelayInput);

            $("#Station1").css("background-color", station1.split("|")[1]);
            $("#Station1").css("color", station1.split("|")[2]);

            $("#Station2").css("background-color", station2.split("|")[1]);
            $("#Station2").css("color", station2.split("|")[2]);

            $("#Station3").css("background-color", station3.split("|")[1]);
            $("#Station3").css("color", station3.split("|")[2]);

            $("#Station4").css("background-color", station4.split("|")[1]);
            $("#Station4").css("color", station4.split("|")[2]);

            $("#Station5").css("background-color", station5.split("|")[1]);
            $("#Station5").css("color", station5.split("|")[2]);

            $("#Station6").css("background-color", station6.split("|")[1]);
            $("#Station6").css("color", station6.split("|")[2]);

            $("#Station7").css("background-color", station7.split("|")[1]);
            $("#Station7").css("color", station7.split("|")[2]);

            $("#Station8").css("background-color", station8.split("|")[1]);
            $("#Station8").css("color", station8.split("|")[2]);

            $("#Station9").css("background-color", station9.split("|")[1]);
            $("#Station9").css("color", station9.split("|")[2]);

            $("#Station10").css("background-color", station10.split("|")[1]);
            $("#Station10").css("color", station10.split("|")[2]);

            $("#Station11").css("background-color", station11.split("|")[1]);
            $("#Station11").css("color", station11.split("|")[2]);

            $("#Station12").css("background-color", station12.split("|")[1]);
            $("#Station12").css("color", station12.split("|")[2]);

            $("#Station13").css("background-color", station13.split("|")[1]);
            $("#Station13").css("color", station13.split("|")[2]);

            $("#Station14").css("background-color", station14.split("|")[1]);
            $("#Station14").css("color", station14.split("|")[2]);

            $("#Station15").css("background-color", station15.split("|")[1]);
            $("#Station15").css("color", station15.split("|")[2]);

            $("#Station16").css("background-color", station16.split("|")[1]);
            $("#Station16").css("color", station16.split("|")[2]);

            $("#DelayNG").html(DelayNG);
            $("#DelayNG").addClass("delay_font");

            $("#DelayInput").html(DelayInput);
            $("#DelayInput").addClass("delay_font");

            ChangeValue = "1";
        }

    </script>
    <script type="text/javascript">
        if (!window.jQuery) {
            document.write('<script src="js/libs/jquery-2.1.1.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript">
        if (!window.jQuery.ui) {
            document.write('<script src="js/libs/jquery-ui-1.10.3.min.js"><\/script>');
        }
    </script>
    <script src="js/app.config.js" type="text/javascript"></script>
    <script src="js/plugin/jquery-touch/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="js/bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/notification/SmartNotification.min.js" type="text/javascript"></script>
    <script src="js/smartwidgets/jarvis.widget.min.js" type="text/javascript"></script>
    <script src="js/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js" type="text/javascript"></script>
    <script src="js/plugin/sparkline/jquery.sparkline.min.js" type="text/javascript"></script>
    <script src="js/plugin/jquery-validate/jquery.validate.min.js" type="text/javascript"></script>
    <script src="js/plugin/masked-input/jquery.maskedinput.min.js" type="text/javascript"></script>
    <script src="js/plugin/select2/select2.min.js" type="text/javascript"></script>
    <script src="js/plugin/bootstrap-slider/bootstrap-slider.min.js" type="text/javascript"></script>
    <script src="js/plugin/msie-fix/jquery.mb.browser.min.js" type="text/javascript"></script>
    <script src="js/plugin/fastclick/fastclick.min.js" type="text/javascript"></script>
    <script src="js/app.min.js" type="text/javascript"></script>
    <script src="js/speech/voicecommand.min.js" type="text/javascript"></script>
    <script src="js/smart-chat-ui/smart.chat.ui.min.js" type="text/javascript"></script>
    <script src="js/smart-chat-ui/smart.chat.manager.min.js" type="text/javascript"></script>
    <script src="js/plugin/vectormap/jquery-jvectormap-1.2.2.min.js" type="text/javascript"></script>
    <script src="js/plugin/vectormap/jquery-jvectormap-world-mill-en.js" type="text/javascript"></script>
    <script src="js/plugin/moment/moment.min.js" type="text/javascript"></script>
    <script src="js/plugin/fullcalendar/jquery.fullcalendar.min.js" type="text/javascript"></script>
    <script src="Scripts/toastr.js" type="text/javascript"></script>
    <script src="Scripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Scripts/dataTables.fixedColumns.min.js" type="text/javascript"></script>
    <script src="Scripts/datatables.responsive.min.js" type="text/javascript"></script>
    <script src="Scripts/bootstrap-waitingfor/bootstrap-waitingfor.min.js" type="text/javascript"></script>
    <script src="Scripts/chartjs/chart.min.js" type="text/javascript"></script>
    <script src="Scripts/flot/jquery.flot.min.js" type="text/javascript"></script>
    <script src="Scripts/flot/jquery.flot.tooltip.min.js" type="text/javascript"></script>
    <script src="Scripts/flot/jquery.flot.axislabels.js" type="text/javascript"></script>
    <script src="Scripts/flot/jquery.flot.dashes.js" type="text/javascript"></script>

</body>
</html>