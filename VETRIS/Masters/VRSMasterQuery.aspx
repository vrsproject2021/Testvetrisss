<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSMasterQuery.aspx.cs" Inherits="VETRIS.Masters.VRSMasterQuery" %>

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
    <%--<link href="../css/style.css" rel="stylesheet" />

    <link href="../css/theme.css" rel="stylesheet" />
    <link href="../css/grid_style.css?1" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery.min.js"></script>
    <script src="scripts/MasterQueryHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Master Query</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>
                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
             <div class="sparkline10-list mt-b-10">
                 <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <h3>Filter By :</h3>
                        </div>
                     </div>
                 </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="col-sm-4 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 class="h3Text">Billing Account</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row marginTP5">

                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Code
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5">
                                    <asp:TextBox ID="txtBACode" runat="server" CssClass="form-control" MaxLength="5" Width="50%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Name
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5">
                                    <asp:TextBox ID="txtBAName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Status
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5 marginBTM10">
                                    <asp:DropDownList ID="ddlBAStatus" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="X" Text="All"></asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Y" Text="Active Only"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Inactive Only"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 class="h3Text">Institution</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row marginTP5">

                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Code
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5">
                                    <asp:TextBox ID="txtInstCode" runat="server" CssClass="form-control" MaxLength="5" Width="50%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Name
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5">
                                    <asp:TextBox ID="txtInstName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Status
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5 marginBTM10">
                                    <asp:DropDownList ID="ddlInstStatus" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="X" Text="All"></asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Y" Text="Active Only"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Inactive Only"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">

                                    <div class="pull-left">
                                        <h3 class="h3Text">Users</h3>
                                    </div>

                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row marginTP5">

                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Login ID
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5">
                                    <asp:TextBox ID="txtLoginID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    Status
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5 ">
                                    <asp:DropDownList ID="ddlUserStatus" runat="server" CssClass="form-control custom-select-value">
                                        <asp:ListItem Value="X" Text="All"></asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Y" Text="Active Only"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Inactive Only"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12 marginTP5">
                                    &nbsp;
                                </div>
                                <div class="col-sm-8 col-xs-12 marginTP5" style="margin-bottom:20px;">
                                    &nbsp;
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">

                    <div class="col-sm-12 col-xs-12 text-center" style="margin: 5px;">
                        <button type="button" id="btnSearch" runat="server" class="btn btn-custon-four btn-warning">
                            <i class="fa fa-search" aria-hidden="true"></i>&nbsp;Search   
                        </button>
                        <button type="button" id="btnRefresh" runat="server" class="btn btn-custon-four btn-success">
                            <i class="fa fa-repeat" aria-hidden="true"></i>&nbsp;Refresh    
                        </button>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="sparkline10-graph">
                    <div class="searchSection">
                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="pull-left marginTP10">
                                    <h3 class="h3Text">Query Result</h3>
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
                                                SearchTextCssClass="GridHeaderText"
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
                                                        DataMember="BillingAccount"
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
                                                        SortImageHeight="19"
                                                        SelectorCellWidth="20"
                                                        ShowSelectorCells="false">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Billing Account Code" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Billing Account Name" AllowGrouping="false" Width="230" />
                                                            <ComponentArt:GridColumn DataField="login_id" Align="left" HeadingText="User Login ID" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="user_email_id" Align="left" HeadingText="User Email ID" AllowGrouping="false" Width="160" DataCellClientTemplateId="ACCTUEMAIL" FixedWidth="true"/>
                                                            <ComponentArt:GridColumn DataField="user_mobile_no" Align="left" HeadingText="User Mobile #" AllowGrouping="false" Width="160" />
                                                            <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" Width="50" DataCellClientTemplateId="ACTIONACCT" FixedWidth="true" AllowSorting="False" />

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
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                            <%--<ComponentArt:GridConditionalFormat ClientFilter="DataItem.GetMember('overdue').Value=='Y'" RowCssClass="RedRow" SelectedRowCssClass="SelectedRowRed" HoverRowCssClass="HoverRowRed" />--%>
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="code" Align="left" HeadingText="Institution Code" AllowGrouping="false" Width="100" />
                                                            <ComponentArt:GridColumn DataField="name" Align="left" HeadingText="Institution Name" AllowGrouping="false" Width="360" />
                                                            <ComponentArt:GridColumn  Align="center" HeadingText=" " AllowGrouping="false" Width="50" DataCellClientTemplateId="ACTIONINST" FixedWidth="true" AllowSorting="False"/>
                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                    <ComponentArt:GridLevel
                                                        AllowGrouping="false"
                                                        DataMember="Users"
                                                        DataKeyField="user_id"
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
                                                        SortDescendingImageUrl="col-desc.png" ShowSelectorCells="false"
                                                        SortImageWidth="10"
                                                        SortImageHeight="19">
                                                        <ConditionalFormats>
                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdBrw.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="SelectedRow" />
                                                        </ConditionalFormats>
                                                        <Columns>
                                                            <ComponentArt:GridColumn DataField="user_id" Align="left" HeadingText="user_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="billing_account_id" Align="left" HeadingText="billing_account_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                            <ComponentArt:GridColumn DataField="user_login_id" Align="left" HeadingText="Login ID" AllowGrouping="false" Width="130" />
                                                            <ComponentArt:GridColumn DataField="user_email" Align="left" HeadingText="User Email ID" AllowGrouping="false" Width="150" DataCellClientTemplateId="USEREMAIL" FixedWidth="true"/>
                                                            <ComponentArt:GridColumn DataField="user_contact_no" Align="left" HeadingText="User Contact #" AllowGrouping="false" Width="150" />
                                                            <ComponentArt:GridColumn Align="center" HeadingText=" " AllowGrouping="false" Width="50" DataCellClientTemplateId="ACTIONUSER" FixedWidth="true" AllowSorting="False" />

                                                        </Columns>
                                                    </ComponentArt:GridLevel>
                                                </Levels>
                                                <ClientEvents>
                                                    <RenderComplete EventHandler="grdBrw_onRenderComplete" />
                                                    <ItemSelect EventHandler="grdBrw_onItemSelect" />
                                                    <ItemExpand EventHandler="grdBrw_onItemExpand" />
                                                    <ItemCollapse EventHandler="grdBrw_onItemCollapse" />
                                                </ClientEvents>
                                                <ClientTemplates>
                                                     <ComponentArt:ClientTemplate ID="ACCTUEMAIL">
                                                        <span id="spnAcctUEmail_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('user_email_id').Value ##">## DataItem.GetMember('user_email_id').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="ACTIONACCT">
                                                        <button type="button" id="btnEditBA_## DataItem.GetMember('id').Value ##" class="btn btn-primary btn_grd" title="click to edit/view the details of the billing account" onclick="javascript:btnEditBA_OnClick('## DataItem.GetMember('id').Value ##');"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></button>
                                                    </ComponentArt:ClientTemplate>
                                                     <ComponentArt:ClientTemplate ID="ACTIONINST">
                                                        <button type="button" id="btnAEditInst_## DataItem.GetMember('institution_id').Value ##" class="btn btn-success btn_grd" title="click to edit/view the details of the institution" onclick="javascript:btnEditInst_OnClick('## DataItem.GetMember('institution_id').Value ##');"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></button>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="USEREMAIL">
                                                        <span id="spnUEmail_## DataItem.GetMember('user_id').Value ##" title="## DataItem.GetMember('user_email').Value ##">## DataItem.GetMember('user_email').Value ##</span>
                                                    </ComponentArt:ClientTemplate>
                                                    <ComponentArt:ClientTemplate ID="ACTIONUSER">
                                                        <button type="button" id="btnEditUser_## DataItem.GetMember('user_id').Value ##" class="btn btn-warning btn_grd" title="click to edit/view the details of the user" onclick="javascript:btnEditUser_OnClick('## DataItem.GetMember('user_id').Value ##');"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></button>
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
                                            <CallbackComplete EventHandler="grdBrw_onCallbackComplete" />
                                        </ClientEvents>
                                    </ComponentArt:CallBack>


                                </div>
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

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

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
    var objtxtBACode = document.getElementById('<%=txtBACode.ClientID %>');
    var objtxtBAName = document.getElementById('<%=txtBAName.ClientID %>');
    var objddlBAStatus = document.getElementById('<%=ddlBAStatus.ClientID %>');
    var objtxtInstCode = document.getElementById('<%=txtInstCode.ClientID %>');
    var objtxtInstName = document.getElementById('<%=txtInstName.ClientID %>');
    var objddlInstStatus = document.getElementById('<%=ddlInstStatus.ClientID %>');
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>');
    var objddlUserStatus = document.getElementById('<%=ddlUserStatus.ClientID %>');
    var strForm = "VRSMasterQuery";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/MasterQuery.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
