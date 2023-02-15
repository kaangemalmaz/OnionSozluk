# _Basit Bir Sozluk Sitesi_

### _Layers_
- Api
	-- Domain
	-- Application
	-- Persistence
	-- API
- Clients
- Common
- Projections
	-- Favorite Service (Worker)
	-- User Service (Worker)
	-- Vote Service (Worker)

### _Features_
- Fluent API
- Entity Framework Core
- Bogus (Generate seed data)
- Auto Mapper
- Fluent Validation
- CQRS
- Mediatr
- Hashing
- Validate Action Filter
- Authentication - JWT
- Exception Custom Middleware
- Worker Service
- Rabbitmq
- Dapper

> Worker service : 
 Uygulamalarinin calismaya basladigi andan calismayi durdurma anina kadar ki gecen surecte arka planda calisan servislerdir. Ornegin belirli zaman araliklarinda Cache temizleme islemleri, belirli zaman araliklarinda sistem durumunu inceleyip raporlayan servisler gibi ornekler verilebilir. Workerlar herhangi bir gorunumden (View) bagimsiz olarak calisir. Yani projelerimizde genellikle bir sayfaya girildiginde ilgili sayfaya ait servisin calistirilmasi saglanir ama Workerlar projenin sunucuya yuklenip yayina alinmasiyla birlikte calisir ve proje omrunu tamamlayana kadar arka planda yasam dongusunu surdurur.
 Worker .Net Core 3.0 ile .Net’ e eklendi. ve .Net Core 2.1 de IHostedService olarak kullanimi mevcut ama 3.0 ile gelen Worker, IHostedService yapisini daha derli toplu bir hale getirerek kullanimi kolaylastirmis oldu.