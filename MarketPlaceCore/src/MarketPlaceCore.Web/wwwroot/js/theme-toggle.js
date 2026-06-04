const themeToggleBtn = document.getElementById('theme-toggle');
const themeIcon = document.getElementById('theme-icon');

const sunIcon = '<path d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364-6.364l-.707.707M6.343 17.657l-.707.707m12.02 0l-.707-.707M6.343 6.344l-.707-.707M12 5a7 7 0 100 14 7 7 0 000-14z"></path>';
const moonIcon = '<path d="M21 12.79A9 9 0 1111.21 3 7 7 0 0021 12.79z"></path>';

function updateIcon() {
    if (document.documentElement.classList.contains('dark')) {
        themeIcon.innerHTML = sunIcon;
    } else {
        themeIcon.innerHTML = moonIcon;
    }
}

function toggleTheme() {
    if (document.documentElement.classList.contains('dark')) {
        document.documentElement.classList.replace('dark', 'light');
        localStorage.setItem('theme', 'light');
    } else {
        document.documentElement.classList.replace('light', 'dark');
        localStorage.setItem('theme', 'dark');
    }
    updateIcon();
}

themeToggleBtn.addEventListener('click', toggleTheme);

(function() {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark' || (!savedTheme && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
        document.documentElement.classList.replace('light', 'dark');
    } else {
        document.documentElement.classList.replace('dark', 'light');
    }
    updateIcon();
})();
