var arrRecords = new Array();
arrRecords[0] = "Afghanistan";
arrRecords[1] = "Algeria";
arrRecords[2] = "Andorra";
$(document).ready($(function () {
    
    //$(".ddlCountry").chosen({
    //    max_selected_options: 4,
    //    no_results_text: "Oops, nothing found!",
    //    width: "95%"
    //});
    //$('.ddlCountry').on('ready', function (evt, params) {
    //    LoadDDL();
    //});
    LoadDDL();

   

}))

//$('.ddlCountry_chosen').on('change', function (evt, params) {
//    alert(params);
//});
//function LoadDDL() {
//    var objddl = document.getElementById("ddlCountry");
//    var selVal = ""; var selIndex = 0; var selText = "";
//    for (var i = 0; i < arrRecords.length; i++) {
//        selVal = arrRecords[i];
//        for (j = 1; j < objddl.length; j++) {
//            if (objddl.options[j].value == selVal) {
//                selIndex = j;
//                selText = objddl.options[j].text;
                
//                //var item = {
//                //    array_index: j,
//                //    classes: "",
//                //    disabled: true,
//                //    group_array_index: undefined,
//                //    group_label: null,
//                //    highlighted_html: "",
//                //    html: selText,
//                //    options_index: j,
//                //    search_match: true,
//                //    selected: true,
//                //    style: "",
//                //    text: selText,
//                //    title: undefined,
//                //    value: selVal
//                //};

//                this.call(objddl.options[j]);

//            }
//        }
//    }
//}
function LoadDDL() {
    var objddl = document.getElementById("ddlCountry");
    var newlink; var node;
    var selVal = ""; var selIndex = 0; var selText = "";
    var arr = new Array();

    var inputCtrl = document.getElementById("ddlCountry_chosen").getElementsByClassName("chosen-choices")[0].getElementsByClassName("search-field")[0].getElementsByClassName("chosen-search-input default");
    inputCtrl.setAttribute('class', 'chosen-search-input');
    for (var i = 0; i < arrRecords.length; i++) {
        selVal = arrRecords[i];
        for (j = 1; j < objddl.length; j++) {
            if (objddl.options[j].value == selVal) {
                selIndex = j;
                selText = objddl.options[j].text;
                break;
            }
        }
        node = document.createElement("LI");
        node.className = "search-choice";
        node.innerHTML = "<span>" + selText + "</span>";
        newlink = document.createElement("a");
        newlink.setAttribute('class', 'search-choice-close');
        newlink.setAttribute('data-option-array-index', selIndex.toString());
        node.appendChild(newlink);
        document.getElementById("ddlCountry_chosen").getElementsByClassName("chosen-choices")[0].appendChild(node);

        arr = document.getElementById("ddlCountry_chosen").getElementsByClassName("chosen-drop")[0].getElementsByClassName("chosen-results")[0].getElementsByClassName("active-result");
        if (arr.length > 0) {
            for (var k = 0; k < arr.length; k++) {
                if (arr[k].getAttribute("data-option-array-index") == k + 1) {
                    arr[k].setAttribute('class', 'result-selected');
                }
            }
        }
        else {
            var ul = document.getElementById("ddlCountry_chosen").getElementsByClassName("chosen-drop")[0].getElementsByClassName("chosen-results")[0];
            for (var k = 1; k < objddl.options.length; k++) {
                var objLi = document.createElement("LI");
                if (objddl.options[k].value == selVal) {
                    objLi.setAttribute('class', 'result-selected');
                }
                else {
                    objLi.setAttribute('class', 'active-result');
                }
                objLi.setAttribute('data-option-array-index', k.toString());
                objLi.textContent = objddl.options[k].value;
                ul.appendChild(objLi);
            }
        }

        //objddl.attributes[0].value = "";
        //objddl.attributes[0].textContent = "";
        //objddl.attributes[0].nodeValue="";
    }
}



function Button1_OnClick() {

    var targetDiv = document.getElementById("ddlCountry_chosen").getElementsByClassName("chosen-drop")[0];
    var arr = new Array();

    var arr = document.getElementById("ddlCountry_chosen").getElementsByClassName("chosen-drop")[0].getElementsByClassName("chosen-results")[0].getElementsByClassName("result-selected");
    for (var i = 0; i < arr.length; i++) {
        var idx = parseInt(arr[i].getAttribute("data-option-array-index"));
        alert(objddl.options[idx].value);
    }
}