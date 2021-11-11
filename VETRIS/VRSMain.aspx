<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSMain.aspx.cs" Inherits="VETRIS.VRSMain" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-icon" href="images/favicon.ico" />
    <!-- Google Fonts
		============================================ -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="css/font-awesome.min.css" />
    <!-- metisMenu CSS
		============================================ -->
    <link rel="stylesheet" href="css/metisMenu/metisMenu.min.css" />
    <link rel="stylesheet" href="css/metisMenu/metisMenu-vertical.css?1" />


    <!-- style CSS
		============================================ -->
   <%-- <link rel="stylesheet" href="css/style.css?7" />--%>
     <link id="lnkSTYLE" runat="server" href = "css/style.css" rel="stylesheet" type="text/css" />
     <link id="lnkLBOX" runat="server" href = "css/lightbox.css" rel="stylesheet" type="text/css" />
    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="css/responsive.css" />
   
</head>
<body>
    <form id="form1" runat="server">
        <div class="left-sidebar-pro">
            <nav id="sidebar" class="">
                <div class="sidebar-header">
                    <a href="#">
                        <img id="imgLogo" runat="server" class="main-logo" src="" alt="" /></a>
                    <strong><a href="#">
                        <img id="imgLogoSN" runat="server" src="" alt="" /></a></strong>
                </div>
                <div class="left-custom-menu-adp-wrap comment-scrollbar" id="App_Menu" runat="server">
                </div>
            </nav>
        </div>
        <!-- End Left menu area -->
        <!-- Start Welcome area -->
        <div class="all-content-wrapper" id="divMenu">
            <div class="hidden-lg hidden-md hidden-sm">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="logo-pro">
                                <a href="#">
                                    <img id="imgMainLogo" runat ="server" class="main-logo" src="" alt="" /></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="header-advance-area">
                <div class="header-top-area">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="header-top-wraper">
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="menu-switcher-pro pull-left marginTP10">
                                                <button type="button" id="sidebarCollapse" class="btn bar-button-pro header-drl-controller-btn btn-info navbar-btn">
                                                    <i class="fa fa-bars" aria-hidden="true"></i>
                                                   <%-- <i class="educate-icon educate-nav"></i>--%>
                                                </button>
                                            </div>
                                            <div class="header-top-menu pull-left marginTP15 marLFT10 mg-t-10 marginLFT10">
                                                <h2>
                                                    <asp:Label ID="lblAppName" runat="server" Text=""></asp:Label>
                                                </h2>
                                                <div class="headerDownPart"><a href="mailto:info@vcradiology.com" class="buttonColor">Contact Us</a> to expand your submission hours to 24/7!</div>
                                            </div>

                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="header-right-info">
                                                <ul class="nav navbar-nav mai-top-nav header-right-menu" style="padding: 0;">
                                                    <li>
                                                        <img src="images/Dashboard-120x60.png" style="cursor: pointer; display: none;" id="dashboard" onclick="javascript:Dashboard_OnClick();" />
                                                    </li>
                                                    
                                                    <li>
                                                        <img src="images/manual_submit.png" style="cursor: pointer; display: none;" id="msIcon" onclick="javascript:LoadManualSubmission();" />
                                                    </li>
                                                    <li>
                                                        <img src="images/chat_Icon.png" style="cursor: pointer;" id="chatIcon" />
                                                    </li>
                                                    <li class="nav-item" style="margin-top: 15px; display: inline;" id="loguser">
                                                        <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="nav-link dropdown-toggle">

                                                            <div class="pull-left marginTP5">
                                                                <span class="admin-name" id="spnUserName" runat="server"></span>
                                                                <i class="fa fa-angle-down edu-icon edu-down-arrow"></i>
                                                            </div>
                                                        </a>
                                                        <ul role="menu" class="dropdown-header-top author-log dropdown-menu animated zoomIn">

                                                            <li>
                                                                <a href="javascript:void(0);" data-toggle="dropdown" role="button" aria-expanded="false" onclick="javascript:LoadChangePwd();">
                                                                    <span class="edu-icon edu-user-rounded author-log-ic"></span>Change Password
                                                                </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:void(0);" id="hrefTheme" data-toggle="dropdown" role="button" aria-expanded="false" onclick="javascript:ToggleTheme();">
                                                                    <span class="edu-icon edu-locked author-log-ic"></span>
                                                                    <span id="spnModeDef" style="display:none;">
                                                                        Switch to DARK theme
                                                                         <%--<i class="fa fa-toggle-on" title="toggle to dark theme" onclick="javascript:ToggleTheme('DARK');"></i>--%>
                                                                    </span>
                                                                    <span id="spnModeDark" style="display:none;">
                                                                        Switch to DEFAULT theme
                                                                        <%--<i class="fa fa-toggle-off" title="toggle to default theme" onclick="javascript:ToggleTheme('DEFAULT');"></i>--%>
                                                                    </span>
                                                                </a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript:void(0);" onclick="javascript:Logout();">
                                                                    <span class="edu-icon edu-locked author-log-ic"></span>Log Out
                                                                </a>
                                                            </li>
                                                        </ul>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Mobile Menu start -->
                <nav class="navbar navbar-default hidden-lg hidden-md hidden-sm">
                    <div class="container-fluid">
                        <!-- Brand and toggle get grouped for better mobile display -->
                        <div class="navbar-header">
                            <button id="ChangeToggle" type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#mobile_menu">
                                <div id="navbar-hamburger">
                                    <span class="sr-only">Toggle navigation</span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                </div>
                                <div id="navbar-close" class="hidden">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </div>
                            </button>
                            <a class="navbar-brand" href="#">Menu</a>
                        </div>

                        <div class="collapse navbar-collapse" id="mobile_menu" runat="server">
                        </div>
                    </div>
                </nav>
                <!-- Mobile Menu end -->

            </div>

            <!--Content Area-->


            <div class="static-table-area">
                <div class="container-fluid">

                    <div class="row">
                        <div>
                            <iframe id="iframePage" scrolling="no" style="padding: 0px; width: 100%; height: auto; background-color: transparent; border: none;"></iframe>
                        </div>
                    </div>
                </div>
            </div>
            <!--Content Area-->

        </div>
        <div class="footer-copyright-area">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="footer-copy-right">
                            <p>
                               (Version:
                                <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>) &nbsp;&nbsp;
                                Copyright © 2019 VETChoice Radiology All rights reserved.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--<div class="chatSection" style="display: none;" id="divChat">--%>
        <%--<div class="chatIcon">
                <img src="images/chatIcon.png" />
            </div>--%>
        <div class="chatBoxArea" id="chatBoxArea">
            <div class="chatCloseBtn" id="chatCloseBtn" style="cursor: pointer;">
                <img src="images/closeBtn.png" />
            </div>
            <div>
                <iframe id="iframeChat" scrolling="no" style="padding: 0px; width: 100%; height: auto; background-color: transparent; border: none; min-height:490px;"></iframe>
            </div>
        </div>
        <%--</div>--%>
        <div id="reportcontainer" style="display: none; position: fixed; z-index: 10000; top: 0px; left: 0px; padding: 0px; bottom: 0px; right: 0px; background-color: white; border: none;">
            <button type="button" style="display: none;" onclick="javascript:closereports();" class="btn btn-custon-four btn-danger">
                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close
            </button>
            <iframe id="reportserver" style="position: fixed; top: 0px; left: 0px; padding: 0px; display: block; width: 100%; height: 100%; border: none;"></iframe>
        </div>

        <div id="dashboardreportcontainer" style="display: none; position: fixed; z-index: 10000; top: 0px; left: 0px; padding: 0px; bottom: 0px; right: 0px; background-color: white; border: none; overflow: hidden;">
            <button type="button" style="display: none;" onclick="javascript:closedashboardreport();" class="btn btn-custon-four btn-danger">
                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close
            </button>
            <iframe id="dashboardreport" style="position: fixed; top: 0px; left: 0px; padding: 0px; display: block; width: 100%; height: 100%; border: none;"></iframe>
        </div>

        <!-- The Modal/Lightbox -->
        <div id="dashboardradiologistcontainer" style="display: none; position: fixed; z-index: 10000; top: 0px; left: 0px; padding: 0px; bottom: 0px; right: 0px; border: none; overflow: hidden;">
            <div class="overlay"></div>
            <div id="myModal" class="modal">
                <span class="modal-title" id="divHeader">Title</span>
                <span class="close cursor" onclick="closeModal()">&times;</span>
                <div class="modal-content">
                    <table class="styled-table" id="data">
                        <thead>
                            <tr>
                                <th rowspan="2">Modality</th>
                                <th colspan="2">Cases in Progress</th>
                                <th colspan="4">Cases Completed</th>
                            </tr>
                            <tr>

                                <th>Assigned</th>
                                <th>Working On</th>
                                <th id="thToday">Today<span class="th-span">(<%= DateTime.Now.ToString("dd-MMM-yyyy") %>)</span></th>
                                <th id="thThisMonth">This Month<span class="th-span">(<%= DateTime.Now.ToString("MMM-yyyy") %>)</span></th>
                                <th id="thLastMonth">Last Month<span class="th-span">(<%= DateTime.Now.AddMonths(-1).ToString("MMM-yyyy") %>)</span></th>
                                <th id="thThisYear">This Year<span class="th-span">(<%= DateTime.Now.ToString("yyyy") %>)</span></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <th>Total</th>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </tfoot>
                    </table>


                </div>
            </div>
        </div>

        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnSessionID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnUserName" runat="server" value="" />
        <input type="hidden" id="hdnUserCode" runat="server" value="" />
        <input type="hidden" id="hdnUserContNo" runat="server" value="" />
        <input type="hidden" id="hdnUserRoleID" runat="server" value="0" />
        <input type="hidden" id="hdnUserRoleCode" runat="server" value="0" />
        <input type="hidden" id="hdnMenuID" runat="server" value="0" />
        <input type="hidden" id="hdnPreDefMenu" runat="server" value="N" />
        <input type="hidden" id="hdnDecPlaces" runat="server" value="2" />
        <input type="hidden" id="hdnDateFormat" runat="server" value="" />
        <input type="hidden" id="hdnDateSep" runat="server" value="" />
        <input type="hidden" id="hdnRootDirectory" runat="server" value="" />
        <input type="hidden" id="hdnServerPath" runat="server" value="" />
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnDivider" runat="server" value="" />
        <input type="hidden" id="hdnSecDivider" runat="server" value="" />
        <input type="hidden" id="hdnFirstLogin" runat="server" value="" />
        <input type="hidden" id="hdnPACSUID" runat="server" value="" />
        <input type="hidden" id="hdnPACSPwd" runat="server" value="" />
        <input type="hidden" id="hdnInstCode" runat="server" value="" />
        <input type="hidden" id="hdnInstName" runat="server" value="" />
        <input type="hidden" id="hdnCHATURL" runat="server" value="" />
        <input type="hidden" id="hdnENBLCHAT" runat="server" value="" />
        <input type="hidden" id="hdnDLDRENBL" runat="server" value="N" />
        <input type="hidden" id="hdnBillAcctID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnBillAcctName" runat="server" value="" />
        <input type="hidden" id="hdnRadiologistID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnRadiologistTimeZone" runat="server" value="" />
        <input type="hidden" id="hdnAPIVER" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8CLTIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVUID" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVPWD" runat="server" value="" />
        <input type="hidden" id="hdnWS8Session" runat="server" value="" />
        <input type="hidden" id="hdnRPTEGNURL" runat="server" value="" />
        <input type="hidden" id="hdnAllowMS" runat="server" value="N" />
        <input type="hidden" id="hdnAllowDB" runat="server" value="N" />
        <input type="hidden" id="hdnTempInstID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnDefTZID" runat="server" value="0" />
        <input type="hidden" id="hdnDefTZStdName" runat="server" value="" />
        <input type="hidden" id="hdnPrefTheme" runat="server" value="" />
        <input type="hidden" id="hdnDashboardId" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnSessionID = document.getElementById('<%=hdnSessionID.ClientID %>');
    var objhdnUserName = document.getElementById('<%=hdnUserName.ClientID %>');
    var objhdnUserCode = document.getElementById('<%=hdnUserCode.ClientID %>');
    var objhdnUserContNo = document.getElementById('<%=hdnUserContNo.ClientID %>');
    var objhdnUserRoleID = document.getElementById('<%=hdnUserRoleID.ClientID %>');
    var objhdnUserRoleCode = document.getElementById('<%=hdnUserRoleCode.ClientID %>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID %>');
    var objhdnPreDefMenu = document.getElementById('<%=hdnPreDefMenu.ClientID %>');
    var objhdnDecPlaces = document.getElementById('<%=hdnDecPlaces.ClientID %>');
    var objhdnDateFormat = document.getElementById('<%=hdnDateFormat.ClientID %>');
    var objhdnDateSep = document.getElementById('<%=hdnDateSep.ClientID %>');
    var objhdnRootDirectory = document.getElementById('<%=hdnRootDirectory.ClientID %>');
    var objhdnServerPath = document.getElementById('<%=hdnServerPath.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objiframePage = document.getElementById('iframePage');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID %>');
    var objhdnSecDivider = document.getElementById('<%=hdnSecDivider.ClientID %>');
    var objhdnFirstLogin = document.getElementById('<%=hdnFirstLogin.ClientID %>');
    var objhdnPACSUID = document.getElementById('<%=hdnPACSUID.ClientID %>');
    var objhdnPACSPwd = document.getElementById('<%=hdnPACSPwd.ClientID %>');
    var objhdnInstCode = document.getElementById('<%=hdnInstCode.ClientID %>');
    var objhdnInstName = document.getElementById('<%=hdnInstName.ClientID %>');
    var objhdnCHATURL = document.getElementById('<%=hdnCHATURL.ClientID %>');
    var objhdnENBLCHAT = document.getElementById('<%=hdnENBLCHAT.ClientID %>');
    var objhdnDLDRENBL = document.getElementById('<%=hdnDLDRENBL.ClientID %>');
    var objhdnBillAcctID = document.getElementById('<%=hdnBillAcctID.ClientID %>');
    var objhdnBillAcctName = document.getElementById('<%=hdnBillAcctName.ClientID %>');
    var objhdnAPIVER = document.getElementById('<%=hdnAPIVER.ClientID %>');
    var objhdnWS8SRVIP = document.getElementById('<%=hdnWS8SRVIP.ClientID %>');
    var objhdnWS8CLTIP = document.getElementById('<%=hdnWS8CLTIP.ClientID %>');
    var objhdnWS8SRVUID = document.getElementById('<%=hdnWS8SRVUID.ClientID %>');
    var objhdnWS8SRVPWD = document.getElementById('<%=hdnWS8SRVPWD.ClientID %>');
    var objhdnWS8Session = document.getElementById('<%=hdnWS8Session.ClientID %>');
    var objhdnRPTEGNURL = document.getElementById('<%=hdnRPTEGNURL.ClientID %>');
    var objhdnAllowMS = document.getElementById('<%=hdnAllowMS.ClientID %>');
    var objhdnAllowDB = document.getElementById('<%=hdnAllowDB.ClientID %>');
    var objhdnRadiologistID = document.getElementById('<%=hdnRadiologistID.ClientID %>');
    var objhdnRadiologistTimeZone = document.getElementById('<%=hdnRadiologistTimeZone.ClientID %>');
    var objhdnDefTZID = document.getElementById('<%=hdnDefTZID.ClientID %>');
    var objhdnDefTZStdName = document.getElementById('<%=hdnDefTZStdName.ClientID %>');
    var objhdnTempInstID = document.getElementById('<%=hdnTempInstID.ClientID%>');
    var objhdnPrefTheme = document.getElementById('<%=hdnPrefTheme.ClientID%>');
    var objApp_Menu = document.getElementById('<%=App_Menu.ClientID %>');
    var objmobile_menu = document.getElementById('<%=mobile_menu.ClientID %>');
    var objiframeChat = document.getElementById('iframeChat');
    var objreportcontainer = document.getElementById('reportcontainer');
    var objreportserver = document.getElementById('reportserver');
    var objdashboardreportcontainer = document.getElementById('dashboardreportcontainer');
    var objdashboardreport = document.getElementById('dashboardreport');
    var strForm = "VRSMain";



