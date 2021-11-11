<script>
    
    import {apiendpoint} from "../../store.js";  
    import { goto } from '@roxi/routify/runtime/helpers';
    import {user} from "../../model/user";
    import { onMount } from 'svelte';
    import Dialog from "../../components/Dialog.svelte";
    import Menu from '../../components/menu/Menu.svelte';
    import MenuOption from '../../components/menu/MenuOption.svelte';
    import Editor from "../../components/properties/Editor.svelte";
  
    
	if(!user.isAuthenticated())
		$goto('/login');
	
    /* menu */
    let contextpos = { x: 0, y: 0 };
	let showContextMenu = false;

    let dialog, loadDone=false;
    let editor;
     let filter=0;
    let sorting=0;
    let waiting=false;
    let sortOptions = [
        {id:0, text:'Recently updated'},
        {id:1, text:'Report name (A-Z)'},
        {id:2, text:'Report name (Z-A)'}
    ];

    let datasetId, name, objectname, bodytext, isNew=false, actionbuttons="", qchanged=false, metadata=[] ;
    let el;
    let maxHeight = '100%';
    $: slimscrollOptions={};
    let datasets=[];
    async function getDatasets () {
        waiting=true;
		const res = await fetch(`${$apiendpoint}/api/metadata/datasets`, {
			method: 'GET',
           headers: {
               'Content-Type': 'application/json',
               'Authorization': user.token()
           }
		});
		
		const json = await res.json();
		datasets = json.result||[];
        
        waiting=false;
        
	}
    async function getSingle (id, callback) {
        waiting=true;
		const res = await fetch(`${$apiendpoint}/api/metadata/dataset?id=${id}`, {
			method: 'GET',
           headers: {
               'Content-Type': 'application/json',
               'Authorization': user.token()
           }
		});
		
		const json = await res.json();
        if(callback){
            callback(json.result)
        }
        waiting=false;
        
	}
    getDatasets();
    onMount(()=>{
        if(window.innerHeight-el.offsetTop-41>=0){
            maxHeight=(window.innerHeight-el.offsetTop-41)+"px";
            slimscrollOptions.height = (window.innerHeight-el.offsetTop-41)+"px";
        } 
        else {
            maxHeight="100%";
            slimscrollOptions.height = "100px";
        }
        loadDone=true;
    });
    function handleResize(e) {
        if(e.target.innerHeight-el.offsetTop-41>=0) {
            maxHeight=(e.target.innerHeight-el.offsetTop-41)+"px";
            slimscrollOptions.height = (e.target.innerHeight-el.offsetTop-41)+"px";
        }
        else {
            maxHeight="100%";
            slimscrollOptions.height = "100px";
        } 
    }
    function closeMenu() {
      showContextMenu = false;
    }

    function onAdd(){
        isNew=true;
        datasetId=undefined;
        selectedTab=0;
        name='Untitled'; objectname='untitled_view'; bodytext='';actionbuttons="",metadata=[];
        dialog=true;

    }
    function onEdit(){
        getSingle(datasetId, (ds)=>{
            isNew=false;
            selectedTab=0;
            name=ds.name; objectname=ds.objectName; bodytext=ds.bodyText;
            actionbuttons=ds.actions;
            if(ds.metadata!==null)
                metadata = [...ds.metadata];
            else 
                metadata=[];
            dialog=true;
        });
    }
    
    async function onContextMenu(e) {
        e.preventDefault();
        e.stopPropagation();
        if (showContextMenu) {
        showContextMenu = false;
        await new Promise(res => setTimeout(res, 100));
      }
      
      contextpos = { x: e.clientX-10, y: e.clientY };
      
      const id=e.target.closest('div').id;
      if(id){
          datasetId=id;
          showContextMenu = true;
      }
      else {
          datasetId=undefined;
      }
    }

    async function save () {
      
       waiting=true;
       let postData = { name: name, objectName: objectname, bodyText: bodytext, actions: actionbuttons||null, metadata:[...metadata] };
       (!isNew)
          postData = { id:datasetId, name: name, objectName: objectname, bodyText: bodytext, actions: actionbuttons||null, metadata:[...metadata] };
       const res = await fetch(`${$apiendpoint}/api/metadata/createorupdateview`, {
           method: 'POST',
           headers: {
               'Content-Type': 'application/json',
               'Authorization': user.token()
           },
           body: JSON.stringify(postData)
       });
       
       const json = await res.json();
       if(!json.isError && json.result===true){
            dialog=false;
            getDatasets().then(()=>waiting=false);
       }
       else 
         waiting=false;
   }

   let selectedTab=0;
   let editorConfig = {
    language: 'sql',
    lineNumbers: true,
    lineWrapping: false,
    lineHighlight: true,
    viewportMargin: 0,
    scrollbarStyle:'simple'
  };
  let buttonOptions = [{
      id:"",
      text: "None"
  },{
      id:"VIEW_IMAGE",
      text: "View Image"
  },{
      id:"VIEW_REPORT",
      text: "View Report"
  },{
      id:"VIEW_IMAGE,VIEW_REPORT",
      text: "View Image, View Report"
  },{
      id:"VIEW_INVOICE",
      text: "Download Invoice"
  }];

  async function validate_query(e) {
        waiting=true;
       bodytext=editor.getValue();
       let postData = { query: bodytext };
       
       const res = await fetch(`${$apiendpoint}/api/metadata/validate?t=${+(new Date())}`, {
           method: 'POST',
           headers: {
               'Content-Type': 'application/json',
               'Authorization': user.token()
           },
           body: JSON.stringify(postData)
       });
       
       const json = await res.json();
       waiting=false;
       if(!json.isError){
          if(json.result.success){
              qchanged=false;
            //   if(metadata.length>0){
            //     let mdata=[...json.result.metaData];
            //     metadata.filter(i=>i.default===true).forEach(i=>{
            //         let m=mdata.find(j=>j.name.toLocaleLowerCase()==i.name.toLocaleLowerCase());
            //         m.default=true;
            //     });
            //     metadata=[...mdata];
            //   }
            //   else{
            //     metadata=[...json.result.metaData];
            //   }
            metadata=[...json.result.metaData];
          }
          else{
              alert(`Error:${json.result.error_message}`);
          }  
       }
       
       
  }
