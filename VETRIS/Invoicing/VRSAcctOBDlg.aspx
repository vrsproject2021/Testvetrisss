<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSAcctOBDlg.aspx.cs" Inherits="VETRIS.Invoicing.VRSAcctOBDlg" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

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
    <%--<link href="../css/style.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="vendor/select2/select2.min.js"></script>
   <%-- <link href="../css/CalendarStyle.css" rel="stylesheet" />--%>
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="js/global.js"></script>
    <script src="scripts/AcctOBDlgHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
              <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Opening balance details</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" id="btnAdd1" runat="server" class="btn btn-custon-four btn-primary">
                                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
                            <button type="button" id="btnSave1" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>


            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Billing Account <span class="mandatory">*</span></label>
                                <div class="input-effect">
                                    <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Year <span class="mandatory">*</span></label>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Document Date<span class="mandatory">*</span></label>
                                <div>
                                    <asp:TextBox ID="txtOpBalDate" runat="server" CssClass="form-control" MaxLength="10" Width="100px" Style="float: left;"></asp:TextBox>
                                    <img src="../images/cal.gif" id="imgOpBalDt" runat="server" alt="" style="cursor: pointer; margin-top: 5px; margin-left: 5px;" />
                                    <ComponentArt:Calendar runat="server" ID="CalOpBalDate" AllowMonthSelection="false" AllowMultipleSelection="false"
                                        AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                        ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                        DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                        NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                        PopUp="Custom" PopUpExpandControlId="imgOpBalDt" PrevImageUrl="cal_prevMonth.gif"
                                        SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                        SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                        SwapDuration="300" SwapSlide="Linear">
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="CalOpBalDate_onSelectionChanged" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Document No.<span class="mandatory">*</span></label>
                                <div class="input-effect">
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Opening Balance $<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" style="text-align:right;"></asp:TextBox>
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
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnAdd2" runat="server">
                                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp; Save</button>
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
        <input type="hidden" id="hdnAccountName" runat="server" value="" />

    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlAccount = document.getElementById('<%=ddlAccount.ClientID %>');
    var objddlYear = document.getElementById('<%=ddlYear.ClientID %>');
    var objtxtOpBalDate = document.getElementById('<%=txtOpBalDate.ClientID %>');
    var objtxtInvoiceNo = document.getElementById('<%=txtInvoiceNo.ClientID %>');
    var objtxtAmount = document.getElementById('<%=txtAmount.ClientID %>');
    var strForm = "VRSAcctOBDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/AcctOBDlg.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
