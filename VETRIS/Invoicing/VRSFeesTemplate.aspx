<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSFeesTemplate.aspx.cs" Inherits="VETRIS.Invoicing.VRSFeesTemplate" %>

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
    <link id="lnkTAB" runat="server" href = "../css/tabStyle1.css" rel="stylesheet" type="text/css" />
    

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/FeesTemplateHdr.js?05022021"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">

                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Default Fee Schedule</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">

                            <button type="button" id="btnClose1" runat="server" class="btn btn-custon-four btn-danger">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close

                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class=" row mt-b-10 marginTP10">
                <div class="col-sm-12 col-xs-12">
                    <div class="sparkline10-list mt-b-10">
                        <div class=" row mt-b-10 marginTP10">
                            <div class="col-sm-12 col-xs-12">
                                <ComponentArt:TabStrip ID="tsFees"
                                    CssClass="TopGroup"
                                    SiteMapXmlFile="FeesTabData.xml"
                                    EnableTheming="false" DefaultGroupSeparatorWidth="5px"
                                    DefaultGroupShowSeparators="true"
                                    DefaultItemLookId="DefaultTabLook"
                                    DefaultSelectedItemLookId="SelectedTabLook"
                                    DefaultGroupTabSpacing="1"
                                    ImagesBaseUrl="../images/"
                                    MultiPageId="ImageManagerPages"
                                    Width="100%"
                                    runat="server">
                                    <ItemLooks>
                                        <ComponentArt:ItemLook LookId="DefaultTabLook" CssClass="DefaultTab" HoverCssClass="DefaultTabHover" LabelPaddingLeft="0" LabelPaddingRight="1" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconWidth="10" />
                                        <ComponentArt:ItemLook LookId="SelectedTabLook" CssClass="SelectedTab" LabelPaddingLeft="2" LabelPaddingRight="1" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconWidth="10" />
                                    </ItemLooks>
                                    <%--<ClientEvents>
                                                <TabSelect EventHandler="tsFees_OnSelect" />
                                            </ClientEvents>--%>
                                </ComponentArt:TabStrip>
                            </div>
                        </div>
                        <div class=" row mt-b-10 marginTP10">
                            <div class=" row mt-b-10 marginTP10">
                                <div class="col-sm-12 col-xs-12">
                                    <ComponentArt:MultiPage ID="ImageManagerPages" runat="server" Width="100%">
                                        <ComponentArt:PageView ID="MF" runat="server" Width="100%">
                                            <div class="col-sm-12 col-xs-12">

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                        <button type="button" id="btnAddMF1" runat="server" class="btn btn-custon-four btn-primary">
                                                            <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add
                                                        </button>
                                                        <button type="button" id="btnSaveMF1" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                    </div>
                                                </div>


                                                <div class="sparkline10-list mt-b-10">
                                                    <div class="searchSection">

                                                        <div class="sparkline10-graph">
                                                            <div class="static-table-list">
                                                                <div class="table-responsive">
                                                                    <ComponentArt:CallBack ID="CallBackMF" runat="server" OnCallback="CallBackMF_Callback">
                                                                        <Content>
                                                                            <ComponentArt:Grid
                                                                                ID="grdMF"
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
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="category_id" Align="left" HeadingText="Category" AllowGrouping="false" DataCellClientTemplateId="CATEGORY" FixedWidth="True" Width="150" />
                                                                                            <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="MODALITY" FixedWidth="True" Width="180" />
                                                                                            <ComponentArt:GridColumn DataField="invoice_by" Align="left" HeadingText="invoice_by" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="invoice_by_desc" Align="left" HeadingText="Charge By" AllowGrouping="false" DataCellClientTemplateId="INVDESC" FixedWidth="True" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="default_count_from" Align="center" HeadingText="Min. Value" AllowGrouping="false" DataCellClientTemplateId="MINVAL" FixedWidth="True" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="default_count_to" Align="center" HeadingText="Max. Value" AllowGrouping="false" DataCellClientTemplateId="MAXVAL" FixedWidth="True" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="fee_amount" Align="right" HeadingText="Fee ($)" AllowGrouping="true" DataCellClientTemplateId="MFEES" FixedWidth="True" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="fee_amount_per_unit" Align="right" HeadingText="Add On Fee/Unit ($)" AllowGrouping="true" DataCellClientTemplateId="ADDON" FixedWidth="True" Width="110" />
                                                                                            <ComponentArt:GridColumn DataField="study_max_amount" Align="right" HeadingText="Max. Study Fee ($)" AllowGrouping="true" DataCellClientTemplateId="MAXSY" FixedWidth="True" Width="110" />
                                                                                            <ComponentArt:GridColumn DataField="gl_code" Align="left" HeadingText="G/L Code" AllowGrouping="false" DataCellClientTemplateId="GLMOD" FixedWidth="True" Width="100" Visible="false" />
                                                                                            <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="DELETEMOD" HeadingText=" " FixedWidth="True" Width="30" />
                                                                                        </Columns>

                                                                                    </ComponentArt:GridLevel>
                                                                                </Levels>
                                                                                <ClientEvents>
                                                                                    <RenderComplete EventHandler="grdMF_onRenderComplete" />
                                                                                   
                                                                                </ClientEvents>
                                                                                <ClientTemplates>
                                                                                    <ComponentArt:ClientTemplate ID="CATEGORY">
                                                                                        <select id="ddlCategory_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 95%;" onchange="javascript:ddlCategory_OnChange('## DataItem.GetMember('row_id').Value ##');"> 
                                                                                        </select>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="MODALITY">
                                                                                        <select id="ddlModality_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 95%;" onchange="javascript:ddlModality_OnChange('## DataItem.GetMember('row_id').Value ##');">
                                                                                        </select>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="INVDESC">
                                                                                        <input type="text" id="txtInvBy_## DataItem.GetMember('row_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('invoice_by_desc').Value ##" style="width: 95%;" readonly="readOnly" tabindex="-1"/>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="MFEES">
                                                                                        <input type="text" id="txtMFees_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtMFees_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="MINVAL">
                                                                                        <input type="text" id="txtMinVal_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('default_count_from').Value ##" style="width: 95%; text-align: center;" onchange="javascript:txtMinVal_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckInteger(event);" onblur="javascript:ResetValuInteger(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                     <ComponentArt:ClientTemplate ID="MAXVAL">
                                                                                        <input type="text" id="txtMaxVal_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('default_count_to').Value ##" style="width: 95%; text-align: center;" onchange="javascript:txtMaxVal_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckInteger(event);" onblur="javascript:ResetValuInteger(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="ADDON">
                                                                                        <input type="text" id="txtAddOn_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount_per_unit').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtAddOn_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="MAXSY">
                                                                                        <input type="text" id="txtMaxFee_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('study_max_amount').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtMaxFee_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="GLMOD">
                                                                                        <input type="text" id="txtGLMod_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('gl_code').Value ##" style="width: 95%;" onchange="javascript:txtGLMod_OnChange('## DataItem.GetMember('row_id').Value ##')"/>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="DELETEMOD">
                                                                                        <button type="button" id="btnDelMod_## DataItem.GetMember('row_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteRow('## DataItem.GetMember('row_id').Value ##','M')">
                                                                                            <i class="fa fa-trash" aria-hidden="true"></i>

                                                                                        </button>
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
                                                                            <CallbackComplete EventHandler="grdMF_onCallbackComplete" />
                                                                        </ClientEvents>
                                                                    </ComponentArt:CallBack>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                         <button type="button" id="btnAddMF2" runat="server" class="btn btn-custon-four btn-primary">
                                                            <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add</button>
                                                        <button type="button" id="btnSaveMF2" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                                                    </div>
                                                </div>

                                            </div>
                                        </ComponentArt:PageView>
                                        <ComponentArt:PageView ID="SF" runat="server" Width="100%">
                                            <div class="col-sm-12 col-xs-12">
                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                         <button type="button" id="btnAddSF1" runat="server" class="btn btn-custon-four btn-primary">
                                                            <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add</button>
                                                        <button type="button" id="btnSaveSF1" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save
                                                        </button>
                                                    </div>
                                                </div>

                                                <div class="sparkline10-list mt-b-10">
                                                    <div class="searchSection">

                                                        <div class="sparkline10-graph">
                                                            <div class="static-table-list">
                                                                <div class="table-responsive">
                                                                    <ComponentArt:CallBack ID="CallBackSF" runat="server" OnCallback="CallBackSF_Callback">
                                                                        <Content>
                                                                            <ComponentArt:Grid
                                                                                ID="grdSF"
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
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                                            <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdMF.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                                                        </ConditionalFormats>
                                                                                        <Columns>
                                                                                            <ComponentArt:GridColumn DataField="row_id" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                                                            <ComponentArt:GridColumn DataField="id" Align="left" HeadingText="id" AllowGrouping="false" Visible="false" />
                                                                                            <ComponentArt:GridColumn DataField="service_id" Align="left" HeadingText="Service" AllowGrouping="false" DataCellClientTemplateId="SERVICE" FixedWidth="True" Width="200" />
                                                                                             <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="Modality" AllowGrouping="false" DataCellClientTemplateId="SMODALITY" FixedWidth="True" Width="200" />
                                                                                            <ComponentArt:GridColumn DataField="invoice_by" Align="left" HeadingText="invoice_by" AllowGrouping="false" Visible="false"/>
                                                                                             <ComponentArt:GridColumn DataField="invoice_by_desc" Align="left" HeadingText="Charge By" AllowGrouping="false" DataCellClientTemplateId="SINVDESC" FixedWidth="True" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="default_count_from" Align="center" HeadingText="Min. Value" AllowGrouping="false" DataCellClientTemplateId="SMINVAL" FixedWidth="True" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="default_count_to" Align="center" HeadingText="Max. Value" AllowGrouping="false" DataCellClientTemplateId="SMAXVAL" FixedWidth="True" Width="100" />
                                                                                            <ComponentArt:GridColumn DataField="fee_amount" Align="right" HeadingText="Default Fee ($)" AllowGrouping="true" DataCellClientTemplateId="SFEES" FixedWidth="True" Width="150" />
                                                                                            <ComponentArt:GridColumn DataField="fee_amount_after_hrs" Align="right" HeadingText="After Hrs. Fee ($)" AllowGrouping="false" DataCellClientTemplateId="SFEESAH" FixedWidth="True" Width="150" />
                                                                                            <ComponentArt:GridColumn DataField="gl_code" Align="left" HeadingText="G/L Code" AllowGrouping="false" DataCellClientTemplateId="GLSVC" FixedWidth="True" Width="100" Visible="false"/>
                                                                                             <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="DELETESVC" HeadingText=" " FixedWidth="True" Width="30" />
                                                                                        </Columns>

                                                                                    </ComponentArt:GridLevel>
                                                                                </Levels>
                                                                                <ClientEvents>
                                                                                    <RenderComplete EventHandler="grdSF_onRenderComplete" />
                                                                                </ClientEvents>

                                                                                <ClientTemplates>
                                                                                    <ComponentArt:ClientTemplate ID="SERVICE">
                                                                                        <select id="ddlService_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 95%;" onchange="javascript:ddlService_OnChange('## DataItem.GetMember('row_id').Value ##');"> 
                                                                                        </select>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SMODALITY">
                                                                                        <select id="ddlSModality_## DataItem.GetMember('row_id').Value ##" class="form-control custom-select-value" style="width: 95%;" onchange="javascript:ddlSModality_OnChange('## DataItem.GetMember('row_id').Value ##');">
                                                                                        </select>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SINVDESC">
                                                                                        <input type="text" id="txtSInvBy_## DataItem.GetMember('row_id').Value ##" class="GridTextBoxNoBorder" value="## DataItem.GetMember('invoice_by_desc').Value ##" style="width: 95%;" readonly="readOnly" tabindex="-1"/>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                     <ComponentArt:ClientTemplate ID="SMINVAL">
                                                                                        <input type="text" id="txtSMinVal_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('default_count_from').Value ##" style="width: 95%; text-align: center;" onchange="javascript:txtSMinVal_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckInteger(event);" onblur="javascript:ResetValuInteger(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                     <ComponentArt:ClientTemplate ID="SMAXVAL">
                                                                                        <input type="text" id="txtSMaxVal_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('default_count_to').Value ##" style="width: 95%; text-align: center;" onchange="javascript:txtSMaxVal_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckInteger(event);" onblur="javascript:ResetValuInteger(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SFEES">
                                                                                        <input type="text" id="txtSFees_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtSFees_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    <ComponentArt:ClientTemplate ID="SFEESAH">
                                                                                        <input type="text" id="txtSFeesAH_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('fee_amount_after_hrs').Value ##" style="width: 95%; text-align: right;" onchange="javascript:txtSFeesAH_OnChange('## DataItem.GetMember('row_id').Value ##');" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" />
                                                                                    </ComponentArt:ClientTemplate>
                                                                                     <ComponentArt:ClientTemplate ID="GLSVC">
                                                                                        <input type="text" id="txtGLSvc_## DataItem.GetMember('row_id').Value ##" class="GridTextBox" value="## DataItem.GetMember('gl_code').Value ##" style="width: 95%;" onchange="javascript:txtGLSvc_OnChange('## DataItem.GetMember('row_id').Value ##')"/>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                    
                                                                                    <ComponentArt:ClientTemplate ID="DELETESVC">
                                                                                        <button type="button" id="btnDelSvc_## DataItem.GetMember('row_id').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteRow('## DataItem.GetMember('row_id').Value ##','S')"><i class="fa fa-trash" aria-hidden="true"></i></button>
                                                                                    </ComponentArt:ClientTemplate>
                                                                                </ClientTemplates>
                                                                            </ComponentArt:Grid>
                                                                            <span id="spnSvcErr" runat="server"></span>
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
                                                                            <CallbackComplete EventHandler="grdSF_onCallbackComplete" />
                                                                        </ClientEvents>
                                                                    </ComponentArt:CallBack>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-12 col-xs-12 text-right" style="margin-left: -5px;">
                                                        <button type="button" id="btnAddSF2" runat="server" class="btn btn-custon-four btn-primary">
                                                            <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add</button>
                                                        <button type="button" id="btnSaveSF2" runat="server" class="btn btn-custon-four btn-success">
                                                            <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </ComponentArt:PageView>
                                    </ComponentArt:MultiPage>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">



                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
         <input type="hidden" id="hdnCategory" runat="server" value="" />
        <input type="hidden" id="hdnModality" runat="server" value="" />
        <input type="hidden" id="hdnServices" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnCategory = document.getElementById('<%=hdnCategory.ClientID %>');
    var objhdnModality = document.getElementById('<%=hdnModality.ClientID %>');
    var objhdnServices = document.getElementById('<%=hdnServices.ClientID %>');
    var strForm = "VRSFeesTemplate";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/FeesTemplate.js?07022021"></script>
</html>
