<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSBeyondOTSubmitConfirm.aspx.cs" Inherits="VETRIS.CaseList.VRSBeyondOTSubmitConfirm" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <title></title>
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/custome-css-style.css" rel="stylesheet" />
    <script type="text/javascript" src="../scripts/jquery-1.11.3.min.js"></script>
</head>
<body class="nav-sm" style="background: transparent;">
    <form id="form1" runat="server">
        <div class="container">
            <div class="main_container singleTask_page" style="margin: 10px;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12 margin-bottom-10">
                        <div class="x_panel">
                            <div class="x_title">
                                <div class="row">
                                    <div class="col-sm-9 col-xs-9">
                                        <h3>
                                            <asp:Label ID="lblMsgHdr" runat="server" Font-Bold="true" Text=""></asp:Label>

                                        </h3>
                                    </div>
                                    <div class="col-sm-3 col-xs-3 text-right">
                                        &nbsp;
                                    </div>
                                    <div class="borderHdr pull-left"></div>
                                    <div class="clearfix"></div>
                                </div>

                            </div>
                            <div class="x_content">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <h4>
                                            <asp:Label ID="lblConfMsg" runat="server"></asp:Label>
                                        </h4>
                                    </div>
                                </div>
                                 <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <h4><asp:Label ID="lblBusinessHr" runat="server"></asp:Label></h4>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <h4>Select one of the following options:</h4>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-sm-6 col-xs-12 margin-bottom-10 margin-top-10">
                                        <div class="pull-left grid_option1 customRadio">
                                            <asp:RadioButton ID="rdoSTAT" runat="server" GroupName="grpOpt" />
                                            <label for="rdoSTAT" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0px; padding-top: 3px; padding-right: 5px;">Submit As STAT</span>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 margin-bottom-10 margin-top-10">
                                        <asp:Label ID="lblStat" runat="server" Text="Report delivery by"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6 col-xs-12 margin-bottom-10 margin-top-10">
                                        <div class="pull-left grid_option1 customRadio">
                                            <asp:RadioButton ID="rdoSTD" runat="server" GroupName="grpOpt" />
                                            <label for="rdoSTD" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0px; padding-top: 3px; padding-right: 5px;">Submit As Standard</span>
                                    </div>
                                    <div class="col-sm-6 col-xs-12 margin-bottom-10 margin-top-10">
                                        <asp:Label ID="lblStd" runat="server" Text="Report delivery by"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6 col-xs-12 margin-bottom-10 margin-top-10">

                                        <div class="pull-left grid_option1 customRadio">
                                            <asp:RadioButton ID="rdoCanc" runat="server" GroupName="grpOpt" />
                                            <label for="rdoCanc" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0px; padding-top: 3px; padding-right: 5px;">Cancel</span>

                                    </div>
                                    <div class="col-sm-6 col-xs-12 margin-bottom-10 margin-top-10">
                                        -----
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-6 col-xs-12 marginTP10" id="divMsg">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="row" style="text-align: center;">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        <button type="button" class="btn btn-primary" id="btnSubmit" runat="server">SUBMIT</button>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript">

    var objrdoSTAT = document.getElementById('<%=rdoSTAT.ClientID %>');
    var objrdoSTD = document.getElementById('<%=rdoSTD.ClientID %>');
    var objrdoCanc = document.getElementById('<%=rdoCanc.ClientID %>');
    var objlblConfMsg = document.getElementById('<%=lblConfMsg.ClientID %>');
    var objdivMsg = document.getElementById("divMsg");
    var strForm = "VRSBeyondOTSubmitConfirm";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/BeyondOTSubmitConfirm.js?05112020"></script>
</html>
