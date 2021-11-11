<script>
    import { goto } from '@roxi/routify/runtime/helpers';
    import {user} from "../../model/user";
    import { onMount } from 'svelte';
    import DataTable from '../../components/datatable/DataTable.svelte';
    import Dialog from '../../components/Dialog.svelte';
    import { Jumper } from 'svelte-loading-spinners'
    import moment from 'moment';
    import {apiendpoint, vetris} from "../../store.js";  
    import Flatpickr from 'svelte-flatpickr'

	import 'flatpickr/dist/flatpickr.css'
	import 'flatpickr/dist/themes/light.css'
    import Pagination from '../../components/datatable/Pagination.svelte';
    import Swal from 'sweetalert2';
    
    let waiting=false;
	$: showwaiting=waiting;	
	let data = [];
    let datasets = [];
    let dataset;
    let datasetName;
    let columnMetadata = [];
    let filter_item_term="";

    $: total_records=0;
    $: page_size=13;
    $: current_page=1; 

    async function getMetaData () {

		const ds = datasets.find(i=>i.id===dataset);
      
        if(!ds) return [];
        try {
            waiting=true;
            const res = await fetch(`${$apiendpoint}/api/metadata/columns/${dataset}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': user.token()
                }
            });
            const json = await res.json();
            columnMetadata = json.result||[];
            heading = columnMetadata.filter(i=>i.default).map(function(i) { return {
                "field": i.name
            };});
        } catch {
            waiting=false;
        }
        
	}

    async function getData () {
       
        const ds = datasets.find(i=>i.id===dataset);
        if(!ds) return [];
        waiting=true;
        try {
            let postData = createFilterSortParameters();
            postData = Object.assign(postData, { pageSize: page_size, pageNo: current_page||1});
            const res = await fetch(`${$apiendpoint}/api/fetch/data`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': user.token()
                },
                body: JSON.stringify(postData)
            });
            
            const json = await res.json();
            // paging enabled
            total_records=json.result.totalRecords;
            current_page=json.result.currentPage;
            data = json.result.items;
        } catch (error) {
            waiting=false;
        }
        
	}
    async function changePage(e){
        current_page=e.detail;
        getData().then(()=> waiting=false);
    }

    async function getDatasets () {
        waiting=true;
        try{

        
            const res = await fetch(`${$apiendpoint}/api/metadata/datasets`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': user.token()
                }
            });
            
            const json = await res.json();
            datasets = json.result||[];
            if(datasets.length>0) {
                dataset=datasets[0].id;
                getMetaData().then(()=>{
                    setTimeout(()=>{
                        getData().then(()=>waiting=false);
                    },10);
                });
                
            } 
            else 
                waiting=false;
        } catch {
            waiting=false;
        }
        
	}
    let head;
    getDatasets();
    
    $: {
        let h = {};
        const ds=(datasets.find(i=>i.id==dataset)||{});
        if(ds){
            if(ds.actions){
                //VIEW_IMAGE,VIEW_INVOICE
                const btns=ds.actions.split(",");
                let buttons=[];
                btns.forEach(c=>{
                    if(c=="VIEW_INVOICE"){
                        buttons.push({
                                    prompt: '<svg class="h-4 w-4 text-red-600" viewBox="0 0 24 24"><path fill="currentColor" d="M19,3A2,2 0 0,1 21,5V19A2,2 0 0,1 19,21H5C3.89,21 3,20.1 3,19V5C3,3.89 3.89,3 5,3H19M10.59,10.08C10.57,10.13 10.3,11.84 8.5,14.77C8.5,14.77 5,16.58 5.83,17.94C6.5,19 8.15,17.9 9.56,15.27C9.56,15.27 11.38,14.63 13.79,14.45C13.79,14.45 17.65,16.19 18.17,14.34C18.69,12.5 15.12,12.9 14.5,13.09C14.5,13.09 12.46,11.75 12,9.89C12,9.89 13.13,5.95 11.38,6C9.63,6.05 10.29,9.12 10.59,10.08M11.4,11.13C11.43,11.13 11.87,12.33 13.29,13.58C13.29,13.58 10.96,14.04 9.9,14.5C9.9,14.5 10.9,12.75 11.4,11.13M15.32,13.84C15.9,13.69 17.64,14 17.58,14.32C17.5,14.65 15.32,13.84 15.32,13.84M8.26,15.7C7.73,16.91 6.83,17.68 6.6,17.67C6.37,17.66 7.3,16.07 8.26,15.7M11.4,8.76C11.39,8.71 11.03,6.57 11.4,6.61C11.94,6.67 11.4,8.71 11.4,8.76Z" /></svg>',
                                    tooltip: 'View invoice',
                                    _class: 'p-1 bg-bg-transparent',
                                    click: action_download_invoice
                        });
                    } else if (c=="VIEW_REPORT"){
                        buttons.push({
                                    prompt: '<svg class="h-4 w-4 text-pink-700" viewBox="0 0 24 24"><path fill="currentColor" d="M6,2A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2H6M6,4H13V9H18V20H6V4M8,12V14H16V12H8M8,16V18H13V16H8Z" /></svg>',
                                    tooltip: 'View final report',
                                    _class: 'p-1 bg-bg-transparent',
                                    click: action_view_report
                        });
                    } else {
                        buttons.push({
                                    prompt: '<svg class="h-4 w-4 text-blue-600 " viewBox="0 0 24 24"><path fill="currentColor" d="M13,9H18.5L13,3.5V9M6,2H14L20,8V20A2,2 0 0,1 18,22H6C4.89,22 4,21.1 4,20V4C4,2.89 4.89,2 6,2M6,20H15L18,20V12L14,16L12,14L6,20M8,9A2,2 0 0,0 6,11A2,2 0 0,0 8,13A2,2 0 0,0 10,11A2,2 0 0,0 8,9Z" /></svg>',
                                    tooltip: 'View Images',
                                    _class: 'p-1 bg-bg-transparent',
                                    click: action_view_image
                        });
                    }

                });
                h["_action_"]={
                            value: "Actions", 
                            action:true,
                            component:{
                                buttons: buttons
                            }
                        };
            }
        }
        heading.forEach(i=>{
            const d = columnMetadata.find(item=>item.name===i.field);
            if(d){
                if(d.type==="Date"){
                    h[i.field]={value: d.description, format:DateFormatter, _class:'datetime-col-width' };
                }
                else if(d.type==="number"){
                    if(d.description.toLowerCase().indexOf("amount")>-1){
                        h[i.field]={value: d.description, format:MoneyFormatter, _class:'text-right' };
                    } else {
                        h[i.field]={value: d.description, _class:'text-right' };
                    }
                } else {
                    h[i.field]={value: d.description, _class:'general-col-width'};
                }
                if(i.hasOwnProperty("filter"))
                    h[i.field]=Object.assign(h[i.field], {filter: true});
                if(i.hasOwnProperty("sort"))
                    h[i.field]=Object.assign(h[i.field],{sort: i.sort});
            }
            
        });
        head = Object.assign({}, h);
        datasetName=ds.name||'None';
    }


    async function action_view_image(e, row, val){
        waiting=true;
        let parametermissing=false;
        if(row.accession_no===undefined) parametermissing=true;
        if((row.patient_id_pacs||null)===null) parametermissing=true; 
        if(parametermissing){
            Swal.fire({
                title: "Warning",
                text: "Accession No and/or Patient Id does not exist!",
                buttonsStyling: false,
                confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                icon: "warning"
                });
                waiting=false;
            return;
        }
        const res = await fetch(`${$apiendpoint}/api/user/getviewer?accno=${row.accession_no}&patient=${row.patient_id_pacs}`, {
            method: 'GET',
           headers: {
               'Content-Type': 'application/json',
               'Authorization': user.token()
           }
        });
        
        const json = await res.json();
        waiting=false;
        let win=window.open(json.result,"_blank");
        win.focus();
    }
    async function action_view_report(e, row, val){
        waiting=true;
        let parametermissing=false;
        if(row.id===undefined) parametermissing=true;
        if(row.patient_name===undefined) parametermissing=true;
        if(parametermissing){
            Swal.fire({
                title: "Warning",
                text: "Id and/or Patient name does not exist in query!",
                buttonsStyling: false,
                confirmButtonClass: "btn bg-red-400 text-white focus:outline-none",
                icon: "warning"
                });
                waiting=false;
            return;
        }
        const res = await fetch(`${$apiendpoint}/api/user/getreport?id=${row.id}&patient=${row.patient_name}&type=${(row.custom_report||'N')=='Y'?1:3}`, {
            method: 'GET',
           headers: {
               'Content-Type': 'application/json',
               'Authorization': user.token()
           }
        });
        
        const json = await res.json();
        waiting=false;
        if(json.result && json.result.path!=null){
            let win=window.open(`${$vetris}/${json.result.path}`,"_blank");
            win.focus();
        }
        if(json.result && json.result.hasError=="Y"){
            Swal.fire({
                title: "Error",
                text: json.result.errorMessage,
                buttonsStyling: false,
                confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                icon: "error"
                });
        }
    }

    async function action_download_invoice(e, row, val){
        waiting=true;
        let parametermissing=false;
        if(row.billing_cycle_id===undefined) parametermissing=true;
        if(row.billing_account_id===undefined) parametermissing=true;
        if(parametermissing){
            Swal.fire({
                title: "Warning",
                text: "Billing cycle Id and/or Billing account Id does not exist in query!",
                buttonsStyling: false,
                confirmButtonClass: "btn bg-red-400 text-white focus:outline-none",
                icon: "warning"
                });
                waiting=false;
            return;
        }
        const res = await fetch(`${$apiendpoint}/api/user/getinvoice?cycleId=${row.billing_cycle_id}&accountId=${row.billing_account_id}`, {
            method: 'GET',
            headers: {
               'Content-Type': 'application/json',
               'Authorization': user.token()
           }
        });
        
        const json = await res.json();
        waiting=false;
        if(json.result && json.result.path!=null){
            let win=window.open(`${$vetris}/${json.result.path}`,"_blank");
            win.focus();
        }
        if(json.result && json.result.hasError=="Y"){
            Swal.fire({
                title: "Error",
                text: json.result.errorMessage,
                buttonsStyling: false,
                confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                icon: "error"
                });
        }
    }   


    let heading = [];
    let selected_col;

	if(!user.isAuthenticated())
		$goto('/login');
	

    let filterdialog=false;
    let filter=0;
    let sorting=0;
    let filterOptions = [
        {id:0, text:'All'},
        {id:1, text:'Public'},
        {id:2, text:'Owned by me'},
        {id:3, text:'Shared with me'},
    ];
    let sortOptions = [
        {id:0, text:'Recently updated'},
        {id:1, text:'Report name (A-Z)'},
        {id:2, text:'Report name (Z-A)'}
    ];
    let el;
    let colpanel;
    let maxHeight = '100%';
    let colpanelHeight = '100%';
    let maxWidth = '100%';
    $: slimscrollOptions={};
    onMount(()=>{
        if(window.innerHeight-el.offsetTop-41>=0){
            maxHeight=(window.innerHeight-el.offsetTop-41)+"px";
            slimscrollOptions.height = (window.innerHeight-el.offsetTop-41)+"px";
        } 
        else {
           
            maxHeight="100%";
            slimscrollOptions.height = "100px";
        }
        if(window.innerWidth-el.offsetLeft>=0){
            maxWidth=(window.innerWidth-el.offsetLeft)+"px";
        } 
        else {
            maxWidth="100%";
        }
        colpanelHeight=(window.innerHeight-drop_zone.offsetTop-61)+"px";
        const pg = parseInt((window.innerHeight-el.offsetTop-80)/36);
        if(pg>1) page_size = pg;
        else page_size = 10;
    });
    function handleResize(e) {
        if(window.innerHeight-el.offsetTop-41>=0){
            maxHeight=(window.innerHeight-el.offsetTop-41)+"px";
            slimscrollOptions.height = (window.innerHeight-el.offsetTop-41)+"px";
        } 
        else {
            maxHeight="100%";
            slimscrollOptions.height = "100px";
        } 
        
        if(window.innerWidth-el.offsetLeft>=0){
            maxWidth=(window.innerWidth-el.offsetLeft)+"px";
        } 
        else {
            maxWidth="100%";
        }
        colpanelHeight=(window.innerHeight-drop_zone.offsetTop-61)+"px";
        const pg = parseInt((window.innerHeight-el.offsetTop-80)/36);
        if(pg>1) page_size = pg;
        else page_size = 10;
    }


    function createFilterSortParameters(){
        let cols=[];
        heading.forEach(i=>{
            const meta=columnMetadata.find(j=>j.name===i.field);
            let col={column: i.field, type: meta.type, title: meta.description };
            if(i.filter && i.filter.operator) {
                if(i.filter.operator=="between") {
                    if(meta.type==="Date"){
                        //col = Object.assign(col, { operator: i.filter.operator, value1: { dateValue: i.filter.range[0]}, value2:{ dateValue: i.filter.range[1]} });
                        col = Object.assign(col, { operator: i.filter.operator, value1: { dateValue: moment(i.filter.range[0]).format("YYYY-MM-DD")}, value2:{ dateValue: moment(i.filter.range[1]).format("YYYY-MM-DD")} });
                    } else if(meta.type==="number"){
                        col = Object.assign(col, { operator: i.filter.operator, value1:{ numberValue: new Number(i.filter.range[0])}, value2:{ numberValue:new Number(i.filter.range[1])} });
                    }
                }
                else {
                    if(meta.type==="Date"){
                        //col = Object.assign(col, { operator: i.filter.operator, value1: { dateValue: i.filter.value}});
                        col = Object.assign(col, { operator: i.filter.operator, value1: { dateValue: moment(i.filter.value).format("YYYY-MM-DD")} });
                    } else if(meta.type==="number"){
                        col = Object.assign(col, { operator: i.filter.operator, value1:{ numberValue: new Number(i.filter.value)} });
                    } else  if(meta.type==="boolean"){
                        col = Object.assign(col, { operator: i.filter.operator, value1:{ booleanValue: new Boolean(i.filter.value)} });
                    } else {
                        col = Object.assign(col, { operator: i.filter.operator, value1:{ stringValue: i.filter.value } });
                    }
                }
            }
            if(["asc","desc"].indexOf(i.sort||"")>-1){
                col = Object.assign(col, {sortDirection: i.sort });
            } 
            cols = [...cols, col];
        });
        const ds = datasets.find(i=>i.id===dataset);

        return { id:dataset, objectName: ds.objectName, title: ds.name, columns: cols };
    }

    let drop_zone;

    
    function table_sort(e){
        let field = heading.find(i=>i.field===e.detail);
        if(!field) return;
        if(field.sort===undefined){
            field.sort="asc";
        } else if(field.sort==="asc"){
            field.sort="desc";
        } else if(field.sort==="desc"){
            delete field["sort"];
        }
        heading = [...heading];
        setTimeout(()=>{
            getData().then(()=>waiting=false);
        },10)
    }
    function table_drop(e){
        
        const drop_col=e.detail.event.dataTransfer.getData("column");
        if(drop_col){
            const targetIndex= heading.findIndex(i=> i.field === e.detail.key);
            heading.splice(targetIndex,0,{field: drop_col});
        }
        else {
            const move_col=e.detail.event.dataTransfer.getData("tablecolumn");
            if(move_col){
                const targetIndex=e.detail.index;
                const sourceIndex = heading.findIndex(i=> i.field === move_col);
                const src=heading[sourceIndex];
                heading.splice(sourceIndex,1);
                heading.splice(targetIndex,0,Object.assign({}, src));
            }
        }
        heading = [...heading];
    }

    function table_filter(e){
        selected_col=e.detail;
        selected_head=heading.find(i=>i.field===selected_col);
        if(!!!selected_head) return;
        setTimeout(()=>{
            filterdialog=true;
            if(selected_head!=null){
                filter_operator=(selected_head.filter && selected_head.filter.operator)||'eq';
                filter_range = ["",""];
                if(filter_operator=="between" && selected_head.filter && selected_head.filter.range.length==2){
                    filter_range=[...selected_head.filter.range];
                } else if(filter_operator!="between"){
                    filter_range[0]= (selected_head.filter && selected_head.filter.value)||null;   
                }
            }
        },100);
    }

    function col_dragenter(e){
        
    }
    function col_dragleave(e){
        
    }
    function col_drop(e){
        const drop_col=e.dataTransfer.getData("tablecolumn");
        if(!drop_col) return;
        if(heading.length==1) return;
        const hpos= heading.findIndex(i=> i.field === drop_col);
        if(hpos==-1) return;
        heading.splice(hpos,1);
        heading = [...heading];

    }

    function handleDragStart(e, colname, index){
        e.dataTransfer.dropEffect = "move";
        e.dataTransfer.setData("column", colname);
    }
    function handleDragEnd(e, colname, index){
        
    }

    let filtertype="string";
    let filterdesc="";
    let selected_head;
    let filter_operator="eq";
    let filter_operators = { 
        string: [
            {id: "eq", text:  "Equals"},
            {id: "neq", text:  "Not Equals"},
            {id: "like", text:  "Like"},
            {id: "notlike", text:  "Not Like"}
        ],
        number: [
            {id: "eq", text:  "Equals"},
            {id: "neq", text:  "Not Equals"},
            {id: "gt", text:  "Greater Than"},
            {id: "gte", text:  "Greater Than or Equal"},
            {id: "lt", text:  "Less Than"},
            {id: "lte", text:  "Less Than or Equal"},
            {id: "between", text:  "Between"}
        ],
        Date: [
            {id: "eq", text:  "Equals"},
            {id: "neq", text:  "Not Equals"},
            {id: "gt", text:  "Greater Than"},
            {id: "gte", text:  "Greater Than or Equal"},
            {id: "lt", text:  "Less Than"},
            {id: "lte", text:  "Less Than or Equal"},
            {id: "between", text:  "Between"}
        ],
        boolean: [
            {id: "eq", text:  "Equals"},
            {id: "neq", text:  "Not Equals"}
        ]
        
    };
    let filter_range=Array(2);
    $: {
        let field=null;
        if(selected_col){
            field = columnMetadata.find(i=>i.name===selected_col);
            selected_head=heading.find(i=>i.field===selected_col);
        }
        if(field){
            filtertype=field.type;
            filterdesc = field.description;
        }
    }
    function filter_clear(e){
        delete selected_head["filter"];
        filterdialog=false;
        selected_col=null;
        selected_head=null;
        heading = [...heading]; 
        setTimeout(()=>{
            current_page=1;
            data=[];
            getData().then(()=>waiting=false);
        },10)  
    }

    function filter_accept(e){
        if(filter_operator==="between"){
            if((filter_range[0]+"")===""||(filter_range[1]+"")===""){
                return;
            }
        }
        else if(filtertype==="number"){
            if((filter_range[0]+"")===""){
                return;
            }
        }
        if(filter_operator==="between")
            selected_head.filter={operator: filter_operator, range: [...filter_range]};
        else
            selected_head.filter={operator: filter_operator, value: filter_range[0]};
        filterdialog=false;
        selected_col=null;
        selected_head=null;
        heading = [...heading];  
        setTimeout(()=>{
            current_page=1;
            data=[];
            getData().then(()=>waiting=false);
        },10)  
    }

    function DateFormatter(val){
        if(!val) return val;
        const dt=new Date(val);
        return moment(dt).format("MMM DD, YYYY HH:mm");
    }
    
    function MoneyFormatter(val){
        if(val===null||val===undefined) return val;
        const n=new Number(val);
        return n.toLocaleString('en-US', { style: 'currency', currency: 'USD' });;
    }

    function datasetchanged(e){
        data=[];
        waiting=true;
        getMetaData().then(()=>getData().then(()=>waiting=false));
    }
    async function download(e){
        const ds = datasets.find(i=>i.id===dataset);
        waiting=true;
        const postData = createFilterSortParameters();
		const res = await fetch(`${$apiendpoint}/api/exportexcel`, {
			method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': user.token()
            },
            body: JSON.stringify(postData)
		});
		
		const json = await res.json();
		if(!json.isError){
            location.href = `${$apiendpoint}/file/downloadtemporaryfile?contentType=${json.result.fileType}&id=${json.result.fileToken}&fileName=${json.result.fileName}`;
        }
        waiting=false;
    }
</script>
<svelte:window on:resize={handleResize}></svelte:window>
<div class="flex flex-row w-full" on:resize={handleResize}>
    <div  class="w-1/5 border-r shadow-md p-2 bg-gray-100">
        <ul class="list-none w-full">
           <li class="list-none p-1">
                <div class="flex flex-row items-center justify-between">
                    <svg class="h-5 w-5 mr-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 12h14M5 12a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v4a2 2 0 01-2 2M5 12a2 2 0 00-2 2v4a2 2 0 002 2h14a2 2 0 002-2v-4a2 2 0 00-2-2m-2-4h.01M17 16h.01" />
                    </svg>
                    <select class="flex flex-auto p-1 my-1 border rounded-sm hover:outline-none focus:outline-none" 
                        bind:value={dataset} 
                        on:change={datasetchanged}
                        style="width: 100%;" >
                        {#each datasets as opt}
                            <option value={opt.id}>
                                {opt.name}
                            </option>
                        {/each}
                    </select>
                </div>
                <div class="flex flex-row my-1 items-center justify-between">
                    <svg class="h-5 w-5 mr-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z" />
                    </svg>
                    <span class="flex flex-auto">Available Columns</span>
                    
                </div>
                <div>
                    <div class="flex flex-1 text-sm items-center bg-white p-1 border rounded-sm mt-1">
                        <input type="text" bind:value={filter_item_term} class="w-full hover:outline-none focus:outline-none bg-transparent" placeholder="Search">
                        {#if filter_item_term.length===0 }
                        <svg class="w-4 h-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                            <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
                        </svg>
                        {:else}
                        <svg class="w-4 h-4" viewBox="0 0 24 24" on:click={(e)=> filter_item_term=""}>
                            <path fill="currentColor" d="M19,6.41L17.59,5L12,10.59L6.41,5L5,6.41L10.59,12L5,17.59L6.41,19L12,13.41L17.59,19L19,17.59L13.41,12L19,6.41Z" />
                        </svg>
                        {/if}
                    </div>
                    <ul class="list-none w-full pl-2  overflow-y-scroll"  style="max-height:{colpanelHeight};"
                        bind:this={drop_zone}
                        on:dragenter={col_dragenter}
                        on:dragleave={col_dragleave}
                        on:drop={col_drop}
                        ondragover="return false">
                        
                        {#each columnMetadata.filter(i=>filter_item_term?(i.description.toLowerCase().indexOf(filter_item_term.toLowerCase())!=-1):true) as h,index}
                            <li class="list-none flex flex-row items-center p-1 border-b {heading.findIndex(i=>i.field===h.name)==-1?'':'font-medium bg-gray-200'}" 
                                data-col={h.field} 
                                draggable={heading.findIndex(i=>i.field===h.name)==-1} 
                                on:dragstart={(e)=> handleDragStart(e, h.name, index)}
                                on:dragend={(e)=> handleDragEnd(e, h.name, index)}>
                                <span class="flex flex-auto text-sm">{h.description}</span>
                                <span class="w-5 h-5 text-blue-600 text-xs pt-0.5 {heading.findIndex(i=>i.field===h.name)==-1?'hidden':''}" >
                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                                        <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                                    </svg>
                                </span>
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
                <div class="flext-auto">
                    <div class="text-lg">Column Report: <span class="font-medium">{datasetName}</span> </div>
                </div>
                <div class="btn text-sm ml-6 bg-gray-100 hover:bg-blue-500 hover:text-white p-1.5 cursor-pointer" on:click|preventDefault={download}>
                    <svg class="w-6 h-6 p-0.5 inline-block" viewBox="0 0 24 24">
                        <path fill="currentColor" d="M21.17 3.25Q21.5 3.25 21.76 3.5 22 3.74 22 4.08V19.92Q22 20.26 21.76 20.5 21.5 20.75 21.17 20.75H7.83Q7.5 20.75 7.24 20.5 7 20.26 7 19.92V17H2.83Q2.5 17 2.24 16.76 2 16.5 2 16.17V7.83Q2 7.5 2.24 7.24 2.5 7 2.83 7H7V4.08Q7 3.74 7.24 3.5 7.5 3.25 7.83 3.25M7 13.06L8.18 15.28H9.97L8 12.06L9.93 8.89H8.22L7.13 10.9L7.09 10.96L7.06 11.03Q6.8 10.5 6.5 9.96 6.25 9.43 5.97 8.89H4.16L6.05 12.08L4 15.28H5.78M13.88 19.5V17H8.25V19.5M13.88 15.75V12.63H12V15.75M13.88 11.38V8.25H12V11.38M13.88 7V4.5H8.25V7M20.75 19.5V17H15.13V19.5M20.75 15.75V12.63H15.13V15.75M20.75 11.38V8.25H15.13V11.38M20.75 7V4.5H15.13V7Z" />
                    </svg>
                    Export to Excel
                </div>
                
            </div>
 
        </div>
        <div bind:this={el} class="p-1 overflow-auto" style="max-height:{maxHeight}; max-width:{maxWidth};">
            
            <DataTable {head} 
                rows={data}
                on:sort={table_sort}
                on:drop={table_drop} 
                on:dblclick={table_filter}
                />
            {#if data.length}    
                <div class="fixed right-4 bottom-10 flex flex-row items-center justify-between">
                    <div class="flex-1 p-1 mr-10">Showing {(current_page-1)*page_size+1} - {Math.min((current_page)*page_size,total_records)} of {total_records} records.</div>
                    <Pagination small rounded 
                        current={current_page} 
                        num_items={total_records} 
                        per_page={page_size}
                        on:navigate={changePage}  />
                </div>    
            {/if}
        </div>
    </div>
</div>


<Dialog bind:visible={filterdialog} title="Filter: {filterdesc}">
    <div class="px-2 py-2 pb-4 flex flex-row items-center">
        <label for="operator"  class="block font-semibold">Operator:</label>
        <select bind:value={filter_operator} class=" mx-2 py-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm">
            {#each filter_operators[filtertype||"string"] as opt}
                <option value={opt.id}>
                    {opt.text}
                </option>
            {/each}
        </select>
    </div>
    <div class="px-2 py-2 pb-4 flex flex-row items-center">
        <label for="filter_value" class="block font-semibold">Value:</label>
        {#if filtertype==='Date'}
        <Flatpickr bind:value={filter_range[0]} id="filter_value" class="border w-full h-5 px-2 py-5 mx-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
        {:else}
            {#if filtertype=='number'}
                <input type="number" autocomplete="false" bind:value={filter_range[0]} id="filter_value" class="border w-full h-5 px-2 py-5 mx-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
            {:else}
                <input type="text" autocomplete="false" bind:value={filter_range[0]} id="filter_value" class="border w-full h-5 px-2 py-5 mx-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
            {/if}
        {/if}
        {#if filter_operator==="between"}
            {#if filtertype==='Date'}
                <Flatpickr bind:value={filter_range[1]} id="filter_value2" class="border w-full h-5 px-2 py-5 mx-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
            {:else}

                {#if filtertype=='number'}
                    <input type="number" autocomplete="false" bind:value={filter_range[1]} id="filter_value2" class="border w-full h-5 px-2 py-5 mx-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
                {:else}
                    <input type="text" autocomplete="false" bind:value={filter_range[1]} id="filter_value2" class="border w-full h-5 px-2 py-5 mx-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
                {/if}
            {/if}
        {/if}
    </div>
    <div class="flex flex-row items-center justify-end mb-2">
      <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click={filter_accept} >OK</button>
      {#if selected_head.filter!==undefined }
      <button class="p-2 mx-2 bg-yellow-400 text-white rounded-sm w-32 hover:outline-none focus:outline-none" on:click={filter_clear}>Clear Filter</button>
      {/if}
      <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click={(e) => filterdialog=false}>Cancel</button>
    </div>
  </Dialog>

  {#if waiting }
  <div class="fixed inset-0 bg-gray-50 bg-opacity-10">
  </div>
  <div class="fixed inset-0 flex flex-col items-center place-items-center py-80">
    <Jumper size="80" color="#FF3E00" unit="px" duration="1s"></Jumper>
  </div>
  {/if}