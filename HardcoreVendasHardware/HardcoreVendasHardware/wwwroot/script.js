/* =========================================================
   HardCore ERP — JavaScript Vanilla
   Preparado para integração com API REST em C# ASP.NET Core
   ========================================================= */

const API_URL = "http://localhost:5000/api";

// Dados carregados da API
let produtos = [];
let clientes = [];
let pedidos = [];



/* ---------- Navegação ---------- */
function mostrarPagina(id, el) {
  document.querySelectorAll(".page").forEach(p => p.style.display = "none");
  document.getElementById(id).style.display = "block";
  document.querySelectorAll(".nav-item").forEach(a => a.classList.remove("active"));
  if (el) el.classList.add("active");
}
function abrirDashboard(e) {if (e) e.preventDefault();mostrarPagina("dashboard", e && e.currentTarget);renderDashboard();}
function abrirProdutos(e)  { if (e) e.preventDefault(); mostrarPagina("produtos",  e && e.currentTarget); renderProdutos(); }
function abrirClientes(e)  { if (e) e.preventDefault(); mostrarPagina("clientes",  e && e.currentTarget); renderClientes(); }
function abrirPedidos(e) { if (e) e.preventDefault(); mostrarPagina("pedidos", e && e.currentTarget); initPedidos(); }

/* ---------- Formatação ---------- */
const moeda = v => v.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
const data  = s => { const [a,m,d] = s.split("-"); return `${d}/${m}/${a}`; };
const pillClass = s => "pill pill-" + s.toLowerCase();

/* ---------- Renderização ---------- */
function renderDashboard() {
  
  document.getElementById("kpiProdutos").textContent = produtos.length;
  document.getElementById("kpiClientes").textContent = clientes.length;
  document.getElementById("kpiPedidos").textContent  = pedidos.length;

  
  const ativos   = produtos.filter(p => p.estoqueDisponivel > 0).length;
  const inativos = produtos.filter(p => p.estoqueDisponivel === 0).length;
  const totalEst = produtos.reduce((acc, p) => acc + p.estoqueDisponivel, 0);

  document.getElementById("kpiAtivos").textContent        = ativos;
  document.getElementById("kpiInativos").textContent      = inativos;
  document.getElementById("kpiTotalEstoque").textContent  = totalEst;

  
  calcularFinancas();
}

function calcularFinancas() {
  const lista = _pedidosLista.length > 0 ? _pedidosLista : pedidos;

  const valorInvestido = produtos.reduce(
    (acc, p) => acc + (p.precoCusto ?? 0) * p.estoqueDisponivel, 0);

  const receitaVendas = lista
    .filter(p => p.status === 1)
    .reduce((acc, p) => acc + p.valorTotal, 0);

  const custoVendidos = lista
    .filter(p => p.status === 1)
    .flatMap(p => p.itens ?? [])
    .reduce((acc, i) => acc + (i.precoCustoUnitario ?? 0) * i.quantidade, 0);

  const lucro = receitaVendas - custoVendidos;
  const margem = receitaVendas > 0
    ? ((lucro / receitaVendas) * 100).toFixed(2)
    : 0;

  const fmt = v => v.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

  document.getElementById("kpiInvestido").textContent    = fmt(valorInvestido);
  document.getElementById("kpiReceita").textContent      = fmt(receitaVendas);
  document.getElementById("kpiCusto").textContent        = fmt(custoVendidos);
  document.getElementById("kpiLucroValor").textContent   = fmt(lucro);
  document.getElementById("kpiLucroPercent").textContent = margem + "% de margem";

  const elValor   = document.getElementById("kpiLucroValor");
  const elPercent = document.getElementById("kpiLucroPercent");
  const elCard    = document.querySelector(".kpi-card-lucro");

  if (lucro < 0) {
    elValor.style.color   = "#dc2626";
    elPercent.style.color = "#dc2626";
    if (elCard) { elCard.style.borderLeftColor = "#dc2626"; elCard.style.background = "#fef2f2"; }
  } else {
    elValor.style.color   = "#15803d";
    elPercent.style.color = "#15803d";
    if (elCard) { elCard.style.borderLeftColor = "#15803d"; elCard.style.background = "#f0fdf4"; }
  }
}
function renderProdutos() {

    const tb =
        document.getElementById("tbodyProdutos");

    tb.innerHTML = produtos.map(p => `

        <tr>

            <td>${p.id}</td>

            <td>${p.nome}</td>

            <td>${p.categoria}</td>

            <td>${p.fabricante}</td>

            <td>${p.modelo}</td>

            <td>${p.estoqueDisponivel}</td>

            <td>${moeda(p.preco)}</td>

       <td class="num">
    <div class="actions">

        <button class="btn-editar"
                onclick="editarProduto(${p.id})">
            Editar
        </button>

        <button class="btn-excluir"
                onclick="excluirProduto(${p.id})">
            Excluir
        </button>

    </div>
</td>

    `).join("");

}

