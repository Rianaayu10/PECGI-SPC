<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ProdSampleVerification.aspx.vb" Inherits="PECGI_SPC.ProdSampleVerification" %>

<%@ Register Assembly="DevExpress.XtraCharts.v20.2.Web, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        /*======== Initialitation R Chart ==========*/
        function InitRBar(s, e) {
            /*console.log(s.cpShow);*/
            var i = s.cpShow;
            var x = document.getElementById("chartRdiv");
            if (i == '1') {
                x.style.display = "block";
            } else {
                x.style.display = "none";
            }
        }

        /*======== Initialitation Grid X ==========*/
        function InitGrid(s, e) {
            HideValue.Set('IOT_URL', s.cp_URL);
            HideValue.Set('IOT_ProcessGroup', s.cp_ProcessGroup);
            HideValue.Set('IOT_LineGroup', s.cp_LineGroup);
            HideValue.Set('IOT_ProcessCode', s.cp_ProcessCode);
            HideValue.Set('IOT_ProcessTableLineCode', s.cp_ProcessTableLineCode);
            HideValue.Set('IOT_ItemCode', s.cp_ItemCode);
            HideValue.Set('IOT_InstructionNo', s.cp_InstructionNo);
            HideValue.Set('IOT_LotNo', s.cp_LotNo);

            var USL = s.cpUSL, LSL = s.cpLSL, UCL = s.cpUCL, LCL = s.cpLCL; CL = s.cpCL; CSCode = s.cpCSCode; RUCL = s.cpRUCL; ChartSetupCount = s.cpChartSetupCount;
            var MIN = s.cpMIN, MAX = s.cpMAX; AVG = s.cpAVG, R = s.cpR, C = s.cpC, NG = s.cpNG, XBarUCL = s.cpXBarUCL, XBarLCL = s.cpXBarLCL;
            var MINClr = s.cpMINClr, MAXClr = s.cpMAXClr, AVClr = s.cpAVGClr, RClr = s.cpRClr, C_Clr = s.cpC_Clr, NG_Clr = s.cpNG_Clr;

            if (ChartSetupCount > 0) {
                lblUSL.SetText(USL);
                lblLSL.SetText(LSL);
                lblUCL.SetText(UCL);
                lblLCL.SetText(LCL);
                lblCL.SetText(CL);
                lblXBarUCL.SetText(XBarUCL);
                lblXBarLCL.SetText(XBarLCL);
                lblRUCL.SetText(RUCL);
                lblMin.SetText(MIN);
                lblMax.SetText(MAX);
                lblAve.SetText(AVG);
                lblR.SetText(R);
                lblC.SetText(C);
                lblNG.SetText(NG);
                lblSpecChar.SetText(CSCode);

                document.getElementById('NG').style.backgroundColor = '' + NG_Clr + '';
                document.getElementById('C').style.backgroundColor = '' + C_Clr + '';
                document.getElementById('Min').style.backgroundColor = '' + MINClr + '';
                document.getElementById('Max').style.backgroundColor = '' + MAXClr + '';
                document.getElementById('Ave').style.backgroundColor = '' + AVClr + '';
                document.getElementById('R').style.backgroundColor = '' + RClr + '';

                if (s.cpCS == "1") {
                    document.getElementById("lblXBarControl").style.display = "";
                    document.getElementById("hdrXBarUCL").style.display = "";
                    document.getElementById("hdrXBarLCL").style.display = "";
                    document.getElementById("bdXBarUCL").style.display = "";
                    document.getElementById("bdXBarLCL").style.display = "";
                } else {
                    document.getElementById("lblXBarControl").style.display = "none";
                    document.getElementById("hdrXBarUCL").style.display = "none";
                    document.getElementById("hdrXBarLCL").style.display = "none";
                    document.getElementById("bdXBarUCL").style.display = "none";
                    document.getElementById("bdXBarLCL").style.display = "none";
                }
            } else {
                lblUSL.SetText('');
                lblLSL.SetText('');
                lblUCL.SetText('');
                lblLCL.SetText('');
                lblCL.SetText('');
                lblXBarUCL.SetText('');
                lblXBarLCL.SetText('');
                lblRUCL.SetText('');
                lblMin.SetText('');
                lblMax.SetText('');
                lblAve.SetText('');
                lblR.SetText('');
                lblC.SetText('');
                lblNG.SetText('');
                lblSpecChar.SetText('');

                document.getElementById('NG').style.backgroundColor = "white";
                document.getElementById('C').style.backgroundColor = "white";
                document.getElementById('Min').style.backgroundColor = "white";
                document.getElementById('Max').style.backgroundColor = "white";
                document.getElementById('Ave').style.backgroundColor = "white";
                document.getElementById('R').style.backgroundColor = "white";

                document.getElementById("lblXBarControl").style.display = "none";
                document.getElementById("hdrXBarUCL").style.display = "none";
                document.getElementById("hdrXBarLCL").style.display = "none";
                document.getElementById("bdXBarUCL").style.display = "none";
                document.getElementById("bdXBarLCL").style.display = "none";
            }

            //console.log(s.cp_Verify);

            if (s.cp_Verify == "1" && s.cp_AllowSkill == true) {
                btnVerification.SetEnabled(true);
            }
            else {
                btnVerification.SetEnabled(false);
            }

            if (s.cp_GridTot > 1) {
                btnExcel.SetEnabled(true);
                btnSPCSample.SetEnabled(true);
                btnIOTProcess.SetEnabled(true);
                btnIOTTraceability.SetEnabled(true);
                btnCorrectiveAction.SetEnabled(true);
            } else {
                btnExcel.SetEnabled(false);
                btnSPCSample.SetEnabled(false);
                btnIOTProcess.SetEnabled(false);
                btnIOTTraceability.SetEnabled(false);
                btnCorrectiveAction.SetEnabled(false);
            }

            /*Validasi button FTA (aktif hanya ketika FTA Statusnya = 1)*/

            if (s.cp_ValButtonFTA != "1") {
                btnCorrectiveAction.SetEnabled(false);
            } else {
                btnCorrectiveAction.SetEnabled(true);
            }

            /*----------------------------------------------------------*/

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
            HideValue.Set('FactoryCode', FactoryCode);
            cboProcessGroup.PerformCallback(FactoryCode);
        }

        function ChangeProcessGroup() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessGroup = cboProcessGroup.GetValue();
            HideValue.Set('ProcessGroup', ProcessGroup);
            cboLineGroup.PerformCallback(FactoryCode + '|' + ProcessGroup);
        }

        function ChangeLineGroup() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessGroup = cboProcessGroup.GetValue();
            var LineGroup = cboLineGroup.GetValue();
            HideValue.Set('LineGroup', LineGroup);
            cboProcessCode.PerformCallback(FactoryCode + '|' + ProcessGroup + '|' + LineGroup);
        }

        function ChangeProcessCode() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessCode = cboProcessCode.GetValue();
            HideValue.Set('ProcessCode', ProcessCode);
            cboLineID.PerformCallback(FactoryCode + '|' + ProcessCode);
        }

        function ChangeLine() {
            var FactoryCode = cboFactory.GetValue();
            var ProcessCode = cboProcessCode.GetValue();
            var LineCode = cboLineID.GetValue();
            HideValue.Set('LineCode', LineCode);
            cboItemType.PerformCallback(FactoryCode + '|' + ProcessCode + '|' + LineCode);
        }

        function ChangeItemType() {
            var FactoryCode = cboFactory.GetValue();
            var LineCode = cboLineID.GetValue();
            var ItemType_Code = cboItemType.GetValue();
            HideValue.Set('ItemType_Code', ItemType_Code);
            cboItemCheck.PerformCallback(FactoryCode + '|' + LineCode + '|' + ItemType_Code);
        }

        function ChangeItemCheck() {
            var FactoryCode = cboFactory.GetValue();
            var LineCode = cboLineID.GetValue();
            var ItemType_Code = cboItemType.GetValue();
            var ItemCheck_Code = cboItemCheck.GetValue();
            HideValue.Set('ItemCheck_Code', ItemCheck_Code);
            cboShift.PerformCallback(FactoryCode + '|' + ItemType_Code + '|' + LineCode + '|' + ItemCheck_Code);
        }

        function ChangeShift() {
            var FactoryCode = cboFactory.GetValue();
            var LineCode = cboLineID.GetValue();
            var ItemType_Code = cboItemType.GetValue();
            var ItemCheck_Code = cboItemCheck.GetValue();
            var ShiftCode = cboShift.GetValue();
            HideValue.Set('ShiftCode', ShiftCode);
            cboSeq.PerformCallback(FactoryCode + '|' + ItemType_Code + '|' + LineCode + '|' + ItemCheck_Code + '|' + ShiftCode);
        }

        function ChangeSeq() {
            var Seq = cboSeq.GetValue();
            HideValue.Set('Seq', Seq);
        }

        function ProdDateChange() {
            var ProdDate = dtProdDate.GetText();
            HideValue.Set('ProdDate', ProdDate);
        }

        function ChangeVarify() {
            var ShowVerify = cboShow.GetValue();
            HideValue.Set('ShowVerify', ShowVerify);
        }

        function EndCallback_GridX(s, e) {

            HideValue.Set('IOT_URL', s.cp_URL);
            HideValue.Set('IOT_ProcessGroup', s.cp_ProcessGroup);
            HideValue.Set('IOT_LineGroup', s.cp_LineGroup);
            HideValue.Set('IOT_ProcessCode', s.cp_ProcessCode);
            HideValue.Set('IOT_ProcessTableLineCode', s.cp_ProcessTableLineCode);
            HideValue.Set('IOT_ItemCode', s.cp_ItemCode);
            HideValue.Set('IOT_InstructionNo', s.cp_InstructionNo);
            HideValue.Set('IOT_LotNo', s.cp_LotNo);

            var USL = s.cpUSL, LSL = s.cpLSL, UCL = s.cpUCL, LCL = s.cpLCL; CL = s.cpCL; CSCode = s.cpCSCode; RUCL = s.cpRUCL; ChartSetupCount = s.cpChartSetupCount;
            var MIN = s.cpMIN, MAX = s.cpMAX; AVG = s.cpAVG, R = s.cpR, C = s.cpC, NG = s.cpNG, XBarUCL = s.cpXBarUCL, XBarLCL = s.cpXBarLCL;
            var MINClr = s.cpMINClr, MAXClr = s.cpMAXClr, AVClr = s.cpAVGClr, RClr = s.cpRClr, C_Clr = s.cpC_Clr, NG_Clr = s.cpNG_Clr;

            console.log(UCL);

            if (ChartSetupCount > 0) {
                lblUSL.SetText(USL);
                lblLSL.SetText(LSL);
                lblUCL.SetText(UCL);
                lblLCL.SetText(LCL);
                lblCL.SetText(CL);
                lblXBarUCL.SetText(XBarUCL);
                lblXBarLCL.SetText(XBarLCL);
                lblRUCL.SetText(RUCL);
                lblMin.SetText(MIN);
                lblMax.SetText(MAX);
                lblAve.SetText(AVG);
                lblR.SetText(R);
                lblC.SetText(C);
                lblNG.SetText(NG);
                lblSpecChar.SetText(CSCode);

                document.getElementById('NG').style.backgroundColor = '' + NG_Clr + '';
                document.getElementById('C').style.backgroundColor = '' + C_Clr + '';
                document.getElementById('Min').style.backgroundColor = '' + MINClr + '';
                document.getElementById('Max').style.backgroundColor = '' + MAXClr + '';
                document.getElementById('Ave').style.backgroundColor = '' + AVClr + '';
                document.getElementById('R').style.backgroundColor = '' + RClr + '';

                if (s.cpCS == "1") {
                    document.getElementById("lblXBarControl").style.display = "";
                    document.getElementById("hdrXBarUCL").style.display = "";
                    document.getElementById("hdrXBarLCL").style.display = "";
                    document.getElementById("bdXBarUCL").style.display = "";
                    document.getElementById("bdXBarLCL").style.display = "";
                } else {
                    document.getElementById("lblXBarControl").style.display = "none";
                    document.getElementById("hdrXBarUCL").style.display = "none";
                    document.getElementById("hdrXBarLCL").style.display = "none";
                    document.getElementById("bdXBarUCL").style.display = "none";
                    document.getElementById("bdXBarLCL").style.display = "none";
                }
            } else {
                lblUSL.SetText('');
                lblLSL.SetText('');
                lblUCL.SetText('');
                lblLCL.SetText('');
                lblCL.SetText('');
                lblXBarUCL.SetText('');
                lblXBarLCL.SetText('');
                lblRUCL.SetText('');
                lblMin.SetText('');
                lblMax.SetText('');
                lblAve.SetText('');
                lblR.SetText('');
                lblC.SetText('');
                lblNG.SetText('');
                lblSpecChar.SetText('');

                document.getElementById('NG').style.backgroundColor = "white";
                document.getElementById('C').style.backgroundColor = "white";
                document.getElementById('Min').style.backgroundColor = "white";
                document.getElementById('Max').style.backgroundColor = "white";
                document.getElementById('Ave').style.backgroundColor = "white";
                document.getElementById('R').style.backgroundColor = "white";

                document.getElementById("lblXBarControl").style.display = "none";
                document.getElementById("hdrXBarUCL").style.display = "none";
                document.getElementById("hdrXBarLCL").style.display = "none";
                document.getElementById("bdXBarUCL").style.display = "none";
                document.getElementById("bdXBarLCL").style.display = "none";
            }

            if (s.cp_Verify == "1" && s.cp_AllowSkill == true) {
                btnVerification.SetEnabled(true);
            } else {
                btnVerification.SetEnabled(false);
            }

            if (s.cp_GridTot > 1) {
                btnExcel.SetEnabled(true);
                btnSPCSample.SetEnabled(true);
                btnIOTProcess.SetEnabled(true);
                btnIOTTraceability.SetEnabled(true);
                btnCorrectiveAction.SetEnabled(true);
            } else {
                btnExcel.SetEnabled(false);
                btnSPCSample.SetEnabled(false);
                btnIOTProcess.SetEnabled(false);
                btnIOTTraceability.SetEnabled(false);
                btnCorrectiveAction.SetEnabled(false);
            }
            /*Validasi button FTA (aktif hanya ketika FTA Statusnya = 1)*/

            if (s.cp_ValButtonFTA != "1") {
                btnCorrectiveAction.SetEnabled(false);
            } else {
                btnCorrectiveAction.SetEnabled(true);
            }

            /*----------------------------------------------------------*/

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

        function EndCallback_Grid(s, e) {
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

        function ChartREndCallBack(s, e) {
            var i = s.cpShow;
            var x = document.getElementById("chartRdiv");
            if (i == '1') {
                x.style.display = "block";
            } else {
                x.style.display = "none";
            }
        }

        function Browse() {
            chartX.PerformCallback();
            chartR.PerformCallback();
            GridX.PerformCallback('Load|');
            Grid.PerformCallback('Load|');
        }

        function Clear(s, e) {
            GridX.PerformCallback('Clear|');
            Grid.PerformCallback('Clear|');
            millisecondsToWait = 1000;
            setTimeout(function () {
                var today = new Date();
                dtProdDate.SetDate(today);
                cboFactory.SetValue('');
                cboProcessGroup.SetValue('');
                cboLineGroup.SetValue('');
                cboProcessCode.SetValue('');
                cboItemType.SetValue('');
                cboLineID.SetValue('');
                cboItemCheck.SetValue('');
                cboShift.SetValue('');
                cboSeq.SetValue('');

                lblUSL.SetText('');
                lblLSL.SetText('');
                lblUCL.SetText('');
                lblLCL.SetText('');
                lblXBarUCL.SetText('');
                lblXBarLCL.SetText('');
                lblMin.SetText('');
                lblMax.SetText('');
                lblAve.SetText('');
                lblR.SetText('');
                lblC.SetText('');
                lblNG.SetText('');

                HideValue.Set('ProdDate', today);
            }, millisecondsToWait);
            e.cancel = true;
        }

        function Verify() {
            GridX.PerformCallback('Verify');
        }

        function Back() {
            var menu = HideValue.Get("prm_menu");
            if (menu == "ProductionSampleVerificationList.aspx") {
                var Factory = HideValue.Get("prm_factory");
                var ItemType = HideValue.Get("prm_ItemType");
                var Line = HideValue.Get("prm_Line");
                var ItemCheck = HideValue.Get("prm_ItemCheck");
                var FromDate = HideValue.Get("prm_FromDate");
                var ToDate = HideValue.Get("prm_ToDate");
                var MK = HideValue.Get("prm_MK");
                var QC = HideValue.Get("prm_QC");

                window.open('ProductionSampleVerificationList.aspx?menu=prodSampleVerification.aspx' + '&FactoryCode=' + Factory + '&ItemTypeCode=' + ItemType
                    + '&Line=' + Line + '&ItemCheckCode=' + ItemCheck + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&MK=' + MK + '&QC=' + QC + '', '_self');
            }
            else if (menu == "SPCDashboard.aspx") {
                window.open('SPCDashboard.aspx', '_self');
            } else if (menu == "AlertDelayVerification.aspx") {
                var Factory = HideValue.Get("prm_factory");
                var FilterDate = HideValue.Get("prm_FilterDate");
                window.open('AlertDelayVerification.aspx?FactoryCode=' + Factory + '&FilterDate=' + FilterDate + '', '_self');
            }
        }

        function SPCSample() {

            var Factory = HideValue.Get('FactoryCode');
            var ItemType = HideValue.Get('ItemType_Code');
            var Line = HideValue.Get('LineCode');
            var ItemCheck = HideValue.Get('ItemCheck_Code');
            var ProdDate = HideValue.Get('ProdDate');
            var Shift = HideValue.Get('ShiftCode');
            var Seq = HideValue.Get('Seq');

            /*console.log(ProdDate);*/
            window.open('ProdSampleInput.aspx?menu=prodSampleVerification.aspx' + '&FactoryCode=' + Factory + '&ItemTypeCode=' + ItemType
                + '&Line=' + Line + '&ItemCheckCode=' + ItemCheck + '&ProdDate=' + ProdDate + '&Shift=' + Shift + '&Sequence=' + Seq
                + '', '_blank');
        }

        function CorrectiveAction() {
            var Factory = HideValue.Get('FactoryCode');
            var ItemType = HideValue.Get('ItemType_Code');
            var Line = HideValue.Get('LineCode');
            var ItemCheck = HideValue.Get('ItemCheck_Code');
            var ProdDate = HideValue.Get('ProdDate');
            var Shift = HideValue.Get('ShiftCode');
            var Seq = HideValue.Get('Seq');

            /*console.log(ProdDate);*/
            window.open('FTACorrectiveAction.aspx?FactoryCode=' + Factory + '&ItemTypeCode=' + ItemType
                + '&Line=' + Line + '&ItemCheckCode=' + ItemCheck + '&ProdDate=' + ProdDate + '&Shift=' + Shift + '&Sequence=' + Seq
                + '', '_blank');
        }

        function IOTProcess() {

            var URL = HideValue.Get('IOT_URL')
            var FactoryCode = HideValue.Get('FactoryCode')
            var ProcessGroup = HideValue.Get('IOT_ProcessGroup')
            var LineGroup = HideValue.Get('IOT_LineGroup')
            var Process = HideValue.Get('IOT_ProcessCode')
            var Line = HideValue.Get('IOT_ProcessTableLineCode')
            var Shift = HideValue.Get('ShiftCode')
            var Item = HideValue.Get('IOT_ItemCode')
            var InstructionNo = HideValue.Get('IOT_InstructionNo')
            var months = {
                Jan: "01", Feb: "02", Mar: "03", Apr: "04", May: "05", Jun: "06",
                Jul: "07", Aug: "08", Sep: "09", Oct: "10", Nov: "11", Dec: "12"
            };
            var p = HideValue.Get('ProdDate').split(' ');
            var Date = p[2] + "-" + months[p[1]] + "-" + p[0];

            var IOT_URL = URL + 'AssyReport/Index?ReportType=018&Factory=' + FactoryCode + '&ProcessGroup=' + ProcessGroup + '&LineGroup=' + LineGroup + '&Process=' + Process + '&Line=' + Line + '&Date=' + Date + '&InstructionNo=' + InstructionNo + '&Shift=' + Shift + '&Item=' + Item + '&UserID=SPC';
            window.open(IOT_URL, '_blank');
        }

        function IOTTraceability() {

            var URL = HideValue.Get('IOT_URL')
            var Item = HideValue.Get('IOT_ItemCode')
            var LotNo = HideValue.Get('IOT_LotNo')

            var IOT_URL = URL + 'TraceabilityReport/Index?ItemCls=02&Item=' + Item + '&LotNo=' + LotNo + '&isExplosion=1&UserID=SPC';
            window.open(IOT_URL, '_blank');
        }

        function ClosePopupRule1(s, e) {
            pcRule1.Hide();
            e.processOnServer = false;
        }

        function ClosePopupRule2(s, e) {
            pcRule2.Hide();
            e.processOnServer = false;
        }

        function ShowPopUpRule1(s, e) {
            pcRule1.Show();
        }

        function ShowPopUpRule2(s, e) {
            pcRule2.Show();
        }
    </script>
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 5px 5px 5px 5px; padding-bottom: 10px; border-bottom: groove">
        <table class="auto-style3">
            <tr style="height: 35px">
                <td>
                    <dx:ASPxLabel ID="lblFactory" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Factory">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboFactory" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboFactory">
                        <ClientSideEvents SelectedIndexChanged="ChangeFactory" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblProcessCode" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Machine">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboProcessCode" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboProcessCode">
                        <ClientSideEvents SelectedIndexChanged="ChangeProcessCode" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblItemCheck" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Item Check">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td colspan="3">
                    <dx:ASPxComboBox ID="cboItemCheck" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboItemCheck">
                        <ClientSideEvents SelectedIndexChanged="ChangeItemCheck" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td style="width: 110px">
                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Show Verified Only">
                    </dx:ASPxLabel>

                </td>
                <td style="width: 10px">&nbsp;</td>
                <td colspan="3">
                    <dx:ASPxComboBox ID="cboShow" runat="server" Theme="Office2010Black"
                        ClientInstanceName="cboShow" Font-Names="Segoe UI"
                        Font-Size="9pt" Height="25px"
                        Width="58px" TabIndex="9" SelectedIndex="0">
                        <ClientSideEvents ValueChanged="ChangeVarify" />
                        <Items>
                            <dx:ListEditItem Text="No" Value="0" Selected="true" />
                            <dx:ListEditItem Text="Yes" Value="1" />
                        </Items>
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                            <Paddings Padding="4px"></Paddings>
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr style="height: 35px">
                <td>
                    <dx:ASPxLabel ID="lblProcessGroup" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Process Group">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboProcessGroup" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboProcessGroup">
                        <ClientSideEvents SelectedIndexChanged="ChangeProcessGroup" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblLineID" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Machine Process">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboLineID" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboLineID">
                        <ClientSideEvents SelectedIndexChanged="ChangeLine" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblFromDate" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Prod. Date">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 25px">&nbsp;</td>
                <td colspan="3">
                    <dx:ASPxDateEdit ID="dtProdDate" runat="server" Theme="Office2010Black" AutoPostBack="false"
                        ClientInstanceName="dtProdDate" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="5">
                        <ClientSideEvents ValueChanged="ProdDateChange" />
                        <CalendarProperties>
                            <HeaderStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="10px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="10px"></ButtonStyle>
                        </CalendarProperties>
                        <ButtonStyle Width="5px" Paddings-Padding="4px"></ButtonStyle>
                    </dx:ASPxDateEdit>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td style="width: 10px">&nbsp;</td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxButton ID="btnBrowse" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Browse" Theme="Office2010Silver" Width="80px">
                        <ClientSideEvents Click="Browse" />
                    </dx:ASPxButton>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxButton ID="btnClear" runat="server" AutoPostBack="False" ClientInstanceName="btnClear"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Clear" Theme="Office2010Silver" Width="80px">
                        <ClientSideEvents Click="Clear" />
                    </dx:ASPxButton>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxButton ID="btnBack" runat="server" AutoPostBack="False" ClientInstanceName="btnBack" Height="25px"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Back" Theme="Office2010Silver" Width="80px">
                        <ClientSideEvents Click="Back" />
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr style="height: 35px">
                <td>
                    <dx:ASPxLabel ID="lblLineGroup" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Line Group">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboLineGroup" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboLineGroup">
                        <ClientSideEvents SelectedIndexChanged="ChangeLineGroup" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblItemType" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Type">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboItemType" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboItemType">
                        <ClientSideEvents SelectedIndexChanged="ChangeItemType" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td style="width: 10px"></td>
                <td>
                    <dx:ASPxLabel ID="lblShift" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Shift">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 25px"></td>
                <td>
                    <dx:ASPxComboBox ID="cboShift" runat="server" Font-Names="Segoe UI" Height="25px" DropDownStyle="DropDownList"
                        ClientInstanceName="cboShift" Theme="Office2010Black" TextField="CODENAME" ValueField="CODE" Width="80px"
                        EnableIncrementalFiltering="True">
                        <ClientSideEvents SelectedIndexChanged="ChangeShift" />
                        <ItemStyle Height="10px" Font-Size="11px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 30px" align="center">
                    <dx:ASPxLabel ID="lblSeq" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Seq">
                    </dx:ASPxLabel>
                </td>

                <td>
                    <dx:ASPxComboBox ID="cboSeq" runat="server" Font-Names="Segoe UI" Height="25px" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        ClientInstanceName="cboSeq" Theme="Office2010Black" EnableTheming="True" Width="60px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE">
                        <ClientSideEvents SelectedIndexChanged="ChangeSeq" />
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding: 5px 5px 5px 5px; padding-top: 10px; padding-bottom: 5px">
        <table style="width: 100%">
            <tr>
                <td>
                    <table>
                        <tr style="height:50px">
                            <td style="width: 100px">
                                <dx:ASPxButton ID="btnVerification" runat="server" AutoPostBack="False" ClientInstanceName="btnVerification" Height="40px"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="Verification" Theme="Office2010Silver" Width="100px">
                                    <ClientSideEvents Click="Verify" />
                                </dx:ASPxButton>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="width: 100px">
                                <dx:ASPxButton ID="btnSPCSample" runat="server" AutoPostBack="False" ClientInstanceName="btnSPCSample" Height="40px"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="SPC Sample Open New Tab" Wrap="True" Theme="Office2010Silver" Width="120px">
                                    <ClientSideEvents Click="SPCSample" />
                                </dx:ASPxButton>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="width: 100px">
                                <dx:ASPxButton ID="btnIOTProcess" runat="server" AutoPostBack="False" ClientInstanceName="btnIOTProcess" Height="40px"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="View IOT Process Table" Theme="Office2010Silver" Width="100px">
                                    <ClientSideEvents Click="IOTProcess" />
                                </dx:ASPxButton>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="width: 100px">
                                <dx:ASPxButton ID="btnIOTTraceability" runat="server" AutoPostBack="False" ClientInstanceName="btnIOTTraceability" Height="40px"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="View IOT Traceability" Theme="Office2010Silver" Width="100px">
                                    <ClientSideEvents Click="IOTTraceability" />
                                </dx:ASPxButton>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="width: 100px">
                                <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" ClientInstanceName="btnExcel" Height="40px"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="Excel" Theme="Office2010Silver" Width="100px">
                                </dx:ASPxButton>
                            </td>
                            <td style="width: 5px"></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <dx:ASPxButton ID="btnCorrectiveAction" runat="server" AutoPostBack="False" ClientInstanceName="btnCorrectiveAction" Height="40px"
                                    Font-Names="Segoe UI" Font-Size="9pt" Text="Corrective Action" Theme="Office2010Silver" Width="100px">
                                    <ClientSideEvents Click="CorrectiveAction" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>

                <td style="width: 120px"></td>
                <td style="width: 600px">
                    <table style="width: 100%">
                        <tr>
                            <td rowspan="3" class="body" align="center" style="width: 50px">
                                <dx:ASPxLabel ID="lblSpecChar" runat="server" Font-Names="Segoe UI" Font-Size="16pt" ClientInstanceName="lblSpecChar"></dx:ASPxLabel>
                            </td>
                            <td colspan="2" class="header">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Specification" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td colspan="3" class="header">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Control Plan" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td colspan="2" class="header" id="lblXBarControl">
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="X Bar Control" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header">
                                <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="R" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td colspan="6" class="header">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Result" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="USL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="LSL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="CL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="LCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px" id="hdrXBarUCL">
                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px" id="hdrXBarLCL">
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="LCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px" id="hdrRUCL">
                                <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="UCL" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Min" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Max" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Ave" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="header" style="width: 50px">
                                <dx:ASPxLabel ID="hASPxLabel14" runat="server" Text="R" Font-Names="Segoe UI" Font-Size="9pt"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" rowspan="2" style="width: 50px" id="C">
                                <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="" Font-Names="Segoe UI" Font-Size="Medium" Font-Bold="True" ForeColor="Black" ClientInstanceName="lblC"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" rowspan="2" style="width: 50px" id="NG">
                                <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="" Font-Names="Segoe UI" Font-Size="Medium" ClientInstanceName="lblNG" Font-Bold="True" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="body" align="center">
                                <dx:ASPxLabel ID="lblUSL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblUSL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center">
                                <dx:ASPxLabel ID="lblLSL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLSL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center">
                                <dx:ASPxLabel ID="lblUCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblUCL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center">
                                <dx:ASPxLabel ID="lblCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblCL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center">
                                <dx:ASPxLabel ID="lblLCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblLCL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" id="bdXBarUCL">
                                <dx:ASPxLabel ID="lblXBarUCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblXBarUCL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" id="bdXBarLCL">
                                <dx:ASPxLabel ID="lblXBarLCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblXBarLCL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center">
                                <dx:ASPxLabel ID="lblRUCL" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblRUCL" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" id="Min">
                                <dx:ASPxLabel ID="lblMin" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMin" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" id="Max">
                                <dx:ASPxLabel ID="lblMax" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblMax" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" id="Ave">
                                <dx:ASPxLabel ID="lblAve" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblAve" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                            <td class="body" align="center" id="R">
                                <dx:ASPxLabel ID="lblR" runat="server" Text=" " Font-Names="Segoe UI" Font-Size="9pt" ClientInstanceName="lblR" ForeColor="Black"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <div>
        <table style="width: 100%;">
            <tr>
                <td>
                    <div style="padding: 5px 5px 5px 5px;">
                        <dx:ASPxGridView ID="GridX" runat="server" AutoGenerateColumns="False" ClientInstanceName="GridX"
                            EnableTheming="True" KeyFieldName="nDesc" Theme="Office2010Black"
                            Width="100%" Font-Names="Segoe UI" Font-Size="9pt">
                            <ClientSideEvents EndCallback="EndCallback_GridX" Init="InitGrid" />
                            <Columns>
                                <dx:GridViewBandColumn Caption="Date" VisibleIndex="0">
                                    <Columns>
                                        <dx:GridViewBandColumn Caption="Shift" VisibleIndex="0">
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Time" FieldName="nDesc" VisibleIndex="0">
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>
                                    </Columns>
                                </dx:GridViewBandColumn>
                            </Columns>
                            <SettingsBehavior ColumnResizeMode="Control" />
                            <SettingsPager AlwaysShowPager="true" Mode="ShowAllRecords" PageSize="30">
                            </SettingsPager>
                            <Settings HorizontalScrollBarMode="Auto" VerticalScrollableHeight="500"
                                VerticalScrollBarMode="Auto" ShowStatusBar="Hidden" />
                            <Styles Header-Paddings-Padding="5px">
                                <Header HorizontalAlign="Center" Wrap="True">
                                    <Paddings Padding="2px" />
                                </Header>
                            </Styles>
                        </dx:ASPxGridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <table>
                            <tr>
                                <td style="padding: 10px 5px 10px 0px; width: 130px">
                                    <dx:ASPxButton ID="btnRule" runat="server" AutoPostBack="False"
                                        ClientInstanceName="btnRule" Font-Names="Segoe UI" Font-Size="9pt"
                                        Height="25px" Text="View SPC Rule (ENG)" Theme="Office2010Silver" UseSubmitBehavior="False"
                                        Width="120px" TabIndex="10">
                                        <Paddings Padding="2px" />
                                        <ClientSideEvents Click="ShowPopUpRule1" />
                                    </dx:ASPxButton>
                                </td>
                                <td style="padding: 10px 0px 10px 5px; width: 130px">
                                    <dx:ASPxButton ID="btnRule2" runat="server" AutoPostBack="False"
                                        ClientInstanceName="btnRule2" Font-Names="Segoe UI" Font-Size="9pt"
                                        Height="25px" Text="View SPC Rule (IND)" Theme="Office2010Silver" UseSubmitBehavior="False"
                                        Width="120px" TabIndex="10">
                                        <Paddings Padding="2px" />
                                        <ClientSideEvents Click="ShowPopUpRule2" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxPopupControl ID="pcRule1" runat="server" ClientInstanceName="pcRule1" Height="250px" Width="600px" HeaderText="SPC Rule" Modal="True"
                                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server">
                                                <div style="height: 100%; text-align: center; padding-top: 30px;">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/SPCRuleEN.png" />
                                                </div>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="text-align: center; padding-top: 10px;">
                                                            <dx:ASPxButton ID="btnHide" runat="server" AutoPostBack="False"
                                                                ClientInstanceName="btnHide" Font-Names="Segoe UI" Font-Size="9pt"
                                                                Height="25px" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False"
                                                                Width="90px" TabIndex="10">
                                                                <Paddings Padding="2px" />
                                                                <ClientSideEvents Click="ClosePopupRule1" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxPopupControl ID="pcRule2" runat="server" ClientInstanceName="pcRule2" Height="250px" Width="600px" HeaderText="SPC Rule" Modal="True"
                                        CloseAction="CloseButton" CloseOnEscape="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server">
                                                <div style="height: 100%; text-align: center; padding-top: 30px;">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/SPCRuleIN.png" />
                                                </div>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="text-align: center; padding-top: 10px;">
                                                            <dx:ASPxButton ID="btnHide2" runat="server" AutoPostBack="False"
                                                                ClientInstanceName="btnHide2" Font-Names="Segoe UI" Font-Size="9pt"
                                                                Height="25px" Text="Close" Theme="Office2010Silver" UseSubmitBehavior="False"
                                                                Width="90px" TabIndex="10">
                                                                <Paddings Padding="2px" />
                                                                <ClientSideEvents Click="ClosePopupRule2" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="padding: 5px 5px 5px 5px;">
                        <div id="chartXdiv" style="overflow-x: auto; width: 100%; border: 1px solid black">
                            <dx:WebChartControl ID="chartX" runat="server" ClientInstanceName="chartX"
                                Height="490px" Width="1080px" CrosshairEnabled="True" SeriesDataMember="Description" ToolTipEnabled="False">
                                <SeriesTemplate SeriesDataMember="Description" ArgumentDataMember="Seq" ValueDataMembersSerializable="Value" ToolTipPointPattern="{V:0.000}" CrosshairLabelPattern="{S}: {V:0.000}">
                                    <ViewSerializable>
                                        <cc1:PointSeriesView>
                                            <PointMarkerOptions Kind="Circle" BorderColor="255, 255, 255" Size="5"></PointMarkerOptions>
                                        </cc1:PointSeriesView>
                                    </ViewSerializable>
                                </SeriesTemplate>
                                <SeriesSerializable>
                                    <cc1:Series ArgumentDataMember="Seq" Name="Rule" ValueDataMembersSerializable="RuleValue" LabelsVisibility="False" ShowInLegend="False" ToolTipEnabled="False" ToolTipSeriesPattern="" CrosshairEnabled="False">
                                        <ViewSerializable>
                                            <cc1:FullStackedBarSeriesView BarWidth="1" Color="Red" Transparency="200" AxisYName="Secondary AxisY 1">
                                                <Border Visibility="False" />
                                            </cc1:FullStackedBarSeriesView>
                                        </ViewSerializable>
                                    </cc1:Series>
                                    <cc1:Series ArgumentDataMember="Seq" Name="RuleYellow" ValueDataMembersSerializable="RuleYellow" LabelsVisibility="False" ShowInLegend="False" ToolTipEnabled="False" ToolTipSeriesPattern="" CrosshairEnabled="False">
                                        <ViewSerializable>
                                            <cc1:FullStackedBarSeriesView BarWidth="1" Color="Yellow" Transparency="200">
                                            </cc1:FullStackedBarSeriesView>
                                        </ViewSerializable>
                                    </cc1:Series>
                                    <cc1:Series ArgumentDataMember="Seq" Name="XBar" ValueDataMembersSerializable="AvgValue" CrosshairLabelPattern="{S}: {V:0.000}">
                                        <ViewSerializable>
                                            <cc1:LineSeriesView Color="Blue">
                                                <LineStyle Thickness="1" />
                                                <LineMarkerOptions Color="Blue" Size="5" Kind="Diamond"></LineMarkerOptions>
                                            </cc1:LineSeriesView>
                                        </ViewSerializable>
                                    </cc1:Series>

                                </SeriesSerializable>
                                <DiagramSerializable>
                                    <cc1:XYDiagram>
                                        <AxisX VisibleInPanesSerializable="-1" MinorCount="1" Visibility="False">
                                            <Label Alignment="Center">
                                                <ResolveOverlappingOptions AllowHide="False" />
                                            </Label>
                                            <VisualRange Auto="False" MaxValueSerializable="9" MinValueSerializable="0" />
                                            <WholeRange AutoSideMargins="False" EndSideMargin="0.5" StartSideMargin="0.5" />
                                            <GridLines MinorVisible="True">
                                            </GridLines>
                                            <NumericScaleOptions AutoGrid="False" />
                                        </AxisX>
                                        <AxisY VisibleInPanesSerializable="-1" MinorCount="1">
                                            <Tickmarks MinorVisible="False" />
                                            <Label TextPattern="{V:0.000}" Font="Tahoma, 7pt">
                                                <ResolveOverlappingOptions AllowHide="True" />
                                            </Label>
                                            <VisualRange Auto="False" AutoSideMargins="False" EndSideMargin="0.015" MaxValueSerializable="2.715" MinValueSerializable="2.71" StartSideMargin="0.025" />
                                            <WholeRange AlwaysShowZeroLevel="False" Auto="False" AutoSideMargins="False" EndSideMargin="0.005" MaxValueSerializable="2.73" MinValueSerializable="2.71" StartSideMargin="0.005" />
                                            <GridLines>
                                                <LineStyle DashStyle="Dot" />
                                            </GridLines>
                                            <NumericScaleOptions AutoGrid="False" CustomGridAlignment="0.005" GridAlignment="Custom" />
                                        </AxisY>
                                        <SecondaryAxesY>
                                            <cc1:SecondaryAxisY Alignment="Near" AxisID="0" Name="Secondary AxisY 1" Visibility="False" VisibleInPanesSerializable="-1">
                                                <WholeRange AutoSideMargins="False" EndSideMargin="0" StartSideMargin="0" />
                                            </cc1:SecondaryAxisY>
                                        </SecondaryAxesY>
                                    </cc1:XYDiagram>
                                </DiagramSerializable>
                                <Titles>
                                    <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="" />
                                </Titles>
                                <Legend AlignmentHorizontal="Left" AlignmentVertical="BottomOutside"
                                    Direction="LeftToRight"></Legend>
                                <ToolTipOptions ShowForPoints="False" ShowForSeries="True">
                                </ToolTipOptions>
                            </dx:WebChartControl>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="padding: 5px 5px 5px 5px;">
                        <div id="chartRdiv">
                            <dx:WebChartControl ID="chartR" runat="server" ClientInstanceName="chartR"
                                Height="230px" Width="1080px" CrosshairEnabled="True">
                                <SeriesSerializable>
                                    <cc1:Series ArgumentDataMember="Seq" Name="R" ValueDataMembersSerializable="RValue">
                                        <ViewSerializable>
                                            <cc1:LineSeriesView Color="Blue">
                                                <LineStyle Thickness="1" />
                                                <LineMarkerOptions Size="7" Color="Blue" Kind="Diamond">
                                                </LineMarkerOptions>
                                            </cc1:LineSeriesView>
                                        </ViewSerializable>
                                    </cc1:Series>
                                    <cc1:Series Name="RuleYellow" ArgumentDataMember="Seq" ShowInLegend="False" ToolTipEnabled="False" ValueDataMembersSerializable="RuleYellow">
                                        <ViewSerializable>
                                            <cc1:FullStackedBarSeriesView BarWidth="1" Color="255, 255, 0" Transparency="100" AxisYName="Secondary AxisY 1">
                                                <Border Visibility="False" />
                                                <FillStyle FillMode="Solid">
                                                </FillStyle>
                                            </cc1:FullStackedBarSeriesView>
                                        </ViewSerializable>
                                    </cc1:Series>
                                    <cc1:Series ArgumentDataMember="Seq" Name="RuleRed" ShowInLegend="False" ValueDataMembersSerializable="RuleRed">
                                        <ViewSerializable>
                                            <cc1:FullStackedBarSeriesView BarWidth="1" Color="Pink" Transparency="100">
                                                <FillStyle FillMode="Solid">
                                                </FillStyle>
                                            </cc1:FullStackedBarSeriesView>
                                        </ViewSerializable>
                                    </cc1:Series>
                                </SeriesSerializable>
                                <SeriesTemplate ValueDataMembersSerializable="Value">
                                    <ViewSerializable>
                                        <cc1:LineSeriesView>
                                            <LineMarkerOptions BorderColor="White" Size="3">
                                            </LineMarkerOptions>
                                        </cc1:LineSeriesView>
                                    </ViewSerializable>
                                </SeriesTemplate>
                                <DiagramSerializable>
                                    <cc1:XYDiagram>
                                        <AxisX VisibleInPanesSerializable="-1" MinorCount="1" Visibility="False">
                                            <Tickmarks Visible="False" />
                                            <WholeRange AutoSideMargins="False" EndSideMargin="0.5" StartSideMargin="0.5" />
                                            <GridLines MinorVisible="True">
                                            </GridLines>
                                            <NumericScaleOptions AutoGrid="False" ScaleMode="Manual" />
                                        </AxisX>
                                        <AxisY VisibleInPanesSerializable="-1" MinorCount="1">
                                            <Tickmarks MinorLength="1" MinorVisible="False" />
                                            <Label TextAlignment="Near" TextPattern="{V:0.000}">
                                                <ResolveOverlappingOptions AllowHide="True" />
                                            </Label>
                                            <VisualRange Auto="False" AutoSideMargins="False" EndSideMargin="0.001" MaxValueSerializable="0.027" MinValueSerializable="0" StartSideMargin="0" />
                                            <WholeRange Auto="False" MaxValueSerializable="0.027" MinValueSerializable="0" AutoSideMargins="False" EndSideMargin="1" StartSideMargin="1" />
                                            <GridLines>
                                                <LineStyle DashStyle="Dot" />
                                            </GridLines>
                                            <NumericScaleOptions AutoGrid="False" CustomGridAlignment="0.001" GridAlignment="Custom" GridOffset="1" />
                                        </AxisY>
                                        <SecondaryAxesY>
                                            <cc1:SecondaryAxisY AxisID="0" Name="Secondary AxisY 1" Visibility="False" VisibleInPanesSerializable="-1">
                                                <Tickmarks MinorVisible="False" Visible="False" />
                                            </cc1:SecondaryAxisY>
                                        </SecondaryAxesY>
                                    </cc1:XYDiagram>
                                </DiagramSerializable>
                                <Titles>
                                    <cc1:ChartTitle Font="Segoe UI, 12pt, style=Bold" Text="R Control Chart" />
                                </Titles>
                                <Legend AlignmentHorizontal="Left" AlignmentVertical="BottomOutside"
                                    Direction="LeftToRight"></Legend>
                                <ClientSideEvents EndCallback="ChartREndCallBack" Init="InitRBar" />
                            </dx:WebChartControl>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>


    <div style="height: 26px; padding-bottom: 5px; padding-left: 450px; padding-top: 20px">
        <dx:ASPxLabel ID="lblGridActivity" runat="server" Text="ACTIVITY MONITORING"
            Font-Names="Segoe UI" Font-Size="12pt" Font-Bold="True"
            Font-Underline="True">
        </dx:ASPxLabel>
    </div>
    <div style="padding: 5px 5px 5px 5px; padding-bottom: 20px">
        <asp:SqlDataSource ID="dsUserSetup" runat="server"
            ConnectionString="<%$ConnectionStrings:ApplicationServices %>"
            SelectCommand="SELECT CODE = UserID, CODENAME = UserID FROM dbo.spc_UserSetup "></asp:SqlDataSource>

        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            OnRowValidating="Grid_Validating" EnableTheming="True" KeyFieldName="ActivityID" Theme="Office2010Black"
            Width="100%" Font-Names="Segoe UI" Font-Size="9pt"
            OnAfterPerformCallback="Grid_AfterPerformCallback">
            <ClientSideEvents EndCallback="EndCallback_Grid" />
            <Columns>
                <dx:GridViewCommandColumn FixedStyle="Left"
                    VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true" ShowNewButtonInHeader="true"
                    ShowClearFilterButton="true" Width="80px">
                    <HeaderStyle Paddings-PaddingLeft="3px" HorizontalAlign="Center"
                        VerticalAlign="Middle">
                        <Paddings PaddingLeft="3px"></Paddings>
                    </HeaderStyle>
                </dx:GridViewCommandColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="ActivityID" Visible="false">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataDateColumn Caption="Date" FieldName="ProdDate" Width="100px" Settings-AutoFilterCondition="Contains" VisibleIndex="0">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormat="Custom" EditFormatString="dd MMM yyyy"
                        MaxDate="9999-12-31" MinDate="2000-12-01">
                        <ButtonStyle Width="5px" Paddings-Padding="2px" />
                        <CalendarProperties>
                            <HeaderStyle Font-Size="12pt" Paddings-Padding="5px" />
                            <DayStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <WeekNumberStyle Font-Size="9pt" Paddings-Padding="5px"></WeekNumberStyle>
                            <FooterStyle Font-Size="9pt" Paddings-Padding="5px" />
                            <ButtonStyle Font-Size="9pt" Paddings-Padding="5px"></ButtonStyle>
                        </CalendarProperties>
                    </PropertiesDateEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataTextColumn Caption="Shift" Width="50px" FieldName="ShiftName" VisibleIndex="1">
                    <PropertiesTextEdit ClientInstanceName="ShiftName" Width="170px">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTimeEditColumn Caption="Time" FieldName="Time" VisibleIndex="2"
                    Width="50px" Settings-AutoFilterCondition="Contains">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <PropertiesTimeEdit DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm" Width="80px">
                        <ButtonStyle Width="5px" Paddings-Padding="4px"></ButtonStyle>
                    </PropertiesTimeEdit>
                    <SettingsHeaderFilter></SettingsHeaderFilter>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewDataTimeEditColumn>

                <dx:GridViewDataComboBoxColumn Caption="PIC" FieldName="PIC" VisibleIndex="3">
                    <PropertiesComboBox DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        DisplayFormatInEditMode="true" Width="170px" TextField="CODE"
                        ValueField="CODENAME" ClientInstanceName="PIC">
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataTextColumn Caption="Action" VisibleIndex="4" Width="200px"
                    FieldName="Action">
                    <PropertiesTextEdit ClientInstanceName="Action" Width="300px">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Remark" VisibleIndex="5" Width="200px"
                    FieldName="Remark">
                    <PropertiesTextEdit ClientInstanceName="Remark" Width="300px">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn Caption="Result" FieldName="Result"
                    VisibleIndex="6" Width="50px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DropDownStyle="DropDownList" Width="80px" TextFormatString="{0}"
                        IncrementalFilteringMode="StartsWith" DisplayFormatInEditMode="true">
                        <Items>
                            <dx:ListEditItem Text="OK" Value="0" />
                            <dx:ListEditItem Text="NG" Value="1" />
                        </Items>
                        <ItemStyle Height="10px" Paddings-Padding="4px">
                            <Paddings Padding="4px"></Paddings>
                        </ItemStyle>
                        <ButtonStyle Width="5px" Paddings-Padding="2px">
                            <Paddings Padding="2px"></Paddings>
                        </ButtonStyle>
                    </PropertiesComboBox>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <HeaderStyle Paddings-PaddingLeft="2px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="2px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataTextColumn Caption="Last User" VisibleIndex="7" Width="120px"
                    FieldName="LastUser">
                    <PropertiesTextEdit ClientInstanceName="LastUser">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Last Update" VisibleIndex="8" Width="100px"
                    FieldName="LastDate">
                    <PropertiesTextEdit ClientInstanceName="LastUpdate">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="FactoryName" Visible="false">
                    <PropertiesTextEdit ClientInstanceName="FactoryName" Width="170px">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="FactoryCode" Visible="false">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="ItemTypeName" Visible="false">
                    <PropertiesTextEdit ClientInstanceName="ItemTypeName" Width="170px">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="ItemTypeCode" Visible="false">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="LineName" Visible="false">
                    <PropertiesTextEdit ClientInstanceName="LineName" Width="170px">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="LineCode" Visible="false">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="ItemCheckName" Visible="false">
                    <PropertiesTextEdit ClientInstanceName="ItemCheckCode" Width="170px">
                    </PropertiesTextEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="ItemCheckCode">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#" Width="0px" FieldName="ShiftCode">
                </dx:GridViewDataTextColumn>

            </Columns>
            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
            <SettingsPager Mode="ShowPager" PageSize="50" AlwaysShowPager="true">
                <PageSizeItemSettings Visible="True">
                </PageSizeItemSettings>
            </SettingsPager>
            <Settings ShowFilterRow="false" VerticalScrollBarMode="Auto"
                VerticalScrollableHeight="300" HorizontalScrollBarMode="Auto" />
            <SettingsText ConfirmDelete="Are you sure want to delete ?"></SettingsText>
            <SettingsPopup>
                <EditForm Modal="false" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200" />
            </SettingsPopup>
            <Styles EditFormColumnCaption-Paddings-PaddingLeft="10px" EditFormColumnCaption-Paddings-PaddingRight="10px">
                <Header Wrap="true">
                    <Paddings Padding="2px"></Paddings>
                </Header>
                <Cell Wrap="true">
                </Cell>
                <EditFormColumnCaption Font-Size="9pt" Font-Names="Segoe UI">
                    <Paddings PaddingLeft="5px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                </EditFormColumnCaption>
            </Styles>

            <Templates>
                <EditForm>
                    <div style="padding: 15px 15px 15px 15px; width: 380px">
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <table align="center">
                                <tr style="height: 30px">
                                    <td>
                                        <dx:ASPxLabel ID="lblFactoryCode" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Factory" Width="80px"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditFactoryName" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="FactoryName"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                    <td style="visibility: hidden">
                                        <dx:ASPxGridViewTemplateReplacement ID="EditFactoryCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="FactoryCode"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Item Type</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditItemTypeName" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemTypeName"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                    <td style="visibility: hidden">
                                        <dx:ASPxGridViewTemplateReplacement ID="EditItemTypeCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemTypeCode"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Line</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditLineName" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="LineName"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                    <td style="visibility: hidden">
                                        <dx:ASPxGridViewTemplateReplacement ID="EditLineCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="LineCode"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Item Check</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditItemCheckName" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemCheckName"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                    <td style="visibility: hidden">
                                        <dx:ASPxGridViewTemplateReplacement ID="EditItemCheckCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemCheckCode"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Shift</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditShiftName" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ShiftName"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                    <td style="visibility: hidden">
                                        <dx:ASPxGridViewTemplateReplacement ID="EditShiftCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ShiftCode"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Prod Date</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditProdDate" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ProdDate"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Time</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditTime" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Time"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>PIC </td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditPIC" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="PIC"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Action</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditAction" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Action"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                    <td style="visibility: hidden">
                                        <dx:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ActivityID"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Remark</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditRemark" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Remark"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Result</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="EditResult" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Result"></dx:ASPxGridViewTemplateReplacement>
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

    <dx:ASPxHiddenField ID="HideValue" runat="server" ClientInstanceName="HideValue"></dx:ASPxHiddenField>

</asp:Content>
