namespace ApiProdutos.Models;

public class MaterialEscolar
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public List<Lote> Lotes { get; set; } = new();
}