function renderClientes() {
  const tb = document.getElementById("tbodyClientes");
  tb.innerHTML = clientes.map((c, i) => `
    <tr>
      <td><strong>${c.nome}</strong></td>
      <td>${c.email}</td>
      <td>${c.telefone}</td>
      <td class="num">
        <div class="actions">
          <button class="btn btn-sm" onclick="editarCliente(${i})">Editar</button>
          <button class="btn btn-sm btn-danger" onclick="excluirCliente(${i})">Excluir</button>
        </div>
      </td>
    </tr>
  `).join("");
}



/* ---------- CRUD simples (mock) ---------- */
async function adicionarProduto() {

    const nome = prompt("Nome do produto:");
    if (!nome) return;

    const categoria = prompt("Categoria:");
    const estoque = parseInt(prompt("Estoque:") || "0");
    const preco = parseFloat(prompt("Preço:") || "0");

    const novoProduto = {
        nome,
        categoria,
        estoqueDisponivel: estoque,
        preco
    };

    try {

        await fetch(`${API_URL}/produtos`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(novoProduto)
        });

        await carregarProdutos();
        renderDashboard();

    } catch(error) {

        console.error(error);

    }

}




/* =========================================================
   INTEGRAÇÃO FUTURA — API REST em C# ASP.NET Core
   =========================================================
   Endpoints previstos:

   GET    /api/produtos        -> listar produtos
   POST   /api/produtos        -> criar produto
   PUT    /api/produtos/{id}   -> atualizar produto
   DELETE /api/produtos/{id}   -> remover produto

   GET    /api/clientes        -> listar clientes
   POST   /api/clientes        -> criar cliente
   PUT    /api/clientes/{id}   -> atualizar cliente
   DELETE /api/clientes/{id}   -> remover cliente

   GET    /api/pedidos         -> listar pedidos
   POST   /api/pedidos         -> criar pedido
   ========================================================= */

async function carregarProdutos() {

    try {

        const response =
            await fetch(`${API_URL}/produtos`);

        produtos =
            await response.json();

        console.log(produtos);

        renderProdutos();

    } catch(error) {

        console.error(error);

    }

}

async function carregarClientes() {

    try {

        const response =
            await fetch(`${API_URL}/clientes`);

        clientes =
            await response.json();

        renderClientes();

    } catch(error) {

        console.error(error);

    }

}

function editarProduto(id) {
  const produto = produtos.find(p => p.id === id);

  document.getElementById("editId").value         = produto.id;
  document.getElementById("editNome").value        = produto.nome;
  document.getElementById("editCategoria").value   = produto.categoria;
  document.getElementById("editFabricante").value  = produto.fabricante;
  document.getElementById("editModelo").value      = produto.modelo;
  document.getElementById("editPrecoCusto").value  = produto.precoCusto ?? 0;
  document.getElementById("editPrecoVenda").value  = produto.precoVenda ?? produto.preco ?? 0;
  document.getElementById("editEstoque").value     = produto.estoqueDisponivel;

  document.getElementById("modalEditar").style.display = "flex";
}

function fecharModal() {
    document.getElementById("modalEditar").style.display = "none";
}

async function salvarEdicao() {
  const id = document.getElementById("editId").value;

  const produto = {
    id:                parseInt(id),
    nome:              document.getElementById("editNome").value,
    categoria:         document.getElementById("editCategoria").value,
    fabricante:        document.getElementById("editFabricante").value,
    modelo:            document.getElementById("editModelo").value,
    precoCusto:        parseFloat(document.getElementById("editPrecoCusto").value) || 0,
    precoVenda:        parseFloat(document.getElementById("editPrecoVenda").value) || 0,
    preco:             parseFloat(document.getElementById("editPrecoVenda").value) || 0,
    estoqueDisponivel: parseInt(document.getElementById("editEstoque").value) || 0,
  };

  const response = await fetch(`${API_URL}/produtos/${id}`, {
    method:  "PUT",
    headers: { "Content-Type": "application/json" },
    body:    JSON.stringify(produto),
  });

  if (response.ok) {
    alert("Produto atualizado com sucesso!");
    fecharModal();
    carregarProdutos();
  } else {
    alert("Erro ao atualizar produto.");
  }
}

