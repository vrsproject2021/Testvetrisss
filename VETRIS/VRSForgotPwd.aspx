<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSForgotPwd.aspx.cs" Inherits="VETRIS.VRSForgotPwd" %>

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
    <link id="lnkSTYLE" runat="server" href = "css/style.css" rel="stylesheet" type="text/css" />

    <script src="scripts/jquery-1.7.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-8 col-xs-12">
                            <h2>
                                <asp:Label ID="lblHdr" runat="server" Font-Bold="true"></asp:Label>
                            </h2>
                        </div>
                        <div class="col-sm-4 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" onclick="parent.HideGeneralSmall();">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">


                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Login ID<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtLoginID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Enter the email id to receive your password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtMailID" runat="server" CssClass="form-control" MaxLength="100" ReadOnly="true"></asp:TextBox>

                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10" style="text-align: center;">
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12 margin-bottom-10" style="text-align: center;">
                            <button type="button" class="btn btn-primary" id="btnMail" runat="server" onclick="javascript: MailPassword();">Email the password to me</button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12 margin-bottom-10 margin-top-10" style="text-align: center;">
                            <asp:Label ID="lblMsg" runat="server" Text="&nbsp;"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12" style="text-align: center;">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <input type="hidden" id="hdnError" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>');
    var objtxtMailID = document.getElementById('<%=txtMailID.ClientID %>');
    var objbtnMai = document.getElementById('<%=btnMail.ClientID %>');
    var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');

    $(document).ready($(function () {
        parent.adjustGenlSmallFrameHeight();
        GetUserEmailID();
    }))

    function GetUserEmailID() {
        if (objtxtLoginID.value != "") {
            try {
                AjaxPro.timeoutPeriod = 1800000;
                VRSForgotPwd.GetUserEmailID(objtxtLoginID.value, ShowProcess);
            }
            catch (expErr) {
                objlblMsg.innerHTML = expErr.message.toString();
                parent.adjustGenlSmallFrameHeight();
            }
        }
        else
            objtxtMailID.value = "";

    }
    function ProcessUserEmailID(Result) {
        debugger;
        var arrRes = new Array();
        arrRes = Result.value;
        switch (arrRes[0]) {
            case "catch":
                objlblMsg.innerHTML = arrRes[1];
                break;
            case "false":
                objlblMsg.innerHTML = arrRes[1];
                break;
            case "true":
                objtxtMailID.value = arrRes[1];
                break;
        }
        parent.adjustGenlSmallFrameHeight();
    }

    function MailPassword() {
        var ArrRecords = new Array();
        objlblMsg.innerHTML = "Please wait while the system retrieves and emails your password to you...";

        try {
            ArrRecords[0] = objtxtLoginID.value;
            ArrRecords[1] = objtxtMailID.value;
            AjaxPro.timeoutPeriod = 1800000;
            VRSForgotPwd.MailPassWord(ArrRecords, ShowProcess);
        }
        catch (expErr) {
            objlblMsg.innerHTML = expErr.message.toString();
            parent.adjustGenlSmallFrameHeight();
        }
    }
    function PopMailPassWord(Result) {

        var arrRes = new Array();
        arrRes = Result.value;
        switch (arrRes[0]) {
            case "catch":
                objlblMsg.innerHTML = arrRes[1];
                break;
            case "false":
                objlblMsg.innerHTML = arrRes[1];
                break;
            case "true":
                objlblMsg.innerHTML = arrRes[1];
                break;
        }
        parent.adjustGenlSmallFrameHeight();
    }
    function ShowProcess(Result, MethodName) {
        GsText = "";

        var strMethod = MethodName.method;
        switch (strMethod) {
            case "MailPassWord":
                PopMailPassWord(Result);
                break;
            case "GetUserEmailID":
                ProcessUserEmailID(Result);
                break;

        }
    }
</script>
</html>
