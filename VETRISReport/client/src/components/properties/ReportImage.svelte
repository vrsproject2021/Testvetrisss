<script>

    import { createEventDispatcher } from "svelte";
    import * as report from "../../model/report/report.js"; 
    import Dialog from "../../components/Dialog.svelte";
    export let reportData=null;

    const dispatch = createEventDispatcher();
    let fields = [];
    let editDialog = false;
    let fileInput;
    
    function addImage(){

    }

    function deleteImage(index) {
        reportData.embeddedImages.embeddedImage.splice(index,1);
        reportData.embeddedImages.embeddedImage=[...reportData.embeddedImages.embeddedImage];
        dispatch("imagechanged");
    }
   
    function processFile(e){
        const image = e.target.files[0];
        const fileName=(image.name).replace(/[\.\-&$!#~+]/g,'_');
        const embedded = reportData.embeddedImages.embeddedImage.find(i=>i.name===fileName);
        fileInput.remove();
        fileInput=null;
        //message
        if(embedded) return; 
        let reader = new FileReader();
        reader.readAsDataURL(image);
        reader.onload = e => {
            
            const m = e.target.result.match(/data:(image\/\w+);base64,(.*)/);
            reportData.embeddedImages.embeddedImage=[...reportData.embeddedImages.embeddedImage, {
                name: fileName,
                mimeType: m[1],
                imageData: m[2]
            }];
            dispatch("imagechanged");
        };
    }
    function addFile(e){
        fileInput = document.createElement("input");
        fileInput.setAttribute("type", "file");
        fileInput.setAttribute("accept", "image/*");
        fileInput.className="hidden";
        fileInput.onchange=(e) => processFile(e);
        fileInput.click();
    }
</script>

<div class="bg-white font-medium p-1 px-2 border-b shadow-sm flex flex-row">
    <span class="flex-auto">Images</span>
    <button class="bg-blue-500 text-white p-1.5 m-1 rounded-sm focus:outline-none w-10" on:click|preventDefault={addFile}>Add</button>
    <!-- <div class="hidden">
        <input bind:this={fileInput} type="file" name="myImage" id="file" accept="image/*" on:change={processFile}>
    </div> -->
</div>

<div class="flex flex-col m-2">
    <div class="flex flex-row items-center justify-between space-x-1 border-b p-2 font-semibold text-gray-400">
        <div class="flex flex-1 hover:text-gray-900">Embedded Images</div>
        <div class="flex w-16 hover:text-gray-900"></div>
    </div>
    <div class="overflow-y-auto border-b border-l border-r text-xs" style="max-height:90%;"> 
        {#each reportData.embeddedImages.embeddedImage as img,index}
        <div class="flex flex-row items-center justify-between space-x-1 p-1 text-gray-900 hover:bg-gray-200">
            <div class="flex flex-auto flex-col p-1">
                <div>{img.name}</div>
                <img src={`data:${img.mimeType};base64,${img.imageData}`} style="max-width: 150px; max-height: 150px;" alt={img.name}/>
            </div>
            <div class="w-6 text-gray-700" on:click={(e)=>deleteImage(index)}>
                <svg xmlns="http://www.w3.org/2000/svg" class="ml-1 h-5 w-5 text-red-600" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                  </svg>
            </div>
        </div>
        {/each}
    </div>
</div>
