<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRadAssignMultiDlg.aspx.cs" Inherits="VETRIS.Radiologist.VRSRadAssignMultiDlg" %>
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

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script src="scripts/RadAssignMultiDlgHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-7 col-xs-12">
                            <h2>Assign Radiologist 
                                 <asp:Label ID="lblAsnType" runat="server"></asp:Label>
                            </h2>
                        </div>

                        <div class="col-sm-5 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave1" runat="server">
                                <i class="fa fa-pencil-square-o edu-danger-error" aria-hidden="true"></i>&nbsp;Assign       
                            </button>

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset1" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset   
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>
           
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Assign Radiologist
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row marginTP10">
                                <div class="col-sm-6 col-xs-12">
                                    <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Availability :</span>
                                    <div class="pull-left grid_option customRadio marginLFT10">
                                        <asp:RadioButton ID="rdoAll" runat="server" Checked="true" GroupName="grpRad" />
                                        <label for="rdoAll" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">All</span>
                                    <div class="pull-left grid_option customRadio marginLFT10">
                                        <asp:RadioButton ID="rdoSchedule" runat="server" GroupName="grpRad" />
                                        <label for="rdoSchedule" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">All Scheduled</span>
                                    
                                </div>
                                <div class="col-sm-6 col-xs-12">
                                    <div class="pull-right">
                                        <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Type : <span class="mandatory">*</span></span>
                                        <div class="pull-left grid_option customRadio marginLFT10">
                                            <asp:RadioButton ID="rdoPrelim" runat="server" GroupName="grpRadType" />
                                            <label for="rdoPrelim" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Reading/Preliminary </span>
                                        <div class="pull-left grid_option customRadio marginLFT10">
                                            <asp:RadioButton ID="rdoFinal" runat="server" GroupName="grpRadType" />
                                            <label for="rdoFinal" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0; padding-top: 5px; padding-right: 2px;">Final</span>
                                    </div>
                                </div>
                            </div>

                            <div class="row marginTP10">
                                <div class="col-sm-12 col-xs-12 marginMobileTP5">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackRad" runat="server" OnCallback="CallBackRad_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdRad"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData5"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopRight"
                                                    SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
                                                    ShowHeader="true"
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
                                                            DataKeyField="radiologist_id"
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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRad.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdRad.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="radiologist_id" Align="left" HeadingText="study_type_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="radiologist_name" Align="left" HeadingText="Radiologist" AllowGrouping="false" Width="300" />
                                                                <ComponentArt:GridColumn DataField="assign" Align="center" HeadingText="Assign" AllowGrouping="false" Width="50" AllowSorting="False" DataCellClientTemplateId="SEL" FixedWidth="true" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="SEL">
                                                            <div class="grid_option">
                                                                <input type="radio" id="rdoSel_## DataItem.GetMember('radiologist_id').Value ##" name="rdoSel" style="width: 18px; height: 18px;" onclick="javascript: rdoSel_OnClick('## DataItem.GetMember('radiologist_id').Value ##');" />
                                                                <label for="rdoSel_## DataItem.GetMember('radiologist_id').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
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
                                                <CallbackComplete EventHandler="grdRad_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-7 col-xs-12">
                            <div id="divMsg">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-sm-5 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-primary" id="btnSave2" runat="server">
                                <i class="fa fa-pencil-square-o edu-danger-error" aria-hidden="true"></i>&nbsp;Assign   
                            </button>

                            <button type="button" class="btn btn-custon-four btn-warning" id="btnReset2" runat="server">
                                <i class="fa fa-repeat edu-danger-error" aria-hidden="true"></i>&nbsp;Reset 
                            </button>

                            <button type="button" class="btn btn-custon-four btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times edu-danger-error" aria-hidden="true"></i>&nbsp;Close     
                            </button>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnStudy" runat="server" value="" />
        <input type="hidden" id="hdnStatusID" runat="server" value="0" />
        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnMenuID" runat="server" value="0" />
        <input type="hidden" id="hdnSessionID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnStudy = document.getElementById('<%=hdnStudy.ClientID %>');
    var objrdoAll = document.getElementById('<%=rdoAll.ClientID %>');
    var objrdoSchedule = document.getElementById('<%=rdoSchedule.ClientID %>');
    var objrdoPrelim = document.getElementById('<%=rdoPrelim.ClientID %>');
    var objrdoFinal = document.getElementById('<%=rdoFinal.ClientID %>');
    var objhdnStatusID = document.getElementById('<%=hdnStatusID.ClientID %>');
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnMenuID = document.getElementById('<%=hdnMenuID.ClientID %>');
    var objhdnSessionID = document.getElementById('<%=hdnSessionID.ClientID %>');
    var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');
    var strForm = "VRSRadAssignMultiDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="scripts/RadAssignMultiDlg.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
