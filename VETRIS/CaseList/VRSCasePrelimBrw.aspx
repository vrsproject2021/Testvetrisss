<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCasePrelimBrw.aspx.cs" Inherits="VETRIS.CaseList.VRSCasePrelimBrw" %>

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
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/CasePrelimBrwHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-2 col-xs-12">
                            <h2>Preliminary Report</h2>
                        </div>
                        <div class="col-sm-10 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span class="pull-left searchBySpan">Search By
                                </span>
                                <span id="Span1" class="pull-right">

                                    <i style="font-size: 12px;" class="fa fa-search searchBySpan" aria-hidden="true"></i>
                                </span>
                            </button>
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnDV" runat="server">
                                <i class="fa fa-desktop" aria-hidden="true"></i>&nbsp;Desktop Viewer(Multiple)
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="searchSection searchPanelBg" id="searchSection" >

                <div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="username">Patient Name</label>
                                <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Modality</label>
                                <asp:DropDownList ID="ddlModality" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list" id="divCategory" style="display: none;">
                                <label class="control-label">Category</label>
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
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
                        <div class="col-sm-4">
                            <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                                <asp:CheckBox ID="chkRecDt" runat="server" />
                                <label for="chkRecDt" class="label-default"></label>
                            </div>
                            <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Received Date Between</div>

                            <div class="clearfix"></div>
                        </div>
                        <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                            Institution
                        </div>
                        <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                            <div id="divPhys1" style="display: none;">
                                Physician
                            </div>
                            <div id="divRad1" style="display: none;">
                                <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                                    <asp:RadioButton ID="rdoRadPrelim" runat="server" GroupName="grdRadType" Checked="true" />
                                    <label for="rdoRadPrelim" class="label-default"></label>
                                </div>
                                <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Preliminary</div>
                                <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                                    <asp:RadioButton ID="rdoRadFinal" runat="server" GroupName="grdRadType" />
                                    <label for="rdoRadFinal" class="label-default"></label>
                                </div>
                                <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Final Radiologist</div>
                            </div>
                            
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

                        <div class="col-sm-4 marginMobileTP10">
                            <div class="form-select-list">
                                <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 marginMobileTP10" id="divPhys2" style="display: none;">
                            <div class="form-select-list">
                                <asp:DropDownList ID="ddlPhys" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 marginMobileTP10" id="divRad2" style="display: none;">
                            <div class="form-select-list">
                                <asp:DropDownList ID="ddlRadiologist" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 marginMobileTP10" id="divTxtStudyUID" style="display: none;">
                            <div id="divLblStudyUID" class="pull-left" style="margin-top: 10px; padding-bottom: 5px;">Study UID</div>
                            <div class="form-select-list">
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
                                                    <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="Study UID" AllowGrouping="false" DataCellClientTemplateId="SUID" FixedWidth="True" Width="150" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient" AllowGrouping="false" DataCellClientTemplateId="PATIENT" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received Date/Time" AllowGrouping="false" Width="120" />
                                                    <ComponentArt:GridColumn DataField="time_left" Align="center" HeadingText="Time Left (HH:mm)" AllowGrouping="false" Width="110" />
                                                    <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" DataCellClientTemplateId="CATEGORY" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" DataCellClientTemplateId="INSTITUTION" FixedWidth="True" Width="110" />
                                                    <ComponentArt:GridColumn DataField="physician_name" Align="left" HeadingText="Ref. Physician" AllowGrouping="false" DataCellClientTemplateId="PHYSICIAN" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="prelim_radiologist" Align="left" HeadingText="Radiologist-Prelim" AllowGrouping="false" DataCellClientTemplateId="ASSIGNPRELIM" FixedWidth="True" Width="110" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="final_radiologist" Align="left" HeadingText="Radiologist-Final" AllowGrouping="false" DataCellClientTemplateId="ASSIGNFINAL" FixedWidth="True" Width="100" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="ACTION" FixedWidth="True" Width="250" />
                                                    <ComponentArt:GridColumn DataField="PACLOGINURL" Align="left" HeadingText="PACLOGINURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACSRPTVWRURL" Align="left" HeadingText="PACSRPTVWRURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACIMGVWRURL" Align="left" HeadingText="PACIMGVWRURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="PROMO" FixedWidth="True" Width="30" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="custom_report" Align="center" HeadingText=" " AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="accession_no" Align="center" HeadingText="accession_no" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="patient_id" Align="center" HeadingText="patient_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="rpt_fmt" Align="center" HeadingText="rpt_fmt" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="EDRPT" FixedWidth="True" Width="30" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="inst_code" Align="left" HeadingText="inst_code" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="inst_name" Align="left" HeadingText="inst_code" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="phys_code" Align="left" HeadingText="phys_code" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="show_download" Align="left" HeadingText="show_download" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="log_available" Align="left" HeadingText="log_available" AllowGrouping="false" Visible="false" />
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
                                            <ComponentArt:ClientTemplate ID="SECTION">
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
                                            <ComponentArt:ClientTemplate ID="ASSIGNPRELIM">
                                                <span id="spnAssnP_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('prelim_radiologist').Value ##">## DataItem.GetMember('prelim_radiologist').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="ASSIGNFINAL">
                                                <span id="spnAssnF_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('final_radiologist').Value ##">## DataItem.GetMember('final_radiologist').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="ACTION">
                                                <button type="button" id="btnWL_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to view the study details" style="display: none;" onclick="javascript:btnWL_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('PACLOGINURL').Value ##');">
                                                    <i class="fa fa-list-alt" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnRpt_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="Preliminary Report" onclick="javascript:btnRpt_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('PACSRPTVWRURL').Value ##','## DataItem.GetMember('patient_name').Value ##','## DataItem.GetMember('custom_report').Value ##','## DataItem.GetMember('rpt_fmt').Value ##');">
                                                    <i class="fa fa-file-text-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnImgViewer_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="Desktop Viewer" style="display: none;" onclick="javascript:btnImgViewer_OnClick('## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-desktop" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnImg_## DataItem.GetMember('id').Value ##" class="btn btn-warning btn_grd" title="Image(s)" onclick="javascript:btnImg_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('accession_no').Value ##','## DataItem.GetMember('patient_id').Value ##','## DataItem.GetMember('PACIMGVWRURL').Value ##');">
                                                    <i class="fa fa-file-image-o" aria-hidden="true"></i>
                                                </button>

                                                <button type="button" id="btnFwd_## DataItem.GetMember('id').Value ##" class="btn btn-info btn_grd" title="Forward report & image links" onclick="javascript:btnFwd_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('rpt_fmt').Value ##');">
                                                    <i class="fa fa-forward" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnDLImg_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to download the image(s)" style="display: none;" onclick="javascript:btnDLImg_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('inst_code').Value ##','## DataItem.GetMember('phys_code').Value ##';">
                                                    <i class="fa fa-download" aria-hidden="true"></i>
                                                </button>
                                                 <button type="button" id="btnDLImgDsbl_## DataItem.GetMember('id').Value ##" class="btn btn-secondary btn_grd" title="All the images are not yet received in VETRIS.Click to check the sync status" onclick="javascript:btnDLImgDsbl_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##','## DataItem.GetMember('inst_code').Value ##');" style="display: none;color:#000;">
                                                    <i class="fa fa-download" aria-hidden="true"></i>
                                                </button>
                                                <img src="../images/loader.gif" id="imgDLProc_## DataItem.GetMember('id').Value ##" style="margin-top: 2px; height: 26px; width: 26px; display: none;" />
                                                <button type="button" id="btnActivity_## DataItem.GetMember('id').Value ##" class="btn btn-gray btn_grd" title="click to view the study activities" onclick="javascript:btnActivity_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-eye" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnSel_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd" style="color:#000;" title="click to select for image viewing" onclick="javascript:btnSel_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-square-o" aria-hidden="true"></i>
                                                </button>
                                                <button type="button" id="btnCheck_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd"  title="click to unselect for image viewing" style="display: none;color:#000;" onclick="javascript:btnCheck_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('study_uid').Value ##');">
                                                    <i class="fa fa-check-square-o" aria-hidden="true"></i>
                                                </button>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PROMO">
                                                <button type="button" id="btnDisc_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to view/apply discount" onclick="javascript:btnDisc_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-usd" aria-hidden="true"></i>
                                                </button>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="EDRPT">
                                                <button type="button" id="btnEditRpt_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="click to read/view the report" onclick="javascript:btnEditRpt_OnClick('## DataItem.GetMember('id').Value ##');">
                                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
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
        <input type="hidden" id="hdnPACSARCHIVEFLDR" runat="server" value="" />
        <input type="hidden" id="hdnPACSARCHALTFLDR" runat="server" value="" />
        <input type="hidden" id="hdnDCMMODIFYEXEPATH" runat="server" value="" />
        <input type="hidden" id="hdnSelSUID" runat="server" value="" />
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
    var objddlRadiologist = document.getElementById('<%=ddlRadiologist.ClientID %>');
    var objrdoRadPrelim = document.getElementById('<%=rdoRadPrelim.ClientID %>');
    var objrdoRadFinal = document.getElementById('<%=rdoRadFinal.ClientID %>');
    var objhdnAPIVER = document.getElementById('<%=hdnAPIVER.ClientID %>');
    var objhdnWS8SRVIP = document.getElementById('<%=hdnWS8SRVIP.ClientID %>');
    var objhdnWS8CLTIP = document.getElementById('<%=hdnWS8CLTIP.ClientID %>');
    var objhdnWS8SRVUID = document.getElementById('<%=hdnWS8SRVUID.ClientID %>');
    var objhdnWS8SRVPWD = document.getElementById('<%=hdnWS8SRVPWD.ClientID %>');
    var objhdnWS8Session = document.getElementById('<%=hdnWS8Session.ClientID %>');
    var objhdnRadFnRights = document.getElementById('<%=hdnRadFnRights.ClientID %>');
    var objhdnPACSCred = document.getElementById('<%=hdnPACSCred.ClientID %>');
    var objhdnPACSARCHIVEFLDR = document.getElementById('<%=hdnPACSARCHIVEFLDR.ClientID %>');
    var objhdnPACSARCHALTFLDR = document.getElementById('<%=hdnPACSARCHALTFLDR.ClientID %>');
    var objhdnDCMMODIFYEXEPATH = document.getElementById('<%=hdnDCMMODIFYEXEPATH.ClientID %>');
    var objhdnSelSUID = document.getElementById('<%=hdnSelSUID.ClientID %>');
    var objtxtStudyUID = document.getElementById('<%=txtStudyUID.ClientID %>');
    var strForm = "VRSCasePrelimBrw";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/CasePrelimBrw.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
