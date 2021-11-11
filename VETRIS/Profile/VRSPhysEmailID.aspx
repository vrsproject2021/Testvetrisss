<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSPhysEmailID.aspx.cs" Inherits="VETRIS.Profile.VRSPhysEmailID" %>

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
    <link href="../css/style.css" rel="stylesheet" />

    <link href="../css/grid_style.css?2" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/PhysEmailIDHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-8 col-xs-8">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 style="color: #1e77bb;">Update Physician Email ID(s)</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-4">
                                            <div class="pull-right">
                                                <button type="button" class="btn btn_grd btn-primary" id="btnAdd" runat="server" title="click to add new row for email id">
                                                    <i class="fa fa-plus " aria-hidden="true"></i>
                                                </button>
                                                <button type="button" class="btn btn_grd btn-success" id="btnDone" runat="server" title="click when update is done">
                                                    <i class="fa fa-check " aria-hidden="true"></i>
                                                </button>
                                                <button type="button" class="btn btn_grd btn-danger" id="btnClose" runat="server" title="click to cancel update">
                                                    <i class="fa fa-times " aria-hidden="true"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="borderSearch pull-left"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackEmail" runat="server" OnCallback="CallBackEmail_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdEmail"
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
                                                    PageSize="3"
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
                                                            DataKeyField="rec_id"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdEmail.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="physician_email" Align="left" HeadingText="Email ID" AllowGrouping="false" Width="250" FixedWidth="true" DataCellClientTemplateId="EMAIL" />
                                                                <ComponentArt:GridColumn DataField="del" Align="center" AllowGrouping="false" DataCellClientTemplateId="DEL" HeadingText=" " FixedWidth="True" Width="50" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdEmail_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>

                                                        <ComponentArt:ClientTemplate ID="EMAIL">
                                                            <input type="text" id="txtEmail_## DataItem.GetMember('rec_id').Value ##" maxlength="50" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('physician_email').Value ##" onchange="javascript:txtEmail_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                      
                                                        <ComponentArt:ClientTemplate ID="DEL">
                                                            <button type="button" id="btnDel_## DataItem.GetMember('rec_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteRow('## DataItem.GetMember('rec_id').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
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
                                                <CallbackComplete EventHandler="grdEmail_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
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
        <input type="hidden" id="hdnPhysID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnDivider" runat="server" value="" />
        <input type="hidden" id="hdnSecDivider" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnPhysID = document.getElementById('<%=hdnPhysID.ClientID %>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID %>');
    var objhdnSecDivider = document.getElementById('<%=hdnSecDivider.ClientID %>');
    var objdivMsg = document.getElementById("divMsg");
    var objbtnAdd = document.getElementById('<%=btnAdd.ClientID %>');
    var strForm = "VRSPhysEmailID";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/PhysEmailID.js"></script>
</html>
