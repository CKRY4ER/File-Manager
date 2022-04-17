using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Manager
{
    internal class Program
    {
        const int WINDOW_WIDTH = 140;
        const int WINDOW_HEIGHT = 45;
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Title = "File-Manager";
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            Console.SetBufferSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            DrawAllWindow(0, 0);
            Console.ReadKey(true);
        }
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

            //Отрисовка окна Командная строка
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
