using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace BarcodeReader
{
    class Program
    {

        private static SerialPort _serialPort;
        public static void Main()
        {
            bool _continue;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;


            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = SetPortName();
            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.DataReceived += _serialPort_DataReceived;
            _serialPort.Open();
            _continue = true;


            Console.WriteLine("Type QUIT to exit");

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.Write(Encoding.UTF8.GetBytes("B??\r"), 0, 4);
                }
            }
            _serialPort.Close();
        }

        private static void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {


            var port = (SerialPort)sender;

            var bytes = new List<int>();

            //int toRead = port.BytesToRead;

            //while (toRead > 0)
            //{
            //    bytes.Add(port.ReadByte());
            //    toRead--;
            //}

            Console.WriteLine(port.ReadExisting());
        }

        // Display Port values and prompt user to enter a port.
        public static string SetPortName()
        {

            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("{0}", s);
            }

            Console.Write("Enter port value: ");
            portName = Console.ReadLine();

            if (portName == "")
            {
                return SetPortName();
            }
            return portName;
        }



    }
}