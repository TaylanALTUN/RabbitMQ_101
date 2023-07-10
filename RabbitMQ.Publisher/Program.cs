// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();

//factory.Uri = new Uri("http://localhost:15672/#/");
factory.Port = 5672;
factory.HostName = "localhost";
factory.UserName = "guest";
factory.Password = "guest";

using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();

    //durable true set edilirse oluşan kuyruklar fiziksel olarak kaydedilir, false olarak set edilirse memoryde tutulur
    //exclusive true set edilirse burada oluşturulan kanal üserinden bağlanabilir, false set edilirse farklı kanallardan da bağlanılabilir.
    //auto delete true olarak set edilirse kuyruğa bağlı en son subscriber giderse kuyrukda silinir.
    channel.QueueDeclare("hello-queue", true, false, false);

    Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
        string message = $"Message {x}";

        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(String.Empty, "hello-queue", null, messageBody);

        Thread.Sleep(1500);

        Console.WriteLine($"Mesaj gönderilmiştir: {message}");
    });
}


Console.ReadLine();
