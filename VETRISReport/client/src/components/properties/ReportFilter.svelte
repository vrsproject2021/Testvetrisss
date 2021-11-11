<script>

    import { createEventDispatcher } from "svelte";
    import * as report from "../../model/report/report.js"; 
    import Dialog from "../../components/Dialog.svelte";
    export let reportData=null;
    export let table=null;

    const dispatch = createEventDispatcher();
    let fields = [];
    let beingAdded=false;
    let editDialog = false;
    let filterExpression, operator, value1, value2;

    const operators = [
        { name: "=", value:"Equal"},
        { name: "<>", value:"NotEqual"},
        { name: "<", value:"LessThan"},
        { name: "<=", value:"LessThanOrEqual"},
        { name: ">", value:"GreaterThan"},
        { name: ">=", value:"GreaterThanOrEqual"},
        { name: "Like", value:"Like"},
        { name: "Between", value:"Between"},
    ];

    const parameters=[{name:'', value:null},...report.getParameters(reportData).map(function(p){
                return { 
                    name: report.getExpressionContentDisplay(`=Parameters!${p.name}.Value`),  
                    value: `=Parameters!${p.name}.Value`, 
                    dataType: report.getFieldType(p.typeName,"d")  
                };
            })];
    $: hasDS = table && reportData && reportData.dataSets && reportData.dataSets.dataSet.length>0;


    $: {
        if(reportData.dataSets && reportData.dataSets.dataSet.length>0){
            fields = [{name:'', value:null},...reportData.dataSets.dataSet[0].fields.field.map(function(f){
                return { 
                    name: report.getExpressionContentDisplay(`=Fields!${f.dataField}.Value`),  
                    value: `=Fields!${f.dataField}.Value`, 
                    dataType: report.getFieldType(f.typeName,"d")  
                };
            })];
        }
        
    }

    function initAddFilter(e){
        filterExpression=null;
        operator='Equal';
        value1=null;
        value2=null;
        editDialog=true;
    }

    function deleteFilter(index){
        report.removeFilter(table, index);
        dispatch("filterchange", {tableName: table.name});
    }

    function addFilter(e) {
        let values=[];
        if(operator==="Between") values=[value1, value2];
        else values=[value1];
        debugger;
        report.addFilter(table, {
            filterExpression: filterExpression,
            operator: operator,
            filterValues: {
                filterValue:[...values]
            }
        });
        beingAdded=false;
        editDialog=false;
        dispatch("filterchange", {tableName: table.name});
    }
</script>

<div class="bg-white font-medium p-1 px-2 border-b shadow-sm flex flex-row">
    <span class="flex-auto">Filters</span>
    {#if hasDS}
    <button class="bg-blue-500 text-white p-1.5 m-1 rounded-sm focus:outline-none w-10" on:click|preventDefault={(e)=> initAddFilter(e) }>Add</button>
    {/if}
</div>

{#if hasDS}
<div class="flex flex-col m-2">
    <div class="flex flex-row items-center justify-between space-x-1 border-b p-2 font-semibold text-gray-400">
        <div class="flex flex-1 hover:text-gray-900">Conditions</div>
        <div class="flex w-16 hover:text-gray-900"></div>
    </div>
    <div class="overflow-y-auto border-b border-l border-r text-xs" style="max-height:80%;"> 
        {#each report.getFilters(table) as f,index}
        <div class="flex flex-row items-center justify-between space-x-1 p-1 text-gray-900 hover:bg-gray-200">
            <div class="flex-auto p-1">
                {report.getExpressionContentDisplay(f.filterExpression)} 
                {(operators.find(i=>i.value===f.operator)??{name:""}).name}
                {f.operator=='Between'?'(':''}
                {#each f.filterValues.filterValue as v, idx}
                    <span>{report.getExpressionContentDisplay(v)}</span>
                    {#if (idx<f.filterValues.filterValue.length-1) }
                    <span>, </span>
                    {/if}
                {/each}
                {f.operator=='Between'?')':''}
            </div>
            <div class="w-6 text-gray-700" on:click={(e)=>deleteFilter(index)}>
                <svg xmlns="http://www.w3.org/2000/svg" class="ml-1 h-5 w-5 text-red-600" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                  </svg>
            </div>
        </div>
        {/each}
    </div>
</div>

<Dialog bind:visible={editDialog} title="{beingAdded?"Add":"Edit"} Filter" w="1/3">
    <div class="px-2 py-2 pb-4 max-w-md">
        <label for="expression"  class="block font-semibold">Expression</label>
        <select bind:value={filterExpression} id="expression" name="expression" class="
            border w-full h-12 px-3 py-3 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm">
            {#each fields as opt}
                <option value={opt.value}>
                    {opt.name}
                </option>
            {/each}
        </select> 
    </div>

    <div class="px-2 py-2 pb-4 max-w-md">
        <label for="operator"  class="block font-semibold">Operator</label>
        <select bind:value={operator} id="operator" name="operator" class="
            border w-full h-12 px-3 py-3 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm">
            {#each operators as opt}
                <option value={opt.value}>
                    {opt.name}
                </option>
            {/each}
        </select> 
    </div>
    
    <div class="flex flex-row space-x-2 justify-center">
        <div class="flex-1 px-2 py-2 pb-4">
            <label for="value1"  class="block font-semibold">{operator=='Between'?'From ':''}Value</label>
            <select bind:value={value1} id="value1" name="value1" class="
                border w-full h-12 px-3 py-3 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm">
                {#each parameters as opt}
                    <option value={opt.value}>
                        {opt.name}
                    </option>
                {/each}
            </select> 
        </div>
        <div class="flex-1 px-2 py-2 pb-4 {operator=='Between'?'':'hidden'}">
            <label for="value2"  class="block font-semibold">To Value</label>
            <select bind:value={value2} id="value2" name="value2" class="
                border w-full h-12 px-3 py-3 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm">
                {#each parameters as opt}
                    <option value={opt.value}>
                        {opt.name}
                    </option>
                {/each}
            </select> 
        </div>
    </div>

    <div class="flex flex-row items-center justify-end mb-2">
      <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e)=>addFilter(e)}>OK</button>
      <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e) => {editDialog=false; beingAdded=false;}}>Cancel</button>
    </div>
  </Dialog>
  {/if}