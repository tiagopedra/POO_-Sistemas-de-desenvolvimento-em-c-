// ===== CONFIGURAÇÃO DA API =====
const API_URL = 'http://localhost:5000/api';

// ===== ARMAZENAMENTO DE CLIENTES PARA FILTRO =====
let clientesCacheados = [];

// ===== FUNÇÃO: Pluralizar Corretamente =====
function pluralizar(tipo) {
    const plurais = {
        'cliente': 'clientes',
        'fornecedor': 'fornecedores',
        'peca': 'pecas',
        'venda': 'vendas'
    };
    return plurais[tipo] || tipo + 's';
}

// ===== FUNÇÃO: Mostrar Aba =====
function mostrarAba(aba, event) {
    if (event) {
        event.preventDefault();
    }

    // Remover active de todas as abas
    document.querySelectorAll('.tab-content').forEach(el => {
        el.classList.remove('active');
    });

    document.querySelectorAll('.tab-button').forEach(el => {
        el.classList.remove('active');
    });

    // Adicionar active na aba selecionada
    const element = document.getElementById(aba);
    if (element) {
        element.classList.add('active');
        if (event && event.target) {
            event.target.classList.add('active');
        }
    }

    // Executar ações específicas
    if (aba === 'dashboard') {
        setTimeout(() => atualizarDashboard(), 100);
    } else if (aba === 'listagem') {
        setTimeout(() => atualizarListagens(), 100);
    } else if (aba === 'cadastro') {
        setTimeout(() => carregarClientesEPecas(), 100);
    }
}

// ===== FUNÇÃO: Mudar Formulário =====
function mudarFormulario(tipo) {
    if (!tipo) {
        console.log('Nenhum tipo selecionado');
        return;
    }

    console.log('Mudando para formulário:', tipo);

    // Remover active de todos os forms
    document.querySelectorAll('.form-section').forEach(el => {
        el.classList.remove('active');
    });

    // Construir ID do form
    const nomeClasse = 'form' + tipo.charAt(0).toUpperCase() + tipo.slice(1);
    const element = document.getElementById(nomeClasse);
    
    if (element) {
        element.classList.add('active');
        console.log('Formulário exibido:', nomeClasse);
    } else {
        console.error('Formulário não encontrado:', nomeClasse);
    }

    if (tipo === 'venda') {
        carregarClientesEPecas();
    }
}

// ===== FUNÇÃO: Carregar Clientes e Peças =====
async function carregarClientesEPecas() {
    try {
        const clientesResponse = await fetch(`${API_URL}/clientes`);
        const clientes = clientesResponse.ok ? await clientesResponse.json() : [];
        clientesCacheados = clientes;

        const pecasResponse = await fetch(`${API_URL}/pecas`);
        const pecas = pecasResponse.ok ? await pecasResponse.json() : [];

        const selectClientes = document.getElementById('clienteSelect');
        if (selectClientes) {
            selectClientes.innerHTML = '<option value="">-- Selecione um cliente --</option>';
            clientes.forEach(cliente => {
                const option = document.createElement('option');
                option.value = cliente.id;
                option.textContent = `${cliente.nome_RazaoSocial} (${cliente.id})`;
                selectClientes.appendChild(option);
            });
        }

        const selectPecas = document.getElementById('pecaSelect');
        if (selectPecas) {
            selectPecas.innerHTML = '<option value="">-- Selecione uma peça --</option>';
            pecas.forEach(peca => {
                const option = document.createElement('option');
                option.value = peca.id;
                option.textContent = `${peca.nomePeca} (${peca.marca}) - R$ ${formatarMoeda(peca.precoVenda)}`;
                option.dataset.preco = peca.precoVenda;
                selectPecas.appendChild(option);
            });
        }

    } catch (error) {
        console.error('Erro ao carregar clientes e peças:', error);
    }
}

