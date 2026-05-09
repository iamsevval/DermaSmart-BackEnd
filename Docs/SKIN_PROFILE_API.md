# Cilt Profili (Skin Profile) API Dokümantasyonu

Bu doküman, kullanıcının cilt profili bilgilerini kaydetmek ve getirmek için kullanılan API uç noktalarını (endpoints) içerir.

> **ÖNEMLİ GÜNCELLEME:** Merge işlemi sonrasında oluşan tablo karmaşasını gidermek adına `AppUser` ve `AppSkinProfile` modelleri silinerek standartlaştırılmış `User` ve `SkinProfile` tablolarıyla birleştirilmiştir. Veritabanındaki ilişkiler bu yeni yapıya göre çalışmaktadır.

> **Not:** Cilt profili API uç noktaları için Authorization (Bearer Token) gereklidir. İsteğin Header kısmında `Authorization: Bearer <Token>` şeklinde JWT gönderilmelidir.

---

### 1. Cilt Profili Oluşturma
**Endpoint:** `POST /api/skinprofile`  
**Açıklama:** Kullanıcının onboarding sürecinde (formdan) doldurduğu cilt tipi, endişeleri ve yaş aralığı gibi bilgileri `SkinProfiles` tablosuna kaydeder. Kullanıcı ID'si Token içerisinden Claim olarak otomatik alınır.

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

### 2. Cilt Profili Getirme
**Endpoint:** `GET /api/skinprofile/{userId}`  
**Açıklama:** Belirtilen `{userId}`'ye ait cilt profili verilerini getirir.

**İstek:** (Sadece URL üzerinden userId parametresi ile GET isteği atılır, Body kullanılmaz)
*Örnek URL:* `https://localhost:5001/api/skinprofile/1`

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
