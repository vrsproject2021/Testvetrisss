<script context="module">
	// retain module scoped expansion state for each tree node
	const _expansionState = {
		/* treeNodeId: expanded <boolean> */
	}
</script>
<script>
    import { createEventDispatcher } from "svelte";
	export let tree;
	export let defaultexpaned=false;
	const {label, children, data} = tree;

	let expanded = _expansionState[label] || defaultexpaned;
	const toggleExpansion = () => {
		expanded = _expansionState[label] = !expanded;
	}

	const dispatch = createEventDispatcher();


	$: arrowDown = expanded;

	function onselected(e){
		let value='';
		if(typeof e === "string")
			value=e;
		else if(e.detail) 
			value=e.detail;
		dispatch("onFunctionSelected", value);
	}
</script>

<ul class="list-none pl-1.5 select-none text-sm"><!-- transition:slide -->
	<li class="pl-1.5 text-sm select-auto">
		{#if children}
			<span class="flex flex-row" on:click={toggleExpansion}>
				<span class="mr-1">
					<svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 {expanded?'hidden':''}" fill="none" viewBox="0 0 24 24" stroke="currentColor">
						<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
					</svg>
					<svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 {!expanded?'hidden':''} " viewBox="0 0 20 20" fill="currentColor">
						<path fill-rule="evenodd" d="M5 10a1 1 0 011-1h8a1 1 0 110 2H6a1 1 0 01-1-1z" clip-rule="evenodd" />
					</svg>
				</span>
				{label}
			</span>
			{#if expanded}
				{#each children as child}
					<svelte:self tree={child} on:onFunctionSelected={onselected} />
				{/each}
			{/if}
		{:else}
			<span on:dblclick={(e)=>onselected(data)}>
				{label}
			</span>
		{/if}
	</li>
</ul>

<style>
	ul {
		margin: 0;
		list-style: none;
		padding-left: 1.2rem; 
		user-select: none;
	}
	.no-arrow { padding-left: 1.0rem; }
	.arrow {
		cursor: pointer;
		display: inline-block;
		/* transition: transform 200ms; */
	}
	.arrowDown { transform: rotate(90deg); }

</style>