// ===== FUNÇÃO: Atualizar Preço da Peça =====
function atualizarPrecoPeca() {
    const selectPecas = document.getElementById('pecaSelect');
    const precoInput = document.getElementById('precoUnitarioVenda');
    
    if (!selectPecas || !precoInput) return;

    const selectedOption = selectPecas.options[selectPecas.selectedIndex];
    const preco = selectedOption.dataset.preco || 0;
    
    precoInput.value = preco;
}

// ===== FUNÇÃO: Submeter Formulário Genérico =====
async function submitFormulario(event, tipo) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);
    const dados = Object.fromEntries(formData);

    const endpoint = pluralizar(tipo);

    try {
        const response = await fetch(`${API_URL}/${endpoint}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dados)
        });

        if (response.ok) {
            const resultado = await response.json();
            console.log(`${tipo} cadastrado:`, resultado);
            mostrarMensagem('sucesso', `${tipo.charAt(0).toUpperCase() + tipo.slice(1)} cadastrado com sucesso!`);
            form.reset();
            resetarSelectorCadastro();
            setTimeout(() => {
                atualizarListagens();
            }, 1000);
        } else {
            const erro = await response.text();
            mostrarMensagem('erro', `Erro ao cadastrar: ${erro}`);
        }
    } catch (error) {
        console.error('Erro na requisição:', error);
        mostrarMensagem('erro', `Erro de conexão com a API: ${error.message}`);
    }
}

// ===== FUNÇÃO: Resetar Seletor de Cadastro =====
function resetarSelectorCadastro() {
    const selector = document.getElementById('selectorCadastro');
    if (selector) {
        selector.value = '';
        // Esconder todos os formulários
        document.querySelectorAll('.form-section').forEach(el => {
            el.classList.remove('active');
        });
    }
}

// ===== FUNÇÃO: Submeter Formulário Venda =====
async function submitFormularioVenda(event) {
    event.preventDefault();

    const novaVenda = {
        id: document.getElementById('vendaId').value,
        numeroNota: document.getElementById('numeroNota').value,
        clienteId: document.getElementById('clienteSelect').value,
        pecaId: document.getElementById('pecaSelect').value,
        quantidadeVendida: parseInt(document.getElementById('quantidadeVenda').value),
        precoUnitarioVenda: parseFloat(document.getElementById('precoUnitarioVenda').value),
        descontoAplicado: parseFloat(document.getElementById('descontoVenda').value) || 0,
        precoComDesconto: 0,
        valorTotalVenda: 0,
        lucroLiquido: 0,
        percentualLucro: 0
    };

    if (!novaVenda.clienteId) {
        mostrarMensagem('erro', 'Por favor, selecione um cliente!');
        return;
    }

    if (!novaVenda.pecaId) {
        mostrarMensagem('erro', 'Por favor, selecione uma peça!');
        return;
    }

    try {
        const response = await fetch(`${API_URL}/vendas`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(novaVenda)
        });

        if (response.ok) {
            const resultado = await response.json();
            console.log('Venda cadastrada:', resultado);
            mostrarMensagem('sucesso', 'Venda registrada com sucesso!');
            document.querySelector('#formVenda form').reset();
            document.getElementById('precoUnitarioVenda').value = '';
            resetarSelectorCadastro();
            setTimeout(() => {
                atualizarListagens();
            }, 1000);
        } else {
            const erro = await response.text();
            mostrarMensagem('erro', `Erro ao registrar venda: ${erro}`);
        }
    } catch (error) {
        console.error('Erro na requisição:', error);
        mostrarMensagem('erro', `Erro de conexão com a API: ${error.message}`);
    }
}

// ===== FUNÇÃO: Mudar Listagem =====
function mudarListagem(tipo) {
    if (!tipo) return;

    document.getElementById('listClientes').style.display = 'none';
    document.getElementById('listFornecedor').style.display = 'none';
    document.getElementById('listPeca').style.display = 'none';
    document.getElementById('listVenda').style.display = 'none';

    if (tipo === 'cliente') {
        document.getElementById('listClientes').style.display = 'block';
    } else if (tipo === 'fornecedor') {
        document.getElementById('listFornecedor').style.display = 'block';
    } else if (tipo === 'peca') {
        document.getElementById('listPeca').style.display = 'block';
    } else if (tipo === 'venda') {
        document.getElementById('listVenda').style.display = 'block';
    }

    atualizarListagens();
}

// ===== FUNÇÃO: Mostrar Mensagem =====
function mostrarMensagem(tipo, mensagem) {
    let elemento;

    if (tipo === 'sucesso') {
        elemento = document.getElementById('successMessage');
    } else if (tipo === 'erro') {
        elemento = document.getElementById('errorMessage');
    }

    if (elemento) {
        elemento.textContent = mensagem;
        elemento.style.display = 'block';

        setTimeout(() => {
            elemento.style.display = 'none';
        }, 4000);
    }
}

// ===== FUNÇÃO: Deletar Item =====
async function deletarItem(tipo, id) {
    const confirmacao = confirm(`Deseja realmente deletar este ${tipo}?`);

    if (confirmacao) {
        const endpoint = pluralizar(tipo);

        try {
            const response = await fetch(`${API_URL}/${endpoint}/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                mostrarMensagem('sucesso', `${tipo.charAt(0).toUpperCase() + tipo.slice(1)} deletado com sucesso!`);
                setTimeout(() => {
                    atualizarListagens();
                }, 1000);
            } else {
                const erro = await response.text();
                mostrarMensagem('erro', `Erro ao deletar: ${erro}`);
            }
        } catch (error) {
            console.error('Erro na requisição:', error);
            mostrarMensagem('erro', `Erro de conexão com a API: ${error.message}`);
        }
    }
}

