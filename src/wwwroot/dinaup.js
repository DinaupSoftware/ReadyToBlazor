window.setCookie = (name, value, hours) => {
    let expires = "";
    if (hours && hours > 0) {
        const date = new Date();
        date.setTime(date.getTime() + hours * 60 * 60 * 1000);
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
};



window.removeCookie = (name) => {
    document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
};
