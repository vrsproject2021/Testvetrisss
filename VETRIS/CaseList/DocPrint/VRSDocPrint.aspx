<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDocPrint.aspx.cs" Inherits="VETRIS.CaseList.DocPrint.VRSDocPrint" %>

<%@ OutputCache Location="None" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../css/responsive.css" rel="stylesheet" />
    <link href="../../css/style.css" rel="stylesheet" />
    <style type="text/css">
        .body {
            margin: 0 0 0 0px;
        }

        .body, td, th {
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
                    <div id="divConfirm" style="width: 100%; text-align: center; display: none; margin-top: 0px;">
                        <table style="width: 100%; border-collapse: separate; border-spacing: 2px;">
                            <tr>
                                <td style="width: 100%; text-align: center;">
                                    <button type="button" class="btn btn-danger" id="btnPDF" onclick="javascript:Format_OnClick('P');">
                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;Download PDF
                                       
                                    </button>
                                    <button type="button" class="btn btn-primary" id="btnRTF" onclick="javascript:Format_OnClick('R');">
                                        <i class="fa fa-file-word-o" aria-hidden="true"></i>&nbsp;Download RTF
                                       
                                    </button>
                                </td>
                            </tr>

                        </table>
                    </div>
                    <div id="divFile" style="width: 100%; text-align: center; display: none; margin-top: 0px;">
                        <table style="width: 100%; border-collapse: separate; border-spacing: 2px;">
                            <tr>
                                <td style="width: 100%; text-align: center; font-weight: bold; font-size: larger; vertical-align: top;">Report File Generated Successfully
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; font-weight: bold; font-size: medium;">
                                    <span id="flLink"></span> to download the file
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; font-weight: bold; font-size: larger;">&nbsp;
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
        <input type="hidden" id="hdnDirect" runat="server" value="N" />
        <input type="hidden" id="hdnFmt" runat="server" value="P" />
        <input type="hidden" id="hdnReqURL" runat="server" value="" />
        
    </form>
</body>
<script src="scripts/DocPrint.js?05022021" type="text/javascript"></script>
<script type="text/javascript">
    var objhdnErr = document.getElementById('<%=hdnErr.ClientID %>');
    var objhdnServerDocPath = document.getElementById('<%=hdnServerDocPath.ClientID %>');
    var objhdnDocPath = document.getElementById('<%=hdnDocPath.ClientID %>');
    var objhdnDirect = document.getElementById('<%=hdnDirect.ClientID %>');
    var objhdnFmt = document.getElementById('<%=hdnFmt.ClientID %>');
    var objhdnReqURL = document.getElementById('<%=hdnReqURL.ClientID %>');
    LoadDoc();
</script>
</html>