async function excluirProduto(id) {

    if (!confirm("Deseja excluir este produto?"))
        return;

    try {

        const response = await fetch(
            `${API_URL}/produtos/${id}`,
            {
                method: "DELETE"
            }
        );

        if (response.ok) {

            await carregarProdutos();

            renderDashboard();

        } else {

            alert("Erro ao excluir produto.");

        }

    } catch (error) {

        console.error(error);

    }
}

/* =========================================================
   MÓDULO DE PEDIDOS — cole no final do script.js
   (requer as classes do adicionar_no_style.css)
   ========================================================= */

let _pedidosItens    = [];
let _pedidosClientes = [];
let _pedidosProdutos = [];
let _pedidosLista    = [];

/* ── Badge de status ──────────────────────────────────────── */
function _badge(status) {
  const labels = ['Pendente','Confirmado','Em Processamento','Enviado','Entregue','Cancelado'];
  return `<span class="badge badge-${status}">${labels[status] ?? status}</span>`;
}

/* ── Inicializa aba de Pedidos ────────────────────────────── */
async function initPedidos() {
  const container = document.getElementById('container-pedidos');
  if (!container) return;

  container.innerHTML = `
    <div class="pedidos-header">
      <div>
        <h1>Pedidos</h1>
        <p>Acompanhe os pedidos</p>
      </div>
      <button id="btn-novo-pedido" class="btn btn-primary">+ Novo Pedido</button>
    </div>

    <div id="loading-pedidos" class="pedidos-loading">Carregando...</div>

    <div class="card" id="card-tabela-pedidos" style="display:none">
      <table class="table">
        <thead>
          <tr>
            <th>Número</th>
            <th>Cliente</th>
            <th>Data</th>
            <th>Status</th>
            <th>Total</th>
            <th class="num">Ações</th>
          </tr>
        </thead>
        <tbody id="tbody-pedidos-lista"></tbody>
      </table>
    </div>`;

  try {
    const [c, pr, pe] = await Promise.all([
      fetch(`${API_URL}/clientes`).then(r => r.json()),
      fetch(`${API_URL}/produtos`).then(r => r.json()),
      fetch(`${API_URL}/pedidos`).then(r => r.json()),
    ]);
    _pedidosClientes = c;
    _pedidosProdutos = pr;
    _pedidosLista    = pe;

    document.getElementById('loading-pedidos').style.display    = 'none';
    document.getElementById('card-tabela-pedidos').style.display = 'block';
    _renderTabelaPedidos();
  } catch (err) {
    document.getElementById('loading-pedidos').textContent =
      '⚠ Erro ao conectar com a API. Verifique se ela está rodando.';
    console.error(err);
  }

  document.getElementById('btn-novo-pedido')
    .addEventListener('click', _abrirModalNovoPedido);
}

/* ── Renderiza linhas da tabela ───────────────────────────── */
function _renderTabelaPedidos() {
  const nomeCliente = id =>
    _pedidosClientes.find(c => c.id === id)?.nome ?? `Cliente #${id}`;

  const tb = document.getElementById('tbody-pedidos-lista');
  if (!tb) return;

  tb.innerHTML = _pedidosLista.length
    ? _pedidosLista.map(p => `
        <tr>
          <td><strong>#${p.id}</strong></td>
          <td>${nomeCliente(p.clienteId)}</td>
          <td>${new Date(p.dataPedido).toLocaleDateString('pt-BR')}</td>
          <td>${_badge(p.status)}</td>
          <td>R$ ${p.valorTotal.toFixed(2).replace('.',',')}</td>
          <td class="num">
            <div class="actions">
              <button class="btn btn-sm" onclick="_verDetalhePedido(${p.id})">Detalhe</button>
            </div>
          </td>
        </tr>`).join('')
    : `<tr class="pedidos-empty"><td colspan="6">Nenhum pedido cadastrado ainda.</td></tr>`;
}

