/* ============================================================
   Transactions UI Module (Brand-New Implementation)
   DO NOT COPY LEGACY WINFORMS EVENT HANDLERS OR SQL STRINGS
   ============================================================ */

const state = {
  page: 1,
  pageSize: 20,               // Mirrors legacy PageSize constant
  sortBy: 'ReceiveDate',
  filters: {
    partId: '',
    user: '',
    building: '',
    dateRangeEnabled: false,
    dateFrom: null,
    dateTo: null
  },
  quickFind: {
    column: '',
    text: ''
  },
  lastResultsCount: 0,
  selected: null,
  isAdmin: false,             // Set via boot payload (server inject or API)
  loading: false,
  batchHistoryOpen: false
};

// API ENDPOINT CONTRACT (Adjust to your backend):
// GET /api/transactions?sortBy=&page=&pageSize=&partId=&user=&building=&dateFrom=&dateTo=
// Quick find fallback: /api/transactions/like?column=&value=&page=&pageSize=
// GET /api/transactions/history?batchNumber= (returns chronological list)
// All endpoints must be parameterized server-side (NO dynamic SQL concatenation in the client).

const q = (sel, ctx = document) => ctx.querySelector(sel);
const qa = (sel, ctx = document) => Array.from(ctx.querySelectorAll(sel));

const els = {
  filtersForm: q('#filtersForm'),
  sortBy: q('#filter_sortBy'),
  partId: q('#filter_partId'),
  user: q('#filter_user'),
  building: q('#filter_building'),
  dateRangeChk: q('#filter_dateRangeEnabled'),
  dateFrom: q('#filter_dateFrom'),
  dateTo: q('#filter_dateTo'),
  quickFindCol: q('#quickFind_column'),
  quickFindText: q('#quickFind_text'),
  searchBtn: q('[data-action="search"]'),
  resetBtn: q('[data-action="resetFilters"]'),
  toggleFiltersBtn: q('[data-action="toggleFilters"]'),
  filtersPanel: q('#filtersPanel'),
  statusRegion: q('[data-status-region]'),
  tableBody: q('[data-tbody]'),
  rowTemplate: q('#transactionRowTemplate'),
  emptyState: q('[data-empty-state]'),
  loadingOverlay: q('[data-loading]'),
  paginationPrev: q('[data-action="prevPage"]'),
  paginationNext: q('[data-action="nextPage"]'),
  pageInfo: q('[data-page-info]'),
  selectionReport: q('[data-selection-report]'),
  historyPanel: q('[data-history-panel]'),
  historyRowsTbody: q('[data-history-rows]'),
  historyRowTemplate: q('#historyRowTemplate'),
  viewHistoryBtn: q('[data-action="viewHistory"]'),
  closeHistoryBtn: q('[data-action="closeHistory"]'),
  printBtn: q('[data-action="print"]')
};

// Boot logic placeholder: Replace with real user/permissions and initial combos
async function boot() {
  // Example: injected via server or separate call
  state.isAdmin = Boolean(document.body.dataset.admin === 'true');

  await loadReferenceData();
  wireEvents();
  restoreFilterPanelState();
  // Optionally auto-run initial search (disabled by default to mimic legacy "must choose a filter")
}

async function loadReferenceData() {
  // Replace with real endpoints:
  // const parts = await fetchJSON('/api/reference/parts');
  // const users = await fetchJSON('/api/reference/users');
  const parts = ['P-100', 'P-200', 'P-300']; // placeholder
  const users = ['alice', 'bob', 'charles'];

  fillSelect(els.partId, parts);
  fillSelect(els.user, users);
  applyAdminState();
}

function fillSelect(selectEl, values) {
  const frag = document.createDocumentFragment();
  values.forEach(v => {
    const opt = document.createElement('option');
    opt.value = v;
    opt.textContent = v;
    frag.appendChild(opt);
  });
  selectEl.appendChild(frag);
}

function applyAdminState() {
  if (!state.isAdmin) {
    els.user.disabled = true;
  } else {
    els.user.disabled = false;
  }
}

