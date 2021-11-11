<script>
    export let left=null;
    export let top=null;
    export let width=null;
    export let height=null;
</script>

<div class="w-full font-medium">Position</div>
<div class="grid-cols-2 gap-1">
   
    <div class="flex flex-col items-center p-1">
        <div class="flex-1">Left</div>
        <div class="flex-1"><input type="text" class="w-full" bind:value={left}></div>
    </div>
    <div class="flex flex-col items-center p-1">
        <div class="flex-1">Top</div>
        <div class="flex-1"><input type="text" class="w-full" bind:value={top}></div>
    </div>
    <div class="flex flex-col items-center p-1">
        <div class="flex-1">Width</div>
        <div class="flex-1"><input type="text" class="w-full" bind:value={width}></div>
    </div>
    <div class="flex flex-col items-center p-1">
        <div class="flex-1">Height</div>
        <div class="flex-1"><input type="text" class="w-full" bind:value={height}></div>
    </div>
    
</div>