/* ── Modal: novo pedido ───────────────────────────────────── */
function _abrirModalNovoPedido() {
  _pedidosItens = [];

  const optsClientes = _pedidosClientes.map(c =>
    `<option value="${c.id}">${c.nome} — ${c.email}</option>`).join('');

  const optsProdutos = _pedidosProdutos.map(p =>
    `<option value="${p.id}" data-preco="${p.preco}" data-estoque="${p.estoqueDisponivel}">
       ${p.nome} (R$ ${p.preco.toFixed(2).replace('.',',')} | Estoque: ${p.estoqueDisponivel})
     </option>`).join('');

  const campoCliente = _pedidosClientes.length === 0
    ? `<p class="aviso-sem-cliente">⚠ Nenhum cliente cadastrado. Cadastre um cliente antes de criar um pedido.</p>`
    : `<select id="sel-cliente">
         <option value="">Selecione um cliente...</option>
         ${optsClientes}
       </select>`;

  const modal = document.createElement('div');
  modal.id = 'modal-novo-pedido';
  modal.className = 'modal-pedido-overlay';
  modal.innerHTML = `
    <div class="modal-pedido-box">
      <h2>Novo Pedido</h2>

      <label class="modal-pedido-label">
        <span>Cliente *</span>
        ${campoCliente}
      </label>

      <div class="modal-pedido-produtos">
        <p>Adicionar produto</p>
        <div class="modal-pedido-produtos-row">
          <select id="sel-produto">
            <option value="">Produto...</option>
            ${optsProdutos}
          </select>
          <input id="inp-qtd" type="number" min="1" value="1" placeholder="Qtd"/>
          <button class="btn btn-primary" onclick="_addItemPedido()">+ Adicionar</button>
        </div>
      </div>

      <div id="lista-itens-pedido" class="modal-pedido-itens">
        <p class="modal-pedido-itens-vazio">Nenhum item adicionado ainda.</p>
      </div>

      <div class="modal-pedido-total">
        <span>Total do Pedido</span>
        <span id="total-novo-pedido" class="modal-pedido-total-valor">R$ 0,00</span>
      </div>

      <textarea id="obs-novo-pedido" class="modal-pedido-obs" placeholder="Observações (opcional)"></textarea>

      <div id="erro-novo-pedido" class="modal-pedido-erro"></div>

      <div class="modal-pedido-actions">
        <button class="btn-cancelar" onclick="_fecharModalPedido()">Cancelar</button>
        <button class="btn-confirmar" onclick="_enviarNovoPedido()">Criar Pedido</button>
      </div>
    </div>`;

  document.body.appendChild(modal);
  modal.addEventListener('click', e => { if (e.target === modal) _fecharModalPedido(); });
}

function _fecharModalPedido() {
  document.getElementById('modal-novo-pedido')?.remove();
}

function _addItemPedido() {
  const sel = document.getElementById('sel-produto');
  const opt = sel.options[sel.selectedIndex];
  const qtd = parseInt(document.getElementById('inp-qtd').value, 10);

  if (!sel.value)      return alert('Selecione um produto.');
  if (!qtd || qtd < 1) return alert('Quantidade inválida.');

  const estoque = parseInt(opt.dataset.estoque, 10);
  if (qtd > estoque)   return alert(`Estoque insuficiente. Disponível: ${estoque}`);

  const idx = _pedidosItens.findIndex(i => i.produtoId === parseInt(sel.value));
  if (idx >= 0) {
    _pedidosItens[idx].quantidade += qtd;
  } else {
    _pedidosItens.push({
      produtoId:     parseInt(sel.value),
      nomeProduto:   opt.text.split(' (')[0],
      quantidade:    qtd,
      precoUnitario: parseFloat(opt.dataset.preco),
    });
  }
  _atualizarListaItens();
}

function _removerItemPedido(i) {
  _pedidosItens.splice(i, 1);
  _atualizarListaItens();
}

function _atualizarListaItens() {
  const lista = document.getElementById('lista-itens-pedido');
  const total = document.getElementById('total-novo-pedido');
  if (!lista) return;

  if (_pedidosItens.length === 0) {
    lista.innerHTML = `<p class="modal-pedido-itens-vazio">Nenhum item adicionado ainda.</p>`;
    if (total) total.textContent = 'R$ 0,00';
    return;
  }

  lista.innerHTML = _pedidosItens.map((item, i) => `
    <div class="modal-pedido-item">
      <span><b>${item.nomeProduto}</b> × ${item.quantidade}</span>
      <span class="modal-pedido-item-preco">
        R$ ${(item.precoUnitario * item.quantidade).toFixed(2).replace('.',',')}
        <button class="modal-pedido-item-remover" onclick="_removerItemPedido(${i})">✕</button>
      </span>
    </div>`).join('');

  const soma = _pedidosItens.reduce((acc, i) => acc + i.precoUnitario * i.quantidade, 0);
  if (total) total.textContent = `R$ ${soma.toFixed(2).replace('.',',')}`;
}

