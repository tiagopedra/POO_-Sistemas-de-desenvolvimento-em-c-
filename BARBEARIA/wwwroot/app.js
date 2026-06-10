// ============================================================
// app.js — Front-end da aplicação BarberPro
//
// Responsabilidade:
//   - Comunicar com a API REST via fetch (HTTP)
//   - Renderizar agendamentos, serviços e resumo financeiro
//   - Gerenciar os modais de criação e edição
//   - Exibir feedback visual ao usuário (toast)
//
// Organização:
//   1. Configuração e variáveis globais
//   2. Helpers (formatação, fetch genérico, toast)
//   3. Navegação entre abas
//   4. Agendamentos (listar, criar, editar, excluir)
//   5. Serviços/Produtos (listar, criar, editar, excluir)
//   6. Financeiro (carregar e renderizar resumo)
//   7. Inicialização
// ============================================================

// 1. CONFIGURAÇÃO E VARIÁVEIS GLOBAIS
// Endereço base da API — deve coincidir com applicationUrl do launchSettings.json
const API = 'http://localhost:5000/api';

var servicosCache     = [];   // Cache dos serviços para popular o <select> sem requisição extra
var editandoAgId      = null; // ID do agendamento em edição (null = criação nova)
var editandoServicoId = null; // ID do serviço em edição (null = criação nova)
var editandoDespesaId = null; // ID da despesa em edição (null = criação nova)

// ============================================================
// 2. HELPERS
// ============================================================

