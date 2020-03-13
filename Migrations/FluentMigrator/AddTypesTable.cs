using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0001)]
    public class AddTypesTable : Migration
    {
        public override void Up()
        {
            Create.Table("types")
                .WithColumn("type_name").AsString(50).NotNullable().PrimaryKey()
                .WithColumn("type_display").AsString(255).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("types");
        }
    }
}