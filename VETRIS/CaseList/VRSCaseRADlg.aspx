<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCaseRADlg.aspx.cs" Inherits="VETRIS.CaseList.VRSCaseRADlg" ValidateRequest="false" %>

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
    <link href="../css/style.css?1" rel="stylesheet" />
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />

    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/CaseRADlgHdr.js?28062020"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <h2>Require Action </h2>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnDel1" runat="server">
                                <i class="fa fa-trash-o edu-danger-error" aria-hidden="true"></i>&nbsp;DELETE CASE
                            </button>
                        </div>
                        <div class="col-sm-5 col-xs-12 text-center">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave1" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save       
                            </button>

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>
                        <div class="col-sm-2 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSubmit1" runat="server">
                                <i class="fa fa-thumbs-o-up edu-danger-error" aria-hidden="true"></i>&nbsp;SUBMIT
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10"> 
                <div class="sparkline10-hd">
                    <div class="row">

                        <div class="col-sm-5 col-xs-12">
                            <div>
                                All the field(s) marked with <span class="mandatory">*</span> are mandatory
                            </div>

                        </div>
                        <div class="col-sm-7 col-xs-12 text-right">
                            <div id="divStudyUID" style="display: none;">
                                Study UID : 
                            <asp:Label ID="lblSUID" runat="server"></asp:Label>
                            </div>
                            &nbsp;
                        </div>

                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Patient Details</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12">
                                            <div class="row">
                                                <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    NAME<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtPFName" runat="server" CssClass="form-control" MaxLength="40" Width="45%" Style="float: left;"></asp:TextBox>
                                                    <span style="float: left;">&nbsp;</span>
                                                    <asp:TextBox ID="txtPLName" runat="server" CssClass="form-control" MaxLength="40" Width="45%" Style="float: left;"></asp:TextBox>
                                                </div>
                                                <%--<div class="col-sm-3 col-xs-12 marginMobileTP5">
                                                    
                                                </div>--%>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    SEX<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                    <asp:DropDownList ID="ddlSex" runat="server" CssClass="form-control custom-select-value">
                                                        <asp:ListItem Selected="True" Value="" Text="Select One"></asp:ListItem>
                                                        <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                                                        <asp:ListItem Value="M" Text="Male"></asp:ListItem>
                                                        <asp:ListItem Value="O" Text="Unknown"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    OWNER-FIRST/LAST NAME
                                                </div>
                                                <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtOwnerFN" runat="server" CssClass="form-control" MaxLength="100" Width="45%" Style="float: left;"></asp:TextBox>
                                                    <span style="float: left;">&nbsp;</span>
                                                    <asp:TextBox ID="txtOwnerLN" runat="server" CssClass="form-control" MaxLength="100" Width="45%" Style="float: left;"></asp:TextBox>
                                                </div>
                                                <%-- <div class="col-sm-3 col-xs-12 marginMobileTP5">
                                                   
                                                </div>--%>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    SPAYED/NEUTERD<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                    <asp:DropDownList ID="ddlSN" runat="server" CssClass="form-control custom-select-value">
                                                        <asp:ListItem Value="" Text="Select One"></asp:ListItem>
                                                        <asp:ListItem Value="YES" Text="YES"></asp:ListItem>
                                                        <asp:ListItem Value="NO" Text="NO"></asp:ListItem>
                                                        <asp:ListItem Value="UNKNOWN" Text="UNKNOWN"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    WEIGHT<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtPWt" runat="server" CssClass="form-control" Width="25%" Style="text-align: right; float: left;"></asp:TextBox>
                                                    <asp:DropDownList ID="ddlUOM" runat="server" CssClass="form-control custom-select-value" Width="40%" Style="float: left; margin-left: 5px;">
                                                        <asp:ListItem Value="" Text="Select One"></asp:ListItem>
                                                        <asp:ListItem Value="lbs" Text="Lb(s)"></asp:ListItem>
                                                        <asp:ListItem Value="kgs" Text="Kg(s)"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    DATE OF BIRTH/AGE<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-3 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtFromDt" runat="server" CssClass="form-control" MaxLength="10" Width="80%" placeholder="" Style="float: left;"></asp:TextBox>
                                                    <img id="imgFrom" runat="server" alt="" style="border: 0; cursor: pointer; float: left; margin-top: 5px; margin-left: 5px;" src="../images/cal.gif" align="absmiddle" />

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
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" MaxLength="50" ReadOnly="true" TabIndex="-1" Width="80%"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    SPECIES<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                    <asp:DropDownList ID="ddlSpecies" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    BREED<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                    <asp:DropDownList ID="ddlBreed" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            
                                        </div>

                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            &nbsp;
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Study Details</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-6 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    STUDY DATE
                                                </div>
                                                <div class="col-sm-6 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtStudyDt" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="-1"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-8 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    PATIENT ID (if required by practice)
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtPID" runat="server" CssClass="form-control" ReadOnly="true" MaxLength="20"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-8 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    ACCESSION # (if required by practice)
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtAccnNo" runat="server" CssClass="form-control" ReadOnly="true" MaxLength="20"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="col-sm-2 col-xs-12 marginTP10" style="font-weight: bold;">
                                            HISTORY / REASON FOR STUDY<span class="mandatory">*</span>
                                            <span style="font-size: 10px;">(Max. 2000 characters)</span>
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginTP10">
                                            <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" MaxLength="2000" TextMode="MultiLine" Height="80px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-2 col-xs-12 marginTP10" style="font-weight: bold;">
                                            NOTE FOR PHYSICIAN
                                            <span style="font-size: 10px;">(Max. 2000 characters)</span>
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginTP10">
                                            <asp:TextBox ID="txtPhysNote" runat="server" CssClass="form-control" MaxLength="2000" TextMode="MultiLine" Height="80px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    MODALITY<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-8 col-xs-12">
                                                    <asp:DropDownList ID="ddlModality" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    PRIORITY<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12" style="font-weight: bold;">
                                                    STUDY TYPE<span class="mandatory">*</span>
                                                    <span style="font-size: 10px;">(Max. 4 can be selected)</span>
                                                </div>
                                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                                    <div class="table-responsive">
                                                        <ComponentArt:CallBack ID="CallBackST" runat="server" OnCallback="CallBackST_Callback">
                                                            <Content>
                                                                <ComponentArt:Grid
                                                                    ID="grdST"
                                                                    CssClass="Grid"
                                                                    DataAreaCssClass="GridData4"
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
                                                                    Width="99%"
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
                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdST.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdST.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                            </ConditionalFormats>
                                                                            <Columns>
                                                                                <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                <ComponentArt:GridColumn DataField="study_type_id" Align="left" HeadingText="study_type_id" AllowGrouping="false" Visible="false" />
                                                                                <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" DataCellClientTemplateId="SELST" HeadingText="Select" FixedWidth="True" Width="40" />
                                                                                <ComponentArt:GridColumn DataField="study_type_name" Align="left" HeadingText="Study Type" AllowGrouping="false" Width="300" />
                                                                                <ComponentArt:GridColumn DataField="validate_study_count" Align="left" HeadingText="validate_study_count" AllowGrouping="false" Visible="false" />
                                                                                <ComponentArt:GridColumn DataField="category_id" Align="left" HeadingText="category_id" AllowGrouping="false" Visible="false" />
                                                                            </Columns>

                                                                        </ComponentArt:GridLevel>
                                                                    </Levels>

                                                                    <ClientEvents>
                                                                        <RenderComplete EventHandler="grdST_onRenderComplete" />
                                                                    </ClientEvents>
                                                                    <ClientTemplates>

                                                                        <ComponentArt:ClientTemplate ID="SELST">
                                                                            <div class="grid_option">
                                                                                <input type="checkbox" id="chkSel_## DataItem.GetMember('srl_no').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSel_OnClick('## DataItem.GetMember('srl_no').Value ##');" />
                                                                                <label for="chkSel_## DataItem.GetMember('srl_no').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                                            </div>
                                                                        </ComponentArt:ClientTemplate>
                                                                    </ClientTemplates>
                                                                </ComponentArt:Grid>
                                                                <span id="spnErrST" runat="server"></span>
                                                                <span id="spnTrackBy" runat="server"></span>

                                                            </Content>
                                                            <LoadingPanelClientTemplate>
                                                                <table style="height: 170px; width: 100%;" border="0">
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
                                                                <CallbackComplete EventHandler="grdST_onCallbackComplete" />
                                                            </ClientEvents>
                                                        </ComponentArt:CallBack>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-12 col-xs-12" style="font-weight: bold;">
                                                    SELECTED STUDY TYPE(S)
                                                </div>
                                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                                    <div class="table-responsive">
                                                        <ComponentArt:CallBack ID="CallBackSelST" runat="server" OnCallback="CallBackSelST_Callback">
                                                            <Content>
                                                                <ComponentArt:Grid
                                                                    ID="grdSelST"
                                                                    CssClass="Grid"
                                                                    DataAreaCssClass="GridData4"
                                                                    SearchOnKeyPress="true"
                                                                    EnableViewState="true"
                                                                    ShowSearchBox="false"
                                                                    SearchBoxPosition="TopRight"
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
                                                                    Width="99%"
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
                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSelST.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSelST.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                            </ConditionalFormats>
                                                                            <Columns>
                                                                                <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                <ComponentArt:GridColumn DataField="study_type_id" Align="left" HeadingText="study_type_id" AllowGrouping="false" Visible="false" />
                                                                                <ComponentArt:GridColumn DataField="study_type_name" Align="left" HeadingText="Study Type" AllowGrouping="false" Width="300" />

                                                                            </Columns>

                                                                        </ComponentArt:GridLevel>
                                                                    </Levels>


                                                                </ComponentArt:Grid>
                                                                <span id="spnErrSelST" runat="server"></span>
                                                            </Content>
                                                            <LoadingPanelClientTemplate>
                                                                <table style="height: 170px; width: 100%;" border="0">
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
                                                                <CallbackComplete EventHandler="grdSelST_onCallbackComplete" />
                                                            </ClientEvents>
                                                        </ComponentArt:CallBack>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    INSTITUTION<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                                    REFERRING PHYSICIAN<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:DropDownList ID="ddlPhys" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            <div class="row">
                                                <div id="divCons" style="display: none;">
                                                    <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                                        CONSULTATION REQUIRED ?
                                                    </div>
                                                    <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                                        <div class="pull-left grid_option customRadio">
                                                            <asp:RadioButton ID="rdoConsY" runat="server" GroupName="grpCons" />
                                                            <label for="rdoConsY" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                        <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Yes</span>
                                                        <div class="pull-left grid_option customRadio marginLFT10">
                                                            <asp:RadioButton ID="rdoConsN" runat="server" GroupName="grpCons" />
                                                            <label for="rdoConsN" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                        <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">No</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginTP10">
                                            &nbsp;
                                             <div class="row" style="display: none;">
                                                 <div class="col-sm-4 col-xs-12 marginTP5">
                                                     CATEGORY<span class="mandatory">*</span>
                                                 </div>
                                                 <div class="col-sm-8 col-xs-12">
                                                     <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                                 </div>
                                             </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-8 col-xs-8" onclick="javascript:ToggleDocUpload();">
                                    <h3 style="color: #1e77bb;">Upload Document(s)</h3>
                                </div>
                                <div class="col-sm-4 col-xs-4 text-right" onclick="javascript:ToggleDocUpload();">
                                    <a class="collapse-link" id="aRDOCCollapse">
                                        <i class="fa fa-chevron-up"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch"></div>
                        </div>
                    </div>
                    <div class="row" id="divDocUpload">
                        <div class="col-sm-6 col-xs-12">
                            <div style="border: solid 1px #bbb;">
                                <iframe id="iframeUpload" scrolling="no" style="padding: 0px; width: 100%; height: auto; background-color: #fff; border: none; min-height: 185px;"></iframe>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12 marginMobileTP5">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackDoc" runat="server" OnCallback="CallBackDoc_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdDoc"
                                            CssClass="Grid"
                                            DataAreaCssClass="GridData5"
                                            SearchOnKeyPress="true"
                                            EnableViewState="true"
                                            ShowSearchBox="true"
                                            SearchBoxPosition="TopRight"
                                            SearchTextCssClass="GridHeaderText"
                                            ShowHeader="false"
                                            FooterCssClass="GridFooter"
                                            GroupingNotificationText=""
                                            PageSize="5"
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
                                                    DataKeyField="document_srl_no"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDoc.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDoc.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="document_srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                        <ComponentArt:GridColumn DataField="document_id" Align="left" HeadingText="document_id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="document_name" Align="left" HeadingText="Document Name" AllowGrouping="false" Width="150" />
                                                        <ComponentArt:GridColumn DataField="document_link" Align="left" HeadingText="Document Link" AllowGrouping="false" DataCellClientTemplateId="FILE" FixedWidth="True" Width="150" />
                                                        <ComponentArt:GridColumn DataField="document_file_type" Align="left" HeadingText="document_file_type" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="del_doc" Align="center" AllowGrouping="false" DataCellClientTemplateId="DELDOC" HeadingText=" " FixedWidth="True" Width="30" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdDoc_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="FILE">
                                                    <a href="#" style="color: blue; text-decoration: underline" id="lnkDoc_## DataItem.GetMember('document_srl_no').Value ##" title="## DataItem.GetMember('document_link').Value ##" onclick="ShowDocument('## DataItem.GetMember('document_srl_no').Value ##','## DataItem.GetMember('document_link').Value ##')">## DataItem.GetMember('document_link').Value ##</a>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="DELDOC">
                                                    <button type="button" id="btnDelDoc_## DataItem.GetMember('document_srl_no').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteDocument('## DataItem.GetMember('document_srl_no').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnERRDoc" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdDoc_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-8 col-xs-8" onclick="javascript:ToggleDCMUpload();">
                                    <h3 style="color: #1e77bb;">Upload DICOM File(s)</h3>
                                </div>
                                <div class="col-sm-4 col-xs-4 text-right" onclick="javascript:ToggleDCMUpload();">
                                    <a class="collapse-link" id="aRDCMCollapse">
                                        <i class="fa fa-chevron-up"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch"></div>
                        </div>
                    </div>
                    <div class="row" id="divDCMUpload">
                        <div class="col-sm-6 col-xs-12">
                            <div style="border: solid 1px #bbb;">
                                <iframe id="iframeUploadDCM" scrolling="no" style="padding: 0px; width: 100%; height: auto; background-color: #fff; border: none; min-height: 185px;"></iframe>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12 marginMobileTP5">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackDCM" runat="server" OnCallback="CallBackDCM_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdDCM"
                                            CssClass="Grid"
                                            DataAreaCssClass="GridData5"
                                            SearchOnKeyPress="true"
                                            EnableViewState="true"
                                            ShowSearchBox="true"
                                            SearchBoxPosition="TopRight"
                                            SearchTextCssClass="GridHeaderText"
                                            ShowHeader="false"
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
                                            Width="99%"
                                            runat="server" HeaderCssClass="GridHeader"
                                            GroupingNotificationPosition="TopLeft">

                                            <Levels>
                                                <ComponentArt:GridLevel
                                                    AllowGrouping="false"
                                                    DataKeyField="dcm_file_srl_no"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDCM.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDCM.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="dcm_file_srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                        <ComponentArt:GridColumn DataField="dcm_file_id" Align="left" HeadingText="#" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="dcm_file_name" Align="left" HeadingText="DICOM File Name" AllowGrouping="false" Width="250" DataCellClientTemplateId="FILEDCM" FixedWidth="True" />
                                                        <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="DELDCM" HeadingText=" " FixedWidth="True" Width="30" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdDCM_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="FILEDCM">
                                                    <span id="spnDCM_## DataItem.GetMember('dcm_file_srl_no').Value ##" title="## DataItem.GetMember('dcm_file_name').Value ##">## DataItem.GetMember('dcm_file_name').Value ##</span>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="DELDCM">
                                                    <button type="button" id="btnDelDCM_## DataItem.GetMember('dcm_file_srl_no').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteDCM('## DataItem.GetMember('dcm_file_srl_no').Value ##')">
                                                        <i class="fa fa-trash" aria-hidden="true"></i>

                                                    </button>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnERRDCM" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdDCM_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-8 col-xs-8">
                                    <h3 style="color: #1e77bb;">Image/Object Count</h3>
                                </div>
                                <div class="col-sm-4 col-xs-4 text-right">
                                   &nbsp;
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight:bold;">
                            CONFIRM IMAGE/OBJECT COUNT RECEIVED<span class="mandatory">*</span>
                        </div>
                        <div class="col-sm-2 col-xs-12 marginMobileTP5">
                            <div class="pull-left grid_option customRadio">
                                <asp:RadioButton ID="rdoConfYes" runat="server" GroupName="grpConf" />
                                <label for="rdoConfYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Yes</span>
                            <div class="pull-left grid_option customRadio marginLFT10">
                                <asp:RadioButton ID="rdoConfNo" runat="server" GroupName="grpConf" />
                                <label for="rdoConfNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">No</span>
                        </div>
                        <div class="col-sm-6 col-xs-12 marginMobileTP5" >
                            <asp:TextBox ID="txtImgCnt" runat="server" CssClass="form-control" Width="15%" ReadOnly="true" TabIndex="-1" Style="float: left; display: none;text-align:center;"></asp:TextBox>
                            <asp:TextBox ID="txtObjCnt" runat="server" CssClass="form-control" Width="15%" ReadOnly="true" TabIndex="-1" Style="float: left; display: none;text-align:center;"></asp:TextBox>
                            <button type="button" class="btn btn-custon-four btn-success" title="click to refresh the count" id="btnRefreshCount" runat="server" style="cursor: pointer; float: left; margin-left: 2px;display:none;">
                                <i class="fa fa-refresh" aria-hidden="true"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnDel2" runat="server">
                                <i class="fa fa-trash-o edu-danger-error" aria-hidden="true"></i>&nbsp;DELETE CASE   
                            </button>
                        </div>
                        <div class="col-sm-5 col-xs-12 text-center">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save   
                            </button>

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close     
                            </button>

                        </div>
                        <div class="col-sm-2 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSubmit2" runat="server">
                                <i class="fa fa-thumbs-o-up edu-danger-error" aria-hidden="true"></i>&nbsp;SUBMIT
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <asp:Label ID="lblPatientID" runat="server" Style="display: none;"></asp:Label>
        <asp:Label ID="lblAccnNo" runat="server" Style="display: none;"></asp:Label>



        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
        <input type="hidden" id="hdnFilename" runat="server" value="" />
        <input type="hidden" id="hdnFilePath" runat="server" value="" />
        <input type="hidden" id="hdnPACSURL" runat="server" value="" />
        <input type="hidden" id="hdnImgVwrURL" runat="server" value="" />
        <input type="hidden" id="hdnImgCntURL" runat="server" value="" />
        <input type="hidden" id="hdnStudyDelUrl" runat="server" value="" />
        <input type="hidden" id="hdnTrackBy" runat="server" value="I" />
        <input type="hidden" id="hdnRecByRouter" runat="server" value="N" />
        <input type="hidden" id="hdnInstConsAppl" runat="server" value="N" />
        <input type="hidden" id="hdnWS8SRVIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8CLTIP" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVUID" runat="server" value="" />
        <input type="hidden" id="hdnWS8SRVPWD" runat="server" value="" />
        <input type="hidden" id="hdnAPIVER" runat="server" value="" />
        <input type="hidden" id="hdnFTPABSPATH" runat="server" value="" />
        <input type="hidden" id="hdnTempDCMFolder" runat="server" value="" />
        <input type="hidden" id="hdnDCMMODIFYEXEPATH" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objtxtPID = document.getElementById('<%=txtPID.ClientID %>');
    var objtxtPFName = document.getElementById('<%=txtPFName.ClientID %>');
    var objtxtPLName = document.getElementById('<%=txtPLName.ClientID %>');
    var objtxtPWt = document.getElementById('<%=txtPWt.ClientID %>');
    var objddlUOM = document.getElementById('<%=ddlUOM.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objtxtAge = document.getElementById('<%=txtAge.ClientID %>');
    var objddlSex = document.getElementById('<%=ddlSex.ClientID %>');
    var objddlSN = document.getElementById('<%=ddlSN.ClientID %>');
    var objddlSpecies = document.getElementById('<%=ddlSpecies.ClientID %>');
    var objddlBreed = document.getElementById('<%=ddlBreed.ClientID %>');
    var objtxtOwnerFN = document.getElementById('<%=txtOwnerFN.ClientID %>');
    var objtxtOwnerLN = document.getElementById('<%=txtOwnerLN.ClientID %>');
    var objtxtStudyDt = document.getElementById('<%=txtStudyDt.ClientID %>'); 
    var objrdoConsY = document.getElementById('<%=rdoConsY.ClientID %>');
    var objrdoConsN = document.getElementById('<%=rdoConsN.ClientID %>');
    var objtxtAccnNo = document.getElementById('<%=txtAccnNo.ClientID %>');
    var objddlPriority = document.getElementById('<%=ddlPriority.ClientID %>');
    var objtxtReason = document.getElementById('<%=txtReason.ClientID %>');
    var objddlModality = document.getElementById('<%=ddlModality.ClientID %>');
    var objddlCategory = document.getElementById('<%=ddlCategory.ClientID %>');
    var objtxtImgCnt = document.getElementById('<%=txtImgCnt.ClientID %>');
    var objtxtObjCnt = document.getElementById('<%=txtObjCnt.ClientID %>');
    var objrdoConfYes = document.getElementById('<%=rdoConfYes.ClientID %>');
    var objrdoConfNo = document.getElementById('<%=rdoConfNo.ClientID %>');
    var objhdnTrackBy = document.getElementById('<%=hdnTrackBy.ClientID %>');
    var objddlInstitution = document.getElementById('<%=ddlInstitution.ClientID %>');
    var objddlPhys = document.getElementById('<%=ddlPhys.ClientID %>');
    var objhdnFilename = document.getElementById('<%=hdnFilename.ClientID %>');
    var objhdnFilePath = document.getElementById('<%=hdnFilePath.ClientID %>');
    var objlblPatientID = document.getElementById('<%=lblPatientID.ClientID %>');
    var objlblAccnNo = document.getElementById('<%=lblAccnNo.ClientID %>');
    var objhdnPACSURL = document.getElementById('<%=hdnPACSURL.ClientID %>');
    var objhdnImgVwrURL = document.getElementById('<%=hdnImgVwrURL.ClientID %>');
    var objhdnImgCntURL = document.getElementById('<%=hdnImgCntURL.ClientID %>');
    var objhdnStudyDelUrl =  document.getElementById('<%=hdnStudyDelUrl.ClientID %>');
    var objiframeUploadDCM = document.getElementById('iframeUploadDCM');
    var objiframeUpload = document.getElementById('iframeUpload');
    var objhdnRecByRouter =document.getElementById('<%=hdnRecByRouter.ClientID %>');
    var objtxtPhysNote = document.getElementById('<%=txtPhysNote.ClientID %>');
    var objhdnInstConsAppl = document.getElementById('<%=hdnInstConsAppl.ClientID %>');
    var objhdnWS8SRVIP= document.getElementById('<%=hdnWS8SRVIP.ClientID %>');
    var objhdnWS8CLTIP= document.getElementById('<%=hdnWS8CLTIP.ClientID %>');
    var objhdnWS8SRVUID= document.getElementById('<%=hdnWS8SRVUID.ClientID %>');
    var objhdnWS8SRVPWD= document.getElementById('<%=hdnWS8SRVPWD.ClientID %>');
    var objhdnAPIVER = document.getElementById('<%=hdnAPIVER.ClientID %>');
    var objhdnFTPABSPATH= document.getElementById('<%=hdnFTPABSPATH.ClientID %>');
    var objhdnTempDCMFolder = document.getElementById('<%=hdnTempDCMFolder.ClientID %>');
    var objhdnDCMMODIFYEXEPATH=document.getElementById('<%=hdnDCMMODIFYEXEPATH.ClientID %>');
    var strForm = "VRSCaseRADlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/CaseRADlg.js?06072020"></script>

</html>