// Formata um número como moeda brasileira (ex: 1500 → "R$ 1.500,00")
function moeda(v) {
  return Number(v).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

// Converte "2026-06-05T00:00:00" ou "2026-06-05" para "05/06/2026"
function formatarData(d) {
  if (!d) return '-';
  return d.split('T')[0].split('-').reverse().join('/');
}

// Função centralizada de fetch para a API
// Trata erros HTTP e decide automaticamente se retorna JSON ou texto
async function apiFetch(path, options) {
  const res = await fetch(API + path, options);
  if (!res.ok) {
    const msg = await res.text().catch(() => res.statusText);
    throw new Error(msg || res.statusText);
  }
  // DELETE retorna texto simples; GET/POST/PUT retornam JSON
  const ct = res.headers.get('content-type') || '';
  return ct.includes('application/json') ? res.json() : res.text();
}

// Exibe uma mensagem temporária no canto inferior direito da tela
function toast(msg) {
  const el = document.getElementById('toast');
  el.textContent = msg;
  el.classList.add('visivel');
  setTimeout(function() { el.classList.remove('visivel'); }, 2800);
}

// ============================================================
// 3. NAVEGAÇÃO ENTRE ABAS (sidebar)
// ============================================================

function mostrarAba(aba, botao) {
  // Oculta todas as seções e exibe apenas a selecionada
  document.getElementById('secao-agendamentos').style.display = aba === 'agendamentos' ? 'block' : 'none';
  document.getElementById('secao-servicos').style.display     = aba === 'servicos'     ? 'block' : 'none';
  document.getElementById('secao-financeiro').style.display   = aba === 'financeiro'   ? 'block' : 'none';

  // Atualiza o item ativo na sidebar
  document.querySelectorAll('.nav-item').forEach(function(b) { b.classList.remove('ativo'); });
  botao.classList.add('ativo');

  // Carrega os dados da seção selecionada
  if (aba === 'agendamentos') carregarAgendamentos();
  if (aba === 'servicos')     carregarServicos();
  if (aba === 'financeiro')   carregarFinanceiro();
}

// ============================================================
// 4. AGENDAMENTOS
// ============================================================

// Busca todos os agendamentos da API e renderiza na tabela
async function carregarAgendamentos() {
  const tbody = document.getElementById('tabela-agendamentos');
  tbody.innerHTML = '<tr><td colspan="7" class="carregando">Carregando...</td></tr>';
  try {
    const dados = await apiFetch('/agendamentos');
    renderizarAgendamentos(dados);
  } catch (e) {
    tbody.innerHTML = '<tr><td colspan="7" class="carregando">Erro ao carregar: ' + e.message + '</td></tr>';
  }
}

// Gera o HTML das linhas da tabela de agendamentos
function renderizarAgendamentos(lista) {
  const tbody = document.getElementById('tabela-agendamentos');
  if (!lista || lista.length === 0) {
    tbody.innerHTML = '<tr><td colspan="7" class="carregando">Nenhum agendamento encontrado.</td></tr>';
    return;
  }
  tbody.innerHTML = lista.map(function(a) {
    return '<tr>' +
      '<td>' + a.id + '</td>' +
      '<td>' + a.cliente + '</td>' +
      '<td>' + a.servico + '</td>' +
      '<td>' + formatarData(a.data) + '</td>' +
      '<td>' + a.horario + '</td>' +
      '<td>' + (a.observacao || '-') + '</td>' +
      '<td>' +
        '<button class="btn-editar"  onclick="abrirEditar(' + a.id + ')">Editar</button>' +
        '<button class="btn-excluir" onclick="excluirAg(' + a.id + ')">Excluir</button>' +
      '</td>' +
    '</tr>';
  }).join('');
}

// Popula o <select> de serviços no modal de agendamento
async function preencherSelectServico(selecionado) {
  // Usa o cache para evitar requisição desnecessária
  if (servicosCache.length === 0) {
    try { servicosCache = await apiFetch('/produtos'); } catch (e) { servicosCache = []; }
  }
  const select = document.getElementById('campo-servico');
  select.innerHTML = servicosCache.map(function(s) {
    return '<option value="' + s.nome + '"' + (s.nome === selecionado ? ' selected' : '') + '>' + s.nome + '</option>';
  }).join('');
}

// Abre o modal para criar um novo agendamento
async function abrirNovo() {
  editandoAgId = null;
  document.getElementById('modal-titulo').textContent = 'Novo Agendamento';
  document.getElementById('campo-cliente').value = '';
  document.getElementById('campo-data').value    = '';
  document.getElementById('campo-horario').value = '';
  document.getElementById('campo-obs').value     = '';
  await preencherSelectServico('');
  document.getElementById('modal-agendamento').classList.add('aberto');
}

// Abre o modal preenchido com os dados de um agendamento existente
async function abrirEditar(id) {
  try {
    const a = await apiFetch('/agendamentos/' + id);
    editandoAgId = id;
    document.getElementById('modal-titulo').textContent = 'Editar Agendamento';
    document.getElementById('campo-cliente').value = a.cliente;
    document.getElementById('campo-data').value    = (a.data || '').split('T')[0];
    document.getElementById('campo-horario').value = a.horario;
    document.getElementById('campo-obs').value     = a.observacao || '';
    await preencherSelectServico(a.servico);
    document.getElementById('modal-agendamento').classList.add('aberto');
  } catch (e) {
    toast('Erro ao carregar agendamento: ' + e.message);
  }
}

// Salva um agendamento (POST para novo, PUT para edição)
async function salvar() {
  const cliente = document.getElementById('campo-cliente').value.trim();
  const servico = document.getElementById('campo-servico').value;
  const data    = document.getElementById('campo-data').value;
  const horario = document.getElementById('campo-horario').value;
  const obs     = document.getElementById('campo-obs').value.trim();

  if (!cliente || !data || !horario) {
    toast('Preencha cliente, data e horário.');
    return;
  }

  const body = { cliente, servico, data, horario, observacao: obs };

  try {
    if (editandoAgId) {
      // PUT — atualiza agendamento existente
      await apiFetch('/agendamentos/' + editandoAgId, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });
      toast('Agendamento atualizado!');
    } else {
      // POST — cria novo agendamento
      await apiFetch('/agendamentos', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });
      toast('Agendamento criado!');
    }
    fecharModal();
    carregarAgendamentos();
  } catch (e) {
    toast('Erro ao salvar: ' + e.message);
  }
}

// Exclui um agendamento pelo ID (DELETE)
async function excluirAg(id) {
  if (!confirm('Excluir este agendamento?')) return;
  try {
    await apiFetch('/agendamentos/' + id, { method: 'DELETE' });
    toast('Agendamento excluído.');
    carregarAgendamentos();
  } catch (e) {
    toast('Erro ao excluir: ' + e.message);
  }
}

// Fecha o modal de agendamento
function fecharModal() {
  document.getElementById('modal-agendamento').classList.remove('aberto');
}

// ============================================================
// 5. SERVIÇOS / PRODUTOS
// ============================================================

