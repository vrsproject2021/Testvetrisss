<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSLogin.aspx.cs" Inherits="VETRIS.VRSLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- favicon
		============================================ -->
    <link rel="shortcut icon" type="image/x-icon" href="images/favicon.ico" />
    <!-- Google Fonts
		============================================ -->
    <link href="https://fonts.googleapis.com/css?family=Play:400,700" rel="stylesheet" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="css/font-awesome.min.css" />

    <%--<link rel="stylesheet" href="css/login.css" />--%>
    <link id="lnkLOGIN" runat="server" href = "css/login.css" rel="stylesheet" type="text/css" />
    <!-- owl.carousel CSS
		============================================ -->
    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="css/responsive.css" />

    <script src="scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="scripts/jquery.soverlay.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="error-pagewrap loginForm">
            <div class="error-page-int">
                <div class="text-center m-b-md custom-login">
                    <div class="text-center">
                        <img id ="imgLogo" src="" alt="" runat="server" />
                    </div>
                    <h4>
                        <asp:Label ID="lblAppName" runat="server" Text=""></asp:Label>
                    </h4>
                    <p>We believe in improved patient care and expanding your veterinary practice.</p>
                </div>
                <div class="content-error">
                    <div class="hpanel">
                        <div class="panel-body">

                            <div class="form-group">
                                <label class="control-label" for="txtLoginID">User Login ID</label>
                                <asp:TextBox ID="txtLoginID" runat="server" placeholder="" title="Please enter you Login ID" CssClass="form-control"></asp:TextBox>
                                <span class="help-block small">Enter your unique Login ID for the application</span>
                            </div>
                            <div class="form-group">
                                <label class="control-label" for="txtPwd">Password</label>
                                <asp:TextBox ID="txtPwd" runat="server" placeholder="******" title="lease enter your password" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <span class="help-block small">Enter your strong password</span>
                            </div>
                            <div class="checkbox login-checkbox">
                                <label>
                                    <asp:CheckBox ID="chkRemember" runat="server" CssClass="i-checks" />Remember me
                                </label>
                                <a href="javascript:void(0);" style="float: right; color: red;" onclick="javascript:PWDHelp();">Forgot Password?</a>
                            </div>
                            <div class="row">
                                <%--<a class="btn btn-success btn-block loginbtn btn-sm" href="javascript:void(0);" style="width: 49%;" id="lnkLogin" runat="server">Login</a>--%>
                                <div class="col-sm-8 hidden-xs"></div>
                                <div class="col-sm-4 col-xs-12">
                                    <button id="btnLogin" type="button" class="btn btn-success btn-block loginbtn" runat="server">
                                        Login
                                    </button>
                                </div>
                            </div>



                        </div>
                    </div>
                </div>
                <div class="text-center login-footer">
                    <p>
                        Version:
                        <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label><br />
                        Copyright © 2019 VETChoice Radiology All rights reserved.

                    </p>
                </div>
            </div>
        </div>

        <div class="footerPartArea">
            <a href="mailto:info@vcradiology.com" class="buttonColor">Contact Us</a> to expand your submission hours to 24/7!
        </div>
    </form>
    <input type="hidden" id="hdnError" runat="server" value="" />
    <input type="hidden" id="hdnDivider" runat="server" value="" />
    <input type="hidden" id="hdnDBVer" runat="server" />
    <input type="hidden" id="hdnRootDirectory" runat="server" />
    <input type="hidden" id="hdnMailTaskRefNo" runat="server" value="" />
    <input type="hidden" id="hdnMailAssignType" runat="server" value="" />
    <input type="hidden" id="hdnUID" runat="server" value="" />
    <input type="hidden" id="hdnPwd" runat="server" value="" />
    <input type="hidden" id="hdnTheme" runat="server" value="" />
    <input type="hidden" id="hdnInstCode" runat="server" value="" />
     <input type="hidden" id="hdnMenuID" runat="server" value="0" />
     <input type="hidden" id="hdnTempInstID" runat="server" value="00000000-0000-0000-0000-000000000000" />
</body>
<script type="text/javascript">
    var objhdnRootDirectory = document.getElementById('<%=hdnRootDirectory.ClientID%>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID%>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID%>');
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID%>');
    var objtxtPwd = document.getElementById('<%=txtPwd.ClientID%>');
    var objchkRemember = document.getElementById('<%=chkRemember.ClientID%>');
    var objhdnDBVer = document.getElementById('<%=hdnDBVer.ClientID%>');
    var objhdnUID = document.getElementById('<%=hdnUID.ClientID%>');
    var objhdnPwd = document.getElementById('<%=hdnPwd.ClientID%>');
    var objhdnInstCode = document.getElementById('<%=hdnInstCode.ClientID%>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID%>');
    var objhdnTempInstID = document.getElementById('<%=hdnTempInstID.ClientID%>');
    var objhdnTheme = document.getElementById('<%=hdnTheme.ClientID%>');
    var objlblVersion = document.getElementById('<%=lblVersion.ClientID%>');
    var GsLaunchURL = "";
    var strForm = "VRSLogin";
</script>
<script src="scripts/custome-javascript.js" type="text/javascript"></script>
<script src="scripts/Login.js?v=<%=DateTime.Now.Ticks%>" type="text/javascript"></script>
</html>
