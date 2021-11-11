<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInstitutionDlg.aspx.cs" Inherits="VETRIS.Profile.VRSInstitutionDlg" %>

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

    <link href="../css/grid_style.css?2" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/InstitutionDlgHdr.js"></script>
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
                                <h3 class="h3Text">Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Code
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblCode" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Name
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Status
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Billing Account Name
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblBA" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    DICOM Router Installed?
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblDRInstalled" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Address
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblAddr1" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    &nbsp;
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblAddr2" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    City
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblCity" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    State
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblState" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Country
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblCountry" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;">
                                    Zip
                                </div>
                                <div class="col-sm-8 col-xs-12">
                                    <asp:Label ID="lblZip" runat="server"></asp:Label>
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
                                <h3 class="h3Text">Contacts</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                          
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="font-weight: bold;margin-top:10px;">
                                        Email ID
                                    </div>
                                    <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                        <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="font-weight: bold;margin-top:15px;">
                                        Office Contact #
                                    </div>
                                    <div class="col-sm-8 col-xs-12" style="margin-top:10px;">
                                        <asp:TextBox ID="txtTel" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 col-xs-12" style="font-weight: bold;margin-top:15px;">
                                        Fax #
                                    </div>
                                    <div class="col-sm-8 col-xs-12" style="margin-top:10px;">
                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                            
                        </div>
                        <div class="col-sm-6 col-xs-12">
                            <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;margin-top:10px;">
                                    Contact Person
                                </div>
                                <div class="col-sm-8 col-xs-12 marginMobileTP5">
                                    <asp:TextBox ID="txtContPerson" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-4 col-xs-12" style="font-weight: bold;margin-top:15px;">
                                    Contact Person Mobile #
                                </div>
                                <div class="col-sm-8 col-xs-12" style="margin-top:10px;">
                                    <asp:TextBox ID="txtContMobile" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
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
                                                <h3 class="h3Text">Physicians</h3>
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
                                                                <ComponentArt:GridColumn DataField="physician_fname" Align="left" HeadingText="First Name" AllowGrouping="false" Width="130" FixedWidth="true" DataCellClientTemplateId="FNAME" />
                                                                <ComponentArt:GridColumn DataField="physician_lname" Align="left" HeadingText="Last Name" AllowGrouping="false" Width="130" FixedWidth="true" DataCellClientTemplateId="LNAME" />
                                                                <ComponentArt:GridColumn DataField="physician_credentials" Align="left" HeadingText="Credentials" AllowGrouping="false" Width="80" FixedWidth="true" DataCellClientTemplateId="CRED" />
                                                                <ComponentArt:GridColumn DataField="physician_email" Align="left" HeadingText="Email ID (Contact)" AllowGrouping="false" Width="350" FixedWidth="true" DataCellClientTemplateId="EMAIL" />
                                                                <ComponentArt:GridColumn DataField="physician_mobile" Align="left" HeadingText="Mobile #" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="MOBILE" />
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
                                                            <input type="text" id="txtEmail_## DataItem.GetMember('rec_id').Value ##" maxlength="500" style="width: 85%;" class="GridTextBoxNoBorder" value="## DataItem.GetMember('physician_email').Value ##" onchange="javascript:txtEmail_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                            <button type="button" id="btnEditPhysEmail_## DataItem.GetMember('rec_id').Value ##" class="btn btn-warning btn_grd" onclick="javascript:btnEditPhysEmail_OnClick('## DataItem.GetMember('rec_id').Value ##','## DataItem.GetMember('physician_id').Value ##');" title="click to add/update the email id(s)"><i class="fa fa-pencil" aria-hidden="true"></i></button>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="MOBILE">
                                                            <input type="text" id="txtMobile_## DataItem.GetMember('rec_id').Value ##" maxlength="20" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('physician_mobile').Value ##" onchange="javascript:txtMobile_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
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
                                                <h3 class="h3Text">Login Credentials</h3>
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
                                                                <ComponentArt:GridColumn DataField="pacs_user_id" Align="left" HeadingText="PACS User ID" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="PACSUSER" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="pacs_password" Align="left" HeadingText="PACS Password" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="PACSPWD" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="email_id" Align="left" HeadingText="Email ID" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="UEMAIL" />
                                                                <ComponentArt:GridColumn DataField="contact_no" Align="left" HeadingText="Contact #" AllowGrouping="false" Width="100" FixedWidth="true" DataCellClientTemplateId="UCONTACT" />
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
                                                            <input type="text" id="txtPACSUser_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('pacs_user_id').Value ##" onchange="javascript:txtPACSUser_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="PACSPWD">
                                                            <input type="text" id="txtPACSPwd_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('pacs_password').Value ##" onchange="javascript:txtPACSPwd_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="UEMAIL">
                                                            <input type="text" id="txtUserEmail_## DataItem.GetMember('rec_id').Value ##" maxlength="50" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('email_id').Value ##" onchange="javascript:txtUserEmail_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="UCONTACT">
                                                            <input type="text" id="txtUserContact_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('contact_no').Value ##" onchange="javascript:txtUserContact_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
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



            <%--<div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-6 col-xs-6">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 style="color: #1e77bb;">Promotions</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-xs-6">
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
                                        <ComponentArt:CallBack ID="CallBackPromo" runat="server" OnCallback="CallBackPromo_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdPromo"
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
                                                            SelectedRowCssClass=""
                                                            SortAscendingImageUrl="col-asc.png"
                                                            SortDescendingImageUrl="col-desc.png"
                                                            SortImageWidth="10"
                                                            SortImageHeight="19">
                                                            <ConditionalFormats>
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdPromo.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="promotion_type" Align="left" HeadingText="promotion_type" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="promotion_type_desc" Align="left" HeadingText="Promotion Type" AllowGrouping="false" Width="90" />
                                                                <ComponentArt:GridColumn DataField="created_by" Align="left" HeadingText="Created By" AllowGrouping="false" Width="80" />
                                                                <ComponentArt:GridColumn DataField="date_created" Align="left" HeadingText="Created On" AllowGrouping="false" Width="80" />
                                                                <ComponentArt:GridColumn DataField="discount_percent" Align="right" HeadingText="Discount %" AllowGrouping="false" Width="70" FormatString="#0.00" />
                                                                <ComponentArt:GridColumn DataField="free_credits" Align="right" HeadingText="Free Credits" AllowGrouping="false" Width="70" />
                                                                <ComponentArt:GridColumn DataField="valid_from" Align="left" HeadingText="Valid From" AllowGrouping="false" DataCellClientTemplateId="VFROM" FixedWidth="True" Width="80" />
                                                                <ComponentArt:GridColumn DataField="valid_till" Align="left" HeadingText="Valid Till" AllowGrouping="false" Width="80" />
                                                                <ComponentArt:GridColumn DataField="reason" Align="left" HeadingText="Reason" AllowGrouping="false" DataCellClientTemplateId="REASON" FixedWidth="True" Width="150" />
                                                                <ComponentArt:GridColumn DataField="is_active" Align="left" HeadingText="Active?" AllowGrouping="false" Width="50" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdPromo_onRenderComplete" />
                                                    </ClientEvents>

                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="VFROM">
                                                            <span id="spnDt_## DataItem.GetMember('id').Value ##"></span>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="REASON">
                                                            <span id="spnReason_## DataItem.GetMember('id').Value ##" title="## DataItem.GetMember('reason').Value ##">## DataItem.GetMember('reason').Value ##</span>
                                                        </ComponentArt:ClientTemplate>



                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnErrPromo" runat="server"></span>
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
                                                <CallbackComplete EventHandler="grdPromo_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>--%>




            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 hidden-xs">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

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

    </form>
</body>
<script type="text/javascript">
    var objhdnID                = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError             = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtEmailID           = document.getElementById('<%=txtEmailID.ClientID %>');//--
    var objtxtTel               = document.getElementById('<%=txtTel.ClientID %>');//--
    var objtxtMobile            = document.getElementById('<%=txtMobile.ClientID %>');//--
    var objtxtContPerson        = document.getElementById('<%=txtContPerson.ClientID %>');//--
    var objtxtContMobile        = document.getElementById('<%=txtContMobile.ClientID %>');//--

    var strForm                 = "VRSInstitutionDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/InstitutionDlg.js?11062020"></script>
</html>
