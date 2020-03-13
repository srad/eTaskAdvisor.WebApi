using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0801)]
    public class AddReadAspectsTable : Migration
    {
        public override void Up()
        {
            Create.Table("read_aspects")
                .WithColumn("aspect_id").AsCustom("BIGINT UNSIGNED").NotNullable().ForeignKey("aspects", "aspect_id")
                .WithColumn("client_id").AsCustom("BIGINT UNSIGNED").NotNullable().ForeignKey("clients", "client_id");
        }

        public override void Down()
        {
            Delete.Table("read_aspects");
        }
    }
}