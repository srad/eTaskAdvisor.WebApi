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
        [Column(Name = "client_id")] public int ClientId { get; set; }

        [Column(Name = "password")] public string Password { get; set; }

        [Column(Name = "token")] public string Token { get; set; }

        [Ignore] public List<ClientTask> ClientTasks { get; set; }
    }

    [TableName("activities")]
    [PrimaryKey("activity_id", AutoIncrement = true)]
    public class Activity
    {
        [Column(Name = "activity_id")] public int ActivityId { get; set; }

        [Column(Name = "name")] public string Name { get; set; }

        [Column(Name = "description")] public string Description { get; set; }
    }

    [TableName("factors")]
    [PrimaryKey("factor_id", AutoIncrement = true)]
    public class Factor
    {
        [Column(Name = "factor_id")] public int FactorId { get; set; }

        [Column(Name = "name")] public string Name { get; set; }

        [Column(Name = "description")] public string Description { get; set; }
    }

    [TableName("influences")]
    [PrimaryKey("influence_name", AutoIncrement = false)]
    public class Influence
    {
        [Column(Name = "influence_name")] public string InfluenceName { get; set; }

        [Column(Name = "influence_display")] public string InfluenceDisplay { get; set; }
    }

    [TableName("affects")]
    [PrimaryKey("affect_id", AutoIncrement = true)]
    public class Affect
    {
        [Column(Name = "affect_id")] public int AffectId { get; set; }

        [Column(Name = "activity_id")] public int ActivityId { get; set; }

        [Column(Name = "factor_id")] public int FactorId { get; set; }

        [Column(Name = "influence_name")] public string InfluenceName { get; set; }

        [Column(Name = "source")] public string Source { get; set; }

        [Column(Name = "description"), AllowNull]
        public string Description { get; set; }

        [Ignore] public Activity Activity { get; set; }
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

        [Column(Name = "client_id")] public int ClientId { get; set; }

        [Column(Name = "task_id")] public int TaskId { get; set; }

        [Column(Name = "activity_id")] public int ActivityId { get; set; }

        [Column(Name = "subject")] public string Subject { get; set; }

        [Column(Name = "at")] public DateTime At { get; set; }

        [Column(Name = "duration")] public int Duration { get; set; }

        [Column(Name = "done")] public bool Done { get; set; }

        [Ignore] public Activity Activity { get; set; }

        [ResultColumn, Column(Name = "activity_name")]
        public string ActivityName { get; set; }

        [ResultColumn, Column(Name = "activity_description")]
        public string ActivityDescription { get; set; }
    }
}