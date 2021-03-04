using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Entities
{
    public class ProjectManagementEntities : DbContext
    {
        public DbSet<Board> Board { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<BoardStep> BoardStep { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Milestone> Milestone { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Sprint> Sprint { get; set; }
        public DbSet<Step> Step { get; set; }
        public DbSet<StepPerson> StepPerson { get; set; }
        public DbSet<StepPersonDetail> StepPersonDetail { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<WhatsappComminucation> WhatsappComminucation { get; set; }
        public DbSet<WhatsappMessage> WhatsappMessage { get; set; }
        public DbSet<WhatsappMessageRedirect> WhatsappMessageRedirect { get; set; }
        public DbSet<ProjectPerson> ProjectPerson { get; set; }
        public DbSet<ToDoUser> ToDoUser { get; set; }
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<TaskUser> TaskUser { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<Category> Category { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=94.73.162.42;Port=3306;Database=pmpdb;Uid=intellityai;Pwd=(intelli.3460);");
            //optionsBuilder.UseMySql("Server=192.168.1.222;Port=3306;Database=PMPDB;Uid=myuser;Pwd=root;");
            //optionsBuilder.UseMySql("Server=3.15.40.116;Port=3306;Database=PMPDB;Uid=myuser;Pwd=toor;");
        }

    }
}
