<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSSchedule.aspx.cs" Inherits="VETRIS.Radiologist.VRSSchedule" %>

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

    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkSCHVW" runat="server" href = "../css/schedulerView.css" rel="stylesheet" type="text/css" />


    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/ScheduleHdr.js?01012020"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Radiologist Schedule</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" id="btnCreateCanc" onclick="view_Searchform();">
                                <span  class="pull-left searchBySpan">Create/Cancel Schedule
                                </span>
                                <span id="Span1" class="pull-right">

                                    <i style="font-size: 12px;" class="fa fa-pencil-square-o searchBySpan" aria-hidden="true"></i>
                                </span>
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="searchSection searchPart searchPanelBg" id="searchSection" >
                <div>
                    <div>
                        <div class="row">
                            <div class="col-sm-5 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-sm-12 col-xs-12">
                                                <div class="searchSection">
                                                    <div class="row">
                                                        <div class="col-sm-12 col-xs-12">
                                                            <div class="pull-left">
                                                                <h3 class="h3Text">Radiologists</h3>
                                                            </div>
                                                            <div class="borderSearch pull-left"></div>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-left: 2px; margin-right: 1px;">
                                                        <div class="table-responsive">
                                                            <ComponentArt:CallBack ID="CallBackRad" runat="server" OnCallback="CallBackRad_Callback">
                                                                <Content>
                                                                    <ComponentArt:Grid
                                                                        ID="grdRAD"
                                                                        CssClass="Grid"
                                                                        DataAreaCssClass="GridData5"
                                                                        SearchOnKeyPress="true"
                                                                        EnableViewState="true"
                                                                        ShowSearchBox="true"
                                                                        SearchBoxPosition="TopLeft"
                                                                        SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                                        ShowHeader="true"
                                                                        FooterCssClass="GridFooter"
                                                                        GroupingNotificationText=""
                                                                        PageSize="4"
                                                                        ScrollBar="Auto"
                                                                        ScrollTopBottomImagesEnabled="true"
                                                                        ScrollTopBottomImageHeight="2"
                                                                        ScrollTopBottomImageWidth="16"
                                                                        ScrollImagesFolderUrl="../images/scroller/"
                                                                        ScrollButtonWidth="16"
                                                                        ScrollButtonHeight="17" ShowFooter="false"
                                                                        ScrollBarCssClass="ScrollBar"
                                                                        ScrollGripCssClass="ScrollGrip"
                                                                        ScrollBarWidth="16"
                                                                        PagerTextCssClass="GridFooterText"
                                                                        ImagesBaseUrl="../images/"
                                                                        LoadingPanelFadeDuration="1000"
                                                                        LoadingPanelFadeMaximumOpacity="80"
                                                                        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                        LoadingPanelPosition="MiddleCenter"
                                                                        Width="98%"
                                                                        runat="server" HeaderCssClass="GridHeader"
                                                                        GroupingNotificationPosition="TopRight">

                                                                        <Levels>
                                                                            <ComponentArt:GridLevel
                                                                                AllowGrouping="false"
                                                                                DataKeyField="srl_no"
                                                                                ShowTableHeading="false"
                                                                                TableHeadingCssClass="GridHeader"
                                                                                RowCssClass="Row"
                                                                                HoverRowCssClass="HoverRow"
                                                                                ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                                DataCellCssClass="DataCell"
                                                                                HeadingCellCssClass="HeadingCell"
                                                                                HeadingRowCssClass="HeadingRow"
                                                                                HeadingTextCssClass="HeadingCellText"
                                                                                SortedDataCellCssClass="SortedDataCell"
                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                SortImageWidth="10"
                                                                                SortImageHeight="19">
                                                                                <ConditionalFormats>
                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRAD.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRAD.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                                </ConditionalFormats>
                                                                                <Columns>
                                                                                    <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="radiologist_id" AllowGrouping="false" Visible="false" />
                                                                                    <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" DataCellClientTemplateId="SELRAD" HeadingText="Select" FixedWidth="True" Width="40" />
                                                                                    <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Radiologist" AllowGrouping="false" DataCellClientTemplateId="RAD" FixedWidth="True" Width="200" />

                                                                                </Columns>

                                                                            </ComponentArt:GridLevel>
                                                                        </Levels>

                                                                        <%-- <ClientEvents>
                                                                            <RenderComplete EventHandler="grdRAD_onRenderComplete" />
                                                                        </ClientEvents>--%>
                                                                        <ClientTemplates>
                                                                            <ComponentArt:ClientTemplate ID="RAD">
                                                                                <span id="spnRAD_## DataItem.GetMember('srl_no').Value ##" title="## DataItem.GetMember('name').Value ##">## DataItem.GetMember('name').Value ##</span>
                                                                            </ComponentArt:ClientTemplate>
                                                                            <ComponentArt:ClientTemplate ID="SELRAD">
                                                                                <div class="grid_option">
                                                                                    <input type="checkbox" id="chkSel_## DataItem.GetMember('srl_no').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSel_OnClick('## DataItem.GetMember('srl_no').Value ##');" />
                                                                                    <label for="chkSel_## DataItem.GetMember('srl_no').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                                                </div>
                                                                            </ComponentArt:ClientTemplate>
                                                                        </ClientTemplates>
                                                                    </ComponentArt:Grid>
                                                                    <span id="spnErrRAD" runat="server"></span>
                                                                </Content>
                                                                <LoadingPanelClientTemplate>
                                                                    <table style="height: 190px; width: 100%;" border="0">
                                                                        <tr>
                                                                            <td style="text-align: center;">
                                                                                <table border="0" style="width: 70px; display: inline-block;">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img src="../images/spinner-darkgrey.gif" width="50" height="65" border="0" alt="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </LoadingPanelClientTemplate>
                                                                <ClientEvents>
                                                                    <CallbackComplete EventHandler="grdRAD_onCallbackComplete" />

                                                                </ClientEvents>
                                                            </ComponentArt:CallBack>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-7 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="searchSection">
                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12">
                                                    <div class="pull-left">
                                                        <h3 class="h3Text">Date/Time Range</h3>
                                                    </div>
                                                    <div class="borderSearch pull-left"></div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-2 col-xs-12 marginTP5">
                                                    Start Date<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtFromDt" runat="server" CssClass="form-control" MaxLength="10" Width="50%" placeholder="" Style="float: left;"></asp:TextBox>
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
                                                <div class="col-sm-2 col-xs-12 marginTP5">
                                                    End Date
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtTillDt" runat="server" CssClass="form-control" MaxLength="10" Width="50%" placeholder="" Style="float: left;"></asp:TextBox>
                                                    <img id="imgTill" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 8px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />
                                                    <ComponentArt:Calendar runat="server" ID="CalTill" AllowMonthSelection="false" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                                        ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                                        DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                                        NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                                        PopUp="Custom" PopUpExpandControlId="imgTill" PrevImageUrl="cal_prevMonth.gif"
                                                        SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                                        SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                                        SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="CalTill_onSelectionChanged" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </div>

                                            </div>
                                            <div class="row marginTP5">
                                                <div class="col-sm-2 col-xs-12 marginTP5">
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginTP5 text-right">
                                                    <label style="margin-right: 5px; font-weight: bold;">OR</label>
                                                </div>
                                                <div class="col-sm-2 col-xs-12 marginTP5">
                                                    <div class="grid_option1">

                                                        <input type="checkbox" id="chkNext" runat="server" style="width: 18px; height: 18px; float: left;" />
                                                        <label for="chkNext" style="margin-top: 5px; margin-left: 5px; float: left;">For Next</label>

                                                    </div>

                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginTP5">

                                                    <asp:TextBox ID="txtDays" runat="server" CssClass="form-control" MaxLength="10" Width="30%" ReadOnly="true" placeholder="" Style="float: left; text-align: center;"></asp:TextBox>
                                                    <label style="margin-top: 8px; margin-left: 5px; float: left;">Day(s)</label>

                                                </div>
                                            </div>

                                            <div class="row marginTP10">
                                                <div class="col-sm-2 col-xs-12 marginTP5">
                                                    Start Time
                                                </div>

                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">

                                                    <asp:DropDownList ID="ddlFromHr" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left;">
                                                    </asp:DropDownList>
                                                    <span style="float: left; margin-left: 2px; margin-top: 5px;">:</span>
                                                    <asp:DropDownList ID="ddlFromMin" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left; margin-left: 2px;">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlFromTT" runat="server" CssClass="form-control custom-select-value customPadding" Width="30%" Style="float: left; margin-left: 2px;">
                                                        <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                                        <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<label style="margin-top: 8px; margin-left: 35px; float: left;">End Time</label>--%>
                                                </div>
                                                <div class="col-sm-2 col-xs-12 marginTP5">
                                                    End Time
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
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
                                                <%-- <div class="col-sm-2 col-xs-12 marginTP5">
                                                </div>--%>
                                            </div>
                                            <div class="row marginTP5">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="searchSection">
                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12">
                                                    <div class="pull-left">
                                                        <h3 class="h3Text">For Week Day(s)</h3>
                                                    </div>
                                                    <div class="borderSearch pull-left"></div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12 marginTP5">
                                                    <div class="row">
                                                        <div class="col-sm-12 col-xs-12 marginTP5">
                                                            <input type="checkbox" id="chkMon" runat="server" style="width: 18px; height: 18px; float: left;" />
                                                            <label for="chkMon" style="margin-top: 5px; margin-left: 5px; float: left;">Monday</label>
                                                            <input type="checkbox" id="chkTue" runat="server" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkTue" style="margin-top: 5px; margin-left: 5px; float: left;">Tuesday</label>
                                                            <input type="checkbox" id="chkWed" runat="server" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkWed" style="margin-top: 5px; margin-left: 5px; float: left;">Wednesday</label>
                                                            <input type="checkbox" id="chkThu" runat="server" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkThu" style="margin-top: 5px; margin-left: 5px; float: left;">Thursday</label>
                                                            <input type="checkbox" id="chkFri" runat="server" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkFri" style="margin-top: 5px; margin-left: 5px; float: left;">Friday</label>
                                                            <input type="checkbox" id="chkSat" runat="server" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkSat" style="margin-top: 5px; margin-left: 5px; float: left;">Saturday</label>
                                                            <input type="checkbox" id="chkSun" runat="server" style="margin-left: 10px; width: 18px; height: 18px; float: left;" />
                                                            <label for="chkSun" style="margin-top: 5px; margin-left: 5px; float: left;">Sunday</label>
                                                        </div>
                                                    </div>
                                                    <%--<div class="row">
                                                        <div class="col-sm-12 col-xs-12 marginTP5">
                                                            
                                                        </div>
                                                    </div>--%>
                                                </div>

                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12 text-right marginTP5">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12 text-right marginTP5">
                                <button type="button" class="btn btn-warning" id="btnCreate" runat="server">
                                    <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Create Schedule
                                           
                                </button>
                                <button type="button" class="btn btn-danger" id="btnCancel" runat="server">
                                    <i class="fa fa-trash" aria-hidden="true"></i>&nbsp;Cancel Schedule
                                           
                                </button>
                                <button type="button" class="btn btn-success" id="btnReset" runat="server">
                                    <i class="fa fa-repeat" aria-hidden="true"></i>&nbsp;Reset Schedule
                                           
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-6 col-xs-12">

                                <div class="col-sm-1 col-xs-12">
                                    <button type="button" class="btn btn-primary pull-left" id="btnPrev" runat="server" title="Previous">
                                        <i class="fa fa-chevron-left" aria-hidden="true"></i>
                                    </button>
                                </div>
                                <div class="col-sm-10 col-xs-12">
                                    <div class="col-sm-1 col-xs-12 marginTP5">
                                        From<span class="mandatory">*</span>
                                    </div>
                                    <div class="col-sm-5 col-xs-12 marginMobileTP5">
                                        <asp:TextBox ID="txtStartDt" runat="server" CssClass="form-control" MaxLength="10" Width="65%" placeholder="" Style="float: left;"></asp:TextBox>
                                        <img id="imgStart" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 8px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />

                                        <ComponentArt:Calendar runat="server" ID="CalStart" AllowMonthSelection="false" AllowMultipleSelection="false"
                                            AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                            ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                            DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                            NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                            PopUp="Custom" PopUpExpandControlId="imgStart" PrevImageUrl="cal_prevMonth.gif"
                                            SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                            SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                            DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                            SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="CalStart_onSelectionChanged" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </div>
                                    <div class="col-sm-1 col-xs-12 marginTP5">
                                        Till<span class="mandatory">*</span>
                                    </div>
                                    <div class="col-sm-5 col-xs-12 marginMobileTP5">
                                        <asp:TextBox ID="txtEndDt" runat="server" CssClass="form-control" MaxLength="10" Width="65%" placeholder="" Style="float: left;"></asp:TextBox>
                                        <img id="imgEnd" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 8px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />

                                        <ComponentArt:Calendar runat="server" ID="CalEnd" AllowMonthSelection="false" AllowMultipleSelection="false"
                                            AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                            ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                            DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                            NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                            PopUp="Custom" PopUpExpandControlId="imgEnd" PrevImageUrl="cal_prevMonth.gif"
                                            SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                            SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                            DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                            SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="CalEnd_onSelectionChanged" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </div>
                                </div>
                                <div class="col-sm-1 col-xs-12">
                                    <button type="button" class="btn btn-primary pull-left marginLFT10" id="btnNext" runat="server" title="Next">
                                        <i class="fa fa-chevron-right" aria-hidden="true"></i>
                                    </button>
                                </div>

                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    <div class="pull-right">
                                        Filter By Radiologist   
                                    </div>
                                    
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:DropDownList ID="ddlRadiologist" runat="server" CssClass="form-control custom-select-value customPadding" Width="90%" Style="float: left;">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="static-table-list">
                        <div class="table-responsive">
                            <ComponentArt:CallBack ID="CallBackSch" runat="server" OnCallback="CallBackSch_Callback">
                                <Content>
                                    <ComponentArt:Scheduler runat="server" ID="schRad"
                                        AutoUpdate="true"
                                        AutoUpdateMode="PostBack"
                                        OnAppointmentAdded="schRad_AppointmentAdded"
                                        OnAppointmentModified="schRad_AppointmentModified"
                                        OnAppointmentRemoved="schRad_AppointmentRemoved">
                                        
                                    </ComponentArt:Scheduler>
                                    

                                    <ComponentArt:SchedulerDaysView runat="server"
                                        ID="viewRad"
                                        SchedulerID="schRad"
                                        AppointmentCssClass="appointment"
                                        AppointmentFocusCssClass="appointment appointmentFocus"
                                        AppointmentHorizontalSpacing="-20"
                                        AppointmentFooterCssClass="appointmentFooter"
                                        AppointmentFooterHeight="9"
                                        AppointmentShowFooter="true"
                                        AppointmentFocusHorizontalSpacing="-20"
                                        AppointmentFocusFooterCssClass="appointmentFooter"
                                        AppointmentFocusFooterHeight="9"
                                        AppointmentFocusShowFooter="true"
                                        ColumnCssClass="column"
                                        ColumnHeaderCellCssClass="columnHeaderCell columnHeaderCellNotToday"
                                        ColumnHeaderDateFormat="dd MMM yy'<br />'dddd"
                                        ColumnHeaderHeight="40px" 
                                        ColumnPaddingLeft="1"
                                        ColumnPaddingRight="2"
                                        ColumnWidth="200px"
                                        CssClass="view"
                                        GridCssClass="surface"
                                        Precision="0.0:01"
                                        RowCssClass="viewrow"
                                        RowHeaderCellCssClass="rowHeaderCell"
                                        RowHeaderTimeFormat="hh:mm tt"
                                        RowHeaderWidth="42px"
                                        RowHeight="45px"
                                        TodayColumnCssClass="column columnToday"
                                        TodayColumnHeaderCellCssClass="columnHeaderCell columnHeaderCellToday"
                                        UseServerNow="true">
                                        <RowHeaderCellClientTemplate>##rowHeaderCellHtml(Parent,DataItem)##</RowHeaderCellClientTemplate>
                                       <%-- <AppointmentLooks>
                                            <ComponentArt:SchedulerAppointmentLook LookId="redAppointment"
                                                CssClass="appointment redTitle" />
                                        </AppointmentLooks>--%>
                                        <ClientEvents>
                                            <AppointmentModifyOpen EventHandler="viewRad_AppointmentModifyOpen" />
                                            <AppointmentAddOpen EventHandler="viewRad_AppointmentAddOpen" />
                                            
                                        </ClientEvents>
                                    </ComponentArt:SchedulerDaysView>
                                    <span id="spnERR" runat="server"></span>
                                    <span id="spnCnt" runat="server"></span>
                                    <span id="spnModify" runat="server"></span>
                                </Content>
                                <ClientEvents>
                                    <CallbackComplete EventHandler="schRad_onCallbackComplete" />
                                    <CallbackError EventHandler="schRad_OnCallbackError" />

                                </ClientEvents>
                            </ComponentArt:CallBack>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnRadID" runat="server" value="00000000-0000-0000-0000-000000000000" />
         <input type="hidden" id="hdnViewSchedule" runat="server" value="A" />
        <input type="hidden" id="hdnRADCALSTARTTIME" runat="server" value="00:00" />

    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtTillDt = document.getElementById('<%=txtTillDt.ClientID %>');
    var objimgTill = document.getElementById('<%=imgTill.ClientID %>');
    var objchkNext = document.getElementById('<%=chkNext.ClientID %>');
    var objtxtDays = document.getElementById('<%=txtDays.ClientID %>');
    var objddlFromHr = document.getElementById('<%=ddlFromHr.ClientID %>');
    var objddlFromMin = document.getElementById('<%=ddlFromMin.ClientID %>');
    var objddlFromTT = document.getElementById('<%=ddlFromTT.ClientID %>');
    var objddlTillHr = document.getElementById('<%=ddlTillHr.ClientID %>');
    var objddlTillMin = document.getElementById('<%=ddlTillMin.ClientID %>');
    var objddlTillTT = document.getElementById('<%=ddlTillTT.ClientID %>');
    var objchkMon  = document.getElementById('<%=chkMon.ClientID %>');
    var objchkTue = document.getElementById('<%=chkTue.ClientID %>');
    var objchkWed = document.getElementById('<%=chkWed.ClientID %>');
    var objchkThu = document.getElementById('<%=chkThu.ClientID %>');
    var objchkFri = document.getElementById('<%=chkFri.ClientID %>');
    var objchkSat = document.getElementById('<%=chkSat.ClientID %>');
    var objchkSun = document.getElementById('<%=chkSun.ClientID %>');
    var objtxtStartDt= document.getElementById('<%=txtStartDt.ClientID %>');
    var objtxtEndDt  = document.getElementById('<%=txtEndDt.ClientID %>');
    var objbtnPrev= document.getElementById('<%=btnPrev.ClientID %>');
    var objbtnNext= document.getElementById('<%=btnNext.ClientID %>');
    var objddlRadiologist = document.getElementById('<%=ddlRadiologist.ClientID %>');
    var objhdnRadID = document.getElementById('<%=hdnRadID.ClientID %>');
    var objhdnViewSchedule= document.getElementById('<%=hdnViewSchedule.ClientID %>');
    var objhdnRADCALSTARTTIME = document.getElementById('<%=hdnRADCALSTARTTIME.ClientID %>');
    var strForm = "VRSSchedule";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?03022020"></script>
<script src="scripts/Schedule.js?07072021"></script>
</html>
