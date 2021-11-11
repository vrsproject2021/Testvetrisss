
<script>
    import { createEventDispatcher } from "svelte";
    import Dialog from "../../components/Dialog.svelte";
    export let title;
    export let color;
    const colors = ['gray', 'red', 'yellow', 'green', 'blue', 'indigo', 'purple', 'pink'];
    const variants = [100, 200, 300, 400, 500, 600, 700, 800, 900];
    let isOpen = false;
    const dispatch = createEventDispatcher();

    
    function selectColor(e, selectedColor, selectedVariant){
        e.stopPropagation();
        const style=getComputedStyle(e.target).backgroundColor;
        const m = style.match(/rgb\((?<r>\d+),\s*(?<g>\d+),\s*(?<b>\d+)\)/);
        if(m){
            color='#' + 
                [
                    parseInt(m.groups.r), 
                    parseInt(m.groups.g), 
                    parseInt(m.groups.b)
                ]
                .map(x => x.toString(16).padStart(2, '0'))
                .join('');
                dispatch("onChange", {color:color});
        } else if(style.match(/#[0-0a-fA-F]{6}/)){
            color=style;
            dispatch("onChange", {color:color});
        }    
        isOpen=false;
    }
</script>

<div class="flex flex-row" >
    <div class="flex-1 p-1">{title}</div>
    <div class="relative inline-block text-left p-1">
        <div>
          <button 
             on:click|preventDefault={(e)=> isOpen=!isOpen }
             class="inline-flex justify-center w-full bg-white text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-0" id="menu-button" aria-expanded="true" aria-haspopup="true">
            <div class="w-5 h-5 rounded-full border border-gray-500" style="background-color:{color||'transparent'}"></div>
            <!-- Heroicon name: solid/chevron-down -->
            <svg class="pr-1 ml-1 h-6 w-6" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
              <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
            </svg>
          </button>
        </div>
      </div>
</div>
<Dialog bind:visible={isOpen} title={title} inverted={true} >
    <div class="flex">
        <div class="">
            <div on:click|preventDefault={(e)=>selectColor(e)} class="cursor-pointer w-6 h-6 border rounded-full border-black bg-white mx-1 my-1"  ></div>
            <div on:click|preventDefault={(e)=>selectColor(e)} class="cursor-pointer w-6 h-6 rounded-full border-white bg-black mx-1 my-1" ></div>
        </div>
        {#each colors as c}
            <div class="">
                {#each variants as v}
                    <div on:click|preventDefault={(e)=>selectColor(e, c,v)} class="cursor-pointer w-6 h-6 rounded-full mx-1 my-1 {`bg-${c}-${v}`}" ></div>
                {/each}
            </div>
        {/each}
    </div>  
</Dialog>
