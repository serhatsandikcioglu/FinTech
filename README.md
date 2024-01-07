
# FinTech


Bu projenin ana hedefi, günümüzdeki bankacılık sektörünün dijitalleşen dinamiklerine uygun, güvenli ve kullanıcı dostu bir RESTful API geliştirmektir. .NET platformu üzerinde gerçekleştirilecek bu proje, finansal hizmetlerin modern gereksinimlerini karşılamak üzere tasarlanmıştır. Kullanıcılar, bankacılık işlemlerini gerçekleştirmek ve finansal ihtiyaçlarını yönetmek adına kapsamlı bir API üzerinden erişim sağlayabileceklerdir.

Proje, temel bankacılık işlevlerini içerirken, kullanıcı yönetimi, hesap işlemleri, para transferleri, kredi başvuruları, otomatik ödemeler ve müşteri destek talepleri gibi geniş bir yelpazede işlevsellik sunmaktadır. Güvenlik odaklı tasarımı sayesinde, kullanıcı kimlik doğrulaması ve yetkilendirme işlemleri, şifre güvenliği, finansal işlemlerin güvenliği gibi kritik alanlarda endüstri standartlarını karşılamayı hedeflemektedir.


## Tech Stack

 C#, Entityframework Core, PostgreSQL, JSON Web Token, Asp.Net Core API, Xunit, Quartz

## Kullanım Kılavuzu

### Kullanıcı Yönetimi

Projenin kullanıcı yönetimi işlevselliğini içermektedir. Kullanıcıların sisteme kaydolması, giriş yapması gibi temel işlemleri içermektedir. Aşağıda bu bölümdeki temel işlevselliğin detayları bulunmaktadır:

**Kullanıcı Kaydı:** Kullanıcılar, sisteme kaydolmak için gerekli bilgileri sağlayarak hesap oluşturabilirler. Bu aşamada sistemin güvenliğini sağlamak adına kullanıcı şifreleri, özel bir hash algoritması kullanılarak güvenli bir şekilde saklanmaktadır.

**Kullanıcı Girişi:** Kayıtlı kullanıcılar, güvenli token tabanlı kimlik doğrulama (JWT) kullanarak sisteme giriş yapabilirler. Bu, kullanıcıların güvenli bir şekilde oturum açmasını sağlayan modern bir kimlik doğrulama yöntemidir.

**Rol Bazlı Yetkilendirme:** Kullanıcıların sistemdeki rollerine göre farklı işlevlere erişimleri vardır. Örneğin, "manager", "customer" gibi roller tanımlanarak, her kullanıcının sistemi belirli bir yetki düzeyinde kullanabilmesi sağlanmaktadır.

Bu bölümdeki işlevselliğin amacı, kullanıcıların sistemi güvenli bir şekilde kullanabilmelerini sağlamak ve sistem yöneticilerine gerekli araçları sunarak kullanıcı yönetimini etkili bir şekilde gerçekleştirebilmelerini sağlamaktır.
### Hesap Yönetimi

Projenin hesap yönetimi işlevselliğini kapsar. Kullanıcıların yeni hesap açmalarını, mevcut bakiyelerini görüntülemelerini ve güncellemelerini sağlar. Aşağıda bu bölümdeki temel işlevselliğin ayrıntıları yer almaktadır:

**Yeni Hesap Açma:** Kullanıcılar, sisteme yeni bir hesap açmak için gereken bilgileri sağlayarak hesap oluşturabilirler. Bu aşamada, hesap açılışında belirlenen minimum bakiye kontrolü yapılarak, kullanıcılardan gerekli olan miktarın belirli bir seviyenin üzerinde olması şartı koşulur. Kullanıcı kaydı yapıldığı sırada verilen ana hesaptan para aktararak yeni hesaplarını oluşturabilirler.

**Bakiye Görüntüleme:** Kullanıcılar, hesaplarına ait bakiyeyi görüntüleyebilirler.

**Bakiye Güncelleme:** Yöneticiler, sistemin belirlediği güvenlik kuralları ve yetkilendirmeler çerçevesinde bakiye güncellemelerini gerçekleştirebilirler. Bu yetki, belirli rollerdeki kullanıcılara atanabilir ve sadece bu kullanıcılar tarafından kullanılabilir.

