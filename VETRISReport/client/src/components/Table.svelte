
<svelte:body on:click|preventDefault={(e)=>{ !(showContextMenu||showRightClickMenu) && onPageClick(e)}} />
{#if data && data.tablixBody }

    <table class="absolute " bind:this={table} style="left:{showHeaders?-20:0}px;top:{showHeaders?-22:0}px; width: {totalWidth}px;">
        {#if showHeaders}
        <colgroup>
            <col style="width:20px; max-width: 20px;"/>
            {#each data.tablixBody.tablixColumns.tablixColumn as cell,index }
                {#if selectedCol===index && selectedRow===-1 }
                    <col class="selected" />
                {:else} 
                    <col />
                {/if}
            {/each}
        </colgroup>
        <tr>
            <th id={'tablehandlecell'} style="height:20px; width:20px; max-width: 20px; cursor:pointer;">
                <div style="height:20px; width:20px;" on:click={(e)=> onCornerHeaderClick(e) }>
                    
                </div>
            </th>
            {#each data.tablixBody.tablixColumns.tablixColumn as cell,index }
                <th id={'headercell-'+index} class="{selectedCol===index && selectedRow===-1?'selected':''}" style="min-height: 20px; position:relative; width: {parseInt(report.cmToPixels(data.tablixBody.tablixColumns.tablixColumn[index].width))}px !important;"> 
                    <div style="height:20px;width:100%;"  
                        on:click|preventDefault|stopPropagation={(e)=> {selectedCol=index; selectedRow=-1; selectedCols=null; dispatch("tableselected", {col:index});} }></div>
                </th>
            {/each}
        <tr/>
        {/if}
        {#each data.tablixBody.tablixRows.tablixRow as row, index}
        <tr class="{selectedRow===index && selectedCol<0 && selectedCols===null ?'selected':''}" >
            {#if showHeaders}
                <th id={'rowheader-'+index} 
                    on:click|preventDefault={(e)=>selectRow(e,index)}
                    on:contextmenu|preventDefault={(e)=>onRightClick(e, index,-1)}
                    class="{selectedRow===index && selectedCol===-1 && selectedCols===null?'selected':''}"
                    style="height:20px; width:20px;" >
                    {#if report.isGroupRow(data,index)}
                        <div class="relative" style="left:-1px;top:-1px;width:20px;">&#40;≡</div>
                    {/if}
                </th>
            {/if}
            {#each row.tablixCells.tablixCell as cell, colindex}
            <td class="relative {selectedRow===index && (selectedCols!==null?selectedCols.indexOf(colindex)!==-1:(selectedCol===colindex))?'selected':''} {report.row.cell.css.textAlign(row,colindex)} {report.row.cell.css.verticalAlign(row,colindex)}" 
                style="min-height: 20px;  position:relative; 
                    {report.row.cell.cssFontStyle(row,colindex)}
                    {!showHeaders?`width: ${parseInt(report.cmToPixels(data.tablixBody.tablixColumns.tablixColumn[colindex].width))}px;`:''}
                "
                id={'bodycell-'+index+'-'+colindex} 
                on:contextmenu|preventDefault={(e)=>onRightClick(e, index,colindex)}
                on:click|preventDefault={(e)=> selectCell(e, index, colindex) }
                on:mouseenter|preventDefault={(e)=> oncellover(e, index, colindex)}
                on:mouseleave|preventDefault={(e)=> oncellleave(e, index, colindex)}> 
                {#if !report.getField(row,colindex)}
                <div style="min-height: 20px;"></div>
                {/if}
                
                <div class="hidden absolute text-blue-500" 
                        style="top:0px; right:0px; width:14px;zIndex:40;" id={'menu-'+index+'_'+colindex}
                        on:click|preventDefault={(e)=>onContextClick(e, index,colindex)}>
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-5 mt-1 pr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 7v10c0 2.21 3.582 4 8 4s8-1.79 8-4V7M4 7c0 2.21 3.582 4 8 4s8-1.79 8-4M4 7c0-2.21 3.582-4 8-4s8 1.79 8 4m0 5c0 2.21-3.582 4-8 4s-8-1.79-8-4" />
                    </svg>    
                    <!-- <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                    </svg> -->
                </div>
                {report.getField(row,colindex)}
            </td>
            {/each}
        </tr>
        {/each}
    
    </table>


    {#if showContextMenu}
        <Menu {...contextpos} on:click={closeMenu} on:clickoutside={closeMenu}>
            
            <MenuOption  text="Fields" isDisabled=true />
            <div class="flex flex-row text-sm items-center bg-white m-1 ml-4 p-1 border rounded-sm mt-1">
                <input type="text" bind:value={mfsearch} class="w-full hover:outline-none focus:outline-none bg-transparent" placeholder="Search">
                <svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
                </svg>
            </div>
            <div class="w-full max-h-60 overflow-y-auto">
                {#each dataset as ds}
                    {#each ds.fields.field.filter(i=>mfsearch?i.dataField.toLowerCase().indexOf(mfsearch.toLowerCase()) >= 0 :true) as field}
                        <MenuOption  text={`${field.dataField}`} noClose=true on:click={(e)=>addField(cellId, field)}  />
                    {/each}
                {/each}
            </div>
            
            
        </Menu>
    {/if}
    {#if showRightClickMenu}
        <Menu {...contextpos} on:click={closeMenu} on:clickoutside={closeMenu}>
            {#if cellId.startsWith('bodycell')}
                {#if !hasMultiCell}
                    <MenuOption  text="Edit Text" isDisabled={(selectedCellContentType===2)} noClose=true on:click={(e)=>addText(e,cellId)}/>
                    
                    <MenuOption  text="Expression" isDisabled={(selectedCellContentType===1)} noClose=true on:click={(e)=>addExpression(e,cellId)}/>
                    
                    <MenuDivider />    
                    <MenuOption  text="Add Cloumn right" noClose=true on:click={(e)=>addColAfter(columnIndex)} />
                    <MenuOption  text="Add Column left" noClose=true on:click={(e)=>addColBefore(columnIndex)}/>
                    <MenuDivider/>  
                    <MenuOption  text="Delete Column" noClose=true on:click={(e)=>removeCol(columnIndex)}/>
                    <MenuDivider/>  
                {:else}
                    <MenuOption  text="Merge Cells" noClose=true on:click={(e)=>mergeCells(selectedRow, selectedCols)}/>
                {/if}
            {:else}
                <MenuOption  text="Add row before" noClose=true on:click={(e)=>addRow(cellId,-1)}/>
                <MenuOption  text="Add row after" noClose=true on:click={(e)=>addRow(cellId,1)}/>
                <MenuOption  text="Remove row" noClose=true on:click={(e)=>removeRow(cellId)}/>
            {/if}    
        </Menu>
    {/if}
{/if}  


<style>
	table, th, td {
		border: 1px solid #ada9a9;
		border-collapse: collapse;
        overflow: hidden;
        /*margin-bottom: 1px;*/
	}
   
	
	table {
        table-layout: fixed;
        border: 1px dashed rgba(156, 163, 175,1);
		/*width: 100%;*/
		text-align: left;
		border-collapse: collapse;
		/*margin-bottom: 1px;*/
	}

	td, th {
		/*padding: 1px 1px;*/
        min-width: 1px;
        border: 1px dashed rgba(156, 163, 175,1);
        min-height: 20px !important;
        text-overflow: ellipsis;
        overflow-wrap: anywhere;
        white-space: normal;
	}

    
    th {
        background-color: #b3b0b0b4;
    }
	td {
        background-color: transparent;
    }
    col.selected {
        border: 2px solid orange !important;
    }
    tr.selected {
        border: 2px solid orange !important;
    }
    .selected {
        border: 2px solid orange !important;
    }
   
	th.selected {
        background-color: orange !important;
        color: #fdfcfc;
    }	

    /* Removing all unwanted border
    from left hand side by calling
    all the elements in the first
    column and removing their left
    border*/
    table tr td:first-child, th:first-child{
        border-left: none;
    }
        
    /* Removing all unwanted border
    from top of the table by calling
    all the elements in first row and
    removing their top border*/
    table tr:first-child th{
        border-top: none;
    }
        
    /* Removing all unwanted border
    from right hand side by calling
    all the elements in last row and
    removing their right border*/
    table tr td:last-child, th:last-child{
        border-right: none;
    }
        
    /* Removing all unwanted border
    from bottom of the table by
    calling all the elements in
    last column and removing their
    bottom border*/
    table tr:last-child td{
        border-bottom: none;
    }
</style>

<script>
    import { onMount, createEventDispatcher } from "svelte";
    import * as report from "../model/report/report.js";
    import Menu from "./menu/Menu.svelte";
    import MenuDivider from "./menu/MenuDivider.svelte";
    import MenuOption from "./menu/MenuOption.svelte";

    export let data=null;
    export let width=0;
    export let dataset=[];
    let mfsearch;
    let totalWidth;
    $:{
        let cols = data.tablixBody.tablixColumns.tablixColumn;
        let current_total_col_width = 0;
        let nc=new Array(cols.length);
        cols.forEach((c,i) => {
            nc[i] = parseInt(report.cmToPixels(c.width));
            current_total_col_width += nc[i];
        });
        let diff=current_total_col_width-width;
        if(diff>0){
            let dx=diff/cols.length;
            if(dx<1) dx=1;
            else dx=Math.round(dx);
            for(let i=0; i<nc.length && diff>0; i++){
                nc[i]-=dx;
                diff-=dx;
            }
            if(diff>0) {
                nc[nc.length-1]-=diff;
            }
        }
        else if(diff<0){
            diff=-diff;
            let dx=diff/cols.length;
            if(dx<1) dx=1;
            dx=Math.round(dx);
            for(let i=0; i<nc.length && diff>0; i++){
                nc[i]+=dx;
                diff-=dx;
            }
            if(diff>0) {
                nc[nc.length-1]+=diff;
            }
        }
        // console.debug("width: "+width);
        // console.debug("total_cols_width: "+current_total_col_width);
        
        cols.forEach((c,i) => {
            c.width=  `${report.px2cmFormat(`${nc[i]}px`)}`;
        });
        data = Object.assign({},data);
        totalWidth = (showHeaders? 20 : 0)+width;
    }

    let container;
    let showcolumnOptions=false;
    let showHeaders=true;
    let table;
    let selectedRow=-1, selectedCol=-1, selectedCols=null;
    let selectedCellContentType=0;

    $: hasMultiCell = selectedCols!=null;

    const dispatch = createEventDispatcher();	

    onMount(()=>{
        setTimeout(()=> {if(showHeaders) makeResizableGrid(table)}, 10);
    });

    
    export function addColBefore(index) {
        report.addColumnLeft(data, index);
        data = Object.assign({}, data);
        selectedRow=-1;selectedCol=-1;
        setTimeout(()=> {if(showHeaders) makeResizableGrid(table); closeMenu();}, 10);
        
    }
    export function addColAfter(index) {
        report.addColumnRight(data, index);
        data = Object.assign({}, data);
        selectedRow=-1;selectedCol=-1;
        setTimeout(()=> {if(showHeaders) makeResizableGrid(table); closeMenu();}, 10);
    }
    export function addRow(cell, bias) {
        let m=cell.match(/(bodycell|rowheader)-(\d+)/);
        const rowIndex=parseInt(m[2]);
        report.addRow(data,rowIndex,bias);
        data = Object.assign({}, data);
        selectedRow=-1;selectedCol=-1;
        setTimeout(()=> {closeMenu();}, 10);
    }
    export function removeRow(cell) {
        let m=cell.match(/(bodycell|rowheader)-(\d+)/);
        const rowIndex=parseInt(m[2]);
        report.removeRow(data,rowIndex);
        data = Object.assign({}, data);
        selectedRow=-1;selectedCol=-1;
        setTimeout(()=> {closeMenu();}, 10);
    }
    export function removeCol(index) {
        report.removeColumn(data, index);
        data = Object.assign({}, data);
        selectedRow=-1;selectedCol=-1;
        setTimeout(()=> {if(showHeaders) makeResizableGrid(table); closeMenu();}, 10);
    }
    
    function mergeCells(row, colIndexs){
        selectedCol=colIndexs[0];
        setTimeout(()=> {closeMenu();}, 10);
    }

    function oncellover(e, rowIndex, colIndex){
        let id=`menu-${rowIndex}_${colIndex}`;
        const menu = e.target.querySelector(`#${id}`);
        if(menu) menu.classList.remove("hidden");
        e.stopPropagation();
    }
    function oncellleave(e, rowIndex, colIndex){
        let id=`menu-${rowIndex}_${colIndex}`;
        const menu = e.target.querySelector(`#${id}`);
        if(menu) menu.classList.add("hidden");
        e.stopPropagation();
    }

    let showContextMenu=false, showRightClickMenu=false;
    let contextpos = { x: 0, y: 0 };
    let columnIndex = -1;
    let cellId = null;

    async function onContextClick(e, rowIndex, colIndex) {
      e.stopPropagation(); 
      const rect = e.target.getBoundingClientRect();
      let top = e.target.closest("td").offsetTop-5;
      let left = e.target.closest("td").offsetLeft+e.target.closest("td").offsetWidth-150;
      if (showRightClickMenu) {
        showRightClickMenu = false;
      }
      if (showContextMenu) {
        showContextMenu = false;
        columnIndex = -1;
        cellId = null;
        await new Promise(res => setTimeout(res, 100));
      }
      
      contextpos = { x: e.clientX-rect.x+left, y: e.clientY-rect.y+top };
      showContextMenu = true;
      columnIndex=colIndex;
      mfsearch="";
      cellId=`bodycell-${rowIndex}-${colIndex}`;
    }
    async function onRightClick(e, rowIndex, colIndex) {
        e.stopPropagation(); 
        const rect = e.target.getBoundingClientRect();
        let top = e.target.closest(colIndex==-1?"th":"td").offsetTop;
        let left = e.target.closest(colIndex==-1?"th":"td").offsetLeft;
        if (showContextMenu) {
        showContextMenu = false;
        }
        if (showRightClickMenu) {
            showRightClickMenu = false;
            columnIndex = -1;
            cellId = null;
            await new Promise(res => setTimeout(res, 100));
        }
        
        contextpos = { x: e.clientX-rect.x+left, y: e.clientY-rect.y+top };
        showRightClickMenu = true;
        columnIndex=colIndex;
        if(colIndex==-1){
            cellId=`rowheader-${rowIndex}`;
        }
        else {
            cellId=`bodycell-${rowIndex}-${colIndex}`;
            const textbox=report.getCellTextbox(data, rowIndex,colIndex);
            selectedCellContentType=0;
            if(textbox){
                const c=report.getTextBoxContentType(textbox);
                if(c==="expr") selectedCellContentType=2;
                if(c==="text") selectedCellContentType=1;
            }
        }
    }

    function closeMenu() {
      showContextMenu = false;
      showRightClickMenu = false;
    }

    function makeResizableGrid(tbl) {
        let row = tbl.getElementsByTagName('tr')[0],
        cc = row ? row.children : undefined;
        if (!cc) return;
        
        tbl.style.overflow = 'hidden';
        
        let tblHeight = tbl.offsetHeight;
        for (var i=1;i<cc.length;i++){
            var div = createDiv(tblHeight);
            cc[i].appendChild(div);
            cc[i].style.position = 'relative';
            setListeners(div);
        }
    }

    function setListeners(div){
        let pageX,curCol,nxtCol,curColWidth,nxtColWidth;

        div.addEventListener('mousedown', function (e) {
            curCol = e.target.parentElement;
            nxtCol = curCol.nextElementSibling;
            pageX = e.pageX; 
            
            let padding = paddingDiff(curCol);
            
            curColWidth = curCol.offsetWidth - padding;
            if (nxtCol)
                nxtColWidth = nxtCol.offsetWidth - padding;
        });
        div.addEventListener('mouseover', function (e) {
            e.target.style.borderRight = '2px solid #0545ce';
        })

        div.addEventListener('mouseout', function (e) {
            e.target.style.borderRight = '';
        })

        document.addEventListener('mousemove', function (e) {
            if (curCol) {
                let diffX = e.pageX - pageX;
            
                if (nxtCol)
                    nxtCol.style.width = (nxtColWidth - (diffX))+'px';

                curCol.style.width = (curColWidth + diffX)+'px';
            }
        });

        document.addEventListener('mouseup', function (e) { 
            
            if(curCol || nxtCol){
                //adjust column widths
                let row = table.getElementsByTagName('tr')[0],
                cc = row ? row.children : undefined;
                if(cc){
                    for (let i=1, c=0;i<cc.length;i++,c++){
                        data.tablixBody.tablixColumns.tablixColumn[c].width=report.pxToCentimeter(cc[i].style.width)+"cm";
                    }
                    data=Object.assign({},data);
                }
            }
            curCol = undefined;
            nxtCol = undefined;
            pageX = undefined;
            nxtColWidth = undefined;
            curColWidth = undefined
        });
    }
    function createDiv(height){
        let div = document.createElement('div');
        div.style.top = 0;
        div.style.right = 0;
        div.style.width = '5px';
        div.style.position = 'absolute';
        div.style.cursor = 'col-resize';
        div.style.userSelect = 'none';
        div.style.height = height + 'px';
        div.style.zIndex = 888;
        return div;
    }
 
    function paddingDiff(col){
        if (getStyleVal(col,'box-sizing') == 'border-box'){
            return 0;
        }
        
        let padLeft = getStyleVal(col,'padding-left');
        let padRight = getStyleVal(col,'padding-right');
        return (parseInt(padLeft) + parseInt(padRight));

    }

    function getStyleVal(elm,css){
        return (window.getComputedStyle(elm, null).getPropertyValue(css))
    }
    function selectRow(e, index) {
        e.preventDefault();
        e.stopPropagation();
        selectedRow=index;
        selectedCols=null;
        selectedCol=-1;
        dispatch("selectrow", {row:selectedRow})
    }
    async function selectCell(e, rowIndex,colIndex) {
        e.stopPropagation();
        closeMenu();
        if(e.shiftKey && selectedRow>-1 && selectedCol>-1 && selectedRow===rowIndex && selectedCol!=colIndex){
            
            let from=colIndex, to=selectedCol;
            if(colIndex>selectedCol){
                from=selectedCol;
                to=colIndex;
            }
            // continuous selected cells
            selectedCols=[...Array(to-from+1).keys()].map((i,j)=> j+from);
        }
        else{
            selectedCols=null; 
        }
        if(!showHeaders) { 
            showHeaders=true; 
            makeHedersVisible(table);
            //return;
            await new Promise(res => setTimeout(res, 10));
        }
        
        selectedRow=rowIndex;
        if(selectedCols==null) {
            selectedCol=colIndex;
            dispatch("selectcell", {row:rowIndex,col: colIndex})
        } else{
            selectedCol=-1;
        }
    }
    function onCornerHeaderClick(e) {
        e.preventDefault();
        e.stopPropagation();
        closeMenu();
        showHeaders=false;
        selectedRow=-1;
        selectedCol=-1;
        dispatch("cornercellclick");
    }
    
    async function makeHedersVisible() {
        await new Promise(res => setTimeout(res, 100));
        if(table) makeResizableGrid(table);
    }

    function addField(id, field) {
        let m=id.match(/bodycell-(\d+)-(\d+)/);
        const rowIndex=parseInt(m[1]);
        const colIndex=parseInt(m[2]);
        report.addFieldToCell(data, rowIndex, colIndex, field);
        data = Object.assign({}, data);
        setTimeout(()=> closeMenu(), 10);
    }

    function addText(e, id) {
        e.preventDefault();
        e.stopPropagation();
        setTimeout(()=> closeMenu(), 10);
        let m=id.match(/bodycell-(\d+)-(\d+)/);
        const rowIndex=parseInt(m[1]);
        const colIndex=parseInt(m[2]);
        dispatch("addtext", {row:rowIndex,col: colIndex});
    }
    function addExpression(e, id) {
        e.preventDefault();
        e.stopPropagation();
        setTimeout(()=> closeMenu(), 10);
        let m=id.match(/bodycell-(\d+)-(\d+)/);
        const rowIndex=parseInt(m[1]);
        const colIndex=parseInt(m[2]);
        dispatch("addexpression", {row:rowIndex,col: colIndex});
    }
    function onPageClick(e){
        if (e.target === table || table.contains(e.target)) return;
        selectedCol=selectedRow=-1;
        showHeaders=false;
        selectedCellContentType=0;
    }

</script>