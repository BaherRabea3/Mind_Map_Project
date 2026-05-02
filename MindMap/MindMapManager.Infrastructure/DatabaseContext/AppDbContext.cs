using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MindMapManager.Core.Entities;

namespace MindMapManager.Infrastructure.DatabaseContext;

public partial class AppDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Progress> Progresses { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Roadmap> Roadmaps { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<UserComment> UserComments { get; set; }

    public virtual DbSet<UserProject> UserProjects { get; set; }

    public DbSet<UserTrack> UserTracks { get; set; }

    public DbSet<CompletedTopic> CompletedTopics { get; set; }

    public virtual DbSet<VwCommentThread> VwCommentThreads { get; set; }

    public virtual DbSet<VwLeaderboard> VwLeaderboards { get; set; }

    public virtual DbSet<VwRoadmapDetail> VwRoadmapDetails { get; set; }

    public virtual DbSet<VwRoadmapReview> VwRoadmapReviews { get; set; }

    public virtual DbSet<VwRoadmapStat> VwRoadmapStats { get; set; }

    public virtual DbSet<VwTopicResource> VwTopicResources { get; set; }

    public virtual DbSet<VwUserBookmark> VwUserBookmarks { get; set; }

    public virtual DbSet<VwUserCertificate> VwUserCertificates { get; set; }

    public virtual DbSet<VwUserDashboard> VwUserDashboards { get; set; }

    public virtual DbSet<VwUserProgress> VwUserProgresses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<IdentityUserLogin<int>>()
       .HasKey(e => new { e.LoginProvider, e.ProviderKey });

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.HasData(
                new ApplicationRole()
                {
                    Id = 1,
                    Name = "Member",
                    NormalizedName = "MEMBER"
                },
                new ApplicationRole()
                {
                    Id = 2,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
        });

