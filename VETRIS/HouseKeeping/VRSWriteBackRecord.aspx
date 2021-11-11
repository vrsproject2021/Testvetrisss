<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSWriteBackRecord.aspx.cs" Inherits="VETRIS.HouseKeeping.VRSWriteBackRecord" %>
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
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/WriteBackRecordHdr.js?11062020"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
        <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Write Back Record</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
        <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    
                    <div class="row">
                        <div class="col-sm-12 col-xs-12" style="margin-top: 20px;">

                            <div class="col-sm-2 col-xs-12 marginTP5">
                                Received Date Between <span class="mandatory">*</span>
                            </div>
                            <div class="col-sm-2 col-xs-12 marginMobileTP5">
                                <asp:TextBox ID="txtFromDt" runat="server" CssClass="form-control" MaxLength="10" Width="110px" placeholder="" Style="float: left;"></asp:TextBox>
                                <img id="imgFrom" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 5px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />

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
                            </div>
                            <div class="col-sm-1 col-xs-12 marginTP5">
                                &nbsp;And &nbsp;
                            </div>

                            <div class="col-sm-2 col-xs-12 marginMobileTP5">
                                <asp:TextBox ID="txtTillDt" runat="server" CssClass="form-control" MaxLength="10" Width="110px" placeholder="" Style="float: left;"></asp:TextBox>
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
                            <div class="col-sm-5 col-xs-12 marginMobileTP5">
                                <button type="button" class="btn btn-custon-four btn-warning" id="btnSearch" runat="server" title="click to search">
                                    <i class="fa fa-search edu-danger-error" aria-hidden="true"></i>
                                </button>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12" style="margin-top: 20px;">
                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackBrw" runat="server" OnCallback="CallBackBrw_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdBrw"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData20"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopRight" 
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="9"
                                                PagerPosition="BottomRight"
                                                PagerInfoPosition="BottomLeft"
                                                PagerStyle="Buttons"
                                                PagerButtonWidth="24"
                                                PagerButtonHeight="24"
                                                PagerButtonHoverEnabled="true"
                                                SliderHeight="26"
                                                SliderWidth="150"
                                                SliderGripWidth="9"
                                                SliderPopupOffsetX="80"
                                                SliderPopupClientTemplateId="SliderTemplate"
                                                SliderPopupCachedClientTemplateId="CachedSliderTemplate"
                                                PagerTextCssClass="GridFooterText"
                                                ImagesBaseUrl="../images/"
                                                PagerImagesFolderUrl="../images/pager/"
                                                LoadingPanelFadeDuration="1000"
                                                LoadingPanelFadeMaximumOpacity="80"
                                                LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                LoadingPanelPosition="MiddleCenter"
                                                Width="99%"
                                                runat="server" HeaderCssClass="GridHeader"
                                                GroupingNotificationPosition="TopLeft">

                                                <Levels>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataKeyField="id"
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
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="Study UID" AllowGrouping="false" DataCellClientTemplateId="SUID" FixedWidth="True" Width="300" />
                                                            <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient" AllowGrouping="false" DataCellClientTemplateId="PATIENT" FixedWidth="True" Width="130" />
                                                            <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received Date/Time" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" DataCellClientTemplateId="INSTITUTION" FixedWidth="True" Width="130" />
                                                            <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Priority" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="status_desc" Align="left" HeadingText="Status" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="pacs_wb" Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="WRITE" FixedWidth="True" Width="40" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                              <ClientEvents>
                                                  <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                              </ClientEvents>
                                                <ClientTemplates>

                                                    <ComponentArt:ClientTemplate ID="PATIENT">
                                                        <span id="spnUCust_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('patient_name').Value ##">## DataItem.GetMember('patient_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SUID">
                                                        <span id="spnUSUID_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('study_uid').Value ##">## DataItem.GetMember('study_uid').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="INSTITUTION">
                                                        <span id="spnInst_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>

                                                    <ComponentArt:ClientTemplate ID="WRITE">
                                                        <button type="button" id="btnWB_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" style="display:none;" title="click to write back this study to PACS"  onclick="javascript:UpdateWriteBack('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('pacs_wb').Value ##');">
                                                            <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                                                             
                                                        </button>
                                                        <button type="button" id="btnRevert_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" style="display:none;" title="click to unlock this study in PACS"  onclick="javascript:UpdateWriteBack('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('pacs_wb').Value ##');">
                                                            <i class="fa fa-unlock" aria-hidden="true"></i>
                                                        </button>
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
                                                                    <img src="../images/spinner-darkgrey.gif" width="50" height="65" border="0" alt="" />
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
            </div>
    </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtTillDt = document.getElementById('<%=txtTillDt.ClientID %>');
    var strForm = "VRSWriteBackRecord";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/WriteBackRecord.js?11062020"></script>
</html>
