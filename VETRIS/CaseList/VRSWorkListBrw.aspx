<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSWorkListBrw.aspx.cs" Inherits="VETRIS.CaseList.VRSWorkListBrw" %>
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
    <link href="../css/style.css" rel="stylesheet" />

    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/WorkListBrwHdr.js?12022020"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <h2>My Worklist</h2>
                        </div>
                        <div class="col-sm-8 col-xs-12 text-right">
                            <asp:DropDownList ID="ddlFIlter" runat="server" Width="20%" Height="30px">
                                <asp:ListItem Value="" Text="Filter By..." Selected="True"></asp:ListItem>
                                <asp:ListItem Value="P" Text="Pending Preliminary"></asp:ListItem>
                                <asp:ListItem Value="F" Text="Pending Final"></asp:ListItem>
                            </asp:DropDownList>
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span style="color: #1e77bb; margin-right: 5px;" class="pull-left">Search By
                                </span>
                                <span id="Span1" class="pull-right">

                                    <i style="color: #1e77bb; font-size: 12px;" class="fa fa-search" aria-hidden="true"></i>
                                </span>
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

            <div class="searchSection searchPart" id="searchSection" style="display: none; position: fixed; z-index: 999; background: #fff;">

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
                                <div class="form-select-list">
                                    <label class="control-label">Category</label>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label class="control-label" for="username">Owner Name</label>
                                    <asp:TextBox ID="txtOwner" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Species</label>
                                    <asp:DropDownList ID="ddlSpecies" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Breed</label>
                                    <asp:DropDownList ID="ddlBreed" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Institution</label>
                                    
                                </div>
                            </div>
                           <div class="col-sm-4 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Status</label>
                                   
                                </div>
                            </div>
                        </div>--%>
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
                               Status
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
                                     <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
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
                                                    <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="Study UID" AllowGrouping="false" DataCellClientTemplateId="SUID" FixedWidth="True" Width="200" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient" AllowGrouping="false" DataCellClientTemplateId="PATIENT" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received Date/Time" AllowGrouping="false" Width="120" />
                                                    <ComponentArt:GridColumn DataField="time_left" Align="center" HeadingText="Time Left (HH:mm)" AllowGrouping="false" Width="110" />
                                                    <ComponentArt:GridColumn DataField="status_last_updated_on" Align="left" HeadingText="Submit Date/Time" AllowGrouping="false" Width="120" />
                                                    <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" DataCellClientTemplateId="CATEGORY" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="priority_id" Align="left" HeadingText="Priority" AllowGrouping="false"  DataCellClientTemplateId="PRIORITY" FixedWidth="True" Width="120" Visible="false"/>
                                                    <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Change Priority" AllowGrouping="false" Width="120"/>
                                                    <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" DataCellClientTemplateId="INSTITUTION" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="physician_name" Align="left" HeadingText="Ref. Physician" AllowGrouping="false" DataCellClientTemplateId="PHYSICIAN" FixedWidth="True" Width="120" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="status_desc" Align="center" HeadingText="Status" AllowGrouping="false" Width="80" />
                                                    <ComponentArt:GridColumn DataField="PACLOGINURL" Align="left" HeadingText="PACLOGINURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACMAILRPTURL" Align="left" HeadingText="PACMAILRPTURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="PACIMGVWRURL" Align="left" HeadingText="PACIMGVWRURL" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="edit_report" Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="EDRPT" FixedWidth="True" Width="30"/>
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
                                            
                                            <ComponentArt:ClientTemplate ID="INSTITUTION">
                                                <span id="spnInst_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="PHYSICIAN">
                                                <span id="spnPhys_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('physician_name').Value ##">## DataItem.GetMember('physician_name').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="RADIOLOGIST">
                                                <span id="spnRads_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('radiologist').Value ##">## DataItem.GetMember('radiologist').Value ##</span>
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="STATUS">
                                                <a href="javascript:void(0);" style="text-decoration: underline;" onclick="javascript:Status_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('PACLOGINURL').Value ##')">## DataItem.GetMember('status_desc').Value ##</a>
                                            </ComponentArt:ClientTemplate>
                                            
                                            <ComponentArt:ClientTemplate ID="EDRPT">
                                                 <button type="button" id="btnEditRpt_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="click to edit/write the report"  onclick="javascript:btnEditRpt_OnClick('## DataItem.GetMember('id').Value ##');">
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
    var objddlCategory = document.getElementById('<%=ddlCategory.ClientID %>');
    var objddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
    var objchkRecDt = document.getElementById('<%=chkRecDt.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtTillDt = document.getElementById('<%=txtTillDt.ClientID %>');
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objhdnPriority = document.getElementById('<%=hdnPriority.ClientID %>');
    var strForm = "VRSWorkListBrw";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?2"></script>
<script src="scripts/WorkListBrw.js?01042020"></script>
</html>
