<script>
    import { createEventDispatcher } from "svelte";
    import Position from "./properties/Position.svelte";
    import * as report from "../model/report/report.js"; 
    import { Accordion, AccordionItem } from "../components/accordion"; 
    import Margin from "./properties/Margin.svelte";
    import Color from "./properties/Color.svelte";
    import Border from "./properties/Border.svelte";
    import Align from "./properties/Align.svelte";
    import TextStyle from "./properties/TextStyle.svelte";
    import BorderProperty from "./properties/BorderProperty.svelte";

    export let title;
    export let objectType="";
    export let obj=null;
    export let reportData=null;


    let unit="in";
    let pageStartUnitValue=0;
    let pageDeltaUnitValue=0.05;
    let pageStopUnitValue=2;
    let hasDS=false;
    let expanded;
    const dispatch = createEventDispatcher();	
    let dummyBorder=null;
    
    function prepare(){
        if(objectType=="textbox" ){
            let data={
                style: "None",
                color: null,
                width: null,
                option: ""
            };
            let a="";
            if(obj.style.leftBorder && obj.style.leftBorder.style && ["Default","None"].indexOf(obj.style.leftBorder.style)==-1 ) {
                a=`${a}l`;
                data.option=a;
                data.color=obj.style.leftBorder.color;
                data.width=obj.style.leftBorder.width;
            }
            
            if(obj.style.rightBorder && obj.style.rightBorder.style && ["Default","None"].indexOf(obj.style.rightBorder.style)==-1 ) {
                a=`${a}r`;
                data.option=a;
                data.color=obj.style.rightBorder.color;
                data.width=obj.style.rightBorder.width;
            }
            if(obj.style.topBorder && obj.style.topBorder.style && ["Default","None"].indexOf(obj.style.topBorder.style)==-1) {
                a=`${a}t`;
                data.option=a;
                data.color=obj.style.topBorder.color;
                data.width=obj.style.topBorder.width;
            }
            if(obj.style.bottomBorder && obj.style.bottomBorder.style && ["Default","None"].indexOf(obj.style.bottomBorder.style)==-1) {
                a=`${a}b`;
                data.option=a;
                data.color=obj.style.bottomBorder.color;
                data.width=obj.style.bottomBorder.width;
            }
            if(obj.style.border && ["Default","None"].indexOf(obj.style.border.style)==-1){
                data.option="a";
                data.color = obj.style.border.color;
                data.width = obj.style.border.width;
                data.style=obj.style.border.style;
            }
            
            return data;    
        }
        else {
            return null;
        }
    }
    
    let pseudoObject={
        dataSetName:null,
        height:null,
        width:null,
        style: {
            border: {
                style: "Solid",
                color: "#000000",
                width: "1pt"
            },
            fontSize:null,
            fontFamily: null,
            fontWeight:null,
            color: null,
            backgroundColor: null,
            textAlign: "Default",
            verticalAlign: "Default",
            textDecoration:null,
        }
    };
    $: {
        
        hasDS = reportData && reportData.dataSets && reportData.dataSets.dataSet.length>0;
        if(objectType=="textbox" && obj) dummyBorder=prepare();
    }
    
    function textbox_property_change(e, prop, type, obj){
        e.stopPropagation();
        e.preventDefault();
        if(type=="textbox" && prop=="textAlign"){
            obj.paragraphs.paragraph.style.textAlign=e.detail.data;
        }
        if(type=="textbox" && prop=="verticalAlign"){
            obj.style.verticalAlign=e.detail.data;
        }
        if(type=="textbox" && prop=="style"){
            obj.paragraphs.paragraph.textRuns.textRun.style.fontFamily=e.detail.fontFamily;
            obj.paragraphs.paragraph.textRuns.textRun.style.fontSize=e.detail.fontSize;
            obj.paragraphs.paragraph.textRuns.textRun.style.fontWeight=e.detail.fontWeight;
            obj.paragraphs.paragraph.textRuns.textRun.style.fontStyle=e.detail.fontStyle;
            obj.paragraphs.paragraph.textRuns.textRun.style.textDecoration=e.detail.textDecoration;
            obj.paragraphs.paragraph.textRuns.textRun.style.format = e.detail.format;
            obj.paragraphs.paragraph.textRuns.textRun.style.color = e.detail.color;
            obj.style.backgroundColor = e.detail.backgroundColor;
        }
        if(type=="textbox" && prop=="padding"){
            obj.style.paddingLeft=e.detail.left;
            obj.style.paddingTop=e.detail.top;
            obj.style.paddingRight=e.detail.right;
            obj.style.paddingBottom=e.detail.bottom;
        }
        let data={};
        data["type"]=type;
        data["object"]=obj;
        dispatch("dataChange", data);
    }

    function section_margin_change(e, prop, type, obj){
        e.stopPropagation();
        e.preventDefault();

        obj.leftMargin=e.detail.left;
        obj.topMargin=e.detail.top;
        obj.rightMargin=e.detail.right;
        obj.bottomMargin=e.detail.bottom;

        let data={};
        data["type"]=type;
        data["object"]=obj;
        dispatch("dataChange", data);
    }

    function line_property_change(e){
        obj.style.border.style=e.detail.style;
        obj.style.border.width=e.detail.width;
        obj.style.border.color=e.detail.color;
        let data={};
        data["type"]="line";
        data["object"]=obj;
        dispatch("dataChange", data);
    }
    function image_property_change(e){
        obj.style.border.style=e.detail.style;
        obj.style.border.width=e.detail.width;
        obj.style.border.color=e.detail.color;
        let data={};
        data["type"]="line";
        data["object"]=obj;
        dispatch("dataChange", data);
    }
    function onBlurImage(e){
        dispatch("dataChange", data);
    }

    function addDS(){
        dispatch("add_data_source_init");
    }
    function border_property_change(e, that){
        
        [
            "border", 
            "leftBorder", 
            "topBorder", 
            "rightBorder", 
            "bottomBorder"
        ].forEach(el => delete obj.style[el]);

        [
            "border", 
            "leftBorder", 
            "topBorder", 
            "rightBorder", 
            "bottomBorder"
        ].forEach(el => obj.style[el]=e.detail[el] );
        dummyBorder=prepare();
        dispatch("dataChange", obj);
    }
