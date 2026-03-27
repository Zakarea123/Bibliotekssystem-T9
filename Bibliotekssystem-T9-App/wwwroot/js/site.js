// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const btn = document.getElementById('notif-btn');
const panel = document.getElementById('notif-panel');
const badge = document.getElementById('notif-badge');

if (btn) { // Om elementet inte finns på sidan returnerar getElementById null
    if (badge) { // Hämtar antalet olästa notifikationer för att göra en count.
        fetch('/Notifications/UnreadCount/')
            .then(r => r.json())
            .then(count => { badge.textContent = count; });
    }

    btn.addEventListener('click', async () => {
        if (panel.classList.contains('open')) {
            panel.classList.remove('open');
            return;
        }

        const resp = await fetch('/Notifications/Dropdown');
        panel.innerHTML = await resp.text();
        
        // En lösning för att visa rätt tidszon i gränssnittet för notifikationer. 
        panel.querySelectorAll('.notif-time[data-utc]').forEach((el) => {
            const date = new Date(el.dataset.utc + 'Z');
            el.textContent = date.toLocaleDateString('sv-SE', {
                day: '2-digit', month: '2-digit', hour: '2-digit', minute: '2-digit' 
            });
        })
        
        panel.classList.add('open');

        const unread = panel.querySelectorAll('.notif-item.unread').length;
        badge.textContent = unread;
    });

    document.addEventListener('click', e => {
        if (!btn.contains(e.target) && !panel.contains(e.target)) {
            panel.classList.remove('open');
        }
    });
}

async function markRead(id, el) {
    await fetch(`/Notifications/MarkRead?id=${id}`, { method: 'POST' });
    el.closest('.notif-item').classList.remove('unread');
    el.remove();
    const unread = panel.querySelectorAll('.notif-item.unread').length;
    badge.textContent = unread;
}

async function deleteNotif(id, el) {
    await fetch(`/Notifications/Delete?id=${id}`, { method: 'POST' });
    el.closest('.notif-item').remove();
    const count = panel.querySelectorAll('.notif-item').length;
    panel.querySelector('.notif-count').textContent = count;
}

// Toggles the inline date picker form visibility without page refresah
function toggleDatePicker(itemId) {
    const form = document.getElementById(`borrow-form-${itemId}`);
    form.style.display = form.style.display === 'none' ? 'flex' : 'none';
}

const accountBtn = document.getElementById('account-btn');
const accountPanel = document.getElementById('account-panel');

if (accountBtn) {
    accountBtn.addEventListener('click', e => {
        e.stopPropagation();
        accountPanel.classList.toggle('open');
        document.getElementById('notif-panel')?.classList.remove('open');
    });

    document.addEventListener('click', e => {
        if (!accountBtn.contains(e.target) && !accountPanel.contains(e.target)) {
            accountPanel.classList.remove('open');
        }
    });
}

// Filters and hides the items table in real time based on the user's search input.
const itemSearch = document.getElementById('itemSearch');
if (itemSearch) {
    itemSearch.addEventListener('input', function () {
        const query = this.value.toLowerCase();
        document.querySelectorAll('.loans-table tbody tr').forEach(function (row) {
            const text = row.textContent.toLowerCase();
            row.style.display = text.includes(query) ? '' : 'none';
        });
    });
}

// Filters and hides the users table in real time based on the user's search input.
const userSearch = document.getElementById('userSearch');
if (userSearch) {
    userSearch.addEventListener('input', function () {
        const query = this.value.toLowerCase();
        document.querySelectorAll('.users-table tbody tr').forEach(function (row) {
            const text = row.textContent.toLowerCase();
            row.style.display = text.includes(query) ? '' : 'none';
        });
    });
}