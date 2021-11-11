<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSApplyPromotion.aspx.cs" Inherits="VETRIS.CaseList.VRSApplyPromotion" %>

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

    <link id="lnkSTYLE" runat="server" href="../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href="../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href="../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h3 class="h3Text">Apply Discount </h3>
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnApply1" runat="server" style="display: inline;">
                                <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Apply       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnRevert1" runat="server" style="display: none;">
                                <i class="fa fa-history" aria-hidden="true"></i>&nbsp;Revert       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

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
                                    <div class="row">
                                        <div class="col-sm-2 col-xs-12 marginMobileTP5">
                                            Study UID
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:Label ID="lblSUID" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                            Institution
                                        </div>
                                        <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                            <asp:Label ID="lblInstitution" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                            Patient Name
                                        </div>
                                        <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                            <asp:Label ID="lblPName" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 marginTP5">
                                            Reason<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                            <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control custom-select-value">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 marginTP5">
                                            Study Cost
                                        </div>
                                        <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" ReadOnly="true" Style="float: left; text-align: right; width: 50%;"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 marginTP10">
                                            Apply By<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                            <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                                                <asp:RadioButton ID="rdoPer" runat="server" GroupName="Disc" />
                                                <label for="rdoPer" class="label-default"></label>
                                            </div>
                                            <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Percentage</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                            Discount %
                                        </div>
                                        <div class="col-sm-8 col-xs-12" style="margin-top: 10px;">
                                            <asp:TextBox ID="txtDiscPer" runat="server" CssClass="form-control" MaxLength="6" Style="float: left; text-align: right; width: 50%;"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                        &nbsp;
                                    </div>
                                    <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                        <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                                            <asp:RadioButton ID="rdoAmt" runat="server" GroupName="Disc" />
                                            <label for="rdoAmt" class="label-default"></label>
                                        </div>
                                        <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Amount</div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12 marginTP10">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                                            Discount ($)
                                        </div>
                                        <div class="col-sm-8 col-xs-12" style="margin-top: 10px;">
                                            <asp:TextBox ID="txtDiscAmt" runat="server" CssClass="form-control" Style="float: left; text-align: right; width: 50%;"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    &nbsp;
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
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-sm-6 col-xs-12">
                            <asp:Label ID="lblInvoiced" runat="server" Font-Bold="true"></asp:Label>
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnApply2" runat="server" style="display: inline;">
                                <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Apply       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnRevert2" runat="server" style="display: none;">
                                <i class="fa fa-history" aria-hidden="true"></i>&nbsp;Revert       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnInvoiced" runat="server" value="N" />
        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnMenuID" runat="server" value="0" />
        <input type="hidden" id="hdnDivider" runat="server" value="0" />
        <input type="hidden" id="hdnDecPlaces" runat="server" value="2" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnInvoiced = document.getElementById('<%=hdnInvoiced.ClientID %>');
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID %>');
    var objhdnDecPlaces = document.getElementById('<%=hdnDecPlaces.ClientID %>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID %>');
    var objrdoPer = document.getElementById('<%=rdoPer.ClientID %>');
    var objrdoAmt = document.getElementById('<%=rdoAmt.ClientID %>');
    var objtxtDiscPer = document.getElementById('<%=txtDiscPer.ClientID %>');
    var objtxtDiscAmt = document.getElementById('<%=txtDiscAmt.ClientID %>');
    var objddlReason = document.getElementById('<%=ddlReason.ClientID %>');
    var objtxtCost = document.getElementById('<%=txtCost.ClientID %>');
    var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');
    var objbtnApply1 = document.getElementById('<%=btnApply1.ClientID %>');
    var objbtnApply2 = document.getElementById('<%=btnApply2.ClientID %>');
    var objbtnRevert1 = document.getElementById('<%=btnRevert1.ClientID %>');
    var objbtnRevert2 = document.getElementById('<%=btnRevert2.ClientID %>');
    var strForm = "VRSApplyPromotion";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/ApplyPromotion.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
