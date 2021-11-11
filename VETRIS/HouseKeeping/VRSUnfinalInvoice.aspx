<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSUnfinalInvoice.aspx.cs" Inherits="VETRIS.HouseKeeping.VRSUnfinalInvoice" %>
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
    <%--<link href="../css/style.css" rel="stylesheet" />

    <link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/UnfinalInvoiceHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Unfinal Invoice</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                           
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>
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
                                <div class="col-sm-5 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Billing Account</label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-2 col-xs-12 text-center" style="margin-top: 22px;">
                                    <button type="button" id="btnOk" runat="server" class="btn btn-primary">
                                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;OK</button>
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
                                <div class="pull-left marginTP10">
                                    <h3 class="h3Text">Statements</h3>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-6">
                                <div class="pull-right">
                                    <button type="button" class="btn btn_grd btn-primary" id="btnUnfinal" runat="server" title="click to unfinal all the invoice(s)" style="display: none;">
                                        <i class="fa fa-thumbs-o-down" aria-hidden="true"></i>
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
                                                            <ComponentArt:GridColumn DataField="billing_account_name" Align="left" HeadingText="Billing Account Name" AllowGrouping="false" Width="230" />
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
                                                            <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution Name" AllowGrouping="false" Width="350" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_std" Align="right" HeadingText="Study Count Standard" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_stat" Align="right" HeadingText="Study Count STAT Prelim." AllowGrouping="false" Width="160" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Total Amount ($)" AllowGrouping="false" Width="120" DataCellClientTemplateId="INSTTOTAL" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="approved" Align="left" HeadingText="approved" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="total_disc_amount" Align="right" HeadingText="Total Discount ($)" AllowGrouping="false" Width="100" AllowSorting="False" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="free_read_count" Align="right" HeadingText="Total Free Credits" AllowGrouping="false" Width="100" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="action" Align="center" HeadingText=" " AllowGrouping="false" Width="150" AllowSorting="False" Visible="false" />
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
                                                            <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" Width="150" />
                                                            <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient Name" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="priority_id" Align="left" HeadingText="priority_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Priority" AllowGrouping="false" Width="90" />
                                                            <ComponentArt:GridColumn DataField="image_count" Align="right" HeadingText="Image #" AllowGrouping="false" Width="50" />
                                                            <ComponentArt:GridColumn DataField="object_count" Align="right" HeadingText="Object #" AllowGrouping="false" Width="50" />
                                                            <ComponentArt:GridColumn DataField="rate" Align="right" HeadingText="Rate ($)" AllowGrouping="false" Width="100" Visible="false" />
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
                                                        <button type="button" id="btnUnfinalAcct_## DataItem.GetMember('billing_account_id').Value ##" class="btn btn-primary btn_grd" title="click to unfinal the invoice of this account" onclick="javascript:UnfinalAccount('## DataItem.GetMember('billing_account_id').Value ##','## DataItem.GetMember('total_amount').Value ##');"><i class="fa fa-thumbs-o-down" aria-hidden="true"></i></button>
                                                       
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="INSTTOTAL">
                                                        <input type="text" id="txtInstTot_## DataItem.GetMember('institution_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('total_amount').Value ##" readonly="readOnly" tabindex="-1" style="width: 90%; padding-right: 5px; text-align: right;" />
                                                    </ComponentArt:ClientTemplate>


                                                  <%--  <ComponentArt:ClientTemplate ID="ACTIONINST">
                                                        
                                                        <button type="button" id="btnUnfinalInst_## DataItem.GetMember('institution_id').Value ##" class="btn btn-success btn_grd" title="click to unfinal invoice of this institution" onclick="javascript:UnfinalInstitution('## DataItem.GetMember('billing_account_id').Value ##','## DataItem.GetMember('institution_id').Value ##')"><i class="fa fa-thumbs-o-down" aria-hidden="true"></i></button>
                                                      
                                                    </ComponentArt:ClientTemplate>--%>
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
                           
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp; Close</button>
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
    var objddlBillingCycle = document.getElementById('<%=ddlBillingCycle.ClientID %>');//--
    var objddlAccount = document.getElementById('<%=ddlAccount.ClientID %>');
    var objbtnUnfinal = document.getElementById('<%=btnUnfinal.ClientID %>');
    var strForm = "VRSUnfinalInvoice";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/UnfinalInvoice.js"></script>
</html>
