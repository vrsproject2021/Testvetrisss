<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSFreeCredit.aspx.cs" Inherits="VETRIS.Invoicing.VRSFreeCredit" %>
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
    <script src="scripts/FreeCreditHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12"> 
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Manage Credits</h2>
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
                                <ComponentArt:CallBack ID="CallBackFreeCredit" runat="server" OnCallback="CallBackFreeCredit_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdFreeCredit"
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
                                                    DataMember="FreeCreditHdr"
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
                                                    ShowSelectorCells="true">
                                                        <ConditionalFormats>

                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFreeCredit.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="rec_id"             Align="left"    HeadingText="id"                    AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="billing_account_id" Align="left"    HeadingText="billing_account_id"    AllowGrouping="false" Width="100" Visible="false"/>
                                                        <ComponentArt:GridColumn DataField="billing_acc_name"   Align="left"    HeadingText="Billing Account Name"  AllowGrouping="false" Width="200" />
                                                        <ComponentArt:GridColumn DataField="total_free_credit"  Align="right"   HeadingText="Free Credits"          AllowGrouping="false" Width="80"   DataCellClientTemplateId="CREDIT"/>
                                                        <ComponentArt:GridColumn DataField="bal_free_credit"    Align="right"   HeadingText="Balance"               AllowGrouping="false" Width="80"  />
                                                        <ComponentArt:GridColumn DataField="proc"               Align="Center"  HeadingText=" "                     AllowGrouping="false" Width="30" DataCellClientTemplateId="PROCESS" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                                <ComponentArt:GridLevel
                                                    AllowGrouping="false"
                                                    DataMember="Institutions"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFreeCredit.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        <%--<ComponentArt:GridConditionalFormat ClientFilter="DataItem.GetMember('overdue').Value=='Y'" RowCssClass="RedRow" SelectedRowCssClass="SelectedRowRed" HoverRowCssClass="HoverRowRed" />--%>
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="rec_id"                 Align="left" HeadingText="id"                   AllowGrouping ="false"  Visible="false" />
                                                        <ComponentArt:GridColumn DataField="billing_account_id"     Align="left" HeadingText="billing_account_id"   AllowGrouping ="false"  Visible="false" />
                                                        <ComponentArt:GridColumn DataField="institution_id"         Align="left" HeadingText="institution_id"       AllowGrouping ="false"  Visible="false" />
                                                        <ComponentArt:GridColumn DataField="institution_code"       Align="left" HeadingText="Institution Code"     AllowGrouping ="false"  Width="90"/>
                                                        <ComponentArt:GridColumn DataField="institution_name"       Align="left" HeadingText="Institution Name"     AllowGrouping ="false"  Width="350"/>
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                                    
                                            </Levels>
                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdFreeCredit_onRenderComplete" />
                                                        <ItemSelect EventHandler="grdFreeCredit_onItemSelect" />
                                                        <ItemExpand EventHandler="grdFreeCredit_onItemExpand" />
                                                        <ItemCollapse EventHandler="grdFreeCredit_onItemCollapse" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="CREDIT">
                                                    <input type="text" id="txtCredit_## DataItem.GetMember('billing_account_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%; text-align:right;"  value="## DataItem.GetMember('total_free_credit').Value ##" onchange="javascript:txtCredit_OnChange('## DataItem.GetMember('billing_account_id').Value ##');" onkeypress="javascript:return parent.CheckInteger(event);" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="PROCESS">
                                                    <div class="pull-right">
                                                        <span class="font16 pointer" title="Print">
                                                        <button type="button" id="btnView_## DataItem.GetMember('billing_account_id').Value ##"    class="btn btn-warning btn_grd" title="click to view transaction details" >   <i class="fa fa-eye"      aria-hidden="true"></i></button>
                                                        </span>
                                                    </div>
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
                                        <CallbackComplete EventHandler="grdFreeCredit_onCallbackComplete" />
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

        var strForm = "VRSFreeCredit";

    </script>
    <script src="../scripts/custome-javascript.js"></script>
    <script src="../scripts/AppPages.js"></script>
    <script src="scripts/FreeCredit.js"></script>
</html>
