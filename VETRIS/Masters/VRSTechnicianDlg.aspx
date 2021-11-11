<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSTechnicianDlg.aspx.cs" Inherits="VETRIS.Masters.VRSTechnicianDlg" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Technicians Details</h2>
                        </div>
                        <div class="col-sm-6 col-xs-12 text-right">
                            <button type="button" id="btnAdd1" runat="server" class="btn btn-custon-four btn-primary" style="display: none;">
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
                                <label class="control-label" for="usermodel">First Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtFName" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Last Name<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtLName" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>

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
                                <label class="control-label" for="usermodel">Address 2</label>
                                <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">City</label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Country</label>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">State</label>
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
                                <label class="control-label" for="usermodel">Email ID<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Telephone #</label>
                                <asp:TextBox ID="txtTel" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-4 col-xs-12">
                            <div class="form-select-list">
                                <label class="control-label">Mobile #<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                             <div class="form-group1">
                                <label class="control-label">Notification Preference</label>
                            </div>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoEmail" runat="server" GroupName="grpPref" />
                                <label for="rdoEmail" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Email</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoSMS" runat="server" GroupName="grpPref" />
                                <label for="rdoSMS" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">SMS</span>
                            <div class="pull-left grid_option1 customRadio">
                                <asp:RadioButton ID="rdoBoth" runat="server" GroupName="grpPref" />
                                <label for="rdoBoth" class="label-default" style="width: auto; margin-top: 10px;"></label>
                            </div>
                            <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Both</span>
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
                                <asp:TextBox ID="txtLoginID" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Password<span class="mandatory">*</span></label>
                                <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control" MaxLength="200" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                       

                    </div>


                </div>
            </div>

            <div class="sparkline10-list mt-b-10">
                <div class="searchSection marginTP10">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="pull-left">
                                <h3 class="h3Text">Fee Details</h3>
                            </div>
                            <div class="borderSearch pull-left"></div>
                        </div>
                    </div>
                    <div class="row">
                         
                        <div class="col-sm-3 col-xs-12">
                            <div class="form-group">
                                <label class="control-label" for="usermodel">Default Fee</label>
                                <asp:TextBox ID="txtDefaultFee" runat="server" CssClass="form-control" MaxLength="10" ></asp:TextBox>
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
                            <button type="button" class="btn btn-custon-four btn-primary" style="display: none;" id="btnAdd2" runat="server">
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


        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="5" Style="display: none;"></asp:TextBox>
        <input type="hidden" id="hdnLoginUserID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnInstitutions" runat="server" value="" />
        <input type="hidden" id="hdnUsers" runat="server" value="" />
    </form>
</body>

    <script type="text/javascript">

        var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
        var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
        var objtxtFName = document.getElementById('<%=txtFName.ClientID %>');//--
        var objtxtLName = document.getElementById('<%=txtLName.ClientID %>');//--
        var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');//--
        var objtxtAddr1 = document.getElementById('<%=txtAddr1.ClientID %>');//--
        var objtxtAddr2 = document.getElementById('<%=txtAddr2.ClientID %>');//--
        var objtxtCity = document.getElementById('<%=txtCity.ClientID %>');//--
        var objtxtZip = document.getElementById('<%=txtZip.ClientID %>');//--
        var objtxtEmailID = document.getElementById('<%=txtEmailID.ClientID %>');//--
        var objtxtTel = document.getElementById('<%=txtTel.ClientID %>');//--
        var objtxtMobile = document.getElementById('<%=txtMobile.ClientID %>');//--
        var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');//--
        var objddlState = document.getElementById('<%=ddlState.ClientID %>');//--
        var objhdnInstitutions = document.getElementById('<%=hdnInstitutions.ClientID %>');//-
        var objhdnUsers = document.getElementById('<%=hdnUsers.ClientID %>');//-
        var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');//---
        var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');//---
        var objrdoEmail = document.getElementById('<%=rdoEmail.ClientID %>');
        var objrdoSMS = document.getElementById('<%=rdoSMS.ClientID %>');
        var objrdoBoth = document.getElementById('<%=rdoBoth.ClientID %>');
        var objhdnLoginUserID = document.getElementById('<%=hdnLoginUserID.ClientID %>');//---
        var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>');//---
        var objtxtPwd = document.getElementById('<%=txtPwd.ClientID %>');//---

        var objtxtDefaultFee = document.getElementById('<%=txtDefaultFee.ClientID %>');//---

        var strForm = "VRSTechnicianDlg";

    </script>
    <script src="../scripts/custome-javascript.js"></script>
    <script src="../scripts/AppPages.js"></script>
    <script src="scripts/TechnicianDlg.js?01102019"></script>
</html>