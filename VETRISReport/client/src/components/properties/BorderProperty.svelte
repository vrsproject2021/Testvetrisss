<script>
    import { createEventDispatcher } from "svelte";
    import Border from "./Border.svelte";
    import Icon from 'mdi-svelte';
    import { 
        mdiBorderAllVariant, 
        mdiBorderTopVariant, 
        mdiBorderBottomVariant, 
        mdiBorderLeftVariant, 
        mdiBorderRightVariant, 
        mdiBorderNoneVariant 
    } from '@mdi/js';
import { withNullAsUndefined } from "monaco-editor/esm/vs/base/common/types";

    export let color;
    export let style;
    export let width="1pt";
    export let option;
    const dispatch = createEventDispatcher();	
    /*
            t
           ---
        l |   | r
           ---
            b
    */
    $:left=option.indexOf('l')!==-1;  
    $:right=option.indexOf('r')!==-1;  
    $:top=option.indexOf('t')!==-1; 
    $:bottom=option.indexOf('b')!==-1;  
    $:all=option==="a";
    $:none = (option??"")==="";
    $:{
        if(style==='None' || style=="Default") width=null;
        if(!(style==='None' || style=="Default") && (width??"")=="") width="1pt";
        if(option!="" && (style==='None' || style=="Default")){
            style="Solid";
            color="#000000";
            width="1pt";
        }
    }

    function onBorder(e, type){
        let opt=`${option}`;
        if(type==="" || type==="a") opt=type;
        else{
            if(opt.indexOf(type)>-1) opt=opt.replace(type,"");
            else opt=`${opt??""}${type}`;
            if(opt.indexOf('l')>-1 && opt.indexOf('t')!==-1 && opt.indexOf('r')>-1 && opt.indexOf('b')!==-1)
                opt="a";
            else
                opt=opt;
        }
       
        if(opt!=""){
            option=opt;
            if(style==='None' || style=="Default"){
                style="Solid";
                color="#000000";
                width="1pt";
            }
        } else {
            option=opt;
            style="None";
            color=null;
            width=null;
        }
        dispatch("onChange", prepare());
    }

    function property_change(e){
        dispatch("onChange", prepare());
    }

    function prepare(){
        let data={};
        if((option??"")==""){
            return {
                border:{
                    style: style,
                    color: null,
                    width: null
                }
            }
        }
        if(option=="a"){
            return {
                border:{
                    style: style,
                    color: color,
                    width: width
                }
            }
        }
        data = {
                border: {
                    style: "None",
                    color: null,
                    width: null
                }
        };
        if(option.indexOf('l')!==-1) {
            data = Object.assign(data,{
                        leftBorder: {
                            style: style,
                            color: color,
                            width: width
                        }
                    });
        }
        if(option.indexOf('t')!==-1) {
            data = Object.assign(data,{
                        topBorder: {
                            style: style,
                            color: color,
                            width: width
                        }
                    });
        }
        if(option.indexOf('r')!==-1) {
            data = Object.assign(data,{
                        rightBorder: {
                            style: style,
                            color: color,
                            width: width
                        }
                    });
        }
        if(option.indexOf('b')!==-1) {
            data = Object.assign(data,{
                        bottomBorder: {
                            style: style,
                            color: color,
                            width: width
                        }
                    });
        }

        return data;
    }
</script>

<div class="flex flex-row p-1">
    <div class="flex-1">Presets</div>
    <div class="flex flex-1 flex-row space-x-1 items-center">
       <div class="hover:bg-gray-200 hover:text-gray-800 {all?'bg-gray-500 text-white':''}" on:click={(e)=>onBorder(e, "a")}><Icon path={mdiBorderAllVariant}/></div>  
       <div class="hover:bg-gray-200 hover:text-gray-800 {none?'bg-gray-500 text-white':''}" on:click={(e)=>onBorder(e, "")}><Icon path={mdiBorderNoneVariant}/></div>  
       <div class="hover:bg-gray-200 hover:text-gray-800 {left?'bg-gray-500 text-white':''}" on:click={(e)=>onBorder(e, "l")}><Icon path={mdiBorderLeftVariant}/></div>  
       <div class="hover:bg-gray-200 hover:text-gray-800 {top?'bg-gray-500 text-white':''}" on:click={(e)=>onBorder(e, "t")}><Icon path={mdiBorderTopVariant}/></div>  
       <div class="hover:bg-gray-200 hover:text-gray-800 {right?'bg-gray-500 text-white':''}" on:click={(e)=>onBorder(e, "r")}><Icon path={mdiBorderRightVariant}/></div>  
       <div class="hover:bg-gray-200 hover:text-gray-800 {bottom?'bg-gray-500 text-white':''}" on:click={(e)=>onBorder(e, "b")}><Icon path={mdiBorderBottomVariant}/></div>  
    </div>
</div>
{#if !(style==='None' || style=="Default")}
    <Border title={'Border Style'} 
                    bind:style={style} 
                    bind:width={width}
                    bind:color={color}
                    on:onChange={property_change}
                    />
{/if}