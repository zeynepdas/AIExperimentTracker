# AIExperimentTracker API

## Proje Açıklaması
AIExperimentTracker, **.NET 9** kullanılarak geliştirilmiş bir RESTful Web API projesidir.  
Bu proje, yapay zeka projelerinin ve bu projelere ait deneylerin (experiment) yönetilmesini amaçlamaktadır.

Bir kullanıcı birden fazla yapay zeka projesine sahip olabilir.  
Her yapay zeka projesi birden fazla deney (experiment) içerebilir ve her deney farklı değerlendirme metrikleri ile ölçülür.

Proje, derste anlatılan **katmanlı mimari yaklaşımı** temel alınarak geliştirilmiş, buna ek olarak **.NET 8/9 ile önerilen Minimal API yaklaşımı** kullanılmıştır.

---

## Kullanılan Teknolojiler
- .NET 9
- ASP.NET Core Minimal API
- Swagger / OpenAPI
- Entity Framework Core
- Git & GitHub

---

## Mimari Yapı
Projede katmanlı mimari yaklaşımı uygulanmıştır.

- **Entities**: Veritabanı tablolarını temsil eden sınıflar
- **DTOs**: API ile veri alışverişi sırasında kullanılan veri transfer nesneleri
- **Services**: İş kurallarının ve iş mantığının bulunduğu katman
- **Data**: Veritabanı bağlantısı ve DbContext yapılandırması
- **Responses**: Standart API response yapıları
- **Middlewares**: Global hata yönetimi gibi ara katmanlar

> Not: Derste controller tabanlı yapı anlatılmıştır.  
> Bu projede aynı mimari mantık korunarak, .NET 8/9 sürümlerinde önerilen **Minimal API yaklaşımı** tercih edilmiştir.

---

## Entity Yapıları ve İlişkiler

### Entity Listesi
- User
- AIProject
- Experiment
- Metric

### İlişkiler
- Bir **User**, birden fazla **AIProject**’e sahip olabilir.
- Bir **AIProject**, birden fazla **Experiment** içerebilir.
- Bir **Experiment**, birden fazla **Metric** ile değerlendirilebilir.

---

## API Response Formatı
Tüm API cevapları standart bir formatta döndürülmektedir:

```json
{
  "success": true,
  "message": "İşlem başarılı",
  "data": {}
}
