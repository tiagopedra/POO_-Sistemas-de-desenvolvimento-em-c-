const API_URL = 'http://localhost:5000/api';
let clientesCacheados = [];

const tipos = {
    cliente: { plural: 'clientes', singular: 'Cliente' },
    fornecedor: { plural: 'fornecedores', singular: 'Fornecedor' },
    peca: { plural: 'pecas', singular: 'Peça' },
    venda: { plural: 'vendas', singular: 'Venda' }
};

function mostrarAba(aba, event) {
    if (event) event.preventDefault();
    
    document.querySelectorAll('.tab-content, .tab-button').forEach(el => 
        el.classList.remove('active')
    );
    
    document.getElementById(aba).classList.add('active');
    event?.target?.classList.add('active');
    
    if (aba === 'dashboard') setTimeout(() => atualizarDashboard(), 100);
    else if (aba === 'listagem') setTimeout(() => atualizarListagens(), 100);
    else if (aba === 'cadastro') setTimeout(() => carregarClientesEPecas(), 100);
}

function mudarFormulario(tipo) {
    if (!tipo) return;
    
    document.querySelectorAll('.form-section').forEach(el => 
        el.classList.remove('active')
    );
    
    document.getElementById('form' + tipo.charAt(0).toUpperCase() + tipo.slice(1)).classList.add('active');
    
    if (tipo === 'venda') carregarClientesEPecas();
}

async function carregarClientesEPecas() {
    try {
        const [clientesRes, pecasRes] = await Promise.all([
            fetch(`${API_URL}/clientes`),
            fetch(`${API_URL}/pecas`)
        ]);
        
        const clientes = await clientesRes.json();
        const pecas = await pecasRes.json();
        clientesCacheados = clientes;
        
        const clienteSelect = document.getElementById('clienteSelect');
        const pecaSelect = document.getElementById('pecaSelect');
        
        if (clienteSelect) {
            clienteSelect.innerHTML = '<option value="">-- Selecione um cliente --</option>';
            clientes.forEach(c => {
                const opt = document.createElement('option');
                opt.value = c.id;
                opt.textContent = `${c.nome_RazaoSocial} (${c.id})`;
                clienteSelect.appendChild(opt);
            });
        }
        
        if (pecaSelect) {
            pecaSelect.innerHTML = '<option value="">-- Selecione uma peça --</option>';
            pecas.forEach(p => {
                const opt = document.createElement('option');
                opt.value = p.id;
                opt.textContent = `${p.nomePeca} (${p.marca}) - R$ ${formatarMoeda(p.precoVenda)}`;
                opt.dataset.preco = p.precoVenda;
                pecaSelect.appendChild(opt);
            });
        }
    } catch (error) {
        mostrarMensagem('erro', `Erro ao carregar dados: ${error.message}`);
    }
}

function atualizarPrecoPeca() {
    const pecaSelect = document.getElementById('pecaSelect');
    const preco = pecaSelect.options[pecaSelect.selectedIndex].dataset.preco || 0;
    document.getElementById('precoUnitarioVenda').value = preco;
}

