<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDCMStudyDlg.aspx.cs" Inherits="VETRIS.Study.VRSDCMStudyDlg" %>

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
    <script src="scripts/DCMStudyDlgHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-5 col-xs-12">
                            <h2>Details : Studies Received With DCM Image </h2>
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
            <div class="sparkline10-list mt-b-10" id="divStudyUID" style="display: none;">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            Study UID : 
                            <asp:Label ID="lblSUID" runat="server"></asp:Label>
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
                                                <div class="col-sm-4 col-xs-12 marginTP5">
                                                    ID (if required by practice)<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtPID" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5">
                                                    Name<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtPFName" runat="server" CssClass="form-control" MaxLength="40" Style="float: left;"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4 col-xs-12 marginMobileTP5">
                                                    <asp:TextBox ID="txtPLName" runat="server" CssClass="form-control" MaxLength="40" Style="float: left;"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

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
                                        <div class="col-sm-3 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5">
                                                    Study Date<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
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
                                                        SwapDuration="300" SwapSlide="Linear" MinDate="01/01/1900" MaxDate="01/01/2050">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="CalFrom_onSelectionChanged" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-7 col-xs-12 marginTP5">
                                                    Image Count/Transfered
                                                </div>
                                                <div class="col-sm-5 col-xs-12 marginTP5">
                                                    <asp:Label ID="lblImgCount" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-5 col-xs-12 marginMobileTP10">
                                            <div class="row">
                                                <div class="col-sm-4 col-xs-12 marginTP5">
                                                    Institution<span class="mandatory">*</span>
                                                </div>
                                                <div class="col-sm-8 col-xs-12 marginTP5">
                                                    <asp:Label ID="lblInstName" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                            </div>
                        </div>

                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 style="color: #1e77bb;">Image File(s) Received</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginTP10">


                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackFiles" runat="server" OnCallback="CallBackFiles_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdFiles"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="false"
                                                    SearchBoxPosition="TopLeft"
                                                    SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFiles.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFiles.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="file_name" Align="left" HeadingText="Image File Name" AllowGrouping="false" Width="500" />
                                                                <ComponentArt:GridColumn DataField="sent_to_pacs" Align="left" HeadingText="sent_to_pacs" AllowGrouping="false" Visible="false" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>



                                                </ComponentArt:Grid>
                                                <span id="spnErrFiles" runat="server"></span>
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
                                                <CallbackComplete EventHandler="grdFiles_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>


                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="sparklineHeader marginTP10">
                    <div class="sparkline10-hd">
                        <div class="row">
                            <div class="col-sm-5 col-xs-12"> &nbsp;
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



            <input type="hidden" id="hdnError" runat="server" value="" />
            <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
            <input type="hidden" id="hdnSUID" runat="server" value="" />
           
            <input type="hidden" id="hdnInstID" runat="server" value="00000000-0000-0000-0000-000000000000" />
            <input type="hidden" id="hdnAppv" runat="server" value="N" />

    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objhdnAppv = document.getElementById('<%=hdnAppv.ClientID %>');
    var objtxtPID = document.getElementById('<%=txtPID.ClientID %>');
    var objtxtPFName = document.getElementById('<%=txtPFName.ClientID %>');
    var objtxtPLName = document.getElementById('<%=txtPLName.ClientID %>');
    var objtxtFromDt = document.getElementById('<%=txtFromDt.ClientID %>');
    var objhdnInstID = document.getElementById('<%=hdnInstID.ClientID %>');
    var objbtnSave1 = document.getElementById('<%=btnSave1.ClientID %>');
    var objbtnSave2 = document.getElementById('<%=btnSave2.ClientID %>');
    var objbtnSubmit1 = document.getElementById('<%=btnSubmit1.ClientID %>');
    var objbtnSubmit2 = document.getElementById('<%=btnSubmit2.ClientID %>');
    var strForm = "VRSDCMStudyDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/DCMStudyDlg.js"></script>
</html>