function wireEvents() {
  // Search form state watchers
  [els.partId, els.user, els.building, els.quickFindCol, els.quickFindText].forEach(el => {
    el.addEventListener('input', evaluateSearchButton);
    el.addEventListener('change', evaluateSearchButton);
  });

  els.dateRangeChk.addEventListener('change', () => {
    const enabled = els.dateRangeChk.checked;
    els.dateFrom.disabled = !enabled;
    els.dateTo.disabled = !enabled;
    els.dateFrom.parentElement.setAttribute('aria-disabled', String(!enabled));
    evaluateSearchButton();
  });

  els.filtersForm.addEventListener('submit', async e => {
    e.preventDefault();
    state.page = 1;
    captureFilters();
    await runSearch();
  });

  els.resetBtn.addEventListener('click', () => {
    els.filtersForm.reset();
    els.dateFrom.disabled = true;
    els.dateTo.disabled = true;
    els.dateFrom.parentElement.setAttribute('aria-disabled', 'true');
    state.selected = null;
    syncSelectionReport();
    evaluateSearchButton();
  });

  els.sortBy.addEventListener('change', () => {
    state.sortBy = els.sortBy.value;
    // If results already loaded, re-run search (optional)
  });

  els.paginationPrev.addEventListener('click', () => pageMove(-1));
  els.paginationNext.addEventListener('click', () => pageMove(1));

  els.toggleFiltersBtn.addEventListener('click', toggleFiltersPanel);

  els.viewHistoryBtn.addEventListener('click', () => loadHistory());
  els.closeHistoryBtn.addEventListener('click', () => closeHistory());

  els.printBtn.addEventListener('click', () => window.print());

  // Table row selection (event delegation)
  els.tableBody.addEventListener('click', e => {
    const row = e.target.closest('[data-row]');
    if (!row) return;
    const id = row.dataset.id;
    const record = row._record;
    state.selected = record;
    syncSelectionReport();
    highlightSelection(row);
  });

  evaluateSearchButton();
}

function evaluateSearchButton() {
  const quickModeValid = els.quickFindCol.value && els.quickFindText.value.trim();
  const primaryFiltersChosen =
    Boolean(els.partId.value) ||
    Boolean(els.user.value) ||
    Boolean(els.building.value) ||
    (els.dateRangeChk.checked && els.dateFrom.value && els.dateTo.value);

  els.searchBtn.disabled = !(quickModeValid || primaryFiltersChosen);
}

function captureFilters() {
  state.sortBy = els.sortBy.value;
  state.filters.partId = els.partId.value;
  state.filters.user = els.user.value;
  state.filters.building = els.building.value;
  state.filters.dateRangeEnabled = els.dateRangeChk.checked;
  state.filters.dateFrom = els.dateRangeChk.checked ? els.dateFrom.value || null : null;
  state.filters.dateTo = els.dateRangeChk.checked ? els.dateTo.value || null : null;
  state.quickFind.column = els.quickFindCol.value;
  state.quickFind.text = els.quickFindText.value.trim();
}

async function runSearch() {
  captureFilters();
  setLoading(true);
  try {
    const data = await fetchTransactions();
    renderResults(data);
    updatePagingControls(data.length);
    announce(`${data.length} transactions loaded.`);
  } catch (err) {
    console.error(err);
    announce('Failed to load transactions.');
  } finally {
    setLoading(false);
  }
}

