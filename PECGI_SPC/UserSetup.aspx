<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UserSetup.aspx.vb" Inherits="PECGI_SPC.UserSetup" %>

<%@ Register Assembly="DevExpress.Web.v20.2, Version=20.2.11.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div style="padding: 5px 5px 5px 5px">
        <div style="padding: 5px 5px 5px 5px">

            <asp:SqlDataSource ID="dsFactory" runat="server"
                ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
                SelectCommand="select FactoryCode, FactoryName from [Ms_Factory]"></asp:SqlDataSource>

            <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="Grid"
                EnableTheming="True" KeyFieldName="UserID; EmployeeID" Theme="Office2010Black"
                OnRowValidating="Grid_RowValidating" OnStartRowEditing="Grid_StartRowEditing"
                OnRowInserting="Grid_RowInserting" OnRowDeleting="Grid_RowDeleting" 
                OnAfterPerformCallback="Grid_AfterPerformCallback" Width="100%"
                Font-Names="Segoe UI" Font-Size="9pt">
                <ClientSideEvents EndCallback="OnEndCallback" />

                <Columns>
                    <dx:GridViewCommandColumn FixedStyle="Left"
                        VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true"
                        ShowNewButtonInHeader="true" ShowClearFilterButton="true" Width="80px">
                        <HeaderStyle Paddings-PaddingLeft="3px" HorizontalAlign="Center"
                            VerticalAlign="Middle">
                            <Paddings PaddingLeft="3px"></Paddings>
                        </HeaderStyle>
                    </dx:GridViewCommandColumn>

                    <%--<dx:GridViewDataTextColumn Caption="" FieldName="" 
                        VisibleIndex="0" Width="70px">
                        <Settings AllowAutoFilter="False" AllowSort="False" />
                        <EditFormSettings Visible="true" />
                    </dx:GridViewDataTextColumn>--%>

                    <dx:GridViewDataTextColumn Caption="Privileges" FieldName="Privileges"
                        VisibleIndex="1" Width="70px">
                        <Settings AllowAutoFilter="False" AllowSort="False" />
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
                            <dx:ASPxHyperLink ID="hyperlink02" Font-Names="Segoe UI" Font-Size="9pt"
                                runat="server" Text='<%#Eval("Privileges")%>' OnInit="PrivilegesLink_Init">
                            </dx:ASPxHyperLink>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Machine Process" FieldName="LinePrivileges"
                        VisibleIndex="2" Width="55px">
                        <Settings AllowAutoFilter="False" AllowSort="False" />
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
                            <dx:ASPxHyperLink ID="hyperlink02" Font-Names="Segoe UI" Font-Size="9pt"
                                runat="server" Text='<%#Eval("LinePrivileges")%>' OnInit="LinePrivilegesLink_Init">
                            </dx:ASPxHyperLink>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>


                    <dx:GridViewDataTextColumn Caption="User ID" FieldName="UserID"
                        VisibleIndex="3" Width="100px" Settings-AutoFilterCondition="Contains">
                        <PropertiesTextEdit MaxLength="50" Width="120px">
                            <Style HorizontalAlign="Left"></Style>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                        </FilterCellStyle>
                        <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Full Name" FieldName="FullName"
                        VisibleIndex="4" Width="150px" Settings-AutoFilterCondition="Contains">
                        <PropertiesTextEdit MaxLength="30" Width="200px">
                            <Style HorizontalAlign="Left"></Style>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                        </FilterCellStyle>
                        <HeaderStyle Paddings-PaddingLeft="6px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="6px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Description" FieldName="Description"
                        VisibleIndex="5" Width="150px" Settings-AutoFilterCondition="Contains">
                        <PropertiesTextEdit MaxLength="100" Width="200px">
                            <Style HorizontalAlign="Left"></Style>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                        </FilterCellStyle>
                        <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataComboBoxColumn Caption="Admin Status" FieldName="AdminStatus"
                        VisibleIndex="6" Width="90px" Settings-AutoFilterCondition="Contains">
                        <PropertiesComboBox DropDownStyle="DropDownList" Width="80px" TextFormatString="{0}"
                            IncrementalFilteringMode="StartsWith" DisplayFormatInEditMode="true">
                            <Items>
                                <dx:ListEditItem Text="Yes" Value="1" />
                                <dx:ListEditItem Text="No" Value="0" />
                            </Items>
                            <ItemStyle Height="10px" Paddings-Padding="4px">
                                <Paddings Padding="4px"></Paddings>
                            </ItemStyle>
                            <ButtonStyle Width="5px" Paddings-Padding="2px">
                                <Paddings Padding="2px"></Paddings>
                            </ButtonStyle>
                        </PropertiesComboBox>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <HeaderStyle Paddings-PaddingLeft="2px" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True">
                            <Paddings PaddingLeft="2px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataComboBoxColumn>

                    <dx:GridViewDataComboBoxColumn Caption="FactoryCode" FieldName="FactoryCode"
                        VisibleIndex="7" Width="130px" Settings-AutoFilterCondition="Contains">
                        <PropertiesComboBox DataSourceID="dsFactory" DropDownStyle="DropDownList" TextFormatString="{0}"
                            IncrementalFilteringMode="Contains" DisplayFormatInEditMode="true" Width="120px"
                            TextField="FactoryName" ValueField="FactoryCode" ClientInstanceName="FactoryCode">
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
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataComboBoxColumn>

                    <dx:GridViewDataComboBoxColumn Caption="Job Position (OP/MK/QC)" FieldName="JobPosition"
                        VisibleIndex="8" Width="90px" Settings-AutoFilterCondition="Contains">
                        <PropertiesComboBox DropDownStyle="DropDownList" Width="80px" TextFormatString="{0}"
                            IncrementalFilteringMode="StartsWith" DisplayFormatInEditMode="true">
                            <Items>
                                <dx:ListEditItem Text="-" Value="" />
                                <dx:ListEditItem Text="OP" Value="OP" />
                                <dx:ListEditItem Text="MK" Value="MK" />
                                <dx:ListEditItem Text="QC" Value="QC" />
                            </Items>
                            <ItemStyle Height="10px" Paddings-Padding="4px">
                                <Paddings Padding="4px"></Paddings>
                            </ItemStyle>
                            <ButtonStyle Width="5px" Paddings-Padding="2px">
                                <Paddings Padding="2px"></Paddings>
                            </ButtonStyle>
                        </PropertiesComboBox>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <HeaderStyle Paddings-PaddingLeft="2px" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True">
                            <Paddings PaddingLeft="2px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataComboBoxColumn>

                    <dx:GridViewDataTextColumn Caption="Employee ID" FieldName="EmployeeID"
                        VisibleIndex="9" Width="100px" Settings-AutoFilterCondition="Contains">
                        <PropertiesTextEdit MaxLength="50" Width="100px">
                            <Style HorizontalAlign="Left"></Style>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                        </FilterCellStyle>
                        <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Email" FieldName="Email"
                        VisibleIndex="9" Width="200px" Settings-AutoFilterCondition="Contains">
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
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Last User" FieldName="LastUser"
                        VisibleIndex="10" Width="120px" Settings-AutoFilterCondition="Contains">
                        <PropertiesTextEdit MaxLength="15" Width="120px">
                            <Style HorizontalAlign="Left"></Style>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                        </FilterCellStyle>
                        <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Last Update" FieldName="LastUpdate"
                        VisibleIndex="11" Width="150px" Settings-AutoFilterCondition="Contains">
                        <PropertiesTextEdit MaxLength="15" Width="120px">
                            <Style HorizontalAlign="Left"></Style>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains"></Settings>
                        <FilterCellStyle Paddings-PaddingRight="4px">
                            <Paddings PaddingRight="4px"></Paddings>
                        </FilterCellStyle>
                        <HeaderStyle Paddings-PaddingLeft="5px" HorizontalAlign="Center" VerticalAlign="Middle">
                            <Paddings PaddingLeft="5px"></Paddings>
                        </HeaderStyle>
                        <CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Password" FieldName="Password"
                        VisibleIndex="12" Visible="false">
                        <PropertiesTextEdit MaxLength="100" Width="200px">
                        </PropertiesTextEdit>
                        <EditFormSettings Visible="True" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataCheckColumn Caption="Locked" FieldName="LockStatus"
                        VisibleIndex="13" Width="60px" Visible="false">
                        <PropertiesCheckEdit ValueChecked="1" ValueType="System.Char"
                            ValueUnchecked="0">
                        </PropertiesCheckEdit>
                        <Settings AllowSort="False" />
                    </dx:GridViewDataCheckColumn>

                </Columns>

                <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
                <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
                <SettingsPager Mode="ShowPager" PageSize="20" AlwaysShowPager="true">
                    <PageSizeItemSettings Visible="True">
                    </PageSizeItemSettings>
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
                        <div style="padding: 15px 15px 15px 15px; width: 300px">
                            <dx:ContentControl ID="ContentControl1" runat="server">
                                <table align="center">
                                    <tr style="height: 30px">
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Segoe UI" Text="User ID" Width="90px"></dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxGridViewTemplateReplacement ID="editUserID" ReplacementType="EditFormCellEditor"
                                                runat="server" ColumnID="UserID"></dx:ASPxGridViewTemplateReplacement>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Full Name</td>
                                        <td>
                                            <dx:ASPxGridViewTemplateReplacement ID="editFullName" ReplacementType="EditFormCellEditor"
                                                runat="server" ColumnID="FullName"></dx:ASPxGridViewTemplateReplacement>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Password</td>
                                        <td>
                                            <dx:ASPxGridViewTemplateReplacement ID="editPassword" ReplacementType="EditFormCellEditor"
                                                runat="server" ColumnID="Password"></dx:ASPxGridViewTemplateReplacement>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Description</td>
                                        <td>
                                            <dx:ASPxGridViewTemplateReplacement ID="editDescription" ReplacementType="EditFormCellEditor"
                                                runat="server" ColumnID="Description"></dx:ASPxGridViewTemplateReplacement>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Admin Status
                                        </td>
                                        <td>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridViewTemplateReplacement ID="editAdminStatus" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="AdminStatus"></dx:ASPxGridViewTemplateReplacement>
                                                <dx:LayoutItemNestedControlContainer>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Factory
                                        </td>
                                        <td>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridViewTemplateReplacement ID="editFactory" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="FactoryCode"></dx:ASPxGridViewTemplateReplacement>
                                                <dx:LayoutItemNestedControlContainer>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Job Position
                                        </td>
                                        <td>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridViewTemplateReplacement ID="editJobPosition" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="JobPosition"></dx:ASPxGridViewTemplateReplacement>
                                            </dx:LayoutItemNestedControlContainer>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Employee ID
                                        </td>
                                        <td>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridViewTemplateReplacement ID="editEmployeeID" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="EmployeeID"></dx:ASPxGridViewTemplateReplacement>
                                            </dx:LayoutItemNestedControlContainer>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Email
                                        </td>
                                        <td>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridViewTemplateReplacement ID="editEmail" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="Email"></dx:ASPxGridViewTemplateReplacement>
                                            </dx:LayoutItemNestedControlContainer>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>Lock Status
                                        </td>
                                        <td>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridViewTemplateReplacement ID="editLockStatus" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="LockStatus"></dx:ASPxGridViewTemplateReplacement>
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
        </div>
    </div>
</asp:Content>
