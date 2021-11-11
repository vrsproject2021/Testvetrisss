<script>

    import { createEventDispatcher } from "svelte";
    import * as report from "../../model/report/report.js"; 
    import Dialog from "../../components/Dialog.svelte";
    import Swal from 'sweetalert2';

    export let reportData=null;

    const dispatch = createEventDispatcher();

    

    let search='';
    let beingAdded=false;
    let editDialog = false;
    let name, prompt, dataType="Integer", nullable=false;
    let dataTypes = [{
        name:"Text", value:"String"
    },{
        name:"Date/Time", value:"DateTime"
    },{
        name:"Boolean", value:"Boolean"
    },{
        name:"Float", value:"Float"
    },{
        name:"Integer", value:"Integer"
    }];

    function addParam(e){
        name='';
        dataType='String';
        prompt='';
        nullable=false;
        beingAdded=true;
        editDialog=true;
        
    }

    function deleteParam(pname){
        if(report.isParameterUsed(reportData, pname)){
            Swal.fire({
                title: "Warning",
                html: "Parameter "+`<b>${pname}</b>`+ " is already in use!",
                buttonsStyling: false,
                confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                icon: "warning"
                });
            return;
        }
        report.removeParameter(reportData, pname);
        dispatch("parameterchange");
    }

    function addParameter(e) {
        report.addParameter(reportData, {
            name: name,
            dataType: dataType,
            prompt: prompt,
            nullable: nullable
        });
        dispatch("parameterchange");
        beingAdded=false;
        editDialog=false;
    }
</script>

<div class="bg-white font-medium p-1 px-2 border-b shadow-sm flex flex-row">
    <span class="flex-auto">Report Parameters</span>
    <button class="bg-blue-500 text-white p-1.5 m-1 rounded-sm focus:outline-none w-10" on:click|preventDefault={(e)=> addParam(e) }>Add</button>
</div>
<div class="flex flex-col m-2">

    <div class="pr-2 flex flex-row border">
        <div class="flex-1 text-sm bg-white p-1">
            <input type="text" class="w-full hover:outline-none focus:outline-none bg-transparent" 
                placeholder="Search" bind:value={search}>
        </div>
        <svg class="w-4 h-4 m-1.5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
        </svg>
    </div>
    <div class="flex flex-row items-center justify-between space-x-1 border-b p-2 font-semibold text-gray-400">
        <div class="flex flex-1 hover:text-gray-900">Name</div>
        <div class="flex w-16 hover:text-gray-900">Type</div>
        <div class="flex w-16 hover:text-gray-900"></div>
    </div>
    <div class="overflow-y-auto border-b border-l border-r" style="max-height:80%;"> 
        {#each report.getParameters(reportData).filter(i=>search?i.name.toLowerCase().indexOf(search.toLowerCase()) >= 0 :true) as p}
        <div class="flex flex-row items-center justify-between space-x-1 p-2 text-gray-900 hover:bg-gray-200">
            <div class="flex flex-1">{p.name}</div>
            <div class="flex w-16 text-white text-xs"><span class="bg-green-800 rounded-sm">{p.dataType}</span></div>
            <div class="flex w-16 text-gray-700" on:click={(e)=>deleteParam(p.name)}>
                <svg xmlns="http://www.w3.org/2000/svg" class="ml-10 h-5 w-5 text-red-600" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                  </svg>
            </div>
        </div>
        {/each}
    </div>
</div>

<Dialog bind:visible={editDialog} title="{beingAdded?"Add":"Edit"} Parameter" w="1/3">
    <div class="px-2 py-2 pb-4 max-w-md">
        <label for="prompt"  class="block font-semibold">Prompt</label>
        <input type="text" bind:value={prompt} id="prompt" placeholder="Prompt" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
    </div>
    <div class="px-2 py-2 pb-4 max-w-md">
        <label for="name"  class="block font-semibold">Name</label>
        <input type="text" bind:value={name} id="name" placeholder="Name" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
    </div>
    <div class="px-2 py-2 pb-4 max-w-md">
        <label for="datatype"  class="block font-semibold">Data Type</label>
        <select bind:value={dataType} id="datatype" name="datatype" class="
            border w-full h-12 px-3 py-3 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm">
            {#each dataTypes as opt}
                <option value={opt.value}>
                    {opt.name}
                </option>
            {/each}
        </select> 
    </div>
    <div class="px-2 py-2 pb-4 max-w-md">
        <input type="checkbox" name="nullable" id="nullable" bind:checked={nullable} class="inline-block align-middle" />
        <label class="inline-block align-middle" for="nullable">Nullable</label>
    </div>
    <div class="flex flex-row items-center justify-end mb-2">
      <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e)=>addParameter(e)}>OK</button>
      <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e) => {editDialog=false; beingAdded=false;}}>Cancel</button>
    </div>
  </Dialog>