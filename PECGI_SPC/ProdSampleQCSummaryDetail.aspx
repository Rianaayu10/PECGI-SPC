<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProdSampleQCSummaryDetail.aspx.vb" Inherits="PECGI_SPC.ProdSampleQCSummaryDetail" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .align {
            text-align: left;
        }
        .text {
            font-weight: bold;
            font-size: 9pt;
            font-family: Segoe UI;
        }
    </style>

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
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            gridHeight(135);
            
            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(25);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(250);
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
    <div style="padding: 10px 0px 0px 0px">
        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            EnableTheming="True" KeyFieldName="Link" Theme="Office2010Black" Width="100%"
            Font-Names="Segoe UI" Font-Size="9pt">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <Columns>
                <dx:GridViewDataHyperLinkColumn Caption=" " VisibleIndex="0" Width="75px" Settings-AutoFilterCondition="Contains" FieldName="Link" PropertiesHyperLinkEdit-Target="_blank" FixedStyle="Left">
                    <PropertiesHyperLinkEdit TextField="TextLink"/>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataHyperLinkColumn>

                <dx:GridViewDataTextColumn Caption="Factory" VisibleIndex="1" Width="75px" Settings-AutoFilterCondition="Contains" FieldName="Factory">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Type" VisibleIndex="2" Width="75px" Settings-AutoFilterCondition="Contains" FieldName="Type">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Machine" VisibleIndex="3" Width="135px" Settings-AutoFilterCondition="Contains" FieldName="Line">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Item Check" VisibleIndex="4" Width="250px" Settings-AutoFilterCondition="Contains" FieldName="ItemCheck">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Date" VisibleIndex="5" Width="85px" Settings-AutoFilterCondition="Contains" FieldName="Prod">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Shift" VisibleIndex="6" Width="55px" Settings-AutoFilterCondition="Contains" FieldName="Shift">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Sequence" VisibleIndex="7" Width="70px" Settings-AutoFilterCondition="Contains" FieldName="Seq">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="8" Width="70px" Settings-AutoFilterCondition="Contains" FieldName="Result">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="#####NOTUSE" VisibleIndex="9" Width="0px" Settings-AutoFilterCondition="Contains" FieldName="Seq">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                
            </Columns>

            <SettingsBehavior ColumnResizeMode="Control" />
            <SettingsPager Mode="ShowPager" PageSize="20" AlwaysShowPager="true">
                <PageSizeItemSettings Visible="True" />
            </SettingsPager>
            <Settings HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="225" />
        </dx:ASPxGridView>
    </div>

    <div style="padding: 5px 5px 5px 5px; padding-top: 20px;">
        <table style="width: 100%; height: 15px">
            <tr>
                <td style="background-color:yellow; width:0.2%; text-align:center">
                    <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text=" "/>
                </td>
                <td align="center" class="align" style="width:0.5%;padding-left:0.5%">
                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Delay" CssClass="text" />
                </td>

                <td style="background-color:red; width:0.2%; text-align:center; color:#fff">
                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="NG" />
                </td>
                <td align="center" class="align" style="width:0.5%;padding-left:0.5%">
                    <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Result NG" CssClass="text" />
                </td>

                <td style="background-color:gray; width:0.2%; text-align:center">
                    <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text=" " />
                </td>
                <td align="center" class="align" style="width:5%;padding-left:0.5%">
                    <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="No Production Plan" CssClass="text" />
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
