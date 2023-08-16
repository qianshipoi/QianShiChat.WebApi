﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QianShiChat.Domain;

#nullable disable

namespace QianShiChat.WebApi.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    partial class ChatDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("QianShiChat.Domain.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint");

                    b.Property<long>("DeleteTime")
                        .HasColumnType("bigint");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("PreviewPath")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<string>("RawPath")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<long>("UpdateTime")
                        .HasColumnType("bigint");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Hash");

                    b.HasIndex("UserId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.ChatMessage", b =>
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

                    b.Property<sbyte>("MessageType")
                        .HasColumnType("tinyint");

                    b.Property<sbyte>("SendType")
                        .HasColumnType("tinyint");

                    b.Property<int>("ToId")
                        .HasColumnType("int");

                    b.Property<long>("UpdateTime")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.ToTable("ChatMessage");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.DefaultAvatar", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("avatar id.");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint")
                        .HasComment("create time.");

                    b.Property<long>("DeleteTime")
                        .HasColumnType("bigint")
                        .HasComment("delete time.");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("is deleted.");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("file path.");

                    b.Property<ulong>("Size")
                        .HasColumnType("bigint unsigned")
                        .HasComment("file size.");

                    b.Property<long>("UpdateTime")
                        .HasColumnType("bigint")
                        .HasComment("update time.");

                    b.HasKey("Id");

                    b.ToTable("DefaultAvatars");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/1.jpg",
                            Size = 208896ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 2L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/2.jpg",
                            Size = 30720ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 3L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/3.jpg",
                            Size = 87757ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 4L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/4.jpg",
                            Size = 159744ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 5L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/5.jpg",
                            Size = 73216ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 6L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/6.jpg",
                            Size = 108544ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 7L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/7.jpg",
                            Size = 93799ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 8L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/8.jpg",
                            Size = 112640ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 9L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/9.jpg",
                            Size = 108544ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 10L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/10.jpg",
                            Size = 113664ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 11L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/11.png",
                            Size = 679936ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 12L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/12.gif",
                            Size = 900096ul,
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 13L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/13.gif",
                            Size = 748544ul,
                            UpdateTime = 0L
                        });
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.FriendApply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint");

                    b.Property<int>("FriendId")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<sbyte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<long>("UpdateTime")
                        .HasColumnType("bigint");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.HasIndex("UserId");

                    b.ToTable("FriendApply");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreateTime = 1668583992424L,
                            FriendId = 2,
                            Remark = "很高兴认识你。",
                            Status = (sbyte)3,
                            UpdateTime = 1668583992424L,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreateTime = 1668583992424L,
                            FriendId = 2,
                            Remark = "Nice to meet you.",
                            Status = (sbyte)2,
                            UpdateTime = 1668583992424L,
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            CreateTime = 1668583992424L,
                            FriendId = 3,
                            Remark = "hi!",
                            Status = (sbyte)1,
                            UpdateTime = 1668583992424L,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint");

                    b.Property<long>("DeleteTime")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<int>("TotalUser")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.GroupApply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<sbyte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<long>("UpdateTime")
                        .HasColumnType("bigint");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupApplies");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.MessageCursor", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasComment("user id");

                    b.Property<long>("LastUpdateTime")
                        .HasColumnType("bigint");

                    b.Property<long>("Postiton")
                        .HasColumnType("bigint")
                        .HasComment("message position");

                    b.HasKey("UserId");

                    b.ToTable("MessageCursor");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserAvatar", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("avatar id.");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint")
                        .HasComment("create time.");

                    b.Property<long>("DeleteTime")
                        .HasColumnType("bigint")
                        .HasComment("delete time.");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("is deleted.");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasComment("file path.");

                    b.Property<ulong>("Size")
                        .HasColumnType("bigint unsigned")
                        .HasComment("file size.");

                    b.Property<long>("UpdateTime")
                        .HasColumnType("bigint")
                        .HasComment("update time.");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasComment("user id.");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserAvatars");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/1.jpg",
                            Size = 208896ul,
                            UpdateTime = 0L,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/2.jpg",
                            Size = 30720ul,
                            UpdateTime = 0L,
                            UserId = 0
                        },
                        new
                        {
                            Id = 3L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/3.jpg",
                            Size = 87757ul,
                            UpdateTime = 0L,
                            UserId = 0
                        },
                        new
                        {
                            Id = 4L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/4.jpg",
                            Size = 159744ul,
                            UpdateTime = 0L,
                            UserId = 0
                        },
                        new
                        {
                            Id = 5L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/5.jpg",
                            Size = 73216ul,
                            UpdateTime = 0L,
                            UserId = 0
                        },
                        new
                        {
                            Id = 6L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/6.jpg",
                            Size = 108544ul,
                            UpdateTime = 0L,
                            UserId = 0
                        },
                        new
                        {
                            Id = 7L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/7.jpg",
                            Size = 93799ul,
                            UpdateTime = 0L,
                            UserId = 0
                        },
                        new
                        {
                            Id = 8L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/8.jpg",
                            Size = 112640ul,
                            UpdateTime = 0L,
                            UserId = 0
                        },
                        new
                        {
                            Id = 9L,
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            Path = "/Raw/DefaultAvatar/9.jpg",
                            Size = 108544ul,
                            UpdateTime = 0L,
                            UserId = 0
                        });
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserGroupRealtion", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<long>("CreateTime")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("UserGroupRealtions");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserInfo", b =>
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

                    b.Property<long>("DeleteTime")
                        .HasColumnType("bigint")
                        .HasComment("删除时间");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasComment("描述");

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

                    b.Property<long>("UpdateTime")
                        .HasColumnType("bigint")
                        .HasComment("修改时间");

                    b.HasKey("Id");

                    b.ToTable("UserInfo", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Account = "admin",
                            Avatar = "/Raw/DefaultAvatar/1.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            Description = "炒鸡管理员！",
                            IsDeleted = false,
                            NickName = "Admin",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 2,
                            Account = "qianshi",
                            Avatar = "/Raw/DefaultAvatar/2.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            Description = "道歉的时候露出肚皮才是常识！",
                            IsDeleted = false,
                            NickName = "千矢",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 3,
                            Account = "kuriyama",
                            Avatar = "/Raw/DefaultAvatar/3.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            Description = "不愉快です",
                            IsDeleted = false,
                            NickName = "栗山未来",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 4,
                            Account = "admin1",
                            Avatar = "/Raw/DefaultAvatar/4.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            NickName = "Admin1",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 5,
                            Account = "qianshi1",
                            Avatar = "/Raw/DefaultAvatar/5.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            NickName = "千矢1",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 6,
                            Account = "kuriyama1",
                            Avatar = "/Raw/DefaultAvatar/6.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            NickName = "栗山未来1",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 7,
                            Account = "admin2",
                            Avatar = "/Raw/DefaultAvatar/7.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            NickName = "Admin2",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 8,
                            Account = "qianshi2",
                            Avatar = "/Raw/DefaultAvatar/8.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            NickName = "千矢2",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        },
                        new
                        {
                            Id = 9,
                            Account = "kuriyama2",
                            Avatar = "/Raw/DefaultAvatar/9.jpg",
                            CreateTime = 1668583992424L,
                            DeleteTime = 0L,
                            IsDeleted = false,
                            NickName = "栗山未来2",
                            Password = "E10ADC3949BA59ABBE56E057F20F883E",
                            UpdateTime = 0L
                        });
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserRealtion", b =>
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

            modelBuilder.Entity("QianShiChat.Domain.Models.Attachment", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithMany("Attachments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.ChatMessage", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromUser");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.FriendApply", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "Friend")
                        .WithMany("FriendApplyFriends")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithMany("FriendApplyUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.Group", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.GroupApply", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.MessageCursor", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithOne("Cursor")
                        .HasForeignKey("QianShiChat.Domain.Models.MessageCursor", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserAvatar", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithMany("UserAvatars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserGroupRealtion", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.Group", "Group")
                        .WithMany("UserGroupRealtions")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserRealtion", b =>
                {
                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "Friend")
                        .WithMany("Friends")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QianShiChat.Domain.Models.UserInfo", "User")
                        .WithMany("Realtions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.Group", b =>
                {
                    b.Navigation("UserGroupRealtions");
                });

            modelBuilder.Entity("QianShiChat.Domain.Models.UserInfo", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Cursor")
                        .IsRequired();

                    b.Navigation("FriendApplyFriends");

                    b.Navigation("FriendApplyUsers");

                    b.Navigation("Friends");

                    b.Navigation("Groups");

                    b.Navigation("Realtions");

                    b.Navigation("UserAvatars");

                    b.Navigation("UserGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
