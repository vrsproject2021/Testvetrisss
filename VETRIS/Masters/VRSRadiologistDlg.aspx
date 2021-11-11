<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRadiologistDlg.aspx.cs" Inherits="VETRIS.Masters.VRSRadiologistDlg" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
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
    <%--<link href="../css/style.css" rel="stylesheet" />--%>
    <link href="../css/ColorPicker.css" rel="stylesheet" />
    <%--<link href="../css/grid_style.css" rel="stylesheet" />--%>
    <link id="lnkSTYLE" runat="server" href="../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkGRID" runat="server" href="../css/grid_style.css" rel="stylesheet" type="text/css" />


    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="ckeditor/ckeditor.js"></script>
    <script src="scripts/RadiologistDlgHdr.js?v=<%=DateTime.Now.Ticks%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="sparklineHeader mt-b-10 marginTP10">
                <div class="sparkline10-hd">
                    <div class="row">
                        <div class="col-sm-6 col-xs-12">
                            <h2>Radiologists Details</h2>
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
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
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
                                <div class="col-sm-2 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Credentials</label>
                                        <asp:TextBox ID="txtCredentials" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-sm-2 col-xs-12">
                                    <div class="form-group1">
                                        <label class="control-label">Identification Color<span class="mandatory">*</span></label>
                                    </div>
                                    <div style="float: left;">
                                        <ComponentArt:Menu
                                            ID="MenuColor"
                                            Orientation="Horizontal"
                                            ExpandDelay="100"
                                            EnableViewState="false"
                                            ImagesBaseUrl="../images/menu/"
                                            runat="server">
                                            <Items>
                                                <ComponentArt:MenuItem DefaultSubGroupExpandOffsetY="2" DefaultSubGroupExpandDirection="BelowLeft" Look-ImageUrl="menuitem.gif" Look-HoverImageUrl="menuitem_over.gif">
                                                    <ComponentArt:MenuItem Look-CssClass="colorpickeritem" ServerTemplateId="ColorPicker" />
                                                </ComponentArt:MenuItem>
                                            </Items>
                                            <ServerTemplates>
                                                <ComponentArt:NavigationCustomTemplate ID="ColorPicker">
                                                    <Template>
                                                        <ComponentArt:ColorPicker
                                                            ID="ColorPicker1"
                                                            GridColumns="12"
                                                            Mode="Grid"
                                                            CssClass="colorpicker"
                                                            ColorCssClass="swatch"
                                                            ColorHoverCssClass="swatch-h"
                                                            ColorActiveCssClass="swatch-a"
                                                            ColorGridCssClass="swatches"
                                                            GridCellSpacing="2"
                                                            runat="server">
                                                            <Colors>
                                                                <ComponentArt:ColorPickerColor Hex="#ff0000" />
                                                                <ComponentArt:ColorPickerColor Hex="#ff8f00" />
                                                                <ComponentArt:ColorPickerColor Hex="#ffff00" />
                                                                <ComponentArt:ColorPickerColor Hex="#c5ff00" />
                                                                <ComponentArt:ColorPickerColor Hex="#00ff00" />
                                                                <ComponentArt:ColorPickerColor Hex="#00ff98" />
                                                                <ComponentArt:ColorPickerColor Hex="#00ffff" />
                                                                <ComponentArt:ColorPickerColor Hex="#00c1ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#0000ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#9400ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#ff00ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#ff0058" />

                                                                <ComponentArt:ColorPickerColor Hex="#e51919" />
                                                                <ComponentArt:ColorPickerColor Hex="#e58b19" />
                                                                <ComponentArt:ColorPickerColor Hex="#e4e519" />
                                                                <ComponentArt:ColorPickerColor Hex="#b6e519" />
                                                                <ComponentArt:ColorPickerColor Hex="#19e519" />
                                                                <ComponentArt:ColorPickerColor Hex="#19e592" />
                                                                <ComponentArt:ColorPickerColor Hex="#19e4e5" />
                                                                <ComponentArt:ColorPickerColor Hex="#19b3e5" />
                                                                <ComponentArt:ColorPickerColor Hex="#1919e5" />
                                                                <ComponentArt:ColorPickerColor Hex="#8f19e5" />
                                                                <ComponentArt:ColorPickerColor Hex="#e519e4" />
                                                                <ComponentArt:ColorPickerColor Hex="#e5195f" />

                                                                <ComponentArt:ColorPickerColor Hex="#a33636" />
                                                                <ComponentArt:ColorPickerColor Hex="#a37336" />
                                                                <ComponentArt:ColorPickerColor Hex="#a3a336" />
                                                                <ComponentArt:ColorPickerColor Hex="#8aa336" />
                                                                <ComponentArt:ColorPickerColor Hex="#36a336" />
                                                                <ComponentArt:ColorPickerColor Hex="#36a376" />
                                                                <ComponentArt:ColorPickerColor Hex="#36a3a3" />
                                                                <ComponentArt:ColorPickerColor Hex="#3688a3" />
                                                                <ComponentArt:ColorPickerColor Hex="#3636a3" />
                                                                <ComponentArt:ColorPickerColor Hex="#7536a3" />
                                                                <ComponentArt:ColorPickerColor Hex="#a336a3" />
                                                                <ComponentArt:ColorPickerColor Hex="#a3365c" />

                                                                <ComponentArt:ColorPickerColor Hex="#602020" />
                                                                <ComponentArt:ColorPickerColor Hex="#604420" />
                                                                <ComponentArt:ColorPickerColor Hex="#606020" />
                                                                <ComponentArt:ColorPickerColor Hex="#516020" />
                                                                <ComponentArt:ColorPickerColor Hex="#206020" />
                                                                <ComponentArt:ColorPickerColor Hex="#206046" />
                                                                <ComponentArt:ColorPickerColor Hex="#206060" />
                                                                <ComponentArt:ColorPickerColor Hex="#205060" />
                                                                <ComponentArt:ColorPickerColor Hex="#202060" />
                                                                <ComponentArt:ColorPickerColor Hex="#452060" />
                                                                <ComponentArt:ColorPickerColor Hex="#602060" />
                                                                <ComponentArt:ColorPickerColor Hex="#602036" />

                                                                <ComponentArt:ColorPickerColor Hex="#ff9999" />
                                                                <ComponentArt:ColorPickerColor Hex="#ffd299" />
                                                                <ComponentArt:ColorPickerColor Hex="#ffff99" />
                                                                <ComponentArt:ColorPickerColor Hex="#e8ff99" />
                                                                <ComponentArt:ColorPickerColor Hex="#99ff99" />
                                                                <ComponentArt:ColorPickerColor Hex="#99ffd6" />
                                                                <ComponentArt:ColorPickerColor Hex="#99ffff" />
                                                                <ComponentArt:ColorPickerColor Hex="#99e6ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#9999ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#d499ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#ff99ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#ff99bc" />

                                                                <ComponentArt:ColorPickerColor Hex="#ffe0e0" />
                                                                <ComponentArt:ColorPickerColor Hex="#fff1e0" />
                                                                <ComponentArt:ColorPickerColor Hex="#ffffe0" />
                                                                <ComponentArt:ColorPickerColor Hex="#f8ffe0" />
                                                                <ComponentArt:ColorPickerColor Hex="#e0ffe0" />
                                                                <ComponentArt:ColorPickerColor Hex="#e0fff3" />
                                                                <ComponentArt:ColorPickerColor Hex="#e0ffff" />
                                                                <ComponentArt:ColorPickerColor Hex="#e0f7ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#e0e0ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#f2e0ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#ffe0ff" />
                                                                <ComponentArt:ColorPickerColor Hex="#ffe0eb" />

                                                                <ComponentArt:ColorPickerColor Hex="#ffffff" />
                                                                <ComponentArt:ColorPickerColor Hex="#e2e2e2" />
                                                                <ComponentArt:ColorPickerColor Hex="#d7d7d7" />
                                                                <ComponentArt:ColorPickerColor Hex="#cdcdcd" />
                                                                <ComponentArt:ColorPickerColor Hex="#b7b7b7" />
                                                                <ComponentArt:ColorPickerColor Hex="#898989" />
                                                                <ComponentArt:ColorPickerColor Hex="#707070" />
                                                                <ComponentArt:ColorPickerColor Hex="#555555" />
                                                                <ComponentArt:ColorPickerColor Hex="#464646" />
                                                                <ComponentArt:ColorPickerColor Hex="#252525" />
                                                                <ComponentArt:ColorPickerColor Hex="#111111" />
                                                                <ComponentArt:ColorPickerColor Hex="#000000" />
                                                            </Colors>
                                                            <ClientEvents>
                                                                <ColorChanged EventHandler="color_changed" />
                                                            </ClientEvents>
                                                        </ComponentArt:ColorPicker>
                                                    </Template>
                                                </ComponentArt:NavigationCustomTemplate>
                                            </ServerTemplates>
                                        </ComponentArt:Menu>
                                    </div>
                                    <div style="float: left; margin-top: 5px; margin-left: 5px;">
                                        <asp:Label ID="lblColor" runat="server"></asp:Label>
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
                                <div class="col-sm-1 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="zip">Zip</label>
                                        <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-sm-3 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label">Time Zone<span class="mandatory">*</span></label>
                                        <asp:DropDownList ID="ddlTimeZone" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-sm-2 col-xs-12">
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
                                <div class="col-sm-2 col-xs-12">
                                    <div class="form-group1">
                                        <label class="control-label">Require Transcription ?</label>
                                    </div>
                                    <div class="pull-left grid_option1 customRadio">
                                        <asp:RadioButton ID="rdoTransYes" runat="server" GroupName="grpTrans" />
                                        <label for="rdoTransYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                                    <div class="pull-left grid_option1 customRadio">
                                        <asp:RadioButton ID="rdoTransNo" runat="server" GroupName="grpTrans" />
                                        <label for="rdoTransNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                                </div>

                                <div class="col-sm-4 col-xs-12">
                                    <div class="form-group1">
                                        <label class="control-label">Account Group <span class="mandatory">*</span></label>
                                        <asp:DropDownList ID="ddlAcctGroup" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>

                                    </div>

                                </div>
                                <div class="col-sm-2 col-xs-12">
                                    <div class="form-group1">
                                        <label class="control-label">Assign Study By Default?</label>
                                    </div>
                                    <div class="pull-left grid_option1 customRadio">
                                        <asp:RadioButton ID="rdoAsnYes" runat="server" GroupName="grpAsn" />
                                        <label for="rdoAsnYes" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Yes</span>
                                    <div class="pull-left grid_option1 customRadio">
                                        <asp:RadioButton ID="rdoAsnNo" runat="server" GroupName="grpAsn" />
                                        <label for="rdoAsnNo" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">No</span>
                                </div>
                                <div class="col-sm-2 col-xs-12">
                                    <div class="form-group1">
                                        <label class="control-label">Active ?</label>
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
                                <div class="col-sm-8 col-xs-12">&nbsp;</div>
                                <div class="col-sm-4 col-xs-12">
                                    <span class="HelpText">(for Merged/Comaprison studies)</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
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
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-4 col-xs-12">
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
                                <div class="col-sm-6 col-xs-12">
                                    <div class="form-select-list">
                                        <label class="control-label">Login ID<span class="mandatory">*</span></label>
                                        <asp:TextBox ID="txtLoginID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">Password<span class="mandatory">*</span></label>
                                        <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control" MaxLength="200" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">PACS Credentials
                                        <img src="../images/info.png" alt="" title="Credentials will not be auto updated into PACS ; It will be a manual process to ensure that this credential is identical to the one set in PACS" />
                                        </h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-xs-12">
                                    <div class="form-select-list">
                                        <label class="control-label">PACS Login ID<span class="mandatory">*</span></label>
                                        <asp:TextBox ID="txtPACSLoginID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <label class="control-label" for="usermodel">PACS Password<span class="mandatory">*</span></label>
                                        <asp:TextBox ID="txtPACSPwd" runat="server" CssClass="form-control" MaxLength="200" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Schedule</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="form-group1">
                                        <label class="control-label">Can view schedule of</label>
                                    </div>
                                    <div class="pull-left grid_option1 customRadio" style="margin-bottom: 7px;">
                                        <asp:RadioButton ID="rdoSVA" runat="server" GroupName="grpSV" />
                                        <label for="rdoSVA" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">All Radiologists</span>
                                    <div class="pull-left grid_option1 customRadio" style="margin-bottom: 7px;">
                                        <asp:RadioButton ID="rdoSVO" runat="server" GroupName="grpSV" />
                                        <label for="rdoSVO" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                    </div>
                                    <span class="pull-left" style="padding-left: 0px; padding-top: 5px; padding-right: 5px;">Only Own</span>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Signature Details</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12">

                                            <CKEditor:CKEditorControl ID="CKEditor1" runat="server" DisableNativeSpellChecker="false" RemovePlugins="scayt,contextmenu"
                                                Toolbar="Source|-|NewPage|Preview|-|Templates
                                                                                Cut|Copy|Paste|PasteText|PasteFromWord|-|Print
                                                                                Undo|Redo|-|Find|Replace|-|SelectAll|RemoveFormat
                                                                                Form|Checkbox|Radio|TextField|Textarea|Select|Button|ImageButton|HiddenField
                                                                                /
                                                                                Bold|Italic|Underline|Strike|-|Subscript|Superscript
                                                                                NumberedList|BulletedList|-|Outdent|Indent|Blockquote|CreateDiv
                                                                                JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock
                                                                                BidiLtr|BidiRtl
                                                                                Link|Unlink|Anchor
                                                                                Image|Table|HorizontalRule|Smiley|SpecialChar|PageBreak|Iframe
                                                                                /
                                                                                Styles|Format|Font|FontSize
                                                                                TextColor|BGColor|Paragraph
                                                                                Maximize|ShowBlocks"
                                                Height="100px" Width="100%">
                                            </CKEditor:CKEditorControl>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Link Modality</h3>
                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="table-responsive">
                                        <ComponentArt:CallBack ID="CallBackModality" runat="server" OnCallback="CallBackModality_Callback">
                                            <Content>
                                                <ComponentArt:Grid
                                                    ID="grdModality"
                                                    CssClass="Grid"
                                                    DataAreaCssClass="GridData4"
                                                    SearchOnKeyPress="true"
                                                    EnableViewState="true"
                                                    ShowSearchBox="true"
                                                    SearchBoxPosition="TopLeft"
                                                    SearchTextCssClass="GridHeaderText" AutoFocusSearchBox="false"
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
                                                    GroupingNotificationPosition="TopRight">

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
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdModality.get_recordOffset()) % 2) > 0" RowCssClass="AltRow" SelectedRowCssClass="AltRow" />
                                                                <ComponentArt:GridConditionalFormat ClientFilter="((DataItem.get_index() + grdModality.get_recordOffset()) % 2) == 0" RowCssClass="Row" SelectedRowCssClass="Row" />
                                                            </ConditionalFormats>
                                                            <Columns>
                                                                <ComponentArt:GridColumn DataField="srl_no" Align="left" HeadingText="#" AllowGrouping="false" Width="20" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="modality_id" Align="left" HeadingText="modality_id" AllowGrouping="false" Visible="false" />
                                                                <ComponentArt:GridColumn DataField="sel" Align="center" AllowGrouping="false" AllowSorting="false" DataCellClientTemplateId="SEL" HeadingCellClientTemplateId="SELHDR" HeadingText="Select" FixedWidth="True" Width="40" />
                                                                <ComponentArt:GridColumn DataField="modality_name" Align="left" HeadingText="Modality" AllowGrouping="false" Width="190" />
                                                                <ComponentArt:GridColumn DataField="prelim_fee" Align="right" HeadingText="Preliminary Fee ($)" AllowGrouping="false" Width="110" DataCellClientTemplateId="PRELIM" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="final_fee" Align="right" HeadingText="Final Fee ($)" AllowGrouping="false" Width="90" DataCellClientTemplateId="FINAL" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="addl_STAT_fee" Align="right" HeadingText="Additional Fee - STAT Prelim. ($)" AllowGrouping="false" Width="190" DataCellClientTemplateId="ADDL" FixedWidth="true" />
                                                                <ComponentArt:GridColumn DataField="work_unit" Align="right" HeadingText="Work Units" AllowGrouping="false" Width="110" DataCellClientTemplateId="WU" FixedWidth="true" />
                                                            </Columns>

                                                        </ComponentArt:GridLevel>
                                                    </Levels>

                                                    <ClientEvents>
                                                        <RenderComplete EventHandler="grdModality_onRenderComplete" />
                                                    </ClientEvents>
                                                    <ClientTemplates>
                                                        <ComponentArt:ClientTemplate ID="SELHDR">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSelHdr" style="width: 18px; height: 18px;" onclick="javascript:chkSelHdr_OnClick();" />
                                                                <label for="chkSelHdr" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="SEL">
                                                            <div class="grid_option">
                                                                <input type="checkbox" id="chkSel_## DataItem.GetMember('srl_no').Value ##" style="width: 18px; height: 18px;" onclick="javascript: chkSel_OnClick('## DataItem.GetMember('srl_no').Value ##');" />
                                                                <label for="chkSel_## DataItem.GetMember('srl_no').Value ##" class="label-default" style="width: auto; margin-top: 10px;"></label>
                                                            </div>
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="PRELIM">
                                                            <input type="text" id="txtPrelim_## DataItem.GetMember('srl_no').Value ##" style="width: 80%; text-align: right;" class="GridTextBox" readonly="true" value="## DataItem.GetMember('prelim_fee').Value ##" onfocus="javascript:this.select();" onkeypress="javascript:parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" onchange="javascript:txtPrelim_OnChange('## DataItem.GetMember('srl_no').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="FINAL">
                                                            <input type="text" id="txtFinal_## DataItem.GetMember('srl_no').Value ##" style="width: 80%; text-align: right;" class="GridTextBox" readonly="true" value="## DataItem.GetMember('final_fee').Value ##" onfocus="javascript:this.select();" onkeypress="javascript:parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" onchange="javascript:txtFinal_OnChange('## DataItem.GetMember('srl_no').Value ##');" />
                                                        </ComponentArt:ClientTemplate>
                                                        <ComponentArt:ClientTemplate ID="ADDL">
                                                            <input type="text" id="txtAddl_## DataItem.GetMember('srl_no').Value ##" style="width: 60%; text-align: right;" class="GridTextBox" readonly="true" value="## DataItem.GetMember('addl_STAT_fee').Value ##" onfocus="javascript:this.select();" onkeypress="javascript:parent.CheckDecimal(event);" onblur="javascript:ResetValueDecimal(this);" onchange="javascript:txtAddl_OnChange('## DataItem.GetMember('srl_no').Value ##');" />
                                                        </ComponentArt:ClientTemplate>

                                                        <ComponentArt:ClientTemplate ID="WU">
                                                            <input type="text" id="txtWU_## DataItem.GetMember('srl_no').Value ##" style="width: 80%; text-align: right;" class="GridTextBox" readonly="true" value="## DataItem.GetMember('work_unit').Value ##" onfocus="javascript:this.select();" onkeypress="javascript:return parent.CheckInteger(event);" onblur="javascript:ResetValueInteger(this);" onchange="javascript:txtWU_OnChange('## DataItem.GetMember('srl_no').Value ##');" />
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
                                                <CallbackComplete EventHandler="grdModality_onCallbackComplete" />
                                            </ClientEvents>
                                        </ComponentArt:CallBack>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-11 col-xs-11" style="margin-top: 10px;">
                                    <div class="pull-right" style="margin-top: 2px;">
                                        Maximum work units per hour

                                    </div>
                                </div>
                                <div class="col-sm-1 col-xs-1" style="margin-top: 7px;">
                                    <asp:TextBox ID="txtMaxWU" runat="server" CssClass="form-control" Width="95%" Style="text-align: right;"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-sm-12 col-xs-12">
                        <div class="searchSection marginTP10">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="pull-left">
                                        <h3 class="h3Text">Notes</h3>

                                    </div>
                                    <div class="borderSearch pull-left"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="form-group">

                                        <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Height="200px"></asp:TextBox>

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
        <input type="hidden" id="hdnColor" runat="server" value="#FFFFFF" />
    </form>
