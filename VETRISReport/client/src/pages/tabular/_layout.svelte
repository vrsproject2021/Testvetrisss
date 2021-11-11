<script>
    import { goto, url } from "@roxi/routify";
    import Dialog from "../../components/Dialog.svelte";

    import {user} from "../../model/user";

    import {basepath} from "../../store.js";  
    
    $: userInfo = JSON.parse(localStorage.getItem("user")||"{}").name||null;
    $: inIframe = !!window.parent.document.getElementById("reportserver");
    let dialog=false;
    let reportname="";
    let userinfomenuopen=false;
    //export let selected_path;
    function onNewReport(){
        dialog=true;
    }
    const newreport=()=>{
      if(reportname!=="")
          $goto('/designer');
    };
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
    
    <slot></slot>
    
    <div class="h-10 text-sm text-gray-600 bg-white flex items-center justify-between border-t shadow-xl p-2">
          <div class="flex items-center">Ready.</div>
          <div class="flex items-center text-xs">Copyright &COPY; 2021 vetchoice.com. All rights reserved.</div>
      </div>
  </div>
  