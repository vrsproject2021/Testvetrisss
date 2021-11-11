<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRegistrationChangePwd.aspx.cs" Inherits="VETRIS.VRSRegistrationChangePwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration : Change Password</title>
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

    <link rel="stylesheet" href="css/login.css" />
    <!-- owl.carousel CSS
		============================================ -->
    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="css/responsive.css" />

    <script src="scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="scripts/jquery.soverlay.min.js" type="text/javascript"></script>
    <style>
        .mandatory {
    font-weight: bold;
    color: #FF0000;
}
        .loginForm {
            margin-top:7%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="error-pagewrap loginForm">
            <div class="error-page-int">
                <div class="text-center m-b-md custom-login">
                    <div class="text-center">
                        <img src="images/logo/logo.png" />
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
                                <label class="control-label" for="usermodel">Institution Name</label>
                                <asp:TextBox ID="txtInstitutionName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label" for="usermodel">User Name</label>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group" style="display:none;">
                                <label class="control-label" for="usermodel">Existing Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label" for="usermodel">New Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label">Confirm New Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 hidden-xs"></div>
                                <div class="col-sm-6 col-xs-12">
                                    <button id="btnChange" type="button" class="btn btn-success btn-block loginbtn" runat="server">
                                        Reset Password & Login
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="text-center login-footer">
                    <p>
                        Copyright © 2019 VETChoice Radiology All rights reserved.
                    </p>
                </div>
            </div>
        </div>
    </form>
    <input type="hidden" id="hdnError" runat="server" value="" />
    <input type="hidden" id="hdnDivider" runat="server" value="" />
    <input type="hidden" id="hdnDBVer" runat="server" />
    <input type="hidden" id="hdnRootDirectory" runat="server" />
    <input type="hidden" id="hdnMailTaskRefNo" runat="server" value="" />
    <input type="hidden" id="hdnMailAssignType" runat="server" value="" />
    <input type="hidden" id="hdnUID" runat="server" value="" />
    <input type="hidden" id="hdnLoginId" runat="server" value="" />
    <input type="hidden" id="hdnPwd" runat="server" value="" />
    <input type="hidden" id="hdnInstCode" runat="server" value="" />
     <input type="hidden" id="hdnMenuID" runat="server" value="0" />
     <input type="hidden" id="hdnTempInstID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    <input type="hidden" id="hdnEmailId" runat="server" value="" />
</body>
<script type="text/javascript">
    var objhdnRootDirectory = document.getElementById('<%=hdnRootDirectory.ClientID%>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID%>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID%>');
    var objTxtInstitutionName = document.getElementById('<%=txtInstitutionName.ClientID%>');
    var objtxtPwd = document.getElementById('<%=txtPassword.ClientID%>');
    var objtxtNewPwd = document.getElementById('<%=txtNewPassword.ClientID%>');
    var objtxtConfirmPwd = document.getElementById('<%=txtConfirmPassword.ClientID%>');
    var objhdnDBVer = document.getElementById('<%=hdnDBVer.ClientID%>');
    var objhdnUID = document.getElementById('<%=hdnUID.ClientID%>');
    var objhdnPwd = document.getElementById('<%=hdnPwd.ClientID%>');
    var objhdnInstCode = document.getElementById('<%=hdnInstCode.ClientID%>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID%>');
    var objhdnTempInstID = document.getElementById('<%=hdnTempInstID.ClientID%>');
    var objhdnLoginId = document.getElementById('<%=hdnLoginId.ClientID%>');
    var objhdnEmailId = document.getElementById('<%=hdnEmailId.ClientID%>');
    var objtxtUserName = document.getElementById('<%=txtUserName.ClientID%>');

    var GsLaunchURL = "";
    var strForm = "VRSRegistrationChangePwd";
</script>
<script src="scripts/custome-javascript.js" type="text/javascript"></script>
<script src="scripts/RegChangePassword.js?07052020" type="text/javascript"></script>
</html>
