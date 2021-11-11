<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSInstitutionRegistration.aspx.cs" Inherits="VETRIS.Registration.VRSInstitutionRegistration" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VETERINARY RADIOLOGY INFORMATION SYSTEM</title>
     <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-icon" href="../images/favicon.ico" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet" />

    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/responsive.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <link href="../css/grid_style.css" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>


    <link href="vendor/select2/select2.min.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/main.css" rel="stylesheet" />

    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="../scripts/jquery-1.12.4.js"></script>
    <script src="../scripts/jquery.sticky.js"></script>
    <script src="../scripts/jquery-1.7.1.js"></script>
    
    <script src="../scripts/jquery.soverlay.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="vendor/select2/select2.min.js"></script>
    <script src="js/global.js"></script>
    <script src="scripts/InsttutionRegistrationHdr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
      <div class="page-wrapper bg-blue p-t-20 p-b-100 font-robo">
        <div class="container">
            <div class="card card-1">
                <div class="card-heading">
                	<img src="images/cta-logo-mid.png" />
                </div>
                <div class="card-body">
                    <h2 class="title">Registration Info</h2>
                    
                    	<div class="row">
							<div class="col-sm-12 marginBTM10">
								Institution Details
							</div>
						</div>
						<div class="row">
							<div class="col-sm-12 marginBTM10">
								<div class="input-group">
		                            <%--<input class="input--style-1" type="text" placeholder="Name" name="name">--%>
                                    <label class="control-label" >Name <span class="mandatory">*</span></label>
                                    <asp:TextBox ID="txtName" class="input--style-1" runat="server"></asp:TextBox>
		                            <i class="fa fa-user input-icon js-btn-calendar"></i>
		                       	</div>
							</div>
						</div>
                        
                        <div class="row">
                            <div class="col-sm-6 marginBTM10">
                                <div class="input-group">
                                    <%--<input class="input--style-1 js-datepicker" type="text" placeholder="Street Name" name="street_name"/>--%>
                                    <label class="control-label" >Street Name </label>
                                    <asp:TextBox ID="txtAddr1" runat="server" class="input--style-1 js-datepicker"></asp:TextBox>
                                    <i class="fa fa-street-view input-icon js-btn-calendar"></i>
                                </div>
                            </div>
                            <div class="col-sm-6 marginBTM10">
                                <div class="input-group">
                                    <%--<input class="input--style-1 js-datepicker" type="text" placeholder="City" name="city">--%>
                                    <label class="control-label" >City </label>
                                    <asp:TextBox ID="txtCity" runat="server" class="input--style-1 js-datepicker" type="text" ></asp:TextBox>
                                    <i class="fa fa-map-marker input-icon js-btn-calendar"></i>
                                </div>
                            </div>
                            <div class="col-sm-6 marginBTM10">
                                <div class="input-group">
                                    <div class="rs-select2 js-select-simple select--no-search">
                                        <label class="control-label" >Country <span class="mandatory">*</span></label>
                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        <%--<select name="Country">
                                            <option disabled="disabled" selected="selected">Country</option>
                                            <option>USA</option>
                                        </select>--%>
                                        <div class="select-dropdown"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 marginBTM10">
                                <div class="input-group">
                                    <div class="rs-select2 js-select-simple select--no-search">
                                        <label class="control-label" >State <span class="mandatory">*</span></label>
                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control custom-select-value"></asp:DropDownList>
                                        <%--<select name="state">
                                            <option disabled="disabled" selected="selected">State</option>
                                            <option>America</option>
                                        </select>--%>
                                        <div class="select-dropdown"></div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-sm-4 marginBTM10">
                                <div class="input-group">
                                    <%--<input class="input--style-1" type="text" placeholder="Zip Code" name="zip_code">--%>
                                    <label class="control-label" >Post Code </label>
                                    <asp:TextBox ID="txtZip" runat="server" class="input--style-1" type="text"></asp:TextBox>
                                    <i class="fa fa-map-pin input-icon js-btn-calendar"></i>
                                </div>
                            </div>
                        </div>
						<div class="row">
							<div class="col-sm-12 marginTP10 marginBTM10">
								Institution Contacts
							</div>
						</div>
                        <div class="row">
                            <div class="col-sm-6 marginBTM10">
                                <div class="input-group">
                                    <%--<input class="input--style-1 js-datepicker" type="text" placeholder="Email Id" name="email_id">--%>
                                    <label class="control-label" >Email Id <span class="mandatory">*</span></label>
                                    <asp:TextBox ID="txtEmailID" runat="server" class="input--style-1 js-datepicker" type="text"></asp:TextBox>
                                    <i class="fa fa-envelope input-icon js-btn-calendar"></i>
                                </div>
                            </div>
                            <div class="col-sm-6 marginBTM10">
                            	
                                <div style="width: 45%; float: left; margin-left: 5%; ">
                                	<div class="input-group">
                                    	<%--<input class="input--style-1 js-datepicker" type="text" placeholder="Office Contact" name="Office_contact">--%>
                                        <label class="control-label" >Office Contact</label>
                                        <asp:TextBox ID="txtTel" runat="server" class="input--style-1 js-datepicker" type="text"  MaxLength="10"></asp:TextBox>
                                    	<i class="fa fa-phone input-icon js-btn-calendar"></i>
                                    </div>
                                </div>

                                <div style="width: 45%; float: left; margin-left: 5%;">
                                	<div class="input-group">
                                    	<%--<input class="input--style-1 js-datepicker" type="text" placeholder="Fax" name="fax">--%>
                                        <label class="control-label" >Fax</label>
                                        <asp:TextBox ID="txtMobile" runat="server" class="input--style-1 js-datepicker" type="text"  MaxLength="10"></asp:TextBox>
                                    	<i class="fa fa-fax input-icon js-btn-calendar"></i>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-sm-6 marginBTM10">
                                <div style="width: 45%; float: left;">
                                    <div class="input-group">
                                        <%--<input class="input--style-1" type="text" placeholder="Contact Person" name="contact_person">--%>
                                        <label class="control-label" >Contact Person</label>
                                        <asp:TextBox ID="txtContPerson" runat="server" class="input--style-1" type="text" ></asp:TextBox>
                                        <i class="fa fa-user-o input-icon js-btn-calendar"></i>
                                    </div>
                                </div>
                                <div style="width: 45%; float: left; margin-left: 5%;">
                                	<div class="input-group">
                                    	<%--<input class="input--style-1 js-datepicker" type="text" placeholder="Mobile no" name="contact_person_mobile">--%>
                                        <label class="control-label" >Contact Person Mobile <span class="mandatory">*</span></label>
                                        <asp:TextBox ID="txtContMobile" runat="server" class="input--style-1 js-datepicker" type="text"  MaxLength="10"></asp:TextBox>
                                    	<i class="fa fa-mobile input-icon js-btn-calendar"></i>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <div class="row">
							<div class="col-sm-12 marginBTM10 marginTP10">
								<div class="pull-left">Login Credentials</div>
								<div class="pull-right pointer">
								</div>
							</div>
						</div>
						<div class="row">
                            <div class="col-sm-4 marginBTM10">
                                <div class="input-group">
                                    <%--<input class="input--style-1 js-datepicker" type="text" placeholder="Login Id" name="email_id">--%>
                                    <label class="control-label" >Login Id <span class="mandatory">*</span></label>
                                    <asp:TextBox ID="txtLoginId" runat="server" class="input--style-1 js-datepicker" type="text" ></asp:TextBox>
                                    <i class="fa fa-user input-icon js-btn-calendar"></i>
                                </div>
                            </div>
                            <div class="col-sm-4 marginBTM10">
                                <div class="input-group">
                                    <%--<input class="input--style-1 js-datepicker" type="password" placeholder="Password" name="email_id">--%>
                                    <label class="control-label" >Password <span class="mandatory">*</span></label>
                                    <asp:TextBox ID="txtPassword" runat="server" class="input--style-1 js-datepicker" type="password" ></asp:TextBox>
                                    <i class="fa fa-lock input-icon js-btn-calendar"></i>
                                </div>
                            </div>
                            <div class="col-sm-4 marginBTM10">
                                <div class="input-group">
                                    <%--<input class="input--style-1 js-datepicker" type="text" placeholder="Email Id" name="email_id">--%>
                                    <label class="control-label" >Email Id <span class="mandatory">*</span></label>
                                    <asp:TextBox ID="txtLoginEmailId" runat="server" class="input--style-1 js-datepicker" type="text" ></asp:TextBox>
                                    <i class="fa fa-envelope input-icon js-btn-calendar"></i>
                                </div>
                            </div>
                        </div>
                        <div class="row">
							<div class="col-sm-8 col-xs-8  marginBTM10 marginTP10">
								<div class="pull-left">Physicians</div>
                            </div>
                            <div class="col-sm-4 col-xs-4">
								<div class="pull-right pointer">
									<%--<h4><i class="fa fa-plus-square" aria-hidden="true"></i></h4>--%>
                                    <button type="button" class="btn btn_grd btn-primary" style="height: 30px !important; line-height: 0px !important;" id="btnAddPhys" runat="server" title="click to add new row for login credentials">
                                        <i class="fa fa-plus " aria-hidden="true"></i>
                                    </button>
								</div>
							</div>
						</div>
						<div class="row">
							<div class="col-sm-12 marginBTM10">
								<%--<div class="table-responsive">
									<table class="table table-bordered">
										<thead>
											<tr>
												<th>First Name</th>
												<th>Last Name</th>
												<th>Credentials</th>
												<th>Email ID (Contact)</th>
												<th>Mobile #</th>
												<th></th>
											</tr>
										</thead>
										<tbody>
											<tr>
												<td></td>
												<td></td>
												<td></td>
												<td></td>
												<td></td>
												<td class="colorRed text-center"><span class="pointer"><i class="fa fa-trash" aria-hidden="true"></i></span></td>
											</tr>
										</tbody>
									</table>
								</div>--%>

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
                                                    Width="100%"
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
                                                            <input type="text" id="txtMobile_## DataItem.GetMember('rec_id').Value ##" maxlength="500" style="width: 99%;" class="GridTextBox" value="## DataItem.GetMember('physician_mobile').Value ##" onchange="javascript:txtMobile_OnChange('## DataItem.GetMember('rec_id').Value ##');" onkeypress="javascript:return CheckInteger(event);" />
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
						
						<div class="row">
							<div class="col-sm-12 text-right">
								<div class="p-t-20 pull-right">
                                       
                                    <button id="btnRegister" type="button" runat="server" class="btn btn--radius btn--blue">Register</button>
                                    
		                            <%--<button id="btnRegister" runat="server" class="btn btn--radius btn--blue" type="button">Register</button>--%>
		                        </div>
							</div>
						</div>
                    
                </div>
            </div>
        </div>
    </div>
        <input type="hidden" id="hdnSecDivider" runat="server" value="»" />
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnPhysicians" runat="server" value="" />
        <input type="hidden" id="hdnUsrUpdUrl" runat="server" value="" />
    </form>
