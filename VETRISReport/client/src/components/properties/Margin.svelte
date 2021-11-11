<script>
    import { createEventDispatcher } from "svelte";
    export let left=null;
    export let top=null;
    export let right=null;
    export let bottom=null;
    export let unit="pt";
    export let start=1;
    export let stop=30;
    export let delta=1;
    const paddings=[..._paddings()];
    function _paddings(){
        let arr=[];
        for (let index = start; index <=stop;index+=delta) {
            if(parseInt(index+"")!=parseFloat(index+"")){
                if(unit=="in") {
                    arr.push(`${index.toFixed(2)}${unit}`);
                }
                else if(unit=="cm") {
                    arr.push(`${index.toFixed(3)}${unit}`);
                }
            }
            else
                arr.push(`${index}${unit}`);
        }
        return arr;
    }
    const dispatch = createEventDispatcher();

    function onBlur(e) {
        e.stopPropagation();
        dispatch("onChange", {left: left, top:top, right:right, bottom:bottom});
    }
</script>


<div class="grid grid-cols-2 gap-1">
   
    <div class="flex flex-col p-1">
        <div class="flex-1">Left</div>
        <!-- svelte-ignore a11y-no-onchange -->
        <select bind:value={left} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
                {#each paddings as opt}
                    <option value={opt}>
                        {opt}
                    </option>
                {/each}
            </select> 
    </div>
    <div class="flex flex-col p-1">
        <div class="flex-1">Top</div>
        <!-- svelte-ignore a11y-no-onchange -->
        <select bind:value={top} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
                {#each paddings as opt}
                    <option value={opt}>
                        {opt}
                    </option>
                {/each}
            </select> 
    </div>
    <div class="flex flex-col p-1">
        <div class="flex-1">Right</div>
        <!-- svelte-ignore a11y-no-onchange -->
        <select bind:value={right} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
                {#each paddings as opt}
                    <option value={opt}>
                        {opt}
                    </option>
                {/each}
            </select> 
    </div>
    <div class="flex flex-col p-1">
        <div class="flex-1">Bottom</div>
        <!-- svelte-ignore a11y-no-onchange -->
        <select bind:value={bottom} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
                {#each paddings as opt}
                    <option value={opt}>
                        {opt}
                    </option>
                {/each}
            </select> 
    </div>
    
</div>