async function submitFormulario(event, tipo) {
    event.preventDefault();
    
    const dados = Object.fromEntries(new FormData(event.target));
    
    try {
        const response = await fetch(`${API_URL}/${tipos[tipo].plural}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dados)
        });
        
        if (response.ok) {
            mostrarMensagem('sucesso', `${tipos[tipo].singular} cadastrado com sucesso!`);
            event.target.reset();
            document.getElementById('selectorCadastro').value = '';
            setTimeout(() => atualizarListagens(), 500);
        } else {
            mostrarMensagem('erro', `Erro ao cadastrar: ${await response.text()}`);
        }
    } catch (error) {
        mostrarMensagem('erro', `Erro na requisição: ${error.message}`);
    }
}

async function submitFormularioVenda(event) {
    event.preventDefault();
    
    const clienteId = document.getElementById('clienteSelect').value;
    const pecaId = document.getElementById('pecaSelect').value;
    
    if (!clienteId || !pecaId) {
        mostrarMensagem('erro', 'Selecione cliente e peça!');
        return;
    }
    
    const venda = {
        id: document.getElementById('vendaId').value,
        numeroNota: document.getElementById('numeroNota').value,
        clienteId,
        pecaId,
        quantidadeVendida: parseInt(document.getElementById('quantidadeVenda').value),
        precoUnitarioVenda: parseFloat(document.getElementById('precoUnitarioVenda').value),
        descontoAplicado: parseFloat(document.getElementById('descontoVenda').value) || 0,
        precoComDesconto: 0,
        valorTotalVenda: 0,
        lucroLiquido: 0,
        percentualLucro: 0
    };
    
    try {
        const response = await fetch(`${API_URL}/vendas`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(venda)
        });
        
        if (response.ok) {
            mostrarMensagem('sucesso', 'Venda registrada com sucesso!');
            event.target.reset();
            document.getElementById('precoUnitarioVenda').value = '';
            document.getElementById('selectorCadastro').value = '';
            setTimeout(() => atualizarListagens(), 500);
        } else {
            mostrarMensagem('erro', `Erro ao registrar: ${await response.text()}`);
        }
    } catch (error) {
        mostrarMensagem('erro', `Erro na requisição: ${error.message}`);
    }
}

function mudarListagem(tipo) {
    if (!tipo) return;
    
    const divs = ['listClientes', 'listFornecedor', 'listPeca', 'listVenda'];
    divs.forEach(d => document.getElementById(d).style.display = 'none');
    
    const mapa = { cliente: 'listClientes', fornecedor: 'listFornecedor', peca: 'listPeca', venda: 'listVenda' };
    document.getElementById(mapa[tipo]).style.display = 'block';
    
    atualizarListagens();
}

function mostrarMensagem(tipo, mensagem) {
    const id = tipo === 'sucesso' ? 'successMessage' : 'errorMessage';
    const el = document.getElementById(id);
    if (el) {
        el.textContent = mensagem;
        el.style.display = 'block';
        setTimeout(() => el.style.display = 'none', 4000);
    }
}

async function deletarItem(tipo, id) {
    if (!confirm(`Deletar este ${tipo}?`)) return;
    
    try {
        const response = await fetch(`${API_URL}/${tipos[tipo].plural}/${id}`, {
            method: 'DELETE'
        });
        
        if (response.ok) {
            mostrarMensagem('sucesso', `${tipos[tipo].singular} deletado!`);
            setTimeout(() => atualizarListagens(), 500);
        } else {
            mostrarMensagem('erro', `Erro ao deletar: ${await response.text()}`);
        }
    } catch (error) {
        mostrarMensagem('erro', `Erro: ${error.message}`);
    }
}

async function editarItem(tipo, id) {
    if (!confirm(`Editar este ${tipo}?`)) return;
    
    try {
        const response = await fetch(`${API_URL}/${tipos[tipo].plural}/${id}`);
        const item = await response.json();
        
        const novoValor = prompt(`Novo valor para ${tipo} (${id}):\n${JSON.stringify(item, null, 2)}`);
        if (novoValor === null) return;
        
        const atualizado = Object.fromEntries(
            Object.entries(item).map(([k]) => [k, novoValor])
        );
        
        const responseUpdate = await fetch(`${API_URL}/${tipos[tipo].plural}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(atualizado)
        });
        
        if (responseUpdate.ok) {
            mostrarMensagem('sucesso', `${tipos[tipo].singular} atualizado!`);
            setTimeout(() => atualizarListagens(), 500);
        } else {
            mostrarMensagem('erro', `Erro ao atualizar: ${await responseUpdate.text()}`);
        }
    } catch (error) {
        mostrarMensagem('erro', `Erro: ${error.message}`);
    }
}

async function atualizarListagens() {
    try {
        const [clientesRes, fornecedoresRes, pecasRes, vendasRes] = await Promise.all([
            fetch(`${API_URL}/clientes`),
            fetch(`${API_URL}/fornecedores`),
            fetch(`${API_URL}/pecas`),
            fetch(`${API_URL}/vendas`)
        ]);
        
        const clientes = await clientesRes.json();
        const fornecedores = await fornecedoresRes.json();
        const pecas = await pecasRes.json();
        const vendas = await vendasRes.json();
        
        clientesCacheados = clientes;
        
        atualizarTabela('tabelaClientes', clientes, (c) => [
            c.id, c.nome_RazaoSocial, c.email || 'N/A', c.telefone || 'N/A', 
            c.CPF_CNPJ || 'N/A', c.status || 'N/A', `R$ ${formatarMoeda(c.totalGasto || 0)}`
        ], 'cliente');
        
        atualizarTabela('tabelaFornecedores', fornecedores, (f) => [
            f.id, f.nome_RazaoSocial, f.CPF_CNPJ || 'N/A', f.pecas || 'N/A', 
            f.status || 'N/A', f.quantidade || 0
        ], 'fornecedor');
        
        atualizarTabela('tabelaPecas', pecas, (p) => [
            p.id, p.nomePeca, p.categoria, p.marca, 
            `R$ ${formatarMoeda(p.custoFornecedor || 0)}`, `R$ ${formatarMoeda(p.precoVenda || 0)}`
        ], 'peca');
        
        atualizarTabela('tabelaVendas', vendas, (v) => [
            v.id, v.numeroNota, v.clienteId, v.pecaId, v.quantidadeVendida, 
            `R$ ${formatarMoeda(v.valorTotalVenda || 0)}`, `R$ ${formatarMoeda(v.lucroLiquido || 0)}`
        ], 'venda');
        
    } catch (error) {
        mostrarMensagem('erro', `Erro ao carregar: ${error.message}`);
    }
}

