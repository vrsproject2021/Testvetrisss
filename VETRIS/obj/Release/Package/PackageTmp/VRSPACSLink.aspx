<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSPACSLink.aspx.cs" Inherits="VETRIS.VRSPACSLink" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .body
        {
            margin: 0 0 0 0px;
        }

        .body, td, th
        {
            font-family: Tahoma,Arial,Verdana,Helvetica, sans-serif;
            color: #736F6E;
            text-decoration: none;
        }
    </style>
</head>
<body class="body">
    <form id="form1" runat="server">
    <table style="width: 100%; border-collapse: separate; border-spacing: 2px; height: 545px;">
            <tr>
                <td style="width: 100%; text-align: center;">
                   
                    <div id="divProcess" style="width: 100%; text-align: center;">
                        <table style="display: inline-block; width: 200px; border-collapse: separate; border-spacing: 2px;">
                            <tr>
                                <td style="width: 100%; text-align: center; font-weight: bold; font-size: larger;">Connecting eRAD
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <img src="images/wait_basket.gif" alt="" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; font-weight: bold; font-size: larger;">Please wait...
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divError" style="width: 100%; text-align: center; display: none;">
                        <table style="display: inline-block; width: 200px; border-collapse: separate; border-spacing: 2px;">
                            <tr>
                                <td style="width: 100%; text-align: center; font-weight: bold; font-size: larger;">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>


                        </table>
                    </div>
                </td>
            </tr>

        </table>
         <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
         <input type="hidden" id="hdnError" runat="server" value="" />
    </form>
</body>
    <script type="text/javascript">
        var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
        var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
        var objlblError = document.getElementById('<%=lblError.ClientID %>')
        var strForm = "VRSPACSLink";

        FetchReportDetails();

        function FetchReportDetails() {
            var arrRes = new Array();
            AjaxPro.timeoutPeriod = 1800000;
            var Result = VRSPACSLink.FetchReportViewDetails(objhdnID.value);

            arrRes = Result.value;
            switch (arrRes[0]) {
                case "catch":
                case "false":
                    if (document.all) objlblError.innerText = arrRes[1];
                    else objlblError.textContent = arrRes[1];
                    break;
                case "true":
                    //alert(arrRes[1]);
                     window.location.href = arrRes[1];
                   // window.location.href = "https://pacs.vcradiology.com/cases/reportview.jsp?cPrintFriendly=true&cGr=1.2.840.113619.2.134.1762929619.1728.1559491687.525";
                    break;
            }
        }

</script>
</html>
