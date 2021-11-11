<script>
   import { element } from "svelte/internal";
import Accordion from "../../components/accordion/index";
import Color from "./Color.svelte";
import Position from "./Position.svelte";
import Size from "./Size.svelte";
/*

    textElement,
    tableElement,
    boxElement,
    lineElement,
    imageElement
    reportElement
    {
        "name":"",
        "text": {
            "text": "",
            "expression": ()=> { return ""; }
        }
        "color": "#fff",
        "border": "1px solid|dotted|none #ccc|rgba(255,255,255,1)",
        "background":{
            "color":"#ccc|rgba(255,255,255,1)",
            "image": "url(...)",
            "pattern": ""
        } 
        "padding": "0 0 0 0",
        "margin" : "0 0 0 0",
        "alignment:{
            "horizontal": "left|right|center",
            "vertical": "top|middle|bottom"
        },
        "position":{
            "top": "0px",
            "left": "0px",
            "bottom": "0px",
            "right": "0px"
        }
        "width": "0px|100%",
        "height": "0px|100%",
        "font": {
            "family":"",
            "face":"",
            "style": "normal|semibold|bold|italic|bolditalic|strikethrough",
            "size": "12px"
        }
    }


*/
export let elementData = {
    name: "",
    color: "black",
    background: {
        color:"#ccc",
        image: undefined,
        pattern: undefined
    } 
};

function isGroup(el){
    if(isNullOrUndefined(el)) return false;
    return typeof el === "object" && Object.keys(el).length>0;
}
function isArray(el){
    return Array.isArray(el);
}
function isProperty(el){
    return !isArray(el) && !isGroup(el);
}

function isDate(el){
    return Object.prototype.toString.call(el)==="[object Date]";
}
function isNumber(el){
    return Object.prototype.toString.call(el)==="[object Number]";
}
function isNullOrUndefined(el){
    if(el===undefined||el===null) return true;
}

</script>

{#each Object.keys(elementData) as property }

        {#if property==='color'}
            <Color bind:color={elementData.color} />
        {:else if property==='size'}
            <Size bind:width={elementData.size.width} bind:height={elementData.size.height}/>
        {:else if property==='position'}
            <Position 
                bind:left={elementData.position.left} 
                bind:top={elementData.position.top}
                bind:right={elementData.position.rigth}
                bind:bottom={elementData.position.bottom}
                />
        {:else}
            <div class="flex flex-row items-center p-1">
                <div class="flex-1">{property}</div>
                <div class="flex-1"><input type="text" class="w-full" bind:value={elementData[property]}></div>
            </div>
        {/if}
{/each}


