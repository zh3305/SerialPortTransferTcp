using System.IO.Ports;
using System.Net.Sockets;
using System.Text;

namespace SerialPortTransferTcp;

public class Program
{
    private static SerialPort _serialPort;

    //TCP服务器端口
    private const int port = 8000;
    //TCP服务器IP
    private const string server = "127.0.0.1";
    static TcpClient client = null;

    static void Main(string[] args)
    {
        //开始链接串口
        _serialPort = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
        _serialPort.Open();//开始链接串口
        Console.WriteLine("串口链接成功!");

        //开始链接TCP 
        startConnectTcp();
        Console.ReadLine();
    }

    public static void startConnectTcp()
    {
        try
        {
            client = new TcpClient();
            client.Connect(server, port);

            NetworkStream stream = client.GetStream();
            Console.WriteLine("TCP Connected:" + client.Connected);
          
            _serialPort.BaseStream.CopyToAsync(stream);//将串口流拷贝到TCP流
            stream.CopyToAsync(_serialPort.BaseStream);//将TCP流拷贝到串口流
         
            Console.WriteLine("按任意键结束!");
            Console.Read();

            stream.Close();
            client.Close();
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
        }

    }
}