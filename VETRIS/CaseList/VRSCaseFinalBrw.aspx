<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCaseFinalBrw.aspx.cs" Inherits="VETRIS.CaseList.VRSCaseFinalBrw" %>

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
    <link href="../css/style.css?02092020" rel="stylesheet" />

    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/AppPages.js?01012021"></script>
    <script src="scripts/CaseFinalBrwHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Final Report</h2>
                        </div>
                        
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span style="color: #1e77bb; margin-right: 5px;" class="pull-left">Search By
                                </span>
                                <span id="Span1" class="pull-right">

                                    <i style="color: #1e77bb; font-size: 12px;" class="fa fa-search" aria-hidden="true"></i>
                                </span>
                            </button>
                            <button type="button" class="btn btn_grd btn-success" id="btnExcel" runat="server">
                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;Generate Excel
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="searchSection searchPart" id="searchSection" style="display: none; position: fixed; z-index: 999; background: #fff;">

                <div>
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
                        <div class="col-sm-3 col-xs-12" id="divApprovedBy" style="display: none;">
                            <div class="form-select-list">
                                <label class="control-label">Approved By</label>
                                <asp:DropDownList ID="ddlApprovedBy" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label"></label>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                                <asp:CheckBox ID="chkRecDt" runat="server" />
                                <label for="chkRecDt" class="label-default"></label>
                            </div>
                            <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Received Date Between</div>

                            <div class="clearfix"></div>
                        </div>
                        <div class="col-sm-3 col-xs-12" style="margin-top: 10px;">
                            Institution
                        </div>
                        <div class="col-sm-3 col-xs-12" style="margin-top: 10px;">
                            Physician
                        </div>
                         <div class="col-sm-3 col-xs-12" style="margin-top: 10px;display:none;" id="divRAD1">
                            Radiologist
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
                                <asp:DropDownList ID="ddlPhys" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 marginMobileTP10" id="divRAD2" style="display:none;">
                            <div class="form-select-list">
                                <asp:DropDownList ID="ddlRadiologist" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="divRptReason" style="display:none;">
                        <div class="col-sm-3">
                            <div class="optSwitch pull-left" style="margin-bottom: 20px;">
                                <asp:CheckBox ID="chkShowAbRpt" runat="server" />
                                <label for="chkShowAbRpt" class="label-default"></label>
                            </div>
                            <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Show only Abnormal Report</div>

                            <div class="clearfix"></div>
                        </div>
                        <div class="col-sm-6 col-xs-12" style="margin-top: 10px;">
                            <div class="form-select-list">
                                <label class="control-label">Reason</label>
                                <asp:DropDownList ID="ddlAbRptReason" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                         <div class="col-sm-3">
                            <div class="optSwitch pull-left" style="margin-bottom: 20px;">
                                <asp:CheckBox ID="chkPendRptRel" runat="server" />
                                <label for="chkPendRptRel" class="label-default"></label>
                            </div>
                            <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Report Release Pending</div>

                            <div class="clearfix"></div>
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
                                                    <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="Study UID" AllowGrouping="false" DataCellClientTemplateId="SUID" FixedWidth="True" Width="150" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient" AllowGrouping="false" DataCellClientTemplateId="PATIENT" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received Date/Time" AllowGrouping="false" Width="120" />
                                                    <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" DataCellClientTemplateId="CATEGORY" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" DataCellClientTemplateId="INSTITUTION" FixedWidth="True" Width="110" />
                                                    <ComponentArt:GridColumn DataField="physician_name" Align="left" HeadingText="Ref. Physician" AllowGrouping="false" DataCellClientTemplateId="PHYSICIAN" FixedWidth="True" Width="100" />
                                                     <ComponentArt:GridColumn DataField="radiologist_pacs" Align="left" HeadingText="Radiologist" AllowGrouping="false" DataCellClientTemplateId="RADIOLOGIST" FixedWidth="True" Width="130" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="final_radiologist" Align="left" HeadingText="Approved By" AllowGrouping="false" DataCellClientTemplateId="FRADIOLOGIST" FixedWidth="True" Width="130" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="ACTION" FixedWidth="True" Width="250" />
                                                    <ComponentArt:GridColumn DataField="PACLOGINURL" Align="left" HeadingText="PACLOGINURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACSRPTVWRURL" Align="left" HeadingText="PACSRPTVWRURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACIMGVWRURL" Align="left" HeadingText="PACIMGVWRURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="PROMO" FixedWidth="True" Width="30" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="RADACTION" FixedWidth="True" Width="80" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="custom_report" Align="center" HeadingText="custom_report" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="accession_no" Align="center" HeadingText="accession_no" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="patient_id" Align="center" HeadingText="patient_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="priority" Align="center" HeadingText="priority" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="rating" Align="center" HeadingText="rating" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="rating_reason" Align="left" HeadingText="Report Rating" AllowGrouping="false" DataCellClientTemplateId="RATINGREASON" FixedWidth="True" Width="120" Visible="false"/>
                                                    <ComponentArt:GridColumn DataField="final_rpt_released" Align="left" HeadingText="final_rpt_released" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="final_rpt_released_by" Align="left" HeadingText="final_rpt_released_by" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="total_rows" Align="left" AllowGrouping="false" Visible="false" />
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
                                            <ComponentArt:ClientTemplate ID="OWNER">
                                                <span id="spnOwn_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('owner_name').Value ##">## DataItem.GetMember('owner_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="SPECIES">
                                                <span id="spnSpc_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('species_name').Value ##">## DataItem.GetMember('species_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="BREED">
                                                <span id="spnBr_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('breed_name').Value ##">## DataItem.GetMember('breed_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>

                                            <ComponentArt:ClientTemplate ID="INSTITUTION">
                                                <span id="spnInst_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PHYSICIAN">
                                                <span id="spnPhys_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('physician_name').Value ##">## DataItem.GetMember('physician_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="FRADIOLOGIST">
                                                <span id="spnFRad_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('final_radiologist').Value ##">## DataItem.GetMember('final_radiologist').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="RADIOLOGIST">
                                                <span id="spnRad_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('radiologist_pacs').Value ##">## DataItem.GetMember('radiologist_pacs').Value ##</span>
                                            </ComponentArt:ClientTemplate>

                                            <ComponentArt:ClientTemplate ID="ACTION">
                                                <button type="button" id="btnWL_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to view the study details" style="display: none;" onclick="javascript:btnWL_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('PACLOGINURL').Value ##');">
                                                    <i class="fa fa-list-alt" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnRpt_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="Final Report" onclick="javascript:btnRpt_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('PACSRPTVWRURL').Value ##','## DataItem.GetMember('patient_name').Value ##','## DataItem.GetMember('custom_report').Value ##');">
                                                    <i class="fa fa-file-text-o" aria-hidden="true"></i>
                                                </button>
                                                 <button type="button" id="btnImgViewer_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="Desktop Viewer" style="display:none;" onclick="javascript:btnImgViewer_OnClick('## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-desktop" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnImg_## DataItem.GetMember('id').Value ##" class="btn btn-warning btn_grd" title="Image(s)" onclick="javascript:btnImg_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('accession_no').Value ##','## DataItem.GetMember('patient_id').Value ##','## DataItem.GetMember('PACIMGVWRURL').Value ##');">
                                                    <i class="fa fa-file-image-o" aria-hidden="true"></i>
                                                </button>
                                               
                                                <button type="button" id="btnArch_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" title="Archive" onclick="javascript:btnArch_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-file-archive-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnFwd_## DataItem.GetMember('id').Value ##" class="btn btn-info btn_grd" title="Forward report & image links" style="display:none;" onclick="javascript:btnFwd_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-forward" aria-hidden="true"></i>
                                                </button>
                                                 <button type="button" id="btnRelease_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="Click to relase the report of this study" style="display:none;" onclick="javascript:btnRelease_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    &nbsp;<i class="fa fa-caret-right" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnDLImg_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to download the image(s)"  onclick="javascript:btnDLImg_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-download" aria-hidden="true"></i>
                                                </button>
                                                 <button type="button" id="btnActivity_## DataItem.GetMember('id').Value ##" class="btn btn-gray btn_grd" title="click to view the study activities"  onclick="javascript:btnActivity_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-eye" aria-hidden="true"></i>
                                                </button>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PROMO">
                                                <button type="button" id="btnDisc_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to view/apply discount" onclick="javascript:btnDisc_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-percent" aria-hidden="true"></i>
                                                </button>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="RADACTION">
                                                <button type="button" id="btnEditRpt_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="click to view/add addendum/update the report" onclick="javascript:btnEditRpt_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnCompareGreen_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="The report marked as Normal" style="display: none;" onclick="javascript:btnCompare_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-flag-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnCompareRed_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" title="The report marked as Abnormal" style="display: none;" onclick="javascript:btnCompare_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-flag-o" aria-hidden="true"></i>
                                                </button>
                                            </ComponentArt:ClientTemplate>

                                            <ComponentArt:ClientTemplate ID="RATINGREASON">
                                                <span id="spnRateReason_## DataItem.GetMember('rating_reason').Value ##" title="## DataItem.GetMember('rating_reason').Value ##">## DataItem.GetMember('rating_reason').Value ##</span>
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
                        <nav aria-label="Page navigation">
                          <ul class="pagination">
                            <li class="page-item"><a class="page-link" href="#">Previous</a></li>
                            <li class="page-item"><a class="page-link" href="#">1</a></li>
                            <li class="page-item"><a class="page-link" href="#">Next</a></li>
                          </ul>
                        </nav>
                    </div>
                </div>
            </div>
        </div>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnAPIVER" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8CLTIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVUID" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVPWD" runat="server" value="" />
        <input type="hidden" id="hdnWS8Session" runat="server" value="" />
        <input type="hidden" id="hdnRadFnRights" runat="server" value="" />
        <input type="hidden" id="hdnPACSCred" runat="server" value="" />
        <input type="hidden" id="totalRecords" runat="server" value="0" />
        <input type="hidden" id="pageSize" runat="server" value="20" />
        <input type="hidden" id="pageNo" runat="server" value="1" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtPatientName = document.getElementById('<%=txtPatientName.ClientID %>');
    var objddlModality = document.getElementById('<%=ddlModality.ClientID %>');
    var objddlCategory = document.getElementById('<%=ddlCategory.ClientID %>');
    var objchkRecDt = document.getElementById('<%=chkRecDt.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtTillDt = document.getElementById('<%=txtTillDt.ClientID %>');
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objddlPhys = document.getElementById('<%=ddlPhys.ClientID %>');
    var objddlApprovedBy = document.getElementById('<%=ddlApprovedBy.ClientID %>');
    var objddlRadiologist = document.getElementById('<%=ddlRadiologist.ClientID %>');
    var objchkShowAbRpt = document.getElementById('<%=chkShowAbRpt.ClientID %>');
    var objddlAbRptReason = document.getElementById('<%=ddlAbRptReason.ClientID %>');
    var objchkPendRptRel = document.getElementById('<%=chkPendRptRel.ClientID %>');
    var objbtnExcel = document.getElementById('<%=btnExcel.ClientID %>');
    var objhdnAPIVER = document.getElementById('<%=hdnAPIVER.ClientID %>');
    var objhdnWS8SRVIP = document.getElementById('<%=hdnWS8SRVIP.ClientID %>');
    var objhdnWS8CLTIP = document.getElementById('<%=hdnWS8CLTIP.ClientID %>');
    var objhdnWS8SRVUID = document.getElementById('<%=hdnWS8SRVUID.ClientID %>');
    var objhdnWS8SRVPWD = document.getElementById('<%=hdnWS8SRVPWD.ClientID %>');
    var objhdnWS8Session = document.getElementById('<%=hdnWS8Session.ClientID %>');
    var objhdnRadFnRights = document.getElementById('<%=hdnRadFnRights.ClientID %>');
    var objhdnPACSCred = document.getElementById('<%=hdnPACSCred.ClientID %>');
    var objhdnTotalRecords = document.getElementById('<%=totalRecords.ClientID %>');
    var objhdnPageNo = document.getElementById('<%=pageNo.ClientID %>');
    var objhdnPageSize = document.getElementById('<%=pageSize.ClientID %>');

    var strForm = "VRSCaseFinalBrw";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/CaseFinalBrw.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
