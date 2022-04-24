using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Manager
{
    internal class Program
    {
        const int WINDOW_WIDTH = 140;
        const int WINDOW_HEIGHT = 45;
        private static List<string> listCommand;
        private static string currentDir = Directory.GetCurrentDirectory();
        static void Main(string[] args)
        {
            listCommand = new List<string>();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Title = "File-Manager";
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            Console.SetBufferSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            DrawAllWindow(0, 0);
            Console.SetCursorPosition(104, WINDOW_HEIGHT - 12);
            // Console.SetCursorPosition(29, WINDOW_HEIGHT - 8);
            ProccesEnterComand();
            Console.ReadLine();
            Console.ReadKey(true);
            DirectoryInfo a = new DirectoryInfo(currentDir);
        }

        /// <summary>
        /// Метод отвечает за процесс ввода команды
        /// </summary>
        static void ProccesEnterComand()
        {
            Console.SetCursorPosition(2, 42);
            Console.Write(currentDir+">");
            (int left, int top) = GetCursorPosition();
            StringBuilder command = new StringBuilder();
            char key;
            do
            {
                key = Console.ReadKey().KeyChar;
                if (key != (char)8 && key != (char)13)
                {
                    command.Append(key);
                }
                (int currentLeft, int currentTop) = GetCursorPosition();
                if (left == WINDOW_WIDTH - 2)
                {
                    Console.SetCursorPosition(left - 1, top);
                    Console.Write(" ");
                    Console.SetCursorPosition(left - 1, top);
                }
                if (key == (char)8)
                {
                    if (command.Length > 0)
                    {
                        command.Remove(command.Length - 1, 1);
                    }
                    if (currentLeft >= left)
                    {
                        Console.SetCursorPosition(currentLeft, top);
                        Console.Write(" ");
                        Console.SetCursorPosition(currentLeft, top);
                    }
                    else
                    {
                        Console.SetCursorPosition(left, top);
                    }
                }
                
            }
            while (key!=(char)13);
            (int currentLeft1, int currentTop1) = GetCursorPosition();
            while (currentLeft1 >= left)
            {
                Console.SetCursorPosition(currentLeft1, top);
                Console.Write(" ");
                
            }
            CommandParser(command.ToString());
        }

        /// <summary>
        /// Парсер команды
        /// </summary>
        /// <param name="command">команда</param>
        static void CommandParser(string command)
        {
            string diskName = "";
            try
            {
                diskName = currentDir[0].ToString() + currentDir[1].ToString() + currentDir[2].ToString();
            }
            catch (Exception e)
            {

            }
            string[] commandParams = command.ToLower().Split(' ');
            switch (commandParams[0])
            {
                case ("cd"):
                    if (commandParams.Length < 2) 
                    {
                        MessageBox.Show("Не верный формат команды cd [путь] | [ . ]", "Ошибка", MessageBoxButtons.OK);
                        DrawAllWindow(0, 0);
                        break;
                    }
                    if (commandParams[1]==null||commandParams[1] == "")
                    {
                        MessageBox.Show("Не верный формат команды cd [путь] | [ . ]", "Ошибка", MessageBoxButtons.OK);
                        DrawAllWindow(0, 0);
                        break;
                    }
                    if (commandParams[1]==".")
                    {
                        currentDir = Directory.GetParent(currentDir)?.ToString();
                        if (currentDir == null)
                            currentDir = diskName;
                        DrawAllWindow(0, 0);
                        break;
                    }
                    if (Directory.Exists(commandParams[1]))
                    {
                        currentDir = commandParams[1];
                        listCommand.Add(command);
                        DrawAllWindow(0, 0);
                    }
                    else
                    {
                        MessageBox.Show("Указанной дирректории не существует", "Ошибка", MessageBoxButtons.OK);
                        DrawAllWindow(0, 0);
                    }
                    break;
                case ("ls"):
                    if (commandParams.Length != 4 || commandParams[2] != "-p") 
                    {
                        MessageBox.Show("Не верный формат команды ls: ls [дирректория] [-p] [номер страницы].", "Ошибка", MessageBoxButtons.OK);
                        DrawAllWindow(0, 0);
                        break;
                    }
                    if (!Directory.Exists(commandParams[1]))
                    {
                        MessageBox.Show("Указанной дирректории не существует", "Ошибка", MessageBoxButtons.OK);
                        DrawAllWindow(0, 0);
                        break;
                    }
                    if (commandParams[3] == "")
                    {
                        DrawAllWindow(0, 0);
                        DrawTree(new DirectoryInfo(commandParams[1]), 1);
                        
                    }
                    else
                    {
                        DrawAllWindow(0, 0);
                        (int left, int top) = GetCursorPosition();
                        DrawTree(new DirectoryInfo(commandParams[1]), int.Parse(commandParams[3]));
                    }
                    listCommand.Add(command);
                    break;
                    //Принцип работы: после команды cp необходимо ввести полный путь до файла или дирректории которую необходимо скопировать.
                    //Копирование происходит в текущую дирректорию (currentDir). Если будет время, реализую
                    //копирование в другую дирректорию, путем ввода третьего параметра в команду (cp c:\someDir c:\newDir)
                case ("cp"):
                    if (commandParams.Length > 1 && commandParams.Length <= 2) 
                    {
                        //Определяем, ввел ли пользователь путь к файлу/дирректории или название
                        if (commandParams[1].Contains('\\') || commandParams[1].Contains(':'))
                        {
                            //Определяем, файл ли это?
                            if (File.Exists(commandParams[1]))
                            {
                                string fileName = Path.GetFileName(commandParams[1]);
                                //Проверяем, существует ли данный файл в currentDir
                                if (!File.Exists(currentDir + fileName))
                                {
                                    string path = Path.Combine(currentDir, fileName);
                                    File.Copy(commandParams[1], path);
                                    listCommand.Add(command);
                                    DrawAllWindow(0, 0);
                                    break;
                                }
                                else
                                {
                                    MessageBox.Show($"Файл {fileName} уже существует в {currentDir}", "Ошибка", MessageBoxButtons.OK);
                                    listCommand.Add(command);
                                    DrawAllWindow(0, 0);
                                    break;
                                }
                            }
                            //Проверка на дирректорию
                            else if (Directory.Exists(commandParams[1]))
                            {
                                string dirName = Path.GetFileName(commandParams[1]);
                                if (!Directory.Exists(currentDir + dirName))
                                {
                                    Directory.CreateDirectory(Path.Combine(currentDir, dirName));
                                    string[] files = Directory.GetFiles(commandParams[1]);
                                    //Копируем все файлы указанной дирр. в currentDir
                                    foreach(string s in files)
                                    {
                                        string fileName = Path.GetFileName(s);
                                        string destination = Path.Combine(currentDir + dirName, fileName);
                                        File.Copy(s, destination, true);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"Дирректория {dirName} уже существует в {currentDir}", "Ошибка", MessageBoxButtons.OK);
                                }
                                DrawAllWindow(0, 0);
                                listCommand.Add(command);
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не верный формат команды cp: cp [путь до дирр/файла]", "Ошибка", MessageBoxButtons.OK);
                        DrawAllWindow(0, 0);
                    }
                    break;
                case ("rm"):

                    break;
                case ("inf"):
                    if (commandParams.Length == 2)
                    {
                        if (File.Exists(commandParams[1]))
                        {
                            FileInfo file = new FileInfo(commandParams[1]);
                            DrawAllWindow(0, 0);
                            Console.SetCursorPosition(29, 33);
                            Console.Write(file.CreationTime);
                            Console.SetCursorPosition(29, 35);
                            Console.Write(file.LastAccessTime);
                            Console.SetCursorPosition(29, 37);
                            Console.Write(file.LastWriteTime);
                            Console.SetCursorPosition(104, 33);
                            Console.Write(file.Extension);
                            Console.SetCursorPosition(104, 35);
                            Console.Write(file.Length);
                            Console.SetCursorPosition(104, 37);
                            Console.Write(file.Name);
                            listCommand.Add(command);
                        }
                        else
                        {
                            MessageBox.Show("Указанного файла не существует", "Ошибка", MessageBoxButtons.OK);
                            DrawAllWindow(0, 0);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не верный формат команды inf: inf [путь файла]", "Ошибка", MessageBoxButtons.OK);
                        DrawAllWindow(0, 0);
                    }
                    break;
                default:
                    MessageBox.Show("Не верная команда", "Ошибка", MessageBoxButtons.OK);
                    DrawAllWindow(0, 0);
                    break;
            }
            ProccesEnterComand();
        }
        /// <summary>
        /// Очистка поля вводы команды
        /// </summary>
        static void ClearBuffer()
        {

        }
        /// <summary>
        /// Отрисовать дерево каталогов
        /// </summary>
        /// <param name="dir">Дирректория отрисовки</param>
        /// <param name="page">Страница</param>
        static void DrawTree(DirectoryInfo dir, int page)
        {
            StringBuilder tree = new StringBuilder();
            GetTree(tree, dir, "", true);
            (int currentLeft, int currentTop) = (0, 0);
            int pageLines=29;
            string[] lines = tree.ToString().Split('\n');
            int pageTotal = (lines.Length + pageLines - 1) / pageLines;
            if (page > pageTotal)
            {
                page = pageTotal;
            }
            for (int i = (page - 1) * pageLines, counter = 0; i < page * pageLines; i++, counter++)
            {
                if (lines.Length - 1 > i)
                {
                    Console.SetCursorPosition(currentLeft + 1, currentTop + 1 + counter);
                    Console.WriteLine(lines[i]);
                }
            }

            //Надпись показывающая, номер страницы и общее кол-во страниц
            string footer = $"╡ {page} из {pageTotal} ╞";
            Console.SetCursorPosition(WINDOW_WIDTH/2 - footer.Length/2, 30);
            Console.WriteLine(footer);
        }
        /// <summary>
        /// Собрать дерево
        /// </summary>
        /// <param name="tree">Дерево</param>
        /// <param name="dir">Дирректория для отрисовки</param>
        /// <param name="indent"></param>
        /// <param name="lastDirectory">Конечная дирректория в списке</param>
        static void GetTree(StringBuilder tree, DirectoryInfo dir, string indent, bool lastDirectory) 
        {
            tree.Append(indent);
            if (lastDirectory)
            {
                tree.Append("└─");
                indent += "  ";
            }
            else
            {
                tree.Append("├─");
                indent += "│ ";
            }
            tree.Append($"{dir.Name}\n");
            FileInfo[] subFiles = dir.GetFiles();
            for(int i = 0; i < subFiles.Length; i++)
            {
                if (i == subFiles.Length - 1)
                {
                    tree.Append($"{indent}└─{subFiles[i].Name}\n");
                }
                else
                {
                    tree.Append($"{indent}├─{subFiles[i].Name}\n");
                }
            }
            DirectoryInfo[] subDirs = dir.GetDirectories();
            for(int i = 0; i < subDirs.Length; i++)
            {
                GetTree(tree, subDirs[i], indent, i == subDirs.Length - 1);
            }
        }
        /// <summary>
        /// Получить координаты позиции курсора
        /// </summary>
        /// <returns></returns>
        static (int left, int top) GetCursorPosition() =>
            (Console.CursorLeft, Console.CursorTop);

        /// <summary>
        /// Отрисовка рабочей области
        /// </summary>
        /// <param name="x">Начальная координата отрисовки по оси X</param>
        /// <param name="y">Начальная координата отрисовки по оси Y</param>
        /// <param name="width">Ширина окна</param>
        /// <param name="height">Высота окна</param>
        static void DrawAllWindow(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            //Отрисовка окна Дерево
            Console.Write("╔");
            for (int i = 0; i < WINDOW_WIDTH - 2; i++)
            {
                if (i == 3)
                {
                    string name = "Дерево файлов и каталогов";
                    Console.Write(name);
                    i += name.Length;
                }
                Console.Write("═");
            }
            Console.Write("╗");
            Console.SetCursorPosition(x, y + 1);
            for (int i = 0; i < WINDOW_HEIGHT - 16; i++) 
            {
                Console.Write("║");
                for (int j = x + 1; j < WINDOW_WIDTH - 1 + x; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
            }
            Console.Write("╚");
            for (int i = 0; i < WINDOW_WIDTH - 2; i++)
            {
                Console.Write("═");
            }
            Console.Write("╝");

            
            //Отрисовка окна Информация
            Console.Write("╔");
            for (int i = 0; i < WINDOW_WIDTH - 2; i++)
            {
                if (i == 3)
                {
                    string name = "Информация";
                    Console.Write(name);
                    i += name.Length;
                }
                Console.Write("═");
            }
            Console.Write("╗");
            for (int i = 0; i < WINDOW_HEIGHT - 16-22; i++)
            {
                Console.Write("║");
                for (int j = x + 1; j < WINDOW_WIDTH - 1 + x; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
            }
            Console.Write("╚");
            for (int i = 0; i < WINDOW_WIDTH - 2; i++)
            {
                Console.Write("═");
            }
            Console.Write("╝");
            Console.SetCursorPosition(2, WINDOW_HEIGHT - 12);
            Console.Write("Дата создания файла: ");
            Console.SetCursorPosition(2, WINDOW_HEIGHT - 10);
            Console.Write("Дата последнего открытия: ");
            Console.SetCursorPosition(2, WINDOW_HEIGHT - 8);
            Console.Write("Дата последнего изменения: ");
            Console.SetCursorPosition(85, WINDOW_HEIGHT - 12);
            Console.Write("Разширение: ");
            Console.SetCursorPosition(85, WINDOW_HEIGHT - 10);
            Console.Write("Размер в байтах: ");
            Console.SetCursorPosition(85, WINDOW_HEIGHT - 8);
            Console.Write("Имя: ");

            //Отрисовка окна Командная строка
            Console.SetCursorPosition(0, WINDOW_HEIGHT - 5);
            Console.Write("╔");
            for (int i = 0; i < WINDOW_WIDTH - 2; i++)
            {
                if (i == 3)
                {
                    string name = "Командная строка";
                    Console.Write(name);
                    i += name.Length;
                }
                Console.Write("═");
            }
            Console.Write("╗");
            for (int i = 0; i < WINDOW_HEIGHT - 16-27; i++)
            {
                Console.Write("║");
                for (int j = x + 1; j < WINDOW_WIDTH - 1 + x; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
            }
            Console.Write("╚");
            for (int i = 0; i < WINDOW_WIDTH - 2; i++)
            {
                Console.Write("═");
            }
            Console.Write("╝");
        }
    }
}
