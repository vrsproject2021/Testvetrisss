<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSRegistration.aspx.cs" Inherits="VETRIS.Registration.VRSRegistration" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VETERINARY RADIOLOGY INFORMATION SYSTEM</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-icon" href="../images/favicon.ico" />
    <link href="https://fonts.googleapis.com/css?family=Play:400,700" rel="stylesheet" />
    <!-- Icons font CSS-->
    <link href="vendor/mdi-font/css/material-design-iconic-font.min.css" rel="stylesheet" media="all" />
    <link href="vendor/font-awesome-4.7/css/font-awesome.min.css" rel="stylesheet" media="all" />


    <!-- Main CSS-->
    <%--<link href="css/bootstrap.min.css" rel="stylesheet" media="all"/>--%>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" media="all" />

    <link href="css/main.css" rel="stylesheet" media="all" />
    <!-- Vendor CSS-->
    <link href="vendor/select2/select2.min.css" rel="stylesheet" media="all" />
    <link href="css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <style>
        td > a[disabled] {
            color: gray;
        }
    </style>
</head>
<body>
    <form id="form1" method="post" runat="server">

        <div class="page-wrapper p-t-20 p-b-100 font-robo">
            <div class="container-fluid">
                <div class="card card-1">
                    <div class="card-heading">
                        <img src="images/cta-logo-mid.png" />
                    </div>
                    <div class="card-body">
                        <div class="dotted-border">
                            <h2 class="title">Tell us about yourself...</h2>
                        </div>

                        <div class="row">
                            <div class="col-sm-6 col-xs-12">
                                <div class="input-group">
                                    <label for="">Institution Name <span>*</span></label>
                                    <asp:TextBox runat="server" ID="name" CssClass="input--style-1"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="input-group">
                                    <label for="">Street<span>*</span></label>
                                    <asp:TextBox runat="server" ID="address_1" CssClass="input--style-1"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="input-group">
                                    <div class="rs-select2 js-select-simple select--no-search">
                                        <label for="">Country <span>*</span></label>
                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="input--style-1"></asp:DropDownList>
                                        <div class="select-dropdown"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="input-group">
                                    <label for="">City <span>*</span></label>
                                    <asp:TextBox runat="server" ID="city" CssClass="input--style-1"></asp:TextBox>

                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-6 col-xs-12">
                                        <div class="input-group">
                                            <div class="rs-select2 js-select-simple select--no-search">
                                                <label for="">State <span>*</span></label>
                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="input--style-1"></asp:DropDownList>
                                                <div class="select-dropdown"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-xs-12">
                                        <div class="input-group">
                                            <label for="">Zip Code <span>*</span></label>
                                            <asp:TextBox runat="server" ID="zip" CssClass="input--style-1"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-6 col-xs-12 marginTP10">
                                <div class="input-group">
                                    <label>Modalities <span>*</span></label>
                                    <div>
                                        <asp:CheckBoxList ID="chkModality" runat="server" class="checkbox-style"></asp:CheckBoxList>
                                        <asp:Label CssClass="checkbox-style-2-label" ID="lblCheckbox" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12 marginTP10">
                                <%--<div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="input-group">
                                            <div class="rs-select2 js-select-simple select--no-search">
                                                <label>Prefferred method of payment <span>*</span></label>
                                                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="input--style-1">
                                                    <asp:ListItem Text="-- Select One --" Value="0" />
                                                    <asp:ListItem Text="Check" Value="CK" />
                                                    <asp:ListItem Text="Credit Card" Value="CC" />
                                                    <asp:ListItem Text="Online Payment" Value="OP" />
                                                    <asp:ListItem Text="Mail Invoice" Value="MI" />
                                                </asp:DropDownList>
                                                <div class="select-dropdown"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row">
                                    <div class="col-sm-12 col-xs-12">
                                        <div class="input-group">
                                            <label for="">Imaging Software Name</label>
                                            <asp:TextBox runat="server" ID="txtImgSoftware" CssClass="input--style-1 js-datepicker" MaxLength="100"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-12 marginTP10">
                                <div class="dotted-border">
                                    <h4 class="title">Contact Details</h4>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 col-xs-12">
                                <div>
                                    <div class="input-group">
                                        <label for="">Contact Person Name <span>*</span></label>
                                        <asp:TextBox runat="server" ID="contact_person_name" CssClass="input--style-1 js-datepicker"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div>
                                    <div class="input-group">
                                        <label for="">Contact Number<span>*</span></label>
                                        <asp:TextBox runat="server" ID="contact_person_mobile" CssClass="input--style-1 js-datepicker" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="input-group">
                                    <label for="">Clinic Email<span>*</span></label>
                                    <asp:TextBox runat="server" ID="email_id" CssClass="input--style-1 js-datepicker" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div>
                                    <div class="input-group">
                                        <label for="">Clinic Number<span>*</span></label>
                                        <asp:TextBox runat="server" ID="phone_no" CssClass="input--style-1 js-datepicker" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="col-sm-6 col-xs-12">
                                <div>
                                    <div class="input-group">
                                        <label for="">Fax</label>
                                        <input class="input--style-1 js-datepicker" type="text" placeholder="" name="fax" />
                                        <!-- <i class="fa fa-fax input-icon js-btn-calendar"></i> -->
                                    </div>
                                </div>
                            </div>--%>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 marginTP10">
                                <div class="dotted-border">
                                    <h4 class="title">Login Credentials</h4>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                Please create preferred login and password
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 col-xs-12 marginTP10">
                                <div class="input-group">
                                    <label for="">Login Id <span>*</span></label>
                                    <asp:TextBox runat="server" ID="login_id" CssClass="input--style-1 js-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12 marginTP10">
                                <div class="input-group">
                                    <label for="">Password <span>*</span></label>
                                    <asp:TextBox ID="login_password" TextMode="Password" runat="server" />
                                </div>
                            </div>
                            <%--<div class="col-sm-6 col-xs-12">
                                <div class="input-group">
                                    <label for="">Email Id</label>
                                    <asp:TextBox runat="server" ID="login_email_id" CssClass="input--style-1 js-datepicker"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="input-group">
                                    <label for="">Mobile Number</label>
                                    <asp:TextBox runat="server" ID="login_mobile_no" CssClass="input--style-1 js-datepicker"></asp:TextBox>
                                </div>
                            </div>--%>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-xs-12 marginBTM10 marginTP10">
                                <div class="pull-left">
                                    <h4 class="title">Veterinarians on Staff</h4>
                                </div>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12 marginBTM10">
                                <div class="pull-left">
                                    Add additional vets
                                </div>
                                <div class="pull-right pointer plusBtn" title="add additional vets">
                                    <h3><i class="fa fa-plus-square" aria-hidden="true" onclick="addNewRow()"></i></h3>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 col-xs-12 marginBTM10">
                                <div class="table-responsive">
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <th>SL</th>
                                                <th>First Name</th>
                                                <th>Last Name</th>
                                                <th>Credentials</th>
                                                <th>Email ID (Contact)</th>
                                                <th>Mobile #</th>
                                                <th style="width: 50px;"></th>
                                            </tr>
                                        </thead>
                                        <tbody id="tblBody">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="margin-top=5px;">
                            <div class="col-sm-6 col-xs-12">
                                <div>
                                    <div class="input-group">
                                        <label for="">Submitted By <span>*</span></label>
                                        <asp:TextBox runat="server" ID="txtSubmitBy" CssClass="input--style-1 js-datepicker" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <div class="p-t-20">
                                    <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="btnRegistration" CssClass="btn btn--radius btn--color" Text="Register" />--%>
                                    <button class="btn btn--radius btn--color" id="btnRegister" onclick="btnRegistration_OnClick()" type="button">Register</button>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnRootDir" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
    </form>
