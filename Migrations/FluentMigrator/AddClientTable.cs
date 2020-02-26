using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0201)]
    public class AddClientTable : Migration
    {
        public override void Up()
        {
            Create.Table("clients")
                .WithColumn("client_id").AsCustom("BIGINT UNSIGNED").PrimaryKey().Identity()
                .WithColumn("password").AsString(50).NotNullable()
                .WithColumn("token").AsString(255).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("clients");
        }
    }
}