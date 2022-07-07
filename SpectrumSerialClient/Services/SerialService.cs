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

        public void Create(string port)
        { 
            _port = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            _port.Handshake = Handshake.RequestToSend;

            _port.DataReceived += _port_DataReceived;
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int read = 0;
            StringBuilder builder = new StringBuilder();

            while (read != 0)
            {
                _port.Open();
                read = _port.ReadByte();

                byte[] by = new byte[1];
                by[0] = Convert.ToByte(read);

                string chr = Encoding.ASCII.GetString(by);

                _port.Close();

                builder.Append(chr);
            }

            string command = builder.ToString();
        }

        public void Send(string data)
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
