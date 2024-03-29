﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using kiralamaSistemi.DataAccess.Concrete;

#nullable disable

namespace kiralamasistemi.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserRole<int>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(7)")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<int?>("ModifiedById")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedByName")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(7)")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("ModifiedById")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedByName")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("State")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Araba", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArabaNumrasi")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte>("Durum")
                        .HasColumnType("tinyint");

                    b.Property<short>("EnumArabaRenk")
                        .HasColumnType("smallint");

                    b.Property<short>("EnumArabaTur")
                        .HasColumnType("smallint");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MusteriId")
                        .HasColumnType("int");

                    b.Property<string>("Notlar")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("MusteriId");

                    b.ToTable("Arabalar", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Kiralama", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArabaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BaslangicTarihi")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(7)")
                        .HasDefaultValueSql("getdate()");

                    b.Property<short>("EnumKiralamaSuresi")
                        .HasColumnType("smallint");

                    b.Property<int>("MusteriId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SonTarihi")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(7)")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("ArabaId");

                    b.HasIndex("MusteriId");

                    b.ToTable("Kiralamalar", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DataId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(7)")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("Event")
                        .HasColumnType("int");

                    b.Property<int?>("LoginId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("Module")
                        .HasColumnType("int");

                    b.Property<string>("NewValue")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("OldValue")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("Title")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.HasKey("Id");

                    b.HasIndex("LoginId");

                    b.ToTable("Logs", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.LogModuleMap", b =>
                {
                    b.Property<int>("LogId")
                        .HasColumnType("int");

                    b.Property<int>("Module")
                        .HasColumnType("int");

                    b.Property<int>("DataId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("NVARCHAR(max)");

                    b.HasKey("LogId", "Module", "DataId");

                    b.ToTable("LogModuleSemalar", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Browser")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(7)")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Ip")
                        .HasColumnType("NVARCHAR(32)");

                    b.Property<string>("OsType")
                        .HasColumnType("NVARCHAR(25)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(MAX)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Logins", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Musteri", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Adres")
                        .HasColumnType("nvarchar(50)");

                    b.Property<short>("Cinsiyet")
                        .HasColumnType("smallint");

                    b.Property<string>("Tc")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Telefon")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Musteriler", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Tarife", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArabaId")
                        .HasColumnType("int");

                    b.Property<short>("EnumKiralamaSuresi")
                        .HasColumnType("smallint");

                    b.Property<decimal>("TarifTutar")
                        .HasColumnType("decimal(6,2)");

                    b.HasKey("Id");

                    b.HasIndex("ArabaId");

                    b.ToTable("Tarifeler", (string)null);
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.AppUserRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserRole<int>");

                    b.HasIndex("RoleId");

                    b.HasDiscriminator().HasValue("AppUserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Araba", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.Musteri", "Musteri")
                        .WithMany("Arabalar")
                        .HasForeignKey("MusteriId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Musteri");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Kiralama", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.Araba", "Araba")
                        .WithMany("Kiralamalar")
                        .HasForeignKey("ArabaId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("kiralamaSistemi.Entities.Tables.Musteri", "Musteri")
                        .WithMany("Kiralamalar")
                        .HasForeignKey("MusteriId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Araba");

                    b.Navigation("Musteri");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Log", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.Login", "Login")
                        .WithMany()
                        .HasForeignKey("LoginId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Login");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.LogModuleMap", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.Log", "Log")
                        .WithMany("LogModuleMaps")
                        .HasForeignKey("LogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Log");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Login", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.AppUser", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Musteri", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.AppUser", "User")
                        .WithOne("Musteri")
                        .HasForeignKey("kiralamaSistemi.Entities.Tables.Musteri", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Tarife", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.Araba", "Araba")
                        .WithMany("Tarifalar")
                        .HasForeignKey("ArabaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Araba");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.AppUserRole", b =>
                {
                    b.HasOne("kiralamaSistemi.Entities.Tables.AppRole", "Role")
                        .WithMany("AppRoleUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("kiralamaSistemi.Entities.Tables.AppUser", "User")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.AppRole", b =>
                {
                    b.Navigation("AppRoleUsers");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.AppUser", b =>
                {
                    b.Navigation("AppUserRoles");

                    b.Navigation("Logins");

                    b.Navigation("Musteri")
                        .IsRequired();
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Araba", b =>
                {
                    b.Navigation("Kiralamalar");

                    b.Navigation("Tarifalar");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Log", b =>
                {
                    b.Navigation("LogModuleMaps");
                });

            modelBuilder.Entity("kiralamaSistemi.Entities.Tables.Musteri", b =>
                {
                    b.Navigation("Arabalar");

                    b.Navigation("Kiralamalar");
                });
#pragma warning restore 612, 618
        }
    }
}
