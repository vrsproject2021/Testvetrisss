<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRadAssignBrw.aspx.cs" Inherits="VETRIS.Radiologist.VRSRadAssignBrw" %>

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
    <%--<link href="../css/style.css" rel="stylesheet" />--%>

   <%-- <link href="../css/CalendarStyle.css" rel="stylesheet" />--%>
     <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
   <%-- <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/RadAssignBrwHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>List Of Study To Assign</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span  class="pull-left searchBySpan">Search By
                                </span>
                                <span id="Span1" class="pull-right">

                                    <i style="font-size: 12px;" class="fa fa-search searchBySpan" aria-hidden="true"></i>
                                </span>
                            </button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Refresh
                            </button>
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnMultipleAssign" runat="server" style="display:none;">
                                <i class="fa fa-pencil-square-o edu-warning-danger" aria-hidden="true"></i>&nbsp;Assign Multiple Studies  
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
                    <div class="col-sm-4 col-xs-12">
                        <div class="form-group">
                            <label class="control-label" for="username">Patient Name</label>
                            <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                     <div class="col-sm-2 col-xs-12">
                        <div class="form-select-list">
                            <label class="control-label">Species</label>
                            <asp:DropDownList ID="ddlSpecies" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-select-list">
                            <label class="control-label">Modality</label>
                            <asp:DropDownList ID="ddlModality" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-select-list">
                            <label class="control-label">Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-2 col-xs-12">
                        <div class="form-select-list">
                            <label class="control-label">Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-4">
                        <div class="optSwitch pull-left" style="margin-left: 10%; margin-bottom: 20px;">
                            <asp:CheckBox ID="chkRecDt" runat="server" />
                            <label for="chkRecDt" class="label-default"></label>
                        </div>
                        <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Study Date Between</div>

                        <div class="clearfix"></div>
                    </div>
                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                        Institution
                    </div>
                    <div class="col-sm-4 col-xs-12" style="margin-top: 10px;">
                        &nbsp;
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
                    <div class="col-sm-4 marginMobileTP10">
                        <div class="form-select-list">
                            &nbsp;
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
                                        PageSize="27"
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
                                                    <ComponentArt:GridColumn DataField="time_left" Align="center" HeadingText="Time Left (HH:mm)" AllowGrouping="false" Width="110" />
                                                    <ComponentArt:GridColumn DataField="status_last_updated_on" Align="left" HeadingText="Submit Date/Time" AllowGrouping="false" Width="120" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="80" />
                                                    <ComponentArt:GridColumn DataField="species_name" Align="left" HeadingText="Species" AllowGrouping="false" DataCellClientTemplateId="SPECIES" FixedWidth="True" Width="60" />
                                                    <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" DataCellClientTemplateId="CATEGORY" FixedWidth="True" Width="80" />
                                                    <ComponentArt:GridColumn DataField="priority_id" Align="left" HeadingText="Priority" AllowGrouping="false" DataCellClientTemplateId="PRIORITY" FixedWidth="True" Width="100" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Priority" AllowGrouping="false" DataCellClientTemplateId="PRIORITYDESC" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" DataCellClientTemplateId="INSTITUTION" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="physician_name" Align="left" HeadingText="Ref. Physician" AllowGrouping="false" DataCellClientTemplateId="PHYSICIAN" FixedWidth="True" Width="120" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="study_status_pacs" Align="left" HeadingText="study_status_pacs" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="status_desc" Align="left" HeadingText="Status" AllowGrouping="false" DataCellClientTemplateId="STATUS" FixedWidth="True" Width="60" />
                                                    <ComponentArt:GridColumn DataField="radiologist_id" Align="left" HeadingText="radiologist_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="assign_prelim" Align="left" HeadingText="Assigned (Prelim)" AllowGrouping="false" DataCellClientTemplateId="ASSIGNPRELIM" FixedWidth="True" Width="110" />
                                                    <ComponentArt:GridColumn DataField="final_radiologist_id" Align="left" HeadingText="radiologist_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="assign_final" Align="left" HeadingText="Assigned (Final)" AllowGrouping="false" DataCellClientTemplateId="ASSIGNFINAL" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="assign" Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="CHKASSIGN" FixedWidth="True" Width="40" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="ASSIGN" FixedWidth="True" Width="30" />
                                                    <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="UNASSIGN" FixedWidth="True" Width="30" />
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
                                            <ComponentArt:ClientTemplate ID="SPECIES">
                                                <span id="spnSpecies_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('species_name').Value ##">## DataItem.GetMember('species_name').Value ##</span>
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

                                            <ComponentArt:ClientTemplate ID="INSTITUTION">
                                                <span id="spnInst_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PHYSICIAN">
                                                <span id="spnPhys_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('physician_name').Value ##">## DataItem.GetMember('physician_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="STATUS">
                                                <span id="spnStat_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('status_desc').Value ##">## DataItem.GetMember('status_desc').Value ##</span>
                                            </ComponentArt:ClientTemplate>

                                            <ComponentArt:ClientTemplate ID="ASSIGNPRELIM">
                                                <span id="spnAssnP_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('assign_prelim').Value ##">## DataItem.GetMember('assign_prelim').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="ASSIGNFINAL">
                                                <span id="spnAssnF_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('assign_final').Value ##">## DataItem.GetMember('assign_final').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="CHKASSIGN">
                                               <div class="grid_option">
                                                    <input type="checkbox" id="chkAsn_## DataItem.GetMember('id').Value ##" title="check to select for multiple assignment" style="width: 18px; height: 18px;" onclick="javascript: ChkAssign_OnClick('## DataItem.GetMember('id').Value ##');" /> 
                                                </div>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="ASSIGN">
                                                <button type="button" id="btnAssign_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to assign radiologist" onclick="javascript:Assign('## DataItem.GetMember('id').Value ##')">
                                                    <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
                                                </button>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="UNASSIGN">
                                                <button type="button" id="btnUnassign_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" title="click to unassign radiologist" style="display: inline;" onclick="javascript:Unassign('## DataItem.GetMember('id').Value ##')">
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
        <input type="hidden" id="hdnPriority" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtPatientName = document.getElementById('<%=txtPatientName.ClientID %>');
    var objddlModality = document.getElementById('<%=ddlModality.ClientID %>');
    var objddlSpecies = document.getElementById('<%=ddlSpecies.ClientID %>');
    var objddlCategory = document.getElementById('<%=ddlCategory.ClientID %>');
    var objddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
    var objchkRecDt = document.getElementById('<%=chkRecDt.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtTillDt = document.getElementById('<%=txtTillDt.ClientID %>');
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objhdnPriority = document.getElementById('<%=hdnPriority.ClientID %>');
    var objbtnMultipleAssign = document.getElementById('<%=btnMultipleAssign.ClientID %>');
    var strForm = "VRSRadAssignBrw";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/RadAssignBrw.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
