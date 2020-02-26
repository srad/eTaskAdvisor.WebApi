using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0301)]
    public class AddFactorsTable : Migration
    {
        public override void Up()
        {
            Create.Table("factors")
                .WithColumn("factor_id").AsCustom("BIGINT UNSIGNED").NotNullable().PrimaryKey().Identity()
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("description").AsCustom("TEXT").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("factors");
        }
    }
}