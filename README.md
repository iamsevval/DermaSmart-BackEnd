# DermaSmart Backend API Dokümantasyonu

Bu doküman, DermaSmart projesi için geliştirilen backend API uç noktalarını (endpoints) açıklamaktadır. Mobil (Flutter) tarafında entegrasyonu kolaylaştırmak için her endpoint için giden ve dönen JSON veri yapıları (şemaları) aşağıda detaylandırılmıştır.

## 1. Authentication (Kimlik Doğrulama) API

### 1.1. Kullanıcı Kaydı (Register)
**Endpoint:** `POST /api/auth/register`
**Açıklama:** Yeni bir kullanıcı hesabı oluşturur. Şifreler BCrypt ile hashlenerek veritabanına kaydedilir.

**İstek (Request Body):**
```json
{
  "email": "kullanici@example.com",
  "password": "Mypassword123!"
}
```

**Başarılı Yanıt (Response - 200 OK):**
```json
{
  "message": "Kayıt başarılı.",
  "userId": 1
}
```

**Hata Yanıtı (Response - 400 Bad Request):**
```json
{
  "errorCode": "EMAIL_ALREADY_EXISTS",
  "message": "Bu email zaten kayıtlı."
}
```

---

### 1.2. Kullanıcı Girişi (Login)
**Endpoint:** `POST /api/auth/login`
**Açıklama:** Kullanıcı bilgilerini doğrular ve JWT (JSON Web Token) döndürür.

**İstek (Request Body):**
```json
{
  "email": "kullanici@example.com",
  "password": "Mypassword123!"
}
```

**Başarılı Yanıt (Response - 200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5c...",
  "userId": 1
}
```

**Hata Yanıtı (Response - 401 Unauthorized):**
```json
{
  "errorCode": "INVALID_CREDENTIALS",
  "message": "Email veya şifre hatalı."
}
```

---

## 2. Cilt Profili (Skin Profile) API

> **Not:** Cilt profili API uç noktaları için Authorization (Bearer Token) gereklidir. İsteğin Header kısmında `Authorization: Bearer <Token>` şeklinde JWT gönderilmelidir.

### 2.1. Cilt Profili Oluşturma
**Endpoint:** `POST /api/skinprofile`
**Açıklama:** Kullanıcının formdan (onboarding) doldurduğu cilt tipi, endişeleri ve yaş aralığı gibi bilgileri veritabanına kaydeder. Kullanıcı ID'si Token içerisinden otomatik alınır.

**İstek (Request Body):**
```json
{
  "skinType": "Karma",
  "concerns": "Sivilce, Leke",
  "ageRange": "18-24"
}
```

**Başarılı Yanıt (Response - 200 OK):**
```json
{
  "message": "Cilt profili oluşturuldu.",
  "profileId": 1
}
```

**Hata Yanıtları:**
- **401 Unauthorized (Token yok/geçersiz):**
```json
{
  "errorCode": "INVALID_TOKEN",
  "message": "Token geçersiz."
}
```

- **400 Bad Request (Profil zaten var):**
```json
{
  "errorCode": "PROFILE_ALREADY_EXISTS",
  "message": "Bu kullanıcı için zaten bir profil mevcut."
}
```

---

### 2.2. Cilt Profili Getirme
**Endpoint:** `GET /api/skinprofile/{userId}`
**Açıklama:** Belirtilen `userId`'ye ait cilt profili verilerini getirir. Mobil uygulamada kullanıcının anket sonucunu ekrana yansıtmak için kullanılır.

**İstek:** (Body gerekmez, sadece URL üzerinden userId parametresi gönderilir)

**Başarılı Yanıt (Response - 200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "skinType": "Karma",
  "concerns": "Sivilce, Leke",
  "ageRange": "18-24"
}
```

**Hata Yanıtı (Response - 404 Not Found):**
```json
{
  "errorCode": "PROFILE_NOT_FOUND",
  "message": "Profil bulunamadı."
}
```

---

## Geliştirici Notları ve İyileştirmeler (Haftalık Scrum Özeti)
* **JWT Entegrasyonu:** Backend tarafında güvenli oturum yönetimi için JWT altyapısı kuruldu.
* **Hata Kodları (Error Codes):** Mobil ekip (Ayşe / Geliştirici 4) ile uyumlu çalışmak adına, hata döndürülen tüm uç noktalara `errorCode` anahtarı eklenerek JSON şemaları standart hale getirildi. Mobil taraf artık string okumak yerine bu hata kodlarını (`EMAIL_ALREADY_EXISTS`, `INVALID_CREDENTIALS` vb.) baz alarak UI kontrolleri yapabilir.
* **Form Entegrasyonu:** Cilt tipi kayıtları veritabanında ilişkisel olarak tutulup `/api/skinprofile` üzerinden servis edilmektedir.
