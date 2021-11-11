<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSCustomerStatement.aspx.cs" Inherits="VETRIS.Invoicing.VRSCustomerStatement" %>

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

    <link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href = "../css/theme.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/CustomerStatementHdr.js?10012020"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-9 col-xs-12">
                            <h3 class="h3Text">Customer Statement</h3>
                        </div>

                        <div class="col-sm-3 col-xs-12 text-right">
                            <button type="button" id="btnClose" runat="server" class="btn btn-custon-four btn-danger">
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
                                        <label class="control-label" for="usermodel">For Billing Account<span class="mandatory">*</span></label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-7 col-xs-12">
                                    &nbsp;
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="pull-left">
                                    <h3 class="h3Text">Statement</h3>
                                </div>
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>
                        </div>
                        <div class="sparkline10-graph">
                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackBrw" runat="server" OnCallback="CallBackBrw_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdBrw"
                                                CssClass="Grid"
                                                AutoTheming="true"
                                                DataAreaCssClass=""
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="false"
                                                SearchBoxPosition="TopLeft"
                                                SearchTextCssClass="GridHeaderText" PageSize="200"
                                                ShowHeader="false"
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
                                                    <ComponentArt:GridLevel 
                                                        AllowGrouping="false"
                                                        DataKeyField="billing_account_id"
                                                        DataMember="AccountBalance"
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
                                                        SortImageHeight="19"
                                                        SelectorCellWidth="20"
                                                        ShowSelectorCells="false">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Billing Account" AllowGrouping="false" Width="220" FixedWidth="true" />
                                                            <ComponentArt:GridColumn  Align="left" HeadingText="" AllowGrouping="false" Width="100" FixedWidth="true" />
                                                            <ComponentArt:GridColumn  Align="left" HeadingText="" AllowGrouping="false" Width="100" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="balance" Align="right" HeadingText="Balance ($)" AllowGrouping="false" Width="120" FormatString="#0.00" FixedWidth="true"/>
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="InvoiceOutstanding"
                                                        DataKeyField="invoice_id"
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
                                                            <ComponentArt:GridColumn DataField="invoice_id" Align="left" HeadingText="invoice_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="invoice_date" Align="left" HeadingText="Invoice Date" AllowGrouping="false" Width="100" Visible="true" />
                                                            <ComponentArt:GridColumn DataField="invoice_no" Align="left" HeadingText="Invoice#" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Total Amount ($)" AllowGrouping="false" Width="120" FormatString="#0.00" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="balance" Align="right" HeadingText="Balance ($)" AllowGrouping="false" Width="120" FormatString="#0.00"  FixedWidth="true" AllowSorting="False" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="PaymentAdjustments"
                                                        DataKeyField="payment_id"
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
                                                        SortDescendingImageUrl="col-desc.png" ShowSelectorCells="false"
                                                        SortImageWidth="10"
                                                        SortImageHeight="19">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="payment_id" Align="left" HeadingText="payment_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="invoice_id" Align="left" HeadingText="invoice_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="payref_date" Align="left" HeadingText="Date" AllowGrouping="false" Width="70" Visible="true" />
                                                            <ComponentArt:GridColumn DataField="payref_no" Align="left" HeadingText="Payment Ref#" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="refundref_no" Align="left" HeadingText="Refund Ref#" AllowGrouping="false" Width="70" />
                                                            <ComponentArt:GridColumn DataField="mode" Align="left" HeadingText="Payment Mode" AllowGrouping="false" Width="90" />
                                                            <ComponentArt:GridColumn DataField="adjusted" Align="right" HeadingText="Adjusted Amount($)" AllowGrouping="false" Width="120" FormatString="#0.00" FixedWidth="true" AllowSorting="False" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                                    <ItemExpand EventHandler="grdBrw_onItemExpand" />
                                                    <ItemCollapse EventHandler="grdBrw_onItemCollapse" />
                                               </ClientEvents>

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
            </div>

        </div>



        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlAccount = document.getElementById('<%=ddlAccount.ClientID %>');
    var strForm = "VRSCustomerStatement";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?IP02012020"></script>
<script src="scripts/CustomerStatement.js?10012020"></script>
</html>
