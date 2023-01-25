<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ItemCheck.aspx.vb" Inherits="PECGI_SPC.ItemCheck" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .ButtonStyleNo * {
            background-color: #003fbe;
            color: #fff;
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

        function up_Browse() {
            var Freq = cboFreq.GetSelectedItem().value;
            Grid.PerformCallback('Load|' + Freq);
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderJavaScriptBody" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            gridHeight(100);

            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(25);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(200);
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
    <div id="divhead" style="padding: 5px 5px 5px 5px; border-bottom-color: #a9a9a9; border-bottom-style: inset; border-bottom-width: 1px">
    </div>

        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            EnableTheming="True" KeyFieldName="ItemCheckCode" Theme="Office2010Black" Width="100%"
            Font-Names="Segoe UI" Font-Size="9pt"
            OnRowValidating="Grid_RowValidating" OnStartRowEditing="Grid_StartRowEditing"
            OnRowInserting="Grid_RowInserting" OnRowDeleting="Grid_RowDeleting"
            OnAfterPerformCallback="Grid_AfterPerformCallback">
            <ClientSideEvents EndCallback="OnEndCallback" />
            <Columns>
                <dx:GridViewCommandColumn FixedStyle="Left"
                    VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true"
                    ShowNewButtonInHeader="true" ShowClearFilterButton="true" Width="80px">
                    <HeaderStyle Paddings-PaddingLeft="3px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="3px"></Paddings>
                    </HeaderStyle>
                </dx:GridViewCommandColumn>

                <dx:GridViewDataTextColumn Caption="Code" FieldName="ItemCheckCode" VisibleIndex="1"
                    Width="75px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="15" Width="55px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Item Check" FieldName="ItemCheck" VisibleIndex="2"
                    Width="250px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="50" Width="200px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Measuring Unit" FieldName="UnitMeasurement"
                    VisibleIndex="3" Width="75px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="15" Width="55px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                    <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                    <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="center" VerticalAlign="Middle"></CellStyle>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" VisibleIndex="4"
                    Width="200px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="50" Width="200px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataCheckColumn Caption="Second Measurement" FieldName="Measure2Cls" 
                    VisibleIndex="5" Width="60px">
                    <PropertiesCheckEdit ValueChecked="1" ValueType="System.Char" 
                        ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <Settings AllowSort="False" />
                </dx:GridViewDataCheckColumn>

                <dx:GridViewDataCheckColumn Caption="Active Status" FieldName="ActiveStatus" 
                    VisibleIndex="6" Width="60px">
                    <PropertiesCheckEdit ValueChecked="1" ValueType="System.Char" 
                        ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <Settings AllowSort="False" />
                </dx:GridViewDataCheckColumn>

                <dx:GridViewDataTextColumn Caption="Last User" FieldName="UpdateUser" VisibleIndex="7"
                    Width="100px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="15" Width="100px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Last Update" FieldName="UpdateDate" VisibleIndex="8"
                    Width="125px" Settings-AutoFilterCondition="Contains">
                    <PropertiesTextEdit MaxLength="15" Width="125px">
                        <Style HorizontalAlign="Left"></Style>
                    </PropertiesTextEdit>
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

            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
            <SettingsPager Mode="ShowPager" PageSize="50" AlwaysShowPager="true">
                <PageSizeItemSettings Visible="True" />
            </SettingsPager>
            <Settings ShowFilterRow="True" VerticalScrollBarMode="Auto"
                VerticalScrollableHeight="300" HorizontalScrollBarMode="Auto" />
            <SettingsText ConfirmDelete="Are you sure want to delete ?"></SettingsText>
            <SettingsPopup>
                <EditForm Modal="false" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="200" />
            </SettingsPopup>

            <Styles EditFormColumnCaption-Paddings-PaddingLeft="10px" EditFormColumnCaption-Paddings-PaddingRight="10px">
                <Header Wrap="True">
                    <Paddings Padding="2px"></Paddings>
                </Header>

                <EditFormColumnCaption Font-Size="9pt" Font-Names="Segoe UI">
                    <Paddings PaddingLeft="5px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                </EditFormColumnCaption>
            </Styles>

            <Templates>
                <EditForm>
                    <div style="padding: 15px 15px 15px 15px; width: 300px"">
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <table align="center">
                                <tr style="height:30px">
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Code" Width="90px"></dx:ASPxLabel>
                                    </td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editItemCheckCode" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemCheckCode">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Item Check</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editItemCheck" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="ItemCheck">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Measuring Unit</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editUnitMeasurement" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="UnitMeasurement">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr style="height:30px">
                                    <td>Description</td>                                
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editDescription" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Description">
                                        </dx:ASPxGridViewTemplateReplacement>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Second Measurement
                                    </td>
                                    <td>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxGridViewTemplateReplacement ID="editMeasure2Cls" ReplacementType="EditFormCellEditor"
                                                runat="server" ColumnID="Measure2Cls">
                                            </dx:ASPxGridViewTemplateReplacement>                                            
                                        </dx:LayoutItemNestedControlContainer>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Active Status
                                    </td>
                                    <td>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxGridViewTemplateReplacement ID="editActiveStatus" ReplacementType="EditFormCellEditor"
                                                runat="server" ColumnID="ActiveStatus">
                                            </dx:ASPxGridViewTemplateReplacement>                                            
                                        </dx:LayoutItemNestedControlContainer>                                    
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
        <dx:ASPxHiddenField ID="HF" runat="server" ClientInstanceName="HF"></dx:ASPxHiddenField>
    </div>
</asp:Content>
