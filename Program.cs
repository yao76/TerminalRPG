using System;
using System.Collections.Generic;
using System.Threading;

namespace Hackathon
{
	class Display
	{
		public void drawRect(int x, int y, int w, int h, ConsoleColor color) {
			Console.ForegroundColor = color;
			Console.SetCursorPosition(x,y);
			string borderTop = "";
			for (int i = 0; i < w-2; i++)
				borderTop += "═";
			Console.Write("╔" + borderTop + "╗");
			for (int i = 1; i <= h - 2; i++)
			{
					Console.SetCursorPosition(x,y+i);
					Console.Write("║");
					Console.SetCursorPosition(x + w - 1,y+i);
					Console.Write("║");
			}
			Console.SetCursorPosition(x,y+h-1);
			Console.Write("╚" + borderTop + "╝");
		}
		public void clearRect(int x, int y, int w, int h, string character) {
			string lineClear = character;
			for (int i = 1; i < w; i++) 
				lineClear += character;
			
			Console.SetCursorPosition(0,0);
			for (int yc = 0; yc < h; yc++) {
				Console.SetCursorPosition(x,y+yc);
				Console.Write(lineClear);
			}
		}
		public void drawText(int x,int y,string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.SetCursorPosition(x,y);
			Console.Write(text);
		}
	}
    class Program
    {
        static void Main(string[] args)
        {

			Console.CursorVisible = false;
			Display display = new Display();


			display.drawRect(0,35,17,15,ConsoleColor.White);
			display.drawRect(0,35,100,15,ConsoleColor.White);
			display.clearRect(16,35,1,1,"╦");
			display.clearRect(16,49,1,1,"╩");

			display.drawText(4,37,"Attack",ConsoleColor.White);
			display.drawText(4,39,"Skills",ConsoleColor.White);
			display.drawText(4,41,"Inventory",ConsoleColor.White);
			display.drawText(4,43,"Run",ConsoleColor.White);

			
			int choice = 0;
			Dictionary<string,object>[] choices = {
				new Dictionary<string, object>() {
					{"x",4}, {"y",37}, {"text", "Attack"}
				},
				new Dictionary<string, object>() {
					{"x",4}, {"y", 39}, {"text", "Skills"}
				},
				new Dictionary<string, object>() {
					{"x",4}, {"y", 41}, {"text", "Inventory"}
				},
				new Dictionary<string, object>() {
					{"x",4}, {"y", 43}, {"text", "Run"}
				}
			};

			ConsoleKeyInfo cki;
			string gameState = "LeftMenu";
			while (true) {
				display.drawText((int)choices[choice]["x"],(int)choices[choice]["y"],choices[choice]["text"].ToString(), ConsoleColor.Green);

				Console.SetCursorPosition(0,0);
				while (!Console.KeyAvailable) {
					Thread.Sleep(16);
				}
				display.clearRect(0,0,10,1," ");

				cki = System.Console.ReadKey(true);

				if (gameState == "LeftMenu") {
					if (cki.Key.ToString() == "DownArrow") {
						display.drawText((int)choices[choice]["x"],(int)choices[choice]["y"],choices[choice]["text"].ToString(), ConsoleColor.White);
						choice = (choice + 1) % choices.Length;
					} else if (cki.Key.ToString() == "UpArrow") {
						display.drawText((int)choices[choice]["x"],(int)choices[choice]["y"],choices[choice]["text"].ToString(), ConsoleColor.White);
						choice--;
						if (choice == -1) choice = choices.Length - 1;
						gameState = "RightMenu";
					}
				} else if (gameState == "RightMenu") {
					Console.WriteLine("wut");
				}

				Console.ForegroundColor = ConsoleColor.White;

			}
        }
    }
}
