using System.Linq.Expressions;
using System.Security.Cryptography;
using SharpHook.Native;
using SharpHook.Reactive;
using SharpHook;
using System.Reactive.Linq;
using System.Timers;
using System.ComponentModel;
using System.Threading;
using System.Drawing.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Reactive;

namespace Autoclicker
{
    public partial class UI : Form
    {
        private bool keyCapturing = false;
        private bool mouseCapturing = false;
        private KeyCode toggleKey = KeyCode.VcPeriod;
        private MouseButton toggleKeyMouse = MouseButton.NoButton;
        private bool toggled = false;
        private int toggleDelay = 0;
        private int clickSpeed = 70;
        private bool holding = false;
        private bool clicking = false;
        private Color bg;
        SimpleReactiveGlobalHook kbHook = new SimpleReactiveGlobalHook(GlobalHookType.All);
        Thread kbThread;

        Stopwatch sw = Stopwatch.StartNew();

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
        }


        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll")]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);


        private void DoClick()
        {
            var inputDown = new INPUT
            {
                type = 0,
                u = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = 0,
                        dy = 0,
                        dwFlags = 0x0002
                    }
                }
            };

            var inputUp = new INPUT
            {
                type = 0,
                u = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = 0,
                        dy = 0,
                        dwFlags = 0x0004
                    }
                }
            };

            INPUT[] inputs = new INPUT[] { inputDown, inputUp };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public DateTimeOffset GetNow()
        {
            long fileTime;
            GetSystemTimePreciseAsFileTime(out fileTime);
            return DateTimeOffset.FromFileTime(fileTime);
        }
        public UI()
        {
            InitializeComponent();

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\neffas";
            string path = directory + @"\autoTxt.txt";

            if (Directory.Exists(directory) && File.Exists(path)) {
                string json = File.ReadAllText(path);

                AppSettings loadedSettings = JsonSerializer.Deserialize<AppSettings>(json);

                toggleDelay = loadedSettings.toggleDelay;
                clickSpeed = loadedSettings.clickSpeed;
                toggleKey = loadedSettings.toggleKey;
                toggleKeyMouse = loadedSettings.toggleKeyMouse;
            }

            if (toggleKey != KeyCode.VcNoName)
            {
                label6.Text = TrimKeyCode(toggleKey.ToString());
            } else
            {
                label6.Text = FormatMouseCode(toggleKeyMouse.ToString());
            }

            this.FormClosing += new FormClosingEventHandler(OnClose);
            bg = this.BackColor;

            input.Text = clickSpeed.ToString();
            input2.Text = toggleDelay.ToString();

            kbThread = new Thread(HookInit)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };

            kbThread.Start();

            Task.Run(() =>
            {
                while (true)
                {
                    if (holding && toggled)
                    {
                        if (toggleDelay > 0)
                        {
                            Thread.Sleep(toggleDelay);
                        }

                        if (!holding || !toggled)
                        {
                            continue;
                        }

                        int interval = 1000 / clickSpeed;
                        clicking = true;

                        long intTicks = (long)(interval * (Stopwatch.Frequency / 1000));
                        long next = sw.ElapsedTicks + intTicks;

                        while (holding && toggled)
                        {

                            if (!holding || !toggled)
                            {
                                break;
                            }

                            if (kbHook.IsDisposed)
                            {
                                break;
                            }

                            if (sw.ElapsedTicks >= next)
                            {
                                DoClick();
                                next = sw.ElapsedTicks + intTicks;
                            }
                        }

                        clicking = false;
                    }

                    Thread.Sleep(1);
                }
            });
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            
            toggled = false;
            holding = false;
            kbHook?.Dispose();
            kbThread?.Join();

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\neffas";
            string path = directory + @"\autoTxt.txt";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            AppSettings save = new AppSettings
            {
                toggleDelay = toggleDelay,
                clickSpeed = clickSpeed,
                toggleKey = toggleKey,
                toggleKeyMouse = toggleKeyMouse,
            };

            string json = JsonSerializer.Serialize(save);

            File.WriteAllText(path, json);
        }
        private void HandleToggle()
        {
            if (keyCapturing || mouseCapturing)
            {
                return;
            }

            toggled = !toggled;

            if (toggled)
            {
                this.BackColor = Color.Green;
            }
            else
            {
                this.BackColor = bg;
            }
        }

        private async void HookInit()
        {
            _ = kbHook.MousePressed.Subscribe(e =>
            {
                if (e.IsEventSimulated)
                {
                    return;
                }

                if (mouseCapturing || keyCapturing)
                {
                    mouseCapturing = false;
                    keyCapturing = false;
                    toggleKeyMouse = e.Data.Button;
                    toggleKey = KeyCode.VcNoName; // bet
                    label6.Text = FormatMouseCode(e.Data.Button.ToString());
                    label6.BackColor = bg;
                    label6.ForeColor = Color.White;
                }
                else if ((!mouseCapturing && !keyCapturing) && (e.Data.Button == toggleKeyMouse))
                {
                    HandleToggle();
                }

                if (clicking)
                {
                    return;
                }

                if (e.Data.Button == MouseButton.Button1)
                {
                    holding = true;
                }
            });

            /*kbHook.MousePressed.Subscribe(async e =>
            {
                if (e.IsEventSimulated)
                {
                    return;
                }

                if (clicking)
                {
                    return;
                }
                
            });*/

            kbHook.MouseReleased.Subscribe(e =>
            {
                if (e.IsEventSimulated)
                {
                    return;
                }

                if (e.Data.Button == MouseButton.Button1)
                {
                    holding = false;
                }
            });

            kbHook.KeyPressed.Subscribe(e =>
            {
                if (e.IsEventSimulated)
                {
                    return;
                }

                e.SuppressEvent = false;

                if (keyCapturing || mouseCapturing)
                {
                    KeyCode kc = e.Data.KeyCode;
                    toggleKey = kc;
                    toggleKeyMouse = MouseButton.NoButton;
                    keyCapturing = false;
                    mouseCapturing = false;
                    label6.Text = TrimKeyCode(kc.ToString());
                    label6.BackColor = bg;
                    label6.ForeColor = Color.White;
                }
                else if ((!mouseCapturing && !keyCapturing) && (e.Data.KeyCode == toggleKey))
                {
                    HandleToggle();
                    e.SuppressEvent = true;
                }
            });

            await kbHook.RunAsync();
        }

        private string FormatMouseCode(string b)
        {
            if (b == "Button1")
            {
                return "LeftMouse";
            }
            else if (b == "Button2")
            {
                return "RightMouse";
            }
            else if (b == "Button3")
            {
                return "MiddleMouse";
            }

            return b;
        }
        private string TrimKeyCode(string kc)
        {
            if (kc.Length > 2 && kc[..2] == "Vc")
            {
                kc = kc.Substring(2);
            }

            return kc;
        }

        private void CaptureInputToggle(bool isMouse)
        {
            mouseCapturing = !mouseCapturing;
            keyCapturing = !keyCapturing;
        }

        private void UI_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void input2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void input_LostFocus(object sender, EventArgs e)
        {
            string txt = input.Text;
            int newSpeed = clickSpeed;

            if ((txt.Length > 0) && (Int32.TryParse(txt, out newSpeed) == true))
            {
                clickSpeed = newSpeed;
            }

            clickSpeed = Math.Max(clickSpeed, 1);
            input.Text = clickSpeed.ToString();
        }
        private void input2_LostFocus(object sender, EventArgs e)
        {
            string txt = input2.Text;
            int newDel = toggleDelay;

            if ((txt.Length > 0) && (Int32.TryParse(txt, out newDel) == true))
            {
                toggleDelay = newDel;
            }

            toggleDelay = Math.Max(toggleDelay, 0);
            input2.Text = toggleDelay.ToString();
        }
        private void label6_Click(object sender, EventArgs e)
        {
            if (keyCapturing || mouseCapturing)
            {
                return;
            }

            label6.BackColor = Color.White;
            label6.ForeColor = Color.Black;
            CaptureInputToggle(false);
        }
    }
    public class AppSettings
    {
        public int toggleDelay { get; set; }
        public int clickSpeed { get; set; }
        public KeyCode toggleKey { get; set; }
        public MouseButton toggleKeyMouse { get; set; }
    }
}
