
import { v4 as uuidv4 } from 'uuid';
import moment from 'moment-timezone';
import { user } from "../user.js"; 
import Swal from 'sweetalert2';

let apiendpoint=null;
let fontlist=[];
export let pagSizes=getPageSizes();

export async function init(url) {
    apiendpoint=url;
    await getFontList();
}
export const fonts=()=> [...fontlist];

export async function NewReportWithDefaultDS () {
    let report = {
        reportId: uuidv4(),
        autoRefresh: 0,
        width: "6.5in",
        reportParameters: null,
        reportParametersLayout: null,
        embeddedImages:{
            embeddedImage:[]
        },
        reportUnitType: "Inch",
        page: makePage(),
        body: makeBody(),
        dataSources: {
            dataSource:[]
        },
        dataSets: {
            dataSet:[]
        }
    };
    addDataSource(report,"datasource1","System.Data.SqlClient","");
    const cmd="\r\n                select \r\n\t                a.id,a.billing_cycle_id,a.billing_cycle,a.billing_account_id,a.billing_account, a.invoice_no, a.invoice_date, a.total_amount, \r\n\t                sum(a.adjusted) adjusted_amount,\r\n\t                sum(a.refunded) refunded_amount,\r\n\t                a.total_amount-sum(a.adjusted+a.refunded) balance_amount\r\n                from (\r\n\t\t                select 'O' adj_source, hdr.id,hdr.billing_account_id,hdr.invoice_no,hdr.opbal_date invoice_date,\r\n\t\t\t                    billing_cycle_id='00000000-0000-0000-0000-000000000000',billing_cycle = '',\r\n\t\t\t                    hdr.opbal_amount total_amount, \r\n\t\t\t\t                case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, \r\n\t\t\t\t                case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded,\r\n\t\t\t\t                ba.name billing_account\r\n\t\t                from ar_opening_balance hdr with(nolock) \r\n\t\t                left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id\r\n\t\t                left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id\r\n\t\t                UNION ALL\r\n\t\t                select 'I' adj_source, hdr.id,hdr.billing_account_id,hdr.invoice_no,hdr.invoice_date,\r\n\t\t\t\t                hdr.billing_cycle_id,billing_cycle=bc.name,\r\n\t\t\t                    hdr.total_amount, \r\n\t\t\t\t                case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, \r\n\t\t\t\t                case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded,\r\n\t\t\t\t                ba.name billing_account\r\n\t\t                from invoice_hdr hdr with(nolock) \r\n\t\t                left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id\r\n\t\t                left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id\r\n\t\t                inner join billing_cycle bc on bc.id = hdr.billing_cycle_id\r\n\t\t                and hdr.approved='Y'\t\r\n                ) a\r\n                group by a.id,a.billing_cycle_id,a.billing_cycle,a.billing_account_id,a.billing_account, a.invoice_no, a.invoice_date,a.total_amount\r\n                having a.total_amount-sum(a.adjusted)>0";
    await addDataSet(report,"dataset1","datasource1",cmd)
    return report;
}

export function NewReport (id) {
    id=id||uuidv4();
    return {
        reportId: id,
        autoRefresh: 0,
        width: "6.5in",
        reportParameters: null,
        reportParametersLayout: null,
        embeddedImages:{
            embeddedImage:[]
        },
        reportUnitType: "Inch",
        page: makePage(),
        body: makeBody(),
        dataSources: {
            dataSource:[]
        },
        dataSets: {
            dataSet:[]
        }
    };
}

