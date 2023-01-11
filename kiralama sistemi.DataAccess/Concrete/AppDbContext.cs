using kiralama_sistemi.Entities.Abstract;
using kiralamaSistemi.DataAccess.Concrete.Config;
using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.Entities.Enums;
using kiralamaSistemi.Entities.Extensions;
using kiralamaSistemi.Entities.Tables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage;

namespace kiralamaSistemi.DataAccess.Concrete
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>, IDisposable
    {

        #region DbSets

          private List<AuditLog>? AuditLogs { get; set; }
        private IDbContextTransaction? Transaction { get; set; }
        public virtual DbSet<AppRole> Roles { get; set; }
        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<AppUserRole> AppUserRoleler { get; set; }
        public virtual DbSet<Araba> Arabalar { get; set; }
        public virtual DbSet<Kiralama> Kiralamalar { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<LogModuleMap> LogModuleMaplar { get; set; }
        public virtual DbSet<Musteri> Musteriler { get; set; }
        public virtual DbSet<Tarife> Tarifeler { get; set; }

        #endregion




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();

            var path = Path.Combine(Directory.GetCurrentDirectory());
            var ss = System.IO.File.Exists(path + "\\appsettings.json");
            if (!ss)
            {
                //path = Path.Combine(Directory.GetCurrentDirectory(), @"..\BilimSenligi.API");
                path = Path.Combine(Directory.GetCurrentDirectory(), @"..\kiralama sistemi.API");
            }
            var _configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AppRoleConfig());
            modelBuilder.ApplyConfiguration(new AppRoleUserConfig());
            modelBuilder.ApplyConfiguration(new AppUserConfig());
            modelBuilder.ApplyConfiguration(new ArabaConfig());
            modelBuilder.ApplyConfiguration(new KiralamaConfig());
            modelBuilder.ApplyConfiguration(new LogConfig());
            modelBuilder.ApplyConfiguration(new LoginConfig());
            modelBuilder.ApplyConfiguration(new LogModelMapConfig());
            modelBuilder.ApplyConfiguration(new MusteriConfig());
            modelBuilder.ApplyConfiguration(new TarifeConfig());

        }


        public async Task<int> SaveChangesAsync(bool isFinished = true)
        {
            try
            {
                Transaction ??= await Database.BeginTransactionAsync();
                AuditLogs ??= new List<AuditLog>();
                OnBeforeSaveChanges();
                var result = await base.SaveChangesAsync(true, CancellationToken.None);
                if (isFinished)
                {
                    await OnAfterSaveChanges();
                    await Transaction.CommitAsync();
                    Transaction = null;
                    AuditLogs = null;
                }
                return result;
            }
            catch (Exception)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                await Transaction?.RollbackAsync();
                throw;
            }
        }

        private void OnBeforeSaveChanges()
        {
            this.ChangeTracker.DetectChanges();

            // Ekleme varsa
            var added = this.ChangeTracker.Entries().Where(t => t.State == EntityState.Added).ToList();
            if (added.Count > 0)
            {
                foreach (var change in added)
                {
                    var newValues = new Dictionary<string, object?>();
                    var entity = change.Entity;
                    var entityType = entity.GetType();
                    foreach (IProperty prop in change.OriginalValues.Properties.Where(i => !i.IsPrimaryKey() && !i.IsForeignKey()))
                    {
                        if (entityType.IsIgnored(prop.Name))
                        {
                            continue;
                        }
                        var currentValue = change.CurrentValues[prop.Name];
                        var propType = currentValue?.GetType();
                        bool defaultValCon = false;

                        if ((propType?.IsValueType ?? false) && propType.BaseType != typeof(Enum) && propType != typeof(bool))
                        {
                            defaultValCon = Activator.CreateInstance(propType)?.Equals(currentValue) ?? false;
                        }

                        if (currentValue == null || defaultValCon)
                        {
                            continue;
                        }
                        var name = entityType.GetPropLogName(prop.Name);

                        if (propType?.IsValueType ?? false)
                        {
                            if (propType.BaseType == typeof(Enum))
                            {
                                var value = ((Enum)currentValue).GetEnumTitle();
                                newValues.Add(name, value);
                            }
                            else if (propType == typeof(bool))
                            {
                                var value = ((bool)currentValue) ? "Evet" : "Hayır";
                                newValues.Add(name, value);

                            }
                            else if (propType == typeof(DateTime))
                            {
                                var value = ((DateTime?)currentValue)?.ToString("dd.MM.yyyy HH:mm:ss");
                                newValues.Add(name, value);
                            }
                            else
                            {
                                newValues.Add(name, currentValue);
                            }
                        }
                        else if (currentValue is string test)
                        {
                            if (!string.IsNullOrWhiteSpace(test))
                            {
                                newValues.Add(name, currentValue);
                            }
                        }
                        else
                        {
                            newValues.Add(name, currentValue);
                        }
                    }
                    AuditLogs.Add(new AuditLog { LogEvent = EnumLogEvent.Create, NewValues = newValues, NewObjLog = change.Entity });
                }
            }

            // Düzenleme varsa
            var modified = ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).ToList();

            if (modified.Count > 0)
            {
                foreach (var change in modified)
                {
                    var oldValues = new Dictionary<string, object?>();
                    var newValues = new Dictionary<string, object?>();

                    var oldEntity = change.GetDatabaseValues()?.ToObject();
                    var newEntity = change.Entity;
                    var entityType = newEntity.GetType();
                    if (oldEntity == null)
                    {
                        throw new Exception("OnBeforeSaveChanges-Appdbcontext-001");
                    }

                    foreach (IProperty prop in change.OriginalValues.Properties)
                    {
                        if (entityType.IsIgnored(prop.Name) || !change.Property(prop.Name).IsModified)
                        {
                            continue;
                        }

                        var currentValue = change.CurrentValues[prop.Name];
                        var propType = currentValue?.GetType();
                        var originalValue = entityType.GetProperty(prop.Name)?.GetValue(oldEntity);

                        if (!originalValue?.Equals(currentValue) ?? currentValue != null) //Sadece Değişen kayıt Log'a atılır.
                        {
                            var logName = entityType.GetPropLogName(prop.Name);
                            if (propType?.IsValueType ?? false)
                            {
                                if (propType.BaseType == typeof(Enum))
                                {
                                    var newVa = ((Enum?)currentValue)?.GetEnumTitle();
                                    var oldVa = ((Enum?)originalValue)?.GetEnumTitle();
                                    oldValues.Add(logName, oldVa);
                                    newValues.Add(logName, newVa);
                                }
                                else if (propType == typeof(bool))
                                {
                                    var newVa = ((bool?)currentValue) ?? false ? "Evet" : "Hayır";
                                    var oldVa = ((bool?)originalValue) ?? false ? "Evet" : "Hayır";
                                    oldValues.Add(logName, oldVa);
                                    newValues.Add(logName, newVa);
                                }
                                else if (propType == typeof(DateTime))
                                {
                                    var newVa = ((DateTime?)currentValue)?.ToString("dd.MM.yyyy HH:mm:ss");
                                    var oldVa = ((DateTime?)originalValue)?.ToString("dd.MM.yyyy HH:mm:ss");
                                    oldValues.Add(logName, oldVa);
                                    newValues.Add(logName, newVa);
                                }
                                else
                                {
                                    oldValues.Add(logName, originalValue);
                                    newValues.Add(logName, currentValue);
                                }
                            }
                            else
                            {
                                oldValues.Add(logName, originalValue);
                                newValues.Add(logName, currentValue);
                            }
                        }
                    }
                    AuditLogs.Add(new AuditLog
                    {
                        LogEvent = EnumLogEvent.Update,
                        OldValues = oldValues,
                        NewValues = newValues,
                        NewObjLog = newEntity,
                        OldObjLog = oldEntity
                    });
                }
            }

            // Silme varsa
            var deleted = ChangeTracker.Entries().Where(t => t.State is EntityState.Deleted).ToList();
            if (deleted.Count > 0)
            {
                foreach (var change in deleted)
                {
                    var oldValues = new Dictionary<string, object?>();
                    var entity = change.Entity;
                    var entityType = entity.GetType();
                    foreach (IProperty prop in change.OriginalValues.Properties)
                    {
                        if (entityType.IsIgnored(prop.Name))
                        {
                            continue;
                        }
                        var propType = change.Property(prop.Name).CurrentValue?.GetType();
                        bool defaultValCon = false;

                        if ((propType?.IsValueType ?? false) && propType.BaseType != typeof(Enum) && propType != typeof(bool))
                        {
                            defaultValCon = Activator.CreateInstance(propType)?.Equals(change.CurrentValues[prop.Name]) ?? false;
                        }
                        var originalValue = entityType.GetProperty(prop.Name)?.GetValue(entity);
                        if (originalValue != null && !defaultValCon)
                        {
                            var name = entityType.GetPropLogName(prop.Name);

                            if ((propType?.IsValueType ?? false) && propType.BaseType == typeof(Enum))
                            {
                                var value = ((Enum)originalValue)?.GetEnumTitle();
                                oldValues.Add(name, value);
                            }
                            else if ((propType?.IsValueType ?? false) && propType == typeof(bool))
                            {
                                var value = ((bool)originalValue) ? "Evet" : "Hayır";
                                oldValues.Add(name, value);

                            }
                            else if ((propType?.IsValueType ?? false) && propType == typeof(DateTime))
                            {
                                var value = ((DateTime?)originalValue)?.ToString("dd.MM.yyyy HH:mm:ss");
                                oldValues.Add(name, value);
                            }
                            else
                            {
                                oldValues.Add(name, originalValue);
                            }
                        }
                    }

                    AuditLogs.Add(new AuditLog { LogEvent = EnumLogEvent.Delete, OldObjLog = entity, OldValues = oldValues });
                }

            }
        }

        private async Task OnAfterSaveChanges()
        {
            if (AuditLogs?.Count > 0)
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    Culture = CultureInfo.CreateSpecificCulture("tr-Tr"),
                    //Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.All),
                };
                var logs = new List<Log>();
                foreach (var auditLog in AuditLogs)
                {
                    var logObject = auditLog.NewObjLog ?? auditLog.OldObjLog;
                    if (logObject.GetProperty(typeof(LogInfo), "LogInfo") is not LogInfo logInfo)
                    {
                        continue;
                    }
                    var log = new Log
                    {
                        UserId = logInfo.UserId,
                        LoginId = logInfo.LoginId,
                        UserName = logInfo.UserName,
                        Date = DateTime.Now,
                        Event = auditLog.LogEvent,
                    };

                    //if (logObject is Content content)
                    //{
                    //    log.Module = EnumModules.Content;
                    //    log.DataId = content.Id;

                    //    if (auditLog.LogEvent == EnumLogEvent.Create)
                    //    {
                    //        log.Title = $"<b>{content.Title} </b> Başlıklı İçerik Eklendi";
                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Update)
                    //    {
                    //        var oldContent = auditLog.OldObjLog as Content ?? throw new NullReferenceException($"{nameof(Content)} null olmamalı");
                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);

                    //        if (content.Title != oldContent.Title)
                    //        {
                    //            log.Title = $"<b><del>{oldContent.Title}</del></b> Başlıklı İçerik <b>{content.Title}</b> Olarak Düzenlendi.";
                    //        }
                    //        else
                    //        {
                    //            log.Title = $"<b>{content.Title}</b> Başlıklı İçerik Düzenlendi.";
                    //        }
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Delete)
                    //    {
                    //        log.Title = $"<b>{content.Title}</b> Başlıklı İçerik Silindi.";

                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);
                    //    }
                    //}
                    //else if (logObject is Category category)
                    //{
                    //    log.Module = EnumModules.Category;
                    //    log.DataId = category.Id;

                    //    if (auditLog.LogEvent == EnumLogEvent.Create)
                    //    {
                    //        log.Title = $"<b>{category.Name} </b> Adlı kategori Eklendi.";
                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Update)
                    //    {
                    //        var oldCategory = auditLog.OldObjLog as Category ?? throw new NullReferenceException($"{nameof(Category)} null olmamalı");
                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);

                    //        if (category.Name != oldCategory.Name)
                    //        {
                    //            log.Title = $"<b><del>{oldCategory.Name}</del></b> Adlı kategori <b>{category.Name}</b> Olarak Düzenlendi.";
                    //        }
                    //        else
                    //        {
                    //            log.Title = $"<b>{category.Name}</b> Adlı kategori Düzenlendi.";
                    //        }
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Delete)
                    //    {
                    //        log.Title = $"<b>{category.Name}</b> Adlı kategori Silindi.";

                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);
                    //    }
                    //}
                    //else if (logObject is Manset manset)
                    //{
                    //    log.Module = EnumModules.Manset;
                    //    log.DataId = manset.Id;
                    //    if (auditLog.LogEvent == EnumLogEvent.Create)
                    //    {
                    //        log.Title = $"<b>{manset.Title} </b> Başlıklı Manşet Eklendi.";
                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Update)
                    //    {
                    //        var oldManset = auditLog.OldObjLog as Manset ?? throw new NullReferenceException($"{nameof(Manset)} null olmamalı");
                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);
                    //        if (manset.Title != oldManset.Title)
                    //        {
                    //            log.Title = $"<b><del>{oldManset}</del></b> Başlıklı Manşet <b>{manset.Title}</b> Olarak Düzenlendi.";
                    //        }
                    //        else
                    //        {
                    //            log.Title = $"<b>{manset.Title} </b> Başlıklı Manşet Düzenlendi.";
                    //        }
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Delete)
                    //    {
                    //        log.Title = $"<b>{manset.Title} </b> Başlıklı Manşet Silindi.";

                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);
                    //    }
                    //}
                    //else if (logObject is AppUser user)
                    //{
                    //    log.Module = EnumModules.User;
                    //    log.DataId = user.Id;

                    //    if (auditLog.LogEvent == EnumLogEvent.Create)
                    //    {
                    //        log.Title = $"<b>{user.NameSurename} </b> Adlı Kullanıcı Eklendi.";
                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Update)
                    //    {
                    //        var oldUser = auditLog.OldObjLog as AppUser ?? throw new NullReferenceException($"{nameof(AppUser)} null olmamalı");

                    //        if (user.NameSurename != oldUser.NameSurename)
                    //        {
                    //            log.Title = $"<b><del>{oldUser.NameSurename}</del></b> Adlı Kullanıcı <b>{user.NameSurename}</b> Olarak Düzenlendi.";
                    //        }
                    //        else
                    //        {
                    //            log.Title = $"<b>{user.NameSurename} </b> Adlı Kullanıcı Düzenlendi.";
                    //        }

                    //        log.NewValue = JsonConvert.SerializeObject(auditLog.NewValues, settings);
                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);
                    //    }
                    //    else if (auditLog.LogEvent == EnumLogEvent.Delete)
                    //    {
                    //        log.Title = $"<b>{user.NameSurename} </b> Adlı Kullanıcı Silindi.";
                    //        log.OldValue = JsonConvert.SerializeObject(auditLog.OldValues, settings);
                    //    }
                    //}
                    //else
                    //{
                    //    continue;
                    //}

                    if (!string.IsNullOrEmpty(logInfo.CustomTitle))
                    {
                        log.Title = logInfo.CustomTitle;
                    }
                    logs.Add(log);
                }

                if (logs?.Count > 0)
                {
                    this.Logs.AddRange(logs);
                    await base.SaveChangesAsync(true, CancellationToken.None);
                }
            }
        }


      

    }
}
