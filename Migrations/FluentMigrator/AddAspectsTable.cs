using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0101)]
    public class AddAspectsTable : Migration
    {
        public override void Up()
        {
            Create.Table("aspects")
                .WithColumn("aspect_id").AsCustom("BIGINT UNSIGNED").NotNullable().PrimaryKey().Identity()
                .WithColumn("type_name").AsString(255).NotNullable().ForeignKey("types", "type_name")
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("description").AsCustom("TEXT").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("aspects");
        }
    }
}