export async function getReportForEdit(id){
    const res = await fetch(`${apiendpoint}/api/report/getforedit?id=${id}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': user.token()
        }
    });
    if(res.ok) {
        const json = await res.json();
        if(!json.isError){
            const result=json.result;
            if(!result.report){
                result.report=NewReport(result.id);
            }
            else{
                result.report={...normalize(result.report)};
            }
            return result;
        }
    }
    else {
        const text=await res.text();

    }
}
export async function createReport(name, category){
    const id=uuidv4();
    const postData ={
        id: id,
        name: name,
        category: category||'Drafts',
        draft: true,
        jsonReport: JSON.stringify(NewReport(id))
    }
    const res = await fetch(`${apiendpoint}/api/report/create`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': user.token()
        },
        body: JSON.stringify(postData)
    });
    if(res.ok) {
        const json = await res.json();
        if(!json.isError){
            return id;
        }
        else{
            throw { message: "Cannot create report." }
        }
    }
    else {
        const text=await res.text();
        throw { message: text }
    }
}
export async function saveDraft(name, category, report){
    const postData ={
        id: report.reportId,
        name: name,
        category: category||'Drafts',
        draft: true,
        report: JSON.stringify(normalizeForSave(report))
    }
    const res = await fetch(`${apiendpoint}/api/report/save`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': user.token()
        },
        body: JSON.stringify(postData)
    });
    if(res.ok) {
        const json = await res.json();
        if(!json.isError){
            return report.reportId;
        }
        else{
            throw { message: "Cannot create report." }
        }
    }
    else {
        const text=await res.text();
        throw { message: text }
    }
}
export async function publish(name, publishcategory, report){
   
    const postData ={
        id: report.reportId,
        name: name,
        category: (publishcategory==="Drafts"?"Published":publishcategory||'Published'),
        draft: false,
        report: JSON.stringify(normalizeForSave(report))
    }
    const res = await fetch(`${apiendpoint}/api/report/save`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': user.token()
        },
        body: JSON.stringify(postData)
    });
    if(res.ok) {
        const json = await res.json();
        if(!json.isError){
            return report.reportId;
        }
        else{
            throw { message: "Cannot save report." }
        }
    }
    else {
        const text=await res.text();
        throw { message: text }
    }
}
export function ConvertWithParameter(report) {
    return {
        reportId: report.reportId,
        autoRefresh: report.autoRefresh,
        reportUnitType: report.reportUnitType,
        dataSources: report.dataSources,
        dataSets: report.dataSets,
        reportSections:{
            reportSection:{
                width: report.width,
                body: report.body,
                page: report.Page
            }
        },
        reportParameters:{
            reportParameter:[]
        },
        reportParametersLayout:{
            gridLayoutDefinition:{
                numberOfColumns:4,
                numberOfRows:2,
                cellDefinitions:{
                    cellDefinition:[]
                }
            }
        },
        embeddedImages:{
            embeddedImage:[]
        }
    }
}

export function normalize(report){
    let data={
        reportId: report.reportId,
        autoRefresh: report.autoRefresh,
        reportUnitType: report.reportUnitType,
        dataSources: report.dataSources,
        dataSets: report.dataSets,
        page: getPage(report),
        body: getBody(report),
        width: getWidth(report),
        reportParameters: report.reportParameters,
        reportParametersLayout: report.reportParametersLayout,
        embeddedImages: report.embeddedImages
    };
    if(!data.embeddedImages){
        data.embeddedImages={
            embeddedImage:[]
        };
    }
    return data;
}
export function getParameters(report){
    const hasParameters=!(report.reportParameters==undefined||report.reportParameters==null);
    if(hasParameters) return report.reportParameters.reportParameter;
    return [];
}


export function normalizeForSave(report){
    const hasParameters=!(report.reportParameters==undefined||report.reportParameters==null||report.reportParameters.reportParameter.length==0);
    let data=(()=>{
        if(hasParameters){
            return {
                reportId: report.reportId,
                autoRefresh: report.autoRefresh,
                reportUnitType: report.reportUnitType,
                dataSources: report.dataSources,
                dataSets: report.dataSets,
                reportSections:{
                    reportSection:{
                        width: report.width,
                        body: report.body,
                        page: report.page
                    }
                },
                reportParameters: report.reportParameters,
                reportParametersLayout: report.reportParametersLayout,
                embeddedImages: report.embeddedImages
            }
        }
        return {
            reportId: report.reportId,
            autoRefresh: report.autoRefresh,
            reportUnitType: report.reportUnitType,
            dataSources: report.dataSources,
            dataSets: report.dataSets,
            width: report.width,
            body: report.body,
            page: report.page,
            embeddedImages: report.embeddedImages
        }
    })();
    // sanitize data 
    data = {...data};
    // ConnectionProperties -> ConnectString is must
    if(data.dataSources && data.dataSources.dataSource){
        data.dataSources.dataSource.forEach(ds=>{
            if(!ds.connectionProperties.connectString){
                ds.connectionProperties.connectString="/* Local connection string */";
            }
        });
    }
    if(data.embeddedImages && data.embeddedImages.embeddedImage && data.embeddedImages.embeddedImage.length==0){
        delete data.embeddedImages;
    } 
    
    data={...data};
    return data;
}


export async function generatePDFForPreview(report){
    let mrpt={...report};
    if(mrpt.reportParameters && mrpt.reportParameters.reportParameter && mrpt.reportParameters.reportParameter && mrpt.reportParameters.reportParameter.length>0){
        let rparams=mrpt.reportParameters.reportParameter;
        let params=[];
        for(let i=0; i<rparams.length; i++){
            let p={...rparams[i]};
            if(p.dataType==="DateTime"){
                p.inputValue=moment(p.inputValue).format("YYYY-MM-DD");
            }
            params=[...params,p];
        }
        mrpt.reportParameters.reportParameter=[...params];
    }
    const data=normalizeForSave(mrpt);

    const postData = {
        report: JSON.stringify(data)
    };
   
    const res = await fetch(`${apiendpoint}/api/report/preview/pdf`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': user.token()
        },
        body: JSON.stringify(postData)
    });
    
    const json = await res.json();
    return json;
    
}



export function addParameter(report, param){
    const hasParameters=!(report.reportParameters==undefined||report.reportParameters==null);
    if(!hasParameters){
        report.reportParameters={
            reportParameter:[]
        };
        report.reportParametersLayout={
            gridLayoutDefinition:{
                numberOfColumns:4,
                numberOfRows:2,
                cellDefinitions:{
                    cellDefinition:[]
                }
            }
        };
    }
    const index = report.reportParameters.reportParameter.length;
    report.reportParameters.reportParameter=[...report.reportParameters.reportParameter,param];
    addParameterInLayout(report.reportParametersLayout,param, index);
    report=Object.assign({}, report);
}

export function removeParameter(report, name){
    const hasParameters=!(report.reportParameters==undefined||report.reportParameters==null);
    if(!hasParameters){
        return;
    }
    const index = report.reportParameters.reportParameter.findIndex(i=>i.name.toLowerCase()===name.toLowerCase());
    report.reportParameters.reportParameter.splice(index,1);
    adjustParameterInLayout(report.reportParametersLayout,report.reportParameters.reportParameter);
    report=Object.assign({}, report);
}

function addParameterInLayout(layout, param, index){
    let cols=layout.gridLayoutDefinition.numberOfColumns;
    let rows = layout.gridLayoutDefinition.numberOfRows;
    /*       0 1 2 3
             -------
        0 => 0 1 2 3
        1 => 4 5 6 7

        index = 5
        col = (index) % cols;
        row = (index) / cols;
    */
    const col = (index) % cols;
    const row = Math.floor(index/cols);
    if(row>=rows) {
        while(row>=rows) rows++;
        layout.gridLayoutDefinition.numberOfRows=rows;
    }
    let definition = layout.gridLayoutDefinition.cellDefinitions.cellDefinition
    definition = [...definition, {
        columnIndex: col,
        rowIndex: row,
        parameterName: param.name
    }];
    layout.gridLayoutDefinition.cellDefinitions.cellDefinition=[...definition];
}
function adjustParameterInLayout(layout, params){
    let cols=layout.gridLayoutDefinition.numberOfColumns;
    let rows = layout.gridLayoutDefinition.numberOfRows;
    /*       0 1 2 3
             -------
        0 => 0 1 2 3
        1 => 4 5 6 7

        index = 5
        col = (index) % cols;
        row = (index) / cols;
    */
    
    let definition = [];
    params.forEach((p, index) => {
        const col = (index) % cols;
        const row = Math.floor(index/cols);
        if(row>=rows) {
            while(row>=rows) rows++;
            layout.gridLayoutDefinition.numberOfRows=rows;
        }
        definition = [...definition, {
            columnIndex: col,
            rowIndex: row,
            parameterName: p.name
        }];
    });
    
    layout.gridLayoutDefinition.cellDefinitions.cellDefinition=[...definition];
}

export function getFilters(table){
    const hasFilters=!(table.filters==undefined||table.filters==null);
    if(hasFilters) return table.filters.filter;
    return [];
}
export function addFilter(table, filter){
    const hasFilters=!(table.filters==undefined||table.filters==null);
    if(!hasFilters){
        table.filters={
            filter:[]
        };
    }
    table.filters.filter=[...table.filters.filter,filter];
}
export function updateFilter(table, index, filter){
    const hasFilters=!(table.filters==undefined||table.filters==null);
    if(!hasFilters){
        return;
    }
    table.filters.filter[index]=Object.assign({},filter);
    table.filters.filter=[...table.filters.filter];
}
export function removeFilter(table, index){
    const hasFilters=!(table.filters==undefined||table.filters==null);
    if(!hasFilters){
        return;
    }
    table.filters.filter.splice(index,1);
    table.filters.filter=[...table.filters.filter];
}

export function hasReportSections(report){
    const hasSections = !!(report.reportSections && report.reportSections.reportSection);
    if(hasSections) return true;
}

export function getPage(report){
    if(!hasReportSections(report))
        return report.page;
    return report.reportSections.reportSection.page;
}

export function getBody(report){
    if(!hasReportSections(report))
        return report.body;
    return report.reportSections.reportSection.body;
}
export function getWidth(report){
    if(!hasReportSections(report))
        return report.width;
    return report.reportSections.reportSection.width;
}



export function makePage(){
    return {
        pageHeight: "11.70000in",
        pageWidth: "8.30000in",
        leftMargin: "0.25000in",
        rightMargin: "0.25000in",
        topMargin: "0.25000in",
        bottomMargin: "0.25000in",
        columnSpacing: "0.06in",
        style: {},
        pageHeader: makePageHeader(),
        pageFooter: makePageFooter()
    }; 
}

export function makePageHeader(){
    return {
        height: "1.524cm",
        printOnFirstPage: true,
        printOnLastPage: true,
        reportItems: {
            tablix:[],
            textbox:[],
            line:[],
            rectangle:[],
            image:[]
        },
        style:{

        }
    }; 
} 
export function makePageFooter(){
    return {
        height: "1.524cm",
        printOnFirstPage: true,
        printOnLastPage: true,
        reportItems: {
            tablix:[],
            textbox:[],
            line:[],
            rectangle:[],
            image:[]
        },
        style:{
            
        }
    }; 
} 

export function makeBody(){
    return {
        height: "1.524cm",
        reportItems: {
            tablix:[],
            textbox:[],
            line:[],
            rectangle:[],
            image:[]
        },
        style:{
            
        }
    }; 
}

export function addDataSource(report, name, provider, connectionString){
    
    if(report.dataSources.dataSource){
        let _ds=report.dataSources.dataSource.find(i=>i.name.toLowerCase()==name.toLowerCase());
        if(!_ds){
            let ds = {
                dataSourceID: uuidv4(),
                name: name,
                connectionProperties: {
                    dataProvider: provider,
                    connectString: connectionString??"/* Local connection string */"
                }
            };
            report.dataSources.dataSource=[...report.dataSources.dataSource, ds];
        }
        else {
            throw {message: 'Duplicate datasource name!' }
        }
    }
    report = Object.assign({}, report); 
}

export async function addDataSet(report, name, sourceName, commandText, params=null){
    if(report.dataSources.dataSource && report.dataSources.dataSource.length>0){
        const _ds=report.dataSources.dataSource.find(i=>i.name.toLowerCase()==sourceName.toLowerCase());
        if(_ds){
            let dss = report.dataSets.dataSet.find(i=>i.name.toLowerCase()==name.toLowerCase());
            if(dss){
                throw {message: 'datasource exists!' }
            }
            let parameterData=null;
            if(params && params.length>0){
                parameterData = getParameters(report)
                .map(function(i) {
                    return {name:i.name, dataType:i.dataType, value: null}
                });
            }
            dss = {
                name: name,
                query: {
                    dataSourceName: sourceName,
                    commandText: commandText
                },
                fields:{
                    field: []
                } 
            };
            await generateDatasetFields(_ds, commandText, parameterData).then(result=>{
                dss.fields={
                    field: result
                }; 
                report.dataSets.dataSet=[...report.dataSets.dataSet, dss];
                // update tables dataset names
                updateTableDatasetName(report,name);
                report = Object.assign({}, report); 
            }).catch(e=>{
                debugger;
            })
            
        }
        else {

            throw {message: 'datasource not found!' }
        }
    }
    else {
        throw {message: 'Add datasource first!' }
    }
}
export async function updateDataSet(report, name, sourceName, commandText, params=null){
    if(report.dataSources.dataSource && report.dataSources.dataSource.length>0){
        const _ds=report.dataSources.dataSource.find(i=>i.name.toLowerCase()==sourceName.toLowerCase());
        if(_ds){
            let index = report.dataSets.dataSet.findIndex(i=>i.name.toLowerCase()==name.toLowerCase());
            if(index==-1){
                throw {message: 'datasource does not exist!' }
            }
            let parameterData=null;
            if(params && params.length>0){
                parameterData = getParameters(report)
                .map(function(i) {
                    return {name:i.name, dataType:i.dataType, value: null}
                });
            }
            let dss = {
                name: name,
                query: {
                    dataSourceName: sourceName,
                    commandText: commandText
                },
                fields:{
                    field: []
                } 
            };
            await generateDatasetFields(_ds, commandText, parameterData).then(result=>{
                dss.fields={
                    field: result
                }; 
                report.dataSets.dataSet[index]=dss;
                report.dataSets.dataSet=[...report.dataSets.dataSet];
                // update tables dataset names
                updateTableDatasetName(report,name);
                report = Object.assign({}, report); 
            }).catch(e=>{
                debugger;
            })
            
        }
        else {

            throw {message: 'datasource not found!' }
        }
    }
    else {
        throw {message: 'Add datasource first!' }
    }
}
export function shouldHaveFilter(dataset){
    return dataset.query.commandText && dataset.query.commandText.match(/order by\s*(\w+)/mi)==null;
}
function updateTableDatasetName(report, dsname){
    if(!(report.body.reportItems.tablix===null||report.body.reportItems.tablix===undefined)){
        report.body.reportItems.tablix.forEach(table=>{
            if(table.dataSetName===undefined||table.dataSetName===null||table.dataSetName==='')
                table.dataSetName=dsname;
        });
    }
    if(report.page.header && !(report.page.header.reportItems.tablix===null||report.page.header.reportItems.tablix===undefined)){
        report.page.header.reportItems.tablix.forEach(table=>{
            if(table.dataSetName===undefined||table.dataSetName===null||table.dataSetName==='')
                table.dataSetName=dsname;
        });
    }
    if(report.page.footer && !(report.page.footer.reportItems.tablix===null||report.page.footer.reportItems.tablix===undefined)){
        report.page.footer.reportItems.tablix.forEach(table=>{
            if(table.dataSetName===undefined||table.dataSetName===null||table.dataSetName==='')
                table.dataSetName=dsname;
        });
    }
}

export function removeDataset(report, name) {
    const index = report.dataSets.dataSet.findIndex(i=>i.name.toLowerCase()==name.toLowerCase());
    if(index==-1){
        //throw {message: 'dataset does not exist!' }
        return;
    }
    report.dataSets.dataSet.splice(index,1);
    report = Object.assign({}, report); 
}

async function generateDatasetFields(datasource, commandText, params) {
    const postData = {
        commandText: commandText,
        parameters: params
    };
   
    const res = await fetch(`${apiendpoint}/api/metadata/generatefieldswithparameters`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': user.token()
        },
        body: JSON.stringify(postData)
    });
    
    const json = await res.json();
    
    if(!json.isError){
        return json.result;
    }
   
    return [];
}

async function getFontList() {
   
   
    const res = await fetch(`${apiendpoint}/api/metadata/reportfonts`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': user.token()
        }
    });
    
    const json = await res.json();
    
    if(!json.isError){
         fontlist=[...json.result];
         return json.result;
    }
    return [];
}
export function makeTable(rows, columns,rowHeight="0.6cm", colWidth="3.9624cm"){
    let tablixBody={
        tablixColumns : makeColumns(columns,colWidth),
        tablixRows:{
            tablixRow: makeRows(rows,columns,rowHeight)
        }
    };
    return {
        repeatColumnHeaders:true,
        dataSetName:null,
        top:null,
        left:null,
        height:null,
        width:null,
        style: {
            border: {
                style: "None"
            },
            fontSize:null,
            fontFamily: null,
            fontWeight:null,
            color: null,
            backgroundColor: null
        },
        tablixBody:tablixBody,
        tablixRowHierarchy:makeGroups(tablixBody),
        tablixColumnHierarchy:makeColumnHierarchy(columns)
    };
}

export function makeColumns(nos, defaultWidth="3.9624cm"){
    let cols = [];

    for (let index = 0; index < nos; index++) {
        const element = {
                width: defaultWidth
          };
        cols.push(element);
    }
    return {
        tablixColumn: cols
    };
}

export function makeColumnHierarchy(nos){
    
    return {
        tablixMembers:{
            tablixMember: (new Array(nos)).fill().map(function(i) {return {};})
        }
    };
}

export function makeGroups(tablixBody){
    let rows=tablixBody.tablixRows.tablixRow.length;
    let groups = [];
    for (let r = 0; r < rows; r++) {
        const element = {
            group: r==1?{
                "name": "Details",
                "groupExpressions": null
              }:null
        };
        groups.push(element);
    }
    return {
        tablixMembers:{
            tablixMember: groups
        }
    };
}

export function addFieldToCell(table, rowIndex, colIndex, field){
    addTextToCell(table, rowIndex, colIndex, `=Fields!${field.dataField}.Value`);
    if(table.tablixRowHierarchy?.tablixMembers?.tablixMember.length<=1){
        return;
    }
    if(rowIndex>0 && isGroupRow(table, rowIndex)){
        let contents = table.tablixBody.tablixRows.tablixRow[rowIndex-1].tablixCells.tablixCell[colIndex].cellContents;
        if(!getField(table.tablixBody.tablixRows.tablixRow[rowIndex-1], colIndex)){
            addTextToCell(table, rowIndex-1, colIndex, getTitleFromField(field));
        }
    }
}

export function isGroupRow(table, rowIndex){
    if(table.tablixRowHierarchy?.tablixMembers?.tablixMember.length==0){
        return false;
    }
    const group=table.tablixRowHierarchy.tablixMembers.tablixMember[rowIndex].group;
    return (group && group.name)?true:false;
}

export function addTextToCell(table, rowIndex, colIndex, text){
    const name= getNewTextBox();
    let textbox = {
        name: name,
        canGrow: true,
        canShrink: true,
        keepTogether: true,
        paragraphs: {
            paragraph: {
                textRuns: {
                    textRun : {
                        value: text||'',
                        style: defaultTextRunStyle()
                    }
                },
                style:{
                    textAlign:"Default"
                }
            }
        },
        defaultName:name,
        style: getDefaultTextboxStyle()
    };
    table.tablixBody.tablixRows.tablixRow[rowIndex].tablixCells.tablixCell[colIndex].cellContents.textbox=textbox;
}

function defaultTextRunStyle(){
    return {
        format:null,
        fontSize:null,
        fontFamily: null,
        fontStyle:null,
        fontWeight:null,
        color: "#000000",
        textDecoration:null,    
    }
}

export function setTextToCell(table, rowIndex, colIndex, text){
    const contents=table.tablixBody.tablixRows.tablixRow[rowIndex].tablixCells.tablixCell[colIndex].cellContents;
    if(contents.textbox){
        textbox.paragraphs.paragraph.textRuns.textRun.value=text;
    }
    else {
        addTextToCell(table, rowIndex, colIndex, text);
    }
}

export function getCellText(table, rowIndex, colIndex){
    const contents=table.tablixBody.tablixRows.tablixRow[rowIndex].tablixCells.tablixCell[colIndex].cellContents;
    if(contents.textbox){
        return contents.textbox.paragraphs.paragraph.textRuns.textRun.value;
    }
    else 
        return null;
}

export function getField(row, colIndex){
    if(!row) return '';
    const cellContents = row.tablixCells.tablixCell[colIndex].cellContents;

    const textbox = cellContents.textbox;
    if(!textbox) return '';

    let data = getTextBoxContentDisplay(textbox);
    return data;
}

export function isTextType(row,colIndex){
    if(!row) return true;
    const cellContents = row.tablixCells.tablixCell[colIndex].cellContents;

    const textbox = cellContents.textbox;
    if(!textbox) return true;

    let data = textbox.paragraphs.paragraph.textRuns.textRun.value;
    if(data.startsWith("=")){
        return false;
    }
    return true;
}

export function getTextBoxContentType(textbox){
    
    if(!textbox) return null;

    let data = textbox.paragraphs.paragraph.textRuns.textRun.value;
    if(data.startsWith("=")){
        return "expr";
    }
    return "text";
}
export function getTextBoxContentDisplay(textbox){
    
    if(!textbox) return null;

    let data = textbox.paragraphs.paragraph.textRuns.textRun.value;
    
    return getExpressionContentDisplay(data);
}
export function getExpressionContentDisplay(text){
    let data=text??"";
    if(data.match(/^=Fields!(\w+)\.Value$/)){
        return `[${data.match(/=Fields!(\w+)\.Value/)[1]}]`;
    }
    else if(data.match(/^=(Sum|Avg|First|Last|Max|Min)\(Fields!(\w+)\.Value$/)){
        let m=data.match(/=(Sum|Avg|First|Last|Max|Min)\(Fields!(\w+)\.Value/);
        return `[${m[1]}(${m[2]})]`;
    }
    else if(data.match(/^=Globals!(\w+)$/)){
        return `[&${data.match(/=Globals!(\w+)/)[1]}]`;
    }
    else if(data.match(/^=Parameters!(\w+)\.Value$/)){
        return `[@${data.match(/=Parameters!(\w+)\.Value/)[1]}]`;
    }
    else if(data.startsWith("=")){
        return "«Expr»";
    }
    return data;
}
export function isExpressionOrFieldType(row,colIndex){
    if(!row) return false;
    const cellContents = row.tablixCells.tablixCell[colIndex].cellContents;

    const textbox = cellContents.textbox;
    if(!textbox) return false;

    let data = textbox.paragraphs.paragraph.textRuns.textRun.value;
    if(data.match(/^=Fields!(\w+)\.Value$/)){
        return true;
    }
    else if(data.startsWith("=")){
        return true;
    }
    return false;
}

export function getTitleFromField(field){
    const titleCase = (s) =>
        s.replace(/([A-Z])/g, ' $1').trim().replace(/^_*(.)|_+(.)/g, (s, c, d) => c ? c.toUpperCase() : ' ' + d.toUpperCase())

    return titleCase(field.dataField);
}

export function getNewTextBox() {
    let index = Math.floor((Math.random() * 1000000) + 1);
    return getNewObjectName("Textbox");
}

export function getNewObjectName(type) {
    let index = Math.floor((Math.random() * 1000000) + 1);
    return `${type}${index}`;
}

export function addColumnRight(tablix, index, defaultWidth="3.9624cm"){
    let tablixBody=tablix.tablixBody;
    let members=tablix.tablixColumnHierarchy.tablixMembers.tablixMember;
    let cols = tablixBody.tablixColumns.tablixColumn;
    
    let totalCols = cols.length;
    let col = { width: defaultWidth };
    

    let endIndex=findSpanEnd(tablixBody, index);
    cols.splice(endIndex+1,0,col);
    // consider TablixColumnHierarchy  tablixMembers.tablixMember
    members.splice(endIndex+1,0,{});

    for (let r = 0; r < tablixBody.tablixRows.tablixRow.length; r++) {
        let row=tablixBody.tablixRows.tablixRow[r];
        const element = {
            cellContents:{
                colSpan: null
            }
        };
        row.tablixCells.tablixCell.splice(endIndex+1,0,element);
    }
}

export function addColumnLeft(tablix, index, defaultWidth="3.9624cm"){
    let tablixBody=tablix.tablixBody;
    let cols = tablixBody.tablixColumns.tablixColumn;
    let members=tablix.tablixColumnHierarchy.tablixMembers.tablixMember;
    let totalCols = cols.length;
    let col = { width: defaultWidth };
    
    let startIndex=findSpanBegin(tablixBody, index);
    cols.splice(startIndex,0,col);
    // consider TablixColumnHierarchy  tablixMembers.tablixMember
    members.splice(startIndex,0,{});
    for (let r = 0; r < tablixBody.tablixRows.tablixRow.length; r++) {
        let row=tablixBody.tablixRows.tablixRow[r];
        const element = {
            cellContents:{
                colSpan: null
            }
        };
        row.tablixCells.tablixCell.splice(startIndex,0,element);
    }
}

export function removeColumn(tablix, index){
    let tablixBody=tablix.tablixBody;
    let members=tablix.tablixColumnHierarchy.tablixMembers.tablixMember;
    let cols = tablixBody.tablixColumns.tablixColumn;
    let startIndex=findSpanBegin(tablixBody, index);
    let endIndex=findSpanEnd(tablixBody, index);
   
    for (let r = 0; r < tablixBody.tablixRows.tablixRow.length; r++) {
        let row=tablixBody.tablixRows.tablixRow[r];
        let c=endIndex;
        while(c>=startIndex){
            row.tablixCells.tablixCell.splice(c,1);
            c--;
        }
    }
    let cc=endIndex;
    while(cc>=startIndex){
        cols.splice(cc,1);
        // consider TablixColumnHierarchy  tablixMembers.tablixMember
        members.splice(cc,1);
        cc--;
    }
}

function findSpanEnd(tablixBody, index){
    let rows=tablixBody.tablixRows.tablixRow;
    let endIndex = index;
    for (let r = 0; r < rows.length; r++) {
        const row = rows[r];
        for (let c = index; c >=0; c--) {
            const s = row.tablixCells.tablixCell[c].colSpan;
            if(s!==null && !isNaN(parseInt(s))){
                let tx=parseInt(s)+c-1;
                if(tx>endIndex) endIndex=tx;
                break;
            }
        }
    }
    return endIndex;
}
function findSpanBegin(tablixBody, index){
    let rows=tablixBody.tablixRows.tablixRow;
    let startIndex = index;
    for (let r = 0; r < rows.length; r++) {
        const row = rows[r];
        for (let c = index; c >=0; c--) {
            const s = row.tablixCells.tablixCell[c].colSpan;
            if(s!==null && !isNaN(parseInt(s))){
                if(c<startIndex) startIndex=c;
                break;
            }
        }
    }
    return startIndex;
}
function findSpanBeginInRow(tablixRow, index){
    let startIndex = index;
    const row = tablixRow;
    for (let c = index; c >=0; c--) {
        const s = row.tablixCells.tablixCell[c].colSpan;
        if(s!==null && !isNaN(parseInt(s))){
            if(c<startIndex) startIndex=c;
            break;
        }
    }
    return startIndex;
}
export function addRow(table, rowIndex, bias){
    let colcount=table.tablixBody.tablixColumns.tablixColumn.length;
    let row = makeRow(colcount,"0.6cm");
    let rows= table.tablixBody.tablixRows.tablixRow;
    let rowh = table.tablixRowHierarchy.tablixMembers.tablixMember;

    // consider tablixRowHierarchy  tablixMembers.tablixMember
    if(bias==-1){
        rowh.splice(rowIndex,0,{group:null});
        rows.splice(rowIndex,0,row);
    } else if(bias==1){
        rowh.splice(rowIndex+1,0,{group:null});
        rows.splice(rowIndex+1,0,row);
    }
}
export function removeRow(table, rowIndex){
    let colcount=table.tablixBody.tablixColumns.tablixColumn.length;
    let rows= table.tablixBody.tablixRows.tablixRow;
    let rowh = table.tablixRowHierarchy.tablixMembers.tablixMember;
    // consider tablixRowHierarchy  tablixMembers.tablixMember
    rowh.splice(rowIndex,1);
    rows.splice(rowIndex,1);
}
export function makeRow(cols,rowHeight="0.6cm"){
    let row = { 
        height: rowHeight, 
        tablixCells:{
            tablixCell:[]
        }
    };

    for (let index = 0; index < cols; index++) {
        const element = {
            cellContents:{
                colSpan: null
            }
          };
          row.tablixCells.tablixCell.push(element);
    }
    return row;
}

export function makeRows(rows, cols,rowHeight="0.6cm"){
    let rowdata= [];

    for (let index = 0; index < rows; index++) {
        const row = makeRow(cols,rowHeight);
        rowdata.push(row);
    }
    return rowdata;
}
export function getRow(table, index) {
    let rows= table.tablixBody.tablixRows.tablixRow;
    if(rows.length>0 && index>=0 && index<rows.length) return rows[index];
    return null;
}
export function getCell(table, rowIndex,colIndex) {
    let row= table.tablixBody.tablixRows.tablixRow[rowIndex];
    let spanIndex=findSpanBeginInRow(row,colIndex);
    let cell= row.tablixCells.tablixCell[spanIndex];
    return cell;
}
export function getCellTextbox(table, rowIndex,colIndex) {
    let cell= getCell(table,rowIndex,colIndex);
    let textbox= getTextBoxInCell(cell);
    return textbox;
}
export function getOrAddCellTextbox(table, rowIndex,colIndex) {
    let cell= getCell(table,rowIndex,colIndex);
    let textbox= getTextBoxInCell(cell);
    if(!textbox){
        const name= getNewTextBox();
        textbox = {
            name: name,
            canGrow: true,
            keepTogether: true,
            paragraphs: {
                paragraph: {
                    textRuns: {
                        textRun : {
                            value: '',
                            style: {

                            }
                        }
                    }
                }
            },
            defaultName:name,
            style: getDefaultTextboxStyle()
        };
        cell.cellContents.textbox=textbox;
    }
    return textbox;
}
export function getTextBoxInCell(cell) {
    let textbox = cell.cellContents.textbox;
    if(textbox) return textbox;
    return null;
}

export function addTextBoxToSection(reportObject, section, expectedname,left, top, width, height) {
    debugger;
    const name= expectedname ?? getNewObjectName("Textbox");
    let textbox = {
            name: name,
            canGrow: true,
            keepTogether: true,
            paragraphs: {
                paragraph: {
                    textRuns: {
                        textRun : {
                            value: '',
                            style: defaultTextRunStyle()
                        }
                    },
                    style:{
                        textAlign:null
                    }
                }
            },
            defaultName:name,
            style: getDefaultTextboxStyle(),
            left: pxToInch(left||0),
            top: pxToInch(top||0),
            height: pxToInch(height||0),
            width: pxToInch(width||0),
        };
    if(section==="body"){
        reportObject.body.reportItems.textbox=[...reportObject.body.reportItems.textbox,textbox];
    } else if(section==="header") {
        reportObject.page.pageHeader.reportItems.textbox=[...reportObject.page.pageHeader.reportItems.textbox,textbox];
    } else if(section==="footer"){
        reportObject.page.pageFooter.reportItems.textbox=[...reportObject.page.pageFooter.reportItems.textbox,textbox];
    } else {
        return null;
    }
    return textbox;
}


export function addLineToSection(reportObject, section, expectedname, left, top, width, height) {
    const name= expectedname ?? getNewObjectName("Line");
    let line = {
            name: name,
            defaultName:name,
            left: pxToInch(left||0)+"in",
            top: pxToInch(top||0)+"in",
            height: pxToInch(height||0)+"in",
            width: pxToInch(width||0)+"in",
            zIndex:1,
            style: getDefaultLineStyle()
        };
    if(section==="body"){
        reportObject.body.reportItems.line=[...reportObject.body.reportItems.line,line];
    } else if(section==="header") {
        reportObject.page.pageHeader.reportItems.line=[...reportObject.page.pageHeader.reportItems.line,line];
    } else if(section==="footer"){
        reportObject.page.pageFooter.reportItems.line=[...reportObject.page.pageFooter.reportItems.line,line];
    } else {
        return null;
    }
    return line;
}

export function addImageToSection(reportObject, section, expectedname, left, top, width, height) {
    const name= expectedname ?? getNewObjectName("Image");
    let image = {
            name: name,
            defaultName:name,
            source:"Embedded",
            value:null,
            left: pxToInch(left||0)+"in",
            top: pxToInch(top||0)+"in",
            height: pxToInch(height||0)+"in",
            width: pxToInch(width||0)+"in",
            zIndex:1,
            sizing: "FitProportional",
            style: getDefaultImageStyle()
        };
    if(section==="body"){
        reportObject.body.reportItems.image=[...reportObject.body.reportItems.image,image];
    } else if(section==="header") {
        reportObject.page.pageHeader.reportItems.image=[...reportObject.page.pageHeader.reportItems.image,image];
    } else if(section==="footer"){
        reportObject.page.pageFooter.reportItems.image=[...reportObject.page.pageFooter.reportItems.image,image];
    } else {
        return null;
    }
    return image;
}

export function getEmbeddedImageData(report, name){
    debugger;
    const image=report.embeddedImages.embeddedImage.find(i=>i.name===name);
    if(image){
        return `data:${image.mimeType};base64,${image.imageData}`;
    }
    return '';
}

/*
    checks
*/
// parameters used
export function isParameterUsed(report, name) {
    //find datasets where parameter can be used
    const queries = findObject(report, "commandText");
    for (let index = 0; index < queries.length; index++) {
        const q = queries[index];
        const m = [...q.matchAll(/@(\w+)/gm)]
                .map(i=> i[1])
                .filter(i=>i===name);
        if(m && m.length>0){
            return true;
        }
        
    }
    

    //find texboxes
    const textboxes = findObject(report, "textbox").flat();
    for (let index = 0; index < textboxes.length; index++) {
        const t = textboxes[index];
        if(t &&
            t.paragraphs &&
            t.paragraphs.paragraph &&
            t.paragraphs.paragraph.textRuns &&
            t.paragraphs.paragraph.textRuns.textRun &&
            t.paragraphs.paragraph.textRuns.textRun.value &&
            t.paragraphs.paragraph.textRuns.textRun.value.startsWith("=")){

            const v=t.paragraphs.paragraph.textRuns.textRun.value;
            const m = [...v.matchAll(/(?:Parameters!)(\w+)(?:\.Value)/gm)]
                    .map(i=> i[1])
                    .filter(i=>i===name);
            if(m && m.length>0){
                return true;
            }
        }
    }
    

    //find filters and return its values only where the parameter is used
    const filters = findObject(report, "filter").flat()
                    .map(i=> i.filterValues.filterValue)
                    .flat();

    for (let index = 0; index < filters.length; index++) {
        const f = filters[index];
        if(f && f.startsWith("=")){
            const m = [...f.matchAll(/(?:Parameters!)(\w+)(?:\.Value)/gm)]
                    .map(i=> i[1])
                    .filter(i=>i===name);
            if(m && m.length>0){
                return true;
            }
        }
    }
    
    return false;
}

// recursive object find function by property name (type)
function findObject(root, type){
    let found=[];
    if(!root) return found;
    for(const key in root){
        if(key===type){
            found =[...found, root[key]];
        }
        else if(Array.isArray(root[key]) && root[key].length>0) {
            for(const el of root[key]){
                found =[...found,... findObject(el, type)];
            }
        }
        if(typeof root[key]==="object"){
            found =[...found,... findObject(root[key], type)];
        }
    }
    return found;
}


function getDefaultImageStyle(){
    return {
        border: {
            style: "None",
            color: null,
            width: null
        }
    }
}
function getDefaultLineStyle(){
    return {
        border: {
            style: "Solid",
            color: "#000000",
            width: "1pt"
        }
    }
}
function getDefaultTextboxStyle(){
    return {
        border: {
            style: "None",
            color: null,
            width: null
        },
        
        paddingLeft:"2pt",
        paddingRight:"2pt",
        paddingTop:"2pt",
        paddingBottom:"2pt",
        backgroundColor: "white",
        verticalAlign: "Default",
    }
}
function getDefaultCellStyle(){
    return {
        border: {
            style: "Solid",
            color: "#000000",
            width: "1pt"
        },
        color: null,
        backgroundColor: null,
        textAlign: null,
        verticalAlign: null,
        textDecoration:null,
    }
}
/*
    returns : {
        type: "",
        object: the object
    }
    or 
    null if not found
*/
export function findObjectByName(reportObject, name){
    if(!reportObject) return null;
    let obj=findObjectInSectionsByName(reportObject.body, name);
    if(!obj) obj=findObjectInPageByName(reportObject.page, name);
    return obj;
}
function findObjectInSectionsByName(rptobj, name){
    if(!rptobj) return null;
    let obj=null;
    if(rptobj.reportItems.tablix.length>0){
        const index=rptobj.reportItems.tablix.findIndex(i=>i.name===name);
        if(index>=0){
            return {
                type: "tablix",
                object: rptobj.reportItems.tablix[index],
                section: rptobj.reportItems.tablix,
                index: index
            };
        }
        
        // find textbox in tablixes
        for(let ti=0; ti<rptobj.reportItems.tablix.length && rptobj.reportItems.tablix.length>0; ti++){
            let t=rptobj.reportItems.tablix[ti];
            let rows=t.tablixBody.tablixRows.tablixRow;
            for(let ri=0; ri<rows.length && rows.length>0; ri++){
                let r=rows[ri];
                const cells = r.tablixCells.tablixCell;
                let cell=cells.find(i=>i.cellContents.textbox!==undefined && i.cellContents.textbox.name===name);
                if(cell){
                    return {
                        type: "textbox",
                        object: cell.cellContents.textbox,
                        section: cell.cellContents
                    };
                }
            }
        }
    }
    if(rptobj.reportItems.textbox.length>0){
        const index=rptobj.reportItems.textbox.findIndex(i=>i.name===name);
        if(index>=0)
            return {
                type: "textbox",
                object: rptobj.reportItems.textbox[index],
                section: rptobj.reportItems.textbox,
                index: index
            };
    }
    if(rptobj.reportItems.line.length>0){
        const index=rptobj.reportItems.line.findIndex(i=>i.name===name);
        if(index>=0)
            return {
                type: "line",
                object: rptobj.reportItems.line[index],
                section: rptobj.reportItems.line,
                index: index
            };
    }
    if(rptobj.reportItems.rectangle.length>0){
        const index=rptobj.reportItems.rectangle.findIndex(i=>i.name===name);
        if(index>=0)
            return {
                type: "rectangle",
                object: rptobj.reportItems.rectangle[index],
                section: rptobj.reportItems.rectangle,
                index: index
            };
    }
    if(rptobj.reportItems.image.length>0){
        const index=rptobj.reportItems.image.findIndex(i=>i.name===name);
        if(index>=0)
            return {
                type: "image",
                object: rptobj.reportItems.image[index],
                section: rptobj.reportItems.image,
                index: index
            };
    }
    return null;
}
function findObjectInPageByName(page, name){
    if(!page) return null;
    let obj=findObjectInSectionsByName(page.pageHeader, name);
    if(!obj) obj=findObjectInSectionsByName(page.pageFooter, name);
    return obj;
}

export function removeObjectByName(reportObject, name){
    if(!reportObject) return false;
    let obj=findObjectInSectionsByName(reportObject.body, name);
    if(!obj) obj=findObjectInPageByName(reportObject.page, name);
    if(obj){
        debugger;
        if(obj.index===undefined){ // textbox in cells
            obj.section={}; 
            return true;
        }
        else {
            debugger;
            obj.section.splice(obj.index,1);
            return true;
        }
    }
    return false;
}

// inch to pixels
export function inToPixels (val){
    let m = (val+"").match(/(\d+(.\d+)?)in/);
    if(m){
        const pixel = parseFloat(m[1]) * 96;
        return Math.round(pixel);
    }
    else {
        const pixel = parseFloat(val) * 96;
        if(!isNaN(pixel))
            return Math.round(pixel);
        return 0;
    }
}

// centimeter to pixels
export function cmToPixels (cm){
    let m = (cm+"").match(/(\d+(.\d+)?)cm/);
    if(m){
        const pixel = parseFloat(m[1]) * 37.7952755906;
        return Math.round(pixel);
    }
    else {
        const pixel = parseFloat(cm)* 37.7952755906;
        if(!isNaN(pixel))
            return Math.round(pixel);
        return 0;
    }
}
/*
  pixel to inch
*/
export function pxToInch (val){
    let m = (val+"").match(/(\d+(.\d+)?)px/);
    if(m){
        const inch = parseFloat(m[1]) * 0.0104166667;
        return inch;
    }
    else {
        const inch = parseFloat(val)* 0.0104166667;
        if(!isNaN(inch))
            return inch;
        return 0;
    }
}
/*
  pixel to centimeter
*/
export function pxToCentimeter (val){
    let m = (val+"").match(/(\d+(.\d+)?)px/);
    if(m){
        const cm = parseFloat(m[1]) * 0.0264583333;
        return cm;
    }
    else {
        const cm = parseFloat(val)* 0.0264583333;
        if(!isNaN(cm))
            return cm;
        return 0;
    }
}



export function px2inFormat(px){
    const v = pxToInch(px);
    return `${v.toFixed(4)}in`; 
}
export function px2cmFormat(px){
    const v = pxToCentimeter(px);
    return `${v.toFixed(3)}cm`; 
}

/*
Tools
*/

export function getFieldType(dataField, type){
    if(!dataField) return '';
    const a={
        "System.String": {d:"Text", n: "String", html: `<svg class="w-5 h-5" viewBox="0 0 24 24">
        <path fill="currentColor" d="M6,11A2,2 0 0,1 8,13V17H4A2,2 0 0,1 2,15V13A2,2 0 0,1 4,11H6M4,13V15H6V13H4M20,13V15H22V17H20A2,2 0 0,1 18,15V13A2,2 0 0,1 20,11H22V13H20M12,7V11H14A2,2 0 0,1 16,13V15A2,2 0 0,1 14,17H12A2,2 0 0,1 10,15V7H12M12,15H14V13H12V15Z" />
    </svg>`},
        "System.Guid": {d:"Text", n: "String", html:`<svg class="w-5 h-5" viewBox="0 0 24 24">
        <path fill="currentColor" d="M6,11A2,2 0 0,1 8,13V17H4A2,2 0 0,1 2,15V13A2,2 0 0,1 4,11H6M4,13V15H6V13H4M20,13V15H22V17H20A2,2 0 0,1 18,15V13A2,2 0 0,1 20,11H22V13H20M12,7V11H14A2,2 0 0,1 16,13V15A2,2 0 0,1 14,17H12A2,2 0 0,1 10,15V7H12M12,15H14V13H12V15Z" />
    </svg>`},
        "System.DateTime": {d:"Date/Time", n: "DateTime", html: `<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
      </svg>`},
        "System.Decimal": {d:"Float", n:"Decimal", html:`<svg class="w-5 h-5" viewBox="0 0 24 24">
        <path fill="currentColor" d="M4,17V9H2V7H6V17H4M22,15C22,16.11 21.1,17 20,17H16V15H20V13H18V11H20V9H16V7H20A2,2 0 0,1 22,9V10.5A1.5,1.5 0 0,1 20.5,12A1.5,1.5 0 0,1 22,13.5V15M14,15V17H8V13C8,11.89 8.9,11 10,11H12V9H8V7H12A2,2 0 0,1 14,9V11C14,12.11 13.1,13 12,13H10V15H14Z" />
    </svg>`},
        "System.Int32": {d:"Integer", n:"Int32", html:`<svg class="w-5 h-5" viewBox="0 0 24 24">
        <path fill="currentColor" d="M4,17V9H2V7H6V17H4M22,15C22,16.11 21.1,17 20,17H16V15H20V13H18V11H20V9H16V7H20A2,2 0 0,1 22,9V10.5A1.5,1.5 0 0,1 20.5,12A1.5,1.5 0 0,1 22,13.5V15M14,15V17H8V13C8,11.89 8.9,11 10,11H12V9H8V7H12A2,2 0 0,1 14,9V11C14,12.11 13.1,13 12,13H10V15H14Z" />
    </svg>`},
        "System.Int64": {d:"Integer", n:"Int64", html:`<svg class="w-5 h-5" viewBox="0 0 24 24">
        <path fill="currentColor" d="M4,17V9H2V7H6V17H4M22,15C22,16.11 21.1,17 20,17H16V15H20V13H18V11H20V9H16V7H20A2,2 0 0,1 22,9V10.5A1.5,1.5 0 0,1 20.5,12A1.5,1.5 0 0,1 22,13.5V15M14,15V17H8V13C8,11.89 8.9,11 10,11H12V9H8V7H12A2,2 0 0,1 14,9V11C14,12.11 13.1,13 12,13H10V15H14Z" />
    </svg>`},
        "System.Int16": {d:"Integer", n:"Int16", html:`<svg class="w-5 h-5" viewBox="0 0 24 24">
        <path fill="currentColor" d="M4,17V9H2V7H6V17H4M22,15C22,16.11 21.1,17 20,17H16V15H20V13H18V11H20V9H16V7H20A2,2 0 0,1 22,9V10.5A1.5,1.5 0 0,1 20.5,12A1.5,1.5 0 0,1 22,13.5V15M14,15V17H8V13C8,11.89 8.9,11 10,11H12V9H8V7H12A2,2 0 0,1 14,9V11C14,12.11 13.1,13 12,13H10V15H14Z" />
    </svg>`},
        "System.Boolean": {d:"Boolean", n:"Boolean", html:`<svg class="w-5 h-5" viewBox="0 0 24 24">
        <path fill="currentColor" d="M7,10A2,2 0 0,1 9,12A2,2 0 0,1 7,14A2,2 0 0,1 5,12A2,2 0 0,1 7,10M17,7A5,5 0 0,1 22,12A5,5 0 0,1 17,17H7A5,5 0 0,1 2,12A5,5 0 0,1 7,7H17M7,9A3,3 0 0,0 4,12A3,3 0 0,0 7,15H17A3,3 0 0,0 20,12A3,3 0 0,0 17,9H7Z" />
    </svg>`},
    };
    const data=(a[dataField]??{})[type];
    if(data) return data;
    return '';
}

export function getPageSizes() {
    return [
        {name: "A4", height:"11.69in", width:"8.27in" },
        {name: "Letter", height:"11in", width:"8.5in" },
        {name: "Legal", height:"14in", width:"8.5in" },
        {name: "A5", height:"8.27in", width:"5.83in" },
    ];
}

function getTextBoxStyleObject(tb){
    return tb && tb.style;
}
function getTextBoxAlignmentCSS(tb){
    const pa=tb.paragraphs.paragraph.style;
    const align = pa && pa.textAlign || "Default";
    switch(align){
        case "Left": return "text-left";
        case "Right": return "text-right";
        case "Center": return "text-center";
        default: return "";
    }
}
function getTextBoxVerticalAlignmentCSS(tb){
    const align = tb && tb.style.verticalAlign||"";
    switch(align){
        case "Middle": return "align-text-middle";
        case "Top": return "align-text-top";
        case "Bottom": return "align-text-bottom";
        default: return "";
    }
}
function getTextBoxValue(tb) {
    return tb && tb.paragraphs.paragraph.textRuns.textRun.value||"";
}

function getRowCellStyle(row, colIndex){
    if(!row) return '';
    const cellContents = row.tablixCells.tablixCell[colIndex].cellContents;

    const textbox = cellContents.textbox;
    if(!textbox) return null;

    let style = textbox.style;
    
    return style;
}
function getRowCellTextAlign(row, colIndex){
    if(!row) return '';
    const cellContents = row.tablixCells.tablixCell[colIndex].cellContents;

    const tb = cellContents.textbox;
    if(!tb) return '';
    
    const pa=tb.paragraphs.paragraph.style
    const align = pa && pa.textAlign || "Default";
    switch(align){
        case "Left": return "text-left";
        case "Right": return "text-right";
        case "Center": return "text-center";
        default: return "";
    }
}
function getRowCellVerticalAlign(row, colIndex){
    const style = getRowCellStyle(row, colIndex);
    const align = style && style.verticalAlign||"Default";
    switch(align){
        case "Middle": return "align-text-middle";
        case "Top": return "align-text-top";
        case "Bottom": return "align-text-bottom";
        default: return "";
    }
}
function getRowCellFontStyleCSS(row, colIndex){
    if(!row) return '';
    const cellContents = row.tablixCells.tablixCell[colIndex].cellContents;

    const tb = cellContents.textbox;
    if(!tb) return '';
    return getFontStyle(tb); 
}

export function hasTextboxBorderStyle(tb){
    if (!tb) return false;
    let style=0;
    const s=tb.style;
    if(s.border){
        const b=s.border;
        if(b.style && !(b.style=="Default" || b.style=="None")){
            style++;
        }
    }
    if(s.leftBorder){
        const b=s.leftBorder;
        if(b.style && !(b.style=="Default" || b.style=="None")){
            style++;
        }
    }
    if(s.topBorder){
        const b=s.topBorder;
        if(b.style && !(b.style=="Default" || b.style=="None")){
            style++;
        }
    }
    if(s.rightBorder){
        const b=s.rightBorder;
        if(b.style && !(b.style=="Default" || b.style=="None")){
            style++;
        }
    }
    if(s.bottomBorder){
        const b=s.bottomBorder;
        if(b.style && !(b.style=="Default" || b.style=="None")){
            style++;
        }
    }
    return style>0;
}

function getFontStyle(tb) {
    if (!tb) return '';
    let style=[];
    const p=tb.paragraphs.paragraph.textRuns.textRun.style;
    const s=tb.style
    if(!(p.fontFamily===undefined||p.fontFamily===null)){
        let fm=p.fontFamily;
        style=[...style,`font-family:${fm};`]
    }
    if(!(p.fontSize===undefined||p.fontSize===null)){
        let sz=p.fontSize;
        style=[...style,`font-size:${sz};`]
    }
    if(!(p.fontWeight===undefined||p.fontWeight===null)){
        style=[...style,`font-weight:${p.fontWeight};`]
    }
    if(!(p.fontStyle===undefined||p.fontStyle===null)){
        style=[...style,`font-style:${p.fontStyle.toLowerCase()};`]
    }
    if(!(p.textDecoration===undefined||p.textDecoration===null)){
        let dec=p.textDecoration.toLowerCase();
        if(dec=="strikethrough") dec="line-through";
        style=[...style,`text-decoration:${dec};`]
    } 
    if(!(s.paddingLeft===undefined||s.paddingLeft===null)){
        let p=s.paddingLeft;
        style=[...style,`padding-left:${p};`]
    }
    if(!(s.paddingRight===undefined||s.paddingRight===null)){
        let p=s.paddingRight;
        style=[...style,`padding-right:${p};`]
    }
    if(!(s.paddingTop===undefined||s.paddingTop===null)){
        let p=s.paddingTop;
        style=[...style,`padding-top:${p};`]
    }
    if(!(s.paddingBottom===undefined||s.paddingBottom===null)){
        let p=s.paddingBottom;
        style=[...style,`padding-bottom:${p};`]
    }
    if(!(p.color===undefined||p.color===null)){
        let c=p.color;
        style=[...style,`color:${c};`]
    }
    if(!(s.backgroundColor===undefined||s.backgroundColor===null)){
        let c=s.backgroundColor;
        style=[...style,`background-color:${c};`]
    }
    if(s.border){
        const b=s.border;
        if(b.style && b.style!=="Default"){
            let c=b.style.toLowerCase();
            style=[...style,`border-style:${c};`];
            if(b.width && b.width.endsWith("pt")){
                let c=b.width.toLowerCase();
                style=[...style,`border-width:${c};`]
            }
            if(b.color){
                let c=b.color;
                style=[...style,`border-color:${c};`]
            }
        }
    }
    if(s.leftBorder){
        const b=s.leftBorder;
        if(b.style && b.style!=="Default"){
            let c=b.style.toLowerCase();
            style=[...style,`border-left-style:${c};`];
            if(b.width && b.width.endsWith("pt")){
                let c=b.width.toLowerCase();
                style=[...style,`border-left-width:${c};`]
            }
            if(b.color){
                let c=b.color;
                style=[...style,`border-left-color:${c};`]
            }
        }
    }
    if(s.topBorder){
        const b=s.topBorder;
        if(b.style && b.style!=="Default"){
            let c=b.style.toLowerCase();
            style=[...style,`border-top-style:${c};`];
            if(b.width && b.width.endsWith("pt")){
                let c=b.width.toLowerCase();
                style=[...style,`border-top-width:${c};`]
            }
            if(b.color){
                let c=b.color;
                style=[...style,`border-top-color:${c};`]
            }
        }
    }
    if(s.rightBorder){
        const b=s.rightBorder;
        if(b.style && b.style!=="Default"){
            let c=b.style.toLowerCase();
            style=[...style,`border-right-style:${c};`];
            if(b.width && b.width.endsWith("pt")){
                let c=b.width.toLowerCase();
                style=[...style,`border-right-width:${c};`]
            }
            if(b.color){
                let c=b.color;
                style=[...style,`border-right-color:${c};`]
            }
        }
    }
    if(s.bottomBorder){
        const b=s.bottomBorder;
        if(b.style && b.style!=="Default"){
            let c=b.style.toLowerCase();
            style=[...style,`border-bottom-style:${c};`];

            if(b.width && b.width.endsWith("pt")){
                let c=b.width.toLowerCase();
                style=[...style,`border-bottom-width:${c};`]
            }
            if(b.color){
                let c=b.color;
                style=[...style,`border-bottom-color:${c};`]
            }
        }
    }
    return style.join(" ");   
}

function getLineBorderWidth(line){
    if(!line) return 0;
    return parseInt(line.style.border.width.match(/(\d+)/)[1]);
} 
function getLineStyle(line){
    if(!line) return '';
    let width = getLineBorderWidth(line);
    if(width>1) width=1+(width*0.15);
    switch(line.style.border.style){
        case "Dotted":
            return `${width*2},${width*2}`;
        case "Dashed":
            return `${width*5},${width*5}`;
    }
    return '';
}
function getLineColor(line){
    if(!line) return 'black';
    return line.style.border.color||'black';
}

export function getImageStyle(img) {
    const st=img && img.style;
    let style=[];
    if(!(st.border.style===undefined||st.border.style===null)){
        switch(st.border.style){
            case "Solid":
                style=[...style,`border-style:solid;`]
                break;
            case "Dotted":
                style=[...style,`border-style:dotted;`]
                break;
            case "Dashed":
                style=[...style,`border-style:dashed;`]
                break;
        }
    }
    if(!(st.border.width===undefined||st.border.width===null) 
        && ["Solid","Dotted","Dashed","Default"].indexOf(st.border.style)>=0){
        let fm=st.border.width;
        style=[...style,`border-width:${fm};`]
    }
    
    if(!(st.border.color===undefined||st.border.color===null)){
        let c=st.border.color;
        style=[...style,`border-color:${c};`]
    }
    
    return style.join(" ");   
}

export const line={
    strokeWidth: getLineBorderWidth,
    strokeStyle: getLineStyle,
    stroke: getLineColor
};

export const textbox = {
    css:{
        textAlign: getTextBoxAlignmentCSS,
        verticalAlign: getTextBoxVerticalAlignmentCSS
    },
    style: getTextBoxStyleObject,
    text: getTextBoxValue,
    cssFontStyle:getFontStyle 
};
export const row = {
    cell:{
        css:{
            textAlign: getRowCellTextAlign,
            verticalAlign: getRowCellVerticalAlign
        },
        style: getRowCellStyle,
        cssFontStyle:getRowCellFontStyleCSS
    }
};

function currencyFormat(num, dec, currency="$") {
    return currency + num.toFixed(dec).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}
function currencyMinusClosedFormat(v, dec , currency="$") {
    let num = Math.abs(v);
    let c = v<0?true:false;
    const x = currency + num.toFixed(dec).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    return c?`(${x})`:x;
}
function percentageFormat(num, dec){
    return (num*100).toFixed(dec)+'%';
}

export const formatter={
    format: (value, formatstring)=>{
        switch(formatstring){
            case "n": return currencyFormat(value,0,"");
            case "n1": return currencyFormat(value,1,"");
            case "n2": return currencyFormat(value,2,"");
            case "n3": return currencyFormat(value,3,"");
            case "'$'#,0,;('$'#,0,)": return currencyMinusClosedFormat(value,0,"$");
            case "'$'#,0,.00;('$'#,0,.00)": return currencyMinusClosedFormat(value,2,"$");
            case "'$'#,0,": return currencyFormat(value,0,"$");
            case "'$'#,0,.00": return currencyFormat(value,2,"$");
            case "0.0%": return percentageFormat(value,1);
            case "0.00%": return percentageFormat(value,2);
            case "0.000%": return percentageFormat(value,3);
        }
    },
    number: {
        n0: (v) => currencyFormat(v,0,""),
        n1: (v) => currencyFormat(v,1,""),
        n2: (v) => currencyFormat(v,2,""),
        n3: (v) => currencyFormat(v,3,""),
        currency: (value,dec,currencysymbol) => currencyFormat(value,dec,currencysymbol),
        currencyNegative: (value,dec,currencysymbol) => currencyMinusClosedFormat(value,dec,currencysymbol),
        percentage:(value, dec) => percentageFormat(value, dec)
    }
} 

function info(message) {
    Swal.fire({
        title: "Info",
        html: message,
        buttonsStyling: false,
        confirmButtonClass: "btn bg-blue-500 text-white focus:outline-none",
        icon: "info"
        });
}
function warn(message) {
    Swal.fire({
        title: "Warning",
        html: message,
        buttonsStyling: false,
        confirmButtonClass: "btn bg-orange-400 text-white focus:outline-none",
        icon: "warning"
        });
}
function error(message) {
    Swal.fire({
        title: "Error",
        html: message,
        buttonsStyling: false,
        confirmButtonClass: "btn bg-red-400 text-white focus:outline-none",
        icon: "error"
        });
}
export const message={
    alert:alert,
    warn:warn,
    error:error
};