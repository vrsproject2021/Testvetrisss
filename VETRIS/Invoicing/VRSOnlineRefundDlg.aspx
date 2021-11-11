<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSOnlineRefundDlg.aspx.cs" Inherits="VETRIS.Invoicing.VRSOnlineRefundDlg" %>
 <%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
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
    <link href="../css/grid_style.css?2" rel="stylesheet" />
    <link href="../css/CalendarStyle.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href = "../css/theme.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/OnlineRefundDlgHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12  marginTP5">
                            <h3 class="h3Text">Online Payment Refund</h3>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" id="btnAdd1" runat="server" class="btn btn-custon-four btn-primary">
                                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
                            <button type="button" id="btnSave1" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <%--------------------------------%>
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Refund Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Billing Account<span class="mandatory">*</span></label>
                                <div class="input-effect">
                                    <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Refund Process Ref#</label>
                                <asp:TextBox ID="txtOurRefNo" runat="server" ReadOnly="true" TabIndex="-1" CssClass="form-control" MaxLength="10" Enabled="false" Width="80%"></asp:TextBox>
                            </div>

                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Date</label>
                                <asp:TextBox ID="txtRefDate" runat="server" ReadOnly="true" TabIndex="-1" CssClass="form-control" MaxLength="10" Width="40%" Enabled="false"></asp:TextBox>
                            </div>

                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">Amount to Refund ($)<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtAmount" runat="server" Enabled="false" CssClass="form-control" Width="60%" style="text-align:right;">
                                </asp:TextBox>
                            </div>
                        </div>
                     </div>
                    
                </div>
            </div>
            <%--------------------------------%>
            <div class="sparkline10-list mt-b-10" id="pmtDiv" runat="server">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                        <div class="col-sm-6 col-xs-12 marginMobileTP10">
                            <h3 class="h3Text">Payment reference details <span class="HelpText">(please select a single payment record from the record(s) enlisted below)</span></h3>
                        </div>
                        
                            <div class="borderSearch pull-left"></div>
                            </div>
                        
                    </div>
                

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
                                                        <ComponentArt:GridColumn DataField="payref_no" Align="left" HeadingText="Payment Ref#" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="payref_date" Align="left" HeadingText="Date" AllowGrouping="false" Width="80" />
                                                        <ComponentArt:GridColumn DataField="processing_ref_no" Align="left" HeadingText="Processing Ref#" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="payment_amount" Align="right" HeadingText="Original Amount($)" AllowGrouping="false" Width="140" FormatString="#,0.00" />
                                                        <ComponentArt:GridColumn DataField="refundable" Align="right" HeadingText="Already Refunded($)" AllowGrouping="false" Width="140" FormatString="#,0.00" />
                                                        <ComponentArt:GridColumn DataField="current_refund" Align="right" HeadingText="Amount to Refund($)" AllowGrouping="false" Width="140" FormatString="#,0.00" />
<%--                                                        <ComponentArt:GridColumn DataField="current_balance" Align="right" HeadingText="Refundable($)" AllowGrouping="false" Width="120" FormatString="#,0.00" />--%>
                                                        <ComponentArt:GridColumn DataField="selected" Align="center" ColumnType="CheckBox" DataType="System.Boolean" HeadingText="Select" AllowEditing="True" AllowSorting="False" AllowGrouping="false" DataCellClientTemplateId="SELECT" FixedWidth="True" Width="80" />
                                                   </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdInvoiceBrw_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="SELECT">
                                                    <div class="grid_option">
                                                        <input type="checkbox" id="chkSel_## DataItem.GetMember('id').Value ##" style="width: 18px; height: 18px;"
                                                            ## if(DataItem.getCurrentMember().get_value() == true) "checked"; else "unchecked" ## 
                                                            onclick="javascript: ToggleCheckbox(this, '## DataItem.ClientId ##', event);"
                                                             />
                                                    </div>
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

                    </div>
            </div>
            <%--------------------------------%>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnAdd2" runat="server">
                                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp; Save</button>
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
        <input type="hidden" id="paymentID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="paymentRefID" runat="server" value="" />
        <input type="hidden" id="hdnIsAdjusted" runat="server" value="" />
        <input type="hidden" id="hdnAID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="maxVal" runat="server" value="0" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objPaymentID = document.getElementById('<%=paymentID.ClientID %>');
    var objPaymentRefID = document.getElementById('<%=paymentRefID.ClientID %>');
    var objmaxVal = document.getElementById('<%=maxVal.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnAID = document.getElementById('<%=hdnAID.ClientID %>');
    var objddlAccount = document.getElementById('<%=ddlAccount.ClientID %>');
    var objtxtRefDate = document.getElementById('<%=txtRefDate.ClientID %>');
    var objtxtOurRefNo = document.getElementById('<%=txtOurRefNo.ClientID %>');
    var objtxtAmount = document.getElementById('<%=txtAmount.ClientID %>');
    var objbtnSave1 = document.getElementById('<%=btnSave1.ClientID %>');
    var objbtnSave2 = document.getElementById('<%=btnSave2.ClientID %>');
    var strForm = "VRSOnlineRefundDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/OnlineRefundDlg.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
