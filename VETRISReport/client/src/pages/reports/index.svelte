<script>
    import { goto } from '@roxi/routify/runtime/helpers';
    import {user} from "../../model/user";
    import { onMount } from 'svelte';
    import {apiendpoint} from "../../store.js";  
    import moment from 'moment-timezone';
    import Flatpickr from 'svelte-flatpickr';
    import { Jumper } from 'svelte-loading-spinners'
    import 'flatpickr/dist/flatpickr.css';
    import 'flatpickr/dist/themes/light.css';
    import Dialog from "../../components/Dialog.svelte";
    
    //import {slimscroll} from "svelte-slimscroll";

    
	if(!user.isAuthenticated())
		$goto('/login');
	
    let waiting=false;
    let dialog;
    let filter=0;
    let sorting="";
    let category="";
    let filterOptions = [
        {id:0, text:'All reports'},
        {id:1, text:'Owned by me'},
        {id:2, text:'Shared with me'},
    ];
    let sortOptions = [
        {id:"", text:'Recently updated'},
        {id:"asc", text:'Report name (A-Z)'},
        {id:"desc", text:'Report name (Z-A)'}
    ];
    let el;
    let maxHeight = '100%';
    $: slimscrollOptions={};
    let search;
    let reportsdata=[];
    let categories=[];
    let reportParameters=[];
    let parameterDialog=false;
    let intent=null;

    loadData();
    onMount(()=>{
        if(window.innerHeight-el.offsetTop-41>=0){
            maxHeight=(window.innerHeight-el.offsetTop-41)+"px";
            slimscrollOptions.height = (window.innerHeight-el.offsetTop-41)+"px";
        } 
        else {
            maxHeight="100%";
            slimscrollOptions.height = "100px";
        }
        
    });

    async function loadData(){
        let res = null;
        try {
            waiting=true;
            res = await Promise.all([
                loadCategories(),
                loadReports()
            ]).then(r=> waiting=false).catch(e=> waiting=false);
        } catch(err){
            waiting=false;
        }
    }
    async function loadReports() {
        const postData ={
            category: category,
            search:search,
            sortDirection: sorting
        };
        const res = await fetch(`${$apiendpoint}/api/report/getall`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': user.token()
            },
            body: JSON.stringify(postData)
        });
       
        const json = await res.json();
        
        reportsdata=[...json.result];    
        
    }
    async function loadCategories() {
       
        const res = await fetch(`${$apiendpoint}/api/report/categories`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': user.token()
            }
        });
       
        const json = await res.json();
        
        categories=[...json.result];    
        
    }
    async function getCallStatement(id, renderType) {
       waiting=true;
       try {
            const res = await fetch(`${$apiendpoint}/api/report/callstatement/${id}?t=${+new Date()}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': user.token()
                }
            });
            
            const json = await res.json();
            debugger;
            waiting=false;
            if(!json.isError){
                const result=json.result; 
                if(result.parameters){
                    reportParameters=[...result.parameters.map(function(i){
                        return {...i, ...{value: null}};
                    })];
                    intent={
                        id: id,
                        name: result.reportName,
                        renderType: renderType
                    };
                    parameterDialog=true;
                }
                else{
                        intent={
                            id: id,
                            name: result.reportName,
                            renderType: renderType
                        };
                        await generateReport();
                }
            }
        }
       catch (err) {
           waiting=false
       }
   }
   async function generateReport() {
        
        if(reportParameters && reportParameters.length>0){
            let params=[];
            for(let i=0; i<reportParameters.length; i++){
                let p={...reportParameters[i]};
                if(p.dataType==="DateTime"){
                    p.value=moment(p.value).format("YYYY-MM-DD");
                }
                params=[...params,p];
            }
            reportParameters=[...params];
        }
        const postData={...intent, ...{parameters: [...reportParameters]}};
        reportParameters=[];
        parameterDialog=false;
        waiting=true;
        try {
            const res = await fetch(`${$apiendpoint}/api/report/generate?t=${+new Date()}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': user.token(),
                },
                body: JSON.stringify(postData)
            });
        
            const json = await res.json();
            waiting=false;
            if(!json.isError){
                debugger;
                location.href = `${$apiendpoint}/file/downloadtemporaryfile?contentType=${json.result.fileType}&id=${json.result.fileToken}&fileName=${json.result.fileName}`;
            }
        }
        catch (err){
            waiting=false;
        }
    }

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
    function formatDate(str){
        if(!str) return ''
        if(str.endsWith('Z'))
            return moment(str).format("LLL")
        return new moment(`${str}Z`).format("LLL")
    }
