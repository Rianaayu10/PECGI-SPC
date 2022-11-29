<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TimeFrequencySetting.aspx.vb" Inherits="PECGI_SPC.TimeFrequencySetting" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

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
            gridHeight(110);

            $("#fullscreen").click(function () {
                var fcval = $("#flscr").val();
                if (fcval == "0") { //toClickFullScreen
                    gridHeight(10);
                    $("#flscr").val("1");
                } else if (fcval == "1") { //toNormalFullScreen
                    gridHeight(220);
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
    <div id="divhead" style="padding: 5px 5px 5px 5px;">
        <table>
            <tr>
                <td style="padding-right: 1em">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Frequency" Theme="Office2010Black" Font-Names="Segoe UI" Font-Size="10pt">
                    </dx:ASPxLabel>
                </td>

                <td style="padding-right: 1em">
                    <dx:ASPxComboBox ID="cboFreq" runat="server" Theme="Office2010Black" Width="100px" Height="25px" ClientInstanceName="cboFreq"
                        EnableIncrementalFiltering="True" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains" TextField="Description" ValueField="Code">
                        <ItemStyle Height="10px" Paddings-Padding="4px" />
                        <ButtonStyle Width="5px" Paddings-Padding="4px" />
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
                            HF.Set('Code', s.GetSelectedItem().value);
                                    Grid.PerformCallback('Kosong');
                                }" />
                    </dx:ASPxComboBox>
                </td>

                <td style="padding-right: 1em">
                    <dx:ASPxButton ID="btnBrowse" runat="server" AutoPostBack="False" ClientInstanceName="btnBrowse" Theme="Office2010Silver" Height="28px"
                        Text="Browse" Font-Names="Segoe UI" Font-Size="10pt">
                        <ClientSideEvents Click="up_Browse" />
                    </dx:ASPxButton>
                </td>

                <td style="padding-right: 1em">
                    <dx:ASPxButton ID="btnClear" runat="server" AutoPostBack="False" ClientInstanceName="btnClear" Theme="Office2010Silver" Height="28px"
                        Text="Clear" Font-Names="Segoe UI" Font-Size="10pt">
                        <ClientSideEvents Click="function(s, e) {
                            cboFreq.SetSelectedIndex(0);
                                    Grid.PerformCallback('Kosong');
                                }" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>



    <div style="padding: 20px 0px 0px 0px">
        <asp:SqlDataSource ID="dsMS_Frequency" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_MS_FrequencySetting_FillCombo '0'"></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsMs_Shift" runat="server"
            ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
            SelectCommand="Exec sp_SPC_MS_FrequencySetting_FillCombo '1'"></asp:SqlDataSource>

        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
            EnableTheming="True" KeyFieldName="No;Frequency" Theme="Office2010Black" Width="100%"
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

                <dx:GridViewDataSpinEditColumn Caption="Sequence No" FieldName="No" VisibleIndex="1"
                    Width="75px" Settings-AutoFilterCondition="Contains">
                    <PropertiesSpinEdit MaxValue="9" MinValue="1" Width="100%">
                        <ButtonStyle Width="5px" 
                            Paddings-PaddingBottom="2px" Paddings-PaddingTop="2px" Paddings-PaddingLeft="4px" Paddings-PaddingRight="4px">
                        </ButtonStyle>
                    </PropertiesSpinEdit>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </dx:GridViewDataSpinEditColumn>

                <dx:GridViewDataComboBoxColumn Caption="Shift" FieldName="Shift" VisibleIndex="2"
                    Width="100px" Settings-AutoFilterCondition="Contains">
                    <PropertiesComboBox DataSourceID="dsMs_Shift" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="100%"
                        TextField="Description" ValueField="Code" ClientInstanceName="FrequencyCode">
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
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataTimeEditColumn Caption="Start" FieldName="Start" VisibleIndex="3"
                    Width="100px" Settings-AutoFilterCondition="Contains">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <PropertiesTimeEdit DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm" Width="55px">
                        <ButtonStyle Width="5px" 
                            Paddings-PaddingBottom="2px" Paddings-PaddingTop="2px" Paddings-PaddingLeft="4px" Paddings-PaddingRight="4px">
                        </ButtonStyle>
                    </PropertiesTimeEdit>
                    <SettingsHeaderFilter></SettingsHeaderFilter>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTimeEditColumn>

                <dx:GridViewDataTimeEditColumn Caption="End" FieldName="End" VisibleIndex="4"
                    Width="100px" Settings-AutoFilterCondition="Contains">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <PropertiesTimeEdit DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm" Width="55px">
                        <ButtonStyle Width="5px" 
                            Paddings-PaddingBottom="2px" Paddings-PaddingTop="2px" Paddings-PaddingLeft="4px" Paddings-PaddingRight="4px">
                        </ButtonStyle>
                    </PropertiesTimeEdit>
                    <SettingsHeaderFilter></SettingsHeaderFilter>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTimeEditColumn>

                <dx:GridViewDataTimeEditColumn Caption="Verfication Time" FieldName="Verif" VisibleIndex="5"
                    Width="100px" Settings-AutoFilterCondition="Contains">
                    <Settings AutoFilterCondition="Contains"></Settings>
                    <PropertiesTimeEdit DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm" Width="55px">
                        <ButtonStyle Width="5px" 
                            Paddings-PaddingBottom="2px" Paddings-PaddingTop="2px" Paddings-PaddingLeft="4px" Paddings-PaddingRight="4px">
                        </ButtonStyle>
                    </PropertiesTimeEdit>
                    <SettingsHeaderFilter></SettingsHeaderFilter>
                    <FilterCellStyle Paddings-PaddingRight="4px">
                        <Paddings PaddingRight="4px"></Paddings>
                    </FilterCellStyle>
                    <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                        <Paddings PaddingLeft="5px"></Paddings>
                    </HeaderStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTimeEditColumn>

                <dx:GridViewDataCheckColumn Caption="Active Status" FieldName="Status" VisibleIndex="6" Width="100px">
                    <PropertiesCheckEdit 
                        ValueChecked="1" ValueType="System.Char"  ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <Settings AllowSort="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataCheckColumn>

                <dx:GridViewDataTextColumn Caption="Last User" FieldName="LastUser" VisibleIndex="7"
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

                <dx:GridViewDataTextColumn Caption="Last Update" FieldName="LastUpdate" VisibleIndex="8"
                    Width="150px" Settings-AutoFilterCondition="Contains">
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
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataComboBoxColumn Caption="Frequency" FieldName="Frequency" VisibleIndex="9"
                    Width="100px" Settings-AutoFilterCondition="Contains" Visible="false">
                    <PropertiesComboBox DataSourceID="dsMS_Frequency" DropDownStyle="DropDownList" TextFormatString="{0}"
                        IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="100%"
                        TextField="Description" ValueField="Code" ClientInstanceName="FrequencyCode">
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
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"/>
                </dx:GridViewDataComboBoxColumn>

            </Columns>

            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
            <SettingsPager Mode="ShowPager" PageSize="20" AlwaysShowPager="true">
                <PageSizeItemSettings Visible="True" />
            </SettingsPager>
            <Settings HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" VerticalScrollableHeight="300" />
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
                    <div style="padding: 5px 15px 5px 15px; width: 200px">
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <table align="center">
                                <tr style="height: 30px">
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Frequency" Width="100px"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editFrequency" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Frequency"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Segoe UI" Font-Size="8pt" Text="Sequence No" Width="90px"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editNo" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="No"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Shift</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editShift" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Shift"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Start Time</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editStart" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Start"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>End Time</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editEnd" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="End"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Verification Time</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Verif"></dx:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td>Active Status</td>
                                    <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="editAdminStatus" ReplacementType="EditFormCellEditor"
                                            runat="server" ColumnID="Status"></dx:ASPxGridViewTemplateReplacement>
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
