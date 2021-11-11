<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSUserEmailVerification.aspx.cs" Inherits="VETRIS.Registration.VRSUserEmailVerification" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VETERINARY RADIOLOGY INFORMATION SYSTEM</title>
     <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />

    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <link href="../css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>


    <link href="vendor/select2/select2.min.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/main.css" rel="stylesheet" />

    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="../scripts/jquery-1.12.4.js"></script>
    <script src="../scripts/jquery.sticky.js"></script>
    <script src="../scripts/jquery-1.7.1.js"></script>
    
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="vendor/select2/select2.min.js"></script>
    <script src="js/global.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-wrapper bg-blue p-t-20 p-b-100 font-robo">
            <div class="container-fluid">
                <div class="card card-1">
                    <div class="card-heading">
                	    <img src="images/cta-logo-mid.png" />
                    </div>
                    <div class="card-body">
                        <h2 class="title">Verification Info</h2>
                        <div class="row">
							<div class="col-sm-12 marginBTM10">
								<label id="lblStatus" runat="server"></label>
							</div>
						</div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
