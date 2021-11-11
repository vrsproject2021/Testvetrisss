<script>
    import { createEventDispatcher } from "svelte";
    import * as report from "../../model/report/report.js"; 
    import Editor from "./Editor.svelte";
    import Swal from 'sweetalert2';

    export let reportData=null;
    export let initiate=false;
    let editor;
    let dataSourceName, beingAdded=false, setBeingAdded=false,setBeingEdited=false;
    let name='', provider, connectionString, userID, database,server='', password ; 
    let dsName, commandText;
    let search = null;
    let commandMaximize=false;

    const providers = [
            { name:"Microsoft SQL Server", value:"System.Data.SqlClient"}
    ]
    const dispatch = createEventDispatcher();

    function onChangeDataSource(e) {
        dispatch("datasourcechange");
    }
    $: commandHeight=commandMaximize?'100%':'300px';

    $: hasDS = reportData && reportData.dataSources && reportData.dataSources.dataSource && reportData.dataSources.dataSource.length>0;
    $: hasDataset = reportData.dataSets.dataSet && reportData.dataSets.dataSet.length>0;
    $: connectionString = `Server=${server||''}; User ID=${userID||''}; Password=${password}; Persist Security Info=True; Database=${database};`
    $: validateDatasourceName = beingAdded && (name||"").match(/^\w{3,30}$/);
    $: validateDatasourceServer = beingAdded && 
            ((server||"").match(/^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$/)!=null ||
            (server||"").match(/^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$/)!=null);
    
    $:editableDataset=reportData.dataSets.dataSet && reportData.dataSets.dataSet.length>0 && dsName;        
    $:{
        if(reportData.dataSources.dataSource.length>0)
            dataSourceName=reportData.dataSources.dataSource[0].name;
        if(reportData.dataSets.dataSet.length>0)
            dsName=reportData.dataSets.dataSet.name;
        if(beingAdded && provider===undefined) provider=providers[0].value;

        if(!beingAdded && initiate===true) {
            beingAdded=true;
            initiate=false;
        }
        
    }   
    function addDataSource(e){
        report.addDataSource(reportData, name,provider, connectionString);
        onChangeDataSource();
    }
    async function addDataSet(e){

        if(setBeingAdded && !dsName){
            Swal.fire({
                title: "Warning",
                text: "Dataset name was not entered!",
                buttonsStyling: false,
                confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                icon: "warning"
                });
            return;
        }
        commandText=editor.getValue();
        if(!commandText.trim()){
            Swal.fire({
                title: "Warning",
                text: "SQL Query was not entered!",
                buttonsStyling: false,
                confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                icon: "warning"
                });
            return;
        }
        const params=(commandText.match(/(@\w*)/g)??[]).filter((m,i,a)=> a.indexOf(m)==i).map(i=> i.substring(1));

        if(params.length>0){
            const report_params=report.getParameters(reportData)
                .map(i=>i.name);
            const not_found = params.filter(i=>report_params.indexOf(i)==-1);
            if(not_found.length>0){
                Swal.fire({
                    title: "Warning",
                    text: `First you have to add following parameters: ${not_found.join(', ')}`,
                    buttonsStyling: false,
                    confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                    icon: "warning"
                    });
                return;
            }
        }
        if(setBeingAdded){
            await report.addDataSet(reportData, dsName, dataSourceName, commandText, params).then(x=>{
                setBeingAdded=beingAdded=false;
                onChangeDataSource();
            })
            .catch(e=>{
                report.removeDataset(reportData, dsName);
                onChangeDataSource();
            });
        } else if(setBeingEdited){
            
            await report.updateDataSet(
                    reportData, 
                    reportData.dataSets.dataSet[0].name, 
                    reportData.dataSets.dataSet[0].query.dataSourceName, 
                    commandText, params).then(x=>{
                setBeingEdited=false;
                onChangeDataSource();
            })
            .catch(e=>{
                setBeingEdited=false;
                dsName=reportData.dataSets.dataSet[0].name;
                Swal.fire({
                    title: "Warning",
                    text: "SQL Query has errors! reverting...",
                    buttonsStyling: false,
                    confirmButtonClass: "btn bg-yellow-400 focus:outline-none",
                    icon: "warning"
                });
                
                onChangeDataSource();
            });
        }
    }
    let fullscreencontainer, fullscreeneditcontainer, fullscreentitle, fheight=0;
    function resizeFullContainer(e){
        fheight=fullscreencontainer.offsetHeight-fullscreentitle.offsetHeight;
    }
    $:fheight = fullscreencontainer && (fullscreencontainer.offsetHeight-fullscreentitle.offsetHeight)||0;

    function editDataSet(e){
        commandText=reportData.dataSets.dataSet[0].query.commandText;
        setBeingEdited=true;
        commandMaximize=false;
    }