async function fetchTransactions() {
  // Build query
  let url;
  if (state.quickFind.column && state.quickFind.text) {
    const params = new URLSearchParams({
      column: state.quickFind.column,
      value: state.quickFind.text,
      page: state.page,
      pageSize: state.pageSize,
      sortBy: state.sortBy
    });
    url = `/api/transactions/like?${params.toString()}`;
  } else {
    const params = new URLSearchParams({
      page: state.page,
      pageSize: state.pageSize,
      sortBy: state.sortBy
    });
    if (state.filters.partId) params.set('partId', state.filters.partId);
    if (state.filters.user) params.set('user', state.filters.user);
    if (state.filters.building) params.set('building', state.filters.building);
    if (state.filters.dateRangeEnabled && state.filters.dateFrom && state.filters.dateTo) {
      params.set('dateFrom', state.filters.dateFrom);
      params.set('dateTo', state.filters.dateTo);
    }
    url = `/api/transactions?${params.toString()}`;
  }

  // Placeholder: simulate fetch
  // const res = await fetch(url, { headers: { Accept: 'application/json' } });
  // if (!res.ok) throw new Error('Network error');
  // return await res.json();

  await delay(300);
  // Simulated dataset
  const simulated = Array.from({ length: Math.floor(Math.random() * 10) }, (_, i) => ({
    ID: i + 1 + (state.page - 1) * state.pageSize,
    TransactionType: ['IN', 'OUT', 'TRANSFER'][i % 3],
    BatchNumber: `BATCH-${Math.floor(Math.random() * 9000 + 1000)}`,
    PartID: ['P-100', 'P-200', 'P-300'][i % 3],
    FromLocation: ['Expo Drive', 'Vits Drive', 'Dock'][i % 3],
    ToLocation: ['Warehouse', 'Line A', 'Line B'][i % 3],
    Operation: ['Receive', 'Move', 'Ship'][i % 3],
    Quantity: Math.floor(Math.random() * 500),
    Notes: i % 2 === 0 ? 'Sample note' : '',
    User: ['alice', 'bob', 'charles'][i % 3],
    ItemType: ['Raw', 'Finished', 'Component'][i % 3],
    ReceiveDate: new Date(Date.now() - i * 86400000).toISOString()
  }));
  return simulated;
}

function renderResults(records) {
  els.tableBody.innerHTML = '';
  state.lastResultsCount = records.length;
  if (!records.length) {
    els.emptyState.hidden = false;
    els.printBtn.disabled = true;
    els.viewHistoryBtn.disabled = true;
    return;
  }
  els.emptyState.hidden = true;
  records.forEach(rec => {
    const row = els.rowTemplate.content.firstElementChild.cloneNode(true);
    row.dataset.id = rec.ID;
    row._record = rec;
    row.querySelector('[data-cell="PartID"]').textContent = rec.PartID || '';
    row.querySelector('[data-cell="Operation"]').textContent = rec.Operation || '';
    row.querySelector('[data-cell="Quantity"]').textContent = rec.Quantity;
    row.querySelector('[data-cell="FromLocation"]').textContent = rec.FromLocation || '';
    row.querySelector('[data-cell="ToLocation"]').textContent = rec.ToLocation || '';
    row.querySelector('[data-cell="ReceiveDate"]').textContent =
      formatDateTime(rec.ReceiveDate);
    els.tableBody.appendChild(row);
  });
  // Reset selection after new render
  state.selected = null;
  syncSelectionReport();
}

function highlightSelection(selectedRow) {
  qa('[data-row]', els.tableBody).forEach(r => r.removeAttribute('aria-selected'));
  if (selectedRow) {
    selectedRow.setAttribute('aria-selected', 'true');
  }
}

function syncSelectionReport() {
  const map = els.selectionReport.querySelectorAll('[data-field]');
  map.forEach(el => {
    const key = el.getAttribute('data-field');
    el.textContent = state.selected ? (state.selected[key] ?? '—') : '—';
  });
  const hasSelection = Boolean(state.selected);
  els.viewHistoryBtn.disabled = !hasSelection;
  els.printBtn.disabled = !hasSelection && state.lastResultsCount === 0;
}

async function loadHistory() {
  if (!state.selected || !state.selected.BatchNumber) return;
  els.historyPanel.hidden = false;
  els.historyRowsTbody.innerHTML = '';
  els.viewHistoryBtn.disabled = true;
  try {
    const history = await fetchHistory(state.selected.BatchNumber);
    renderHistory(history);
  } catch (e) {
    console.error(e);
  } finally {
    els.viewHistoryBtn.disabled = false;
  }
}

