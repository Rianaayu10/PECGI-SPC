﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserPrivilege.aspx.vb" Inherits="PECGI_SPC.UserPrivilege" %>

<%@ Register namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="ASPxCallbackPanel" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>STATISTICS QUALITY CONTROL</title>    
    <link href="Styles/images/icon.ico" rel="SHORTCUT ICON" type="image/icon" />

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
    <link rel="Stylesheet" type="text/css" href="content/toastr.css" />
    <link rel="Stylesheet" href="content/toastr.min.css" />
    <%--<link href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet" />--%>

<script type="text/javascript">
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
                //toastr.options.timeOut = 1000;
                //toastr.options.extendedTimeOut = 0;
                //toastr.options.fadeOut = 250;
                //toastr.options.fadeIn = 250;
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
                //toastr.options.timeOut = 1000;
                //toastr.options.extendedTimeOut = 0;
                //toastr.options.fadeOut = 250;
                //toastr.options.fadeIn = 250;
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
                //toastr.options.timeOut = 1000;
                //toastr.options.extendedTimeOut = 0;
                //toastr.options.fadeOut = 250;
                //toastr.options.fadeIn = 250;
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

    }

    function OnBatchEditStartEditing(s, e) {
        currentColumnName = e.focusedColumn.fieldName;
        if (currentColumnName == "GroupID" || currentColumnName == "MenuID" || currentColumnName == "MenuDesc") {
            e.cancel = true;
        }
        currentEditableVisibleIndex = e.visibleIndex;
    }

    function OnAccessCheckedChanged(s, e) {
        gridMenu.SetFocusedRowIndex(-1);
        if (s.GetValue() == -1) s.SetValue(1);
        for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
            if (gridMenu.batchEditApi.GetCellValue(i, "AllowAccess", false) != s.GetValue()) {
                gridMenu.batchEditApi.SetCellValue(i, "AllowAccess", s.GetValue());
            }
        }
    }

    function OnUpdateCheckedChanged(s, e) {
        if (s.GetValue() == -1) s.SetValue(1);
        gridMenu.SetFocusedRowIndex(-1);
        for (var i = 0; i < gridMenu.GetVisibleRowsOnPage(); i++) {
            if (gridMenu.batchEditApi.GetCellValue(i, "AllowUpdate", false) != s.GetValue()) {
                gridMenu.batchEditApi.SetCellValue(i, "AllowUpdate", s.GetValue());
            }
        }
    }

    function SaveData(s, e) {
        cbkValid.PerformCallback('save|' + cboUser.GetText() + '|' + txtUser.GetText());
    }

    function SavePrivilege(s, e) {
        gridMenu.UpdateEdit();
        millisecondsToWait = 1000;
        setTimeout(function () {
            gridMenu.PerformCallback('save|' + txtUser.GetText());
        }, millisecondsToWait);
    }
    </script>
</head>
<body class="smart-style fixed-header">
    <!-- HEADER -->
