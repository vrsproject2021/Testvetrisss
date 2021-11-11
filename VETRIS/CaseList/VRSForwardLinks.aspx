<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSForwardLinks.aspx.cs" Inherits="VETRIS.CaseList.VRSForwardLinks" %>

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
   <%-- <link href="../css/style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Forward Report/Image Link(s)</h2>
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-success" id="btnSend1" runat="server">
                                <i class="fa fa-paper-plane-o" aria-hidden="true"></i>&nbsp;Send       
                            </button>
                            <button type="button" class="btn btn-danger" id="btnClose1" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>
            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="optSwitch pull-left" style="margin-left: 2%; margin-bottom: 20px;">
                        <asp:RadioButton ID="rdoEmail" runat="server" Checked="true" GroupName="grpVia" />
                        <label for="rdoEmail" class="label-default"></label>
                    </div>
                    <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Via Email</div>
                    <div class="optSwitch pull-left" style="margin-left: 2%; margin-bottom: 20px;">
                        <asp:RadioButton ID="rdoSMS" runat="server" GroupName="grpVia" />
                        <label for="rdoSMS" class="label-default"></label>
                    </div>
                    <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Via SMS</div>
                    <div class="optSwitch pull-left" style="margin-left: 2%; margin-bottom: 20px;">
                        <asp:RadioButton ID="rdoFax" runat="server" GroupName="grpVia" />
                        <label for="rdoFax" class="label-default"></label>
                    </div>
                    <div class="pull-left" style="margin-top: 10px; margin-left: 20px;">Via Fax</div>
                    <div class="clearfix"></div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="row">
                    <div class="pull-left" style="margin-top: 10px; margin-left: 2%;">Insert</div>
                    <div class="pull-left" style="margin-top: 10px;margin-left:2%;">
                         
                         <asp:CheckBox ID="chkRpt" runat="server" Checked="true" />
                         <label for="chkRpt" class="label-default"></label>
                       
                    </div>
                    <div class="pull-left" style="margin-top: 12px; margin-left:2px;">Report Link</div>
                    <div class="pull-left" style="margin-top: 10px;margin-left:2%;">
                         <asp:CheckBox ID="chkImg" runat="server" Checked="true"/>
                         <label for="chkImg" class="label-default"></label>
                    </div>
                   
                    <div class="pull-left" style="margin-top: 12px; margin-left:2px;">Image Link</div>
                    <div class="clearfix"></div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10" id="divEmail" style="display:block;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            &nbsp;
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                           <span class="HelpText" style="font-weight:bold;">
                                               For multiple receipients, specify the multiple email ids seperated by semi colon (;)
                                           </span>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            To <span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Cc 
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtCC" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Subject 
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Preview
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5" style="height:225px;overflow:auto;">
                                           <asp:Label ID="lblEmailPrev" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Additonal Text 
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtBody" runat="server" CssClass="form-control" TextMode="MultiLine" Height="50px" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Attachment
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginTP5">
                                            <asp:Label ID="lblAttachment" runat="server" Font-Bold="true" style="cursor:pointer;"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10" id="divSMS" style="display:none;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            &nbsp;
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                           <span class="HelpText" style="font-weight:bold;">
                                               For multiple receipients, specify the multiple mobile numbers seperated by semi colon (;).Prefix number(s) with country code
                                           </span>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Mobile Number(s)<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Preview
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5" style="height:120px;overflow:auto;">
                                           <asp:Label ID="lblSMSPrev" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparkline10-list mt-b-10" id="divFax" style="display:none;">
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-12 col-xs-12 marginTP5">
                                            &nbsp;
                                        </div>
                                       
                                    </div>
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            Send To Fax Number<span class="mandatory">*</span>
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginMobileTP5">
                                            <asp:TextBox ID="txtFaxNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                        </div>
                                    </div>
                                    
                                    <div class="row" style="margin-top: 5px;">
                                        <div class="col-sm-2 col-xs-12 marginTP5">
                                            File To Fax
                                        </div>
                                        <div class="col-sm-10 col-xs-12 marginTP5" style="height:120px;overflow:auto;">
                                           <asp:Label ID="lblFileName" runat="server" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="sparklineHeader marginTP10" id="divMsg" style="display: none;">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <asp:Label ID="lblMsg" runat="server">&nbsp;</asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="sparklineHeader marginTP10">
                <div class="sparkline10-hd">
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-sm-6 col-xs-12">
                            &nbsp;
                        </div>

                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" class="btn btn-success" id="btnSend2" runat="server" style="display: inline;">
                                <i class="fa fa-paper-plane-o" aria-hidden="true"></i>&nbsp;Send       
                            </button>
                            <button type="button" class="btn btn-danger" id="btnClose2" runat="server">
                                <i class="fa fa-times" aria-hidden="true"></i>&nbsp;Close   
                            </button>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnRootDir" runat="server" value="" />
        <input type="hidden" id="hdnUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnDivider" runat="server" value="0" />
        <input type="hidden" id="hdnStudyID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnFmt" runat="server" value="" />

        <input type="hidden" id="hdnMAILSVRNAME" runat="server" value="" />
        <input type="hidden" id="hdnMAILSVRPORT" runat="server" value="0" />
        <input type="hidden" id="hdnMAILSVRUSRCODE" runat="server" value="" />
        <input type="hidden" id="hdnMAILSVRUSRPWD" runat="server" value="" />
        <input type="hidden" id="hdnMAILSSLENABLED" runat="server" value="" />
        <input type="hidden" id="hdnSMSACCTSID" runat="server" value="" />
        <input type="hidden" id="hdnSMSAUTHTKNNO" runat="server" value="" />
        <input type="hidden" id="hdnSMSSENDERNO" runat="server" value="" />
        <input type="hidden" id="hdnVRSPACSLINKURL" runat="server" value="" />

        <input type="hidden" id="hdnFAXENABLE" runat="server" value="" />
        <input type="hidden" id="hdnFAXAPIURL" runat="server" value="" />
        <input type="hidden" id="hdnFAXAUTHUSERID" runat="server" value="" />
        <input type="hidden" id="hdnFAXAUTHPWD" runat="server" value="" />
        <input type="hidden" id="hdnFAXCSID" runat="server" value="" />
        <input type="hidden" id="hdnFAXREFTEXT" runat="server" value="" />
        <input type="hidden" id="hdnFAXREPADDR" runat="server" value="" />
        <input type="hidden" id="hdnFAXCONTACT" runat="server" value="" />
        <input type="hidden" id="hdnFAXRETRY" runat="server" value="0" />
        <input type="hidden" id="hdnFaxRpt" runat="server" value="" />
        <input type="hidden" id="hdnEmailFilePath" runat="server" value="" />
        <input type="hidden" id="hdnFaxFilePath" runat="server" value="" />
        <input type="hidden" id="hdnStudyStatus" runat="server" value="0" />
        <input type="hidden" id="hdnCustomRpt" runat="server" value="N" />
        <input type="hidden" id="hdnPName" runat="server" value="" />
        <input type="hidden" id="hdnRptServerPath" runat="server" value="" />
        <input type="hidden" id="hdnFileName" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnUserID = document.getElementById('<%=hdnUserID.ClientID %>');
    var objhdnDivider = document.getElementById('<%=hdnDivider.ClientID %>');
    var objhdnRootDir = document.getElementById('<%=hdnRootDir.ClientID %>');
    var objhdnStudyID = document.getElementById('<%=hdnStudyID.ClientID %>');
    var objlblMsg = document.getElementById('<%=lblMsg.ClientID %>');
    var objtxtTo = document.getElementById('<%=txtTo.ClientID %>');
    var objtxtCC = document.getElementById('<%=txtCC.ClientID %>');
    var objtxtSubject = document.getElementById('<%=txtSubject.ClientID %>');
    var objtxtBody = document.getElementById('<%=txtBody.ClientID %>');
    var objhdnMAILSVRNAME = document.getElementById('<%=hdnMAILSVRNAME.ClientID %>');
    var objhdnMAILSVRPORT = document.getElementById('<%=hdnMAILSVRPORT.ClientID %>');
    var objhdnMAILSVRUSRCODE = document.getElementById('<%=hdnMAILSVRUSRCODE.ClientID %>');
    var objhdnMAILSVRUSRPWD = document.getElementById('<%=hdnMAILSVRUSRPWD.ClientID %>');
    var objhdnMAILSSLENABLED = document.getElementById('<%=hdnMAILSSLENABLED.ClientID %>');
    var objhdnSMSACCTSID = document.getElementById('<%=hdnSMSACCTSID.ClientID %>');
    var objhdnSMSAUTHTKNNO = document.getElementById('<%=hdnSMSAUTHTKNNO.ClientID %>');
    var objhdnSMSSENDERNO = document.getElementById('<%=hdnSMSSENDERNO.ClientID %>');
    var objhdnVRSPACSLINKURL = document.getElementById('<%=hdnVRSPACSLINKURL.ClientID %>');
    var objlblAttachment = document.getElementById('<%=lblAttachment.ClientID %>');
    var objhdnEmailFilePath = document.getElementById('<%=hdnEmailFilePath.ClientID %>');

    var objhdnFAXENABLE = document.getElementById('<%=hdnFAXENABLE.ClientID %>');
    var objhdnFAXAPIURL = document.getElementById('<%=hdnFAXAPIURL.ClientID %>');
    var objhdnFAXAUTHUSERID = document.getElementById('<%=hdnFAXAUTHUSERID.ClientID %>');
    var objhdnFAXAUTHPWD = document.getElementById('<%=hdnFAXAUTHPWD.ClientID %>');
    var objhdnFAXCSID = document.getElementById('<%=hdnFAXCSID.ClientID %>');
    var objhdnFAXREFTEXT = document.getElementById('<%=hdnFAXREFTEXT.ClientID %>');
    var objhdnFAXREPADDR = document.getElementById('<%=hdnFAXREPADDR.ClientID %>');
    var objhdnFAXCONTACT = document.getElementById('<%=hdnFAXCONTACT.ClientID %>');
    var objhdnFAXRETRY = document.getElementById('<%=hdnFAXRETRY.ClientID %>');
    var objhdnFaxFilePath = document.getElementById('<%=hdnFaxFilePath.ClientID %>');
    var objhdnFaxRpt = document.getElementById('<%=hdnFaxRpt.ClientID %>');
    var objtxtFaxNo = document.getElementById('<%=txtFaxNo.ClientID %>');
    var objhdnStudyStatus = document.getElementById('<%=hdnStudyStatus.ClientID %>');
    var objhdnCustomRpt = document.getElementById('<%=hdnCustomRpt.ClientID %>');
    var objhdnPName = document.getElementById('<%=hdnPName.ClientID %>');
    var objlblFileName = document.getElementById('<%=lblFileName.ClientID %>');

    var objrdoEmail = document.getElementById('<%=rdoEmail.ClientID %>');
    var objrdoSMS = document.getElementById('<%=rdoSMS.ClientID %>');
    var objrdoFax = document.getElementById('<%=rdoFax.ClientID %>');
    var objchkRpt = document.getElementById('<%=chkRpt.ClientID %>');
    var objchkImg = document.getElementById('<%=chkImg.ClientID %>');
    var objlblEmailPrev = document.getElementById('<%=lblEmailPrev.ClientID %>');
    var objlblSMSPrev = document.getElementById('<%=lblSMSPrev.ClientID %>');
    var objtxtMobileNo = document.getElementById('<%=txtMobileNo.ClientID %>');
    var objhdnFmt = document.getElementById('<%=hdnFmt.ClientID %>');
    var objhdnRptServerPath = document.getElementById('<%=hdnRptServerPath.ClientID %>');
    var objhdnFileName = document.getElementById('<%=hdnFileName.ClientID %>');
    var strForm = "VRSForwardLinks";

</script>
<script src="scripts/ForwardLinks.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