async function fetchHistory(batchNumber) {
  // const res = await fetch(`/api/transactions/history?batchNumber=${encodeURIComponent(batchNumber)}`);
  // if (!res.ok) throw new Error('Failed history');
  // return await res.json();
  await delay(250);
  // Simulated oldest last (we'll reverse to show newest first if needed)
  return [
    {
      PartID: state.selected.PartID,
      Quantity: state.selected.Quantity,
      Operation: 'Receive',
      User: 'alice',
      FromLocation: '',
      ToLocation: 'Expo Drive',
      ReceiveDate: new Date(Date.now() - 86400000 * 4).toISOString(),
      Description: 'Initial Transaction'
    },
    {
      PartID: state.selected.PartID,
      Quantity: state.selected.Quantity,
      Operation: 'Move',
      User: 'bob',
      FromLocation: 'Expo Drive',
      ToLocation: 'Warehouse',
      ReceiveDate: new Date(Date.now() - 86400000 * 2).toISOString(),
      Description: 'Part transferred from Expo Drive to Warehouse'
    },
    {
      PartID: state.selected.PartID,
      Quantity: state.selected.Quantity,
      Operation: state.selected.Operation,
      User: state.selected.User,
      FromLocation: 'Warehouse',
      ToLocation: state.selected.ToLocation,
      ReceiveDate: state.selected.ReceiveDate,
      Description: 'Latest Transaction'
    }
  ];
}

function renderHistory(items) {
  els.historyRowsTbody.innerHTML = '';
  items.forEach(h => {
    const row = els.historyRowTemplate.content.firstElementChild.cloneNode(true);
    row.querySelector('[data-cell="PartID"]').textContent = h.PartID || '';
    row.querySelector('[data-cell="Quantity"]').textContent = h.Quantity;
    row.querySelector('[data-cell="Operation"]').textContent = h.Operation || '';
    row.querySelector('[data-cell="User"]').textContent = h.User || '';
    row.querySelector('[data-cell="FromLocation"]').textContent = h.FromLocation || '';
    row.querySelector('[data-cell="ToLocation"]').textContent = h.ToLocation || '';
    row.querySelector('[data-cell="ReceiveDate"]').textContent = formatDateTime(h.ReceiveDate);
    row.querySelector('[data-cell="Description"]').textContent = h.Description || '';
    els.historyRowsTbody.appendChild(row);
  });
}

function closeHistory() {
  els.historyPanel.hidden = true;
  els.historyRowsTbody.innerHTML = '';
}

function updatePagingControls(resultCount) {
  els.paginationPrev.disabled = state.page <= 1;
  // If results returned less than pageSize we assume no further page
  els.paginationNext.disabled = resultCount < state.pageSize;
  els.pageInfo.textContent = `Page ${state.page}`;
}

async function pageMove(delta) {
  state.page = Math.max(1, state.page + delta);
  await runSearch();
}

function setLoading(isLoading) {
  state.loading = isLoading;
  els.loadingOverlay.hidden = !isLoading;
  if (isLoading) {
    els.loadingOverlay.textContent = 'Loading…';
  }
}

function announce(msg) {
  els.statusRegion.textContent = msg;
}

function toggleFiltersPanel() {
  const hidden = !els.filtersPanel.hasAttribute('hidden') ? true : false;
  if (hidden) {
    els.filtersPanel.setAttribute('hidden', '');
    els.toggleFiltersBtn.textContent = 'Show Filters';
    els.toggleFiltersBtn.setAttribute('aria-expanded', 'false');
  } else {
    els.filtersPanel.removeAttribute('hidden');
    els.toggleFiltersBtn.textContent = 'Hide Filters';
    els.toggleFiltersBtn.setAttribute('aria-expanded', 'true');
  }
  localStorage.setItem('tx.filters.collapsed', hidden ? '1' : '0');
}

function restoreFilterPanelState() {
  const collapsed = localStorage.getItem('tx.filters.collapsed') === '1';
  if (collapsed) {
    els.filtersPanel.setAttribute('hidden', '');
    els.toggleFiltersBtn.textContent = 'Show Filters';
    els.toggleFiltersBtn.setAttribute('aria-expanded', 'false');
  }
}

function formatDateTime(iso) {
  if (!iso) return '';
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return iso;
  return d.toLocaleString();
}

function delay(ms) {
  return new Promise(res => setTimeout(res, ms));
}

document.addEventListener('DOMContentLoaded', boot);