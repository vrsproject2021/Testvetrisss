<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSUserDlg.aspx.cs" Inherits="VETRIS.Settings.VRSUserDlg" %>

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
    <link href="../css/theme.css" rel="stylesheet" />--%>

    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkTHEME" runat="server" href="../css/theme.css" rel="stylesheet"  type="text/css"/>
    <link id="lnkTV" runat="server" href="../css/treeStyle.css" rel="stylesheet"  type="text/css"/>

    <%--<link href="../css/treeStyle.css" rel="stylesheet" />--%>
    <link href="../css/menuStyle.css" rel="stylesheet" />


    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/UserDlgHdr.js?25062020"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>User Details</h2>
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
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Code<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">User Role<span class="mandatory">*</span></label>
                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
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
                </div>

                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Institution/Billing Account</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Institution Name</label>
                                <asp:TextBox ID="txtInstName" runat="server" CssClass="form-control" MaxLength="100" ReadOnly="true"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Billing Account Name</label>
                                <asp:TextBox ID="txtBAName" runat="server" CssClass="form-control" MaxLength="100" ReadOnly="true"></asp:TextBox>

                            </div>
                        </div>

                    </div>


                </div>

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
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Email ID <span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Contact #</label>
                                <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                            </div>
                        </div>

                    </div>


                </div>

                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Login Credentials</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Login ID<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtLoginID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control" MaxLength="20" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">PACS User ID<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtPACSUserID" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">PACS User Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtPACSPwd" runat="server" CssClass="form-control" MaxLength="20" TextMode="Password" ></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Access Rights</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div style="margin-left: 5px; margin-bottom: 10px; padding: 5px; border: solid 1px #bbb;">

                                <div class="row">
                                    <div class="col-sm-3 col-xs-12">
                                        <div class="form-group1">
                                            <label class="control-label">Allow Manual Submission?</label>
                                        </div>
                                        <div class="pull-left grid_option1 customRadio">
                                            <asp:RadioButton ID="rdoMSYes" runat="server" GroupName="grpMS" />
                                            <label for="rdoMSYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                                        <div class="pull-left grid_option1 customRadio">
                                            <asp:RadioButton ID="rdoMSNo" runat="server" GroupName="grpMS" />
                                            <label for="rdoMSNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                                    </div>
                                    <div class="col-sm-3 col-xs-12">
                                        <div class="form-group1">
                                            <label class="control-label">Allow Dashboard Viewing?</label>
                                        </div>
                                        <div class="pull-left grid_option1 customRadio">
                                            <asp:RadioButton ID="rdoDBYes" runat="server" GroupName="grpDB" />
                                            <label for="rdoDBYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                                        <div class="pull-left grid_option1 customRadio">
                                            <asp:RadioButton ID="rdoDBNo" runat="server" GroupName="grpDB" />
                                            <label for="rdoDBNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                        </div>
                                        <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="row marginMobileTP10">
                        <ComponentArt:CallBack ID="CallBackRights" runat="server" OnCallback="CallBackRights_Callback">
                            <Content>
                                <div id="divRights" runat="server" style="width: 97%; height: 350px; overflow: auto; margin-left: 20px; padding-left: 10px; border: solid 1px #bbb;">
                                    <ComponentArt:TreeView ID="tvRights"
                                        DragAndDropEnabled="false"
                                        DragAndDropAcrossTreesEnabled="false"
                                        NodeEditingEnabled="false"
                                        AutoTheming="true" CssClass="TreeView4" EnableTheming="True"
                                        MultipleSelectEnabled="False" AutoScroll="False" ShowLines="True"
                                        runat="server" Width="99%">
                                        <ClientEvents>

                                            <NodeCheckChange EventHandler="tvRights_onNodeCheckChanged" />
                                        </ClientEvents>
                                    </ComponentArt:TreeView>
                                </div>
                            </Content>
                            <LoadingPanelClientTemplate>
                                <table style="height: 350px; width: 100%;" border="0">
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
                                <CallbackComplete EventHandler="tvRights_onCallbackComplete" />
                            </ClientEvents>
                        </ComponentArt:CallBack>
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

        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnCF" runat="server" value="" />
    </form>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objhdnCF = document.getElementById('<%=hdnCF.ClientID %>');
    var objtxtName = document.getElementById('<%=txtName.ClientID %>');//--
    var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');//--
    var objtxtEmailID = document.getElementById('<%=txtEmailID.ClientID %>');//--
    var objtxtContactNo = document.getElementById('<%=txtContactNo.ClientID %>');//--
    var objddlRole = document.getElementById('<%=ddlRole.ClientID %>');//--
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>')
    var objtxtPwd = document.getElementById('<%=txtPwd.ClientID %>')
    var objtxtPACSUserID = document.getElementById('<%=txtPACSUserID.ClientID %>');//--
    var objtxtPACSPwd = document.getElementById('<%=txtPACSPwd.ClientID %>');//--
    var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');//---
    var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');//---
    var objrdoMSYes = document.getElementById('<%=rdoMSYes.ClientID %>');//---
    var objrdoMSNo = document.getElementById('<%=rdoMSNo.ClientID %>');
    var objrdoDBYes = document.getElementById('<%=rdoDBYes.ClientID %>');//---
    var objrdoDBNo = document.getElementById('<%=rdoDBNo.ClientID %>');
    var strForm = "VRSUserDlg";
</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js?25042021"></script>
<script src="scripts/UserDlg.js?28052021"></script>
</html>
