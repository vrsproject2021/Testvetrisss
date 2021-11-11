<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRegConfirmation.aspx.cs" Inherits="VETRIS.Registration.VRSRegConfirmation" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VETERINARY RADIOLOGY INFORMATION SYSTEM</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-icon" href="../images/favicon.ico" />
    <link href="https://fonts.googleapis.com/css?family=Play:400,700" rel="stylesheet" />
    <!-- Icons font CSS-->
    <link href="vendor/mdi-font/css/material-design-iconic-font.min.css" rel="stylesheet" media="all" />
    <link href="vendor/font-awesome-4.7/css/font-awesome.min.css" rel="stylesheet" media="all" />


    <!-- Main CSS-->
    <%--<link href="css/bootstrap.min.css" rel="stylesheet" media="all"/>--%>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" media="all" />

    <link href="css/main.css" rel="stylesheet" media="all" />
    <!-- Vendor CSS-->
    <link href="vendor/select2/select2.min.css" rel="stylesheet" media="all" />
    <link href="css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <style>
        td > a[disabled] {
    color: gray;
}
    </style>
</head>
<body>
    <form id="form1" method="post" runat="server">
        <div class="page-wrapper p-t-20 p-b-100 font-robo">
            <div class="container-fluid">
                <div class="card card-1">
                    <div class="card-heading">
                        <img src="images/cta-logo-mid.png" />
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="text-align:center;margin-top: 20px;">
                            <label style="font-size:25px;">Thank you for submitting your Information Form.</label>
                        </div>
                        <div class="col-md-12" style="text-align:center;margin-bottom: 20px;">
                            <label style="font-size:16px;">
                               <%-- Please call 8337382020 to complete the creation of your portal access.<br />--%>
                                SOMEONE WILL CALL YOU SHORTLY TO COMPLETE THE CREATION OF YOUR PORTAL ACCESS.<br />
                                <span style="color:#555;font-size:13px;">(Customer Care staff hours 9 a.m. – 9 p.m. Central Standard Time Mon.-Sun.)</span>
                            </label>
                        </div>
                        <div class="col-md-12" style="text-align:center;margin-bottom: 20px;">
                            <label style="font-size:16px;">
                                Portal access will enable you to submit your first free case immediately.
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script>
    var strForm = "VRSRegConfirmation";
</script>