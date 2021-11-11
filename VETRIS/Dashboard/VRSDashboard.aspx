<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDashboard.aspx.cs" Inherits="VETRIS.Dashboard.VRSDashboard" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
    <!-- Google Fonts
		============================================ -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="../css/bootstrap.min.css" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="../css/font-awesome.min.css" />
    <!-- metisMenu CSS
		============================================ -->
    <link rel="stylesheet" href="../css/metisMenu/metisMenu.min.css" />
    <link rel="stylesheet" href="../css/metisMenu/metisMenu-vertical.css?1" />
    <!-- style CSS
		============================================ -->
    <%--<link rel="stylesheet" href="../css/style.css?t=<%=DateTime.Now.Ticks%>" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="dashboardSTYLE" runat="server" href = "../css/dashboard.css" rel="stylesheet" type="text/css" />
    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="../css/responsive.css" />

    
</head>
<body class="dashboard">
    <form id="form1" runat="server">
         <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" id="divFrame">
            <div class="row">
                <div class="col-md-12" style="padding-left: 0; padding-right: 0;">
                    <div class="col-md-2 col-sm-2" style="padding-left: 0; padding-right: 0;width: 14.666667% !important;">
                        <div class="left-sidebar-pro" style="width: 200px;">
                            <nav id="sidebar" class="">
                                <div class="sidebar-header" style="padding: 3px 0;">
                                    <a href="../VRSMain.aspx">
                                        <img class="main-logo" src="../images/logo/logo.png" runat="server" id="vetrisLOGO" alt="" /></a>
                                </div>
                                <div class="left-custom-menu-adp-wrap comment-scrollbar" id="App_Menu" runat="server">
                                    <nav class="sidebar-nav left-sidebar-menu-pro">
                                        <ul class="metismenu" id="dashboardMenu" runat="server">
                                            <li>
                                                <a href="#">Test</a>
                                            </li>
                                        </ul>
                                    </nav>
                                </div>
                            </nav>
                        </div>
                    </div>
                    <div class="col-md-10 col-sm-10" style="padding-left: 0; padding-right: 0;width: 84.333333%;">
                        <div class="row case-div">
                            <div class="col-sm-6">
                                <h2 id="txtDescription" runat="server"></h2>
                            </div>
                            <div class="col-sm-6 text-right" style="padding-right: 8px;">
                                <button type="button" class="btn btn-custon-four btn-warning refreshMe" id="btnReset" runat="server">
                                    <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Refresh
                                </button>
                                <button type="button" class="btn btn-custon-four btn-danger" onclick="javascript:closedashboardreport()" id="btnClose" runat="server">
                                    <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                </button>
                            </div>
                        </div>
                        <div class="row" style="margin-left: -14px;">
                            <iframe id="iframeDashboard" scrolling="no" onload="javascript:adjustDashboardFrameHeight();"  style="padding: 0px; height: auto; width: 100%; border: none"></iframe>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>
    <input type="hidden" runat="server" id="hdnUserID" value="00000000-0000-0000-0000-000000000000" />
    <input type="hidden" runat="server" id="hdnDefaultDesc" value="" />
    <input type="hidden" runat="server" id="hdnDefaultUrl" value="" />
    <input type="hidden" runat="server" id="hdnUrlReferrer" value="" />
    <input type="hidden" runat="server" id="hdnMenuId" value="" />
    <input type="hidden" runat="server" id="hdnRefreshTime" value="" />
    <input type="hidden" runat="server" id="hdnIsRefreshBtn" value="N" />
    <input type="hidden" runat="server" id="hdnreportTitle" value="" />
    <input type="hidden" runat="server" id="hdnTheme" value="DEFAULT" />
</body>

</html>
<script src="../scripts/jquery-1.12.4.min.js" type="text/javascript"></script>
<script src="../scripts/bootstrap.min.js" type="text/javascript"></script>
<script src="../scripts/custome-javascript.js" type="text/javascript"></script>
<script src="../scripts/jquery-1.7.1.js" type="text/javascript"></script>
<script src="scripts/dashboard.js?t=<%=DateTime.Now.Ticks%>"></script>
