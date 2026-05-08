# DermaSmart — Auth API Teknik Dokümantasyonu

**Hazırlayan:** Zeynep (Geliştirici)  
**Tarih:** 08.05.2026  
**Branch:** `feature/auth_zeynep`  
**İlgili Test Raporu:** Ayşe'nin Postman Negative Test Raporu (`test/auth-postman-ayse`)

---

## 1. Yapılan Değişiklikler

Bu döküman, Ayşe tarafından hazırlanan Postman negatif test raporunda tespit edilen backend hatalarının giderilmesi ve Auth API'nin mobil istemci için iyileştirilmesi kapsamında yapılan değişiklikleri açıklamaktadır.

### 1.1 Tespit Edilen Hatalar

| # | Test Adı | Beklenen | Gerçekleşen | Durum |
|---|----------|----------|-------------|-------|
| 6 | Register – Invalid Email Format | 400 Bad Request | 200 OK | ❌ Hata |
| 7 | Register – Empty Password | 400 Bad Request | 200 OK | ❌ Hata |

### 1.2 Düzeltilen Dosyalar

| Dosya | Değişiklik |
|-------|------------|
| `DTOs/RegisterDto.cs` | Email ve password validasyon attribute'ları eklendi |
| `Controllers/AuthController.cs` | Register metoduna `ModelState.IsValid` kontrolü eklendi |
| `Program.cs` | `InvalidModelStateResponseFactory` ile standart hata response formatı tanımlandı |

---

## 2. Endpoint Referansı

### POST `/api/auth/register`

Yeni kullanıcı kaydı oluşturur.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "MyPass123"
}
```

**Validasyon Kuralları:**
- `email`: Zorunlu, geçerli email formatında olmalı (RFC 5322)
- `password`: Zorunlu, en az 6 karakter

**Başarılı Response (200):**
```json
{
  "success": true,
  "message": "Kayıt başarılı.",
  "userId": 1
}
```

**Hata Response (400):**
```json
{
  "success": false,
  "errorCode": "INVALID_EMAIL_FORMAT",
  "message": "Geçerli bir email adresi giriniz.",
  "statusCode": 400
}
```

---

### POST `/api/auth/login`

Kayıtlı kullanıcı girişi yapar, JWT token döner.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "MyPass123"
}
```

**Başarılı Response (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1
}
```

**Hata Response (401):**
```json
{
  "message": "Email veya şifre hatalı."
}
```

---

## 3. Hata Kodları (errorCode)

Mobil istemci, `errorCode` alanına göre UI davranışını belirleyebilir.

| errorCode | HTTP Status | Açıklama | Mobil Davranış Önerisi |
|-----------|-------------|----------|------------------------|
| `INVALID_EMAIL_FORMAT` | 400 | Email formatı geçersiz | Email inputuna kırmızı border + hata mesajı göster |
| `INVALID_PASSWORD` | 400 | Şifre boş veya çok kısa | Şifre inputuna kırmızı border + hata mesajı göster |
| `VALIDATION_ERROR` | 400 | Genel validasyon hatası | Genel hata mesajı göster |
| `EMAIL_ALREADY_EXISTS` | 400 | Email zaten kayıtlı | "Bu email kullanımda" mesajı göster |

---

## 4. Negatif Test Sonuçları (Güncel)

Ayşe'nin test raporundaki tüm testler düzeltmeler sonrası yeniden çalıştırılmıştır.

| # | Test Adı | HTTP Status | Sonuç |
|---|----------|-------------|-------|
| 1 | Login – Wrong Password | 401 | ✅ Başarılı |
| 2 | Login – Empty Password | 401 | ✅ Başarılı |
| 3 | Login – Empty Email | 401 | ✅ Başarılı |
| 4 | Login – Invalid Email Format | 401 | ✅ Başarılı |
| 5 | Login – Empty Body | 401 | ✅ Başarılı |
| 6 | Register – Invalid Email Format | 400 | ✅ Düzeltildi |
| 7 | Register – Empty Password | 400 | ✅ Düzeltildi |

---

## 5. JSON Şema Dosyası

Mobil ekip için hazırlanan tam JSON şema dosyası: `auth_schemas.json`

Bu dosya şunları içerir:
- `RegisterRequest` şeması
- `LoginRequest` şeması  
- `RegisterSuccess` response şeması
- `LoginSuccess` response şeması
- `ErrorResponse` standart hata şeması
- Tüm `errorCode` değerleri ve açıklamaları
