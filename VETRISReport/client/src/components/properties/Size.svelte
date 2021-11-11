<script>
    export let width=null;
    export let height=null;
</script>

<div class="grid grid-cols-2 gap-1">
   
    <div class="flex flex-col p-1">
        <div class="flex-1">Width</div>
        <div class="flex-1"><input type="text" class="p-1 border rounded-sm hover:outline-none focus:outline-none bg-transparent" bind:value={width}></div>
    </div>
    <div class="flex flex-col p-1">
        <div class="flex-1">Height</div>
        <div class="flex-1"><input type="text" class="p-1 border rounded-sm hover:outline-none focus:outline-none bg-transparent" bind:value={height}></div>
    </div>
    
</div>