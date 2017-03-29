using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.Users;
using Model.Authentication;
using System.Linq;
using Model.Logs;
using Model.Confirm;
using Model.Auctions;

namespace Model
{
    public class PaskolDbContext : IdentityDbContext<PaskolUser>
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Customer> Buyers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Copyright> Copyrights { get; set; }
        public DbSet<PermissionsCategory> PermissionsCategories { get; set; }
        public DbSet<PermissionPropertyValue> PermissionPropertyValues { get; set; }
        public DbSet<PermissionProperty> PermissionProperties { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<ActionOnLogData> LogsData { get; set; }
        public DbSet<Purchase.Purchase> Purchase { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<UserActiveLog> UserActiveLogs { get; set; }
        public DbSet<NCRegister> NCRegisters { get; set; }
        public DbSet<NCDataUpload> NCDataUploads { get; set; }
        public DbSet<NCPurchase> NCPurchases { get; set; }
        public DbSet<MailUnsubscribe> MailUnsubscribe { get; set; }

        public PaskolDbContext()
            : base("PaskolDb", throwIfV1Schema: false)
        {
        }
        
        static PaskolDbContext()
        {
            Database.SetInitializer<PaskolDbContext>(new IdentityDbInit());
        }

        public static PaskolDbContext Create()
        {
            return new PaskolDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaskolUser>().HasOptional(u => u.Artist).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<PaskolUser>().HasOptional(u => u.Customer).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<PaskolUser>().HasOptional(u => u.ArtistAgent).WithRequired().WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class IdentityDbInit : NullDatabaseInitializer<PaskolDbContext>
    {
        //protected override void Seed(PaskolDbContext context)
        //{
        //    PerformInitialSetup(context);
        //    base.Seed(context);
        //}

        //public void PerformInitialSetup(PaskolDbContext context)
        //{
        //    PaskolUserManager userMgr = new PaskolUserManager(new UserStore<PaskolUser>(context));
        //    PaskolRoleManager roleMgr = new PaskolRoleManager(new RoleStore<PaskolRole>(context));

        //    // Roles
        //    roleMgr.Create(new PaskolRole(PasskolRoles.SystemAdmin.ToString()));
        //    roleMgr.Create(new PaskolRole(PasskolRoles.AdminProfile.ToString()));
        //    roleMgr.Create(new PaskolRole(PasskolRoles.AdminLabel.ToString()));
        //    roleMgr.Create(new PaskolRole(PasskolRoles.AdminMusicEditor.ToString()));
        //    roleMgr.Create(new PaskolRole(PasskolRoles.AdminFinance.ToString()));
        //    roleMgr.Create(new PaskolRole(PasskolRoles.Artist.ToString()));
        //    roleMgr.Create(new PaskolRole(PasskolRoles.Customer.ToString()));
        //    roleMgr.Create(new PaskolRole(PasskolRoles.ArtistAgent.ToString()));
            
        //    // Admin
        //    userMgr.Create(new PaskolUser()
        //    {
        //        Status = UserStatus.Active,
        //        UserType = UserType.Admin,
        //        RegisteredDate = DateTime.Now,
        //        UserName = "Moshe",
        //        Email = "admin@example.com"
        //    }, "Moshe123");
        //    userMgr.AddToRole(userMgr.FindByName("Moshe").Id, PasskolRoles.SystemAdmin.ToString());

        //    // artist agent
        //    userMgr.Create(new PaskolUser()
        //    {
        //        Status = UserStatus.Active,
        //        UserType = UserType.ArtistAgent,
        //        RegisteredDate = DateTime.Now,
        //        UserName = "mgr_mgr",
        //        Email = "mgr@mgr.com",
        //        ArtistAgent = new ArtistAgent()
        //        {
        //            Artists = new List<Artist>()
        //        }
        //    }, "Moshe123");
        //    userMgr.AddToRole(userMgr.FindByName("mgr_mgr").Id,
        //        PasskolRoles.ArtistAgent.ToString());

        //    // Artist
        //    var res = userMgr.Create(new PaskolUser()
        //    {
        //        UserType = UserType.Artist,
        //        Status = UserStatus.Active,
        //        RegisteredDate = DateTime.Now,
        //        UserName = "Asaf_Amdursky",
        //        Email = "asaf@gamil.com",
        //        Artist = new Artist()
        //        {
        //            ArtistAgent =  userMgr.FindByName("mgr_mgr").ArtistAgent,
        //            ParticipateInAuction = true,
        //            AgentMail = "maosi@gmail.com"
        //        }
        //    }, "Moshe123");
        //    userMgr.AddToRole(userMgr.FindByName("Asaf_Amdursky").Id, "Artist");
            
        //    for (int i = 0; i < 30; i++)
        //    {
        //        Confirmation c = new Confirmation();
        //        PaskolUser p = new PaskolUser()
        //        {
        //            UserType = UserType.Artist,
        //            Status = UserStatus.WaitingNewArtist,
        //            RegisteredDate = DateTime.Now,
        //            UserName = (i * 11).ToString(),
        //            Email = i + "asaf@gamil.com",
        //            Artist = new Artist()
        //            {
        //                ParticipateInAuction = true,
        //                AgentMail = i + "maosi@gmail.com"
        //            }
        //        };

        //        userMgr.Create(p, i + "Moshe123");

        //        c.ConfirmType = ConfirmType.NewArtist;
        //        c.DateUpdate = DateTime.Now;
        //        c.EntityId = p.Id;
        //        c.ManagerNote = p.Note;
        //        c.Name = p.UserName;
        //        context.Confirmations.Add(c);
        //        userMgr.AddToRole(userMgr.FindByName((i * 11).ToString()).Id, "Artist");
        //    }

        //    //Confirm Edited Artist
        //    PaskolUser coUser = new PaskolUser()
        //    {
        //        UserType = UserType.Artist,
        //        Status = UserStatus.WaitingEditedArtist,
        //        RegisteredDate = DateTime.Now,
        //        UserName = "test1",
        //        Email = "test1@gamil.com",
        //        Artist = new Artist()
        //        {
        //            TempArtist = new TempArtist()
        //            {
        //                UserName = "newName",
        //                Email = "newEmail"
        //            },
        //            AgentMail = "maosi@gmail.com"
        //        }
        //    };
        //    Confirmation confirmUser = new Confirmation()
        //    {
        //        ConfirmType = ConfirmType.UpdateArtist,
        //        DateUpdate = DateTime.Now,
        //        Name = "test1",
        //        EntityId = coUser.Id
        //    };

        //    userMgr.Create(coUser, "Moshe123");
        //    context.Confirmations.Add(confirmUser);

        //    // Not completed User
        //    NCRegister ncUser1 = new NCRegister()
        //    {
        //        DateTime = DateTime.Now,
        //        Id = coUser.Id,
        //        NCUserType = NCUserType.ArtistProfile,
        //        User = coUser
        //    };

        //    NCRegister ncUser2 = new NCRegister()
        //    {
        //        DateTime = DateTime.Now,
        //        Id = userMgr.Users.FirstOrDefault().Id,
        //        NCUserType = NCUserType.ArtistProfile,
        //        User = userMgr.Users.FirstOrDefault()
        //    };
            
        //    NCRegister ncUser3 = new NCRegister()
        //    {
        //        DateTime = DateTime.Now,
        //        Id = coUser.Id,
        //        NCUserType = NCUserType.Register,
        //        User = coUser
        //    };

        //    NCRegister ncUser4 = new NCRegister()
        //    {
        //        DateTime = DateTime.Now,
        //        Id = coUser.Id,
        //        NCUserType = NCUserType.Register,
        //        User = coUser
        //    };

        //    context.NCRegisters.Add(ncUser1);
        //    context.NCRegisters.Add(ncUser2);
        //    context.NCRegisters.Add(ncUser3);
        //    context.NCRegisters.Add(ncUser4);
        //    // Customer
        //    res = userMgr.Create(new PaskolUser()
        //    {
        //        Status = UserStatus.Blocked,
        //        UserType = Users.UserType.Customer,
        //        RegisteredDate = DateTime.Now,
        //        UserName = "Rochesh_Tochen",
        //        Email = "Rochesh@gamil.com",
        //        Customer = new Customer()
        //        {
        //            CompanyName = "songs"
        //        }
        //    }, "Moshe123");
        //    userMgr.AddToRole(userMgr.FindByName("Rochesh_Tochen").Id, "Customer");

        //    // Tags
        //    List<Tag> tags = new List<Tag>()
        //        {
        //            new Tag()
        //            {
        //                ID = Guid.NewGuid(),
        //                Name ="מילות מפתח",
        //                IsCanDelete = false,
        //                IsDynamic = true,
        //                IsPublicTag = false,
        //                Level = TagLevel.Root,
        //                ViewOrder = 30,
        //            },
        //            new Tag()
        //            {
        //                ID = Guid.NewGuid(),
        //                Name ="חדש במערכת",
        //                IsCanDelete = false,
        //                IsDynamic = true,
        //                IsPublicTag = false,
        //                Level = TagLevel.Root,
        //                ViewOrder = 30,
        //            },
        //            new Tag()
        //            {
        //                ID = Guid.NewGuid(),
        //                Name ="נשמע כמו - Sound alike",
        //                IsCanDelete = false,
        //                IsDynamic = false,
        //                IsPublicTag = false,
        //                Level = TagLevel.Root,
        //                ViewOrder = 30,
        //            },
        //            new Tag()
        //            {
        //                ID = Guid.NewGuid(),
        //                Name ="זאנר",
        //                IsCanDelete = true,
        //                IsDynamic = false,
        //                IsPublicTag = true,
        //                Level = TagLevel.Root,
        //                ViewOrder = 30,
        //            },
        //            new Tag()
        //            {
        //                ID = Guid.NewGuid(),
        //                Name ="אווירה/FEEL",
        //                IsCanDelete = true,
        //                IsDynamic = false,
        //                IsPublicTag = true,
        //                Level = TagLevel.Root,
        //                ViewOrder = 30,
        //            },
        //            new Tag()
        //            {
        //                ID = Guid.NewGuid(),
        //                Name ="מצבי רוח",
        //                IsCanDelete = true,
        //                IsDynamic = false,
        //                IsPublicTag = true,
        //                Level = TagLevel.Root,
        //                ViewOrder = 30,
        //            }
        //        };

        //    // Save
        //    var resTags = context.Tags.AddRange(tags);
        //    context.SaveChanges();
        //    var arts = userMgr.Users.Where(x => x.UserType == UserType.Artist).ToList();
        //    var tagLst = resTags.Where(tg => !tg.IsDynamic).ToList();

        //    List<Music> lstMsc = new List<Music>();
        //    lstMsc.Add(new Music()
        //    {
        //        ArtistID = arts[0].Id,
        //        BPM = 20,
        //        Comment = "123456",
        //        CostLevel = PriceLevel.Medium,
        //        CreateDate = DateTime.Now,
        //        EnglishName = "Lonely",
        //        Exceptions = "לא רוצה!",
        //        HebrewName = "בדד",
        //        MusicComposer = "בטהובן",
        //        MusicLength = TimeSpan.FromSeconds(187),
        //        MusicPerformer = "אברהם פריד",
        //        MusicWriter = "שייקספיר",
        //        SongText = "בדד, בדד, בדד",
        //        Status = MusicActiveStatus.Public,
        //        Tags = tagLst,
        //    });
        //    for (int i = 0; i < 25; i++)
        //    {
        //        Music m = new Music()
        //        {
        //            ArtistID = arts[i % 30].Id,
        //            BPM = 20,
        //            Comment = "123456",
        //            CostLevel = PriceLevel.Medium,
        //            CreateDate = DateTime.Now - TimeSpan.FromHours(i),
        //            EnglishName = "Lonely",
        //            Exceptions = "לא רוצה!",
        //            HebrewName = (i * 11).ToString(),
        //            MusicComposer = "בטהובן",
        //            MusicLength = TimeSpan.FromSeconds(187),
        //            MusicPerformer = "אברהם פריד",
        //            MusicWriter = "שייקספיר",
        //            SongText = "בדד, בדד, בדד",

        //            Status = MusicActiveStatus.New,
        //            Tags = tagLst,
        //        };

        //        NCDataUpload ncDU = new NCDataUpload()
        //        {
        //            DateTime = DateTime.Now,
        //            Music = m,
        //            UploaderMail = "someMail@gamil.com",
        //            UploaderName = "someName",
        //            UploaderPhone = "0547784444"
        //        };

        //        context.NCDataUploads.Add(ncDU);
        //    }
        //    for (int i = 25; i < 50; i++)
        //    {
        //        Music m = new Music()
        //        {
        //            ArtistID = arts[i % 30].Id,
        //            BPM = 20,
        //            Comment = "123456",
        //            CostLevel = PriceLevel.Medium,
        //            CreateDate = DateTime.Now - TimeSpan.FromHours(i),
        //            EnglishName = "Lonely",
        //            Exceptions = "לא רוצה!",
        //            HebrewName = (i * 11).ToString(),
        //            MusicComposer = "בטהובן",
        //            MusicLength = TimeSpan.FromSeconds(187),
        //            MusicPerformer = "אברהם פריד",
        //            MusicWriter = "שייקספיר",
        //            SongText = "בדד, בדד, בדד",

        //            Status = MusicActiveStatus.New,
        //            Tags = tagLst,
        //        };
        //        Confirmation c = new Confirmation()
        //        {
        //            ConfirmType = ConfirmType.NewMusic,
        //            DateUpdate = DateTime.Now.AddDays(i),
        //            EntityId = (i + 2).ToString(),
        //            Name = m.HebrewName,
        //            ManagerNote = "אין"
        //        };

        //        context.Confirmations.Add(c);
        //        lstMsc.Add(m);
        //    }

            
        //    var resMusics = context.Musics.AddRange(lstMsc);

        //    // LogData
        //    var userId = userMgr.Users.FirstOrDefault(u => u.UserType == UserType.Customer).Id;
        //    var eId = resMusics.First().ID.ToString();
            
        //    var logs = new List<ActionOnLogData>()
        //    {
        //        new ActionOnLogData()
        //        {
        //            EntityId = "1",
        //            UserID = userId,
        //            EntityType = EntityType.Music,
        //            ActionType = LogActionType.Listen,
        //            DateTime = DateTime.Now,
        //            Value = 20
        //        },
        //        new ActionOnLogData()
        //        {
        //            EntityId = "1",
        //            UserID = userId,
        //            EntityType = EntityType.Music,
        //            ActionType = LogActionType.Watch,
        //            DateTime = DateTime.Now,
        //            Value = 10
        //        },
        //        new ActionOnLogData()
        //        {
        //            EntityId = "1",
        //            UserID = userId,
        //            EntityType = EntityType.Music,
        //            ActionType = LogActionType.Watch,
        //            DateTime = DateTime.Now.AddDays(-2),
        //            Value = 5
        //        },
        //        new ActionOnLogData()
        //        {
        //            EntityId = userMgr.Users.FirstOrDefault(u => u.UserType == UserType.Artist).Id,
        //            UserID = userId,
        //            EntityType = EntityType.Artist,
        //            ActionType = LogActionType.Watch,
        //            DateTime = DateTime.Now.AddDays(-2),
        //            Value = 5
        //        },
        //        new ActionOnLogData()
        //        {
        //            EntityId =  context.Tags.First().ID.ToString(),
        //            UserID = userId,
        //            EntityType = EntityType.Tag,
        //            ActionType = LogActionType.Watch,
        //            DateTime = DateTime.Now.AddDays(-2),
        //            Value = 5
        //        },
        //        new ActionOnLogData()
        //        {
        //            EntityId =  "45",
        //            UserID = userId,
        //            EntityType = EntityType.Music,
        //            ActionType = LogActionType.Listen,
        //            DateTime = DateTime.Now.AddDays(-2),
        //            Value = 2
        //        }
        //    };
        //    context.LogsData.AddRange(logs);
        //    context.SaveChanges();
            
        //    var musicId = 1;
        //    var lstCopy = new List<Copyright>()
        //    {
        //        new Copyright()
        //        {
        //            MusicID = musicId,
        //            Type = CopyrightType.Composer,
        //            CopyrightAuthorName = "רוטשילד",
        //            Percents = 20,
        //        },
        //        new Copyright()
        //        {
        //            MusicID = musicId,
        //            Type = CopyrightType.Cultivator,
        //            CopyrightAuthorName = "פיימנטה",
        //            Percents = 80
        //        }
        //    };

        //    Confirmation phoneConfirm = new Confirmation()
        //    {
        //        ConfirmType = ConfirmType.PhonePurchase,
        //        DateUpdate = DateTime.Now,
        //        EntityId = musicId.ToString(),
        //        Name = resMusics.First().HebrewName
        //    };

        //    context.Copyrights.AddRange(lstCopy);
        //    context.SaveChanges();
        //    CreatePermissionsData(context);

        //    List<Auction> lstauc = new List<Auction>();
        //    lstauc.Add(new Auction()
        //    {
        //        AuctionBudget = 100,
        //        AuctionName = "ביסלי אוסם",
        //        AuctionNote = "אחלה מכרז",
        //        AuctionStatus = AuctionStatus.ConfirmWating,
        //        Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //        CloseDate = DateTime.Now.AddDays(31),
        //        MusicLength = 200,
        //        MusicProperties = "צריכה להיות מוזיקה מגניבה",
        //        OpenDate = DateTime.Now.AddDays(-2),
        //        PickWinnerDate = DateTime.Now.AddDays(10),
        //        UsingName = "לצורך כלשהוא"
        //    });
        //    lstauc.Add(new Auction()
        //    {
        //        AuctionBudget = 100,
        //        AuctionName = "במבה שוש",
        //        AuctionNote = "nfrx מכרז",
        //        AuctionStatus = AuctionStatus.Open,
        //        Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //        CloseDate = DateTime.Now.AddDays(-2),
        //        MusicLength = 200,
        //        PickWinnerDate = DateTime.Now.AddDays(-1),
        //        MusicProperties = "צריכה להיות מוזיקה מגניבה",
        //        OpenDate = DateTime.Now.AddDays(-12),
        //        Participants = userMgr.Users.Where(u => u.UserType == UserType.Artist).Select(s => s.Artist).Take(10).ToList(),
        //        WinnerArtist = userMgr.Users.FirstOrDefault(u => u.UserType == UserType.Artist),
        //        UsingName = "לצורך כלשהוא"
        //    });
        //    lstauc.Add(new Auction()
        //    {
        //        AuctionBudget = 100,
        //        AuctionName = "ddd",
        //        AuctionNote = "nfrx מכרז",
        //        AuctionStatus = AuctionStatus.Open,
        //        Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //        CloseDate = DateTime.Now.AddDays(-2),
        //        MusicLength = 200,
        //        PickWinnerDate = DateTime.Now.AddDays(12),
        //        MusicProperties = "צריכה להיות מוזיקה מגניבה",
        //        OpenDate = DateTime.Now.AddDays(-12),
        //        Participants = userMgr.Users.Where(u => u.UserType == UserType.Artist).Select(s => s.Artist).Take(10).ToList(),
                
        //        UsingName = "לצורך כלשהוא"
        //    });

        //    context.Auctions.AddRange(lstauc);
        //    context.SaveChanges();

        //    var Purchases = new List<Purchase.Purchase>()
        //    {
        //        new Purchase.Purchase()
        //        {
        //            ArtistEarn = 100,
        //            PasskolEarn = 50,
        //            Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //            Music = resMusics.FirstOrDefault(),
        //            Permision = context.Permissions.FirstOrDefault(),
        //            PurchaseDate = DateTime.Now,
        //            CustomerReference = "sdfgf-234234-fg67"
        //        },
        //        new Purchase.Purchase()
        //        {
        //            ArtistEarn = 300,
        //            PasskolEarn = 70,
        //            Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //            Music = resMusics.FirstOrDefault(),
        //            Permision = context.Permissions.FirstOrDefault(),
        //            PurchaseDate = DateTime.Now,
        //            CustomerReference = "sdfgf-2f234-fg67"
        //        },
        //        new Purchase.Purchase()
        //        {
        //            ArtistEarn = 270,
        //            PasskolEarn = 123,
        //            Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //            Music = resMusics.FirstOrDefault(),
        //            Permision = context.Permissions.FirstOrDefault(),
        //            PurchaseDate = DateTime.Now,
        //            CustomerReference = "sdfgf-2f234-fg67",
        //            PurchaseCost = 393
        //        },
        //        new Purchase.Purchase()
        //        {
        //            ArtistEarn = 400,
        //            PasskolEarn = 150,
        //            Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //            Music = resMusics.FirstOrDefault(),
        //            Permision = context.Permissions.FirstOrDefault(),
        //            PurchaseDate = DateTime.Now,
        //            CustomerReference = "sdfgf-2f234-fg67",
        //            PurchaseCost = 550
        //        },
        //        new Purchase.Purchase()
        //        {
        //            Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //            Music = resMusics.FirstOrDefault(),
        //            Permision = context.Permissions.FirstOrDefault(),
        //            PurchaseDate = DateTime.Now,
        //            CustomerReference = "sdfgf-2f234-fg67",
        //        }
        //    };
            
        //    context.Purchase.AddRange(Purchases);
        //    context.SaveChanges();

        //    NCPurchase ncPurchae1 = new NCPurchase()
        //    {
        //        DateTime = DateTime.Now,
        //        Id = Purchases[0].PurchaseId,
        //        Purchase = Purchases[0]
        //    };

        //    NCPurchase ncPurchae2 = new NCPurchase()
        //    {
        //        DateTime = DateTime.Now,
        //        Id = Purchases[1].PurchaseId,
        //        Purchase = Purchases[1]
        //    };

        //    context.NCPurchases.Add(ncPurchae1);
        //    context.NCPurchases.Add(ncPurchae2);

        //    Purchase.Purchase phonePurchase = new Purchase.Purchase()
        //    {
        //        Customer = userMgr.Users.SingleOrDefault(u => u.UserType == UserType.Customer),
        //        Music = context.Musics.FirstOrDefault(),
        //        Permision = context.Permissions.FirstOrDefault(p => p.PermissionCost == null),
        //        PurchaseDate = DateTime.Now

        //    };

        //    Confirmation PhoneConfirm = new Confirmation()
        //    {
        //        ConfirmType = ConfirmType.PhonePurchase,
        //        DateUpdate = DateTime.Now,
        //        EntityId = phonePurchase.PurchaseId.ToString(),
        //        Name = phonePurchase.Music.HebrewName
        //    };

        //    context.Purchase.Add(phonePurchase);
        //    context.Confirmations.Add(PhoneConfirm);
        //    context.SaveChanges();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="context"></param>
        //private void CreatePermissionsData(PaskolDbContext context)
        //{
        //    var permCat = new PermissionsCategory()
        //    {
        //        IsActive = true,
        //        Name = "קולנוע",
        //        ViewOrder = 1,
        //    };
        //    permCat.PermissionProperties = PermissionsCategory.CreateCostLevelProperties(permCat.ID);

        //    context.PermissionsCategories.Add(permCat);
        //    context.SaveChanges();
        //    var permProps = new List<PermissionProperty>()
        //    {
        //        new PermissionProperty()
        //        {
        //            IsActive = true,
        //            Name="טריטוריה",
        //            ViewOrder=3,
        //            PermissionsCategoryID = permCat.ID,
        //            IsCostLevel = false
        //        },
        //        new PermissionProperty()
        //        {
        //            IsActive = true,
        //            Name="סוג שימוש",
        //            ViewOrder=4,
        //            PermissionsCategoryID = permCat.ID,
        //            IsCostLevel = false
        //        },
        //        new PermissionProperty()
        //        {
        //            IsActive = true,
        //            Name="גירסה",
        //            ViewOrder=5,
        //            PermissionsCategoryID = permCat.ID,
        //            IsCostLevel = false
        //        },
        //    };
        //    context.PermissionProperties.AddRange(permProps);
        //    context.SaveChanges();

        //    var permValues = new List<PermissionPropertyValue>()
        //    {
        //        new PermissionPropertyValue(permProps[0].ID, permCat.ID, "פסטיבלים בלבד",true,1),
        //        new PermissionPropertyValue(permProps[0].ID, permCat.ID, "סרט טלוויזיה בלבד",true,2),
        //        new PermissionPropertyValue(permProps[0].ID, permCat.ID, "הקרנה מסחרית (כולל הכל)",true,3),
        //        new PermissionPropertyValue(permProps[1].ID, permCat.ID, "שיר נושא / כותרות פתיחה",true,1),
        //        new PermissionPropertyValue(permProps[1].ID, permCat.ID, "כותרות סיום",true,2),
        //        new PermissionPropertyValue(permProps[1].ID, permCat.ID, "תמה מוזיקלית",true,3),
        //        new PermissionPropertyValue(permProps[1].ID, permCat.ID, "שילוב חד פעמי",true,4),
        //        new PermissionPropertyValue(permProps[1].ID, permCat.ID, "שימוש אחר",true,5),
        //        new PermissionPropertyValue(permProps[2].ID, permCat.ID, "גרסת כיסוי",true,1),
        //        new PermissionPropertyValue(permProps[2].ID, permCat.ID, "הקלטה מקורית מהאתר",true,2),
        //    };
        //    context.PermissionPropertyValues.AddRange(permValues);
        //    context.SaveChanges();
        //    var perms = new List<Permission>
        //    {
        //        new Permission(permCat.ID, 2499)
        //        {
        //            PropertyValues = new List<PermissionPropertyValue>()
        //            {
        //                permCat.PermissionProperties.First().PropertyValues.First(),
        //                permCat.PermissionProperties.Skip(1).First().PropertyValues.First(),
        //                permValues[2],
        //                permValues[5],
        //                permValues[8]
        //            }
        //        },
        //        new Permission(permCat.ID,null)
        //        {
        //            PropertyValues = new List<PermissionPropertyValue>()
        //            {
        //                permCat.PermissionProperties.First().PropertyValues.First(),
        //                permCat.PermissionProperties.Skip(1).First().PropertyValues.First(),
        //                permValues[0],
        //                permValues[6],
        //                permValues[9]
        //            }
        //        }
        //    };
        //    context.Permissions.AddRange(perms);
        //    context.SaveChanges();
        //}
    }

    public enum ModelGetNavigtorOptions
    {
        None,
        GetNavigator,
        GetNavigatorRecursive
    }
}