</body>

    <script type="text/javascript">
        var objhdnSecDivider = document.getElementById('<%=hdnSecDivider.ClientID %>');

        var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
        var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
        var objtxtName = document.getElementById('<%=txtName.ClientID %>');
        var objtxtAddr1 = document.getElementById('<%=txtAddr1.ClientID %>');
        var objtxtCity = document.getElementById('<%=txtCity.ClientID %>');
        var objtxtZip = document.getElementById('<%=txtZip.ClientID %>');
        var objtxtEmailID = document.getElementById('<%=txtEmailID.ClientID %>');
        var objtxtTel = document.getElementById('<%=txtTel.ClientID %>');
        var objtxtMobile = document.getElementById('<%=txtMobile.ClientID %>');
        var objtxtContPerson = document.getElementById('<%=txtContPerson.ClientID %>');
        var objtxtContMobile = document.getElementById('<%=txtContMobile.ClientID %>');

        var objtxtLoginId = document.getElementById('<%=txtLoginId.ClientID %>');
        var objtxtPassword = document.getElementById('<%=txtPassword.ClientID %>');
        var objtxtLoginEmailId = document.getElementById('<%=txtLoginEmailId.ClientID %>');

        var objddlCountry = document.getElementById('<%=ddlCountry.ClientID %>');
        var objddlState = document.getElementById('<%=ddlState.ClientID %>');
        var objhdnPhysicians = document.getElementById('<%=hdnPhysicians.ClientID %>');
       
        var objhdnUsrUpdUrl = document.getElementById('<%=hdnUsrUpdUrl.ClientID %>');
        var strForm = "VRSInstitutionRegistration";
    </script>

    <script src="../scripts/custome-javascript.js"></script>
    <script src="../scripts/AppPages.js"></script>
    <script src="scripts/InstitutionRegistration.js"></script>
    <%--<script src="../scripts/popups.js"></script>--%>

</html>
