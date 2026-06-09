const API_URL = "http://localhost:5181";

const listaMateriais = document.getElementById("listaMateriais");
const statusLista = document.getElementById("statusLista");

const totalMateriais = document.getElementById("totalMateriais");
const totalAtivos = document.getElementById("totalAtivos");
const totalInativos = document.getElementById("totalInativos");
const totalUnidadesEstoque = document.getElementById("totalUnidadesEstoque");
const valorInvestido = document.getElementById("valorInvestido");
const valorVendaEstimado = document.getElementById("valorVendaEstimado");
const lucroEstimado = document.getElementById("lucroEstimado");
const roiPercentual = document.getElementById("roiPercentual");
const tabelaDashboard = document.getElementById("tabelaDashboard");
const tabelaItensLotes = document.getElementById("tabelaItensLotes");

let materiaisCache = [];

function irParaDashboard() {
  carregarDashboard();

  document.getElementById("dashboard").scrollIntoView({
    behavior: "smooth",
    block: "start"
  });
}

function irParaCadastro() {
  document.getElementById("cadastro").scrollIntoView({
    behavior: "smooth",
    block: "start"
  });
}

function irParaTabelaItens() {
  document.getElementById("tabela-itens").scrollIntoView({
    behavior: "smooth",
    block: "start"
  });
}

function irParaMateriais() {
  listarMateriais();

  document.getElementById("materiais").scrollIntoView({
    behavior: "smooth",
    block: "start"
  });
}

async function atualizarTudo() {
  await listarMateriais();
  await carregarDashboard();
}

async function listarMateriais() {
  try {
    statusLista.textContent = "Carregando materiais...";

    const response = await fetch(`${API_URL}/api/materiais`);

    if (!response.ok) {
      throw new Error("Erro na resposta da API");
    }

    const materiais = await response.json();

    materiaisCache = materiais;

    renderizarMateriais(materiais);
    atualizarCardsBasicos(materiais);
    preencherSelectMateriais(materiais);
    renderizarTabelaItensLotes(materiais);

    statusLista.textContent = `${materiais.length} material(is) encontrado(s)`;

    await carregarDashboard();
  } catch (error) {
    statusLista.textContent = "Erro ao carregar materiais";
    listaMateriais.innerHTML = `
      <p class="empty">
        Não foi possível carregar os materiais. Verifique se a API está rodando e se a porta no script.js está correta.
      </p>
    `;
    console.error(error);
  }
}

async function carregarDashboard() {
  try {
    const response = await fetch(`${API_URL}/api/dashboard`);

    if (!response.ok) {
      throw new Error("Erro ao carregar dashboard");
    }

    const dados = await response.json();

    totalMateriais.textContent = dados.totalMateriais;
    totalAtivos.textContent = dados.totalAtivos;
    totalInativos.textContent = dados.totalInativos;
    totalUnidadesEstoque.textContent = dados.totalUnidadesEstoque;

    valorInvestido.textContent = formatarMoeda(dados.valorInvestido);
    valorVendaEstimado.textContent = formatarMoeda(dados.valorVendaEstimado);
    lucroEstimado.textContent = formatarMoeda(dados.lucroEstimado);
    roiPercentual.textContent = `${Number(dados.roiPercentual).toFixed(2)}%`;

    renderizarTabelaDashboard(dados.materiais);
  } catch (error) {
    console.error("Erro ao carregar dashboard:", error);

    tabelaDashboard.innerHTML = `
      <tr>
        <td colspan="7">Erro ao carregar dashboard. Verifique se a rota /api/dashboard está funcionando.</td>
      </tr>
    `;
  }
}

