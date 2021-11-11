<script>
import { onMount } from "svelte";
import { ResizableColumns } from 'svelte-resizable-columns';
import Menu from "./menu/Menu.svelte";
import MenuDivider from "./menu/MenuDivider.svelte";
import MenuOption from "./menu/MenuOption.svelte";


    export let cols=3;
    export let showFooter=false;
    let showcolumnOptions=false;
    let table;
    let defs = [
        {type:'header', data:['Column1','Column2','Column3'], widths:[0,0,0]},
        {type:'body', data:['Text','Text','Text'] },
        {type:'footer', data:['','','']}
    ];
    function init() {
        if (defs.length==0) data=[...defaultdefs];
        if(defs[0].data.length!=cols){
            if(defs[0].data.length<cols){
                for(let c=defs[0].data.length; c<=cols; c++){
                    defs[0].data=[...defs[0].data, `Column${c+1}`]
                    defs[1].data=[...defs[1].data, '']
                    defs[2].data=[...defs[2].data, '']
                } 
            } else {
                    const len=defs[0].data.length-cols;
                    defs[0].data.splice(cols-1, len);
                    defs[1].data.splice(cols-1, len);
                    defs[2].data.splice(cols-1, len);
            }
            defs=[...defs];
        } 
    }
    
    onMount(()=>{
        if(table) makeResizableGrid(table);
    });
    
    export function addColBefore(index) {
        const len=defs[0].data.length;
        if(index>=0 && index<=len){
            defs[0].data.splice(index,0,`Column${len+1}`); 
            defs[1].data.splice(index,0,'Text');
            defs[2].data.splice(index,0,'Text');
            defs=[...defs];
            cols+=1;
            makeResizableGrid(table);
        }
    }
    export function addColAfter(index) {
        const len=defs[0].data.length;
        if(index>=0 && index<len-1){
            defs[0].data.splice(index+1,0,`Column${len+1}`); 
            defs[1].data.splice(index+1,0,'Text');
            defs[2].data.splice(index+1,0,'Text');
           
            defs=[...defs];
            cols+=1;
            makeResizableGrid(table);
        } else if(index==len-1) {
            defs[0].data.push(`Column${len+1}`); 
            defs[1].data.push('Text');
            defs[2].data.push('Text');
            defs=[...defs];
            cols+=1;
            makeResizableGrid(table);
        }
        
    }
    export function removeCol(index) {
        const len=defs[0].data.length;
        if(index>=0 && index<len){
            defs[0].data.splice(index,1); 
            defs[1].data.splice(index,1);
            defs[2].data.splice(index,1);
            defs=[...defs];
            cols-=1;
            makeResizableGrid(table);
        }
        if(cols==0){
            cols=1;
            init();
            makeResizableGrid(table);
        }
        
    }
    const update = (e) => {
        
        let event = e.type;
        let leftWidth = e.detail.leftWidth;
        let rightWidth = e.detail.rightWidth;
        let leftColumn = e.detail.leftColumn;
        let rightColumn = e.detail.rightColumn;
    };
    function oncellover(e, celltype, index){
        let id=`${celltype}menu-${index}`;
        const menu = e.target.querySelector(`#${id}`);
        if(menu) menu.classList.remove("hidden");
        e.stopPropagation();
    }
    function oncellleave(e, celltype, index){
        let id=`${celltype}menu-${index}`;
        const menu = e.target.querySelector(`#${id}`);
        if(menu) menu.classList.add("hidden");
        e.stopPropagation();
    }

    let showContextMenu=false;
    let contextpos = { x: 0, y: 0 };
    let columnIndex = -1;
    let cellId = null;

    function onContextClick(e, coltype, index) {
      showcolumnOptions=(coltype==="header");  
      e.stopPropagation(); 
      const rect = e.target.getBoundingClientRect();
      let top = e.target.parentNode.parentNode.parentNode.offsetTop;
      let left = e.target.parentNode.parentNode.parentNode.offsetLeft+e.target.parentNode.parentNode.parentNode.offsetWidth;
      if (showContextMenu) {
        showContextMenu = false;
        columnIndex = -1;
        cellId = null;
        //await new Promise(res => setTimeout(res, 100));
      }
      
      contextpos = { x: e.clientX-rect.x+left, y: e.clientY-rect.y+top };
      showContextMenu = true;
      columnIndex=index;
      cellId=`${coltype}cell-${index}`;
    }
    function closeMenu() {
      showContextMenu = false;
    }

    function makeResizableGrid(tbl) {
        let row = tbl.getElementsByTagName('tr')[0],
        cc = row ? row.children : undefined;
        if (!cc) return;
        
        tbl.style.overflow = 'hidden';
        
        let tblHeight = tbl.offsetHeight;
        for (var i=0;i<cc.length;i++){
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
            e.target.style.borderRight = '2px solid #0000ff';
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
</script>
{#if defs && defs.length>0}
<table class="blueTable" bind:this={table}>
	<thead>
		<tr>
			{#each defs[0].data as cell,index }
				<th id={'headercell-'+index} 
                    on:mouseenter|preventDefault={(e)=> oncellover(e,"headercell", index)}
                    on:mouseleave|preventDefault={(e)=> oncellleave(e,"headercell", index)}
                    > 
                    <div class="flex flex-row items-center" >
                        <div class="flex-auto">{cell}</div>
                        <div class="hidden text-blue-500" 
                             style="top:0px; right:0px; width:12px;" id={'headercellmenu-'+index}
                             on:click|preventDefault={(e)=>onContextClick(e,"header", index)}>
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                            </svg>
                        </div>
                    </div></th>
			{/each}
		<tr/>
	</thead>
	<tbody>
		
        <tr>
            {#each defs[1].data as cell, index}
                <td id={'bodycell-'+index}
                    on:mouseenter|preventDefault={(e)=> oncellover(e,"bodycell", index)}
                    on:mouseleave|preventDefault={(e)=> oncellleave(e,"bodycell", index)}> 
                    <div class="flex flex-row ">
                        <div class="flex-auto">{cell}</div>
                        <div class="hidden text-blue-500" 
                            style="top:0px; right:0px; width:12px;" id={'bodycellmenu-'+index}
                            on:click|preventDefault={(e)=>onContextClick(e,"body", index)}>
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                            </svg>
                        </div>
                    </div>
                </td>
            {/each}
        </tr>
		
	</tbody>
    {#if showFooter}
    <tfoot>
		<tr>
			{#each defs[2].data as columnHeading, index}
				<td id={'footercell-'+index}>{columnHeading}</td>
			{/each}
		<tr/>
    </tfoot>
    {/if}
</table>
    {#if showContextMenu}
        <Menu {...contextpos} on:click={closeMenu} on:clickoutside={closeMenu}>
            <MenuOption  text="Expression" />
            <MenuOption  text="Field" />
            {#if showcolumnOptions}
            <MenuDivider />    
            <MenuOption  text="Add Cloumn right" on:click={(e)=>addColAfter(columnIndex)} />
            <MenuOption  text="Add Column left" on:click={(e)=>addColBefore(columnIndex)}/>
            <MenuDivider/>  
            <MenuOption  text="Delete Column" on:click={(e)=>removeCol(columnIndex)}/>
            {/if}
        </Menu>
    {/if}
{/if}
<style>
	table, th, td {
		border: 1px solid;
		border-collapse: collapse;
		margin-bottom: 1px;
	}
   
	
	table {
		border: 1px solid #1C6EA4;
		width: 100%;
		text-align: left;
		border-collapse: collapse;
	}
	table td, table th {
		border: 1px solid #998888;
		padding: 2px 2px;
        min-width: 1%;
        min-height: 20px !important;
        overflow: hidden;
        text-overflow: ellipsis;
	}
	
	
	
</style>