Bu bölümdeki işlevselliğin amacı, kullanıcıların hesaplarını güvenli bir şekilde yönetmelerini sağlamak ve sistemin belirlediği kurallara uygun olarak hesap açma ve bakiye güncelleme işlemlerini gerçekleştirmelerine olanak tanımaktır.
### Para Giriş ve Çıkış Kayıtları

Projenin para yatırma ve çekme işlevselliğini kapsar. Kullanıcılar, finansal işlemlerini gerçekleştirirken, her bir işlem için detaylı kayıtların tutulması, bakiye kontrolü ve güvenli işlem süreçleri öncelikli olarak ele alınmaktadır. Aşağıda bu bölümdeki temel işlevselliğin detayları yer almaktadır:

**İşlem Kayıtları:** Her bir finansal işlem, miktar, işlem türü, tarih ve zaman gibi detaylarla birlikte sistemde kaydedilir. Bu, kullanıcılar ve yöneticiler için geçmiş işlemlere erişim sağlayarak hesap geçmişini takip etmelerini sağlar.

**Bakiye Kontrolü:** Para çekme işlemlerinde, talep edilen miktar mevcut bakiyeden fazla ise işlem otomatik olarak reddedilir. Bu kural, kullanıcıların bakiye sınırları içinde güvenli bir şekilde işlem yapmalarını sağlar.

**Bakiye Tutarsızlığı Kontrolü:** Birden fazla işlem aynı anda gerçekleştiğinde, bakiye tutarsızlığını önlemek adına kilit mekanizması kullanılır. Bu, aynı hesaptaki işlemlerin birbirini etkilememesini ve bakiyenin doğru bir şekilde güncellenmesini sağlar.

Bu bölümdeki işlevselliğin amacı, kullanıcıların güvenli ve tutarlı bir şekilde finansal işlemlerini gerçekleştirebilmelerini sağlamak ve sistemdeki finansal verilerin doğruluğunu korumaktır.
### İletişim Kurma

Projenin iç ve dış transfer işlevselliğini kapsar. Kullanıcılar, finansal kaynaklarını gönderen ve alıcı hesaplar arasında güvenli bir şekilde aktarabilmek adına bu işlevselliği kullanabilirler. Aşağıda, bu bölümdeki temel işlevselliğin detayları yer almaktadır:

**İç ve Dış Transfer İşlemleri:** Kullanıcılar, hem kendi hesapları arasında hem de dışarıdaki hesaplar arasında transfer işlemleri gerçekleştirebilirler.

**Atomik İşlemler:** Transfer işlemleri, gönderen ve alıcı hesaplar için atomik işlemler olarak gerçekleşir. Eğer transferin bir kısmı başarısız olursa, tüm işlem iptal edilir. Bu, finansal güvenliği sağlayan kritik bir özelliktir.

**Transfer Limit Kontrolleri:** Her transfer işlemi için günlük ve işlem başına transfer limitleri kontrol edilir. Bu, güvenlik ve kontrol amacıyla kullanıcının günlük maksimum transfer miktarını sınırlandırarak potansiyel riskleri azaltır.

Bu bölümdeki işlevselliğin amacı, kullanıcıların finansal varlıklarını güvenli bir şekilde yönetebilmelerini sağlamak ve transfer işlemlerinin güvenilir ve bütünlük içinde gerçekleşmesini sağlamaktır.

### Kredi İşlemleri

Projenin kredi işlemleri işlevselliğini kapsar. Kullanıcılar, kredi başvurularında bulunabilir ve kredi durumlarını sorgulayabilirler. Aşağıda, bu bölümdeki temel işlevselliğin detayları yer almaktadır:

**Kredi Başvuruları:** Kullanıcılar, kredi başvurularını sisteme ileterek finansal destek talebinde bulunabilirler. Bu aşamada, müşterinin kredi geçmişi ve mali durumu değerlendirilir.

**Kredi Skoru Kontrolü:** Onay sırasında müşterinin kredi geçmişi ve mali durumu, kredi skoru kontrolü ile değerlendirilir. Bu değerlendirme, kredi başvurusunun onaylanıp onaylanmamasına karar verilmesinde etkilidir.

