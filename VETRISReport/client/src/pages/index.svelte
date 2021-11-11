<script>
	import { goto } from '@roxi/routify';
	import { user } from "../model/user";

	let name = "world";
	$: userInfo = JSON.parse(localStorage.getItem("user")||'{}').name||null;
	
	if(!user.isAuthenticated()){
		$goto('/login');
	} else
		$goto('/reports');
</script>

<main>
	
	<h1>Hello {userInfo}!</h1>
	
	<p>Welcome to VETRIS Reports Server.</p>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>
