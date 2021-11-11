<script>
	export let id = "item" + (Math.random()*+(new Date())).toString();
	export let title = "Title";
	export let expanded = false;
	export let disabled = false;
	export let ref = null;
	import { getContext, onDestroy } from "svelte";
	const ctx = getContext("Accordion");
	let unsubscribe = undefined;
	onDestroy(() => {
	  if (ctx) ctx.remove({ id });
	  if (unsubscribe) unsubscribe();
	});
	$: button_id = `button-${id}`;
	$: if (ctx) {
	  ctx.add({ id, expanded });
	  unsubscribe = ctx.items.subscribe((value) => {
		expanded = value[id];
	  });
	}
  </script>
  
  <li class="list-none border-b cursor-pointer py-0.5 px-1" {...$$restProps}>
	<div class="flex flex-row justify-between items-center p-1 bg-gray-100 border-t border-b" 
		bind:this={ref}
		aria-expanded={expanded}
		aria-controls={id}
		aria-disabled={disabled}
		{disabled}
		id={button_id}
		on:click={() => {
			if (ctx) {
				ctx.toggle({ id, expanded: !expanded });
				if (expanded && ref.getBoundingClientRect().top < 0) ref.scrollIntoView();
			}
			}}>
		<span class="px-2"><slot name="title" >{title}</slot></span>
		<svg class="w-4 h-4 {expanded?'hidden':''}" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
			<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
		</svg>
		<svg class="w-4 h-4 {!expanded?'hidden':''}" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
			<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 12H6" />
		</svg>
	</div>
	<div role="region" class="w-full p-0.5" {id} aria-labelledby={button_id} hidden={!expanded}>
	  <slot />
	</div>
  </li>