</script>
<svelte:window on:resize={handleResize}></svelte:window>
<div class="flex flex-col w-full" on:resize={handleResize}>
    
    
        <div class="flex flex-col">
            <div class="flex flex-row items-center justify-between p-1 space-x-2 border-b shadow-sm" >
                <div>
                    <div class="font-semibold">Datasets ({datasets.length})</div>
                </div>
                <div>
                    <button 
                        class="mt-1 bg-gray-200 text-gray-500 py-2 px-6 rounded-sm hover:bg-indigo-600 hover:text-white"
                        on:click={onAdd}>
                        Add new
                    </button>
                </div>
                <div class="flex flex-1 text-sm bg-white p-1">
                    <select bind:value={sorting} style="width: 100%;" class="hover:outline-none focus:outline-none">
                        {#each sortOptions as opt}
                            <option value={opt.id}>
                                {opt.text}
                            </option>
                        {/each}
                    </select>
                </div>
                <div class="flex flex-1 text-sm bg-white p-1">
                    <input type="text" style="width: 100%;" class="hover:outline-none focus:outline-none bg-transparent" placeholder="Search">
                    <svg class="w-5 h-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
                    </svg>
                </div>
            </div>
            <div class="flex flex-row items-center justify-between space-x-1 border-b p-2 font-semibold text-gray-400">
                <div class="flex w-16 "></div>
                <div class="flex flex-1 ">Name</div>
                <div class="flex flex-1 ">Object Name</div>
                <div class="flex flex-1 ">Last Modified</div>
                <div class="w-10"></div>
                <div class="w-10 "></div>
            </div>
        </div>
        <div bind:this={el} class="p-1 overflow-y-auto" style="max-height:{maxHeight};"> 
                {#each datasets as r}
                <div class="flex flex-row items-center justify-between space-x-1">
                    <div class="flex flex-col w-16 items-center text-gray-300 hover:text-yellow-600">
                        <svg class="p-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z" />
                        </svg>
                    </div>
                    <div class="flex flex-1 flex-col">
                        <div class="text-sm font-semibold">{r.name}</div>
                        <div class="text-xs">Created by: {r.createdBy}</div>
                    </div>
                    <div class="flex flex-1 flex-row ">
                        <svg class="h-5 w-5 mr-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                        </svg>
                        
                        <div class="text-sm">{r.objectName}</div>
                        
                        <div class="text-xs mx-2 p-0.5 leading-5 font-semibold borlder rounded-full border-yellow-500 bg-yellow-300">{r.objectType}</div>
                    </div>
                    <div class="flex flex-1 flex-col">
                        <div class="text-sm font-semibold">{r.lastModifiedOn}</div>
                        <div class="text-xs">Created by: {r.lastModifedBy}</div>
                    </div>
                    <div class="w-10">
                        <svg class="w-6 h-6 p-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.684 13.342C8.886 12.938 9 12.482 9 12c0-.482-.114-.938-.316-1.342m0 2.684a3 3 0 110-2.684m0 2.684l6.632 3.316m-6.632-6l6.632-3.316m0 0a3 3 0 105.367-2.684 3 3 0 00-5.367 2.684zm0 9.316a3 3 0 105.368 2.684 3 3 0 00-5.368-2.684z" />
                        </svg>
                    </div>
                    <div class="w-10" on:click|preventDefault={onContextMenu} id="{r.id}">
                        <svg class="w-6 h-6 p-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 12h.01M12 12h.01M19 12h.01M6 12a1 1 0 11-2 0 1 1 0 012 0zm7 0a1 1 0 11-2 0 1 1 0 012 0zm7 0a1 1 0 11-2 0 1 1 0 012 0z" />
                        </svg>
                    </div>
                </div>
                {/each}
        </div>
</div>
{#if showContextMenu}
    
    <Menu {...contextpos} on:click={closeMenu} on:clickoutside={closeMenu}>
    <MenuOption 
        on:click={onEdit} 
            text="Edit" />
    
    </Menu>
{/if}
{#if dialog}
<Dialog bind:visible={dialog} w={'1/2'} title="{isNew?'Create new dataset':'Edit dataset'}">
    <div class="px-2 py-2 pb-4 grid grid-cols-4 gap-3 min-w-max" >
        <div class="col-span-2">
            <label for="name"  class="block font-semibold">Name</label>
            <input type="text" maxlength="30" bind:value={name} id="name" placeholder="Name" class="border w-full h-8 px-2 py-2 mt-1 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
        </div>
        <div class="col-span-2">
            <label for="objectname" maxlength="100" class="block font-semibold">Database Object name</label>
            <input type="text" bind:value={objectname} id="objectname" placeholder="Database Object name" class="border w-full h-8 px-2 py-2 mt-1 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
        </div>
        <ul class="list-reset flex border-b col-span-4 ">
            <li class="mr-1 {selectedTab==0?'-mb-px':''}" on:click={(e)=> selectedTab=0}>
              <a class="bg-white inline-block {selectedTab==0?'border-l border-t border-r rounded-t':''} py-2 px-4 text-blue-dark font-semibold" >Query</a>
            </li>
            <li class="mr-1 {selectedTab==1?'-mb-px':''}" on:click={(e)=> selectedTab=1}>
              <a class="bg-white inline-block {selectedTab==1?'border-l border-t border-r rounded-t':''} py-2 px-4 text-blue hover:text-blue-darker font-semibold">Columns</a>
            </li>
        </ul>
        <div class="col-span-4 {selectedTab==0?'':'hidden'}">
            
            <Editor 
                bind:this={editor} 
                bind:value={bodytext} 
                height={'300px'} 
                theme={'vs'}
                language={'sql'} 
                lineNumbers={'on'}
                eof={false}
                on:onChange={(e)=> {qchanged=true;} }
                />
        </div>
        <div class="col-span-4 {selectedTab==1?'':'hidden'}">
            <div class="font-semibold border-b w-full">Select Default columns</div>
            <div class="w-full flex flex-col border overflow-y-auto" style="height:200px; max-height:300px;">
                {#each metadata as col}
                    <div class="flex flex-row items-center" >
                        <input type="checkbox" bind:checked={col.default} class="mx-1 p-1">
                        <div class="mx-1 p-1 flex-auto" >{col.description}</div>
                        <div class="mx-1 p-1 {col.type!=='number'?'hidden':''}"  >Calculate Total</div>
                        <input type="checkbox" bind:checked={col.calculateTotal} class="mx-1 p-1 {col.type!=='number'?'hidden':''}">
                    </div>
                {/each}
            </div>
            <div class="p-1">
                <label for="actionbuttons" maxlength="100" class="block font-semibold">Tabular Actions</label>
                <select bind:value={actionbuttons} style="width: 100%;" class="hover:outline-none focus:outline-none">
                    {#each buttonOptions as opt}
                        <option value={opt.id}>
                            {opt.text}
                        </option>
                    {/each}
                </select>
            </div>
        </div>
        
        <div class="flex flex-row items-center justify-end mb-2 col-span-4">
            <button class="p-2 mx-2 bg-green-900 text-white rounded-sm w-16 hover:outline-none focus:outline-none {!qchanged?'hidden':''}" on:click|preventDefault={validate_query}>Validate</button>
            <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none {qchanged?'hidden':''}" on:click|preventDefault={save}>OK</button>
            <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e) => dialog=false}>Cancel</button>
        </div>
        <!-- <textarea placeholder="Query" bind:value={bodytext} autocorrect="false" class="col-span-2 text-grey-darkest p-2 m-1 bg-transparent" id="bodytext" rows="15" cols="80"></textarea> -->
    </div>
    
    
</Dialog>
{/if}  
