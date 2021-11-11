<script>
    
    import { createEventDispatcher } from "svelte";
    import Color from "./Color.svelte";
    export let title=null;
    export let width="1pt";
    export let style=null;
    export let color=null;
    const dispatch = createEventDispatcher();

    const widths=[..._widths()];
    function _widths(){
        let arr=[];
        for (let index = 0.25; index <=5;index+=0.25) {
            arr.push(`${index}pt`);
        }
        return arr;
    }

    function onBlur(e) {
        e.stopPropagation();
        dispatch("onChange", {style:style, width:width, color:color});
    }
    const borderStyle=["None","Default","Dashed","Dotted","Solid"];
    $:{
        if(style==='None' || style=="Default"){
            color=null;
            width=null;
        }
    }
</script>
<div class="flex flex-row p-1">
    <div class="flex-1">{title}</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={style} style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none" on:change={onBlur} >
        {#each borderStyle as opt}
            <option value={opt}>
                {opt}
            </option>
        {/each}
    </select>
</div>
{#if !(style==='None' || style=="Default")}
<div class="flex flex-row p-1">
    <div class="flex-1">Line Width</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={width} on:change|preventDefault={onBlur} 
            style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
            {#each widths as opt}
                <option value={opt}>
                    {opt}
                </option>
            {/each}
        </select> 
</div>
<Color title="Border Color" bind:color={color} on:onChange={(e)=>{ color=e.detail.color; onBlur(e);}} />
{/if}    