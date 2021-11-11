<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DCMUploader.aspx.cs" Inherits="VETRIS.RND.DCMUploader" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload DCM File(s)</title>
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />
    <link rel="stylesheet" href="../css/bootstrap.min.css" />
    <link rel="stylesheet" href="../css/font-awesome.min.css" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <link href="../css/style.css?1" rel="stylesheet" />
    <link rel="stylesheet" href="../css/custom.css" />
    <link href="../css/custome-css-style.css" rel="stylesheet" />
    <link href="../css/grid_style.css" rel="stylesheet" />
    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script type="text/javascript">
        function grdDCM_onCallbackComplete(sender, eventArgs) {
            grdDCM.Width = "99%";
            //parent.adjustFrameHeight();
            var strErr = parent.Trim(document.getElementById("hdnCBErrDCM").value);
            //if (strErr != "") {
            //    parent.PopupMessage(RootDirectory, strForm, "grdDCM_onCallbackComplete()", strErr, "true");
            //}
        }
        function grdDCM_onRenderComplete(sender, eventArgs) {
            //parent.adjustFrameHeight();
            //if (DOCADD == "N") parent.GsRetStatus = "false";
            //else parent.GsRetStatus = "true";
        }
    </script>
</head>
<body style="background-color:#fff;">
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparkline10-list mt-b-10">
                <div class="searchSection">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-8 col-xs-8">
                                    <h3 style="color: #1e77bb;">Upload DICOM File(s)</h3>
                                </div>
                                <div class="col-sm-4 col-xs-4 text-right">
                                    <button type="button" class="btn btn_grd btn-primary" title="click to upload" id="btnUpload" runat="server" style="display: none;">
                                        <i class="fa fa-upload" aria-hidden="true"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-xs-12">
                            <div class="borderSearch"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <div style="border: solid 1px #bbb;">
                                <iframe id="iframeUploadDCM" scrolling="no" style="padding: 0px; width: 100%; height: auto; background-color: white; border: none; min-height: 185px;"></iframe>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-12 marginMobileTP5">
                            <div class="table-responsive">
                                <ComponentArt:CallBack ID="CallBackDCM" runat="server" OnCallback="CallBackDCM_Callback">
                                    <Content>
                                        <ComponentArt:Grid
                                            ID="grdDCM"
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
                                                    DataKeyField="srl_no"
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
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDCM.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                        <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdDCM.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                    </ConditionalFormats>
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="30" />
                                                        <ComponentArt:GridColumn DataField="dcm_file_name" Align="left" HeadingText="DICOM File Name" AllowGrouping="false" Width="250" DataCellClientTemplateId="FILEDCM" FixedWidth="True" />
                                                        <ComponentArt:GridColumn Align="center" AllowGrouping="false" DataCellClientTemplateId="DELDCM" HeadingText=" " FixedWidth="True" Width="30" />
                                                    </Columns>

                                                </ComponentArt:GridLevel>
                                            </Levels>

                                            <ClientEvents>
                                                <RenderComplete EventHandler="grdDCM_onRenderComplete" />
                                            </ClientEvents>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="FILEDCM">
                                                    <span id="spnDCM_## DataItem.GetMember('srl_no').Value ##" title="## DataItem.GetMember('dcm_file_name').Value ##">## DataItem.GetMember('dcm_file_name').Value ##</span>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="DELDCM">
                                                    <button type="button" id="btnDelDCM_## DataItem.GetMember('srl_no').Value ##" class="btn btn-danger btn_grd" onclick="javascript:DeleteDCM('## DataItem.GetMember('srl_no').Value ##')">
                                                        <i class="fa fa-trash" aria-hidden="true"></i>

                                                    </button>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        <span id="spnERRDCM" runat="server"></span>
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
                                        <CallbackComplete EventHandler="grdDCM_onCallbackComplete" />
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

                        <div class="col-sm-12 col-xs-12 text-right">
                            <button type="button" class="btn btn-custon-four btn-success" id="btnSubmit" runat="server">
                                <i class="fa fa-thumbs-o-up edu-danger-error" aria-hidden="true"></i>&nbsp;SUBMIT
                                       
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnRecDivider" value="" runat="server" />
        <input type="hidden" id="hdnSecDivider" value="" runat="server" />
        <input type="hidden" id="hdnTempFolder" value="" runat="server" />
        <input type="hidden" id="hdnRootDirectory" runat="server" />
    </form>
