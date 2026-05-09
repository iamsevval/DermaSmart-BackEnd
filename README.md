# DermaSmart Backend API

DermaSmart projesi için geliştirilen backend API uygulaması. 

## Dokümantasyon

API uç noktalarının (endpoints) detayları, giden ve dönen JSON veri yapıları (şemaları) takım çalışmasını kolaylaştırmak adına **`Docs`** klasöründe modüler olarak ayrıştırılmıştır:

* 🔐 **[Kimlik Doğrulama (Auth) API Dokümantasyonu](./Docs/AUTH_API_DOCUMENTATION.md)**
* 👤 **[Cilt Profili (Skin Profile) API Dokümantasyonu](./Docs/SKIN_PROFILE_API.md)**

---

## Geliştirici Notları ve İyileştirmeler (Haftalık Scrum Özeti)
* **Veritabanı ve Modellerin Birleştirilmesi (Güncel Değişiklik):** Merge işlemi sonrasında oluşan tablo karmaşasını gidermek adına, `AppUser` ve `AppSkinProfile` tabloları sistemden tamamen kaldırıldı. Proje baştan aşağıya standart `User` ve `SkinProfile` modellerine bağlandı. Bu sayede 500 hataları ve veritabanı uyuşmazlıkları giderildi.
* **JWT Entegrasyonu:** Backend tarafında güvenli oturum yönetimi için JWT altyapısı kuruldu.
* **Hata Kodları (Error Codes):** Mobil ekip (Ayşe / Geliştirici 4) ile uyumlu çalışmak adına, hata döndürülen tüm uç noktalara `errorCode` anahtarı eklenerek JSON şemaları standart hale getirildi. Mobil taraf artık string okumak yerine bu hata kodlarını (`EMAIL_ALREADY_EXISTS`, `INVALID_CREDENTIALS` vb.) baz alarak UI kontrolleri yapabilir.
* **Form Entegrasyonu:** Cilt tipi kayıtları, güncellenen veritabanında ilişkisel olarak tutulup `/api/skinprofile` üzerinden başarılı ve eksiksiz bir şekilde servis edilmektedir.
