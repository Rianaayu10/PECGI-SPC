<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteHome.Master" CodeBehind="Main.aspx.vb" Inherits="SSO.Main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
            var a = document.cookie;
            console.log(a);
    </script>
    <style type="text/css">
        .card {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            padding: 25px;
            border-radius: 10px;
        }

            .card:hover {
                box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
            }

        .center {
            margin: auto;
           /* width: 60%;*/
            padding: 10px;
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="text-align-center" style="padding-top:5%">
        <h1 style="font-family: Arial; font-size: 35px; font-weight: bold;">Panasonic Smart Factory</h1>
       <%--  <h4 style="font-family: Arial; font-size: 25px; font-weight: bold; ">SPC SYSTEM</h4>--%>
    </div>
    <%--<img src="Styles/images/dashboard.png" alt="Dashboard" style="width:50%"/>--%>
    <div style="padding-top:5%">
        <table class="center">
            <tr>
                <asp:Repeater ID="Account" runat="server">
                    <ItemTemplate>
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <a target="_self" class="btn" href="<%# Eval("URL")%>">
                                                <button class="card" style="border: none; text-align: left; color: white; background-color: orange">
                                                    <i class="fa fa-5x fa-bar-chart-o"></i>
                                                </button>

                                            </a>
                                        </td>
                                    </tr>
                                    <tr style="height: 50px">
                                        <td>
                                            <span style="font-family: Arial; font-size: 20px; font-weight: bold;"><%# Eval("FactoryName")%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </table>
    </div>
</asp:Content>
