using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTaskAdvisor.WebApi.Models.Archived
{
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Column("client_id")]
        public int ClientId { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("password")]
        public string Password { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }

    public class Aspect
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActivityId { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description {  get; set; }
    }
    
    public class Factor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Column("factor_id")]
        public int FactorId { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description {  get; set; }
    }

    public class Influence
    {
        [Column("influence_name")]
        public string InfluenceName { get; set; }
        
        [Column("influence_display")]
        public string InfluenceDisplay { get; set; }
    }

    public class Affect
    {
        [Column("aspect_id")]
        public int ActivityId { get; set; }
        
        [Column("factor_id")]
        public int FactorId { get; set; }
        
        [Column("influence_name")]
        public string InfluenceName { get; set; }
        
        [Column("source")]
        public string Source { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        public virtual Aspect Aspect { get; set; }
        public virtual Factor Factor { get; set; }
        public virtual Influence Influence { get; set; }
    }

    public class Task
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Column("task_id")]
        public int TaskId { get; set; }
        [Column("aspect_id")]
        public int ActivityId { get; set; }
        [Column("client_id")]
        public int ClientId { get; set; }
        [Column("subject")]
        public string Subject { get; set; }
        [Column("at")]
        public DateTime At { get; set; }
        [Column("duration")]
        public int Duration { get; set; }

        public virtual Aspect Aspect { get; set; }
        public virtual Client Client { get; set; }
    }

}