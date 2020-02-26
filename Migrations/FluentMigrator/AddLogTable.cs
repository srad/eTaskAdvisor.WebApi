using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0)]
    public class AddLogTable : Migration
    {
        public override void Up()
        {
            Create.Table("logs")
                .WithColumn("log_id").AsCustom("BIGINT UNSIGNED").PrimaryKey().Identity()
                .WithColumn("created").AsDateTime().NotNullable()
                .WithColumn("message").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Log");
        }
    }
}