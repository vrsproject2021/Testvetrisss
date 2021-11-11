<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSPromoDetailsParams.aspx.cs" Inherits="VETRIS.Reports.VRSPromoDetailsParams" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
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
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <link href="../css/bootstrap3.3.6.min.css" rel="stylesheet" />
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>--%>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/css/bootstrap-multiselect.css" type="text/css" />

    <link id="lnkSTYLE" runat="server" href="../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href="../css/CalendarStyle.css" rel="stylesheet" type="text/css" />


    <script src="../scripts/jquery.min.js"></script>
    <script src="../scripts/bootstrap3.3.6.min.js"></script>
    <script src="scripts/PromoDetailsRptHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">

                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Promotion wise details</h2>
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
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-6 col-xs-6">
                            <div class="pull-left marginTP10">
                                <h3 class="h3Text">Parameters</h3>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-6">
                            &nbsp;
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch pull-left"></div>
                        </div>


                    </div>

                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-3 col-xs-12 marginTP5">
                                From Date<span class="mandatory">*</span>
                            </div>
                            <div class="col-sm-9 col-xs-12 marginTP5">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" MaxLength="10" Width="100px" Style="float: left;"></asp:TextBox>
                                <img src="../images/cal.gif" id="imgFromDt" runat="server" alt="" style="cursor: pointer; margin-top: 5px; margin-left: 5px;" />
                                <ComponentArt:Calendar runat="server" ID="CalFromDate" AllowMonthSelection="false" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                    ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                    DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                    NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                    PopUp="Custom" PopUpExpandControlId="imgFromDt" PrevImageUrl="cal_prevMonth.gif"
                                    SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                    SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                    DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                    SwapDuration="300" SwapSlide="Linear">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="CalFromDate_onSelectionChanged" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-3 col-xs-12 marginTP5">
                                To Date <span class="mandatory">*</span>
                            </div>
                            <div class="col-sm-9 col-xs-12 marginTP5">

                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" MaxLength="10" Width="100px" Style="float: left;"></asp:TextBox>
                                <img src="../images/cal.gif" id="imgToDt" runat="server" alt="" style="cursor: pointer; margin-top: 5px; margin-left: 5px;" />
                                <ComponentArt:Calendar runat="server" ID="CalToDate" AllowMonthSelection="false" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                    ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                    DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                    NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                    PopUp="Custom" PopUpExpandControlId="imgToDt" PrevImageUrl="cal_prevMonth.gif"
                                    SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                    SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                    DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                    SwapDuration="300" SwapSlide="Linear">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="CalToDate_onSelectionChanged" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-3 col-xs-12 marginTP5">
                                Promotion Reason
                            </div>
                            <div class="col-sm-9 col-xs-12 marginTP5">
                                <select class="form-control custom-select-value" id="multiselect" multiple="multiple" style="width:50%;">
                                </select>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-3 col-xs-12 marginTP5">
                                Institution
                            </div>
                            <div class="col-sm-9 col-xs-12 marginTP5">
                                <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value" Width="50%"></asp:DropDownList>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">

                            <div class="col-sm-3 col-xs-12 marginTP5">
                                &nbsp;
                            </div>
                            <div class="col-sm-9 col-xs-12 marginTP5">

                                <button type="button" id="btnExcel" runat="server" class="btn btn_grd btn-success">
                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;Generate Excel
                                </button>

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
                <span id="spnERR" runat="server"></span>

            </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnMode" runat="server" value="1" />
        <input type="hidden" id="hdnInfo" runat="server" value="" />
        <style>
            .multiselect-container {
                width: 100% !important;
            }

            .checkbox input[type=checkbox] {
                margin-left: -17px;
                margin-top: 0px;
            }

            .btn .caret {
                float: right;
                margin-top: 8px;
            }
            /*Added AM_15-09-2021*/


            .btn-default {
                border-radius: 0px;
            }



            .multiselect-selected-text {
                float: left;
                margin-left: 17px;
            }
        </style>
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    <%--var objddlPromoReason = document.getElementById('<%=ddlPromoReason.ClientID %>');--%>
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objtxtFromDate = document.getElementById('<%=txtFromDate.ClientID %>');
    var objtxtToDate = document.getElementById('<%=txtToDate.ClientID %>');

    var strForm = "VRSPromoDetailsParams";
</script>


<script src="../scripts/custome-javascript.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/js/bootstrap-multiselect.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/PromoDetailsRpt.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
