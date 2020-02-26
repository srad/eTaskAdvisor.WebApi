using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0601)]
    public class AddAffectsTable : Migration
    {
        public override void Up()
        {
            Create.Table("affects")
                .WithColumn("affect_id").AsCustom("BIGINT UNSIGNED").NotNullable().PrimaryKey().Identity()
                .WithColumn("activity_id").AsCustom("BIGINT UNSIGNED").NotNullable().ForeignKey("activities", "activity_id")
                .WithColumn("factor_id").AsCustom("BIGINT UNSIGNED").NotNullable().ForeignKey("factors", "factor_id")
                .WithColumn("influence_name").AsString(50).NotNullable().ForeignKey("influences", "influence_name")
                .WithColumn("source").AsString(1000).NotNullable()
                .WithColumn("description").AsString(1000).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("affects");
        }
    }
}