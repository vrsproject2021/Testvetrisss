<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDRLog.aspx.cs" Inherits="VETRIS.HouseKeeping.VRSDRLog" %>

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
    <%--<link href="../css/style.css" rel="stylesheet" />
    <link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/tabStyle1.css" rel="stylesheet" />
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <link id="lnkTAB" runat="server" href = "../css/tabStyle1.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/DRLogHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">

                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Logs</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" id="btnClose" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close

                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class=" row mt-b-10 marginTP10">
                <div class="col-sm-12 col-xs-12">
                    <div class=" row mt-b-10 marginTP10">
                        <div class="col-sm-12 col-xs-12">
                            <ComponentArt:TabStrip ID="tsLog"
                                CssClass="TopGroup"
                                SiteMapXmlFile="LogTabData.xml"
                                EnableTheming="false" DefaultGroupSeparatorWidth="5px"
                                DefaultGroupShowSeparators="true"
                                DefaultItemLookId="DefaultTabLook"
                                DefaultSelectedItemLookId="SelectedTabLook"
                                DefaultGroupTabSpacing="1"
                                ImagesBaseUrl="../images/"
                                MultiPageId="ImageManagerPages"
                                Width="100%"
                                runat="server">
                                <ItemLooks>
                                    <ComponentArt:ItemLook LookId="DefaultTabLook" CssClass="DefaultTab" HoverCssClass="DefaultTabHover" LabelPaddingLeft="0" LabelPaddingRight="1" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconWidth="10" />
                                    <ComponentArt:ItemLook LookId="SelectedTabLook" CssClass="SelectedTab" LabelPaddingLeft="2" LabelPaddingRight="1" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconWidth="10" />
                                </ItemLooks>
                                <%--<ClientEvents>
                                                <TabSelect EventHandler="tsLog_OnSelect" />
                                            </ClientEvents>--%>
                            </ComponentArt:TabStrip>
                        </div>
                    </div>
                    <div class=" row mt-b-10 marginTP10">
                        <div class=" row mt-b-10 marginTP10">
                            <div class="col-sm-12 col-xs-12">
                                <ComponentArt:MultiPage ID="ImageManagerPages" runat="server" Width="100%">
                                    <ComponentArt:PageView ID="UA" runat="server" Width="100%">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="sparkline10-list mt-b-10">

                                                <div class="row">

                                                    <div class="col-sm-4 col-xs-12">
                                                        <div class="pull-left" style="margin-left: 85px;">Log Date Between</div>
                                                    </div>


                                                    <div class="col-sm-4 col-xs-12">
                                                        User                                                       
                                                    </div>
                                                    <div class="col-sm-4 col-xs-12">
                                                        Activity Text
                                                    </div>

                                                </div>


                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFromDtUA" runat="server" CssClass="form-control" MaxLength="10" Width="30%" placeholder="" Style="float: left;"></asp:TextBox>
                                                        <img id="imgFromUA" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 8px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />

                                                        <ComponentArt:Calendar runat="server" ID="CalFromUA" AllowMonthSelection="false" AllowMultipleSelection="false"
                                                            AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                                            ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                                            DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                                            NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                                            PopUp="Custom" PopUpExpandControlId="imgFromUA" PrevImageUrl="cal_prevMonth.gif"
                                                            SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                                            SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                                            SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                                            <ClientEvents>
                                                                <SelectionChanged EventHandler="CalFromUA_onSelectionChanged" />
                                                            </ClientEvents>
                                                        </ComponentArt:Calendar>
                                                        <asp:TextBox ID="txtTillDtUA" runat="server" CssClass="form-control" MaxLength="10" Width="30%" placeholder="" Style="float: left; margin-left: 20px;"></asp:TextBox>
                                                        <img id="imgTillUA" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 8px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />
                                                        <ComponentArt:Calendar runat="server" ID="CalTillUA" AllowMonthSelection="false" AllowMultipleSelection="false"
                                                            AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                                            ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                                            DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                                            NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                                            PopUp="Custom" PopUpExpandControlId="imgTillUA" PrevImageUrl="cal_prevMonth.gif"
                                                            SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                                            SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                                            SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                                            <ClientEvents>
                                                                <SelectionChanged EventHandler="CalTillUA_onSelectionChanged" />
                                                            </ClientEvents>
                                                        </ComponentArt:Calendar>
                                                        <div class="clearfix"></div>
                                                    </div>

                                                    <div class="col-sm-4 marginMobileTP10">
                                                        <div class="form-select-list">
                                                            <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4 marginMobileTP10">
                                                        <asp:TextBox ID="txtActivity" runat="server" CssClass="form-control" placeholder="" Style="float: left;"></asp:TextBox>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12 col-xs-12 text-right marginTP5">
                                                        <button type="button" class="btn btn-custon-four btn-success" id="btnOKUA" runat="server">
                                                            <i class="fa fa-check" aria-hidden="true"></i>&nbsp; OK</button>

                                                    </div>

                                                </div>
                                            </div>
                                            <div class="sparkline10-list mt-b-10">
                                                <div class="sparkline10-graph">
                                                    <div class="static-table-list">
                                                        <div class="table-responsive">
                                                            <ComponentArt:CallBack ID="CallBackUA" runat="server" OnCallback="CallBackUA_Callback">
                                                                <Content>
                                                                    <ComponentArt:Grid
                                                                        ID="grdUA"
                                                                        CssClass="Grid"
                                                                        DataAreaCssClass="GridData17"
                                                                        SearchOnKeyPress="true"
                                                                        EnableViewState="true"
                                                                        RunningMode="Client"
                                                                        ShowSearchBox="false"
                                                                        SearchBoxPosition="TopLeft"
                                                                        SearchTextCssClass="GridHeaderText"
                                                                        ShowHeader="false"
                                                                        FooterCssClass="GridFooter"
                                                                        GroupingNotificationText=""
                                                                        PageSize="20"
                                                                        PagerPosition="BottomRight"
                                                                        PagerInfoPosition="BottomLeft"
                                                                        PagerStyle="Buttons"
                                                                        PagerTextCssClass="GridFooterText"
                                                                        PagerButtonWidth="24"
                                                                        PagerButtonHeight="24"
                                                                        PagerButtonHoverEnabled="true"
                                                                        SliderHeight="26"
                                                                        SliderWidth="150"
                                                                        SliderGripWidth="9"
                                                                        SliderPopupOffsetX="80"
                                                                        SliderPopupClientTemplateId="SliderTemplate"
                                                                        SliderPopupCachedClientTemplateId="CachedSliderTemplate"
                                                                        ImagesBaseUrl="../images/"
                                                                        PagerImagesFolderUrl="../images/pager/"
                                                                        LoadingPanelFadeDuration="1000"
                                                                        LoadingPanelFadeMaximumOpacity="80"
                                                                        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                        LoadingPanelPosition="MiddleCenter"
                                                                        Width="100%"
                                                                        runat="server"
                                                                        HeaderCssClass="GridHeader"
                                                                        GroupingNotificationPosition="TopRight"
                                                                        SearchBoxCssClass="EditTextBoxStyle">

                                                                        <Levels>
                                                                            <ComponentArt:GridLevel
                                                                                AllowGrouping="false"
                                                                                DataKeyField="rec_id"
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
                                                                                SelectedRowCssClass="SelectedRow"
                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                SortImageWidth="10"
                                                                                SortImageHeight="19">
                                                                                <ConditionalFormats>

                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                </ConditionalFormats>
                                                                                <Columns>
                                                                                    <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                    <ComponentArt:GridColumn DataField="log_date" Align="left" HeadingText="Date/Time" AllowGrouping="false" Width="120" />
                                                                                    <ComponentArt:GridColumn DataField="session_id" Align="left" HeadingText="Session ID" AllowGrouping="false" Width="250" />
                                                                                    <ComponentArt:GridColumn DataField="user_name" Align="left" HeadingText="User Name" AllowGrouping="false" DataCellClientTemplateId="USERNAME" FixedWidth="True" Width="120" />
                                                                                    <ComponentArt:GridColumn DataField="log_message" Align="left" HeadingText="Activity" AllowGrouping="false" DataCellClientTemplateId="UALOGDESC" FixedWidth="True" Width="600" />
                                                                                </Columns>

                                                                            </ComponentArt:GridLevel>
                                                                        </Levels>


                                                                        <ClientTemplates>

                                                                            <ComponentArt:ClientTemplate ID="USERNAME">
                                                                                <span id="spnNameUA_## DataItem.GetMember('rec_id').Value ##" title="## DataItem.GetMember('user_name').Value ##">## DataItem.GetMember('user_name').Value ##</span>
                                                                            </ComponentArt:ClientTemplate>
                                                                            <ComponentArt:ClientTemplate ID="UALOGDESC">
                                                                                <span id="spnLMUA_## DataItem.GetMember('rec_id').Value ##" title="## DataItem.GetMember('log_message').Value ##">## DataItem.GetMember('log_message').Value ##</span>
                                                                            </ComponentArt:ClientTemplate>


                                                                        </ClientTemplates>
                                                                    </ComponentArt:Grid>
                                                                    <span id="spnUAErr" runat="server"></span>
                                                                </Content>
                                                                <LoadingPanelClientTemplate>
                                                                    <table style="height: 610px; width: 100%;" border="0">
                                                                        <tr>
                                                                            <td style="text-align: center;">
                                                                                <table border="0" style="width: 70px; display: inline-block;">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img src="../images/Searching.gif" border="0" alt="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </LoadingPanelClientTemplate>
                                                                <ClientEvents>
                                                                    <CallbackComplete EventHandler="grdUA_onCallbackComplete" />
                                                                </ClientEvents>
                                                            </ComponentArt:CallBack>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ComponentArt:PageView>
                                    <ComponentArt:PageView ID="DR" runat="server" Width="100%">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="sparkline10-list mt-b-10">

                                                <div class="row">
                                                    <div class="col-sm-4 col-xs-12">
                                                        <div class="form-group">
                                                            <label class="control-label" for="username">Institution</label>
                                                            <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4 col-xs-12">
                                                        <div class="form-select-list">
                                                            <label class="control-label">Service</label>
                                                            <asp:DropDownList ID="ddlService" runat="server" CssClass="form-control custom-select-value">
                                                                <asp:ListItem Value="0" Text="All" Selected="true"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Dicom Receiving Service"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Dicom Sending Service"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4 col-xs-12">
                                                        <div class="form-select-list">
                                                            <label class="control-label">Log Type</label>
                                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control custom-select-value">
                                                                <asp:ListItem Value="X" Text="All" Selected="true"></asp:ListItem>
                                                                <asp:ListItem Value="N" Text="Information"></asp:ListItem>
                                                                <asp:ListItem Value="Y" Text="Error"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="pull-left" style="margin-top: 10px; margin-left: 85px;">Log Date Between</div>

                                                    </div>
                                                    <div class="col-sm-8 col-xs-12" style="margin-top: 10px;">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFromDt" runat="server" CssClass="form-control" MaxLength="10" Width="30%" placeholder="" Style="float: left;"></asp:TextBox>
                                                        <img id="imgFrom" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 8px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />

                                                        <ComponentArt:Calendar runat="server" ID="CalFrom" AllowMonthSelection="false" AllowMultipleSelection="false"
                                                            AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                                            ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                                            DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                                            NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                                            PopUp="Custom" PopUpExpandControlId="imgFrom" PrevImageUrl="cal_prevMonth.gif"
                                                            SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                                            SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                                            SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                                            <ClientEvents>
                                                                <SelectionChanged EventHandler="CalFrom_onSelectionChanged" />
                                                            </ClientEvents>
                                                        </ComponentArt:Calendar>
                                                        <asp:TextBox ID="txtTillDt" runat="server" CssClass="form-control" MaxLength="10" Width="30%" placeholder="" Style="float: left; margin-left: 20px;"></asp:TextBox>
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
                                                        <div class="clearfix"></div>
                                                    </div>

                                                    <div class="col-sm-8 marginMobileTP10 text-right">
                                                        <button type="button" class="btn btn-custon-four btn-success" id="btnOkDR" runat="server">
                                                            <i class="fa fa-check" aria-hidden="true"></i>&nbsp;OK
                                                        </button>
                                                    </div>

                                                </div>
                                               
                                            </div>
                                            <div class="sparkline10-list mt-b-10">
                                                <div class="sparkline10-graph">
                                                    <div class="static-table-list">
                                                        <div class="table-responsive">
                                                            <ComponentArt:CallBack ID="CallBackBrw" runat="server" OnCallback="CallBackBrw_Callback">
                                                                <Content>
                                                                    <ComponentArt:Grid
                                                                        ID="grdBrw"
                                                                        CssClass="Grid"
                                                                        DataAreaCssClass="GridData10"
                                                                        SearchOnKeyPress="true"
                                                                        EnableViewState="true"
                                                                        RunningMode="Client"
                                                                        ShowSearchBox="false"
                                                                        SearchBoxPosition="TopLeft"
                                                                        SearchTextCssClass="GridHeaderText"
                                                                        ShowHeader="false"
                                                                        FooterCssClass="GridFooter"
                                                                        GroupingNotificationText=""
                                                                        PageSize="10"
                                                                        PagerPosition="BottomRight"
                                                                        PagerInfoPosition="BottomLeft"
                                                                        PagerStyle="Buttons"
                                                                        PagerTextCssClass="GridFooterText"
                                                                        PagerButtonWidth="24"
                                                                        PagerButtonHeight="24"
                                                                        PagerButtonHoverEnabled="true"
                                                                        SliderHeight="26"
                                                                        SliderWidth="150"
                                                                        SliderGripWidth="9"
                                                                        SliderPopupOffsetX="80"
                                                                        SliderPopupClientTemplateId="SliderTemplate"
                                                                        SliderPopupCachedClientTemplateId="CachedSliderTemplate"
                                                                        ImagesBaseUrl="../images/"
                                                                        PagerImagesFolderUrl="../images/pager/"
                                                                        LoadingPanelFadeDuration="1000"
                                                                        LoadingPanelFadeMaximumOpacity="80"
                                                                        LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                                        LoadingPanelPosition="MiddleCenter"
                                                                        Width="100%"
                                                                        runat="server"
                                                                        HeaderCssClass="GridHeader"
                                                                        GroupingNotificationPosition="TopRight"
                                                                        SearchBoxCssClass="EditTextBoxStyle">

                                                                        <Levels>
                                                                            <ComponentArt:GridLevel
                                                                                AllowGrouping="false"
                                                                                DataKeyField="rec_id"
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
                                                                                SelectedRowCssClass="SelectedRow"
                                                                                SortAscendingImageUrl="col-asc.png"
                                                                                SortDescendingImageUrl="col-desc.png"
                                                                                SortImageWidth="10"
                                                                                SortImageHeight="19">
                                                                                <ConditionalFormats>

                                                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                                </ConditionalFormats>
                                                                                <Columns>
                                                                                    <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                    <ComponentArt:GridColumn DataField="is_error" Align="center" HeadingText=" " AllowGrouping="false" Width="30" DataCellClientTemplateId="LOGTYPE" FixedWidth="True" />
                                                                                    <ComponentArt:GridColumn DataField="log_date" Align="left" HeadingText="Log Date/Time" AllowGrouping="false" Width="120" />
                                                                                    <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" DataCellClientTemplateId="INSTITUTION" FixedWidth="True" Width="120" />
                                                                                    <ComponentArt:GridColumn DataField="service_name" Align="left" HeadingText="Service" AllowGrouping="false" DataCellClientTemplateId="SERVICE" FixedWidth="True" Width="130" />
                                                                                    <ComponentArt:GridColumn DataField="log_message" Align="left" HeadingText="Log Descripton" AllowGrouping="false" DataCellClientTemplateId="DESC" FixedWidth="True" Width="600" />
                                                                                </Columns>

                                                                            </ComponentArt:GridLevel>
                                                                        </Levels>
                                                                        <ClientEvents>
                                                                            <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                                                        </ClientEvents>

                                                                        <ClientTemplates>

                                                                            <ComponentArt:ClientTemplate ID="INSTITUTION">
                                                                                <span id="spnInst_## DataItem.GetMember('rec_id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>
                                                                            </ComponentArt:ClientTemplate>
                                                                            <ComponentArt:ClientTemplate ID="SERVICE">
                                                                                <span id="spnSvc_## DataItem.GetMember('rec_id').Value ##" title="## DataItem.GetMember('service_name').Value ##">## DataItem.GetMember('service_name').Value ##</span>
                                                                            </ComponentArt:ClientTemplate>
                                                                            <ComponentArt:ClientTemplate ID="DESC">
                                                                                <span id="spnDesc_## DataItem.GetMember('rec_id').Value ##" title="## DataItem.GetMember('log_message').Value ##">## DataItem.GetMember('log_message').Value ##</span>
                                                                            </ComponentArt:ClientTemplate>

                                                                            <ComponentArt:ClientTemplate ID="LOGTYPE">
                                                                                <img src="../images/error.png" id="imgErr_## DataItem.GetMember('rec_id').Value ##" style="display: inline;" title="error" />
                                                                                <img src="../images/info.png" id="imgInfo_## DataItem.GetMember('rec_id').Value ##" style="display: inline;" title="information" />
                                                                            </ComponentArt:ClientTemplate>

                                                                        </ClientTemplates>
                                                                    </ComponentArt:Grid>
                                                                    <span id="spnERR" runat="server"></span>
                                                                </Content>
                                                                <LoadingPanelClientTemplate>
                                                                    <table style="height: 830px; width: 100%;" border="0">
                                                                        <tr>
                                                                            <td style="text-align: center;">
                                                                                <table border="0" style="width: 70px; display: inline-block;">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img src="../images/Searching.gif" border="0" alt="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </LoadingPanelClientTemplate>
                                                                <ClientEvents>
                                                                    <CallbackComplete EventHandler="grdBrw_onCallbackComplete" />
                                                                </ClientEvents>
                                                            </ComponentArt:CallBack>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ComponentArt:PageView>
                                </ComponentArt:MultiPage>
                            </div>
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
    var objddlUserName = document.getElementById('<%=ddlUserName.ClientID %>');
    var objtxtActivity = document.getElementById('<%=txtActivity.ClientID %>');
    var objtxtFromDtUA = document.getElementById('<%=txtFromDtUA.ClientID %>');
    var objtxtTillDtUA = document.getElementById('<%=txtTillDtUA.ClientID %>');
    var objddlService = document.getElementById('<%=ddlService.ClientID %>');
    var objddlType = document.getElementById('<%=ddlType.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtTillDt = document.getElementById('<%=txtTillDt.ClientID %>');
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var strForm = "VRSDRLog";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?1"></script>
<script src="scripts/DRLog.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
