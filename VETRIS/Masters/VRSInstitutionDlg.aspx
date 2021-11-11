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
    <%--<link href="../css/style.css" rel="stylesheet" />

    <link href="../css/grid_style.css?2" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href = "../css/grid_style.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/InstitutionDlgHdr.js?06012020"></script>
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
                                <h3 class="h3Text">Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>

                    <div class="row"> 
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Code<%--<span class="mandatory">*</span>--%></label>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="5" ReadOnly="true"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-2 col-xs-12">
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
                        <div class="col-sm-2 col-xs-12">
                            <div class="form-group">
                                <label class="control-label">How was account acquired ?</label>
                                <asp:DropDownList ID="ddlInfoSrc" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
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
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Link To Existing Billing Account ?<span class="mandatory">*</span></label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoBAYes" runat="server" GroupName="grpBA" />
                                <label for="rdoBAYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoBANo" runat="server" GroupName="grpBA" />
                                <label for="rdoBANo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Billing Account</label>
                                <asp:DropDownList ID="ddlBA" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                           &nbsp;
                        </div>
                        
                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Services Applicable</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                             <div class="form-group1">
                                <label class="control-label">Consultation</label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoConsultY" runat="server" GroupName="grpCons" />
                                <label for="rdoConsultY" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoConsultN" runat="server" GroupName="grpCons" />
                                <label for="rdoConsultN" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                            </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Storage</label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoStoreY" runat="server" GroupName="grpStore" />
                                <label for="rdoStoreY" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoStoreN" runat="server" GroupName="grpStore" />
                                <label for="rdoStoreN" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                            </div>
                        <div class="col-sm-4 col-xs-12">
                            &nbsp;
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
                                                <h3 class="h3Text">Link the alternate name, if required</h3>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-xs-4">
                                            <div class="pull-right">
                                               &nbsp;
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
                                        <ComponentArt:CallBack ID="CallBackInst" runat="server" OnCallback="CallBackInst_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdInst"
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
                                                    PageSize="6"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInst.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInst.get_recordOffset()) % 2) == 0" RowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="institution_id" Align="left" HeadingText="institution_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="instituion_name" Align="left" HeadingText="Institution" AllowGrouping="false" Width="200" FixedWidth="true" DataCellClientTemplateId="INSTNAME" />
                                                                <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" DataCellClientTemplateId="SELINST" HeadingText="Select" FixedWidth="True" Width="80" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdInst_onRenderComplete" />
                                                    </ClientEvents>
                                                    
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="INSTNAME">
                                                            <span id="spnInst_## DataItem.GetMember('rec_id').Value ##" title ="## DataItem.GetMember('instituion_name').Value ##" ">## DataItem.GetMember('instituion_name').Value ##</span>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="SELINST">
                                                             <div class="grid_option">
                                                                <input type="checkbox" id="chkSelInst_## DataItem.GetMember('rec_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelInst_OnClick('## DataItem.GetMember('rec_id').Value ##');" />
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnInstERR" runat="server"></span>
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
                                                <CallbackComplete EventHandler="grdInst_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
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
                                <h3 class="h3Text">Settings</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">

                            <div class="pull-left">
                                <h3 class="h3Text">Processing of DICOM Media Files</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">DICOM Router Installed?<span class="mandatory">*</span></label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoManual" runat="server" GroupName="grpMethod" />
                                <label for="rdoManual" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoNotReg" runat="server" GroupName="grpMethod" />
                                <label for="rdoNotReg" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                            <div class="pull-left grid_option1 customRadio" style="display: none;">
                                <asp:RadioButton ID="rdoAuto" runat="server" GroupName="grpMethod" />
                                <label for="rdoAuto" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px; display: none;">DICOM Router - Auto Process</span>


                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Format DICOM Files ?<span class="mandatory">*</span></label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoFmtYes" runat="server" GroupName="grpFmt" />
                                <label for="rdoFmtYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoFmtNo" runat="server" GroupName="grpFmt" />
                                <label for="rdoFmtNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                        </div>
                        <div class="col-sm-3 col-xs-12" style="display:none;">
                            <div class="form-group1">
                                <label class="control-label">Compress DICOM Files To Transfer?<span class="mandatory">*</span></label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoCompXferY" runat="server" GroupName="grpXfer" />
                                <label for="rdoCompXferY" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoCompXferN" runat="server" GroupName="grpXfer" />
                                <label for="rdoCompXferN" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                           

                        </div>
                        
                        <div class="col-sm-3 col-xs-12" style="display:none;">
                            <div class="form-group">
                                <label class="control-label" for="usermodality">Study Image File Receiving Path</label>
                                <asp:TextBox ID="txtRecPath" runat="server" CssClass="form-control" MaxLength="250" Width="90%"></asp:TextBox>

                            </div>
                        </div>


                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch pull-left"></div>
                            <div class="pull-left">
                                <h3 class="h3Text">DICOM Tags To Format</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackTags" runat="server" OnCallback="CallBackTags_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdTags"
                                            CssClass="Grid"
                                            DataAreaCssClass="GridData5_1"
                                            SearchOnKeyPress="true"
                                            EnableViewState="true"
                                            ShowSearchBox="true"
                                            SearchBoxPosition="TopRight"
                                            SearchTextCssClass="GridHeaderText"
                                            ShowHeader="true"
                                            FooterCssClass="GridFooter"
                                            GroupingNotificationText=""
                                            PageSize="6"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdTags.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="rec_id" Align="left" HeadingText="rec_id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="sel" Align="left" HeadingText=" " AllowGrouping="false" Width="30" FixedWidth="true" DataCellClientTemplateId="SELTAG" />
                                                        <ComponentArt:GridColumn DataField="group_id" Align="left" HeadingText="Group" AllowGrouping="false" Width="80" />
                                                        <ComponentArt:GridColumn DataField="element_id" Align="left" HeadingText="Element" AllowGrouping="false" Width="80" />
                                                        <ComponentArt:GridColumn DataField="tag_desc" Align="left" HeadingText="Description" AllowGrouping="false" Width="200" />
                                                        <ComponentArt:GridColumn DataField="default_value" Align="left" HeadingText="Default Value" AllowGrouping="false" Width="150" FixedWidth="true" DataCellClientTemplateId="DEFVAL" />
                                                        <ComponentArt:GridColumn DataField="junk_characters" Align="left" HeadingText="Remove Junk Characters" AllowGrouping="false" Width="150" FixedWidth="true" DataCellClientTemplateId="JUNKCHAR" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdTags_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="SELTAG">
                                                    <div class="grid_option">
                                                        <input type="checkbox" id="chkSelTag_## DataItem.GetMember('rec_id').Value ##" style="width: 18px; height: 18px;" disabled="disabled" onclick="javascript: chkSelTag_OnClick('## DataItem.GetMember('rec_id').Value ##');" />
                                                    </div>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="DEFVAL">
                                                    <input type="text" id="txtDefVal_## DataItem.GetMember('rec_id').Value ##" maxlength="250" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('default_value').Value ##" onchange="javascript:txtDefVal_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="JUNKCHAR">
                                                    <input type="text" id="txtJunkChar_## DataItem.GetMember('rec_id').Value ##" maxlength="100" class="GridTextBox" value="## DataItem.GetMember('junk_characters').Value ##" onchange="javascript:txtJunkChar_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                </ComponentArt:ClientTemplate>

                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnErrTags" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdTags_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch pull-left"></div>
                            <div class="pull-left">
                                <h3 class="h3Text">Reports</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Customise Reports ?<span class="mandatory">*</span></label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoCustRptY" runat="server" GroupName="grpCustRpt" />
                                <label for="rdoCustRptY" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoCustRptN" runat="server" GroupName="grpCustRpt" />
                                <label for="rdoCustRptN" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div style="text-align: center">
                                <ComponentArt:CallBack ID="CallBackLogo" runat="server" OnCallback="CallBackLogo_Callback">
                                    <Content>
                                        <asp:Image runat="server" ID="imgLogo" Height="52px" Width="225px" ImageUrl="../images/nologo.jpg" BorderColor="Black" BorderWidth="1px" />
                                    </Content>
                                    <LoadingPanelClientTemplate>
                                        <table style="height: 65px; width: 50px; border-collapse: collapse;" border="0">
                                            <tr>
                                                <td style="text-align: center; vertical-align: middle;">
                                                    <table style="border-collapse: collapse;" border="0">
                                                        <tr>
                                                            <td>
                                                                <img src="../images/spinner-darkgrey.gif" style="height: 65px; width: 50px;" border="0" alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </LoadingPanelClientTemplate>
                                </ComponentArt:CallBack>
                            </div>
                        </div>
                         <div class="col-sm-3 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Fax Reports ?</label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoFaxRptY" runat="server" GroupName="grpFaxRpt" />
                                <label for="rdoFaxRptY" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoFaxRptN" runat="server" GroupName="grpFaxRpt" />
                                <label for="rdoFaxRptN" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                        </div>
                        <div class="col-sm-2 col-xs-12">
                            <div class="form-group1">
                                <label class="control-label">Report Format</label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoRFPdf" runat="server" GroupName="grpRptFmt" />
                                <label for="rdoRFPdf" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">PDF</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoRFRTF" runat="server" GroupName="grpRptFmt" />
                                <label for="rdoPFRTF" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">RTF</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoRFBoth" runat="server" GroupName="grpRptFmt" />
                                <label for="rdoRFBoth" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Both</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            &nbsp;
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div style="text-align: center">
                                <button type="button" class="btn btn_grd btn-warning" id="btnUploadLogo" runat="server" title="click to upload the logo to be viewed in reports">
                                    <i class="fa fa-upload" aria-hidden="true"></i>
                                </button>
                                <button type="button" class="btn btn_grd btn-danger" id="btnDelLogo" runat="server" title="click to remove the logo">
                                    <i class="fa fa-trash-o" aria-hidden="true"></i>
                                </button>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            &nbsp;
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch pull-left"></div>
                            <div class="pull-left">
                                <h3 class="h3Text">Category Linked</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackInsCtg" runat="server" OnCallback="CallBackInsCtg_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdInsCategory"
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
                                                    DataKeyField="category_id"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdInsCategory.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="category_id" Align="left" HeadingText="category_id" AllowGrouping="false" Visible="false" />
                                                        <ComponentArt:GridColumn DataField="sel" Align="left" HeadingText=" " AllowGrouping="false" Width="30" FixedWidth="true" DataCellClientTemplateId="SELINSCTG" />
                                                        <ComponentArt:GridColumn DataField="category_name" Align="left" HeadingText="Category" AllowGrouping="false" Width="200" />
                                                        
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdInsCategory_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="SELINSCTG">
                                                    <div class="grid_option">
                                                        <input type="checkbox" id="chkSelInsCtg_## DataItem.GetMember('category_id').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSelInsCtg_OnClick('## DataItem.GetMember('category_id').Value ##');" />
                                                    </div>
                                                </ComponentArt:ClientTemplate>
                                             
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnErrInsCtg" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdInsCategory_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
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
                                <label class="control-label">Fax #</label>
                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

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
                                                            <input type="password" id="txtPwd_## DataItem.GetMember('rec_id').Value ##" maxlength="20" pattern="" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('password').Value ##" onchange="javascript:txtPwd_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="PACSUSER">
                                                            <input type="text" id="txtPACSUser_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('pacs_user_id').Value ##" onchange="javascript:txtPACSUser_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="PACSPWD">
                                                            <input type="password" id="txtPACSPwd_## DataItem.GetMember('rec_id').Value ##" maxlength="20" class="GridTextBox" style="width: 95%;" value="## DataItem.GetMember('pacs_password').Value ##" onchange="javascript:txtPACSPwd_OnChange('## DataItem.GetMember('rec_id').Value ##');" />
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
          

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Notes</h3>
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
                                                <h3 class="h3Text">Promotions</h3>
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
                                                <h3 class="h3Text">Devices</h3>
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
                                                                <ComponentArt:GridColumn DataField="weight_uom" Align="left" HeadingText="Weight UOM" AllowGrouping="false" DataCellClientTemplateId="WTUOM" FixedWidth="True" Width="100" />
                                                                <ComponentArt:GridColumn DataField="del" Align="center" HeadingText=" " AllowGrouping="false" DataCellClientTemplateId="DELDEVICE" FixedWidth="True" Width="30" />
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

                                                        <ComponentArt:ClientTemplate ID="WTUOM">
                                                          
                                                                <select name="" id="ddlWeightUOM_## DataItem.GetMember('rec_id').Value ##" class="form-control custom-select-value" style="width: 95%;" onchange="javascript:ddlWeightUOM_OnChange('## DataItem.GetMember('rec_id').Value ##');">
                                                                    <option value="0">--Select UOM--</option>
                                                                    <option value="1">Lbs</option>
                                                                    <option value="2">Kgs</option>
                                                                </select>
                                                         
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

            <%--<div class="sparklineHeader mt-b-10 marginTP10">
                <div class="row marginTP10">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-4">
                                            <div class="pull-left" style="margin-top: 8px;">
                                                <h3 style="color: #1e77bb;">Fees Schedule<span class="mandatory">*</span></h3>
                                            </div>
                                        </div>
                                         <div class="col-sm-2 col-xs-2" style="margin-top: 10px;">
                                            <div class="pull-right">
                                                <asp:Label ID="lblAccName" runat="server" Text="Accountant Name"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-sm-2 col-xs-2" style="margin-top: 5px;">
                                            <asp:TextBox ID="txtAccName" runat="server" CssClass="form-control" Width="95%" ></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2 col-xs-2" style="margin-top: 10px;">
                                            <div class="pull-right">
                                                <asp:Label ID="lblDisc" runat="server" Text="Apply Discount (%)"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-sm-1 col-xs-1" style="margin-top: 5px;">
                                            <asp:TextBox ID="txtDisc" runat="server" CssClass="form-control" Text="0.00" Width="95%" Style="text-align: right; padding:3px 6px;" MaxLength="6"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1 col-xs-1" style="margin-top: 5px;">
                                            <div class="pull-right">
                                                <button type="button" class="btn btn-success" id="btnApplyDisc" runat="server" title="click to apply discount">
                                                    APPLY
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
                                        <ComponentArt:CallBack ID="CallBackFees" OnCallback="CallBackFees_Callback" runat="server">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdFees"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData6_1"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    RunningMode="Client"
                                                    ShowSearchBox="false"
                                                    SearchBoxPosition="TopLeft"
                                                    SearchTextCssClass="GridHeaderText"
                                                    ShowHeader="false"
                                                    FooterCssClass="GridFooter"
                                                    GroupingNotificationText=""
                                                    PageSize="7"
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
                                                            DataKeyField="row_id"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFees.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdFees.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                <ComponentArt:GridColumn DataField="rate_id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="head_type" Align="left" HeadingText="Type" AllowGrouping="false" Width="100" />
                                                                <ComponentArt:GridColumn DataField="head_id" Align="left" HeadingText="head_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="head_name" Align="left" HeadingText="Modality/Service" AllowGrouping="false" Width="170" />
                                                                <ComponentArt:GridColumn DataField="img_count_from" Align="center" HeadingText="Min. Image Count" AllowGrouping="true" Width="110" />
                                                                <ComponentArt:GridColumn DataField="img_count_to" Align="center" HeadingText="Max. Image Count" AllowGrouping="false" Width="110" />
                                                                <ComponentArt:GridColumn DataField="fee_amount" Align="right" HeadingText="Fees ($)" AllowGrouping="false" Width="80" FormatString="#0.00" />
                                                                <ComponentArt:GridColumn DataField="inst_fee_amount" Align="right" HeadingText="Applicable Fees ($)" AllowGrouping="false" DataCellClientTemplateId="FEES" FixedWidth="True" Width="130" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdFees_onRenderComplete" />
                                                    </ClientEvents>

                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="FEES">
                                                            <input type="text" id="txtFees_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('inst_fee_amount').Value ##" style="width: 95%; text-align: center;" onchange="javascript:txtFees_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                        </ComponentArt:ClientTemplate>

                                                    </ClientTemplates>
                                                </ComponentArt:Grid>
                                                <span id="spnFeeERR" runat="server"></span>
                                            </Content>
                                            <LoadingPanelClientTemplate>
                                                <table style="height: 255px; width: 100%;" border="0">
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
                                                <CallbackComplete EventHandler="grdFees_onCallbackComplete" />
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
        <input type="hidden" id="hdnUsrUpdUrl" runat="server" value="" />
        <input type="hidden" id="hdnFilePath" runat="server" value="" />
        <input type="hidden" id="hdnFilename" runat="server" value="" />
         <input type="hidden" id="hdnCF" runat="server" value="" />
    </form>
    <script type="text/javascript">
        var objhdnID                = document.getElementById('<%=hdnID.ClientID %>');
        var objhdnError             = document.getElementById('<%=hdnError.ClientID %>');
        var objhdnFilePath          = document.getElementById('<%=hdnFilePath.ClientID %>');
        var objhdnFilename          = document.getElementById('<%=hdnFilename.ClientID %>');
        var objhdnCF                = document.getElementById('<%=hdnCF.ClientID %>');
        var objtxtName              = document.getElementById('<%=txtName.ClientID %>');//--
        var objtxtCode              = document.getElementById('<%=txtCode.ClientID %>');//--
        var objtxtAddr1             = document.getElementById('<%=txtAddr1.ClientID %>');//--
        var objtxtAddr2             = document.getElementById('<%=txtAddr2.ClientID %>');//--
        var objtxtCity              = document.getElementById('<%=txtCity.ClientID %>');//--
        var objtxtZip               = document.getElementById('<%=txtZip.ClientID %>');//--
        var objtxtEmailID           = document.getElementById('<%=txtEmailID.ClientID %>');//--
        var objtxtTel               = document.getElementById('<%=txtTel.ClientID %>');//--
        var objtxtFax               = document.getElementById('<%=txtFax.ClientID %>');//--
        var objtxtContPerson        = document.getElementById('<%=txtContPerson.ClientID %>');//--
        var objtxtContMobile        = document.getElementById('<%=txtContMobile.ClientID %>');//--

        var objddlCountry           = document.getElementById('<%=ddlCountry.ClientID %>');//--
        var objddlState             = document.getElementById('<%=ddlState.ClientID %>');//--
        var objrdoBAYes             = document.getElementById('<%=rdoBAYes.ClientID %>');
        var objrdoBANo              = document.getElementById('<%=rdoBANo.ClientID %>');
        var objddlBA                = document.getElementById('<%=ddlBA.ClientID %>');
        var objrdoCompXferY         = document.getElementById('<%=rdoCompXferY.ClientID %>');
        var objrdoCompXferN         = document.getElementById('<%=rdoCompXferN.ClientID %>');
        var objrdoStoreY            = document.getElementById('<%=rdoStoreY.ClientID %>');
        var objrdoStoreN            = document.getElementById('<%=rdoStoreN.ClientID %>');
        var objrdoConsultY          = document.getElementById('<%=rdoConsultY.ClientID %>');
        var objrdoConsultN          = document.getElementById('<%=rdoConsultN.ClientID %>');
        var objrdoFaxRptY           = document.getElementById('<%=rdoFaxRptY.ClientID %>');
        var objrdoFaxRptN           = document.getElementById('<%=rdoFaxRptN.ClientID %>');
        var objrdoFmtYes            = document.getElementById('<%=rdoFmtYes.ClientID %>');
        var objrdoFmtNo             = document.getElementById('<%=rdoFmtNo.ClientID %>');
        var objrdoNotReg            = document.getElementById('<%=rdoNotReg.ClientID %>');
        var objrdoAuto              = document.getElementById('<%=rdoAuto.ClientID %>');
        var objrdoManual            = document.getElementById('<%=rdoManual.ClientID %>');
        var objtxtRecPath           = document.getElementById('<%=txtRecPath.ClientID %>');
        var objhdnPhysicians        = document.getElementById('<%=hdnPhysicians.ClientID %>');//-
        var objrdoCustRptY          = document.getElementById('<%=rdoCustRptY.ClientID %>');
        var objrdoCustRptN          = document.getElementById('<%=rdoCustRptN.ClientID %>');
        var objrdoRFPdf             = document.getElementById('<%=rdoRFPdf.ClientID %>');
        var objrdoRFRTF             = document.getElementById('<%=rdoRFRTF.ClientID %>');
        var objrdoRFBoth            = document.getElementById('<%=rdoRFBoth.ClientID %>');
        
        var objrdoStatYes           = document.getElementById('<%=rdoStatYes.ClientID %>');//---
        var objrdoStatNo            = document.getElementById('<%=rdoStatNo.ClientID %>');//---
        var objddlInfoSrc           = document.getElementById('<%=ddlInfoSrc.ClientID %>');//---
        var objtxtNotes             = document.getElementById('<%=txtNotes.ClientID %>');//--
       
        var objhdnUsrUpdUrl         = document.getElementById('<%=hdnUsrUpdUrl.ClientID %>');
        var strForm                 = "VRSInstitutionDlg";

    </script>
    <script src="../scripts/custome-javascript.js"></script>
    <script src="../scripts/AppPages.js?2"></script>
    <script src="scripts/InstitutionDlg.js?10052021"></script>
</body>
</html>
