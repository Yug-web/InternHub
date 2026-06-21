/* ============================================================
   InternHub — site.js
   Active nav highlighting, stat counter animation, UX helpers
   ============================================================ */

document.addEventListener('DOMContentLoaded', function () {

    // ── Active Nav Link Highlighting ────────────────────────────
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.nav-pill');

    navLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (!href) return;
        const linkPath = href.toLowerCase();

        const isActive =
            (linkPath === '/' && currentPath === '/') ||
            (linkPath !== '/' && currentPath.startsWith(linkPath));

        if (isActive) link.classList.add('active');
    });

    // ── Stat Counter Animation ───────────────────────────────────
    const counters = document.querySelectorAll('.stat-value[data-target]');
    counters.forEach(counter => {
        const target = parseInt(counter.getAttribute('data-target'), 10);
        const duration = 900;
        const stepTime = 16;
        const steps = Math.ceil(duration / stepTime);
        const increment = target / steps;
        let current = 0;

        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                counter.textContent = target;
                clearInterval(timer);
            } else {
                counter.textContent = Math.floor(current);
            }
        }, stepTime);
    });

    // ── Delete Confirmation ──────────────────────────────────────
    const deleteForms = document.querySelectorAll('form[data-confirm]');
    deleteForms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const msg = this.dataset.confirm || 'Are you sure you want to delete this record?';
            if (!confirm(msg)) e.preventDefault();
        });
    });

    // ── Auto-dismiss Bootstrap toasts ───────────────────────────
    const toastElements = document.querySelectorAll('.toast.show');
    toastElements.forEach(el => {
        setTimeout(() => {
            el.classList.remove('show');
            setTimeout(() => el.style.display = 'none', 300);
        }, 4000);
    });

    // ── Form: prevent double submit ─────────────────────────────
    const forms = document.querySelectorAll('form:not([data-no-disable])');
    forms.forEach(form => {
        form.addEventListener('submit', function () {
            const btn = this.querySelector('button[type="submit"]');
            if (btn) {
                setTimeout(() => {
                    btn.disabled = true;
                    btn.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span>Saving…';
                }, 10);
            }
        });
    });

    // ── Highlight table row on click (mobile UX) ────────────────
    const tableRows = document.querySelectorAll('.table-dark-custom tbody tr');
    tableRows.forEach(row => {
        row.style.cursor = 'default';
    });
});
