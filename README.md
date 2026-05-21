#  Mechanic Shop System 
# 🛠️ MechanicShop Management System

نظام متكامل لإدارة الورش الميكانيكية مبني باستخدام أحدث تقنيات **.NET 10** وبتصميم معماري نظيف (**Clean Architecture**) لضمان القابلية للتوسع والصيانة الفائقة.

---

## 🏗️ البنية المعمارية ومكونات النظام (Architecture)

المشروع مقسم ومبني بناءً على مبادئ **Clean Architecture** و **DDD (Domain-Driven Design)** لضمان فصل المهام (Separation of Concerns):

* **`MechanicShop.Api`**: طبقة العرض والـ REST APIs، وهي نقطة الدخول للنظام.
* **`MechanicShop.Client`**: الواجهة الأمامية للمشروع مبنية باستخدام **Blazor WebAssembly** لتوفير تجربة مستخدم سريعة وتفاعلية (SPA).
* **`MechanicShop.Application`**: تحتوي على الـ Core Business Logic، والـ Use Cases، وتطبيق نمط **CQRS** باستخدام **MediatR**.
* **`Mechanic.Infrastructure`**: طبقة البنية التحتية، مسؤولة عن الاتصال بقاعدة البيانات (EF Core)، والهوية (Identity)، والـ Logging، والخدمات الخارجية.
* **`MechanicShop.Domain`**: قلب المشروع، يحتوي على الـ Enterprise Entities، والـ Value Objects، والـ Domain Events بدون أي اعتمادية خارجية.
* **`MechanicShop.Contracts`**: تحتوي على الـ DTOs والمواصفات المشتركة بين الـ API والـ Client.

---

## 🚀 التقنيات المستخدمة (Tech Stack)

### الـ Backend والـ Core:
* **Framework:** .NET 10 (C#)
* **Database ORM:** Entity Framework Core 10 (SQL Server)
* **Mediator Pattern & CQRS:** MediatR 14
* **Validation:** FluentValidation
* **Logging & Tracing:** Serilog المعزز بنظام **Seq** لجمع وتحليل اللوجات بشكل مركزي.
* **PDF Generation:** QuestPDF لإنشاء وتصدير فواتير وتقارير احترافية.
* **Payments:** Stripe.net لدمج بوابات الدفع الإلكتروني.

### الـ Frontend:
* **Framework:** Blazor WebAssembly (SPA)
* **State Management:** Blazored.LocalStorage

### إدارة المشروع والـ DevOps:
* **Central Package Management (CPM):** إدارة جميع نسخ بكجات NuGet مركزياً عبر ملف `Directory.Packages.props`.
* **Global Build Configurations:** توحيد إعدادات البناء لجميع المشاريع عبر `Directory.Build.props`.
* **Containerization:** دمج **Docker** و **Docker Compose** لبيئة تطوير معزولة وسلسة تشمل (API, SQL Server, Seq).

---

## ⚙️ كيفية التشغيل والتثبيت (Quick Start)

المشروع مهيأ بالكامل ليعمل بضغطة زر واحدة بفضل Docker، دون الحاجة لتثبيت SQL Server أو أي أدوات خارجية على جهازك الشخصي.

### المتطلبات الأساسية:
1.  مثبت لديك **Visual Studio 2022** (أحدث نسخة تدعم .NET 10).
2.  برنامج **Docker Desktop** شغال في الخلفية.

### خطوات التشغيل:
1.  قم بعمل `Clone` للمستودع (Repository):
    ```bash
    git clone [https://github.com/your-username/MechanicShopFinalProject.git](https://github.com/your-username/MechanicShopFinalProject.git)
    ```
2.  افتح ملف الـ Solution `MechanicShopFinalProject.sln` باستخدام Visual Studio.
3.  من الـ Solution Explorer، تأكد أن المشروع الافتراضي للتشغيل (Startup Project) هو **`docker-compose`**.
4.  اضغط على زر **Play (F5)**.

سيتولى الـ Docker بناء وتشغيل الخدمات التالية تلقائياً:
* **Web API & Swagger:** لفحص وتجربة الـ Endpoints.
* **SQL Server Database:** مع عمل الـ Migrations والـ Data Seeding تلقائياً للبيانات التجريبية (Customers, Vehicles, Repair Tasks, Work Orders).
* **Seq Dashboard:** لمراقبة اللوجات على بورت `5341`.

---

## 📝 ميزات النظام الحالية (Features)
- [x] نظام هوية متكامل وصلاحيات (Manager, Labor) مع دعم الـ Refresh Tokens آمنة الطول.
- [x] إدارة بيانات الزبائن (Customers) ومركباتهم (Vehicles) مع نظام التحقق من البيانات (Validation).
- [x] جدولة أوامر الإصلاح (Work Orders) وتوزيعها على الفنيين بناءً على ورشات العمل المتاحة (Spots).
- [x] نظام تتبع لوجات مركزي لمراقبة الأخطاء في بيئة التطوير.