</body>
<script type="text/javascript">
    var objhdnRecDivider = document.getElementById('<%=hdnRecDivider.ClientID %>');
    var objhdnSecDivider = document.getElementById('<%=hdnSecDivider.ClientID %>');
    var objhdnTempFolder = document.getElementById('<%=hdnTempFolder.ClientID %>');
    var objhdnRootDirectory = document.getElementById('<%=hdnRootDirectory.ClientID%>');
    var objiframeUploadDCM = document.getElementById("iframeUploadDCM");
    var strForm = "DCMUploader";

    objiframeUploadDCM.src = "WebForm1.aspx";
    var strRowID = "";
    var SecDivider = objhdnSecDivider.value;
    var GsText = ""; var strRootDirectory = objhdnRootDirectory.value; var strDivider = objhdnRecDivider.value; var GsLaunchURL = ""; var GsConfirmAction = "";

    function ProcessDCMUpload(ArgsRet) {
        //debugger;
        var arrFiles = new Array();
        var strExistingDtls = ""; var strNewDtls = "";
        if (ArgsRet != null) {
            parent.GsRetStatus = "true";
            arrFiles = ArgsRet[0].split(objhdnRecDivider.value);
            strExistingDtls = GetDCMGridDetails();
            for (var i = 0; i < arrFiles.length; i++) {
                if (strNewDtls != "") strNewDtls += SecDivider;
                strNewDtls += arrFiles[i];
            }
            objiframeUploadDCM.src = "WebForm1.aspx";
            CallBackDCM.callback("A", strExistingDtls, strNewDtls);
        }
    }
    function DeleteDCM(ID) {

        strRowID = ID;
        DeleteRecord();
    }
    function DeleteRecord() {
        var strExistingDtls = GetDCMGridDetails();
        CallBackDCM.callback("D", strRowID, strExistingDtls, objhdnTempFolder.value);
    }
    function GetDCMGridDetails() {
        var strDtls = "";
        var itemIndex = 0;
        var gridItem;

        while (gridItem = grdDCM.get_table().getRow(itemIndex)) {
            if (strDtls != "") strDtls = strDtls + SecDivider;
            strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[1].get_value().toString();

            itemIndex++;
        }
        return strDtls;
    }

    function btnSubmit_OnClick() {
        debugger;
        var itemIndex = 0;
        var gridItem;
        var arrFiles = new Array();
        var idx = 0;
        GsText = "Submitting Files...";
        try {
            PopupLoad();
            while (gridItem = grdDCM.get_table().getRow(itemIndex)) {

                arrFiles[idx] = gridItem.get_cells()[1].get_value().toString();
                idx = idx + 1;
                itemIndex++;
            }

            AjaxPro.timeoutPeriod = 1800000;
            DCMUploader.SubmitFiles(arrFiles, ShowProcess);
        }
        catch (expErr) {
            HideLoad();
            PopupMessage(strRootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
        }
    }
    function SubmitFiles(Result) {

        var arrRes = new Array();
        arrRes = Result.value;
        switch (arrRes[0]) {
            case "catch":
                PopupMessage(strRootDirectory, strForm, "SubmitFiles()", arrRes[1], "true");
                break;
            case "true":
                PopupMessage(strRootDirectory, strForm, "SubmitFiles()", "File(s) submitted successfully", "false");
                break;
        }

    }
    function ShowProcess(Result, MethodName) {
        GsText = "";

        var strMethod = MethodName.method;
        switch (strMethod) {
            case "SubmitFiles":
                HideLoad();
                SubmitFiles(Result);
                break;
        }
    }
    function PopupLoad() {
        var sUrl = "htmls/Loading.html";
        $('#tblProcess1').surfOverlay('ld', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
        return false;
    }
    function HideLoad() {
        closepopup('ld');
    }
    function PopupMessage(argRootDirectory, argForm, argMethod, argErrCode, argShowErr, argsText1, argsText2, argsRet) {
        if (argsText1 == null) argsText1 = ""; if (argsText2 == null) argsText2 = "";
        if (argsRet == null) argsRet = "";
        GsLaunchURL = "../Common/VRSMessages.aspx?FORM=" + argForm + "&METHOD=" + argMethod + "&ERRCODE=" + argErrCode + "&ShowErr=" + argShowErr + "&TEXT1=" + argsText1 + "&TEXT2=" + argsText2 + "&RETVAL=" + argsRet;
        var sUrl = "htmls/message.html";
        $('#tblMsg').surfOverlay('msg', { url: sUrl, zIndex: 4000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
        return false;
    }
    function HideMessage(ArgsRet) {
        $('#tblMsg').surfOverlay('msg', { zIndex: 100 });
        closepopup('msg');
        GsLaunchURL = "";

        if (ArgsRet != null) {
            if (ArgsRet == "068")
                ProcessMessage(ArgsRet);
            else if (typeof (objiframePage) != "undefined") {
                if (objiframePage.contentWindow)
                    objiframePage.contentWindow.ProcessMessage(ArgsRet);
                else
                    objiframePage.contentDocument.parentWindow.ProcessMessage(ArgsRet);
            }
        }
    }
</script>

</html>
