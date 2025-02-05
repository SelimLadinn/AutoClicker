using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace autoclickerBeta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public int type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
        }

        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        const int INPUT_MOUSE = 0;
        const int MOUSEEVENTF_LEFTDOWN = 0x02;
        const int MOUSEEVENTF_LEFTUP = 0x04;

        public void leftClick()
        {
            INPUT[] inputs = new INPUT[2];

            inputs[0] = new INPUT
            {
                type = INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT { dwFlags = MOUSEEVENTF_LEFTDOWN }
                }
            };

            inputs[1] = new INPUT
            {
                type = INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT { dwFlags = MOUSEEVENTF_LEFTUP }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private bool isClicking = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = 10000; // Maksimum 10 saniye (10000 ms)
            numericUpDown1.Minimum = 1; // Minimum 1 ms olmalı
            numericUpDown1.Value = 100; // Varsayılan olarak 100 ms
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isClicking)
            {
                timer1.Stop();
                button2.Text = "Başlat";
            }
            else
            {
                int intervalValue = (int)numericUpDown1.Value;
                if (intervalValue < 1) intervalValue = 1;

                timer1.Interval = intervalValue;
                timer1.Start();
                button2.Text = "Durdur";
            }

            isClicking = !isClicking;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            leftClick();
        }
    }
}
