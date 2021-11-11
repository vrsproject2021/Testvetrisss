<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInstitutionBrw.aspx.cs" Inherits="VETRIS.Masters.VRSInstitutionBrw" %>

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

    <link href="../css/grid_style.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />
    <script src="../scripts/jquery.min.js"></script>
    <%--<script src="../scripts/jquery-1.7.1.js"></script>--%>
    <script src="scripts/InstitutionBrwHdr.js?1"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Institutions</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span style="color: #1e77bb; margin-right: 5px;" class="pull-left">Search By
                                </span>
                                <span id="Span1" class="pull-right">
                                    <i style="color: #1e77bb; font-size: 12px;" class="fa fa-search" aria-hidden="true"></i>
                                </span>
                            </button>

                            <button type="button" id="btnAdd" runat="server" class="btn btn-custon-four btn-primary">
                                <i class="fa fa-plus " aria-hidden="true"></i>&nbsp;Add New</button>
                            <button type="button" id="btnClose" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="searchSection searchPart" id="searchSection" style="display: none; position: fixed; z-index: 999; background: #fff;">
                <div>
                    <div>
                        <div class="row">
                            <div class="col-sm-3 col-xs-12">
                               <div class="form-group">
                                    <label class="control-label" for="username">Code</label>
                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Name</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <label class="control-label">Status</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control custom-select-value">
                                    <asp:ListItem  Value="X" Text="All"></asp:ListItem>
                                    <asp:ListItem  Selected="True" Value="Y" Text="Active Only"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Inactive Only"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Country</label>
                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                    
                                </div>
                            </div>

                            <div class="col-sm-3 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">State</label>
                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">City</label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label class="control-label" for="username">Zipcode</label>
                                    <input type="text" placeholder="" value="" name="institutename" id="txtZip" runat="server" class="form-control" />
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
                                        DataAreaCssClass="GridData20"
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

                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                </ConditionalFormats>
                                                <Columns>
                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Code" AllowGrouping="false" Width="80" />
                                                    <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Name" AllowGrouping="false" Width="200" />
                                                    <ComponentArt:GridColumn DataField="city" Align="left" HeadingText="City" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="country_name" Align="left" HeadingText="Country" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="state_name" Align="left" HeadingText="State" AllowGrouping="false" Width="100" />
                                                    <ComponentArt:GridColumn DataField="zip" Align="right" HeadingText="Zip" AllowGrouping="false" Width="50" />
                                                    <ComponentArt:GridColumn DataField="is_active" Align="left" HeadingText="Status" AllowGrouping="false" Width="120" DataCellClientTemplateId="STAT" FixedWidth="true" />
                                                    <ComponentArt:GridColumn DataField="is_new" Align="left" HeadingText="is_new" AllowGrouping="false" Visible="false" />

                                                </Columns>

                                            </ComponentArt:GridLevel>
                                        </Levels>
                                        <ClientEvents>
                                            <ItemSelect EventHandler="grdBrw_onItemSelect" />
                                            <RenderComplete EventHandler ="grdBrw_onRenderComplete" />
                                        </ClientEvents>
                                        <ClientTemplates>
                                            <ComponentArt:ClientTemplate ID="STAT">
                                                <span id="spnStat_## DataItem.GetMember('id').Value ##">## DataItem.GetMember('is_active').Value ##</span>
                                                <img src="../images/newSmall.gif" id="imgNew_## DataItem.GetMember('id').Value ##" alt="" style="height:30px;width:45px;display:none;" />
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
    var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');
    var objtxtName = document.getElementById('<%=txtName.ClientID %>');
    var objddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
    var objtxtCity = document.getElementById('<%=txtCity.ClientID %>');
    var objtxtZip = document.getElementById('<%=txtZip.ClientID %>');
    var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');
    var objddlState = document.getElementById('<%=ddlState.ClientID %>');
    var strForm = "VRSInstitutionBrw";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?2"></script>
<script src="scripts/InstitutionBrw.js"></script>
</html>
