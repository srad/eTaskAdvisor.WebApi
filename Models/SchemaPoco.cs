using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PetaPoco;

namespace eTaskAdvisor.WebApi.Models
{
    [TableName("clients")]
    [PrimaryKey("client_id", AutoIncrement = true)]
    public class Client
    {
        [Column("client_id")] public int ClientId { get; set; }
        [Column("password")] public string Password { get; set; }
        [Column("token")] public string Token { get; set; }
        [Ignore] public List<ClientTask> ClientTasks { get; set; }
    }

    [TableName("aspects")]
    [PrimaryKey("aspect_id", AutoIncrement = true)]
    public class Aspect
    {
        [Column("aspect_id")] public int AspectId { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("type_name")] public string TypeName { get; set; }
        [Column("description")] public string Description { get; set; }
        [Ignore] public AspectType AspectType { get; set; }
    }

    [TableName("factors")]
    [PrimaryKey("factor_id", AutoIncrement = true)]
    public class Factor
    {
        [Column("factor_id")] public int FactorId { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("description")] public string Description { get; set; }
    }

    [TableName("influences")]
    [PrimaryKey("influence_name", AutoIncrement = false)]
    public class Influence
    {
        [Column("influence_name")] public string InfluenceName { get; set; }
        [Column("influence_display")] public string InfluenceDisplay { get; set; }
    }
    
    [TableName("types")]
    [PrimaryKey("type_name", AutoIncrement = false)]
    public class AspectType
    {
        [Column("type_name")] public string TypeName { get; set; }
        [Column("type_display")] public string TypeDisplay { get; set; }
    }

    [TableName("affects")]
    [PrimaryKey("affect_id", AutoIncrement = true)]
    public class Affect
    {
        [Column("affect_id")] public int AffectId { get; set; }
        [Column("aspect_id")] public int AspectId { get; set; }
        [Column("factor_id")] public int FactorId { get; set; }
        [Column("influence_name")] public string InfluenceName { get; set; }
        [Column("source")] public string Source { get; set; }
        [Column("description"), AllowNull]
        public string Description { get; set; }

        [Ignore] public Aspect Aspect { get; set; }
        [Ignore] public Factor Factor { get; set; }
        [Ignore] public Influence Influence { get; set; }
    }

    [TableName("tasks")]
    [PrimaryKey("task_id", AutoIncrement = true)]
    public class ClientTask
    {
        public ClientTask()
        {
        }

        [Column("client_id")] public int ClientId { get; set; }
        [Column("task_id")] public int TaskId { get; set; }
        [Column("aspect_id")] public int AspectId { get; set; }
        [Column("subject")] public string Subject { get; set; }
        [Column("at")] public DateTime At { get; set; }
        [Column("duration")] public int Duration { get; set; }
        [Column("done")] public bool Done { get; set; }

        [ResultColumn("at_formatted")] public string AtFormatted { get; set; }
        [ResultColumn("aspect_name")] public string AspectName { get; set; }
        [ResultColumn("aspect_description")] public string AspectDescription { get; set; }
        [ResultColumn("aspect_type_name")] public string AspectType { get; set; }
        [ResultColumn("aspect_type_display")] public string AspectDisplay { get; set; }
    }
}