/* CONTRACT FLOW — Arquivo de Comportamento
   
   Este arquivo é responsável por:
   1. Buscar os contratos da API C#
   2. Exibir os dados na tabela
   3. Filtrar e buscar contratos
   4. Abrir/fechar as janelas flutuantes
   5. Criar, editar e excluir contratos
   6. Exportar relatórios */


/* CONFIGURAÇÕES INICIAIS */

// Endereço da API C# (onde os dados estão)
const ENDERECO_API = 'http://localhost:5043/api/contratos';

// Quantos contratos aparecem por página na tabela
const CONTRATOS_POR_PAGINA = 10;


/* VARIÁVEIS DE ESTADO
   (guardam informações enquanto a página está aberta) */
let todosOsContratos  = [];  // lista completa vinda da API
let contratosFiltrados = [];  // lista após aplicar filtros
let paginaAtual       = 1;   // qual página está sendo exibida
let idEditando        = null; // id do contrato sendo editado (null = novo)
let contratoVisualizando = null; // contrato aberto na janela de detalhes


/* ATALHO PARA PEGAR ELEMENTOS DO HTML */
// Em vez de escrever document.getElementById('nome') toda hora,
// usamos apenas: pegar('nome')
const pegar = id => document.getElementById(id);


/* INICIALIZAÇÃO
   (executado assim que a página termina de carregar) */
document.addEventListener('DOMContentLoaded', () => {
  carregarContratos(); // busca os dados da API
  vincularEventos();   // configura os cliques e interações
});


/* COMUNICAÇÃO COM A API C#
   (as funções abaixo fazem requisições HTTP) */

// Busca todos os contratos da API (método GET)
async function carregarContratos() {
  try {
    mostrarCarregamento();

    const resposta = await fetch(ENDERECO_API); // faz a requisição

    if (!resposta.ok) throw new Error(`Erro HTTP ${resposta.status}`);

    todosOsContratos = await resposta.json(); // converte a resposta para objeto JS
    aplicarFiltros();
    atualizarIndicadores();
    mostrarMensagem('Dados carregados com sucesso', 'sucesso');

  } catch (erro) {
    console.error(erro);
    mostrarErroNaTabela('Não foi possível conectar à API. Verifique se a API C# está rodando em http://localhost:5043');
    mostrarMensagem('Erro ao conectar à API', 'erro');
  }
}

// Cria um novo contrato na API (método POST)
async function criarContrato(dados) {
  const resposta = await fetch(ENDERECO_API, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' }, // avisa que está enviando JSON
    body: JSON.stringify(dados) // converte objeto JS para texto JSON
  });
  if (!resposta.ok) throw new Error(`Erro HTTP ${resposta.status}`);
  return resposta.json();
}

