<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSManualSubmissionUploadFile.aspx.cs" Inherits="VETRIS.CaseList.VRSManualSubmissionUploadFile" %>
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
    <script src="scripts/ManualSubmissionUploadFileHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <h2>Submit Study Manually </h2>
                        </div>

                        <div class="col-sm-9 col-xs-12 text-right">

                            <button type="button" class="btn btn-custon-four btn-success" id="btnSubmit1" runat="server">
                                <i class="fa fa-thumbs-o-up edu-danger-error" aria-hidden="true"></i>&nbsp;CONTINUE
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-8 col-xs-8">
                                    <h3 class="h3Text">Upload Study File(s)</h3>
                                </div>
                                <div class="col-sm-4 col-xs-4 text-right" >
                                   &nbsp;
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch"></div>
                        </div>
                    </div>
                    <div class="row" id="divDCMUpload" style="display: block;">
                        <div class="col-sm-6 col-xs-12">
                            <div style="border: solid 1px #bbb;">
                                <iframe id="iframeUploadSF" scrolling="no" style="width: 100%; height: auto; background-color: transparent; border: none; min-height: 185px;padding:0px;"></iframe>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12 marginMobileTP5">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackSF" runat="server" OnCallback="CallBackSF_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdSF"
                                            CssClass="Grid"
                                            DataAreaCssClass="GridData5"
                                            SearchOnKeyPress="true"
                                            EnableViewState="true"
                                            ShowSearchBox="true"
                                            SearchBoxPosition="TopRight"
                                            SearchTextCssClass="GridHeaderText"
                                            ShowHeader="false"
                                            FooterCssClass="GridFooter"
                                            GroupingNotificationText=""
                                            PageSize="4"
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
                                                    DataKeyField="file_srl_no"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSF.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdSF.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="file_srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                        <ComponentArt:GridColumn DataField="file_name" Align="left" HeadingText="File Name" AllowGrouping="false" Width="250" DataCellClientTemplateId="STUDYFILE" FixedWidth="True" />
                                                        <ComponentArt:GridColumn DataField="file_type" Align="left" HeadingText="#" AllowGrouping="false" Visible="false"/>
                                                        <ComponentArt:GridColumn DataField="file_type_desc" Align="left" HeadingText="File Type" AllowGrouping="false" Width="100"/>
                                                        <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="DELDCM" HeadingText=" " FixedWidth="True" Width="30" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdSF_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="STUDYFILE">
                                                    <span id="spnSF_## DataItem.GetMember('file_srl_no').Value ##" title="## DataItem.GetMember('file_name').Value ##">## DataItem.GetMember('file_name').Value ##</span>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="DELDCM">
                                                    <button type="button" id="btnDelDCM_## DataItem.GetMember('file_srl_no').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteStudyFile('## DataItem.GetMember('file_srl_no').Value ##')">
                                                        <i class="fa fa-trash" aria-hidden="true"></i>

                                                    </button>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnERR" runat="server"></span>
                                       
                                    </Content>
                                    <LoadingPanelClientTemplate>
                                        <table style="height: 190px; width: 100%;" border="0">
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
                                        <CallbackComplete EventHandler="grdSF_onCallbackComplete" />
                                    </ClientEvents>
                                </ComponentArt:CallBack>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            

            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                        </div>

                        <div class="col-sm-9 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSubmit2" runat="server">
                                <i class="fa fa-thumbs-o-up edu-danger-error" aria-hidden="true"></i>&nbsp;CONTINUE
                                       
                            </button>
                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close     
                            </button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <input type="hidden" id="hdnRegInstitutionId" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnSUID" runat="server" value="" />
        <input type="hidden" id="hdnFilename" runat="server" value="" />
        <input type="hidden" id="hdnFilePath" runat="server" value="" />
        <input type="hidden" id="hdnTempFolder" runat="server" value="" />
        <input type="hidden" id="hdnDCMMODIFYEXEPATH" runat="server" value="" />
      
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnSUID = document.getElementById('<%=hdnSUID.ClientID %>');
    var objRegInstitutionId = document.getElementById('<%=hdnRegInstitutionId.ClientID %>');

  
    var objhdnFilename = document.getElementById('<%=hdnFilename.ClientID %>');
    var objhdnFilePath = document.getElementById('<%=hdnFilePath.ClientID %>');
    var objiframeUploadSF = document.getElementById('iframeUploadSF');
   
    
    var objhdnTempFolder = document.getElementById('<%=hdnTempFolder.ClientID %>');
    var objhdnDCMMODIFYEXEPATH=document.getElementById('<%=hdnDCMMODIFYEXEPATH.ClientID %>');
    var objbtnSubmit1 = document.getElementById('<%=btnSubmit1.ClientID %>');
    var objbtnSubmit2 = document.getElementById('<%=btnSubmit2.ClientID %>');
    var objbtnClose1= document.getElementById('<%=btnClose1.ClientID %>');
    var objbtnClose2= document.getElementById('<%=btnClose2.ClientID %>');
    var strForm = "VRSManualSubmissionUploadFile";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?v=<%=DateTime.Now.Ticks%>"></script>
<script src="scripts/ManualSubmissionUploadFile.js?v=<%=DateTime.Now.Ticks%>"></script>

</html>
