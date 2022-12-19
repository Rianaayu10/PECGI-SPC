<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TroubleHistoryByLine.aspx.vb" Inherits="PECGI_SPC.TroubleHistoryByLine" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
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

        function ChangeFactory() {
            var FactoryCode = cboFactory.GetValue();
            var ItemType_Code = cboItemType.GetValue();
        }

        function ChangeItemType() {
            var FactoryCode = cboFactory.GetValue();
            var ItemType_Code = cboItemType.GetValue();
        }

        function OnInitDateFrom(s, e) {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            dtFromDate.SetValue(firstDay);
		}

        function Browse() {
            if (cboFactory.GetText() == "") {
                toastr.warning('Please select combo box Type!', 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                return;
            }

            else if (cboItemType.GetText() == "") {
                toastr.warning('Please select combo box Type!', 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
                return;
            }

            var Factory = cboFactory.GetValue();
            var Type = cboItemType.GetValue();

            var dFrom = new Date(dtFromDate.GetValue()),
                monthFrom = '' + (dFrom.getMonth() + 1),
                dayFrom = '' + dFrom.getDate(),
                yearFrom = dFrom.getFullYear();

            if (monthFrom.length < 2)
                monthFrom = '0' + monthFrom;
            if (dayFrom.length < 2)
                dayFrom = '0' + dayFrom;

            var DateFrom = [yearFrom, monthFrom, dayFrom].join('-');

            var dTo = new Date(dtToDate.GetValue()),
                monthTo = '' + (dTo.getMonth() + 1),
                dayTo = '' + dTo.getDate(),
                yearTo = dTo.getFullYear();

            if (monthTo.length < 2)
                monthTo = '0' + monthTo;
            if (dayTo.length < 2)
                dayTo = '0' + dayTo;

            var DateTo = [yearTo, monthTo, dayTo].join('-');

            GetChartMachineProcess(Factory, Type, DateFrom, DateTo);
            GetTableMachineProcess(Factory, Type, DateFrom, DateTo);

            GetChartItemCheck('', '', '', DateFrom, DateTo, '');
            GetTableItemCheck('', '', '', DateFrom, DateTo);
        }

        function DetailItemCheck(Line, LineDesc) {
            var Factory = cboFactory.GetValue();
            var Type = cboItemType.GetValue();

            var dFrom = new Date(dtFromDate.GetValue()),
                monthFrom = '' + (dFrom.getMonth() + 1),
                dayFrom = '' + dFrom.getDate(),
                yearFrom = dFrom.getFullYear();

            if (monthFrom.length < 2)
                monthFrom = '0' + monthFrom;
            if (dayFrom.length < 2)
                dayFrom = '0' + dayFrom;

            var DateFrom = [yearFrom, monthFrom, dayFrom].join('-');

            var dTo = new Date(dtToDate.GetValue()),
                monthTo = '' + (dTo.getMonth() + 1),
                dayTo = '' + dTo.getDate(),
                yearTo = dTo.getFullYear();

            if (monthTo.length < 2)
                monthTo = '0' + monthTo;
            if (dayTo.length < 2)
                dayTo = '0' + dayTo;

            var DateTo = [yearTo, monthTo, dayTo].join('-');

            GetChartItemCheck(Factory, Type, Line, DateFrom, DateTo, LineDesc);
            GetTableItemCheck(Factory, Type, Line, DateFrom, DateTo);
        }

        function GetChartMachineProcess(Factory, Type, DateFrom, DateTo) {
            $.ajax({
                url: 'TroubleHistoryByLine.aspx/GetChartMachineProcess',
                type: 'POST',
                data: '{Factory : "' + Factory + '", Type : "' + Type + '", DateFrom : "' + DateFrom + '", DateTo : "' + DateTo + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
				success: function (data, textStatus, jQxhr) {
                    document.getElementById('lblTitleChartMachineProcess').innerHTML = 'NG Count By Machine Process';
                    document.getElementById('lblSubTitleChartMachineProcess').innerHTML = 'Type : ' + cboItemType.GetText();
                    if (Factory == "" && Type == "") {
                        document.getElementById('lblDescsChartMachineProcess').innerHTML = 'Period : ' + ' - ' + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;n = ' + data.d.Max;
                    }
                    else {
                        document.getElementById('lblDescsChartMachineProcess').innerHTML = 'Period : ' + dtFromDate.GetText() + ' - ' + dtToDate.GetText() + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;n = ' + data.d.Max;
                    }

                    

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
							axisLabel: 'Quality NG',
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
                    $.plot($('#chartMachineProcess'), eval(data.d.data), options);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
		}

        function GetTableMachineProcess(Factory, Type, DateFrom, DateTo) {
            $.ajax({
                url: 'TroubleHistoryByLine.aspx/GetTableMachineProcess',
                type: 'POST',
                data: '{Factory : "' + Factory + '", Type : "' + Type + '", DateFrom : "' + DateFrom + '", DateTo : "' + DateTo + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data, textStatus, jQxhr) {
                    if (data.d != null) {
                        $("#tableMachineProcess").DataTable().clear();
                        $("#tableMachineProcess").DataTable().rows.add(data.d).draw();
                    } else {
                        $("#tableMachineProcess").DataTable().clear();
                        $("#tableMachineProcess").DataTable().clear().draw;
                        $("#tableMachineProcess").DataTable().rows().remove().draw();
                    }
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }

        function GetChartItemCheck(Factory, Type, Line, DateFrom, DateTo, LineDesc) {
            $.ajax({
                url: 'TroubleHistoryByLine.aspx/GetChartItemCheck',
                type: 'POST',
                data: '{Factory : "' + Factory + '", Type : "' + Type + '", Line : "' + Line + '", DateFrom : "' + DateFrom + '", DateTo : "' + DateTo + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data, textStatus, jQxhr) {
                    document.getElementById('lblTitleChartItemCheck').innerHTML = 'NG Count By Item Check';
                    document.getElementById('lblSubTitleChartItemCheck').innerHTML = 'Machine : ' + LineDesc;
                    if (Factory == "" && Type == "" && Line == "") {
                        document.getElementById('lblDescsChartItemCheck').innerHTML = 'Period : ' + ' - ' + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;n = ' + data.d.Max;
                    }
                    else {
                        document.getElementById('lblDescsChartItemCheck').innerHTML = 'Period : ' + dtFromDate.GetText() + ' - ' + dtToDate.GetText() + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;n = ' + data.d.Max;
                    }


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
                            axisLabel: 'Quality NG',
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
                    $.plot($('#chartItemCheck'), eval(data.d.data), options);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }

        function GetTableItemCheck(Factory, Type, Line, DateFrom, DateTo) {
            $.ajax({
                url: 'TroubleHistoryByLine.aspx/GetTableItemCheck',
                type: 'POST',
                data: '{Factory : "' + Factory + '", Type : "' + Type + '", Line : "' + Line + '", DateFrom : "' + DateFrom + '", DateTo : "' + DateTo + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data, textStatus, jQxhr) {
                    if (data.d != null) {
                        $("#tableItemCheck").DataTable().clear();
                        $("#tableItemCheck").DataTable().rows.add(data.d).draw();
                    } else {
                        $("#tableItemCheck").DataTable().clear();
                        $("#tableItemCheck").DataTable().clear().draw;
                        $("#tableItemCheck").DataTable().rows().remove().draw();
                    }
                    
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }
    </script>
	<style type="text/css">
		.flot {
			left: 0px;
			top: 0px;
			right: 0px;
			bottom: 0px;
			height:20vw;
			width :30vw;
        
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
			display:none;
		}
	</style>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
		
        $(document).ready(function () {
            cb.PerformCallback();
	        var responsiveHelper_dt_basic = undefined;

            var breakpointDefinition = {
                tablet: 1024,
                phone: 480
            };

            $("#tableMachineProcess").dataTable({
                bDestroy: true,
                "pageLength": 100,
                data: [],
                columns: [
                    {
                        "data": "No", "autoWidth": true, className: "txtAlignLeft"
                        //"data": "No", "autoWidth": true, className: "txtAlignLeft", "render": function (data, type, row) {
                        //    console.log(row.No);
                        //    //if (row.No == "") {
                        //    //    console.log("true");
                        //    //    return '2'
                        //    //}
                        //    //else {
                        //    //    return row.No
                        //    //}
                        //    //console.log(type);
                        //    //console.log(row);
                        //    //if (row.No != "") {
                        //    //    return row.No
                        //    //}
                        //    //else {
                        //    //    return 'a'
                        //    //}    
                        //}
                    },
                    {
                        "data": "Machine", "autoWidth": true, className: "txtAlignLeft", "render": function (data, type, row) {
                            return '<a href="javascript:void(0)" onclick="return DetailItemCheck(\'' + row.LineCode + '\', \'' + row.Machine + '\')" >' + row.Machine + '</a>'
                        }
                    },
                    { "data": "AbnormalFreq", "autoWidth": true, className: "txtAlignCenter" },
                    { "data": "LineCode", "autoWidth": false, Width: 0, className: "txtAlignCenter hidden" },
                ],
                rowCallback: function (row, data) { responsiveHelper_dt_basic.createExpandIcon(row) },
			    filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bLengthChange: false,
                bFilter: false,
                bSort: false,
                bPaginate: false,
                scrollX: false,
			    scrollX: "100%",
                scrollY:"300px",
                autoWidth: true,
                preDrawCallback: function () {
                // Initialize the responsive datatables helper once.
                if (!responsiveHelper_dt_basic) {
                    responsiveHelper_dt_basic = new ResponsiveDatatablesHelper($('#tableMachineProcess'), breakpointDefinition);
                    }
                },
                drawCallback: function (oSettings) {
                    responsiveHelper_dt_basic.respond();
                }
            });


            var responsiveHelper_dt_basic1 = undefined;
            var responsiveHelper_dt_basic_downtime1 = undefined;

            var breakpointDefinition1 = {
                tablet: 1024,
                phone: 480
            };

            $("#tableItemCheck").dataTable({
                bDestroy: true,
                "pageLength": 100,
                data: [],
                columns: [
                    { "data": "No", "autoWidth": true, className: "txtAlignLeft" },
                    { "data": "Machine", "autoWidth": true, className: "txtAlignLeft" },
                    { "data": "AbnormalFreq", "autoWidth": true, className: "txtAlignCenter" },
                    { "data": "LineCode", "autoWidth": false, Width: 0, className: "txtAlignCenter hidden" },
                ],
                rowCallback: function (row, data) { responsiveHelper_dt_basic1.createExpandIcon(row) },
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bLengthChange: false,
                bFilter: false,
                bSort: false,
                bPaginate: false,
                scrollX: false,
                scrollX: "100%",
                scrollY: "300px",
                autoWidth: true,
                preDrawCallback: function () {
                    // Initialize the responsive datatables helper once.
                    if (!responsiveHelper_dt_basic1) {
                        responsiveHelper_dt_basic1 = new ResponsiveDatatablesHelper($('#tableItemCheck'), breakpointDefinition1);
                    }
                },
                drawCallback: function (oSettings) {
                    responsiveHelper_dt_basic1.respond();
                }
            });


            var Factory = cboFactory.GetValue();
            var Type = cboItemType.GetValue();

            var dFrom = new Date(dtFromDate.GetValue()),
                monthFrom = '' + (dFrom.getMonth() + 1),
                dayFrom = '' + dFrom.getDate(),
                yearFrom = dFrom.getFullYear();

            if (monthFrom.length < 2)
                monthFrom = '0' + monthFrom;
            if (dayFrom.length < 2)
                dayFrom = '0' + dayFrom;

            var DateFrom = [yearFrom, monthFrom, dayFrom].join('-');

            var dTo = new Date(dtToDate.GetValue()),
                monthTo = '' + (dTo.getMonth() + 1),
                dayTo = '' + dTo.getDate(),
                yearTo = dTo.getFullYear();

            if (monthTo.length < 2)
                monthTo = '0' + monthTo;
            if (dayTo.length < 2)
                dayTo = '0' + dayTo;

            var DateTo = [yearTo, monthTo, dayTo].join('-');

            GetChartMachineProcess('', '', DateFrom, DateTo);
            GetTableMachineProcess('', '', DateFrom, DateTo);

            GetChartItemCheck('', '', '', DateFrom, DateTo, '');
            GetTableItemCheck('', '', '', DateFrom, DateTo);
        });
    </script>  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 0px 5px 5px 5px; padding-bottom: 20px; border-bottom: groove 1px">
        <table class="auto-style3">
            <tr style="height: 40px">
                <td>
                    <dx:ASPxLabel ID="lblFactory" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Factory">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboFactory" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboFactory" Width="120px">
                        <ClientSideEvents SelectedIndexChanged="ChangeFactory" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 30px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblPeriod" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Period">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxDateEdit ID="dtFromDate" runat="server" Theme="Office2010Black" AutoPostBack="false"
                        ClientInstanceName="dtFromDate" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="5" Width="100px">
                        <ClientSideEvents Init="OnInitDateFrom" />
                        <CalendarProperties>
                            <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="10px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="10px"></ButtonStyle>
                        </CalendarProperties>
                        <ButtonStyle Width="5px" Paddings-Padding="4px"></ButtonStyle>
                    </dx:ASPxDateEdit>
                </td>

                <td style="width: 10px">&nbsp;</td>

                <td style="width: 10px">
                    <dx:ASPxLabel ID="lblToDate" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="To">
                    </dx:ASPxLabel>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td style="width: 20px">
                    <dx:ASPxDateEdit ID="dtToDate" runat="server" Theme="Office2010Black" AutoPostBack="false"
                        ClientInstanceName="dtToDate" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="5" Width="100px">
                        <CalendarProperties>
                            <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="10px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="10px"></ButtonStyle>
                        </CalendarProperties>
                        <ButtonStyle Width="5px" Paddings-Padding="4px"></ButtonStyle>
                    </dx:ASPxDateEdit>
                </td>
                <td>
                    &nbsp;</td>

                <td style="width: 10px">&nbsp;</td>

                <td style="width: 20px">
                    &nbsp;</td>
                <td colspan="3">
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxLabel ID="lblItemType" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Type">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboItemType" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboItemType" Width="120px">
                        <ClientSideEvents SelectedIndexChanged="ChangeItemType" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 30px">&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxButton ID="btnBrowse" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Height="25px"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Browse" Theme="Office2010Silver" Width="80px">
                        <ClientSideEvents Click="Browse" />
                    </dx:ASPxButton>
                </td>

                <td style="width: 10px">&nbsp;</td>

                <td style="width: 10px">
                    &nbsp;</td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td style="width: 20px">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>

                <td style="width: 10px">&nbsp;</td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>

    </div>
	   <dx:ASPxCallback ID="cb" runat="server" ClientInstanceName="cb">
           <ClientSideEvents CallbackComplete="MessageError" />
	   </dx:ASPxCallback>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div style="padding: 20px 5px 5px 5px; padding-bottom: 20px;" class="widget-body">
    <!-- widget grid -->
		<section id="widget-grid" class="">
			<!-- START ROW -->
			<div class="row">
				<!-- NEW COL START -->
				<article class="col-sm-6 col-md-6 col-lg-6">
					<!-- Widget ID (each widget will need unique ID)-->
					<div class="jarviswidget" id="wid1" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-custombutton="false">
						<header>
							<h2>NG Count By Machine Process </h2>
						</header>
						<!-- widget div-->
						<div>
							<!-- widget edit box -->
							<div class="jarviswidget-editbox">
							<!-- This area used as dropdown edit box -->
							</div>
							
							<!-- end widget edit box -->
				
							<!-- widget content -->
							<div class="widget-body no-padding">
								<div class="row">
									<div class="col-xs-12">
										<table style="width:100%;" border="1">
											<tr>
												<td align="center">
													<div><h3 id="lblTitleChartMachineProcess"></h3><p id="lblSubTitleChartMachineProcess"></p> <p id="lblDescsChartMachineProcess"></p></div>
													<div id="chartMachineProcess" class="flot"></div>
												</td>
											</tr>
										</table>
									</div>
								</div>
								<div class="row">
									&nbsp
								</div>
								<div class="row">
									<div class="col-xs-12">
										<table id="tableMachineProcess" class="table table-striped table-bordered table-hover row-border" width:"100%" style="font-family:'Trebuchet MS'">
											<thead>
												<tr role="row">
													<th style="text-align:center" width="5%">
														No
													</th>
													<th style="text-align:center" width="70%">
														Machine Process
													</th>
													<th style="text-align:center" width="25%">
														NG Count
													</th>
													<th style="text-align:center;width:0%;display:none">
														1
													</th>
												</tr>
											</thead>
											<tbody></tbody>
										</table>
									</div>
								</div>
							</div>
							<!-- end widget content -->
						</div>
						<!-- end widget div -->
					</div>
					<!-- end widget -->
				</article>
				<!-- END COL -->

                <!-- NEW COL START -->
				<article class="col-sm-6 col-md-6 col-lg-6">
					<!-- Widget ID (each widget will need unique ID)-->
					<div class="jarviswidget" id="wid2" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-custombutton="false">
						<header>
							<h2>NG Count By Item Check</h2>
						</header>
						<!-- widget div-->
						<div>
							<!-- widget edit box -->
							<div class="jarviswidget-editbox">
							<!-- This area used as dropdown edit box -->
							</div>
							
							<!-- end widget edit box -->
				
							<!-- widget content -->
							<div class="widget-body no-padding">
								<div class="row">
									<div class="col-xs-12">
										<table style="width:100%;" border="1">
											<tr>
												<td align="center">
													<div><h3 id="lblTitleChartItemCheck"></h3><p id="lblSubTitleChartItemCheck"></p> <p id="lblDescsChartItemCheck"></p></div>
													<div id="chartItemCheck" class="flot"></div>
												</td>
											</tr>
										</table>
									</div>
								</div>
								<div class="row">
									&nbsp
								</div>
								<div class="row">
									<div class="col-xs-12">
										<table id="tableItemCheck" class="table table-striped table-bordered table-hover row-border" width:"100%" style="font-family:'Trebuchet MS'">
											<thead>
												<tr role="row">
													<th style="text-align:center" width="5%">
														No
													</th>
													<th style="text-align:center" width="70%">
														Item Check
													</th>
													<th style="text-align:center" width="25%">
														NG Count
													</th>
													<th style="text-align:center;width:0%;display:none">
														1
													</th>
												</tr>
											</thead>
											<tbody></tbody>
										</table>
									</div>
								</div>
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
		<!-- end widget grid -->
    </div>
</asp:Content>