</script>
<div class="bg-white font-medium p-1 px-2 border-b shadow-sm">{title} ({objectType})</div>
{#if !hasDS}
<div class="bg-red-100 font-light p-2 flex flex-row items-center">
    <svg class="w-8 h-8 text-yellow-500" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
        <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd" />
    </svg>
    <div class="font-light p-2"> No data source has been added!</div>
    <button class="bg-blue-500 text-white p-1.5 m-1 rounded-sm focus:outline-none" on:click={addDS} >Add data source</button>
</div>
{/if}

<div class="flex-auto flex flex-col  overflow-y-auto">
    <div class="flex flex-row p-2 items-center">
        <label class="font-semibold text-sm" for="name">Name</label> 
        <input type="text" bind:value={obj.name} class="p-1 mx-1 w-full border rounded-sm hover:outline-none focus:outline-none bg-transparent" name="name" id="name" />
    </div>  

    <Accordion>
        {#if objectType==='header'||objectType==='page' || objectType==='body'|| objectType==='footer'}
        <AccordionItem  title="Margin" id="item-1">
            <Margin 
                left={obj.leftMargin} 
                top={obj.topMargin} 
                right={obj.rightMargin} 
                bottom={obj.bottomMargin}
                unit={unit}
                delta={pageDeltaUnitValue}
                start={pageStartUnitValue}
                stop={pageStopUnitValue}
                on:onChange={(e)=>section_margin_change(e, "margin", objectType, obj)} 
            />
            
        </AccordionItem>
        {/if}
        {#if objectType==='tablix'}
           <!-- <AccordionItem title="Style" id="item-2">
                <Border title={'Border Style'} 
                    style={obj.style.border.style} 
                    width={obj.style.border.width}
                    color={obj.style.border.color}
                    on:onChange={line_property_change}/>
            </AccordionItem> -->    
        {/if}
        {#if objectType==='tablixrow'}
            <!-- <AccordionItem title="Style" id="item-2">
                <Border title={'Border Style'} 
                    style={obj.style.border.style} 
                    width={obj.style.border.width}
                    color={obj.style.border.color}
                    on:onChange={line_property_change}/>
            </AccordionItem> -->
        {/if}
        {#if objectType==='textbox'}
            <AccordionItem  title="Padding" id="item-11">
                <Margin bind:left={obj.style.paddingLeft} bind:top={obj.style.paddingTop} bind:right={obj.style.paddingRight} bind:bottom={obj.style.paddingBottom}
                    on:onChange={(e)=>textbox_property_change(e, "padding", "textbox", obj)}
                />
            </AccordionItem>
            {#if dummyBorder}
            <AccordionItem  title="Border" id="item-14">
                <BorderProperty 
                    bind:option={dummyBorder.option} 
                    bind:color={dummyBorder.color} 
                    bind:style={dummyBorder.style} on:onChange={(e)=>border_property_change(e, obj)} /> 
            </AccordionItem>
            {/if}
            <AccordionItem title="Style" id="item-3">
                
                <TextStyle 
                        bind:fontFamily={obj.paragraphs.paragraph.textRuns.textRun.style.fontFamily} 
                        bind:fontSize={obj.paragraphs.paragraph.textRuns.textRun.style.fontSize} 
                        bind:fontWeight={obj.paragraphs.paragraph.textRuns.textRun.style.fontWeight}
                        bind:fontStyle={obj.paragraphs.paragraph.textRuns.textRun.style.fontStyle}
                        bind:textDecoration={obj.paragraphs.paragraph.textRuns.textRun.style.textDecoration}
                        bind:format={obj.paragraphs.paragraph.textRuns.textRun.style.format} 
                        bind:color={obj.paragraphs.paragraph.textRuns.textRun.style.color} 
                        bind:backgroundColor={obj.style.backgroundColor} 
                        on:styleChange={(e)=>textbox_property_change(e, "style", "textbox", obj)}/>
                <Align title={'Text Align'} bind:value={obj.paragraphs.paragraph.style.textAlign} on:alignmentChange={(e)=>textbox_property_change(e, "textAlign", "textbox", obj)} />
                <Align title={'Vertical Align'} vertical={true} bind:value={obj.style.verticalAlign} on:alignmentChange={(e)=>textbox_property_change(e, "verticalAlign", "textbox", obj)} />
            </AccordionItem>
            
        {/if}
        {#if objectType==='line'}
            

            <AccordionItem title="Style" id="item-12">
                <Border title={'Border Style'} 
                    style={obj.style.border.style} 
                    width={obj.style.border.width}
                    color={obj.style.border.color}
                    on:onChange={line_property_change}
                    />
            </AccordionItem>
            
        {/if}
        {#if objectType==='image'}
            <AccordionItem title="Embedded" id="item-13">
                <div class="flex flex-row p-1">
                    <div class="flex-1 p-1">Image</div>
                    <!-- svelte-ignore a11y-no-onchange -->
                    <select bind:value={obj.value} on:change|preventDefault={onBlurImage}  
                        class="flex-1 border hover:outline-none focus:outline-none">
                        {#each reportData.embeddedImages.embeddedImage as img,index}
                            <option value={img.name}>
                                {img.name}
                            </option>
                        {/each}
                    </select>
                </div>
            </AccordionItem>

            <AccordionItem title="Style" id="item-13">
                <Border title={'Border Style'} 
                    style={obj.style.border.style} 
                    width={obj.style.border.width}
                    color={obj.style.border.color}
                    on:onChange={image_property_change}
                    />
            </AccordionItem>
            
        {/if}
    </Accordion>
    
</div>

