<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSPmtGatewayLink.aspx.cs" Inherits="VETRIS.MyPayment.VRSPmtGatewayLink" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <link id="lnkPMT" runat="server" href = "" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 100%; border-collapse: separate; border-spacing: 2px;">
            <tr>
                <td style="width: 100%; text-align: center;padding-top:100px;padding-bottom:100px;display:inline-block;" id="tdCalc">
                        <table style="width:100%; border-collapse: separate; border-spacing: 2px;">
                            <tr>
                                <td style="width: 100%; text-align: center; font-weight: bold; font-size: larger;">
                                    Calculating outstanding amount
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <img src="../images/calc.gif" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; font-weight: bold; font-size: larger;">Please wait...
                                </td>
                            </tr>
                        </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; text-align: center;padding-top:100px;padding-bottom:100px;display:none;" id="tdBlank">
                        &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 100%; text-align: center;padding-top:100px;padding-bottom:100px;display:none;" id="tdRedirect">
                        <table style="width:100%; border-collapse: separate; border-spacing: 2px;">
                            <tr>
                                <td style="width: 100%; text-align: center; font-weight: bold; font-size: larger;">
                                    Redirecting to payment gateway
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <img src="../images/redirect.gif" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; font-weight: bold; font-size: larger;">
                                    Please do not refresh the page or press the back button
                                </td>
                            </tr>
                        </table>
                </td>
            </tr>
        </table>
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnTotAmt" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnTotAmt = document.getElementById('<%=hdnTotAmt.ClientID %>');
    var strForm = "VRSPmtGatewayLink";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/PmtGatewayLink.js?11052020"></script>
</html>
