using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexoDesk.Infrastructure.Persistence.Migrations;

[DbContext(typeof(HelpDeskDbContext))]
[Migration("20260926000000_TranslatePersistedValues")]
public partial class TranslatePersistedValues : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE [Users]
            SET [Role] = CASE [Role]
                WHEN 'User' THEN 'Usuario'
                WHEN 'Agent' THEN 'Agente'
                ELSE [Role]
            END;

            UPDATE [Tickets]
            SET [Status] = CASE [Status]
                WHEN 'Open' THEN 'Aberto'
                WHEN 'InProgress' THEN 'EmProgresso'
                WHEN 'Resolved' THEN 'Resolvido'
                WHEN 'Closed' THEN 'Fechado'
                ELSE [Status]
            END;

            UPDATE [Tickets]
            SET [Priority] = CASE [Priority]
                WHEN 'Low' THEN 'Baixa'
                WHEN 'Medium' THEN 'Media'
                WHEN 'High' THEN 'Alta'
                WHEN 'Critical' THEN 'Critica'
                ELSE [Priority]
            END;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE [Users]
            SET [Role] = CASE [Role]
                WHEN 'Usuario' THEN 'User'
                WHEN 'Agente' THEN 'Agent'
                ELSE [Role]
            END;

            UPDATE [Tickets]
            SET [Status] = CASE [Status]
                WHEN 'Aberto' THEN 'Open'
                WHEN 'EmProgresso' THEN 'InProgress'
                WHEN 'Resolvido' THEN 'Resolved'
                WHEN 'Fechado' THEN 'Closed'
                ELSE [Status]
            END;

            UPDATE [Tickets]
            SET [Priority] = CASE [Priority]
                WHEN 'Baixa' THEN 'Low'
                WHEN 'Media' THEN 'Medium'
                WHEN 'Alta' THEN 'High'
                WHEN 'Critica' THEN 'Critical'
                ELSE [Priority]
            END;
            """);
    }
}
