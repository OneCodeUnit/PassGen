(function () {
    const THEME_KEY = 'passgen-theme';

    function readTheme() {
        try {
            const value = localStorage.getItem(THEME_KEY);
            return value === 'light' || value === 'dark' ? value : 'auto';
        } catch {
            return 'auto';
        }
    }

    function applyTheme(theme) {
        const root = document.documentElement;
        if (theme === 'light' || theme === 'dark') root.setAttribute('data-theme', theme);
        else root.removeAttribute('data-theme');
    }

    applyTheme(readTheme());

    window.passgen = {
        getTheme: readTheme,

        setTheme(theme) {
            applyTheme(theme);
            try {
                if (theme === 'auto') localStorage.removeItem(THEME_KEY);
                else localStorage.setItem(THEME_KEY, theme);
            } catch { /* хранилище недоступно — тема действует до перезагрузки */ }
        },

        async copy(text) {
            try {
                if (navigator.clipboard && window.isSecureContext) {
                    await navigator.clipboard.writeText(text);
                    return true;
                }
            } catch { /* пробуем запасной способ */ }

            try {
                const area = document.createElement('textarea');
                area.value = text;
                area.setAttribute('readonly', '');
                area.style.cssText = 'position:fixed;top:0;left:0;opacity:0';
                document.body.appendChild(area);
                area.select();
                area.setSelectionRange(0, text.length);
                const ok = document.execCommand('copy');
                area.remove();
                return ok;
            } catch {
                return false;
            }
        }
    };
})();
