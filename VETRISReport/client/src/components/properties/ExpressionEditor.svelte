<script>
    import { createEventDispatcher } from "svelte";
    import * as report from "../../model/report/report.js";
    import TreeView from "../TreeView.svelte";
    import Editor from "./Editor.svelte";

    export let reportData;
    export let expression;
    export let height='100px';
    let editor, search='';
    const dispatch = createEventDispatcher();

    function updateContent(e){
        expression=editor.getValue();
        dispatch("expressionChange", {expression: expression});
    }
    if(!(expression || "").startsWith("=")) expression=`=${expression}`;
    const trees = [
            {
                label: "Built-in Fields", 
                children:[
                    { label: "PageNumber", data:"Globals!PageNumber"},
                    { label: "TotalPages", data:"Globals!TotalPages"},
                    //{ label: "OverallTotalPages", data:"Globals!OverallTotalPages"},
                    //{ label: "ExecutionTime", data:"Globals!ExecutionTime"},
                ]
            },
            {label: "Funtions", children:[
                {label: "Data Conversion", children:[
                    {label: "CBool", data:"CBool(expression)"},
                    {label: "CInt", data:"CInt(expression)"},
                    {label: "CSng", data:"CSng(expression)"},
                    {label: "CDbl", data:"CDbl(expression)"},
                    {label: "CStr", data:"CStr(expression)"},
                    {label: "CDate", data:"CDate(expression)"},
                    {label: "Val", data:"Val(expression)"},
                ]},
                {label: "Aggreegate", children:[
                    {label: "Sum", data:"Sum(expression)"},
                    {label: "Avg", data:"Avg(expression)"},
                    {label: "Min", data:"Min(expression)"},
                    {label: "Max", data:"Max(expression)"},
                    {label: "First", data:"First(expression)"},
                    {label: "Last", data:"Last(expression)"},
                    {label: "Count", data:"Count(expression)"},
                    {label: "CountDistinct", data:"CountDistinct(expression)"},
                ]},
                {
                    label: "Program Flow", 
                    children:[
                        { label: "IIf", data:"IIf(logicalexpression, true, false)"},
                    ]
                },
                {
                    label: "Inspection", 
                    children:[
                        { label: "IsNothing", data:"IsNothing(expression)"},
                        { label: "IsNumeric", data:"IsNumeric(expression)"},
                        { label: "IsDate", data:"IsDate(expression)"},
                    ]
                },
                {
                    label: "Text", 
                    children:[
                        { label: "Format", data:"Format(expression, formatstring)", description:"Returns a string formatted according to instructions contained in a format String expression.", example:'=Format(Globals!ExecutionTime, "Long Date")' },
                        { label: "FormatCurrency", data:"FormatCurrency(expression, decimalplaces)", description:"Returns an expression formatted as a currency value using the currency symbol defined in the server.", example:'=FormatCurrency(Fields!YearlyIncome.Value,0)' },
                        { label: "InStr", data:"InStr(expression, findstring)", description:"Returns an integer specifying the start position of the first occurrence of one string within another.", example:'=InStr(Fields!Description.Value, "car")' },
                        { label: "LCase", data:"LCase(expression)"},
                        { label: "UCase", data:"UCase(expression)"},
                        { label: "Len", data:"Len(expression)"},
                        { label: "Left", data:"Left(expression, noofchars)"},
                        { label: "Right", data:"Right(expression, noofchars)"},
                        { label: "Mid", data:"Mid(expression, startindex, noofchars)"},
                    ]
                },{
                    label: "Date/Time", 
                    children:[
                        { label: "Today", data:"Today()"},
                        { label: "Day", data:"Day(expression)"},
                        { label: "Month", data:"Month(expression)"},
                        { label: "MonthName", data:"MonthName(expression)"},
                        { label: "Weekday", data:"Weekday(expression)"},
                        { label: "WeekdayName", data:"WeekdayName(expression)"},
                        { label: "Year", data:"Year(expression)"},
                        { label: "Hour", data:"Hour(expression)"},
                        { label: "Minute", data:"Minute(expression)"},
                        { label: "Second", data:"Second(expression)"},
                        { label: "DateAdd", data:"DateAdd(DateInterval, interval, dateexpr)"},
                        { label: "DateDiff", data:"DateDiff(DateInterval, laterdateexpr, earlierdateexr)"},
                    ]
                },{
                    label: "Math", 
                    children:[
                        { label: "Abs", data:"Abs(expression)"},
                        { label: "Ceiling", data:"Ceiling(expression)"},
                        { label: "Floor", data:"Floor(expression)"},
                        { label: "Round", data:"Round(expression, decimal)"},
                    ]
                }
            ]},
            {label: "Operators", children:[
                {label: "Math", children:[
                    {label: "^", data:" ^ "},
                    {label: "+", data:" + "},
                    {label: "-", data:" - "},
                    {label: "*", data:" * "},
                    {label: "/", data:" / "},
                    {label: "\\", data:" \\ "},
                    {label: "Mod", data:" Mod "},
                ]},
                {label: "Comparison", children:[
                    {label: "=", data:" = "},
                    {label: "<", data:" < "},
                    {label: "<=", data:" <= "},
                    {label: ">", data:" > "},
                    {label: ">=", data:" >= "},
                    {label: "Like", data:" Like "},
                    {label: "Is", data:" Is "},
                ]},
                {label: "Concatenation", children:[
                    {label: "&", data:" & "},
                ]},
                {label: "Logical", children:[
                    {label: "And", data:" And "},
                    {label: "Or", data:" Or "},
                    {label: "Not", data:" Not "},
                    {label: "Xor", data:" Xor "},
                    {label: "AndAlso", data:" AndAlso "},
                    {label: "OrElse", data:" OrElse "}
                ]}
            ]}
        ];
    

    function onFuncSelected(e){
        let textValue=e.detail;
        editor.setValueAtCursor(textValue);
    }
    function onFieldSelected(textValue){
        editor.setValueAtCursor(textValue);
    }
