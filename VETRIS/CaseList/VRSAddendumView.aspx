<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSAddendumView.aspx.cs" Inherits="VETRIS.CaseList.VRSAddendumView" %>

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
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
     <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/jquery.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Addendum # 
                                <asp:Label ID="lblSrl" runat="server" Text=""></asp:Label>
                            </h2>
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>


            <div class="sparkline10-list mt-b-10" id="divEmail" style="display: block;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="row" style="margin-top: 5px;">

                                        <div class="col-sm-12 col-xs-12 marginMobileTP5" style="height: 80px; overflow: auto;" id="divAddnText" runat="server">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="sparklineHeader marginTP10" id="divMsg" style="display: none;">
                    <div class="sparkline10-hd">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>&nbsp;
                            </h2>
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnDivider" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID %>');
    var strForm = "VRSAddendumView";

    $(document).ready($(function () {
        window.history.forward();
        parent.adjusDataListFrameHeight();
        CheckError();

    }))

    function CheckError() {
        if (parent.Trim(objhdnError.value) != "") {
            document.getElementById("divMsg").style.display = "inline-block";
            parent.adjusDataListFrameHeight();
        }
        objhdnError.value = "";
    }
</script>
</html>
