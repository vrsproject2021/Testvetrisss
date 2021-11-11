<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRefunds.aspx.cs" Inherits="VETRIS.Invoicing.VRSRefunds" %>
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
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <script src="js/global.js"></script>
    <script src="scripts/RefundsHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Online Payment Refund</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span class="pull-left searchBySpan">Search By
                                </span>
                                <span id="Span1" class="pull-right">
                                    <i style="font-size: 12px;" class="fa fa-search searchBySpan" aria-hidden="true"></i>
                                </span>
                            </button>
                            <button type="button" id="btnAdd" runat="server" class="btn btn-custon-four btn-primary">
                                <i class="fa fa-plus " aria-hidden="true"></i>&nbsp;Add Refund</button>
                            <button type="button" id="btnClose" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="searchSection searchPanelBg" id="searchSection">
                <div>
                    <div>
                        <div class="row">
                            
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Created By</label>
                                    <div class="input-effect">
                                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Billing Account</label>
                                    <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Refund Mode</label>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="1" Text="Online"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Status</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="A" Text="All" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Failed"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Passed"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12 text-right marginTP5">
                                <button type="button" id="btnSearch" runat="server" class="btn btn-custon-four btn-warning">
                                    <i class="fa fa-search edu-warning-danger" aria-hidden="true"></i>&nbsp;Search   
                                </button>
                                <button type="button" id="btnRefresh" runat="server" class="btn btn-custon-four btn-success">
                                    <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Refresh    
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="static-table-list">
                        <div class="table-responsive">
                            <ComponentArt:CallBack ID="CallBackBrw" runat="server" OnCallback="CallBackBrw_Callback">
                                <Content>
                                    <ComponentArt:Grid
                                        ID="grdBrw"
                                        CssClass="Grid"
                                        DataAreaCssClass="GridData17"
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
                                        PagerPosition="BottomRight"
                                        PagerInfoPosition="BottomLeft"
                                        PagerStyle="Buttons"
                                        PagerButtonWidth="24"
                                        PagerButtonHeight="24"
                                        PagerButtonHoverEnabled="true"
                                        SliderHeight="26"
                                        SliderWidth="150"
                                        SliderGripWidth="9"
                                        SliderPopupOffsetX="80"
                                        SliderPopupClientTemplateId="SliderTemplate"
                                        SliderPopupCachedClientTemplateId="CachedSliderTemplate"
                                        PagerTextCssClass="GridFooterText"
                                        ImagesBaseUrl="../images/"
                                        PagerImagesFolderUrl="../images/pager/"
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
                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                </ConditionalFormats>
                                                <Columns>
                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="date_created" Align="left" HeadingText="Created On" AllowGrouping="false" Width="80" FormatString="MMM dd, yyyy hh:mmtt" />
                                                    <ComponentArt:GridColumn DataField="user_id" Align="left" HeadingText="user_id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="user_name" Align="left" HeadingText="Created By" AllowGrouping="false" Width="80" />
                                                    <ComponentArt:GridColumn DataField="billing_account_name" Align="left" HeadingText="Billing Account" AllowGrouping="false" Width="200" />
                                                    <ComponentArt:GridColumn DataField="refund_mode_name" Align="left" HeadingText="Mode" AllowGrouping="false" Width="70" />
                                                    <ComponentArt:GridColumn DataField="refundref_no" Align="left" HeadingText="Refund Ref#" AllowGrouping="false" Width="90" />
                                                    <ComponentArt:GridColumn DataField="refundref_date" Align="left" HeadingText="Date" AllowGrouping="false" Width="80"/>
                                                    <ComponentArt:GridColumn DataField="refund_amount" Align="right" HeadingText="Amount ($)" AllowGrouping="false" Width="80" FormatString="#0.00" />
                                                    <ComponentArt:GridColumn DataField="processing_ref_no" Align="left" HeadingText="External Ref#" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="processing_ref_date" Align="left" HeadingText="External Ref Date" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="processing_status" Align="left" HeadingText="StatusId" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="processing_status_name" Align="left" HeadingText="Status" AllowGrouping="false" Width="70" />
                                                </Columns>

                                            </ComponentArt:GridLevel>
                                        </Levels>
                                        <ClientEvents>
                                            <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                            <ItemSelect EventHandler="grdBrw_onItemSelect" />
                                        </ClientEvents>
                                        
                                    </ComponentArt:Grid>
                                    <span id="spnERR" runat="server"></span>
                                </Content>
                                <LoadingPanelClientTemplate>
                                    <table style="height: 610px; width: 100%;" border="0">
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



        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlType = document.getElementById('<%=ddlType.ClientID %>');
    var objddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
    var objddlUser = document.getElementById('<%=ddlUser.ClientID %>');
    var objddlAccount = document.getElementById('<%=ddlAccount.ClientID %>');
    var strForm = "VRSPaymentProcess";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/Refunds.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
