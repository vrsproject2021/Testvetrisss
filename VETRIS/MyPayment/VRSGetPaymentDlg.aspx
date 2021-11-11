<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRSGetPaymentDlg.aspx.cs" Inherits="VETRIS.MyPayment.VRSGetPaymentDlg" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
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
    <link id="lnkSTYLE" runat="server" href = "../css/style.css" rel="stylesheet" type="text/css" />
    <link id="lnkPMT" runat="server" href = "" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Abel&display=swap" rel="stylesheet" />

    <script src="../scripts/jquery-1.7.1.js"></script>
    <script src="scripts/GetPaymentDlgHdr.js?v=<%=DateTime.Now.Ticks%>">  
    </script>
    <%--<style>
        html
        {
            font-family: 'Abel';
        }

        .pgtn-logo
        {
            height: 2.2rem;
            min-width: 2.2rem;
            margin-right: .8rem;
            vertical-align: top;
            margin: 0px 0px 20px;
        }

        .card-logo
        {
            margin-right: .8rem;
            vertical-align: top;
            margin: 0px 0px 20px;
        }

        .theForm .pageTitle
        {
            text-align: center;
            margin-top: 10px;
            font-size: 20px;
            font-family: "Abel" !important;
        }

        .theForm .subTitle
        {
            text-align: center;
            margin-top: 5px;
            font-size: 20px;
            font-family: "Abel" !important;
        }

        .theForm .form-group
        {
            width: 390px;
        }

        .theForm .formInner
        {
            font-family: 'Abel' !important;
            width: 700px;
            max-width: 100%;
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            margin: 20px auto;
        }

        #payButton
        {
            width: 400px;
            display: block;
            margin: 20px auto;
            height: 50px !important;
            font-size: 20px;
            background-color: #1AD18E;
            border-color: #1AD18E;
            box-shadow: 0 3px 10px #bbbbbb;
        }

            #payButton:hover
            {
                background-color: #19C687;
                border-color: #19C687;
                box-shadow: 0 3px 4px #bbbbbb;
            }

            #payButton:active
            {
                opacity: 0.7;
            }

        #btnClose1
        {
            width: 400px;
            display: block;
            margin: 20px auto;
            height: 50px !important;
            font-size: 20px;
            background-color: #d9534f;
            border-color: #dddddd;
            box-shadow: 0 3px 10px #bbbbbb;
        }

            #btnClose1:hover
            {
                background-color: #d9534f;
                border-color: gray;
                box-shadow: 0 3px 4px #bbbbbb;
            }

            #btnClose1:active
            {
                opacity: 0.7;
            }

        .checkboxLabel
        {
            margin: 0 0 0 7px;
            height: 40px !important;
            font-size: 18px;
        }

        .payment-field
        {
            border-radius: 2px;
            width: 48%;
            margin-bottom: 14px;
            box-shadow: 0 2px 8px #dddddd;
            font-size: 16px;
            transition: 200ms;
        }
         .payment-field-dummy
        {
            width: 48%;
            margin-bottom: 14px;
            font-size: 16px;
            transition: 200ms;
        }

        .payment-field-70
        {
            border-radius: 2px;
            width: 68%;
            margin-bottom: 14px;
            box-shadow: 0 2px 8px #dddddd;
            font-size: 16px;
            transition: 200ms;
        }

        .payment-field-30
        {
            border-radius: 2px;
            width: 28%;
            margin-bottom: 14px;
            box-shadow: 0 2px 8px #dddddd;
            font-size: 16px;
            transition: 200ms;
        }

        .option-field
        {
            border-radius: 2px;
            width: 48%;
            transition: 200ms;
        }

        .CollectJSValid
        {
            border-color: #B40E3E;
        }

        .CollectJSInvalid
        {
            border-color: #B40E3E;
        }

        .payment-field input:focus
        {
            border: 1px solid #1AD18E;
            outline: none !important;
        }

        .payment-field:hover
        {
            box-shadow: 0 2px 4px #dddddd;
        }

        .payment-field input
        {
            border: 1px solid #ffffff;
            width: 100%;
            border-radius: 2px;
            padding: 4px 8px;
        }

        payment-field-70 input:focus
        {
            border: 1px solid #1AD18E;
            outline: none !important;
        }

        .payment-field-70:hover
        {
            box-shadow: 0 2px 4px #dddddd;
        }

        .payment-field-70 input
        {
            border: 1px solid #ffffff;
            width: 100%;
            border-radius: 2px;
            padding: 4px 8px;
        }

        payment-field-30 input:focus
        {
            border: 1px solid #1AD18E;
            outline: none !important;
        }

        .payment-field-30:hover
        {
            box-shadow: 0 2px 4px #dddddd;
        }

        .payment-field-30 input
        {
            border: 1px solid #ffffff;
            width: 100%;
            border-radius: 2px;
            padding: 4px 8px;
        }

        .payment-field input:invalid
        {
            border: 1px solid #ffffff;
            width: 100%;
            border-radius: 2px;
            /*border-left-color: lightcoral;
                border-left: 3px solid;
            padding: 4px 8px;
        }

        .payment-field input:required
        {
            border: 1px solid #ffffff;
            width: 100%;
            border-radius: 2px;
            border-left-color: lightcoral;
                border-left: 3px solid;
            padding: 4px 8px;
        }

        .payment-field input:valid
        {
            border: 1px solid #1AD18E;
            width: 100%;
            border-radius: 2px;
            border-left-color: #1AD18E;
                border-left: 3px solid;
            padding: 4px 8px;
        }

        #payment-fields
        {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
        }

        #ccnumber
        {
            width: 100%;
            font-size: 16px;
        }

        #ccexp,
        #cvv
        {
            font-size: 16px;
        }

        #paymentTokenInfo
        {
            width: 700px;
            display: block;
            margin: 30px auto;
        }

        .separator
        {
            margin-top: 30px;
            width: 100%;
        }

        .general
        {
            font-size: 16px;
            padding: 2px;
            background-color: #ffffff;
        }

        .general-block
        {
            border: 1px solid #1AD18E;
            padding: 2px;
        }

        .general-underlined
        {
            border-bottom: 1px solid #1AD18E;
            padding: 2px;
        }

        @media only screen and (max-width: 600px)
        {
            .theForm .pageTitle
            {
                font-size: 30px;
            }

            .theForm
            {
                width: 300px;
                max-width: 90%;
                margin: auto;
            }

                .theForm .form-group
                {
                    width: 100%;
                }
        }

        select
        {
            font-family: 'Abel' !important;
            border: 1px solid #ffffff;
            width: 100%;
            border-radius: 2px;
            padding: 4px 8px;
            font-size: 16px;
            cursor: pointer;
            background-color: #c0392b;
            border-bottom: 2px solid #962d22;
            color: white;
            padding-right: 38px;
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
             Adding transition effect 
            transition: color 0.3s ease, background-color 0.3s ease, border-bottom-color 0.3s ease;
        }
             For IE <= 11 
            select::-ms-expand
            {
                display: none;
            }

            select:hover,
            select:focus
            {
                border: 1px solid #1AD18E;
                outline: none !important;
            }
        select:focus {
                color: #c0392b;
                background-color: white;
                border-bottom-color: #DCDCDC;
            }

        ---------------------------------
        .border-all
        {
            font-size: 16px;
            border: 1px solid #ddd;
            background: #ffffff;
        }

        .border-bottom
        {
            border-bottom: 1px solid #ddd;
            margin-bottom: 5px;
        }

        .d-bg-solid
        {
            border: 1px solid #fbd8b4;
            background: #fcf5ee;
            margin-bottom: 10px;
        }

        .border-all .row
        {
            padding: 8px 0px;
        }

        .d-bg-solid .row
        {
            padding: 8px 0px;
        }

        .form-check-input
        {
            font-size: 16px;
            margin-right: 10px !important;
        }

        .date-exp select
        {
            width: 100px;
            float: left;
            margin-right: 10px;
        }

        .payment-m-p
        {
            padding-left: 15px !important;
        }

        .payment-text-p
        {
            padding-left: 14px !important;
        }

        .payment-text-box
        {
            padding-left: 24px !important;
        }

        .border-all h3
        {
            font-size: 16px;
            font-weight: bold;
            margin: 0;
        }

        .border-all b
        {
            font-size: 18px;
            font-weight: bold;
        }

        .border-all .img-c
        {
            padding-left: 10px;
        }

        .box-hidden
        {
            display: none;
        }

        .box-show
        {
            display: block;
        }


        /*----------------------------------
    </style>--%>

    <% if (usingOption.Value == "0")
       { %>

    <script src="https://secure.tnbcigateway.com/token/Collect.js" data-tokenization-key="<%=VETRIS.Global.TokenizationKey%>"
        data-variant="inline"></script>
    <script>

        document.addEventListener('DOMContentLoaded', function () {
            CollectJS.configure({

                'paymentSelector': '#payButton',

                variant: 'inline',
                googleFont: 'Abel',
                invalidCss: {
                    color: '#B40E3E'
                },
                validCss: {
                    color: '#13AA73'
                },
                customCss: {
                    'border-color': '#ffffff',
                    'border-style': 'solid'
                },
                focusCss: {
                    'border-color': '#1AD18E',
                    'border-style': 'solid',
                    'border-width': '3px'
                },
                fields: {

                    cvv: {
                        placeholder: 'CVV'
                    },
                    ccnumber: {
                        placeholder: 'Card Number'
                    },
                    ccexp: {
                        placeholder: 'Card Expiration in MM / YY'
                    }

                },
                'validationCallback': function (field, status, message) {
                    if (!status) {
                        var message = field + " is now Invalid: " + message;
                        console.log(message);
                    }


                },
                "timeoutDuration": 5000,
                "timeoutCallback": function () {
                    console.log("The tokenization didn't respond in the expected timeframe. poor connectivity");
                },
                "fieldsAvailableCallback": function () {
                    console.log("Collect.js loaded the fields onto the form");
                },
                "callback": function (response) {
                    objhdnToken.value = response.token;
                    objtrnType.value = response.card ? "c" : 'a';
                    var cc = response.card.number;

                    objtrnItem1.value = cc.substr(0, 1) + (new Array(cc.length - 4).join('X')) + cc.substr(cc.length - 4);
                    objtrnItem2.value = response.card.exp;
                    objtrnItem3.value = response.card.type;

                    btnSave_OnClick();


                }
            });
        });
    </script>
    <%}
       else if (usingOption.Value == "1")
       {%>
    <script src="https://secure.tnbcigateway.com/token/Collect.js" data-tokenization-key="<%=VETRIS.Global.TokenizationKey%>"
        data-variant="inline"></script>
    <script>

        document.addEventListener('DOMContentLoaded', function () {
            CollectJS.configure({

                'paymentSelector': '#payButton',

                variant: 'inline',
                googleFont: 'Abel',
                invalidCss: {
                    color: '#B40E3E'
                },
                validCss: {
                    color: '#13AA73'
                },
                customCss: {
                    'border-color': '#ffffff',
                    'border-style': 'solid'
                },
                focusCss: {
                    'border-color': '#1AD18E',
                    'border-style': 'solid',
                    'border-width': '3px'
                },
                fields: {

                    checkaccount: {
                        title: "Account Number",
                        placeholder: "Account Number (10 digits)"
                    },

                    checkaba: {
                        title: "Routing Number",
                        placeholder: "Routing Number (10 digits)"
                    },
                    checkname: {
                        title: "Name on Checking Account",
                        placeholder: "Name on Checking Account"
                    }

                },
                'validationCallback': function (field, status, message) {
                    if (!status) {
                        var message = field + " is now Invalid: " + message;
                        console.log(message);
                    }


                },
                "timeoutDuration": 5000,
                "timeoutCallback": function () {
                    console.log("The tokenization didn't respond in the expected timeframe. poor connectivity");
                },
                "fieldsAvailableCallback": function () {
                    console.log("Collect.js loaded the fields onto the form");
                },
                "callback": function (response) {

                    objhdnToken.value = response.token;
                    objtrnType.value = response.check ? "a" : 'c';
                    objtrnItem1.value = response.check.name;
                    objtrnItem2.value = response.check.account;
                    objtrnItem3.value = response.check.aba;
                    btnSave_OnClick();
                }
            });
        });
    </script>
    <%} %>
</head>

<body>





    <form id="form1" class="theForm" runat="server">
        <h2 class="pageTitle"><span>Online payment Information</span></h2>

        <% if (usingOption.Value != "")
           { %>
        <div class="formInner">
            <div class="option-field">
                <asp:RadioButton ID="rdoCC" runat="server" GroupName="grpPref" Checked="true" AutoPostBack="true" />
                <label for="rdoCC" class="checkboxLabel" style="width: auto; margin-top: 10px;">Pay using Credit/Debit Card</label>
            </div>
            <div class="option-field">
                <asp:RadioButton ID="rdoACH" runat="server" GroupName="grpPref" AutoPostBack="true" />
                <label for="rdoACH" class="checkboxLabel" style="width: auto; margin-top: 10px;">Pay by e-Check</label>
            </div>
            <div style="width: 100%; margin-bottom: 10px;">
                <h2><span>Billing Address</span></h2>
            </div>


            <div class="payment-field">
                <input type="text" id="fname" name="fname" placeholder="First Name*" required="required" value="" runat="server" autofocus="autofocus" autocomplete="off" />
            </div>
            <div class="payment-field">
                <input type="text" id="lname" name="lname" placeholder="Last Name" value="" runat="server" autocomplete="off" />
            </div>

            <div class="payment-field">
                <input type="text" id="address" name="address" placeholder="Address*" required="required" value="" runat="server" autocomplete="off" />
            </div>
            <div class="payment-field">
                <input type="text" id="city" name="city" placeholder="City*" required="required" value="" runat="server" autocomplete="off" />
            </div>
            <div class="payment-field">
                <input type="text" id="state" name="state" placeholder="State*" required="required" value="" runat="server" autocomplete="off" />
            </div>
            <div class="payment-field">
                <input type="text" id="country" name="country" placeholder="Country*" required="required" value="" runat="server" autocomplete="off" />
            </div>
            <div class="payment-field">
                <input type="text" id="zip" name="zip" placeholder="Zip*" required="required" value="" runat="server" autocomplete="off" />
            </div>
            <div class="payment-field" style="display:none;">
                <input type="text" id="phone" name="phone" placeholder="Phone" value="" runat="server" autocomplete="off" />
            </div>
            <div class="payment-field-dummy">
                </div>
            <% } %>



            <% if (usingOption.Value == "0")
               { // cc trandsaction can be saved for future %>

            <div class="payment-fields">
                Powered By <img class="pgtn-logo" src="../images/paymentgateway/tn-pg.svg" alt="TRANSNATIONAL" />
                &nbsp;<img class="card-logo" src="../images/paymentgateway/card_Banner.png" alt="cards" id="cc_banner" style="display:none;"/>
            </div>
            <div id="payment-fields">
                <div class="payment-field" id="ccnumber"></div>
                <div class="payment-field" id="ccexp"></div>
                <div class="payment-field" id="cvv"></div>
            </div>

            <div class="option-field">
                <input type="checkbox" id="checkSave" />
                <label for="checkSave" class="checkboxLabel" style="width: auto; margin-top: 10px;">Save for future use</label>
            </div>
            <%}
               else if (usingOption.Value == "1")
               { // ach transaction %>

            <div class="payment-fields">
                <img class="pgtn-logo" src="../images/paymentgateway/tn-pg.svg" alt="TRANSNATIONAL" />
            </div>
            <div id="payment-fields">
                <div class="payment-field" id="checkaccount"></div>
                <div class="payment-field" id="checkaba"></div>
                <div class="payment-field" id="checkname"></div>
            </div>

            <%} %>
        </div>
        <% if (usingOption.Value == "")
           {
               int row = 1;

               // Saved cards %>
        <div class="formInner">
            <div class="option-field">
                <asp:RadioButton ID="rdoSaved" runat="server" GroupName="grpAnother" Checked="true" />
                <label for="rdoSaved" class="checkboxLabel" style="width: auto; margin-top: 10px;">Using saved cards</label>
            </div>
            <div class="option-field">
                <asp:RadioButton ID="rodAnother" runat="server" GroupName="grpAnother" Checked="false" AutoPostBack="true" />
                <label for="rodAnother" class="checkboxLabel" style="width: auto; margin-top: 10px;">Other mode of transaction</label>
            </div>
            <div class="payment-fields">
                <img class="pgtn-logo" src="../images/paymentgateway/tn-pg.svg" alt="TRANSNATIONAL" />
            </div>
            <div class="container border-all" style="margin-top: 20px;">
                <div class="col-md-12">
                    <div class="row border-bottom">
                        <div class="col-md-9">
                            <h3>Your saved credit and debit cards</h3>
                        </div>

                        <div class="col-md-3 pull-right">
                            Expiry
                        </div>
                    </div>
                </div>

                <%foreach (VETRIS.Core.TransNationalPaymentGateway.TransactionVault item in vaults)
                  {
                   
                %>
                <div class="col-md-12 card-payment-option" id="div-each-row<%=row %>">
                    <div class="row">
                        <div class="col-md-9">
                            <input type="radio" class="form-check-input" id="chkrow-<%=row %>" name="paymentCard" data-selected="<%=item.last_used %>"
                                value="<%=item.vault_id.ToString()%>" /><%=item.vault_card.ToUpper()%>
                            <%=item.vault_card_type.ToUpper()%>
                            <img class="img-c" src="../images/paymentgateway/<%=item.vault_card_type.ToLower()%>.png" />
                        </div>
                       <%-- <div class="col-md-4">
                            <%=item.holder_name.ToUpper()%>
                            <input type="hidden" id="hdnVaultHolder_<%=item.vault_id.ToString()%>" value="<%=item.holder_name.ToUpper()%>" />
                        </div>--%>
                        <div class="col-md-3 pull-right">
                            <%=item.vault_exp %>
                        </div>
                    </div>
                    <div class="row box-hidden" id="input-<%=row %>">
                        <div class="col-md-9" id="place-holder-<%=row %>">
                            Enter CVV(？)  
                            
                        </div>

                        <div class="col-md-3 pull-right">
                            &nbsp;
                        </div>
                    </div>

                </div>

                <%  row++;
                  } %>
            </div>





            <div class="payment-field-30 box-hidden">
                <input type="text" id="txtcvv" name="txtcvv" placeholder="CVV" value="" maxlength="5" style="max-width: 60px;" runat="server" required="required" autocomplete="off" />
            </div>
        </div>

        <%} %>
        <button type="submit" id="payButton" class="btn btn-primary btn-block" runat="server">
            Pay due amount <span id="lblAmount" runat="server"></span>
        </button>


        <input type="hidden" id="txtAmount" runat="server" value="" />
        <input type="hidden" id="trnType" runat="server" value="" />
        <input type="hidden" id="trnItem1" runat="server" value="" />
        <input type="hidden" id="trnItem2" runat="server" value="" />
        <input type="hidden" id="trnItem3" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" value="00000000-0000-0000-0000-000000000000" />
        <input type="hidden" id="hdnHasVault" runat="server" value="" />
        <input type="hidden" id="initialGo" runat="server" value="1" />
        <input type="hidden" id="hdnError" runat="server" value="" />
        <input type="hidden" id="UserIp" runat="server" value="" />
        <input type="hidden" id="hdnToken" runat="server" value="" />
        <input type="hidden" id="usingOption" runat="server" value="" />
        <input type="hidden" id="vaultSelected" runat="server" value="" />
        <input type="hidden" id="hdnCF" runat="server" value="" />
        <input type="hidden" id="hdnAID" runat="server" value="" />
    </form>

    <button id="btnClose1" class="btn btn-danger btn-block" runat="server">
        Cancel/Different payment amount
    </button>
    <div id="paymentTokenInfo"></div>


    <%--------------------------------%>
</body>
<script type="text/javascript">
    var objhdnID = document.getElementById('<%=hdnID.ClientID %>');
    var objhdnError = document.getElementById('<%=hdnError.ClientID %>');
    var objtxtAmount = document.getElementById('<%=txtAmount.ClientID %>');
    var objuserIp = document.getElementById('<%=UserIp.ClientID %>');
    var objhdnToken = document.getElementById('<%=hdnToken.ClientID %>');
    var objlblAmount = document.getElementById('<%=lblAmount.ClientID %>');
    var objrdoCC = document.getElementById('<%=rdoCC.ClientID %>');
    var objrdoACH = document.getElementById('<%=rdoACH.ClientID %>');
    var objfName = document.getElementById('<%=fname.ClientID %>');
    var objlName = document.getElementById('<%=lname.ClientID %>');
    var objAddress = document.getElementById('<%=address.ClientID %>');
    var objCity = document.getElementById('<%=city.ClientID %>');
    var objState = document.getElementById('<%=state.ClientID %>');
    var objCountry = document.getElementById('<%=country.ClientID %>');
    var objZip = document.getElementById('<%=zip.ClientID %>');
    var objZip = document.getElementById('<%=zip.ClientID %>');
    var objPhone = document.getElementById('<%=phone.ClientID %>');
    var objtrnType = document.getElementById('<%=trnType.ClientID %>');
    var objtrnItem1 = document.getElementById('<%=trnItem1.ClientID %>');
    var objtrnItem2 = document.getElementById('<%=trnItem2.ClientID %>');
    var objtrnItem3 = document.getElementById('<%=trnItem3.ClientID %>');
    var objhdnHasVault = document.getElementById('<%=hdnHasVault.ClientID %>');
    var objvaultSelected = document.getElementById('<%=vaultSelected.ClientID %>');
    var objusingOption = document.getElementById('<%=usingOption.ClientID %>');
    var objhdnCF = document.getElementById('<%=hdnCF.ClientID %>');
    var objhdnAID = document.getElementById('<%=hdnAID.ClientID %>');
    var strForm = "VRSGetPaymentDlg";



</script>

<script src="../scripts/custome-javascript.js"></script>
<script src="../scripts/AppPages.js"></script>
<script src="scripts/GetPaymentDlg.js?v=<%=DateTime.Now.Ticks%>"></script>
</html>