**Kredi Skoru Kontrol Servisi:** Kredi skoru kontrol servisi, müşterinin kredi skorunu hesaplar ve kredi başvurusunun onaylanıp onaylanmaması konusunda bir öneri sunar. Bu servis, kredi başvurularının objektif bir şekilde değerlendirilmesine yardımcı olur.

**Ödeme Planları:** Onaylanan kredi başvuruları için ödeme planları oluşturulur. Kredi ödemeleri, belirlenen ödeme planına uygun şekilde takip edilir.

**Kredi Durum Sorgulamaları:** Kullanıcılar, kredi başvurularının onaylanma veya reddedilme durumunu sorgulayabilirler.

Bu bölümdeki işlevselliğin amacı, kullanıcılara finansal destek sağlama sürecini yönetmelerine olanak tanımak ve kredi başvurularının doğru ve güvenilir bir şekilde değerlendirilmesini sağlamaktır.

### Otomatik Ödemeler
Projenin otomatik ödemeler işlevselliğini kapsar. Kullanıcılar, düzenli fatura ödemelerini gerçekleştirmek amacıyla otomatik ödeme ayarlarını yapabilirler. Aşağıda, bu bölümdeki temel işlevselliğin detayları yer almaktadır:

**Otomatik Ödeme Ayarları:** Kullanıcılar, düzenli fatura ödemelerini kolayca yönetebilmek için otomatik ödeme ayarları yapabilirler. Bu ayarlar, belirli bir hesaptan belirli bir periyot içinde düzenli olarak ödeme yapılmasını sağlar.

**Yeterli Bakiye Kontrolü:** Otomatik ödemeler, yeterli bakiye kontrolü ile gerçekleşir. Her ödeme döneminde, sistem belirlenen hesaplardaki bakiyeleri kontrol eder.

**Otomatik Ödemeleri Yönetme ve İptal Etme:** Kullanıcılar, otomatik ödemeleri yönetebilir ve istedikleri zaman bu ödemeleri iptal edebilirler.

Bu bölümdeki işlevselliğin amacı, kullanıcılara düzenli fatura ödemelerini otomatikleştirme ve yönetme konusunda kolaylık sağlamaktır.

### Destek Talepleri

Projenin müşteri destek talepleri yönetimi işlevselliğini kapsar. Kullanıcılar, çeşitli konularda destek talepleri oluşturabilir ve bu taleplerin durumlarını takip edebilirler. Aşağıda, bu bölümdeki temel işlevselliğin detayları yer almaktadır:

**Destek Talepleri Oluşturma:** Kullanıcılar, sistem üzerinden müşteri destek taleplerini oluşturabilirler. Bu talepler genellikle hesaplar, ödemeler veya genel sorularla ilgili olabilir.

**Öncelik ve İşlenme Sırası:** Destek talepleri, oluşturulma sırasına göre işlenir ve önceliklendirilir. Bu, müşteri destek süreçlerinin adil ve düzenli bir şekilde yönetilmesine olanak tanır.

**Durum Takibi:** Kullanıcılar, oluşturdukları destek taleplerinin durumunu sistem üzerinden takip edebilirler. Talepler, oluşturulduktan sonra farklı aşamalardan geçer ve kullanıcılara bu süreçte bilgi sağlanır.

Bu bölümdeki işlevselliğin amacı, müşteri destek taleplerini düzenli bir şekilde yönetmek, önceliklendirmek ve çözüme kavuşturmak olarak özetlenebilir. Kullanıcıların sorunlarına hızlı ve etkili bir şekilde yanıt vermek ve destek süreçlerini şeffaf bir şekilde yönetmek ön planda tutulmuştur.

## Kullanıcılar ve Roller

Bu projede, aşağıdaki kullanıcılar ve roller sistem başlatılırken oluşmaktadır.

### Kullanıcılar

- **Admin**
  - Kullanıcı Adı: admin
  - Şifre: Asd123*

- **Manager**
  - Kullanıcı Adı: manager
  - Şifre: Asd123*

- **Loan Officer**
  - Kullanıcı Adı: loanOfficer
  - Şifre: Asd123*

- **Customer Support**
  - Kullanıcı Adı: customerSupport
  - Şifre: Asd123*

- **Support Ticket Analyst**
  - Kullanıcı Adı: supportTicketAnalyst
  - Şifre: Asd123*

## Author

- [@Serhat Sandıkçıoğlu](https://github.com/serhatsandikcioglu)
