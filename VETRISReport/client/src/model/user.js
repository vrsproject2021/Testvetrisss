import { writable } from 'svelte/store';

export const user = writable(false);

user.isAuthenticated=()=> {
    const detail =JSON.parse(localStorage.getItem("user")||null);
    if(detail)
        user.set((detail.accessToken||null)!==null);
    return detail && detail.accessToken!==undefined;
};

user.token=()=> {
    const detail =JSON.parse(localStorage.getItem("user")||null);
    return (detail && detail.accessToken)||null;
}; 

user.login = (detail) => {
    localStorage.setItem("user", JSON.stringify(detail));
    user.set(!!detail.accessToken);
};

user.logout = ()=> {
    localStorage.removeItem('user')
    user.set(false);
};



