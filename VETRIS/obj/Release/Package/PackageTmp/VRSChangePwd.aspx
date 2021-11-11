<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSChangePwd.aspx.cs" Inherits="VETRIS.VRSChangePwd" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/responsive.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Change Password</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">

                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">User Name</label>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                            </div>
                        </div>

                      
                        <div class="col-sm-9 col-xs-12">
                            &nbsp;

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Existing Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-9 col-xs-12">
                            &nbsp;
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">New Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-9 col-xs-12">
                            &nbsp;
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Confirm New Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-9 col-xs-12">
                            &nbsp;
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12 text-left">

                            <button type="button" class="btn btn-custon-four btn-primary" id="btnChange" runat="server">
                                <i class="fa fa-repeat" aria-hidden="true"></i>&nbsp; Change</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');

    var objtxtUserName = document.getElementById('<%=txtUserName.ClientID %>');
    var objtxtPassword = document.getElementById('<%=txtPassword.ClientID %>');
    var objtxtNewPassword = document.getElementById('<%=txtNewPassword.ClientID %>');
    var objtxtConfirmPassword = document.getElementById('<%=txtConfirmPassword.ClientID %>');
    var objbtnChange = document.getElementById('<%=btnChange.ClientID %>');
    var strForm = "VRSChangePwd";
</script>
<script src="scripts/custome-javascript.js"></script>
<script src="scripts/AppPages.js"></script>
<script src="scripts/ChangePwd.js"></script>

</html>