async function listarAtivos() {
  try {
    statusLista.textContent = "Carregando materiais ativos...";

    const response = await fetch(`${API_URL}/api/materiais/ativos`);

    if (!response.ok) {
      throw new Error("Erro na resposta da API");
    }

    const materiais = await response.json();

    renderizarMateriais(materiais);
    renderizarTabelaItensLotes(materiais);

    statusLista.textContent = `${materiais.length} material(is) ativo(s)`;

    document.getElementById("materiais").scrollIntoView({
      behavior: "smooth",
      block: "start"
    });
  } catch (error) {
    statusLista.textContent = "Erro ao carregar ativos";
    console.error(error);
  }
}

async function listarInativos() {
  try {
    statusLista.textContent = "Carregando materiais inativos...";

    const response = await fetch(`${API_URL}/api/materiais/inativos`);

    if (!response.ok) {
      throw new Error("Erro na resposta da API");
    }

    const materiais = await response.json();

    renderizarMateriais(materiais);
    renderizarTabelaItensLotes(materiais);

    statusLista.textContent = `${materiais.length} material(is) inativo(s)`;

    document.getElementById("materiais").scrollIntoView({
      behavior: "smooth",
      block: "start"
    });
  } catch (error) {
    statusLista.textContent = "Erro ao carregar inativos";
    console.error(error);
  }
}

function atualizarCardsBasicos(materiais) {
  totalMateriais.textContent = materiais.length;
  totalAtivos.textContent = materiais.filter(material => material.ativo).length;
  totalInativos.textContent = materiais.filter(material => !material.ativo).length;

  const unidades = materiais.reduce((total, material) => {
    return total + calcularEstoqueTotal(material);
  }, 0);

  totalUnidadesEstoque.textContent = unidades;
}

function preencherSelectMateriais(materiais) {
  const select = document.getElementById("saidaMaterialId");

  select.innerHTML = `<option value="">Selecione um material</option>`;

  materiais
    .filter(material => material.ativo)
    .forEach(material => {
      const estoqueTotal = calcularEstoqueTotal(material);

      const option = document.createElement("option");
      option.value = material.id;
      option.textContent = `${material.nome} | Estoque: ${estoqueTotal}`;

      select.appendChild(option);
    });
}

function renderizarTabelaDashboard(materiais) {
  tabelaDashboard.innerHTML = "";

  if (!materiais || materiais.length === 0) {
    tabelaDashboard.innerHTML = `
      <tr>
        <td colspan="7">Nenhum dado financeiro encontrado.</td>
      </tr>
    `;
    return;
  }

  materiais.forEach(material => {
    const lucroClasse = Number(material.lucroEstimado) >= 0 ? "positive" : "negative";
    const roiClasse = Number(material.roiPercentual) >= 0 ? "positive" : "negative";

    const linha = document.createElement("tr");

    linha.innerHTML = `
      <td>${material.nome}</td>
      <td>${material.categoria}</td>
      <td>${material.estoqueTotal}</td>
      <td>${formatarMoeda(material.valorInvestido)}</td>
      <td>${formatarMoeda(material.valorVendaEstimado)}</td>
      <td class="${lucroClasse}">${formatarMoeda(material.lucroEstimado)}</td>
      <td class="${roiClasse}">${Number(material.roiPercentual).toFixed(2)}%</td>
    `;

    tabelaDashboard.appendChild(linha);
  });
}

