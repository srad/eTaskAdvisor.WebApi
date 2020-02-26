using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0101)]
    public class AddActivitiesTable : Migration
    {
        public override void Up()
        {
            Create.Table("activities")
                .WithColumn("activity_id").AsCustom("BIGINT UNSIGNED").NotNullable().PrimaryKey().Identity()
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("description").AsCustom("TEXT").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("activities");
        }
    }
}