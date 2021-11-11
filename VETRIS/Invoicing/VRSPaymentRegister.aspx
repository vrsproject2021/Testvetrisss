<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSPaymentRegister.aspx.cs" Inherits="VETRIS.Invoicing.VRSPaymentRegister" %>

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
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />

    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkCAL" runat="server" href = "../css/CalendarStyle.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href = "../css/theme.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/PaymentRegisterHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">

                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Payment Register</h2>
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
                        <div class="col-sm-6 col-xs-6">
                            <div class="pull-left marginTP10">
                                <h3 class="h3Text">Filter By</h3>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-6">
                            &nbsp;
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch pull-left"></div>
                        </div>


                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">

                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <label class="control-label">From Date<span class="mandatory">*</span></label>
                                    <div>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" MaxLength="10" Width="100px" Style="float: left;"></asp:TextBox>
                                        <img src="../images/cal.gif" id="imgFromDt" runat="server" alt="" style="cursor: pointer; margin-top: 5px; margin-left: 5px;" />
                                        <ComponentArt:Calendar runat="server" ID="CalFromDate" AllowMonthSelection="false" AllowMultipleSelection="false"
                                            AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                            ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                            DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                            NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                            PopUp="Custom" PopUpExpandControlId="imgFromDt" PrevImageUrl="cal_prevMonth.gif"
                                            SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                            SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                            SwapDuration="300" SwapSlide="Linear">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="CalFromDate_onSelectionChanged" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-group">
                                    <label class="control-label" for="usermodel">To Date <span class="mandatory">*</span></label>
                                    <div>

                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" MaxLength="10" Width="100px" Style="float: left;"></asp:TextBox>
                                        <img src="../images/cal.gif" id="imgToDt" runat="server" alt="" style="cursor: pointer; margin-top: 5px; margin-left: 5px;" />
                                        <ComponentArt:Calendar runat="server" ID="CalToDate" AllowMonthSelection="false" AllowMultipleSelection="false"
                                            AllowWeekSelection="false" CalendarCssClass="calendar" CalendarTitleCssClass="title"
                                            ControlType="Calendar" DayCssClass="day" DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover"
                                            DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../images/" MonthCssClass="month"
                                            NextImageUrl="cal_nextMonth.gif" NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday"
                                            PopUp="Custom" PopUpExpandControlId="imgToDt" PrevImageUrl="cal_prevMonth.gif"
                                            SelectedDayCssClass="selectedday" SelectMonthCssClass="selector" SelectMonthText="&curren;"
                                            SelectWeekCssClass="selector" ReactOnSameSelection="true" SelectWeekText="&raquo;"
                                            SwapDuration="300" SwapSlide="Linear">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="CalToDate_onSelectionChanged" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Payment Mode</label>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="A" Text="All" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Offline"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Online"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-1 col-xs-12">
                                <div class="form-group">
                                    <div style="height: 22px;"></div>
                                    <button type="button" id="btnSearch" runat="server" class="btn btn-custon-four btn-warning">
                                        <i class="fa fa-question" aria-hidden="true"></i>&nbsp;Query
                                    </button>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>



            <div class="sparkline10-list mt-b-10" id="pmtDiv" runat="server">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-6 col-xs-6">
                            <div class="pull-left marginTP10">
                                <h3 class="h3Text">Transactions</h3>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-6">
                            <div class="pull-right">

                                <button type="button" class="btn btn_grd btn-success" id="btnExcel" runat="server">
                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;Generate Excel
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
                                                <ComponentArt:GridLevel AllowGrouping="false"
                                                    DataMember="Report"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="payref_no" Align="left" HeadingText="Payment Ref#" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="payref_date" Align="left" HeadingText="Date" AllowGrouping="false" Width="80" />
                                                        <ComponentArt:GridColumn DataField="billing_account_name" Align="left" HeadingText="Billing Account Name" AllowGrouping="false" Width="150" DataCellClientTemplateId="BA" FixedWidth="true" />
                                                        <ComponentArt:GridColumn DataField="payment_amount" Align="right" HeadingText="Payment Amount" AllowGrouping="false" Width="120" FormatString="#0.00" />
                                                        <ComponentArt:GridColumn DataField="payment_mode_name" Align="left" HeadingText="Mode" AllowGrouping="false" Width="60" />
                                                        <ComponentArt:GridColumn DataField="processing_ref_no" Align="left" HeadingText="External Ref#" AllowGrouping="false" Width="80" />
                                                        <ComponentArt:GridColumn DataField="processing_pg_name" Align="left" HeadingText="Payment Gateway" AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="auth_code" Align="left" HeadingText="Auth Code" AllowGrouping="false" Width="70" />
                                                        <ComponentArt:GridColumn DataField="cvv_response" Align="left" HeadingText="CVV Response" AllowGrouping="false" Width="90" />
                                                        <ComponentArt:GridColumn DataField="avs_response" Align="left" HeadingText="AVS response" AllowGrouping="false" Width="90" />


                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                                <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="AdjustmentInvoice"
                                                        DataKeyField="adj_id"
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
                                                            <ComponentArt:GridColumn DataField="adj_id" Align="left" HeadingText="adj_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="ar_payments_id" Align="left" HeadingText="ar_payments_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="invoice_date" Align="left" HeadingText="Date" AllowGrouping="false" Width="100"/>
                                                            <ComponentArt:GridColumn DataField="invoice_no" Align="left" HeadingText="Invoice#" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="refundref_no" Align="left" HeadingText="Refund#" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="adj_amount" Align="right" HeadingText="Adjusted Amount ($)" AllowGrouping="false" Width="120" FormatString="#0.00" FixedWidth="true" AllowSorting="False" />
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                                <ItemExpand EventHandler="grdBrw_onItemExpand" />
                                                    <ItemCollapse EventHandler="grdBrw_onItemCollapse" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="BA">
                                                        <span id="spnBA_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('billing_account_name').Value ##">## DataItem.GetMember('billing_account_name').Value ##</span>
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
                                        <CallbackComplete EventHandler="grdBrw_onCallbackComplete" />
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

    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objddlType = document.getElementById('<%=ddlType.ClientID %>');
    var objtxtFromDate = document.getElementById('<%=txtFromDate.ClientID %>');
    var objtxtToDate = document.getElementById('<%=txtToDate.ClientID %>');

    var strForm = "VRSPaymentRegister";


</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/PaymentRegister.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
