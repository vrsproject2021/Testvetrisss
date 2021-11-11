<script>
  import {apiendpoint} from "../../store.js";  
  import { goto } from '@roxi/routify/runtime/helpers';
  import {user} from "../../model/user";
  import { onMount } from "svelte";
  import Moveable from "svelte-moveable";
  import { beforeUpdate, element, query_selector_all } from "svelte/internal";
  import Menu from '../../components/menu/Menu.svelte';
  import MenuOption from '../../components/menu/MenuOption.svelte';
  import MenuDivider from '../../components/menu/MenuDivider.svelte';
  import Table from "../../components/Table.svelte";
  import Dialog from "../../components/Dialog.svelte";
  import * as report from "../../model/report/report.js"; 
  import PropertyBox from "../../components/PropertyBox.svelte";
  import Datasource from "../../components/properties/Datasource.svelte";
  import ExpressionEditor from "../../components/properties/ExpressionEditor.svelte";
  import ReportParameter from "../../components/properties/ReportParameter.svelte";
  import ReportFilter from "../../components/properties/ReportFilter.svelte";
  import ReportImage from "../../components/properties/ReportImage.svelte";
  import Flatpickr from 'svelte-flatpickr';
	import 'flatpickr/dist/flatpickr.css';
	import 'flatpickr/dist/themes/light.css';
  if(!user.isAuthenticated())
    $goto('/login');
    
    report.init($apiendpoint);  
    
    let rcollapse = true;
    let lcollapse = false;
    let preview=false, headercontrols, previewcontainer;

    let scale = 1;
    let selectedObjectType=null, selectedObject=null;
    function zoomOut(){
        // if(scale>2 && scale<=4) scale -=1;
        // else if(scale>1 && scale<=2) scale -=0.5;
        // else if(scale>0.25) scale -=0.25;
    }
    function zoomIn(){
        // if(scale>=2 && scale<4) scale +=1;
        // else if(scale>=1 && scale<2) scale +=0.5;
        // else if(scale<1) scale +=0.25;
    }
    let controls=false;
    let charts=true;
    let viewOpen=false;
    let viewHeader=false;
    let viewFooter=false;
    let viewGrid=true;
    let isFullscreen=false;
    let body;
    let grid;
    let canvasWidth=600;
    let canvasHeight=300;
    let canvasMaxWidth;
    let header;
    let footer;
    let editTextDialog=false, editExprDialog=false;
    let text, textExpr;
    let reportObject=null;
    let selectedProp="Property";

    let dataset=[], dataSource=[];
    reportObject=report.NewReport();
    // report.NewReportWithDefaultDS().then(data=>{
    //       reportObject= data;
    //       canvasMaxWidth = report.inToPixels(reportObject.page.pageWidth);
    //     });


    $:{
      
      if(reportObject!==null){
        dataSource = reportObject.dataSources.dataSource;
        dataset = reportObject.dataSets.dataSet;
        const pgwidth = report.inToPixels(reportObject.page.pageWidth);
        const lm = report.inToPixels(reportObject.page.leftMargin);
        const rm = report.inToPixels(reportObject.page.rightMargin);
        canvasMaxWidth=pgwidth-(lm+rm)-5;
        if(canvasWidth>canvasMaxWidth) canvasWidth=canvasMaxWidth;
        if(!selectedObjectType) {
          
          selectedObjectType="page";
          selectedObject=reportObject.page;
        }
        if(selectedProp=="Filter" && selectedObjectType!="tablix"){
          selectedProp=="Property";
        }
      }
    }

    $: showFilters = dataset.length>0 && selectedObjectType=="tablix" && !!selectedObject;
    $: previewHeight= (preview && headercontrols!==undefined && (window.innerHeight - headercontrols.offsetHeight)) || 0;
    function onviewheader(){
        viewOpen=false;
        viewHeader=!viewHeader;
    }
    function onviewfooter(){
        viewOpen=false;
        viewFooter=!viewFooter;
    }
    function onviewgrid(){
        viewOpen=false;
        viewGrid=!viewGrid;
    }
    function onviewfullscreen(){
        viewOpen=false;
        isFullscreen=!isFullscreen;
    }
    function handleResize(e) {
      if(!reportObject) return;
      if(window.innerHeight-designcontainer.offsetTop>=0){
        containerHeight=(window.innerHeight-designcontainer.offsetTop-22)+"px";
      } 
      else {
        containerHeight="100%";
      }
      if(window.innerWidth-designcontainer.offsetLeft>=0){
        containerWidth=(window.innerWidth-designcontainer.offsetLeft-34)+"px";  
      } 
      else {
        containerWidth="100%";
      }
    }
    let currentDrag;
    let containerHeight='100%';
    let containerWidth='100%';
    let headerHeight=report.inToPixels("1.0in");;
    let footerHeight=report.inToPixels("1.0in");;
    let bodyHeight=report.inToPixels("1.0in");
    let minHeaderFooter = report.inToPixels("0.2in");
    let maxHeaderFooter = report.inToPixels("5in");
    let minBodyHeight = report.inToPixels("0.3in");
    let minHeight = 100;
    let minWidth = 100;
    
    let currentX, currentY;
    let designholder;
    let resizeContext;
    let designcontainer;
    let bodysection;
    let container;
    let target;
    let refreshElementIndex;
    let currentElementIndex;
    let dsInitiate=false;
    let bounds = {};
    let elementGuidelines = [];

    let path = (()=> { 
        let s = (location.pathname||"");
        let m=s.match(/(\/\w+)/);
        if(m){
          s=m[1];
        }
        return s;
    })();
    $: hasDataset = reportObject && reportObject.dataSets && reportObject.dataSets.dataSet && reportObject.dataSets.dataSet.length>0||false;
    $: bodyelements = elements.filter(i=>i.section==="bodysection");
    $: headerelements = elements.filter(i=>i.section==="headersection");
    $: footerelements = elements.filter(i=>i.section==="footersection");

    $: canvasHeight=(viewHeader?headerHeight:0)+bodyHeight+(viewFooter?footerHeight:0);

    function calculateDimension(partelements) {
      let _top=0, _left=0, _right=0, _bottom=0;
      partelements.filter(i=>i.data!=undefined).forEach(el=>{
        let {left,top, height, width} = el.data;
         _left = Math.min(_left, report.cmToPixels(left));
         _top = Math.min(_top, report.cmToPixels(top));
         _right = Math.max(_right, report.cmToPixels(left)+report.cmToPixels(width));
         _bottom = Math.max(_bottom, report.cmToPixels(top)+report.cmToPixels(height));
      });
      let _height=(_bottom-_top)+_top, _width=(_right-_left)+_left;
      return {height: _height, width:_width};
    }

    // let target;
    // let targets = [].slice.call(document.querySelectorAll(".itemelement"));
    function onmousedownhorizontal(e){
        target=null;
        e.preventDefault();
        resizeContext = "hsz_designholder";
        
    }
    function onmousedownvertical(e){
        target=null;
        e.preventDefault();
        resizeContext = "vsz_designholder";
        
    }
    function onheadermousedown(e){
        target=null
        //e.preventDefault();
        resizeContext = "vsz_header";
        
    }
    function onfootermousedown(e){
        target=null
        //e.preventDefault();
        resizeContext = "vsz_footer";
        
    }
    function holdermousemove(e){
        e=e || window.event;
        //e.preventDefault();
        
        if(resizeContext==="hsz_designholder"){
            const dy = (e.pageY-designholder.offsetTop-5)-canvasHeight ;
            // if(canvasHeight+dy>=minHeight)
            //     canvasHeight += dy;
            if(viewFooter){
              // increase/decrease footer
              const dim = calculateDimension(footerelements);
              const minH =Math.min(maxHeaderFooter,Math.max(dim.height,minHeaderFooter));
              if(footerHeight+dy>=minH && footerHeight<=maxHeaderFooter){
                footerHeight+=dy;
                
              }
            } else {
              const dim = calculateDimension(bodyelements);
              const minH =Math.max(dim.height,minBodyHeight);
              // increase/decrease body
              if(bodyHeight+dy>=minH){
                bodyHeight+=dy;
              }
            }    
        }
        
        if(resizeContext==="vsz_designholder"){
            const dx = e.pageX - (designholder.offsetLeft+6)-canvasWidth;
            if(canvasWidth+dx>=minWidth && canvasWidth+dx<=canvasMaxWidth){
                canvasWidth += dx; 
            }
        }

        if(resizeContext==="vsz_header"){
            const dy = e.pageY - header.offsetParent.offsetTop - header.offsetHeight;
            // increase/decrease header when header is visible
            const dim = calculateDimension(headerelements);
            const minH =Math.min(maxHeaderFooter,Math.max(dim.height,minHeaderFooter));
            if(headerHeight+dy>=minH && headerHeight+dy <= maxHeaderFooter)
                headerHeight += dy; 
           
        }
        if(resizeContext==="vsz_footer"){
            //const dy = canvasHeight-(e.pageY - footer.offsetParent.offsetTop)-footerHeight;
            const dy = (e.pageY-designholder.offsetTop-5)-((viewHeader?headerHeight:0)+bodyHeight);
            const dim = calculateDimension(bodyelements);
            const minH =Math.max(dim.height,minBodyHeight);
            // increase/decrease body when footer is visible
            if(bodyHeight+dy>=minH){
                bodyHeight+=dy;
            }
        }

        
        
    }
    
    function mouseup (e) {
        //e.preventDefault();
        if(resizeContext==="hsz_designholder"||resizeContext==="vsz_designholder" || resizeContext==="vsz_header"||resizeContext==="vsz_footer"){
          resizeContext = undefined;
          if(reportObject){
            reportObject.width=report.px2inFormat(canvasWidth);
            reportObject.page.pageHeader.height=report.px2cmFormat(headerHeight);
            reportObject.page.pageFooter.height=report.px2cmFormat(footerHeight);
            reportObject.body.height=report.px2cmFormat(bodyHeight);
            reportObject=Object.assign({}, reportObject);
            elements=[...elements];
          }
        }
        resizeContext = undefined;
    }

    onMount(()=>{
      handleResize();
      
    });
    
    function lefttoggle() {
      lcollapse=!lcollapse;
      setTimeout( () => handleResize(), 10);
    }

    let frame = {
        translate: [0, 0],
    };
    let frames = [];
    let elements = [];
    let temporaryImageIntent=null;
    let imageDialog=false, chosenImageName;
    
    function dragstart(event, i) {
     
      event.dataTransfer.effectAllowed = 'move';
      event.dataTransfer.dropEffect = 'move';
      const start = i;
      event.dataTransfer.setData('text/plain', start);
      
    }

    function beginImageIntent(intent){
      temporaryImageIntent=intent;
      imageDialog=true;
    }
    function addImageFromIntent(){
      frames.push({
          translate: [0, 0],
      }); 
      
      const { args, section, type, field, text, position } = temporaryImageIntent;
      
      let obj = report.addImageToSection(reportObject, args.section ,args.name, args.x, args.y, args.height, args.width );
      obj.value=chosenImageName;
      reportObject = Object.assign({}, reportObject);
      let image = report.findObjectByName(reportObject,obj.name).object;

      let el=Object.assign({}, {
        section: section,
        type: type,
        field: field,
        text: text,
        data: image,
        position: position
      });
      elements=[...elements, el];
      temporaryImageIntent=null;
      imageDialog=false;
    }

    function cancelImageIntent(){
      temporaryImageIntent=null;
      imageDialog=false;
    }

    function drop (event, target) {
      event.dataTransfer.dropEffect = 'move'; 
      const start = event.dataTransfer.getData("text/plain");
      if(["headersection", "bodysection","footersection"].indexOf(target)>=0 ){
        let section=target.replace("section","");
        if (start=="textelement" ) {
          frames.push({
              translate: [0, 0],
          }); 
          let obj = report.addTextBoxToSection(reportObject, section,null, event.offsetX+'px', event.offsetY+'px', 28+'px', 140+'px');
          reportObject = Object.assign({}, reportObject);
          let textbox = report.findObjectByName(reportObject,obj.name).object;
          let el={
              section:target, 
              type: start, 
              field:"", text:"Text",
              position: {x: event.offsetX, y: event.offsetY, height: 28, width:140, editing: false},
              data: textbox
            };
          elements=[...elements, el];
        }
        if (start=="lineelement" ) {
          frames.push({
              translate: [0, 0],
          }); 
          let obj = report.addLineToSection(reportObject, section,null, event.offsetX+'px', event.offsetY+'px', 50+'px', 50+'px');
          reportObject = Object.assign({}, reportObject);
          let line = report.findObjectByName(reportObject,obj.name).object;

          let el={
                section:target, 
                type: start, 
                field:"", 
                text:"Line",
                data: line,
                position: {x: event.offsetX, y: event.offsetY, height: 50, width:5},};
          elements=[...elements, el];
        }
        if (start=="imageelement" ) {
          beginImageIntent({
                args: {
                  section:section,
                  name: null, 
                  x: event.offsetX+'px', 
                  y: event.offsetY+'px', 
                  height: 64+'px', 
                  width: 64+'px'
                },
                section:target, 
                type: start, 
                field:"", 
                text:"Image",
                position: {x: event.offsetX, y: event.offsetY, height: 64, width:64}
          });
          return;
          
        }
        if (start=="boxelement" ) {
          frames.push({
              translate: [0, 0],
          }); 
          let el={section:target, type: start, field:"", text:"Box",position: {x: event.offsetX, y: event.offsetY, height: 100, width:100}};
          elements=[...elements, el];
        }
        if (start=="tableelement" && target=="bodysection") {
          frames.push({
              translate: [0, 0],
          }); 
          let tablix=report.makeTable(2,3,"0.6cm", "2.0cm");
          if(hasDataset){
            tablix.dataSetName = reportObject.dataSets.dataSet[0].name;
          }
          tablix.top=report.px2cmFormat(0); 
          tablix.left=report.px2cmFormat(0); 
          tablix.height=report.px2cmFormat(50); 
          tablix.width=report.px2cmFormat(canvasWidth); 
          tablix.name = `body_table_${reportObject.body.reportItems.tablix.length+1}`;
          reportObject.body.reportItems.tablix.push(tablix);
          reportObject = Object.assign({}, reportObject);
          let el={section:target, type: start, field:"", text:"Table",
                    position: {x: 0, y: 0, height: 50, width: canvasWidth},
                    data: reportObject.body.reportItems.tablix.find(i=>i.name==tablix.name)
                };
          
          elements=[...elements, el];
        }
      }
      
      
    }
    function onclick(e, objectIndex){
      e.stopPropagation();
      
      if(e.target.id.match(/(body|footer|header)element\d+/)){
          target = e.target;
      }
      else {
        let t = e.target.closest(".itemelement");
        if(t!=null && t.id.match(/(body|footer|header)element\d+/)!=null){
          target = t;
        } else {
          target=null;
        }
      }
      selectedProp="Property";
      setBounds(target);
      if(!isDialogOpen && !(objectIndex===undefined && objectIndex===null )){
        const el=elements[objectIndex];
        if(el && el.type!=='tableelement'){
          const obj = report.findObjectByName(reportObject, el.data.name);
          if(obj){
            selectedObjectType= obj.type;
            selectedObject = obj.object; 
          } 
        }
      }
    }

    function setBounds(target){
      if(!target) return null;
      let section=null;
      const toset = target && target.id.match(/(body|footer|header)element\d+/)!=null;
      const secname = target && target.id.match(/(body|footer|header)element\d+/)[1];
      if(toset && target.id.match(/(body)element\d+/)!=null) section=bodysection;
      else if(toset && target.id.match(/(header)element\d+/)!=null) section=header;
      else if(toset && target.id.match(/(footer)element\d+/)!=null) section=footer;
      if(toset){
          let left=0, top=0, width=0, height=0, right=0, bottom=0;
          left= section.offsetParent.offsetLeft+section.offsetLeft;
          top= section.offsetParent.offsetTop+section.offsetTop;
          width=section.offsetWidth;
          height = section.offsetHeight;
          if(secname=="header"){
              top+=8;
              height-=12;
          }
          if(secname=="body"){
              if(!viewHeader && viewFooter){
                height-=6;
              } else if(viewHeader && !viewFooter){
                height+=8;
              } else if(viewHeader && viewFooter){
                height+=4;
              }
          }
          if(secname=="footer"){
              if(viewHeader){
                height+=6;  
              } else {
                height+=4;
              }
          }
          elementGuidelines = [
            document.querySelector(".itemelement")
          ];
          right = left + width;
          bottom = top + height;
          bounds = {left:left,top:top,right:right,bottom:bottom};
      
      } else {
        bounds= {};
      }
    }

    let currentEditing=null;
    const onDoubleClick=(e, obj)=>{
      obj.editing=true;
      currentEditing=obj;
    };

    /* menu */
    let contextpos = { x: 0, y: 0 };
	  let showContextMenu = false;
    let currentTarget=null;

    $:isDialogOpen = editTextDialog || editExprDialog;

    function findReportObject(target, index){
      let m=target.id.match(/(body|footer|header)element(\d+)/);
      if(m) {
        let element_index=parseInt(m[2]);
        let section=m[1];
        const el=element[element_index];

        let obj=report.findObjectByName(reportObject, el.data.name);
        if(obj){
          selectedObjectType=obj.type;
          selectedObject = obj.object;
        }
      }
    }

    function selectTableObject(id, objectIndex){
        target=document.getElementById(id);
        setBounds(target);
    }

    function selectedTable(objectIndex, colIndex){
      const el=elements[objectIndex];
      const obj = report.findObjectByName(reportObject, el.data.name);
      refreshElementIndex=objectIndex;
      if(obj){
        selectedObjectType= obj.type;
        selectedObject = obj.object; 
      }
    }

    async function onRightClick(e, objectIndex) {
      e.stopPropagation();
      if (showContextMenu) {
        showContextMenu = false;
        await new Promise(res => setTimeout(res, 100));
      }
      
      
      if(e.target.id.match(/(body|footer|header)element\d+/)){
          if(target=e.target) target=null;
          currentTarget = e.target;
      }
      else {
        let t = e.target.closest(".itemelement");
        if(t && t.id.match(/(body|footer|header)element\d+/)){
          if(target=e.target) target=null;
          currentTarget = t;
        } else {
          currentTarget=null;
        }
      }
      if(!(objectIndex===undefined && objectIndex===null)){
        currentElementIndex=objectIndex;
      }
      contextpos = { x: e.clientX, y: e.clientY };
      showContextMenu = true;
    }
    
    function closeMenu() {
      showContextMenu = false;
    }

    function deleteElement() {
      if(currentTarget){
        if(currentEditing) { 
          currentEditing.editing=false;
          currentEditing=false;
        }
        let index = -1;
        let m= currentTarget.id.match(/element(\d+)/);
        if(m) index = parseInt(m[1]);
        if(index>-1) {
          let el=elements[index];
          elements.splice(index,1);
          if(el.data!==undefined){
            if(report.removeObjectByName(reportObject, el.data.name)){
              reportObject = Object.assign({}, reportObject);
              selectedProp="Property";
              pageClick();
            }
          }
          elements=[...elements];
          target=null;
        }
      }
      currentTarget=null;
      currentElementIndex=null;
    }

    function onSelectTableCell(tbl, row, col, element_index){
      const table = report.findObjectByName(reportObject, tbl.name).object;
      let obj=report.getCell(table,row,col);
      if(obj){
        let textbox=report.getTextBoxInCell(obj);
        if(textbox){
          selectedObject = textbox; 
          selectedObjectType="textbox";
        } else {
          selectedObject = obj; 
          selectedObjectType="tablixcell";
        }
      }
    }
    function onAddEditTextInTableCell(tbl, row, col, reportElementIndex){
      refreshElementIndex=reportElementIndex;
      let obj=report.getCell(tbl,row,col);
      if(obj){
        let textbox=report.getTextBoxInCell(obj);
        if(textbox){
          text=report.getCellText(tbl, row, col);
          selectedObject = textbox; 
          selectedObjectType="textbox";
          editTextDialog=true;
          
        } else {
          report.addTextToCell(tbl,row,col,'');
          obj=report.getCell(tbl,row,col);
          textbox=report.getTextBoxInCell(obj);
          text=report.getCellText(tbl, row, col);
          selectedObject = textbox; 
          selectedObjectType="textbox";
          editTextDialog=true;
          
        }
      }
    }
    function onAddEditExpressionInTableCell(tbl, row, col, reportElementIndex){
      refreshElementIndex=reportElementIndex;
      let obj=report.getCell(tbl,row,col);
      if(obj){
        let textbox=report.getTextBoxInCell(obj);
        if(textbox){
          textExpr=report.getCellText(tbl, row, col);
          selectedObject = textbox; 
          selectedObjectType="textbox";
          editExprDialog=true;
        } else {
          report.addTextToCell(tbl,row,col,'');
          obj=report.getCell(tbl,row,col);
          textbox=report.getTextBoxInCell(obj);
          textExpr=report.getCellText(tbl, row, col);
          selectedObject = textbox; 
          selectedObjectType="textbox";
          editExprDialog=true;
        }
      }
    }
    function onSelectTableRow(tbl, row){
      let obj=report.getRow(tbl,row);
      if(obj){
        selectedObjectType="tablixrow";
        selectedObject = obj; 
      }
    }

    function updateContent(e){
      e.stopPropagation();
      
      if(selectedObject && selectedObjectType=="textbox"){
        selectedObject.paragraphs.paragraph.textRuns.textRun.value=text;
        const el = elements[refreshElementIndex];
        reportObject=Object.assign({}, reportObject);
        el.data=report.findObjectByName(reportObject, el.data.name).object;
        elements=[...elements];
        editTextDialog=false;
        pageClick();
      }
    }

    function getTextOrExpression(element_index) {
          let el = elements[element_index];
          if(el && el.type=="textelement" && el.data){
            const type = report.getTextBoxContentType(el.data);
            return type;
          }
          return null;
      }

    function editTextElementByIndex(e, element_index) {
        e.preventDefault();
        e.stopPropagation();
        let el = elements[element_index];
        const obj=report.findObjectByName(reportObject, el.data.name);
        if(obj.type==="textbox" && obj.object){
          selectedObject = obj.object; 
          selectedObjectType="textbox";
          refreshElementIndex=element_index;
          text=selectedObject.paragraphs.paragraph.textRuns.textRun.value;
          editTextDialog=true;
        }
    }
    function editExpressionElementByIndex(e, element_index) {
        e.preventDefault();
        e.stopPropagation();
        let el = elements[element_index];
        const obj=report.findObjectByName(reportObject, el.data.name);
        if(obj.type==="textbox" && obj.object){
          selectedObject = obj.object; 
          selectedObjectType="textbox";

          refreshElementIndex=element_index;
          textExpr=selectedObject.paragraphs.paragraph.textRuns.textRun.value;
          editExprDialog=true;
        }
    }
    function updateContentExpr(e){
      if(!validateExpr(e.detail.expression)) return;
      if(selectedObject && selectedObjectType=="textbox"){
        selectedObject.paragraphs.paragraph.textRuns.textRun.value=e.detail.expression;
        let el = elements[refreshElementIndex];
        reportObject=Object.assign({}, reportObject);
        el.data=report.findObjectByName(reportObject, el.data.name).object;
        elements=[...elements];
        editExprDialog=false;
        pageClick();
      }
    }
    function pageClick (e){
      if(editTextDialog || editExprDialog) return;
      selectedObjectType="page";
      selectedObject=reportObject.page;
    }

    
    function validateExpr(expr) {
      if(expr=='') return true;
      if(expr.startsWith("=")) return true;
      // expression validation regex required
      return false;
    }

    function report_object_change(e) {
      
      reportObject = Object.assign({},reportObject);
      elements=[...elements];
    }
    function table_object_change(e) {
        const tableName = e.detail.tableName;
        let el = elements.find(i=>i.data && i.data.name===tableName);
        if(el){
          reportObject = Object.assign({},reportObject);
          let obj=report.findObjectByName(reportObject,tableName);
          
          el.data=obj.object;
          selectedObject=obj.object;
          elements=[...elements];
        }
        
    }
    function add_data_source_init() {
      dsInitiate = true;  
      selectedProp="Dataset";
    }

    /*
        preview section 
    */

    $:hasParameters = (reportObject && reportObject.reportParameters && reportObject.reportParameters.reportParameter && reportObject.reportParameters.reportParameter||[]).length>0;
    let reportPdf=null;
    async function controlPreview(e){
        reportPdf=null;
        preview=true;
        if(hasParameters) {
          return;
        }
        await showReport();
    }

    async function showReport(e){
      await processPreview();
    }

    async function processPreview(){
        await report.generatePDFForPreview(reportObject).then(result=>{
            if(result.isError==false){
              reportPdf = `${$apiendpoint}/file/gettemporaryfile?contentType=${result.result.fileType}&id=${result.result.fileToken}`;            
            } else {
              reportPdf=null;
            }
        }).catch(reason=>{
          
          reportPdf=null;
        });
    }
    
