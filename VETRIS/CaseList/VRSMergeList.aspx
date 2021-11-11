<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSMergeList.aspx.cs" Inherits="VETRIS.CaseList.VRSMergeList" %>

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
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/MergeListHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body class="nav-sm" style="background: transparent;">
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">


                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-8 col-xs-12">
                                    <div class="pull-left" style="margin-top: 8px;">
                                        <h3 class="h3Text">Found the following study(ies) for this patient which qualify(ies) for merging/comaprison :</h3>
                                    </div>

                                </div>
                                <div class="col-sm-4 col-xs-12">
                                    <div class="pull-right">
                                        <button type="button" class="btn btn-success" id="btnMerge" runat="server">Merge All</button>&nbsp;
                                         &nbsp;
                                        <button type="button" class="btn btn-primary" id="btnComp" runat="server">Compare All</button>
                                        &nbsp;
                                        <button type="button" class="btn btn-danger" id="btnNone" runat="server">Ignore All</button>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-xs-12">
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackStudy" runat="server" OnCallback="CallBackStudy_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdStudy"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData4"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopLeft"
                                                    SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                    ShowHeader="false"
                                                    FooterCssClass="GridFooter"
                                                    GroupingNotificationText=""
                                                    PageSize="5"
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
                                                    GroupingNotificationPosition="TopRight">

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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdStudy.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdStudy.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>

                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="study_uid" Align="left" HeadingText="study_uid" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="synched_on" Align="left" HeadingText="Received On/At" AllowGrouping="false" Width="100" />
                                                                <ComponentArt:GridColumn DataField="study_date" Align="left" HeadingText="Study Date/Time" AllowGrouping="false" Width="100" />
                                                                <ComponentArt:GridColumn DataField="patient_name" Align="left" HeadingText="Patient" AllowGrouping="false" DataCellClientTemplateId="PATIENT" FixedWidth="True" Width="100" />
                                                                <ComponentArt:GridColumn DataField="modality" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="130" />
                                                                <ComponentArt:GridColumn DataField="img_count" Align="right" HeadingText="Image Count" AllowGrouping="false" Width="80" />
                                                                <ComponentArt:GridColumn DataField="status_desc" Align="left" HeadingText="Status" AllowGrouping="false" DataCellClientTemplateId="STAT" FixedWidth="True" Width="100" />
                                                                <ComponentArt:GridColumn DataField="merge_compare_none" Align="right" HeadingText="merge_compare_none" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn Align="Left" HeadingText="Merge" AllowGrouping="false" DataCellClientTemplateId="MERGE" FixedWidth="True" Width="50" />
                                                                <ComponentArt:GridColumn Align="Left" HeadingText="Compare" AllowGrouping="false" DataCellClientTemplateId="COMPARE" FixedWidth="True" Width="55" />
                                                                <ComponentArt:GridColumn Align="Left" HeadingText="Ignore" AllowGrouping="false" DataCellClientTemplateId="NONE" FixedWidth="True" Width="50" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdStudy_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="PATIENT">
                                                            <span id="spnPatient_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('patient_name').Value ##">## DataItem.GetMember('patient_name').Value ##</span>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MODALITY">
                                                            <span id="spnMod_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('modality').Value ##">## DataItem.GetMember('modality').Value ##</span>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="STAT">
                                                            <span id="spnStatus" title="## DataItem.GetMember('status_desc').Value ##">## DataItem.GetMember('status_desc').Value ##</span>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MERGE">

                                                            <div class="grid_option optSwitch pull-left">
                                                                <input type="radio" id="rdoMerge_## DataItem.GetMember('id').Value ##" name="grpType__## DataItem.GetMember('id').Value ##" onclick="javascript: UpdateType('## DataItem.GetMember('id').Value ##','M');" />
                                                                <label for="rdoMerge_## DataItem.GetMember('id').Value ##" class="label-default"></label>
                                                            </div>

                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="COMPARE">

                                                            <div class="grid_option optSwitch pull-left">
                                                                <input type="radio" id="rdoComp_## DataItem.GetMember('id').Value ##" name="grpType__## DataItem.GetMember('id').Value ##" onclick="javascript: UpdateType('## DataItem.GetMember('id').Value ##','C');" />
                                                                <label for="rdoComp_## DataItem.GetMember('id').Value ##" class="label-default"></label>
                                                            </div>

                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="NONE">

                                                            <div class="grid_option optSwitch pull-left">
                                                                <input type="radio" id="rdoNone_## DataItem.GetMember('id').Value ##" name="grpType_## DataItem.GetMember('id').Value ##" onclick="javascript: UpdateType('## DataItem.GetMember('id').Value ##','N');" />
                                                                <label for="rdoNone_## DataItem.GetMember('id').Value ##" class="label-default"></label>
                                                            </div>

                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>


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
                                                <CallbackComplete EventHandler="grdStudy_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



            </div>
            <div class="sparklineHeader marginTP10" id="divErr" style="display: none;">
                <div class="sparkline10-hd">
                    <div class="row">

                        <div class="col-sm-12 col-xs-12 text-center" id="divMsg" style="color: red;">
                            &nbsp;
                        </div>

                    </div>
                </div>
            </div>
            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">

                        <div class="col-sm-12 col-xs-12 text-center">
                            <button type="button" class="btn btn-success" id="btnOK" runat="server">
                                <i class="fa fa-check" aria-hidden="true"></i>&nbsp;OK</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');

    var strForm = "VRSMergeList";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/MergeList.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
