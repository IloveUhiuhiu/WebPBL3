const tabs = document.querySelectorAll('.nav-item');
const currentPath = window.location.pathname;
tabs.forEach(tab => {
    const tabLink = tab.querySelector('a');
    if (tabLink && tabLink.getAttribute('href') === currentPath) {
        tab.classList.add('active');
    } else {
        tab.classList.remove('active');
    }
});