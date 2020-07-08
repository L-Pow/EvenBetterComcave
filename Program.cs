using System;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;
using System.Data.Common;

namespace EvenBetterComcave
{
    class Program
    {

        private const int SW_SHOWNORMAL = 1;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        static void Main(string[] args)
        {
            //int minimumMinuten = 30;
            //int maximumMinuten = 60;
            Random r = new Random();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            string username = configuration["username"];
            string passwort = configuration["passwort"];
            int minimumMinuten = Int32.Parse(configuration["minimumRestartTime"]);
            int maximumMinuten = Int32.Parse(configuration["maximumRestartTime"]);
            string launcherPath = configuration["LauncherDateipfad"];

            if (username.Contains(" ") || passwort.Contains(" "))
            {
                Console.WriteLine("Hallo!");
                Console.WriteLine("Es sieht so aus als ob du das Programm zum ersten mal startest, und noch kein username und passwort eingetragen hast");
                Console.WriteLine("Damit dieses Programm funktioniert musst du deinen username und passwort in die Datei \"appsetting.json\" eintragen");
                Console.WriteLine("Du findest diese Datei im gleichen Ordner wie diese EvenBetterComcave.exe");
                Console.WriteLine("um Änderungen an \"appsetting.json\" vorzunehmen musst du die Datei mit einem Texteditor öffnen und bearbeiten");
                Console.WriteLine("Mache einen Rechtsklick auf die Datei, klicke \"öffnen mit...\" --> wähle das Programm Editor (eventuell musst du auf \"weitere Apps\" klicken um den Editor zu finden)");
                Console.WriteLine("Innerhalb der Datei findest du weitere Kommentare die dir helfen sollen alles korrekt einzutragen");
                Console.WriteLine("Falls du bei der Eingabe einen Fehler gemacht hast kannst du jederzeit dein username und passwort ändern");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("BITTE USERNAME/PASSWORT FESTLEGEN UND DAS PROGRAMM NEU STARTEN!!! ");
                Console.WriteLine("");
                Console.WriteLine("");
                while (true)
                {
                    var t = Task.Run(async delegate
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.WriteLine("BITTE USERNAME/PASSWORT FESTLEGEN UND DAS PROGRAMM NEU STARTEN!!! ");
                        await Task.Delay(1000);
                    });
                    t.Wait();
                }

            }

            //if (!LauncherRunning())
            //{
            //    Console.WriteLine("CCLauncher not running");
            //    Console.WriteLine("Starting CCLauncher now!");
            //    StartLauncher(launcherPath);
            //}


            while (true)
            {
                int rInt = r.Next(minimumMinuten, maximumMinuten + 1);
                int secondsUntilRestart = rInt * 60;

                if (!LauncherRunning())
                {
                    Console.WriteLine("CCLauncher not running");
                    Console.WriteLine("Starting CCLauncher now!");
                    StartLauncher(launcherPath);

                    for (int i = 0; i < 30; i++) //testet maximal 30 sekunden lang ob der launcher gestartet wurde, bricht ab sobald window gefunden wurde
                    {
                        if (LoginWindowOpen())
                        {
                            Console.WriteLine("Login Fenster wurde gefunden, starte Eingabe von Zugangsdaten");
                            break;
                        }

                        var t = Task.Run(async delegate
                        {
                            Console.WriteLine("Waiting another " + (30 - i) + " seconds for LoginWindow to open");
                            await Task.Delay(1000);
                        });
                        t.Wait();
                        if (i == 29)
                        {
                            Console.WriteLine("Login Window still not open, trying to start Launcher again");
                            KillLauncherProcess();
                            var x = Task.Run(async delegate
                            {
                                Console.WriteLine("Waiting a bit before Launching again");
                                await Task.Delay(1000);
                            });
                            x.Wait();
                            StartLauncher(launcherPath);
                            i = -1;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Launcher already running");
                }

                EnterCredentials(username, passwort);

                while (secondsUntilRestart >= 0)
                {
                    var t = Task.Run(async delegate
                    {
                        Console.Clear();
                        Console.WriteLine("Waiting another " + secondsUntilRestart + " seconds until restarting CCLauncher");
                        await Task.Delay(1000);
                        secondsUntilRestart -= 1;
                    });
                    t.Wait();

                    if (secondsUntilRestart % 30 == 0) //check every 30 seconds if the launcher is still running or if it was closed or crashed somehow
                    {
                        if (!LauncherRunning())
                        {
                            var x = Task.Run(async delegate
                            {
                                Console.WriteLine("CCLauncher was closed or crashed");
                                Console.WriteLine("Time to start from the beginning!");
                                await Task.Delay(5000);
                            });
                            x.Wait();
                            secondsUntilRestart = 0;
                        }
                    }

                    if (secondsUntilRestart == 0)
                    {
                        KillLauncherProcess();
                    }
                }
            }
        }

        public static void EnterCredentials(string username, string passwort)
        {
            var process = Process.GetProcessesByName("java").FirstOrDefault();
            if (process != null && process.MainWindowTitle == "Login")
            {
                ShowWindow(process.MainWindowHandle, SW_SHOWNORMAL);
                SetForegroundWindow(process.MainWindowHandle);
                var sim = new InputSimulator();
                var x = Task.Run(async delegate
                {
                    await Task.Delay(500);
                });
                x.Wait();
                sim.Keyboard.TextEntry(username);
                var a = Task.Run(async delegate
                {
                    await Task.Delay(500);
                });
                a.Wait();
                sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
                var b = Task.Run(async delegate
                {
                    await Task.Delay(500);
                });
                b.Wait();
                sim.Keyboard.TextEntry(passwort);
                var c = Task.Run(async delegate
                {
                    await Task.Delay(500);
                });
                c.Wait();
                sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            }

        }

        public static bool LoginWindowOpen()
        {
            var process = Process.GetProcessesByName("java").FirstOrDefault();
            if (process != null && process.MainWindowTitle == "Login")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LauncherRunning()
        {
            var process = Process.GetProcessesByName("java").FirstOrDefault();
            if (process != null && process.MainWindowTitle == "CC Launcher 3.0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void StartLauncher(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception e)
            {
                Console.Write($" Message : {e.Message}");
            }
        }

        public static void KillLauncherProcess()
        {
            var process = Process.GetProcessesByName("java").FirstOrDefault();
            if (process != null)
            {
                try
                {
                    process.Kill();
                }
                catch (Exception e)
                {
                    Console.Write($" Message : {e.Message}");
                }

                var x = Task.Run(async delegate
                {
                    Console.WriteLine("Waiting a bit before Launching again");
                    await Task.Delay(1000);
                });
                x.Wait();
            }
        }
    }
}