</script>
<script src="scripts/jquery-1.12.4.min.js" type="text/javascript"></script>
<script src="scripts/bootstrap.min.js" type="text/javascript"></script>
<script src="scripts/custome-javascript.js" type="text/javascript"></script>

<script src="scripts/jquery.meanmenu.js" type="text/javascript"></script>
<script src="scripts/jquery.sticky.js" type="text/javascript"></script>
<script src="scripts/metisMenu/metisMenu.min.js" type="text/javascript"></script>
<script src="scripts/metisMenu/metisMenu-active.js" type="text/javascript"></script>
<script src="scripts/mainScript.js" type="text/javascript"></script>
<script src="scripts/jquery-1.7.1.js" type="text/javascript"></script>
<script src="scripts/jquery.soverlay.min.js" type="text/javascript"></script>
<script src="scripts/chatScript.js?v=<%=DateTime.Now.Ticks%>" type="text/javascript"></script>

<script src="scripts/popups.js?v=<%=DateTime.Now.Ticks%>" type="text/javascript"></script>
<script src="scripts/Common.js?v=<%=DateTime.Now.Ticks%>" type="text/javascript"></script>
<script src="scripts/Main.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/ValidateDate.js?v=<%=DateTime.Now.Ticks%>" type="text/javascript"></script>
<script src="scripts/color_set.js?v=<%=DateTime.Now.Ticks%>"></script>

</html>
