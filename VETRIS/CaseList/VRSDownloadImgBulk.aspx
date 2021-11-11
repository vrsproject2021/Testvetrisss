<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSDownloadImgBulk.aspx.cs" Inherits="VETRIS.CaseList.VRSDownloadImgBulk" %>
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
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/ExceptionInstitutionHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Processing Download</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" id="btnOk1" runat="server" class="btn btn-success">
                                <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Ok</button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        

                        <div class="sparkline10-graph">
                            <div class="static-table-list">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackInst" runat="server" OnCallback="CallBackInst_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdInst"
                                                CssClass="Grid"
                                                AutoTheming="true"
                                                DataAreaCssClass=""
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                RunningMode="Client"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopLeft"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="true"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText="" PageSize="10"
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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInst.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Code" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Name" AllowGrouping="false" Width="250" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="left" HeadingText="Select" AllowGrouping="false" Width="50" DataCellClientTemplateId="SEL" FixedWidth="true" AllowSorting="False" />

                                                        </Columns>

                                                    </ComponentArt:GridLevel>

                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdInst_onRenderComplete" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="SEL">
                                                        <div class="grid_option optSwitch pull-left">
                                                            <input type="checkbox" id="chkSel_## DataItem.GetMember('institution_id').Value ##" onclick="javascript: chkSel_OnClick('## DataItem.GetMember('institution_id').Value ##');" />
                                                            <label for="chkSel_## DataItem.GetMember('institution_id').Value ##" class="label-default"></label>
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
                                                                    <img src="../images/spinner-darkgrey.gif" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdInst_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparklineHeader marginTP10" id="divMsg">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <asp:Label ID="lblMsg" runat="server">&nbsp;</asp:Label>
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
                            <button type="button" id="btnOk2" runat="server" class="btn btn-success">
                                <i class="fa fa-check" aria-hidden="true"></i>&nbsp;Ok</button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp; Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