</body>
</html>

<!-- Jquery JS-->

<!-- Vendor JS-->
<script src="vendor/select2/select2.min.js"></script>
<script src="vendor/datepicker/moment.min.js"></script>
<script src="js/bootstrap.min.js"></script>
<script src="js/popups.js"></script>
<!-- Main JS-->
<script src="js/global.js"></script>

<script>
    addNewRow();
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');
    var objddlState = document.getElementById('<%=ddlState.ClientID %>');
    var objName = document.getElementById('<%=name.ClientID %>');
    var objAddress_1 = document.getElementById('<%=address_1.ClientID %>');
    var objCity = document.getElementById('<%=city.ClientID %>');
    var objZip = document.getElementById('<%=zip.ClientID %>');

    var objContact_person_name = document.getElementById('<%=contact_person_name.ClientID %>');
    var objContact_person_mobile = document.getElementById('<%=contact_person_mobile.ClientID %>');
    var objEmail_id = document.getElementById('<%=email_id.ClientID %>');
    var objPhone_no = document.getElementById('<%=phone_no.ClientID %>');
    var objLogin_id = document.getElementById('<%=login_id.ClientID %>');
    var objLogin_password = document.getElementById('<%=login_password.ClientID %>');

    var objChkModality = document.getElementById('<%=chkModality.ClientID %>');
    var objtxtSubmitBy = document.getElementById('<%=txtSubmitBy.ClientID %>');
    var objtxtImgSoftware = document.getElementById('<%=txtImgSoftware.ClientID %>');
    var objhdnRootDir = document.getElementById('<%=hdnRootDir.ClientID %>');
    var strForm = "VRSRegistration";

    function addNewRow() {
        var countRow = $('#tblBody tr').length + 1;
        var rows = Math.random().toString(36).substr(2, 9);
        var tblRow = '';
        tblRow = '<tr id="tr_' + rows + '">' +
                    '<td id="physician_id_' + rows + '" style="display:none">' +
                        '<span></span>' +
                    '</td>' +
                    '<td id="serial_' + rows + '">' +
                        countRow +
                    '</td>' +
                    '<td>' +
                    '<span></span>' +
                    '<input type="text" id="physician_fname_' + rows + '"/>' +
                    '</td>' +
                    '<td>' +
                    '<span></span>' +
                    '<input type="text" id="physician_lname_' + rows + '"/>' +
                    '</td>' +
                    '<td>' +
                    '<span></span>' +
                    '<input type="text" id="physician_credentials_' + rows + '" />' +
                    '</td>' +
                    '<td>' +
                    '<span></span>' +
                    '<input type="text" id="physician_email_' + rows + '" />' +
                    '</td>' +
                    '<td>' +
                    '<span></span>' +
                    '<input type="text" id="physician_mobile_' + rows + '" />' +
                    '</td>' +
                    '<td class="text-center">' +
                    //'<a class="Edit" href="javascript:;" id="Edit_' + rows + '"><i class="fa fa-pencil" aria-hidden="true"></i></a>' +
                    //'<span style="display:inline-block;padding-left: 3px;padding-right: 3px;">|</span>' +
                    '<a class="Update" href="javascript:;" style="display: none">Update</a>' +
                    '<a class="Cancel" href="javascript:;" style="display: none">Cancel</a>';
        if ($('#tblBody tr').length >= 1) {
            tblRow += '<a class="Delete" style="display:inline-block;text-align:center;cursor:pointer;" title="Delete This Veterinarian" onclick="DeleteRow(' + "'" + rows + "'" + ')" id="Delete_' + rows + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
        }
        else {
            tblRow += '<a class="Delete" href="javascript:void(0);" style="display:inline-block;text-align:center;;cursor:pointer;pointer-events: none;" title="Delete This Veterinarian"  disabled id="Delete_' + rows + '"><i class="fa fa-trash" disabled aria-hidden="true"></i></a>';
        }
        tblRow += '</td>' +
            '</tr>';
        countRow++;
        var isEmpty = false;
        if ($('#tblBody tr').length == 0) {
            $('#tblBody').append(tblRow);
        }
        else {
            $('#tblBody tr').each(function (e, val) {
                var id = $(this).index();
                var rowId = $(this).attr("id");
                if ($('#physician_fname_' + rowId.split('_')[1]).val() == '' && $('#physician_lname_' + rowId.split('_')[1]).val() == '') {
                    PopupMessage(objhdnRootDir.value, strForm, "addNewRow()", "414", "true");
                    isEmpty = true;
                }
            });
            if (!isEmpty) {
                $('#tblBody').append(tblRow);
            }
        }
    }

    function DeleteRow(rowId) {
        $('#tr_' + rowId).remove();
        $('#tblBody tr').each(function (idx, elem) {
            var rowId = $(this).attr("id");
            $('#serial_' + rowId.split('_')[1]).text(idx + 1);
        });
    }

</script>
<script src="js/Registration.js?v=<%=DateTime.Now.Ticks%>""></script>