</script>
<svelte:window on:resize={handleResize}></svelte:window>
{#if reportObject}

<div  class="flex flex-col text-sm min-h-screen" on:resize={handleResize} 
    on:mousemove={holdermousemove}
    on:mouseup={mouseup}>
    <!-- top toolbar 1 -->
    <div bind:this={headercontrols} class="p-1 bg-gray-100 flex flex-row items-center justify-between">
      <div class="flex flex-row">
        <svg class="w-8 v-8 p-1 mx-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
        </svg>
        <input type="text" class="text-sm w-50 p-1 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
      </div>
      <div class="flex flex-row">
        <button class="w-20 p-1 { !preview? 'bg-blue-400 text-white rounded-l-sm':'bg-white text-blue-400 rounded-l-sm border border-blue-400' } focus:outline-none" on:click={(e)=> preview=false}>Design</button>
        <button class="w-20 p-1 { preview? 'bg-blue-400 text-white rounded-r-sm':'bg-white text-blue-400 rounded-r-sm border border-blue-400' } focus:outline-none" on:click={controlPreview}>Preview</button>
      </div>
      <div class="flex flex-row space-x-2 items-center justify-between">
        <div>
          <button class="p-1 px-2 text-white bg-blue-500  border border-blue-600 rounded-sm hover:bg-green-500 focus:outline-none">Save Draft</button>
        </div>
        <div>
           <button class="p-1 px-2 text-white bg-blue-500 border border-blue-600 rounded-sm hover:bg-green-500 focus:outline-none">Publish Report</button>
        </div>
      </div>
      
   </div>
   <!-- top toolbar 2 -->
    <div class="relative flex flex-row items-center border justify-between space-x-2 {preview?'hidden':''}">
        <div class="flex flex-row">
        <!-- edit tools -->
        <ul class="inline-flex border-r">
            <li>
            <svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.121 14.121L19 19m-7-7l7-7m-7 7l-2.879 2.879M12 12L9.121 9.121m0 5.758a3 3 0 10-4.243 4.243 3 3 0 004.243-4.243zm0-5.758a3 3 0 10-4.243-4.243 3 3 0 004.243 4.243z" />
            </svg>
            </li>
            <li><svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
            </svg></li>
            <li><svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg></li>
            <li><svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z" clip-rule="evenodd" />
            </svg></li>
        </ul>
        
        <!-- undo/redo -->
        <ul class="inline-flex border-r">
            <li><svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 15l-3-3m0 0l3-3m-3 3h8M3 12a9 9 0 1118 0 9 9 0 01-18 0z" />
            </svg></li>
            <li><svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 9l3 3m0 0l-3 3m3-3H8m13 0a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg></li>
        </ul>
        <!-- zoom controls -->
        <ul class="inline-flex border-r">
            <li on:click={(e)=>zoomIn()}><svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0zM10 7v3m0 0v3m0-3h3m-3 0H7" />
            </svg></li>
            <li class="pt-1.5"><span class="text-xs font-medium">{scale*100}%</span></li>
            <li on:click={(e)=>zoomOut()}><svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0zM13 10H7" />
            </svg></li>
        </ul>
        </div>
        <!-- drop down -->
        <ul class="inline-flex border-l relative">
        <li class="flex flex-row items-center" on:click={(e)=> viewOpen=true}>
            <svg class="w-8 h-8 p-1.5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path d="M3 4a1 1 0 011-1h12a1 1 0 011 1v2a1 1 0 01-1 1H4a1 1 0 01-1-1V4zM3 10a1 1 0 011-1h6a1 1 0 011 1v6a1 1 0 01-1 1H4a1 1 0 01-1-1v-6zM14 9a1 1 0 00-1 1v6a1 1 0 001 1h2a1 1 0 001-1v-6a1 1 0 00-1-1h-2z" />
            </svg>
            <svg class="w-5 h-5 {viewOpen?'hidden':''}" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
            </svg>
            <svg class="w-5 h-5 {!viewOpen?'hidden':''}" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M14.707 12.707a1 1 0 01-1.414 0L10 9.414l-3.293 3.293a1 1 0 01-1.414-1.414l4-4a1 1 0 011.414 0l4 4a1 1 0 010 1.414z" clip-rule="evenodd" />
            </svg>
        </li>
        <div class="fixed bg-black opacity-0 top-0 left-0 right-0 bottom-0 z-30 {!viewOpen?'hidden':''}" on:click={(e)=> viewOpen=false}></div>
        <div class="fixed top-16 right-0 mt-2.5 mr-1 w-max bg-white border rounded-sm shadow-md z-30 {!viewOpen?'hidden':''}">
            <ul class="flex-inline">
            <li class="p-2 pr-5 cursor-pointer" on:click={onviewheader}>
                <span class="inline-block w-4 h-4 text-gray-400">
                <svg class="inline-block w-4 h-4 {!viewHeader?'hidden':''}" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                </svg>
                </span>
                <span>
                    <svg class="h-6 w-6 inline-block" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 30" x="0px" y="0px"><title>header</title><path d="M17,2H7A2.00229,2.00229,0,0,0,5,4V20a2.00229,2.00229,0,0,0,2,2H17a2.00229,2.00229,0,0,0,2-2V4A2.00229,2.00229,0,0,0,17,2Zm0,18H7V4H17ZM9,7h6a1,1,0,0,0,0-2H9A1,1,0,0,0,9,7Z"/></svg>
                </span> Show Header</li>
            <li class="border-b p-2 pr-5 cursor-pointer" on:click={onviewfooter}>
                <span class="inline-block w-4 h-4 text-gray-400">
                <svg class="inline-block w-4 h-4 {!viewFooter?'hidden':''}" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                </svg>
                </span>
                <span>
                <svg class="h-6 w-6 inline-block" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 30" x="0px" y="0px"><title>footer</title><path d="M17,2H7A2.00229,2.00229,0,0,0,5,4V20a2.00229,2.00229,0,0,0,2,2H17a2.00229,2.00229,0,0,0,2-2V4A2.00229,2.00229,0,0,0,17,2Zm0,18H7V4H17ZM9,19h6a1,1,0,0,0,0-2H9a1,1,0,0,0,0,2Z"/></svg>
                <span>
                Show footer</li>
            <li class="p-2 pr-5 cursor-pointer" on:click={onviewgrid}>
                <span class="inline-block w-4 h-4 text-gray-400">
                    <svg class="inline-block w-4 h-4 {!viewGrid?'hidden':''}" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                    </svg>
                </span>
                <span>
                <svg class="h-6 w-6 inline-block" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" x="0px" y="0px" viewBox="0 0 100 125" style="enable-background:new 0 0 100 100;" xml:space="preserve"><path d="M92,6H71H50H29H8C6.9,6,6,6.9,6,8v21v21v21v21c0,1.1,0.9,2,2,2h21h21h21h21c1.1,0,2-0.9,2-2V71V50V29V8C94,6.9,93.1,6,92,6z   M90,48H73V31h17V48z M69,48H52V31h17V48z M48,48H31V31h17V48z M27,48H10V31h17V48z M10,52h17v17H10V52z M31,52h17v17H31V52z M52,52  h17v17H52V52z M73,52h17v17H73V52z M90,27H73V10h17V27z M69,27H52V10h17V27z M48,27H31V10h17V27z M10,10h17v17H10V10z M10,73h17v17  H10V73z M31,73h17v17H31V73z M52,73h17v17H52V73z M90,90H73V73h17V90z"/></svg>
                <span>
                Show Grid</li>
            <li class="p-2 pr-5 cursor-pointer" on:click={onviewfullscreen}>
                <span class="inline-block w-4 h-4 text-gray-400">
                <svg class="inline-block w-4 h-4 {!isFullscreen?'hidden':''}" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                </svg>
                </span>
                <span>
                    <svg class="h-6 w-6 inline-block" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" x="0px" y="0px" viewBox="0 0 48 60" style="enable-background:new 0 0 48 48;" xml:space="preserve"><title>full-screen</title><g><path d="M3.2,9.4v-4L14,16.2c0.6,0.6,1.6,0.6,2.1,0c0.5-0.6,0.6-1.6,0-2.1L5.4,3.3h4.1c0.8,0,1.5-0.7,1.5-1.5s-0.7-1.5-1.5-1.5H1.6   c-0.1,0-0.2,0.1-0.3,0.1c-0.1,0-0.2,0-0.3,0H0.9C0.7,0.5,0.5,0.7,0.4,0.9V1c0,0.1-0.1,0.2-0.1,0.3v0.1v0.1c0,0,0,0.1,0,0.2v7.7   c0,0.8,0.6,1.5,1.5,1.5l0,0C2.6,10.9,3.2,10.2,3.2,9.4z"/><path d="M38.5,3.2h4L31.8,14c-0.6,0.6-0.6,1.6,0,2.1c0.6,0.5,1.6,0.6,2.1,0L44.7,5.4v4.1c0,0.8,0.7,1.5,1.5,1.5s1.5-0.7,1.5-1.5   V1.8c0-0.1,0-0.1,0-0.2s-0.1-0.2-0.1-0.3S47.5,1.1,47.5,1V0.9c-0.1-0.2-0.3-0.4-0.5-0.5h-0.1c-0.1,0-0.2-0.1-0.3-0.1h-0.1h-0.1   h-7.9c-0.8,0-1.5,0.6-1.5,1.5l0,0C37,2.7,37.7,3.3,38.5,3.2z"/><path d="M1.4,47.6c-0.1,0-0.2-0.1-0.3-0.1H1c-0.2-0.1-0.4-0.3-0.5-0.5v-0.1c0-0.1-0.1-0.2-0.1-0.3v-0.1v-0.1c0,0,0-0.1,0-0.2v-7.7   c0-0.8,0.7-1.5,1.5-1.5l0,0c0.8,0.1,1.4,0.7,1.4,1.5v4l10.8-10.7c0.6-0.6,1.6-0.6,2.1,0c0.5,0.6,0.6,1.6,0,2.1L5.4,44.7h4.1   c0.8,0,1.5,0.7,1.5,1.5s-0.7,1.5-1.5,1.5H1.7"/><path d="M38.6,44.7h4L31.8,34c-0.6-0.6-0.6-1.6,0-2.1s1.6-0.6,2.1,0l10.8,10.7v-4.1c0-0.8,0.7-1.5,1.5-1.5s1.5,0.7,1.5,1.5v7.7   c0,0.1,0,0.1,0,0.2c0,0.1-0.1,0.2-0.1,0.3s-0.1,0.2-0.1,0.3v0.1c-0.1,0.2-0.3,0.4-0.5,0.5h-0.1c-0.1,0-0.2,0.1-0.3,0.1h-0.1h-0.1   h-7.9c-0.8,0-1.5-0.6-1.5-1.5l0,0C37.1,45.4,37.8,44.8,38.6,44.7z"/></g></svg>
                </span>
                Full screen</li>
            </ul>
        </div>
        </ul>
    </div>
   
    <div class="flex-auto flex flex-row border shadow-xl z-20 {preview?'hidden':''}">
        <!-- left vertical toolbar -->
        <div class="flex-none {lcollapse?'w-4':'w-40'} flex flex-col z-20" on:resize={handleResize}>
          {#if !lcollapse}
          <!-- search box -->
          <div class=" z-20 flex flex-row text-sm items-center bg-white m-1 mr-5 p-1 border rounded-sm mt-1">
              <input type="text" class="w-full hover:outline-none focus:outline-none bg-transparent" placeholder="Search">
              <svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
              </svg>
          </div>
          {/if}
          <!-- collapse control -->
          <div 
                class="z-20 absolute {lcollapse?'':'ml-36'} mt-1.5 border rounded-full bg-white w-6 h-6 p-0.5 shadow hover:bg-blue-400 hover:text-white hover:border-blue-400"
                on:click={lefttoggle}>
              {#if !lcollapse}  
              <svg class="w-4 h-4 pl-0.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
              </svg>
              {/if}
              {#if lcollapse}  
              <svg class="w-4 h-4 pl-0.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
              </svg>
              {/if}
          </div>
          {#if !lcollapse}
          <!-- accordion item 1 -->
          
          <div class=" z-20 flex flex-col bg-gray-100">
            <!-- accordion header -->
            <div class="flex flex-row justify-between items-center p-1 bg-gray-100 border-t border-b" on:click={(e)=>controls=!controls}>
              <span class="px-2">Controls</span>
              <svg class="w-4 h-4 {!controls?'hidden':''}" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
              </svg>
              <svg class="w-4 h-4 {controls?'hidden':''}" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 12H6" />
              </svg>
            </div>
            <!-- accordion body -->
            <div class="grid grid-cols-2 bg-white {controls?'hidden':''}">
              <div class="flex flex-col items-center border hover:bg-gray-200" 
                  draggable={true}
                  on:dragstart={event=>dragstart(event, "textelement")}>
                  <span class="text-yellow-500 text-4xl font-semibold px-1"  >A</span>
                  <span class="text-xs pb-1">Text</span>
              </div>
              <div class="flex flex-col items-center border  hover:bg-gray-200" draggable={true}
                on:dragstart={event=>dragstart(event, "imageelement")}>
                <svg class="h-10 w-10 p-1 text-blue-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
                <span class="text-xs pb-1">Image</span>
              </div>
              <div class="flex flex-col items-center border hover:bg-gray-200" 
                   draggable={true}
                   on:dragstart={event=>dragstart(event, "lineelement")}>
                <svg class="h-10 w-10 p-2 text-blue-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <line x1="0" y1="0" x2="200" y2="200" style="stroke-width:2" />
                </svg>
                <span class="text-xs pb-1">Line</span>
              </div>
              <div class="flex flex-col items-center border  hover:bg-gray-200 hidden"
                  draggable={true}
                  on:dragstart={event=>dragstart(event, "boxelement")}>
                <svg class="h-10 w-10 p-2 text-blue-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <rect x="0" y="0" rx="0" ry="0" width="24" height="24" style="stroke-width:4;"/>
                </svg>
                <span class="text-xs pb-1">Box</span>
              </div>
              <div class="flex flex-col items-center border  hover:bg-gray-200"
                    draggable={true}
                    on:dragstart={event=>dragstart(event, "tableelement")}>
                <svg class="h-10 w-10 p-2 text-blue-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h18M3 14h18m-9-4v8m-7 0h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z" />
                </svg>
                <span class="text-xs pb-1">Table</span>
              </div>
            </div>
          </div>
         
          <!-- accordion item 2 -->
          
          <div class="z-20 flex flex-col bg-gray-100 hidden">
            <div class="flex flex-row justify-between items-center p-1 bg-gray-100 border-t border-b" on:click={(e)=>charts=!charts}>
              <!-- accordion header -->
              <span class="px-2">Charts</span>
              <svg class="w-4 h-4 {!charts?'hidden':''}" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
              </svg>
              <svg class="w-4 h-4 {charts?'hidden':''}" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 12H6" />
              </svg>
            </div>
            <!-- accordion body -->
            <div class="grid grid-flow-col grid-cols-2 gap-2 bg-white {charts?'hidden':''}">
              
              <div class="flex flex-col items-center border">
                <svg class="h-10 w-10 p-2 text-blue-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                  <path d="M2 10a8 8 0 018-8v8h8a8 8 0 11-16 0z" />
                  <path d="M12 2.252A8.014 8.014 0 0117.748 8H12V2.252z" />
                </svg>
                <span class="text-xs pb-1">Pie</span>
              </div>
              <div class="flex flex-col items-center border">
                <svg class="h-10 w-10 p-2 text-blue-400" viewBox="0 0 20 20" fill="currentColor">
                  <path d="M2 11a1 1 0 011-1h2a1 1 0 011 1v5a1 1 0 01-1 1H3a1 1 0 01-1-1v-5zM8 7a1 1 0 011-1h2a1 1 0 011 1v9a1 1 0 01-1 1H9a1 1 0 01-1-1V7zM14 4a1 1 0 011-1h2a1 1 0 011 1v12a1 1 0 01-1 1h-2a1 1 0 01-1-1V4z" />
                </svg>
                <span class="text-xs pb-1">Bar</span>
              </div>
              <div class="flex flex-col items-center border">
                <svg class="h-10 w-10 p-2 text-blue-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <polyline points="0,10 1,7 2,10 7,1 8,10 12,4 20,13 20,12" style="fill:none;stroke-width:2" />            
                </svg>
                <span class="text-xs pb-1">Line</span>
              </div>
              
            </div>
          </div>
          
          {/if}
        </div>
        <!-- container -->
        <div class="flex-auto flex flex-col " on:resize={handleResize}  >
            <!-- container -->
            <div bind:this={designcontainer} 
                    class="flex-auto block bg-gray-300 overflow-auto" 
                    style="max-height:{containerHeight}; max-width:{containerWidth}">
                
                    <div bind:this={designholder} 
                        on:mousemove={holdermousemove}
                        on:mouseup={mouseup}
                        on:click|preventDefault={pageClick}
                        class="mt-10 ml-10" style="transform: scale({scale}, {scale}); transform-origin: 0px 0px;">
                        <!-- rulars -->
                        {#if false}
                        <div class="absolute h-8 -mt-16 -ml-8 inset-x-0 bg-gray-600"></div>
                        <div class="absolute w-8 -mt-8 -ml-16 bg-gray-600" style="top:0px; bottom:0;"></div>
                        {/if}
                        <!-- grid -->
                        <div bind:this={grid} class="absolute m-2 bg-white" style="top:0px; left:0px; width:{canvasWidth}px; height:{canvasHeight}px;">
                            <svg class="{!viewGrid?'hidden':''}" width="{canvasWidth}px" height="{canvasHeight}px"> 
                            <g>
                                {#each [...Array(parseInt(canvasWidth/9.6+1)).keys()].map(function(i,j) { return {value:i*9.6, even:(i+1)%11==0};}) as dot }
                                <line x1={dot.value} y1="0" x2={dot.value} y2="100%" style="stroke:{dot.even?'#dbeafe':'#fce7f3'}; stroke-width:0.5; fill:none;"></line>
                                {/each}
                                {#each [...Array(parseInt(canvasHeight/9.6+1)).keys()].map(function(i,j) { return {value:i*9.6, even:(i+1)%11==0};}) as dot }
                                <line x1="0" y1={dot.value} x2="100%" y2={dot.value} style="stroke:{dot.even?'#dbeafe':'#fce7f3'}; stroke-width:0.5; fill:none;"></line>
                                {/each}
                            </g>
                            </svg>
                        </div>
                        <!-- header -->
                        <div bind:this={header} 
                            class="absolute mx-2 bg-transparent  {!viewHeader?'hidden':''} resize-y" style="top:0px; left:0px; width:{canvasWidth}px; height:{headerHeight}px;"
                            ondragover="return false"
                            on:drop|preventDefault={event => drop(event, "headersection")}
                            on:click|preventDefault={onclick}>
                            <div class="relative bg-blue-600 px-1 pt-0.5 -mt-1 w-max text-xs text-white border border-blue-600 rounded-tr-xl" style="top:{headerHeight-22+3}px; left:-36px; font-size: 8px;" on:click={(e)=>{target=null;}}>Header</div>
                            <div class="absolute border border-b-1 border-blue-600 row-resize" style="top:{headerHeight-3}px; left:-36px; right:0px; cursor: row-resize" 
                            on:click={(e)=>{target=null;}}
                            on:mousedown={onheadermousedown}
                            on:mouseup={mouseup}
                            
                            ></div>
                            <!-- header elements -->
                            {#each headerelements as be}
                              {#if be.type=='textelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement {report.hasTextboxBorderStyle(be.data)?'': 'border border-dashed border-gray-400'} {report.textbox.css.textAlign(be.data)} {report.textbox.css.verticalAlign(be.data)}" id={"headerelement"+elements.indexOf(be)}
                                    style="z-index:3; left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute;">
                                    <div class="w-full h-full" style="{report.textbox.cssFontStyle(be.data)}">{report.getTextBoxContentDisplay(be.data)}</div>
                                </div>
                              {/if}
                              {#if be.type=='lineelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"headerelement"+elements.indexOf(be)}
                                    style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%">
                                      <line x1="0" y1="0" x2="100%" y2="100%" 
                                            stroke="{report.line.stroke(be.data)}" 
                                            stroke-width="{report.line.strokeWidth(be.data)}" 
                                            stroke-dasharray="{report.line.strokeStyle(be.data)}"/>
                                    </svg>
                                </div>
                              {/if}
                              {#if be.type=='imageelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"headerelement"+elements.indexOf(be)}
                                    style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <img src={report.getEmbeddedImageData(reportObject, be.data.value)} style="width: 100%; height: 100%; object-fit:contain; {report.getImageStyle(be.data)}" alt={be.data.name}/>
                                </div>
                              {/if}
                              {#if be.type=='boxelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"headerelement"+elements.indexOf(be)}
                                    style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%"><rect x="0" y="0" width="100%" height="100%" stroke="black" fill="none" stroke-width="1.5"/></svg>
                                </div>
                              {/if} 
                            {/each}
                            <!-- <div class="border border-red-200" style="left: 0px; top:0px; position: absolute">TITLE</div> -->
                          </div>
                       <!-- body -->
                        <div bind:this={bodysection}
                          id="bodysection"
                          ondragover="return false"
                          on:drop|preventDefault={event => drop(event, "bodysection")}
                          on:click|preventDefault={(e)=>target=null}
                          class="absolute m-2" style="top:{(viewHeader?headerHeight-10:0)}px; left:0px; width:{canvasWidth}px; height:{bodyHeight}px;">
                          <!-- body elements -->
                          {#each bodyelements as be}
                            {#if be.type=='textelement'}
                              <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                  contenteditable={be.editing}
                                  class="itemelement  {report.hasTextboxBorderStyle(be.data)?'': 'border border-dashed border-gray-400'}  {report.textbox.css.textAlign(be.data)} {report.textbox.css.verticalAlign(be.data)}" id={"bodyelement"+elements.indexOf(be)}
                                  style="z-index:3; left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute;">
                                  <div class="w-full h-full" style="{report.textbox.cssFontStyle(be.data)}">{report.getTextBoxContentDisplay(be.data)}</div>
                              </div>
                            {/if}
                            {#if be.type=='lineelement'}
                              <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                  class="itemelement text-black" id={"bodyelement"+elements.indexOf(be)}
                                  style="z-index:3; left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%">
                                    <line x1="0" y1="0" x2="100%" y2="100%" 
                                          stroke="{report.line.stroke(be.data)}" 
                                          stroke-width="{report.line.strokeWidth(be.data)}" 
                                          stroke-dasharray="{report.line.strokeStyle(be.data)}"/>
                                  </svg>
                              </div>
                            {/if}
                            {#if be.type=='imageelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"bodyelement"+elements.indexOf(be)}
                                    style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <img src={report.getEmbeddedImageData(reportObject, be.data.value)} style="width: 100%; height: 100%; object-fit:contain; {report.getImageStyle(be.data)}" alt={be.data.name}/>
                                </div>
                              {/if}
                            {#if be.type=='boxelement'}
                              <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                  class="itemelement text-black" id={"bodyelement"+elements.indexOf(be)}
                                  style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%"><rect x="0" y="0" width="100%" height="100%" stroke="black" fill="none" stroke-width="1.5"/></svg>
                              </div>
                            {/if}       
                            {#if be.type=='tableelement'}
                                <div on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"bodyelement"+elements.indexOf(be)}
                                    style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <Table data={be.data} 
                                        dataset={dataset} 
                                        width={be.position.width}
                                        on:tableselected={(e)=>selectedTable(elements.indexOf(be), e.detail.col)}
                                        on:cornercellclick={(e)=>selectTableObject("bodyelement"+elements.indexOf(be), elements.indexOf(be))}
                                        on:selectrow= {(e)=>onSelectTableRow(be.data, e.detail.row, elements.indexOf(be))}
                                        on:selectcell={(e)=>onSelectTableCell(be.data, e.detail.row, e.detail.col, elements.indexOf(be))}
                                        on:addtext={(e)=>onAddEditTextInTableCell(be.data, e.detail.row, e.detail.col, elements.indexOf(be))}
                                        on:addexpression={(e)=>onAddEditExpressionInTableCell(be.data, e.detail.row, e.detail.col, elements.indexOf(be))} />
                                </div>
                            {/if}      
                          {/each}
                         
                       </div>
                        <!-- footer -->
                        <div bind:this={footer}
                            class="mx-2 bg-transparent  {!viewFooter?'hidden':''} resize-y" style="top:{canvasHeight-footerHeight}px; height:{footerHeight}px; left:0px; width:{canvasWidth}px; position: absolute"
                            ondragover="return false"
                            on:drop|preventDefault={event => drop(event, "footersection")}
                            on:click|preventDefault={onclick}>
                            <div class="absolute border border-t-1 border-blue-600" style="top:{0}px; left:-34px; right:0px; cursor:row-resize"  
                            on:mousedown={onfootermousedown}
                            on:mouseup={mouseup}
                            on:click={(e)=>{target=null;}}></div>
                            <div class="absolute bg-blue-600 px-1 mt-0.5 w-max text-white border border-blue-600 rounded-br-xl" style="font-size: 8px; left:-34px; " on:click={(e)=>{target=null;}}>Footer</div>
                            <!-- footer elements -->
                            {#each footerelements as be}
                              {#if be.type=='textelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement {report.hasTextboxBorderStyle(be.data)?'': 'border border-dashed border-gray-400'} {report.textbox.css.textAlign(be.data)} {report.textbox.css.verticalAlign(be.data)}" id={"footerelement"+elements.indexOf(be)}
                                    style="z-index:3; left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute;">
                                    <div class="w-full h-full" style="{report.textbox.cssFontStyle(be.data)}">{report.getTextBoxContentDisplay(be.data)}</div>
                                </div>
                              {/if}
                              {#if be.type=='lineelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"footerelement"+elements.indexOf(be)}
                                    style="z-index:3; left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%">
                                      <line x1="0" y1="0" x2="100%" y2="100%" 
                                            stroke="{report.line.stroke(be.data)}" 
                                            stroke-width="{report.line.strokeWidth(be.data)}" 
                                            stroke-dasharray="{report.line.strokeStyle(be.data)}"/>
                                    </svg>
                                </div>
                              {/if}
                              {#if be.type=='imageelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"footerelement"+elements.indexOf(be)}
                                    style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <img src={report.getEmbeddedImageData(reportObject, be.data.value)} style="width: 100%; height: 100%; object-fit:contain; {report.getImageStyle(be.data)}" alt={be.data.name}/>
                                </div>
                              {/if}
                              {#if be.type=='boxelement'}
                                <div on:click|preventDefault={(e)=>onclick(e, elements.indexOf(be))} on:contextmenu|preventDefault={(e)=>onRightClick(e, elements.indexOf(be))}
                                    class="itemelement text-black" id={"footerelement"+elements.indexOf(be)}
                                    style="z-index:3;left: {be.position.x}px; top:{be.position.y}px; height:{be.position.height}px; width:{be.position.width}px; position: absolute">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%"><rect x="0" y="0" width="100%" height="100%" stroke="black" fill="none" stroke-width="1.5"/></svg>
                                </div>
                              {/if}       
                            {/each}
                            <!-- <div class="border border-red-200" style="left: 0px; top:0px; position: absolute">Footer titles</div> -->
                        </div>
                        
                        
                        <div class="relative mx-2 border border-b-2 border-gray-400" style="left:0px; top:{canvasHeight+6}px; width:{canvasWidth}px; cursor:row-resize;" 
                                on:click={(e)=>{target=null;}}
                                on:mousedown={onmousedownhorizontal}
                                on:mouseup={mouseup}
                                ></div>
                        <div class="relative my-2 border border-r-2 border-gray-400" style="top:0px; left:{canvasWidth+6}px; width:1px; height:{canvasHeight-5}px; cursor:col-resize;"
                            on:click={(e)=>{target=null;}}
                            on:mousedown={onmousedownvertical}
                            on:mouseup={mouseup}
                        ></div>
                    </div>
                    <Moveable
                      target={target}
                      resizable={true}
                      draggable={true}
                      snappable={true}
                      snapThreshold={5}
                      isDisplaySnapDigit={false}
                      bounds = {bounds}
                      throttleResize={0}
                      rotatable={false}
                      zoom={1}
                      throttleDrag={0}
                      startDragRotate={0}
                      throttleDragRotate={0}
                      origin={false}
                      padding={{"left":0,"top":0,"right":0,"bottom":0}}
                      pinchable={false}
                      edgeDraggable={true}
                      
                      on:resizeStart={({ detail: {target, set, setOrigin, dragStart }}) => {
                          // Set origin if transform-orgin use %.
                          setOrigin(["%", "%"]);
                  
                          // If cssSize and offsetSize are different, set cssSize. (no box-sizing)
                          const style = window.getComputedStyle(target);
                          const cssWidth = parseFloat(style.width);
                          const cssHeight = parseFloat(style.height);
                          set([cssWidth, cssHeight]);
                  
                          // If a drag event has already occurred, there is no dragStart.
                          dragStart && dragStart.set(frame.translate);
                      }}
                      on:resize={({ detail: { target, width, height, drag }}) => {
                          
                          target.style.width = `${width}px`;
                          target.style.height = `${height}px`;
                          let index = -1;
                          let m= target.id.match(/element(\d+)/);
                          if(m) index = parseInt(m[1]);
                          if(index>-1) frame = frames[index];
                          // get drag event
                          frame.translate = drag.beforeTranslate;
                          target.style.transform
                              = `translate(${drag.beforeTranslate[0]}px, ${drag.beforeTranslate[1]}px)`;
                          
                          if(index>-1){
                            let el = elements[index];
                            el.position = {
                                x: target.offsetLeft,
                                y: target.offsetTop,
                                width: Math.max(width,1),
                                height: Math.max(height,1)
                            };
                            elements=[...elements];
                          }
                      }}
                      on:resizeEnd={({ detail: { target, isDrag, clientX, clientY }}) => {
                          let index = -1;
                          let m= target.id.match(/element(\d+)/);
                          if(m) index = parseInt(m[1]);
                          if(index>-1){
                            let el = elements[index];
                            const tran = (target.style.transform||"").match(/translate\((?<x>-?\d+)px,\s*(?<y>-?\d+)px\)/);
                            let tx=0, ty=0;  
                            if(tran && tran.groups){
                               tx=parseInt(tran.groups.x);
                               ty=parseInt(tran.groups.y);
                            }
                            if(el.type=="textelement" || el.type=="lineelement"|| el.type=="imageelement"|| el.type=="tableelement"){
                              let obj = report.findObjectByName(reportObject, el.data.name).object;
                              obj.left= report.px2cmFormat(`${el.position.x+tx}`);
                              obj.top= report.px2cmFormat(`${el.position.y+ty}`);
                              obj.width = report.px2cmFormat(`${el.position.width}`);
                              obj.height = report.px2cmFormat(`${el.position.height}`);
                              reportObject=Object.assign({}, reportObject);
                              el.data = report.findObjectByName(reportObject, el.data.name).object;
                            }
                            elements=[...elements];
                          }
                      }}
                      on:dragStart={({ detail: {target, set } }) => {
                        let index = -1;
                        let m= target.id.match(/element(\d+)/);
                        if(m) index = parseInt(m[1]);
                        if(index>-1) frame = frames[index];
                        set(frame.translate)
                      }}
                      on:drag={({ detail: { target, beforeTranslate }}) => {
                        let index = -1;
                        let m= target.id.match(/element(\d+)/);
                        if(m) index = parseInt(m[1]);
                        if(index>-1) frame = frames[index];

                          frame.translate = beforeTranslate;
                          target.style.transform
                              = `translate(${beforeTranslate[0]}px, ${beforeTranslate[1]}px)`;
                      }}
                      on:dragEnd={({ detail: { target, isDrag, clientX, clientY }}) => {
                          let index = -1;
                          let m= target.id.match(/element(\d+)/);
                          if(m) index = parseInt(m[1]);
                          if(index>-1){
                            let el = elements[index];
                            
                            el.position = {
                                x: target.offsetLeft,
                                y: target.offsetTop,
                                width: el.position.width,
                                height: el.position.height
                            };
                            const tran = (target.style.transform||"").match(/translate\((?<x>-?\d+)px,\s*(?<y>-?\d+)px\)/);
                            let tx=0, ty=0;  
                            if(tran && tran.groups){
                               tx=parseInt(tran.groups.x);
                               ty=parseInt(tran.groups.y);
                            }
                            if(el.type=="textelement" || el.type=="lineelement"|| el.type=="imageelement" || el.type=="tableelement"){
                              let obj = report.findObjectByName(reportObject, el.data.name).object;
                              obj.left= report.px2cmFormat(`${el.position.x+tx}`);
                              obj.top= report.px2cmFormat(`${el.position.y+ty}`);
                              obj.width = report.px2cmFormat(`${el.position.width}`);
                              obj.height = report.px2cmFormat(`${el.position.height}`);
                              reportObject=Object.assign({}, reportObject);
                              el.data = report.findObjectByName(reportObject, el.data.name).object;
                            }
                            elements=[...elements];
                          }
                      }}
                    />
                    {#if showContextMenu}
                      <Menu {...contextpos} on:click={closeMenu} on:clickoutside={closeMenu}>
                        <MenuOption 
                          on:click={deleteElement} 
                          text="Delete" />
                         {#if currentElementIndex>=0 && elements[currentElementIndex].type=="textelement"}
                              <MenuOption 
                                  on:click={(e) => editTextElementByIndex(e, currentElementIndex)} 
                                  text={"Edit Text"} />
                              <MenuOption 
                                  on:click={(e) => editExpressionElementByIndex(e, currentElementIndex)} 
                                      text={"Edit Expression"} />
                         {/if} 
                      </Menu>
                    {/if}
            </div>
            

            <!-- group info -->
            <div class="bg-blue-200  z-20">groups</div>
        </div>
        <!-- right properties drawer -->
        <div class="absolute right-8 bottom-4 top-16 mt-2 flex-none w-96 flex flex-col border-r border-r shadow-2xl text-gray-500 bg-white z-20 {rcollapse?'hidden':''}">
          {#if selectedProp==="Property" }
            <PropertyBox 
                    title="Properties" 
                    bind:reportData={reportObject} 
                    bind:objectType={selectedObjectType} 
                    bind:obj={selectedObject}
                    on:dataChange={report_object_change}
                    on:add_data_source_init={add_data_source_init}
            />
          {/if}
          {#if selectedProp==="Dataset" }
              <Datasource 
                bind:reportData={reportObject} 
                on:datasourcechange={report_object_change} initiate={dsInitiate} />
          {/if}
          {#if selectedProp==="Parameters" }
              <ReportParameter reportData={reportObject} on:parameterchange={report_object_change} />
          {/if}
          {#if selectedProp==="Filters" && showFilters }
              <ReportFilter reportData={reportObject} table={selectedObject} on:filterchange={table_object_change} />
          {/if}
          {#if selectedProp==="Images" }
              <ReportImage reportData={reportObject} on:imagechanged={report_object_change} />
          {/if}
        </div>
        <!-- right toolbar -->
        <div class="flex-none w-8 flex flex-col z-20">
          <!-- right toolbar collapse button -->  
          <div class="w-8 h-5 text-gray-500 border-b hover:bg-gray-300" on:click={(e)=> rcollapse=!rcollapse}>
            {#if rcollapse}
            <svg class="w-5 h-5 p-0.5 mx-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
            </svg>
            {/if}
            {#if !rcollapse}
            <svg class="w-5 h-5 p-0.5 mx-1.5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
            </svg>
            {/if}
          </div>
          <div on:click={(e)=> {selectedProp="Property"; rcollapse=false;} }
              class="w-8 h-8 p-1 {selectedProp=="Property"?"bg-gray-500 text-white":"text-gray-500"} border-b hover:text-white hover:bg-blue-500" >
              
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" />
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
              </svg>
          </div>
          <div 
            on:click={(e)=> { selectedProp="Dataset"; rcollapse=false;} }
            class="w-8 h-8 p-1 {selectedProp=="Dataset"?"bg-gray-500 text-white":"text-gray-500"} border-b hover:text-white hover:bg-blue-500">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 7v10c0 2.21 3.582 4 8 4s8-1.79 8-4V7M4 7c0 2.21 3.582 4 8 4s8-1.79 8-4M4 7c0-2.21 3.582-4 8-4s8 1.79 8 4m0 5c0 2.21-3.582 4-8 4s-8-1.79-8-4" />
          </svg>
          </div>
          <div on:click={(e)=> {selectedProp="Parameters"; rcollapse=false;} }
              class="w-8 h-8 p-1 {selectedProp=="Parameters"?"bg-gray-500 text-white":"text-gray-500"} border-b hover:text-white hover:bg-blue-500">
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
              <path fill="currentColor" d="M12 16C13.1 16 14 16.9 14 18S13.1 20 12 20 10 19.1 10 18 10.9 16 12 16M12 10C13.1 10 14 10.9 14 12S13.1 14 12 14 10 13.1 10 12 10.9 10 12 10M12 4C13.1 4 14 4.9 14 6S13.1 8 12 8 10 7.1 10 6 10.9 4 12 4M6 16C7.1 16 8 16.9 8 18S7.1 20 6 20 4 19.1 4 18 4.9 16 6 16M6 10C7.1 10 8 10.9 8 12S7.1 14 6 14 4 13.1 4 12 4.9 10 6 10M6 4C7.1 4 8 4.9 8 6S7.1 8 6 8 4 7.1 4 6 4.9 4 6 4M18 16C19.1 16 20 16.9 20 18S19.1 20 18 20 16 19.1 16 18 16.9 16 18 16M18 10C19.1 10 20 10.9 20 12S19.1 14 18 14 16 13.1 16 12 16.9 10 18 10M18 4C19.1 4 20 4.9 20 6S19.1 8 18 8 16 7.1 16 6 16.9 4 18 4Z" />
            </svg>
          </div>
          {#if showFilters}
          <div on:click={(e)=> {selectedProp="Filters"; rcollapse=false;} }
              class="w-8 h-8 p-1 {selectedProp=="Filters"?"bg-gray-500 text-white":"text-gray-500"} border-b hover:text-white hover:bg-blue-500">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z" />
            </svg>
          </div>  
          {/if}
          <div 
            on:click={(e)=> {selectedProp="Images"; rcollapse=false;} }
            class="w-8 h-8 p-1 {selectedProp=="Images"?"bg-gray-500 text-white":"text-gray-500"} border-b hover:text-white hover:bg-blue-500">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
            </svg>
          </div>           
        </div>
    </div>
    {#if preview }
    
    <div bind:this={previewcontainer} class="flex-auto flex flex-row">
        <div class="flex flex-col w-1/3 bg-gray-100 border-t {!hasParameters?'hidden':''}">
            <div class="font-bold border-b p-2 bg-gray-200">Report Parameters</div>
            <div class="flex flex-wrap content-start justify-evenly overscroll-y-auto">
            {#each (reportObject.reportParameters && reportObject.reportParameters.reportParameter && reportObject.reportParameters.reportParameter||[]) as p}
            <div class="flex flex-col m-4">
                <label for="p_{p.name}">{p.prompt}</label>
                {#if p.dataType==='String'}
                
                  <input type="text" bind:value={p.inputValue} id="p_{p.name}" placeholder="{p.prompt}" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
                
                {/if}
                {#if p.dataType==='Float'||p.dataType==='Integer'}
                
                  <input type="number" bind:value={p.inputValue} id="p_{p.name}" placeholder="{p.prompt}" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
                
                {/if}
                {#if p.dataType==='DateTime'}
               
                  <Flatpickr bind:value={p.inputValue}  id="p_{p.name}" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
               
                {/if}
                {#if p.dataType==='Boolean'}
                <div>
                  <input type="checkbox" id="p_{p.name}" bind:checked={p.inputValue} class="inline-block align-middle" />
                  <label class="inline-block align-middle" for="p_{p.name}">{p.inputValue?'True':'False'}</label>
                </div>
                {/if}
            </div>
            {/each}
           </div>
           <div class="flex flex-row items-center justify-center mt-2 border-t pt-4">
            <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm hover:outline-none focus:outline-none" on:click|preventDefault={showReport}>Show Report</button>
          </div>
        </div>
        <div class="flex-auto bg-gray-200 h-full overflow-y-auto {reportPdf==null?'flex flex-wrap content-center justify-items-center':''}" style="height:{previewHeight}px;">
            <!-- svelte-ignore a11y-missing-attribute -->
            {#if reportPdf!=null}
              <iframe src="{reportPdf}" height="100%" width="100%"></iframe>
            {:else}
              <p class="w-full text-center">Enter values in parameters and click Show Report.</p>
            {/if}
        </div>
    </div>
    {/if}
</div>
{/if}
<Dialog bind:visible={editTextDialog} title="Edit text" w="1/3" >
  <div class="px-2 py-2 pb-4 max-w-md">
      <label for="text"  class="block font-semibold">Content</label>
      <input type="text" bind:value={text} id="text" placeholder="Content" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
  </div>
  <div class="flex flex-row items-center justify-end mb-2">
    <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={updateContent}>OK</button>
    <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e) => editTextDialog=false}>Cancel</button>
  </div>
</Dialog>


<Dialog bind:visible={editExprDialog} title="Edit Expression" w="1/2" >
   <ExpressionEditor 
      expression={textExpr} 
      height={'100px'} 
      reportData={reportObject}
      on:expressionChange={updateContentExpr}
      on:close={(e)=> editExprDialog=false } />
</Dialog>

<Dialog bind:visible={imageDialog} title="Add Image" w="1/3" >
  <div class="px-2 py-2 pb-4 max-w-md">
      <label for="chosenImageName"  class="block font-semibold">Chosse Embedded Image</label>
      <select bind:value={chosenImageName} id="chosenImageName" name="chosenImageName" class="
          border w-full h-12 px-3 py-3 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm">
          {#each reportObject.embeddedImages.embeddedImage as img,index}
              <option value={img.name}>
                  {img.name}
              </option>
          {/each}
      </select>
  </div>
  <div class="flex flex-row items-center justify-end mb-2">
    <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={addImageFromIntent}>OK</button>
    <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={cancelImageIntent}>Cancel</button>
  </div>
</Dialog>

<style>
  [contenteditable="true"]:focus{
    border:none;
    outline:none;
  }
</style>