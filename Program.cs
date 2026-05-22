using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=filmesTerror.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();
}

app.MapGet("/", () =>
{
    return "API de Filmes de Terror - Minimal API com .NET";
});

app.MapGet("/status", () =>
{
    return Results.Ok(new
    {
        status = "online",
        mensagem = "API funcionando",
        dataHora = DateTime.Now
    });
});

app.MapGet("/filmes", async (AppDbContext db) =>
{
    var filmes = await db.Filmes.ToListAsync();

    return Results.Ok(filmes);
});

app.MapPost("/filmes", async (FilmeTerror filme, AppDbContext db) =>
{
    await db.Filmes.AddAsync(filme);

    await db.SaveChangesAsync();

    return Results.Created($"/filmes/{filme.Id}", filme);
});

app.MapGet("/filmes/{id}", async (int id, AppDbContext db) =>
{
    var filme = await db.Filmes.FindAsync(id);

    if (filme == null)
        return Results.NotFound();

    return Results.Ok(filme);
});

app.MapPut("/filmes/{id}", async (int id, FilmeTerror dados, AppDbContext db) =>
{
    var filme = await db.Filmes.FindAsync(id);

    if (filme == null)
        return Results.NotFound();

    filme.Titulo = dados.Titulo;
    filme.Diretor = dados.Diretor;
    filme.NotaIMDb = dados.NotaIMDb;
    filme.DisponivelStreaming = dados.DisponivelStreaming;
    filme.DataLancamento = dados.DataLancamento;

    await db.SaveChangesAsync();

    return Results.Ok(filme);
});

app.MapDelete("/filmes/{id}", async (int id, AppDbContext db) =>
{
    var filme = await db.Filmes.FindAsync(id);

    if (filme == null)
        return Results.NotFound();

    db.Filmes.Remove(filme);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();