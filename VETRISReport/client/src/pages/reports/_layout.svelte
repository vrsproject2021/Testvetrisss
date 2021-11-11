<script>
    import { goto, url } from "@roxi/routify";
    import Dialog from "../../components/Dialog.svelte";
    import * as report from "../../model/report/report.js"; 
    import {user} from "../../model/user";

    import {apiendpoint, basepath} from "../../store.js";  
    
    $: userInfo = JSON.parse(localStorage.getItem("user")||"{}").name||null;
    $: inIframe = !!window.parent.document.getElementById("reportserver");
    let dialog=false;
    let reportname="";
    let userinfomenuopen=false;

    report.init($apiendpoint);

    //export let selected_path;
    function onNewReport(){
        dialog=true;
    }
    async function newreport(){
      if(reportname!==""){
        await report.createReport(reportname,'Drafts')
              .then(result=>{
                $goto(`/designer/${result}`);
              })
              .catch(e=>{
                  debugger;
              }); 
      }
          
    }
    const logout=()=>{
      localStorage.removeItem("user");
      $goto('/login');
    };
    const tabularreport=()=>{
      $goto('./tabular');
    };
    const gotoreports=()=>{
      $goto('/reports');
    };
    const gotodatasets=()=>{
      $goto('./datasets');
    };

    function closeiframe(){
      let d = window.parent.document;
      let frame = d.getElementById('reportserver');
      let button=frame.closest("div").firstElementChild;
      if(button){
        button.click();
      }
    }

</script>

<div class="flex flex-col text-sm  text-gray-600 min-h-screen overflow-y-hidden">
    <div class="flex items-center justify-between border-b bg-white shadow-sm">
          <div class="flex flex-row items-center">
              <img src="{$basepath}/assets/images/logo.png" alt="" class="h-12 p-1"/>
              <div class="p-2">
                  <h1>Report Server</h1>
              </div>
          </div>
          <div class="flex flex-none items-center px-2 mr-1 hover:bg-gray-50" on:click={(e)=>userinfomenuopen=!userinfomenuopen}>
              <div class="p-2">{userInfo}</div>
              <div class="ml-2"><img src="{$basepath}/assets/images/user.png" alt="" class="h-8 rounded-full border"/></div>
              <div class="text-gray-500 h-4 w-4" >
                <svg class="h-4 w-4" class:hidden={userinfomenuopen} xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                </svg>
                <svg class="h-4 w-4" class:hidden={!userinfomenuopen} xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 15l7-7 7 7" />
                </svg>
              </div>
          </div>
          <div class="fixed bg-black opacity-0 top-0 left-0 right-0 bottom-0 z-20" class:hidden={!userinfomenuopen} on:click={(e)=>userinfomenuopen=false}></div>
          <div class="fixed right-0 top-12  mr-1 p-2 bg-white border rounded-sm shadow-md z-30" class:hidden={!userinfomenuopen}>
            <ul>
              {#if !inIframe}
                <li on:click|preventDefault={logout}>Logout {userInfo}</li>
              {:else}
                <li on:click|preventDefault={closeiframe}>Close</li>
              {/if}
            </ul>
          </div>
      </div>
    <div class="flex-auto flex flex-row">
      <div class="w-14 flex flex-col items-center justify-between bg-white">
        <div class="flex flex-col">
          <a href="#" class="text-xs text-center text-gray-600 hover:bg-gray-200 pb-2 " 
                on:click|preventDefault={gotodatasets}>
              <div class="w-14 h-14 p-1.5 align-middle">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 12h14M5 12a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v4a2 2 0 01-2 2M5 12a2 2 0 00-2 2v4a2 2 0 002 2h14a2 2 0 002-2v-4a2 2 0 00-2-2m-2-4h.01M17 16h.01" />
                </svg>
              </div>
              Tabular Datasets
          </a>
           
          <a href="#" class="text-xs text-center text-gray-600 hover:bg-gray-200 pb-2 border-b" 
                on:click|preventDefault={tabularreport}>
              <div class="w-14 h-14 p-1.5 align-middle">
                <svg  viewBox="0 0 24 24">
                  <path fill="currentColor" d="M10,4V8H14V4H10M16,4V8H20V4H16M16,10V14H20V10H16M16,16V20H20V16H16M14,20V16H10V20H14M8,20V16H4V20H8M8,14V10H4V14H8M8,8V4H4V8H8M10,14H14V10H10V14M4,2H20A2,2 0 0,1 22,4V20A2,2 0 0,1 20,22H4C2.92,22 2,21.1 2,20V4A2,2 0 0,1 4,2Z" />
                </svg>
              </div>
            Tablulars
          </a>
          <a href="#" class="text-xs text-center text-gray-600 hover:bg-gray-200 pb-2" on:click|preventDefault={gotoreports}>
            <div class="w-14 h-14 p-1.5 align-middle">
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M12 7a1 1 0 110-2h5a1 1 0 011 1v5a1 1 0 11-2 0V8.414l-4.293 4.293a1 1 0 01-1.414 0L8 10.414l-4.293 4.293a1 1 0 01-1.414-1.414l5-5a1 1 0 011.414 0L11 10.586 14.586 7H12z" clip-rule="evenodd" />
              </svg>
            </div>
            Reports
          </a>
          <a href="#" class="text-blue-600 hover:bg-gray-200 pb-2"
               on:click|preventDefault={onNewReport}>
              <div class="w-14 h-14 p-1.5 align-middle">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v3m0 0v3m0-3h3m-3 0H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
            </a>
        </div>
        <!-- <div class="flex flex-col">
          <a href="#">
            <div class="w-12 h-12 text-gray-600 hover:bg-gray-300 p-3">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
              </svg>
            </div>
          </a>
          <a href="#">
            <div class="w-12 h-12 text-gray-600 hover:bg-gray-300 p-3">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
          </a>
        </div> -->
      </div>
      <slot></slot>
    </div>
    <div class="h-10 text-sm text-gray-600 bg-white flex items-center justify-between border-t shadow-xl p-2">
          <div class="flex items-center">Ready.</div>
          <div class="flex items-center text-xs">Copyright &COPY; 2021 vetchoice.com. All rights reserved.</div>
      </div>
  </div>
  
  <Dialog bind:visible={dialog} title="Create new report">
    <div class="px-2 py-2  pb-4">
        <label for="reportname"  class="block font-semibold">Let's start with a name for your report.</label>
        <input type="text" bind:value={reportname} id="reportname" placeholder="Report name" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-sm"/>
        <p class="text-xs py-4">All reports should be saved as drafts to continue where you left off. Publishing a report must be done manually.</p>
    </div>
    <div class="flex flex-row items-center justify-end mb-2">
      <button class="p-2 mx-2 bg-blue-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={newreport}>OK</button>
      <button class="p-2 mx-2 bg-red-500 text-white rounded-sm w-16 hover:outline-none focus:outline-none" on:click|preventDefault={(e) => dialog=false}>Cancel</button>
    </div>
  </Dialog>
  