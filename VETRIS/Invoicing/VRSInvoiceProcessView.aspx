<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInvoiceProcessView.aspx.cs" Inherits="VETRIS.Invoicing.VRSInvoiceProcessView" %>

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
    <script src="scripts/InvoiceProcessViewHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
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
                                        <label class="control-label" for="usermodality">Billing Cycle</label>
                                        <asp:TextBox ID="txtBA" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-sm-5 col-xs-12">
                                    &nbsp;
                                </div>
                                <div class="col-sm-2 col-xs-12 text-center" style="margin-top: 22px;">
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
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-left">
                                    <h3 class="h3Text">Statements</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                &nbsp;
                            </div>
                            <div class="col-sm-12 col-xs-12">
                                <div class="borderSearch pull-left"></div>
                            </div>


                        </div>

                        <div class="sparkline10-graph">
                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackInvoice" runat="server" OnCallback="CallBackInvoice_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdInvoice"
                                                CssClass="Grid"
                                                AutoTheming="true"
                                                DataAreaCssClass=""
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="false"
                                                SearchBoxPosition="TopLeft"
                                                SearchTextCssClass="GridHeaderText"
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
                                                    <ComponentArt:GridLevel AllowGrouping="false"
                                                        DataMember="InvoiceHdr"
                                                        DataKeyField="billing_account_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInvoice.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_id" Align="left" HeadingText="billing_cycle_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_account_name" Align="left" HeadingText="Billing Account Name" AllowGrouping="false" Width="280" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_std" Align="right" HeadingText="Study Count Standard" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_stat" Align="right" HeadingText="Study Count STAT Prelim." AllowGrouping="false" Width="160" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Total Amount ($)" AllowGrouping="false" Width="120" DataCellClientTemplateId="ACCTTOTAL" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="approved" Align="left" HeadingText="approved" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="total_disc_amount" Align="right" HeadingText="Total Discount ($)" AllowGrouping="false" Width="100" AllowSorting="False" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="total_free_credits" Align="right" HeadingText="Total Free Credits" AllowGrouping="false" Width="100" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="action" Align="center" HeadingText=" " AllowGrouping="false" Width="150" DataCellClientTemplateId="ACTIONACCT" FixedWidth="true" AllowSorting="False" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="InvoiceDtlsHdr"
                                                        DataKeyField="institution_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInvoice.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                            <%--<ComponentArt:GridConditionalFormat ClientFilter="DataItem.GetMember('overdue').Value=='Y'" RowCssClass="RedRow" SelectedRowCssClass="SelectedRowRed" HoverRowCssClass="HoverRowRed" />--%>
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_id" Align="left" HeadingText="billing_cycle_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="institution_code" Align="left" HeadingText="Institution Code" AllowGrouping="false" Width="90" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution Name" AllowGrouping="false" Width="250" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_std" Align="right" HeadingText="Study Count Standard" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_stat" Align="right" HeadingText="Study Count STAT Prelim." AllowGrouping="false" Width="160" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Total Amount ($)" AllowGrouping="false" Width="120" DataCellClientTemplateId="INSTTOTAL" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="approved" Align="left" HeadingText="approved" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="total_disc_amount" Align="right" HeadingText="Total Discount ($)" AllowGrouping="false" Width="100" AllowSorting="False" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="free_read_count" Align="right" HeadingText="Total Free Credits" AllowGrouping="false" Width="100" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="action" Align="center" HeadingText=" " AllowGrouping="false" Width="150" DataCellClientTemplateId="ACTIONINST" FixedWidth="true" AllowSorting="False" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="InvoiceDtls"
                                                        DataKeyField="study_id"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInvoice.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="study_id" Align="left" HeadingText="study_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_id" Align="left" HeadingText="billing_cycle_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="study_uid" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received Date" AllowGrouping="false" Width="85" />
                                                            <ComponentArt:GridColumn DataField="category_id" Align="left" HeadingText="category_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" Width="120" DataCellClientTemplateId="CTG" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" Width="120" DataCellClientTemplateId="MOD" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient Name" AllowGrouping="false" Width="100" DataCellClientTemplateId="PNAME" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="priority_id" Align="left" HeadingText="priority_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Priority" AllowGrouping="false" Width="90" DataCellClientTemplateId="PRIORITY" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="image_count" Align="right" HeadingText="Image/Body Part #" AllowGrouping="false" Width="120" />
                                                            <ComponentArt:GridColumn DataField="object_count" Align="right" HeadingText="Object #" AllowGrouping="false" Width="50" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="rate" Align="right" HeadingText="Rate ($)" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="amount" Align="right" HeadingText="Study ($)" AllowGrouping="false" Width="80" DataCellClientTemplateId="SYAMT" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="service_amount" Align="right" HeadingText="Service ($)" AllowGrouping="false" Width="80" DataCellClientTemplateId="SVCAMT" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Total ($)" AllowGrouping="false" Width="80" DataCellClientTemplateId="SYTOTAL" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="billed" Align="center" HeadingText="Billed?" AllowGrouping="false" Width="50" DataCellClientTemplateId="BILL" FixedWidth="true" AllowSorting="False" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="approved" Align="left" HeadingText="approved" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="promo_dtls" Align="left" HeadingText="Promotion" AllowGrouping="false" Width="80" DataCellClientTemplateId="PROMO" FixedWidth="true" AllowSorting="False" />

                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdInvoice_onRenderComplete" />
                                                    <ItemSelect EventHandler="grdInvoice_onItemSelect" />
                                                    <ItemExpand EventHandler="grdInvoice_onItemExpand" />
                                                    <ItemCollapse EventHandler="grdInvoice_onItemCollapse" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="ACCTTOTAL">
                                                        <input type="text" id="txtAcctTot_## DataItem.GetMember('billing_account_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('total_amount').Value ##" readonly="readOnly" tabindex="-1" style="width: 90%; padding-right: 5px; text-align: right;" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="ACTIONACCT">
                                                        <button type="button" id="btnNotApproveAcct_## DataItem.GetMember('billing_account_id').Value ##" class="btn btn-danger btn_grd" title="click to approve the invoice for this account" style="display: none;"><i class="fa fa-thumbs-o-up" aria-hidden="true" onclick="javascript:ApproveAcct('## DataItem.GetMember('billing_account_id').Value ##')"></i></button>
                                                        <button type="button" id="btnApproveAcct_## DataItem.GetMember('billing_account_id').Value ##" class="btn btn-success btn_grd" title="The invoice(s) of this account is/are approved" style="display: none;"><i class="fa fa-thumbs-o-up" aria-hidden="true"></i></button>
                                                        <button type="button" id="btnEmailAcct_## DataItem.GetMember('billing_account_id').Value ##" class="btn btn-primary btn_grd" title="click to send email for this account" style="display: none;"><i class="fa fa-envelope" aria-hidden="true" onclick="javascript:btnAcctEmail_OnClick('## DataItem.GetMember('billing_cycle_id').Value ##','## DataItem.GetMember('billing_account_id').Value ##','## DataItem.GetMember('billing_account_name').Value ##');"></i></button>
                                                        <button type="button" id="btnPrintAcct_## DataItem.GetMember('billing_account_id').Value ##" class="btn btn-warning btn_grd" title="click to print the Invoice(s) for this account"><i class="fa fa-print" aria-hidden="true" onclick="javascript:btnPrintAcct_OnClick('## DataItem.GetMember('billing_cycle_id').Value ##','## DataItem.GetMember('billing_account_id').Value ##');"></i></button>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="INSTTOTAL">
                                                        <input type="text" id="txtInstTot_## DataItem.GetMember('institution_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('total_amount').Value ##" readonly="readOnly" tabindex="-1" style="width: 90%; padding-right: 5px; text-align: right;" />
                                                    </ComponentArt:ClientTemplate>


                                                    <ComponentArt:ClientTemplate ID="ACTIONINST">
                                                        <button type="button" id="btnNotApproveInst_## DataItem.GetMember('institution_id').Value ##" class="btn btn-danger btn_grd" title="click to approve the invoice for this institution" style="display: none;"><i class="fa fa-thumbs-o-up" aria-hidden="true" onclick="javascript:ApproveInstitution('## DataItem.GetMember('billing_account_id').Value ##','## DataItem.GetMember('institution_id').Value ##')"></i></button>
                                                        <button type="button" id="btnApproveInst_## DataItem.GetMember('institution_id').Value ##" class="btn btn-success btn_grd" title="The invoice of this institution is approved" style="display: none;"><i class="fa fa-thumbs-o-up" aria-hidden="true"></i></button>
                                                        <button type="button" id="btnEmailInst_## DataItem.GetMember('institution_id').Value ##" class="btn btn-primary btn_grd" title="click to send email to this institution" style="display: none;"><i class="fa fa-envelope" aria-hidden="true" onclick="javascript:btnInstEmail_OnClick('## DataItem.GetMember('billing_cycle_id').Value ##','## DataItem.GetMember('billing_account_id').Value ##','## DataItem.GetMember('institution_id').Value ##','## DataItem.GetMember('institution_name').Value ##');"></i></button>
                                                        <button type="button" id="btnPrintInst_## DataItem.GetMember('institution_id').Value ##" class="btn btn-warning btn_grd" title="click to print the invoice annexure of this institution"><i class="fa fa-print" aria-hidden="true" onclick="javascript:btnPrintInst_OnClick('## DataItem.GetMember('billing_cycle_id').Value ##','## DataItem.GetMember('institution_id').Value ##');"></i></button>
                                                        <button type="button" id="btnEditInst_## DataItem.GetMember('institution_id').Value ##" class="btn btn-primary btn_grd" title="click to edit/amend the studies of this institution"><i class="fa fa-pencil" aria-hidden="true" onclick="javascript:btnEditInst_OnClick('## DataItem.GetMember('billing_account_id').Value ##','## DataItem.GetMember('institution_id').Value ##');"></i></button>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="CTG">
                                                        <span id="spnCtg_## DataItem.GetMember('study_id').Value ##" title="## DataItem.GetMember('category_name').Value ##">## DataItem.GetMember('category_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="MOD">
                                                        <span id="spnMod_## DataItem.GetMember('study_id').Value ##" title="## DataItem.GetMember('modality_name').Value ##">## DataItem.GetMember('modality_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="PNAME">
                                                        <span id="spnPname_## DataItem.GetMember('study_id').Value ##" title="## DataItem.GetMember('patient_name').Value ##">## DataItem.GetMember('patient_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="PRIORITY">
                                                        <span id="spnPriority_## DataItem.GetMember('study_id').Value ##" title="## DataItem.GetMember('priority_desc').Value ##">## DataItem.GetMember('priority_desc').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SYAMT">
                                                        <input type="text" id="txtSYAmt_## DataItem.GetMember('study_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('amount').Value ##" readonly="readOnly" tabindex="-1" style="width: 90%; padding-right: 5px; text-align: right;" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SVCAMT">
                                                        <input type="text" id="txtSVCAmt_## DataItem.GetMember('study_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('service_amount').Value ##" readonly="readOnly" tabindex="-1" style="width: 90%; padding-right: 5px; text-align: right;" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="SYTOTAL">
                                                        <input type="text" id="txtSYTot_## DataItem.GetMember('study_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('total_amount').Value ##" readonly="readOnly" tabindex="-1" style="width: 90%; padding-right: 5px; text-align: right;" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="BILL">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkBill_## DataItem.GetMember('study_id').Value ##" style="width: 18px; height: 18px; display: inline;" onclick="javascript: chkBill_OnClick('## DataItem.GetMember('study_id').Value ##','## DataItem.GetMember('billing_account_id').Value ##','## DataItem.GetMember('institution_id').Value ##');" />
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="PROMO">
                                                        <span id="spnPromo_## DataItem.GetMember('study_id').Value ##" title="## DataItem.GetMember('promo_dtls').Value ##">## DataItem.GetMember('promo_dtls').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnERR" runat="server"></span>
                                            <span id="spnUser" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 830px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdInvoice_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>


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
    var objhdnAID = document.getElementById('<%=hdnAID.ClientID %>');
    var objhdnBCID = document.getElementById('<%=hdnBCID.ClientID %>');
    var strForm = "VRSInvoiceProcessView";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/InvoiceProcessView.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
