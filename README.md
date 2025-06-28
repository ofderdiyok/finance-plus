Finance Plus - Full Stack Finance Tracker
=========================================

Finance Plus, kişisel finans takibi ve yönetimi için geliştirilmiş tam yığınlı (full-stack) bir web uygulamasıdır. Kullanıcılar gelir/gider işlemlerini kaydedebilir, para transferleri gerçekleştirebilir ve bütçe durumlarını görselleştirilmiş paneller aracılığıyla analiz edebilirler.

📦 Proje Bileşenleri
-------------------

**Backend**
- ASP.NET Core Web API (C#)
- Entity Framework Core ile veritabanı işlemleri
- PostgreSQL (veritabanı)
- JWT Token ile kimlik doğrulama
- Docker desteği

**Frontend**
- React (TypeScript)
- PrimeReact (Sakai teması) — ⚠️ PrimeReact Sakai teması lisans gerektirir.
- React Router
- Axios (API istekleri için)

🔧 Kurulum Talimatları
----------------------

### Gereksinimler
- .NET 8 SDK
- Node.js (v18+)
- PostgreSQL
- Docker (opsiyonel)

### Backend Kurulumu

```bash
cd finance-plus
dotnet restore
dotnet ef database update
dotnet run
```

> Not: `appsettings.json` içinde veritabanı bağlantı bilgilerini ve JWT secret değerini güncelleyin.

### Frontend Kurulumu

```bash
cd finance-plus/sakai-react-master
npm install
npm start
```

> Uygulama varsayılan olarak `http://localhost:3000` adresinde çalışır.

🎯 Özellikler
------------

- 🔐 JWT tabanlı kullanıcı girişi ve kimlik doğrulama
- 💸 Gelir-gider işlemleri ve para transfer kayıtları
- 📊 Dashboard üzerinden finansal özet ve analizler
- 🔍 Veri filtreleme, sıralama ve sayfalama (pagination)
- 👤 Kullanıcı profili düzenleme sayfası
- 🧩 Modüler servis mimarisi
- 📦 Docker ile container üzerinde çalışma desteği

📁 Proje Yapısı
--------------

```
finance-plus/
├── Controllers/
├── Services/
├── Models/
├── Data/
└── Program.cs

sakai-react-master/
├── src/
│   ├── components/
│   ├── pages/
│   ├── services/
│   └── App.tsx
└── public/
```

📄 Lisans
--------

- Backend kodları: MIT Lisansı
- Frontend UI: PrimeReact Sakai Teması — _Ticari lisans_ gerektirir (https://www.primefaces.org/licenses/#sakai)

📬 İletişim
-----------

Her türlü geri bildirim veya katkı için iletişime geçebilirsiniz: farukderdiyok16@gmail.com

🕒 Son Güncelleme: 28.06.2025
