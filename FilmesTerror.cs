/// Esta classe representa um filme de terror. Cada propriedade se torna uma coluna da tabela no BD///
public class FilmeTerror
{
    public int Id { get; set; }

    public string Titulo { get; set; } = "";

    public string Diretor { get; set; } = "";

    public double NotaIMDb { get; set; }

    public bool DisponivelStreaming { get; set; }

    public DateTime DataLancamento { get; set; }
}