</script>
<svelte:window on:resize={handleResize}></svelte:window>
<div class="flex flex-row w-full" on:resize={handleResize}>
    <div class="w-1/5 border-r shadow-xl p-2 bg-gray-100">
        <ul class="list-none w-full">
            <li class="list-none flex flex-row items-center p-1  border-b cursor-pointer hover:bg-gray-200" on:click={(e)=>{category=""; loadData(); }}>
                <svg class="h-5 w-5 mr-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z" />
                  </svg>
                <span>All reports</span>
            </li>
            <!-- <li class="list-none flex flex-row items-center  p-1  border-b">
                <svg class="h-5 w-5 mr-1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                    <path d="M17.414 2.586a2 2 0 00-2.828 0L7 10.172V13h2.828l7.586-7.586a2 2 0 000-2.828z" />
                    <path fill-rule="evenodd" d="M2 6a2 2 0 012-2h4a1 1 0 010 2H4v10h10v-4a1 1 0 112 0v4a2 2 0 01-2 2H4a2 2 0 01-2-2V6z" clip-rule="evenodd" />
                  </svg>
                <span>Drafts</span>
            </li> -->
            <li class="list-none  p-1">
                <div class="flex flex-row items-center justify-between">
                    <svg class="h-5 w-5 mr-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                    </svg>
                    <span class="flex flex-auto">Category</span>
                    <svg class="h-5 w-5"xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 15l7-7 7 7" />
                      </svg>
                    <!-- <svg class="h-4 w-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                    </svg> -->
                </div>
                <div>
                    <div class="flex flex-1 text-sm items-center bg-white p-1 border rounded-sm mt-1">
                        <input type="text" class="w-full hover:outline-none focus:outline-none bg-transparent" placeholder="Search">
                        <svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                            <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
                        </svg>
                    </div>
                    <ul class="list-none w-full pl-2">
                        {#each categories as opt}
                        <li class="list-none flex flex-row items-center p-1 border-b cursor-pointer hover:bg-gray-200" on:click={(e)=>{category=opt.category; loadData();}}>
                            <span class="flex flex-auto text-sm">{opt.category}</span>
                            <span class="w-8 h-6 rounded-xl border-blue-900 text-center bg-blue-800 text-white text-xs pt-0.5" >{opt.count}</span>
                        </li>
                        {/each}
                    </ul>
                </div>
            </li>
        </ul>
    </div>
    <div class="w-4/5 text-sm flex-col" >
        <div class="flex flex-col">
            <div class="flex flex-row items-center justify-between p-1 space-x-2 border-b shadow-sm" >
                <div>
                    <div class="font-semibold">{category==""?'All Reports':category} ({reportsdata.length})</div>
                </div>
                <div class="flex flex-1 text-sm ml-6 bg-white p-1">
                    <select bind:value={filter} style="width: 100%;" class="hover:outline-none focus:outline-none">
                        {#each filterOptions as opt}
                            <option value={opt.id}>
                                {opt.text}
                            </option>
                        {/each}
                    </select>
                </div>
                <div class="flex flex-1 text-sm bg-white p-1">
                    <!-- svelte-ignore a11y-no-onchange -->
                    <select bind:value={sorting} on:change={loadData} style="width: 100%;" class="hover:outline-none focus:outline-none">
                        {#each sortOptions as opt}
                            <option value={opt.id}>
                                {opt.text}
                            </option>
                        {/each}
                    </select>
                </div>
                <div class="flex flex-1 text-sm bg-white p-1">
                    <input type="text" bind:value={search} on:input={loadData} style="width: 100%;" class="hover:outline-none focus:outline-none bg-transparent" placeholder="Search">
                    <svg class="w-5 h-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
                    </svg>
                </div>
            </div>
            <div class="flex flex-row items-center justify-between space-x-1 border-b p-2 font-semibold text-gray-900 bg-gray-100 shadow-lg">
                <div class="flex w-16 "></div>
                <div class="flex flex-1 ">Name</div>
                <div class="flex flex-1 ">Category</div>
                <div class="flex flex-1 ">Last Modified</div>
                <div class="w-20"></div>
            </div>
        </div>
        <div bind:this={el} class="p-1 overflow-y-auto" style="max-height:{maxHeight};"> 
                {#each reportsdata as r}
                <div class="flex flex-row items-center justify-between space-x-1 p-2 cursor-pointer hover:bg-blue-100 hover:border-1 hover:border-blue-700 hover:shadow-sm">
                    <div class="flex flex-col w-16 items-center text-gray-300 hover:text-yellow-600">
                        <svg class="p-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z" />
                        </svg>
                    </div>
                    <div class="flex flex-1 flex-col">
                        <div class="vfont-semibold hover:text-blue-700 hover:underline" on:click={(e)=>$goto(`/designer/${r.id}`)}>{r.name}</div>
                        <div class="text-xs text-gray-400">{r.createdByUser}, Created on: {formatDate(r.createdOn)}</div>
                    </div>
                    <div class="flex flex-1 flex-row ">
                        <svg class="h-5 w-5 mr-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                        </svg>
                        <div class="text-xs ">{r.category}</div>
                    </div>
                    <div class="flex flex-1 flex-col">
                        <div class="text-sm">{formatDate(r.lastModifiedOn)}</div>
                        <div class="text-xs text-gray-400">{r.lastModifiedByUser}</div>
                    </div>
                   
                    <div class="w-20 flex flex-row space-x-1 items-center hover:border-l hover:border-gray-500">
                        <!-- Pdf -->
                        <svg on:click|preventDefault={(e)=>getCallStatement(r.id,"pdf")} 
                            class="w-6 h-6 text-red-500 hover:bg-white hover:shadow-lg hover:border-2" viewBox="0 0 24 24">
                            <path fill="currentColor" d="M19 3H5C3.9 3 3 3.9 3 5V19C3 20.1 3.9 21 5 21H19C20.1 21 21 20.1 21 19V5C21 3.9 20.1 3 19 3M19 5V19H5V5H19M17.9 13.5C17.6 13 16.9 12.8 15.7 12.8C15.3 12.8 14.9 12.8 14.5 12.9C14.2 12.7 13.9 12.5 13.7 12.3C13.1 11.8 12.5 10.9 12.2 9.8V9.7C12.5 8.4 12.8 6.9 12.2 6.2C11.9 6.1 11.7 6 11.5 6H11.3C10.9 6 10.6 6.4 10.5 6.7C10.1 8 10.4 8.7 10.7 9.9C10.5 10.8 10.1 11.7 9.7 12.7C9.3 13.4 9 14 8.7 14.5C8.3 14.7 8 14.8 7.8 15C6.7 15.7 6.1 16.5 6 17V17.5L6.5 17.8C6.7 18 6.8 18 7 18C7.8 18 8.7 17.1 9.9 15H10C11 14.7 12.2 14.5 13.9 14.3C14.9 14.8 16.1 15 16.8 15C17.2 15 17.5 14.9 17.7 14.7C17.9 14.5 18 14.3 18 14.1C18 13.8 18 13.6 17.9 13.5M6.8 17.3C6.8 16.9 7.3 16.3 8 15.7C8.1 15.6 8.3 15.5 8.5 15.4C7.8 16.5 7.2 17.2 6.8 17.3M11.3 6.7C11.3 6.7 11.3 6.6 11.4 6.6H11.5C11.7 6.8 11.7 7.1 11.6 7.7V7.9C11.5 8.1 11.5 8.4 11.4 8.7C11.1 7.8 11.1 7.1 11.3 6.7M10.1 14.3H10C10.1 14 10.3 13.7 10.5 13.3C10.9 12.5 11.3 11.7 11.5 11C11.9 11.9 12.4 12.6 13 13.1C13.1 13.2 13.3 13.3 13.4 13.4C12.5 13.5 11.3 13.8 10.1 14.3M17.3 14.2H17.1C16.7 14.2 16 14 15.3 13.7C15.4 13.6 15.5 13.6 15.5 13.6C16.9 13.6 17.2 13.8 17.3 13.9L17.4 14C17.4 14.2 17.4 14.2 17.3 14.2Z" />
                        </svg>
                        <!-- Excel -->
                        <svg on:click|preventDefault={(e)=>getCallStatement(r.id,"xlsx")} 
                            class="w-6 h-6 text-green-700 hover:bg-white hover:shadow-lg hover:border-2" viewBox="0 0 24 24">
                            <path fill="currentColor" d="M16.2,17H14.2L12,13.2L9.8,17H7.8L11,12L7.8,7H9.8L12,10.8L14.2,7H16.2L13,12M19,3H5C3.89,3 3,3.89 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5C21,3.89 20.1,3 19,3Z" />
                        </svg>
                        <!-- Word -->
                        <svg on:click|preventDefault={(e)=>getCallStatement(r.id,"docx")}
                            class="w-6 h-6 text-blue-600 hover:bg-white hover:shadow-xl hover:border-2" viewBox="0 0 24 24">
                            <path fill="currentColor" d="M15.5,17H14L12,9.5L10,17H8.5L6.1,7H7.8L9.34,14.5L11.3,7H12.7L14.67,14.5L16.2,7H17.9M19,3H5C3.89,3 3,3.89 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5C21,3.89 20.1,3 19,3Z" />
                        </svg>
                    </div>
                </div>
                {/each}
        </div>
    </div>
</div>

{#if parameterDialog}
<Dialog bind:visible={parameterDialog} title="{intent.name} Parameters" w="1/3" >
    <div class="px-2 py-2 pb-4 max-w-md">
        <div class="flex flex-wrap content-start justify-evenly overscroll-y-auto">
            {#each (reportParameters||[]) as p}
            <div class="flex flex-col m-4">
                <label for="p_{p.name}">{p.prompt}</label>
                {#if p.dataType==='String'}
                
                    <input type="text" bind:value={p.value} id="p_{p.name}" placeholder="{p.prompt}" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
                
                {/if}
                {#if p.dataType==='Float'||p.dataType==='Integer'}
                
                    <input type="number" bind:value={p.value} id="p_{p.name}" placeholder="{p.prompt}" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
                
                {/if}
                {#if p.dataType==='DateTime'}
                
                    <Flatpickr bind:value={p.value}  id="p_{p.name}" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
                
                {/if}
                {#if p.dataType==='Boolean'}
                <div>
                    <input type="checkbox" id="p_{p.name}" bind:checked={p.value} class="inline-block align-middle" />
                    <label class="inline-block align-middle" for="p_{p.name}">{p.value?'True':'False'}</label>
                </div>
                {/if}
            </div>
            {/each}
        </div>
        <div class="flex flex-row items-center justify-end mb-2">
            <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={generateReport}>OK</button>
            <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e)=>{intent=null; reportParameters=[];parameterDialog=false; }}>Cancel</button>
        </div>
    </div>
</Dialog>
{/if}
{#if waiting }
  <div class="fixed inset-0 bg-gray-50 bg-opacity-10">
  </div>
  <div class="fixed inset-0 flex flex-col items-center place-items-center py-80">
    <Jumper size="80" color="#FF3E00" unit="px" duration="1s"></Jumper>
  </div>
{/if}
