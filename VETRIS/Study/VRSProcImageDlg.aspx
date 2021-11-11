<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSProcImageDlg.aspx.cs" Inherits="VETRIS.Study.VRSProcImageDlg" %>

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
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <link href="../css/grid_style.css?" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/ProcImageDlgHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <h2>Process Image(s) </h2>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            &nbsp;
                        </div>
                        <div class="col-sm-5 col-xs-12 text-center">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave1" runat="server" style="display: none;">
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
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="col-sm-4 col-xs-12 pull-left" style="margin-top: 5px;">
                                        <h3 class="h3Text">Image(s) Received</h3>
                                    </div>
                                    <div class="col-sm-8 col-xs-12 pull-left">
                                        <asp:DropDownList ID="ddlFiltInst" runat="server" CssClass="form-control custom-select-value" Width="90%" Style="float: left;">
                                        </asp:DropDownList>
                                        <button type="button" class="btn btn-custon-four btn-primary" id="btnFilter" runat="server" style="float: right;" title="click to filter">
                                            <i class="fa fa-filter edu-danger-error" aria-hidden="true"></i>
                                        </button>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP5">

                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackFiles" runat="server" OnCallback="CallBackFiles_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdFiles"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData7_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                    ShowHeader="true"
                                                    FooterCssClass="GridFooter"
                                                    GroupingNotificationText=""
                                                    PageSize="7"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFiles.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFiles.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="file_name" Align="left" HeadingText="Image File Name" AllowGrouping="false" Width="230" DataCellClientTemplateId="FILE" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="received_on" Align="left" HeadingText="Received On/At" AllowGrouping="false" Width="100" />
                                                                <ComponentArt:GridColumn DataField="sel" Align="center" HeadingText="Select" AllowGrouping="false" Width="40" DataCellClientTemplateId="SEL" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="instititution_name" Align="left" HeadingText="instititution_name" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="del" Align="center" HeadingText=" " AllowGrouping="false" Width="30" DataCellClientTemplateId="DEL" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="patient_country_id" Align="left" HeadingText="patient_country_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="patient_state_id" Align="left" HeadingText="patient_state_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="patient_city" Align="left" HeadingText="patient_city" AllowGrouping="false" Visible="false" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdFiles_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="FILE">
                                                            <a href="#" class="spanLink" id="lnk_## DataItem.GetMember('srl_no').Value ##" title="## DataItem.GetMember('file_name').Value ##" onclick="javascript:ShowImage('## DataItem.GetMember('srl_no').Value ##','## DataItem.GetMember('file_name').Value ##');">## DataItem.GetMember('file_name').Value ##</a>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="SEL">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSel_## DataItem.GetMember('srl_no').Value ##" style="width: 18px; height: 18px; margin-top: 8px;" onclick="javascript:chkSel_OnClick('## DataItem.GetMember('srl_no').Value ##','## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('patient_country_id').Value ##','## DataItem.GetMember('patient_state_id').Value ##','## DataItem.GetMember('patient_city').Value ##');" />
                                                                <label for="chkSel_## DataItem.GetMember('srl_no').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="DEL">
                                                            <button type="button" id="btnDelFile_## DataItem.GetMember('srl_no').Value ##" class="btn btn-danger btn_grd" title="click to delete the file" onclick="javascript:btnDelFile_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('file_name').Value ##')">
                                                                <i class="fa fa-trash" aria-hidden="true"></i>

                                                            </button>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>


                                                </ComponentArt:Grid>
                                                <span id="spnErrFiles" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 280px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="grdFiles_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>

                                </div>
                            </div>
                            <div class="row">&nbsp;</div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 class="h3Text">Study Details</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="row" style="margin-top: 20px;">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            IMAGE(S) RCECEIVED
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtImgCnt" runat="server" CssClass="form-control" ReadOnly="true" Style="width: 25%; text-align: center;"></asp:TextBox>
                                        </div>

                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            STUDY DATE<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtDOS" runat="server" CssClass="form-control" MaxLength="10" Width="90px" placeholder="" Style="float: left;"></asp:TextBox>
                                            <img id="imgDOS" runat="server" alt="" style="border: 0; cursor: pointer; margin-top: 5px; margin-left: 5px; float: left;" src="../images/cal.gif" align="absmiddle" />

                                            <ComponentArt:Calendar runat="server" ID="CalDOS" AllowMonthSelection="false" AllowMultipleSelection="false"
                                                AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                                ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                                DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                                NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                                PopUp="Custom" PopUpExpandControlId="imgDOS" PrevImageUrl="cal_prevMonth.gif"
                                                SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                                SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                                SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                                <ClientEvents>
                                                    <SelectionChanged EventHandler="CalDOS_onSelectionChanged" />
                                                </ClientEvents>
                                            </ComponentArt:Calendar>

                                            <asp:DropDownList ID="ddlHr" runat="server" CssClass="form-control custom-select-value" Width="25%" Style="float: left; margin-left: 10px;">
                                            </asp:DropDownList>
                                            <span style="float: left; margin-left: 10px; margin-top: 5px;">:</span>
                                            <asp:DropDownList ID="ddlMin" runat="server" CssClass="form-control custom-select-value" Width="25%" Style="float: left; margin-left: 10px;">
                                            </asp:DropDownList>

                                        </div>
                                        <%-- <div class="col-sm-5 col-xs-12 marginMobileTP5">
                                            
                                        </div>--%>
                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            MODALITY<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:DropDownList ID="ddlModality" runat="server" CssClass="form-control custom-select-value">
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="row marginTP10" style="display: none;">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            CATEGORY<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            INSTITUTION<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtInstName" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="-1" Style="display: none;"></asp:TextBox>
                                            <input type="hidden" id="hdnInstID" runat="server" value="00000000-0000-0000-0000-000000000000" />

                                            <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="form-control custom-select-value">
                                            </asp:DropDownList>
                                        </div>

                                    </div>

                                    <div class="row marginTP10">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            REFERRING PHYSICIAN<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:DropDownList ID="ddlPhys" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            PATIENT ID<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtPID" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            ACCESSION NO.
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtAccnNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">
                                        <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                            PRIORITY<span class="mandatory">*</span>
                                            <img src="../images/help.png" id="imgCheckSvc" style="height:20px;width:20px;cursor:pointer;display:none;" align="absmiddle" title="click to check the availability of the service" onclick="javascript:imgCheckSvc_OnClick();"/>
                                        </div>
                                        <div class="col-sm-7 col-xs-12 marginMobileTP5">
                                            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="row marginTP10">

                                        <div id="divCons" style="display: none;">
                                            <div class="col-sm-5 col-xs-12 marginTP5" style="font-weight: bold;">
                                                CONSULTATION REQUIRED ?
                                            </div>
                                            <div class="col-sm-5 col-xs-12 marginMobileTP5">
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
                                        <div class="col-sm-1 col-xs-12 marginMobileTP5">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12" style="font-weight: bold;">
                                            &nbsp;
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Patient Details</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>



                            <div class="row marginTP10">
                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    PATIENT FIRST/LAST NAME<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:TextBox ID="txtPFName" runat="server" CssClass="form-control" MaxLength="40" Width="49%" Style="float: left;"></asp:TextBox>
                                    <span style="float: left;">&nbsp;</span>
                                    <asp:TextBox ID="txtPLName" runat="server" CssClass="form-control" MaxLength="40" Width="49%" Style="float: left;"></asp:TextBox>
                                </div>

                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    OWNER - FIRST/LAST NAME
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:TextBox ID="txtOwnerFN" runat="server" CssClass="form-control" MaxLength="100" Width="49%" Style="float: left;"></asp:TextBox>
                                    <span style="float: left;">&nbsp;</span>
                                    <asp:TextBox ID="txtOwnerLN" runat="server" CssClass="form-control" MaxLength="100" Width="49%" Style="float: left;"></asp:TextBox>
                                </div>

                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    COUNTRY / STATE
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control custom-select-value pull-left" Width="49%">
                                    </asp:DropDownList>
                                    <span style="float: left;">&nbsp;</span>
                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control custom-select-value pull-left" Width="49%">
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    CITY
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="100" Style="float: left;"></asp:TextBox>
                                   
                                </div>

                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    DATE OF BIRTH /AGE<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:DropDownList ID="ddlDOBMonth" runat="server" CssClass="form-control custom-select-value" Width="22%" Style="float: left; padding: 2px;">
                                        <asp:ListItem Value="1" Text="January"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="February"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="March"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="April"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="June"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="July"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="August"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="September"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="December"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlDOBDay" runat="server" CssClass="form-control custom-select-value" Width="12%" Style="float: left; margin-left: 2px; padding: 2px;">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlDOBYear" runat="server" CssClass="form-control custom-select-value" Width="16%" Style="float: left; margin-left: 2px; padding: 2px;">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" MaxLength="50" ReadOnly="true" TabIndex="-1" Width="47%" Style="float: left; margin-left: 3px;"></asp:TextBox>

                                </div>

                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    SEX<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:DropDownList ID="ddlSex" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Selected="True" Value="" Text="Select One"></asp:ListItem>
                                        <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                                        <asp:ListItem Value="M" Text="Male"></asp:ListItem>
                                        <asp:ListItem Value="O" Text="Unknown"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    SPAYED/NEUTERED<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:DropDownList ID="ddlSN" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="" Text="Select One"></asp:ListItem>
                                        <asp:ListItem Value="YES" Text="YES"></asp:ListItem>
                                        <asp:ListItem Value="NO" Text="NO"></asp:ListItem>
                                        <asp:ListItem Value="UNKNOWN" Text="UNKNOWN"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row marginTP10">
                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    WEIGHT<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:TextBox ID="txtPWt" runat="server" CssClass="form-control" Width="20%" Style="text-align: right; float: left;"></asp:TextBox>
                                    <asp:DropDownList ID="ddlUOM" runat="server" CssClass="form-control custom-select-value" Width="40%" Style="float: left; margin-left: 5px;">
                                        <asp:ListItem Value="" Text="Select One"></asp:ListItem>
                                        <asp:ListItem Value="lbs" Text="Lb(s)"></asp:ListItem>
                                        <asp:ListItem Value="kgs" Text="Kg(s)"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    SPECIES<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:DropDownList ID="ddlSpecies" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row marginTP10">

                                <div class="col-sm-4 col-xs-12 marginTP5" style="font-weight: bold;">
                                    BREED<span class="mandatory">*</span>
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:DropDownList ID="ddlBreed" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="00000000-0000-0000-0000-000000000000" Text="Please select a species"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">History/Reason for study <span class="mandatory">*</span><span style="font-size: 10px;">(Max. 2000 characters)</span></h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP5">
                                    <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" MaxLength="2000" TextMode="MultiLine" Height="160px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Private Note For Radiologist <span style="font-size: 10px;">(Max. 2000 characters)</span></h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP5">
                                    <asp:TextBox ID="txtNotePhys" runat="server" CssClass="form-control" MaxLength="2000" TextMode="MultiLine" Height="160px"></asp:TextBox>
                                </div>


                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Study Types <span class="mandatory">*</span><span style="font-size: 10px;">(Max. 4 can be selected)</span></h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
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
                                                                <input type="checkbox" id="chkStudySel_## DataItem.GetMember('srl_no').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkStudySel_OnClick('## DataItem.GetMember('srl_no').Value ##');" />
                                                                <label for="chkStudySel_## DataItem.GetMember('srl_no').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrST" runat="server"></span>
                                                <span id="spnInvBy" runat="server"></span>
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
                    </div>
                    <div class="col-sm-6 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Selected Study Type(s)</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
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
                                                                <ComponentArt:GridColumn DataField="study_type_name" Align="left" HeadingText="Study Type" AllowGrouping="false" Width="250" />

                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdSelST_onRenderComplete" />
                                                    </ClientEvents>

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
                </div>


            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-8 col-xs-8" onclick="javascript:ToggleDocUpload();">
                                    <h3 class="h3Text">Upload Document(s)</h3>
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
                                <iframe id="iframeUpload" scrolling="no" style="padding: 0px; width: 100%; height: auto; background-color: transparent; border: none; min-height: 185px;"></iframe>
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
                                                        <ComponentArt:GridColumn DataField="document_link" Align="left" HeadingText="Document Link" AllowGrouping="false" DataCellClientTemplateId="FILE_LINK" FixedWidth="True" Width="150" />
                                                        <ComponentArt:GridColumn DataField="document_file_type" Align="left" HeadingText="document_file_type" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="del_doc" Align="center" AllowGrouping="false" DataCellClientTemplateId="DELDOC" HeadingText=" " FixedWidth="True" Width="30" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdDoc_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="FILE_LINK">
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

            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            &nbsp;
                        </div>
                        <div class="col-sm-5 col-xs-12 text-center">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave2" runat="server" style="display: none;">
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
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
        <input type="hidden" id="hdnSeriesUID" runat="server" value="" />
        <input type="hidden" id="hdnSeriesNo" runat="server" value="" />
        <input type="hidden" id="hdnFTPDLFLDRTMP" runat="server" value="" />
        <input type="hidden" id="hdnAppv" runat="server" value="N" />
        <input type="hidden" id="hdnFilename" runat="server" value="" />
        <input type="hidden" id="hdnFilePath" runat="server" value="" />
        <input type="hidden" id="hdnInstConsAppl" runat="server" value="N" />
        <input type="hidden" id="hdnDOBDay" runat="server" value="01" />
        <input type="hidden" id="hdnModSvcAvbl" runat="server" value="" />
        <input type="hidden" id="hdnModSvcAvblAH" runat="server" value="" />
        <input type="hidden" id="hdnModSvcAvblExInst" runat="server" value="" />
        <input type="hidden" id="hdnModSvcAvblAHExInst" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvbl" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvblAH" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvblExInst" runat="server" value="" />
        <input type="hidden" id="hdnSpcSvcAvblAHExInst" runat="server" value="" />
        <input type="hidden" id="hdnAfterHrs" runat="server" value="N" />
        <input type="hidden" id="hdnPriority" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objlblSUID = document.getElementById('<%=lblSUID.ClientID %>');
    var objhdnSeriesUID = document.getElementById('<%=hdnSeriesUID.ClientID %>'); 
    var objhdnSeriesNo = document.getElementById('<%=hdnSeriesNo.ClientID %>'); 
    var objhdnFTPDLFLDRTMP = document.getElementById('<%=hdnFTPDLFLDRTMP.ClientID %>');
    var objhdnAppv = document.getElementById('<%=hdnAppv.ClientID %>');
    var objddlFiltInst = document.getElementById('<%=ddlFiltInst.ClientID %>');
    var objbtnFilter = document.getElementById('<%=btnFilter.ClientID %>');
    var objtxtImgCnt = document.getElementById('<%=txtImgCnt.ClientID %>');
    var objtxtInstName = document.getElementById('<%=txtInstName.ClientID %>');
    var objhdnInstID = document.getElementById('<%=hdnInstID.ClientID %>');
    var objddlPriority = document.getElementById('<%=ddlPriority.ClientID %>');
    var objtxtPID = document.getElementById('<%=txtPID.ClientID %>');
    var objtxtPFName = document.getElementById('<%=txtPFName.ClientID %>');
    var objtxtPLName = document.getElementById('<%=txtPLName.ClientID %>');
    var objddlCountry= document.getElementById('<%=ddlCountry.ClientID %>');
    var objddlState= document.getElementById('<%=ddlState.ClientID %>');
    var objtxtCity= document.getElementById('<%=txtCity.ClientID %>');
    var objtxtPWt = document.getElementById('<%=txtPWt.ClientID %>');
    var objddlUOM = document.getElementById('<%=ddlUOM.ClientID %>');
    var objddlDOBMonth = document.getElementById('<%=ddlDOBMonth.ClientID %>');
    var objddlDOBDay = document.getElementById('<%=ddlDOBDay.ClientID %>');
    var objddlDOBYear = document.getElementById('<%=ddlDOBYear.ClientID %>');
    var objhdnDOBDay = document.getElementById('<%=hdnDOBDay.ClientID %>');
    var objtxtAge = document.getElementById('<%=txtAge.ClientID %>');
    var objddlSex = document.getElementById('<%=ddlSex.ClientID %>');
    var objddlSN = document.getElementById('<%=ddlSN.ClientID %>');
    var objtxtDOS = document.getElementById('<%=txtDOS.ClientID %>');
    var objddlHr = document.getElementById('<%=ddlHr.ClientID %>');
    var objddlMin = document.getElementById('<%=ddlMin.ClientID %>');
    var objddlModality = document.getElementById('<%=ddlModality.ClientID %>');
    var objddlCategory = document.getElementById('<%=ddlCategory.ClientID %>');
    var objddlInstitution= document.getElementById('<%=ddlInstitution.ClientID%>');
    var objddlPhys = document.getElementById('<%=ddlPhys.ClientID %>');
    var objtxtAccnNo = document.getElementById('<%=txtAccnNo.ClientID %>');
    var objrdoConsY = document.getElementById('<%=rdoConsY.ClientID %>');
    var objrdoConsN = document.getElementById('<%=rdoConsN.ClientID %>');
    var objddlSpecies = document.getElementById('<%=ddlSpecies.ClientID %>');
    var objddlBreed = document.getElementById('<%=ddlBreed.ClientID %>');
    var objtxtReason = document.getElementById('<%=txtReason.ClientID %>');
    var objtxtOwnerFN = document.getElementById('<%=txtOwnerFN.ClientID %>');
    var objtxtOwnerLN = document.getElementById('<%=txtOwnerLN.ClientID %>');
    var objhdnFilename = document.getElementById('<%=hdnFilename.ClientID %>');
    var objhdnFilePath = document.getElementById('<%=hdnFilePath.ClientID %>');
    var objtxtNotePhys = document.getElementById('<%=txtNotePhys.ClientID %>');
    var objhdnInstConsAppl = document.getElementById('<%=hdnInstConsAppl.ClientID %>');
    var objhdnModSvcAvbl = document.getElementById('<%=hdnModSvcAvbl.ClientID %>');
    var objhdnModSvcAvblAH = document.getElementById('<%=hdnModSvcAvblAH.ClientID %>');
    var objhdnModSvcAvblExInst = document.getElementById('<%=hdnModSvcAvblExInst.ClientID %>');
    var objhdnModSvcAvblAHExInst = document.getElementById('<%=hdnModSvcAvblAHExInst.ClientID %>');
    var objhdnSpcSvcAvbl = document.getElementById('<%=hdnSpcSvcAvbl.ClientID %>');
    var objhdnSpcSvcAvblAH = document.getElementById('<%=hdnSpcSvcAvblAH.ClientID %>');
    var objhdnSpcSvcAvblExInst = document.getElementById('<%=hdnSpcSvcAvblExInst.ClientID %>');
    var objhdnSpcSvcAvblAHExInst = document.getElementById('<%=hdnSpcSvcAvblAHExInst.ClientID %>');
    var objhdnAfterHrs = document.getElementById('<%=hdnAfterHrs.ClientID %>');
    var objhdnPriority = document.getElementById('<%=hdnPriority.ClientID %>');
    var objiframeUpload = document.getElementById('iframeUpload');
    var objbtnSave1 = document.getElementById('<%=btnSave1.ClientID %>');
    var objbtnSave2 = document.getElementById('<%=btnSave2.ClientID %>');
    var objbtnReset1 = document.getElementById('<%=btnReset1.ClientID %>');
    var objbtnReset2 = document.getElementById('<%=btnReset2.ClientID %>');
    var objbtnClose1 = document.getElementById('<%=btnClose1.ClientID %>');
    var objbtnClose2 = document.getElementById('<%=btnClose2.ClientID %>');
    var objbtnSubmit1 = document.getElementById('<%=btnSubmit1.ClientID %>');
    var objbtnSubmit2 = document.getElementById('<%=btnSubmit2.ClientID %>');
    var strForm = "VRSProcImageDlg";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/ProcImageDlg.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
