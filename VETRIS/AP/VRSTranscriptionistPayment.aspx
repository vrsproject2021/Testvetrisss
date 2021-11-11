<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSTranscriptionistPayment.aspx.cs" Inherits="VETRIS.AP.VRSTranscriptionistPayment" %>
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
    <link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
 
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/TranscriptionistPaymentHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Transcriptionist Payment</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave1" style="display:none;" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save
                                       
                            </button>
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
                                        <label class="control-label" for="usermodel">Transcriptionist</label>
                                        <div class="input-effect">
                                            <asp:DropDownList ID="ddlTranscriptionist" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-2 col-xs-12 text-center" style="margin-top: 22px;">
                                    <button type="button" id="btnOk" runat="server" class="btn btn-primary">
                                        <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Process</button>
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
                                    <button type="button" class="btn btn_grd btn-danger" id="btnApprove" runat="server" title="click to approve all the payments" style="display: none;">
                                        <i class="fa fa-thumbs-o-up" aria-hidden="true"></i>
                                    </button>
                                    <button type="button" class="btn btn_grd btn-warning" id="btnPrint" runat="server" title="click to view payment statement" style="display: none;">
                                        <i class="fa fa-print" aria-hidden="true"></i>
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
                                    <ComponentArt:CallBack ID="CallBackPayment" runat="server" OnCallback="CallBackPayment_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdPayment"
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
                                                    <ComponentArt:GridLevel AllowGrouping="false"
                                                        DataMember="PaymentHdr"
                                                        DataKeyField="transcriptionist_id"
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

                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPayment.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>

                                                            <ComponentArt:GridColumn DataField="transcriptionist_id" Align="left" HeadingText="transcriptionist_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_id" Align="left" HeadingText="billing_cycle_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="transcriptionist_name" Align="left" HeadingText="Transcriptionist Name" AllowGrouping="false" Width="620" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_std" Align="right" HeadingText="Study Count (Std.)" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="total_study_count_stat" Align="right" HeadingText="Study Count (STAT)" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="right" HeadingText="Total Amount ($)" AllowGrouping="false" Width="120" AllowSorting="False" DataCellClientTemplateId="TOTAMT" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="approved" Align="left" HeadingText="approved" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" Width="190" DataCellClientTemplateId="ACTIONTRANS" FixedWidth="true" AllowSorting="False" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>

                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="PaymentDtls"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPayment.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="row_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="study_id" Align="left" HeadingText="study_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="transcriptionist_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_cycle_id" Align="left" HeadingText="billing_cycle_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="study_uid" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="received_date" Align="left" HeadingText="Received Date" AllowGrouping="false" Width="105" />
                                                            <ComponentArt:GridColumn DataField="institution_name" Align="left" HeadingText="Institution" AllowGrouping="false" Width="170" DataCellClientTemplateId="INST" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient Name" AllowGrouping="false" Width="120" DataCellClientTemplateId="PNAME" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" Width="100" DataCellClientTemplateId="MOD" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="priority_desc" Align="left" HeadingText="Priority" AllowGrouping="false" Width="100" DataCellClientTemplateId="PRIOR" FixedWidth="true" />
                                                            <ComponentArt:GridColumn DataField="amount" Align="right" HeadingText="Amount ($)" AllowGrouping="false" Width="110" FormatString="#0.00" />
                                                            <ComponentArt:GridColumn DataField="adhoc_amount" Align="right" HeadingText="Adhoc Amount ($)" AllowGrouping="false" Width="110" DataCellClientTemplateId="ADHOC" FixedWidth="true" AllowSorting="false" />
                                                            <ComponentArt:GridColumn DataField="total_amount" Align="left" HeadingText="total_amount" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" Width="40" DataCellClientTemplateId="REPORT" FixedWidth="true" AllowSorting="false" />
                                                            <ComponentArt:GridColumn DataField="custom_report" Align="left" HeadingText="custom_report" AllowGrouping="false" Visible="false" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdPayment_onRenderComplete" />
                                                    <ItemExpand EventHandler="grdPayment_onItemExpand" />
                                                    <ItemCollapse EventHandler="grdPayment_onItemCollapse" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="TOTAMT">
                                                        <input type="text" id="txtTotAmt_## DataItem.GetMember('transcriptionist_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('total_amount').Value ##" readonly="readOnly" tabindex="-1" style="width: 90%; padding-right: 5px; text-align: right;" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="ACTIONTRANS">
                                                        <button type="button" id="btnSave_## DataItem.GetMember('transcriptionist_id').Value ##" class="btn btn-primary btn_grd" title="click to save the Transcriptionist's record" style="display: none;" onclick="javascript:SaveTranscriptionist('## DataItem.GetMember('transcriptionist_id').Value ##','N');">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>
                                                        </button>
                                                        <button type="button" id="btnNotApprove_## DataItem.GetMember('transcriptionist_id').Value ##" class="btn btn-danger btn_grd" title="click to approve the payment for this Transcriptionist" style="display: none;">
                                                            <i class="fa fa-thumbs-o-up" aria-hidden="true" onclick="javascript:SaveTranscriptionist('## DataItem.GetMember('transcriptionist_id').Value ##','Y');"></i>
                                                        </button>
                                                        <button type="button" id="btnApprove_## DataItem.GetMember('transcriptionist_id').Value ##" class="btn btn-success btn_grd" title="The payment of this Transcriptionist is/are approved" style="display: none;">
                                                            <i class="fa fa-thumbs-o-up" aria-hidden="true"></i>
                                                        </button>

                                                        <button type="button" id="btnPrintTrans_## DataItem.GetMember('transcriptionist_id').Value ##" class="btn btn-warning btn_grd" title="click to print the payment for this Transcriptionist" style="display: inline;">
                                                            <i class="fa fa-print" aria-hidden="true" onclick="javascript:btnPrintTrans_OnClick('## DataItem.GetMember('billing_cycle_id').Value ##','## DataItem.GetMember('transcriptionist_id').Value ##');"></i>
                                                        </button>
                                                        <%--<button type="button" id="btnEmailAcct_## DataItem.GetMember('billing_account_id').Value ##" class="btn btn-primary btn_grd" title="click to send email for this account"  style="display: none;"><i class="fa fa-envelope" aria-hidden="true"></i></button>--%>
                                                    </ComponentArt:ClientTemplate>

                                                    <ComponentArt:ClientTemplate ID="INST">
                                                        <span id="spnInst_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('institution_name').Value ##">## DataItem.GetMember('institution_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="PNAME">
                                                        <span id="spnPname_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('patient_name').Value ##">## DataItem.GetMember('patient_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="MOD">
                                                        <span id="spnMod_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('modality_name').Value ##">## DataItem.GetMember('modality_name').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="PRIOR">
                                                        <span id="spnPrior_## DataItem.GetMember('row_id').Value ##" title="## DataItem.GetMember('priority_desc').Value ##">## DataItem.GetMember('priority_desc').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    
                                                    <ComponentArt:ClientTemplate ID="ADHOC">
                                                        <input type="text" id="txtAdhoc_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('adhoc_amount').Value ##" style="width: 90%; padding-right: 5px; text-align: right;" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onchange="javscript:txtAdhoc_OnChange('## DataItem.GetMember('transcriptionist_id').Value ##','## DataItem.GetMember('study_id').Value ##');" onblur="javascript:ResetValueDecimal(this);" />
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="REPORT">
                                                        <button type="button" id="btnRpt_## DataItem.GetMember('row_id').Value ##" class="btn btn-success btn_grd" title="Report" onclick="javascript:btnRpt_OnClick('## DataItem.GetMember('row_id').Value ##','## DataItem.GetMember('study_id').Value ##','## DataItem.GetMember('patient_name').Value ##','## DataItem.GetMember('custom_report').Value ##');">
                                                            <i class="fa fa-file-text-o" aria-hidden="true"></i>
                                                        </button>
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
                                            <CallbackComplete EventHandler="grdPayment_onCallbackComplete" />
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
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" style="display:none;" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save
                                       
                            </button>
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
    var objddlBillingCycle = document.getElementById('<%=ddlBillingCycle.ClientID %>');
    var objddlTranscriptionist = document.getElementById('<%=ddlTranscriptionist.ClientID %>');
    var objbtnApprove = document.getElementById('<%=btnApprove.ClientID %>');
    var objbtnPrint = document.getElementById('<%=btnPrint.ClientID %>');
    var strForm = "VRSTranscriptionistPayment";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/TranscriptionistPayment.js"></script>
</html>