// ===== FUNÇÃO: Editar Item =====
async function editarItem(tipo, id) {
    const novoValor = prompt(`Digite o novo valor para ${tipo} ${id}:`);

    if (novoValor === null) return;

    const endpoint = pluralizar(tipo);

    try {
        const response = await fetch(`${API_URL}/${endpoint}/${id}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ novoValor: novoValor })
        });

        if (response.ok) {
            mostrarMensagem('sucesso', `${tipo.charAt(0).toUpperCase() + tipo.slice(1)} atualizado com sucesso!`);
            setTimeout(() => {
                atualizarListagens();
            }, 1000);
        } else {
            const erro = await response.text();
            mostrarMensagem('erro', `Erro ao atualizar: ${erro}`);
        }
    } catch (error) {
        console.error('Erro na requisição:', error);
        mostrarMensagem('erro', `Erro de conexão com a API: ${error.message}`);
    }
}

// ===== FUNÇÃO: Atualizar Listagens =====
async function atualizarListagens() {
    try {
        const clientesResponse = await fetch(`${API_URL}/clientes`);
        const clientes = clientesResponse.ok ? await clientesResponse.json() : [];
        clientesCacheados = clientes;

        const fornecedoresResponse = await fetch(`${API_URL}/fornecedores`);
        const fornecedores = fornecedoresResponse.ok ? await fornecedoresResponse.json() : [];

        const pecasResponse = await fetch(`${API_URL}/pecas`);
        const pecas = pecasResponse.ok ? await pecasResponse.json() : [];

        const vendasResponse = await fetch(`${API_URL}/vendas`);
        const vendas = vendasResponse.ok ? await vendasResponse.json() : [];

        atualizarTabelaClientes(clientes);
        atualizarTabelaFornecedores(fornecedores);
        atualizarTabelaPecas(pecas);
        atualizarTabelaVendas(vendas);

    } catch (error) {
        console.error('Erro ao carregar dados:', error);
    }
}

// ===== FUNÇÃO: Atualizar Tabela Clientes =====
function atualizarTabelaClientes(clientes) {
    const tabela = document.getElementById('tabelaClientes');
    if (!tabela) return;

    if (clientes.length === 0) {
        tabela.innerHTML = '<tr><td colspan="8" style="text-align: center; color: #999;">Nenhum cliente cadastrado</td></tr>';
        return;
    }

    tabela.innerHTML = clientes.map(c => `
        <tr>
            <td>${c.id}</td>
            <td>${c.nome_RazaoSocial}</td>
            <td>${c.email || 'N/A'}</td>
            <td>${c.telefone || 'N/A'}</td>
            <td>${c.CPF_CNPJ || 'N/A'}</td>
            <td>${c.status || 'N/A'}</td>
            <td>R$ ${formatarMoeda(c.totalGasto || 0)}</td>
            <td>
                <button class="btn-edit" onclick="editarItem('cliente', '${c.id}')">Editar</button>
                <button class="btn-delete" onclick="deletarItem('cliente', '${c.id}')">Deletar</button>
            </td>
        </tr>
    `).join('');
}

// ===== FUNÇÃO: Filtrar Clientes =====
function filtrarClientes() {
    const termoBusca = document.getElementById('buscaClientes').value.toLowerCase();
    const tabelaClientes = document.getElementById('tabelaClientes');
    
    if (!tabelaClientes) return;

    const clientesFiltrados = clientesCacheados.filter(cliente => {
        const nome = cliente.nome_RazaoSocial.toLowerCase();
        const id = cliente.id.toLowerCase();
        return nome.includes(termoBusca) || id.includes(termoBusca);
    });

    if (clientesFiltrados.length === 0) {
        tabelaClientes.innerHTML = '<tr><td colspan="8" style="text-align: center; color: #999;">Nenhum cliente encontrado</td></tr>';
        return;
    }

    tabelaClientes.innerHTML = clientesFiltrados.map(c => `
        <tr>
            <td>${c.id}</td>
            <td>${c.nome_RazaoSocial}</td>
            <td>${c.email || 'N/A'}</td>
            <td>${c.telefone || 'N/A'}</td>
            <td>${c.CPF_CNPJ || 'N/A'}</td>
            <td>${c.status || 'N/A'}</td>
            <td>R$ ${formatarMoeda(c.totalGasto || 0)}</td>
            <td>
                <button class="btn-edit" onclick="editarItem('cliente', '${c.id}')">Editar</button>
                <button class="btn-delete" onclick="deletarItem('cliente', '${c.id}')">Deletar</button>
            </td>
        </tr>
    `).join('');
}

// ===== FUNÇÃO: Atualizar Tabela Fornecedores =====
function atualizarTabelaFornecedores(fornecedores) {
    const tabela = document.getElementById('tabelaFornecedores');
    if (!tabela) return;

    if (fornecedores.length === 0) {
        tabela.innerHTML = '<tr><td colspan="7" style="text-align: center; color: #999;">Nenhum fornecedor cadastrado</td></tr>';
        return;
    }

    tabela.innerHTML = fornecedores.map(f => `
        <tr>
            <td>${f.id}</td>
            <td>${f.nome_RazaoSocial}</td>
            <td>${f.CPF_CNPJ || 'N/A'}</td>
            <td>${f.pecas || 'N/A'}</td>
            <td>${f.status || 'N/A'}</td>
            <td>${f.quantidade || 0}</td>
            <td>
                <button class="btn-edit" onclick="editarItem('fornecedor', '${f.id}')">Editar</button>
                <button class="btn-delete" onclick="deletarItem('fornecedor', '${f.id}')">Deletar</button>
            </td>
        </tr>
    `).join('');
}

// ===== FUNÇÃO: Atualizar Tabela Peças =====
function atualizarTabelaPecas(pecas) {
    const tabela = document.getElementById('tabelaPecas');
    if (!tabela) return;

    if (pecas.length === 0) {
        tabela.innerHTML = '<tr><td colspan="7" style="text-align: center; color: #999;">Nenhuma peça cadastrada</td></tr>';
        return;
    }

    tabela.innerHTML = pecas.map(p => `
        <tr>
            <td>${p.id}</td>
            <td>${p.nomePeca}</td>
            <td>${p.categoria}</td>
            <td>${p.marca}</td>
            <td>R$ ${formatarMoeda(p.custoFornecedor || 0)}</td>
            <td>R$ ${formatarMoeda(p.precoVenda || 0)}</td>
            <td>
                <button class="btn-edit" onclick="editarItem('peca', '${p.id}')">Editar</button>
                <button class="btn-delete" onclick="deletarItem('peca', '${p.id}')">Deletar</button>
            </td>
        </tr>
    `).join('');
}

// ===== FUNÇÃO: Atualizar Tabela Vendas =====
function atualizarTabelaVendas(vendas) {
    const tabela = document.getElementById('tabelaVendas');
    if (!tabela) return;

    if (vendas.length === 0) {
        tabela.innerHTML = '<tr><td colspan="8" style="text-align: center; color: #999;">Nenhuma venda registrada</td></tr>';
        return;
    }

    tabela.innerHTML = vendas.map(v => `
        <tr>
            <td>${v.id}</td>
            <td>${v.numeroNota}</td>
            <td>${v.clienteId}</td>
            <td>${v.pecaId}</td>
            <td>${v.quantidadeVendida}</td>
            <td>R$ ${formatarMoeda(v.valorTotalVenda || 0)}</td>
            <td>R$ ${formatarMoeda(v.lucroLiquido || 0)}</td>
            <td>
                <button class="btn-edit" onclick="editarItem('venda', '${v.id}')">Editar</button>
                <button class="btn-delete" onclick="deletarItem('venda', '${v.id}')">Deletar</button>
            </td>
        </tr>
    `).join('');
}

// ===== FUNÇÃO: Atualizar Dashboard =====
async function atualizarDashboard() {
    try {
        const vendasResponse = await fetch(`${API_URL}/vendas/relatorio/financeiro`);
        
        if (vendasResponse.ok) {
            const relatorio = await vendasResponse.json();

            document.getElementById('totalFaturamento').textContent = 'R$ ' + formatarMoeda(relatorio.totalFaturamento || 0);
            document.getElementById('totalLucro').textContent = 'R$ ' + formatarMoeda(relatorio.totalLucro || 0);
            document.getElementById('totalVendas').textContent = relatorio.totalVendas || 0;
            document.getElementById('totalClientes').textContent = relatorio.clientesAtivos || 0;
            document.getElementById('totalPecasVendidas').textContent = relatorio.volumePecas || 0;
            document.getElementById('ticketMedio').textContent = 'R$ ' + formatarMoeda(relatorio.ticketMedio || 0);

            document.getElementById('rel-faturamento').textContent = 'R$ ' + formatarMoeda(relatorio.totalFaturamento || 0);
            document.getElementById('rel-custo').textContent = 'R$ ' + formatarMoeda((relatorio.totalFaturamento - relatorio.totalLucro) || 0);
            document.getElementById('rel-lucro').textContent = 'R$ ' + formatarMoeda(relatorio.totalLucro || 0);
            document.getElementById('rel-top').textContent = relatorio.produtoMaisVendido || 'N/A';
            document.getElementById('rel-bottom').textContent = relatorio.produtoMenosVendido || 'N/A';

            const margem = relatorio.totalFaturamento > 0 ? ((relatorio.totalLucro / relatorio.totalFaturamento) * 100).toFixed(2) : 0;
            document.getElementById('rel-margem').textContent = margem + '%';
        }
    } catch (error) {
        console.error('Erro ao carregar dashboard:', error);
    }
}

// ===== FUNÇÃO: Formatar Moeda =====
function formatarMoeda(valor) {
    if (typeof valor !== 'number') {
        valor = parseFloat(valor) || 0;
    }
    return valor.toLocaleString('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

// ===== INICIALIZAÇÃO =====
document.addEventListener('DOMContentLoaded', function() {
    console.log('Zion System carregado com sucesso!');
    console.log('Conectando à API:', API_URL);
    
    // Carregar dados ao iniciar
    setTimeout(() => {
        carregarClientesEPecas();
    }, 500);
});