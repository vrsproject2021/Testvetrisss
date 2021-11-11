<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSStudyRadiologistActivity.aspx.cs" Inherits="VETRIS.HouseKeeping.VRSStudyRadiologistActivity" %>
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
    <script src="scripts/StudyRadiologistActivityHdr.js?01092020"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Study Activities</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Study UID :</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-9 col-xs-12">
                            <div class="form-group">
                                <asp:TextBox ID="txtSUID" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>


                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <button type="button" class="btn btn-custon-four btn-success" id="btnOK" runat="server">
                                    <i class="fa fa-check" aria-hidden="true"></i>&nbsp; OK</button>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">
                                    Study Activities of&nbsp;
                                    <span id="spnPNM"></span>
                                </h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12" >
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
                                                SearchBoxPosition="TopRight"
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
                                                        SelectedRowCssClass="SelectedRow"
                                                        SortAscendingImageUrl="col-asc.png"
                                                        SortDescendingImageUrl="col-desc.png"
                                                        SortImageWidth="10"
                                                        SortImageHeight="19">
                                                        <ConditionalFormats>

                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false"/>
                                                            <ComponentArt:GridColumn DataField="functionality" Align="left" HeadingText="Function/Menu" AllowGrouping="false" Width="200" DataCellClientTemplateId="FUNCTIONALITY" FixedWidth="True"/>
                                                            <ComponentArt:GridColumn DataField="activity_text" Align="left" HeadingText="Activity" AllowGrouping="false" DataCellClientTemplateId="ACTIVITY" FixedWidth="True" Width="330" />
                                                            <ComponentArt:GridColumn DataField="activity_by" Align="left" HeadingText="By" AllowGrouping="false" DataCellClientTemplateId="USER" FixedWidth="True" Width="200" />
                                                            <ComponentArt:GridColumn DataField="activity_datetime" Align="left" HeadingText="Date/Time" AllowGrouping="false" Width="130" />
                                                           <ComponentArt:GridColumn DataField="session_id" Align="left" HeadingText="Session ID" AllowGrouping="false" Width="250" />
                                                        </Columns>

                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                              
                                                <ClientTemplates>

                                                    <ComponentArt:ClientTemplate ID="ACTIVITY">
                                                        <span id="spnActivity_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('activity_text').Value ##">## DataItem.GetMember('activity_text').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="USER">
                                                        <span id="spnActivityBy_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('activity_by').Value ##">## DataItem.GetMember('activity_by').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="FUNCTIONALITY">
                                                        <span id="spnFn_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('functionality').Value ##">## DataItem.GetMember('functionality').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                </ClientTemplates>
                                            </ComponentArt:Grid>
                                            <span id="spnERR" runat="server"></span>
                                            <span id="spnDtls" runat="server"></span>
                                        </Content>
                                        <LoadingPanelClientTemplate>
                                            <table style="height: 610px; width: 100%;" border="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <table border="0" style="width: 180px; display: inline-block;">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/Searching.gif"  border="0" alt="" />
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

        </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCF" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnCF = document.getElementById('<%=hdnCF.ClientID %>');
    var objtxtSUID = document.getElementById('<%=txtSUID.ClientID %>');
    var strForm = "VRSStudyRadiologistActivity";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?1"></script>
<script src="scripts/StudyRadiologistActivity.js?20042021"></script>
</html>