// Busca todos os serviços da API e renderiza na tabela
async function carregarServicos() {
  const tbody = document.getElementById('tabela-servicos');
  tbody.innerHTML = '<tr><td colspan="5" class="carregando">Carregando...</td></tr>';
  try {
    const dados = await apiFetch('/produtos');
    servicosCache = dados; // Atualiza o cache global
    renderizarServicos(dados);
  } catch (e) {
    tbody.innerHTML = '<tr><td colspan="5" class="carregando">Erro ao carregar: ' + e.message + '</td></tr>';
  }
}

// Gera o HTML das linhas da tabela de serviços
function renderizarServicos(lista) {
  const tbody = document.getElementById('tabela-servicos');
  if (!lista || lista.length === 0) {
    tbody.innerHTML = '<tr><td colspan="5" class="carregando">Nenhum serviço encontrado.</td></tr>';
    return;
  }
  tbody.innerHTML = lista.map(function(s) {
    return '<tr>' +
      '<td>' + s.id + '</td>' +
      '<td>' + s.nome + '</td>' +
      '<td>' + s.descricao + '</td>' +
      '<td>' + moeda(s.preco) + '</td>' +
      '<td>' +
        '<button class="btn-editar"  onclick="abrirEditarServico(' + s.id + ')">Editar</button>' +
        '<button class="btn-excluir" onclick="excluirServico(' + s.id + ')">Excluir</button>' +
      '</td>' +
    '</tr>';
  }).join('');
}

// Abre o modal para criar um novo serviço
function abrirNovoServico() {
  editandoServicoId = null;
  document.getElementById('modal-servico-titulo').textContent = 'Novo Serviço';
  document.getElementById('campo-nome').value  = '';
  document.getElementById('campo-desc').value  = '';
  document.getElementById('campo-preco').value = '';
  document.getElementById('modal-servico').classList.add('aberto');
}

// Abre o modal preenchido com os dados de um serviço existente
async function abrirEditarServico(id) {
  try {
    const s = await apiFetch('/produtos/' + id);
    editandoServicoId = id;
    document.getElementById('modal-servico-titulo').textContent = 'Editar Serviço';
    document.getElementById('campo-nome').value  = s.nome;
    document.getElementById('campo-desc').value  = s.descricao;
    document.getElementById('campo-preco').value = s.preco;
    document.getElementById('modal-servico').classList.add('aberto');
  } catch (e) {
    toast('Erro ao carregar serviço: ' + e.message);
  }
}

// Salva um serviço (POST para novo, PUT para edição)
async function salvarServico() {
  const nome  = document.getElementById('campo-nome').value.trim();
  const desc  = document.getElementById('campo-desc').value.trim();
  const preco = parseFloat(document.getElementById('campo-preco').value);

  if (!nome || !desc || isNaN(preco)) {
    toast('Preencha todos os campos.');
    return;
  }

  const body = { nome, descricao: desc, preco };

  try {
    if (editandoServicoId) {
      await apiFetch('/produtos/' + editandoServicoId, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });
      toast('Serviço atualizado!');
    } else {
      await apiFetch('/produtos', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });
      toast('Serviço criado!');
    }
    servicosCache = []; // Limpa cache para recarregar da API
    fecharModalServico();
    carregarServicos();
  } catch (e) {
    toast('Erro ao salvar: ' + e.message);
  }
}

// Exclui um serviço pelo ID (DELETE)
async function excluirServico(id) {
  if (!confirm('Excluir este serviço?')) return;
  try {
    await apiFetch('/produtos/' + id, { method: 'DELETE' });
    servicosCache = [];
    toast('Serviço excluído.');
    carregarServicos();
  } catch (e) {
    toast('Erro ao excluir: ' + e.message);
  }
}

// Fecha o modal de serviço
function fecharModalServico() {
  document.getElementById('modal-servico').classList.remove('aberto');
}

// ============================================================
// 6. FINANCEIRO
// ============================================================

