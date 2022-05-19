using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsChatApp
{
    public partial class Form1 : Form
    {

        public string ipRemote, portRemote, userName;
        byte[] bfr;
        Socket socket;
        EndPoint epLocal, epRemote;
        public PersonName nameF = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReuseAddress,true);
            lblLocalIp.Text = GetLocalIpAddress();
            buttonConnect.Enabled = false;
            this.ControlBox = false;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonConnect.Text == "Connect")
                {
                    epLocal = new IPEndPoint(IPAddress.Parse(lblLocalIp.Text), Convert.ToInt32(txtLocalPort.Text));
                    socket.Bind(epLocal);
                    epRemote = new IPEndPoint(IPAddress.Parse(ipRemote), Convert.ToInt32(portRemote));
                    socket.Connect(epRemote);
                    bfr = new byte[1500];
                    socket.BeginReceiveFrom(bfr, 0, bfr.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), bfr);
                    MessageBox.Show("Start communicate now", "Connected");
                    buttonConnect.Text = "Disconnect";
                }
                else
                {
                    epLocal = null;
                    epRemote = null;
                    socket = null;
                    bfr = null;
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    buttonConnect.Text = "Connect";
                    buttonConnect.Enabled = true;
                    ipRemote = "";
                    portRemote = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid IP Address",ex.Message);;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            nameF.Show();
            
        }

        private string GetLocalIpAddress()
        {
            IPHostEntry hostAdd;
            hostAdd = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in hostAdd.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                byte[] sendingMessage = new byte[1500];
                var msg = "[" + userName.ToUpper() + " " + DateTime.Now + "]-> " + textMessage.Text;
                sendingMessage = aEncoding.GetBytes(msg);
                socket.Send(sendingMessage);
                listMessage.Items.Add(msg);
                textMessage.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message,"Communication Network Error");
            }
           
        }

        private void btnConfigureIP_Click(object sender, EventArgs e)
        {
            PersonName person = new PersonName();
            person.label1.Text = "IP ";
            person.label2.Text = "Port";
            person.txtPass.PasswordChar ='\0';
            person.button1.Text = "Set";
            person.label4.Text = "Enter User Name, IP Address and Port No.";
            person.ShowDialog();
            ipRemote = person.txtUserName.Text;
            portRemote = person.txtPass.Text;
            if (!string.IsNullOrEmpty(ipRemote) && !string.IsNullOrEmpty(portRemote) )
            {
                MessageBox.Show("IP and port Configured , Click connect to communicate");
                buttonConnect.Enabled = true;
            }
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                byte[] receivedData = new byte[1500];
                receivedData = (byte[]) aResult.AsyncState;
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                string receivedMessage = aEncoding.GetString(receivedData);
                listMessage.Items.Add(receivedMessage);
                bfr = new byte[1500];
                socket.BeginReceiveFrom(bfr, 0, bfr.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), bfr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
