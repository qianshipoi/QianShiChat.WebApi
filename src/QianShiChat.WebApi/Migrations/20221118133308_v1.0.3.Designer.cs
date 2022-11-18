﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QianShiChat.WebApi.Models;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    [Migration("20221118133308_v1.0.3")]
    partial class v103
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("QianShiChat.WebApi.Models.Entity.ChatMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint");

                    b.Property<int>("FromId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("LastUpdateTime")
                        .HasColumnType("bigint");

                    b.Property<sbyte>("MessageType")
                        .HasColumnType("tinyint");

                    b.Property<sbyte>("SendType")
                        .HasColumnType("tinyint");

                    b.Property<int>("ToId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("Id");

                    b.ToTable("ChatMessage");
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.Entity.MessageCursor", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasComment("sender user id");

                    b.Property<int>("ToId")
                        .HasColumnType("int")
                        .HasComment("receiver id (user or group)");

                    b.Property<sbyte>("SendType")
                        .HasColumnType("tinyint")
                        .HasComment("ToId type");

                    b.Property<long>("CurrentPosition")
                        .HasColumnType("bigint")
                        .HasComment("message current position");

                    b.Property<long>("LastUpdateTime")
                        .HasColumnType("bigint");

                    b.Property<long>("StartPosition")
                        .HasColumnType("bigint")
                        .HasComment("message start position");

                    b.HasKey("UserId", "ToId", "SendType");

                    b.ToTable("MessageCursor");
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.FriendApply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("申请编号");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint");

                    b.Property<int>("FriendId")
                        .HasColumnType("int");

                    b.Property<long>("LaseUpdateTime")
                        .HasColumnType("bigint");

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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreateTime = 1668583992424L,
                            FriendId = 2,
                            LaseUpdateTime = 1668583992424L,
                            Remark = "很高兴认识你。",
                            Status = (sbyte)3,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreateTime = 1668583992424L,
                            FriendId = 2,
                            LaseUpdateTime = 1668583992424L,
                            Remark = "Nice to meet you.",
                            Status = (sbyte)2,
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            CreateTime = 1668583992424L,
                            FriendId = 3,
                            LaseUpdateTime = 1668583992424L,
                            Remark = "hi!",
                            Status = (sbyte)1,
                            UserId = 1
                        });
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
                        .HasColumnType("longtext");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint")
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
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "Admin",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 2,
                            Account = "qianshi",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "千矢",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 3,
                            Account = "kuriyama",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "栗山未来",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 4,
                            Account = "admin1",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "Admin1",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 5,
                            Account = "qianshi1",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "千矢1",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 6,
                            Account = "kuriyama1",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "栗山未来1",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 7,
                            Account = "admin2",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "Admin2",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 8,
                            Account = "qianshi2",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "千矢2",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        },
                        new
                        {
                            Id = 9,
                            Account = "kuriyama2",
                            CreateTime = 1668583992424L,
                            IsDeleted = false,
                            NickName = "栗山未来2",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E"
                        });
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.UserRealtion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("用户关系表主键");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint")
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreateTime = 1668583992424L,
                            FriendId = 2,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreateTime = 1668583992424L,
                            FriendId = 1,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            CreateTime = 1668583992424L,
                            FriendId = 3,
                            UserId = 2
                        },
                        new
                        {
                            Id = 4,
                            CreateTime = 1668583992424L,
                            FriendId = 2,
                            UserId = 3
                        });
                });

            modelBuilder.Entity("QianShiChat.WebApi.Models.Entity.ChatMessage", b =>
                {
                    b.HasOne("QianShiChat.WebApi.Models.UserInfo", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromUser");
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
