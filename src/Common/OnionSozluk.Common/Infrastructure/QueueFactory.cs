using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OnionSozluk.Common.Infrastructure
{

    // TODO daha okunakli yapılacak.
    public static class QueueFactory
    {
        public static void SendMessageToExchange(string exchangeName,
                                                 string exchangeType,
                                                 string queueName,
                                                 object obj)
        {
            // tüketici için channel oluştururken exchange ve queue oluştu mu kontrol et!
            // create basic consumer yerine connection, exchange ve queue yapıları verilebilir.
            var channel = CreateBasicConsumer()
                            .EnsureExchange(exchangeName, exchangeType)
                            .EnsureQueue(queueName, exchangeName)
                            .Model; // channel içindeki model gelmelidir. Channel IModelden türer.

            // önce objeyi stringe çevir sonrasında bytes'a çevir.
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
        }

        // Basit bir tüketici 
        public static EventingBasicConsumer CreateBasicConsumer()
        {
            // aslında burası sadece connection bağlantısını ve channeli oluşturuyor.

            var factory = new ConnectionFactory() { HostName = SozlukConstants.RabbitMQHost };
            var connection = factory.CreateConnection(); //connection oluştur.
            var channel = connection.CreateModel(); // channel oluştur.

            return new EventingBasicConsumer(channel); //burası consumer için kritik sadece.
        }

        // İlk olarak emin olmak için exchange üretildi mi?
        public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer,
                                                           string exchangeName,
                                                           string exchangeType = SozlukConstants.DefaultExchangeType)
        {
            consumer.Model.ExchangeDeclare(exchange: exchangeName,
                                           type: exchangeType,
                                           durable: false,
                                           autoDelete: false);
            return consumer;
        }

        // kuyruk üretildi mi kontrol ? 
        public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer,
                                                           string queueName,
                                                           string exchangeName)
        {
            consumer.Model.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        null);

            // routing key "direct" typelarda yine queue name e denk gelmektedir.
            consumer.Model.QueueBind(queueName, exchangeName, queueName);

            return consumer;
        }

        public static EventingBasicConsumer Receive<T>(this EventingBasicConsumer consumer, Action<T> act)
        {
            //EventingBasicConsumer bir model ve birtanede eventargs verir. yani rabbitmq dan bir kayıt geldiğinde.
            consumer.Received += (m, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var model = JsonSerializer.Deserialize<T>(message);

                act(model);
                consumer.Model.BasicAck(eventArgs.DeliveryTag, false);
            };

            return consumer;
        }

        public static EventingBasicConsumer StartConsuming(this EventingBasicConsumer consumer, string queueName)
        {
            consumer.Model.BasicConsume(queue: queueName,
                                        autoAck: false,
                                        consumer: consumer);

            return consumer;
        }
    }
}