function renderizarTabelaItensLotes(materiais) {
  tabelaItensLotes.innerHTML = "";

  if (!materiais || materiais.length === 0) {
    tabelaItensLotes.innerHTML = `
      <tr>
        <td colspan="11">Nenhum item encontrado.</td>
      </tr>
    `;
    return;
  }

  materiais.forEach(material => {
    if (!material.lotes || material.lotes.length === 0) {
      const linha = document.createElement("tr");

      linha.innerHTML = `
        <td>${material.id}</td>
        <td>${material.nome}</td>
        <td>${material.categoria}</td>
        <td>
          <span class="status-text ${material.ativo ? "status-ativo" : "status-inativo"}">
            ${material.ativo ? "Ativo" : "Inativo"}
          </span>
        </td>
        <td>-</td>
        <td>Sem lote</td>
        <td>0</td>
        <td>-</td>
        <td>-</td>
        <td>${formatarMoeda(0)}</td>
        <td>${formatarMoeda(0)}</td>
      `;

      tabelaItensLotes.appendChild(linha);
      return;
    }

    material.lotes.forEach(lote => {
      const linha = document.createElement("tr");

      linha.innerHTML = `
        <td>${material.id}</td>
        <td>${material.nome}</td>
        <td>${material.categoria}</td>
        <td>
          <span class="status-text ${material.ativo ? "status-ativo" : "status-inativo"}">
            ${material.ativo ? "Ativo" : "Inativo"}
          </span>
        </td>
        <td>${lote.id}</td>
        <td>${lote.codigo}</td>
        <td>${lote.quantidade}</td>
        <td>${formatarData(lote.dataEntrada)}</td>
        <td>${lote.dataVencimento ? formatarData(lote.dataVencimento) : "Sem vencimento"}</td>
        <td>${formatarMoeda(lote.valorCusto)}</td>
        <td>${formatarMoeda(lote.valorVenda)}</td>
      `;

      tabelaItensLotes.appendChild(linha);
    });
  });
}

function renderizarMateriais(materiais) {
  listaMateriais.innerHTML = "";

  if (!materiais || materiais.length === 0) {
    listaMateriais.innerHTML = `
      <p class="empty">Nenhum material encontrado.</p>
    `;
    return;
  }

  materiais.forEach(material => {
    const estoqueTotal = calcularEstoqueTotal(material);
    const financeiro = calcularFinanceiroMaterial(material);

    const lotesHtml = material.lotes && material.lotes.length > 0
      ? material.lotes.map(lote => `
          <div class="lote">
            <strong>Lote:</strong> ${lote.codigo}
            <br />
            <strong>Quantidade:</strong> ${lote.quantidade}
            <br />
            <strong>Entrada:</strong> ${formatarData(lote.dataEntrada)}
            <br />
            <strong>Vencimento:</strong> ${lote.dataVencimento ? formatarData(lote.dataVencimento) : "Sem vencimento"}
            <br />
            <strong>Custo unitário:</strong> ${formatarMoeda(lote.valorCusto)}
            <br />
            <strong>Venda unitária:</strong> ${formatarMoeda(lote.valorVenda)}
          </div>
        `).join("")
      : `<p class="empty">Sem lotes cadastrados.</p>`;

    const item = document.createElement("div");
    item.className = "material-item";

    item.innerHTML = `
      <div class="material-top">
        <h4>${material.nome}</h4>

        <span class="badge ${material.ativo ? "active" : "inactive"}">
          ${material.ativo ? "Ativo" : "Inativo"}
        </span>
      </div>

      <div class="material-info">
        <strong>ID:</strong> ${material.id}
        <br />
        <strong>Categoria:</strong> ${material.categoria}
      </div>

      <div class="stock">
        Estoque total: ${estoqueTotal}
      </div>

      <div class="finance-info">
        <strong>Investido:</strong> ${formatarMoeda(financeiro.valorInvestido)}
        <br />
        <strong>Venda estimada:</strong> ${formatarMoeda(financeiro.valorVendaEstimado)}
        <br />
        <strong>Lucro estimado:</strong> ${formatarMoeda(financeiro.lucroEstimado)}
        <br />
        <strong>ROI:</strong> ${financeiro.roiPercentual.toFixed(2)}%
      </div>

      <div class="lotes">
        ${lotesHtml}
      </div>

      <div class="actions">
        <button class="edit-btn" onclick="prepararEdicao(${material.id})">Editar material</button>
        <button onclick="removerMaterial(${material.id})">Remover material</button>
      </div>
    `;

    listaMateriais.appendChild(item);
  });
}