<header id="header">
			<div id="logo-group" style="width: 220px; margin-left: 0px; padding:8px 0px 0px 35px;">
				<!-- PLACE YOUR LOGO HERE -->
                <%--<span id="logo">--%>
                 <img src="Styles/images/logohead.png" alt="MUSASHI" width="150px"/>
                <%--</span>--%><%--END LOGO PLACEHOLDER --%>
		    </div>

			<!-- projects dropdown -->
			<div class="project-context hidden-xs" >
				<span></span>
				<span class="project-selector dropdown-toggle" style="font-family:Tahoma; font-weight:bold; font-size:12pt; margin-top:10px; padding:0px 0px 0px 18px">STATISTICS QUALITY CONTROL</span>
			</div>
			<!-- end projects dropdown -->

    </header>    
    <!-- END HEADER -->
    <!-- Left panel : Navigation area -->
    <!-- Note: This width of the aside area can be adjusted through LESS variables -->
    <!-- END NAVIGATION -->
    <!-- MAIN PANEL -->
    <div id="main" role="main">
        <!-- MAIN CONTENT -->
        <div id="content">
            <!-- widget grid -->
            <section id="widget-grid" class="">
				<!-- START ROW -->
				<div class="row">
				    <!-- NEW COL START -->
					<article class="col-sm-12 col-md-12 col-lg-12">
				        <!-- Widget ID (each widget will need unique ID)-->
						<div class="jarviswidget" id="wid-id-0" data-widget-deletebutton="false" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-custombutton="false">
                            <header>
							    <span class="widget-icon"><i class="fa fa-edit"></i> </span>
								<h2><strong><asp:Label ID="Label1" runat="server" Text="User Privileges"></asp:Label></strong></h2>
                            </header>
								<!-- widget div-->
                            <div>
						        <!-- widget content -->
						        <div class="widget-body">
                                    <form class="smart-form" id="Form1" runat="server" >

                                    <div>
                                        <table>
                                            <tr style="height: 30px">
                                                <td style="width: 120px">
                                                    &nbsp;<dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="User">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    
                                                    <dx:ASPxTextBox ID="txtUser" runat="server" ClientInstanceName="txtUser" 
                                                        Width="170px" ReadOnly="True" Font-Names="Segoe UI" Font-Size="8pt">
                                                        <ReadOnlyStyle BackColor="WhiteSmoke">
                                                        </ReadOnlyStyle>
                                                    </dx:ASPxTextBox>
                                                    
                                                </td>
                                            </tr>
                                            <tr style="height: 30px">
                                                <td>
                                                    &nbsp;<dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Segoe UI" 
                                                        Font-Size="8pt" Text="Copy Privileges From">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    
                                                    
                                                    
                                                    <dx:ASPxComboBox ID="cboUser" runat="server" Font-Names="Segoe UI" 
                                                        Font-Size="8pt" Theme="Office2010Black" DataSourceID="dsUser" 
                                                        EnableTheming="True" TextField="UserID" TextFormatString="{0}" 
                                                        ValueField="UserID" ClientInstanceName="cboUser">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	gridMenu.PerformCallback('load|' + cboUser.GetText());
}" />
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="User ID" FieldName="UserID" Width="60px" />
                                                            <dx:ListBoxColumn Caption="Full Name" FieldName="FullName" Width="120px" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                    
                                                    
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="height:10px">
                                        <asp:SqlDataSource ID="dsUser" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
                                        SelectCommand="select UserID, FullName from UserSetup order by UserID"></asp:SqlDataSource>
                                    </div>


                                    <%--<asp:ScriptManager ID="ScriptManager" runat="server" />--%>

