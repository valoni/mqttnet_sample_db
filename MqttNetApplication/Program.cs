using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Diagnostics;
using MQTTnet.Exceptions;
using System.IO;
using System.Diagnostics.Tracing;
using System.Net;
using MQTTnet.Internal;
using System.Net.NetworkInformation;
using static System.Collections.Specialized.BitVector32;

namespace MqttNetApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MsSqlDataAccess.ConnStr = ConfigurationManager.AppSettings["DB"].ToString();
             

            var ipAddress = GetIpAddress();
            
            var mqttsport = 8883;
            var mqttport = 1883;


            var mqttFactory = new MqttFactory();

             //var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

            var mqttServerOptions = new MqttServerOptionsBuilder()
               .WithDefaultEndpoint()
                .WithDefaultEndpointPort(mqttport)
                .WithEncryptedEndpoint()
                .WithEncryptedEndpointPort(mqttsport)
                //.WithEncryptionCertificate(certificate.Export(X509ContentType.Pfx))
                //.WithEncryptionSslProtocol(SslProtocols.Tls12)
                .Build();


            using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions))
            {
                mqttServer.InterceptingClientEnqueueAsync+=MqttServer_InterceptingClientEnqueueAsync;
                mqttServer.ClientConnectedAsync+=MqttServer_ClientConnectedAsync;

                mqttServer.ValidatingConnectionAsync += e =>
                {
                    var vName = e.UserName;
                    var vPassword = e.Password;
                    var vClientId = e.ClientId;
                    var vIp = e.Endpoint.ToString();

                    DateTime vDt = DateTime.Now;

                    /*logs to know who did connection request and from which ip*/
                    UserLogs(vName, vPassword, vClientId, vDt , vIp);

                    /* to check authorisation */
                    string scrptstoexecuted = "select count(*) cnt from dbo.users where allowed=1 and username='"+vName+"' and userpass='"+vPassword+"';";
                   
                    /* check did exists it */
                    int rez = Convert.ToInt32(MsSqlDataAccess.ExecuteScalar(scrptstoexecuted));

                    if (rez==0)
                    {
                        e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                    }
                        

                    return Task.CompletedTask;
                };

                 mqttServer.StartAsync();

                Console.WriteLine("----------------------------");
                Console.Write(" BROKER START ("+ ipAddress.ToString()+") : ");
                Console.WriteLine(DateTime.Now.ToString());
                Console.WriteLine("----------------------------\n");
                Console.WriteLine("netsh advfirewall firewall add rule name = \"MQTTS Port "+mqttsport+"\" dir =in action = allow protocol = TCP localport = "+mqttsport+" \n");
                Console.WriteLine("netsh advfirewall firewall add rule name = \"MQTT Port "+mqttport+"\" dir =in action = allow protocol = TCP localport = "+mqttport+" \n\n");
                Console.WriteLine("----------------------------\n");
                Console.WriteLine("Press Enter to exit.");
                Console.WriteLine("----------------------------\n");
                Console.ReadLine();

                 mqttServer.StopAsync();

            }

        }

        /* user requests logs */
        private static void UserLogs(string username,string password,string clientid, DateTime dt, string ip)
        {
            Console.Write("user logs from ip : ");
            Console.WriteLine(ip);

            string InsertUserLogs = "INSERT INTO [dbo].[userlogs]  ([username] ,[password],[clientid],[timestamps],[clientip])  VALUES ('"+username+"','"+password+"','"+clientid+"','"+dt.ToString("yyyy-MM-dd HH:mm:ss.fff")+"','"+ip+"');";

            MsSqlDataAccess.NonQuery(InsertUserLogs);

        }

        /* client are connected */
        private static Task MqttServer_ClientConnectedAsync(ClientConnectedEventArgs arg)
        {
            DateTime dt= DateTime.Now;
            string ClientsId = arg.ClientId.ToString();
            string ClientIp = arg.Endpoint.ToString();

            Console.WriteLine("time -> "+dt.ToString());
            Console.WriteLine("client id -> "+ClientsId);
            Console.WriteLine("connected from -> "+ClientIp+"\n");


            InsertToIpLogs(dt, ClientsId, ClientIp);

            return Task.CompletedTask;
        }

        private static void InsertToIpLogs(DateTime dt,string clientid,string ipaddress)
        {
            string SqlInsert = "insert into iplogs values('"+clientid+"', '"+ipaddress+"', '"+dt.ToString("yyyy-MM-dd HH:mm:ss.fff")+"');";

             MsSqlDataAccess.NonQuery(SqlInsert);
          
        }

        /* to know what client what topic is used  */
        private static Task MqttServer_InterceptingClientEnqueueAsync(InterceptingClientApplicationMessageEnqueueEventArgs arg)
        {
            DateTime dt = DateTime.Now;
            string ClientId = arg.SenderClientId.ToString();
            string Topic = arg.ApplicationMessage.Topic.ToString();
            string Payload = arg.ApplicationMessage.ConvertPayloadToString();

            /*
             
                https://www.hivemq.com/blog/mqtt-essentials-part-6-mqtt-quality-of-service-levels/
             
               At most once (QoS 0)
               At least once (QoS 1)
               Exactly once (QoS 2)
            */
            //string QOS = ((int)arg.ApplicationMessage.QualityOfServiceLevel).ToString();
            string QOS =  arg.ApplicationMessage.QualityOfServiceLevel.ToString();


            Console.WriteLine("time -> " + dt.ToString());
            Console.WriteLine("client -> "+ ClientId);
            Console.WriteLine("topic -> "+Topic);
            Console.WriteLine("payload -> "+Payload);
            Console.WriteLine("QOS -> "+QOS+"\n");
            
            InsertIntoLogs(dt, ClientId, Topic , Payload, QOS);

            return Task.CompletedTask;
        }

        private static void InsertIntoLogs(DateTime dt,string clientid, string topic,string payload, string qos)
        {
            string SqlInsert = "INSERT INTO [dbo].[logs] ([logdate] ,[clientid],[topic] ,[payload],[qos]) VALUES ('"+dt.ToString("yyyy-MM-dd HH:mm:ss.fff")+"', '"+clientid+"', '"+topic+"','"+payload+"','"+qos+"');";

            MsSqlDataAccess.NonQuery(SqlInsert);
        }


        public static System.Net.IPAddress GetIpAddress()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .Where(n => n.Name == "Wi-Fi")
                .SelectMany(n => n.GetIPProperties()?.UnicastAddresses)
                .Where(n => n.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .FirstOrDefault();
        }

        public static System.Net.IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .FirstOrDefault();
        }
    }

    /* helper to concert payload into string ... */
    public static class MqttApplicationMessageExtensions
    {
        public static string ConvertPayloadToString(this MqttApplicationMessage applicationMessage)
        {
            if (applicationMessage == null)
            {
                throw new ArgumentNullException(nameof(applicationMessage));
            }

            if (applicationMessage.PayloadSegment == EmptyBuffer.ArraySegment)
            {
                return null;
            }

            if (applicationMessage.PayloadSegment.Array == null)
            {
                return null;
            }

            var payloadSegment = applicationMessage.PayloadSegment;
            return Encoding.UTF8.GetString(payloadSegment.Array, payloadSegment.Offset, payloadSegment.Count);
        }
    }
}
