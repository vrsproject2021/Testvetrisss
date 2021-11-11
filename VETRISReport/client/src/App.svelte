<script>
	import { Router } from "@roxi/routify";
  	import { routes } from "../.routify/routes";
	import {apiendpoint, basepath, vetris} from "./store.js";  
	//import moment from 'moment-timezone';
	export let name; 
	$: loaded=false;

	function test_time(){
		
	}
	test_time();
	async function loadConfig() {
		
		try {
			const res = await fetch(`/assets/config.json?t=`+(+new Date()), {
				method: 'GET'
			});
			const json = await res.json();
			apiendpoint.set(json.apiendpoint);
			basepath.set(json.basepath||"");
			vetris.set(json.vetris||"");
			await getLogin(()=>loaded=true);
		} catch {
			let path = (location.pathname||"");
			let m=path.match(/(\/\w+)/);
			if(m){
				path=m[1];
			}
			const res = await fetch(`${path}/assets/config.json?t=`+(+new Date()), {
				method: 'GET'
			});
			const json = await res.json();
			apiendpoint.set(json.apiendpoint);
			basepath.set(json.basepath||"");
			vetris.set(json.vetris||"");
			await getLogin(()=>{
				loaded=true
			});
		}
		
		
	}

	function getToken() {
    	const urlParams = new URLSearchParams(location.search);
		let token=null;
		if (urlParams.has("token")) {
			token = encodeURI(urlParams.get("token"));
			if(!token.match('^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$'))
				token=null;
		} 

		return token;
	}
	
	async function getLogin(callback){
		const token=getToken();
		if(token){
			localStorage.removeItem("user");
			const res = await fetch(`${$apiendpoint}/api/user/loginstatus?userId=${encodeURI(token)}`, {
				method: 'GET'
			});
			
			const json = await res.json();
			if(json.isError==false){
				localStorage.setItem("user", JSON.stringify(json.result));
				callback();
			}
			else{
				callback();
			}
		}
		else {
			callback();
		}
	}

	loadConfig();
	const config = {
		urlTransform: {
			apply: url => `${url}`, //external URL
			remove: url => url.replace(`${$basepath}`, ''), //internal URL
		},
    	useHash: true
	}
</script>
{#if loaded}
<Router config={config} {routes} />
{/if}
<style global lang="postcss">
    @tailwind base;
    @tailwind components;
	@tailwind utilities;  
	
	.btn {
		@apply px-4 py-2 rounded;
	}

	
</style>