<dx:ASPxGridView ID="gridMenu" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridMenu"
                    Font-Names="Segoe UI" Font-Size="8pt" KeyFieldName="MenuID" Theme="Office2010Black"
                    Width="100%">
                                        <ClientSideEvents 
                                        BatchEditStartEditing="OnBatchEditStartEditing" 
                                        EndCallback="OnEndCallback" CallbackError="function(s, e) {
	                                        e.Cancel=True;
                                        }" />                    
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Menu Group" FieldName="GroupID" Name="GroupID"
                            ReadOnly="True" VisibleIndex="0" Width="200px">
                            <CellStyle Font-Names="Segoe UI" Font-Size="8pt">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Menu ID" FieldName="MenuID" Name="MenuID"
                            VisibleIndex="1" Width="100px">
                            <CellStyle Font-Names="Segoe UI" Font-Size="8pt" HorizontalAlign="Left">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Menu Name" FieldName="MenuDesc" 
                            Name="MenuDesc" VisibleIndex="2" Width="320px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataCheckColumn Caption="" FieldName="AllowAccess" Name="AllowAccess"
                            VisibleIndex="3" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String" 
                                                    ValueUnchecked="0" AllowGrayedByClick="false">
                                                </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                            <dx:ASPxCheckBox ID="chkAccess" runat="server" ClientInstanceName="chkAccess" ClientSideEvents-CheckedChanged="OnAccessCheckedChanged" ValueType="System.String" ValueChecked="1" ValueUnchecked="0" Text="Access" Font-Names="Segoe UI" Font-Size="8pt" ForeColor="White"> 
                            </dx:ASPxCheckBox>
                            </HeaderCaptionTemplate> 
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>
                        <dx:GridViewDataCheckColumn Caption="" FieldName="AllowUpdate" Name="AllowUpdate"
                            VisibleIndex="4" Width="80px">
                            <PropertiesCheckEdit ValueChecked="1" ValueType="System.String" 
                                                    ValueUnchecked="0" AllowGrayedByClick="false">
                                                </PropertiesCheckEdit>
                            <HeaderCaptionTemplate>
                            <dx:ASPxCheckBox ID="chkUpdate" runat="server" ClientInstanceName="chkUpdate" ClientSideEvents-CheckedChanged="OnUpdateCheckedChanged" ValueType="System.String" ValueChecked="1" ValueUnchecked="0" Text="Update" Font-Names="Segoe UI" Font-Size="8pt" ForeColor="White"> 
                            </dx:ASPxCheckBox>
                            </HeaderCaptionTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataCheckColumn>
                    </Columns>
                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" ColumnResizeMode="Control" EnableRowHotTrack="True" />
                    <SettingsPager Mode="ShowAllRecords" NumericButtonCount="10">
                    </SettingsPager>
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="False" />
                    </SettingsEditing>
                    <Settings HorizontalScrollBarMode="Visible" ShowStatusBar="Hidden" ShowVerticalScrollBar="True"
                        VerticalScrollableHeight="320" VerticalScrollBarMode="Visible" />
                                        <Styles>
                                            <Header HorizontalAlign="Center">
                                                <Paddings PaddingBottom="5px" PaddingTop="5px" />
                                            </Header>
                                        </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                </dx:ASPxGridView>   

                                <div style="height:10px">
                                    
                                    </div>
                                <div align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="False" ClientInstanceName="btnSave"
                                                    Font-Names="Segoe UI" Font-Size="8pt" Text="Save" Theme="Office2010Silver" 
                                                    Width="80px">
                                                    <ClientSideEvents Click="SaveData" />
                                                </dx:ASPxButton>               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            
                                                <dx:ASPxCallback ID="cbkValid" runat="server" ClientInstanceName="cbkValid">
                                                    <ClientSideEvents EndCallback="SavePrivilege" />
                                                </dx:ASPxCallback>
                                            
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                    </form>
                                </div>
							    <!-- end widget content -->
							</div>
						<!-- end widget div -->
						</div>
					    <!-- end widget -->
                    </article>
                    <!-- END COL -->
                </div>
                <!-- END ROW -->
            </section>
        </div>
        <!-- END MAIN CONTENT -->
    </div>
    <!-- END MAIN PANEL -->
    <!-- PACE LOADER - turn this on if you want ajax loading to show (caution: uses lots of memory on iDevices)-->
    <script data-pace-options='{ "restartOnRequestAfter": true }' src="js/plugin/pace/pace.min.js"></script>
    <!-- Link to Google CDN's jQuery + jQueryUI; fall back to local -->
    <!--<script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>-->
    <script>
        if (!window.jQuery) {
            document.write('<script src="js/libs/jquery-2.1.1.min.js"><\/script>');
        }
    </script>
    <!--<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js"></script>-->
    <script>
        if (!window.jQuery.ui) {
            document.write('<script src="js/libs/jquery-ui-1.10.3.min.js"><\/script>');
        }
    </script>
    <!-- IMPORTANT: APP CONFIG -->
    <script src="js/app.config.js"></script>
    <!-- JS TOUCH : include this plugin for mobile drag / drop touch events-->
    <script src="js/plugin/jquery-touch/jquery.ui.touch-punch.min.js"></script>
    <!-- BOOTSTRAP JS -->
    <script src="js/bootstrap/bootstrap.min.js"></script>
    <!-- CUSTOM NOTIFICATION -->
    <script src="js/notification/SmartNotification.min.js"></script>
    <!-- JARVIS WIDGETS -->
    <script src="js/smartwidgets/jarvis.widget.min.js"></script>
    <!-- EASY PIE CHARTS -->
    <script src="js/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js"></script>
    <!-- SPARKLINES -->
    <script src="js/plugin/sparkline/jquery.sparkline.min.js"></script>
    <!-- JQUERY VALIDATE -->
    <script src="js/plugin/jquery-validate/jquery.validate.min.js"></script>
    <!-- JQUERY MASKED INPUT -->
    <script src="js/plugin/masked-input/jquery.maskedinput.min.js"></script>
    <!-- JQUERY SELECT2 INPUT -->
    <script src="js/plugin/select2/select2.min.js"></script>
    <!-- JQUERY UI + Bootstrap Slider -->
    <script src="js/plugin/bootstrap-slider/bootstrap-slider.min.js"></script>
    <!-- browser msie issue fix -->
    <script src="js/plugin/msie-fix/jquery.mb.browser.min.js"></script>
    <!-- FastClick: For mobile devices -->
    <script src="js/plugin/fastclick/fastclick.min.js"></script>
    <!-- Demo purpose only -->
    <%--<script src="js/demo.min.js"></script>--%>
    <!-- MAIN APP JS FILE -->
    <script src="js/app.min.js"></script>
    <!-- ENHANCEMENT PLUGINS : NOT A REQUIREMENT -->
    <!-- Voice command : plugin -->
    <script src="js/speech/voicecommand.min.js"></script>
    <!-- SmartChat UI : plugin -->
    <script src="js/smart-chat-ui/smart.chat.ui.min.js"></script>
    <script src="js/smart-chat-ui/smart.chat.manager.min.js"></script>
    <!-- PAGE RELATED PLUGIN(S) -->
    <!-- Flot Chart Plugin: Flot Engine, Flot Resizer, Flot Tooltip -->
    <script src="js/plugin/flot/jquery.flot.cust.min.js"></script>
    <script src="js/plugin/flot/jquery.flot.resize.min.js"></script>
    <script src="js/plugin/flot/jquery.flot.time.min.js"></script>
    <script src="js/plugin/flot/jquery.flot.tooltip.min.js"></script>
    <!-- Vector Maps Plugin: Vectormap engine, Vectormap language -->
    <script src="js/plugin/vectormap/jquery-jvectormap-1.2.2.min.js"></script>
    <script src="js/plugin/vectormap/jquery-jvectormap-world-mill-en.js"></script>
    <!-- Full Calendar -->
    <script src="js/plugin/moment/moment.min.js"></script>
    <script src="js/plugin/fullcalendar/jquery.fullcalendar.min.js"></script>

    <script src="Scripts/toastr.js"></script>
    <%--<script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>--%>
    <script>
        $(document).ready(function () {

            // DO NOT REMOVE : GLOBAL FUNCTIONS!
            pageSetUp();

            /*
            * PAGE RELATED SCRIPTS
            */

            $(".js-status-update a").click(function () {
                var selText = $(this).text();
                var $this = $(this);
                $this.parents('.btn-group').find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
                $this.parents('.dropdown-menu').find('li').removeClass('active');
                $this.parent().addClass('active');
            });

            /*
            * TODO: add a way to add more todo's to list
            */

            // initialize sortable
            $(function () {
                $("#sortable1, #sortable2").sortable({
                    handle: '.handle',
                    connectWith: ".todo",
                    update: countTasks
                }).disableSelection();
            });

            // check and uncheck
            $('.todo .checkbox > input[type="checkbox"]').click(function () {
                var $this = $(this).parent().parent().parent();

                if ($(this).prop('checked')) {
                    $this.addClass("complete");

                    // remove this if you want to undo a check list once checked
                    //$(this).attr("disabled", true);
                    $(this).parent().hide();

                    // once clicked - add class, copy to memory then remove and add to sortable3
                    $this.slideUp(500, function () {
                        $this.clone().prependTo("#sortable3").effect("highlight", {}, 800);
                        $this.remove();
                        countTasks();
                    });
                } else {
                    // insert undo code here...
                }

            })
            // count tasks
            function countTasks() {

                $('.todo-group-title').each(function () {
                    var $this = $(this);
                    $this.find(".num-of-tasks").text($this.next().find("li").size());
                });

            }

            /*
            * RUN PAGE GRAPHS
            */

            /* TAB 1: UPDATING CHART */
            // For the demo we use generated data, but normally it would be coming from the server

            var data = [], totalPoints = 200, $UpdatingChartColors = $("#updating-chart").css('color');

            function getRandomData() {
                if (data.length > 0)
                    data = data.slice(1);

                // do a random walk
                while (data.length < totalPoints) {
                    var prev = data.length > 0 ? data[data.length - 1] : 50;
                    var y = prev + Math.random() * 10 - 5;
                    if (y < 0)
                        y = 0;
                    if (y > 100)
                        y = 100;
                    data.push(y);
                }

                // zip the generated y values with the x values
                var res = [];
                for (var i = 0; i < data.length; ++i)
                    res.push([i, data[i]])
                return res;
            }

            // setup control widget
            var updateInterval = 1500;
            $("#updating-chart").val(updateInterval).change(function () {

                var v = $(this).val();
                if (v && !isNaN(+v)) {
                    updateInterval = +v;
                    $(this).val("" + updateInterval);
                }

            });

            // setup plot
            var options = {
                yaxis: {
                    min: 0,
                    max: 100
                },
                xaxis: {
                    min: 0,
                    max: 100
                },
                colors: [$UpdatingChartColors],
                series: {
                    lines: {
                        lineWidth: 1,
                        fill: true,
                        fillColor: {
                            colors: [{
                                opacity: 0.4
                            }, {
                                opacity: 0
                            }]
                        },
                        steps: false

                    }
                }
            };

            var plot = $.plot($("#updating-chart"), [getRandomData()], options);

            /* live switch */
            $('input[type="checkbox"]#start_interval').click(function () {
                if ($(this).prop('checked')) {
                    $on = true;
                    updateInterval = 1500;
                    update();
                } else {
                    clearInterval(updateInterval);
                    $on = false;
                }
            });

            function update() {
                if ($on == true) {
                    plot.setData([getRandomData()]);
                    plot.draw();
                    setTimeout(update, updateInterval);

                } else {
                    clearInterval(updateInterval)
                }

            }

            var $on = false;

            /*end updating chart*/

            /* TAB 2: Social Network  */

            $(function () {
                // jQuery Flot Chart
                var twitter = [[1, 27], [2, 34], [3, 51], [4, 48], [5, 55], [6, 65], [7, 61], [8, 70], [9, 65], [10, 75], [11, 57], [12, 59], [13, 62]], facebook = [[1, 25], [2, 31], [3, 45], [4, 37], [5, 38], [6, 40], [7, 47], [8, 55], [9, 43], [10, 50], [11, 47], [12, 39], [13, 47]], data = [{
                    label: "Twitter",
                    data: twitter,
                    lines: {
                        show: true,
                        lineWidth: 1,
                        fill: true,
                        fillColor: {
                            colors: [{
                                opacity: 0.1
                            }, {
                                opacity: 0.13
                            }]
                        }
                    },
                    points: {
                        show: true
                    }
                }, {
                    label: "Facebook",
                    data: facebook,
                    lines: {
                        show: true,
                        lineWidth: 1,
                        fill: true,
                        fillColor: {
                            colors: [{
                                opacity: 0.1
                            }, {
                                opacity: 0.13
                            }]
                        }
                    },
                    points: {
                        show: true
                    }
                }];

                var options = {
                    grid: {
                        hoverable: true
                    },
                    colors: ["#568A89", "#3276B1"],
                    tooltip: true,
                    tooltipOpts: {
                        //content : "Value <b>$x</b> Value <span>$y</span>",
                        defaultTheme: false
                    },
                    xaxis: {
                        ticks: [[1, "JAN"], [2, "FEB"], [3, "MAR"], [4, "APR"], [5, "MAY"], [6, "JUN"], [7, "JUL"], [8, "AUG"], [9, "SEP"], [10, "OCT"], [11, "NOV"], [12, "DEC"], [13, "JAN+1"]]
                    },
                    yaxes: {

                    }
                };

                var plot3 = $.plot($("#statsChart"), data, options);
            });

            // END TAB 2

            // TAB THREE GRAPH //
            /* TAB 3: Revenew  */

            $(function () {

                var trgt = [[1354586000000, 153], [1364587000000, 658], [1374588000000, 198], [1384589000000, 663], [1394590000000, 801], [1404591000000, 1080], [1414592000000, 353], [1424593000000, 749], [1434594000000, 523], [1444595000000, 258], [1454596000000, 688], [1464597000000, 364]], prft = [[1354586000000, 53], [1364587000000, 65], [1374588000000, 98], [1384589000000, 83], [1394590000000, 980], [1404591000000, 808], [1414592000000, 720], [1424593000000, 674], [1434594000000, 23], [1444595000000, 79], [1454596000000, 88], [1464597000000, 36]], sgnups = [[1354586000000, 647], [1364587000000, 435], [1374588000000, 784], [1384589000000, 346], [1394590000000, 487], [1404591000000, 463], [1414592000000, 479], [1424593000000, 236], [1434594000000, 843], [1444595000000, 657], [1454596000000, 241], [1464597000000, 341]], toggles = $("#rev-toggles"), target = $("#flotcontainer");

                var data = [{
                    label: "Target Profit",
                    data: trgt,
                    bars: {
                        show: true,
                        align: "center",
                        barWidth: 30 * 30 * 60 * 1000 * 80
                    }
                }, {
                    label: "Actual Profit",
                    data: prft,
                    color: '#3276B1',
                    lines: {
                        show: true,
                        lineWidth: 3
                    },
                    points: {
                        show: true
                    }
                }, {
                    label: "Actual Signups",
                    data: sgnups,
                    color: '#71843F',
                    lines: {
                        show: true,
                        lineWidth: 1
                    },
                    points: {
                        show: true
                    }
                }]

                var options = {
                    grid: {
                        hoverable: true
                    },
                    tooltip: true,
                    tooltipOpts: {
                        //content: '%x - %y',
                        //dateFormat: '%b %y',
                        defaultTheme: false
                    },
                    xaxis: {
                        mode: "time"
                    },
                    yaxes: {
                        tickFormatter: function (val, axis) {
                            return "$" + val;
                        },
                        max: 1200
                    }

                };

                plot2 = null;

                function plotNow() {
                    var d = [];
                    toggles.find(':checkbox').each(function () {
                        if ($(this).is(':checked')) {
                            d.push(data[$(this).attr("name").substr(4, 1)]);
                        }
                    });
                    if (d.length > 0) {
                        if (plot2) {
                            plot2.setData(d);
                            plot2.draw();
                        } else {
                            plot2 = $.plot(target, d, options);
                        }
                    }

                };

                toggles.find(':checkbox').on('change', function () {
                    plotNow();
                });
                plotNow()

            });

            /*
            * VECTOR MAP
            */

            data_array = {
                "US": 4977,
                "AU": 4873,
                "IN": 3671,
                "BR": 2476,
                "TR": 1476,
                "CN": 146,
                "CA": 134,
                "BD": 100
            };

            $('#vector-map').vectorMap({
                map: 'world_mill_en',
                backgroundColor: '#fff',
                regionStyle: {
                    initial: {
                        fill: '#c4c4c4'
                    },
                    hover: {
                        "fill-opacity": 1
                    }
                },
                series: {
                    regions: [{
                        values: data_array,
                        scale: ['#85a8b6', '#4d7686'],
                        normalizeFunction: 'polynomial'
                    }]
                },
                onRegionLabelShow: function (e, el, code) {
                    if (typeof data_array[code] == 'undefined') {
                        e.preventDefault();
                    } else {
                        var countrylbl = data_array[code];
                        el.html(el.html() + ': ' + countrylbl + ' visits');
                    }
                }
            });

            /*
            * FULL CALENDAR JS
            */

            if ($("#calendar").length) {
                var date = new Date();
                var d = date.getDate();
                var m = date.getMonth();
                var y = date.getFullYear();

                var calendar = $('#calendar').fullCalendar({

                    editable: true,
                    draggable: true,
                    selectable: false,
                    selectHelper: true,
                    unselectAuto: false,
                    disableResizing: false,

                    header: {
                        left: 'title', //,today
                        center: 'prev, next, today',
                        right: 'month, agendaWeek, agenDay' //month, agendaDay,
                    },

                    select: function (start, end, allDay) {
                        var title = prompt('Event Title:');
                        if (title) {
                            calendar.fullCalendar('renderEvent', {
                                title: title,
                                start: start,
                                end: end,
                                allDay: allDay
                            }, true // make the event "stick"
                            );
                        }
                        calendar.fullCalendar('unselect');
                    },

                    events: [{
                        title: 'All Day Event',
                        start: new Date(y, m, 1),
                        description: 'long description',
                        className: ["event", "bg-color-greenLight"],
                        icon: 'fa-check'
                    }, {
                        title: 'Long Event',
                        start: new Date(y, m, d - 5),
                        end: new Date(y, m, d - 2),
                        className: ["event", "bg-color-red"],
                        icon: 'fa-lock'
                    }, {
                        id: 999,
                        title: 'Repeating Event',
                        start: new Date(y, m, d - 3, 16, 0),
                        allDay: false,
                        className: ["event", "bg-color-blue"],
                        icon: 'fa-clock-o'
                    }, {
                        id: 999,
                        title: 'Repeating Event',
                        start: new Date(y, m, d + 4, 16, 0),
                        allDay: false,
                        className: ["event", "bg-color-blue"],
                        icon: 'fa-clock-o'
                    }, {
                        title: 'Meeting',
                        start: new Date(y, m, d, 10, 30),
                        allDay: false,
                        className: ["event", "bg-color-darken"]
                    }, {
                        title: 'Lunch',
                        start: new Date(y, m, d, 12, 0),
                        end: new Date(y, m, d, 14, 0),
                        allDay: false,
                        className: ["event", "bg-color-darken"]
                    }, {
                        title: 'Birthday Party',
                        start: new Date(y, m, d + 1, 19, 0),
                        end: new Date(y, m, d + 1, 22, 30),
                        allDay: false,
                        className: ["event", "bg-color-darken"]
                    }, {
                        title: 'Smartadmin Open Day',
                        start: new Date(y, m, 28),
                        end: new Date(y, m, 29),
                        className: ["event", "bg-color-darken"]
                    }],

                    eventRender: function (event, element, icon) {
                        if (!event.description == "") {
                            element.find('.fc-event-title').append("<br/><span class='ultra-light'>" + event.description + "</span>");
                        }
                        if (!event.icon == "") {
                            element.find('.fc-event-title').append("<i class='air air-top-right fa " + event.icon + " '></i>");
                        }
                    }
                });

            };

            /* hide default buttons */
            $('.fc-header-right, .fc-header-center').hide();

            // calendar prev
            $('#calendar-buttons #btn-prev').click(function () {
                $('.fc-button-prev').click();
                return false;
            });

            // calendar next
            $('#calendar-buttons #btn-next').click(function () {
                $('.fc-button-next').click();
                return false;
            });

            // calendar today
            $('#calendar-buttons #btn-today').click(function () {
                $('.fc-button-today').click();
                return false;
            });

            // calendar month
            $('#mt').click(function () {
                $('#calendar').fullCalendar('changeView', 'month');
            });

            // calendar agenda week
            $('#ag').click(function () {
                $('#calendar').fullCalendar('changeView', 'agendaWeek');
            });

            // calendar agenda day
            $('#td').click(function () {
                $('#calendar').fullCalendar('changeView', 'agendaDay');
            });

            /*
            * CHAT
            */

            $.filter_input = $('#filter-chat-list');
            $.chat_users_container = $('#chat-container > .chat-list-body')
            $.chat_users = $('#chat-users')
            $.chat_list_btn = $('#chat-container > .chat-list-open-close');
            $.chat_body = $('#chat-body');

            /*
            * LIST FILTER (CHAT)
            */

            // custom css expression for a case-insensitive contains()
            jQuery.expr[':'].Contains = function (a, i, m) {
                return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
            };

            function listFilter(list) {// header is any element, list is an unordered list
                // create and add the filter form to the header

                $.filter_input.change(function () {
                    var filter = $(this).val();
                    if (filter) {
                        // this finds all links in a list that contain the input,
                        // and hide the ones not containing the input while showing the ones that do
                        $.chat_users.find("a:not(:Contains(" + filter + "))").parent().slideUp();
                        $.chat_users.find("a:Contains(" + filter + ")").parent().slideDown();
                    } else {
                        $.chat_users.find("li").slideDown();
                    }
                    return false;
                }).keyup(function () {
                    // fire the above change event after every letter
                    $(this).change();

                });

            }

            // on dom ready
            listFilter($.chat_users);

            // open chat list
            $.chat_list_btn.click(function () {
                $(this).parent('#chat-container').toggleClass('open');
            })

            $.chat_body.animate({
                scrollTop: $.chat_body[0].scrollHeight
            }, 500);

        });

    </script>
    <!-- Your GOOGLE ANALYTICS CODE Below -->
    <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-XXXXXXXX-X']);
        _gaq.push(['_trackPageview']);

        (function () {
            //            var ga = document.createElement('script');
            //            ga.type = 'text/javascript';
            //            ga.async = true;
            //            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            //            var s = document.getElementsByTagName('script')[0];
            //            s.parentNode.insertBefore(ga, s);
        })();

    </script>
</body>
</html>

