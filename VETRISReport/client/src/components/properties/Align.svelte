<script>
    import { onMount, createEventDispatcher } from "svelte";
    export let value=null;
    export let title;
    export let vertical=false;
    const haligns=["Default","General","Left","Center","Right"];
    const valigns=["Default","Top","Middle","Bottom"];

    const dispatch = createEventDispatcher();
    
    function onChange(e) {
        e.stopPropagation();
        dispatch("alignmentChange", {data:value});
    }
</script>


<div class="flex flex-row p-1">
    <div class="flex-1 p-1">{title}</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={value} on:change|preventDefault={onChange} 
            style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
        {#if !vertical}
            {#each haligns as opt}
                <option value={opt}>
                    {opt}
                </option>
            {/each}
        {:else}
            {#each valigns as opt}
                <option value={opt}>
                    {opt}
                </option>
            {/each}
        {/if}
    </select>
   
</div>