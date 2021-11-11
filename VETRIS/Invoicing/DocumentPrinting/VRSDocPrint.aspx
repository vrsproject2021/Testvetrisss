<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDocPrint.aspx.cs" Inherits="VETRIS.Invoicing.DocumentPrinting.VRSDocPrint" %>
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
                                <td style="width: 100%; text-align: center; font-weight: bold; font-size: larger;">Generating Report
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <img src="../../images/wait_basket.gif" alt="" />
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
        <input type="hidden" id="hdnErr" runat="server" value="N" />
        <input type="hidden" id="hdnServerDocPath" runat="server" value="" />
        <input type="hidden" id="hdnDocPath" runat="server" value="" />
    </form>
</body>
<script src="scripts/DocPrint.js" type="text/javascript"></script>
<script type="text/javascript">
    var objhdnErr = document.getElementById('<%=hdnErr.ClientID %>');
    var objhdnServerDocPath = document.getElementById('<%=hdnServerDocPath.ClientID %>');
    var objhdnDocPath = document.getElementById('<%=hdnDocPath.ClientID %>');
    LoadDoc();
</script>
</html>