</body>

<script type="text/javascript">

    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtFName = document.getElementById('<%=txtFName.ClientID %>');//--
    var objtxtLName = document.getElementById('<%=txtLName.ClientID %>');//--
    var objtxtCredentials = document.getElementById('<%=txtCredentials.ClientID %>');//// Added on 12th SEP 2019 @BK
    var objtxtCode = document.getElementById('<%=txtCode.ClientID %>');//--
    var objtxtAddr1 = document.getElementById('<%=txtAddr1.ClientID %>');//--
    var objtxtAddr2 = document.getElementById('<%=txtAddr2.ClientID %>');//--
    var objtxtCity = document.getElementById('<%=txtCity.ClientID %>');//--
    var objtxtZip = document.getElementById('<%=txtZip.ClientID %>');//--
    var objddlTimeZone = document.getElementById('<%=ddlTimeZone.ClientID %>');
    var objlblColor = document.getElementById('<%=lblColor.ClientID %>');
    var objhdnColor = document.getElementById('<%=hdnColor.ClientID %>');
    var objtxtEmailID = document.getElementById('<%=txtEmailID.ClientID %>');//--
    var objtxtTel = document.getElementById('<%=txtTel.ClientID %>');//--
    var objtxtMobile = document.getElementById('<%=txtMobile.ClientID %>');//--
    var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');//--
    var objddlState = document.getElementById('<%=ddlState.ClientID %>');//--
    var objhdnInstitutions = document.getElementById('<%=hdnInstitutions.ClientID %>');//-
    var objhdnUsers = document.getElementById('<%=hdnUsers.ClientID %>');//-
    var objrdoStatYes = document.getElementById('<%=rdoStatYes.ClientID %>');//---
    var objddlAcctGroup = document.getElementById('<%=ddlAcctGroup.ClientID %>');//---
    var objrdoStatNo = document.getElementById('<%=rdoStatNo.ClientID %>');//---
    var objrdoEmail= document.getElementById('<%=rdoEmail.ClientID %>');
    var objrdoSMS= document.getElementById('<%=rdoSMS.ClientID %>');
    var objrdoBoth= document.getElementById('<%=rdoBoth.ClientID %>');
    var objrdoSVA= document.getElementById('<%=rdoSVA.ClientID %>');
    var objrdoSVO = document.getElementById('<%=rdoSVO.ClientID %>');
    var objrdoTransYes= document.getElementById('<%=rdoTransYes.ClientID %>');
    var objrdoTransNo = document.getElementById('<%=rdoTransNo.ClientID %>');
    var objrdoAsnYes= document.getElementById('<%=rdoAsnYes.ClientID %>');
    var objrdoAsnNo = document.getElementById('<%=rdoAsnNo.ClientID %>');
    var objhdnLoginUserID = document.getElementById('<%=hdnLoginUserID.ClientID %>');//---
    var objtxtLoginID = document.getElementById('<%=txtLoginID.ClientID %>');//---
    var objtxtPwd = document.getElementById('<%=txtPwd.ClientID %>');//---
    var objtxtPACSLoginID = document.getElementById('<%=txtPACSLoginID.ClientID %>');
    var objtxtPACSPwd = document.getElementById('<%=txtPACSPwd.ClientID %>');
    var objtxtNotes = document.getElementById('<%=txtNotes.ClientID %>');//--
    var objtxtMaxWU = document.getElementById('<%=txtMaxWU.ClientID %>');
    var strForm = "VRSRadiologistDlg";

</script>
<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/RadiologistDlg.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
