using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChat
{
    class Program
    {
        static TcpClient socket = new TcpClient();

        static void Main(string[] args)
        {
            //Task.Run(() =>
            //{
            //    socket.Connect("127.0.0.1", 320); //thread-blocking...
            //    Console.WriteLine("We are now connected to the server");
            //});

            ConnectToServer();

            Console.WriteLine("Hello world! {0}", DateTime.Now.Ticks);

            // client-side loop:
            while (true)
            {
                string input = Console.ReadLine();
                byte[] data = Encoding.ASCII.GetBytes(input);

                socket.GetStream().Write(data, 0, data.Length);
            }
        }

        async static void ConnectToServer()
        {
            try
            {
                await socket.ConnectAsync("127.0.0.1", 320);
                Console.WriteLine("We are now connected to server... {0}", DateTime.Now.Ticks);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect: {0}", e.Message);              
            }

            // get data from server:
            while (true)
            {
                byte[] data = new byte[socket.Available];

                await socket.GetStream().ReadAsync(data, 0, data.Length);                
                Console.WriteLine(Encoding.ASCII.GetString(data));
            }

        }

        //DEPRECATED DEPRECATED DEPRECATED

        //static void Main(string[] args)
        //{
        //    socket = new TcpClient();
        //    socket.BeginConnect("127.0.0.1", 320, new AsyncCallback(HandleConnection), null);

        //    Console.ReadLine();
        //}

        //static void HandleConnection(IAsyncResult ar)
        //{
        //    try
        //    {
        //        socket.EndConnect(ar);
        //        Console.WriteLine("Now connected to the server...");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("ERROR: {0}", e.Message);
        //    }
        //}
    }
}
