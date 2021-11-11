<script>
import { goto, url } from "@roxi/routify";

import {user} from "../model/user";
import {apiendpoint, basepath} from "../store.js";  


let username='';
let password='';
let error="";
$: valid = (username||'').length>0 && (password||'').length>0;


async function onSubmit () {
    const postData = { userId: username, password: password};
    const res = await fetch(`${$apiendpoint}/api/user/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postData)
    });
    
    const json = await res.json();
    if(!json.isError && json.result.success){
        user.login(json.result);
        $goto('/');
    }
    else {
        error="Login failed.";
    }
}



</script>
<div class="min-h-screen bg-gray-100 text-greay-800 antialiased flex flex-col justify-items-center " 
    style="background-image: url('{$basepath}/assets/images/loginBG.jpg')">
    <div class="absolute py-3 sm:w-96 sm:mx-auto right-8 bottom-5 text-center">
        
        <div class="mt-4 bg-white shadow-md rounded-lg text-left">
            <div class="h-2 bg-indigo-400 rounded-t-md"></div>
            <div class="h-4 text-center p-2"><span class ="text-xl font-semibold">Login to VETRIS Report Server</span></div>
            
            <div class="px-8 py-8">
                <label for="username"  class="block font-semibold">Username or Email</label>
                <input type="text" bind:value={username} id="username" placeholder="Email" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-md"/>
                <label for="password" class="block font-semibold">Password</label>
                <input type="password" bind:value={password} id="password" placeholder="Password" class="border w-full h-5 px-3 py-5 mt-2 hover:outline-none focus:outline-none focus:ring-1 focus:ring-indigo-400 rounded-md"/>
                {#if error}
                <div class="text-red-600" >
                    {error}
                </div>
                {/if}
                <div class="flex justify-between items-baseline">
                    <button 
                        disabled='{!valid}'
                        type="submit" 
                        class="btn mt-4 bg-indigo-500 text-white py2 px-6 rounded-sm hover:bg-indigo-600"
                        on:click={onSubmit}>Login</button>
                    <!-- <a href={$url('/forgotpassword')} class="text-sm hover:underline">Forgot password?</a> -->
                </div>
            </div>
            
        </div>
    </div>
</div>