</script>

<div class="flex flex-col p2">
    <div class="w-full border rounded-sm p-1">
        <Editor bind:this={editor} value={expression} height={height} language={'vb'} wordWrap={'on'} />
    </div>
    <div class="flex flex-row text-sm ">
        <div class="flex flex-col w-1/3 overflow-y-auto border rounded-sm" style="max-height:217px;">
            {#each trees as tree}
            <TreeView tree={tree} defaultexpaned={true} on:onFunctionSelected={onFuncSelected}/>
            {/each}
        </div>
        <div class="flex flex-col w-2/3 ml-2">
            <div class="w-full font-bold">Datasource and Parameters</div>
            <div class="flex flex-col mr-2">
                <div class="flex flex-row border">
                    <div class="flex-1 text-sm bg-white p-1">
                        <input type="text" class="w-full hover:outline-none focus:outline-none bg-transparent" 
                            placeholder="Search" bind:value={search}>
                    </div>
                    <svg class="w-4 h-4 m-1.5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
                    </svg>
                </div>
                <div class="flex flex-row items-center space-x-1 border-b p-1 font-semibold text-gray-400">
                    <div class="flex flex-1 hover:text-gray-900">Name</div>
                    <div class="flex w-20 hover:text-gray-900">Type</div>
                </div>
                <div class="overflow-y-auto border-b border-l border-r" style="max-height:140px;"> 
                    {#if reportData.dataSets.dataSet.length>0}
                    {#each reportData.dataSets.dataSet[0].fields.field.filter(i=>search?i.dataField.toLowerCase().indexOf(search.toLowerCase()) >= 0 :true) as field}
                    <div class="flex flex-row items-center p-1 text-gray-900 hover:bg-gray-200" on:dblclick={(e)=>onFieldSelected(`Fields!${field.dataField}.Value`)}>
                        <div class="flex-auto">{reportData.dataSets.dataSet[0].name}.{field.dataField}</div>
                        <div class="flex w-20 text-white text-xs"><span class="px-1 bg-green-800 rounded-sm">{report.getFieldType(field.typeName,"d")}</span></div>
                    </div>
                    {/each}
                    {/if}
                    {#each report.getParameters(reportData).filter(i=>search?i.name.toLowerCase().indexOf(search.toLowerCase()) >= 0 :true) as p}
                    <div class="flex flex-row items-center p-1 text-gray-900 hover:bg-gray-200" on:dblclick={(e)=>onFieldSelected(`Parameters!${p.name}.Value`)}>
                        <div class="flex-auto">Parameter.{p.name}</div>
                        <div class="w-20 text-white text-xs"><span class="px-1 bg-green-800 rounded-sm">{p.dataType}</span></div>
                    </div>
                    {/each}
                </div>
            </div>
        </div>
    </div>
</div>
<div class="flex flex-row items-center justify-end mb-2">
    <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={updateContent}>OK</button>
    <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e)=> dispatch("close")}>Cancel</button>
</div>
