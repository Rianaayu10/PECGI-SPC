<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ProductionSampleVerificationList.aspx.vb" Inherits="PECGI_SPC.ProductionSampleVerificationList" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
            if (s.cp_GridTot > 0) {
                console.log(s.cp_GridTot);
                btnExcel.SetEnabled(true);
            }
        }

        function OnInit(s, e) {
            if (s.cp_GridTot > 0) {
                console.log(s.cp_GridTot);
                btnExcel.SetEnabled(true);
            } else {
                btnExcel.SetEnabled(false);                
            }
            btnVerification.SetEnabled(false);
            btnViewSampleInput.SetEnabled(false);
        }

        function Browse() {
            var FactoryCode = cboFactory.GetValue();
            var ItemType_Code = cboItemType.GetValue();
            var LineCode = cboLineID.GetValue();
            var ItemCheck_Code = cboItemCheck.GetValue();
            var ProdDate_From = dtFromDate.GetText();
            var ProdDate_To = dtToDate.GetText();
            var MK = cboMK.GetValue();
            var QC = cboQC.GetValue();

            if (FactoryCode == null || ItemType_Code == null || LineCode == null || ItemCheck_Code == null || MK == null || QC == null) {
                toastr.warning('Please Fill All Combo Box Filter!', 'Warning');
                toastr.options.closeButton = false;
                toastr.options.debug = false;
                toastr.options.newestOnTop = false;
                toastr.options.progressBar = false;
                toastr.options.preventDuplicates = true;
            }
            //else if ((new Date(ProdDate_To).getTime()) >  (new Date(ProdDate_From).getTime())){
            //    toastr.warning('Date From Can Not Less Then Date To!', 'Warning');
            //    toastr.options.closeButton = false;
            //    toastr.options.debug = false;
            //    toastr.options.newestOnTop = false;
            //    toastr.options.progressBar = false;
            //    toastr.options.preventDuplicates = true;
            //}
            else {
                Grid.PerformCallback('Load|' + FactoryCode + '|' + ItemType_Code + '|' + LineCode + '|' + ItemCheck_Code + '|' + ProdDate_From + '|' + ProdDate_To + '|' + MK + '|' + QC);
            }
        }

        function Clear() {
            var today = new Date();
            dtFromDate.SetDate(today);
            dtToDate.SetDate(today);
            cboFactory.SetValue('');
            cboItemType.SetValue('');
            cboLineID.SetValue('');
            cboItemCheck.SetValue('');
            cboMK.SetValue('');
            cboQC.SetValue('');

            Grid.PerformCallback('Clear|');
            e.cancel = true;
        }

        function ChangeFactory() {
            var FactoryCode = cboFactory.GetValue();
            var ItemType_Code = cboItemType.GetValue();
            var LineCode = cboLineID.GetValue();
            cboLineID.PerformCallback(FactoryCode + '|' + ItemType_Code); //GET LIST LINE
            cboItemCheck.PerformCallback(FactoryCode + '|' + ItemType_Code + '|' + LineCode); //GET LIST ITEM CHECK
        }

        function ChangeItemType() {
            var FactoryCode = cboFactory.GetValue();
            var ItemType_Code = cboItemType.GetValue();
            var LineCode = cboLineID.GetValue();
            if (LineCode == null) {
                LineCode = ""
            }
            cboLineID.PerformCallback(FactoryCode + '|' + ItemType_Code);
            cboItemCheck.PerformCallback(FactoryCode + '|' + ItemType_Code + '|' + LineCode); //GET LIST ITEM CHECK
        }

        function ChangeLine() {
            var FactoryCode = cboFactory.GetValue();
            var LineCode = cboLineID.GetValue();
            var ItemType_Code = cboItemType.GetValue();
            cboItemCheck.PerformCallback(FactoryCode + '|' + ItemType_Code + '|' + LineCode); //GET LIST ITEM CHECK
        }

        var Content_SelectData = ''

        function GetSelectedFieldValuesCallback(values) {
            var result = ''
            var n = values.length;
            for (var j = 0; j < values[n-1].length; j++) {
                result += values[n-1][j] + '|';
            }
         
            Content_SelectData = result;
          /*  console.log(Content_SelectData);*/
            if (values.length == 0) {
                btnVerification.SetEnabled(false);
                btnViewSampleInput.SetEnabled(false);
            }
            else {
                btnVerification.SetEnabled(true);
                btnViewSampleInput.SetEnabled(true);
            }
        };


        function SelectionChanged(s, e) {
            if (e.isSelected) {
                if (s.GetSelectedRowCount() > 1 && previuosRowIndex != null) {
                    s.UnselectRowsByKey(previuosRowIndex);
                } 
                previuosRowIndex = s.GetRowKey(e.visibleIndex);
                s.GetSelectedFieldValues("SPCResultID;FactoryCode;ItemTypeCode;LineCode;ItemCheckCode;ProdDate;Shift;SequenceNo;ShowVerify", GetSelectedFieldValuesCallback);
            }
        }

        function Verification() {
         /*   console.log(Content_SelectData);*/
            let text = Content_SelectData;
            const myArray = text.split("|");
            var Factory = myArray[1];
            var ItemType = myArray[2];
            var Line = myArray[3];
            var ItemCheck = myArray[4];
            var ProdDate = myArray[5];
            var Shift = myArray[6];
            var Seq = myArray[7];
            var ShowVerify = myArray[8];

            var prmLine = cboLineID.GetValue();
            var prmItemCheck = cboItemCheck.GetValue();
            var prmFromDate = dtFromDate.GetText();
            var prmToDate = dtToDate.GetText();
            var prmMK = cboMK.GetValue();
            var prmQC = cboMK.GetValue();

            console.log(ShowVerify);

            window.open('ProdSampleVerification.aspx?menu=ProductionSampleVerificationList.aspx' + '&FactoryCode=' + Factory + '&ItemTypeCode=' + ItemType
                + '&Line=' + Line + '&ItemCheckCode=' + ItemCheck + '&ProdDate=' + ProdDate + '&Shift=' + Shift + '&Sequence=' + Seq + '&ShowVerify=' + ShowVerify
                + '&cboLine=' + prmLine + '&cboItemCheck=' + prmItemCheck + '&FromDate=' + prmFromDate + '&ToDate='+ prmToDate + '&MK=' +prmMK + '&QC=' +prmQC + '', '_self');           
        }

        function ViewSampleInput() {
            /*   console.log(Content_SelectData);*/
            let text = Content_SelectData;
            const myArray = text.split("|");
            var Factory = myArray[1];
            var ItemType = myArray[2];
            var Line = myArray[3];
            var ItemCheck = myArray[4];
            var ProdDate = myArray[5];
            var Shift = myArray[6];
            var Seq = myArray[7];
            var ShowVerify = myArray[8];

            window.open('ProdSampleInput.aspx?menu=prodSampleVerification.aspx' + '&FactoryCode=' + Factory + '&ItemTypeCode=' + ItemType
                + '&Line=' + Line + '&ItemCheckCode=' + ItemCheck + '&ProdDate=' + ProdDate + '&Shift=' + Shift + '&Sequence=' + Seq + '&ShowVerify=' + ShowVerify
                + '', '_blank');
        }

    </script>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            gridHeight(150);

            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(50);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(260);
                    $("#flscr").val("0");
                }
            })
        });

        function gridHeight(pF) {
            var h1 = 49;
            var p1 = 10;
            var h2 = 34;
            var p2 = 13;
            var h3 = $("#divhead").height();

            var hAll = h1 + p1 + h2 + p2 + h3 + pF;
            /* alert(h1 + p1 + h2 + p2 + h3);*/
            var height = Math.max(0, document.documentElement.clientHeight);
            Grid.SetHeight(height - hAll);
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 5px 5px 5px 5px; padding-bottom: 20px; border-bottom: groove">
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
                <td style="width: 20px">&nbsp;</td>
                <td colspan="3">
                    <dx:ASPxDateEdit ID="dtFromDate" runat="server" Theme="Office2010Black" AutoPostBack="false"
                        ClientInstanceName="dtFromDate" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="5">
                        <ClientSideEvents Init="OnInit" />
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

                <td>&nbsp;</td>
                <td style="width: 50px" align="center">
                    <dx:ASPxLabel ID="lblToDate" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="To">
                    </dx:ASPxLabel>
                </td>

                <td style="width: 20px">&nbsp;</td>
                <td colspan="3">
                    <dx:ASPxDateEdit ID="dtToDate" runat="server" Theme="Office2010Black" AutoPostBack="false"
                        ClientInstanceName="dtToDate" EditFormatString="dd MMM yyyy" DisplayFormatString="dd MMM yyyy"
                        Font-Names="Segoe UI" Font-Size="9pt" Height="25px" TabIndex="5">
                        <ClientSideEvents Init="OnInit" />
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

                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblItemCheck" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="Item Check">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboItemCheck" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboItemCheck">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxLabel ID="lblMK" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="MK Verification">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 20px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboMK" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True" Width="60px"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboMK">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>
                <td>&nbsp;</td>
                <td style="text-align: right">
                    <dx:ASPxLabel ID="lblQC" runat="server" Font-Names="Segoe UI" Font-Size="9pt" Text="QC Verification">
                    </dx:ASPxLabel>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxComboBox ID="cboQC" runat="server" Font-Names="Segoe UI" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                        Theme="Office2010Black" EnableTheming="True" Height="25px" EnableIncrementalFiltering="True" Width="60px"
                        TextField="CODENAME" ValueField="CODE" ClientInstanceName="cboQC">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Paddings-Padding="4px" Width="5px">
                        </ButtonStyle>
                    </dx:ASPxComboBox>
                </td>

                <td style="width: 10px">&nbsp;</td>
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
                        <ClientSideEvents Click="Clear" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding: 5px 5px 5px 5px; padding-top: 20px; padding-bottom: 10px">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnVerification" runat="server" AutoPostBack="False" ClientInstanceName="btnVerification" Height="25px"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Verification" Theme="Office2010Silver" Width="100px">
                        <ClientSideEvents Click="Verification" />
                    </dx:ASPxButton>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxButton ID="btnViewSampleInput" runat="server" AutoPostBack="False" ClientInstanceName="btnViewSampleInput" Height="25px"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="View Sample Input" Theme="Office2010Silver" Width="100px">
                         <ClientSideEvents Click="ViewSampleInput" />
                    </dx:ASPxButton>
                </td>
                <td style="width: 10px">&nbsp;</td>
                <td>
                    <dx:ASPxButton ID="btnExcel" runat="server" AutoPostBack="False" ClientInstanceName="btnExcel" Height="25px"
                        Font-Names="Segoe UI" Font-Size="9pt" Text="Excel" Theme="Office2010Silver" Width="100px">
                    </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxGridViewExporter ID="GridExport" runat="server" GridViewID="Grid">
                    </dx:ASPxGridViewExporter>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding: 5px 5px 5px 5px">
        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            EnableTheming="True" KeyFieldName="SPCResultID" Theme="Office2010Black"
            Width="100%" Font-Names="Segoe UI" Font-Size="9pt">
            <ClientSideEvents EndCallback="OnEndCallback" Init="OnInit" SelectionChanged="SelectionChanged" />
            <Columns>
                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="1" Width="30px" FixedStyle="Left" Caption=" ">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewCommandColumn>

                <dx:GridViewDataTextColumn FieldName="FactoryCode" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ItemTypeCode" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LineCode" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ItemCheckCode" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPCResultID" Visible="false">
                </dx:GridViewDataTextColumn>
                 <dx:GridViewDataTextColumn FieldName="Shift" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nMaxColor" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nMinColor" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="nAvgColor" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ResultColor" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CorStsColor" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="MKColor" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="QCColor" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ShowVerify" Width="0" VisibleIndex="0">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Date" VisibleIndex="2" Width="80px"
                    FieldName="ProdDate">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Shift" VisibleIndex="3" Width="35px"
                    FieldName="ShiftCode">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Seq" VisibleIndex="4" Width="35px"
                    FieldName="SequenceNo">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Item Check" VisibleIndex="5" Width="250px"
                    FieldName="ItemCheck">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Min" VisibleIndex="6" Width="50px"
                    FieldName="nMin">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Right" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Max" VisibleIndex="7" Width="50px"
                    FieldName="nMax">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Right" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Avg" VisibleIndex="8" Width="50px"
                    FieldName="nAvg">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Right" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="R" VisibleIndex="9" Width="50px"
                    FieldName="nR">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Right" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Correction Status" VisibleIndex="10" Width="70px"
                    FieldName="Cor_Sts">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Result" VisibleIndex="11" Width="55px"
                    FieldName="Result">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="SampleTime" VisibleIndex="12" Width="120px"
                    FieldName="SampleTime">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Operator" VisibleIndex="13" Width="100px"
                    FieldName="Operator">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewBandColumn Caption="Verification by MK" VisibleIndex="14" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Paddings-PaddingBottom="3px" HeaderStyle-Paddings-PaddingTop="3px">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="PIC" VisibleIndex="1" Width="100px"
                            FieldName="MK_PIC" HeaderStyle-Paddings-PaddingBottom="2px" HeaderStyle-Paddings-PaddingTop="2px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Time" VisibleIndex="2" Width="120px"
                            FieldName="MK_Time" HeaderStyle-Paddings-PaddingBottom="2px" HeaderStyle-Paddings-PaddingTop="2px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:GridViewBandColumn>

                <dx:GridViewBandColumn Caption="Verification by QC" VisibleIndex="15" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Paddings-PaddingBottom="3px" HeaderStyle-Paddings-PaddingTop="3px">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="PIC" VisibleIndex="1" Width="100px"
                            FieldName="QC_PIC" HeaderStyle-Paddings-PaddingBottom="2px" HeaderStyle-Paddings-PaddingTop="2px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Time" VisibleIndex="2" Width="120px"
                            FieldName="QC_Time" HeaderStyle-Paddings-PaddingBottom="2px" HeaderStyle-Paddings-PaddingTop="2px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:GridViewBandColumn>
            </Columns>
            <SettingsBehavior ColumnResizeMode="control"/>
            <SettingsPager Mode="ShowPager" PageSize="20" AlwaysShowPager="true" PageSizeItemSettings-Visible="true">
                <PageSizeItemSettings Visible="True"></PageSizeItemSettings>
            </SettingsPager>
            <Settings HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="300" />
            
        </dx:ASPxGridView>
    </div>

    <dx:ASPxHiddenField ID="HV" runat="server" ClientInstanceName="HV"></dx:ASPxHiddenField>
</asp:Content>
