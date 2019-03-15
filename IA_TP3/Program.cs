using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
	class Program
	{
		static void Main(string[] args)
		{
			while(true) {
				Generate();
				Console.ReadKey();
				Console.Clear();
			}
		}

		static void Generate()
		{
			Forest f = new Forest(5);
			f.Log();
		}
	}
}
