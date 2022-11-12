﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QianShiChat.WebApi.Models;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    [Migration("20221112121030_v1.0.2")]
    partial class v102
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("QianShiChat.WebApi.Models.FriendApply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("申请编号");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FriendId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LaseUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<sbyte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.HasIndex("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FriendApply");
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("用户表主键");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasComment("账号");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasComment("创建时间");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasComment("是否已删除");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasComment("昵称");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasComment("密码");

                    b.HasKey("Id");

                    b.ToTable("UserInfo", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Account = "admin",
                            Avatar = "123",
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            NickName = "Admin",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 2,
                            Account = "qianshi",
                            Avatar = "123",
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            NickName = "千矢",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 3,
                            Account = "kuriyama",
                            Avatar = "123",
                            CreateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            NickName = "栗山未来",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        });
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.UserRealtion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("用户关系表主键");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("创建时间");

                    b.Property<int>("FriendId")
                        .HasColumnType("int")
                        .HasComment("朋友编号");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasComment("用户编号");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.HasIndex("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRealtion");
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.FriendApply", b =>
                {
                    b.HasOne("QianShiChat.WebApi.Models.UserInfo", "Friend")
                        .WithMany("FriendApplyFriends")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QianShiChat.WebApi.Models.UserInfo", "User")
                        .WithMany("FriendApplyUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.UserRealtion", b =>
                {
                    b.HasOne("QianShiChat.WebApi.Models.UserInfo", "Friend")
                        .WithMany("Friends")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QianShiChat.WebApi.Models.UserInfo", "User")
                        .WithMany("Realtions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.UserInfo", b =>
                {
                    b.Navigation("FriendApplyFriends");

                    b.Navigation("FriendApplyUsers");

                    b.Navigation("Friends");

                    b.Navigation("Realtions");
                });
#pragma warning restore 612, 618
        }
    }
}