async function _enviarNovoPedido() {
  const clienteId = parseInt(document.getElementById('sel-cliente')?.value);
  const obs       = document.getElementById('obs-novo-pedido')?.value ?? '';
  const erro      = document.getElementById('erro-novo-pedido');

  if (!clienteId) {
    erro.textContent = 'Selecione um cliente.';
    erro.style.display = 'block';
    return;
  }
  if (_pedidosItens.length === 0) {
    erro.textContent = 'Adicione ao menos um produto.';
    erro.style.display = 'block';
    return;
  }
  erro.style.display = 'none';

  const payload = {
    clienteId,
    itens: _pedidosItens.map(i => ({
      produtoId:     i.produtoId,
      nomeProduto:   i.nomeProduto,
      quantidade:    i.quantidade,
      precoUnitario: i.precoUnitario,
    })),
    observacoes: obs || null,
  };

  try {
    const res = await fetch(`${API_URL}/pedidos`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload),
    });
    if (!res.ok) throw new Error(await res.text());

    const novo = await res.json();
    _pedidosLista.push(novo);
    pedidos = _pedidosLista; // sincroniza com o array global (dashboard)
    await carregarProdutos();
    _renderTabelaPedidos();
    renderDashboard();
    _fecharModalPedido();
  } catch (e) {
    erro.textContent = 'Erro ao criar pedido: ' + e.message;
    erro.style.display = 'block';
  }
}

/* ── Modal: detalhe do pedido ─────────────────────────────── */
function _verDetalhePedido(pedidoId) {
  const pedido  = _pedidosLista.find(p => p.id === pedidoId);
  if (!pedido) return;
  const cliente = _pedidosClientes.find(c => c.id === pedido.clienteId);

  const itensHtml = (pedido.itens ?? []).map(i => `
    <tr>
      <td>${i.nomeProduto}</td>
      <td style="text-align:center">${i.quantidade}</td>
      <td style="text-align:right">R$ ${i.precoUnitario.toFixed(2).replace('.',',')}</td>
      <td style="text-align:right"><strong>R$ ${i.subtotal.toFixed(2).replace('.',',')}</strong></td>
    </tr>`).join('');

  const modal = document.createElement('div');
  modal.id = 'modal-detalhe-pedido';
  modal.className = 'modal-pedido-overlay';
  modal.innerHTML = `
    <div class="modal-pedido-box">
      <div class="modal-detalhe-header">
        <h2>Pedido #${pedido.id}</h2>
        ${_badge(pedido.status)}
      </div>

      <div class="modal-detalhe-info">
        <div><span>Cliente</span><br><strong>${cliente?.nome ?? 'N/A'}</strong></div>
        <div><span>Email</span><br>${cliente?.email ?? '—'}</div>
        <div><span>Telefone</span><br>${cliente?.telefone ?? '—'}</div>
        <div><span>Data</span><br>${new Date(pedido.dataPedido).toLocaleDateString('pt-BR')}</div>
      </div>

      <table class="table">
        <thead>
          <tr>
            <th>Produto</th>
            <th style="text-align:center">Qtd</th>
            <th style="text-align:right">Preço Unit.</th>
            <th style="text-align:right">Subtotal</th>
          </tr>
        </thead>
        <tbody>
          ${itensHtml || '<tr><td colspan="4" class="pedidos-empty">Sem itens</td></tr>'}
        </tbody>
      </table>

      <div class="modal-detalhe-total">
        Total: R$ ${pedido.valorTotal.toFixed(2).replace('.',',')}
      </div>

      ${pedido.observacoes
        ? `<p class="modal-detalhe-obs"><strong>Obs:</strong> ${pedido.observacoes}</p>`
        : ''}

      <div class="modal-pedido-actions">
        <button class="btn-cancelar" onclick="document.getElementById('modal-detalhe-pedido').remove()">
          Fechar
        </button>
      </div>
    </div>`;

  document.body.appendChild(modal);
  modal.addEventListener('click', e => { if (e.target === modal) modal.remove(); });
}

function adicionarCliente() {
  const nome = prompt("Nome:"); if (!nome) return;
  const email = prompt("Email:") || "";
  const telefone = prompt("Telefone:") || "";
  clientes.push({ nome, email, telefone });
  renderClientes(); renderDashboard();
}

function editarCliente(i) {
  const c = clientes[i];
  const nome = prompt("Nome:", c.nome); if (nome === null) return;
  c.nome = nome;
  c.email = prompt("Email:", c.email) || c.email;
  c.telefone = prompt("Telefone:", c.telefone) || c.telefone;
  renderClientes();
}

function excluirCliente(i) {
  if (confirm("Excluir este cliente?")) {
    clientes.splice(i, 1);
    renderClientes();
    renderDashboard();
  }
}
async function carregarPedidos() {
  try {
    const response = await fetch(`${API_URL}/pedidos`);
    const data = await response.json();
    pedidos = data;
    _pedidosLista = data; // mantém os dois arrays sincronizados
  } catch (error) {
    console.error(error);
  }
}
/* ---------- Inicialização ---------- */
document.addEventListener("DOMContentLoaded", async () => {
  await carregarProdutos();
  await carregarClientes();
  await carregarPedidos();
  renderDashboard();
});