document.getElementById("formCadastro").addEventListener("submit", async function(event) {
  event.preventDefault();

  const modoEdicao = document.getElementById("modoEdicao").value === "true";
  const idOriginalEdicao = Number(document.getElementById("idOriginalEdicao").value);

  const id = Number(document.getElementById("id").value);
  const nome = document.getElementById("nome").value.trim();
  const categoria = document.getElementById("categoria").value.trim();
  const ativo = document.getElementById("ativo").value === "true";

  const loteId = Number(document.getElementById("loteId").value);
  const codigoLote = document.getElementById("codigoLote").value.trim();
  const quantidade = Number(document.getElementById("quantidade").value);
  const valorCusto = Number(document.getElementById("valorCusto").value);
  const valorVenda = Number(document.getElementById("valorVenda").value);
  const dataEntrada = document.getElementById("dataEntrada").value;
  const dataVencimento = document.getElementById("dataVencimento").value || null;

  if (!nome || !categoria || !codigoLote) {
    alert("Preencha todos os campos obrigatórios.");
    return;
  }

  if (quantidade < 0) {
    alert("A quantidade do lote não pode ser negativa.");
    return;
  }

  if (valorCusto < 0 || valorVenda < 0) {
    alert("Os valores de custo e venda não podem ser negativos.");
    return;
  }

  const material = {
    id: id,
    nome: nome,
    categoria: categoria,
    ativo: ativo,
    lotes: [
      {
        id: loteId,
        codigo: codigoLote,
        quantidade: quantidade,
        dataEntrada: dataEntrada,
        dataVencimento: dataVencimento,
        valorCusto: valorCusto,
        valorVenda: valorVenda
      }
    ]
  };

  try {
    const url = modoEdicao
      ? `${API_URL}/api/materiais/${idOriginalEdicao}`
      : `${API_URL}/api/materiais`;

    const metodo = modoEdicao ? "PUT" : "POST";

    const response = await fetch(url, {
      method: metodo,
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(material)
    });

    if (!response.ok) {
      alert(modoEdicao ? "Erro ao editar material." : "Erro ao cadastrar material.");
      return;
    }

    alert(modoEdicao ? "Material editado com sucesso!" : "Material cadastrado com sucesso!");

    cancelarEdicao();
    await atualizarTudo();
  } catch (error) {
    alert("Erro ao conectar com a API.");
    console.error(error);
  }
});

document.getElementById("formSaida").addEventListener("submit", async function(event) {
  event.preventDefault();

  const materialId = Number(document.getElementById("saidaMaterialId").value);
  const quantidade = Number(document.getElementById("saidaQuantidade").value);

  if (!materialId) {
    alert("Selecione um material para registrar a saída.");
    return;
  }

  if (quantidade <= 0) {
    alert("A quantidade de saída precisa ser maior que zero.");
    return;
  }

  const saida = {
    materialId: materialId,
    quantidade: quantidade
  };

  try {
    const response = await fetch(`${API_URL}/api/materiais/saida`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(saida)
    });

    const contentType = response.headers.get("content-type");

    let resultado;

    if (contentType && contentType.includes("application/json")) {
      resultado = await response.json();
    } else {
      resultado = await response.text();
    }

    if (!response.ok) {
      document.getElementById("resultadoSaida").textContent =
        typeof resultado === "string" ? resultado : "Erro ao registrar saída.";
      return;
    }

    document.getElementById("resultadoSaida").textContent =
      `Saída realizada: ${resultado.quantidadeRetirada} unidade(s) de ${resultado.material}.`;

    this.reset();
    await atualizarTudo();
  } catch (error) {
    document.getElementById("resultadoSaida").textContent =
      "Erro ao conectar com a API.";

    console.error(error);
  }
});

