<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSScheduleUpdate.aspx.cs" Inherits="VETRIS.Radiologist.VRSScheduleUpdate" %>

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
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />

    <%--<link href="../css/style.css?1" rel="stylesheet" />
    <link href="../css/CalendarStyle.css" rel="stylesheet" />--%>

    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script type="text/javascript">
        function CalFrom_onSelectionChanged(sender, eventArgs) {
            var dt = new Date(CalFrom.getSelectedDate());
            objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-4 col-xs-4">
                            <h2>
                                <asp:Label ID="lblHdr" runat="server"></asp:Label>
                            </h2>
                        </div>

                        <div class="col-sm-8 col-xs-8 text-right">


                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
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
                                <div class="col-sm-2 col-xs-2 marginTP10">
                                    Radiologist <span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-10 col-xs-10 marginTP5">
                                    <asp:DropDownList ID="ddlRadiologist" runat="server" CssClass="form-control custom-select-value customPadding" Width="60%" Style="float: left;">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row marginTP10">
                                <div class="col-sm-2 col-xs-2 marginTP10">
                                    Date <span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-10 col-xs-10 marginTP5">
                                    <asp:TextBox ID="txtFromDt" runat="server" CssClass="form-control" MaxLength="10" Width="25%" placeholder="" Style="float: left;"></asp:TextBox>
                                    <img id="imgFrom" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 8px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />

                                    <ComponentArt:Calendar runat="server" ID="CalFrom" AllowMonthSelection="false" AllowMultipleSelection="false"
                                        AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                        ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                        DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                        NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                        PopUp="Custom" PopUpExpandControlId="imgFrom" PrevImageUrl="cal_prevMonth.gif"
                                        SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                        SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                        DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                        SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="CalFrom_onSelectionChanged" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                </div>
                            </div>
                            <div class="row marginTP10">
                                <div class="col-sm-2 col-xs-2 marginTP10">
                                    Start Time<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-4 col-xs-4 marginMobileTP5">

                                    <asp:DropDownList ID="ddlFromHr" runat="server" CssClass="form-control custom-select-value customPadding"  Width="30%" Style="float: left;">
                                    </asp:DropDownList>
                                    <span style="float: left; margin-left: 2px; margin-top: 5px;">:</span>
                                    <asp:DropDownList ID="ddlFromMin" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left; margin-left: 2px;">
                                    </asp:DropDownList>
                                   <span style="float: left; margin-left: 2px; margin-top: 5px;">&nbsp;</span>
                                    <asp:DropDownList ID="ddlFromTT" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left; margin-left: 2px;">
                                        <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                        <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-2 col-xs-2 marginTP10">
                                    End Time<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-4 col-xs-4 marginMobileTP5">
                                    <asp:DropDownList ID="ddlTillHr" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left;">
                                    </asp:DropDownList>
                                    <span style="float: left; margin-left: 2px; margin-top: 5px;">:</span>
                                    <asp:DropDownList ID="ddlTillMin" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left; margin-left: 2px;">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlTillTT" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left; margin-left: 2px;">
                                        <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                        <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row marginTP10">
                                <div class="col-sm-2 col-xs-2 marginTP10">
                                    Notes <span class="HelpText">(max. 250 chars.)</span>
                                </div>
                                <div class="col-sm-10 col-xs-10 marginMobileTP5">
                                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" Width="80%" TextMode="MultiLine" Height="70px" placeholder="" ></asp:TextBox>
                                </div>
                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-7 col-xs-12">
                            <asp:Label ID="lblMsg" runat="server" ForeColor="red"></asp:Label>
                        </div>
                        
                        <div class="col-sm-5 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save   
                            </button>
                             <button type="button" class="btn btn-custon-four btn-warning" id="btnDel" runat="server">
                                <i class="fa fa-trash-o edu-danger-error" aria-hidden="true"></i>&nbsp;Delete 
                            </button>
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
        var objddlRadiologist = document.getElementById('<%=ddlRadiologist.ClientID %>');
        var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
        var objddlFromHr = document.getElementById('<%=ddlFromHr.ClientID %>');
        var objddlFromMin = document.getElementById('<%=ddlFromMin.ClientID %>');
        var objddlFromTT = document.getElementById('<%=ddlFromTT.ClientID %>');
        var objddlTillHr = document.getElementById('<%=ddlTillHr.ClientID %>');
        var objddlTillMin = document.getElementById('<%=ddlTillMin.ClientID %>');
        var objddlTillTT = document.getElementById('<%=ddlTillTT.ClientID %>');
        var objtxtNotes = document.getElementById('<%=txtNotes.ClientID %>');
        var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');
       
        var objbtnSave = document.getElementById('<%=btnSave.ClientID %>');
        var objbtnDel = document.getElementById('<%=btnDel.ClientID %>');
        var strForm = "VRSScheduleUpdate";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/ScheduleUpdate.js?01022020"></script>
</html>
