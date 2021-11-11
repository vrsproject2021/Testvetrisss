<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInstitutionDlg.aspx.cs" Inherits="VETRIS.Masters.VRSInstitutionDlg" %>

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

    <link href="../css/grid_style.css?1" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/InstitutionDlgHdr.js?1"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Institution Details</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" id="btnAdd1" runat="server" class="btn btn-custon-four btn-primary">
                                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
                            <button type="button" id="btnSave1" runat="server" class="btn btn-custon-four btn-success">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
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

                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Code<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Active ?<span class="mandatory">*</span></label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoStatYes" runat="server" GroupName="grpStat" />
                                <label for="rdoStatYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoStatNo" runat="server" GroupName="grpStat" />
                                <label for="rdoStatNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Address 1</label>
                                <asp:TextBox ID="txtAddr1" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Address 2</label>
                                <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">City</label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Country</label>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">State</label>
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>

                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="zip">Zip</label>
                                <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 style="color: #1e77bb;">Contacts</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Email ID</label>
                                <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Office Contact #</label>
                                <asp:TextBox ID="txtTel" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">PACS Contact #</label>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Contact Person</label>
                                <asp:TextBox ID="txtContPerson" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Contact Person Mobile #</label>
                                <asp:TextBox ID="txtContMobile" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-8 col-xs-8">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 style="color: #1e77bb;">Physicians</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-4">
                                            <div class="pull-right">
                                                <button type="button" class="btn btn_grd btn-primary" id="btnAddPhys" runat="server" title="click to add new row for physician">
                                                    <i class="fa fa-plus " aria-hidden="true"></i>
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
                                        <ComponentArt:CallBack ID="CallBackPhys" runat="server" OnCallback="CallBackPhys_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdPhys"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPhys.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="physician_id" Align="left" HeadingText="physician_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="physician_fname" Align="left" HeadingText="First Name" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="FNAME" />
                                                                <ComponentArt:GridColumn DataField="physician_lname" Align="left" HeadingText="Last Name" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="LNAME" />
                                                                <ComponentArt:GridColumn DataField="physician_credentials" Align="left" HeadingText="Credentials" AllowGrouping="false" Width="80" FixedWidth="true" DataCellClientTemplateId="CRED" />
                                                                <ComponentArt:GridColumn DataField="physician_email" Align="left" HeadingText="Email ID (Contact)" AllowGrouping="false" Width="250" FixedWidth="true" DataCellClientTemplateId="EMAIL" />
                                                                <ComponentArt:GridColumn DataField="physician_mobile" Align="left" HeadingText="Mobile #" AllowGrouping="false" Width="180" FixedWidth="true" DataCellClientTemplateId="MOBILE" />
                                                                <ComponentArt:GridColumn DataField="del" Align="center" AllowGrouping="false" DataCellClientTemplateId="DELPHYS" HeadingText=" " FixedWidth="True" Width="50" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdPhys_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="FNAME">
                                                            <input type="text" id="txtFname_## DataItem.GetMember('rec_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('physician_fname').Value ##" onchange="javascript:txtFname_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="LNAME">
                                                            <input type="text" id="txtLname_## DataItem.GetMember('rec_id').Value ##" maxlength="80" class="GridTextBox" value="## DataItem.GetMember('physician_lname').Value ##" onchange="javascript:txtLname_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="CRED">
                                                            <input type="text" id="txtCred_## DataItem.GetMember('rec_id').Value ##" maxlength="30" class="GridTextBox" value="## DataItem.GetMember('physician_credentials').Value ##" onchange="javascript:txtCred_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>

                                                        <ComponentArt:ClientTemplate ID="EMAIL">
                                                            <input type="text" id="txtEmail_## DataItem.GetMember('rec_id').Value ##" maxlength="500" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('physician_email').Value ##" onchange="javascript:txtEmail_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MOBILE">
                                                            <input type="text" id="txtMobile_## DataItem.GetMember('rec_id').Value ##" maxlength="500" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('physician_mobile').Value ##" onchange="javascript:txtMobile_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>

                                                        <ComponentArt:ClientTemplate ID="DELPHYS">
                                                            <button type="button" id="btnDelPhys_## DataItem.GetMember('rec_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeletePhysicianRow('## DataItem.GetMember('rec_id').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrPhys" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 205px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="grdPhys_onCallbackComplete" />
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
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-8 col-xs-8">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 style="color: #1e77bb;">Login Credentials</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-4">
                                            <div class="pull-right">
                                                <button type="button" class="btn btn_grd btn-primary" id="btnAddCred" runat="server" title="click to add new row for login credentials">
                                                    <i class="fa fa-plus " aria-hidden="true"></i>
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
                                        <ComponentArt:CallBack ID="CallBackCred" runat="server" OnCallback="CallBackCred_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdCred"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdCred.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="login_id" Align="left" HeadingText="Login ID" AllowGrouping="false" Width="130" FixedWidth="true" DataCellClientTemplateId="LOGIN" />
                                                                <ComponentArt:GridColumn DataField="password" Align="left" HeadingText="Password" AllowGrouping="false" Width="80" FixedWidth="true" DataCellClientTemplateId="PWD" />
                                                                <ComponentArt:GridColumn DataField="pacs_user_id" Align="left" HeadingText="PACS User ID" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="PACSUSER" />
                                                                <ComponentArt:GridColumn DataField="pacs_password" Align="left" HeadingText="PACS Password" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="PACSPWD" />
                                                                <ComponentArt:GridColumn DataField="email_id" Align="left" HeadingText="Email ID" AllowGrouping="false" Width="250" FixedWidth="true" DataCellClientTemplateId="UEMAIL" />
                                                                <ComponentArt:GridColumn DataField="is_active" Align="center" HeadingText="Active ?" AllowGrouping="false" DataCellClientTemplateId="STATUS" FixedWidth="True" Width="100" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdCred_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>

                                                        <ComponentArt:ClientTemplate ID="LOGIN">
                                                            <input type="text" id="txtLoginID_## DataItem.GetMember('rec_id').Value ##" maxlength="50" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('login_id').Value ##" onchange="javascript:txtLoginID_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="PWD">
                                                            <input type="text" id="txtPwd_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('password').Value ##" onchange="javascript:txtPwd_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="PACSUSER">
                                                            <input type="text" id="txtPACSUser_## DataItem.GetMember('rec_id').Value ##" maxlength="10" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('pacs_user_id').Value ##" onchange="javascript:txtPACSUser_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="PACSPWD">
                                                            <input type="text" id="txtPACSPwd_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('pacs_password').Value ##" onchange="javascript:txtPACSPwd_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="UEMAIL">
                                                            <input type="text" id="txtUserEmail_## DataItem.GetMember('rec_id').Value ##" maxlength="50" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('email_id').Value ##" onchange="javascript:txtUserEmail_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="STATUS">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkAct_## DataItem.GetMember('rec_id').Value ##" style="width: 18px; height: 18px; display: inline;" onclick="javascript: ChkStatus_OnClick('## DataItem.GetMember('rec_id').Value ##');" />
                                                                <button type="button" id="btnDelUser_## DataItem.GetMember('rec_id').Value ##" class="btn btn-danger btn_grd" style="display: none;" onclick="javascript:DeleteUserRow('## DataItem.GetMember('rec_id').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                            </div>

                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrCred" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 205px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="grdCred_onCallbackComplete" />
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
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-8 col-xs-6">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 style="color: #1e77bb;">Sales Person</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-6">
                                            <div class="pull-right">
                                            </div>
                                        </div>
                                        <div class="col-sm-12 col-xs-12">
                                            <div class="borderSearch pull-left"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group">

                                        <asp:DropDownList ID="ddlSalesPerson" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 style="color: #1e77bb; float: left;">Notes</h3>
                                <span>&nbsp;(Max. 250 charaters)</span>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="form-group">

                                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" MaxLength="250" TextMode="MultiLine" Height="50px" Width="99%"></asp:TextBox>

                            </div>
                        </div>

                    </div>


                </div>
            </div>
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-6">
                                        <div class="pull-left" style="margin-top: 8px;">
                                            <h3 style="color: #1e77bb;">Devices</h3>
                                        </div>
                                    </div>
                                        <div class="col-sm-6 col-xs-6">
                                        <div class="pull-right">
                                            <button type="button" class="btn btn_grd btn-primary" id="btnAddDevice" runat="server" title="click to add new row for device">
                                                <i class="fa fa-plus " aria-hidden="true"></i>
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
                                        <ComponentArt:CallBack ID="CallBackDevice" runat="server" OnCallback="CallBackDevice_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdDevice"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    RunningMode="Client"
                                                    ShowSearchBox="false"
                                                    SearchBoxPosition="TopLeft"
                                                    SearchTextCssClass="GridHeaderText"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDevice.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="device_id" Align="left" HeadingText="device_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="manufacturer" Align="left" HeadingText="Manufacturer" AllowGrouping="false" DataCellClientTemplateId="MFG" FixedWidth="True" Width="180" />
                                                                <ComponentArt:GridColumn DataField="modality" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="100" />
                                                                <ComponentArt:GridColumn DataField="modality_ae_title" Align="left" HeadingText="Modality AE Title" AllowGrouping="false" DataCellClientTemplateId="AETITLE" FixedWidth="True" Width="130" />
                                                                <ComponentArt:GridColumn DataField="del" Align="center" AllowGrouping="false" DataCellClientTemplateId="DELDEVICE" HeadingText=" " FixedWidth="True" Width="30" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdDevice_onRenderComplete" />
                                                    </ClientEvents>

                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="MFG">
                                                            <input type="text" id="txtManf_## DataItem.GetMember('rec_id').Value ##" maxlength="200" style="width: 90%;" class="GridTextBox" value="## DataItem.GetMember('manufacturer').Value ##" onchange="javascript:txtManf_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MODALITY">
                                                            <input type="text" id="txtModality_## DataItem.GetMember('rec_id').Value ##" maxlength="50" style="width: 90%;" class="GridTextBox" value="## DataItem.GetMember('modality').Value ##" onchange="javascript:txtModality_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>

                                                        <ComponentArt:ClientTemplate ID="AETITLE">
                                                            <input type="text" id="txtAETitle_## DataItem.GetMember('rec_id').Value ##" maxlength="20" style="width: 90%;" class="GridTextBox" value="## DataItem.GetMember('modality_ae_title').Value ##" onchange="javascript:txtAETitle_OnChange('## DataItem.GetMember('rec_id').Value ##');" />

                                                        </ComponentArt:ClientTemplate>

                                                        <ComponentArt:ClientTemplate ID="DELDEVICE">
                                                            <button type="button" id="btnDelDev_## DataItem.GetMember('rec_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteDeviceRow('## DataItem.GetMember('rec_id').Value ##')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnDevERR" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 205px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="grdDevice_onCallbackComplete" />
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
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnAdd2" runat="server">
                                <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add New</button>
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSave2" runat="server">
                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp; Save</button>
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

        <%--<div class="sparkline10-list mt-b-10 marginTP10">
            
        </div>--%>




        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnPhysicians" runat="server" value="" />
    </form>
    <script type="text/javascript">
        var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
        var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
        var objtxtName = document.getElementById('<%=txtName.ClientID %>');//--
        var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');//--
        var objtxtAddr1 = document.getElementById('<%=txtAddr1.ClientID %>');//--
        var objtxtAddr2 = document.getElementById('<%=txtAddr2.ClientID %>');//--
        var objtxtCity = document.getElementById('<%=txtCity.ClientID %>');//--
        var objtxtZip = document.getElementById('<%=txtZip.ClientID %>');//--
        var objtxtEmailID = document.getElementById('<%=txtEmailID.ClientID %>');//--
        var objtxtTel = document.getElementById('<%=txtTel.ClientID %>');//--
        var objtxtMobile = document.getElementById('<%=txtMobile.ClientID %>');//--
        var objtxtContPerson = document.getElementById('<%=txtContPerson.ClientID %>');//--
        var objtxtContMobile = document.getElementById('<%=txtContMobile.ClientID %>');//--

        var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');//--
        var objddlState = document.getElementById('<%=ddlState.ClientID %>');//--
        var objhdnPhysicians = document.getElementById('<%=hdnPhysicians.ClientID %>');//-
        var objddlSalesPerson = document.getElementById('<%=ddlSalesPerson.ClientID %>');
        var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');//---
        var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');//---
        var objtxtNotes = document.getElementById('<%=txtNotes.ClientID %>');//--
        var strForm = "VRSInstitutionDlg";

    </script>
    <script src="../scripts/custome-javascript.js"></script>
    <script src="../scripts/AppPages.js?2"></script>
    <script src="scripts/InstitutionDlg.js?27052019"></script>
</body>
</html>