// Atualiza um contrato existente na API (método PUT)
async function atualizarContrato(id, dados) {
  const resposta = await fetch(`${ENDERECO_API}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dados)
  });
  if (!resposta.ok) throw new Error(`Erro HTTP ${resposta.status}`);
  return resposta.json();
}

// Remove um contrato da API (método DELETE)
async function excluirContrato(id) {
  const resposta = await fetch(`${ENDERECO_API}/${id}`, { method: 'DELETE' });
  if (!resposta.ok) throw new Error(`Erro HTTP ${resposta.status}`);
}


/*  INDICADORES (os 4 cartões do topo) */
function atualizarIndicadores() {
  const total    = todosOsContratos.length;
  const ativos   = todosOsContratos.filter(c => c.status === 'Ativo').length;
  const perto    = todosOsContratos.filter(c => c.status === 'PertoDeVencer').length;
  const vencidos = todosOsContratos.filter(c => c.status === 'Vencido').length;

  // Anima os números subindo até o valor correto
  animarNumero(pegar('totalContratos'), total);
  animarNumero(pegar('totalAtivos'), ativos);
  animarNumero(pegar('totalPerto'), perto);
  animarNumero(pegar('totalVencidos'), vencidos);
}

// Faz o número "subir" gradualmente (efeito visual)
function animarNumero(elemento, valorFinal) {
  let valorAtual = 0;
  const incremento = Math.ceil(valorFinal / 20);
  const intervalo = setInterval(() => {
    valorAtual = Math.min(valorAtual + incremento, valorFinal);
    elemento.textContent = valorAtual;
    if (valorAtual >= valorFinal) clearInterval(intervalo);
  }, 30);
}


/* FILTROS */
function aplicarFiltros() {
  const textoBusca = pegar('campoBusca').value.toLowerCase().trim();
  const statusEscolhido = pegar('filtroStatus').value;
  const periodoEscolhido = pegar('filtroPeriodo').value;

  contratosFiltrados = todosOsContratos.filter(contrato => {

    // Verifica se o texto digitado está em algum campo
    const passouBusca = !textoBusca ||
      contrato.empresa?.toLowerCase().includes(textoBusca) ||
      contrato.nome?.toLowerCase().includes(textoBusca) ||
      contrato.responsavel?.toLowerCase().includes(textoBusca);

    // Verifica se o status bate com o filtro
    const passouStatus = !statusEscolhido || contrato.status === statusEscolhido;

    // Verifica se a validade está dentro do período escolhido
    let passouPeriodo = true;
    if (periodoEscolhido) {
      const limiteDias = parseInt(periodoEscolhido);
      const diasParaVencer = contrato.diasParaVencer
       passouPeriodo = diasParaVencer >= 0 && diasParaVencer <= limiteDias;
    }

    return passouBusca && passouStatus && passouPeriodo;
  });

  paginaAtual = 1; // volta para a primeira página ao filtrar
  montarTabela();
}


/* TABELA */
function montarTabela() {
  const corpoTabela = pegar('corpoTabela');
  const total = contratosFiltrados.length;
  const totalPaginas = Math.max(1, Math.ceil(total / CONTRATOS_POR_PAGINA));

  // Garante que a página atual é válida
  paginaAtual = Math.min(paginaAtual, totalPaginas);

  const inicio = (paginaAtual - 1) * CONTRATOS_POR_PAGINA;
  const contratosDaPagina = contratosFiltrados.slice(inicio, inicio + CONTRATOS_POR_PAGINA);

  // Atualiza o texto "Mostrando X–Y de Z contratos"
  pegar('contagemRegistros').textContent =
    `Mostrando ${inicio + 1}–${Math.min(inicio + CONTRATOS_POR_PAGINA, total)} de ${total} contrato${total !== 1 ? 's' : ''}`;
  pegar('infoPagina').textContent = `Página ${paginaAtual} de ${totalPaginas}`;

  // Nenhum resultado encontrado
  if (contratosDaPagina.length === 0) {
    corpoTabela.innerHTML = `
      <tr><td colspan="10">
        <div class="estado-vazio">
          <i class="fa-solid fa-file-circle-exclamation"></i>
          <h3>Nenhum contrato encontrado</h3>
          <p>Tente ajustar os filtros ou cadastrar um novo contrato.</p>
        </div>
      </td></tr>`;
  } else {
    // Monta as linhas da tabela
    corpoTabela.innerHTML = contratosDaPagina.map(c => montarLinha(c)).join('');
    vincularBotoesLinha(); // adiciona os eventos de clique nos botões de cada linha
  }

  montarPaginacao(totalPaginas);
}

// Monta o HTML de uma linha da tabela
function montarLinha(contrato) {
  const dias = contrato.diasParaVencer

  const classeDias = dias < 0 ? 'dias-vencido' :
                     dias <= 3 ? 'dias-critico' :
                     dias <= 7 ? 'dias-atencao' : 'dias-normal';

  const textoDias = dias < 0 ? `${Math.abs(dias)} dias vencido` :
                    dias === 0 ? 'Vence hoje' :
                    `${dias} dias`;

  const classeStatus = contrato.status === 'Ativo' ? 'status-ativo' :
                       contrato.status === 'PertoDeVencer' ? 'status-perto' : 'status-vencido';

  const textoStatus = contrato.status === 'Ativo' ? 'Ativo' :
                      contrato.status === 'PertoDeVencer' ? 'Perto de vencer' : 'Vencido';

  const textoTipoFinanceiro = contrato.tipoFinanceiro === 'Saida' ? 'Saída' : 'Entrada';

  const classeTipoFinanceiro = contrato.tipoFinanceiro === 'Saida'
    ? 'tipo-saida'
    : 'tipo-entrada';

  return `
    <tr>
      <td class="coluna-empresa">${escapar(contrato.empresa)}</td>
      <td class="coluna-contrato">${escapar(contrato.nome)}</td>
      <td>${escapar(contrato.responsavel)}</td>
      <td class="coluna-valor">${formatarDinheiro(contrato.valor)}</td>
      <td>
        <span class="badge-tipo-financeiro ${classeTipoFinanceiro}">
          ${textoTipoFinanceiro}
        </span>
      </td>
      <td>${formatarData(contrato.dataInicio)}</td>
      <td>${formatarData(contrato.validade)}</td>
      <td class="coluna-dias ${classeDias}">${textoDias}</td>
      <td><span class="etiqueta-status ${classeStatus}">${textoStatus}</span></td>
      <td>
        <div class="celula-acoes">
          <button class="botao-acao botao-visualizar" data-id="${contrato.id}" title="Visualizar">
            <i class="fa-solid fa-eye"></i>
          </button>
          <button class="botao-acao botao-editar" data-id="${contrato.id}" title="Editar">
            <i class="fa-solid fa-pen"></i>
          </button>
          <button class="botao-acao botao-excluir" data-id="${contrato.id}" title="Excluir">
            <i class="fa-solid fa-trash"></i>
          </button>
        </div>
      </td>
    </tr>`;
}

// Adiciona os eventos de clique nos botões de cada linha
function vincularBotoesLinha() {
  document.querySelectorAll('.botao-visualizar').forEach(btn =>
    btn.addEventListener('click', () => abrirVisualizacao(parseInt(btn.dataset.id))));
  document.querySelectorAll('.botao-editar').forEach(btn =>
    btn.addEventListener('click', () => abrirEdicao(parseInt(btn.dataset.id))));
  document.querySelectorAll('.botao-excluir').forEach(btn =>
    btn.addEventListener('click', () => confirmarExclusao(parseInt(btn.dataset.id))));
}

// Monta os botões de navegação entre páginas
function montarPaginacao(totalPaginas) {
  const paginacao = pegar('paginacao');

  let html = `<button class="botao-pagina" onclick="irParaPagina(${paginaAtual - 1})" ${paginaAtual === 1 ? 'disabled' : ''}>
    <i class="fa-solid fa-chevron-left"></i>
  </button>`;

  for (let i = 1; i <= totalPaginas; i++) {
    html += `<button class="botao-pagina ${i === paginaAtual ? 'pagina-ativa' : ''}" onclick="irParaPagina(${i})">${i}</button>`;
  }

  html += `<button class="botao-pagina" onclick="irParaPagina(${paginaAtual + 1})" ${paginaAtual === totalPaginas ? 'disabled' : ''}>
    <i class="fa-solid fa-chevron-right"></i>
  </button>`;

  paginacao.innerHTML = html;
}

// Função chamada pelos botões de paginação (precisa ser global)
window.irParaPagina = numeroPagina => {
  paginaAtual = numeroPagina;
  montarTabela();
};


/* JANELAS FLUTUANTES */

// Abre a janela de novo contrato
function abrirJanelaNovoContrato() {
  idEditando = null;
  pegar('tituloJanela').textContent = 'Novo contrato';
  pegar('btnSalvar').textContent = 'Salvar contrato';
  limparFormulario();
  pegar('fundoJanela').classList.add('aberto');
}

// Abre a janela para editar um contrato existente
function abrirEdicao(id) {
  const contrato = todosOsContratos.find(c => c.id === id);
  if (!contrato) return;

  idEditando = id;
  pegar('tituloJanela').textContent = 'Editar contrato';
  pegar('btnSalvar').textContent = 'Atualizar contrato';

  // Preenche os campos com os dados do contrato
  pegar('campoId').value          = contrato.id;
  pegar('campoEmpresa').value     = contrato.empresa?.trim() || '';
  pegar('campoNome').value        = contrato.nome?.trim() || '';
  pegar('campoValor').value       = contrato.valor;
  pegar('campoTipoFinanceiro').value = contrato.tipoFinanceiro || 'Entrada';
  pegar('campoResponsavel').value = contrato.responsavel?.trim() || '';
  pegar('campoInicio').value      = paraFormatoData(contrato.dataInicio);
  pegar('campoValidade').value    = paraFormatoData(contrato.validade);

  pegar('fundoJanela').classList.add('aberto');
}

// Fecha a janela de criar/editar
function fecharJanela() {
  pegar('fundoJanela').classList.remove('aberto');
  limparFormulario();
  idEditando = null;
}

// Abre a janela de visualização dos detalhes
function abrirVisualizacao(id) {
  
function abrirVisualizacao(id) {
  const contrato = todosOsContratos.find(c => c.id === id);
  if (!contrato) return;

  const dias = contrato.diasParaVencer

  const textoDias = dias < 0 ? `${Math.abs(dias)} dias vencido` :
                    dias === 0 ? 'Vence hoje' :
                    `${dias} dias`;

  const classeStatus = contrato.status === 'Ativo' ? 'status-ativo' :
                       contrato.status === 'PertoDeVencer' ? 'status-perto' : 'status-vencido';

  const textoStatus = contrato.status === 'Ativo' ? 'Ativo' :
                      contrato.status === 'PertoDeVencer' ? 'Perto de vencer' : 'Vencido';

  const textoTipoFinanceiro = contrato.tipoFinanceiro === 'Saida' ? 'Saída' : 'Entrada';

  const classeTipoFinanceiro = contrato.tipoFinanceiro === 'Saida'
    ? 'tipo-saida'
    : 'tipo-entrada';

  pegar('gradeDetalhes').innerHTML = `
    <div class="campo-detalhe largura-total">
      <span class="rotulo-detalhe">Empresa</span>
      <span class="valor-detalhe">${escapar(contrato.empresa)}</span>
    </div>

    <div class="campo-detalhe largura-total">
      <span class="rotulo-detalhe">Contrato</span>
      <span class="valor-detalhe">${escapar(contrato.nome)}</span>
    </div>

    <div class="campo-detalhe">
      <span class="rotulo-detalhe">Responsável</span>
      <span class="valor-detalhe">${escapar(contrato.responsavel)}</span>
    </div>

    <div class="campo-detalhe">
      <span class="rotulo-detalhe">Valor</span>
      <span class="valor-detalhe">${formatarDinheiro(contrato.valor)}</span>
    </div>

    <div class="campo-detalhe">
      <span class="rotulo-detalhe">Tipo financeiro</span>
      <span class="badge-tipo-financeiro ${classeTipoFinanceiro}" style="margin-top:4px">
        ${textoTipoFinanceiro}
      </span>
    </div>

    <div class="campo-detalhe">
      <span class="rotulo-detalhe">Data de início</span>
      <span class="valor-detalhe">${formatarData(contrato.dataInicio)}</span>
    </div>

    <div class="campo-detalhe">
      <span class="rotulo-detalhe">Validade</span>
      <span class="valor-detalhe">${formatarData(contrato.validade)}</span>
    </div>

    <div class="campo-detalhe">
      <span class="rotulo-detalhe">Dias para vencer</span>
      <span class="valor-detalhe">${textoDias}</span>
    </div>

    <div class="campo-detalhe">
      <span class="rotulo-detalhe">Status</span>
      <span class="etiqueta-status ${classeStatus}" style="margin-top:4px">${textoStatus}</span>
    </div>
  `;

  pegar('fundoVisualizacao').classList.add('aberto');
}
  
}

// Fecha a janela de visualização
function fecharVisualizacao() {
  pegar('fundoVisualizacao').classList.remove('aberto');
}


/* FORMULÁRIO */

// Executado quando o formulário é enviado
async function enviarFormulario(evento) {
  evento.preventDefault(); // evita que a página recarregue
  if (!validarFormulario()) return;

  // Monta o objeto com os dados do formulário
  const dados = {
  empresa: pegar('campoEmpresa').value.trim(),
  nome: pegar('campoNome').value.trim(),
  valor: parseFloat(pegar('campoValor').value),
  responsavel: pegar('campoResponsavel').value.trim(),
  tipoFinanceiro: pegar('campoTipoFinanceiro').value,
  dataInicio: pegar('campoInicio').value,
  validade: pegar('campoValidade').value
  };

  const botao = pegar('btnSalvar');
  botao.disabled = true;
  botao.textContent = 'Salvando…';

  try {
    if (idEditando) {
      // Atualiza contrato existente
      const atualizado = await atualizarContrato(idEditando, dados);
      const posicao = todosOsContratos.findIndex(c => c.id === idEditando);
      if (posicao > -1) todosOsContratos[posicao] = atualizado;
      mostrarMensagem('Contrato atualizado com sucesso!', 'sucesso');
    } else {
      // Cria novo contrato
      const criado = await criarContrato(dados);
      todosOsContratos.push(criado);
      mostrarMensagem('Contrato cadastrado com sucesso!', 'sucesso');
    }

    fecharJanela();
    aplicarFiltros();
    atualizarIndicadores();

  } catch (erro) {
    mostrarMensagem('Erro ao salvar contrato. Verifique a API.', 'erro');
  } finally {
    botao.disabled = false;
    botao.textContent = idEditando ? 'Atualizar contrato' : 'Salvar contrato';
  }
}

// Verifica se todos os campos obrigatórios foram preenchidos
function validarFormulario() {
  let valido = true;
  const campos = ['campoEmpresa','campoNome','campoValor','campoTipoFinanceiro','campoResponsavel','campoInicio','campoValidade'];

  campos.forEach(id => {
    const campo = pegar(id);
    if (!campo.value.trim()) {
      campo.classList.add('invalido'); // marca o campo com borda vermelha
      valido = false;
    } else {
      campo.classList.remove('invalido');
    }
  });

  if (!valido) mostrarMensagem('Preencha todos os campos obrigatórios.', 'aviso');
  return valido;
}

// Limpa todos os campos do formulário
function limparFormulario() {
  pegar('formularioContrato').reset();
  document.querySelectorAll('.campo-input.invalido').forEach(c => c.classList.remove('invalido'));
}


/* EXCLUSÃO */
async function confirmarExclusao(id) {
  const contrato = todosOsContratos.find(c => c.id === id);

  // Pede confirmação antes de excluir
  const confirmou = confirm(`Excluir o contrato "${contrato?.nome?.trim()}"?\n\nEsta ação não pode ser desfeita.`);
  if (!confirmou) return;

  try {
    await excluirContrato(id);
    todosOsContratos = todosOsContratos.filter(c => c.id !== id); // remove da lista local
    aplicarFiltros();
    atualizarIndicadores();
    mostrarMensagem('Contrato excluído com sucesso.', 'sucesso');
  } catch {
    mostrarMensagem('Erro ao excluir contrato.', 'erro');
  }
}


/* EXPORTAÇÃO DE RELATÓRIOS */

// Exporta os dados em formato CSV (abre no Excel)
function exportarCSV() {
  const dados = contratosFiltrados.length ? contratosFiltrados : todosOsContratos;
  if (!dados.length) { mostrarMensagem('Sem dados para exportar.', 'aviso'); return; }

  const cabecalho = ['ID','Empresa','Contrato','Responsável','Valor (R$)','Tipo Financeiro','Data Início','Validade','Dias p/ Vencer','Status'];
  const linhas = dados.map(c => [
    c.id,
    `"${(c.empresa||'').trim()}"`,
    `"${(c.nome||'').trim()}"`,
    `"${(c.responsavel||'').trim()}"`,
    c.valor,
    c.tipoFinanceiro === 'Saida' ? 'Saída' : 'Entrada',
    formatarData(c.dataInicio),
    formatarData(c.validade),
    c.diasParaVencer,
    c.status === 'PertoDeVencer' ? 'Perto de vencer' : c.status
  ]);

  const conteudoCSV = [cabecalho.join(','), ...linhas.map(l => l.join(','))].join('\n');
  // \uFEFF é o BOM (byte order mark) que faz o Excel abrir o arquivo com acentos corretos
  const arquivo = new Blob(['\uFEFF' + conteudoCSV], { type: 'text/csv;charset=utf-8' });
  baixarArquivo(arquivo, `ContractFlow_relatorio_${dataHoje()}.csv`);
  mostrarMensagem('Relatório CSV exportado!', 'sucesso');
}

// Exporta os dados em formato JSON
function exportarJSON() {
  const dados = contratosFiltrados.length ? contratosFiltrados : todosOsContratos;
  if (!dados.length) { mostrarMensagem('Sem dados para exportar.', 'aviso'); return; }

  const relatorio = {
    empresa: 'Contract Flow',
    geradoEm: new Date().toISOString(),
    totalContratos: dados.length,
    resumo: {
      ativos: dados.filter(c => c.status === 'Ativo').length,
      pertoDeVencer: dados.filter(c => c.status === 'PertoDeVencer').length,
      vencidos: dados.filter(c => c.status === 'Vencido').length,
      valorTotal: dados.reduce((soma, c) => soma + c.valor, 0)
    },
    contratos: dados
  };

  const arquivo = new Blob([JSON.stringify(relatorio, null, 2)], { type: 'application/json' });
  baixarArquivo(arquivo, `ContractFlow_relatorio_${dataHoje()}.json`);
  mostrarMensagem('Relatório JSON exportado!', 'sucesso');
}

// Cria um link temporário e clica nele para baixar o arquivo
function baixarArquivo(blob, nomeArquivo) {
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = nomeArquivo;
  link.click();
  URL.revokeObjectURL(url); // libera a memória
}

// Retorna a data de hoje no formato AAAA-MM-DD
function dataHoje() {
  return new Date().toISOString().slice(0, 10);
}


/* FUNÇÕES AUXILIARES */

// Formata um número para moeda brasileira (R$ 2.500,00)
function formatarDinheiro(valor) {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor ?? 0);
}

// Formata uma data do formato "2026-01-10" para "10/01/2026"
function formatarData(data) {
  if (!data) return '–';
  const texto = typeof data === 'string' ? data.slice(0, 10) : data;
  const [ano, mes, dia] = texto.split('-');
  return `${dia}/${mes}/${ano}`;
}

// Converte "2026-01-10" para o formato aceito pelo campo de data do HTML
function paraFormatoData(data) {
  if (!data) return '';
  return typeof data === 'string' ? data.slice(0, 10) : data;
}

// Escapa caracteres especiais para evitar problemas de segurança no HTML
function escapar(texto) {
  if (!texto) return '';
  return texto.trim()
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;');
}

// Mostra as linhas de carregamento na tabela
function mostrarCarregamento() {
  pegar('corpoTabela').innerHTML = Array(5).fill(`
    <tr class="linha-carregando"><td colspan="10"><div class="animacao-carregamento"></div></td></tr>
  `).join('');
}

// Mostra uma mensagem de erro na tabela
function mostrarErroNaTabela(texto) {
  pegar('corpoTabela').innerHTML = `
    <tr><td colspan="10">
      <div class="estado-vazio">
        <i class="fa-solid fa-circle-exclamation"></i>
        <h3>Erro de conexão</h3>
        <p>${texto}</p>
      </div>
    </td></tr>`;
  pegar('contagemRegistros').textContent = 'Erro ao carregar dados';
  ['totalContratos','totalAtivos','totalPerto','totalVencidos'].forEach(id => pegar(id).textContent = '–');
}


/*
    MENSAGENS DE CONFIRMAÇÃO
   (aparecem no canto inferior direito) */
function mostrarMensagem(texto, tipo = 'info') {
  const icones = {
    sucesso: '<i class="fa-solid fa-check"></i>',
    erro:    '<i class="fa-solid fa-xmark"></i>',
    aviso:   '<i class="fa-solid fa-triangle-exclamation"></i>',
    info:    '<i class="fa-solid fa-circle-info"></i>'
  };

  const mensagem = document.createElement('div');
  mensagem.className = `mensagem mensagem-${tipo}`;
  mensagem.innerHTML = `${icones[tipo] || icones.info} <span>${texto}</span>`;

  pegar('areaMensagens').appendChild(mensagem);

  // Remove automaticamente após 3,5 segundos
  setTimeout(() => {
    mensagem.style.animation = 'sair .25s ease forwards';
    setTimeout(() => mensagem.remove(), 260);
  }, 3500);
}


/* VINCULAR EVENTOS
   (conecta os cliques aos elementos do HTML) */
function vincularEventos() {

  // Botão "Novo contrato"
  pegar('btnNovoContrato').addEventListener('click', abrirJanelaNovoContrato);

  // Botões para fechar a janela de criar/editar
  pegar('btnFecharJanela').addEventListener('click', fecharJanela);
  pegar('btnCancelar').addEventListener('click', fecharJanela);
  // Clique no fundo escuro também fecha
  pegar('fundoJanela').addEventListener('click', e => {
    if (e.target === pegar('fundoJanela')) fecharJanela();
  });

  // Envio do formulário
  pegar('formularioContrato').addEventListener('submit', enviarFormulario);
  // Remove o marcador de inválido ao digitar
  document.querySelectorAll('.campo-input').forEach(campo =>
    campo.addEventListener('input', () => campo.classList.remove('invalido')));

  // Botões da janela de visualização
  pegar('btnFecharVisualizacao').addEventListener('click', fecharVisualizacao);
  pegar('btnFecharDetalhes').addEventListener('click', fecharVisualizacao);
  pegar('fundoVisualizacao').addEventListener('click', e => {
    if (e.target === pegar('fundoVisualizacao')) fecharVisualizacao();
  });
  pegar('btnEditarDoDetalhes').addEventListener('click', () => {
    if (contratoVisualizando) {
      fecharVisualizacao();
      abrirEdicao(contratoVisualizando.id);
    }
  });

  // Filtros (com atraso de 300ms na busca para não buscar a cada letra)
  pegar('campoBusca').addEventListener('input', comAtraso(aplicarFiltros, 300));
  pegar('filtroStatus').addEventListener('change', aplicarFiltros);
  pegar('filtroPeriodo').addEventListener('change', aplicarFiltros);
  pegar('btnLimparFiltros').addEventListener('click', () => {
    pegar('campoBusca').value = '';
    pegar('filtroStatus').value = '';
    pegar('filtroPeriodo').value = '';
    aplicarFiltros();
  });

  // Botão de atualizar
  pegar('btnAtualizar').addEventListener('click', carregarContratos);

  // Botões de exportação
  pegar('btnExportarCSV').addEventListener('click', exportarCSV);
  pegar('btnExportarJSON').addEventListener('click', exportarJSON);
  pegar('btnImprimir').addEventListener('click', () => window.print());

  // Tecla ESC fecha qualquer janela aberta
  document.addEventListener('keydown', e => {
    if (e.key === 'Escape') { fecharJanela(); fecharVisualizacao(); }
  });
}

// Espera um tempo antes de executar a função (evita chamar muitas vezes seguidas)
function comAtraso(funcao, tempo) {
  let temporizador;
  return (...args) => {
    clearTimeout(temporizador);
    temporizador = setTimeout(() => funcao(...args), tempo);
  };
}


/* PAINEL INFORMATIVO — FUNCIONALIDADES FUTURAS
   Aparece quando o usuário clica nos itens do
   menu lateral e no sino de notificações */

// Dados de cada funcionalidade futura
const informacoesFuncionalidades = {
  contratos: {
    icone: 'fa-solid fa-file-contract',
    cor: '#1a56db',
    corFundo: '#ebf1ff',
    titulo: 'Módulo de Contratos',
    descricao: 'Página dedicada para visualizar e gerenciar todos os contratos com filtros avançados e visualizações detalhadas.',
    itens: [
      'Visualização em lista e em cards',
      'Filtros avançados por empresa, valor e período',
      'Histórico de alterações de cada contrato',
      'Anexo de documentos em PDF'
    ]
  },
  vencimentos: {
    icone: 'fa-solid fa-clock',
    cor: '#c27803',
    corFundo: '#fdf3c0',
    titulo: 'Módulo de Vencimentos',
    descricao: 'Calendário e linha do tempo com todos os contratos ordenados por data de vencimento.',
    itens: [
      'Calendário visual de vencimentos',
      'Alertas automáticos por e-mail',
      'Linha do tempo interativa',
      'Renovação de contratos com um clique'
    ]
  },
  empresas: {
    icone: 'fa-solid fa-building',
    cor: '#057a55',
    corFundo: '#def7ec',
    titulo: 'Módulo de Empresas',
    descricao: 'Cadastro completo das empresas parceiras com todos os contratos vinculados a cada uma.',
    itens: [
      'Cadastro de empresas com CNPJ',
      'Histórico de contratos por empresa',
      'Contatos e responsáveis cadastrados',
      'Relatório de faturamento por empresa'
    ]
  },
  relatorios: {
    icone: 'fa-solid fa-chart-bar',
    cor: '#6b21a8',
    corFundo: '#f3e8ff',
    titulo: 'Módulo de Relatórios',
    descricao: 'Painel analítico com gráficos e métricas detalhadas sobre todos os contratos da empresa.',
    itens: [
      'Gráficos de contratos por status',
      'Evolução de faturamento mensal',
      'Exportação em PDF com gráficos',
      'Comparativo entre períodos'
    ]
  },
  notificacoes: {
    icone: 'fa-solid fa-bell',
    cor: '#c81e1e',
    corFundo: '#fde8e8',
    titulo: 'Central de Notificações',
    descricao: 'Sistema de alertas automáticos para manter a equipe informada sobre eventos importantes.',
    itens: [
      'Alertas de contratos próximos ao vencimento',
      'Notificações de novos cadastros',
      'Histórico de todas as notificações',
      'Configuração de preferências de alerta'
    ]
  }
};

// Abre o painel com as informações da funcionalidade clicada
function abrirPainelInfo(chave) {
  const info = informacoesFuncionalidades[chave];
  if (!info) return;

  // Atualiza o ícone e sua cor
  const iconeEl = pegar('painelInfoIcone');
  iconeEl.innerHTML = `<i class="${info.icone}"></i>`;
  iconeEl.style.background = info.corFundo;
  iconeEl.style.color = info.cor;

  // Atualiza o título e descrição
  pegar('painelInfoTitulo').textContent = info.titulo;
  pegar('painelInfoDescricao').textContent = info.descricao;

  // Monta a lista de funcionalidades
  const lista = pegar('painelInfoLista');
  lista.innerHTML = info.itens.map(item => `
    <li>
      <i class="fa-solid fa-check"></i>
      ${item}
    </li>
  `).join('');

  // Abre o painel
  pegar('fundoPainelInfo').classList.add('aberto');
}

// Fecha o painel
function fecharPainelInfo() {
  pegar('fundoPainelInfo').classList.remove('aberto');
}

// Vincula os eventos do painel informativo
function vincularEventosPainelInfo() {

  // Itens do menu lateral (exceto o Dashboard que já está ativo)
  const mapaMenus = {
    'Contratos':   'contratos',
    'Vencimentos': 'vencimentos',
    'Empresas':    'empresas',
    'Relatórios':  'relatorios'
  };

  document.querySelectorAll('.item-menu:not(.ativo)').forEach(link => {
    link.addEventListener('click', e => {
      e.preventDefault();
      // Descobre qual menu foi clicado pelo texto
      const texto = link.textContent.trim();
      const chave = mapaMenus[texto];
      if (chave) abrirPainelInfo(chave);
    });
  });

  // Sino de notificações
  pegar('btnNotificacoes').addEventListener('click', () => {
    abrirPainelInfo('notificacoes');
  });

  // Botões de fechar o painel
  pegar('btnFecharPainelInfo').addEventListener('click', fecharPainelInfo);
  pegar('btnFecharPainelInfoOk').addEventListener('click', fecharPainelInfo);
  pegar('fundoPainelInfo').addEventListener('click', e => {
    if (e.target === pegar('fundoPainelInfo')) fecharPainelInfo();
  });
}

// Chama a função de vincular ao carregar a página
document.addEventListener('DOMContentLoaded', vincularEventosPainelInfo);