function prepararEdicao(id) {
  const material = materiaisCache.find(m => m.id === id);

  if (!material) {
    alert("Material não encontrado para edição.");
    return;
  }

  const primeiroLote = material.lotes && material.lotes.length > 0
    ? material.lotes[0]
    : null;

  document.getElementById("modoEdicao").value = "true";
  document.getElementById("idOriginalEdicao").value = material.id;

  document.getElementById("id").value = material.id;
  document.getElementById("nome").value = material.nome;
  document.getElementById("categoria").value = material.categoria;
  document.getElementById("ativo").value = material.ativo ? "true" : "false";

  if (primeiroLote) {
    document.getElementById("loteId").value = primeiroLote.id;
    document.getElementById("codigoLote").value = primeiroLote.codigo;
    document.getElementById("quantidade").value = primeiroLote.quantidade;
    document.getElementById("valorCusto").value = primeiroLote.valorCusto;
    document.getElementById("valorVenda").value = primeiroLote.valorVenda;
    document.getElementById("dataEntrada").value = formatarDataParaInput(primeiroLote.dataEntrada);
    document.getElementById("dataVencimento").value = primeiroLote.dataVencimento
      ? formatarDataParaInput(primeiroLote.dataVencimento)
      : "";
  }

  document.getElementById("btnSalvarMaterial").textContent = "Salvar alterações";
  document.getElementById("btnCancelarEdicao").classList.remove("hidden");

  document.getElementById("cadastro").scrollIntoView({
    behavior: "smooth",
    block: "start"
  });
}

function cancelarEdicao() {
  document.getElementById("formCadastro").reset();

  document.getElementById("modoEdicao").value = "false";
  document.getElementById("idOriginalEdicao").value = "";

  document.getElementById("btnSalvarMaterial").textContent = "Cadastrar material";
  document.getElementById("btnCancelarEdicao").classList.add("hidden");
}

async function removerMaterial(id) {
  const confirmar = confirm("Deseja remover este material?");

  if (!confirmar) {
    return;
  }

  try {
    const response = await fetch(`${API_URL}/api/materiais/${id}`, {
      method: "DELETE"
    });

    if (!response.ok) {
      alert("Erro ao remover material.");
      return;
    }

    alert("Material removido com sucesso!");
    await atualizarTudo();
  } catch (error) {
    alert("Erro ao conectar com a API.");
    console.error(error);
  }
}

function calcularEstoqueTotal(material) {
  if (!material.lotes || material.lotes.length === 0) {
    return 0;
  }

  return material.lotes.reduce((total, lote) => {
    return total + Number(lote.quantidade || 0);
  }, 0);
}

function calcularFinanceiroMaterial(material) {
  if (!material.lotes || material.lotes.length === 0) {
    return {
      valorInvestido: 0,
      valorVendaEstimado: 0,
      lucroEstimado: 0,
      roiPercentual: 0
    };
  }

  const valorInvestido = material.lotes.reduce((total, lote) => {
    return total + Number(lote.quantidade || 0) * Number(lote.valorCusto || 0);
  }, 0);

  const valorVendaEstimado = material.lotes.reduce((total, lote) => {
    return total + Number(lote.quantidade || 0) * Number(lote.valorVenda || 0);
  }, 0);

  const lucroEstimado = valorVendaEstimado - valorInvestido;

  const roiPercentual = valorInvestido > 0
    ? (lucroEstimado / valorInvestido) * 100
    : 0;

  return {
    valorInvestido,
    valorVendaEstimado,
    lucroEstimado,
    roiPercentual
  };
}

function formatarMoeda(valor) {
  return Number(valor || 0).toLocaleString("pt-BR", {
    style: "currency",
    currency: "BRL"
  });
}

function formatarData(data) {
  if (!data) {
    return "-";
  }

  const dataObj = new Date(data);

  return dataObj.toLocaleDateString("pt-BR", {
    timeZone: "UTC"
  });
}

function formatarDataParaInput(data) {
  if (!data) {
    return "";
  }

  return data.split("T")[0];
}

atualizarTudo();