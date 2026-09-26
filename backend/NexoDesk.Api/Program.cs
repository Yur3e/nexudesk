using System.Text;
using NexoDesk.Api.ExceptionHandling;
using NexoDesk.Application.Authentication.Contracts;
using NexoDesk.Application.Authentication.Interfaces;
using NexoDesk.Application.Authentication.Services;
using NexoDesk.Application.Authentication.Validators;
using NexoDesk.Application.Comments.Contracts;
using NexoDesk.Application.Comments.Interfaces;
using NexoDesk.Application.Comments.Services;
using NexoDesk.Application.Comments.Validators;
using NexoDesk.Application.Tickets.Contracts;
using NexoDesk.Application.Tickets.Interfaces;
using NexoDesk.Application.Tickets.Services;
using NexoDesk.Application.Tickets.Validators;
using NexoDesk.Infrastructure.Authentication;
using NexoDesk.Infrastructure.Persistence;
using NexoDesk.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

const string clientCorsPolicy = "Client";
var connectionString = builder.Configuration.GetConnectionString("HelpDeskDatabase")
    ?? throw new InvalidOperationException("Connection string 'HelpDeskDatabase' was not configured.");
var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("JWT configuration was not found.");

jwtOptions.Validate();

builder.Services.AddDbContext<HelpDeskDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddScoped<DevelopmentDbInitializer>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<CreateCommentRequest>, CreateCommentRequestValidator>();
builder.Services.AddScoped<IValidator<CreateTicketRequest>, CreateTicketRequestValidator>();
builder.Services.AddScoped<IValidator<TicketListQuery>, TicketListQueryValidator>();
builder.Services.AddScoped<IValidator<UpdateTicketRequest>, UpdateTicketRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateTicketStatusRequest>, UpdateTicketStatusRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateTicketPriorityRequest>, UpdateTicketPriorityRequestValidator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

    options.AddPolicy(clientCorsPolicy, policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NexoDesk API",
        Version = "v1",
        Description = "API para gerenciamento de chamados."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe apenas o token JWT."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document, null)] = new List<string>()
    });
});
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<DevelopmentDbInitializer>();
    await databaseInitializer.InitializeAsync(CancellationToken.None);
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(clientCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
