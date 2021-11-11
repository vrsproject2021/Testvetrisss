<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSModality.aspx.cs" Inherits="VETRIS.Masters.VRSModality" %>
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

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/ModalityHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
       <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <h2>Modality</h2>
                        </div>
                        <div class="col-sm-8 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-default" onclick="view_Searchform();">
                                <span style="color: #1e77bb; margin-right: 5px;" class="pull-left">Search By
                                </span>
                                <span id="Span1" class="pull-right">

                                    <i style="color: #1e77bb; font-size: 12px;" class="fa fa-search" aria-hidden="true"></i>
                                </span>
                            </button>
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnAdd" runat="server">
                                <i class="fa fa-plus edu-danger-error" aria-hidden="true"></i>&nbsp;Add Row
                                       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave" runat="server">
                                <i class="fa fa-floppy-o edu-danger-error" aria-hidden="true"></i>&nbsp;Save
                                       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="searchSection searchPart" id="searchSection" style="display: none; position: fixed; z-index: 999; background: #fff;">
                <div>
                    <div>

                        <div class="row">
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <label class="control-label" for="username">Code</label>
                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="form-select-list">
                                    <label class="control-label">Name</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>

                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12 marginMobileTP10">
                                <label class="control-label">Status</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control custom-select-value">
                                    <asp:ListItem  Value="X" Text="All"></asp:ListItem>
                                    <asp:ListItem  Selected="True" Value="Y" Text="Active Only"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Inactive Only"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-xs-12 text-right marginTP5">
                                <button type="button" class="btn btn-custon-four btn-warning" id="btnSearch" runat="server">
                                    <i class="fa fa-search edu-warning-danger" aria-hidden="true"></i>&nbsp;Search
                                           
                                </button>
                                <button type="button" class="btn btn-custon-four btn-success" id="btnRefresh" runat="server">
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
                                        PageSize="23"
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
                                                SelectedRowCssClass=""
                                                SortAscendingImageUrl="col-asc.png"
                                                SortDescendingImageUrl="col-desc.png"
                                                SortImageWidth="10"
                                                SortImageHeight="19">
                                                <ConditionalFormats>
                                                    <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                </ConditionalFormats>
                                                <Columns>
                                                    <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30"/>
                                                    <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                    <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Code" AllowGrouping="false" DataCellClientTemplateId="CODE" FixedWidth="True" Width="100"/>
                                                    <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Name" AllowGrouping="false" DataCellClientTemplateId="NAME" FixedWidth="True" Width="150" />
                                                     <ComponentArt:GridColumn DataField="dicom_tag" Align="left" HeadingText="DICOM Tag" AllowGrouping="false" DataCellClientTemplateId="TAG" FixedWidth="True" Width="150" />
                                                    <ComponentArt:GridColumn DataField="is_active" Align="center" HeadingText="Active" AllowGrouping="false" DataCellClientTemplateId="STATUS" FixedWidth="True" Width="100" />
                                                    <ComponentArt:GridColumn DataField="changed" Align="left" HeadingText="changed" AllowGrouping="false" Visible="false" />
                                                     <ComponentArt:GridColumn DataField="action" Align="center" AllowGrouping="false" DataCellClientTemplateId="ACTION" HeadingText=" " FixedWidth="True" Width="30" />
                                                </Columns>

                                            </ComponentArt:GridLevel>
                                        </Levels>
                                        <ClientEvents>
                                            <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                        </ClientEvents>

                                        <ClientTemplates>
                                            <ComponentArt:ClientTemplate ID="CODE">
                                                <input type="text" id="txtCode_## DataItem.GetMember('rec_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('code').Value ##" maxlength="10" style="width:80%;padding-left:5px;" onchange="javascript:txtCode_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="NAME">
                                                <input type="text" id="txtName_## DataItem.GetMember('rec_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('name').Value ##" maxlength="50" style="width:90%;" onchange="javascript:txtName_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="TAG">
                                                <input type="text" id="txtTag_## DataItem.GetMember('rec_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('dicom_tag').Value ##" maxlength="50" style="width:90%;" onchange="javascript:txtTag_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                            </ComponentArt:ClientTemplate>
                                            <ComponentArt:ClientTemplate ID="STATUS">
                                                <div class="grid_option">
                                                    <input type="checkbox" id="chkAct_## DataItem.GetMember('rec_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: ChkStatus_OnClick('## DataItem.GetMember('rec_id').Value ##');" /> 
                                                   
                                                   <%-- <label for="chkAct_## DataItem.GetMember('rec_id').Value ##" style="width: auto; padding-top: 10px;">Active</label>--%>
                                                </div>
                                              
                                            </ComponentArt:ClientTemplate>

                                              <ComponentArt:ClientTemplate ID="ACTION">
                                                    <button type="button" id="btnDel_## DataItem.GetMember('rec_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteRow('## DataItem.GetMember('rec_id').Value ##')" style="display:none;"><i class="fa fa-trash" aria-hidden="true"></i></button>
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
        <input type="hidden" id="hdnID" runat="server" value="0" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');
    var objtxtName = document.getElementById('<%=txtName.ClientID %>');
    var objddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
    var strForm = "VRSModality";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?10052019"></script>
<script src="scripts/Modality.js"></script>
</html>
