using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0501)]
    public class AddTasksTable : Migration
    {
        public override void Up()
        {
            Create.Table("tasks")
                .WithColumn("task_id").AsCustom("BIGINT UNSIGNED").NotNullable().PrimaryKey().Identity()
                .WithColumn("aspect_id").AsCustom("BIGINT UNSIGNED").NotNullable().ForeignKey("aspects", "aspect_id")
                .WithColumn("client_id").AsCustom("BIGINT UNSIGNED").NotNullable().ForeignKey("clients", "client_id")
                .WithColumn("subject").AsString(255).NotNullable()
                .WithColumn("description").AsString(1000).Nullable()
                .WithColumn("at").AsDateTime().NotNullable()
                .WithColumn("duration").AsCustom("SMALLINT UNSIGNED").NotNullable()
                .WithColumn("done").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("tasks");
        }
    }
}