function atualizarTabela(idTabela, dados, extrator, tipo) {
    const tabela = document.getElementById(idTabela);
    if (!tabela) return;
    
    if (!dados.length) {
        tabela.innerHTML = `<tr><td colspan="8" style="text-align:center;color:#999;">Nenhum item</td></tr>`;
        return;
    }
    
    tabela.innerHTML = dados.map(item => {
        const valores = extrator(item);
        const acoes = `<button class="btn-edit" onclick="editarItem('${tipo}', '${item.id}')">Editar</button>
                       <button class="btn-delete" onclick="deletarItem('${tipo}', '${item.id}')">Deletar</button>`;
        return `<tr>${valores.map(v => `<td>${v}</td>`).join('')}<td>${acoes}</td></tr>`;
    }).join('');
}

function filtrarClientes() {
    const termo = document.getElementById('buscaClientes').value.toLowerCase();
    const filtrados = clientesCacheados.filter(c => 
        c.nome_RazaoSocial.toLowerCase().includes(termo) || c.id.toLowerCase().includes(termo)
    );
    
    atualizarTabela('tabelaClientes', filtrados, (c) => [
        c.id, c.nome_RazaoSocial, c.email || 'N/A', c.telefone || 'N/A', 
        c.CPF_CNPJ || 'N/A', c.status || 'N/A', `R$ ${formatarMoeda(c.totalGasto || 0)}`
    ], 'cliente');
}

async function atualizarDashboard() {
    try {
        const response = await fetch(`${API_URL}/vendas/relatorio/financeiro`);
        if (!response.ok) {
            carregarDadosPadroes();
            return;
        }
        
        const relatorio = await response.json();
        
        const campos = {
            'totalFaturamento': `R$ ${formatarMoeda(relatorio.totalFaturamento)}`,
            'totalLucro': `R$ ${formatarMoeda(relatorio.totalLucro)}`,
            'totalVendas': relatorio.totalVendas,
            'totalClientes': relatorio.clientesAtivos,
            'totalPecasVendidas': relatorio.volumePecas,
            'ticketMedio': `R$ ${formatarMoeda(relatorio.ticketMedio)}`,
            'rel-faturamento': `R$ ${formatarMoeda(relatorio.totalFaturamento)}`,
            'rel-custo': `R$ ${formatarMoeda(relatorio.totalFaturamento - relatorio.totalLucro)}`,
            'rel-lucro': `R$ ${formatarMoeda(relatorio.totalLucro)}`,
            'rel-margem': `${relatorio.margemLucro.toFixed(2)}%`,
            'rel-top': relatorio.produtoMaisVendido || 'N/A',
            'rel-bottom': relatorio.produtoMenosVendido || 'N/A'
        };
        
        Object.entries(campos).forEach(([id, valor]) => {
            const el = document.getElementById(id);
            if (el) el.textContent = valor;
        });
        
    } catch (error) {
        console.error('Erro dashboard:', error);
        carregarDadosPadroes();
    }
}

function carregarDadosPadroes() {
    const padrao = {
        'totalFaturamento': 'R$ 0,00', 'totalLucro': 'R$ 0,00', 'totalVendas': '0',
        'totalClientes': '0', 'totalPecasVendidas': '0', 'ticketMedio': 'R$ 0,00',
        'rel-faturamento': 'R$ 0,00', 'rel-custo': 'R$ 0,00', 'rel-lucro': 'R$ 0,00',
        'rel-margem': '0.00%', 'rel-top': 'Carregando...', 'rel-bottom': 'Carregando...'
    };
    
    Object.entries(padrao).forEach(([id, valor]) => {
        const el = document.getElementById(id);
        if (el) el.textContent = valor;
    });
}

function formatarMoeda(valor) {
    valor = typeof valor !== 'number' ? parseFloat(valor) || 0 : valor;
    return valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

document.addEventListener('DOMContentLoaded', () => {
    console.log('Zion System carregado');
    setTimeout(() => {
        carregarClientesEPecas();
        atualizarDashboard();
    }, 500);
});