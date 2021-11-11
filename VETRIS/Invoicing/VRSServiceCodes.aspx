<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSServiceCodes.aspx.cs" Inherits="VETRIS.Invoicing.VRSServiceCodes" %>

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
    <script src="scripts/ServiceCodesHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
               
                    <div class="searchSection">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-8 col-xs-8">
                                    <div class="pull-left" style="margin-top:8px;">
                                        <h3 class="h3Text">Update Service Codes</h3>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-xs-4">
                                    <div class="pull-right">

                                        <button type="button" class="btn btn_grd btn-success" id="btnDone" runat="server" title="click when update is done">
                                            <i class="fa fa-check " aria-hidden="true"></i>
                                        </button>
                                        <button type="button" class="btn btn_grd btn-danger" id="btnClose" runat="server" title="click to cancel update">
                                            <i class="fa fa-times " aria-hidden="true"></i>
                                        </button>
                                    </div>
                                </div>
                                
                                    <div class="borderSearch pull-left"></div>
                                
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="table-responsive">
                                    <ComponentArt:CallBack ID="CallBackCodes" runat="server" OnCallback="CallBackCodes_Callback">
                                        <Content>
                                            <ComponentArt:Grid
                                                ID="grdCodes"
                                                CssClass="Grid"
                                                DataAreaCssClass="GridData3_1"
                                                SearchOnKeyPress="true"
                                                EnableViewState="true"
                                                ShowSearchBox="true"
                                                SearchBoxPosition="TopRight"
                                                SearchTextCssClass="GridHeaderText"
                                                ShowHeader="false"
                                                FooterCssClass="GridFooter"
                                                GroupingNotificationText=""
                                                PageSize="4"
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
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
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
                                                        SortedDataCellCssClass="SortedDataCell"
                                                        SortAscendingImageUrl="col-asc.png"
                                                        SortDescendingImageUrl="col-desc.png"
                                                        SortImageWidth="10"
                                                        SortImageHeight="19">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdCodes.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                            <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Description" AllowGrouping="false" Width="200" />
                                                            <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" DataCellClientTemplateId="SEL" HeadingText="Select" FixedWidth="True" Width="50" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>

                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdCodes_onRenderComplete" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                    <ComponentArt:ClientTemplate ID="SEL">
                                                        <div class="grid_option">
                                                            <input type="checkbox" id="chkSel_## DataItem.GetMember('id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSel_OnClick('## DataItem.GetMember('id').Value ##');" />
                                                            <label for="chkSel_## DataItem.GetMember('id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                        </div>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnErr" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 170px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 70px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spinner-darkgrey.gif" width="50" height="65" border="0" alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LoadingPanelClientTemplate>
                                        <ClientEvents>
                                            <CallbackComplete EventHandler="grdCodes_onCallbackComplete" />
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
                        <div class="col-sm-12 col-xs-12 marginTP10" id="divMsg">
                            &nbsp;
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCodes" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnCodes = document.getElementById('<%=hdnCodes.ClientID %>');
    var objdivMsg = document.getElementById("divMsg");
    var strForm = "VRSServiceCodes";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/ServiceCodes.js?01122020"></script>
</html>
