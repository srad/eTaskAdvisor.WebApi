using FluentMigrator;

namespace eTaskAdvisor.WebApi.Migrations.FluentMigrator
{
    [Migration(0401)]
    public class AddInfluencesTable : Migration
    {
        public override void Up()
        {
            Create.Table("influences")
                .WithColumn("influence_name").AsString(50).NotNullable().PrimaryKey()
                .WithColumn("influence_display").AsString(255).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("influences");
        }
    }
}