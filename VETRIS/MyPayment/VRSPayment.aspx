<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSPayment.aspx.cs" Inherits="VETRIS.MyPayment.VRSPayment" %>
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
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="vendor/select2/select2.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/3.3.4/jquery.inputmask.bundle.js"></script>
    <script src="js/global.js"></script>
    <script src="scripts/PaymentHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">

                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>My Transactions</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>


            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-2 col-xs-12">
                                <h3 class="h3Text">Invoices</h3>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="pull-left grid_option1 customRadio">
                                    <asp:RadioButton ID="rdoOutstanding" runat="server" GroupName="grpPref" Checked="true" OnCheckedChanged="rdoOutstanding_CheckedChanged" AutoPostBack="true" />
                                    <label for="rdoOutstanding" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                </div>
                                <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Outstanding</span>
                                <div class="pull-left grid_option1 customRadio">
                                    <asp:RadioButton ID="rdoAll" runat="server" GroupName="grpPref" OnCheckedChanged="rdoAll_CheckedChanged" AutoPostBack="true" />
                                    <label for="rdoAll" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                </div>
                                <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">All Invoices</span>
                            </div>
                            <div class="col-sm-6 col-xs-12 text-right">
                                <button type="button" id="btnClearSelection" runat="server" class="btn btn-custon-four btn-danger">
                                    <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Clear selection</button>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div id="outDiv" runat="server">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackInvoiceBrw" runat="server" OnCallback="CallBackInvoiceBrw_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdInvoiceBrw"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData7_1"
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
                                                    <ComponentArt:GridLevel AllowGrouping="true"
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

                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInvoiceBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="invoice_no" Align="left" HeadingText="Invoice #" AllowGrouping="false" DataCellClientTemplateId="INVOS" FixedWidth="True" Width="100" />
                                                            <ComponentArt:GridColumn DataField="invoice_date" Align="left" HeadingText="Date" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_id" Align="left" HeadingText="billing_cycle_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_name" Align="left" HeadingText="Billing Cycle" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Invoice Amount($)" AllowGrouping="false" Width="110" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="adjusted" Align="right" HeadingText="Payment Applied($)" AllowGrouping="false" Width="120" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="refunded" Align="right" HeadingText="Refunded($)" AllowGrouping="false" Width="120" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="balance" Align="right" HeadingText="Balance Due($)" AllowGrouping="false" Width="100" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="selected" Align="center" HeadingText="Select" AllowGrouping="false" DataCellClientTemplateId="SELECT" FixedWidth="True" Width="50" />
                                                            <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="PAYACTION" HeadingText="" FixedWidth="True" Width="100" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdInvoiceBrw_onRenderComplete" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="INVOS">

                                                        <a href="javascript:void(0);" id="lnkINVOS_## DataItem.GetMember('id').Value ##" title="click to view this invoice" class="spanLink" style="cursor: pointer; text-decoration: none;" class="spanLink" onclick="javascript:ShowInvoice('## DataItem.GetMember('billing_cycle_id').Value ##');">## DataItem.GetMember('invoice_no').Value ##</a>

                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SELECT">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSel_## DataItem.GetMember('id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: ChkSelect_OnClick(this, '## DataItem.GetMember('id').Value ##', grdInvoiceBrw.get_table().getRow(## DataItem.get_index() ##));" />
                                                        </div>
                                                    </ComponentArt:ClientTemplate>

                                                    <ComponentArt:ClientTemplate ID="PAYACTION">
                                                        <button type="button" id="btnPay_## DataItem.GetMember('id').Value ##" class="btn btn-grid btn-success" onclick="javascript:btnPayRow_Click(this, '## DataItem.GetMember('id').Value ##', grdInvoiceBrw.get_table().getRow(## DataItem.get_index() ##))">
                                                            <i class="fa fa-credit-card"></i>&nbsp;Pay Now

                                                        </button>
                                                    </ComponentArt:ClientTemplate>

                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnERR" runat="server"></span>
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
                                            <CallbackComplete EventHandler="grdInvoiceBrw_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-sm-12 col-xs-12">

                                <%-- <div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="borderSearch pull-left"></div>
                                        <div class="col-sm-12 col-xs-12">
                                            <h3 style="color: #1e77bb;">Payment</h3>
                                        </div>
                                        <div class="borderSearch pull-left"></div>
                                    </div>
                                </div>--%>

                                <div class="row marginTP5">
                                    <div class="col-sm-2 col-xs-12 marginTP5">
                                        Payment Amount ($)
                                    </div>
                                    <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                        <asp:TextBox ID="txtSelectedAmount" runat="server" CssClass="form-control" Style="text-align: right; float: left;" Width="15%"></asp:TextBox>
                                        <button type="button" id="btnPay" runat="server" class="btn btn-custon-four btn-success" style="margin-left: 5px;">
                                            <i class="fa fa-credit-card"></i>&nbsp;Pay Now

                                        </button>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>

                    <div id="invAll" runat="server">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackAllInvoiceBrw" runat="server" OnCallback="CallBackAllInvoiceBrw_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdAllInvoiceBrw"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData7_1"
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
                                                    <ComponentArt:GridLevel AllowGrouping="true"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdAllInvoiceBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="invoice_no" Align="left" HeadingText="Invoice#" AllowGrouping="false" DataCellClientTemplateId="ALLINV" FixedWidth="True" Width="100" />
                                                            <ComponentArt:GridColumn DataField="invoice_date" Align="left" HeadingText="Date" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_id" Align="left" HeadingText="billing_cycle_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_name" Align="left" HeadingText="Billing Cycle" AllowGrouping="false" Width="90" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Invoice Amount($)" AllowGrouping="false" Width="100" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="adjusted" Align="right" HeadingText="Payment Applied($)" AllowGrouping="false" Width="110" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="refunded" Align="right" HeadingText="Refunded($)" AllowGrouping="false" Width="80" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="balance" Align="right" HeadingText="Balance Due($)" AllowGrouping="false" Width="100" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="PAYALLINV" HeadingText="" FixedWidth="True" Width="100" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdAllInvoiceBrw_onRenderComplete" />
                                                    <ItemSelect EventHandler="grdAllInvoiceBrw_onItemSelect" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="ALLINV">

                                                        <a href="javascript:void(0);" id="lnkINVALL_## DataItem.GetMember('id').Value ##" title="click to view this invoice" class="spanLink" style="cursor: pointer; text-decoration: none;" onclick="javascript:ShowInvoice('## DataItem.GetMember('billing_cycle_id').Value ##');">## DataItem.GetMember('invoice_no').Value ##</a>

                                                    </ComponentArt:ClientTemplate>

                                                    <ComponentArt:ClientTemplate ID="PAYALLINV">
                                                        <button type="button" id="btnAllPay_## DataItem.GetMember('id').Value ##" class="btn btn-grid btn-success" style="display:none;" onclick="javascript:btnAllPayRow_Click(this, '## DataItem.GetMember('id').Value ##', grdAllInvoiceBrw.get_table().getRow(## DataItem.get_index() ##))">
                                                            <i class="fa fa-credit-card"></i>&nbsp;Pay Now
                                                        </button>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnErrInvAll" runat="server"></span>
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
                                            <CallbackComplete EventHandler="grdAllInvoiceBrw_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="sparkline10-list mt-b-10" id="pmtDiv" runat="server">
                <div class="searchSection">

                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-12 col-xs-12">
                                <h3 class="h3Text">Previous Payments</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackPaymentBrw" runat="server" OnCallback="CallBackPaymentBrw_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdPaymentBrw"
                                            CssClass="Grid"
                                            DataAreaCssClass="GridData7_1"
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
                                                <ComponentArt:GridLevel AllowGrouping="true"
                                                    DataKeyField="id"
                                                    ShowTableHeading="false"
                                                    TableHeadingCssClass="GridHeader"
                                                    RowCssClass=""
                                                    HoverRowCssClass=""
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="(DataItem.GetMember('processing_status').Value=='0') && ((DataItem.get_index() + grdPaymentBrw.get_recordOffset()) % 2) > 0" RowCssClass="RedRowAlt" SelectedRowCssClass="SelectedRow" HoverRowCssClass="HoverRowRed"/>
                                                        <ComponentArt:GridConditionalFormat ClientFilter="(DataItem.GetMember('processing_status').Value=='0') && ((DataItem.get_index() + grdPaymentBrw.get_recordOffset()) % 2) == 0" RowCssClass="RedRow" SelectedRowCssClass="SelectedRow" HoverRowCssClass="HoverRowRed"/>
                                                        <ComponentArt:GridConditionalFormat ClientFilter="(DataItem.GetMember('processing_status').Value=='1') && ((DataItem.get_index() + grdPaymentBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" HoverRowCssClass="HoverRow" />
                                                        <ComponentArt:GridConditionalFormat ClientFilter="(DataItem.GetMember('processing_status').Value=='1') && ((DataItem.get_index() + grdPaymentBrw.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="SelectedRow" HoverRowCssClass="HoverRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="payref_no" Align="left" HeadingText="Pay Process Ref#" AllowGrouping="false" Width="130" />
                                                        <ComponentArt:GridColumn DataField="date_created" Align="left" HeadingText="Date" AllowGrouping="false" Width="130" FormatString="MMM dd yyyy hh:mm tt" />
                                                        <ComponentArt:GridColumn DataField="payment_amount" Align="right" HeadingText="Amount($)" AllowGrouping="false" Width="100" FormatString="#0.00" />
                                                        <ComponentArt:GridColumn DataField="payment_mode_name" Align="left" HeadingText="Mode" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="processing_ref_no" Align="left" HeadingText="External Ref#" AllowGrouping="false" Width="130" />
                                                        <ComponentArt:GridColumn DataField="processing_status" Align="left" HeadingText="StatusId" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="remarks" Align="left" HeadingText="Remarks" AllowGrouping="false" Width="150" />
                                                        <ComponentArt:GridColumn DataField="processing_status_name" Align="left" HeadingText="Status" AllowGrouping="false" Width="90" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdPaymentBrw_onRenderComplete" />
                                            </ClientEvents>

                                        </ComponentArt:Grid>
                                        <span id="spnERR1" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdInvoiceBrw_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>


                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="sparkline10-list mt-b-10" id="adjDiv" runat="server">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="col-sm-12 col-xs-12">
                                <h3 class="h3Text">Payments Applied</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackAdjBrw" runat="server" OnCallback="CallBackAdjBrw_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdAdjBrw"
                                            CssClass="Grid"
                                            DataAreaCssClass="GridData7_1"
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
                                                <ComponentArt:GridLevel AllowGrouping="true"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="DataItem.GetMember('processing_status').Value=='0'" RowCssClass="RedRow" SelectedRowCssClass="SelectedRow" />
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdAdjBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="payref_no" Align="left" HeadingText="Ref#" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="refundref_no" Align="left" HeadingText="Refund#" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="date_created" Align="left" HeadingText="Date" AllowGrouping="false" Width="130" />
                                                        <ComponentArt:GridColumn DataField="payment_amount" Align="right" HeadingText="Invoice Amount($)" AllowGrouping="false" Width="110" FormatString="#0.00" />
                                                        <ComponentArt:GridColumn DataField="adj_amount" Align="right" HeadingText="Payment Applied($)" AllowGrouping="false" Width="120" FormatString="#0.00" />
                                                        <ComponentArt:GridColumn DataField="payment_mode_name" Align="left" HeadingText="Mode" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="processing_ref_no" Align="left" HeadingText="External Ref#" AllowGrouping="false" Width="130" />
                                                        <ComponentArt:GridColumn DataField="processing_status" Align="left" HeadingText="Status" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="processing_status_name" Align="left" HeadingText="Status" AllowGrouping="false" Width="100" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdAdjBrw_onRenderComplete" />
                                            </ClientEvents>

                                        </ComponentArt:Grid>
                                        <span id="spnErrAdj" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdAdjBrw_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>



        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnMode" runat="server" value="1" />
        <input type="hidden" id="hdnInfo" runat="server" value="" />
        <input type="hidden" id="initial" runat="server" value="1" />

    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtSelectedAmount = document.getElementById('<%=txtSelectedAmount.ClientID %>');
    var objbtnPay = document.getElementById('<%=btnPay.ClientID %>');
    var objhdnMode = document.getElementById('<%=hdnMode.ClientID %>');
    var objhdnInfo = document.getElementById('<%=hdnInfo.ClientID %>');
    var objinitial = document.getElementById('<%=initial.ClientID %>');

    var strForm = "VRSPayment";
    
    
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/Payment.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