function renderProdutos() {
    const tb = document.getElementById("tbodyProdutos");
 
    tb.innerHTML = produtos.map(p => {
        // ── NOVO: badge de status baseado no estoque ──
        const ativo = p.estoqueDisponivel > 0;
        const statusBadge = ativo
            ? `<span class="status-badge status-ativo">Ativo</span>`
            : `<span class="status-badge status-inativo">Inativo</span>`;
 
        return `
        <tr>
            <td>${p.id}</td>
            <td>${p.nome}</td>
            <td>${p.categoria}</td>
            <td>${p.fabricante}</td>
            <td>${p.modelo}</td>
            <td>${p.estoqueDisponivel}</td>
            <td>${moeda(p.preco)}</td>
            <td>${statusBadge}</td>
            <td class="num">
                <div class="actions">
                    <button class="btn-editar" onclick="editarProduto(${p.id})">Editar</button>
                    <button class="btn-excluir" onclick="excluirProduto(${p.id})">Excluir</button>
                </div>
            </td>
        </tr>`;
    }).join("");
}
 
/* ATENÇÃO: adicione também o <th>Status</th> na tabela de produtos no HTML
   (ver PATCH_index.html) */
 
 
/* ═══════════════════════════════════════════════════════════
   FUNCIONALIDADE 2 — Cancelar Pedido
   (botão "Cancelar" na tabela + PUT na API para status 5)
 
   ONDE: função _renderTabelaPedidos()
   SUBSTITUA o bloco tb.innerHTML = _pedidosLista.length ? ... inteiro:
   ═══════════════════════════════════════════════════════════ */
 
function _renderTabelaPedidos() {
    const nomeCliente = id =>
        _pedidosClientes.find(c => c.id === id)?.nome ?? `Cliente #${id}`;
 
    const tb = document.getElementById('tbody-pedidos-lista');
    if (!tb) return;
 
    tb.innerHTML = _pedidosLista.length
        ? _pedidosLista.map(p => `
            <tr>
              <td><strong>#${p.id}</strong></td>
              <td>${nomeCliente(p.clienteId)}</td>
              <td>${new Date(p.dataPedido).toLocaleDateString('pt-BR')}</td>
              <td>${_badge(p.status)}</td>
              <td>R$ ${p.valorTotal.toFixed(2).replace('.', ',')}</td>
              <td class="num">
                <div class="actions">
                  <button class="btn btn-sm" onclick="_verDetalhePedido(${p.id})">Detalhe</button>
                  ${p.status !== 5
                    ? `<button class="btn btn-sm btn-danger" onclick="_cancelarPedido(${p.id})">Cancelar</button>`
                    : ''}
                </div>
              </td>
            </tr>`).join('')
        : `<tr class="pedidos-empty"><td colspan="6">Nenhum pedido cadastrado ainda.</td></tr>`;
}
 
/* ADICIONE esta função nova logo após _renderTabelaPedidos(): */
 
async function _cancelarPedido(pedidoId) {
    if (!confirm("Deseja realmente cancelar o pedido #" + pedidoId + "?")) return;

    try {
        const res = await fetch(API_URL + "/pedidos/" + pedidoId + "/cancelar", {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
        });

        if (!res.ok) {
            const msg = await res.text();
            throw new Error(msg || 'Erro desconhecido');
        }

        const pedido = _pedidosLista.find(p => p.id === pedidoId);
        if (pedido) pedido.status = 5;
        pedidos = _pedidosLista;
        await carregarProdutos(); 
        _renderTabelaPedidos();
        renderDashboard();

    } catch (e) {
        alert('Erro ao cancelar pedido: ' + e.message);
    }
}
/* ═══════════════════════════════════════════════════════════
   FUNCIONALIDADE 3 — Busca de cliente por texto no modal Novo Pedido
   + múltiplos produtos diferentes por pedido (já existe no código,
     mas o campo de cliente vira busca com filtro dinâmico)
 
   ONDE: função _abrirModalNovoPedido()
   SUBSTITUA o bloco inteiro da função:
   ═══════════════════════════════════════════════════════════ */
 
