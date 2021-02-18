using System;
using System.IO.Ports;


namespace SerialPortTest.SerialConnection
{
    public class SerialPortArduinoConnection
    {

        
        private SerialPort serialPort;

        private string ComPort;
        private int Baud;
        public bool IsOpen => serialPort != null ? serialPort.IsOpen : false ;

        public event SerialDataFromArduinoEvent DataFromArduinoHandler;//Evento que se asigna fuera de esta clase

        public delegate void SerialDataFromArduinoEvent(string data);//Delegado que sirve para ser usado fuera de esta clase


        public SerialPortArduinoConnection(string ComPort, int Baud)
        {

            this.ComPort = ComPort;
            this.Baud = Baud;
            Connect();
        }

        public bool Connect()
        {
            //Las siguientes 3 líneas se conectan a un puerto serial con un baud en especifico, abre el puerto y asigna un evento en espera de que llegue información desde Arduino
            serialPort = new SerialPort(ComPort, Baud);
            serialPort.Open();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            return serialPort.IsOpen;
        }

        //Evento que envía la información  Arduino por el puerto serial
        internal void SendToArduino(string data) => serialPort.Write(data);

        //Manejador del puerto serial 
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //las siguientes 2 líneas obtienen el string del pruerto serial
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Se envía la información obtenida por el puerto serial al manejador en el program (DataFromArduinoEventHandler)
            DataFromArduinoHandler.Invoke(indata);

        }

    }
}