        modelBuilder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasData(new IdentityUserRole<int>()
            {
                UserId = 3,
                RoleId = 1
            },
                new IdentityUserRole<int>()
                {
                    UserId = 4,
                    RoleId = 2
                });
        });
            

        modelBuilder.Entity<IdentityUserToken<int>>()
            .HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<ApplicationUser>(entity =>
        {


            entity.HasIndex(e => e.LastActDate, "idx_user_last_act_date");

            entity.HasIndex(e => e.Status, "idx_user_status");

           
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            
            entity.Property(e => e.IsVerified).HasColumnName("is_verified");
            entity.Property(e => e.LastActDate)
                .HasColumnType("datetime")
                .HasColumnName("last_act_date");
            entity.Property(e => e.OtpCode).HasColumnName("otp_code");
            entity.Property(e => e.OtpExpire)
                .HasColumnType("datetime")
                .HasColumnName("otp_expire");
           
            entity.Property(e => e.Pid).HasColumnName("pid");
           
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status")
                .HasDefaultValue("Active");
            entity.Property(e => e.Streak)
                .HasDefaultValue(0)
                .HasColumnName("streak");
           

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.ApplicationUsers)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("user_plan_fk");

            entity.HasMany(d => d.Res).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Bookmark",
                    r => r.HasOne<Resource>().WithMany()
                        .HasForeignKey("ResId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("res_bookmark_fk"),
                    l => l.HasOne<ApplicationUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_bookmark_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "ResId").HasName("pk_bookmark");
                        j.ToTable("bookmark");
                        j.HasIndex(new[] { "ResId" }, "idx_bookmark_res_id");
                        j.HasIndex(new[] { "UserId" }, "idx_bookmark_user_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("ResId").HasColumnName("res_id");
                    });

            entity.HasMany(d => d.ResNavigation).WithMany(p => p.UsersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "UserResource",
                    r => r.HasOne<Resource>().WithMany()
                        .HasForeignKey("ResId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__user_reso__res_i__18EBB532"),
                    l => l.HasOne<ApplicationUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__user_reso__user___17F790F9"),
                    j =>
                    {
                        j.HasKey("UserId", "ResId").HasName("PK__user_res__7BB73C5F3DCE2088");
                        j.ToTable("user_resource");
                        j.HasIndex(new[] { "ResId" }, "idx_user_resource_res_id");
                        j.HasIndex(new[] { "UserId" }, "idx_user_resource_user_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("ResId").HasColumnName("res_id");
                    });

        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertId).HasName("PK__certific__024B46EC706205C1");

            entity.ToTable("certificate");

            entity.HasIndex(e => e.CertUrl, "UQ__certific__98B88B7F87AEF197").IsUnique();

            entity.HasIndex(e => e.Rid, "idx_certificate_rid");

            entity.HasIndex(e => e.UserId, "idx_certificate_user_id");

            entity.Property(e => e.CertId).HasColumnName("cert_id");
            entity.Property(e => e.CertUrl)
                .HasMaxLength(200)
                .HasColumnName("cert_url");
            entity.Property(e => e.CertificateCode).HasColumnType("varchar()").HasMaxLength(32);
            entity.Property(e => e.IssuedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("issued_at");
            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.RidNavigation).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.Rid)
                .HasConstraintName("certificate_roadmap_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_certificate_fk");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__comment__5066738CC2CD55D0");

            entity.ToTable("comment");

            entity.HasIndex(e => e.ParentComId, "idx_comment_parent_com_id");

            entity.HasIndex(e => e.TopicId, "idx_comment_topic_id");

            entity.Property(e => e.ComId).HasColumnName("com_id");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ParentComId).HasColumnName("parent_com_id");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");

            entity.HasOne(d => d.ParentCom).WithMany(p => p.InverseParentCom)
                .HasForeignKey(d => d.ParentComId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("reply_comment_fk");

            entity.HasOne(d => d.Topic).WithMany(p => p.Comments)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comment_topic_fk");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Lid).HasName("PK__level__DE105D0723A1DED8");

            entity.ToTable("level");

            entity.HasIndex(e => e.Rid, "idx_level_rid");

            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Rid).HasColumnName("rid");

            entity.HasOne(d => d.RidNavigation).WithMany(p => p.Levels)
                .HasForeignKey(d => d.Rid)
                .HasConstraintName("roadmap_level_fk");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotId).HasName("PK__notifica__0E4BE3002DCFD2AB");

            entity.ToTable("notification");

            entity.HasIndex(e => e.Read, "idx_notification_read");

            entity.HasIndex(e => e.UserId, "idx_notification_user_id");

            entity.Property(e => e.NotId).HasColumnName("not_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(200)
                .HasColumnName("message");
            entity.Property(e => e.Read).HasColumnName("read");
            entity.Property(e => e.RefId).HasColumnName("ref_id");
            entity.Property(e => e.RefType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ref_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_notification_fk");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__plan__DD37D91A1E7995DF");

            entity.ToTable("plan");

            entity.Property(e => e.Pid).HasColumnName("pid");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.ProgId).HasName("PK__progress__8C41E25C33043CD1");

            entity.ToTable("progress");

            entity.HasIndex(e => e.Lid, "idx_progress_lid");

            entity.HasIndex(e => e.UserId, "idx_progress_user_id");

            entity.HasIndex(e => new { e.UserId, e.Lid }, "user_progress_level_uq").IsUnique();

            entity.Property(e => e.ProgId).HasColumnName("prog_id");
            entity.Property(e => e.CompPerc)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("comp_perc");
            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.LidNavigation).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.Lid)
                .HasConstraintName("progress_level_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_progress_fk");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjId).HasName("PK__project__A04A0C2D8FB345F5");

            entity.ToTable("project");

            entity.HasIndex(e => e.Lid, "idx_project_lid");

            entity.Property(e => e.ProjId).HasColumnName("proj_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.LidNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Lid)
                .HasConstraintName("project_level_fk");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResId).HasName("PK__resource__2090B50D06C7CAAD");

            entity.ToTable("resource");

            entity.HasIndex(e => e.Paid, "idx_resource_paid");

            entity.HasIndex(e => e.TopicId, "idx_resource_topic_id");

            entity.HasIndex(e => e.Type, "idx_resource_type");

            entity.Property(e => e.ResId).HasColumnName("res_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Paid)
                .HasDefaultValue(false)
                .HasColumnName("paid");
            entity.Property(e => e.ResUrl)
                .HasMaxLength(200)
                .HasColumnName("res_url");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");

            entity.HasOne(d => d.Topic).WithMany(p => p.Resources)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("resource_topic_fk");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.RevId).HasName("PK__review__397465D6E4969990");

            entity.ToTable("review");

            entity.HasIndex(e => e.Rid, "idx_review_rid");

            entity.HasIndex(e => e.UserId, "idx_review_user_id");

            entity.HasIndex(e => new { e.UserId, e.Rid }, "user_review_roadmap_uq").IsUnique();

            entity.Property(e => e.RevId).HasColumnName("rev_id");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.RidNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Rid)
                .HasConstraintName("roadmap_review_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_review_fk");
        });

        modelBuilder.Entity<Roadmap>(entity =>
        {
            entity.HasKey(e => e.Rid).HasName("PK__roadmap__C2B7EDE8844A8F08");

            entity.ToTable("roadmap");

            entity.HasIndex(e => e.TrackId, "idx_roadmap_track_id");

            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TrackId).HasColumnName("track_id");

            entity.HasOne(d => d.Track).WithMany(p => p.Roadmaps)
                .HasForeignKey(d => d.TrackId)
                .HasConstraintName("track_roadmap_fk");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PK__topic__D5DAA3E97151A72F");

            entity.ToTable("topic");

            entity.HasIndex(e => e.Lid, "idx_topic_lid");

            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");

            entity.HasOne(d => d.LidNavigation).WithMany(p => p.Topics)
                .HasForeignKey(d => d.Lid)
                .HasConstraintName("level_topic_fk");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("PK__track__24ECC82ED7FB6338");

            entity.ToTable("track");

            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IconUrl)
                .HasMaxLength(200)
                .HasColumnName("icon_url");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserComment>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__user_com__5066738C8EC67004");

            entity.ToTable("user_comment");

            entity.HasIndex(e => e.ComId, "idx_user_comment_com_id");

            entity.HasIndex(e => e.UserId, "idx_user_comment_user_id");

            entity.Property(e => e.ComId)
                .ValueGeneratedNever()
                .HasColumnName("com_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Com).WithOne(p => p.UserComment)
                .HasForeignKey<UserComment>(d => d.ComId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("user_comment_com_fk");

            entity.HasOne(d => d.User).WithMany(p => p.UserComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("user_comment_user_fk");
        });

        modelBuilder.Entity<UserProject>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ProjId }).HasName("PK__user_pro__73BA97CD94F2D5C3");

            entity.ToTable("user_project");

            entity.HasIndex(e => e.ProjId, "idx_user_project_proj_id");

            entity.HasIndex(e => e.UserId, "idx_user_project_user_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ProjId).HasColumnName("proj_id");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("submitted_at");

            entity.HasOne(d => d.Proj).WithMany(p => p.UserProjects)
                .HasForeignKey(d => d.ProjId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_proj__proj___1CBC4616");

            entity.HasOne(d => d.User).WithMany(p => p.UserProjects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_proj__user___1BC821DD");
        });

        modelBuilder.Entity<UserTrack>(entity =>
        {
            entity.HasKey(x => new { x.userId, x.trackId });


            entity.Property(e => e.userId).HasColumnName("user_id");
            entity.Property(e => e.trackId).HasColumnName("track_id");
            entity.Property(e => e.EnrolledAt).HasColumnName("enrolled_at");


            entity.HasOne(e => e.User)
                      .WithMany(u => u.UserTracks)
                      .HasForeignKey(e => e.userId)
                      .HasConstraintName("user_track_user_fk");

            entity.HasOne(e => e.Track)
                  .WithMany(t => t.UserTracks)
                  .HasForeignKey(e => e.trackId)
                  .HasConstraintName("user_track_track_fk");


            entity.ToTable("user_track");
        });

        modelBuilder.Entity<CompletedTopic>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.userId).HasColumnName("user_id");
            entity.Property(x => x.topicId).HasColumnName("topic_id");

            entity.HasOne(x => x.User)
            .WithMany(x => x.CompletedTopics)
            .HasForeignKey(x => x.userId)
            .HasConstraintName("completed_user_user_fk");

            entity.HasOne(x => x.Topic)
            .WithMany(x => x.CompletedTopics)
            .HasForeignKey(x => x.topicId)
            .HasConstraintName("completed_user_topic_fk");

            entity.ToTable("completed_topics");

        });

        modelBuilder.Entity<VwCommentThread>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_comment_threads");

            entity.Property(e => e.ComId).HasColumnName("com_id");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ParentComId).HasColumnName("parent_com_id");
            entity.Property(e => e.Topic)
                .HasMaxLength(100)
                .HasColumnName("topic");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.Type)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<VwLeaderboard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_leaderboard");

            entity.Property(e => e.AvgProgress)
                .HasColumnType("decimal(38, 6)")
                .HasColumnName("avg_progress");
            entity.Property(e => e.CompletedRoadmaps).HasColumnName("completed_roadmaps");
            entity.Property(e => e.Streak).HasColumnName("streak");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<VwRoadmapDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_roadmap_details");

            entity.Property(e => e.AvgRating)
                .HasColumnType("decimal(38, 6)")
                .HasColumnName("avg_rating");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.TotalLevels).HasColumnName("total_levels");
            entity.Property(e => e.TotalResources).HasColumnName("total_resources");
            entity.Property(e => e.TotalReviews).HasColumnName("total_reviews");
            entity.Property(e => e.TotalTopics).HasColumnName("total_topics");
            entity.Property(e => e.Track)
                .HasMaxLength(100)
                .HasColumnName("track");
        });

        modelBuilder.Entity<VwRoadmapReview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_roadmap_reviews");

            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<VwRoadmapStat>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_roadmap_stats");

            entity.Property(e => e.AvgRating)
                .HasColumnType("decimal(38, 6)")
                .HasColumnName("avg_rating");
            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.TotalCompletions).HasColumnName("total_completions");
            entity.Property(e => e.TotalEnrollments).HasColumnName("total_enrollments");
            entity.Property(e => e.Track)
                .HasMaxLength(100)
                .HasColumnName("track");
        });

        modelBuilder.Entity<VwTopicResource>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_topic_resources");

            entity.Property(e => e.Level)
                .HasMaxLength(100)
                .HasColumnName("level");
            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.ResId).HasColumnName("res_id");
            entity.Property(e => e.ResUrl)
                .HasMaxLength(200)
                .HasColumnName("res_url");
            entity.Property(e => e.Resource)
                .HasMaxLength(100)
                .HasColumnName("resource");
            entity.Property(e => e.ResourceOrder).HasColumnName("resource_order");
            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.Topic)
                .HasMaxLength(100)
                .HasColumnName("topic");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.TopicOrder).HasColumnName("topic_order");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
        });

        modelBuilder.Entity<VwUserBookmark>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_user_bookmarks");

            entity.Property(e => e.Level)
                .HasMaxLength(100)
                .HasColumnName("level");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.ResId).HasColumnName("res_id");
            entity.Property(e => e.ResUrl)
                .HasMaxLength(200)
                .HasColumnName("res_url");
            entity.Property(e => e.Resource)
                .HasMaxLength(100)
                .HasColumnName("resource");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.Topic)
                .HasMaxLength(100)
                .HasColumnName("topic");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<VwUserCertificate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_user_certificate");

            entity.Property(e => e.CertUrl)
                .HasMaxLength(200)
                .HasColumnName("cert_url");
            entity.Property(e => e.IssuedAt)
                .HasColumnType("datetime")
                .HasColumnName("issued_at");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.Track)
                .HasMaxLength(100)
                .HasColumnName("track");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<VwUserDashboard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_user_dashboard");

            entity.Property(e => e.CompPerc)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("comp_perc");
            entity.Property(e => e.LastActDate)
                .HasColumnType("datetime")
                .HasColumnName("last_act_date");
            entity.Property(e => e.Level)
                .HasMaxLength(100)
                .HasColumnName("level");
            entity.Property(e => e.Plan)
                .HasMaxLength(100)
                .HasColumnName("plan");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.Streak).HasColumnName("streak");
            entity.Property(e => e.Track)
                .HasMaxLength(100)
                .HasColumnName("track");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<VwUserProgress>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_user_progress");

            entity.Property(e => e.CompPerc)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("comp_perc");
            entity.Property(e => e.Level)
                .HasMaxLength(100)
                .HasColumnName("level");
            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.ProjectStatus)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("project_status");
            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.Roadmap)
                .HasMaxLength(100)
                .HasColumnName("roadmap");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