function _abrirModalNovoPedido() {
    _pedidosItens = [];
 
    const optsProdutos = _pedidosProdutos.map(p =>
        `<option value="${p.id}" data-preco="${p.preco}" data-estoque="${p.estoqueDisponivel}">
           ${p.nome} (R$ ${p.preco.toFixed(2).replace('.', ',')} | Estoque: ${p.estoqueDisponivel})
         </option>`).join('');
 
    // ── NOVO: campo de busca de cliente em vez de select fixo ──
    const campoCliente = _pedidosClientes.length === 0
        ? `<p class="aviso-sem-cliente">⚠ Nenhum cliente cadastrado. Cadastre um cliente antes de criar um pedido.</p>`
        : `
          <input type="text" id="busca-cliente" class="busca-cliente-input"
                 placeholder="Digite o nome ou e-mail do cliente..."
                 oninput="_filtrarClientes()" autocomplete="off"/>
          <ul id="lista-clientes-sugestoes" class="clientes-sugestoes"></ul>
          <input type="hidden" id="sel-cliente"/>
          <span id="cliente-selecionado" class="cliente-selecionado-label"></span>`;
 
    const modal = document.createElement('div');
    modal.id = 'modal-novo-pedido';
    modal.className = 'modal-pedido-overlay';
    modal.innerHTML = `
      <div class="modal-pedido-box">
        <h2>Novo Pedido</h2>
 
        <label class="modal-pedido-label">
          <span>Cliente *</span>
          ${campoCliente}
        </label>
 
        <div class="modal-pedido-produtos">
          <p>Adicionar produto</p>
          <div class="modal-pedido-produtos-row">
            <select id="sel-produto">
              <option value="">Produto...</option>
              ${optsProdutos}
            </select>
            <input id="inp-qtd" type="number" min="1" value="1" placeholder="Qtd"/>
            <button class="btn btn-primary" onclick="_addItemPedido()">+ Adicionar</button>
          </div>
        </div>
 
        <div id="lista-itens-pedido" class="modal-pedido-itens">
          <p class="modal-pedido-itens-vazio">Nenhum item adicionado ainda.</p>
        </div>
 
        <div class="modal-pedido-total">
          <span>Total do Pedido</span>
          <span id="total-novo-pedido" class="modal-pedido-total-valor">R$ 0,00</span>
        </div>
 
        <textarea id="obs-novo-pedido" class="modal-pedido-obs" placeholder="Observações (opcional)"></textarea>
 
        <div id="erro-novo-pedido" class="modal-pedido-erro"></div>
 
        <div class="modal-pedido-actions">
          <button class="btn-cancelar" onclick="_fecharModalPedido()">Cancelar</button>
          <button class="btn-confirmar" onclick="_enviarNovoPedido()">Criar Pedido</button>
        </div>
      </div>`;
 
    document.body.appendChild(modal);
    modal.addEventListener('click', e => { if (e.target === modal) _fecharModalPedido(); });
}
 
/* ADICIONE estas duas funções novas logo após _abrirModalNovoPedido(): */
 
function _filtrarClientes() {
    const termo = document.getElementById('busca-cliente').value.toLowerCase();
    const lista = document.getElementById('lista-clientes-sugestoes');
 
    if (!termo) { lista.innerHTML = ''; return; }
 
    const filtrados = _pedidosClientes.filter(c =>
        c.nome.toLowerCase().includes(termo) || c.email.toLowerCase().includes(termo)
    );
 
    lista.innerHTML = filtrados.length
        ? filtrados.map(c => `
            <li onclick="_selecionarCliente(${c.id}, '${c.nome.replace(/'/g, "\\'")}')">
              <strong>${c.nome}</strong> <span>${c.email}</span>
            </li>`).join('')
        : `<li class="sem-resultado">Nenhum cliente encontrado.</li>`;
}
 
function _selecionarCliente(id, nome) {
    document.getElementById('sel-cliente').value = id;
    document.getElementById('busca-cliente').value = nome;
    document.getElementById('lista-clientes-sugestoes').innerHTML = '';
    document.getElementById('cliente-selecionado').textContent = `✔ Cliente selecionado: ${nome}`;
}
 
 
/* ═══════════════════════════════════════════════════════════
   FUNCIONALIDADE 4 — Puxar clientes cadastrados na aba "Novo Produto"
   (adicionarProduto usa prompt simples; substituímos por modal com
    select de clientes — interpretado como: ao criar produto, pode
    associar um cliente/fornecedor, ou simplesmente o modal já
    aproveita os clientes para referência)
 
   ONDE: função adicionarProduto()
   SUBSTITUA a função inteira:
   ═══════════════════════════════════════════════════════════ */
 
