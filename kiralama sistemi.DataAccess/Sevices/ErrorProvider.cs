using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.DataAccess.Sevices
{
    public static class ErrorProvider
    {

        /*
         * Content  Error Codes: Co-Er-000
         * Genel    Error Codes: Ge-Er-000
         * Auth     Error Codes: Au-Er-000
         * Account  Error Codes: Ac-Er-000
         * Template Error Codes: Te-Er-000
         * Card     Error Codes: Ca-Er-000
         * Blog     Error Codes: Bl-Er-000
         * */

        //FrontEnt (Özel) Hatalar kodu : (FE-Oz-000)

        /// <summary>
        /// GENERAL
        /// </summary>
        /// 
        public static Error NotValid = new() { Code = "Ge-Er-000", Description = "Lütfen zorunlu alanları doldurunuz", Key = EnumErrorTypes.danger };
        public static Error NotValidParams = new() { Code = "Ge-Er-001", Title = "Eksik veya hatalı parametre", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error NotFoundData = new() { Code = "Ge-Er-002", Title = "Aradığınız veri bulunamadı.", Description = "Lütfen bilgileri kontrol ederek tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error UnableErroeee = new() { Code = "Ge-Er-003", Title = "Bilinmeyen bir hata oluştu", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error Updated = new() { Code = "Ge-Er-004", Title = "Bilgiler güncellendi", Description = "Değişikliler başarılı bir şekilde kaydedildi", Key = EnumErrorTypes.success };
        public static Error NotUpdated = new() { Code = "Ge-Er-005", Title = "Bilgiler güncellenemedi", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error Deleted = new() { Code = "Ge-Er-006", Description = "Bilgiler silindi", Key = EnumErrorTypes.success };
        public static Error NotDeleted = new() { Code = "Ge-Er-007", Title = "Bilgiler silinenemedi", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error Added = new() { Code = "Ge-Er-008", Description = "Bilgiler eklendi", Key = EnumErrorTypes.success };
        public static Error NotAdded = new() { Code = "Ge-Er-009", Title = "Bilgiler eklenemedi", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error AccessDenied = new() { Code = "Ge-Er-010", Title = "İzin yok", Description = "Bu işlemi yapmaya yetkiniz yok", Key = EnumErrorTypes.danger };
        public static Error OperationCouldNotBePerformed = new() { Code = "Ge-Er-012", Title = "İşleminiz gerçekleştirilemedi.", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error InvalidFile = new() { Code = "Ge-Er-014", Title = "Dosya Geçersiztir", Description = "Geçerli bir dosya silin.", Key = EnumErrorTypes.danger };
        public static Error InvalidNote = new() { Code = "Ge-Er-015", Title = "Note Geçersiz", Description = "Not boş olmamamalı.", Key = EnumErrorTypes.danger };
        public static Error NotFoundPage = new() { Code = "404", Title = "Sayfa Bulunmadı", Description = "İstenilen sayfa bulunamadı.", Key = EnumErrorTypes.danger };
        public static Error SmsGonderimedi = new() { Code = "Sms-H-001", Title = "Sms Gönderilmedi", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error DataAccessHatasi = new() { Code = "Ex-H-001", Title = "Beklenmedik hata oluştu.", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        public static Error APIHatasi = new Error { Code = "Ex-H-002", Title = "Beklenmedik hata oluştu.", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
        /// <summary>
        /// LOGIN
        /// </summary>


        public static class System
        {
            public static Error KotuNiyyetliIstek = new() { Code = "Sy-H-001", Title = "Erişim Yasaklandı", Description = "Kötü niyyetli istek algılandı.", Key = EnumErrorTypes.danger };
            public static Error ErisimYasaklandi = new() { Code = "Sy-H-001", Title = "Erişim Yasaklandı", Description = "20'den fazla kötü niyyetli istek tespit edildi.", Key = EnumErrorTypes.danger };
        }

        public static class FrontEnd
        {
            public static readonly Error IncorrectParams = new("FE-Er-001", "Beklenmedik Hata Oluştu (Hatalı Parametre)", "Hata Tekrar Olunca İletişime Geçiniz.", EnumErrorTypes.danger);
            public static readonly Error InvalidModel = new("FE-Er-002", "Beklenmedik Hata Oluştu (Hatalı Model)", "Hata Tekrar Olunca İletişime Geçiniz.", EnumErrorTypes.danger);
        }

        public static class Auth
        {
            //General 
            public static readonly Error RequiredUsername = new("Au-Er-000", null, "Lütfen geçerli bir kullanıcı adı giriniz.", EnumErrorTypes.danger);
            public static readonly Error InvalidUserName = new("Au-Er-001", "Geçersiz kullanıcı adı", "Lütfen geçerli bir kullanıcı adı giriniz.", EnumErrorTypes.danger);
            public static readonly Error UserNameMaxSize = new("Au-Er-002", null, "Kullanıcı adı max {0} karakter olmalıdır.", EnumErrorTypes.danger);
            public static readonly Error UserNameDuplicate = new("Au-Er-003", "Bu kullanıcı adı daha önce alınmış", "Lütfen başka bir kullanıcı adı giriniz.", EnumErrorTypes.danger);
            public static readonly Error RequiredPass = new("Au-Er-004", null, "Parola alanı zorunludur.", EnumErrorTypes.danger);
            public static readonly Error InvalidPassword = new("Au-Er-005", "Geçersiz parola", "Lütfen geçerli bir parola giriniz.", EnumErrorTypes.danger);
            public static readonly Error InvalidCurrentPass = new("Au-Er-006", null, "Mevcut parola hatalı", EnumErrorTypes.danger);
            public static readonly Error InvalidPassMaxSize = new("Au-Er-007", null, "Parola max {0} karakter olmalıdır.", EnumErrorTypes.danger);
            public static readonly Error InvalidPassMinSize = new("Au-Er-008", null, "Parola min {0} karakter olmalıdır.", EnumErrorTypes.danger);
            public static readonly Error InvalidPassLowercase = new("Au-Er-009", null, "Parolanız en az bir küçük harf içermelidir.(a b c vb.)", EnumErrorTypes.danger);
            public static readonly Error InvalidPassDigit = new("Au-Er-010", null, "Parolanız en az bir rakam içermelidir.(1 2 3 vb.)", EnumErrorTypes.danger);
            public static readonly Error InvalidPassSymbol = new("Au-Er-011", null, "Parolanız en az bir simge içermelidir.( '-' '_' '.' vb.)", EnumErrorTypes.danger);
            public static readonly Error NotMatchPasses = new("Au-Er-012", "Hata", "Parolalar eşleşmiyor", EnumErrorTypes.danger);
            public static readonly Error NotChangedPass = new("Au-Er-013", "Hata", "Parola değiştirilemedi. Lütfen daha sonra tekrar deneyiniz.", EnumErrorTypes.danger);
            public static readonly Error SucceedPassChange = new("Au-Er-014", "Parolanız başarıyla yenilendi.", " Yeni parolanız ile sisteme giriş yapabilirisiniz.", EnumErrorTypes.success);
            public static readonly Error SucceedForgotPass = new("Au-Er-015", "Parola yenileme isteği aldık", "Girilen mail adresi geçerli ise e-mail üzerinden parolanızı yenileyebilirsiniz.", EnumErrorTypes.info);
            public static readonly Error RequiredEmail = new("Au-Er-016", null, "Lütfen e-posta adresinizi giriniz.", EnumErrorTypes.danger);
            public static readonly Error InvalidEmail = new("Au-Er-017", null, "Lütfen geçerli bir E-mail Adresi giriniz.", EnumErrorTypes.danger);
            public static readonly Error InvalidEmailMaxSize = new("Au-Er-018", null, "E-mail adresi max {0} karakter olmalıdır.", EnumErrorTypes.danger);
            public static readonly Error NotConfirmedEmail = new("Au-Er-019", "E-posta adresi doğrulanmamış", "Lütfen e-posta adresinizi doğrulamak için e-postanızı kontrol edin.", EnumErrorTypes.warning);
            public static readonly Error SucceedConfirmEmail = new("Au-Er-020", "E-posta doğrulandı", "E-posta adresiniz doğrulandı. Sisteme giriş yapara kaydınızı tamamlayabilirsiniz.", EnumErrorTypes.success);
            public static readonly Error InvalidLogin = new("Au-Er-021", "Giriş yapılamadı", "Kullanıcı adı veya parola hatalı", EnumErrorTypes.danger);
            public static readonly Error InvalidCaptcha = new("Au-Er-022", null, "Lütfen geçerli bir güvenlik kodu giriniz.", EnumErrorTypes.danger);
            public static readonly Error NotFoundUser = new("Au-Er-023", "Kullanıcı hesabı bulunamadı", "Bu kullanıcı sistemde kayıtlı değil veya silinmiş", EnumErrorTypes.warning);
            public static readonly Error InvalidToken = new("Au-Er-024", "Geçersiz token", "Geçersiz veya süresi dolmuş token bilgisi yeni email göderildi.", EnumErrorTypes.danger);
            public static readonly Error IdentityLockedout = new("Au-Er-025", "Hesabınız kilitlendi", "Art arda {0} başarısız giriş denemesi yaptığınızdan dolayı hesabınız {1} kilitlenmiştir. Hesabınızı tekrar aktif etmek için parolamı unuttum kısmını kullanarak tekrar aktifleştirebilirsiniz. ", EnumErrorTypes.danger);
            public static readonly Error FarkliLogin = new("Au-Er-026", "Girişiniz sonlandırıldı", "Farklı konumdan hesabınız ile giriş yapıldı.", EnumErrorTypes.warning);
            public static readonly Error NotAuthenticated = new("Au-Er-027", "Aktif Kullanici Yok", "Giriş yaparak tekrar deneyiniz.", EnumErrorTypes.warning);
            public static readonly Error Logouted = new("Au-Er-028", "Çıkış Yapıldı", "Güvenli bir şekilde çıkış yapıldı.", EnumErrorTypes.success);
            public static readonly Error Banned = new("Au-Er-029", "Giriş Yasaklandı", "Lütfen {0} sonra tekrar deneyiniz.", EnumErrorTypes.danger);
            public static readonly Error MaliciousLogin = new("Au-Er-030", "Kötü Niyetli Giriş Algılandı", "Sisteme girişiniz geçici bir süreyle kısıtlandı. <br> Lütfen {0} sonra deneyiniz.", EnumErrorTypes.danger);

            //Register

        }

        public static class Account
        {
            public static readonly Error FailedCreateUserExistAlready = new() { Code = "Ac-Er-000", Title = "Hata", Description = "Bu kullanıcı adı ile  kayıtlı bir kullanıcı zaten var", Key = EnumErrorTypes.danger };
            public static readonly Error RequiredFirstLastName = new() { Code = "Ac-Er-001", Description = "Adı Soyadı alanı zorunludur.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidFirstLastNameMinSize = new() { Code = "Ac-Er-002", Description = "Adı Soyadı alanı min {0} karakter olmalıdır.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidFirstLastNameMaxSize = new() { Code = "Ac-Er-003", Description = "Adı Soyadı alanı max {0} karakter olmalıdır.", Key = EnumErrorTypes.danger };
            public static readonly Error RequiredUnvan = new() { Code = "Ac-Er-004", Description = "Firma Ünvanı zorunludur.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidUnvanMinSize = new() { Code = "Ac-Er-005", Description = "Firma Ünvanı min {0} karakter olmalıdır.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidUnvanMaxSize = new() { Code = "Ac-Er-006", Description = "Firma Ünvanı max {0} karakter olmalıdır.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidTelefon = new() { Code = "Ac-Er-007", Description = "Telefon numarası max {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidAdres = new() { Code = "Ac-Er-008", Description = "Adres  max {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidFaks = new() { Code = "Ac-Er-009", Description = "Faks numarası max {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidGSM = new() { Code = "Ac-Er-010", Description = "GSM numarası max {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidRegisterNumber = new() { Code = "Ac-Er-011", Description = "Kayıt Numarası max {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidVergiDairesi = new() { Code = "Ac-Er-012", Description = "Vergi Dairesi max {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidTamUnvanMinSize = new() { Code = "Ac-Er-013", Description = "Tam Ünvan min {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidTamUnvanMaxSize = new() { Code = "Ac-Er-014", Description = "Tam Ünvan max {0} karakter olmalı.", Key = EnumErrorTypes.danger };
            public static readonly Error FailedCompanyRegister = new() { Code = "Ac-Er-015", Title = "Hesap bilgileriniz oluşturulamadı.", Description = "Lütfen daha sonra tekrar deneyiniz.", Key = EnumErrorTypes.danger };
            public static readonly Error FailedCreateCompanyRequireUser = new() { Code = "Ac-Er-016", Title = "Hata", Description = "Firma kaydı için en az bir kullanıcı gerekli", Key = EnumErrorTypes.danger };
            public static readonly Error SucceedCompanyRegister = new() { Code = "Ac-Er-017", Title = "Hesap bilgileriniz oluşturuldu.", Description = "Aktivasyon e-postası gönderildi.", Key = EnumErrorTypes.success };
            public static readonly Error AccountIsNotActive = new() { Code = "Ac-Er-018", Title = "Hesap pasif", Description = "Hesabınız yönetici  tarafınden pasif edilmiştir.", Key = EnumErrorTypes.warning };
            public static readonly Error PersonelMevcut = new() { Code = "Ac-Er-019", Title = "Personel Mevcut", Description = "Personel başka bir kullanıcıya atanmış. Lütfen başka bir personel seçiniz.", Key = EnumErrorTypes.danger };
            public static readonly Error RequiredPersonel = new() { Code = "Ac-Er-020", Title = "Personel Zorunludur.", Description = "Lütfen geçerli bir personel seçiniz.", Key = EnumErrorTypes.danger };
            public static readonly Error RequiredRole = new() { Code = "Ac-Er-021", Title = "Role Zorunludur.", Description = "Lütfen geçerli bir role seçiniz.", Key = EnumErrorTypes.danger };
            public static readonly Error UserIsNotNew = new() { Code = "Ac-Er-022", Title = "Kullanıcı Yeni Değil", Description = "Kayıt olma işlem daha önce tamamlanmıştır.", Key = EnumErrorTypes.danger };
            public static readonly Error AccountIsNew = new() { Code = "Ac-Er-023", Title = "Kayıt İşlemi Tamamlanmamış", Description = "Lütfen kaydınız tamamlayınız.", Key = EnumErrorTypes.warning };
            public static readonly Error RegisterComplated = new() { Code = "Ac-Er-024", Title = "Kayıt İşlemi Tamamlandı", Description = "Giriş yapabiliriniz.", Key = EnumErrorTypes.success };
            public static readonly Error FailedCreateUser = new() { Code = "Ac-Er-025", Title = "Kullanıcı Eklenmedi", Description = "Bilgileri kontrol ederek daha sonra tekrar deneyiniz", Key = EnumErrorTypes.danger };

        }

        //public static class Musteri
        //{
        //    public static readonly Error NotFound = new("Mu-Er-001", "Müşteri Bulunmadı", "Aradığınız mşteri silinmiş olabilir. Kontrol ederek tekrar deneyiniz.", EnumErrorTypes.danger);
        //    public static readonly Error validation = new("Bl-Er-001", "Tekrarlanan Başlık Var", "Aynı başlıkta blogunuz mecvuttur. Farklı başlık yazarak tekrear deneyiniz.", EnumErrorTypes.warning);
        //}


        public static class File
        {
            public static readonly Error InvalidDosya = new() { Code = "Ds-H-001", Title = "Dosya Geçersiztir", Description = "Lütfen geçerli bir Dosya yükleyiniz.", Key = EnumErrorTypes.danger };
            public static readonly Error InvalidPhoto = new() { Code = "Ds-H-002", Title = "Fotograf Geçersiztir", Description = "Lütfen geçerli bir fotograf yükleyiniz.", Key = EnumErrorTypes.danger };
            public static readonly Error DosyaBulunmadi = new() { Code = "Ds-H-003", Title = "Dosya Bulunmadı", Description = "Lütfen daha sonra tekrar deneyiniz..", Key = EnumErrorTypes.danger };
            public static readonly Error DosyaYuklenmedi = new() { Code = "Ds-H-004", Title = "Dosya Yüklenmedi", Description = "Lütfen daha sonra tekrar deneyiniz..", Key = EnumErrorTypes.danger };

        }
    }
}
