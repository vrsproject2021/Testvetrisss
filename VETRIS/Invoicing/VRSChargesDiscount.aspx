<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSChargesDiscount.aspx.cs" Inherits="VETRIS.Invoicing.VRSChargesDiscount" %>
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
    <link href="../css/style.css" rel="stylesheet" />

    <link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />
    <link href="../css/CalendarStyle.css" rel="stylesheet" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/ChargesDiscountHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12"> 
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Charges Discount</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                           <%-- <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span style="color: #1e77bb; margin-right: 5px;" class="pull-left">Search By
                                </span>
                                <span id="Span1" class="pull-right">
                                    <i style="color: #1e77bb; font-size: 12px;" class="fa fa-search" aria-hidden="true"></i>
                                </span>
                            </button>--%>
                            <button type="button" id="btnSave1" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                            
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
                            <div class="pull-left">
                                <h3 style="color: #1e77bb;">Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                        
                    <div class="sparkline10-graph">
                        <div class="static-table-list">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackChargesDiscount" runat="server" OnCallback="CallBackChargesDiscount_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdChargesDiscount"
                                            CssClass="Grid"
                                            AutoTheming="true"
                                            DataAreaCssClass="GridData20"
                                            SearchOnKeyPress="true"
                                            EnableViewState="true"
                                            RunningMode="Client"
                                            ShowSearchBox="false"
                                            SearchBoxPosition="TopLeft"
                                            SearchTextCssClass="GridHeaderText" PageSize="23"
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

                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdChargesDiscount.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                      
                                                        <ComponentArt:GridColumn DataField="billing_account_id" Align="left"    HeadingText="billing_account_id"    AllowGrouping="false" Width="100" Visible="false"/>
                                                        <ComponentArt:GridColumn DataField="code"   Align="left"    HeadingText="Code"                  AllowGrouping="false" Width="100" />
                                                        <ComponentArt:GridColumn DataField="name"   Align="left"    HeadingText="Billing Account Name"  AllowGrouping="false" Width="200" />
                                                        <ComponentArt:GridColumn DataField="discount_perc"       Align="right"   HeadingText="Discount ( % )"        AllowGrouping="false" Width="80"   DataCellClientTemplateId="DISCOUNT" />
                                                        
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                               
                                            </Levels>
                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdChargesDiscount_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="DISCOUNT">
                                                    <input type="text" id="txtDiscount_## DataItem.GetMember('billing_account_id').Value ##" maxlength="8" class="GridTextBox" style="width: 95%; text-align:right;"  value="## DataItem.GetMember('discount_perc').Value ##" onchange="javascript:txtDiscount_OnChange('## DataItem.GetMember('billing_account_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                </ComponentArt:ClientTemplate>
                                                
                                            </ClientTemplates>
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
                                        <CallbackComplete EventHandler="grdChargesDiscount_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>

                            
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
                                
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp; Save</button>
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

        var strForm = "VRSChargesDiscount";

    </script>
    <script src="../scripts/custome-javascript.js"></script>
    <script src="../scripts/AppPages.js"></script>
<script src="scripts/ChargesDiscount.js"></script>
</html>