async function adicionarProduto() {
    // Monta options de clientes para o modal
    const optsClientes = clientes.length > 0
        ? clientes.map(c => `<option value="${c.id}">${c.nome}</option>`).join('')
        : '<option value="">Nenhum cliente cadastrado</option>';
 
    // Cria modal de novo produto
    const modal = document.createElement('div');
    modal.id = 'modal-novo-produto';
    modal.className = 'modal-pedido-overlay';
    modal.innerHTML = `
      <div class="modal-pedido-box">
        <h2>Novo Produto</h2>
 
        <label class="modal-pedido-label">
          <span>Nome *</span>
          <input type="text" id="np-nome" class="modal-pedido-obs" style="height:auto;padding:10px" placeholder="Nome do produto"/>
        </label>
 
        <label class="modal-pedido-label">
          <span>Categoria</span>
          <input type="text" id="np-categoria" class="modal-pedido-obs" style="height:auto;padding:10px" placeholder="Ex: GPU, CPU, Memória..."/>
        </label>
 
        <label class="modal-pedido-label">
          <span>Fabricante</span>
          <input type="text" id="np-fabricante" class="modal-pedido-obs" style="height:auto;padding:10px" placeholder="Ex: NVIDIA, Intel..."/>
        </label>
 
        <label class="modal-pedido-label">
          <span>Modelo</span>
          <input type="text" id="np-modelo" class="modal-pedido-obs" style="height:auto;padding:10px" placeholder="Ex: RTX 4070"/>
        </label>
 
        <label class="modal-pedido-label">
          <span>Preço (R$) *</span>
          <input type="number" id="np-preco" class="modal-pedido-obs" style="height:auto;padding:10px" placeholder="0.00" min="0" step="0.01"/>
        </label>
 
        <label class="modal-pedido-label">
          <span>Estoque *</span>
          <input type="number" id="np-estoque" class="modal-pedido-obs" style="height:auto;padding:10px" placeholder="0" min="0"/>
        </label>
 
        <label class="modal-pedido-label">
          <span>Cliente / Fornecedor vinculado (opcional)</span>
          <select id="np-cliente">
            <option value="">Nenhum</option>
            ${optsClientes}
          </select>
        </label>
 
        <div id="erro-novo-produto" class="modal-pedido-erro"></div>
 
        <div class="modal-pedido-actions">
          <button class="btn-cancelar" onclick="document.getElementById('modal-novo-produto').remove()">Cancelar</button>
          <button class="btn-confirmar" onclick="_salvarNovoProduto()">Salvar Produto</button>
        </div>
      </div>`;
 
    document.body.appendChild(modal);
    modal.addEventListener('click', e => { if (e.target === modal) modal.remove(); });
}
 
/* ADICIONE esta função nova logo após adicionarProduto(): */
 
async function _salvarNovoProduto() {
    const nome     = document.getElementById('np-nome').value.trim();
    const preco    = parseFloat(document.getElementById('np-preco').value);
    const estoque  = parseInt(document.getElementById('np-estoque').value);
    const erro     = document.getElementById('erro-novo-produto');
 
    if (!nome) {
        erro.textContent = 'O nome do produto é obrigatório.';
        erro.style.display = 'block';
        return;
    }
    if (isNaN(preco) || preco < 0) {
        erro.textContent = 'Informe um preço válido.';
        erro.style.display = 'block';
        return;
    }
    erro.style.display = 'none';
 
    const novoProduto = {
        nome,
        categoria:          document.getElementById('np-categoria').value.trim(),
        fabricante:         document.getElementById('np-fabricante').value.trim(),
        modelo:             document.getElementById('np-modelo').value.trim(),
        preco,
        estoqueDisponivel:  isNaN(estoque) ? 0 : estoque,
        clienteId:          document.getElementById('np-cliente').value || null,
    };
 
    try {
        const res = await fetch(`${API_URL}/produtos`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoProduto),
        });
 
        if (!res.ok) throw new Error(await res.text());
 
        document.getElementById('modal-novo-produto').remove();
        await carregarProdutos();
        renderDashboard();
    } catch (e) {
        erro.textContent = 'Erro ao salvar produto: ' + e.message;
        erro.style.display = 'block';
    }
}

document.addEventListener("DOMContentLoaded", () => {

    const busca =
        document.getElementById("globalSearch");
        console.log("Campo encontrado:", campo);

    busca.addEventListener("input", () => {

        const termo =
            busca.value.toLowerCase();

        executarBusca(termo);

    });

});

function executarBuscaGlobal() {

    const termo =
        document
            .getElementById("globalSearch")
            .value
            .toLowerCase();
             console.log("Digitando:", termo);

    if (
        document.getElementById("produtos").style.display !== "none"
    ) {
        filtrarTabela("tbodyProdutos", termo);
        return;
    }

    if (
        document.getElementById("clientes").style.display !== "none"
    ) {
        filtrarTabela("tbodyClientes", termo);
        return;
    }

    if (
        document.getElementById("pedidos").style.display !== "none"
    ) {
        filtrarTabela("tbody-pedidos-lista", termo);
        return;
    }
}

function filtrarTabela(idTabela, termo) {

    const linhas =
        document.querySelectorAll(`#${idTabela} tr`);

    linhas.forEach(linha => {

        const texto =
            linha.textContent.toLowerCase();

        linha.style.display =
            texto.includes(termo)
                ? ""
                : "none";

    });

}
testarProdutos();