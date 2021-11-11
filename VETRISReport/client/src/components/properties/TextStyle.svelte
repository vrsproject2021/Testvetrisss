
<script>
    import * as report from "../../model/report/report.js";
    import Color from "./Color.svelte";
   
    import { createEventDispatcher } from "svelte";
    export let fontSize;
    export let fontWeight;
    export let fontStyle;
    export let textDecoration;
    export let format;
    export let fontFamily;
    export let color;
    export let backgroundColor;

    const dispatch = createEventDispatcher();

    function onBlur(e) {
        e.stopPropagation();
        dispatch("styleChange", {fontFamily: fontFamily, fontSize:fontSize, fontWeight:fontWeight, fontStyle:fontStyle,textDecoration:textDecoration, format:format, color:color, backgroundColor:backgroundColor});
    }
    const weights = [{"name":"Default", value:null},{"name":"Bold", value:"Bold"}]
    const styles = [{"name":"Default", value:null},{ "name":"Italic", value:"Italic"}]
    const decorations = [
            {"name":"Default", value:null},
            { "name":"None", value:"None"},
            { "name":"Underline", value:"Underline"},
            { "name":"Overline", value:"Strikethrough"},
            { "name":"Strikethrough", value:"Strikethrough"},
        ]
    const formats=[
        {name:"Default", value: null},
        {name:"1,234", value:"n"},
        {name:"1,234.0", value:"n1"},
        {name:"1,234.00", value:"n2"},
        {name:"1,234.000", value:"n3"},
        {name:"($1,234)", value:"'$'#,0;('$'#,0)"},
        {name:"($1,234.00)", value:"'$'#,0.00;('$'#,0.00)"},
        {name:"$1,234", value:"'$'#,0,00"},
        {name:"$1,234.00", value:"'$'#,0.00"},

        {name:"31/10/21", value:"dd'/'MM'/'yy"},
        {name:"31/10/2021", value:"dd'/'MM'/'yyyy"},
        {name:"31/10/21 10:00", value:"dd'/'MM'/'yy HH:mm"},
        {name:"31/10/2021 10:00", value:"dd'/'MM'/'yyyy HH:mm"},
        {name:"31-Jan-2021", value:"dd-MMM-yyyy"},
        {name:"31 January 2021", value:"dd MMMM yyyy"},
        {name:"January 31, 2021", value:"MMMM dd, yyyy"},
        {name:"10/31/21", value:"MM'/'dd'/'yy"},
        {name:"10/31/2021", value:"MM'/'dd'/'yyyy"},
        {name:"2021-01-31", value:"yyyy'-'MM'-'dd"},
        {name:"10/31/21 10:00", value:"MM'/'dd'/'yy HH:mm"},
        {name:"10/31/2021 10:00", value:"MM'/'dd'/'yyyy HH:mm"},
        {name:"10:00", value:"HH:mm"},
        {name:"10:00:00", value:"HH:mm:ss"},

        {name:"35.0%", value:"0.0%"},
        {name:"35.00%", value:"0.00%"},
        {name:"35.000%", value:"0.000%"},
        
    ];
    const fonts = [null,...report.fonts()];
    const fontSizes=[null,..._fontSizes()];
    function _fontSizes(){
        let arr=[];
        for (let index = 8; index <=72;) {
            arr.push(`${index}pt`);
            if(index<12) index++;
            else if(index<28) index+=2;
            else if(index<36) index+=8;
            else if(index<48) index+=16;
            else index+=24;
        }
        return ['',...arr];
    }
 
</script>
<div class="flex flex-row p-1">
    <div class="flex-1 p-1">Format</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={format} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
        {#each formats as opt}
            <option value={opt.value}>
                {opt.name}
            </option>
        {/each}
    </select>   
</div>

<div class="flex flex-row p-1">
    <div class="flex-1 p-1">Font Family</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={fontFamily} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
        {#each fonts as opt}
            <option value={opt}>
                {opt}
            </option>
        {/each}
    </select> 
</div>

<div class="flex flex-row p-1">
    <div class="flex-1 p-1">Font Size</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={fontSize} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
        {#each fontSizes as opt}
            <option value={opt}>
                {opt}
            </option>
        {/each}
    </select> 
</div>

<div class="flex flex-row p-1">
    <div class="flex-1 p-1">Font Weight</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={fontWeight} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
        {#each weights as opt}
            <option value={opt.value}>
                {opt.name}
            </option>
        {/each}
    </select>   
</div>
<div class="flex flex-row p-1">
    <div class="flex-1 p-1">Style</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={fontStyle} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
        {#each styles as opt}
            <option value={opt.value}>
                {opt.name}
            </option>
        {/each}
    </select>   
</div>
<div class="flex flex-row p-1">
    <div class="flex-1 p-1">Text Decoration</div>
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={textDecoration} on:change|preventDefault={onBlur} 
                style="width: 100%;" class="flex-1 border hover:outline-none focus:outline-none">
        {#each decorations as opt}
            <option value={opt.value}>
                {opt.name}
            </option>
        {/each}
    </select>   
</div>
<Color title="Color" color={color} on:onChange={(e)=>{ color=e.detail.color; onBlur(e);}} />
<Color title="Background Color" color={backgroundColor} on:onChange={(e)=>{ backgroundColor=e.detail.color; onBlur(e);}} />
