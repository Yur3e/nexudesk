using InternApi.Interface;
using InternApi.Repository;
using InternApi.Service;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevSwagger", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Intern API",
        Version = "v1",
        Description = "API de estagiários com CRUD em memória e consulta de CEP via ViaCEP."
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Intern API",
        Version = "v2",
        Description = "Versão 2 com filtro por nome e resposta enriquecida para estagiários."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    options.DocInclusionPredicate((documentName, apiDescription) =>
        string.Equals(apiDescription.GroupName, documentName, StringComparison.OrdinalIgnoreCase));
});
builder.Services.AddSingleton<IEstagiarioRepository, EstagiarioRepository>();
builder.Services.AddSingleton<ICepCacheRepository, CepCacheRepository>();
builder.Services.AddScoped<IEstagiarioService, EstagiarioService>();
builder.Services.AddScoped<IEnderecoService, EnderecoService>();
builder.Services.AddHttpClient("ViaCep", client =>
{
    client.BaseAddress = new Uri("https://viacep.com.br/ws/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(options =>
{
    options.PreSerializeFilters.Add((swagger, request) =>
    {
        swagger.Servers = new List<OpenApiServer>
        {
            new()
            {
                Url = $"{request.Scheme}://{request.Host.Value}"
            }
        };
    });
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Intern API v1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Intern API v2");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Intern API Docs";
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("DevSwagger");
app.MapGet("/docs/v1", (HttpContext context) =>
{
    context.Response.Redirect("/swagger/index.html?urls.primaryName=Intern%20API%20v1");
    return Task.CompletedTask;
}).ExcludeFromDescription();

app.MapGet("/docs/v2", (HttpContext context) =>
{
    context.Response.Redirect("/swagger/index.html?urls.primaryName=Intern%20API%20v2");
    return Task.CompletedTask;
}).ExcludeFromDescription();

app.MapControllers();
app.Run();
