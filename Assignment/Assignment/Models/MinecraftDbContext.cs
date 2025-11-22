using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Models;

public partial class MinecraftDbContext : DbContext
{
    public MinecraftDbContext()
    {
    }

    public MinecraftDbContext(DbContextOptions<MinecraftDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterTask> CharacterTasks { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Mob> Mobs { get; set; }

    public virtual DbSet<MobDrop> MobDrops { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ShopItem> ShopItems { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HUY\\PHUONGHUY;Initial Catalog=MinecraftDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5865A461110");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E434F1FE65").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__A9D105346EDF3EB7").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.DateCreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(16);
            entity.Property(e => e.Username).HasMaxLength(15);
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.CharacterId).HasName("PK__Characte__757BCA4047DF183C");

            entity.ToTable("Character");

            entity.Property(e => e.CharacterId).HasColumnName("CharacterID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.LevelXp)
                .HasDefaultValue(0)
                .HasColumnName("LevelXP");
            entity.Property(e => e.Mode).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.Characters)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Character__Accou__531856C7");

            entity.HasMany(d => d.Vehicles).WithMany(p => p.Characters)
                .UsingEntity<Dictionary<string, object>>(
                    "VehicleOwn",
                    r => r.HasOne<Vehicle>().WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Vehicle_O__Vehic__690797E6"),
                    l => l.HasOne<Character>().WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Vehicle_O__Chara__681373AD"),
                    j =>
                    {
                        j.HasKey("CharacterId", "VehicleId").HasName("PK__Vehicle___410D7F0B5E36E53D");
                        j.ToTable("Vehicle_Own");
                        j.IndexerProperty<int>("CharacterId").HasColumnName("CharacterID");
                        j.IndexerProperty<int>("VehicleId").HasColumnName("VehicleID");
                    });
        });

        modelBuilder.Entity<CharacterTask>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.TaskId }).HasName("PK__Characte__72BD5EDDE59468BF");

            entity.ToTable("Character_Task");

            entity.Property(e => e.CharacterId).HasColumnName("CharacterID");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.DateCompleted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterTasks)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Character__Chara__7EF6D905");

            entity.HasOne(d => d.Task).WithMany(p => p.CharacterTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Character__TaskI__7FEAFD3E");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.ResourceId }).HasName("PK__Inventor__9196D25454A2AAD2");

            entity.ToTable("Inventory");

            entity.Property(e => e.CharacterId).HasColumnName("CharacterID");
            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");

            entity.HasOne(d => d.Character).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Chara__58D1301D");

            entity.HasOne(d => d.Resource).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Resou__59C55456");
        });

        modelBuilder.Entity<Mob>(entity =>
        {
            entity.HasKey(e => e.MobId).HasName("PK__Mob__FB9B04F5840639A9");

            entity.ToTable("Mob");

            entity.Property(e => e.MobId).HasColumnName("MobID");
            entity.Property(e => e.Description).HasMaxLength(30);
            entity.Property(e => e.MobName).HasMaxLength(10);
        });

        modelBuilder.Entity<MobDrop>(entity =>
        {
            entity.HasKey(e => e.DropId).HasName("PK__Mob_Drop__47D9F44365EEC4CC");

            entity.ToTable("Mob_Drop");

            entity.Property(e => e.DropId).HasColumnName("DropID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.MobId).HasColumnName("MobID");

            entity.HasOne(d => d.Item).WithMany(p => p.MobDrops)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mob_Drop__ItemID__70A8B9AE");

            entity.HasOne(d => d.Mob).WithMany(p => p.MobDrops)
                .HasForeignKey(d => d.MobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mob_Drop__MobID__6FB49575");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BDEE1AB3CBD");

            entity.ToTable("Purchase");

            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.CharacterId).HasColumnName("CharacterID");
            entity.Property(e => e.DatePurchased)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Character).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Purchase__Charac__6166761E");

            entity.HasOne(d => d.Item).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Purchase__ItemID__625A9A57");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__Resource__4ED1814F24B83E1C");

            entity.ToTable("Resource");

            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
            entity.Property(e => e.ResourceName).HasMaxLength(20);
            entity.Property(e => e.ResourceType).HasMaxLength(15);
        });

        modelBuilder.Entity<ShopItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__ShopItem__727E83EBC154A907");

            entity.ToTable("ShopItem");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Description).HasMaxLength(30);
            entity.Property(e => e.Image).HasMaxLength(30);
            entity.Property(e => e.ItemName).HasMaxLength(20);
            entity.Property(e => e.ItemType).HasMaxLength(15);
            entity.Property(e => e.PriceXp).HasColumnName("PriceXP");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__7C6949D1917AB8E1");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.RewardXp).HasColumnName("RewardXP");
            entity.Property(e => e.TaskName).HasMaxLength(50);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicle__476B54B27A5402D7");

            entity.ToTable("Vehicle");

            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            entity.Property(e => e.Description).HasMaxLength(30);
            entity.Property(e => e.VehicleName).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
