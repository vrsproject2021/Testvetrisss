<script>
    import Sort from "./icons/Sort.svelte";
    import SortAscending from "./icons/SortAscending.svelte";
    import SortDescending from "./icons/SortDescending.svelte";
    import { createEventDispatcher } from 'svelte';
    import { ResizableColumns } from 'svelte-resizable-columns';
    import Filter from "./icons/Filter.svelte";
import { basepath } from "../../store";

	  const dispatch = createEventDispatcher();

  
    export let stripped = false;
    export let hover = false;
  
    export let head = {};
  
    export let rows = [];
  
    // get string or object value
    function val(attr) {
      if (attr===undefined||attr===null) return "";
      if (typeof attr === "object") return attr.value || "";
      return attr;
    }
    function format(headfn, attr) {
      if (typeof headfn === "string") return attr;
      if (headfn.format!==undefined && typeof headfn.format === "function") return  headfn.format(attr);
      return attr;
    }
    function isCmp(attr) {
      return (
        (!(attr===null||attr===undefined) && typeof attr === "object" && typeof attr.value === "function") ||
        typeof attr === "function"
      );
    }
    function hasComponents(headfn) {
      if (typeof headfn === "string") return false;
      if (headfn.component!==undefined && typeof headfn.component === "object" && headfn.component.buttons!==undefined && Array.isArray(headfn.component.buttons) && headfn.component.buttons.length>0){
          return true;
      }
      return false;
    }
    function props(attr) {
      if (!(attr===null||attr===undefined) && typeof attr === "object") return attr.props || {};
      return {};
    }
  
    function classes(attr) {
      if (!(attr===null||attr===undefined) && typeof attr === "object") return attr._class || "";
      return "";
    }
  
    // exclude _class
    function keys(obj) {
      return Object.keys(obj).filter(k => k != "_class");
    }
  
    function mergeClass(c1, c2) {
      return c1
        .split(" ")
        .filter(c => c.split("-")[0] && ~c.split("-")[0].indexOf(c2))
        .join(" ");
    }
  
    // override with head and default classes class
    function rowClasses(row) {
      const default_class = "bg-white border-b border-gray-100";
      const row_class = classes(row);
      const head_class = classes(head);
      let res_class = default_class;
  
      if (row_class !== "") {
        res_class = mergeClass(default_class, row_class);
      }
      res_class += " " + row_class;
      // TODO Need to test it !!
      if (head_class !== "") {
        res_class += " " + mergeClass(head_class, res_class);
      }
  
      return res_class;
    }
  
    function colClasses(row, h) {
      const col_class = classes(row[h]);
      const head_class = classes(head[h]);
      let res_class = head_class;
      if (col_class != "") {
        res_class = mergeClass(head_class, col_class);
      }
      
      return `${res_class} ${col_class}`;
    }
  
    let sorted_by;
    let sorted_asc = {};
    function sort(h) {
      if (sorted_asc[h]) {
        head[h].sort.desc();
        sorted_asc[h] = false;
      } else {
        head[h].sort.asc();
        sorted_asc[h] = true;
      }
      sorted_by = h;
    }

    function colDragEnter(e, h, index) {
        if(!e.dataTransfer.items[0].type=="column") return;
        const el=drop_zone.querySelector(`#${h}${index}`);

        if(el){
            el.classList.add("bg-green-200");
        }
        
        dispatch('dragenter', {event: e, key:h, index:index, tr:drop_zone});
    }
    function colDragLeave(e, h, index) {
        const el=drop_zone.querySelector(`#${h}${index}`);

        if(el){
            el.classList.remove("bg-green-200");
        }
        if(!e.dataTransfer.items[0].type=="column") return;
        dispatch('dragleave', {event: e, key:h, index:index, tr:drop_zone });
    }
    function colDragDrop(e, h, index) {
        const el=drop_zone.querySelector(`#${h}${index}`);
        if(el){
            el.classList.remove("bg-green-200");
        }
        //if(!e.dataTransfer.getData("column")) return;
        dispatch('drop', {event: e, key:h, index:index, tr:drop_zone});
    }
    let drop_zone;
    function handleDragStart(e, colname, index){
        e.dataTransfer.dropEffect = "move";
        e.dataTransfer.setData("tablecolumn", colname);
    }
    function colClick(e, colname, index){
        e.stopPropagation();
        dispatch('sort', colname);
    }
    function colDoubleClick(e, colname, index){
        e.stopPropagation();
        dispatch('dblclick', colname);
    }
  </script>
  {#if head}
  <table class="border border-gray-100 rounded table-auto w-max min-w-full">
    <thead
      class="border-b border-gray-200 {!stripped ? 'bg-gray-100' : ''}
      text-center font-medium text-xs text-gray-600">
      <tr class={classes(head)} 
        bind:this={drop_zone}
        id="drop_zone">
        {#each keys(head) as h,index}
          <td class="p-2 border-l border-gray-300 " 
            draggable="true"
            id={h+index}
            on:dblclick={(e)=>colDoubleClick(e, h, index)}
            on:dragenter={(e)=>colDragEnter(e, h, index)}
            on:dragleave={(e)=>colDragLeave(e, h, index)}
            on:drop={(e)=>colDragDrop(e, h, index)}
            on:dragstart={(e)=> handleDragStart(e, h, index)}
            ondragover="return false">
            {val(head[h])}
            {#if !head[h].action}
              {#if head[h].filter}
                  <Filter class="inline-block float-right cursor-pointer w-4 hover:text-blue-400" />
              {/if}
              <div class="inline-block float-right cursor-pointer w-4 hover:text-blue-400" on:click|preventDefault={(e)=>colClick(e, h, index)}>
                {#if head[h].sort==='asc'}
                  <SortAscending  />
                {:else}
                  {#if head[h].sort==='desc'}
                      <SortDescending />
                  {:else}
                      <Sort  />
                  {/if}
                {/if}
              </div>
            {/if} 
          </td>
        {/each}
      </tr>
    </thead>
  
    <tbody>
      {#each rows as row, i}
        <tr
          class="{hover ? 'hover:bg-teal-100' : ''}
          {stripped && i % 2 == 0 ? 'bg-gray-100' : ''}
          {rowClasses(row)}">
          {#each keys(head) as h}
            <td class="{hasComponents(head[h])?'p-1':'p-2'} border-l border-r border-gray-200 text-xs {colClasses(row, h)}">
              {#if isCmp(row[h])}
                <svelte:component this={val(row[h])} {...props(row[h])} />
              {:else}
                {#if hasComponents(head[h])}
                  <div class="flex flex-row items-center place-content-center" > 
                    {#each head[h].component.buttons as b}
                        <button class="{b._class} focus:outline-none  {b.tooltip?'has-tooltip':''}" on:click={(e)=>b.click(e,row,val(h))}>
                          {@html b.prompt} 
                          {#if b.tooltip}
                            <span class='tooltip rounded shadow-lg p-1 text-white bg-blue-800 -mt-10 -mr-16'>{b.tooltip}</span>
                          {/if}
                        </button>
                    {/each}
                  </div>
                {:else}
                  {format(head[h], val(row[h]))}
                {/if}
              {/if}
            </td>
          {/each}
        </tr>
        <!-- <tr class="border-b md:hidden">
          {#each keys(head) as h}
            <tr
              class="md:hidden p-2 {stripped && i % 2 == 0 ? 'bg-gray-100' : ''}
              {rowClasses(row)}">
              <th
                class="p-2 text-xs text-gray-600 bg-gray-100 border-l
                border-gray-300">
                {val(head[h])}
              </th>
              <td class="p-2 w-full border-l border-r border-gray-200 {colClasses(row, h)}">
                {#if isCmp(row[h])}
                  <svelte:component this={val(row[h])} {...props(row[h])} />
                {:else}{format(head[h], val(row[h]))}{/if}
              </td>
            </tr>
          {/each}
        </tr> -->
      {/each}
    </tbody>
  </table>
  {/if}

  