using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSerialClient.Services
{
    public class SerialService
    {
        private SerialPort _port;
        private ApiService _api;

        public void Create(string port)
        { 
            _port = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            _port.Handshake = Handshake.RequestToSend;

            _port.DataReceived += _port_DataReceived;

            _api = new ApiService();
        }

        private async void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int read = 0;
            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < 256; i++)
            {
                //_port.Open();
                read = _port.ReadByte();

                byte[] by = new byte[1];
                by[0] = Convert.ToByte(read);

                string chr = Encoding.ASCII.GetString(by);

                //_port.Close();

                builder.Append(chr);
            }

            string command = builder.ToString();

            int end = command.LastIndexOf('X');
            command = command.Substring(0, end);

            string[] commands = command.Split(' ');

            switch (commands[0])
            {
                case "exit":
                    break;
                case "loaddoc":
                    string doc = "";

                    if (commands.Length > 1)
                    {
                        if (commands.Length == 2)
                        {
                            doc = await _api.GetDocument(commands[1]);
                        }
                        else
                        {
                            doc = await _api.GetDocument(commands[1], commands[2]);
                        }
                    }
                    else
                    {
                        doc = await _api.GetDocument("");
                    }

                    //Send("DOCUMENT");
                    string length = doc.Length.ToString("000000");
                    Send(length);
                    Send(doc);

                    break;
            }
        }

        private void Send(string data)
        {
            foreach (var ch in data.ToArray())
            {
                _port.Open();
                _port.Write(ch.ToString());
                _port.Close();
            }
        }
    }
}
