<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSLogout.aspx.cs" Inherits="VETRIS.VRSLogout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-icon" href="images/favicon.ico" />
    <link href="https://fonts.googleapis.com/css?family=Play:400,700" rel="stylesheet" />
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/font-awesome.min.css" />

    <link rel="stylesheet" href="css/login.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="error-pagewrap loginForm">
            <div class="error-page-int">
                <div class="hpanel">
                    <div class="panel-body text-center lock-inner">
                        <p>You have been logged out !!!</p>
                        <p>
                            <asp:Label ID="lblShowMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></p>
                        <button type="button" class="btn btn-primary block full-width" onclick="javascript:location.href='VRSLogin.aspx';">Click here to login</button>

                    </div>
                </div>
                <div class="text-center login-footer">
                    <p>Copyright © 2019 VETChoice Radiology All rights reserved.</p>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
