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
    <!-- owl.carousel CSS
		============================================ -->
    <link rel="stylesheet" href="css/owl.carousel.css" />
    <link rel="stylesheet" href="css/owl.theme.css" />
    <link rel="stylesheet" href="css/owl.transitions.css" />
    <!-- animate CSS
		============================================ -->
    <link rel="stylesheet" href="css/animate.css" />
    <!-- normalize CSS
		============================================ -->
    <link rel="stylesheet" href="css/normalize.css" />
    <!-- meanmenu icon CSS
		============================================ -->
    <link rel="stylesheet" href="css/meanmenu.min.css" />
    <!-- main CSS
		============================================ -->
    <link rel="stylesheet" href="css/main.css" />
    <!-- educate icon CSS
		============================================ -->
    <link rel="stylesheet" href="css/educate-custon-icon.css" />
    <!-- morrisjs CSS
		============================================ -->
    <link rel="stylesheet" href="css/morrisjs/morris.css" />
    <!-- mCustomScrollbar CSS
		============================================ -->
    <link rel="stylesheet" href="css/scrollbar/jquery.mCustomScrollbar.min.css" />
    <!-- metisMenu CSS
		============================================ -->
    <link rel="stylesheet" href="css/metisMenu/metisMenu.min.css" />
    <link rel="stylesheet" href="css/metisMenu/metisMenu-vertical.css" />

    <!-- style CSS
		============================================ -->
    <link rel="stylesheet" href="css/style.css" />
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
                        <img class="main-logo" src="images/logo/logo.png" alt="" /></a>
                    <strong><a href="#">
                        <img src="images/logo/logosn.png" alt="" /></a></strong>
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
                                    <img class="main-logo" src="images/logo/logo.png" alt="" /></a>
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
                                                    <i class="educate-icon educate-nav"></i>
                                                </button>
                                            </div>
                                            <div class="header-top-menu pull-left marginTP15">
                                                <h2>
                                                    <asp:Label ID="lblAppName" runat="server" Text=""></asp:Label>
                                                </h2>
                                            </div>

                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="header-right-info">
                                                <ul class="nav navbar-nav mai-top-nav header-right-menu">
                                                    <li class="nav-item">
                                                        <a href="#" data-toggle="dropdown" role="button" aria-expanded="false" class="nav-link dropdown-toggle">

                                                            <div class="pull-left marginTP5">
                                                                <span class="admin-name" id="spnUserName" runat="server"></span>
                                                                <i class="fa fa-angle-down edu-icon edu-down-arrow"></i>
                                                            </div>
                                                        </a>
                                                        <ul role="menu" class="dropdown-header-top author-log dropdown-menu animated zoomIn">

                                                            <li><a href="javascript:void(0);"  data-toggle="dropdown" role="button" aria-expanded="false" onclick="javascript:LoadChangePwd();"><span class="edu-icon edu-user-rounded author-log-ic"></span>Change Password</a>
                                                            </li>

                                                            <li><a href="javascript:void(0);" onclick="javascript:Logout();"><span class="edu-icon edu-locked author-log-ic"></span>Log Out</a>
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
                            <iframe id="iframePage" scrolling="no" style="padding: 0px; width: 100%; height: auto; background-color: transparent; border: none; min-height: 545px;"></iframe>
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
                            <p>Copyright © 2019 VETChoice Radiology All rights reserved.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnUserID" runat="server" value="0" />
        <input type="hidden" id="hdnUserName" runat="server" value="" />
        <input type="hidden" id="hdnUserCode" runat="server" value="" />
        <input type="hidden" id="hdnUserRoleID" runat="server" value="0" />>
        <input type="hidden" id="hdnMenuID" runat="server" value="0" />
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

    </form>
</body>
<script type="text/javascript">
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnUserName = document.getElementById('<%=hdnUserName.ClientID %>');
    var objhdnUserCode = document.getElementById('<%=hdnUserCode.ClientID %>');
    var objhdnUserRoleID = document.getElementById('<%=hdnUserRoleID.ClientID %>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID %>');
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
    var objApp_Menu = document.getElementById('<%=App_Menu.ClientID %>');
    var objmobile_menu = document.getElementById('<%=mobile_menu.ClientID %>');
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

<script src="scripts/popups.js?06052019" type="text/javascript"></script>
<script src="scripts/Common.js" type="text/javascript"></script>
<script src="scripts/Main.js?6"></script>
<script src="scripts/ValidateDate.js?1" type="text/javascript"></script>
</html>