</script>
{#if commandMaximize}
<div bind:this={fullscreencontainer} 
    class="{commandMaximize?'fixed inset-0':'hidden'}" style="{commandMaximize?'z-index:1000':''}">
    <div bind:this={fullscreentitle} class="flex flex-row box__title bg-gray-300 px-3 py-2 border-b">
        <h3 class="flex-auto text-sm text-grey-darker font-medium">SQL Query</h3>
        <svg 
            xmlns="http://www.w3.org/2000/svg"
            on:click={(e)=> { commandText=editor.getValue(); commandMaximize=false; dispatch("hiderighttoolbar",false);}}
            class="h-5 w-5 mr-0.5 mt-0.5" viewBox="0 0 24 24">
            <path fill="currentColor" d="M14,14H19V16H16V19H14V14M5,14H10V19H8V16H5V14M8,5H10V10H5V8H8V5M19,8V10H14V5H16V8H19Z" />
        </svg>
    </div>
    <div bind:this={fullscreeneditcontainer} class="flex-auto w-full h-full" style="height:{fheight}px;">
        <Editor 
                bind:this={editor} 
                value={commandText} 
                height={commandHeight} 
                theme={'vs'}
                language={'sql'} 
                lineNumbers={'on'}
                />
    </div>
</div>
{/if}

<div class="bg-white font-medium p-1 px-2 border-b shadow-sm">Data Source</div>
{#if !hasDS}
    {#if !beingAdded}
    <div class="bg-red-100 font-light p-2 flex flex-row items-center">
        <svg class="w-8 h-8 text-yellow-500" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd" />
        </svg>
        <div class="font-light p-2"> No data source has been added!</div>
        <button class="bg-blue-500 text-white p-1.5 m-1 rounded-sm focus:outline-none" on:click|preventDefault={(e)=> beingAdded=true }>Add data source</button>
    </div>
    {/if}
    {#if beingAdded}
        <div class="flex flex-col  overflow-y-auto">
            <div class="flex flex-row p-1">
                <label class="w-1/3 p-1 font-semibold text-sm" for="provider">Provider</label> 
                <select bind:value={provider} 
                    class="w-2/3 p-1 border hover:outline-none focus:outline-none">
                    {#each providers as opt}
                        <option value={opt.value}>
                            {opt.name}
                        </option>
                    {/each}
                </select> 
            </div>
            
            <div class="flex flex-row p-1 place-items-start">
                <label class="w-1/3 p-1 font-semibold text-sm" for="name">Name</label> 
                <div class="flex flex-col w-2/3">
                    <input type="text" bind:value={name} class="p-1 mx-1 border rounded-sm hover:outline-none focus:outline-none bg-transparent" name="name" id="name" />
                    {#if !validateDatasourceName }
                    <span class="flex items-center font-medium tracking-wide text-red-500 text-xs mt-1 ml-1">
                        Invalid name.
                    </span>
                    {/if}
                </div>
                
            </div>
            
            <div class="flex flex-row p-1 place-items-start">
                <label class="w-1/3 p-1 font-semibold text-sm" for="server">Server</label> 
                <div class="flex flex-col w-2/3">
                    <input type="text" bind:value={server} class="p-1 mx-1 border rounded-sm hover:outline-none focus:outline-none bg-transparent" name="server" id="server" />
                    {#if !validateDatasourceServer }
                    <span class="ml-1 flex items-center font-medium tracking-wide text-red-500 text-xs mt-1">
                        Invalid server name.
                    </span>
                    {/if}
                </div>
            </div>
                
            
            <div class="flex flex-row p-2 items-center">
                <label class="w-1/3 p-1 font-semibold text-sm" for="database">Database</label> 
                <input type="text" bind:value={database} class="w-2/3 p-1 mx-1 border rounded-sm hover:outline-none focus:outline-none bg-transparent" name="database" id="database" />
            </div>
            <div class="flex flex-row p-2 items-center">
                <label class="w-1/3 p-1 font-semibold text-sm" for="userid">User Id</label> 
                <input type="text" bind:value={userID} noautofill class="w-2/3 p-1 mx-1 border rounded-sm hover:outline-none focus:outline-none bg-transparent" name="userid" id="userid" />
            </div>
            <div class="flex flex-row p-2 items-center">
                <label class="w-1/3 p-1 font-semibold text-sm" for="password">Password</label> 
                <input type="password" bind:value={password} noautofill class="w-2/3 p-1 mx-1 border rounded-sm hover:outline-none focus:outline-none bg-transparent" name="password" id="password" />
            </div>
            <div class="font-light p-2 flex flex-row items-center">
                <div class="flex-auto"></div>
                {#if (validateDatasourceName && validateDatasourceServer)}
                    <button class="bg-blue-500 text-white p-1.5 m-1 w-14 rounded-sm focus:outline-none" on:click|preventDefault={addDataSource}>Add</button>
                {:else}
                    <button class="bg-blue-500 opacity-60 text-white p-1.5 m-1 w-14 rounded-sm focus:outline-none" disabled >Add</button>
                {/if}
                <button class="bg-gray-300 p-1.5 m-1 w-14 rounded-sm focus:outline-none" on:click|preventDefault={(e)=> {beingAdded=false; initiate=false;} }>Cancel</button>
            </div>
        </div>
    {/if}    
{/if}
 {#if hasDS}
    <div class="flex-auto flex flex-col p-2 overflow-y-auto">
        {#if !hasDataset}
            <div class="flex flex-row items-center mt-2">
                <label class="w-1/3 p-1 font-semibold text-sm" for="name">Source name</label> 
                <select bind:value={dataSourceName} 
                        class="w-2/3 p-1 border hover:outline-none focus:outline-none">
                    {#each reportData.dataSources.dataSource as opt}
                        <option value={opt.name}>
                            {opt.name}
                        </option>
                    {/each}
                </select> 
            </div>
            <div class="flex flex-row mt-2">
                <label class="w-1/3 p-1 font-semibold text-sm" for="dsname">Dataset Name</label> 
                <input type="text" bind:value={dsName} class="w-2/3 border rounded-sm hover:outline-none focus:outline-none bg-transparent" name="dsname" id="dsname" />
            </div>

            {#if !commandMaximize}
            <div class="flex flex-col mt-2">

                <div class="box border rounded flex flex-col shadow bg-white">
                    <div class="flex flex-row box__title bg-gray-300 px-3 py-2 border-b">
                        <h3 class="flex-auto text-sm text-grey-darker font-medium">SQL Query</h3>
                        <svg 
                            xmlns="http://www.w3.org/2000/svg" 
                            on:click={(e)=> { commandText=editor.getValue(); commandMaximize=true; dispatch("hiderighttoolbar",true);}}
                            class="h-5 w-5 mr-0.5 mt-0.5 {commandMaximize?'hidden':''}" viewBox="0 0 20 20" fill="currentColor">
                            <path stroke="#374151" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8V4m0 0h4M3 4l4 4m8 0V4m0 0h-4m4 0l-4 4m-8 4v4m0 0h4m-4 0l4-4m8 4l-4-4m4 4v-4m0 4h-4" />
                        </svg>
                        
                    </div>
                    
                    <!-- <textarea class="text-grey-darkest flex-1 p-2 m-1 bg-transparent hover:outline-none focus:outline-none" bind:value={commandText} rows="10" /> -->
                    <Editor 
                        bind:this={editor} 
                        value={commandText} 
                        height={commandHeight} 
                        theme={'vs'}
                        language={'sql'} 
                        lineNumbers={'on'}
                        />
                </div>

            </div>
            {/if}
            <div class="font-light flex flex-row items-center mt-2">
                <div class="flex-auto"></div>
                <button class="bg-blue-500 text-white p-1.5 m-1 w-14 rounded-sm focus:outline-none" on:click|preventDefault={(e)=>{ setBeingAdded=true; addDataSet(e);}}>Add</button>
                <button class="bg-gray-300 p-1.5 m-1 w-14 rounded-sm focus:outline-none" on:click|preventDefault={(e)=> setBeingAdded=beingAdded=false }>Cancel</button>
            </div>
        {/if}

        {#if hasDataset}
            <div class="flex flex-row items-center mt-2">
                <label class="w-1/3 p-1 font-semibold text-sm" for="name">Dataset name</label> 
                <select bind:value={dsName} 
                        class="w-2/3 p-1 border hover:outline-none focus:outline-none">
                    {#each reportData.dataSets.dataSet as opt}
                        <option value={opt.name}>
                            {opt.name}
                        </option>
                    {/each}
                </select> 
                <button class="bg-blue-500 text-white p-1.5 m-1 w-14 rounded-sm focus:outline-none {setBeingEdited?'hidden':''}" on:click|preventDefault={(e)=>{ setBeingEdited=true; editDataSet(e);}}>Edit</button>
            </div>
            {#if !setBeingEdited}
                <div class="flex flex-col w-full mt-2">
                    <div class="flex flex-row border">
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
                        <div class="flex w-20 hover:text-gray-900">Type</div>
                    </div>
                    <div class="overflow-y-auto border-b border-l border-r" style="max-height:80%;"> 
                        {#each reportData.dataSets.dataSet[0].fields.field.filter(i=>search?i.dataField.toLowerCase().indexOf(search.toLowerCase()) >= 0 :true) as field}
                        <div class="flex flex-row items-center justify-between space-x-1 p-2 text-gray-900 hover:bg-gray-200">
                            <div class="flex flex-1 ">{field.dataField}</div>
                            <div class="flex w-16 text-white"><span class="ml-2 bg-green-800 rounded-sm">{@html report.getFieldType(field.typeName,"html")}</span></div>
                        </div>
                        {/each}
                    </div>
                </div>
            {/if}
            {#if setBeingEdited}
                {#if !commandMaximize}
                    <div class="flex flex-col mt-2">
            
                        <div class="box border rounded flex flex-col shadow bg-white">
                            <div class="flex flex-row box__title bg-gray-300 px-3 py-2 border-b">
                                <h3 class="flex-auto text-sm text-grey-darker font-medium">SQL Query</h3>
                                <svg 
                                    xmlns="http://www.w3.org/2000/svg" 
                                    on:click={(e)=> { commandText=editor.getValue(); commandMaximize=true; dispatch("hiderighttoolbar",true);}}
                                    class="h-5 w-5 mr-0.5 mt-0.5 {commandMaximize?'hidden':''}" viewBox="0 0 20 20" fill="currentColor">
                                    <path stroke="#374151" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8V4m0 0h4M3 4l4 4m8 0V4m0 0h-4m4 0l-4 4m-8 4v4m0 0h4m-4 0l4-4m8 4l-4-4m4 4v-4m0 4h-4" />
                                </svg>
                                
                            </div>
                            
                            <!-- <textarea class="text-grey-darkest flex-1 p-2 m-1 bg-transparent hover:outline-none focus:outline-none" bind:value={commandText} rows="10" /> -->
                            <Editor 
                                bind:this={editor} 
                                value={commandText} 
                                height={commandHeight} 
                                theme={'vs'}
                                language={'sql'} 
                                lineNumbers={'on'}
                                />
                        </div>
                    </div>
                {/if}
                <div class="font-light flex flex-row items-center mt-2">
                    <div class="flex-auto"></div>
                    <button class="bg-blue-500 text-white p-1.5 m-1 w-14 rounded-sm focus:outline-none" on:click|preventDefault={(e)=>{ setBeingEdited=true; addDataSet(e);}}>Update</button>
                    <button class="bg-gray-300 p-1.5 m-1 w-14 rounded-sm focus:outline-none" on:click|preventDefault={(e)=> setBeingEdited=false }>Cancel</button>
                </div>
            {/if}
        {/if}
    </div>
{/if}