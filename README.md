# EvenBetterComcave
Inspiriert durch das Projekt [Better Comcave](https://github.com/scysys/Better-Comcave) das leider auf Grund von Batch/VB sehr limitiert in seinen Fähigkeiten ist, und in der Praxis immer wieder unzuverlässig gearbeitet hat, habe ich eine technisch wesentlich verbesserte Version als .NET Core Console App in C# geschrieben.
Hier ist es z.B. möglich direkt lesend auf den java Prozess zuzugreifen um zum Beispiel den MainWindowName, also den Namen des momentanten Hauptfensters, abzufragen. Dadurch kann sichergestellt werden, dass der Launcher auch wirklich gestartet wurde und das Login Fenster geöffnet und angewählt ist, bevor die Zugangsdaten eingegeben werden.
Natürlich ist das ganze Open Source, wenn ihr also paranoid seid oder das Programm anpassen/verbessern wollt, könnt ihr euch das Programm einfach selbst kompilieren.
Für alle anderen habe ich das Programm für verschiedene Plattformen kompiliert so dass es direkt ausführbar ist.

[DOWNLOAD Portable](https://github.com/L-Pow/EvenBetterComcave/raw/master/Download%20Releases/portable.zip) (benötigt eine installierte [ASP.NET Core Runtime 3.1 oder höher](https://dotnet.microsoft.com/download/dotnet-core/3.1)) 

[DOWNLOAD Windows 32bit](https://github.com/L-Pow/EvenBetterComcave/raw/master/Download%20Releases/win_x86.zip)

[DOWNLOAD Windows 64bit](https://github.com/L-Pow/EvenBetterComcave/raw/master/Download%20Releases/win_x64.zip)

[DOWNLOAD OsX 64bit](https://github.com/L-Pow/EvenBetterComcave/raw/master/Download%20Releases/osx_x64.zip)

[DOWNLOAD Linux 64bit](https://github.com/L-Pow/EvenBetterComcave/raw/master/Download%20Releases/linux_x64.zip)

Zum starten des Programms musst du die EvenBetterComcave.exe starten. <br>
Damit dieses Programm funktioniert musst du deinen username und passwort in die Datei "appsetting.json" eintragen.<br>
Du findest diese Datei im gleichen Ordner wie die EvenBetterComcave.exe<br>
Um Änderungen an "appsetting.json" vorzunehmen musst du die Datei mit einem Texteditor öffnen und bearbeiten<br>
Mache einen Rechtsklick auf die Datei, klicke "öffnen mit..." --> wähle das Programm Editor (eventuell musst du auf "weitere Apps" klicken um den Editor zu finden)<br>
Innerhalb der Datei findest du weitere Kommentare die dir helfen sollen alles korrekt einzutragen<br>
Falls du bei der Eingabe einen Fehler gemacht hast kannst du jederzeit dein username und passwort ändern<br>
Du kannst ausserdem bei Bedarf ein eigenes Intervall für die Restart Dauer festlegen<br>
