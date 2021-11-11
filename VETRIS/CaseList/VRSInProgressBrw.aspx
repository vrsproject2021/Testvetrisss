<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInProgressBrw.aspx.cs" Inherits="VETRIS.CaseList.VRSInProgressBrw" %>

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
    <link id="lnkSTYLE" runat="server" href="../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href="../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href="../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/InProgressBrwHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-2 col-xs-12">
                            <h2>In Progress</h2>
                        </div>
                        <div class="col-sm-10 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span class="pull-left searchBySpan">Search By
                                </span>
                                <span id="Span1" class="pull-right">

                                    <i style="font-size: 12px;" class="fa fa-search searchBySpan" aria-hidden="true"></i>
                                </span>
                            </button>
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnDV" runat="server" style="display: none;">
                                <i class="fa fa-desktop" aria-hidden="true"></i>&nbsp;Desktop Viewer(Multiple)
                            </button>
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnGetCase" runat="server" style="display: none;">
                                <i class="fa fa-object-group" aria-hidden="true"></i>&nbsp;Get Case(s)
                            </button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Refresh
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="searchSection searchPanelBg" id="searchSection">

                <div class="row">
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-group">
                            <label class="control-label" for="username">Patient Name</label>
                            <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12">
                        <div class="form-select-list">
                            <label class="control-label">Modality</label>
                            <asp:DropDownList ID="ddlModality" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" id="divCategory" style="display: none;">
                        <div class="form-select-list">
                            <label class="control-label">Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-3 col-xs-12" id="divRadiologist" style="display: none;">
                        <div class="form-select-list">
                            <label class="control-label">Radiologist Assigned</label>
                            <asp:DropDownList ID="ddlRadiologist" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-3">
                        <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                            <asp:CheckBox ID="chkRecDt" runat="server" />
                            <label for="chkRecDt" class="label-default"></label>
                        </div>
                        <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Study Date Between</div>

                        <div class="clearfix"></div>
                    </div>
                    <div class="col-sm-3 col-xs-12" style="margin-top: 10px;">
                        Institution
                    </div>
                    <div class="col-sm-3 col-xs-12" style="margin-top: 10px;">
                        Species
                    </div>
                    <div class="col-sm-3 col-xs-12" style="margin-top: 10px;">
                        Status
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtFromDt" runat="server" CssClass="form-control" MaxLength="10" Width="35%" placeholder="" Style="float: left;"></asp:TextBox>
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
                        <asp:TextBox ID="txtTillDt" runat="server" CssClass="form-control" MaxLength="10" Width="35%" placeholder="" Style="float: left; margin-left: 20px;"></asp:TextBox>
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

                    <div class="col-sm-3 marginMobileTP10">
                        <div class="form-select-list">
                            <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-3 marginMobileTP10">
                        <div class="form-select-list">
                            <asp:DropDownList ID="ddlSpecies" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-3 marginMobileTP10">
                        <div class="form-select-list">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 col-xs-12 form-group" style="margin-top: 10px;">
                        Priority
                    </div>
                    <div class="col-sm-4 col-xs-12 form-group" style="margin-top: 10px;" id="divLblStudyUID" hidden="hidden">
                        Study UID
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 marginMobileTP10">
                        <div class="form-select-list">
                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12" id="divTxtStudyUID" hidden="hidden">
                        <div class="form-group">
                            <asp:TextBox ID="txtStudyUID" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12 text-right marginTP5">
                        <button type="button" class="btn btn-custon-four btn-warning" id="btnSearch" runat="server">
                            <i class="fa fa-search edu-warning-danger" aria-hidden="true"></i>&nbsp;Search
                                           
                        </button>
                        <button type="button" class="btn btn-custon-four btn-success" id="btnRefresh" runat="server">
                            <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Refresh
                                           
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
                                        DataAreaCssClass="GridData20"
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
                                                    <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="Study UID" AllowGrouping="false" DataCellClientTemplateId="SUID" FixedWidth="True" Width="200" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient" AllowGrouping="false" DataCellClientTemplateId="PATIENT" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received On/At" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="time_left" Align="center" HeadingText="Time Left" AllowGrouping="false" Width="60" FixedWidth="True" />
                                                    <ComponentArt:GridColumn DataField="time_left_trans" Align="center" HeadingText="Trans. Time Left" AllowGrouping="false" Width="95" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="status_last_updated_on" Align="left" HeadingText="Submitted On/At" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="90" />
                                                    <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" DataCellClientTemplateId="CATEGORY" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="priority_id" Align="left" HeadingText="Change Priority" AllowGrouping="false" DataCellClientTemplateId="PRIORITY" FixedWidth="True" Width="120" />
                                                    <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Priority" AllowGrouping="false" DataCellClientTemplateId="PRIORITYDESC" FixedWidth="True" Width="80" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="species_name" Align="left" HeadingText="Species" AllowGrouping="false" DataCellClientTemplateId="SPECIES" FixedWidth="True" Width="60" />
                                                    <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" DataCellClientTemplateId="INSTITUTION" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="physician_name" Align="left" HeadingText="Ref. Physician" AllowGrouping="false" DataCellClientTemplateId="PHYSICIAN" FixedWidth="True" Width="120" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="status_desc" Align="left" HeadingText="Status" AllowGrouping="false" Width="65" />
                                                    <ComponentArt:GridColumn DataField="study_status_pacs" Align="left" HeadingText="study_status_pacs" Width="80" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="assigned_radiologist" Align="left" HeadingText="Radiologist Assn." AllowGrouping="false" DataCellClientTemplateId="RADIOLOGIST" FixedWidth="True" Width="110" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="locked_user" Align="left" HeadingText="Radiologist" AllowGrouping="false" DataCellClientTemplateId="LOCKEDUSER" FixedWidth="True" Width="130" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACLOGINURL" Align="left" HeadingText="PACLOGINURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACMAILRPTURL" Align="left" HeadingText="PACMAILRPTURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACIMGVWRURL" Align="left" HeadingText="PACIMGVWRURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="PROMO" FixedWidth="True" Width="30" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="EDRPT" FixedWidth="True" Width="105" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="locked" Align="left" HeadingText="locked" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="under_trans" Align="left" HeadingText="under_trans" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="req_trans" Align="left" HeadingText="req_trans" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="accession_no" Align="center" HeadingText="accession_no" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="patient_id" Align="center" HeadingText="patient_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="VWIMG" FixedWidth="True" Width="150" />
                                                    <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="inst_code" Align="left" HeadingText="inst_code" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="inst_name" Align="left" HeadingText="inst_code" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="phys_code" Align="left" HeadingText="phys_code" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="assigned_rad_id" Align="left" HeadingText="assigned_rad_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="show_download" Align="left" HeadingText="show_download" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="log_available" Align="left" HeadingText="log_available" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="species_id" Align="left" HeadingText="species_id" AllowGrouping="false" Visible="false" />
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
                                            <ComponentArt:ClientTemplate ID="MODALITY">
                                                <span id="spnMod_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('modality_name').Value ##">## DataItem.GetMember('modality_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="CATEGORY">
                                                <span id="spnCat_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('category_name').Value ##">## DataItem.GetMember('category_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PRIORITY">
                                                <select id="ddlPriority_## DataItem.GetMember('id').Value ##" class="form-control custom-select-value" style="width: 99%;" onchange="javascript:ddlPriority_OnChange('## DataItem.GetMember('id').Value ##');">
                                                </select>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PRIORITYDESC">
                                                <span id="spnPrior_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('priority_desc').Value ##">## DataItem.GetMember('priority_desc').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="SPECIES">
                                                <span id="spnSpecies_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('species_name').Value ##">## DataItem.GetMember('species_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="INSTITUTION">
                                                <span id="spnInst_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PHYSICIAN">
                                                <span id="spnPhys_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('physician_name').Value ##">## DataItem.GetMember('physician_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="RADIOLOGIST">
                                                <span id="spnRads_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('assigned_radiologist').Value ##">## DataItem.GetMember('assigned_radiologist').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="LOCKEDUSER">
                                                <span id="spnLU_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('locked_user').Value ##">## DataItem.GetMember('locked_user').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="STATUS">
                                                <a href="javascript:void(0);" style="text-decoration: underline;" onclick="javascript:Status_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('PACLOGINURL').Value ##')">## DataItem.GetMember('status_desc').Value ##</a>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PROMO">
                                                <button type="button" id="btnDisc_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to view/apply discount" onclick="javascript:btnDisc_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-usd" aria-hidden="true"></i>
                                                </button>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="EDRPT">

                                                <button type="button" id="btnEditRpt_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="click to read/view the report" onclick="javascript:btnEditRpt_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_status_pacs').Value ##');">
                                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnTransReq_## DataItem.GetMember('id').Value ##" class="btn btn-warning btn_grd" title="Transcription required" style="display: none;" onclick="javascript:btnEditRpt_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_status_pacs').Value ##');">
                                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnTrans_## DataItem.GetMember('id').Value ##" class="btn btn-warning btn_grd" style="display: none;" title="click to transcribe" onclick="javascript:btnTrans_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_status_pacs').Value ##');">
                                                    <i class="fa fa-headphones" aria-hidden="true"></i>
                                                </button>

                                                <button type="button" id="btnLocked_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" title="This study is locked by ## DataItem.GetMember('locked_user').Value ##" style="display: none;" onclick="javascript:btnEditRpt_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_status_pacs').Value ##');">
                                                    <i class="fa fa-lock" aria-hidden="true"></i>
                                                </button>

                                                <button type="button" id="btnImgViewer_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="Desktop Viewer" onclick="javascript:btnImgViewer_OnClick('## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-desktop" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnActivity_## DataItem.GetMember('id').Value ##" class="btn btn-gray btn_grd" title="click to view the study activities" onclick="javascript:btnActivity_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-eye" aria-hidden="true"></i>
                                                </button>

                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="VWIMG">
                                                <button type="button" id="btnRelCase_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" title="Release the case from the list" style="display: none;" onclick="javascript:btnRelCase_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-minus-circle" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnImg_## DataItem.GetMember('id').Value ##" class="btn btn-warning btn_grd" title="View image(s) in web viewer" style="display: inline;" onclick="javascript:btnImg_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('accession_no').Value ##','## DataItem.GetMember('patient_id').Value ##','## DataItem.GetMember('PACIMGVWRURL').Value ##');">
                                                    <i class="fa fa-file-image-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnDLImg_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to download the image(s)" style="display: none;" onclick="javascript:btnDLImg_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('inst_code').Value ##','## DataItem.GetMember('phys_code').Value ##');">
                                                    <i class="fa fa-download" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnDLImgDsbl_## DataItem.GetMember('id').Value ##" class="btn btn-secondary btn_grd" title="All the images are not yet received in VETRIS.Click to check the sync status" onclick="javascript:btnDLImgDsbl_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('inst_code').Value ##');" style="display: none; color: #000;">
                                                    <i class="fa fa-download" aria-hidden="true"></i>
                                                </button>
                                                <img src="../images/loader.gif" id="imgDLProc_## DataItem.GetMember('id').Value ##" style="margin-top: 2px; height: 26px; width: 26px; display: none;" />
                                                <button type="button" id="btnSel_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd" style="color: #000;" title="click to select for image viewing" onclick="javascript:btnSel_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-square-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnCheck_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd" title="click to unselect for image viewing" style="display: none; color: #000;" onclick="javascript:btnCheck_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-check-square-o" aria-hidden="true"></i>
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
        <%--<div id="divDL" style="display: none; position: fixed; bottom: 0; left: 0; top: 580px; width: 40%;height:300px; background-color:white; padding: 0;">
            
        </div>--%>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnRptID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnPriority" runat="server" value="" />
        <input type="hidden" id="hdnRadFnRights" runat="server" value="" />
        <input type="hidden" id="hdnPACSCred" runat="server" value="" />
        <input type="hidden" id="hdnAfterHrs" runat="server" value="N" />
        <input type="hidden" id="hdnPACSARCHIVEFLDR" runat="server" value="" />
        <input type="hidden" id="hdnPACSARCHALTFLDR" runat="server" value="" />
        <input type="hidden" id="hdnDCMMODIFYEXEPATH" runat="server" value="" />
        <input type="hidden" id="hdnSCHCASVCENBL" runat="server" value="" />
        <input type="hidden" id="hdnSelSUID" runat="server" value="" />
        <input type="hidden" id="hdnModSvcAvbl" runat="server" value="" />
        <input type="hidden" id="hdnModSvcAvblAH" runat="server" value="" />
        <input type="hidden" id="hdnModSvcAvblExInst" runat="server" value="" />
        <input type="hidden" id="hdnModSvcAvblAHExInst" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvbl" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvblAH" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvblExInst" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvblAHExInst" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnRptID = document.getElementById('<%=hdnRptID.ClientID %>')
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtPatientName = document.getElementById('<%=txtPatientName.ClientID %>');
    var objddlModality = document.getElementById('<%=ddlModality.ClientID %>');
    var objddlCategory = document.getElementById('<%=ddlCategory.ClientID %>');
    var objddlSpecies = document.getElementById('<%=ddlSpecies.ClientID %>');
    var objddlRadiologist = document.getElementById('<%=ddlRadiologist.ClientID %>');
    var objddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
    var objchkRecDt = document.getElementById('<%=chkRecDt.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtTillDt = document.getElementById('<%=txtTillDt.ClientID %>');
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objhdnPriority = document.getElementById('<%=hdnPriority.ClientID %>');
    var objhdnRadFnRights = document.getElementById('<%=hdnRadFnRights.ClientID %>');
    var objhdnPACSCred = document.getElementById('<%=hdnPACSCred.ClientID %>');
    var objhdnModSvcAvbl = document.getElementById('<%=hdnModSvcAvbl.ClientID %>');
    var objhdnModSvcAvblAH = document.getElementById('<%=hdnModSvcAvblAH.ClientID %>');
    var objhdnModSvcAvblExInst = document.getElementById('<%=hdnModSvcAvblExInst.ClientID %>');
    var objhdnModSvcAvblAHExInst = document.getElementById('<%=hdnModSvcAvblAHExInst.ClientID %>');
    var objhdnSpcSvcAvbl = document.getElementById('<%=hdnSpcSvcAvbl.ClientID %>');
    var objhdnSpcSvcAvblAH = document.getElementById('<%=hdnSpcSvcAvblAH.ClientID %>');
    var objhdnSpcSvcAvblExInst = document.getElementById('<%=hdnSpcSvcAvblExInst.ClientID %>');
    var objhdnSpcSvcAvblAHExInst = document.getElementById('<%=hdnSpcSvcAvblAHExInst.ClientID %>');
    var objhdnAfterHrs = document.getElementById('<%=hdnAfterHrs.ClientID %>');
    var objhdnPACSARCHIVEFLDR = document.getElementById('<%=hdnPACSARCHIVEFLDR.ClientID %>');
    var objhdnPACSARCHALTFLDR = document.getElementById('<%=hdnPACSARCHALTFLDR.ClientID %>');
    var objhdnDCMMODIFYEXEPATH = document.getElementById('<%=hdnDCMMODIFYEXEPATH.ClientID %>');
    var objhdnSCHCASVCENBL = document.getElementById('<%=hdnSCHCASVCENBL.ClientID %>');
    var objhdnSelSUID = document.getElementById('<%=hdnSelSUID.ClientID %>');
    var objbtnDV = document.getElementById('<%=btnDV.ClientID %>');
    var objbtnGetCase = document.getElementById('<%=btnGetCase.ClientID %>');
    var objtxtStudyUID = document.getElementById('<%=txtStudyUID.ClientID %>');
    var objddlPriority = document.getElementById('<%=ddlPriority.ClientID %>');

    var strForm = "VRSInProgressBrw";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/InProgressBrw.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
