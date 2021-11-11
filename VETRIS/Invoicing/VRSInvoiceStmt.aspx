<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInvoiceStmt.aspx.cs" Inherits="VETRIS.Invoicing.VRSInvoiceStmt" %>

<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <%--<link href="../css/style.css" rel="stylesheet" />--%>
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
                            <h2>Invoice Statement</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>
                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                             <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Parameters</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP10">
                                    <div class="col-sm-3 col-xs-12 marginTP5">
                                        Billing Cycle<span class="mandatory">*</span>
                                    </div>
                                    <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                        <asp:DropDownList ID="ddlBillingCycle" runat="server" CssClass="form-control custom-select-value" Width="50%"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP10">
                                    <div class="col-sm-3 col-xs-12 marginTP5">
                                        Billing Account
                                    </div>
                                    <div class="col-sm-9 col-xs-12 marginMobileTP5">
                                        <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control custom-select-value" Width="50%"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 text-center">
                                    <button type="button" id="btnGen" runat="server" class="btn btn-primary">
                                        <i class="fa fa-cog" aria-hidden="true"></i>&nbsp;Generate

                                    </button>
                                </div>
                            </div>


                        </div>

                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp; Close</button>
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
    var objddlBillingCycle = document.getElementById('<%=ddlBillingCycle.ClientID %>');
    var objddlAccount = document.getElementById('<%=ddlAccount.ClientID %>');
    var strForm = "VRSInvoiceStmt";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/InvoiceStmt.js"></script>
</html>