// Busca o resumo financeiro da API e renderiza os cards e tabelas
async function carregarFinanceiro() {
  // Exibe estado de carregamento nos cards
  ['fin-receitas', 'fin-despesas', 'fin-lucro'].forEach(function(id) {
    document.getElementById(id).textContent = 'Carregando...';
  });
  document.getElementById('fin-receitas-sub').textContent = '';
  document.getElementById('fin-situacao').textContent = '';

  try {
    // Busca o resumo financeiro calculado pela API
    const fin = await apiFetch('/financeiro');

    // Atualiza os cards de topo
    document.getElementById('fin-receitas').textContent     = moeda(fin.totalReceitas);
    document.getElementById('fin-receitas-sub').textContent = (fin.detalhes?.totalAgendamentos ?? 0) + ' agendamento(s)';
    document.getElementById('fin-despesas').textContent     = moeda(fin.totalDespesas);
    document.getElementById('fin-lucro').textContent        = moeda(fin.lucro);

    // Detalha receitas por serviço cruzando agendamentos × preço dos serviços
    const agendamentos = await apiFetch('/agendamentos');
    if (servicosCache.length === 0) {
      servicosCache = await apiFetch('/produtos');
    }

    const receitasPorServico = {};
    for (const a of agendamentos) {
      const srv   = servicosCache.find(function(x) { return x.nome === a.servico; });
      const preco = srv ? srv.preco : 0;
      receitasPorServico[a.servico] = (receitasPorServico[a.servico] || 0) + preco;
    }

    // Renderiza a tabela de receitas por serviço
    const tbReceit  = document.getElementById('tabela-receitas');
    const entradas  = Object.keys(receitasPorServico);
    tbReceit.innerHTML = entradas.length === 0
      ? '<tr><td colspan="2" class="carregando">Nenhuma receita</td></tr>'
      : entradas.map(function(nome) {
          return '<tr><td>' + nome + '</td><td class="valor-receita">' + moeda(receitasPorServico[nome]) + '</td></tr>';
        }).join('');

    // Renderiza a tabela de despesas buscando de /api/despesas (contém o Id necessário para excluir)
    const tbDesp  = document.getElementById('tabela-despesas');
    const despesas = await apiFetch('/despesas');
    tbDesp.innerHTML = despesas.length === 0
      ? '<tr><td colspan="3" class="carregando">Nenhuma despesa</td></tr>'
      : despesas.map(function(d) {
          return '<tr>' +
            '<td>' + d.descricao + '</td>' +
            '<td class="valor-despesa">' + moeda(d.valor) + '</td>' +
            '<td><button class="btn-excluir" onclick="excluirDespesa(' + d.id + ')">Excluir</button></td>' +
          '</tr>';
        }).join('');

  } catch (e) {
    ['fin-receitas', 'fin-despesas', 'fin-lucro'].forEach(function(id) {
      document.getElementById(id).textContent = 'Erro';
    });
    document.getElementById('fin-situacao').textContent = e.message;
  }
}

// ============================================================
// 7. DESPESAS
// ============================================================

// Abre o modal para criar uma nova despesa
function abrirNovaDespesa() {
  editandoDespesaId = null;
  document.getElementById('modal-despesa-titulo').textContent = 'Nova Despesa';
  document.getElementById('campo-desp-desc').value  = '';
  document.getElementById('campo-desp-valor').value = '';
  document.getElementById('modal-despesa').classList.add('aberto');
}

// Salva uma despesa (POST para nova)
async function salvarDespesa() {
  const descricao = document.getElementById('campo-desp-desc').value.trim();
  const valor     = parseFloat(document.getElementById('campo-desp-valor').value);

  if (!descricao || isNaN(valor) || valor <= 0) {
    toast('Preencha descrição e valor.');
    return;
  }

  try {
    await apiFetch('/despesas', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ descricao, valor })
    });
    toast('Despesa cadastrada!');
    fecharModalDespesa();
    carregarFinanceiro();
  } catch (e) {
    toast('Erro ao salvar despesa: ' + e.message);
  }
}

// Exclui uma despesa pelo ID (DELETE)
async function excluirDespesa(id) {
  if (!confirm('Excluir esta despesa?')) return;
  try {
    await apiFetch('/despesas/' + id, { method: 'DELETE' });
    toast('Despesa excluída.');
    carregarFinanceiro();
  } catch (e) {
    toast('Erro ao excluir despesa: ' + e.message);
  }
}

// Fecha o modal de despesa
function fecharModalDespesa() {
  document.getElementById('modal-despesa').classList.remove('aberto');
}

// ============================================================
// 8. INICIALIZAÇÃO
// Executa quando o DOM está pronto, carregando os agendamentos
// ============================================================
document.addEventListener('DOMContentLoaded', function() {
  carregarAgendamentos();
});
