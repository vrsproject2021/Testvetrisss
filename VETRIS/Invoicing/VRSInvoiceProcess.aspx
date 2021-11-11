<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInvoiceProcess.aspx.cs" Inherits="VETRIS.Invoicing.VRSInvoiceProcess" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />

    <link id="lnkSTYLE" runat="server" href="../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href="../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/InvoiceProcessHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Process Invoice</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">


                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-5 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Billing Cycle<span class="mandatory">*</span></label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlBillingCycle" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-5 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-6 col-xs-12">
                                    <div class="pull-left marginTP10">
                                        <h3 class="h3Text">Process Pending</h3>
                                    </div>

                                </div>
                                <div class="col-sm-6 col-xs-12">
                                    <div class="pull-right">
                                        <button type="button" id="btnProcess" runat="server" class="btn btn-primary btn_grd" title="click to process selected billing accounts">
                                            <i class="fa fa-cogs" aria-hidden="true"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>

                            </div>
                            <div class="sparkline10-graph">
                                <div class="static-table-list">
                                    <div class="table-responsive">
                                        <div class="static-table-list">
                                            <div class="table-responsive">
                                                <ComponentArt:CallBack ID="CallBackBA" runat="server" OnCallback="CallBackBA_Callback">
                                                    <Content>
                                                        <ComponentArt:Grid
                                                            ID="grdBA"
                                                            CssClass="Grid"
                                                            AutoTheming="true"
                                                            DataAreaCssClass=""
                                                            SearchOnKeyPress="true"
                                                            EnableViewState="true"
                                                            RunningMode="Client"
                                                            ShowSearchBox="true"
                                                            SearchBoxPosition="TopLeft"
                                                            SearchTextCssClass="GridHeaderText"
                                                            PageSize="200"
                                                            ShowHeader="true"
                                                            FooterCssClass="GridFooter"
                                                            GroupingNotificationText=""
                                                            ScrollBar="Off"
                                                            ScrollTopBottomImagesEnabled="true"
                                                            ScrollTopBottomImageHeight="2"
                                                            ScrollTopBottomImageWidth="16"
                                                            ScrollImagesFolderUrl="../images/scroller/"
                                                            ScrollButtonWidth="16"
                                                            ScrollButtonHeight="17"
                                                            ShowFooter="false"
                                                            ScrollBarCssClass="ScrollBar"
                                                            ScrollGripCssClass="ScrollGrip"
                                                            ScrollBarWidth="16"
                                                            PagerTextCssClass="GridFooterText"
                                                            PagerButtonWidth="24"
                                                            PagerButtonHeight="24"
                                                            PagerButtonHoverEnabled="true"
                                                            ImagesBaseUrl="../images/"
                                                            LoadingPanelFadeDuration="1000"
                                                            LoadingPanelFadeMaximumOpacity="80"
                                                            LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                            LoadingPanelPosition="MiddleCenter"
                                                            Width="99%"
                                                            runat="server"
                                                            HeaderCssClass="GridHeader"
                                                            GroupingNotificationPosition="TopRight"
                                                            SearchBoxCssClass="EditTextBoxStyle">
                                                            <Levels>
                                                                <ComponentArt:GridLevel AllowGrouping="false"
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
                                                                    EditCellCssClass="active"
                                                                    SortedDataCellCssClass="SortedDataCell"
                                                                    SelectedRowCssClass="SelectedRow"
                                                                    SortAscendingImageUrl="col-asc.png"
                                                                    SortDescendingImageUrl="col-desc.png"
                                                                    SortImageWidth="10"
                                                                    SortImageHeight="19">
                                                                    <ConditionalFormats>
                                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBA.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                    </ConditionalFormats>
                                                                    <Columns>
                                                                        <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                        <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Billing Account" AllowGrouping="false" Width="200" DataCellClientTemplateId="BANAME" FixedWidth="true" />
                                                                        <ComponentArt:GridColumn DataField="study_count" Align="Right" HeadingText="Study Count" AllowGrouping="false" Width="75" />
                                                                        <ComponentArt:GridColumn DataField="sel" Align="left" HeadingText="sel" AllowGrouping="false" Visible="false" />
                                                                        <ComponentArt:GridColumn Align="center" HeadingText="Select" AllowGrouping="false" Width="80" DataCellClientTemplateId="ACTION" FixedWidth="true" />
                                                                    </Columns>
                                                                </ComponentArt:GridLevel>

                                                            </Levels>
                                                            <ClientEvents>
                                                                <RenderComplete EventHandler="grdBA_onRenderComplete" />
                                                            </ClientEvents>
                                                            <ClientTemplates>
                                                                <ComponentArt:ClientTemplate ID="BANAME">
                                                                    <span id="spnBA_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('name').Value ##">## DataItem.GetMember('name').Value ##</span>
                                                                </ComponentArt:ClientTemplate>
                                                                <ComponentArt:ClientTemplate ID="ACTION">
                                                                    <button type="button" id="btnProc_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to process" onclick="javascript:btnProcBA_OnClick('## DataItem.GetMember('id').Value ##');">
                                                                        <i class="fa fa-cog" aria-hidden="true"></i>
                                                                    </button>
                                                                    <button type="button" id="btnSel_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd" style="color: #000; display: inline;" title="click to select for processing" onclick="javascript:btnSel_OnClick('## DataItem.GetMember('id').Value ##');">
                                                                        <i class="fa fa-square-o" aria-hidden="true"></i>
                                                                    </button>
                                                                    <button type="button" id="btnCheck_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd" title="click to unselect for processing" style="display: none; color: #000;" onclick="javascript:btnCheck_OnClick('## DataItem.GetMember('id').Value ##');">
                                                                        <i class="fa fa-check-square-o" aria-hidden="true"></i>
                                                                    </button>
                                                                </ComponentArt:ClientTemplate>

                                                            </ClientTemplates>
                                                        </ComponentArt:Grid>
                                                        <span id="spnBAERR" runat="server"></span>
                                                    </Content>
                                                    <LoadingPanelClientTemplate>
                                                        <table style="height: 400px; width: 100%;" border="0">
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
                                                        <CallbackComplete EventHandler="grdBA_onCallbackComplete" />
                                                    </ClientEvents>
                                                </ComponentArt:CallBack>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-7 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-6 col-xs-12">
                                    <div class="pull-left marginTP10">
                                        <h3 class="h3Text">Processed/Approved</h3>
                                    </div>

                                </div>
                                <div class="col-sm-6 col-xs-12">
                                    <div class="pull-right">
                                        <button type="button" id="btnRefresh" runat="server" class="btn btn-warning btn_grd" title="click to refresh the lists">
                                            <i class="fa fa-refresh" aria-hidden="true"></i>
                                        </button>
                                        <button type="button" id="btnApprove" runat="server" class="btn btn-primary btn_grd" title="click to approve the selected record(s)">
                                            <i class="fa fa-thumbs-o-up" aria-hidden="true"></i>
                                        </button>
                                        <button type="button" id="btnFiltAppv" runat="server" class="btn btn-success btn_grd" title="click to filter the approved records">
                                            <i class="fa fa-filter" aria-hidden="true"></i>
                                        </button>
                                        <button type="button" id="btnFiltNotAppv" runat="server" class="btn btn-danger btn_grd" title="click to filter the records not approved">
                                            <i class="fa fa-filter" aria-hidden="true"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="sparkline10-graph">
                                <div class="static-table-list">
                                    <div class="table-responsive">
                                        <div class="static-table-list">
                                            <div class="table-responsive">
                                                <ComponentArt:CallBack ID="CallBackBAProc" runat="server" OnCallback="CallBackBAProc_Callback">
                                                    <Content>
                                                        <ComponentArt:Grid
                                                            ID="grdBAProc"
                                                            CssClass="Grid"
                                                            AutoTheming="true"
                                                            DataAreaCssClass=""
                                                            SearchOnKeyPress="true"
                                                            EnableViewState="true"
                                                            RunningMode="Client"
                                                            ShowSearchBox="true"
                                                            SearchBoxPosition="TopLeft"
                                                            SearchTextCssClass="GridHeaderText"
                                                            PageSize="200"
                                                            ShowHeader="true"
                                                            FooterCssClass="GridFooter"
                                                            GroupingNotificationText=""
                                                            ScrollBar="Off"
                                                            ScrollTopBottomImagesEnabled="true"
                                                            ScrollTopBottomImageHeight="2"
                                                            ScrollTopBottomImageWidth="16"
                                                            ScrollImagesFolderUrl="../images/scroller/"
                                                            ScrollButtonWidth="16"
                                                            ScrollButtonHeight="17"
                                                            ShowFooter="false"
                                                            ScrollBarCssClass="ScrollBar"
                                                            ScrollGripCssClass="ScrollGrip"
                                                            ScrollBarWidth="16"
                                                            PagerTextCssClass="GridFooterText"
                                                            PagerButtonWidth="24"
                                                            PagerButtonHeight="24"
                                                            PagerButtonHoverEnabled="true"
                                                            ImagesBaseUrl="../images/"
                                                            LoadingPanelFadeDuration="1000"
                                                            LoadingPanelFadeMaximumOpacity="80"
                                                            LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                                            LoadingPanelPosition="MiddleCenter"
                                                            Width="99%"
                                                            runat="server"
                                                            HeaderCssClass="GridHeader"
                                                            GroupingNotificationPosition="TopRight"
                                                            SearchBoxCssClass="EditTextBoxStyle">
                                                            <Levels>
                                                                <ComponentArt:GridLevel AllowGrouping="false"
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
                                                                    EditCellCssClass="active"
                                                                    SortedDataCellCssClass="SortedDataCell"
                                                                    SelectedRowCssClass="SelectedRow"
                                                                    SortAscendingImageUrl="col-asc.png"
                                                                    SortDescendingImageUrl="col-desc.png"
                                                                    SortImageWidth="10"
                                                                    SortImageHeight="19">
                                                                    <ConditionalFormats>
                                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBAProc.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                                    </ConditionalFormats>
                                                                    <Columns>
                                                                        <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                        <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Billing Account" AllowGrouping="false" Width="200" DataCellClientTemplateId="BAPROCNAME" FixedWidth="true" />
                                                                        <ComponentArt:GridColumn DataField="study_count" Align="Right" HeadingText="Study Count" AllowGrouping="false" Width="75" />
                                                                        <ComponentArt:GridColumn DataField="amount" Align="Right" HeadingText="Amount ($)" AllowGrouping="false" Width="70" FormatString="#0.00" />
                                                                        <ComponentArt:GridColumn DataField="approved" Align="left" HeadingText="approved" AllowGrouping="false" Visible="false" />
                                                                        <ComponentArt:GridColumn DataField="locked" Align="left" HeadingText="locked" AllowGrouping="false" Visible="false" />
                                                                        <ComponentArt:GridColumn DataField="locked_user" Align="left" HeadingText="locked_user" AllowGrouping="false" Visible="false" />
                                                                        <ComponentArt:GridColumn DataField="sel" Align="left" HeadingText="sel" AllowGrouping="false" Visible="false" />
                                                                        <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" Width="180" DataCellClientTemplateId="ACTIONPROC" FixedWidth="true" />

                                                                    </Columns>
                                                                </ComponentArt:GridLevel>

                                                            </Levels>
                                                            <ClientEvents>
                                                                <RenderComplete EventHandler="grdBAProc_onRenderComplete" />
                                                            </ClientEvents>
                                                            <ClientTemplates>
                                                                <ComponentArt:ClientTemplate ID="BAPROCNAME">
                                                                    <span id="spnBAProc_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('name').Value ##">## DataItem.GetMember('name').Value ##</span>
                                                                </ComponentArt:ClientTemplate>
                                                                <ComponentArt:ClientTemplate ID="ACTIONPROC">
                                                                    <button type="button" id="btnReProc_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to re process" style="display: none;" onclick="javascript:btnReProcBA_OnClick('## DataItem.GetMember('id').Value ##');">
                                                                        <i class="fa fa-cog" aria-hidden="true"></i>
                                                                    </button>
                                                                    <button type="button" id="btnNotApprove_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" title="click to approve the invoice for this account" style="display: none;">
                                                                        <i class="fa fa-thumbs-o-up" aria-hidden="true" onclick="javascript:Approve('## DataItem.GetMember('id').Value ##')"></i>
                                                                    </button>
                                                                    <button type="button" id="btnApprove_## DataItem.GetMember('id').Value ##" class="btn btn-success btn_grd" title="The invoice of this account is/are approved" style="display: none;">
                                                                        <i class="fa fa-thumbs-o-up" aria-hidden="true"></i>
                                                                    </button>
                                                                    <button type="button" id="btnLocked_## DataItem.GetMember('id').Value ##" class="btn btn-danger btn_grd" title="This record is locked by ## DataItem.GetMember('locked_user').Value ##" style="display: none;">
                                                                        <i class="fa fa-lock" aria-hidden="true"></i>
                                                                    </button>
                                                                    <button type="button" id="btnEmail_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to send email for this account" style="display: none;">
                                                                        <i class="fa fa-envelope" aria-hidden="true" onclick="javascript:btnEmail_OnClick('## DataItem.GetMember('id').Value ##','## DataItem.GetMember('name').Value ##');"></i>
                                                                    </button>
                                                                    <button type="button" id="btnPrint_## DataItem.GetMember('id').Value ##" class="btn btn-warning btn_grd" title="click to print the Invoice for this account">
                                                                        <i class="fa fa-print" aria-hidden="true" onclick="javascript:btnPrintAcct_OnClick('## DataItem.GetMember('id').Value ##');"></i>
                                                                    </button>
                                                                    <button type="button" id="btnView_## DataItem.GetMember('id').Value ##" class="btn btn-info btn_grd" title="Click to view/update details"  onclick="javascript:btnView_OnClick('## DataItem.GetMember('id').Value ##');">
                                                                        <i class="fa fa-eye" aria-hidden="true"></i>
                                                                    </button>
                                                                    <button type="button" id="btnSelProc_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd" style="color: #000; display: inline;" title="click to select for viewing details" onclick="javascript:btnSelProc_OnClick('## DataItem.GetMember('id').Value ##');">
                                                                        <i class="fa fa-square-o" aria-hidden="true"></i>
                                                                    </button>
                                                                    <button type="button" id="btnCheckProc_## DataItem.GetMember('id').Value ##" class="btn btn-light btn_grd" title="click to unselect for viewing details" style="display: none; color: #000;" onclick="javascript:btnCheckProc_OnClick('## DataItem.GetMember('id').Value ##');">
                                                                        <i class="fa fa-check-square-o" aria-hidden="true"></i>
                                                                    </button>
                                                                </ComponentArt:ClientTemplate>

                                                            </ClientTemplates>
                                                        </ComponentArt:Grid>
                                                        <span id="spnBAProcERR" runat="server"></span>
                                                    </Content>
                                                    <LoadingPanelClientTemplate>
                                                        <table style="height: 400px; width: 100%;" border="0">
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
                                                        <CallbackComplete EventHandler="grdBAProc_onCallbackComplete" />
                                                    </ClientEvents>
                                                </ComponentArt:CallBack>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>




                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp; Close</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnAID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnBCID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlBillingCycle = document.getElementById('<%=ddlBillingCycle.ClientID %>');//--
    var objbtnProcess = document.getElementById('<%=btnProcess.ClientID %>');
    var objhdnAID = document.getElementById('<%=hdnAID.ClientID %>');
    var objhdnBCID = document.getElementById('<%=hdnBCID.ClientID %>');
    var strForm = "VRSInvoiceProcess";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/InvoiceProcess.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
